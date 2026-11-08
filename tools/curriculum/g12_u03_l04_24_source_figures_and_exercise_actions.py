from pathlib import Path
import json,re
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
def read(n):return json.loads((base/n).read_text(encoding="utf-8"))
def put(n,v):(base/n).write_text(json.dumps(v,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
source=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit03\lesson004.json").read_text(encoding="utf-8"))
meaning=[
"Continuous polynomial graph contrasted with discontinuous nonpolynomial",
"Smooth polynomial curve versus cusp and disconnected graph classification",
"Even-degree polynomial with factorized real roots ±1,±√2",
"Polynomial with multiple simple real zeros after grouping",
"Multiple root double touch versus simple-cross intercept labelled",
"Numerical graph of cubic exact roots found from factorization",
"Odd and even multiplicity crossing or touching behavior shown visually",
"Degree-specific zero multiplicities 1,2,3 and local graph shape",
"Graph of degree6 polynomial with labelled zeros and possible multiplicities",
"Degree5 graph labelled roots/multiplicities for independent TryIt",
"All four polynomial end-behavior cases by parity and leading sign",
"Three turning points on a quartic graph; degree bound illustration",
"First phase sketch using end behavior at negative infinity",
"Root and y-intercept plotting from a fully specified factored cubic",
"Connect multiple known roots to entire curve with tail sign",
"Complete sketch of polynomial with roots and multiplicity",
"Intermediate Value Theorem continuous sign-change function",
"Graph of polynomial sign changes between two selected endpoints",
"Derive polynomial formula from graphed roots and known nonzero intercept",
"Source graph-to-polynomial TryIt; first-party independent factor model",
"Local versus global extrema on a plotted polynomial",
"Open-top box corner cut diagram and physical dimensions",
"Box volume cubic and constrained interval visualization",
"Zoomed local maximum of box-volume function"
]
assert len(meaning)==24
figures=[]
for n,m in enumerate(meaning,1):
 pages=[p["PdfPage"]for p in source["Pages"]if re.search(r"Figure\s*"+str(n)+r"\b",p["Text"],re.I)]
 assert pages,n
 figures.append({
 "OriginalSourceFigureNumber":n,"OriginalPdfPage":min(pages),
 "OriginalMathematicalCognitiveSkillAndIndependentFirstPartyAlternative":m,
 "OriginalPublisherThirdPartyImageCopied":False,
 "OriginalPublisherGraphicCoordinatesAndPixelsSameCertified":False,
 "StudentGraphRuntime":"NOT_RUN"})
put("u03-l04-original24-polynomial-graph-figure-source-skills.json",{
"SchemaVersion":1,"OriginalSourceSHA256":source["SourceSha256"],
"OriginalSourceFigureAnchors":24,
"FirstPartyIndependentFigurePedagogicMathSkillAlternatives":24,
"OriginalPublisherImagesCopied":False,"ExactPublisherGraphicsPixelMatchingClaim":False,
"StageD":"NOT_RUN","Items":figures})
q=read("u03-l04-original79-polynomial-graph-exercise-contracts.json")
leaves=[]
for item in q["Items"]:
 for i,action in enumerate(item["NamedSourceCognitiveActions"],1):
  leaves.append({
   "SourceID":f"G12-U03-L04-EX{item['Number']:03d}-ACTION{i:02d}",
   "OriginalNumberedSourceExercise":item["Number"],
   "OriginalBookPdfPage":item["OriginalSourcePdfPage"],
   "FirstPartyIndependentCognitiveStudentMathAction":action,
   "TeacherMathRubricQuestionNumber":item["Number"],
   "ExactPublisherOriginalVerbatimNumericGraphicPixelParityClaim":False,
   "StudentTeacherRuntime":"NOT_RUN"})
assert len(q["Items"])==79 and len({x["SourceID"]for x in leaves})==len(leaves)
put("u03-l04-original79-exercise-cognitive-action-leaves.json",{
"SchemaVersion":1,"OriginalSourceNumberedExercises":79,
"OriginalSourceNumberedExerciseNamedCognitiveActions":len(leaves),
"FirstPartyIndependentMathStudentActions":len(leaves),
"DoesNotRepresentFullPublisherParagraphAndPixelAtomicDenominator":True,
"ExactPublisherOriginalGraphicAndNumericalQuestionParityCertified":False,
"StageD":"NOT_RUN","Items":leaves})
cp=read("u03-l04-abc-checkpoint.json")
cp["OriginalSourceFigureMathSkills"]="24_SOURCE_FIGURES_ACADEMIC_COGNITIVE_SKILLS_CROSSWALKED"
cp["OriginalSourceNumberedExerciseActionLeaves"]=len(leaves)
cp["OriginalFigureSkillCrosswalk"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u03-l04-original24-polynomial-graph-figure-source-skills.json"
cp["OriginalExerciseActionsPath"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u03-l04-original79-exercise-cognitive-action-leaves.json"
cp["ReadyForStageD"]=False
put("u03-l04-abc-checkpoint.json",cp)
print("G12_U03_L04_24_SOURCE_FIGURE_SKILLS_"+str(len(leaves))+"_EXERCISE_MATH_ACTIONS_CROSSWALKED")
