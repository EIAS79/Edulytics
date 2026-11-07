from pathlib import Path
import json,re
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
def read(f):return json.loads((base/f).read_text(encoding="utf-8"))
def put(f,v):(base/f).write_text(json.dumps(v,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
source=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit02\lesson004.json").read_text(encoding="utf-8"))
concepts={
1:"Introductory scatterplot relating an input to a school test outcome; independent point plotting with strength/correlation caveat",
2:"Cricket-temperature paired observation scatterplot; independent first-party near-linear five-point scatter dataset",
3:"Straight line hand fit over chirp/temperature observations; independent y≈2.2x+3 visual fit",
4:"Observed x-window showing interpolation versus extrapolation regions; independent x=2.5 vs x=7",
5:"Graph and input domain illustrating location of prediction outside observations; first-party explicit x-range labels",
6:"Least-squares regression line over independent scatter points; first-party LS regression worked example",
7:"Correlation coefficient reference plots and relative linear strength; independent Pearson r key concepts",
8:"Historical gasoline regression versus time; independent regression prediction model and caution",
9:"Two qualitative scatter panels for correlation sign/magnitude; independent r≈+0.95 versus r≈−0.89 classification",
10:"Other qualitative scatter panels with weak or moderate r; independent r=0.26, r=−0.39 classification"
}
fig=[]
for n,meaning in concepts.items():
 pages=[p["PdfPage"]for p in source["Pages"]if re.search(r'Figure\s*'+str(n)+r'\b',p["Text"],re.I)]
 assert pages,n
 fig.append({
 "OriginalPublisherFigureNumber":n,"OriginalSourcePdfPage":min(pages),
 "OriginalFigureMathSkillAndIndependentAlternative":meaning,
 "OriginalPublisherOrThirdPartyImageCopied":False,
 "OriginalExactGraphicCoordinatesAndPixelEqualityCertified":False,
 "StageDGraphRenderingRuntime":"NOT_RUN"})
put("u02-l04-original10-figure-source-skill-crosswalk.json",{
"SchemaVersion":1,"OriginalSourceSha256":source["SourceSha256"],
"SourceOriginalFigureAnchors":10,"IndependentFirstPartyFigureLearningSkillsCovered":10,
"SourcePublisherOriginalBitmapsEmbedded":0,"PublisherPixelExactVisualParityClaim":False,
"StageD":"NOT_RUN","Items":fig})
q=read("u02-l04-original42-source-skill-question-contracts.json")
assert len(q["Items"])==42
actions=[]
for task in q["Items"]:
 for i,action in enumerate(task["SourceNamedCognitiveActions"],1):
  actions.append({
   "SourceID":f"G12-U02-L04-EX{task['Number']:03d}-ACTION{i:02d}",
   "OriginalSourceExerciseNumber":task["Number"],
   "OriginalSourcePdfPage":task["OriginalSourcePDFPage"],
   "FirstPartyIndependentStudentCognitiveAction":action,
   "TeacherAssessmentResourceQuestionNumber":task["Number"],
   "ExactOriginalSourceVerbatimNumericAndGraphicParityCertified":False,
   "StageDStudentTeacherRuntime":"NOT_RUN"})
assert len({x["SourceID"]for x in actions})==len(actions)
put("u02-l04-original-numbered-source-exercise-cognitive-action-leaves.json",{
"SchemaVersion":1,"OriginalSectionNumberedExercises":42,
"SourceNamedExerciseActionLeaves":len(actions),
"FirstPartyMathSkillEquivalentActionsAuthored":len(actions),
"ExcludesAllSourceParagraphAndPixelAtomicDenominator":True,
"OriginalPublisherExactVerbatimAndImageParityClaim":False,
"StageD":"NOT_RUN","Items":actions})
cp=read("u02-l04-abc-checkpoint.json")
cp["OriginalSourceFiguresPedagogicallyMapped"]=10
cp["OriginalNumberedExerciseActionsMapped"]=len(actions)
cp["SourceFigureCrosswalkPath"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u02-l04-original10-figure-source-skill-crosswalk.json"
cp["SourceCognitiveActionLedgerPath"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u02-l04-original-numbered-source-exercise-cognitive-action-leaves.json"
cp["ReadyForStageD"]=False
put("u02-l04-abc-checkpoint.json",cp)
print("U02_L04_10_SOURCE_FIGURE_SKILLS_"+str(len(actions))+"_NUMBERED_EXERCISE_COGNITIVE_ACTIONS_CROSSWALKED")
