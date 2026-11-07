#!/usr/bin/env python3
"""Targeted Grade 12 Section 11.1 teacher-key sign repair; does not certify source parity."""
from pathlib import Path
import json,re,subprocess,ast
R=Path(__file__).resolve().parents[2]
BASE="us-ccss-g12-precalculus-u11-l01"
TEACH="src/Edulytics.Core/Curriculum/LessonContent/Assessment/"+BASE+"-firstparty-teacher-led.json"
RICH="src/Edulytics.Core/Curriculum/LessonContent/RichV2/"+BASE+".rich-lesson-v2.json"
def load(rel):
    p=R/rel
    if p.exists():return json.loads(p.read_text(encoding="utf8"))
    return json.loads(subprocess.check_output(["git","show","HEAD:"+rel],cwd=R).decode("utf8"))
def save(rel,x):
    p=R/rel
    p.parent.mkdir(parents=True,exist_ok=True)
    p.write_text(json.dumps(x,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
teacher=load(TEACH)
rich=load(RICH)
questions=teacher["IndependentStudentMathQuestions"]
assert len(questions)==71
lesson=rich["Lessons"][0]
fixed=[]
for q in questions:
    question=q["IndependentStudentQuestion"]
    if "alternating" not in question:continue
    match=re.search(r"\(\u22121\)\^n\((-?\d+)\+n\)",question)
    if match is None:raise ValueError("Unexpected alternating series question "+question)
    start=int(match.group(1))
    expected=str([((-1)**n)*(start+n)for n in range(1,6)])+"."
    old=q["TeacherMath"]
    computed_old=ast.literal_eval(old.rstrip("."))
    assert len(computed_old)==5
    if old!=expected:
        q["TeacherMath"]=expected
        fixed.append((q["OriginalExercise"],question,old,expected))
assert len(fixed)==12,("Expected exactly 12 sign bugs",fixed)
teacher_answers={q["IndependentStudentQuestion"]:q["TeacherMath"] for q in questions}
changed_worked=0
for worked in lesson["WorkedExamples"]:
    q=worked["Question"]
    if q not in teacher_answers or "alternating" not in q:continue
    new=teacher_answers[q]
    old=worked["Answer"]
    if old==new:continue
    worked["Answer"]=new
    worked["Steps"]=[
        step.replace(old,new) if old in step else step
        for step in worked["Steps"]
    ]
    assert new==worked["Answer"]
    assert all(old not in step for step in worked["Steps"])
    changed_worked+=1
changed_try=0
for t in teacher.get("IndependentTryIt",[]):
    q=t["Question"]
    if q in teacher_answers and "alternating" in q:
        new=teacher_answers[q]
        if t.get("TeacherExpectedMath")!=new:
            t["TeacherExpectedMath"]=new
            changed_try+=1
cool=teacher.get("IndependentCoolDown",{})
if cool.get("StudentQuestions") and "alternating" in cool["StudentQuestions"][0]:
    cool["TeacherExpectedMath"]=[teacher_answers[cool["StudentQuestions"][0]]]
assert changed_worked>=1,(fixed,changed_worked)
assert changed_try>=1,(fixed,changed_try)
save(TEACH,teacher)
save(RICH,rich)
print(json.dumps({"fixedKeys":len(fixed),"workedChanged":changed_worked,
"tryItsChanged":changed_try,"items":[x[0]for x in fixed]},ensure_ascii=False))
