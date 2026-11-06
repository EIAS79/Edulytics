#!/usr/bin/env python3
"""One authorized, disposable Grade 10 teacher review persistence smoke test.

Runs only on GitHub's temporary PostgreSQL + localhost HTTPS fixture.
Does not grade real student work or operate production systems.
"""
import html
import http.cookiejar
import json
import os
import re
import ssl
import subprocess
import urllib.error
import urllib.parse
import urllib.request

assert os.environ.get("GITHUB_ACTIONS") == "true"
assert os.environ.get("EDULYTICS_CI_REHEARSAL") == "true"
assert os.environ.get("EDULYTICS_CI_G10_GEOMETRY") == "true"
ROOT = "https://localhost:5052"
CTX = ssl.create_default_context(cafile="/tmp/phase-uat-cert.pem")
PASSWORD = os.environ["Edulytics__MeetingDemo__Password"]
CODE = "PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U01:L01"

def sql(query):
    return subprocess.check_output(["psql", "-h", "127.0.0.1", "-U", "postgres", "-d",
        "edulytics_clean_ci", "-A", "-t", "-c", query], text=True).strip()

def token(page):
    hit = re.search(r'name="__RequestVerificationToken"[^>]*value="([^"]+)"', page)
    assert hit, "missing CSRF token"
    return hit.group(1)

def login(email, role):
    cookies = http.cookiejar.CookieJar()
    opener = urllib.request.build_opener(urllib.request.HTTPCookieProcessor(cookies),
        urllib.request.HTTPSHandler(context=CTX))
    with opener.open(ROOT + "/account/login?culture=en", timeout=30) as login_page:
        assert login_page.status == 200
        entry = login_page.read().decode()
    body = urllib.parse.urlencode({"Email":email, "Password":PASSWORD,
        "AccountType":role, "__RequestVerificationToken":token(entry)}).encode()
    with opener.open(urllib.request.Request(ROOT + "/account/login?culture=en",
            data=body, method="POST"), timeout=30) as result:
        assert result.status == 200 and "/account/login" not in result.url
    assert any(cookie.secure for cookie in cookies)
    return opener

def view(opener, endpoint):
    with opener.open(ROOT + endpoint, timeout=35) as result:
        assert result.status == 200
        assert not urllib.parse.urlparse(result.url).path.lower().startswith("/account/login")
        return html.unescape(result.read().decode())

def submit(opener, endpoint, parameters):
    body = urllib.parse.urlencode(parameters).encode()
    with opener.open(urllib.request.Request(ROOT + endpoint, data=body,
            method="POST"), timeout=35) as result:
        assert result.status == 200
        return html.unescape(result.read().decode())

lesson_id = sql('SELECT "Id"::text FROM "CurriculumPedagogicalLessons" WHERE "Code"=\'' + CODE + '\';')
assert re.fullmatch(r'[0-9a-f-]{36}', lesson_id)
student = login("commoncore-secondary.student.primary@edulytiks.com", "Student")
teacher = login("commoncore-secondary.teacher.secondary@edulytiks.com", "Teacher")
other = login("commoncore-primary.teacher.primary@edulytiks.com", "Teacher")
page = view(student, "/student/g10/geometry/rubric?lessonId=" + lesson_id)
assert "Submit for teacher review" in page
response = submit(student, "/student/g10/geometry/rubric/submit", {
    "lessonId":lesson_id,
    "explanation":"Synthetic CI: construct equal segments with a fixed compass opening and justify the equality; a human must inspect the actual drawing.",
    "__RequestVerificationToken":token(page)
})
assert "Teacher review pending" in response
queue = view(teacher, "/school/g10/geometry/rubric")
ids = re.findall(r'/school/g10/geometry/rubric/([0-9a-fA-F-]{36})', queue)
assert len(ids) == 1, ("expected one synthetic queued response", len(ids))
review_id = ids[0]
try:
    view(other, "/school/g10/geometry/rubric/" + review_id)
    raise AssertionError("foreign-school teacher could access private submission")
except urllib.error.HTTPError as error:
    assert error.code in (401, 403, 404), error.code
review = view(teacher, "/school/g10/geometry/rubric/" + review_id)
assert "Save teacher assessment" in review
score = submit(teacher, "/school/g10/geometry/rubric/" + review_id,
    [("marks","1"),("marks","1"),("marks","0"),("marks","1"),
     ("teacherFeedback","Synthetic teacher verification of manual grading persistence; not real academic evaluation."),
     ("__RequestVerificationToken", token(review))])
assert "Teacher-awarded score: 3 / 4" in score
assert "Teacher score: 3 / 4" in view(student, "/student/g10/geometry/rubric?lessonId=" + lesson_id)
assert sql('SELECT count(*) FROM "Grade10GeometryTeacherRubrics" WHERE "TeacherScore"=3 AND "ReviewedAtUtc" IS NOT NULL;') == "1"
assert sql('SELECT count(*) FROM "Grade10GeometryTeacherRubrics";') == "1"
report = {"course":"G10 Geometry","lessonCode":CODE,"studentSubmitted":True,
          "teacherAwardedScore":3,"teacherScorePersisted":True,
          "studentReadBack":True,"wrongSchoolTeacherDenied":True,
          "academicCorrectnessVerified":False,"all124Verified":False}
with open("/tmp/g10-one-teacher-review-https.json", "w", encoding="utf8") as out:
    json.dump(report, out, indent=2)
print("G10_ONE_SYNTHETIC_TEACHER_REVIEW_PERSISTED_WITH_CROSS_SCHOOL_DENIAL", flush=True)
