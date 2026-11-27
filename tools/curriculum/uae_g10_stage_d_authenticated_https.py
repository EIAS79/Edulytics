#!/usr/bin/env python3
"""Stage D.3 diagnostic: real four-role HTTPS and all 148 G10 pages, disposable only.
Probe declared digital candidates in isolated diagnostic or explicit release-token mode.
No source parity, rights, teacher rubric or complete digital eligibility claim.
"""
import hashlib, html, json, os, re, sys, urllib.error
from pathlib import Path
sys.path.insert(0,str(Path(__file__).resolve().parents[1]/"phase29"))
from g5_stage_d_authenticated_https import (
    sql, login_actor, get_page, BASE, UUID,
    extract_adoption, extract_csrf, extract_idempotency_key, probe_lesson
)
ROOT=Path(__file__).resolve().parents[2]
CONTENT=ROOT/"src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json"
BLUEPRINT=ROOT/"src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g10-general-t1-ogl-v1.lesson-blueprint.json"
MAPPINGS=ROOT/"src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json"
OUT=Path("/tmp/uae-g10-stage-d-authenticated-148-lessons.json")
PREFIX="PED:UAE-MOE-MATH:L10:GENERAL:"
SCHOOL="REHEARSAL-AE-SECONDARY"
STUDENT="uae-secondary.student.primary@edulytiks.com"
TEACHER="uae-secondary.teacher.secondary@edulytiks.com"
SUPERVISOR="uae-secondary.supervisor@edulytiks.com"
ADMIN="uae-secondary.admin@edulytiks.com"
FOREIGN="commoncore-secondary.student.primary@edulytiks.com"

def required_environment():
    for k,v in {"GITHUB_ACTIONS":"true","EDULYTICS_CI_REHEARSAL":"true",
                "EDULYTICS_CI_REHEARSAL_PROGRAMME":"UAE-G10",
                "EDULYTICS_CLEAN_DATABASE_NAME":"edulytics_clean_ci",
                "ASPNETCORE_ENVIRONMENT":"Staging"}.items():
        if os.environ.get(k)!=v: raise RuntimeError("G10 HTTPS CI only: "+k)
    assert BASE=="https://localhost:5052"

def check_catalogue():
    pack=json.loads(CONTENT.read_text(encoding="utf-8-sig"))
    bp=json.loads(BLUEPRINT.read_text(encoding="utf-8-sig"))
    assert pack["AcademicLanguage"]=="ar" and pack["Status"]=="Published"
    assert bp["LogicalLevel"]==10 and bp["Pathway"]=="General"
    expected={x["LessonCode"]:x for x in pack["Lessons"]}
    ordered=sorted(bp["Lessons"],key=lambda x:x["SortOrder"])
    assert len(bp["Units"])==18 and len(ordered)==len(expected)==148
    assert [x["SortOrder"] for x in ordered]==list(range(1,149))
    assert {x["LessonCode"] for x in ordered}==set(expected)
    assert all(k.startswith(PREFIX) for k in expected)
    db=json.loads(sql("""SELECT COALESCE(json_agg(json_build_object(
        'code',"Code",'id',"Id"::text,'title',"Title",'pathway',"Pathway")),'[]')
       FROM "CurriculumPedagogicalLessons"
       WHERE "Code" LIKE 'PED:UAE-MOE-MATH:L10:%';"""))
    assert len(db)==148 and all(x["pathway"]=="General" for x in db)
    rows={x["code"]:x for x in db}
    assert set(rows)==set(expected) and len({x["id"] for x in db})==148
    pairs=[]
    for b in ordered:
        code=b["LessonCode"]; row=rows[code]
        title=next(t["Title"] for t in expected[code]["Translations"] if t["CultureCode"]=="ar")
        assert title==b["Title"]==row["title"],code
        assert re.fullmatch(UUID,row["id"])
        pairs.append((code,row["id"]))
    published=sql("""SELECT COUNT(*)::text FROM "CurriculumLessonContents" c
       JOIN "CurriculumPedagogicalLessons" p ON p."Id"=c."LessonNodeId"
       WHERE p."Code" LIKE 'PED:UAE-MOE-MATH:L10:GENERAL:%' AND c."Status"=3;""").strip()
    assert published=="148",f"G10 Published metadata not 148: {published}"
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
    assert len(rows)==1,"G10 only one exact student school adoption"
    adopt,level,key,culture,pathway=rows[0].split("|",4)
    assert re.fullmatch(UUID,adopt) and level=="10" and culture=="ar" and pathway=="General",(level,culture,pathway)
    return adopt,key

def validate_html(page,code,title):
    assert title in page,"missing Arabic G10 lesson title: "+code
    for part in ("explanation","concepts","examples","steps","mistakes","summary"):
        assert "lesson-learning-section--"+part in page,"missing "+part+": "+code
    assert re.search(r"[\u0600-\u06ff]",page)
    for bad in ("Concept model","Instructional diagram","YouTube discovery could not load right now",
                "McGraw-Hill","McGraw Hill","McGraw-Hill Education"):
        assert bad not in page,"G10 unapproved fallback/source branding: "+code

def run():
    required_environment()
    release_mode = os.environ.get("EDULYTICS_G10_DIGITAL_PRACTICE_RELEASE") == "CONFIRM_RELEASE_UAE_GRADE10_GENERAL_148_DIGITAL_PRACTICE"
    diagnostic_mode = os.environ.get("EDULYTICS_G10_CI_PRACTICE_REHEARSAL") == "EXERCISE_97_DECLARED_CANDIDATES_ONLY"
    assert release_mode != diagnostic_mode, "G10 requires exactly one explicit Practice mode"
    pairs,expected=check_catalogue()
    adoption,key=school_adoption()
    mappings={x["lessonCode"]:x for x in json.loads(MAPPINGS.read_text(encoding="utf-8-sig"))["mappings"]
              if x["lessonCode"].startswith(PREFIX)}
    assert set(mappings)==set(expected)
    student=login_actor(STUDENT,"Student")
    teacher=login_actor(TEACHER,"Teacher")
    supervisor=login_actor(SUPERVISOR,"SubjectSupervisor")
    admin=login_actor(ADMIN,"SchoolAdmin")
    foreign=login_actor(FOREIGN,"Student")
    assert "assessments" in get_page(teacher,"/school/assessments").lower()
    try:
        foreign_page=get_page(foreign,"/student/learning/lesson/"+pairs[0][1]+"?culture=ar")
    except urllib.error.HTTPError as exc:
        foreign_denied=exc.code in (401,403,404)
    else:
        foreign_denied="lesson-learning-section--explanation" not in foreign_page
    records=[]
    for ordinal,(code,lesson_id) in enumerate(pairs,1):
        m=mappings[code]; families=m.get("allowedQuestionFamilies") or []
        row={"ordinal":ordinal,"lessonCode":code,"lessonId":lesson_id,
             "declaredFamilyCount":len(families),"practiceReadiness":m.get("practiceReadiness"),
             "pageStatus":"FAIL","englishRequestStatus":"NOT_RUN","practiceStatus":"NOT_RUN"}
        try:
            page=html.unescape(get_page(student,"/student/learning/lesson/"+lesson_id+"?culture=ar"))
            title=next(t["Title"] for t in expected[code]["Translations"] if t["CultureCode"]=="ar")
            validate_html(page,code,title)
            row.update(pageStatus="PASS",pageSha256=hashlib.sha256(page.encode()).hexdigest())
            try:
                english=html.unescape(get_page(student,"/student/learning/lesson/"+lesson_id+"?culture=en"))
            except urllib.error.HTTPError as exc:
                assert exc.code in (403,404,406)
            else:
                validate_html(english,code,title)
            row["englishRequestStatus"]="PASS"
        except (AssertionError,ValueError,urllib.error.URLError,TimeoutError) as exc:
            row["pageErrorType"]=type(exc).__name__
            row["pageErrorDetail"]=str(exc)[:350]
            print("G10_PAGE_FAILURE",ordinal,row["pageErrorType"],row["pageErrorDetail"],flush=True)
        if families:
            # Exercise only declared G10 candidate contracts via the real
            # runtime in isolated CI. Release-token mode is tested separately.
            # Generation/QA denial remains a blocker, not a passed attempt.
            for field,mode,required in (
                ("correct","correct","SELF_CONSISTENT_GRADE_PERSISTED"),
                ("wrong","well_formed_wrong","WELL_FORMED_WRONG_ANSWER_REJECTED_AND_PERSISTED"),
                ("malformed","malformed","MALFORMED_ANSWER_REJECTED_AND_PERSISTED")):
                try:
                    form=get_page(student,"/student/practice")
                    adopt=extract_adoption(form);csrf=extract_csrf(form);nonce=extract_idempotency_key(form)
                    assert adopt and adopt.lower()==adoption.lower() and csrf and nonce
                    # Reuse the existing typed numerical/category negative
                    # fixture where supported. Any unsupported G10 answer
                    # shape fails closed; no malformed sentinel counts as wrong.
                    result=probe_lesson(student,BASE,lesson_id,adopt,csrf,nonce,sql,
                        answer_mode=mode,wrong_answer_profile="uae-g9" if mode=="well_formed_wrong" else None)
                    row[field]=result
                    row[field+"Passed"]=(result.get("status")==required and result.get("family") in families)
                    if not row[field+"Passed"]:
                        print("G10_PRACTICE_FAILURE",ordinal,field,result.get("status"),result.get("family"),flush=True)
                except (AssertionError,ValueError,urllib.error.URLError,TimeoutError) as exc:
                    row[field]={"status":"PROBE_FAILED","errorType":type(exc).__name__,"detail":str(exc)[:300]}
                    print("G10_PRACTICE_FAILURE",ordinal,field,type(exc).__name__,str(exc)[:300],flush=True)
                    row[field+"Passed"]=False
            row["practiceStatus"]="PASS" if all(row.get(k+"Passed") for k in ("correct","wrong","malformed")) else "FAIL"
        else:
            row["practiceStatus"]="NO_DECLARED_DIGITAL_FAMILY_TEACHER_REVIEW_PENDING"
            row["teacherReviewRequired"]=bool(m.get("teacherReviewRequiredFor"))
        records.append(row)
        print("UAE_G10_REAL_HTTPS_LESSON",ordinal,code,row["pageStatus"],row["practiceStatus"],flush=True)
    total=sum(x["declaredFamilyCount"]>0 for x in records)
    pages=sum(x["pageStatus"]=="PASS" for x in records)
    language=sum(x["englishRequestStatus"]=="PASS" for x in records)
    correct=sum(x.get("correctPassed",False) for x in records)
    wrong=sum(x.get("wrongPassed",False) for x in records)
    malformed=sum(x.get("malformedPassed",False) for x in records)
    result={"candidateSha":os.environ["GITHUB_SHA"],"scope":"UAE_G10_GENERAL_DISPOSABLE",
       "grade":10,"pathway":"General","academicLanguage":"ar",
       "roleLogins":{"student":True,"teacher":True,"supervisor":True,"admin":True,"foreign":True},
       "sourceLiteralParity":"UNKNOWN","teacherReviewExecution":"NOT_CERTIFIED",
       "releaseTokenMode":release_mode,
       "crossSchoolAccessDenied":foreign_denied,"adoptionKey":key,"checkedLessons":len(records),
       "lessonPageCount":pages,"negativeEnglishFallbackChecks":language,
       "declaredDigitalCandidateLessons":total,
       "noDeclaredDigitalFamilyLessons":len(records)-total,
       "fullDigitalEligibilityDenominator":"NOT_CERTIFIED",
       "correctAttemptPersistenceCount":correct,"wrongAttemptPersistenceCount":wrong,
       "malformedAttemptRejectionCount":malformed,
       "productionWritten":False,"lessons":records}
    OUT.write_text(json.dumps(result,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
    assert len(records)==148 and pages==language==148 and foreign_denied
    assert total>0 and correct==wrong==malformed==total,"G10 declared digital routes blocked or not verified"

if __name__=="__main__":run()
