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
import uuid

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
            # The public CC BY 4.0 footer links the original teacher
            # preparation page, not the private V9 source audit's lesson.html.
            # Validate the actual credited URL instead of a stale test literal.
            "im.kendallhunt.com/k5/teachers/grade-1/unit-1/lesson-1/preparation.html",
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
                    finally:
                        context.close()
            finally:
                browser.close()
        print("DISPOSABLE_US_CCSS_G1_U01_L01_STUDENT_RICH_V2_RENDER_PASS",
              pilot_code, pilot_lesson_id)
        # Second original lesson: dot-cube equivalence is a different
        # content SHA. L01 screenshots or a successful sign-in cannot certify L02.
        next_code="PED:US-CCSS-MATH:G1:U01:L02"
        l02_screens={}
        l02_counts={}

        next_ids=sql("""
            SELECT "Id"::text FROM "CurriculumPedagogicalLessons"
            WHERE "Code"='PED:US-CCSS-MATH:G1:U01:L02';""").splitlines()
        assert len(next_ids)==1, ("Original G1 L02 source identity",next_ids)
        next_id=next_ids[0]
        next_html=html.unescape(get_page(
            student,"/student/learning/lesson/"+next_id))
        assert "rich-lesson-visual--placevaluechart" in next_html, (
            "G1 L02 authentic student Rich V2 dot pictures missing")
        assert "Match dot pictures" in next_html, (
            "G1 L02 source dot-cube activity title not visible")
        with sync_playwright() as playwright:
            browser=playwright.chromium.launch(headless=True,args=["--no-sandbox"])
            try:
                for screen,width,height in (
                    ("desktop",1440,900),
                    ("mobile",390,844),
                ):
                    context=browser.new_context(
                        ignore_https_errors=True,
                        viewport={"width":width,"height":height})
                    try:
                        context.add_cookies(cookies)
                        page=context.new_page()
                        resp=page.goto(
                            BASE+"/student/learning/lesson/"+next_id,
                            wait_until="domcontentloaded",timeout=40000)
                        assert resp is not None and resp.status==200, (
                            screen,"L02 real student page unavailable")
                        figures=page.locator(
                            "figure.rich-lesson-visual--placevaluechart")
                        assert figures.count()>0, (
                            screen,"L02 actual dot cube visuals absent")
                        count_sequence=[]
                        for figure in figures.all():
                            dots=figure.locator(
                                ".rich-lesson-table td").all_text_contents().count("●")
                            assert 1<=dots<=20,(
                                screen,"L02 dot pictures have invalid count",dots)
                            count_sequence.append(dots)
                        l02_counts[screen]=count_sequence
                        assert figures.first.is_visible(),(
                            screen,"L02 learner visual hidden")
                        shot=Path(f"/tmp/g1-u01-l02-{screen}.png")
                        page.screenshot(path=str(shot),full_page=True)
                        assert shot.exists() and shot.stat().st_size>1000,(
                            screen,"L02 screenshot was not persisted")
                        l02_screens[screen]={
                            "viewportWidth":width,"viewportHeight":height,
                            "screenshotSha256":hashlib.sha256(shot.read_bytes()).hexdigest()
                        }
                        print("DISPOSABLE_G1_L02_CHROMIUM_VIEWPORT_PASS",
                              screen,width,height,flush=True)
                    finally:
                        context.close()
            finally:
                browser.close()
        print("DISPOSABLE_US_CCSS_G1_U01_L02_STUDENT_RICH_V2_RENDER_PASS",
              next_code,next_id,flush=True)
        # G1 Unit 1 lessons 03-05: distinct source-bound authenticated
        # rendering witnesses. These are candidate L6 checks, NOT approval.
        additional_lessons = (
            ("PED:US-CCSS-MATH:G1:U01:L03", "Add 1 or 2",
             "add within 10 when one addend is 1 or 2"),
            ("PED:US-CCSS-MATH:G1:U01:L04", "More Work with 1 and 2",
             "subtract 1 or 2 within 10"),
            ("PED:US-CCSS-MATH:G1:U01:L05",
             "Explore Addition and Subtraction PLC Activity",
             "add and subtract within 10"),
        )
        root=Path(__file__).resolve().parents[2]
        manifest=json.loads((root/"artifacts/academic-us-ccss/batches/B01-lesson-manifest.json").read_text())
        ledger=json.loads((root/"artifacts/academic-us-ccss/04-lesson-certification-records.json").read_text())
        manifest_by_code={x["lessonCode"]:x for x in manifest["lessons"]}
        ledger_by_code={x["lessonCode"]:x for x in ledger["records"]}
        extra_witness={
            "schemaVersion":1,
            "kind":"B01_G1_U01_L03_L05_AUTHENTICATED_STUDENT_RENDER_CANDIDATES",
            "environment":"DISPOSABLE_US_PRIMARY_HTTPS_POSTGRES",
            "checkedOutCommit":os.environ["GITHUB_SHA"],
            "academicDecision":"NOT_CERTIFIED",
            "lessonRecords":[]
        }
        extra_ids={}
        for code,title,objective in additional_lessons:
            result=sql("""
                SELECT "Id"::text FROM "CurriculumPedagogicalLessons"
                WHERE "Code"='""" + code + """';""").splitlines()
            assert len(result)==1, ("G1 U01 additional lesson source ID mismatch",code,result)
            lesson_id=result[0]
            rendered=html.unescape(get_page(
                student,"/student/learning/lesson/"+lesson_id))
            for required in (title,objective,"rich-lesson-section","rich-lesson-visual"):
                assert required.lower() in rendered.lower(), (
                    "Authenticated source-purpose lesson has incomplete learner content",
                    code,required)
            assert code in manifest_by_code and code in ledger_by_code
            m=manifest_by_code[code]
            l=ledger_by_code[code]
            assert l["status"]=="AUTOMATED_SCREENED" and l["academicDecision"]=="NOT_CERTIFIED",(
                "Do not claim an academic certificate via disposable browser",code)
            assert len(l["contentSha256"])==64
            extra_ids[code]=lesson_id
            extra_witness["lessonRecords"].append({
                "lessonCode":code,"title":title,
                "sourceSemanticSha256":m["sourceSemanticSha256"],
                "effectiveContentSha256":l["contentSha256"],
                "learnerPageHttpVerified":True,
                "screenshots":{}
            })
        with sync_playwright() as playwright:
            browser=playwright.chromium.launch(headless=True,args=["--no-sandbox"])
            try:
                for screen,width,height in (("desktop",1440,900),("mobile",390,844)):
                    context=browser.new_context(ignore_https_errors=True,
                        viewport={"width":width,"height":height})
                    try:
                        context.add_cookies(cookies)
                        page=context.new_page()
                        for row in extra_witness["lessonRecords"]:
                            code=row["lessonCode"]
                            lesson_id=extra_ids[code]
                            response=page.goto(
                                BASE+"/student/learning/lesson/"+lesson_id,
                                wait_until="domcontentloaded",timeout=40000)
                            assert response is not None and response.status==200,(
                                "G1 U01 student Chromium page unavailable",code,screen)
                            assert page.get_by_text(row["title"],exact=False).first.is_visible(),(
                                "G1 U01 lesson title not visible",code,screen)
                            figures=page.locator("figure.rich-lesson-visual")
                            assert figures.count()>0,(
                                "G1 U01 mathematically meaningful visual absent",
                                code,screen)
                            assert figures.first.is_visible(),(
                                "G1 U01 first maths representation hidden",
                                code,screen)
                            suffix=code.rsplit(":",1)[-1].lower()
                            image_path=Path("/tmp/g1-u01-"+suffix+"-"+screen+".png")
                            page.screenshot(path=str(image_path),full_page=True)
                            digest=hashlib.sha256(image_path.read_bytes()).hexdigest()
                            row["screenshots"][screen]={
                                "viewportWidth":width,"viewportHeight":height,
                                "imageSha256":digest,
                                "visibleMathVisualCount":figures.count()
                            }
                            print("DISPOSABLE_G1_SOURCE_SCREENSHOT_CANDIDATE_PASS",
                                  code,screen,figures.count(),flush=True)
                    finally:
                        context.close()
            finally:
                browser.close()
        Path("/tmp/g1-u01-l03-l05-authenticated-student-evidence.json").write_text(
            json.dumps(extra_witness,indent=2)+"\n",encoding="utf-8")
        print("DISPOSABLE_G1_U01_L03_L05_SOURCE_SCOPED_L6_CANDIDATE_PASS",
              len(extra_witness["lessonRecords"]),flush=True)
        # Enumerate EVERY G1 original lesson through the authenticated
        # student's actual HTTPS endpoint, with exact source identity and
        # learner title; not an academic L6/visual correctness decision.
        grade1_lessons=[
            x for x in manifest["lessons"]
            if x["lessonCode"].startswith("PED:US-CCSS-MATH:G1:")
        ]
        assert len(grade1_lessons)==146, "Original grade 1 lesson count drift"
        db_rows=sql("""
            SELECT "Code", "Id"::text FROM "CurriculumPedagogicalLessons"
            WHERE "Code" LIKE 'PED:US-CCSS-MATH:G1:%'
            ORDER BY "Code";""").splitlines()
        id_by_code={}
        for line in db_rows:
            code, sep, lesson_id=line.partition("|")
            assert sep and code not in id_by_code, (
                "Grade 1 duplicate/malformed DB lesson identity",line)
            id_by_code[code]=lesson_id
        assert len(id_by_code)==146,(
            "Grade 1 missing/extra source identities",len(id_by_code))
        grade1_report={
            "schemaVersion":1,
            "kind":"B01_G1_146_AUTHENTICATED_STUDENT_HTTPS_CONTENT_INVENTORY",
            "decision":"ROUTE_AND_STUDENT_CONTENT_SCREENED_NO_L6_OR_ACADEMIC_PASS",
            "checkedOutCommit":os.environ["GITHUB_SHA"],
            "batchId":"B01",
            "schoolCode":"REHEARSAL-US-PRIMARY",
            "lessonRows":[]
        }
        for position,m in enumerate(grade1_lessons,1):
            code=m["lessonCode"]
            assert code in id_by_code and code in ledger_by_code,(
                "Missing DB or certification registry original G1 identity",code)
            item=ledger_by_code[code]
            assert item["contentSha256"] and len(item["contentSha256"])==64
            lesson_html=html.unescape(get_page(
                student,"/student/learning/lesson/"+id_by_code[code]))
            matched_title=m["title"].casefold() in lesson_html.casefold()
            has_section="rich-lesson-section" in lesson_html
            if not matched_title or not has_section:
                raise AssertionError((
                    "Authenticated student lesson fails source-title/RichV2 check",
                    code,matched_title,has_section))
            # These are actual HTML response checks; rendered Chromium
            # screenshot and correct grading must be assessed separately.
            grade1_report["lessonRows"].append({
                "ordinal":position,
                "lessonCode":code,
                "sourceSemanticSha256":m["sourceSemanticSha256"],
                "effectiveContentSha256":item["contentSha256"],
                "learnerTitleMatched":True,
                "richV2HtmlPresent":True,
                "mathVisualMarkupPresent":"rich-lesson-visual" in lesson_html,
                "authenticatedHtmlSha256":
                    hashlib.sha256(lesson_html.encode("utf-8")).hexdigest()
            })
        assert len(grade1_report["lessonRows"])==146
        Path("/tmp/g1-all146-authenticated-student-html-inventory.json").write_text(
            json.dumps(grade1_report,indent=2)+"\n",encoding="utf-8")
        print("DISPOSABLE_G1_ALL146_AUTHENTICATED_HTTPS_HTML_SCREEN_PASS",
              len(grade1_report["lessonRows"]),
              sum(x["mathVisualMarkupPresent"]
                  for x in grade1_report["lessonRows"]),flush=True)
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
    form_key = re.search(
        r'name="_idempotencyKey"\s+value="([0-9a-fA-F]{32})"',
        practice_html)
    assert practice_token and adoption and form_key, (
        key, "private Practice form lacks CSRF/adoption/unique idempotency key")
    before_practice = int(sql('SELECT count(*) FROM "PracticeAttempts";'))
    practice_fields = {
        "curriculumAdoptionId": adoption.group(1),
        "scope": "WholeCurriculum",
        "difficulty": "AtClassLevel",
        "questionCount": "1",
        "__RequestVerificationToken": practice_token.group(1),
        "_idempotencyKey": form_key.group(1),
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
        if key == "commoncore-primary":
            # A separate source lesson must independently prove generation,
            # all three L02 source-task families, persistence and grading.
            # Neither the L01 answer pass nor the L02 screenshot substitutes
            # for this exact learner assessment acceptance evidence.
            next_families = {
                "usccss.b01.g1.match_dot_cubes_expression",
                "usccss.b01.g1.complete_missing_dots",
                "usccss.b01.g1.check_off_add_within10",
            }
            l02_grading_evidence=[]
            for wrong_answers in (False, True):
                l02_fields = dict(practice_fields)
                l02_fields["lessonId"] = next_id
                l02_fields["questionCount"] = "3"  # 3 allowed lesson items, one per family
                # Every legitimate new Practice attempt must use a NEW
                # idempotency key. Reusing the same anti-forgery form token
                # as an implicit key across differing POST bodies is rejected.
                l02_fields["_idempotencyKey"] = "ci-g1-l02-" + uuid.uuid4().hex
                # Expose the exact redirect, route and HTTP failure instead
                # of treating a 404 during lesson Practice as a source error.
                assert next_id in offered_ids, (
                    "L02 is visible as a lesson but not offered for Practice",
                    next_id, offered_ids)
                try:
                    response = student.open(urllib.request.Request(
                        BASE + "/student/practice/generate",
                        data=urllib.parse.urlencode(l02_fields).encode(),
                        method="POST"), timeout=60)
                except urllib.error.HTTPError as error:
                    print("DISPOSABLE_G1_L02_PRACTICE_HTTP_FAILURE",
                          error.code, error.url,
                          "location", error.headers.get("Location"),
                          "body", error.read(650).decode("utf-8","replace"),
                          flush=True)
                    raise
                with response:
                    assert response.status == 200
                    l02_path = urllib.parse.urlparse(response.url).path.lower()
                    l02_match = re.fullmatch(
                        r"/student/practice/attempt/([0-9a-f-]{36})",
                        l02_path)
                    assert l02_match,("L02 generation failed",l02_path)
                    l02_body = response.read().decode("utf-8")
                l02_attempt_id=l02_match.group(1)
                l02_rows=sql("""
                    SELECT a."CurriculumPedagogicalLessonId"::text,
                           i."GenerationFamily",pi."Id"::text,i."CorrectAnswer"
                    FROM "PracticeAttempts" a
                    JOIN "PracticeAttemptItems" pi
                      ON pi."PracticeAttemptId"=a."Id"
                    JOIN "AssessmentItems" i ON i."Id"=pi."AssessmentItemId"
                    WHERE a."Id"='""" + l02_attempt_id + """'
                    ORDER BY pi."Order";""").splitlines()
                l02_items=[row.split("|",3) for row in l02_rows]
                assert len(l02_items)==3,("L02 three source questions missing",l02_rows)
                assert all(row[0]==next_id and row[1] in next_families
                           and row[3].strip() for row in l02_items),(
                               "L02 wrong lesson/family/answer",l02_items)
                assert {row[1] for row in l02_items}==next_families,(
                    "L02 source activity coverage incomplete",l02_items)
                csrf=re.search(
                    r'name="__RequestVerificationToken"[^>]*value="([^"]+)"',
                    l02_body)
                assert csrf,("L02 Practice CSRF token absent")
                fields=[("__RequestVerificationToken",csrf.group(1))]
                for index,(_,family,item_id,correct) in enumerate(l02_items):
                    hidden=re.search(
                        r'name="attemptItemIds\['+str(index)+
                        r'\]"[^>]*value="([0-9a-fA-F-]{36})"',
                        l02_body)
                    assert hidden and hidden.group(1)==item_id,(
                        "L02 item identity changed",index,family)
                    fields.extend([
                        (f"attemptItemIds[{index}]",item_id),
                        (f"answers[{index}]","999999" if wrong_answers else correct),
                    ])
                with student.open(urllib.request.Request(
                    BASE+"/student/practice/attempt/"+l02_attempt_id+
                    "/personal-test-submit",
                    data=urllib.parse.urlencode(fields).encode(),
                    method="POST"),timeout=60) as graded:
                    assert graded.status==200,("L02 response not submitted")
                saved=sql("""
                    SELECT CASE WHEN p."SubmittedAtUtc" IS NOT NULL THEN 1 ELSE 0 END,
                           p."Score"::text,COUNT(r."Id")::text
                    FROM "PracticeAttempts" p
                    JOIN "PracticeAttemptItems" ai ON ai."PracticeAttemptId"=p."Id"
                    LEFT JOIN "PracticeResponses" r
                      ON r."PracticeAttemptItemId"=ai."Id" AND r."IsCorrect"=true
                    WHERE p."Id"='""" + l02_attempt_id + """'
                    GROUP BY p."Id",p."SubmittedAtUtc",p."Score";""")
                submitted,score,correct_count=saved.split("|")
                wanted=0 if wrong_answers else 3
                assert submitted=="1" and float(score)==wanted and int(correct_count)==wanted,(
                    "L02 wrong answer grading/persistence",wrong_answers,saved)
                l02_grading_evidence.append({
                    "case":"wrong" if wrong_answers else "correct",
                    "submitted":submitted=="1",
                    "expectedCorrectItems":wanted,
                    "persistedCorrectItems":int(correct_count),
                    "persistedScore":float(score),
                    "familyIds":sorted(next_families)
                })
                print("DISPOSABLE_G1_U01_L02_PRACTICE_SCORING_PASS",
                      "negative" if wrong_answers else "positive",
                      ",".join(sorted(next_families)),flush=True)
            # The screenshot and positive/negative grading evidence is
            # written only after the authenticated Chromium/DB proof passes.
            # Never infer L6 PASS or academic certification from the JSON alone.
            assert set(l02_screens)=={"desktop","mobile"}
            assert set(l02_counts)=={"desktop","mobile"}
            assert l02_counts["desktop"]==l02_counts["mobile"] and l02_counts["desktop"], (
                "L02 viewport numerical visuals diverge",l02_counts)
            assert {x["case"] for x in l02_grading_evidence}=={"correct","wrong"}
            manifest=json.loads(Path(
                "artifacts/academic-us-ccss/batches/B01-lesson-manifest.json"
            ).read_text(encoding="utf-8"))
            lesson_rows=[x for x in manifest["lessons"]
                         if x["lessonCode"]==next_code]
            assert len(lesson_rows)==1
            ledger=json.loads(Path(
                "artifacts/academic-us-ccss/04-lesson-certification-records.json"
            ).read_text(encoding="utf-8"))
            certified=[x for x in ledger["records"]
                       if x["lessonCode"]==next_code]
            assert len(certified)==1
            current=certified[0]
            assert current["contentSha256"] and len(current["contentSha256"])==64
            assert current["status"]=="AUTOMATED_SCREENED"
            evidence={
                "schemaVersion":1,
                "kind":"G1_U01_L02_AUTHENTICATED_STUDENT_L6_CANDIDATE",
                "decision":"BROWSER_AND_GRADING_PROOF_ONLY_NOT_ACADEMIC_CERTIFICATION",
                "checkedOutCommit":os.environ.get("GITHUB_SHA"),
                "lessonCode":next_code,
                "sourceSemanticSha256":lesson_rows[0]["sourceSemanticSha256"],
                "effectiveContentSha256":current["contentSha256"],
                "environment":"DISPOSABLE_US_PRIMARY_HTTPS_POSTGRES",
                "screenshots":l02_screens,
                "visualDotCountsByViewport":l02_counts,
                "grading":l02_grading_evidence,
                "academicDecision":"NOT_CERTIFIED"
            }
            Path("/tmp/g1-u01-l02-student-l6-evidence.json").write_text(
                json.dumps(evidence,indent=2)+"\n",encoding="utf-8")
            print("DISPOSABLE_G1_L02_AUTHENTICATED_SHA_BOUND_L6_CANDIDATE_PASS",
                  current["contentSha256"],flush=True)

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

# Full B01 student HTTP source-identity scan in a DISPOSABLE database.
# Rebind a disposable, pre-provisioned student login to EXISTING anonymized
# grade cohorts. Keep the number of users/schools constant and restore the
# original G1 login mapping; no real student/school data is touched.
if PROGRAMME=="US-CCSS":
    import html as html_module
    source_manifest=json.loads(Path(
        "artifacts/academic-us-ccss/batches/B01-lesson-manifest.json"
    ).read_text(encoding="utf-8"))
    source_ledger=json.loads(Path(
        "artifacts/academic-us-ccss/04-lesson-certification-records.json"
    ).read_text(encoding="utf-8"))
    ledger_map={r["lessonCode"]:r for r in source_ledger["records"]}
    user_email="commoncore-primary.student.primary@edulytiks.com"
    login_id=sql("""SELECT "Id"::text FROM "AspNetUsers"
        WHERE "Email"='""" + user_email + """';""")
    assert re.fullmatch(r"[0-9a-fA-F-]{36}",login_id), (
        "Disposable US primary student login missing",login_id)
    original_profile=sql("""SELECT "Id"::text FROM "StudentProfiles"
        WHERE "UserId"='""" + login_id + """';""")
    assert re.fullmatch(r"[0-9a-fA-F-]{36}",original_profile)
    all_rows=list(grade1_report["lessonRows"])
    assert len(all_rows)==146
    expected_counts={"G2":146,"G3":143,"G4":65}
    try:
        for grade_name,number in (("G2",2),("G3",3),("G4",4)):
            cohort=[
                m for m in source_manifest["lessons"]
                if m["lessonCode"].startswith(
                    "PED:US-CCSS-MATH:"+grade_name+":")
            ]
            assert len(cohort)==expected_counts[grade_name],(
                "Missing original source cohort",grade_name,len(cohort))
            logical_level=number+1
            profile_id=sql("""
                SELECT p."Id"::text
                FROM "StudentProfiles" p
                JOIN "StudentEnrollments" e
                    ON e."StudentProfileId"=p."Id"
                JOIN "ClassGroups" c ON c."Id"=e."ClassGroupId"
                JOIN "SchoolCurriculumAdoptions" a
                    ON a."Id"=c."CurriculumAdoptionId"
                JOIN "Schools" s ON s."Id"=p."SchoolId"
                WHERE s."SchoolCode"='REHEARSAL-US-PRIMARY'
                  AND a."CurriculumLogicalLevel"=""" + str(logical_level) + """
                  AND p."UserId" IS NULL
                ORDER BY p."StudentNumber" LIMIT 1;""")
            assert re.fullmatch(r"[0-9a-fA-F-]{36}",profile_id), (
                "Exact disposable grade cohort target missing",grade_name)
            # Temporary changes are confined to this CI database. Re-authenticate
            # after changing class context so the student session uses its own
            # actual grade-specific membership/entitlements.
            unlinked=sql("""UPDATE "StudentProfiles" SET "UserId"=NULL
                WHERE "UserId"='""" + login_id + """';""")
            relinked=sql("""UPDATE "StudentProfiles"
                SET "UserId"='""" + login_id + """'
                WHERE "Id"='""" + profile_id + """'
                  AND "SchoolId"=(SELECT "Id" FROM "Schools"
                      WHERE "SchoolCode"='REHEARSAL-US-PRIMARY')
                  AND "UserId" IS NULL;""")
            assert unlinked=="UPDATE 1" and relinked=="UPDATE 1",(
                "Disposable student class relink failed",
                grade_name,unlinked,relinked)
            grade_student=connect("commoncore-primary","Student")
            ids=sql("""SELECT "Code","Id"::text FROM "CurriculumPedagogicalLessons"
                WHERE "Code" LIKE 'PED:US-CCSS-MATH:""" + grade_name + """:%'
                ORDER BY "Code";""").splitlines()
            grade_ids={}
            for line in ids:
                code,sep,lesson_id=line.partition("|")
                assert sep and code not in grade_ids,(
                    "Duplicate grade lesson code",grade_name,line)
                grade_ids[code]=lesson_id
            assert len(grade_ids)==expected_counts[grade_name],(
                "Unexpected grade lesson identities",grade_name,len(grade_ids))
            for m in cohort:
                code=m["lessonCode"]
                assert code in grade_ids and code in ledger_map,(
                    "Missing original lesson code in disposable DB",code)
                cert=ledger_map[code]
                assert cert["academicDecision"]=="NOT_CERTIFIED"
                assert cert["contentSha256"] and len(cert["contentSha256"])==64
                source_html=html_module.unescape(get_page(
                    grade_student,
                    "/student/learning/lesson/"+grade_ids[code]))
                title_present=m["title"].casefold() in source_html.casefold()
                rich_present="rich-lesson-section" in source_html
                assert title_present and rich_present,(
                    "Authenticated grade-specific Rich V2 page mismatch",
                    grade_name,code,title_present,rich_present)
                all_rows.append({
                    "ordinal":m["ordinal"],
                    "lessonCode":code,
                    "sourceSemanticSha256":m["sourceSemanticSha256"],
                    "effectiveContentSha256":cert["contentSha256"],
                    "learnerTitleMatched":True,
                    "richV2HtmlPresent":True,
                    "mathVisualMarkupPresent":"rich-lesson-visual" in source_html,
                    "authenticatedHtmlSha256":
                        hashlib.sha256(source_html.encode("utf-8")).hexdigest(),
                    "authenticatedCohort":grade_name
                })
            print("DISPOSABLE_B01_AUTHENTICATED_GRADE_HTTP_PASS",
                  grade_name,len(cohort),flush=True)
    finally:
        # NEVER leave even a disposable student's fixture enrolled in another
        # grade when subsequent product regression checks might reuse the DB.
        sql("""UPDATE "StudentProfiles" SET "UserId"=NULL
            WHERE "UserId"='""" + login_id + """';""")
        restored=sql("""UPDATE "StudentProfiles" SET "UserId"='""" +
            login_id + """' WHERE "Id"='""" + original_profile + """'
            AND "UserId" IS NULL;""")
        assert restored=="UPDATE 1","Grade 1 original test account not restored"
    assert len(all_rows)==500 and len({r["lessonCode"] for r in all_rows})==500
    all500={
        "schemaVersion":1,
        "kind":"B01_ALL500_GRADE_SPECIFIC_AUTHENTICATED_STUDENT_HTTPS_INVENTORY",
        "batchId":"B01",
        "checkedOutCommit":os.environ["GITHUB_SHA"],
        "sourceProgramme":"US-CCSS",
        "schoolCode":"REHEARSAL-US-PRIMARY",
        "decision":"500_STUDENT_HTTP_SOURCE_AND_RICHV2_SCREEN_NOT_ACADEMIC_L6_PASS",
        "lessonRows":all_rows
    }
    Path("/tmp/b01-all500-authenticated-student-https-inventory.json").write_text(
        json.dumps(all500,indent=2)+"\n",encoding="utf-8")
    print("DISPOSABLE_B01_ALL500_GRADE_SPECIFIC_AUTHENTICATED_HTTPS_PASS",
          len(all_rows),
          sum(r["mathVisualMarkupPresent"] for r in all_rows),flush=True)

print("DISPOSABLE_TWO_SCHOOL_FULL_DATA_HTTP_REHEARSAL_PASS")
