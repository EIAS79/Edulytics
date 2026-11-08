from pathlib import Path
import json,re
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
def read(f):return json.loads((base/f).read_text(encoding="utf-8"))
def put(f,v):(base/f).write_text(json.dumps(v,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
source=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit02\lesson002.json").read_text(encoding="utf-8"))
descriptions=[
"Three plotted collinear points and slope verification",
"Graph reference illustrating rise and run from a y-intercept",
"Constructing a line from intercept plus negative slope",
"Vertical scaling of identity toolkit y=x",
"Vertical output translation of identity toolkit",
"Intermediate identity graph scaled horizontally/vertically",
"Result of scaling identity and shifting output",
"Identifying linear equation by two plotted coordinates",
"Matching four distinct slope/intercept lines to visually labelled plots",
"Same four lines independently labelled by formulas",
"Horizontal line graph y=constant and no x-intercept where nonzero",
"Exact x-intercept of a sloping function",
"Constant horizontal output on y=2",
"Vertical line x=2 violating single-valued y=f(x)",
"Read equation of horizontal line from graph",
"Read equation of vertical line from graph",
"Two lines of same slope and distinct y-intercepts",
"Two perpendicular lines with negative reciprocal slope",
"Compare mixed pair of parallel and perpendicular lines",
"Construct parallel line through given point by equal slope",
"Construct perpendicular through point with negative reciprocal slope",
"Intersection of two linear graphs at a labelled coordinate",
"Business break-even comparison of cost and revenue graphs",
"Six labelled candidate line graphs compared with source equations"
]
assert len(descriptions)==24
fig=[]
for n,desc in enumerate(descriptions,1):
 pages=[p["PdfPage"]for p in source["Pages"]if re.search(r"Figure\s*"+str(n)+r"\b",p["Text"],re.I)]
 assert pages,n
 fig.append({"OriginalSourceFigure":n,"SourcePdfPage":min(pages),
 "SourceFigureSkill":"Line-graph source cognitive action: "+desc,
 "IndependentFirstPartyEquivalentRepresentation":"Independent numeric slope/intercept graph/table and/or teacher-review explanation in U02 L02",
 "OriginalPublisherBitmapOrThirdPartyArtworkCopied":False,
 "ExactPublisherImagePixelAndOriginalNumericGraphParityClaim":False,
 "StageDVisualRuntime":"NOT_RUN"})
put("u02-l02-original24-figure-source-skill-crosswalk.json",{
"SchemaVersion":1,"SourceBookSection":"2.2","OriginalSourceSha256":source["SourceSha256"],
"OriginalSourceFigureAnchors":24,
"FirstPartyPedagogicFigureLearningSkillAlternatives":24,
"OriginalPublisherAndThirdPartyArtworkCopied":0,
"FullPublisherGraphicsPixelParityCertified":False,
"StageD":"NOT_RUN","Items":fig})
q=read("u02-l02-original76-firstparty-question-contracts.json")
assert len(q["Items"])==76
leaves=[]
for item in q["Items"]:
 for j,action in enumerate(item["OriginalSourceNamedCognitiveActions"],1):
  leaves.append({
   "SourceID":f"G12-U02-L02-EX{item['Number']:03d}-ACTION{j:02d}",
   "OriginalNumberedSourceExercise":item["Number"],
   "OriginalPDFPage":item["OriginalSourcePDFPage"],
   "IndependentFirstPartyAssessmentCognitiveAction":action,
   "CorrespondingTeacherAssessmentNumber":item["Number"],
   "ExactPublisherSourceGraphOrVerbatimQuestionParity":False,
   "StageDStudentGradebookRuntime":"NOT_RUN"})
assert len({l["SourceID"]for l in leaves})==len(leaves)
put("u02-l02-original-numbered-exercise-cognitive-action-leaves.json",{
"SchemaVersion":1,"OriginalSectionNumberedExerciseParents":76,
"OriginalSectionExerciseNamedCognitiveActionLeaves":len(leaves),
"FirstPartyIndependentPedagogicActionAlternatives":len(leaves),
"ExcludesFullParagraphAndSourceGraphPixelDenominator":True,
"ExactPublisherOriginalNumericalInstanceParityCertified":False,
"StageD":"NOT_RUN","Items":leaves})
cp=read("u02-l02-abc-checkpoint.json")
cp["Original24SourceFigureSkillsCrosswalked"]=24
cp["OriginalNumberedExerciseActionsMapped"]=len(leaves)
cp["SourceGraphFigureSkillReview"]="24_SOURCE_FIGURES_ACADEMIC_MATH_SKILL_ALTERNATIVES_MAPPED_WITHOUT_COPYING_BITMAPS"
cp["SourceFigureCrosswalk"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u02-l02-original24-figure-source-skill-crosswalk.json"
cp["SourceExerciseActionLedger"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u02-l02-original-numbered-exercise-cognitive-action-leaves.json"
cp["ReadyForStageD"]=False
put("u02-l02-abc-checkpoint.json",cp)
print("U02_L02_ORIGINAL_24_FIGURE_SOURCE_SKILLS_CROSSWALKED_"+str(len(leaves))+"_NUMBERED_EXERCISE_ACTIONS")
