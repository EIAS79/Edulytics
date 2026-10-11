#!/usr/bin/env python3
"""Real HTTPS two-tenant smoke test. Run only against disposable CI PostgreSQL."""
import http.cookiejar
import json
import os
from pathlib import Path
import re
import ssl
import subprocess
import urllib.error
import urllib.parse
import urllib.request

BASE = "https://localhost:5052"
CTX = ssl.create_default_context(cafile="/tmp/phase-uat-cert.pem")
PASSWORD = os.environ["Edulytics__MeetingDemo__Password"]
PROGRAMME = os.environ.get("EDULYTICS_CI_REHEARSAL_PROGRAMME", "CAMBRIDGE")
if PROGRAMME == "US-CCSS":
    SCHOOLS = [
        ("commoncore-primary", "Liberty American Primary School"),
        ("commoncore-secondary", "Liberty American Secondary School"),
    ]
    SCHOOL_CODES = {
        "commoncore-primary": "REHEARSAL-US-PRIMARY",
        "commoncore-secondary": "REHEARSAL-US-SECONDARY",
    }
elif PROGRAMME == "CAMBRIDGE":
    SCHOOLS = [
        ("cambridge-primary", "Horizon British Primary School"),
        ("cambridge-middle", "Horizon British Middle School"),
    ]
    SCHOOL_CODES = {
        "cambridge-primary": "REHEARSAL-GB-PRIMARY",
        "cambridge-middle": "REHEARSAL-GB-MIDDLE",
    }
else:
    raise RuntimeError("Unsupported disposable rehearsal programme")


def sql(query):
    return subprocess.check_output(
        ["psql", "-h", "127.0.0.1", "-U", "postgres", "-d",
         "edulytics_clean_ci", "-A", "-t", "-F", "|", "-c", query],
        text=True,
    ).strip()


rows = tuple(map(int, sql("""SELECT
    (SELECT count(*) FROM "Schools"),
    (SELECT count(*) FROM "AspNetUsers"),
    (SELECT count(*) FROM "StudentProfiles"),
    (SELECT count(*) FROM "Assessments"),
    (SELECT count(*) FROM "AssessmentResults"),
    (SELECT count(*) FROM "PracticeAttempts"),
    (SELECT count(*) FROM "SchoolAnalyticsSnapshots");""").split("|")))
print("DISPOSABLE_ACADEMIC_REHEARSAL_COUNTS", rows)
assert rows[:2] == (2, 12), rows
assert all(count > 0 for count in rows[2:]), rows
assert sql("""SELECT count(*) FROM "Schools" WHERE "SchoolCode" LIKE 'REHEARSAL-%';""") == "2"


def connect(key, role):
    jar = http.cookiejar.CookieJar()
    opener = urllib.request.build_opener(
        urllib.request.HTTPCookieProcessor(jar),
        urllib.request.HTTPSHandler(context=CTX),
    )
    with opener.open(BASE + "/account/login?culture=en", timeout=20) as res:
        html = res.read().decode("utf-8")
    match = re.search(r'name="__RequestVerificationToken"[^>]*value="([^"]+)"', html)
    assert match, (key, role, "missing CSRF")
    if role == "SchoolAdmin":
        user = f"{key}.admin@edulytiks.com"
    elif role == "SubjectSupervisor":
        user = f"{key}.supervisor@edulytiks.com"
    elif role == "Teacher":
        user = f"{key}.teacher.primary@edulytiks.com"
    else:
        user = f"{key}.student.primary@edulytiks.com"
    payload = urllib.parse.urlencode({
        "Email": user, "Password": PASSWORD, "AccountType": role,
        "__RequestVerificationToken": match.group(1),
    }).encode()
    with opener.open(urllib.request.Request(
        BASE + "/account/login?culture=en", data=payload, method="POST",
    ), timeout=20) as res:
        final = urllib.parse.urlparse(res.url).path.lower()
        assert res.status == 200 and final != "/account/login", (user, final)
    assert any(c.secure for c in jar), (role, "missing secure auth cookie")
    return opener


def get_page(opener, path):
    with opener.open(BASE + path, timeout=30) as res:
        assert res.status == 200, (path, res.status)
        assert urllib.parse.urlparse(res.url).path.lower() != "/account/login"
        return res.read().decode("utf-8")


def deny(opener, path):
    try:
        with opener.open(BASE + path, timeout=30) as res:
            final = urllib.parse.urlparse(res.url).path.lower()
            assert final != path.lower(), ("cross-tenant resource accessible", path)
    except urllib.error.HTTPError as exc:
        assert exc.code in (401, 403, 404), (path, exc.code)


teachers = {}
students = {}
practice_attempts = {}
for key, schoolname in SCHOOLS:
    admin = connect(key, "SchoolAdmin")
    supervisor = connect(key, "SubjectSupervisor")
    teacher = connect(key, "Teacher")
    student = connect(key, "Student")
    for opener in (admin, supervisor, teacher):
        dash = get_page(opener, "/school/dashboard")
        assert schoolname in dash, (key, "wrong school context")
        other = next(name for k, name in SCHOOLS if k != key)
        assert other not in dash, (key, "foreign school context visible")
    get_page(admin, "/school/reports")
    get_page(supervisor, "/school/reports")
    get_page(teacher, "/school/assessments")
    get_page(student, "/student/dashboard")
    get_page(student, "/student/learning")
    get_page(student, "/student/practice")
    deny(student, "/school/assessments")
    deny(teacher, "/student/dashboard")
    # Complete a real student assessment: GET form, CSRF-protected POST,
    # and verify a new submitted assessment attempt in this disposable DB.
    student_index = get_page(student, "/student/assessments")
    open_link = re.search(
        r'href="([^"]*/student/assessments/([0-9a-fA-F-]{36}))"',
        student_index, re.IGNORECASE)
    assert open_link, (key, "no available assessment")
    assessment_id = open_link.group(2)
    before_attempts = int(sql('SELECT count(*) FROM "AssessmentAttempts";'))
    attempt_html = get_page(student, "/student/assessments/" + assessment_id)
    attempt_csrf = re.search(
        r'name="__RequestVerificationToken"[^>]*value="([^"]+)"',
        attempt_html)
    question_ids = re.findall(
        r'name="questionIds"[^>]*value="([0-9a-fA-F-]{36})"',
        attempt_html)
    assert attempt_csrf and question_ids, (key, "assessment form incomplete")
    fields = [("__RequestVerificationToken", attempt_csrf.group(1))]
    for question_id in question_ids:
        fields.extend((("questionIds", question_id), ("responses", "")))
    try:
        with student.open(urllib.request.Request(
            BASE + "/student/assessments/" + assessment_id + "/submit",
        data=urllib.parse.urlencode(fields).encode(), method="POST"
        ), timeout=40) as submitted:
            assert submitted.status == 200, (key, "submit POST failed")
            result_html = submitted.read().decode("utf-8")
            assert "student-assessment" in result_html or "Submitted" in result_html, key
    except urllib.error.HTTPError as error:
        print("STUDENT_SUBMIT_FAILURE", key, error.code, error.url)
        raise
    after_attempts = int(sql('SELECT count(*) FROM "AssessmentAttempts";'))
    assert after_attempts > before_attempts, (key, before_attempts, after_attempts)
    get_page(student, "/student/results")
    print("DISPOSABLE_STUDENT_ASSESSMENT_SUBMISSION_PASS", key, assessment_id)

    # Generate actual private Practice through HTTPS; these attempts exist
    # only in the disposable localhost PostgreSQL database.
    practice_html = get_page(student, "/student/practice")
    practice_token = re.search(
        r'name="__RequestVerificationToken"[^>]*value="([^"]+)"',
        practice_html)
    adoption = re.search(
        r'name="curriculumAdoptionId"[^>]*value="([0-9a-fA-F-]{36})"',
        practice_html)
    assert practice_token and adoption, (key, "private Practice form missing")
    before_practice = int(sql('SELECT count(*) FROM "PracticeAttempts";'))
    practice_fields = {
        "curriculumAdoptionId": adoption.group(1),
        "scope": "WholeCurriculum",
        "difficulty": "AtClassLevel",
        "questionCount": "1",
        "__RequestVerificationToken": practice_token.group(1),
    }
    expected_families = None
    chosen_lesson_id = None
    if PROGRAMME == "US-CCSS":
        # Do not mistake "an American student can log in" for "the
        # American curriculum is actually assigned to this student."
        framework = sql("""
            SELECT f."Code" || '|' || v."VersionCode"
            FROM "SchoolCurriculumAdoptions" a
            JOIN "CurriculumFrameworkVersions" v ON v."Id"=a."FrameworkVersionId"
            JOIN "CurriculumFrameworks" f ON f."Id"=v."FrameworkId"
            WHERE a."Id"='""" + adoption.group(1) + """';""")
        assert framework == "US-CCSS-MATH|CCSSM-2010", (key, "wrong curriculum", framework)

        # Select a real lesson displayed to the student and independently
        # prove the lesson has a READY_VERIFIED, exact-family US route.
        offered_ids = re.findall(
            r'<option value="([0-9a-fA-F-]{36})"\s+data-learning-focus-node-ids=',
            practice_html)
        assert offered_ids, (key, "no US student lessons offered")
        mappings_path = Path("src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json")
        mappings = json.loads(mappings_path.read_text(encoding="utf-8"))["mappings"]
        verified = {
            row["lessonCode"]: row["allowedQuestionFamilies"]
            for row in mappings
            if row.get("lessonCode", "").startswith("PED:US-CCSS-MATH:")
            and row.get("practiceReadiness") == "READY_VERIFIED"
            and row.get("allowedQuestionFamilies")
        }
        ids = ",".join("'" + i + "'" for i in offered_ids)
        offered = sql("""
            SELECT l."Id"::text || '|' || l."Code"
            FROM "CurriculumPedagogicalLessons" l
            WHERE l."Id" IN (""" + ids + """)
            ORDER BY l."Code";""").splitlines()
        if key == "commoncore-secondary":
            # The same publisher pathway spans Algebra 1, Geometry and
            # Algebra 2. This accepted fixture explicitly chose ALG1; verify
            # HTTPS did not offer any other high-school source course.
            native_levels = sql("""
                SELECT DISTINCT l."NativeLevel"
                FROM "CurriculumPedagogicalLessons" l
                WHERE l."Id" IN (""" + ids + """)
                ORDER BY l."NativeLevel";""").splitlines()
            assert native_levels == ["Algebra 1"], (
                "secondary practice leaked another source course", native_levels)
            configured = sql("""
                SELECT "CurriculumLevelKey" || '|' ||
                       "CurriculumPathway" || '|' ||
                       "CurriculumLevelLabel"
                FROM "SchoolCurriculumAdoptions"
                WHERE "Id"='""" + adoption.group(1) + """';""")
            assert configured == (
                "US-CCSS-MATH:L11:TRADITIONAL:COURSE-ALG1|Traditional|Algebra 1"
            ), ("secondary adoption lacks explicit course identity", configured)
            print("DISPOSABLE_US_CCSS_HS_ALG1_COURSE_ISOLATION_PASS")
        candidates = []
        for row in offered:
            parts = row.split("|", 1)
            if len(parts) == 2 and parts[1] in verified:
                candidates.append((parts[0], parts[1]))
        assert candidates, (key, "no learner-visible READY_VERIFIED US CCSS lesson")
        chosen_lesson_id, chosen_lesson_code = candidates[0]
        expected_families = verified[chosen_lesson_code]
        practice_fields["scope"] = "Lesson"
        practice_fields["lessonId"] = chosen_lesson_id
        print("DISPOSABLE_US_CCSS_LESSON_ROUTE_READY", key, chosen_lesson_code)
    practice_body = urllib.parse.urlencode(practice_fields).encode()
    with student.open(urllib.request.Request(
        BASE + "/student/practice/generate",
        data=practice_body, method="POST"
    ), timeout=60) as generated:
        generated_path = urllib.parse.urlparse(generated.url).path.lower()
        assert generated.status == 200, (key, generated.status)
        match_attempt = re.fullmatch(
            r"/student/practice/attempt/([0-9a-f-]{36})", generated_path)
        assert match_attempt, (key, "practice generation did not open an attempt", generated_path)
        generated_body = generated.read().decode("utf-8")
        assert "practice" in generated_body.lower(), (key, "no Practice page content")
    after_practice = int(sql('SELECT count(*) FROM "PracticeAttempts";'))
    assert after_practice > before_practice, (key, before_practice, after_practice)

    if PROGRAMME == "US-CCSS":
        # Verify the persisted question's exact family and lesson identity,
        # not merely that the HTTP redirect showed a Practice page.
        item_rows = sql("""
            SELECT a."CurriculumPedagogicalLessonId"::text, i."GenerationFamily",
                   pi."Id"::text, i."CorrectAnswer"
            FROM "PracticeAttempts" a
            JOIN "PracticeAttemptItems" pi ON pi."PracticeAttemptId"=a."Id"
            JOIN "AssessmentItems" i ON i."Id"=pi."AssessmentItemId"
            WHERE a."Id"='""" + match_attempt.group(1) + """'
            ORDER BY pi."Order";""").splitlines()
        assert len(item_rows) == 1, (key, "missing generated exact item")
        actual_lesson, generated_family, item_id, expected_answer = item_rows[0].split("|", 3)
        assert actual_lesson == chosen_lesson_id, (key, "practice lesson identity mismatch")
        assert generated_family in expected_families, (key, "wrong lesson question family", generated_family)
        assert expected_answer.strip(), (key, "empty exact answer")

        # Exercise actual student answer grading and private-attempt persistence
        # on disposable CI, never relying solely on generation success.
        csrf = re.search(
            r'name="__RequestVerificationToken"[^>]*value="([^"]+)"',
            generated_body)
        html_id = re.search(
            r'name="attemptItemIds\[0\]"[^>]*value="([0-9a-fA-F-]{36})"',
            generated_body)
        assert csrf and html_id and html_id.group(1) == item_id, (
            key, "personal test answer form mismatch")
        answer_body = urllib.parse.urlencode({
            "__RequestVerificationToken": csrf.group(1),
            "attemptItemIds[0]": item_id,
            "answers[0]": expected_answer,
        }).encode()
        with student.open(urllib.request.Request(
            BASE + "/student/practice/attempt/" + match_attempt.group(1)
            + "/personal-test-submit",
            data=answer_body, method="POST",
        ), timeout=60) as submitted_practice:
            assert submitted_practice.status == 200, (key, "Practice submission HTTP failure")
        evidence = sql("""
            SELECT CASE WHEN p."SubmittedAtUtc" IS NOT NULL THEN 1 ELSE 0 END,
                   p."Score"::text, COUNT(r."Id")::text
            FROM "PracticeAttempts" p
            JOIN "PracticeAttemptItems" ai ON ai."PracticeAttemptId"=p."Id"
            LEFT JOIN "PracticeResponses" r
                ON r."PracticeAttemptItemId"=ai."Id" AND r."IsCorrect"=true
            WHERE p."Id"='""" + match_attempt.group(1) + """'
            GROUP BY p."Id", p."SubmittedAtUtc", p."Score";""")
        complete, score, correct_responses = evidence.split("|")
        assert complete == "1" and float(score) == 1.0 and correct_responses == "1", (
            key, "exact Practice answer not scored and persisted", evidence)
        print("DISPOSABLE_US_CCSS_EXACT_LESSON_ANSWER_PASS", key, generated_family)

    practice_attempts[key] = match_attempt.group(1)
    students[key] = student
    teachers[key] = teacher
    print("DISPOSABLE_STUDENT_PRIVATE_PRACTICE_PASS", key)
    print("DISPOSABLE_ACADEMIC_ROLE_WORKFLOWS_PASS", key)


for source, _ in SCHOOLS:
    target = next(key for key, _ in SCHOOLS if key != source)
    query = ("""SELECT a."Id" FROM "Assessments" a JOIN "Schools" s
        ON s."Id"=a."SchoolId" WHERE s."SchoolCode"='"""
        + SCHOOL_CODES[target] + """' ORDER BY a."Id" LIMIT 1;""")
    foreign_id = sql(query)
    assert re.fullmatch(r"[0-9a-fA-F-]{36}", foreign_id), (target, foreign_id)
    deny(teachers[source], "/school/assessments/" + foreign_id)
    deny(students[source], "/student/assessments/" + foreign_id)
    deny(students[source], "/student/practice/attempt/" + practice_attempts[target])
    print("DISPOSABLE_CROSS_TENANT_ASSESSMENT_DENIAL_PASS", source, target)
    print("DISPOSABLE_CROSS_TENANT_STUDENT_DENIAL_PASS", source, target)

# POST real teacher assessment and validate persistence on disposable DB.
import uuid
teacher = teachers[SCHOOLS[0][0]]
values = sql("""SELECT a."ClassGroupId",a."SubjectId",a."TermId"
FROM "Assessments" a JOIN "Schools" s ON a."SchoolId"=s."Id"
WHERE s."SchoolCode"='""" + SCHOOL_CODES[SCHOOLS[0][0]] + """' LIMIT 1;""").split("|")
assert len(values) == 3, values
html = get_page(teacher, "/school/assessments")
token = re.search(r'name="__RequestVerificationToken"[^>]*value="([^"]+)"', html)
assert token
title = "CI acceptance " + uuid.uuid4().hex[:12]
data = urllib.parse.urlencode({
    "classGroupId": values[0], "subjectId": values[1],
    "termId": values[2], "title": title,
    "assessmentDate": "2026-10-09", "maxScore": "100",
    "assessmentType": "Exam", "deliveryMode": "Online",
    "__RequestVerificationToken": token.group(1)
}).encode()
with teacher.open(urllib.request.Request(
    BASE+"/school/assessments", data=data, method="POST"
), timeout=30) as response:
    assert response.status == 200
assert sql('SELECT count(*) FROM "Assessments" WHERE "Title"=\''+title+'\';') == "1"
print("DISPOSABLE_TEACHER_ASSESSMENT_POST_PASS")

print("DISPOSABLE_TWO_SCHOOL_FULL_DATA_HTTP_REHEARSAL_PASS")
