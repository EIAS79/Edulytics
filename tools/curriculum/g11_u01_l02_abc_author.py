"""Source-specific U01 L02 Algebra 2 authoring. No Stage D tests are run."""
from pathlib import Path
import datetime,hashlib,json
ROOT=Path(__file__).resolve().parents[2]
SRC=Path(r"C:\Users\khali\Downloads\New folder (2)\Common Core")
CODE="PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:U01:L02"
NOW=datetime.datetime.now(datetime.timezone.utc).isoformat()
def dump(path,value):
 p=ROOT/path;p.parent.mkdir(parents=True,exist_ok=True)
 p.write_text(json.dumps(value,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
books=[("teacher","11_HS_Algebra_2",["preparation.html","index.html","practice.html"]),("student","11_HS_Algebra_2_Student",["index.html","practice.html"])]
docs={f"{aud}/{name}":SRC/folder/"1"/"2"/name for aud,folder,names in books for name in names}
hashes={k:hashlib.sha256(p.read_bytes()).hexdigest() for k,p in docs.items()}
inventory=json.loads((ROOT/"artifacts/curriculum-delivery/US-CCSS-MATH/G11/local-archive-source-inventory-20261106.json").read_text(encoding="utf8"))["lessons"][1]
assert inventory["lessonCode"]==CODE
assert all(inventory["documents"][k]["sha256"]==value for k,value in hashes.items())
BLUEPRINT=json.loads((ROOT/"src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/us-ccss-math-hs-traditional-algebra2-im1e.lesson-blueprint.json").read_text(encoding="utf8"))
assert BLUEPRINT["Lessons"][1]["LessonCode"]==CODE
assert BLUEPRINT["Lessons"][1]["Title"]=="Introducing Geometric Sequences"
PACK=Path("src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-hs-alg2-phase29-v1.lesson-content-pack.json")
pack=json.loads((ROOT/PACK).read_text(encoding="utf8"));item=next(x for x in pack["lessons"] if x["lessonCode"]==CODE)
tr=next(x for x in item["translations"] if x["cultureCode"]=="en")
tr.update({
 "explanation":"Learn to distinguish a geometric sequence by a constant nonzero growth factor between consecutive terms. In a paper-halving context, the number of pieces doubles while the area of each piece halves, yet their product stays constant. Use tables, index-versus-value graphs, and numerical rules rather than treating a continuous curve as the discrete sequence. A growth factor between zero and one describes decay; negative factors alternate signs.",
 "keyConceptsAndRules":"Geometric sequence: a(n+1)=r*a(n), r a constant ratio when a(n) nonzero. Explicit form a(n)=a(0)*r^n for an index starting at zero. For an 8-by-10-inch sheet cut into equal halves n times, pieces P(n)=2^n, area A(n)=80/2^n square inches and P(n)A(n)=80. From adjacent terms, r=a(n+1)/a(n) if denominator not zero; compare consecutive ratios before claiming a sequence is geometric.",
 "workedExamples":"Paper cuts: after 5 cuts 32 pieces and 2.5 square inches each; after 6 cuts 64 pieces and 1.25 square inches each, preserving 80 square inches. Missing terms: 1.5, 3, 6, 12, 24, 48 has factor 2; 200,20,2,0.2,0.02,0.002 has factor 0.1. Shrinking pattern 256,192,144,108,81 has factor 3/4. In a fractal-style triangle pattern, the number of retained pieces grows by 3 each step while their individual areas shrink by 4 each step.",
 "stepByStepSolutions":"1. Label the input as whole-number steps or cuts. 2. Form a table for two successive terms. 3. Divide new by previous to find the growth factor when defined. 4. Extend terms by multiplying by the same factor. 5. Check units, discrete graph domain and any conserved quantity. 6. Explain a graph or physical cutting task to a teacher rather than assuming a number proves the diagram.",
 "commonMistakes":"Confusing growth factor 0.75 with a decrease of 0.75 units. Treating 0.5 times an area as a decrease of 0.5 square inches. Graphing continuous x-values as if a discrete number of cuts could be fractional. Using a single ratio without checking the rest of the sequence.",
 "quickSummary":"A geometric sequence has a constant multiplier. Doubling the number of pieces and halving their individual areas preserves the original sheet area. Multipliers above one grow; positive multipliers below one decay. Show a table or graph and justify each model."
})
(ROOT/PACK).write_text(json.dumps(pack,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
RICH=Path("src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-alg2-u01-l02.rich-lesson-v2.json")
rich={
 "LessonCode":CODE,"CultureCode":"en","Title":"Introducing Geometric Sequences",
 "ExplanationParagraphs":[
  "Learning goal: recognise equal ratios between consecutive terms, recover missing terms and model geometric growth or decay through tables and discrete graphs.",
  "Warm-up: compare four numerical lists, including one that triples, one that quadruples, one that halves and one multiplied by three-quarters. Ask how to distinguish equal multipliers from equal differences; classify by calculating at least two adjacent ratios.",
  "Paper investigation: start with an 8 by 10 inch rectangle. After every equal halving cut the number of pieces doubles and the area of each piece is halved. Complete an input-output table for zero through six cuts.",
  "Model the whole-number step n as P(n)=2^n and A(n)=80/2^n square inches. After n=5 there are 32 pieces of area 2.5 square inches; n=6 gives 64 pieces of 1.25 square inches. P(n)A(n)=80 at each step, so the total area remains constant.",
  "Draw or graph two separate sets of discrete points: cuts versus number of pieces, then cuts versus area per piece. Compare their opposite trends and explain why a fractional number of physical cuts is not a permitted input in this model.",
  "Generate and repair missing terms for geometric sequences with factors 2, 3, one-tenth, three and one-half; whenever an entry is fractional, preserve exact rational notation rather than rounding away the relationship.",
  "Practice transfer: a balance growing by 2% each month without payments follows B(n)=1000(1.02)^n. A triangle-subdivision pattern can have 3^n retained triangles, each 256/4^n square inches, so retained area is 256(3/4)^n. Distinguish piece count from area per piece.",
  "Teacher-led: if scissors or graphing technology are available, ask students to demonstrate cut-and-stack or plot points; verify labels, units, and the conservation argument. Do not treat a bare numeric answer as proof of a correct diagram.",
  "Independent cool-down (Edulytics): a rectangular board has area 96 square centimetres. Each operation doubles the count of equal regions and halves the area per region. Predict both quantities after four operations, write the two recursive rules and justify the invariant product."
 ],
 "KeyConcepts":[
  {"Title":"Geometric sequence","Definition":"Consecutive nonzero terms differ by a single constant multiplier r.","Rule":"a(n+1)=r*a(n); for n beginning at zero, a(n)=a(0)*r^n.","Example":"24,12,6,3,1.5 has r=1/2."},
  {"Title":"Growth factor","Definition":"A fixed ratio of a new term to the preceding nonzero term.","Rule":"r=a(n+1)/a(n), checked for several adjacent pairs.","Example":"256,192,144 has r=3/4."},
  {"Title":"Conserved total area","Definition":"More equal pieces can have smaller individual areas without changing the original area.","Rule":"P(n)=2^n; A(n)=A(0)/2^n; P(n)*A(n)=A(0).","Example":"64 pieces of 1.25 square inches still make 80 square inches."}
 ],
 "WorkedExamples":[
  {"Title":"Complete two geometric patterns","Question":"Find the missing terms in 1.5,3,6,?,24,? and 200,20,2,?,0.02,?.","Method":"Recover the constant multiplier from adjacent terms and use it consistently.","Steps":["First ratio is 3/1.5=2, giving 12 and 48.","Second ratio is 20/200=0.1, giving 0.2 and 0.002.","Check a second pair to confirm both factors."],"Answer":"12 and 48; 0.2 and 0.002.","Check":"Consecutive ratios are 2 and 0.1 respectively."},
  {"Title":"Doubling count, halving size","Question":"How many pieces and what area per piece result from six halving cuts of an 80-square-inch sheet?","Method":"Use powers of two, with n counting cuts starting at zero.","Steps":["Count=2^6=64.","Area per piece=80/64=1.25 square inches.","Total=64*1.25=80 square inches."],"Answer":"64 pieces, 1.25 square inches each.","Check":"The original sheet's total area is unchanged."},
  {"Title":"Two exponential quantities in a subdivided triangle","Question":"A diagram begins with one shaded triangle of area 256. At each step each retained triangle produces three retained smaller triangles, each a quarter the previous area. What is the shaded area at step two?","Method":"Track number and area per triangle independently.","Steps":["At step 2 the count is 3^2=9.","At step 2 each area is 256/4^2=16.","Shaded total =9*16=144."],"Answer":"144 square units.","Check":"The shaded fraction after two steps is (3/4)^2."}
 ],
 "CommonMistakes":[
  {"Mistake":"Subtracting a common difference instead of multiplying by a ratio.","WhyWrong":"Growth and decay change by factors, not fixed absolute increments.","Correction":"Divide adjacent nonzero terms twice to verify a common ratio."},
  {"Mistake":"Graphing a sequence as a continuous curve without indicating discrete points.","WhyWrong":"A cut count is a whole number and the sequence is indexed by whole steps.","Correction":"Plot only integer inputs with proper axis labels."},
  {"Mistake":"Claiming total sheet area grows when pieces multiply.","WhyWrong":"Area per piece decreases by the inverse multiplier.","Correction":"Multiply piece count by area per piece."}
 ],
 "SummaryPoints":["Constant adjacent ratios characterise a geometric sequence.","An index starting at zero has explicit rule a(n)=a(0)*r^n.","In a cut-and-stack model pieces double while area per piece halves.","Teacher verifies physical diagrams and graphs; exact numeric families are authored for Stage D verification."],
 "Visuals":[],"Videos":[]
}
dump(RICH,{"SchemaVersion":2,"PackCode":"US-CCSS-MATH","ContentVersion":"g11-u01-l02-source-aligned-20261106","Lessons":[rich]})
ASSESS=Path("src/Edulytics.Core/Curriculum/LessonContent/Assessment/alg2-u01-l02-source-teacher-led-20261106.json")
assess={"LessonCode":CODE,"SourceEdition":"Kendall Hunt Algebra 2 local archive (copyright 2019 footer)","SkillContracts":[{"SkillId":"sequences.core","QuestionFamilies":["sequences.core.geometric_nth_term","usccss.g11.alg2.u01.l02.paper_piece_count","usccss.g11.alg2.u01.l02.paper_piece_area"],"AnswerTypes":["integer","exact_fraction"],"Readiness":"WRITTEN_NOT_RUN","Forbidden":["irrelevant-arithmetic","unexplained-graph-auto-pass"]}],"TeacherReview":{"Required":True,"Tasks":["warm-up compare four growth patterns","paper cut-and-stack table with lengths and units","graph of count and area on integer domain","triangle subdivision pictures and interpretation","credit-card monthly growth with financial rounding","multiple valid geometric starting values"],"ScoringRubric":["correct factor and recurrence","correct table and units","justified discrete graphs","explanation of preserved area"]},"FirstPartyCoolDown":{"Prompt":"A board of area 96 square centimetres is divided into twice as many equal regions after each operation. From one region at step 0, what are the number and area of regions at step four? State both recursive rules and justify the total area.","Expected":"16 regions, area 6 square centimetres each, constant product 96.","Origin":"EDULYTICS_ORIGINAL","TeacherReviewRequired":True},"TestsAuthoredNotRun":["tools/curriculum/us-ccss-g11-u01-l02-authoring-contract.test.cjs"],"TestsExecuted":"NOT_RUN","StageD":"NOT_RUN","RichV2Path":str(RICH).replace("\\","/")}
dump(ASSESS,assess)
BASE=Path("artifacts/curriculum-delivery/US-CCSS-MATH/G11")
sections=[
 ("PREP-GOALS","teacher/preparation.html","Learning Goals","geometric sequence; growth factor; tables and graphs","TEACHER_LED"),
 ("WU-A","student/index.html","2.1","recognise a ratio of three","DIGITAL"),
 ("WU-B","student/index.html","2.1","recognise a ratio of four","DIGITAL"),
 ("WU-C","student/index.html","2.1","recognise a factor of one-half","DIGITAL"),
 ("WU-D","student/index.html","2.1","recognise a factor of three-quarters","DIGITAL"),
 ("PAPER-TABLE","student/index.html","2.2","8 inch by 10 inch paper; count and area by cuts","DIGITAL"),
 ("PAPER-SIX","student/index.html","2.2","infer step six from step five","DIGITAL"),
 ("PAPER-COUNT-GRAPH","student/index.html","2.2","integer-valued step graph for number of pieces","TEACHER_LED"),
 ("PAPER-AREA-GRAPH","student/index.html","2.2","integer-valued step graph for area per piece","TEACHER_LED"),
 ("PAPER-RULE","student/index.html","2.2","2^n pieces and 80/2^n area","DIGITAL"),
 ("PAPER-CONSERVATION","student/index.html","2.2","invariant total area 80","TEACHER_LED"),
 ("MISSING-01","student/index.html","2.3","1.5 3 6 12 24 48 factor2","DIGITAL"),
 ("MISSING-02","student/index.html","2.3","40 120 360 1080 3240 factor3","DIGITAL"),
 ("MISSING-03","student/index.html","2.3","200 20 2 0.2 0.02 0.002 factor one tenth","DIGITAL"),
 ("MISSING-04","student/index.html","2.3","1/7 3/7 9/7 27/7 81/7 factor3","DIGITAL"),
 ("MISSING-05","student/index.html","2.3","24 12 6 3 1.5 factor one-half","DIGITAL"),
 ("MISSING-FACTORS","student/index.html","2.3","state growth factor for each list","DIGITAL"),
 ("PRACTICE-01","student/practice.html","Problem 1","2 4 geometric three successor terms","DIGITAL"),
 ("PRACTICE-02","student/practice.html","Problem 2","calculate growth factors of multiple sequences","DIGITAL"),
 ("PRACTICE-03","student/practice.html","Problem 3","1000 balance grows 2 percent monthly without payment","TEACHER_LED"),
 ("PRACTICE-04-COUNT","student/practice.html","Problem 4","triangle substitution count 3^n","DIGITAL"),
 ("PRACTICE-04-AREA","student/practice.html","Problem 4","triangle substitution area per piece 256/4^n","DIGITAL"),
 ("PRACTICE-04-GRAPH","student/practice.html","Problem 4","compare separate charts of count vs area","TEACHER_LED"),
 ("PRACTICE-05","student/practice.html","Problem 5","start10 or 1, recurrence next=3*previous-4","TEACHER_LED"),
 ("PRACTICE-06","student/practice.html","Problem 6","construct two distinct valid recurrences beginning 1,-1","TEACHER_LED"),
 ("TEACH-MATERIALS","teacher/preparation.html","Required Preparation","scissors and student paper models optional","TEACHER_LED")
]
elements=[{"SourceElementId":f"G11-U01-L02-{sid}","SourceLocator":ref+"#"+sec,"MathRequirement":math,"ImplementationType":mode,"Status":"MATCHED_TEACHER_LED" if mode=="TEACHER_LED" else "MATCHED_DIGITAL","TargetArtifact":str(RICH).replace("\\","/"),"StageDRuntime":"NOT_RUN","Rights":"CC_BY_4_SOURCE_CONTEXT"} for sid,ref,sec,math,mode in sections]
E=BASE/"u01-l02-element-coverage.json";dump(E,{"LessonCode":CODE,"Documents":hashes,"Elements":elements,"Count":len(elements),"TestsExecuted":"NOT_RUN","PublisherCoolDown":"restricted; independent Edulytics item authored","SourceVersion":"copyright 2019, version within archival metadata not independently stamped"})
CP=BASE/"u01-l02-abc-checkpoint.json"
dump(CP,{"LessonCode":CODE,"Ordinal":2,"StartedUtc":NOW,"ClosedUtc":NOW,"PhaseA":"SOURCE_INVENTORIED_AND_REVIEWED","PhaseB":"FIRST_PARTY_RICH_V2_AUTHORED_NOT_RUNTIME_PROVEN","PhaseC":"EXACT_FAMILIES_AND_TEACHER_RUBRIC_AUTHORED_NOT_RUN","Status":"READY_FOR_STAGE_D","ReadyForStageD":True,"SourceElements":len(elements),"SourceEvidencePath":str(E).replace("\\","/"),"AssessmentPath":str(ASSESS).replace("\\","/"),"RichV2Path":str(RICH).replace("\\","/"),"TestsExecuted":"NOT_RUN","StageD":"NOT_RUN"})
ledgerp=BASE/"abc-independent-course-ledger.json";ledger=json.loads((ROOT/ledgerp).read_text(encoding="utf8"));assert len(ledger["LessonEntries"])==1 and ledger["LessonEntries"][-1]["Ordinal"]==1;ledger["LessonEntries"].append({"Ordinal":2,"LessonCode":CODE,"Unit":1,"Lesson":2,"ABCReadyForD":True,"Status":"READY_FOR_STAGE_D","ClosedUtc":NOW,"SourceCheckpoint":str(CP).replace("\\","/"),"ElementEvidence":str(E).replace("\\","/"),"AssessmentPath":str(ASSESS).replace("\\","/"),"TestsExecuted":"NOT_RUN","StageD":"NOT_RUN"});ledger["UpdatedUtc"]=NOW;dump(ledgerp,ledger)
p=BASE/"full-grade-progress.json";progress=json.loads((ROOT/p).read_text(encoding="utf8"));assert progress["LessonsReadyForStageD"]==1;progress.update({"LessonsReadyForStageD":2,"LessonsRemaining":118,"LastClosedLesson":CODE,"FirstOpenLesson":"PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:U01:L03","UpdatedUtc":NOW});dump(p,progress)
print("U01_L02_AC_READY_SOURCE_ELEMENTS",len(elements),"TESTS_NOT_RUN")
