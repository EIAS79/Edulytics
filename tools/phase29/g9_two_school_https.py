#!/usr/bin/env python3
"""Grade 9 disposable two-school HTTPS + Postgres Stage D candidate probe.

No production DB, no real students, no academic approval inference.
"""
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
import urllib.error

BASE = "https://localhost:5052"
CTX = ssl.create_default_context(cafile="/tmp/phase-uat-cert.pem")
PASSWORD = os.environ["Edulytics__MeetingDemo__Password"]
ROOT = Path(__file__).resolve().parents[2]
PREFIX = "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:"
COUNTS = [16, 26, 10, 18, 21, 17, 24]
REGISTRY = {
    row["id"]
    for row in json.loads((ROOT / "src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json").read_text(encoding="utf-8"))["families"]
}

def sql(query):
    return subprocess.check_output(
        ["psql", "-h", "127.0.0.1", "-U", "postgres", "-d",
         "edulytics_clean_ci", "-A", "-t", "-F", "|", "-c", query],
        text=True,
    ).strip()

def connect(school, role):
    jar = http.cookiejar.CookieJar()
    opener = urllib.request.build_opener(
        urllib.request.HTTPCookieProcessor(jar),
        urllib.request.HTTPSHandler(context=CTX))
    with opener.open(BASE + "/account/login?culture=en", timeout=30) as resp:
        document = resp.read().decode("utf-8")
    csrf = re.search(r'name="__RequestVerificationToken"[^>]*value="([^"]+)"', document)
    assert csrf, (school, role, "login csrf missing")
    if role == "Student":
        email = school + ".student.primary@edulytiks.com"
    elif role == "Teacher":
        email = school + ".teacher.primary@edulytiks.com"
    else:
        email = school + ".admin@edulytiks.com"
    data = urllib.parse.urlencode({
        "Email": email, "Password": PASSWORD, "AccountType": role,
        "__RequestVerificationToken": csrf.group(1),
    }).encode()
    with opener.open(urllib.request.Request(
        BASE + "/account/login?culture=en", data=data, method="POST"
    ), timeout=30) as resp:
        assert resp.status == 200 and urllib.parse.urlparse(resp.url).path.lower() != "/account/login", (email, resp.url)
    assert any(c.secure for c in jar), (school, role, "secure cookie absent")
    return opener

def get_page(opener, url):
    with opener.open(BASE + url, timeout=40) as response:
        assert response.status == 200 and urllib.parse.urlparse(response.url).path.lower() != "/account/login", url
        return response.read().decode("utf-8")

def token(page):
    match = re.search(r'name="__RequestVerificationToken"[^>]*value="([^"]+)"', page)
    assert match, "CSRF missing"
    return match.group(1)

def submit(opener, path, data):
    with opener.open(urllib.request.Request(
        BASE + path, data=urllib.parse.urlencode(data).encode(),
        method="POST"), timeout=75) as response:
        assert response.status == 200, (path, response.status)
        return urllib.parse.urlparse(response.url).path.lower(), response.read().decode("utf-8")

def run_practice(student, adoption_id, code, lesson_id, response_is_correct):
    form = get_page(student, "/student/practice")
    params = {
        "curriculumAdoptionId": adoption_id,
        "scope": "Lesson", "lessonId": lesson_id,
        "difficulty": "AtClassLevel", "questionCount": "1",
        "__RequestVerificationToken": token(form),
    }
    redirected, rendered = submit(student, "/student/practice/generate", params)
    match = re.fullmatch(r"/student/practice/attempt/([0-9a-f-]{36})", redirected)
    assert match, (code, "no authenticated attempt", redirected)
    attempt_id = match.group(1)
    evidence = sql("""
        SELECT a."CurriculumPedagogicalLessonId"::text,
               i."GenerationFamily", pi."Id"::text, i."CorrectAnswer"
        FROM "PracticeAttempts" a
        JOIN "PracticeAttemptItems" pi ON pi."PracticeAttemptId"=a."Id"
        JOIN "AssessmentItems" i ON i."Id"=pi."AssessmentItemId"
        WHERE a."Id"='""" + attempt_id + """'
        ORDER BY pi."Order";""").splitlines()
    assert len(evidence) == 1, (code, "not exactly one item", evidence)
    lesson_actual, family, item_id, correct = evidence[0].split("|", 3)
    assert lesson_actual == lesson_id and family in REGISTRY and correct, (code, family, "bad persisted family", evidence)
    assert "practice" in rendered.lower() and "CorrectAnswer" not in rendered, (code, "missing or leaky practice HTML")
    item_match = re.search(r'name="attemptItemIds\[0\]"[^>]*value="([0-9a-fA-F-]{36})"', rendered)
    assert item_match and item_match.group(1) == item_id, (code, "wrong item in HTML")
    response = correct if response_is_correct else "NOT_VALID_ANSWER___!"
    _, after = submit(student, redirected + "/personal-test-submit", {
        "__RequestVerificationToken": token(rendered),
        "attemptItemIds[0]": item_id,
        "answers[0]": response,
    })
    result = sql("""
        SELECT CASE WHEN a."SubmittedAtUtc" IS NOT NULL THEN 1 ELSE 0 END,
               a."Score"::text, COUNT(CASE WHEN r."IsCorrect" THEN 1 END)::text
        FROM "PracticeAttempts" a
        JOIN "PracticeAttemptItems" ai ON ai."PracticeAttemptId"=a."Id"
        LEFT JOIN "PracticeResponses" r ON r."PracticeAttemptItemId"=ai."Id"
        WHERE a."Id"='""" + attempt_id + """'
        GROUP BY a."Id",a."SubmittedAtUtc",a."Score";""")
    submitted, score, yes = result.split("|")
    expected_score = 1.0 if response_is_correct else 0.0
    assert submitted == "1" and float(score) == expected_score and int(yes) == int(response_is_correct), (
        code, family, "server grade/persistence mismatch", result)
    return {"attemptId": attempt_id, "family": family, "status": "CORRECT_PERSISTED" if response_is_correct else "MALFORMED_REJECTED_PERSISTED"}

def verify_browser_visuals(student, expected, lesson_ids):
    """Check all 132 rendered Grade 9 documents with real Chromium/CSS, plus
    representative desktop/mobile screenshots. No screenshot means no visual pass.
    """
    from playwright.sync_api import sync_playwright
    jar = next(handler.cookiejar for handler in student.handlers
               if isinstance(handler, urllib.request.HTTPCookieProcessor))
    cookies = [{"name": cookie.name, "value": cookie.value, "url": BASE}
               for cookie in jar if cookie.secure]
    assert cookies, "No authenticated Grade9 browser session cookies"
    screenshots = []
    with sync_playwright() as playwright:
        browser = playwright.chromium.launch(headless=True,args=["--no-sandbox"])
        try:
            desktop=browser.new_context(ignore_https_errors=True,
                viewport={"width":1440,"height":900})
            mobile=browser.new_context(ignore_https_errors=True,
                viewport={"width":390,"height":844})
            for context in (desktop,mobile):
                context.add_cookies(cookies)
            desktop_page=desktop.new_page()
            mobile_page=mobile.new_page()
            style=desktop_page.request.get(BASE+"/css/site.css",timeout=30000)
            assert style.status==200 and "text/css" in style.headers.get("content-type",""), (
                "CSS missing in published Grade9 application",style.status)
            for ordinal,code in enumerate(expected,1):
                lesson_id=lesson_ids[code]
                url=BASE+"/student/learning/lesson/"+lesson_id
                response=desktop_page.goto(url,wait_until="domcontentloaded",timeout=40000)
                assert response is not None and response.status==200,(
                    code,"Chromium desktop HTTP failure", response.status if response else None)
                assert desktop_page.locator("section.rich-lesson-section").count()>=3,(
                    code,"Grade9 Rich V2 sections missing in actual browser")
                assert desktop_page.locator("section.rich-lesson-section").first.is_visible(),(
                    code,"Grade9 Rich V2 not visible")
                assert desktop_page.evaluate(
                    "() => parseFloat(getComputedStyle(document.body).fontSize)>0"),(
                    code,"No CSS computed body font")
                if ordinal in (1,17,43,53,71,92,109):
                    name=f"/tmp/g9-u{int(code.split('U')[-1].split(':')[0]):02d}-desktop.png"
                    desktop_page.screenshot(path=name,full_page=False)
                    screenshots.append(name)
                    mobile_response=mobile_page.goto(url,wait_until="domcontentloaded",timeout=40000)
                    assert mobile_response is not None and mobile_response.status==200,(
                        code,"Chromium mobile HTTP failure")
                    assert mobile_page.locator("section.rich-lesson-section").first.is_visible(),(
                        code,"Grade9 mobile content not visible")
                    mobile_name=name.replace("-desktop.png","-mobile.png")
                    mobile_page.screenshot(path=mobile_name,full_page=False)
                    screenshots.append(mobile_name)
                if ordinal % 22==0:
                    print("G9_CHROMIUM_AUTHENTICATED_RENDER",ordinal,"/132",flush=True)
            desktop.close()
            mobile.close()
        finally:
            browser.close()
    assert len(screenshots)==14,screenshots
    return {"chromiumDesktopLessonsRendered":132,"mobileLessonsRendered":7,
            "screenshots":screenshots,"cssVerified":True}

def main():
    assert sql('SELECT count(*) FROM "Schools";') == "2", "not a two-school isolated fixture"
    secondary = connect("commoncore-secondary", "Student")
    primary = connect("commoncore-primary", "Student")
    teacher = connect("commoncore-secondary", "Teacher")
    get_page(teacher, "/school/assessments")
    get_page(secondary, "/student/learning")
    get_page(primary, "/student/learning")
    adoption = sql("""
      SELECT a."Id"::text FROM "SchoolCurriculumAdoptions" a
      JOIN "Schools" s ON s."Id"=a."SchoolId"
      WHERE s."SchoolCode"='REHEARSAL-US-SECONDARY'
        AND a."CurriculumLevelKey"='US-CCSS-MATH:L11:TRADITIONAL:COURSE-ALG1';""").splitlines()
    assert len(adoption)==1, ("Grade9 ALG1 school adoption absent", adoption)
    rows = sql("""
      SELECT "Id"::text || '|' || "Code"
      FROM "CurriculumPedagogicalLessons"
      WHERE "Code" LIKE 'PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:%'
      ORDER BY "Code";""").splitlines()
    assert len(rows)==132, ("DB lesson inventory should be 132",len(rows))
    found={code:identifier for identifier,code in (r.split("|",1) for r in rows)}
    expected=[PREFIX+f"U{u:02d}:L{n:02d}" for u,count in enumerate(COUNTS,1) for n in range(1,count+1)]
    assert len(found)==132 and set(found)==set(expected), ("missing or duplicated lesson codes",set(expected)-set(found))
    evidence=[]
    for ordinal, code in enumerate(expected,1):
        ident=found[code]
        page=html.unescape(get_page(secondary, "/student/learning/lesson/"+ident))
        assert "rich-lesson-section" in page, (code, "Rich V2 not rendered in authenticated student route")
        assert "im.kendallhunt.com" not in page.lower(), (code, "source URL exposed to learner")
        correct=run_practice(secondary,adoption[0],code,ident,True)
        malformed=run_practice(secondary,adoption[0],code,ident,False)
        evidence.append({"ordinal":ordinal,"lessonCode":code,"lessonId":ident,
          "contentRenderedAuthenticated":True,
          "correct":correct,"malformed":malformed,
          "teacherSessionAuthorized":True})
        if ordinal%11==0: print("G9_HTTPS_BATCH",ordinal,"/132",flush=True)
    visual=verify_browser_visuals(secondary,expected,found)
    denied=0
    for code in expected:
        target="/student/learning/lesson/"+found[code]
        try:
            with primary.open(BASE+target,timeout=30) as response:
                redirected_to=urllib.parse.urlparse(response.url).path.lower()
                assert redirected_to!=target, ("cross-school Grade9 lesson returned 200",code)
        except urllib.error.HTTPError as error:
            assert error.code in (401,403,404), ("unexpected cross-school HTTP response",code,error.code)
        denied+=1
    assert denied==132
    out={"course":"US-CCSS-MATH:HS:TRAD:ALG1:IM-1E",
        "candidateSha":os.environ["G9_CANDIDATE_SHA"],
        "runId":os.environ["GITHUB_RUN_ID"],
        "testEnvironment":"Disposable two-school PostgreSQL + authenticated self-signed HTTPS",
        "lessonsChecked":len(evidence),
        "correctGradesPersisted":sum(x["correct"]["status"]=="CORRECT_PERSISTED" for x in evidence),
        "malformedGradesRejectedPersisted":sum(x["malformed"]["status"]=="MALFORMED_REJECTED_PERSISTED" for x in evidence),
        "renderedAuthenticated":sum(x["contentRenderedAuthenticated"] for x in evidence),
        "crossSchoolLessonDenied":True,
        "crossSchoolLessonsDenied":denied,
        "browserVisual":visual,
        "scopeNote":"Technical student/teacher authenticated route and exact grading checks; not licensed source parity or authoring review",
        "lessons":evidence}
    out_path=Path("/tmp/g9-132-https-practice-evidence.json")
    out_path.write_text(json.dumps(out,indent=2)+"\n",encoding="utf-8")
    assert out["correctGradesPersisted"]==132 and out["malformedGradesRejectedPersisted"]==132
    print("GRADE9_132_ALL_AUTHENTICATED_HTTPS_AND_POSTGRES_PASSED",flush=True)

if __name__=="__main__":
    main()
