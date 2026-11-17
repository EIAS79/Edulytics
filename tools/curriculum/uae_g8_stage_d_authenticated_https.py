#!/usr/bin/env python3
"""UAE Grade 8 Stage D authenticated 76-lesson HTTPS/DB probe. CI-only; never production.

Per lesson: canonical Arabic student page, isolated real scoped Practice
generation, correct, valid-format wrong, and malformed grade persistence. This proves runtime
self-consistency, NOT source-edition academic parity or human RTL acceptance.
"""
import hashlib
import html
import json
import os
from pathlib import Path
import re
import sys
import urllib.error
sys.path.insert(0, str(Path(__file__).resolve().parents[1] / "phase29"))
from g5_stage_d_authenticated_https import (
    sql, login_actor, get_page, BASE, UUID, extract_adoption, extract_csrf,
    extract_idempotency_key, probe_lesson,
)

ROOT = Path(__file__).resolve().parents[2]
CONTENT = ROOT / "src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g8-general-t1-ogl-v1.lesson-content-pack.json"
OUT = Path("/tmp/uae-g8-stage-d-authenticated-76-lessons.json")
SCHOOL = "REHEARSAL-AE-MIDDLE"
STUDENT = "uae-middle.student.secondary@edulytiks.com"
TEACHER = "uae-middle.teacher.primary@edulytiks.com"
SUPERVISOR = "uae-middle.supervisor@edulytiks.com"
ADMIN = "uae-middle.admin@edulytiks.com"
FOREIGN = "commoncore-middle.student.primary@edulytiks.com"
PREFIX = "PED:UAE-MOE-MATH:L8:GENERAL:"


def required_environment():
    required = {
        "GITHUB_ACTIONS": "true",
        "EDULYTICS_CI_REHEARSAL": "true",
        "EDULYTICS_CI_REHEARSAL_PROGRAMME": "UAE-G8",
        "EDULYTICS_CLEAN_DATABASE_NAME": "edulytics_clean_ci",
        "ASPNETCORE_ENVIRONMENT": "Staging",
    }
    for name, expected in required.items():
        if os.environ.get(name) != expected:
            raise RuntimeError("UAE G8 E2E denied outside isolated GitHub Actions: " + name)
    assert BASE == "https://localhost:5052"


def one(query, label):
    rows = sql(query).splitlines()
    if len(rows) != 1 or not rows[0]:
        raise AssertionError(f"{label}: expected exactly one row, found {len(rows)}")
    return rows[0]


def check_catalogue():
    pack = json.loads(CONTENT.read_text(encoding="utf-8"))
    assert pack["AcademicLanguage"] == "ar"
    expected = {row["LessonCode"]: row for row in pack["Lessons"]}
    if len(expected) != 76 or not all(code.startswith(PREFIX) for code in expected):
        raise AssertionError("UAE G8 canonical source exact 76 denominator invalid")
    rows = sql("""
      SELECT "Code"||'|'||"Id"::text
      FROM "CurriculumPedagogicalLessons"
      WHERE "Code" LIKE 'PED:UAE-MOE-MATH:L8:GENERAL:%'
      ORDER BY "Code";
    """).splitlines()
    parsed = [x.split("|", 1) for x in rows]
    if len(parsed) != 76 or len({p[1] for p in parsed}) != 76 or (
        set(p[0] for p in parsed) != set(expected)
    ) or not all(re.fullmatch(UUID, p[1]) for p in parsed):
        raise AssertionError(
            f"UAE G8 seeded runtime lesson set mismatch: {len(parsed)} / 76"
        )
    # Catch a Draft-only seed immediately; the student portal intentionally
    # exposes only Published canonical content, never silently renders drafts.
    published = sql("""
      SELECT COUNT(*)::text
      FROM "CurriculumLessonContents" c
      JOIN "CurriculumPedagogicalLessons" l ON l."Id"=c."LessonNodeId"
      WHERE l."Code" LIKE 'PED:UAE-MOE-MATH:L8:GENERAL:%'
        AND c."Status"=3;
    """).strip()
    if published != "76":
        raise AssertionError(
            "UAE G8 76 canonical lesson identities exist but Published content count is " + published)
    return parsed, expected


def check_school_adoption():
    # Select only the chosen Grade 8 UAE school, not any Grade 8 school fixture.
    q = """
      SELECT c."CurriculumAdoptionId"::text||'|'||
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
      WHERE u."Email"='""" + STUDENT + """' AND s."SchoolCode"='""" + SCHOOL + """';
    """
    adoption, level, key, culture, pathway = one(q, "Grade8 Arabic student adoption").split("|", 4)
    if not re.fullmatch(UUID, adoption) or level != "8" or culture != "ar" or pathway != "General":
        raise AssertionError(f"UAE G8 adoption/culture/pathway mismatch: {level}/{culture}/{pathway}")
    if "UAE" not in key.upper() or "08" not in key:
        raise AssertionError("UAE G8 adoption key not UAE Grade 8")
    return adoption, key


def run():
    required_environment()
    ordered, canonical = check_catalogue()
    adoption_id, adoption_key = check_school_adoption()
    student = login_actor(STUDENT, "Student")
    teacher = login_actor(TEACHER, "Teacher")
    supervisor = login_actor(SUPERVISOR, "SubjectSupervisor")
    school_admin = login_actor(ADMIN, "SchoolAdmin")
    foreign = login_actor(FOREIGN, "Student")
    role_evidence = {
        "student": True,
        "teacher": True,
        "subjectSupervisor": True,
        "schoolAdmin": True,
        "foreignSchoolStudent": True,
    }

    # Four real authenticated roles; verify their session grants at an
    # authenticated endpoint, do not treat successful anonymous login page as access.
    if "assessments" not in get_page(teacher, "/school/assessments").lower():
        raise AssertionError("UAE Grade8 assigned teacher cannot open assessments")

    try:
        foreign_page = get_page(
            foreign, "/student/learning/lesson/" + ordered[0][1] + "?culture=ar"
        )
    except urllib.error.HTTPError as exc:
        if exc.code not in (401, 403, 404):
            raise
        cross_tenant_denied = True
    else:
        cross_tenant_denied = (
            "rich-lesson-section" not in foreign_page and
            "uae-g8-original-arabic-lesson" not in foreign_page
        )
    if not cross_tenant_denied:
        raise AssertionError("UAE Grade 8 cross-school lesson access LEAKED")

    records = []
    for index, (code, lesson_id) in enumerate(ordered, 1):
        page = html.unescape(get_page(
            student, "/student/learning/lesson/" + lesson_id + "?culture=ar"
        ))
        if 'class="uae-g8-original-arabic-lesson" lang="ar" dir="rtl"' not in page:
            raise AssertionError(f"UAE G8 scoped canonical Arabic/RTL lesson unavailable {index}: {code}")
        for section_kind in ("explanation", "concepts", "examples",
                             "steps", "mistakes", "summary"):
            if f"lesson-learning-section--{section_kind}" not in page:
                raise AssertionError(
                    f"UAE G8 missing canonical Arabic section {section_kind}: {code}"
                )
        arabic = next((t for t in canonical[code]["Translations"]
                       if t["CultureCode"] == "ar"), None)
        if not arabic or arabic["Title"] not in page:
            raise AssertionError(f"UAE G8 canonical Arabic title not served for {code}")
        # Negative-locale runtime guard: English UI selection must not
        # silently switch this General curriculum to obsolete English lessons.
        # An unsupported locale may be rejected or display the same Arabic
        # canonical lesson; neither path may serve English instructional content.
        try:
            english_request = html.unescape(get_page(
                student, "/student/learning/lesson/" + lesson_id + "?culture=en"
            ))
        except urllib.error.HTTPError as exc:
            if exc.code not in (403, 404, 406):
                raise
            negative_locale_result = "DENIED_" + str(exc.code)
        else:
            if ('class="uae-g8-original-arabic-lesson" lang="ar" dir="rtl"'
                    not in english_request or arabic["Title"] not in english_request):
                raise AssertionError(
                    f"UAE G8 legacy English lesson fallback for {index}: {code}"
                )
            negative_locale_result = "ARABIC_CANONICAL"
        # HTML check is not screenshot/visual a11y sign-off.
        rtl_markup = (
            'class="uae-g8-original-arabic-lesson" lang="ar" dir="rtl"' in page
        )
        practice_page = get_page(student, "/student/practice")
        csrf = extract_csrf(practice_page)
        nonce = extract_idempotency_key(practice_page)
        form_adoption = extract_adoption(practice_page)
        if not csrf or not nonce or not form_adoption or form_adoption.lower() != adoption_id.lower():
            raise AssertionError(f"UAE Grade8 Practice form adoption/CSRF invalid at {index}")
        positive = probe_lesson(
            student, BASE, lesson_id, form_adoption, csrf, nonce, sql
        )
        if positive.get("status") != "SELF_CONSISTENT_GRADE_PERSISTED":
            raise AssertionError(
                f"UAE G8 scoped correct Practice failed for ordinal {index} {code}: {positive}"
            )
        practice_page = get_page(student, "/student/practice")
        wrong = probe_lesson(
            student, BASE, lesson_id, extract_adoption(practice_page),
            extract_csrf(practice_page), extract_idempotency_key(practice_page),
            sql, answer_mode="well_formed_wrong", wrong_answer_profile="uae-g8"
        )
        if wrong.get("status") != "WELL_FORMED_WRONG_ANSWER_REJECTED_AND_PERSISTED":
            raise AssertionError(
                f"UAE G8 valid-format wrong answer not rejected at {index} {code}: {wrong}"
            )
        practice_page = get_page(student, "/student/practice")
        negative = probe_lesson(
            student, BASE, lesson_id, extract_adoption(practice_page),
            extract_csrf(practice_page), extract_idempotency_key(practice_page),
            sql, answer_mode="malformed"
        )
        if negative.get("status") != "MALFORMED_ANSWER_REJECTED_AND_PERSISTED":
            raise AssertionError(
                f"UAE G8 malformed Practice not rejected at {index} {code}: {negative}"
            )
        records.append({
            "lessonCode": code,
            "lessonOrdinalInCanonicalSortedScope": index,
            "studentPageSha256": hashlib.sha256(page.encode()).hexdigest(),
            "servedArabicCanonicalTitle": True,
            "htmlRtlMarker": rtl_markup,
            "negativeEnglishLocale": negative_locale_result,
            "correct": positive,
            "wellFormedWrong": wrong,
            "malformed": negative,
        })
        print("UAE_G8_REAL_HTTPS_LESSON_AND_GRADE", index, code, flush=True)
    if len(records) != 76:
        raise AssertionError("UAE G8 partial E2E evidence is not complete")
    report = {
        "scope": "DISPOSABLE_UAE_G8_76_LESSON_AUTHENTICATED_HTTPS_NOT_PUBLICATION",
        "candidateSha": os.environ["GITHUB_SHA"],
        "testedRefKind": os.environ.get("GITHUB_EVENT_NAME"),
        "curriculum": "UAE-MOE-MATH",
        "grade": 8,
        "pathway": "General",
        "academicLanguage": "ar",
        "sourceEditionParity": "UNVERIFIED",
        "rtlVisualAccessibility": "NOT_TESTED_BROWSER_SCREENSHOT",
        "realStudentUat": "NOT_TESTED",
        "roleLogins": role_evidence,
        "crossSchoolAccessDenied": cross_tenant_denied,
        "adoptionKey": adoption_key,
        "lessonPageCount": 76,
        "correctAttemptPersistenceCount": 76,
        "wrongAttemptPersistenceCount": 76,
        "malformedAttemptRejectionCount": 76,
        "htmlRtlMarkedCount": sum(x["htmlRtlMarker"] for x in records),
        "negativeEnglishFallbackChecks": sum(
            x["negativeEnglishLocale"].startswith(("DENIED_", "ARABIC_CANONICAL"))
            for x in records
        ),
        "lessons": records,
    }
    OUT.write_text(json.dumps(report, ensure_ascii=False, indent=2), encoding="utf-8")
    print("UAE_G8_76_AUTH_HTTPS_228_PRACTICE_ATTEMPTS_PASS", flush=True)


if __name__ == "__main__":
    run()
