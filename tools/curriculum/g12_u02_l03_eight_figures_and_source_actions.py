from pathlib import Path
import json,re
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
def read(f):return json.loads((base/f).read_text(encoding="utf-8"))
def put(f,o):(base/f).write_text(json.dumps(o,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
source=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit02\lesson003.json").read_text(encoding="utf-8"))
descs={
1:"Original travel-cost motivation photograph; independent fixed-plus-variable cost model",
2:"Right-angle diagram of two people walking perpendicularly at constant speeds; independent 6–8–10 right-triangle model",
3:"Distance components and time on the same right-angle geometry; independent d(t) with correct units",
4:"Route and perpendicular side road projection in map coordinates; independent right-triangle projection example",
5:"System graphs yielding one, infinitely many or no solution; independent slope/intercept system classification",
6:"Competing rental plan cost line intersection graph; independent $40+1.5d versus $75+0.8d",
7:"Original graph profits over time from year1980; independent y=120+2t profit graph and contextual intercept read",
8:"Second original profit graph showing decreasing returns; independent y=400−10t contextual graph"
}
assert len(descs)==8
items=[]
for n,skill in descs.items():
 pages=[x["PdfPage"]for x in source["Pages"]if re.search(r'Figure\s*'+str(n)+r'\b',x["Text"],re.I)]
 assert pages,n
 items.append({"SourceFigureNumber":n,"SourceOriginalPdfPage":min(pages),
 "FirstPartyMathSkillRepresentation":skill,
 "OriginalPublisherOrThirdPartyImagesCopied":False,
 "OriginalSourceDiagramPixelEqualityCertified":False,
 "StudentVisualRuntime":"NOT_RUN"})
put("u02-l03-original-eight-figure-source-skill-crosswalk.json",{
"SchemaVersion":1,"SourceSHA256":source["SourceSha256"],"OriginalSourceFigureAnchors":8,
"FirstPartyMathFigureSkillAlternatives":8,"OriginalPublisherOrThirdPartyImagesCopied":0,
"ExactOriginalSourceFigurePixelsOrVerbatimClaim":False,
"StageD":"NOT_RUN","Items":items})
qs=read("u02-l03-source58-firstparty-question-contracts.json")
leaves=[]
for q in qs["Items"]:
 for i,action in enumerate(q["OriginalNumberedSourceNamedActions"],1):
  leaves.append({
  "SourceID":f"G12-U02-L03-EX{q['Number']:03d}-ACTION{i:02d}",
  "OriginalSourceExerciseParentNumber":q["Number"],
  "OriginalPDFPage":q["OriginalSourcePdfPage"],
  "IndependentFirstPartyStudentCognitiveAction":action,
  "TeacherAssessmentQuestionNumber":q["Number"],
  "OriginalVerbatimQuestionOrPublisherGraphicCopied":False,
  "StageD":"NOT_RUN"})
assert len(qs["Items"])==58 and len(set(x["SourceID"]for x in leaves))==len(leaves)
put("u02-l03-original-numbered-exercise-cognitive-action-leaves.json",{
"SchemaVersion":1,"OriginalSourceNumberedExerciseParents":58,
"OriginalNamedExerciseCognitiveActions":len(leaves),
"FullPublisherArtAndProseAtomicDenominatorCertified":False,
"FirstPartyEquivalentCognitiveActionAlternatives":len(leaves),
"ExactSourceNumberAndDiagramPixelQuestionMatchingClaim":False,
"StageD":"NOT_RUN","Items":leaves})
cp=read("u02-l03-abc-checkpoint.json")
cp["OriginalSourceFigureMathSkillsMapped"]=8
cp["SourceExerciseCognitiveActionLeavesAuthored"]=len(leaves)
cp["SourceEightFigureCrosswalk"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u02-l03-original-eight-figure-source-skill-crosswalk.json"
cp["Source58ExerciseActionLeaves"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u02-l03-original-numbered-exercise-cognitive-action-leaves.json"
cp["ReadyForStageD"]=False
put("u02-l03-abc-checkpoint.json",cp)
print("G12_U02_L03_8_SOURCE_FIGURES_AND_"+str(len(leaves))+"_COGNITIVE_ACTIONS_MAPPED")
