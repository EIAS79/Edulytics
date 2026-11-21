#!/usr/bin/env python3
"""G9 General Arabic: disposable four-role HTTPS, 82 pages, 80 eligible Practice.
Never production. Any non-verified digital contract must fail the actual API check.
Publisher source parity remains UNKNOWN; teacher-only proofs are not auto-graded.
"""
import hashlib,html,json,os,re,sys,urllib.error
from pathlib import Path
sys.path.insert(0,str(Path(__file__).resolve().parents[1]/"phase29"))
from g5_stage_d_authenticated_https import (
    sql, login_actor, get_page, BASE, UUID,
    extract_adoption,extract_csrf,extract_idempotency_key,probe_lesson
)

ROOT=Path(__file__).resolve().parents[2]
CONTENT=ROOT/"src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json"
OUT=Path("/tmp/uae-g9-stage-d-authenticated-82-lessons.json")
PREFIX="PED:UAE-MOE-MATH:L9:GENERAL:"
SCHOOL="REHEARSAL-AE-SECONDARY"
STUDENT="uae-secondary.student.primary@edulytiks.com"
TEACHER="uae-secondary.teacher.primary@edulytiks.com"
SUPERVISOR="uae-secondary.supervisor@edulytiks.com"
ADMIN="uae-secondary.admin@edulytiks.com"
FOREIGN="commoncore-secondary.student.primary@edulytiks.com"
TEACHER_ONLY={":13:07:CONGRUENCE-TRANSFORMATIONS",":13:08:TRIANGLES-AND-COORDINATE-PROOF"}

def required_environment():
    for k,v in {"GITHUB_ACTIONS":"true","EDULYTICS_CI_REHEARSAL":"true",
                "EDULYTICS_CI_REHEARSAL_PROGRAMME":"UAE-G9",
                "EDULYTICS_CLEAN_DATABASE_NAME":"edulytics_clean_ci",
                "ASPNETCORE_ENVIRONMENT":"Staging"}.items():
        if os.environ.get(k)!=v: raise RuntimeError("G9 HTTPS CI only: "+k)
    assert BASE=="https://localhost:5052"

def check_catalogue():
    pack=json.loads(CONTENT.read_text(encoding="utf-8"))
    assert pack["AcademicLanguage"]=="ar" and pack["Status"]=="Published"
    expected={x["LessonCode"]:x for x in pack["Lessons"]}
    assert len(expected)==82 and all(k.startswith(PREFIX) for k in expected)
    rows=sql("""SELECT "Code"||'|'||"Id"::text FROM "CurriculumPedagogicalLessons"
       WHERE "Code" LIKE 'PED:UAE-MOE-MATH:L9:GENERAL:%' ORDER BY "Code";""").splitlines()
    pairs=[x.split("|",1) for x in rows]
    assert len(pairs)==82 and len({x[1] for x in pairs})==82
    assert {x[0] for x in pairs}==set(expected)
    assert all(re.fullmatch(UUID,x[1]) for x in pairs)
    published=sql("""SELECT COUNT(*)::text FROM "CurriculumLessonContents" c
       JOIN "CurriculumPedagogicalLessons" p ON p."Id"=c."LessonNodeId"
       WHERE p."Code" LIKE 'PED:UAE-MOE-MATH:L9:GENERAL:%' AND c."Status"=3;""").strip()
    assert published=="82",f"G9 Published metadata not 82: {published}"
    return pairs,expected

def school_adoption():
    q="""SELECT c."CurriculumAdoptionId"::text||'|'||
        a."CurriculumLogicalLevel"::text||'|'||
        COALESCE(a."CurriculumLevelKey",'')||'|'||
        COALESCE(s."DefaultCulture",'')||'|'||
        COALESCE(a."CurriculumPathway",'')
      FROM "StudentEnrollments" e
      JOIN "StudentProfiles" p ON p."Id"=e."StudentProfileId"
      JOIN "AspNetUsers" u ON u."Id"=p."UserId"
      JOIN "ClassGroups" c ON c."Id"=e."ClassGroupId"
      JOIN "SchoolCurriculumAdoptions" a ON a."Id"=c."CurriculumAdoptionId"
      JOIN "Schools" s ON s."Id"=c."SchoolId"
      WHERE u."Email"='"""+STUDENT+"""' AND s."SchoolCode"='"""+SCHOOL+"""';"""
    rows=sql(q).splitlines()
    assert len(rows)==1,"G9 only one exact student school adoption"
    adopt,level,key,culture,pathway=rows[0].split("|",4)
    if not (re.fullmatch(UUID,adopt) and level=="9" and culture=="ar" and pathway=="General"):
        raise AssertionError(
            f"G9_DISPOSABLE_ADOPTION_SCOPE_MISMATCH level={level!r} "
            f"culture={culture!r} pathway={pathway!r}"
        )
    return adopt,key

def validate_html(page,code,title):
    assert title in page, "missing Arabic G9 lesson title: "+code
    for part in ("explanation","concepts","examples","steps","mistakes","summary"):
        assert "lesson-learning-section--"+part in page, "missing "+part+": "+code
    assert re.search(r"[\u0600-\u06ff]",page)
    for bad in ("Concept model","Instructional diagram","YouTube discovery could not load right now",
                "McGraw-Hill", "McGraw Hill", "McGraw-Hill Education"):
        assert bad not in page,"G9 English generic fallback: "+code

def run():
    required_environment()
    pairs,expected=check_catalogue()
    adoption,key=school_adoption()
    student=login_actor(STUDENT,"Student")
    teacher=login_actor(TEACHER,"Teacher")
    supervisor=login_actor(SUPERVISOR,"SubjectSupervisor")
    admin=login_actor(ADMIN,"SchoolAdmin")
    foreign=login_actor(FOREIGN,"Student")
    assert "assessments" in get_page(teacher,"/school/assessments").lower()
    try: foreign_page=get_page(foreign,"/student/learning/lesson/"+pairs[0][1]+"?culture=ar")
    except urllib.error.HTTPError as exc:
        assert exc.code in (401,403,404)
        foreign_denied=True
    else: foreign_denied="lesson-learning-section--explanation" not in foreign_page
    assert foreign_denied,"CROSS_SCHOOL_G9_LESSON_LEAK"
    records=[];pass_count=0;wrong_count=0;malformed_count=0
    for ordinal,(code,lesson_id) in enumerate(pairs,1):
        page=html.unescape(get_page(student,
              "/student/learning/lesson/"+lesson_id+"?culture=ar"))
        title=next(x["Title"] for x in expected[code]["Translations"] if x["CultureCode"]=="ar")
        validate_html(page,code,title)
        try: english=html.unescape(get_page(student,
              "/student/learning/lesson/"+lesson_id+"?culture=en"))
        except urllib.error.HTTPError as exc: assert exc.code in (403,404,406)
        else: validate_html(english,code,title)
        teacher_only=any(code.endswith(s) for s in TEACHER_ONLY)
        row={"ordinal":ordinal,"lessonCode":code,"lessonId":lesson_id,
             "pageSha256":hashlib.sha256(page.encode()).hexdigest(),
             "gradingRoute":"TEACHER_REVIEW_REQUIRED" if teacher_only else "DIGITAL_ELIGIBLE"}
        if not teacher_only:
            # Deliberately fails while candidate G9 contracts are still PREPARED
            # and no independent answer verifier is runtime-authorized.
            form=get_page(student,"/student/practice")
            adopt=extract_adoption(form);csrf=extract_csrf(form);nonce=extract_idempotency_key(form)
            assert adopt and adopt.lower()==adoption.lower() and csrf and nonce
            correct=probe_lesson(student,BASE,lesson_id,adopt,csrf,nonce,sql)
            assert correct.get("status")=="SELF_CONSISTENT_GRADE_PERSISTED",(code,correct)
            pass_count+=1
            form=get_page(student,"/student/practice")
            wrong=probe_lesson(student,BASE,lesson_id,extract_adoption(form),
                extract_csrf(form),extract_idempotency_key(form),sql,
                answer_mode="well_formed_wrong",wrong_answer_profile="uae-g9")
            assert wrong.get("status")=="WELL_FORMED_WRONG_ANSWER_REJECTED_AND_PERSISTED",(code,wrong)
            wrong_count+=1
            form=get_page(student,"/student/practice")
            invalid=probe_lesson(student,BASE,lesson_id,extract_adoption(form),
                extract_csrf(form),extract_idempotency_key(form),sql,answer_mode="malformed")
            assert invalid.get("status")=="MALFORMED_ANSWER_REJECTED_AND_PERSISTED",(code,invalid)
            malformed_count+=1
            row.update({"correct":correct,"wellFormedWrong":wrong,"malformed":invalid})
        records.append(row)
        print("UAE_G9_REAL_HTTPS_LESSON",ordinal,code,flush=True)
    assert len(records)==82 and pass_count==wrong_count==malformed_count==80
    result={"candidateSha":os.environ["GITHUB_SHA"],"scope":"UAE_G9_GENERAL_DISPOSABLE",
       "grade":9,"pathway":"General","academicLanguage":"ar",
       "roleLogins":{"student":True,"teacher":True,"supervisor":True,"admin":True,"foreign":True},
       "sourceLiteralParity":"UNKNOWN","teacherSignatures":"NOT_CLAIMED",
       "crossSchoolAccessDenied":foreign_denied,"adoptionKey":key,
       "lessonPageCount":82,"negativeEnglishFallbackChecks":82,
       "eligibleDigital":80,"teacherOnly":2,
       "correctAttemptPersistenceCount":pass_count,
       "wrongAttemptPersistenceCount":wrong_count,
       "malformedAttemptRejectionCount":malformed_count,
       "productionWritten":False,"lessons":records}
    OUT.write_text(json.dumps(result,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
    print("UAE_G9_82_REAL_HTTPS_80_DIGITAL_GRADERS_PASS",flush=True)

if __name__=="__main__":run()
