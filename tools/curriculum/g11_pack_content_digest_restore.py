"""Reconcile exact Grade11 learner pack digests and restore specific lost first-party math.
All other 117 lesson body fields remain byte-for-byte equal after JSON parse; no
publisher answer keys are imported and sourceSha256 stays fixed.
"""
from pathlib import Path
import json,hashlib,subprocess
root=Path(__file__).resolve().parents[2]
p=Path("src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-hs-alg2-phase29-v1.lesson-content-pack.json")
current=json.loads((root/p).read_text(encoding="utf8"))
original=json.loads(subprocess.check_output(["git","show","origin/main:"+p.as_posix()],cwd=root,text=True,encoding="utf8"))
old={x["lessonCode"]:x for x in original["lessons"]}
keys=("title","explanation","keyConceptsAndRules","workedExamples","stepByStepSolutions","commonMistakes","quickSummary")
target=("U02:L17","U02:L18","U05:L10")
modified=[]
changes=0
for lesson in current["lessons"]:
 code=lesson["lessonCode"]
 if any(code.endswith(suffix) for suffix in target):
  base=old[code]
  t=next(x for x in lesson["translations"] if x["cultureCode"]=="en")
  b=next(x for x in base["translations"] if x["cultureCode"]=="en")
  for field in ("keyConceptsAndRules","workedExamples","stepByStepSolutions","commonMistakes","quickSummary"):
   assert b[field],(code,field)
   # Retain a longer already-authored A-B-C lesson summary rather than shrinking it.
   if field=="quickSummary" and len(b[field])<=len(t[field]):
    continue
   assert len(b[field])>len(t[field]),(code,field)
   t[field]=b[field]
  if code.endswith("U05:L10"):
   marker="Edulytics original standards extension"
   assert marker in b["explanation"] and marker not in t["explanation"]
   t["explanation"]=t["explanation"]+"\n\n"+b["explanation"][b["explanation"].index(marker):]
  modified.append(code)
 t=next(x for x in lesson["translations"] if x["cultureCode"]=="en")
 canonical="\n".join(k+":"+t[k] for k in keys)
 digest=hashlib.sha256(canonical.encode("utf8")).hexdigest()
 if digest!=lesson.get("canonicalBodySha256"):changes+=1
 lesson["canonicalBodySha256"]=digest
assert len(current["lessons"])==120 and len(modified)==3
assert changes==120,(changes,"Unexpected source-checksum state; inspect, do not overwrite")
(root/p).write_text(json.dumps(current,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
print("RECONCILED_CHEKSUMS",changes,"RECOVERED_SOURCE_SPECIFIC_EDULYTICS_MATH",modified)
