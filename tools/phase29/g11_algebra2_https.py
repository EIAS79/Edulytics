#!/usr/bin/env python3
"""CI-only G11 authenticated HTTPS learner Rich V2 route probe, isolated database."""
import html,http.cookiejar,json,os,re,ssl,subprocess,urllib.parse,urllib.request,urllib.error
assert os.environ.get("GITHUB_ACTIONS")=="true"
assert os.environ.get("EDULYTICS_CI_REHEARSAL")=="true"
assert os.environ.get("EDULYTICS_CI_G11_ALG2")=="true"
ROOT="https://localhost:5052"
CTX=ssl.create_default_context(cafile="/tmp/phase-uat-cert.pem")
PREFIX="PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:"
PASSWORD=os.environ["Edulytics__MeetingDemo__Password"]
COUNT=[11,26,19,18,11,19,16]
def sql(statement):
 return subprocess.check_output(["psql","-h","127.0.0.1","-U","postgres","-d","edulytics_clean_ci","-A","-t","-F","|","-c",statement],text=True).strip()
def login(email,role):
 jar=http.cookiejar.CookieJar()
 browser=urllib.request.build_opener(urllib.request.HTTPCookieProcessor(jar),urllib.request.HTTPSHandler(context=CTX))
 with browser.open(ROOT+"/account/login?culture=en",timeout=40) as response:
  page=response.read().decode()
 match=re.search(r'name="__RequestVerificationToken"[^>]*value="([^"]+)"',page)
 assert match, "missing anti-forgery token"
 data=urllib.parse.urlencode({"Email":email,"Password":PASSWORD,"AccountType":role,"__RequestVerificationToken":match.group(1)}).encode()
 with browser.open(urllib.request.Request(ROOT+"/account/login?culture=en",data=data,method="POST"),timeout=40) as response:
  assert response.status==200 and "/account/login" not in response.url
 assert any(cookie.secure for cookie in jar)
 return browser
def page(browser,url):
 with browser.open(ROOT+url,timeout=35) as response:
  assert response.status==200
  assert urllib.parse.urlparse(response.url).path.lower()!="/account/login"
  return html.unescape(response.read().decode())
rows=sql('SELECT "Code" || \'|\' || "Id"::text FROM "CurriculumPedagogicalLessons" WHERE "Code" LIKE \'PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:%\' ORDER BY "Code";').splitlines()
assert len(rows)==120,("missing or extra Algebra 2 canonical lessons",len(rows))
ids=dict(row.split("|",1) for row in rows)
assert len(ids)==120
for u,num in enumerate(COUNT,1):
 for l in range(1,num+1):
  code=PREFIX+f"U{u:02d}:L{l:02d}"
  assert code in ids,("wrong lesson sequence",code)
adopt=sql('SELECT "CurriculumLevelKey" || \'|\' || "CurriculumPathway" FROM "SchoolCurriculumAdoptions" a JOIN "Schools" s ON s."Id"=a."SchoolId" WHERE s."SchoolCode"=\'REHEARSAL-US-SECONDARY\' AND "CurriculumLogicalLevel"=11;')
assert "COURSE-ALG2|Traditional" in adopt,("not Grade 11 Algebra2 school adoption",adopt)
student=login("commoncore-secondary.student.primary@edulytiks.com","Student")
other=login("commoncore-primary.student.primary@edulytiks.com","Student")
page(student,"/student/dashboard")
page(student,"/student/learning")
data={"candidateSha":os.environ.get("G11_EXPECTED_CANDIDATE_SHA"),"schoolAdoption":adopt,"expected":120,"checked":0,"passed":0,"failures":[],"lessons":[],"source":"isolated HTTPS PostgreSQL18 synthetic schools"}
for code,identifier in ids.items():
 try:
  raw=page(student,"/student/learning/lesson/"+identifier)
  assert 'rich-lesson-section' in raw and 'rich-lesson-' in raw,("missing rendered RichV2",code)
  assert len(raw)>2000,("empty lesson",code)
  data["passed"]+=1
  data["lessons"].append({"lessonCode":code,"actualHtmlLength":len(raw),"status":"AUTHENTICATED_RICHV2_RENDER"})
 except Exception as exc:
  data["failures"].append({"lessonCode":code,"error":str(exc)[:240]})
 data["checked"]+=1
try:
 page(other,"/student/learning/lesson/"+ids[PREFIX+"U01:L01"])
 raise AssertionError("cross-school Grade11 lesson visible to unauthorised learner")
except urllib.error.HTTPError as error:
 assert error.code in (401,403,404),("unexpected RBAC response",error.code)
except AssertionError as error:
 if str(error).startswith("cross-school"):raise
data["crossSchoolDenial"]="PASS"
data["notVerified"]=["Practice generation/feedback","Teacher grading persistence","all source drawings, media, and element parity","visual accessibility and keyboard","all question family mathematical accuracy"]
data["technicalRenderingStatus"]="PASS" if data["passed"]==120 else "FAIL"
with open("/tmp/g11-algebra2-120-authenticated-render.json","w",encoding="utf8") as f:json.dump(data,f,indent=2)
print("GRADE11_HTTPS_AUTHENTICATED_RENDER",data["passed"],"/120","failures",len(data["failures"]),flush=True)
assert data["passed"]==120,data["failures"][:5]
