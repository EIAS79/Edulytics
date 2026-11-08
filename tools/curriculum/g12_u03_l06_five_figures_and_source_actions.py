from pathlib import Path
import json,re
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
def read(n):return json.loads((base/n).read_text(encoding="utf-8"))
def put(n,v):(base/n).write_text(json.dumps(v,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
src=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit03\lesson006.json").read_text(encoding="utf-8"))
meanings=[
"Graph of cubic polynomial with double real zero that touches x-axis, another simple crossing; independent factorization and repeated-zero teacher skill",
"Graph of cubic polynomial with odd-multiplicity crossing and complex conjugate pair not visible on x-axis; independent complex root examples",
"Displayed original Descartes positive sign-change algebraic expression; independent first-party sign-change tabular rubric",
"Displayed p(−x) sign-change comparison for negative roots; independent first-party exact coefficient sign table",
"Graph confirming actual positive/negative roots are within Descartes allowable candidate counts; first-party source-skill root-count questions"
]
fig=[]
for n,skill in enumerate(meanings,1):
 pages=[p["PdfPage"]for p in src["Pages"]if re.search(r"Figure\s*"+str(n)+r"\b",p["Text"],re.I)]
 assert pages,n
 fig.append({"OriginalFigureNumber":n,"SourcePdfPage":min(pages),
 "OriginalAcademicGraphicSkillAndFirstPartyAlternative":skill,
 "PublisherImageOriginalPixelsCopied":False,
 "OriginalPublisherExactNumericalGraphIdentityCertified":False,
 "StageDVisualRuntime":"NOT_RUN"})
put("u03-l06-original-five-figure-polynomial-zero-skill-crosswalk.json",{
"SchemaVersion":1,"OriginalSourceSHA256":src["SourceSha256"],
"OriginalFigureAnchors":5,"FirstPartyIndependentFigureMathSkillAlternatives":5,
"OriginalPublisherArtworkCopied":False,
"ExactOriginalPublisherSourceFigurePixelParityCertified":False,
"StageD":"NOT_RUN","Items":fig})
q=read("u03-l06-original80-polynomial-zeros-question-contracts.json")
leaves=[]
for item in q["Items"]:
 for i,action in enumerate(item["SourceCognitiveActions"],1):
  leaves.append({
   "SourceID":f"G12-U03-L06-EX{item['Number']:03d}-ACTION{i:02d}",
   "OriginalSourceNumberedExercise":item["Number"],
   "SourcePdfPage":item["OriginalBookPdfPage"],
   "IndependentFirstPartyStudentMathAction":action,
   "TeacherMathRubricNumber":item["Number"],
   "ExactOriginalPublisherVerbatimGraphNumberParityClaim":False,
   "StudentTeacherRuntime":"NOT_RUN"})
assert len(q["Items"])==80 and len({x["SourceID"]for x in leaves})==len(leaves)
put("u03-l06-original80-exercise-cognitive-actions.json",{
"SchemaVersion":1,"OriginalNumberedSourceExercises":80,
"SourceNamedExerciseCognitiveActions":len(leaves),
"FirstPartyIndependentEquivalentMathActions":len(leaves),
"AllOriginalPublisherProseImagePixelAtomicElementsRepresented":False,
"ExactOriginalPublisherGraphicNumericSourceParityClaim":False,
"StageD":"NOT_RUN","Items":leaves})
cp=read("u03-l06-abc-checkpoint.json")
cp["SourceFigureSkillCrosswalk"]="5_ORIGINAL_FIGURES_FIRSTPARTY_PEDAGOGIC_MATH_COGNITIVE_SKILLS"
cp["OriginalNumberedSourceExerciseActionLeaves"]=len(leaves)
cp["FigureSkillAsset"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u03-l06-original-five-figure-polynomial-zero-skill-crosswalk.json"
cp["NumberedExerciseActionAsset"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u03-l06-original80-exercise-cognitive-actions.json"
cp["ReadyForStageD"]=False
put("u03-l06-abc-checkpoint.json",cp)
print("G12_U03_L06_SOURCE_FIGURE_MATH_SKILLS_5_EXERCISE_COGNITIVE_ACTIONS_"+str(len(leaves)))
