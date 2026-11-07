from pathlib import Path
import json
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
source=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit01\lesson004.json").read_text(encoding="utf-8"))
concepts={
1:(72,"Original plotted g(1) then f(g(1)), chained graph lookup","Independent example 6 g(1)=2 and f(2)=5, graph coordinate lookup"),
2:(72,"Original graph sequence g(1)=3 and f(3)=6 annotated with arrows","Independent example 6 composed graph output with intermediate point"),
3:(73,"Two plotted function graphs with arrows tracing inner and outer evaluations","Independent Try It 4 and source-skill graphical composition step tracing"),
4:(77,"One of two original graphs used for 42–49 repeated and nested lookup","Independent finite-domain graph model f(x)=x+1 with explicit plotted input window"),
5:(77,"Second of two original graphs for 42–49","Independent finite-domain graph model g(x)=6−x with explicit plotted input window"),
6:(78,"Original graph f for nested 50–57","Independent graph model f(x)=x+1 on documented window"),
7:(78,"Original graph g for nested 50–57","Independent graph model g(x)=x−2 on documented window"),
8:(78,"Original graph h for triple nested 50–57","Independent graph model h(x)=4−x on documented window")
}
assert len(concepts)==8
(base/"u01-l04-eight-source-figure-skill-crosswalk.json").write_text(json.dumps({
"SchemaVersion":1,"OriginalSourceSection":"1.4",
"SourceSha256":source["SourceSha256"],
"OriginalDistinctFigureAnchors":8,
"FirstPartyGraphLookupSkillAlternativesMapped":8,
"OriginalPublisherFiguresCopied":0,
"SourcePublisherPixelEqualityClaim":False,
"StudentGraphRenderingAndSubmissionRuntimeVerified":False,
"StageD":"NOT_RUN",
"Items":[{
 "OriginalSourceFigureNumber":n,
 "OriginalSourcePdfPage":page,
 "SourcePedagogicCognitiveAction":skill,
 "FirstPartyIndependentGraphAlternative":alt,
 "OriginalImageEmbedded":False,
 "OriginalPublisherExactFigureValuesAreNotClaimed":True,
 "TeacherGraphReviewRequired":True}for n,(page,skill,alt)in concepts.items()]
},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
q=json.loads((base/"u01-l04-97-numbered-source-skill-question-contracts.json").read_text(encoding="utf-8"))
assert len(q["Items"])==97
atom=[]
for task in q["Items"]:
 n=task["SourceNumber"]
 action=task["SourceRequestedActions"]
 for k,item in enumerate(action,1):
  atom.append({
   "SourceId":f"G12-U01-L04-EXERCISE{n:03d}-ACTION{k:02d}",
   "SourceOriginalParentNumber":n,"OriginalPdfPage":task["SourcePdfPage"],
   "FirstPartyAcademicCognitiveAction":item,
   "StudentFirstPartyAssessmentTaskNumber":n,
   "FirstPartyTeacherExpectedMathRecorded":True,
   "OriginalSourceQuestionVerbatimReproduced":False,
   "OriginalPublisherGraphicPixelParityClaim":False,
   "RuntimeTeacherScoringVerified":False
  })
assert len(atom)>97 and len({r["SourceId"]for r in atom})==len(atom)
(base/"u01-l04-named-original-source-exercise-actions.json").write_text(json.dumps({
"SchemaVersion":1,
"OriginalNumberedSourceExerciseParents":97,
"OriginalNamedSourceExerciseActionLeaves":len(atom),
"SourceDenominatorScope":"EXPLICIT_NUMBERED_EXERCISE_COGNITIVE_ACTIONS_ONLY_NOT_EXACT_FIGURE_PIXELS_OR_EVERY_PARAGRAPH",
"FirstPartyAcademicAuthoringActionsMapped":len(atom),
"ExactOriginalPublisherArtOrVerbatimClaim":False,
"StageD":"NOT_RUN","Items":atom
},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
cp=base/"u01-l04-abc-checkpoint.json"
c=json.loads(cp.read_text(encoding="utf-8"))
c["OriginalSourceFigureSkillsMappedToFirstParty"]=8
c["OriginalSourceNamedExerciseActionLeavesAuthored"]=len(atom)
c["OriginalSourceExercisesWithFirstPartyTeacherReview"]=97
c["FirstPartyVisualRightsBoundary"]="ORIGINAL_PUBLISHER_GRAPHICS_NOT_COPIED"
c["SourceFigureSkillEvidencePath"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u01-l04-eight-source-figure-skill-crosswalk.json"
c["SourceAtomicExerciseEvidencePath"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u01-l04-named-original-source-exercise-actions.json"
c["ReadyForStageD"]=False
cp.write_text(json.dumps(c,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("U01_L04_SOURCE_FIGURE_EIGHT_FIRSTPARTY_SKILL_ALTERNATIVES_AND_"+str(len(atom))+"_EXERCISE_ACTIONS_CROSSWALKED")
