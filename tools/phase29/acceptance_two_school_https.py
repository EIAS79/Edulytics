#!/usr/bin/env python3
"""Real HTTPS two-tenant smoke test. Run only against disposable CI PostgreSQL."""
import http.cookiejar
import os
import re
import ssl
import subprocess
import urllib.error
import urllib.parse
import urllib.request

BASE = "https://localhost:5052"
CTX = ssl.create_default_context(cafile="/tmp/phase-uat-cert.pem")
PASSWORD = os.environ["Edulytics__MeetingDemo__Password"]
SCHOOLS = [
    ("cambridge-primary", "Horizon British Primary School"),
    ("cambridge-middle", "Horizon British Middle School"),
]


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
        fields.extend((("questionIds", question_id), ("responses", "1")))
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

    teachers[key] = teacher
    print("DISPOSABLE_ACADEMIC_ROLE_WORKFLOWS_PASS", key)


for source, _ in SCHOOLS:
    target = next(key for key, _ in SCHOOLS if key != source)
    query = ("""SELECT a."Id" FROM "Assessments" a JOIN "Schools" s
        ON s."Id"=a."SchoolId" WHERE s."SchoolCode"='REHEARSAL-"""
        + ("GB-PRIMARY" if target == "cambridge-primary" else "GB-MIDDLE")
        + """' ORDER BY a."Id" LIMIT 1;""")
    foreign_id = sql(query)
    assert re.fullmatch(r"[0-9a-fA-F-]{36}", foreign_id), (target, foreign_id)
    deny(teachers[source], "/school/assessments/" + foreign_id)
    print("DISPOSABLE_CROSS_TENANT_ASSESSMENT_DENIAL_PASS", source, target)

# POST real teacher assessment and validate persistence on disposable DB.
import uuid
teacher = teachers["cambridge-primary"]
values = sql("""SELECT a."ClassGroupId",a."SubjectId",a."TermId"
FROM "Assessments" a JOIN "Schools" s ON a."SchoolId"=s."Id"
WHERE s."SchoolCode"='REHEARSAL-GB-PRIMARY' LIMIT 1;""").split("|")
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
