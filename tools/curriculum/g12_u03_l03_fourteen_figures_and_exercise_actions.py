from pathlib import Path
import json,re
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
def read(n):return json.loads((base/n).read_text(encoding="utf-8"))
def put(n,v):(base/n).write_text(json.dumps(v,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
src=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit03\lesson003.json").read_text(encoding="utf-8"))
descriptions=[
"Original bird habitat population introduction photo and polynomial growth; independent x³ polynomial model",
"Even positive powers x²,x⁴,x⁶ have both tails up; independent degree and leading sign table",
"Odd positive powers x³,x⁵,x⁷ have opposite tails; independent tail table",
"Four end-behavior combinations by degree parity and leading sign; independent summary",
"Even positive leading power plotted with both tails rising; independent table",
"Odd negative leading power with opposite tails; independent −2x⁵ worked example",
"Graph-based possible degree from endpoints and turning points; first-party turning-point reasoning",
"Additional graph-based degree and end behavior; independent four-tail table",
"Graph showing labelled roots, y-intercept and turning points; first-party factored model",
"Real roots and y-intercept of cubic with simple factors; independent three-zero polynomial",
"Graph of even quartic factored as quadratic in x²; first-party four-root example",
"Graph reasoning from number of roots and turns; first-party theoretical degree bound",
"Even-degree polynomial graph and turning points illustrating upper bound; first-party even-tail table",
"Independent source graph reasoning on number of zeros and turning points; first-party example for source TryIt8"
]
assert len(descriptions)==14
fig=[]
for n,desc in enumerate(descriptions,1):
 pages=[p["PdfPage"]for p in src["Pages"]if re.search(r"Figure\s*"+str(n)+r"\b",p["Text"],re.I)]
 assert pages,n
 fig.append({
 "SourceFigureNumber":n,"OriginalPdfPage":min(pages),
 "OriginalMathFigureAcademicSkillAndIndependentAlternative":desc,
 "OriginalPublisherBitmapOrThirdPartyArtworkCopied":False,
 "ExactPublisherNumericCoordinatesAndGraphicPixelParityCertified":False,
 "StageDVisualRuntime":"NOT_RUN"})
put("u03-l03-original14-polynomial-figure-source-skill-crosswalk.json",{
"SchemaVersion":1,"OriginalBookSha256":src["SourceSha256"],
"SourceOriginalFigureAnchors":14,
"FirstPartyIndependentFigureMathSkillAlternatives":14,
"OriginalPublisherOriginalArtworkCopied":0,
"OriginalPublisherExactGraphPixelMatchingCertified":False,
"StageD":"NOT_RUN","Items":fig})
q=read("u03-l03-original70-power-polynomial-question-contracts.json")
leaves=[]
for task in q["Items"]:
 for i,action in enumerate(task["SourceNamedCognitiveActions"],1):
  leaves.append({
   "SourceID":f"G12-U03-L03-EX{task['Number']:03d}-ACTION{i:02d}",
   "OriginalNumberedSourceExercise":task["Number"],
   "OriginalSourcePdfPage":task["OriginalSourcePDFPage"],
   "IndependentStudentMathCognitiveAction":action,
   "TeacherAssessmentQuestionNumber":task["Number"],
   "ExactOriginalPublisherGraphicTextNumericQuestionParityCertified":False,
   "StudentTeacherRuntime":"NOT_RUN"})
assert len(q["Items"])==70 and len({x["SourceID"]for x in leaves})==len(leaves)
put("u03-l03-original70-exercise-action-leaves.json",{
"SchemaVersion":1,"OriginalSourceNumberedExerciseParents":70,
"OriginalSourceNamedExerciseActionLeaves":len(leaves),
"IndependentFirstPartyMathActionAlternatives":len(leaves),
"AllPublisherParagraphImagePixelAtomicElementsIncluded":False,
"OriginalPublisherVerbatimNumberAndImageParityClaim":False,
"StageD":"NOT_RUN","Items":leaves})
cp=read("u03-l03-abc-checkpoint.json")
cp["OriginalFigureSkillsCrosswalked"]=14
cp["OriginalSourceNumberedExerciseActionLeaves"]=len(leaves)
cp["SourceFigureSkillCrosswalk"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u03-l03-original14-polynomial-figure-source-skill-crosswalk.json"
cp["SourceExerciseActions"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u03-l03-original70-exercise-action-leaves.json"
cp["ReadyForStageD"]=False
put("u03-l03-abc-checkpoint.json",cp)
print("G12_U03_L03_14_FIGURE_MATH_SKILLS_"+str(len(leaves))+"_SOURCE_EXERCISE_ACTIONS_MAPPED")
