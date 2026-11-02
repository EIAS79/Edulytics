#!/usr/bin/env python3
"""Exact Grade 2 final 44 source-companion HTTPS/tenant smoke check.

Run ONLY after the existing 500-lesson acceptance script in the same
disposable CI PostgreSQL web fixture. This is no educator signoff.
"""
import html
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

def sql(query):
    return subprocess.check_output(
        ["psql", "-h", "127.0.0.1", "-U", "postgres", "-d",
         "edulytics_clean_ci", "-A", "-t", "-F", "|", "-c", query],
        text=True).strip()

def connect(school, suffix):
    user = f"{school}.student.{suffix}@edulytiks.com"
    jar = http.cookiejar.CookieJar()
    opener = urllib.request.build_opener(
        urllib.request.HTTPCookieProcessor(jar),
        urllib.request.HTTPSHandler(context=CTX))
    with opener.open(BASE + "/account/login?culture=en", timeout=20) as res:
        initial = res.read().decode("utf-8")
    match = re.search(
        r'name="__RequestVerificationToken"[^>]*value="([^"]+)"', initial)
    assert match, (user, "login anti-forgery token missing")
    form = urllib.parse.urlencode({
        "Email": user,
        "Password": PASSWORD,
        "AccountType": "Student",
        "__RequestVerificationToken": match.group(1),
    }).encode()
    with opener.open(urllib.request.Request(
        BASE + "/account/login?culture=en", data=form, method="POST",
    ), timeout=20) as res:
        assert res.status == 200
        assert urllib.parse.urlsplit(res.url).path.lower() != "/account/login"
    assert any(cookie.secure for cookie in jar), (user, "secure login cookie missing")
    return opener

def page(opener, path):
    assert path.startswith("/") and not path.startswith("//"), path
    with opener.open(BASE + path, timeout=30) as res:
        assert res.status == 200, (path, res.status)
        assert urllib.parse.urlsplit(res.url).path.lower() != "/account/login"
        return html.unescape(res.read().decode("utf-8"))

def deny_cross_school(opener, path):
    try:
        with opener.open(BASE + path, timeout=30) as response:
            # A 200 at the protected exact route is a data leak.
            redirect = urllib.parse.urlsplit(response.url).path.lower()
            assert redirect != urllib.parse.urlsplit(path).path.lower(), (
                "cross-school source companion exposed", path)
    except urllib.error.HTTPError as error:
        assert error.code in (401, 403, 404), (path, error.code)

assert os.environ.get("EDULYTICS_CI_REHEARSAL_PROGRAMME") == "US-CCSS", (
    "This exact-run probe must not target live schools")
assert sql("""SELECT count(*) FROM "Schools"
WHERE "SchoolCode" LIKE 'REHEARSAL-%';""") == "2"

# Prior B001 rehearsal has advanced the one disposable primary-school
# test learner to Grade 4. Re-enroll only that synthetic learner in the
# EXISTING Grade 2 primary school class; never change a real school.
classes = sql("""
SELECT c."Id"::text FROM "ClassGroups" c
JOIN "Schools" s ON s."Id" = c."SchoolId"
WHERE s."SchoolCode"='REHEARSAL-US-PRIMARY'
AND c."Code" LIKE 'L03-%';""").splitlines()
assert len(classes) == 1, ("Expected exactly one primary Grade 2 class", classes)
updated = sql("""
WITH update_fixture AS (
 UPDATE "StudentEnrollments" e
 SET "ClassGroupId"='""" + classes[0] + """'
 FROM "StudentProfiles" p
 JOIN "AspNetUsers" u ON u."Id"=p."UserId"
 WHERE e."StudentProfileId"=p."Id"
 AND u."Email"='commoncore-primary.student.secondary@edulytiks.com'
 RETURNING e."Id"
) SELECT count(*) FROM update_fixture;""")
assert updated == "1", ("Must re-enroll exactly one disposable learner", updated)

student = connect("commoncore-primary", "secondary")
cross_school_student = connect("commoncore-secondary", "primary")
manifest = json.loads(Path(
    "artifacts/curriculum-delivery/US-CCSS-MATH/G2/grade-manifest.json"
).read_text(encoding="utf-8"))
source = [row for row in manifest["lessons"] if row["unitNumber"] in (7, 8, 9)]
assert len(source) == 44 and [
    sum(row["unitNumber"] == n for row in source) for n in (7, 8, 9)
] == [18, 13, 13]
evidence = []
for row in source:
    unit, lesson, code = (
        row["unitNumber"], row["lessonNumber"], row["lessonCode"])
    ids = sql("""SELECT "Id"::text FROM "CurriculumPedagogicalLessons"
WHERE "Code"='""" + code + """';""").splitlines()
    assert len(ids) == 1 and re.fullmatch(r"[0-9a-f-]{36}", ids[0]), code
    lesson_id = ids[0]
    student_html = page(student, "/student/learning/lesson/" + lesson_id)
    target = f"/student/practice/grade2-u{unit:02d}/l{lesson}"
    matches = [
        href for href in re.findall(r'href="([^"]+)"', student_html)
        if urllib.parse.urlsplit(href).path.casefold() == target.casefold()
    ]
    assert len(matches) == 1, (code, "missing/duplicate student companion link")
    href = matches[0]
    assert href.startswith("/") and not href.startswith("//"), code
    query = urllib.parse.parse_qs(urllib.parse.urlsplit(href).query)
    assert query.get("lessonId") == [lesson_id], (code, "wrong lesson binding")
    adoption = query.get("curriculumAdoptionId", [])
    assert len(adoption) == 1 and re.fullmatch(
        r"[0-9a-fA-F-]{36}", adoption[0]), (code, "adoption scope missing")
    companion = page(student, href)
    assert "Publisher source-specific original task" in companion, code
    assert "Edulytics first-party independent numerical examples" in companion, code
    assert 'name="__RequestVerificationToken"' in companion, code
    assert 'name="curriculumAdoptionId"' in companion, code
    assert 'name="lessonId"' in companion, code
    deny_cross_school(cross_school_student, href)
    evidence.append({
        "ordinal": row["gradeOrdinal"], "lessonCode": code,
        "authenticatedStudentCompanionPage": "PASSED",
        "studentBoundToExpectedLessonAndAdoption": "PASSED",
        "csrfTokenPresent": True,
        "crossSchoolAccess": "DENIED",
        "originalTaskTeacherApproval": "NOT_CLAIMED",
    })
    print("G2_LATE_SOURCE_COMPANION_HTTPS_PASS",
          row["gradeOrdinal"], code, flush=True)

assert len(evidence) == 44 and len({row["lessonCode"] for row in evidence}) == 44
out = {
    "programme": "US-CCSS-MATH", "grade": "G2", "checked": 44,
    "authenticatedStudentPagesPassed": 44, "crossSchoolDenied": 44,
    "teacherAcademicApproval": "NOT_EVALUATED",
    "gradeRelease": "NO_GO_UNVERIFIED", "lessons": evidence,
}
Path("/tmp/g2-44-source-companions-authenticated-https-no-academic-approval.json"
     ).write_text(json.dumps(out, indent=2) + "\n", encoding="utf-8")
print("G2_LATE_SOURCE_COMPANION_HTTPS_COMPLETE", len(evidence), flush=True)
