#!/usr/bin/env python3
"""CI-only 120-lesson teacher-scored Algebra 2 rubric integration: disposable database.
Tests access control, anti-forgery, one submission per lesson, teacher review and
stored scores. It NEVER certifies that a synthetic answer is academically correct.
"""
import html,http.cookiejar,json,os,re,ssl,subprocess,urllib.parse,urllib.request,urllib.error
from pathlib import Path

assert os.environ.get("GITHUB_ACTIONS") == "true"
assert os.environ.get("EDULYTICS_CI_REHEARSAL") == "true"
assert os.environ.get("EDULYTICS_CI_G11_ALG2") == "true"
ROOT="https://localhost:5052"
CTX=ssl.create_default_context(cafile="/tmp/phase-uat-cert.pem")
PASSWORD=os.environ["Edulytics__MeetingDemo__Password"]
PREFIX="PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:"
STUDENT_URL="/student/g11/algebra2/rubric"
TEACHER_URL="/school/g11/algebra2/rubric"

def sql(statement):
    return subprocess.check_output(
        ["psql","-h","127.0.0.1","-U","postgres","-d","edulytics_clean_ci",
         "-A","-t","-F","|","-c",statement],text=True).strip()

def csrf(page):
    match=re.search(r'name="__RequestVerificationToken"[^>]*value="([^"]+)"',page)
    assert match,"Missing anti-forgery token"
    return match.group(1)

def login(email,role):
    jar=http.cookiejar.CookieJar()
    browser=urllib.request.build_opener(
        urllib.request.HTTPCookieProcessor(jar),urllib.request.HTTPSHandler(context=CTX))
    with browser.open(ROOT+"/account/login?culture=en",timeout=30) as response:
        page=response.read().decode()
    form=urllib.parse.urlencode({
        "Email":email,"Password":PASSWORD,"AccountType":role,
        "__RequestVerificationToken":csrf(page)}).encode()
    with browser.open(urllib.request.Request(
        ROOT+"/account/login?culture=en",data=form,method="POST"),timeout=40) as response:
        assert response.status==200 and "/account/login" not in response.url
    assert any(cookie.secure for cookie in jar)
    return browser

def get(browser,path):
    with browser.open(ROOT+path,timeout=35) as response:
        assert response.status==200
        assert urllib.parse.urlparse(response.url).path.lower()!="/account/login"
        return html.unescape(response.read().decode())

def post(browser,path,data):
    form=urllib.parse.urlencode(data).encode()
    with browser.open(urllib.request.Request(
        ROOT+path,data=form,method="POST"),timeout=35) as response:
        assert response.status==200
        return html.unescape(response.read().decode())

rows=sql('SELECT "Code" || \'|\' || "Id"::text FROM "CurriculumPedagogicalLessons" '
         'WHERE "Code" LIKE \'PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:%\' ORDER BY "Code"').splitlines()
assert len(rows)==120
ids=dict(x.split("|",1) for x in rows)
assert len(ids)==120 and all(x.startswith(PREFIX) for x in ids)
student=login("commoncore-secondary.student.primary@edulytiks.com","Student")
teacher=login("commoncore-secondary.teacher.secondary@edulytiks.com","Teacher")
other=login("commoncore-primary.teacher.primary@edulytiks.com","Teacher")
report={"course":"G11 Algebra 2","candidateSha":os.environ.get("G11_EXPECTED_CANDIDATE_SHA"),
        "studentSubmitted":False,"studentsSubmittedCount":0,
        "teacherAwardedScore":3,"teacherScorePersisted":False,
        "teacherScoresPersistedCount":0,"studentReadBack":False,
        "studentReadBackCount":0,"wrongSchoolTeacherDenied":False,
        "academicCorrectnessVerified":False,"all120Verified":False,
        "gradedLessonCodes":[],"technicalStatus":"IN_PROGRESS"}

# The same authenticated route must submit on every real lesson id.
for code,lesson_id in ids.items():
    entry=get(student,STUDENT_URL+"?lessonId="+lesson_id)
    assert "Submit for teacher review" in entry,code
    response=(f"CI synthetic example for {code}: this answer is a test of the "
              "student submission and authorized teacher-review workflow, not an academic assertion.")
    rendered=post(student,STUDENT_URL+"/submit",{
        "lessonId":lesson_id,"explanation":response,
        "__RequestVerificationToken":csrf(entry)})
    assert "Teacher review pending" in rendered,code
    report["studentsSubmittedCount"]+=1
report["studentSubmitted"]=report["studentsSubmittedCount"]==120
print("G11_TEACHER_ROUTE_120_STUDENT_SUBMISSIONS",report["studentsSubmittedCount"],flush=True)

# Attempt a duplicate answer on the first lesson: never overwrite original.
first_id=next(iter(ids.values()))
first_view=get(student,STUDENT_URL+"?lessonId="+first_id)
try:
    post(student,STUDENT_URL+"/submit",{
        "lessonId":first_id,"explanation":"CI deliberately duplicate submission must never overwrite original response.",
        "__RequestVerificationToken":csrf(first_view)})
    raise AssertionError("Duplicate lesson submission was incorrectly accepted")
except urllib.error.HTTPError as error:
    assert error.code==409,error.code
report["duplicateWriteDenied"]=True

queue=get(teacher,TEACHER_URL)
review_ids=re.findall(r'/school/g11/algebra2/rubric/([0-9a-fA-F-]{36})',queue)
assert len(review_ids)==120,("teacher must see 120 queued lesson submissions",len(review_ids))
assert len(set(review_ids))==120
try:
    get(other,TEACHER_URL+"/"+review_ids[0])
    raise AssertionError("Cross-school teacher accessed protected review")
except urllib.error.HTTPError as error:
    assert error.code in (401,403,404),error.code
report["wrongSchoolTeacherDenied"]=True

# Review the 120 distinct submissions and confirm persistence.
for review_id in review_ids:
    review=get(teacher,TEACHER_URL+"/"+review_id)
    assert "Save teacher assessment" in review,review_id
    result=post(teacher,TEACHER_URL+"/"+review_id,[
        ("marks","1"),("marks","1"),("marks","0"),("marks","1"),
        ("teacherFeedback","CI-only synthetic scoring persistence check; academic mathematical correctness is NOT certified."),
        ("__RequestVerificationToken",csrf(review))
    ])
    assert "Teacher-awarded score: 3 / 4" in result,review_id
    report["teacherScoresPersistedCount"]+=1
report["teacherScorePersisted"]=report["teacherScoresPersistedCount"]==120
print("G11_TEACHER_ROUTE_120_TEACHER_REVIEWS",report["teacherScoresPersistedCount"],flush=True)

for code,lesson_id in ids.items():
    state=get(student,STUDENT_URL+"?lessonId="+lesson_id)
    assert "Teacher score: 3 / 4" in state,code
    report["studentReadBackCount"]+=1
    report["gradedLessonCodes"].append(code)
report["studentReadBack"]=report["studentReadBackCount"]==120
assert sql('SELECT count(*) FROM "Grade11Algebra2TeacherRubrics"')=="120"
assert sql('SELECT count(*) FROM "Grade11Algebra2TeacherRubrics" WHERE "TeacherScore"=3 AND "ReviewedAtUtc" IS NOT NULL;')=="120"
report["databaseRowsVerified"]=120
report["lessonCode"]=PREFIX+"U01:L01"
report["technicalStatus"]="PASS"
with Path("/tmp/g11-one-teacher-review-https.json").open("w",encoding="utf8") as out:
    json.dump(report,out,indent=2)
print("G11_ALL_120_TEACHER_REVIEW_SUBMISSIONS_PERSISTED_TENANT_DENIED",flush=True)
