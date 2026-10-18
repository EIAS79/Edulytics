#!/usr/bin/env python3
"""Audit exact V1 source goals. Preserve honest pending release statuses."""
import json
from pathlib import Path
root=Path(__file__).resolve().parents[2]
p=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G1/batches/US-CCSS-G1-010-LOCAL-SOURCE-CROSSWALK.json"
d=json.loads(p.read_text(encoding="utf-8"))
requirements={
46:["Compose and decompose 11–19 as one ten and some ones, and recognize card 20 as two full tens, using double ten-frames",
    "Find the value that makes an addition equation true when one addend is ten and the unknown changes position"],
47:["Find the unknown start or change in teen-total addition/subtraction story problems",
    "Use the relationship between addition and subtraction to find missing values involving a ten"],
48:["Solve an Add To, Result Unknown story by adding one digit to a teen number within 20",
    "Find a missing addend or total in different-position equations with a teen addend"],
49:["Solve a Take From, Result Unknown story by subtracting from a teen number without decomposing a ten",
    "Find missing values in the original student Activity 12.2, Addition and Subtraction Equations with Teen Numbers; teacher-led Shake and Spill is a separate center"],
50:["Solve a Take From, Result Unknown story using a teen starting quantity",
    "Solve a Take From, Change Unknown story and explain why the position of the unknown changes the equation"]
}
assert d["planId"]=="EDULYTICS-GRADE-FIRST-5-V1" and len(d["lessons"])==5
for ordinal,row in enumerate(d["lessons"],46):
 assert row["gradeOrdinal"]==ordinal and len(row["sourceGoals"])==2
 for goal,requirement in zip(row["sourceGoals"],requirements[ordinal]):
  assert goal["practiceFamily"].startswith("usccss.g1.u03.l")
  goal["sourceGoalRequirement"]=requirement
  goal["evidenceScope"]="Original 2021 teacher preparation goal, teacher-embedded student task, and original student-task PDF; local skill contract and mathematical negative-control tests. Teacher-facilitated oral work is not digitally graded."
  assert goal["assessmentStatus"]=="LOCAL_IMPLEMENTED_PENDING_EXACT_HEAD_CI"
p.write_text(json.dumps(d,indent=2,ensure_ascii=False)+"\n",encoding="utf-8")
print("10 goal requirements crosswalked from original 2021 sources, without claiming external approval")
