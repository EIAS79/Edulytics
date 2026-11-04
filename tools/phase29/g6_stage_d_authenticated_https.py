#!/usr/bin/env python3
"""Real Grade 6 Stage D HTTPS/DB rehearsal. Disposable GitHub CI ONLY."""
import hashlib, html, http.cookiejar, json, os, re, ssl, subprocess
import urllib.error, urllib.parse, urllib.request
from pathlib import Path
from b001_practice_attempt_probe import probe_lesson,extract_csrf,extract_adoption,extract_idempotency_key
from b001_practice_form_coverage import has_lesson_start_form,has_lesson_start_nonce

ROOT=Path(__file__).resolve().parents[2]
BASE="https://localhost:5052"
CTX=ssl.create_default_context(cafile="/tmp/phase-uat-cert.pem")
PASSWORD=os.environ["Edulytics__MeetingDemo__Password"]
OUT=Path("/tmp/g6-147-isolated-auth-https-evidence.json")
REPORT={"recordType":"G6_STAGE_D_AUTHENTICATED_HTTPS_DISPOSABLE", "lessonCount":147,
        "candidateSha":os.getenv("G6_CANDIDATE_SHA"),"runId":os.getenv("GITHUB_RUN_ID"),
        "sourceParity":"UNKNOWN","teacherAcademicApproval":"NOT_CLAIMED",
        "releaseDecision":"NO_GO_TECHNICAL","lessons":[],"status":"INCOMPLETE"}

def require(cond, message):
    if not cond: raise AssertionError("G6_AUTH_FAIL_CLOSED "+str(message))
def sql(query):
    return subprocess.check_output(["psql","-h","127.0.0.1","-U","postgres","-d","edulytics_clean_ci",
        "-A","-t","-F","|","-c",query], text=True, timeout=50).strip()
def save():
    OUT.write_text(json.dumps(REPORT,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
def connect(school,role):
    require(school in ("commoncore-middle","commoncore-secondary"),"unscoped school")
    require(role in ("Student","Teacher","SubjectSupervisor","SchoolAdmin"),"unscoped role")
    suffix={"Student":"student.primary","Teacher":"teacher.secondary",
            "SubjectSupervisor":"supervisor","SchoolAdmin":"admin"}[role]
    user=f"{school}.{suffix}@edulytiks.com"
    jar=http.cookiejar.CookieJar()
    opener=urllib.request.build_opener(urllib.request.HTTPCookieProcessor(jar),
        urllib.request.HTTPSHandler(context=CTX))
    with opener.open(BASE+"/account/login?culture=en",timeout=25) as response:
        login=response.read().decode("utf-8")
    match=re.search(r'name="__RequestVerificationToken"[^>]*value="([^"]+)"',login)
    require(match,"login CSRF missing "+role)
    payload=urllib.parse.urlencode({"Email":user,"Password":PASSWORD,"AccountType":role,
        "__RequestVerificationToken":match.group(1)}).encode()
    with opener.open(urllib.request.Request(BASE+"/account/login?culture=en",
        method="POST",data=payload),timeout=25) as response:
        require(response.status==200 and urllib.parse.urlsplit(response.url).path.lower()!="/account/login",
            "login failed "+role)
    require(any(c.secure for c in jar),"cookie not secure "+role)
    return opener
def page(opener,route):
    require(route.startswith("/") and not route.startswith("//"),"invalid route")
    with opener.open(BASE+route,timeout=40) as response:
        require(response.status==200 and urllib.parse.urlsplit(response.url).path.lower()!="/account/login",
            "page denied "+route)
        return html.unescape(response.read().decode("utf-8"))
def cross_denied(opener,route):
    try:
        with opener.open(BASE+route,timeout=40) as response:
            require(urllib.parse.urlsplit(response.url).path.lower()!=route.lower(),
                "cross-school protected lesson exposed "+route)
    except urllib.error.HTTPError as error:
        require(error.code in (401,403,404),"unexpected foreign school response "+str(error.code))
def main():
    require(os.getenv("GITHUB_ACTIONS")=="true" and os.getenv("EDULYTICS_CI_REHEARSAL")=="true"
        and os.getenv("EDULYTICS_CI_REHEARSAL_PROGRAMME")=="US-CCSS-G6"
        and os.getenv("ASPNETCORE_ENVIRONMENT")=="Staging","not isolated GitHub G6 environment")
    require(os.getenv("G6_CANDIDATE_SHA") and len(os.environ["G6_CANDIDATE_SHA"])==40,
        "candidate SHA unbound")
    require(sql('SELECT "SchoolCode" FROM "Schools" ORDER BY "SchoolCode";')
        =="REHEARSAL-US-MIDDLE\nREHEARSAL-US-SECONDARY","wrong disposable school scope")
    require(sql('SELECT count(*) FROM "AspNetUsers";')=="12","wrong synthetic user count")
    classes=sql("""SELECT c."Id"::text FROM "ClassGroups" c
        JOIN "Schools" s ON s."Id"=c."SchoolId"
        WHERE s."SchoolCode"='REHEARSAL-US-MIDDLE'
        AND c."Code" LIKE 'L07-%';""").splitlines()
    require(len(classes)==1 and re.fullmatch(r"[0-9a-fA-F-]{36}",classes[0]),"Grade 6 class missing")
    move=sql(f"""WITH moved AS (
        UPDATE "StudentEnrollments" e SET "ClassGroupId"='{classes[0]}'
        FROM "StudentProfiles" p
        JOIN "AspNetUsers" u ON u."Id"=p."UserId"
        JOIN "Schools" s ON s."Id"=p."SchoolId"
        WHERE e."StudentProfileId"=p."Id" AND e."SchoolId"=p."SchoolId"
          AND s."SchoolCode"='REHEARSAL-US-MIDDLE'
          AND u."Email"='commoncore-middle.student.primary@edulytiks.com'
        RETURNING e."Id") SELECT count(*) FROM moved;""")
    require(move=="1","synthetic learner Grade6 re-enrol failed")
    student=connect("commoncore-middle","Student")
    foreign=connect("commoncore-secondary","Student")
    teacher=connect("commoncore-middle","Teacher")
    supervisor=connect("commoncore-middle","SubjectSupervisor")
    admin=connect("commoncore-middle","SchoolAdmin")
    page(teacher,"/school/assessments")
    page(supervisor,"/school/reports")
    page(admin,"/school/assessments")
    rows=sql("""SELECT "Code","Id"::text,"Title","NativeLevel"
        FROM "CurriculumPedagogicalLessons"
        WHERE "Code" LIKE 'PED:US-CCSS-MATH:G6:%'
        ORDER BY "SortOrder";""").splitlines()
    bp=json.loads((ROOT/"src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/us-ccss-math-grade6-our-2017.lesson-blueprint.json").read_text(encoding="utf-8"))
    pack=json.loads((ROOT/"src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g6-phase29-v1.lesson-content-pack.json").read_text(encoding="utf-8"))
    expected=[x["LessonCode"] for x in sorted(bp["Lessons"],key=lambda x:(x["UnitNumber"],x["LessonNumber"]))]
    require(len(expected)==147 and len(set(expected))==147 and len(rows)==147
        and len(pack["lessons"])==147,"missing G6 source or database lessons")
    by_code={x["lessonCode"]:x for x in pack["lessons"]}
    require(len(by_code)==147,"duplicate authored content code")
    for ordinal,row in enumerate(rows,1):
        code,lesson_id,db_title,level=row.split("|",3)
        require(code==expected[ordinal-1] and level=="Grade 6"
            and re.fullmatch(r"[0-9a-fA-F-]{36}",lesson_id),"identity mismatch "+code)
        english=[x for x in by_code[code]["translations"] if x["cultureCode"]=="en"]
        require(len(english)==1,"missing first-party English content "+code)
        body=page(student,"/student/learning/lesson/"+lesson_id)
        require("rich-lesson-section" in body,"no authenticated Rich V2 "+code)
        visible=re.sub(r"\s+"," ",re.sub(r"<[^>]+>"," ",body)).casefold()
        title=re.sub(r"\s+"," ",english[0]["title"]).casefold()
        goal=re.sub(r"\s+"," ",english[0]["explanation"].split("\n",1)[0]).casefold()
        require(title in visible,"source-bound title absent from rendered learner page "+code)
        require(goal[:48] in visible,"authored learning goal absent from rendered learner page "+code)
        # A green title/goal is insufficient: each canonical teaching section
        # must be readable in the same authenticated learner page. Compare
        # first eight normalized source tokens for all six independently.
        visible_words=" ".join(re.findall(r"\w+",visible,flags=re.UNICODE))
        for field in ("explanation","keyConceptsAndRules","workedExamples",
                      "stepByStepSolutions","commonMistakes","quickSummary"):
            authored=english[0].get(field)
            require(isinstance(authored,str) and len(authored.strip())>=20,
                "missing canonical Grade6 section "+code+" "+field)
            source_words=re.findall(r"\w+",html.unescape(authored).casefold(),flags=re.UNICODE)
            require(len(source_words)>=8,"too-short source section "+code+" "+field)
            require(" ".join(source_words[:8]) in visible_words,
                "canonical source section not rendered to authenticated student "+code+" "+field)
        require(has_lesson_start_form(body) and has_lesson_start_nonce(body),
            "lesson Practice form or unique nonce absent "+code)
        cross_denied(foreign,"/student/learning/lesson/"+lesson_id)
        f=page(student,"/student/practice")
        correct=probe_lesson(student,BASE,lesson_id,extract_adoption(f),
            extract_csrf(f),extract_idempotency_key(f),sql)
        require(correct.get("status")=="SELF_CONSISTENT_GRADE_PERSISTED",
            "correct Grade6 attempt failed "+code+" "+repr(correct))
        family=correct.get("family")
        unit_lesson=re.fullmatch(r"PED:US-CCSS-MATH:G6:U(\d{2}):L(\d{2})",code)
        require(unit_lesson is not None,"invalid G6 canonical lesson code")
        exact_family_prefix=f"usccss.g6.u{unit_lesson.group(1)}.l{unit_lesson.group(2)}."
        require(isinstance(family,str) and family.startswith(exact_family_prefix),
            "unrelated lesson-level generator family "+code+" "+str(family))
        f=page(student,"/student/practice")
        negative=probe_lesson(student,BASE,lesson_id,extract_adoption(f),
            extract_csrf(f),extract_idempotency_key(f),sql,answer_mode="malformed")
        require(negative.get("status")=="MALFORMED_ANSWER_REJECTED_AND_PERSISTED",
            "incorrect answer accepted or not persisted "+code+" "+repr(negative))
        f=page(student,"/student/practice")
        valid_wrong=probe_lesson(student,BASE,lesson_id,extract_adoption(f),
            extract_csrf(f),extract_idempotency_key(f),sql,
            answer_mode="well_formed_wrong")
        require(valid_wrong.get("status")=="WELL_FORMED_WRONG_ANSWER_REJECTED_AND_PERSISTED",
            "well-formed mathematical wrong answer incorrectly accepted or not persisted "+code+" "+repr(valid_wrong))
        require(isinstance(valid_wrong.get("family"),str) and
            valid_wrong["family"].startswith(exact_family_prefix),
            "valid-wrong attempt generated unrelated lesson family "+code)
        require(isinstance(negative.get("family"),str) and
            negative["family"].startswith(exact_family_prefix),
            "malformed attempt generated unrelated lesson family "+code)
        REPORT["lessons"].append({"ordinal":ordinal,"lessonCode":code,"unit":int(re.search(r":G6:U(\d{2}):L\d{2}$",code).group(1)),
            "authenticatedLearnerRichV2":"PASS","firstPartyTitleAndGoal":"PASS",
            "sixCanonicalInstructionalSections":"PASS","exactLessonScopedPracticeFamily":"PASS",
            "practiceFormAndNonce":"PASS","realGeneratedFamily":family,
            "correctAnswerStoredWithScoreOne":"PASS","malformedAnswerStoredWithScoreZero":"PASS",
            "wellFormedWrongStoredWithScoreZero":"PASS",
            "foreignSchoolLessonAccess":"DENIED","teacherAssessment":"NOT_EVALUATED",
            "sourceParity":"UNKNOWN","learnerHtmlSha256":hashlib.sha256(body.encode()).hexdigest()})
        save()
        print("G6_D_AUTH_HTTPS_PASS",ordinal,code,family,flush=True)
    require(len(REPORT["lessons"])==147,"incomplete G6 real-runtime coverage")
    # Real Grade 6 teacher assessment creation against the SAME disposable DB.
    # Creating a draft is not approval, publication or learner submission.
    import datetime
    import uuid
    fields=sql(f"""SELECT c."Id"::text,a."SubjectId"::text,t."Id"::text
        FROM "ClassGroups" c
        JOIN "SchoolCurriculumAdoptions" a ON a."Id"=c."CurriculumAdoptionId"
        JOIN "Terms" t ON t."AcademicYearId"=c."AcademicYearId"
        WHERE c."Id"='{classes[0]}' AND t."Name"='Term 1'
        ORDER BY t."Id" LIMIT 1;""").split("|")
    require(len(fields)==3 and fields[0].lower()==classes[0].lower(),
        "Grade6 assessment class/subject/term fixture missing")
    teaching_page=page(teacher,"/school/assessments")
    csrf=re.search(r'name="__RequestVerificationToken"[^>]*value="([^"]+)"',teaching_page)
    require(csrf,"real teacher assessment CSRF missing")
    title="G6 Stage D CI "+uuid.uuid4().hex[:12]
    data=urllib.parse.urlencode({
        "classGroupId":fields[0],"subjectId":fields[1],"termId":fields[2],
        "title":title,"assessmentDate":datetime.date.today().isoformat(),
        "maxScore":"100","assessmentType":"Exam","deliveryMode":"Online",
        "__RequestVerificationToken":csrf.group(1)}).encode()
    with teacher.open(urllib.request.Request(BASE+"/school/assessments",
            data=data,method="POST"),timeout=40) as response:
        require(response.status==200,"teacher assessment POST not accepted")
    saved=sql(f"""SELECT a."Id"::text FROM "Assessments" a
        JOIN "Schools" s ON s."Id"=a."SchoolId"
        WHERE a."Title"='{title}'
          AND a."ClassGroupId"='{classes[0]}'
          AND s."SchoolCode"='REHEARSAL-US-MIDDLE';""").splitlines()
    require(len(saved)==1 and re.fullmatch(r"[0-9a-fA-F-]{36}",saved[0]),
        "Grade6 teacher assessment not persisted correctly")
    other_teacher=connect("commoncore-secondary","Teacher")
    cross_denied(other_teacher,"/school/assessments/"+saved[0])
    REPORT["grade6TeacherAssessmentCreatedAndStored"]=True
    REPORT["crossSchoolTeacherAssessmentAccess"]="DENIED"
    REPORT["teacherFinalAcademicApproval"]="NOT_CLAIMED"
    print("G6_D_TEACHER_GRADE6_ASSESSMENT_POST_AND_TENANT_DENIAL_PASS",flush=True)
    REPORT.update(status="G6_147_REAL_AUTHENTICATED_HTTPS_PASS",
        correctGradedAndPersisted=147,invalidRejectedAndPersisted=147,
        wellFormedWrongRejectedAndPersisted=147,
        crossSchoolDenied=147,studentRichPagesPassed=147,stageDVerified=0)
    save()
    print("G6_D_AUTH_HTTPS_FINAL_PASS 147/147 lessons; not academic release",flush=True)
if __name__=="__main__":
    try: main()
    except Exception as error:
        REPORT.update(status="FAIL_CLOSED",errorType=type(error).__name__,
            lastError=str(error)[:800])
        save()
        raise
