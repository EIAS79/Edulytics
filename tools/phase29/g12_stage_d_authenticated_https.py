#!/usr/bin/env python3
"""Isolated GitHub Actions-only Grade 12 authenticated HTTPS regression; never production."""
import hashlib, html, json, os, re, urllib.error
from pathlib import Path
from g5_stage_d_authenticated_https import (
    sql, login_actor, get_page, BASE, UUID,
    extract_adoption, extract_csrf, extract_idempotency_key, probe_lesson,
)
COUNTS=(7,4,9,8,4,3,6,8,8,5,7,4)
STUDENT="commoncore-secondary.student.secondary@edulytiks.com"
TEACHER="commoncore-secondary.teacher.secondary@edulytiks.com"
FOREIGN="commoncore-primary.student.primary@edulytiks.com"
SCHOOL="REHEARSAL-US-SECONDARY"
PREFIX="PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:"

def one(statement, reason):
    rows=sql(statement).splitlines()
    if len(rows)!=1 or not rows[0]:
        raise AssertionError(f"G12 {reason}: expected exactly one row, got {rows!r}")
    return rows[0]

def lessons():
    expected=[f"{PREFIX}U{u:02d}:L{l:02d}"
        for u,num in enumerate(COUNTS,1) for l in range(1,num+1)]
    assert len(expected)==73
    rows=sql("""SELECT "Code"||'|'||"Id"::text FROM "CurriculumPedagogicalLessons"
          WHERE "Code" LIKE 'PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:%'
          ORDER BY "Code";""").splitlines()
    pairs=[r.split("|",1) for r in rows]
    if len(pairs)!=73 or [r[0] for r in pairs]!=expected or len(set(r[1]for r in pairs))!=73:
        raise AssertionError("G12 seeded SQL curriculum lesson denominator/ordering does not match original 73")
    if any(not re.fullmatch(UUID,x[1]) for x in pairs):
        raise AssertionError("G12 seeded lesson UUID invalid")
    return pairs

def adoption():
    q="""SELECT c."Id"::text||'|'||c."CurriculumAdoptionId"::text||'|'||
          COALESCE(a."CurriculumLevelKey",'')||'|'||a."CurriculumLogicalLevel"::text
          FROM "StudentEnrollments" e
          JOIN "StudentProfiles" p ON p."Id"=e."StudentProfileId"
          JOIN "AspNetUsers" u ON u."Id"=p."UserId"
          JOIN "ClassGroups" c ON c."Id"=e."ClassGroupId"
          JOIN "SchoolCurriculumAdoptions" a ON a."Id"=c."CurriculumAdoptionId"
          JOIN "Schools" s ON s."Id"=c."SchoolId"
          WHERE u."Email"='"""+STUDENT+"""' AND s."SchoolCode"='"""+SCHOOL+"""';"""
    cid,aid,key,level=one(q,"explicit PRECALC student enrollment").split("|")
    if level!="13" or "PRECALC" not in key or not re.fullmatch(UUID,aid):
        raise AssertionError("G12 student adoption not explicit level-13 Precalculus: "+key+"/"+level)
    return aid,key

def run():
    must={"GITHUB_ACTIONS":"true","EDULYTICS_CI_REHEARSAL":"true",
          "EDULYTICS_CI_G12_PRECALC":"true",
          "EDULYTICS_CLEAN_DATABASE_NAME":"edulytics_clean_ci",
          "ASPNETCORE_ENVIRONMENT":"Staging"}
    for k,v in must.items():
        if os.environ.get(k)!=v:raise RuntimeError("G12 E2E denied outside disposable CI: "+k)
    expected=lessons()
    adoption_id,adoption_key=adoption()
    student=login_actor(STUDENT,"Student")
    teacher=login_actor(TEACHER,"Teacher")
    foreign=login_actor(FOREIGN,"Student")
    if "assessments" not in get_page(teacher,"/school/assessments").lower():
        raise AssertionError("G12 teacher cannot access authorized dashboard")
    records=[]
    for ordinal,(code,lesson_id) in enumerate(expected,1):
        print("G12_AUTH_HTTPS_LESSON_CHECK",ordinal,code,flush=True)
        try:
            content=html.unescape(get_page(student,"/student/learning/lesson/"+lesson_id))
        except urllib.error.HTTPError as exc:
            raise AssertionError(
                f"G12_AUTHENTICATED_LESSON_HTTP_BLOCKED ordinal={ordinal} "
                f"code={code} http={exc.code}; cannot claim student release") from exc
        if "rich-lesson-section" not in content:
            raise AssertionError(f"G12 U/L {code}: student did not see actual source-aligned RichV2")
        if code.endswith(("U08:L03","U08:L04")) and (
                "<svg " not in content or "role='img'" not in content):
            raise AssertionError(f"G12 {code} missing accessible original math diagrams")
        records.append({"ordinal":ordinal,"lessonCode":code,
                        "contentSha256":hashlib.sha256(content.encode()).hexdigest(),
                        "authenticatedStudentPage":True})
    try:
        foreign_content=get_page(foreign,"/student/learning/lesson/"+expected[0][1])
    except urllib.error.HTTPError as exc:
        if exc.code not in (401,403,404):raise
    else:
        if "rich-lesson-section" in foreign_content:
            raise AssertionError("G12 lesson leaked to a different school's student")
    form=get_page(student,"/student/practice")
    csrf,nonce,form_adoption=(extract_csrf(form),extract_idempotency_key(form),
                              extract_adoption(form))
    if not csrf or not nonce or not form_adoption or form_adoption.lower()!=adoption_id.lower():
        raise AssertionError("G12 lesson Practice not bound to PRECALC adoption")
    positive=probe_lesson(student,BASE,expected[0][1],form_adoption,csrf,nonce,sql)
    if positive.get("status")!="SELF_CONSISTENT_GRADE_PERSISTED":
        raise AssertionError("G12 actual correct answer not persisted: "+str(positive))
    form2=get_page(student,"/student/practice")
    negative=probe_lesson(student,BASE,expected[0][1],
         extract_adoption(form2),extract_csrf(form2),
         extract_idempotency_key(form2),sql,answer_mode="malformed")
    if negative.get("status")!="MALFORMED_ANSWER_REJECTED_AND_PERSISTED":
        raise AssertionError("G12 malformed answer not rejected/persisted: "+str(negative))
    # Every canonical lesson now has a distinct bounded exact Practice route.
    # Exercise each lesson's real ASP.NET endpoint, SQL-generated item and
    # persisted grade. This proves self-consistent runtime, not a publisher
    # source-item math oracle (the independent xUnit suite covers the latter).
    all_lesson_practice=[{
        "ordinal":1,"lessonCode":expected[0][0],"positive":positive,"negative":negative}]
    records[0]["correctAndMalformedPracticePersisted"]=True
    for ordinal,(code,lesson_id) in enumerate(expected[1:],2):
        page=get_page(student,"/student/practice")
        token=extract_csrf(page)
        nonce=extract_idempotency_key(page)
        target_adoption=extract_adoption(page)
        if not token or not nonce or target_adoption != adoption_id:
            raise AssertionError(f"G12_PRAC_FORM_INVALID ordinal={ordinal} code={code}")
        correct=probe_lesson(student,BASE,lesson_id,target_adoption,
            token,nonce,sql)
        if correct.get("status")!="SELF_CONSISTENT_GRADE_PERSISTED":
            raise AssertionError(f"G12_PRAC_CORRECT_FAILED ordinal={ordinal} "
                f"code={code} result={correct}")
        negative_page=get_page(student,"/student/practice")
        malformed=probe_lesson(student,BASE,lesson_id,
            extract_adoption(negative_page),extract_csrf(negative_page),
            extract_idempotency_key(negative_page),sql,
            answer_mode="malformed")
        if malformed.get("status")!="MALFORMED_ANSWER_REJECTED_AND_PERSISTED":
            raise AssertionError(f"G12_PRAC_WRONG_FAILED ordinal={ordinal} "
                f"code={code} result={malformed}")
        all_lesson_practice.append({"ordinal":ordinal,"lessonCode":code,
            "positive":correct,"negative":malformed})
        records[ordinal-1]["correctAndMalformedPracticePersisted"]=True
        print("G12_PRACTICE_PERSISTENCE_CHECK",ordinal,code,flush=True)
    if len(all_lesson_practice)!=73:
        raise AssertionError("Grade12 cannot close a partial practice acceptance")
    report={"scope":"CI_DISPOSABLE_G12_AUTHENTICATED_HTTPS_NOT_PUBLICATION",
            "candidateSha":os.environ.get("GITHUB_SHA"),
            "sourceAcademicParity":"UNKNOWN","reviewerSignoff":"NOT_TESTED",
            "studentPages":records,"adoptionKey":adoption_key,
            "crossSchoolAccessDenied":True,"teacherAssessmentDashboard":True,
            "realPersistedCorrectAnswer":positive,"realPersistedMalformedAnswer":negative,
            "all73LessonPracticePositiveAndNegative":all_lesson_practice,
            "persistedExactPracticeLessonCount":len(all_lesson_practice),
            "teacherGradebookPerLessonVerified":False}
    Path("/tmp/g12-stage-d-authenticated-73-lessons.json").write_text(
       json.dumps(report,ensure_ascii=False,indent=2,default=str),encoding="utf8")
    print("G12_AUTHENTICATED_73_LESSONS_146_REAL_GRADED_ATTEMPTS_PASS",len(records),flush=True)

if __name__=="__main__":run()
