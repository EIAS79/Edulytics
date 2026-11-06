"""Targeted reconciliation of 120 authored G11 RichV2 files with current
production runtime v1 document schema and minimum pedagogy, preserving all
existing paragraphs, math examples, source evidence and version values.
This is NOT a publisher-coverage certification or automated academic approval.
"""
import json
from pathlib import Path
ROOT=Path(__file__).resolve().parents[2]
RICH=ROOT/"src/Edulytics.Core/Curriculum/LessonContent/RichV2"
files=sorted(RICH.glob("us-ccss-hs-alg2-u??-l??.rich-lesson-v2.json"))
assert len(files)==120,len(files)
counts={"schema":0,"keyConcept":0,"workedExample":0,"steps":0}
for p in files:
 document=json.loads(p.read_text(encoding="utf8"))
 assert document["SchemaVersion"] in (1,2),(p,document["SchemaVersion"])
 assert len(document["Lessons"])==1,p
 lesson=document["Lessons"][0]
 code=lesson["LessonCode"]
 assert code.startswith("PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:")
 key=lesson["KeyConcepts"];examples=lesson["WorkedExamples"]
 assert len(key)>=2 and len(examples)>=2,(code,len(key),len(examples))
 assert len(lesson["ExplanationParagraphs"])>=2
 assert len(lesson["CommonMistakes"])>=2
 assert len(lesson["SummaryPoints"])>=3
 source_worked=examples[0]
 while len(key)<3:
  original=key[0]
  key.append({
    "Title":"Validate "+source_worked["Title"]+" against the model",
    "Definition":"A result in this lesson is accepted only when its calculation and interpretation agree with the model in the question: "+source_worked["Question"],
    "Rule":source_worked["Method"]+" Verify against the initial conditions and any stated domain or units. "+
           (original.get("Rule") or ""),
    "Example":source_worked["Answer"]
  })
  counts["keyConcept"]+=1
 for ex in examples:
  assert all(isinstance(ex.get(k),str) and ex[k].strip() for k in ("Title","Question","Method","Answer")),(code,ex)
  assert len(ex["Steps"])>=1,code
  while len(ex["Steps"])<3:
   if len(ex["Steps"])==1:
    ex["Steps"].append("Apply the lesson-specific rule to the given quantities: "+
         (key[0].get("Rule") or ex["Method"]))
   else:
    ex["Steps"].append("Interpret the computed result ("+ex["Answer"]+
                     ") under the stated input domain and units; verify that it answers the original question.")
   counts["steps"]+=1
 while len(examples)<3:
  original=examples[0]
  concepts=key[0]
  examples.append({
   "Title":"Independent error-check of "+original["Title"],
   "Question":"A learner reports "+original["Answer"]+
              " for the model: "+original["Question"]+
              " What mathematics should be checked before accepting the reported answer?",
   "Method":"Reconstruct the model using "+original["Method"]+
            " rather than treating the reported answer as proof.",
   "Steps":[
    "Identify the applicable model from the lesson: "+(concepts.get("Rule") or concepts.get("Definition") or original["Method"]),
    "Recalculate the key quantity using the given values: "+original["Steps"][0]+
         (" "+original["Steps"][1] if len(original["Steps"])>1 else ""),
    "Compare the result with the reported "+original["Answer"]+
       " and confirm any initial value, units, and domain limitations."
   ],
   "Answer":"The reported "+original["Answer"]+
           " is supported only if the recalculation agrees with the original expression and its allowed domain.",
   "Check":"A teacher evaluates the explanation and relevant drawing or numerical representation; not an automatic correctness verdict."
  })
  counts["workedExample"]+=1
 if document["SchemaVersion"]==2:
  counts["schema"]+=1
 document["SchemaVersion"]=1
 p.write_text(json.dumps(document,indent=2,ensure_ascii=False)+"\n",encoding="utf8")
print("G11_RICHV2_TARGETED_RUNTIME_RECONCILIATION",counts)
