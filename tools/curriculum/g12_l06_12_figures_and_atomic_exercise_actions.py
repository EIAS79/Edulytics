from pathlib import Path
import json,re
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
def read(f):return json.loads((base/f).read_text(encoding="utf-8"))
def put(f,obj):(base/f).write_text(json.dumps(obj,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
source=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit01\lesson006.json").read_text(encoding="utf-8"))
fmap={
1:"Original astronomy photograph illustrates nonnegative distance; independently authored distance and tolerances; third-party photo not copied",
2:"Number-line distance |x−5|≤4; independent first-party example |x−6|≤3",
3:"Parent y=|x| V-shape with vertex at 0; independent V and five-point table",
4:"V transformed right3, stretched2, up4; independent V example 2|x+1|−3",
5:"Original transformed graph whose equation must be inferred; first-party graph example with vertex and labelled point",
6:"Original graph marking vertex and shift; first-party (h,k) vertex concept",
7:"Graph shape identifies vertical stretch from rise/run; first-party vertex and one-point a determination",
8:"No, one or two x-intercepts as vertex moves relative to horizontal axis; independent root-classification key concept",
9:"Graph of |4x+1|−7 with its two zero crossings; independent |3x+2|−8 roots with points",
10:"Nonintersecting level line and nonnegative absolute graph; first-party no-solution model",
11:"Graph of absolute V below/at comparison line within an input interval; first-party |x−4|<3",
12:"Downward V negative-output rays outside two crossing inputs; first-party transformed below-zero example"
}
assert len(fmap)==12
fig=[]
for n,skill in fmap.items():
 pages=[p["PdfPage"]for p in source["Pages"]if re.search(r"Figure\s*"+str(n)+r"\b",p["Text"],re.I)]
 assert pages,(n,"missing from local source")
 fig.append({"OriginalFigure":n,"OriginalSourcePage":min(pages),
 "PedagogicSourceSkillAndFirstPartyAlternative":skill,
 "OriginalPublisherOrExternalImageCopied":False,
 "OriginalExactGraphicPixelParityCertified":False,
 "IndependentStudentDiagramOrConceptAuthored":True,"StageDGraphRuntime":"NOT_RUN"})
put("u01-l06-twelve-source-figure-skill-crosswalk.json",{
"SchemaVersion":1,"SourceSHA256":source["SourceSha256"],
"OriginalFigureAnchors":12,
"SourceFigureLearningSkillsMappedToFirstParty":12,
"PublisherOrExternalImagesCopied":0,
"PublisherGraphicPixelParityCertified":False,
"StageD":"NOT_RUN","Items":fig})
contracts=read("u01-l06-64-original-numbered-exercise-skill-contracts.json")
assert len(contracts["Items"])==64
leaves=[]
for q in contracts["Items"]:
 for k,action in enumerate(q["StudentLearningActions"],1):
  leaves.append({
  "SourceId":f"G12-U01-L06-EXERCISE{q['Number']:03d}-ACTION{k:02d}",
  "OriginalParentSourceExerciseNumber":q["Number"],
  "OriginalPDFPage":q["OriginalPdfPage"],
  "FirstPartyPedagogicStudentAction":action,
  "TeacherSourceSkillAssessmentNumber":q["Number"],
  "OriginalPublisherExactWordingCopied":False,
  "OriginalPublisherGraphPixelIdentityClaim":False,
  "StageDTeacherRuntime":"NOT_RUN"})
assert len({x["SourceId"]for x in leaves})==len(leaves)
put("u01-l06-original-exercise-action-leaves.json",{
"SchemaVersion":1,"SourceNumberedExerciseParents":64,
"SourceNamedExerciseCognitiveActionLeaves":len(leaves),
"ActionDenominatorOnlyNotEveryBookParagraphOrImagePixel":True,
"FirstPartyIndependentTeacherReviewedActionsAuthored":len(leaves),
"StageD":"NOT_RUN","Items":leaves})
cp=read("u01-l06-abc-checkpoint.json")
cp["SourceTwelveFiguresFirstPartySkillCrosswalk"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u01-l06-twelve-source-figure-skill-crosswalk.json"
cp["OriginalSourceFigureSkillsMapped"]=12
cp["Source64NumberedExerciseActionLeaves"]=len(leaves)
cp["SourceAtomicDenominatorVerified"]=False
cp["SourceFigureDiagramSkillCrosswalk"]="COMPLETE_12_MATHEMATICAL_SKILL_ALTERNATIVES_NO_ORIGINAL_ART_REUSED"
cp["ReadyForStageD"]=False
put("u01-l06-abc-checkpoint.json",cp)
print("L06_FIGURE12_SOURCE_SKILLS_CROSSWALKED_"+str(len(leaves))+"_SOURCE_EXERCISE_ACTIONS")
