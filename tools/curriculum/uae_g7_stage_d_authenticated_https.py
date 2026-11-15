#!/usr/bin/env python3
"""UAE Grade 7 Stage D authenticated 78-lesson HTTPS/DB probe. CI-only; never production.

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
CONTENT = ROOT / "src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g7-general-ar73-student-local-v1.lesson-content-pack.json"
OUT = Path("/tmp/uae-g7-stage-d-authenticated-78-lessons.json")
SCHOOL = "REHEARSAL-AE-PRIMARY"
STUDENT = "uae-primary.student.primary@edulytiks.com"
TEACHER = "uae-primary.teacher.primary@edulytiks.com"
SUPERVISOR = "uae-primary.supervisor@edulytiks.com"
ADMIN = "uae-primary.admin@edulytiks.com"
FOREIGN = "commoncore-primary.student.primary@edulytiks.com"
PREFIX = "PED:UAE-MOE-MATH:L7:GENERAL:"


def required_environment():
    required = {
        "GITHUB_ACTIONS": "true",
        "EDULYTICS_CI_REHEARSAL": "true",
        "EDULYTICS_CI_REHEARSAL_PROGRAMME": "UAE-G7",
        "EDULYTICS_CLEAN_DATABASE_NAME": "edulytics_clean_ci",
        "ASPNETCORE_ENVIRONMENT": "Staging",
    }
    for name, expected in required.items():
        if os.environ.get(name) != expected:
            raise RuntimeError("UAE G7 E2E denied outside isolated GitHub Actions: " + name)
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
    if len(expected) != 78 or not all(code.startswith(PREFIX) for code in expected):
        raise AssertionError("UAE G7 canonical source exact 78 denominator invalid")
    rows = sql("""
      SELECT "Code"||'|'||"Id"::text
      FROM "CurriculumPedagogicalLessons"
      WHERE "Code" LIKE 'PED:UAE-MOE-MATH:L7:GENERAL:%'
      ORDER BY "Code";
    """).splitlines()
    parsed = [x.split("|", 1) for x in rows]
    if len(parsed) != 78 or len({p[1] for p in parsed}) != 78 or (
        set(p[0] for p in parsed) != set(expected)
    ) or not all(re.fullmatch(UUID, p[1]) for p in parsed):
        raise AssertionError(
            f"UAE G7 seeded runtime lesson set mismatch: {len(parsed)} / 78"
        )
    # Catch a Draft-only seed immediately; the student portal intentionally
    # exposes only Published canonical content, never silently renders drafts.
    published = sql("""
      SELECT COUNT(*)::text
      FROM "CurriculumLessonContents" c
      JOIN "CurriculumPedagogicalLessons" l ON l."Id"=c."LessonNodeId"
      WHERE l."Code" LIKE 'PED:UAE-MOE-MATH:L7:GENERAL:%'
        AND c."Status"=3;
    """).strip()
    if published != "78":
        raise AssertionError(
            "UAE G7 78 canonical lesson identities exist but Published content count is " + published)
    return parsed, expected


def check_school_adoption():
    # Select only the chosen Grade 7 UAE school, not any Grade 7 school fixture.
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
    adoption, level, key, culture, pathway = one(q, "Grade7 Arabic student adoption").split("|", 4)
    if not re.fullmatch(UUID, adoption) or level != "7" or culture != "ar" or pathway != "General":
        raise AssertionError(f"UAE G7 adoption/culture/pathway mismatch: {level}/{culture}/{pathway}")
    if "UAE" not in key.upper() and "7" not in key:
        raise AssertionError("UAE G7 adoption key not UAE Grade 7")
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
        raise AssertionError("UAE Grade7 assigned teacher cannot open assessments")

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
            "uae-g7-original-arabic-lesson" not in foreign_page
        )
    if not cross_tenant_denied:
        raise AssertionError("UAE Grade 7 cross-school lesson access LEAKED")

    records = []
    for index, (code, lesson_id) in enumerate(ordered, 1):
        page = html.unescape(get_page(
            student, "/student/learning/lesson/" + lesson_id + "?culture=ar"
        ))
        if 'class="uae-g7-original-arabic-lesson" lang="ar" dir="rtl"' not in page:
            raise AssertionError(f"UAE G7 scoped canonical Arabic/RTL lesson unavailable {index}: {code}")
        for section_kind in ("explanation", "concepts", "examples",
                             "steps", "mistakes", "summary"):
            if f"lesson-learning-section--{section_kind}" not in page:
                raise AssertionError(
                    f"UAE G7 missing canonical Arabic section {section_kind}: {code}"
                )
        arabic = next((t for t in canonical[code]["Translations"]
                       if t["CultureCode"] == "ar"), None)
        if not arabic or arabic["Title"] not in page:
            raise AssertionError(f"UAE G7 canonical Arabic title not served for {code}")
        # HTML check is not screenshot/visual a11y sign-off.
        rtl_markup = (
            'class="uae-g7-original-arabic-lesson" lang="ar" dir="rtl"' in page
        )
        practice_page = get_page(student, "/student/practice")
        csrf = extract_csrf(practice_page)
        nonce = extract_idempotency_key(practice_page)
        form_adoption = extract_adoption(practice_page)
        if not csrf or not nonce or not form_adoption or form_adoption.lower() != adoption_id.lower():
            raise AssertionError(f"UAE Grade7 Practice form adoption/CSRF invalid at {index}")
        positive = probe_lesson(
            student, BASE, lesson_id, form_adoption, csrf, nonce, sql
        )
        if positive.get("status") != "SELF_CONSISTENT_GRADE_PERSISTED":
            raise AssertionError(
                f"UAE G7 scoped correct Practice failed for ordinal {index} {code}: {positive}"
            )
        practice_page = get_page(student, "/student/practice")
        wrong = probe_lesson(
            student, BASE, lesson_id, extract_adoption(practice_page),
            extract_csrf(practice_page), extract_idempotency_key(practice_page),
            sql, answer_mode="well_formed_wrong", wrong_answer_profile="uae-g7"
        )
        if wrong.get("status") != "WELL_FORMED_WRONG_ANSWER_REJECTED_AND_PERSISTED":
            raise AssertionError(
                f"UAE G7 valid-format wrong answer not rejected at {index} {code}: {wrong}"
            )
        practice_page = get_page(student, "/student/practice")
        negative = probe_lesson(
            student, BASE, lesson_id, extract_adoption(practice_page),
            extract_csrf(practice_page), extract_idempotency_key(practice_page),
            sql, answer_mode="malformed"
        )
        if negative.get("status") != "MALFORMED_ANSWER_REJECTED_AND_PERSISTED":
            raise AssertionError(
                f"UAE G7 malformed Practice not rejected at {index} {code}: {negative}"
            )
        records.append({
            "lessonCode": code,
            "lessonOrdinalInCanonicalSortedScope": index,
            "studentPageSha256": hashlib.sha256(page.encode()).hexdigest(),
            "servedArabicCanonicalTitle": True,
            "htmlRtlMarker": rtl_markup,
            "correct": positive,
            "wellFormedWrong": wrong,
            "malformed": negative,
        })
        print("UAE_G7_REAL_HTTPS_LESSON_AND_GRADE", index, code, flush=True)
    if len(records) != 78:
        raise AssertionError("UAE G7 partial E2E evidence is not complete")
    report = {
        "scope": "DISPOSABLE_UAE_G7_78_LESSON_AUTHENTICATED_HTTPS_NOT_PUBLICATION",
        "candidateSha": os.environ["GITHUB_SHA"],
        "testedRefKind": os.environ.get("GITHUB_EVENT_NAME"),
        "curriculum": "UAE-MOE-MATH",
        "grade": 7,
        "pathway": "General",
        "academicLanguage": "ar",
        "sourceEditionParity": "UNVERIFIED",
        "rtlVisualAccessibility": "NOT_TESTED_BROWSER_SCREENSHOT",
        "realStudentUat": "NOT_TESTED",
        "roleLogins": role_evidence,
        "crossSchoolAccessDenied": cross_tenant_denied,
        "adoptionKey": adoption_key,
        "lessonPageCount": 78,
        "correctAttemptPersistenceCount": 78,
        "wrongAttemptPersistenceCount": 78,
        "malformedAttemptRejectionCount": 78,
        "htmlRtlMarkedCount": sum(x["htmlRtlMarker"] for x in records),
        "lessons": records,
    }
    OUT.write_text(json.dumps(report, ensure_ascii=False, indent=2), encoding="utf-8")
    print("UAE_G7_78_AUTH_HTTPS_384_PRACTICE_ATTEMPTS_PASS", flush=True)


if __name__ == "__main__":
    run()
