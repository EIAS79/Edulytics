"""U01:L04 source-specific spreadsheet and graphing authoring; no test or CI execution."""
from pathlib import Path
import json,hashlib,datetime
ROOT=Path(__file__).resolve().parents[2]; BASE=Path("artifacts/curriculum-delivery/US-CCSS-MATH/G11")
CODE="PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:U01:L04"; NOW=datetime.datetime.now(datetime.timezone.utc).isoformat()
def write(p,x):
 q=ROOT/p;q.parent.mkdir(parents=True,exist_ok=True);q.write_text(json.dumps(x,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
inv=json.loads((ROOT/BASE/"local-archive-source-inventory-20261106.json").read_text(encoding="utf8"))["lessons"][3];assert inv["lessonCode"]==CODE
assert all(hashlib.sha256(Path(v["path"]).read_bytes()).hexdigest()==v["sha256"] for v in inv["documents"].values())
blueprint=json.loads((ROOT/"src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/us-ccss-math-hs-traditional-algebra2-im1e.lesson-blueprint.json").read_text(encoding="utf8"))["Lessons"][3]; assert blueprint["LessonCode"]==CODE
PACK=Path("src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-hs-alg2-phase29-v1.lesson-content-pack.json")
pack=json.loads((ROOT/PACK).read_text(encoding="utf8"));tr=next(t for l in pack["lessons"] if l["lessonCode"]==CODE for t in l["translations"] if t["cultureCode"]=="en")
tr.update({
"explanation":"Learn how a spreadsheet uses cell references and formula fill-down to generate recursive sequences; use graphing technology to display values at integer positions. Example inputs A1=2,A2=3,A3=-10,A4=0.2 yield B1=A1+A2=5, B2=A3*A4=-2, B3=B1+333=338 and B4=abs(B2)=2. Entering a formula begins with '=', whereas a value or text entry does not recalculate a referenced formula. To generate 10,13,16,... set A1=10 and A2= A1+3, then fill down. For 16,8,4,... use B1=16,B2=B1*0.5. Read ratios and differences in adjacent rows to verify the type.",
"keyConceptsAndRules":"Cell references are addresses. Relative reference A1 in formula =A1+3 changes to A2 when copied down one row. Arithmetic sequence in a sheet: =previous+constant; geometric sequence =previous*constant. For list 700,70,7,.7,.07 use successive quotient =C2/C1 =0.1; for 7,10.5,14,17.5 successive difference =E2-E1 =3.5. Graphing (n,a(n)) plots term number horizontally and value vertically. To ensure plotted points form a function, no input n can pair with two different outputs.",
"workedExamples":"Spreadsheet formula demonstration: B1=2+3=5; B2=-10*0.2=-2; B3=5+333=338; B4=abs(-2)=2. Fill-down examples: initial 12 and +3 yields 12,15,18; initial 12 and -1 yields 12,11,10; initial 10 and factor3 yields 10,30,90. For a table of term indices 0 through9, formula =2*A1+5 in B1 and fill down gives 5,7,9,...,23. The sequence 8,12,18,27,40.5 is geometric factor1.5, while 50,42.1,34.2,26.3 is arithmetic difference-7.9.",
"stepByStepSolutions":"1. Enter the starting value in the first row. 2. Type an equals sign and a formula that references the previous row. 3. Fill down and check the first three calculated entries by hand. 4. Use adjacent subtraction or quotient columns, taking care not to divide by zero. 5. To graph, put integer term indices into column one and output values into column two. 6. For each graph check whether one input has more than one output and explain why a vertical line is or is not valid.",
"commonMistakes":"Typing A1+3 without '=' and expecting a calculation; copying an absolute address when a relative reference is needed; claiming an arithmetic pattern because all outputs rise, without checking constant differences; drawing a continuous curve for a discrete sequence; treating negative x-values as incorrect when plotting valid second-quadrant points.",
"quickSummary":"Spreadsheet cell references and fill-down generate recurrence terms. Adjacent differences reveal arithmetic sequences, adjacent quotients reveal geometric sequences. A table of ordered pairs can be graphed; a function requires exactly one output for each input."})
write(PACK,pack)
RICH=Path("src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-alg2-u01-l04.rich-lesson-v2.json")
rich={"LessonCode":CODE,"CultureCode":"en","Title":blueprint["Title"],
"ExplanationParagraphs":[
"Learning goal: use spreadsheet and graphing technology to generate a sequence from a recurrence and represent sequence terms as a graph of ordered pairs.",
"Optional technology workshop (teacher-led): open a spreadsheet; enter A1=2,A2=3,A3=-10,A4=1/5; then enter B1=A1+A2, B2=A3*A4, B3=B1+333 and B4=abs(B2). Results are 5,-2,338,2. Change one A-value and explain how dependent B-cells update. Try a power formula using ^ and note that the equals sign is required.",
"Fill-down: start at 10 and type =A1+3 in A2. Show 10,13,16,...; change the first entry to 12 to get 12,15,18,... and change +3 to -1 to get 12,11,10,... . Explain how A2 becomes A3 when copied to the next row.",
"Fill-down for a geometric sequence: start at 16 and use =B1*0.5 for 16,8,4,2,... ; modify it to start 10 and multiply by 3 to make 10,30,90,... .",
"To verify common ratios, place 700,70,7,0.7,0.07 in a column and enter =C2/C1 in the adjacent row, then fill down to get 0.1 repeatedly. For a second list 7,10.5,14,17.5 use =E2-E1 to get difference 3.5 repeatedly.",
"The list 8,12,18,27,40.5 has factor 1.5 and is geometric. The list 50,42.1,34.2,26.3 subtracts 7.9 each time and is arithmetic. Compare adjacent values with a table; do not infer from shape alone.",
"Graphing workshop: create a table with one column of term indices and a column of term values, then plot the discrete ordered pairs. Alter the table to form an upward sloping diagonal line, a horizontal line, and a relation with repeated input but different output (not a function). Make a nonlinear list and a negative input with positive output in quadrant II.",
"Model a line using integer inputs n from 0 to 9. The rule y=2n+5 yields outputs 5,7,9,...23. Filling down a formula =2*A1+5 is different from filling a previous-row recurrence; explain both.",
"Practice: write spreadsheet formulas to generate doubling and +2 from 2, halving and -75 from 400, and specific five-term recursive lists from starts 5 or 1 and rules 3*previous+2 or 3*(previous+2). Check signed/fractional growth factors including -1 and 0.5.",
"Independent original cool-down: enter a starting value 6, then generate the next five terms with formula =2*previous-3; explain which cell reference you would use in the second row, write the output table, and plot the first three discrete points. Teacher checks spreadsheet formulas and graph, not simply numeric answers."
],
"KeyConcepts":[
{"Title":"Relative spreadsheet reference","Definition":"Cell address whose row/column adjusts when a formula is copied.","Rule":"=A1+3 in A2 becomes =A2+3 in A3 after fill-down.","Example":"10,13,16 from A1=10."},
{"Title":"Common ratio and difference","Definition":"Multiplicative ratio or additive change between consecutive terms.","Rule":"Ratio = new/previous (when previous nonzero); difference = new-previous.","Example":"700,70,7 ratio 0.1; 7,10.5,14 difference 3.5."},
{"Title":"Discrete sequence graph","Definition":"Set of (term number,value) coordinate pairs.","Rule":"Each input position has exactly one output for a function.","Example":"n=0,1,2 with y=2n+5 plots (0,5),(1,7),(2,9)."}],
"WorkedExamples":[
{"Title":"Spreadsheet reference chain","Question":"Compute B1=A1+A2, B2=A3*A4, B3=B1+333, B4=abs(B2) for A=[2,3,-10,1/5].","Method":"Evaluate referenced cells in order.","Steps":["B1=2+3=5.","B2=-10*(1/5)=-2.","B3=5+333=338.","B4=|-2|=2."],"Answer":"5,-2,338,2","Check":"All inputs come from the given A-cells or previously computed B-cells."},
{"Title":"Geometric ratio via fill-down","Question":"What value appears in every ratio cell next to 700,70,7,0.7,0.07?","Method":"Divide each new term by the immediately preceding term.","Steps":["70/700=0.1.","7/70=0.1.","0.7/7=0.1 and 0.07/0.7=0.1."],"Answer":"0.1","Check":"Same ratio throughout."},
{"Title":"Produce integer-indexed linear values","Question":"For n=0,...,9 compute y=2n+5 and show first and last points.","Method":"Apply the function to each index.","Steps":["n=0 gives y=5.","n=1 gives y=7.","n=9 gives y=23."],"Answer":"First (0,5), last (9,23).","Check":"Difference in y is 2 for each step of n."}],
"CommonMistakes":[{"Mistake":"Omitting = from a formula.","WhyWrong":"Spreadsheet may store text instead of a calculated expression.","Correction":"Start each computed formula with = and verify a referenced cell changes results."},
{"Mistake":"Mixing previous-row references when filling down.","WhyWrong":"It changes the recurrence and produces a different sequence.","Correction":"Check the copied formula references the immediately preceding row."},
{"Mistake":"Claiming points are a function despite repeated inputs with different outputs.","WhyWrong":"A function cannot assign multiple outputs to one input.","Correction":"Review the input column and graph using the vertical-line test."}],
"SummaryPoints":["Use spreadsheet formulas and relative references to generate sequences.","Adjacent differences and quotients test arithmetic and geometric patterns.","Plot discrete (term, value) pairs and test function validity.","Actual spreadsheet and graph actions are evaluated by a teacher; unrun tests remain Stage D."],
"Visuals":[],"Videos":[]}
write(RICH,{"SchemaVersion":2,"PackCode":"US-CCSS-MATH","ContentVersion":"g11-u01-l04-source-aligned-20261106","Lessons":[rich]})
ASSESS=Path("src/Edulytics.Core/Curriculum/LessonContent/Assessment/alg2-u01-l04-source-teacher-led-20261106.json")
write(ASSESS,{"LessonCode":CODE,"BlueprintAlignments":blueprint["Alignments"],"SourceCategory":"optional spreadsheet and graphing technology workshop","Contract":{"Skills":["sequence table from recursive formula","relative cell references","classify constant differences and ratios","ordered pair function check"],"AllowedQuestionFamilies":["sequences.core.arithmetic_nth_term","sequences.core.geometric_nth_term","usccss.g11.alg2.u01.l01.affine_recurrence"],"Readiness":"WRITTEN_NOT_RUN","ManualTeacherRequired":True,"DoNotClaimIntegratedSpreadsheet":True},"TeacherReview":{"Tasks":["actual spreadsheet relative reference entry and fill down","plot discrete points","test function by repeated input","explain zero or negative term handling"],"Rubric":["formula syntax","input/output consistency","correct difference or ratio","correct graph and axis labels"]},"FirstPartyCoolDown":{"Prompt":"In a spreadsheet start A1=6 and use A2=2*A1-3, then fill down for six terms. State the row formulas and graph the first three values as points indexed from n=1.","Expected":"6,9,15,27,51,99; (1,6),(2,9),(3,15).","Origin":"EDULYTICS_ORIGINAL","TeacherGraded":True},"StageD":"NOT_RUN","TestsExecuted":"NOT_RUN","TestsAuthoredNotRun":["tools/curriculum/us-ccss-g11-u01-l04-authoring-contract.test.cjs"]})
spec=[
("GOALS","teacher/preparation.html","Learning Goals","spreadsheet and graphing learning outcomes"),
("CELL-VALUES","student/index.html","4.1","A1=2,A2=3,A3=-10,A4=1/5"),
("CELL-FORMULAS","student/index.html","4.1","B1 B2 B3 B4 formulas and resulting 5,-2,338,2"),
("CELL-EDITS","student/index.html","4.1","change input cells, observe dependent formula"),
("CELL-POWER","student/index.html","4.1","power operation syntax and omitted equal sign"),
("FILL-ARITH","student/index.html","4.2","10,+3; 12,+3; 12,-1"),
("FILL-GEO","student/index.html","4.2","16,*0.5; 10,*3"),
("RATIO-COLUMN","student/index.html","4.2","700,70,7,.7,.07 quotients .1"),
("DIFF-COLUMN","student/index.html","4.2","7,10.5,14,17.5 differences 3.5"),
("CLASSIFY-GEO","student/index.html","4.2","8,12,18,27,40.5 ratio 1.5"),
("CLASSIFY-ARITH","student/index.html","4.2","50,42.1,34.2,26.3 difference -7.9"),
("TABLE-FUNCTION","student/index.html","4.2","n=0..9 y=2n+5"),
("GRAPH-LINE","student/index.html","4.3","points on positive slope line"),
("GRAPH-CONSTANT","student/index.html","4.3","horizontal line"),
("GRAPH-NOT-FUNCTION","student/index.html","4.3","same input with multiple outputs"),
("GRAPH-NEW-SLOPE","student/index.html","4.3","change expression to alter steepness"),
("GRAPH-NONLINEAR","student/index.html","4.3","curved/nonlinear discrete pattern"),
("GRAPH-QII","student/index.html","4.3","negative x positive y quadrant two"),
("P1","student/practice.html","Problem 1","2 times2 or plus2 formula"),
("P2","student/practice.html","Problem 2","400 times .5 or minus75 formula"),
("P3","student/practice.html","Problem 3","A1=5 or1 with 3*previous+2 versus3*(previous+2)"),
("P4","student/practice.html","Problem 4","negative ratio or fractional ratio and signed arithmetic rate"),
("P5","student/practice.html","Problem 5","read graph of term number versus value"),
("P6","student/practice.html","Problem 6","6,3 geometric factor half"),
("TEACH-TOOLS","teacher/preparation.html","Required Preparation","spreadsheet and graphing technology access")]
elements=[{"SourceElementId":f"G11-U01-L04-{key}","SourceLocator":file+"#"+section,"MathematicalRequirement":req,"ImplementationType":"TEACHER_LED" if key.startswith(('GRAPH','CELL','FILL','TEACH','P5')) else "DIGITAL","Status":"MATCHED_TEACHER_LED" if key.startswith(('GRAPH','CELL','FILL','TEACH','P5')) else "MATCHED_DIGITAL","TargetArtifact":str(RICH).replace("\\","/"),"TestState":"WRITTEN_NOT_RUN"} for key,file,section,req in spec]
E=BASE/"u01-l04-element-coverage.json";write(E,{"LessonCode":CODE,"SourceDocuments":inv["documents"],"Count":len(elements),"Elements":elements,"TestsExecuted":"NOT_RUN","Edition":"Kendall Hunt 2019 archive"})
CP=BASE/"u01-l04-abc-checkpoint.json";write(CP,{"LessonCode":CODE,"Ordinal":4,"Status":"READY_FOR_STAGE_D","A":"SOURCE_ALIGNED","B":"RICH_V2_TEACHER_TECH_WORKSHOP_AUTHORED","C":"SOURCE_SCOPED_PRACTICE_REUSED_AND_HUMAN_RUBRIC_AUTHORED","AssessmentPath":str(ASSESS).replace("\\","/"),"ElementEvidence":str(E).replace("\\","/"),"RichV2":str(RICH).replace("\\","/"),"TestsExecuted":"NOT_RUN","StageD":"NOT_RUN","ClosedUtc":NOW})
ledgerp=BASE/"abc-independent-course-ledger.json";ledger=json.loads((ROOT/ledgerp).read_text(encoding="utf8"));assert len(ledger["LessonEntries"])==3 and ledger["LessonEntries"][-1]["Ordinal"]==3
ledger["LessonEntries"].append({"Ordinal":4,"LessonCode":CODE,"Unit":1,"Lesson":4,"ABCReadyForD":True,"Status":"READY_FOR_STAGE_D","ClosedUtc":NOW,"SourceCheckpoint":str(CP).replace("\\","/"),"ElementEvidence":str(E).replace("\\","/"),"AssessmentPath":str(ASSESS).replace("\\","/"),"TestsExecuted":"NOT_RUN","StageD":"NOT_RUN"});ledger["UpdatedUtc"]=NOW;write(ledgerp,ledger)
progressp=BASE/"full-grade-progress.json";progress=json.loads((ROOT/progressp).read_text(encoding="utf8"));assert progress["LessonsReadyForStageD"]==3
progress.update({"LessonsReadyForStageD":4,"LessonsRemaining":116,"LastClosedLesson":CODE,"FirstOpenLesson":"PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:U01:L05","UpdatedUtc":NOW});write(progressp,progress)
print("G11_U01_L04_TECH_SOURCE_ALIGNED",len(elements),"SOURCE_TASKS_AUTHORING_READY_TESTS_NOT_RUN")
