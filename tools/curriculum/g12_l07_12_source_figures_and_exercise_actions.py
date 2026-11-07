from pathlib import Path
import json,re
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
source=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit01\lesson007.json").read_text(encoding="utf-8"))
assert source["Section"]=="1.7"
skills={
1:"Function-machine input/output reversal; first-party inverse mapping lesson objective",
2:"Weather forecast Celsius/Fahrenheit conversion prompt; independent temperature-unit inverse example; no forecast image reproduced",
3:"Domain and range exchange for inverse maps; independent finite-mapping RatioTable",
4:"Restrict toolkit x², 1/x² or |x| to injective branch; independent worked example 4",
5:"Read g(3) original plotted value and reverse g⁻¹(3); independent affine graph example 6",
6:"Plot g(3)=1 and g(5)=3 from original source; independently plotted affine inverse-coordinate read",
7:"Restricted original quadratic domain [0,∞); independent root branch inversion",
8:"Square-root and square graphs reflected across identity y=x; independent reciprocal mapping visual",
9:"Original graph input/output pairs for plotting inverse; first-party example10 with independent labelled points",
10:"Reflect first-party graph coordinates about y=x; first-party example10 table and rule",
11:"Original section-exercise graph requiring f(0), f⁻¹(0), zero lookup; independent f(x)=x+1 graph tasks25–28",
12:"Original section-exercise complete graph requiring inverse, domain/range; independent bounded affine graph tasks29–32"
}
assert len(skills)==12
fig=[]
for n,desc in skills.items():
 pages=[p["PdfPage"]for p in source["Pages"]if re.search(r'Figure\s*'+str(n)+r'\b',p["Text"],re.I)]
 assert pages,n
 fig.append({"OriginalSourceFigure":n,"OriginalPdfPage":min(pages),
 "SourceFigureCognitiveLearningSkillAndFirstPartyAlternative":desc,
 "FirstPartySourceSkillRepresentationAuthored":True,
 "OriginalSourcePublisherBitmapCopied":False,
 "OriginalSourceGraphicExactPixelParityCertified":False,
 "StageDGraphRuntime":"NOT_RUN"})
(base/"u01-l07-twelve-source-figure-skill-crosswalk.json").write_text(json.dumps({
"SchemaVersion":1,"SourceSha256":source["SourceSha256"],
"OriginalFigureAnchorsIdentified":12,
"FirstPartySourceFigureCognitiveSkillAlternatives":12,
"OriginalPublisherOrExternalImagesEmbedded":0,
"OriginalPublisherExactGraphPointAndPixelEqualityClaim":False,
"StageD":"NOT_RUN","Items":fig
},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
q=json.loads((base/"u01-l07-47-original-numbered-skill-assessment-contracts.json").read_text(encoding="utf-8"))
assert len(q["Items"])==47
leaves=[]
for item in q["Items"]:
 for idx,action in enumerate(item["OriginalExerciseNamedCognitiveActions"],1):
  leaves.append({
   "SourceId":f"G12-U01-L07-EXERCISE{item['Number']:03d}-ACTION{idx:02d}",
   "OriginalSourceParentNumber":item["Number"],
   "OriginalSourcePdfPage":item["SourcePdfPage"],
   "FirstPartyCognitiveAction":action,
   "StudentTeacherSourceSkillAssessmentNumber":item["Number"],
   "PublisherOriginalGraphicPixelReproduced":False,
   "PublisherExactQuestionTextMatchClaim":False,
   "StageDTeacherScoringRuntime":"NOT_RUN"})
assert len(leaves)>47 and len({x["SourceId"]for x in leaves})==len(leaves)
(base/"u01-l07-original-numbered-exercise-cognitive-action-leaves.json").write_text(json.dumps({
"SchemaVersion":1,"SourceOriginalNumberedExerciseParents":47,
"OriginalNumberedExerciseActionLeaves":len(leaves),
"SourceActionDenominatorExcludesEntireParagraphAndGraphicPixelSet":True,
"FirstPartyMathPedagogicSkillActionsAuthored":len(leaves),
"ExactOriginalPublisherPixelAndVerbatimParityClaim":False,
"StageD":"NOT_RUN","Items":leaves},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
cp=base/"u01-l07-abc-checkpoint.json"
c=json.loads(cp.read_text(encoding="utf-8"))
c["OriginalSourceFigureSkillsFirstPartyMapped"]=12
c["OriginalSourceNamedExerciseActionLeaves"]=len(leaves)
c["SourceFigureSkillReview"]="12_SOURCE_ANCHORS_TO_INDEPENDENT_INVERSE_FUNCTION_COGNITIVE_REPRESENTATIONS"
c["OriginalPublisherFigureImagesCopied"]=0
c["SourceFigureCrosswalkPath"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u01-l07-twelve-source-figure-skill-crosswalk.json"
c["SourceExerciseAtomicActionPath"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u01-l07-original-numbered-exercise-cognitive-action-leaves.json"
c["ReadyForStageD"]=False
cp.write_text(json.dumps(c,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("G12_U01_L07_TWELVE_SOURCE_FIGURE_SKILLS_AND_"+str(len(leaves))+"_SOURCE_EXERCISE_COGNITIVE_ACTIONS_MAPPED")
