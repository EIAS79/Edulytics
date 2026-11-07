from pathlib import Path
import json,re
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
def read(name):return json.loads((base/name).read_text(encoding="utf-8"))
def put(name,data):(base/name).write_text(json.dumps(data,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
source=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit02\lesson001.json").read_text(encoding="utf-8"))
assert source["Section"]=="2.1" and source["Pages"][0]["PdfPage"]==141 and source["Pages"][-1]["PdfPage"]==157
q=read("u02-l01-original76-firstparty-question-contracts.json")
native=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u02-l01-firstparty-teacher-led.json"
a=json.loads(native.read_text(encoding="utf-8"))
assert q["Items"][57]["Number"]==58
q["Items"][57]["TeacherExpectedMathAndAssessment"]="Slope m=−112/31≈−3.613; intercept −190/31≈−6.129; a=k(67.5)=−250; missing input b=587/56≈10.482. k(w)=−(112/31)w−190/31; rounded k(w)≈−3.613w−6.129."
a["TeacherOnlyMathRubrics"][57]["Expected"]=q["Items"][57]["TeacherExpectedMathAndAssessment"]
put("u02-l01-original76-firstparty-question-contracts.json",q)
native.write_text(json.dumps(a,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
descriptions={
1:"Original transit photo motivating constant-speed motion; independent distance/time example with slope and initial value; external photo not reused",
2:"Time-distance tabular constant increment 83 m per second; independent tabulated linear growth",
3:"The corresponding constant-rate line graph; independent two-point and intercept representation",
4:"Original diver pressure and depth photograph; independent physical-unit linear pressure model",
5:"Positive, negative and zero slopes compared graphically; independent monotonicity classification and table",
6:"Slope rise-over-run between two plotted points; independent slope worked examples",
7:"Point-slope versus slope-intercept line representations; independent point-slope example 5",
8:"Line through slope and initial point; independent worked example 6",
9:"Two labelled coordinates on original graph with decreasing slope; independent graph coordinate slope tasks 38–40",
10:"Unknown slope from visually read coordinates on one line; independent two-point graph equation tasks 41–46",
11:"Y-intercept and chosen point annotated on graphed line; independent graph equation task group",
12:"Initial value and monthly linear changes in a diagram; independent count/time line graph model"
}
assert len(descriptions)==12
fig=[]
for n,desc in descriptions.items():
 page_numbers=[p["PdfPage"]for p in source["Pages"]if re.search(r"Figure\s*"+str(n)+r"\b",p["Text"],re.I)]
 assert page_numbers,(n,"source figure absent")
 fig.append({"SourceFigureNumber":n,"SourcePdfPage":min(page_numbers),
 "FirstPartyEquivalentPedagogicRepresentation":desc,
 "OriginalPublisherThirdPartyImageReproduced":False,
 "PublisherDiagramExactCoordinateOrPixelIdentityClaim":False,
 "StageDVisualRuntime":"NOT_RUN"})
put("u02-l01-original12-figure-source-skill-crosswalk.json",{
"SchemaVersion":1,"LocalSourceBook":"OpenStax Precalculus 2017 Section 2.1",
"SourceSha256":source["SourceSha256"],"OriginalFigureAnchors":12,
"FirstPartyFigureCognitiveSkillAlternativesMapped":12,
"OriginalCopyrightedAndThirdPartyPhotographsCopied":False,
"ExactPublisherGraphPixelParityCertified":False,
"Items":fig,"StageD":"NOT_RUN"})
assert len(q["Items"])==76
actions=[]
for record in q["Items"]:
 for index,action in enumerate(record["RequiredCognitiveActions"],1):
  actions.append({
   "SourceID":f"G12-U02-L01-EX{record['Number']:03d}-ACTION{index:02d}",
   "SourceNumberedExercise":record["Number"],
   "OriginalSourcePdfPage":record["OriginalSourcePDFPage"],
   "FirstPartyStudentPedagogicAction":action,
   "TeacherManualAssessmentNumber":record["Number"],
   "ExactOriginalSourceGraphPixelMatchingClaim":False,
   "ExactPublisherVerbatimQuestionMatchClaim":False,
   "Runtime":"NOT_RUN"})
assert len(actions)>76 and len({a["SourceID"]for a in actions})==len(actions)
put("u02-l01-original-exercise-action-leaves.json",{
"SchemaVersion":1,
"OriginalSourceNumberedExercises":76,
"OriginalNumberedExerciseSourceSkillActionLeaves":len(actions),
"DenominatorOnlyExplicitNumberedActions_NotEveryBookParagraphOrPixel":True,
"FirstPartyStudentManualAssessmentActionsMapped":len(actions),
"OriginalSourceExactWordingOrGraphicsParityCertified":False,
"StageD":"NOT_RUN","Items":actions})
cp=read("u02-l01-abc-checkpoint.json")
cp.update({
"SourceOriginalFigureSkillsReviewed":12,
"SourceOriginalNumberedExerciseActionLeaves":len(actions),
"FigureSkillCrosswalkPath":"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u02-l01-original12-figure-source-skill-crosswalk.json",
"ExerciseActionLedgerPath":"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u02-l01-original-exercise-action-leaves.json",
"SourceFigureSkills":"12_ORIGINAL_FIGURE_COGNITIVE_ACTIONS_FIRSTPARTY_MODELLED_NO_SOURCE_BITMAP_REUSED",
"ReadyForStageD":False})
put("u02-l01-abc-checkpoint.json",cp)
print("U02_L01_SOURCE_FIGURES12_MAPPED_EXERCISE_ACTIONS_"+str(len(actions))+"_TEACHER_KEY58_FIXED")
