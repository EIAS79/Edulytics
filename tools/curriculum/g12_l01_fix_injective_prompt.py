#!/usr/bin/env python3
"""Fix genuine ambiguity in first-party Try It 9 for G12 U01:L01."""
from pathlib import Path
import json,hashlib
ROOT=Path(__file__).resolve().parents[2]
TRY=ROOT/"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u01-l01-tryit-first-party-teacher-review.json"
RICH=ROOT/"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u01-l01.rich-lesson-v2.json"
CANON=ROOT/"src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g12-precalculus-u01-l01-abc-draft.lesson-content-pack.json"
HAND=ROOT/"artifacts/curriculum-delivery/US-CCSS-MATH/G12/grade12-a-c-handoff.md"
old="Each employee is assigned one department; departments can contain multiple employees."
new="Each employee is assigned exactly one department, and at least two employees are assigned to the same department."
for f in [TRY,RICH,CANON]:
    raw=f.read_text(encoding="utf8")
    assert raw.count(old)==1, (f,raw.count(old))
    f.write_text(raw.replace(old,new),encoding="utf8")
data=json.loads(TRY.read_text(encoding="utf8"))
item=next(x for x in data["Items"] if x["SourceId"]=="G12-U01-L01-TRY009")
item["ExpectedTeacherAnswer"]="Department is a function of employee. Employee is not a function of department, because the stipulated shared department maps to at least two employees. The first function is not one-to-one."
TRY.write_text(json.dumps(data,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
p=json.loads(CANON.read_text(encoding="utf8"))
tr=p["Lessons"][0]["Translations"][0]
tr_keys=["Title","Explanation","KeyConceptsAndRules","WorkedExamples","StepByStepSolutions","CommonMistakes","QuickSummary"]
p["Lessons"][0]["CanonicalBodySha256"]=hashlib.sha256("\n".join(tr[k] for k in tr_keys).encode("utf8")).hexdigest()
CANON.write_text(json.dumps(p,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
h=HAND.read_text(encoding="utf8")
h+="\n- Corrected first-party Try It 9: \"can contain multiple\" was insufficient to prove non-injectivity. The exercise now explicitly specifies at least two employees in one department, with an aligned teacher answer. Same correction is applied to Rich V2 and canonical Draft.\n"
HAND.write_text(h,encoding="utf8")
print("FIXED_FIRST_PARTY_TRYIT_009_UNIQUE_OUTPUT_LOGIC",p["Lessons"][0]["CanonicalBodySha256"])
