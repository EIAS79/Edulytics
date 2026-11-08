from pathlib import Path
import json,re
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
def read(n):return json.loads((base/n).read_text(encoding="utf-8"))
def put(n,v):(base/n).write_text(json.dumps(v,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
src=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit03\lesson005.json").read_text(encoding="utf-8"))
desc=[
"Original landmark rectangular-solid photograph motivating volume division; independent volume-to-height model, source photo not copied",
"Zero intercept and synthetic division remainder visual graph; independent coefficient trace and factor relation",
"Three-dimensional prism labelled width/length/height; independent factorized volume and missing side expression"
]
figures=[]
for n,meaning in enumerate(desc,1):
 pages=[p["PdfPage"]for p in src["Pages"]if re.search(r"Figure\s*"+str(n)+r"\b",p["Text"],re.I)]
 assert pages,n
 figures.append({"OriginalSourceFigureNumber":n,"PdfPage":min(pages),
 "SourceMathematicsSkillAndIndependentFirstPartyAlternative":meaning,
 "PublisherAndThirdPartyOriginalArtCopied":False,
 "ExactOriginalSourceGraphicPixelParityCertified":False,
 "StageDVisualRuntime":"NOT_RUN"})
put("u03-l05-original-three-figure-source-skill-crosswalk.json",{
"SchemaVersion":1,"SourceSha256":src["SourceSha256"],
"SourceFigureAnchors":3,"IndependentFigureMathSkillAlternatives":3,
"PublisherArtworkCopied":0,"ExactPublisherGraphPixelIdentityCertified":False,
"StageD":"NOT_RUN","Items":figures})
q=read("u03-l05-original73-polynomial-division-question-contracts.json")
leaves=[]
for item in q["Items"]:
 for i,action in enumerate(item["NamedSourceCognitiveActions"],1):
  leaves.append({"SourceID":f"G12-U03-L05-EX{item['Number']:03d}-ACTION{i:02d}",
  "OriginalNumberedExercise":item["Number"],
  "OriginalSourcePDFPage":item["OriginalSourcePdfPage"],
  "IndependentFirstPartyStudentCognitiveAction":action,
  "TeacherMathAssessmentNumber":item["Number"],
  "ExactPublisherVerbatimAndNumericGraphicPixelParityCertified":False,
  "StudentTeacherRuntime":"NOT_RUN"})
assert len(q["Items"])==73 and len({x["SourceID"]for x in leaves})==len(leaves)
put("u03-l05-source73-exercise-cognitive-action-leaves.json",{
"SchemaVersion":1,"OriginalSourceNumberedExerciseParents":73,
"OriginalSourceExerciseNamedActions":len(leaves),
"IndependentFirstPartyStudentMathActions":len(leaves),
"NotFullPublisherAtomicProseAndGraphicPixelDenominator":True,
"ExactPublisherSourceItemAndGraphicParityCertified":False,
"StageD":"NOT_RUN","Items":leaves})
cp=read("u03-l05-abc-checkpoint.json")
cp["OriginalSourceFigureCrosswalk"]="3_SOURCE_FIGURE_MATH_COGNITIVE_SKILLS_FIRSTPARTY"
cp["OriginalSourceNumberedExerciseActionLeaves"]=len(leaves)
cp["OriginalSourceFigureCrosswalkPath"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u03-l05-original-three-figure-source-skill-crosswalk.json"
cp["OriginalSourceExerciseActionPath"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u03-l05-source73-exercise-cognitive-action-leaves.json"
cp["ReadyForStageD"]=False
put("u03-l05-abc-checkpoint.json",cp)
print("G12_U03_L05_3_ORIGINAL_SOURCE_FIGURE_MATH_SKILLS_"+str(len(leaves))+"_NUMBERED_SOURCE_EXERCISE_ACTIONS_MAPPED")
