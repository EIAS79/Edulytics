#!/usr/bin/env python3
"""Materialize 72 first-party teacher-review banks already authored in A–B–C.
The U01:L01 92-task specialist bank remains authoritative. Do not claim source
verbatim parity, independent examiner verification, or 100% rights clearance."""
import json,re,subprocess
from pathlib import Path
ROOT=Path(__file__).resolve().parents[2]
progress=json.loads(subprocess.check_output(["git","show","HEAD:artifacts/curriculum-delivery/US-CCSS-MATH/G12/full-grade-progress.json"],cwd=ROOT).decode("utf8"))
assert len(progress["LessonEntries"])==73
fields=("Prompt","IndependentStudentQuestion","Question","StudentQuestion","StudentPrompt","Task","IndependentStudentMathQuestion","IndependentQuestion","FirstPartyQuestion","Text","Description")
out=[]
for entry in progress["LessonEntries"][1:]:
    code=entry["LessonCode"]
    unit,lesson=map(int,re.search(r":U(\d\d):L(\d\d)$",code).groups())
    file=f"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u{unit:02d}-l{lesson:02d}-firstparty-teacher-led.json"
    source=json.loads(subprocess.check_output(["git","show","HEAD:"+file],cwd=ROOT).decode("utf8"))
    candidates=[]
    for key,values in source.items():
        if not isinstance(values,list):continue
        questions=[]
        for row in values:
            if not isinstance(row,dict):continue
            prompt=next((row[k].strip() for k in fields if isinstance(row.get(k),str) and len(row[k].strip())>=12),None)
            if prompt and len(prompt)<7900:
                number=row.get("Number",row.get("OriginalExercise",row.get("ExerciseNumber",len(questions)+1)))
                if type(number)!=int or not(1<=number<=10000):number=len(questions)+1
                questions.append((number,prompt))
        if questions:
            preferred=("StudentQuestions" in key or "Independent" in key or "FirstParty" in key)
            candidates.append((len(questions),int(preferred),key,questions))
    if not candidates:raise RuntimeError(f"No verifiable authored teacher-led bank {code}")
    _,_,field,selected=max(candidates,key=lambda x:(x[0],x[1]))
    tasks=[]
    seen_numbers=set()
    for original,prompt in selected:
        if original in seen_numbers:continue
        seen_numbers.add(original)
        tasks.append({"Number":original,"Prompt":prompt})
    if not tasks:raise RuntimeError(f"Empty teacher task list {code}")
    out.append({"LessonCode":code,"OriginalSourceField":field,
        "Tasks":tasks})
assert len(out)==72
assert len({x["LessonCode"] for x in out})==72
outfile=ROOT/"src/Edulytics.Web/Curriculum/G12/grade12-firstparty-teacher-tasks.json"
outfile.parent.mkdir(parents=True,exist_ok=True)
outfile.write_text(json.dumps({"SchemaVersion":1,"AcademicOwnerApproval":"2026-11-07",
    "ReviewMode":"MANUAL_TEACHER_ONLY",
    "SourceExactParity":False,
    "Lessons":out},ensure_ascii=False,indent=2)+"\n",encoding="utf8")
print("G12_TEACHER_BANKS",len(out),"TASKS",sum(len(x["Tasks"]) for x in out),
    "SMALLEST",min(len(x["Tasks"]) for x in out),"LARGEST",max(len(x["Tasks"]) for x in out),
    "BYTES",outfile.stat().st_size)
