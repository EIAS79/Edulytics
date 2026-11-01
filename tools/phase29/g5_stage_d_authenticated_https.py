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



def verify_g5_u03_l02_actual_practice_svg(opener, lesson_id, adoption_id):
    """Generate a new Grade5-only attempt and verify what the learner really sees.

    Uses the ephemeral HTTPS school and real persisted server-owned parameters.
    The extra attempt intentionally remains unsubmitted: drawing/copying is not
    automatically assessed or counted as evidence of teacher rubric persistence.
    """
    source = get_page(opener, "/student/practice")
    token = extract_csrf(source)
    nonce = extract_idempotency_key(source)
    actual_adoption = extract_adoption(source)
    if not token or not nonce or actual_adoption != adoption_id:
        raise AssertionError("G5 visual test: scoped lesson form invalid")
    request_data = urllib.parse.urlencode({
        "curriculumAdoptionId": adoption_id,
        "scope": "Lesson",
        "lessonId": lesson_id,
        "difficulty": "AtClassLevel",
        "questionCount": "1",
        "__RequestVerificationToken": token,
        "_idempotencyKey": nonce
    }).encode()
    with opener.open(urllib.request.Request(
            BASE + "/student/practice/generate",
            data=request_data, method="POST"), timeout=45) as response:
        endpoint = urllib.parse.urlparse(response.url).path.lower()
        match = re.fullmatch(r"/student/practice/attempt/(" + UUID + r")", endpoint)
        if response.status != 200 or not match:
            raise AssertionError("G5 visual test: no authenticated attempt page")
        attempt_id = match.group(1)
        rendered = html.unescape(response.read().decode("utf-8"))
    item = only_one_row("""
        SELECT i."GenerationFamily",
               COALESCE(i."GenerationParametersJson"::text,'')
        FROM "PracticeAttempts" p
        JOIN "PracticeAttemptItems" pi ON pi."PracticeAttemptId"=p."Id"
        JOIN "AssessmentItems" i ON i."Id"=pi."AssessmentItemId"
        WHERE p."Id"='""" + attempt_id + """';""",
        "real G5 selected visual source")
    family, parameter_text = item.split("|", 1)
    allowed = {
        "usccss.g5.u03.l02.unit_fraction_of_unit_fraction",
        "usccss.g5.u03.l02.same_original_whole_order_comparison"
    }
    if family not in allowed:
        raise AssertionError("G5 visual test: unrelated question family")
    try:
        payload = json.loads(parameter_text)
        parameters = payload.get("parameters", payload)
    except (ValueError, AttributeError) as exc:
        raise AssertionError("G5 visual test: missing trusted parameters") from exc
    if not isinstance(parameters, dict) or set(parameters) != {
            "case", "layout", "slot", "comparison", "differentWhole"
    }:
        raise AssertionError("G5 visual test: unexpected parameter dictionary")
    if any(type(x) is not int for x in parameters.values()) or (
            parameters["case"] not in range(4) or
            parameters["comparison"] not in range(4) or
            parameters["layout"] not in range(4) or
            parameters["slot"] not in range(3) or
            parameters["differentWhole"] not in range(2)):
        raise AssertionError("G5 visual test: invalid Grade5 fraction parameters")
    svg = re.search(
        r"<svg\b[^>]*class='practice-math-visual'[^>]*>.*?</svg>",
        rendered, re.IGNORECASE | re.DOTALL)
    if svg is None:
        raise AssertionError("G5 visual test: actual student attempt has no SVG")
    drawing = svg.group(0)
    pair = [(3, 4), (4, 3), (2, 5), (5, 2)]
    first_count = pair[parameters["case"]][0] * pair[parameters["case"]][1]
    comparison_count = pair[parameters["comparison"]][0] * pair[parameters["comparison"]][1]
    product = family.endswith(".unit_fraction_of_unit_fraction")
    expected_cells = first_count if product else first_count + comparison_count
    expected_shaded = 1 if product else 2
    if len(re.findall(r"class='fraction-cell ", drawing)) != expected_cells or (
        len(re.findall(r"class='fraction-cell fraction-shaded'", drawing)) !=
            expected_shaded):
        raise AssertionError("G5 visual test: drawn cells diverge from canonical grid")
    if any(answer in drawing for answer in (
            "1/10", "1/12", "Answer 1=", "Select 1:",
            "SAME whole", "DIFFERENT whole")):
        raise AssertionError("G5 visual test: correct answer leaked in source visual")
    return {
        "selectedFamily": family,
        "studentAttemptId": attempt_id,
        "renderedSvgSha256": hashlib.sha256(drawing.encode("utf-8")).hexdigest(),
        "canonicalEqualAreaCells": expected_cells,
        "shadedCells": expected_shaded,
        "learnerSvgRendered": True,
        "publisherImageCopied": False,
        "unsubmittedConstructionTeacherScored": False
    }

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
            visual_evidence = (
                verify_g5_u03_l02_actual_practice_svg(student, lesson_id, adoption_id)
                if code.endswith("U03:L02") else None
            )
            results.append({
                "g5U03L02Visual": visual_evidence,
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
            if visual_evidence:
                print("G5_U03_L02_REAL_LEARNER_SVG_PASS",
                      visual_evidence["selectedFamily"],
                      visual_evidence["canonicalEqualAreaCells"], flush=True)
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
