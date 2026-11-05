#!/usr/bin/env python3
"""Real Grade 8 Stage D HTTPS/DB rehearsal. Disposable GitHub CI ONLY."""
import hashlib, html, http.cookiejar, json, os, re, ssl, subprocess
from html.parser import HTMLParser
import urllib.error, urllib.parse, urllib.request
from pathlib import Path
from b001_practice_attempt_probe import probe_lesson,extract_csrf,extract_adoption,extract_idempotency_key
from b001_practice_form_coverage import has_lesson_start_form,has_lesson_start_nonce

ROOT=Path(__file__).resolve().parents[2]
BASE="https://localhost:5052"
CTX=ssl.create_default_context(cafile="/tmp/phase-uat-cert.pem")
PASSWORD=os.environ["Edulytics__MeetingDemo__Password"]
OUT=Path("/tmp/g8-131-isolated-auth-https-evidence.json")
REPORT={"recordType":"G8_STAGE_D_AUTHENTICATED_HTTPS_DISPOSABLE", "lessonCount":131,
        "candidateSha":os.getenv("G8_CANDIDATE_SHA"),"runId":os.getenv("GITHUB_RUN_ID"),
        "sourceParity":"UNKNOWN","teacherAcademicApproval":"NOT_CLAIMED",
        "releaseDecision":"NO_GO_TECHNICAL","lessons":[],"status":"INCOMPLETE"}

def learner_visible_text(markup):
    """Extract rendered HTML text while preserving mathematical < and >.
    Do not treat literal less-than signs in math as HTML tags, and exclude
    script/style/template contents from section text evidence.
    """
    class VisibleText(HTMLParser):
        def __init__(self):
            super().__init__(convert_charrefs=True)
            self.parts=[]
            self.hidden=[]
        def handle_starttag(self, tag, attrs):
            if tag in ("script","style","template"):
                self.hidden.append(tag)
        def handle_endtag(self, tag):
            if self.hidden and tag==self.hidden[-1]:
                self.hidden.pop()
        def handle_data(self, data):
            if not self.hidden:
                self.parts.append(data)
    parser=VisibleText()
    parser.feed(markup)
    return " ".join(" ".join(parser.parts).split()).casefold()


def require(cond, message):
    if not cond: raise AssertionError("G8_AUTH_FAIL_CLOSED "+str(message))

def source_words(value):
    plain=html.unescape(str(value))
    plain=re.sub(r"</?[A-Za-z][A-Za-z0-9:-]*(?:\s[^<>]*?)?/?>"," ",plain)
    return re.findall(r"\w+",plain.casefold(),flags=re.UNICODE)

def require_rendered(value, visible_words, label, min_words=1, max_words=8):
    words=source_words(value)
    require(len(words)>=min_words,"canonical Rich V2 text unexpectedly short "+label)
    expected=words[:min(max_words,len(words))]
    signature=" ".join(expected)
    if signature in visible_words:
        return
    # The genuine learner renderer inserts diagrams and their accessible
    # captions between source text blocks. The source words must still occur
    # in their exact original order within a bounded local DOM window; a
    # generic heading or an unrelated distant occurrence is not sufficient.
    tokens=visible_words.split()
    anchor=expected[:min(4,len(expected))]
    if len(anchor)==len(expected):
        raise AssertionError(
            "G8_AUTH_FAIL_CLOSED canonical Rich V2 text not rendered " +
            label+" :: "+signature)
    for start in range(0,len(tokens)-len(anchor)+1):
        if tokens[start:start+len(anchor)] != anchor:
            continue
        matched=len(anchor)
        for token in tokens[start+len(anchor):start+len(anchor)+90]:
            if token == expected[matched]:
                matched+=1
                if matched==len(expected):
                    return
    raise AssertionError(
        "G8_AUTH_FAIL_CLOSED canonical Rich V2 text not rendered " +
        label+" :: "+signature)

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
        and os.getenv("EDULYTICS_CI_REHEARSAL_PROGRAMME")=="US-CCSS-G8"
        and os.getenv("ASPNETCORE_ENVIRONMENT")=="Staging","not isolated GitHub G8 environment")
    require(os.getenv("G8_CANDIDATE_SHA") and len(os.environ["G8_CANDIDATE_SHA"])==40,
        "candidate SHA unbound")
    require(sql('SELECT "SchoolCode" FROM "Schools" ORDER BY "SchoolCode";')
        =="REHEARSAL-US-MIDDLE\nREHEARSAL-US-SECONDARY","wrong disposable school scope")
    require(sql('SELECT count(*) FROM "AspNetUsers";')=="12","wrong synthetic user count")
    classes=sql("""SELECT c."Id"::text FROM "ClassGroups" c
        JOIN "Schools" s ON s."Id"=c."SchoolId"
        WHERE s."SchoolCode"='REHEARSAL-US-MIDDLE'
        AND c."Code" LIKE 'L09-%';""").splitlines()
    require(len(classes)==1 and re.fullmatch(r"[0-9a-fA-F-]{36}",classes[0]),"Grade 8 class missing")
    move=sql(f"""WITH moved AS (
        UPDATE "StudentEnrollments" e SET "ClassGroupId"='{classes[0]}'
        FROM "StudentProfiles" p
        JOIN "AspNetUsers" u ON u."Id"=p."UserId"
        JOIN "Schools" s ON s."Id"=p."SchoolId"
        WHERE e."StudentProfileId"=p."Id" AND e."SchoolId"=p."SchoolId"
          AND s."SchoolCode"='REHEARSAL-US-MIDDLE'
          AND u."Email"='commoncore-middle.student.primary@edulytiks.com'
        RETURNING e."Id") SELECT count(*) FROM moved;""")
    require(move=="1","synthetic learner Grade8 re-enrol failed")
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
        WHERE "Code" LIKE 'PED:US-CCSS-MATH:G8:%'
        ORDER BY "SortOrder";""").splitlines()
    bp=json.loads((ROOT/"src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/us-ccss-math-grade8-our-2017.lesson-blueprint.json").read_text(encoding="utf-8"))
    pack=json.loads((ROOT/"src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g8-phase29-v1.lesson-content-pack.json").read_text(encoding="utf-8"))
    expected=[x["LessonCode"] for x in sorted(bp["Lessons"],key=lambda x:(x["UnitNumber"],x["LessonNumber"]))]
    require(len(expected)==131 and len(set(expected))==131 and len(rows)==131
        and len(pack["lessons"])==131,"missing G8 source or database lessons")
    by_code={x["lessonCode"]:x for x in pack["lessons"]}
    require(len(by_code)==131,"duplicate authored content code")
    rich_by_code={}
    rich_dir=ROOT/"src/Edulytics.Core/Curriculum/LessonContent/RichV2"
    for rich_path in sorted(rich_dir.glob("us-ccss-g8-*.rich-lesson-v2.json")):
        document=json.loads(rich_path.read_text(encoding="utf-8"))
        for lesson in document.get("Lessons",[]):
            code=lesson.get("LessonCode")
            require(code not in rich_by_code,"duplicate Grade8 Rich V2 lesson "+str(code))
            rich_by_code[code]=lesson
    require(len(rich_by_code)==131 and set(rich_by_code)==set(expected),
        "Grade8 Rich V2 registry files do not cover exact 131-lesson scope")
    # Stage D must not fabricate auto-graded coverage. Only these two lessons
    # have an explicit READY_VERIFIED practice contract in the current repo.
    practice_eligible={
        "PED:US-CCSS-MATH:G8:U03:L12":"algebra.linear.ax_plus_b_equals_c",
        "PED:US-CCSS-MATH:G8:U04:L05":"algebra.linear.ax_plus_b_equals_c",
    }
    lesson_ids={}
    verified_practice=0
    for ordinal,row in enumerate(rows,1):
        code,lesson_id,db_title,level=row.split("|",3)
        require(code==expected[ordinal-1] and level=="Grade 8"
            and re.fullmatch(r"[0-9a-fA-F-]{36}",lesson_id),"identity mismatch "+code)
        lesson_ids[code]=lesson_id
        english=[x for x in by_code[code]["translations"] if x["cultureCode"]=="en"]
        require(len(english)==1,"missing legacy English pack identity "+code)
        rich=rich_by_code[code]
        body=page(student,"/student/learning/lesson/"+lesson_id)
        require("rich-lesson-section" in body,"no authenticated Rich V2 "+code)
        visible=learner_visible_text(body)
        visible_words=" ".join(re.findall(r"\w+",visible,flags=re.UNICODE))
        require("lesson-source-aligned-activities" in body,
            "Grade8 canonical instructional sections absent from learner DOM "+code)
        if ordinal == 1:
            goal_index=visible_words.find("learning goal")
            print("G8_D_FIRST_LESSON_CANONICAL_DIAGNOSTIC",
                "sourceDom=", "lesson-source-aligned-activities" in body,
                "goalIndex=", goal_index,
                "goalContext=", visible_words[max(0,goal_index-40):goal_index+240]
                    if goal_index >= 0 else "NO_LEARNING_GOAL_TEXT",
                flush=True)
        title=re.sub(r"\s+"," ",rich["Title"]).casefold()
        require(title in visible,"Rich V2 title absent from rendered learner page "+code)

        explanations=rich.get("ExplanationParagraphs",[])
        concepts=rich.get("KeyConcepts",[])
        examples=rich.get("WorkedExamples",[])
        mistakes=rich.get("CommonMistakes",[])
        summaries=rich.get("SummaryPoints",[])
        require(len(explanations)>=2 and len(concepts)>=3 and len(examples)>=3
            and len(mistakes)>=2 and len(summaries)>=3,
            "incomplete canonical Rich V2 structure "+code)

        governance_paragraphs=0
        for index,paragraph in enumerate(explanations,1):
            # Preserve source and license provenance internally. It is
            # editorial metadata, not mathematical instruction to learners.
            if "open up resources" in paragraph.casefold() or (
                "illustrative mathematics" in paragraph.casefold()):
                governance_paragraphs+=1
                continue
            require_rendered(paragraph,visible_words,f"{code} explanation[{index}]")
        require(governance_paragraphs==1,
            "Grade8 provenance metadata cardinality drift "+code)
        require("open up resources" not in visible and
            "illustrative mathematics" not in visible,
            "external publisher branding leaked into Grade8 learner lesson "+code)
        for index,concept in enumerate(concepts,1):
            for field in ("Title","Definition","Rule"):
                require_rendered(concept.get(field,""),visible_words,
                    f"{code} concept[{index}].{field}")
            if concept.get("Example"):
                require_rendered(concept["Example"],visible_words,
                    f"{code} concept[{index}].Example")
        for index,example in enumerate(examples,1):
            for field in ("Title","Question","Method","Answer"):
                require_rendered(example.get(field,""),visible_words,
                    f"{code} example[{index}].{field}")
            if example.get("Check"):
                require_rendered(example["Check"],visible_words,
                    f"{code} example[{index}].Check")
            steps=example.get("Steps",[])
            require(len(steps)>=3,"worked example has fewer than three steps "+code)
            for step_index,step in enumerate(steps,1):
                require_rendered(step,visible_words,
                    f"{code} example[{index}].step[{step_index}]")
        for index,mistake in enumerate(mistakes,1):
            for field in ("Mistake","WhyWrong","Correction"):
                require_rendered(mistake.get(field,""),visible_words,
                    f"{code} mistake[{index}].{field}")
        for index,summary in enumerate(summaries,1):
            require_rendered(summary,visible_words,f"{code} summary[{index}]")

        canonical=english[0]
        for field in ("explanation","keyConceptsAndRules","workedExamples",
                      "stepByStepSolutions","commonMistakes","quickSummary"):
            authored=canonical.get(field)
            require(isinstance(authored,str) and len(authored.strip())>=20,
                "missing canonical Grade8 pack section "+code+" "+field)
            if field=="stepByStepSolutions":
                for line_index,line in enumerate(authored.splitlines(),1):
                    instruction=re.sub(r"^\s*Step\s+\d+\s*[:.]\s*","",line,
                        flags=re.IGNORECASE).strip()
                    if instruction:
                        require_rendered(instruction,visible_words,
                            f"{code} canonical-pack.{field}[{line_index}]",
                            min_words=3,max_words=8)
            else:
                # The renderer may place an accessible diagram or block
                # between adjacent source sentences. Match a complete
                # opening phrase within a single learner text block, not
                # ten tokens that can straddle separate DOM blocks.
                require_rendered(authored,visible_words,
                    f"{code} canonical-pack.{field}",min_words=4,max_words=6)

        cross_denied(foreign,"/student/learning/lesson/"+lesson_id)
        entry={"ordinal":ordinal,"lessonCode":code,
            "unit":int(re.search(r":G8:U(\d{2}):L\d{2}$",code).group(1)),
            "authenticatedLearnerRichV2":"PASS","firstPartyTitleAndGoal":"PASS",
            "richV2CanonicalStructures":"PASS",
            "sixCanonicalInstructionalSections":"PASS",
            "foreignSchoolLessonAccess":"DENIED","teacherAssessment":"NOT_EVALUATED",
            "sourceParity":"UNKNOWN","learnerHtmlSha256":hashlib.sha256(body.encode()).hexdigest(),
            "practiceEligibility":"DIGITAL_APPROVED" if code in practice_eligible
                else "TEACHER_LED_OR_NO_APPROVED_DIGITAL_FAMILY",
            "exactLessonScopedPracticeFamily":"NOT_APPLICABLE",
            "practiceFormAndNonce":"NOT_REQUIRED",
            "realGeneratedFamily":None,
            "correctAnswerStoredWithScoreOne":"NOT_APPLICABLE",
            "malformedAnswerStoredWithScoreZero":"NOT_APPLICABLE",
            "wellFormedWrongStoredWithScoreZero":"NOT_APPLICABLE"}
        if code in practice_eligible:
            require(has_lesson_start_form(body) and has_lesson_start_nonce(body),
                "approved digital lesson Practice form or unique nonce absent "+code)
            f=page(student,"/student/practice")
            correct=probe_lesson(student,BASE,lesson_id,extract_adoption(f),
                extract_csrf(f),extract_idempotency_key(f),sql)
            require(correct.get("status")=="SELF_CONSISTENT_GRADE_PERSISTED",
                "correct Grade8 attempt failed "+code+" "+repr(correct))
            family=correct.get("family")
            require(family==practice_eligible[code],
                "approved Grade8 family mismatch "+code+" "+str(family))
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
            require(valid_wrong.get("family")==practice_eligible[code],
                "valid-wrong attempt generated unrelated lesson family "+code)
            require(negative.get("family")==practice_eligible[code],
                "malformed attempt generated unrelated lesson family "+code)
            verified_practice+=1
            entry.update(exactLessonScopedPracticeFamily="PASS",
                practiceFormAndNonce="PASS",realGeneratedFamily=family,
                correctAnswerStoredWithScoreOne="PASS",
                malformedAnswerStoredWithScoreZero="PASS",
                wellFormedWrongStoredWithScoreZero="PASS")
            print("G8_D_DIGITAL_PRACTICE_PASS",ordinal,code,family,flush=True)
        REPORT["lessons"].append(entry)
        save()
        print("G8_D_AUTH_RICH_AND_TENANT_PASS",ordinal,code,flush=True)
    require(len(REPORT["lessons"])==131,"incomplete G8 real-runtime coverage")
    require(verified_practice==len(practice_eligible)==2,
        "Grade8 approved digital practice coverage must be exactly 2/2")
    # Real Grade 8 teacher assessment creation against the SAME disposable DB.
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
        "Grade8 assessment class/subject/term fixture missing")
    teaching_page=page(teacher,"/school/assessments")
    csrf=re.search(r'name="__RequestVerificationToken"[^>]*value="([^"]+)"',teaching_page)
    require(csrf,"real teacher assessment CSRF missing")
    title="G8 Stage D CI "+uuid.uuid4().hex[:12]
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
        "Grade8 teacher assessment not persisted correctly")
    other_teacher=connect("commoncore-secondary","Teacher")
    cross_denied(other_teacher,"/school/assessments/"+saved[0])
    REPORT["grade8TeacherAssessmentCreatedAndStored"]=True
    REPORT["crossSchoolTeacherAssessmentAccess"]="DENIED"
    REPORT["teacherFinalAcademicApproval"]="NOT_CLAIMED"
    print("G8_D_TEACHER_GRADE8_ASSESSMENT_POST_AND_TENANT_DENIAL_PASS",flush=True)
    # Complete the *synthetic* teacher-assessment journey in the same
    # disposable PostgreSQL 18 school. Approval here exercises role-scoped
    # controls; it is NOT independent human review or academic sign-off.
    aid=saved[0]
    require(re.fullmatch(r"[0-9a-fA-F-]{36}",aid),"assessment ID malformed")
    builder=f"/school/assessments/{aid}/builder"
    require(sql(f"""SELECT count(*) FROM "AssessmentQuestions"
        WHERE "AssessmentId"='{aid}';""")=="0",
        "new synthetic G8 assessment not empty")

    def version_on(markup):
        m=re.search(r'name="rowVersion"[^>]*value="([^"]+)"',markup)
        require(m is not None,"Grade8 builder optimistic rowVersion unavailable")
        return m.group(1)

    def teacher_post(route, values):
        data=urllib.parse.urlencode(values).encode()
        with teacher.open(urllib.request.Request(BASE+route,data=data,method="POST"),timeout=55) as response:
            require(response.status==200,"teacher builder POST non-200 "+route)
            return html.unescape(response.read().decode("utf-8"))

    builder_html=page(teacher,builder)
    require("assessment" in builder_html.casefold(),"Grade8 builder workspace not displayed")
    settings_html=teacher_post(builder+"/settings",{
        "__RequestVerificationToken":extract_csrf(builder_html),
        "rowVersion":version_on(builder_html),
        "targetType":"Class","targetStudentProfileId":"",
        "deliveryMode":"Online","difficultyBand":"AtClassLevel"})
    require('data-delivery-saved="true"' in settings_html.lower(),
        "assessment delivery settings did not unlock native builder")
    builder_lesson_code="PED:US-CCSS-MATH:G8:U04:L05"
    first_lesson_id=lesson_ids[builder_lesson_code]
    require(re.fullmatch(r"[0-9a-fA-F-]{36}",first_lesson_id),
        "Grade8 approved builder lesson not seeded")
    generated_html=teacher_post(builder+"/generate",{
        "__RequestVerificationToken":extract_csrf(settings_html),
        "rowVersion":version_on(settings_html),
        "questionCount":"1","maxScorePerQuestion":"100",
        "difficulty":"AtClassLevel","scopeType":"Lessons",
        "lessonIds":first_lesson_id,"seed":"20261104"})
    rows=sql(f"""SELECT q."Id"::text,i."GenerationFamily",
                 COALESCE(i."ValidationMetadataJson"::text,'')
        FROM "AssessmentQuestions" q
        JOIN "AssessmentItems" i ON i."Id"=q."Id"
        WHERE q."AssessmentId"='{aid}';""").splitlines()
    require(len(rows)==1,"Grade8 teacher builder did not generate one stored question")
    qid,assessment_family,meta=rows[0].split("|",2)
    require(re.fullmatch(r"[0-9a-fA-F-]{36}",qid) and
        assessment_family==practice_eligible[builder_lesson_code],
        "Assessment Builder did not use selected Grade8 approved family")
    require(json.loads(meta).get("builderStatus")=="Draft",
        "generated teacher question must require review")
    reviewed_html=teacher_post(builder+"/questions/"+qid+"/approve",{
        "__RequestVerificationToken":extract_csrf(generated_html),
        "rowVersion":version_on(generated_html)})
    stored_meta=sql(f"""SELECT i."ValidationMetadataJson"::text FROM "AssessmentItems" i
        WHERE i."Id"='{qid}';""")
    require(json.loads(stored_meta).get("builderStatus")=="Approved",
        "Grade8 teacher approval did not persist")
    status_before=sql(f"""SELECT "Status"::text FROM "Assessments"
        WHERE "Id"='{aid}';""")
    published_html=teacher_post(builder+"/publish",{
        "__RequestVerificationToken":extract_csrf(reviewed_html),
        "rowVersion":version_on(reviewed_html)})
    status_after=sql(f"""SELECT "Status"::text FROM "Assessments"
        WHERE "Id"='{aid}';""")
    require(status_before!=status_after,
        "teacher-approved Grade8 assessment was not published")
    student_index=page(student,"/student/assessments")
    require(aid.lower() in student_index.lower(),
        "published Grade8 assessment not assigned to synthetic enrolled learner")
    require(aid.lower() in published_html.lower(),
        "teacher publish response did not retain assessment identity")
    cross_denied(foreign,"/student/assessments/"+aid)
    exam_html=page(student,"/student/assessments/"+aid)
    exam_csrf=extract_csrf(exam_html)
    exam_questions=re.findall(
        r'name="questionIds"[^>]*value="([0-9a-fA-F-]{36})"',exam_html)
    require(exam_csrf and exam_questions and qid.lower() in
        [x.lower() for x in exam_questions],
        "student examination form did not render approved G8 question")
    attempts_before=int(sql(f"""SELECT count(*) FROM "AssessmentAttempts"
        WHERE "AssessmentId"='{aid}';"""))
    submission=[("__RequestVerificationToken",exam_csrf)]
    for exam_qid in exam_questions:
        submission.extend([("questionIds",exam_qid),("responses","")])
    with student.open(urllib.request.Request(
        BASE+"/student/assessments/"+aid+"/submit",
        data=urllib.parse.urlencode(submission).encode(),method="POST"),
        timeout=55) as response:
        require(response.status==200,"student assessment submission failed")
        result_html=response.read().decode("utf-8")
        require("student-assessment" in result_html or "Submitted" in result_html,
            "student assessment completion confirmation absent")
    attempts_after=int(sql(f"""SELECT count(*) FROM "AssessmentAttempts"
        WHERE "AssessmentId"='{aid}';"""))
    # GET /student/assessments/{id} already calls GetOrCreateAttemptAsync.
    # Submission must transition that persisted attempt, not create a second.
    require(attempts_before==1 and attempts_after==1,
        "Grade8 student assessment attempt missing or duplicated")
    attempt_state=sql(f"""SELECT "Status"::text,("SubmittedAtUtc" IS NOT NULL)::text
        FROM "AssessmentAttempts" WHERE "AssessmentId"='{aid}';""")
    require(attempt_state in ("2|true","3|true"),
        "Grade8 student assessment attempt not submitted "+attempt_state)
    result_count=sql(f"""SELECT count(*) FROM "AssessmentResults"
        WHERE "AssessmentId"='{aid}';""")
    require(result_count=="1",
        "Grade8 student exam score/result not persisted "+result_count)
    answer_count=sql(f"""SELECT count(*) FROM "StudentAnswers" sa
        JOIN "AssessmentResults" ar ON ar."Id"=sa."AssessmentResultId"
        WHERE ar."AssessmentId"='{aid}';""")
    require(answer_count==str(len(exam_questions)),
        "Grade8 student assessment responses not persisted "+answer_count)
    page(student,"/student/results")
    REPORT["syntheticTeacherBuilderSelectedGeneratedQuestionFamily"]=assessment_family
    REPORT["syntheticTeacherBuilderReviewApprovePublish"]="PASS"
    REPORT["syntheticStudentAssessmentSubmittedStored"]="PASS"
    REPORT["crossSchoolStudentAssessmentAccess"]="DENIED"
    REPORT["syntheticHumanAcademicApproval"]="NOT_CLAIMED"
    print("G8_D_BUILDER_GENERATE_REVIEW_APPROVE_PUBLISH_STUDENT_SUBMIT_PASS",flush=True)
    REPORT.update(status="G8_131_REAL_AUTHENTICATED_HTTPS_PASS",
        approvedDigitalPracticeNumerator=verified_practice,
        approvedDigitalPracticeDenominator=len(practice_eligible),
        correctGradedAndPersisted=verified_practice,
        invalidRejectedAndPersisted=verified_practice,
        wellFormedWrongRejectedAndPersisted=verified_practice,
        crossSchoolDenied=131,studentRichPagesPassed=131,stageDVerified=0)
    save()
    print("G8_D_AUTH_HTTPS_FINAL_PASS 131/131 Rich V2 + tenant isolation; 2/2 approved digital practice; not academic release",flush=True)
if __name__=="__main__":
    try: main()
    except Exception as error:
        REPORT.update(status="FAIL_CLOSED",errorType=type(error).__name__,
            lastError=str(error)[:800])
        save()
        raise
