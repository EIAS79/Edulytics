from pathlib import Path
import json,re
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
def read(n):return json.loads((base/n).read_text(encoding="utf-8"))
def put(n,v):(base/n).write_text(json.dumps(v,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
src=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit03\lesson002.json").read_text(encoding="utf-8"))
figures=[
"Satellite-dish photograph motivates parabola shape; independent quadratic interpretation, original photo not reused",
"Labelled vertex, symmetry axis, intercepts on a parabola; independent x²−4x−5 tabular graph",
"Read vertex, zeros and axis from labelled parabola graph; independent marked coordinate parabola",
"General form coefficients and symmetry x=−b/(2a); first-party vertex formula example",
"Negative coefficient and vertex form for downward opening; independent −3(x−2)²+5",
"Parent function y=x²; independent source-skill symmetric table",
"Translated and scaled vertex-form parabola; independent from vertex and one point",
"Basketball arc photograph with quadratic fit; independent projectile height formula",
"Upward-opening minimum and downward-opening maximum examples; first-party contrasts",
"Three-sided rectangular garden enclosure diagram; independent dimensions and quadratic constraint",
"Plot of enclosure area with peak; independent price-revenue and quadratic area tables",
"Revenue maximum plot; independent price-demand quadratic optimization",
"Quadratic 0/1/2 real-root intersection outcomes; first-party discriminant graph examples",
"Graph and intercepts of factorable quadratic; independent exact roots and graph",
"Graph and approximate irrational intercepts; independent quadratic formula exact roots",
"Projectile trajectory displaying peak time/height and ground impact; independent physics model"
]
assert len(figures)==16
anchors=[]
for n,meaning in enumerate(figures,1):
 pages=[p["PdfPage"]for p in src["Pages"]if re.search(r"Figure\s*"+str(n)+r"\b",p["Text"],re.I)]
 assert pages,n
 anchors.append({
 "SourceOriginalFigureNumber":n,"SourcePDFPage":min(pages),
 "OriginalSourceMathFigureSkillAndIndependentFirstPartyAlternative":meaning,
 "PublisherOrThirdPartyOriginalFigureReproduced":False,
 "OriginalPublisherExactCoordinateAndPixelSimilarityCertified":False,
 "StageDGraphRendering":"NOT_RUN"})
put("u03-l02-original16-figure-quadratic-skills-crosswalk.json",{
"SchemaVersion":1,"OriginalBookSha256":src["SourceSha256"],
"OriginalFigureAnchors":16,"FirstPartySourceFigureAcademicSkillAlternatives":16,
"PublisherOrThirdPartyArtworkCopied":0,
"OriginalPublisherGraphicPixelParityCertified":False,
"StageD":"NOT_RUN","Items":anchors})
q=read("u03-l02-original94-firstparty-quadratic-exercise-contracts.json")
leaves=[]
for item in q["Items"]:
 for i,action in enumerate(item["SourceCognitiveActions"],1):
  leaves.append({
   "SourceID":f"G12-U03-L02-EX{item['Number']:03d}-ACTION{i:02d}",
   "OriginalSourceNumberedExercise":item["Number"],
   "OriginalSourcePdfPage":item["OriginalSourcePDFPage"],
   "IndependentFirstPartyStudentMathAction":action,
   "TeacherQuestion":item["Number"],
   "PublisherVerbatimNumericAndImageGraphicParityClaim":False,
   "StageD":"NOT_RUN"})
assert len(q["Items"])==94 and len({e["SourceID"]for e in leaves})==len(leaves)
put("u03-l02-original94-exercise-cognitive-action-leaves.json",{
"SchemaVersion":1,
"OriginalNumberedSourceExercises":94,
"OriginalNumberedExerciseNamedActionLeaves":len(leaves),
"IndependentFirstPartyMathActionAlternatives":len(leaves),
"DoesNotIncludeAllPublisherParagraphImagePixelDenominator":True,
"ExactSourceVerbatimGraphicParityCertified":False,
"StageD":"NOT_RUN","Items":leaves})
cp=read("u03-l02-abc-checkpoint.json")
cp["OriginalFigureSkills"]="16_ORIGINAL_FIGURE_ACADEMIC_SKILL_ALTERNATIVES_INDEPENDENT_FIRSTPARTY"
cp["SourceOriginalCognitiveActionLeaves"]=len(leaves)
cp["SourceFigureCrosswalkPath"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u03-l02-original16-figure-quadratic-skills-crosswalk.json"
cp["SourceExerciseActionLedger"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u03-l02-original94-exercise-cognitive-action-leaves.json"
cp["ReadyForStageD"]=False
put("u03-l02-abc-checkpoint.json",cp)
print("G12_U03_L02_16_ORIGINAL_QUADRATIC_FIGURE_SKILLS_"+str(len(leaves))+"_NUMBERED_EXERCISE_ACTIONS")
