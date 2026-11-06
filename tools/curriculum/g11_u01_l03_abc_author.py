"""Grade 11 Algebra 2 U01 L03 targeted A-B-C source repair; Stage D tests authored only."""
from pathlib import Path
import json,hashlib,datetime
ROOT=Path(__file__).resolve().parents[2]
BASE=Path("artifacts/curriculum-delivery/US-CCSS-MATH/G11")
CODE="PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:U01:L03"
NOW=datetime.datetime.now(datetime.timezone.utc).isoformat()
def dump(p,x):
 q=ROOT/p;q.parent.mkdir(parents=True,exist_ok=True);q.write_text(json.dumps(x,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
inv=json.loads((ROOT/BASE/"local-archive-source-inventory-20261106.json").read_text(encoding="utf8"))["lessons"][2]
assert inv["lessonCode"]==CODE
for k,d in inv["documents"].items():assert hashlib.sha256(Path(d["path"]).read_bytes()).hexdigest()==d["sha256"]
bp=json.loads((ROOT/"src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/us-ccss-math-hs-traditional-algebra2-im1e.lesson-blueprint.json").read_text(encoding="utf8"))["Lessons"][2]
assert bp["LessonCode"]==CODE and bp["Title"]=="Different Types of Sequences"
PACK=Path("src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-hs-alg2-phase29-v1.lesson-content-pack.json")
pack=json.loads((ROOT/PACK).read_text(encoding="utf8"));tr=next(t for x in pack["lessons"] if x["lessonCode"]==CODE for t in x["translations"] if t["cultureCode"]=="en")
tr.update({"explanation":"Distinguish arithmetic sequences, which add a fixed common difference, from geometric sequences, which multiply by a fixed common ratio; a sequence may be neither. Read tabular and graphed term data with the correct indices, compare the 10th terms of different growth patterns, and explain how a positive constant sequence can satisfy both rules.",
"keyConceptsAndRules":"Arithmetic a(n+1)=a(n)+d, a(n)=a(1)+(n-1)d, indexed at 1. Geometric a(n+1)=r*a(n), a(n)=a(1)r^(n-1), for nonzero consecutive terms and constant r. A sequence such as 0,5,15,30,50 grows with differences 5,10,15,20, so it is neither arithmetic nor geometric. For positive constant c,c,c,... the common difference is 0 and the ratio is 1.",
"workedExamples":"Arithmetic A: 30,40,50,60,70 gives A(10)=30+9*10=120. Nonlinear B: 0,5,15,30,50 gives B(n)=5*n*(n-1)/2, B(10)=225. Geometric C: 1,2,4,8,16 gives C(10)=2^9=512. So B, not A, is second largest at term 10. Function warm-up: f(n)=3n-7 yields f(0)=-7, f(3)=2 and f(10)=23. When input intervals vary, repeated output ratios alone do not establish a single ratio between consecutive positions.",
"stepByStepSolutions":"Identify each term's actual position. Subtract consecutive terms to test for constant difference; divide consecutive nonzero terms to test for constant ratio. If neither invariant holds, classify 'neither' and find a separate justified rule where possible. For a comparison of 10th terms calculate each from its particular model and compare actual values, not only initial terms.",
"commonMistakes":"Confusing a constant growth difference with a growth ratio; mistaking gaps in graph x-values for consecutive terms; claiming any geometric pattern must rise; treating a constant sequence with positive terms as belonging to only one category.",
"quickSummary":"Arithmetic means constant difference, geometric means constant ratio, and some sequences satisfy neither. Check multiple consecutive pairs and preserve the index domain when reading graphs."})
dump(PACK,pack)
RICH=Path("src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-alg2-u01-l03.rich-lesson-v2.json")
rich={"LessonCode":CODE,"CultureCode":"en","Title":"Different Types of Sequences",
"ExplanationParagraphs":[
 "Goal: compare, distinguish and generate arithmetic and geometric sequences from tables, graphs, function notation and numerical patterns.",
 "Warm-up: evaluate a function of the form f(n)=3n-7 at specified integer inputs; explain the role of the input n rather than confusing it with the term value.",
 "Discuss three distinct examples: A=30,40,50,60,70 is arithmetic (+10); B=0,5,15,30,50 has growing differences (+5,+10,+15,+20); C=1,2,4,8,16 is geometric (times two). The same first terms do not guarantee the same type of growth.",
 "With continuation of those rules to term 10, A(10)=120, B(10)=225 and C(10)=512. The nonlinear B value is second largest; do not choose based on the appearance of only the first five terms.",
 "Arithmetic rule: from a starting term a(1), a(n)=a(1)+(n-1)d. Geometric rule: from starting term g(1), g(n)=g(1)r^(n-1). Both require an explicit starting index.",
 "A positive constant sequence such as 5,5,5,... has common difference zero and common ratio one. It is both arithmetic and geometric; this is an exception to an informal either/or classification.",
 "Compare a table and a graph of the same sequence carefully. If the input positions skip integers, multiplying successive listed output values does not prove a constant multiplier from n to n+1. Teacher reviews the graph, its domain, axes and argument.",
 "Practice: fill missing arithmetic terms with signed or fractional differences, find geometric factors, distinguish 'neither', and supply a valid continuation for an arithmetic, geometric or nonstandard rule.",
 "Independent cool-down (Edulytics): compare U=4,10,16,22 and V=4,10,25,62.5. Decide which is arithmetic and which is geometric, state a recursive rule, and predict the next term. Justify using consecutive differences or ratios."
],
"KeyConcepts":[{"Title":"Arithmetic sequence","Definition":"One constant amount is added at each step.","Rule":"a(n)=a(1)+(n-1)d.","Example":"30,40,50,... has d=10."},
 {"Title":"Geometric sequence","Definition":"One constant factor multiplies consecutive nonzero terms.","Rule":"g(n)=g(1)r^(n-1).","Example":"1,2,4,... has r=2."},
 {"Title":"Neither or both","Definition":"Not all sequences fit either classification, while positive constant sequences fit both.","Rule":"Check at least two differences and two well-defined adjacent ratios.","Example":"0,5,15,30,50 is neither; 5,5,5,... is both."}],
"WorkedExamples":[{"Title":"Compare term 10 in three patterns","Question":"Which has the second-largest 10th term: 30,40,50,...; 0,5,15,30,50,...; or 1,2,4,8,16,...?","Method":"Identify a separate rule for each sequence.","Steps":["A(n)=30+10(n-1), so A(10)=120.","B(n)=5n(n-1)/2, so B(10)=225.","C(n)=2^(n-1), so C(10)=512.","Order 120 < 225 < 512."],"Answer":"The second sequence, B, at 225.","Check":"B has nonconstant differences and is neither arithmetic nor geometric."},
 {"Title":"Find an unknown arithmetic rate","Question":"Fill -3,-2,_,_,1 and give the common difference.","Method":"Compare adjacent terms.","Steps":["Difference -2-(-3)=+1.","Next terms are -1 and 0.","The following term 1 verifies a constant +1."],"Answer":"-1, 0; d=1.","Check":"Each step adds one."},
 {"Title":"A sequence that fits two definitions","Question":"Can a positive constant sequence such as 7,7,7,... be arithmetic and geometric?","Method":"Check differences and ratios.","Steps":["Every difference is 7-7=0.","Every ratio is 7/7=1.","Both differences and ratios are constant."],"Answer":"Yes; d=0 and r=1.","Check":"This example refutes a claim that the categories never overlap."}],
"CommonMistakes":[{"Mistake":"Deciding a sequence is geometric after observing one ratio.","WhyWrong":"A common ratio must be maintained at each step.","Correction":"Check every available consecutive nonzero pair."},
 {"Mistake":"Equating a nonlinear sequence with arithmetic because the terms increase.","WhyWrong":"Increasing differences are not a common difference.","Correction":"Compute a difference table first."},
 {"Mistake":"Assuming a positive constant sequence cannot be both.","WhyWrong":"It has difference zero and ratio one.","Correction":"Test both definitions directly."}],
"SummaryPoints":["Arithmetic: constant difference; geometric: constant ratio.","Some sequences are neither, and positive constant sequences are both.","Term indices are discrete; source graphs need justified axis and step interpretation.","Teacher checks the open classification argument and graphs."],"Visuals":[],"Videos":[]}
dump(RICH,{"SchemaVersion":2,"PackCode":"US-CCSS-MATH","ContentVersion":"g11-u01-l03-source-aligned-20261106","Lessons":[rich]})
ASSESS=Path("src/Edulytics.Core/Curriculum/LessonContent/Assessment/alg2-u01-l03-source-teacher-led-20261106.json")
assess={"LessonCode":CODE,"BlueprintAlignments":bp["Alignments"],"SkillContracts":[{"SkillId":"sequences.core","AllowedFamilies":["sequences.core.arithmetic_nth_term","sequences.core.geometric_nth_term","sequences.core.find_position_arithmetic","usccss.g11.alg2.u01.l02.rational_geometric_term"],"Forbidden":["random-unrelated-arithmetic"],"Readiness":"WRITTEN_NOT_RUN"}],"TeacherReview":{"Required":True,"Tasks":["evaluate function notation without a calculator","comparison of three source sequence patterns","third source table-versus-graph dispute","signed and fractional common differences","classify positive constant sequence as both"],"Rubric":["correct start index","difference or ratio checked for each step","appropriate function domain and graph","justification of classification"]},"FirstPartyCoolDown":{"Prompt":"Classify U: 4,10,16,22 and V: 4,10,25,62.5. Write a recurrence for each and predict the next term. Explain how you can tell.","Expected":"U arithmetic d=6 next28; V geometric r=5/2 next156.25.","Grader":"TEACHER_REVIEW_REQUIRED","Origin":"EDULYTICS_ORIGINAL"},"TestsAuthoredNotRun":["tools/curriculum/us-ccss-g11-u01-l03-authoring-contract.test.cjs"],"TestsExecuted":"NOT_RUN","StageD":"NOT_RUN"}
dump(ASSESS,assess)
groups=[
 ("PREP-GOALS","teacher/preparation.html","Learning Goals","arithmetic versus geometric; common difference; interpretation","TEACHER_LED"),
 ("WU-FUNCTION","student/index.html","3.1","evaluate function notation 3n-7","DIGITAL"),
 ("A-TERMS","student/index.html","3.2","arithmetic pattern A: 30 40 50 60 70","DIGITAL"),
 ("B-TERMS","student/index.html","3.2","non-arithmetic and non-geometric pattern B: 0 5 15 30 50","TEACHER_LED"),
 ("C-TERMS","student/index.html","3.2","geometric pattern C: 1 2 4 8 16","DIGITAL"),
 ("COMPARE-NEXT","student/index.html","3.2","three recurrence descriptions","TEACHER_LED"),
 ("COMPARE-TENTH","student/index.html","3.2","relative order at tenth term; 120 225 512","DIGITAL"),
 ("COMPARE-TYPE","student/index.html","3.2","identify geometric among example patterns","TEACHER_LED"),
 ("BOTH-CLASSIFICATIONS","student/index.html","3.2","counterexample positive constant sequence both arithmetic and geometric","TEACHER_LED"),
 ("TABLE-GRAPH","student/index.html","3.3","examine table/graph mismatch and non-unit input steps","TEACHER_LED"),
 ("P1-TERMS","student/practice.html","Problem 1","continue given arithmetic sequences","DIGITAL"),
 ("P2-TYPES","student/practice.html","Problem 2","classify arithmetic geometric or neither","TEACHER_LED"),
 ("P3-01","student/practice.html","Problem 3","-3,-2,...,1 consecutive difference +1","DIGITAL"),
 ("P3-02","student/practice.html","Problem 3","...,13,25,... difference12","DIGITAL"),
 ("P3-03","student/practice.html","Problem 3","1,.25,...,-1.25 difference-.75","DIGITAL"),
 ("P3-04","student/practice.html","Problem 3","92,...,80 constant negative difference","DIGITAL"),
 ("P4-ARITH","student/practice.html","Problem 4","1,10 arithmetic continuation","DIGITAL"),
 ("P4-GEO","student/practice.html","Problem 4","1,10 geometric continuation","DIGITAL"),
 ("P4-NEITHER","student/practice.html","Problem 4","1,10 continuation with nonconstant difference and ratio","TEACHER_LED"),
 ("P5-MISSING","student/practice.html","Problem 5","missing geometric terms and growth factor","DIGITAL"),
 ("MATERIALS","teacher/preparation.html","Lesson Narrative","use student conversation and independent graphic representations","TEACHER_LED")
]
ev=[{"SourceElementId":f"G11-U01-L03-{id}","SourceLocator":locator+"#"+sect,"MathematicalRequirement":math,"ImplementationType":mode,"Status":"MATCHED_DIGITAL" if mode=="DIGITAL" else "MATCHED_TEACHER_LED","TargetArtifact":str(RICH).replace("\\","/"),"TestsExecuted":"NOT_RUN"} for id,locator,sect,math,mode in groups]
E=BASE/"u01-l03-element-coverage.json";dump(E,{"LessonCode":CODE,"SourceDocuments":inv["documents"],"Elements":ev,"SourceElementCount":len(ev),"TestsExecuted":"NOT_RUN"})
CP=BASE/"u01-l03-abc-checkpoint.json";dump(CP,{"LessonCode":CODE,"Ordinal":3,"StartedUtc":NOW,"ClosedUtc":NOW,"Status":"READY_FOR_STAGE_D","ReadyForStageD":True,"PhaseA":"SOURCE_COMPARED","PhaseB":"RICH_V2_AUTHORING_COMPLETE","PhaseC":"PRACTICE_AND_TEACHER_RUBRIC_AUTHORED","TestsExecuted":"NOT_RUN","StageD":"NOT_RUN","AssessmentPath":str(ASSESS).replace("\\","/"),"SourceCheckpoint":str(E).replace("\\","/"),"RichV2Path":str(RICH).replace("\\","/")})
ledgerp=BASE/"abc-independent-course-ledger.json";ledger=json.loads((ROOT/ledgerp).read_text(encoding="utf8"));assert len(ledger["LessonEntries"])==2 and ledger["LessonEntries"][-1]["Ordinal"]==2;ledger["LessonEntries"].append({"Ordinal":3,"LessonCode":CODE,"Unit":1,"Lesson":3,"ABCReadyForD":True,"Status":"READY_FOR_STAGE_D","ClosedUtc":NOW,"SourceCheckpoint":str(CP).replace("\\","/"),"ElementEvidence":str(E).replace("\\","/"),"AssessmentPath":str(ASSESS).replace("\\","/"),"TestsExecuted":"NOT_RUN","StageD":"NOT_RUN"});ledger["UpdatedUtc"]=NOW;dump(ledgerp,ledger)
pp=BASE/"full-grade-progress.json";progress=json.loads((ROOT/pp).read_text(encoding="utf8"));assert progress["LessonsReadyForStageD"]==2;progress.update({"LessonsReadyForStageD":3,"LessonsRemaining":117,"LastClosedLesson":CODE,"FirstOpenLesson":"PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:U01:L04","UpdatedUtc":NOW});dump(pp,progress)
print('G11_U01_L03_SOURCE_ALIGNED_AUTHORING',len(ev),'SOURCE_TASKS_READY_STAGE_D_TESTS_NOT_RUN')
