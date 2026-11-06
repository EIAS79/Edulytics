#!/usr/bin/env python3
"""Fail-closed G10 Geometry synthetic HTTPS student render probe; CI-only."""
import html
import http.cookiejar
import json
import os
import re
import ssl
import subprocess
import urllib.parse
import urllib.request
import urllib.error

assert os.environ.get("GITHUB_ACTIONS") == "true"
assert os.environ.get("EDULYTICS_CI_REHEARSAL") == "true"
assert os.environ.get("EDULYTICS_CI_G10_GEOMETRY") == "true"
BASE = "https://localhost:5052"
CTX = ssl.create_default_context(cafile="/tmp/phase-uat-cert.pem")
PREFIX = "PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:"
PASSWORD = os.environ["Edulytics__MeetingDemo__Password"]

def sql(statement):
    return subprocess.check_output(["psql", "-h", "127.0.0.1", "-U", "postgres",
        "-d", "edulytics_clean_ci", "-A", "-t", "-F", "|", "-c", statement], text=True).strip()

def connect(username, role):
    jar = http.cookiejar.CookieJar()
    browser = urllib.request.build_opener(urllib.request.HTTPCookieProcessor(jar),
                                          urllib.request.HTTPSHandler(context=CTX))
    with browser.open(BASE + "/account/login?culture=en", timeout=30) as response:
        raw = response.read().decode()
    token = re.search(r'name="__RequestVerificationToken"[^>]*value="([^"]+)"', raw)
    assert token, "missing antiforgery token"
    form = urllib.parse.urlencode({"Email":username,"Password":PASSWORD,
              "AccountType":role,"__RequestVerificationToken":token.group(1)}).encode()
    with browser.open(urllib.request.Request(BASE+"/account/login?culture=en",
                 data=form,method="POST"), timeout=40) as response:
        assert response.status == 200
        assert not urllib.parse.urlparse(response.url).path.lower().startswith("/account/login")
    assert any(c.secure for c in jar), "no secure cookie"
    return browser

def page(browser, url):
    with browser.open(BASE+url,timeout=40) as response:
        assert response.status == 200
        assert urllib.parse.urlparse(response.url).path.lower()!= "/account/login"
        return html.unescape(response.read().decode())

rows = sql('SELECT "Code" || \'|\' || "Id"::text FROM "CurriculumPedagogicalLessons" '
           'WHERE "Code" LIKE \'PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:%\' ORDER BY "Code";').splitlines()
assert len(rows)==124, ("expected 124 canonical Geometry rows",len(rows))
ids = dict(x.split("|",1) for x in rows)
assert len(ids)==124
units=[22,15,16,11,18,17,14,11]
for u,num in enumerate(units,1):
    for l in range(1,num+1):
        code=PREFIX+f"U{u:02d}:L{l:02d}"
        assert code in ids,("missing",code)

adoption=sql('SELECT a."CurriculumLevelKey" || \'|\' || a."CurriculumPathway" || '
    '\'|\' || a."CurriculumLevelLabel" FROM "SchoolCurriculumAdoptions" a '
    'JOIN "Schools" s ON s."Id"=a."SchoolId" WHERE '
    's."SchoolCode"=\'REHEARSAL-US-SECONDARY\' AND '
    'a."CurriculumLogicalLevel"=10;')
assert adoption=="US-CCSS-MATH:L10:TRADITIONAL:COURSE-GEO|Traditional|Geometry",adoption
print("G10_GEOMETRY_DISPOSABLE_COURSE_ADOPTION_PASS",adoption,flush=True)

student=connect("commoncore-secondary.student.primary@edulytiks.com","Student")
primary=connect("commoncore-primary.student.primary@edulytiks.com","Student")
page(student,"/student/dashboard")
page(student,"/student/learning")
result={"candidateSha":os.environ.get("G10_EXPECTED_CANDIDATE_SHA"),"source":"disposable-https-postgres18",
        "expected":124,"checked":0,"passed":0,"failures":[],"lessons":[]}
for code,identifier in ids.items():
    try:
        raw=page(student,"/student/learning/lesson/"+identifier)
        assert "rich-lesson-section" in raw,("no rendered Rich V2 section",code)
        assert "rich-lesson-" in raw,("no Rich V2 marker",code)
        result["passed"]+=1
        result["lessons"].append({"lessonCode":code,"status":"AUTHENTICATED_RENDERED",
                                  "actualBodyLength":len(raw)})
    except Exception as exc:
        result["failures"].append({"lessonCode":code,"error":str(exc)[:220]})
    result["checked"]+=1

# Denial control: primary school must not read the secondary geometry course.
probe_id=ids[PREFIX+"U01:L01"]
try:
    _=page(primary,"/student/learning/lesson/"+probe_id)
    raise AssertionError("cross-school geometry lesson was visible to a primary learner")
except urllib.error.HTTPError as exc:
    assert exc.code in (401,403,404),(exc.code,"unexpected denial response")
except AssertionError as exc:
    if str(exc).startswith("cross-school"):raise
    # Login redirect or another access gate is a denial, not an allowed page.

result["crossSchoolDenial"]="PASS"

# Verify the newly wired first-party teacher rubric is available to each
# learner for exactly the assigned Geometry course; scoring persistence is a
# separate acceptance gate, never inferred from page rendering.
result["rubricEntryPagesRendered"]=0
for lesson_code,lesson_id in ids.items():
    rubric_html=page(student,"/student/g10/geometry/rubric?lessonId="+lesson_id)
    assert "Submit for teacher review" in rubric_html,lesson_code
    result["rubricEntryPagesRendered"]+=1
print("G10_GEOMETRY_RUBRIC_STUDENT_ENTRY_PAGES",result["rubricEntryPagesRendered"],"/124",flush=True)
result["technicalStatus"]="PASS" if result["passed"]==124 else "FAIL"
result["notProved"]=["all mathematical correctness","Practice generating/grading and persistence",
     "teacher-scored submission review","graph/visual/accessibility","source-element parity"]
with open("/tmp/g10-geometry-124-authenticated-render.json","w",encoding="utf-8") as output:
    json.dump(result,output,indent=2)
print("G10_GEOMETRY_AUTHENTICATED_RENDER",result["passed"],"/",result["checked"],
      "failures",len(result["failures"]),flush=True)
assert result["passed"]==124, result["failures"][:5]
