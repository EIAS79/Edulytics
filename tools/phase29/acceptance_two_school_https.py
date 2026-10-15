#!/usr/bin/env python3
"""Real HTTPS two-tenant smoke test. Run only against disposable CI PostgreSQL."""
import html
import hashlib
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
    if PROGRAMME == "US-CCSS" and key == "commoncore-primary":
        # EXACT one-lesson student-facing authenticated HTTPS regression.
        # No claim about deployed Render staging; CI runs a disposable school.
        pilot_code = "PED:US-CCSS-MATH:G1:U01:L01"
        pilot_ids = sql("""
            SELECT "Id"::text FROM "CurriculumPedagogicalLessons"
            WHERE "Code"='PED:US-CCSS-MATH:G1:U01:L01';""").splitlines()
        assert len(pilot_ids) == 1, ("Count and Add source identity", pilot_ids)
        pilot_lesson_id = pilot_ids[0]
        pilot_meta = sql("""
            SELECT l."Code", l."LogicalLevelFrom"::text,
                   l."LogicalLevelTo"::text,
                   COALESCE(c."Status"::text, 'NO_CANONICAL_CONTENT'),
                   COALESCE(c."ContentVersion", 'NO_CONTENT_VERSION')
            FROM "CurriculumPedagogicalLessons" l
            LEFT JOIN "CurriculumLessonContents" c
              ON c."LessonNodeId"=l."Id"
            WHERE l."Id"='""" + pilot_lesson_id + """';""")
        print("DISPOSABLE_G1_U01_L01_CONTENT_METADATA", pilot_meta, flush=True)
        try:
            pilot_page = html.unescape(
                get_page(student, "/student/learning/lesson/" + pilot_lesson_id))
        except urllib.error.HTTPError:
            # Access remains fail-closed. Do not fake a student page to pass L6.
            print("DISPOSABLE_G1_U01_L01_NOT_VISIBLE_TO_STUDENT",
                  pilot_meta, flush=True)
            raise
        for required in (
            "Count and Add",
            "Count and show 16 counters",
            "Add dots on two cubes",
            "5 + 3 = 8",
            "16 counters",
            "rich-lesson-visual",
            "rich-lesson-section",
            "CC BY 4.0",
            "im.kendallhunt.com/k5/teachers/grade-1/unit-1/lesson-1/lesson.html",
        ):
            assert required in pilot_page, (
                "G1 U01 L01 authenticated Rich V2 content missing", required)
        assert "rich-lesson-visual--placevaluechart" in pilot_page, (
            "G1 U01 L01 ten-frame and dot-cube representation is not rendered")
        # Real Chromium rendering at desktop + mobile viewport. The validated
        # student login cookie is transferred only into the disposable CI browser.
        # This does not interact with Render or a real user's school account.
        from playwright.sync_api import sync_playwright
        jar = next(
            handler.cookiejar for handler in student.handlers
            if isinstance(handler, urllib.request.HTTPCookieProcessor))
        cookies = [{"name": cookie.name, "value": cookie.value, "url": BASE}
                   for cookie in jar if cookie.secure]
        assert cookies, "Authenticated Grade 1 browser session lacks secure cookies"
        with sync_playwright() as playwright:
            browser = playwright.chromium.launch(headless=True, args=["--no-sandbox"])
            try:
                for screen, width, height in (
                    ("desktop", 1440, 900),
                    ("mobile", 390, 844),
                ):
                    context = browser.new_context(
                        ignore_https_errors=True,
                        viewport={"width": width, "height": height})
                    try:
                        context.add_cookies(cookies)
                        page = context.new_page()
                        response = page.goto(
                            BASE + "/student/learning/lesson/" + pilot_lesson_id,
                            wait_until="domcontentloaded", timeout=40000)
                        assert response is not None and response.status == 200, (
                            screen, "Grade 1 browser lesson was not accessible")
                        # Rendering without CSS previously gave misleading
                        # green DOM checks but a huge unstyled brand logo.
                        # Require actual static asset delivery AND applied CSS.
                        site_css=page.request.get(BASE+"/css/site.css",timeout=20000)
                        assert (site_css.status==200
                                and "text/css" in site_css.headers.get("content-type","")
                                and len(site_css.body())>20000), (
                            "T001 student site.css absent or replaced by HTML",
                            screen,site_css.status,
                            site_css.headers.get("content-type",""),
                            len(site_css.body()),site_css.url)
                        brand_box=page.locator(
                            ".student-brand-link .localized-brand-logo").bounding_box()
                        assert brand_box and 0<brand_box["height"]<=90, (
                            "T001 student brand oversized / CSS not applied",
                            screen,brand_box)
                        if screen=="desktop":
                            shell_display=page.locator(
                                ".student-app-shell").evaluate(
                                    "(element) => getComputedStyle(element).display")
                            assert shell_display=="grid", (
                                "T001 desktop app shell CSS missing",shell_display)
                        print("DISPOSABLE_T001_STYLESHEET_LAYOUT_PASS",
                              screen,round(brand_box["height"]),flush=True)
                        figures = page.locator(
                            "figure.rich-lesson-visual--placevaluechart")
                        assert figures.count() == 2, (
                            screen, "expected both Count and Add representations")
                        assert figures.nth(0).locator(
                            ".rich-lesson-table td").all_text_contents().count("●") == 16, (
                            screen, "10+6 model lost counters")
                        assert figures.nth(1).locator(
                            ".rich-lesson-table td").all_text_contents().count("●") == 8, (
                            screen, "5+3 cube model lost dots")
                        assert figures.nth(0).get_by_text(
                            "Count and show 16 counters").is_visible(), (
                            screen, "counting representation not visible")
                        assert figures.nth(1).get_by_text(
                            "Add dots on two cubes").is_visible(), (
                            screen, "cube representation not visible")
                        assert page.get_by_text("CC BY 4.0").first.is_visible(), (
                            screen, "publisher source attribution hidden")
                        page.screenshot(
                            path=f"/tmp/g1-u01-l01-{screen}.png", full_page=True)
                        print("DISPOSABLE_G1_L01_CHROMIUM_VIEWPORT_PASS",
                              screen, width, height, flush=True)
                        # Full visual proof for all ten T001 lessons at
                        # both viewports. This checks the actual browser DOM
                        # and licensed source attribution, not merely HTTP.
                        lesson_rows=sql("""
                            SELECT "Code" || '|' || "Id"::text
                            FROM "CurriculumPedagogicalLessons"
                            WHERE "Code" LIKE 'PED:US-CCSS-MATH:G1:U01:L%'
                            ORDER BY "Code";""").splitlines()
                        assert len(lesson_rows)>=10, (
                            "Source-backed G1 unit 1 lesson IDs missing",lesson_rows)
                        candidates=[
                            row.split("|",1) for row in lesson_rows
                            if row.split("|",1)[0].split(":")[-1] in
                               ["L"+str(v).zfill(2) for v in range(1,11)]
                        ]
                        assert len(candidates)==10, (
                            "T001 visual source lesson identity count",candidates)
                        for lesson_code, rendered_id in candidates:
                            if lesson_code==pilot_code:
                                continue
                            rendered=page.goto(
                                BASE+"/student/learning/lesson/"+rendered_id,
                                wait_until="domcontentloaded", timeout=40000)
                            assert rendered and rendered.status==200, (
                                "T001 browser lesson inaccessible",lesson_code,screen)
                            assert page.locator(".rich-lesson-section").count()>0, (
                                "T001 browser Rich V2 missing",lesson_code,screen)
                            assert page.get_by_text(
                                "Worked model — lesson skill").first.is_visible(), (
                                "T001 source-goal worked model not in student runtime",
                                lesson_code,screen)
                            assert page.get_by_text(
                                "— question forms",exact=False).count()==0, (
                                "T001 answer-filled question wall should stay in Practice",
                                lesson_code,screen)
                            assert page.get_by_text(
                                "Learning goal:",exact=False).first.is_visible(), (
                                "T001 source-goal learner explanation not rendered",
                                lesson_code,screen)
                            if lesson_code.endswith(":L09"):
                                assert page.get_by_text(
                                    "real survey",exact=False).first.is_visible(), (
                                    "L09 must require real survey collection",
                                    lesson_code,screen)
                            assert page.get_by_text("CC BY 4.0").first.is_visible(), (
                                "T001 publisher license invisible",lesson_code,screen)
                            suffix=lesson_code.split(":")[-1]
                            expected_source=(
                                "https://im.kendallhunt.com/k5/teachers/"
                                "grade-1/unit-1/lesson-"
                                +str(int(suffix[1:]))+"/lesson.html")
                            displayed_source=page.locator(
                                ".lesson-source-attribution a").first.get_attribute("href")
                            assert displayed_source==expected_source, (
                                "T001 source attribution points to wrong original lesson",
                                lesson_code,displayed_source,expected_source)
                            assert page.locator(
                                "figure.rich-lesson-visual--placevaluechart"
                            ).count()>0, (
                                "T001 original skill visual missing",
                                lesson_code,screen)
                            visual_models={
                                "L02":"Two dot cards make an addition expression",
                                "L03":"Start at five, count forward two",
                                "L04":"Start at eight, take away two",
                                "L05":"Joining and taking away are different actions",
                            }
                            if suffix in visual_models:
                                assert page.get_by_text(
                                    visual_models[suffix],exact=True).first.is_visible(), (
                                    "T001 actual counter/counting model not displayed",
                                    lesson_code,screen)
                            if screen=="desktop":
                                rendered_sections=page.locator(
                                    ".rich-lesson-section").all_inner_texts()
                                rendered_text="\\n".join(
                                    item.strip() for item in rendered_sections)
                                assert (len(rendered_text)>400
                                        and "Worked model" in rendered_text), (
                                    "T001 rendered academic body unexpectedly empty",
                                    lesson_code,len(rendered_text))
                                digest=hashlib.sha256(
                                    rendered_text.encode("utf-8")).hexdigest()
                                print("T001_RENDERED_LESSON_SHA256",
                                      lesson_code,digest,
                                      "sections",len(rendered_sections),
                                      "characters",len(rendered_text),
                                      flush=True)
                            page.screenshot(
                                path=f"/tmp/g1-u01-{suffix.lower()}-{screen}.png",
                                full_page=True)
                            print("DISPOSABLE_T001_CHROMIUM_RENDER_PASS",
                                  lesson_code,screen,flush=True)
                    finally:
                        context.close()
            finally:
                browser.close()
        print("DISPOSABLE_US_CCSS_G1_U01_L01_STUDENT_RICH_V2_RENDER_PASS",
              pilot_code, pilot_lesson_id)
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
        if key == "commoncore-primary":
            # Source-grounded lesson-specific contract: this must prove the
            # actual first Grade 1 lesson, not any unrelated mapped lesson.
            target = [row.split("|", 1) for row in offered if
                      row.endswith("|PED:US-CCSS-MATH:G1:U01:L01")]
            assert len(target) == 1, (
                "G1 Count and Add is not offered to the actual Grade 1 student", target)
            chosen_lesson_id, chosen_lesson_code = target[0]
            practice_fields["questionCount"] = "2"  # cover BOTH publisher lesson activities
            expected_families = [
                "usccss.g1.u01.l01.count_collection_to20",
                "usccss.g1.u01.l01.add_dot_cubes_to10",
            ]
        else:
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
        expected_count = 2 if key == "commoncore-primary" else 1
        assert len(item_rows) == expected_count, (key, "missing generated exact items", item_rows)
        generated_items = [row.split("|", 3) for row in item_rows]
        assert all(row[0] == chosen_lesson_id for row in generated_items), (
            key, "practice lesson identity mismatch")
        assert all(row[1] in expected_families and row[3].strip()
                   for row in generated_items), (
            key, "unexpected family or missing exact answer", generated_items)
        if key == "commoncore-primary":
            assert {row[1] for row in generated_items} == set(expected_families), (
                "G1 counting and dot-cube activities both must be assessed", generated_items)

        # Exercise authenticated student grading for BOTH lesson activity goals,
        # not a random successful item from some other American lesson.
        csrf = re.search(
            r'name="__RequestVerificationToken"[^>]*value="([^"]+)"',
            generated_body)
        assert csrf, (key, "missing personal test CSRF token")
        answer_fields = [("__RequestVerificationToken", csrf.group(1))]
        for index, (_, family, item_id, expected_answer) in enumerate(generated_items):
            html_id = re.search(
                r'name="attemptItemIds\[' + str(index) +
                r'\]"[^>]*value="([0-9a-fA-F-]{36})"',
                generated_body)
            assert html_id and html_id.group(1) == item_id, (
                key, "personal test item identity mismatch", family, index)
            answer_fields.extend([
                ("attemptItemIds[" + str(index) + "]", item_id),
                ("answers[" + str(index) + "]", expected_answer),
            ])
        with student.open(urllib.request.Request(
            BASE + "/student/practice/attempt/" + match_attempt.group(1)
            + "/personal-test-submit",
            data=urllib.parse.urlencode(answer_fields).encode(), method="POST",
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
        assert complete == "1" and float(score) == float(expected_count) and int(correct_responses) == expected_count, (
            key, "exact Practice answers not correctly scored/persisted", evidence)
        print("DISPOSABLE_US_CCSS_EXACT_LESSON_ANSWER_PASS", key,
              ",".join(row[1] for row in generated_items), flush=True)

        # T001 isolated 10-lesson candidate: one actual authenticated
        # StudentPortal GET, generation, correct-answer marking and persisted
        # attempt for EACH remaining lesson 02-10. CI fixtures only.
        # The already-proven L01 pilot is assessed above.
        if key == "commoncore-primary":
            t001 = {
                "02": ["usccss.t001.g1.dot_pair_sum",
                       "usccss.t001.g1.choose_dot_expression"],
                "03": ["usccss.t001.g1.add_one_or_two"],
                "04": ["usccss.t001.g1.subtract_one_or_two"],
                "05": ["usccss.t001.g1.mixed_add_subtract"],
                "06": ["usccss.t001.g1.make_ten_missing_part",
                       "usccss.t001.g1.mixed_add_subtract"],
                "07": ["usccss.t001.g1.sort_math_tools",
                       "usccss.t001.g1.choose_tool_sort"],
                "08": ["usccss.t001.g1.sort_shape_cards",
                       "usccss.t001.g1.choose_shape_chart"],
                "09": ["usccss.t001.g1.class_survey_categories",
                       "usccss.t001.g1.choose_survey_table"],
                "10": ["usccss.t001.g1.make_ten_missing_part",
                       "usccss.t001.g1.mixed_add_subtract"],
            }
            offered_by_code = {
                code: lesson_id
                for lesson_id, code in (
                    row.split("|", 1) for row in offered if "|" in row)
            }
            for suffix, expected in t001.items():
                code = "PED:US-CCSS-MATH:G1:U01:L" + suffix
                assert code in offered_by_code, (
                    "T001 lesson not offered to the actual Grade 1 pupil", code)
                lesson_id = offered_by_code[code]
                lesson_html = html.unescape(
                    get_page(student, "/student/learning/lesson/" + lesson_id))
                assert "rich-lesson-section" in lesson_html, (
                    "T001 rendered lesson lacks Rich V2", code)
                if suffix == "05":
                    assert "3 + 2 = 5" in lesson_html and "6 - 2 = 4" in lesson_html, (
                        "T001 L05 cannot teach both operations", code)
                if suffix == "06":
                    assert "6 + 4 = 10" in lesson_html and "10 - 6 = 4" in lesson_html, (
                        "T001 L06 missing make-ten instruction", code)
                if suffix in ("07", "08", "09"):
                    assert "rich-lesson-visual--placevaluechart" in lesson_html, (
                        "T001 category picture not rendered", code)
                form = get_page(student, "/student/practice")
                token = re.search(
                    r'name="__RequestVerificationToken"[^>]*value="([^"]+)"',
                    form)
                assert token, ("T001 practice form missing CSRF", code)
                fields = {
                    "curriculumAdoptionId": adoption.group(1),
                    "scope": "Lesson",
                    "lessonId": lesson_id,
                    "difficulty": "AtClassLevel",
                    "questionCount": str(len(expected)),
                    "__RequestVerificationToken": token.group(1),
                }
                with student.open(urllib.request.Request(
                    BASE + "/student/practice/generate",
                    data=urllib.parse.urlencode(fields).encode(), method="POST",
                ), timeout=60) as response:
                    assert response.status == 200, (code, "generation HTTP failure")
                    attempt_url = urllib.parse.urlparse(response.url).path.lower()
                    found = re.fullmatch(
                        r"/student/practice/attempt/([0-9a-f-]{36})",
                        attempt_url)
                    assert found, (code, "did not create Practice attempt", attempt_url)
                    attempt_id = found.group(1)
                    attempt_body = response.read().decode("utf-8")
                recorded = sql("""
                    SELECT a."CurriculumPedagogicalLessonId"::text,
                           i."GenerationFamily",pi."Id"::text,i."CorrectAnswer"
                    FROM "PracticeAttempts" a
                    JOIN "PracticeAttemptItems" pi
                      ON pi."PracticeAttemptId"=a."Id"
                    JOIN "AssessmentItems" i ON i."Id"=pi."AssessmentItemId"
                    WHERE a."Id"='""" + attempt_id + """'
                    ORDER BY pi."Order";""").splitlines()
                items = [line.split("|", 3) for line in recorded]
                assert len(items) == len(expected), (
                    code, "missing persisted exact generated items", items)
                assert all(row[0] == lesson_id and row[1] in expected
                           for row in items), (
                    code, "wrong lesson or out-of-scope Practice family", items)
                assert {row[1] for row in items} == set(expected), (
                    code, "incomplete source goal question families", items)
                csrf = re.search(
                    r'name="__RequestVerificationToken"[^>]*value="([^"]+)"',
                    attempt_body)
                assert csrf, (code, "no attempt CSRF token")
                response_fields = [
                    ("__RequestVerificationToken", csrf.group(1))]
                for index, (_, family, item_id, answer) in enumerate(items):
                    posted_id = re.search(
                        r'name="attemptItemIds\[' + str(index) +
                        r'\]"[^>]*value="([0-9a-fA-F-]{36})"',
                        attempt_body)
                    assert posted_id and posted_id.group(1) == item_id, (
                        code, family, "item mismatch")
                    response_fields.extend([
                        ("attemptItemIds[" + str(index) + "]", item_id),
                        ("answers[" + str(index) + "]", answer),
                    ])
                with student.open(urllib.request.Request(
                    BASE + "/student/practice/attempt/" + attempt_id
                    + "/personal-test-submit",
                    data=urllib.parse.urlencode(response_fields).encode(),
                    method="POST",
                ), timeout=60) as grade_result:
                    assert grade_result.status == 200, (
                        code, "student answer submission failed")
                saved = sql("""
                    SELECT CASE WHEN p."SubmittedAtUtc" IS NOT NULL THEN 1 ELSE 0 END,
                           p."Score"::text,COUNT(r."Id")::text
                    FROM "PracticeAttempts" p
                    JOIN "PracticeAttemptItems" ai
                      ON ai."PracticeAttemptId"=p."Id"
                    LEFT JOIN "PracticeResponses" r
                      ON r."PracticeAttemptItemId"=ai."Id"
                     AND r."IsCorrect"=true
                    WHERE p."Id"='""" + attempt_id + """'
                    GROUP BY p."Id",p."SubmittedAtUtc",p."Score";""")
                completed, score, correct = saved.split("|")
                assert completed == "1" and float(score) == len(items) \
                       and int(correct) == len(items), (
                    code, "student correct-answer grading/persistence failed",
                    saved)
                print("DISPOSABLE_T001_AUTHENTICATED_L6_PRACTICE_PASS",
                      code,attempt_id,",".join(expected),flush=True)

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
