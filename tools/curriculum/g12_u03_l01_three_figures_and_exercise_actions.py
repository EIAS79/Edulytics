from pathlib import Path
import json,re
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
def read(f):return json.loads((base/f).read_text(encoding="utf-8"))
def put(f,v):(base/f).write_text(json.dumps(v,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
source=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit03\lesson001.json").read_text(encoding="utf-8"))
desc={
1:"Original plotted complex number −2+3i as point (−2,3); first-party independent complex coordinate table",
2:"Labelled orthogonal real and imaginary axes of Argand plane; first-party corresponding coordinate lookup",
3:"Original 3−4i plotted as (3,−4); first-party independently plotted points with quadrants"
}
fig=[]
for n,skill in desc.items():
 pages=[p["PdfPage"]for p in source["Pages"]if re.search(r'Figure\s*'+str(n)+r'\b',p["Text"],re.I)]
 assert pages,n
 fig.append({"OriginalFigureNumber":n,"OriginalSourcePage":min(pages),
 "CognitiveSourceFigureSkillAndFirstPartyAlternative":skill,
 "SourceOriginalPublisherImageCopied":False,
 "ExactPublisherGraphicPixelIdentityCertified":False,
 "StageDGraphRuntime":"NOT_RUN"})
put("u03-l01-original-three-figure-complex-plane-skill-crosswalk.json",{
"SchemaVersion":1,"SourceSHA256":source["SourceSha256"],
"OriginalSourceFigureAnchors":3,"FirstPartyIndependentComplexPlaneFigureSkillAlternatives":3,
"PublisherOriginalGraphicsCopied":0,"ExactPublisherVisualImagePixelParityCertified":False,
"StageD":"NOT_RUN","Items":fig})
questions=read("u03-l01-source58-independent-complex-question-contracts.json")
leaves=[]
for q in questions["Items"]:
 for i,action in enumerate(q["StudentCognitiveActions"],1):
  leaves.append({
  "SourceID":f"G12-U03-L01-EX{q['Number']:03d}-ACTION{i:02d}",
  "OriginalNumberedSourceExercise":q["Number"],
  "OriginalSourcePDFPage":q["SourcePdfPage"],
  "FirstPartyIndependentPedagogicAction":action,
  "TeacherComplexMathAssessmentNumber":q["Number"],
  "ExactOriginalVerbatimSourceQuestionOrGraphicCopied":False,
  "StageD":"NOT_RUN"})
assert len(questions["Items"])==58 and len({x["SourceID"]for x in leaves})==len(leaves)
put("u03-l01-source-numbered-exercise-action-leaves.json",{
"SchemaVersion":1,"OriginalNumberedExerciseParents":58,
"OriginalNumberedExerciseCognitiveActionLeaves":len(leaves),
"FirstPartyIndependentCognitiveMathActions":len(leaves),
"ExcludesEntirePublisherParagraphAndGraphicPixelDenominator":True,
"ExactOriginalPublisherTextAndImageParityClaim":False,
"StageD":"NOT_RUN","Items":leaves})
cp=read("u03-l01-abc-checkpoint.json")
cp["OriginalFigureAcademicMathSkillsCrosswalked"]=3
cp["OriginalSourceNumberedExerciseCognitiveActions"]=len(leaves)
cp["SourceFigureCrosswalk"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u03-l01-original-three-figure-complex-plane-skill-crosswalk.json"
cp["SourceExerciseActionLedger"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u03-l01-source-numbered-exercise-action-leaves.json"
cp["ReadyForStageD"]=False
put("u03-l01-abc-checkpoint.json",cp)
print("G12_U03_L01_THREE_COMPLEX_PLANE_SOURCE_FIGURE_SKILLS_"+str(len(leaves))+"_EXERCISE_ACTIONS_MAPPED")
