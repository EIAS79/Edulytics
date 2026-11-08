import html
import json
import re
import urllib.error
import urllib.parse
import urllib.request

def all_other_g12_teacher_rubrics(student,teacher,expected,one,base,
    get_page,extract_csrf,sql):
    """Real disposable authenticated student/teacher HTTPS and SQL for 72
    first-party lesson exercise submissions. Teacher score is zero for the
    deliberately incomplete synthetic response, not an academic assessment.
    A verified rubric endpoint is not a claim that all source exercises pass."""
    from pathlib import Path
    bank=json.loads(Path(
        "src/Edulytics.Web/Curriculum/G12/grade12-firstparty-teacher-tasks.json"
    ).read_text(encoding="utf8"))
    lessons={x["LessonCode"]:x["Tasks"] for x in bank["Lessons"]}
    if len(lessons)!=72 or len(expected)!=73:
        raise AssertionError("Expected all 72 additional Grade12 teacher banks")
    results=[]
    for ordinal,(code,lesson_id) in enumerate(expected[1:],2):
        entries=lessons.get(code,[])
        if not entries:
            raise AssertionError(f"G12 teacher source bank missing {code}")
        learner_page=get_page(student,"/student/learning/lesson/"+lesson_id)
        if "first-party teacher-reviewed source exercises" not in learner_page.lower():
            raise AssertionError(f"G12_LEARNER_RUBRIC_DISCOVERY_ABSENT ordinal={ordinal}")
        for exercise_task in entries:
            number=int(exercise_task["Number"])
            if not re.search(r"exerciseNumber="+str(number)+r"(?![0-9])",learner_page):
                raise AssertionError(f"G12_TEACHER_TASK_NAV_MISSING ordinal={ordinal} number={number}")
        # Review P2: the U07:L05 authored bank has exercise 106. The
        # actual disposable PostgreSQL CHECK must accept it, not only 1..105.
        if code.endswith(":U07:L05"):
            exercise=max(int(x["Number"]) for x in entries)
            if exercise!=106:
                raise AssertionError("G12 high exercise number 106 vanished")
        else:
            exercise=int(entries[0]["Number"])
        target=next(x for x in entries if int(x["Number"])==exercise)
        path="/student/g12/precalculus/rubric?"+urllib.parse.urlencode({
            "lessonId":lesson_id,"exerciseNumber":exercise})
        form=get_page(student,path)
        token=extract_csrf(form)
        if not token or 'name="explanation"' not in form:
            raise AssertionError(
                f"G12_TEACHER_STUDENT_FORM_ABSENT ordinal={ordinal}")
        response=(
            "Synthetic incomplete source-skill attempt for CI verification only. "
            "It deliberately provides no steps or mathematical justification; "
            "the authorized teacher must award zero until reasoning is supplied."
        )
        body=urllib.parse.urlencode({
            "lessonId":lesson_id,"exerciseNumber":exercise,
            "explanation":response,"__RequestVerificationToken":token
        }).encode()
        with student.open(urllib.request.Request(
            base+"/student/g12/precalculus/rubric/submit",
            data=body,method="POST"),timeout=45) as http:
            if http.status!=200:
                raise AssertionError(
                    f"G12_TEACHER_STUDENT_SUBMIT_FAILED ordinal={ordinal}")
            http.read()
        submission=one(
            """SELECT "Id"::text FROM "Grade12PrecalculusTeacherRubrics"
                WHERE "CurriculumPedagogicalLessonId"='"""+lesson_id+"""'
                  AND "ExerciseNumber"="""+str(exercise)+"""
                  AND "TeacherScore" IS NULL;""",
            f"G12 unreviewed teacher response ordinal={ordinal}")
        view=get_page(teacher,"/school/g12/precalculus/rubric/"+submission)
        teacher_csrf=extract_csrf(view)
        if not teacher_csrf or view.count('name="marks"')!=4:
            raise AssertionError(
                f"G12_TEACHER_ASSIGNED_REVIEW_ABSENT ordinal={ordinal}")
        if target["Prompt"][:38] not in html.unescape(view):
            raise AssertionError(f"G12_TEACHER_PROMPT_NOT_VISIBLE ordinal={ordinal}")
        feedback=(
            "Synthetic incomplete assessment: no mathematical model or "
            "justification. All criteria fail until the learner explains."
        )
        grading=urllib.parse.urlencode([
            *(( "marks","0") for _ in range(4)),
            ("teacherFeedback",feedback),
            ("__RequestVerificationToken",teacher_csrf)
        ]).encode()
        path=base+"/school/g12/precalculus/rubric/"+submission
        with teacher.open(urllib.request.Request(
            path,data=grading,method="POST"),timeout=45) as http:
            if http.status!=200:
                raise AssertionError(
                    f"G12_TEACHER_GRADE_POST_FAILED ordinal={ordinal}")
            http.read()
        graded=one(
            """SELECT "TeacherScore"::text||'|'||"CriteriaJson"||'|'||
                      "TeacherFeedback"
                FROM "Grade12PrecalculusTeacherRubrics"
                WHERE "Id"='"""+submission+"""';""",
            f"G12 graded teacher rubric ordinal={ordinal}")
        score,criteria,saved=graded.split("|",2)
        if score!="0" or json.loads(criteria)!=[0,0,0,0] or saved!=feedback:
            raise AssertionError(
                f"G12_TEACHER_RUBRIC_DB_INVALID ordinal={ordinal}")
        results.append({
            "ordinal":ordinal,"lessonCode":code,
            "exerciseNumber":exercise,"allSourceTaskLinksRendered":len(entries),
            "reviewerSeesOriginalPrompt":True,"teacherAssigned":True,
            "studentSubmitted":True,"manualTeacherScore":0,
            "criteriaPersisted":4,"sqlPersisted":True,
            "responseIsSyntheticIncomplete":True
        })
        print("G12_TEACHER_RUBRIC_PERSISTENCE_CHECK",
            ordinal,code,flush=True)
    return results
