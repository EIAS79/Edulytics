#!/usr/bin/env python3
"""Grade 5 Stage D real authenticated learner source + Practice persistence.

Disposable GitHub Actions PostgreSQL only, two representative G5 lessons.
This is technical E2E evidence, not 148/148 academic source parity or
persisted open-response teacher rubric scoring.
"""
import hashlib
import html
import http.cookiejar
import json
import os
from pathlib import Path
import re
import ssl
import subprocess
import urllib.parse
import urllib.request

from b001_practice_attempt_probe import (
    extract_adoption, extract_csrf, extract_idempotency_key, probe_lesson
)

BASE = "https://localhost:5052"
SCHOOL = "REHEARSAL-US-PRIMARY"
STUDENT = "commoncore-primary.student.secondary@edulytiks.com"
UUID = r"[0-9a-fA-F-]{36}"
CA = "/tmp/phase-uat-cert.pem"
TARGETS = {
    "PED:US-CCSS-MATH:G5:U01:L01":
        ("What Is Volume?", ("three-dimensional", "cubes")),
    "PED:US-CCSS-MATH:G5:U03:L02":
        ("Represent Unit Fraction Multiplication", ("1/15", "1/10")),
}


def sql(query):
    return subprocess.check_output(
        ["psql", "-h", "127.0.0.1", "-U", "postgres",
         "-d", "edulytics_clean_ci", "-A", "-t", "-F", "|",
         "-c", query], text=True).strip()


def get_page(opener, path):
    with opener.open(BASE + path, timeout=45) as response:
        if response.status != 200 or urllib.parse.urlparse(response.url).path == "/account/login":
            raise AssertionError("G5 authenticated page unavailable: " + path)
        return response.read().decode("utf-8")


def login_student():
    opener = urllib.request.build_opener(
        urllib.request.HTTPCookieProcessor(http.cookiejar.CookieJar()),
        urllib.request.HTTPSHandler(context=ssl.create_default_context(cafile=CA)))
    # The login form is deliberately anonymous; only later learner pages
    # must reject an unauthenticated redirect back to /account/login.
    with opener.open(BASE + "/account/login?culture=en", timeout=30) as response:
        if response.status != 200:
            raise AssertionError("G5 disposable login form unavailable")
        page = response.read().decode("utf-8")
    token = extract_csrf(page)
    if not token:
        raise AssertionError("G5 CI login CSRF missing")
    password = os.environ["Edulytics__MeetingDemo__Password"]
    data = urllib.parse.urlencode({
        "Email": STUDENT, "Password": password,
        "AccountType": "Student",
        "__RequestVerificationToken": token}).encode()
    with opener.open(urllib.request.Request(
            BASE + "/account/login?culture=en", data=data,
            method="POST"), timeout=45) as response:
        if (response.status != 200 or
                urllib.parse.urlparse(response.url).path.lower() == "/account/login"):
            raise AssertionError("G5 student login failed")
    return opener


def only_one_row(query, label):
    rows = sql(query).splitlines()
    if len(rows) != 1 or not rows[0]:
        raise AssertionError(f"G5 {label}: expected one row; got {len(rows)}")
    return rows[0]


def run():
    if os.environ.get("GITHUB_ACTIONS") != "true" or (
        os.environ.get("EDULYTICS_CLEAN_DATABASE_NAME") != "edulytics_clean_ci"
    ) or os.environ.get("ASPNETCORE_ENVIRONMENT") != "Staging":
        raise RuntimeError("G5 HTTPS probe is allowed only on disposable Actions staging")
    student_row = only_one_row("""
        SELECT e."ClassGroupId"::text
        FROM "StudentEnrollments" e
        JOIN "StudentProfiles" p ON p."Id"=e."StudentProfileId"
        JOIN "AspNetUsers" u ON u."Id"=p."UserId"
        WHERE u."Email"='""" + STUDENT + """';""", "student enrollment")
    grade5 = only_one_row("""
        SELECT c."Id"::text||'|'||c."CurriculumAdoptionId"::text
        FROM "ClassGroups" c
        JOIN "Schools" s ON s."Id"=c."SchoolId"
        WHERE s."SchoolCode"='""" + SCHOOL + """'
          AND c."Code" LIKE 'L06-%';""", "genuine Grade5 school class")
    class_id, adoption_id = grade5.split("|")
    if not re.fullmatch(UUID, class_id) or not re.fullmatch(UUID, adoption_id):
        raise AssertionError("G5 class/adoption GUID malformed")
    if not re.fullmatch(UUID, student_row):
        raise AssertionError("G5 original student class GUID malformed")
    results = []
    original_restored = False
    try:
        changed = sql("""
          WITH changed AS (
            UPDATE "StudentEnrollments" e
            SET "ClassGroupId"='""" + class_id + """'
            FROM "StudentProfiles" p
            JOIN "AspNetUsers" u ON u."Id"=p."UserId"
            WHERE e."StudentProfileId"=p."Id"
              AND u."Email"='""" + STUDENT + """'
            RETURNING e."Id"
          ) SELECT count(*) FROM changed;""")
        if changed != "1":
            raise AssertionError("G5 disposable reassignment must affect one enrollment")
        student = login_student()
        for code, (title, phrases) in TARGETS.items():
            lesson_id = only_one_row("""
                SELECT "Id"::text FROM "CurriculumPedagogicalLessons"
                WHERE "Code"='""" + code + """';""", "lesson " + code)
            if not re.fullmatch(UUID, lesson_id):
                raise AssertionError("G5 lesson GUID malformed")
            page = html.unescape(get_page(student, "/student/learning/lesson/" + lesson_id))
            for required in (title, "rich-lesson-section", *phrases):
                if required.casefold() not in page.casefold():
                    raise AssertionError(f"G5 {code}: missing actual authenticated content: {required}")
            form = get_page(student, "/student/practice")
            csrf = extract_csrf(form)
            adoption = extract_adoption(form)
            nonce = extract_idempotency_key(form)
            if adoption is None or adoption.lower() != adoption_id.lower():
                raise AssertionError(f"G5 {code}: Practice form has wrong grade's adoption")
            positive = probe_lesson(student, BASE, lesson_id, adoption,
                                    csrf, nonce, sql)
            if positive.get("status") != "SELF_CONSISTENT_GRADE_PERSISTED":
                raise AssertionError(f"G5 {code}: real correct answer failed: {positive}")
            if code.endswith("U03:L02") and positive.get("family") not in {
                "usccss.g5.u03.l02.unit_fraction_of_unit_fraction",
                "usccss.g5.u03.l02.same_original_whole_order_comparison"
            }:
                raise AssertionError(f"G5 U03:L02 selected family mismatch: {positive}")
            wrong_form = get_page(student, "/student/practice")
            negative = probe_lesson(student, BASE, lesson_id,
                extract_adoption(wrong_form), extract_csrf(wrong_form),
                extract_idempotency_key(wrong_form), sql, answer_mode="malformed")
            if negative.get("status") != "MALFORMED_ANSWER_REJECTED_AND_PERSISTED":
                raise AssertionError(f"G5 {code}: bad answer was not rejected: {negative}")
            results.append({
                "lessonCode": code,
                "lessonId": lesson_id,
                "studentGrade": 5,
                "learnerHtmlSha256": hashlib.sha256(page.encode("utf-8")).hexdigest(),
                "authenticatedLearnerContentRendered": True,
                "correctAnswerPersisted": positive["status"],
                "malformedAnswerRejectedAndPersisted": negative["status"],
                "selectedGenerationFamily": positive["family"],
                "sourceFigurePixelsCopiedOrVerified": False,
                "humanTeacherRubricPersistenceVerified": False,
                "academicSourceParityVerified": False})
            print("G5_DISPOSABLE_HTTPS_LESSON_PASS", code,
                  positive["family"], flush=True)
    finally:
        restored = sql("""
          WITH restored AS (
            UPDATE "StudentEnrollments" e
            SET "ClassGroupId"='""" + student_row + """'
            FROM "StudentProfiles" p
            JOIN "AspNetUsers" u ON u."Id"=p."UserId"
            WHERE e."StudentProfileId"=p."Id"
              AND u."Email"='""" + STUDENT + """'
            RETURNING e."Id"
          ) SELECT count(*) FROM restored;""")
        original_restored = restored == "1"
        if not original_restored:
            raise AssertionError("G5 fixture enrollment restoration failed")
    if len(results) != len(TARGETS) or not original_restored:
        raise AssertionError("G5 technical E2E count/integrity mismatch")
    output = {
        "scope": "GRADE5_TWO_EXPLICIT_LESSONS_NOT_ALL_148",
        "technicalAcceptanceOnly": True,
        "sourceEdition": "IM K5 2021 first-edition RichV2",
        "runGitSha": subprocess.check_output(
            ["git", "rev-parse", "HEAD"], text=True).strip(),
        "testDatabase": "edulytics_clean_ci",
        "originalEnrollmentRestored": original_restored,
        "lessonCount": len(results),
        "independentAcademicSourceSignoff": False,
        "teacherRubricPersistenceApproved": False,
        "rows": results}
    Path("/tmp/g5-stage-d-authenticated-two-lessons.json").write_text(
        json.dumps(output, indent=2) + "\n", encoding="utf-8")
    print("G5_REAL_AUTHENTICATED_HTTPS_TECHNICAL_PASS 2/2", flush=True)


if __name__ == "__main__":
    run()
