from pathlib import Path
import json,datetime,collections
R=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
B=R/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
js=R/"tools/curriculum/g12_stage_d_content_shard.cjs"
s=js.read_text(encoding="utf8")
replacements=[
("if(!teacherIsCSharpSection11 && rich.SchemaVersion!==2)fail(n,'RICHV2_SCHEMA_NOT_2');","if(rich.SchemaVersion!==1)fail(n,'RICHV2_RUNTIME_SCHEMA_UNSUPPORTED');"),
("if(teacherIsCSharpSection11 && rich.SchemaVersion!==1)fail(n,'SECTION11_EXISTING_SCHEMA_CHANGED');",""),
("if(!Array.isArray(student.WorkedExamples)||!student.WorkedExamples.length)fail(n,'NO_WORKED_EXAMPLES');","if(!Array.isArray(student.WorkedExamples)||student.WorkedExamples.length<3)fail(n,'WORKED_EXAMPLES_BELOW_RUNTIME_MINIMUM_3');"),
("if(!Array.isArray(student.KeyConcepts)||!student.KeyConcepts.length)fail(n,'NO_KEY_CONCEPTS');","if(!Array.isArray(student.KeyConcepts)||student.KeyConcepts.length<3)fail(n,'KEY_CONCEPTS_BELOW_RUNTIME_MINIMUM_3');"),
("if(!Array.isArray(student.CommonMistakes)||!student.CommonMistakes.length)fail(n,'NO_MISTAKES_GUIDANCE');","if(!Array.isArray(student.CommonMistakes)||student.CommonMistakes.length<2)fail(n,'COMMON_MISTAKES_BELOW_RUNTIME_MINIMUM_2');"),
("if(!Array.isArray(student.SummaryPoints)||!student.SummaryPoints.length)fail(n,'NO_SUMMARY');","if(!Array.isArray(student.SummaryPoints)||student.SummaryPoints.length<3)fail(n,'SUMMARY_BELOW_RUNTIME_MINIMUM_3');"),
("if(!w.Question||!w.Answer||!Array.isArray(w.Steps)||!w.Steps.length)fail(n,'WORKED_EXAMPLE_INCOMPLETE_'+(i+1));","if(!w.Title||!w.Question||!w.Method||!w.Answer||!Array.isArray(w.Steps)||w.Steps.length<3)fail(n,'WORKED_EXAMPLE_RUNTIME_CONTRACT_INVALID_'+(i+1));")]
for old,new in replacements:
 assert old in s,old
 s=s.replace(old,new)
js.write_text(s,encoding="utf8")
ledger_file=B/"grade12-stage-d-execution-ledger.json"
ledger=json.loads(ledger_file.read_text(encoding="utf8"))
issues=collections.Counter()
now=datetime.datetime.now(datetime.timezone.utc).isoformat()
for row in ledger["lessonResults"]:
    unit=int(row["unit"]);lesson=int(row["section"].split(".")[-1])
    path=R/f"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u{unit:02d}-l{lesson:02d}.rich-lesson-v2.json"
    d=json.loads(path.read_text(encoding="utf8"))
    l=d["Lessons"][0]
    detected=[]
    if d["SchemaVersion"]!=1:detected.append("RUNTIME_RICHV2_SCHEMA_VERSION_UNSUPPORTED")
    if len(l["KeyConcepts"])<3:detected.append("RUNTIME_KEY_CONCEPTS_BELOW_MIN_3")
    if len(l["WorkedExamples"])<3:detected.append("RUNTIME_WORKED_EXAMPLES_BELOW_MIN_3")
    short=sum(len(w["Steps"])<3 for w in l["WorkedExamples"])
    if short:detected.append("RUNTIME_WORKED_EXAMPLE_STEPS_BELOW_MIN_3_COUNT_"+str(short))
    if len(l["CommonMistakes"])<2:detected.append("RUNTIME_COMMON_MISTAKES_BELOW_MIN_2")
    if len(l["SummaryPoints"])<3:detected.append("RUNTIME_SUMMARY_BELOW_MIN_3")
    for issue in detected:issues[issue.split("_COUNT_")[0]]+=1
    row["openDefects"]=sorted(set(row["openDefects"]+detected))
    row["dStatus"]="D_BLOCKED" if row["openDefects"] else "D_PENDING"
    row["technicalStatus"]="BLOCKED_RUNTIME_RESOURCE_CONTRACT" if detected else row["technicalStatus"]
    row["updatedAtUtc"]=now
ledger["blockedLessons"]=[x["lessonOrdinal"]for x in ledger["lessonResults"]if x["dStatus"]=="D_BLOCKED"]
ledger["firstIncompleteLessonOrdinal"]=1
ledger["gradeStatus"]="NO_GO_TECHNICAL"
ledger["lastSourceRuntimeContractAuditAtUtc"]=now
ledger["runtimeContractFindings"]=dict(sorted(issues.items()))
ledger["ciRunIds"]=sorted(set(ledger.get("ciRunIds",[])+[42407859571,42409720619,42409973898]))
ledger["runtimeTestsStarted"]=True
ledger["notes"].append("Real runtime requires SchemaVersion=1, >=3 key concepts, >=3 steps per example, >=2 common mistakes and >=3 summary points; prior static checks were insufficient.")
ledger_file.write_text(json.dumps(ledger,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
report=B/"grade12-stage-d-evidence-index.md"
with report.open("a",encoding="utf8")as f:
 f.write("\n## Actual runtime contract failure inventory — "+now+"\n\n")
 f.write("Checked against RichLessonContentV2Registry.Validate, not just JSON syntax.\n")
 f.write("72/73 resources have unsupported SchemaVersion 2; runtime accepts 1.\n")
 for issue,count in sorted(issues.items()):f.write("- "+issue+": "+str(count)+" lessons\n")
 f.write("No invented filler content; repair documented deficiencies in source-aligned lessons sequentially.\n")
 f.write("No D_VERIFIED or release decision has been made.\n")
print("BLOCKED",len(ledger["blockedLessons"]),"FINDINGS",dict(issues))
