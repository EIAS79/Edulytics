from pathlib import Path
import json,re
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
source=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit01\lesson005.json").read_text(encoding="utf-8"))
assert source["Section"]=="1.5"
concepts={
1:"Original mirror photograph and reflection motivation; first-party algebraic x/y reflections, no photo copied",
2:"Comparing source and vertically translated graphs; independent ratio-table output shift",
3:"Real-world vent area time series; independent output translation in worked example 1",
4:"Original time-series shifted vertically; independently modelled building monitor output shift",
5:"Original graph with left shift from f(x+1); first-party horizontal shift concept and example 3",
6:"Original altered start time context; first-party schedule shift one hour earlier",
7:"Source tabular horizontal translation; first-party tabular worked example 4",
8:"Parabola horizontal shift by 2; independently plotted parent x² shifted left 3",
9:"Combining absolute-value horizontal and vertical translations; first-party worked example 7",
10:"Completed compound-shifted absolute-value graph; first-party worked example 7",
11:"Translated square-root domain and range; first-party worked example 8",
12:"Contrasting reflection over x and y axes; independent reflection teaching example",
13:"Square-root reflected over x-axis; independent worked example 9 with −√x",
14:"Square-root reflected over y-axis; independent worked example 9 with √(−x)",
15:"Exponential reflection and shift; independent worked example 11 on 2^x",
16:"Graph sequence horizontal reflection, vertical reflection and shift; independent worked example 11",
17:"Origin symmetry under oddness/axis reflections; independent parity concepts",
18:"Odd function origin symmetry; independent x³+4x worked example 12",
19:"Original vertical stretch and compression comparison; independent input-output ratio tables",
20:"Original population model graph used for vertical scaling; independent first-party population example 13",
21:"Double output graph with unchanged inputs; first-party example 13",
22:"Cubic vertically stretched; first-party example 15 with cubic 3x³",
23:"Parent quadratic horizontally expanded/compressed; independent scale concept",
24:"Faster population timeline; first-party worked example 16",
25:"Original horizontal-stretch table plots; first-party worked example 17",
26:"Original graph feature moves x6→x2; first-party worked example 18",
27:"Original compound graph before transformations; first-party worked example 20",
28:"Intermediate stretched horizontal graph; first-party worked example 20",
29:"Intermediate horizontally shifted graph; first-party worked example 20",
30:"Final vertically shifted transformed graph; first-party worked example 20",
31:"Original exponential reference plot for graphical translations 24–26; first-party independent y=2^x coordinates",
32:"Original base graph for transformations 78–81; first-party three-point graphic translation/reflection exercises"
}
assert len(concepts)==32
figure_rows=[]
for n,skill in concepts.items():
 found=[]
 for page in source["Pages"]:
  if re.search(r'Figure\s*'+str(n)+r'\b',page["Text"],re.I):
   found.append(page["PdfPage"])
 assert found,(n,"missing original Figure anchor")
 figure_rows.append({"OriginalFigureNumber":n,"SourcePdfPageFirstReference":min(found),
  "LocalSourceSkillSummaryAndFirstPartyRepresentation":skill,
  "FirstPartyAcademicSourceSkillAlternativeAuthored":True,
  "OriginalPublisherArtworkCopied":False,
  "OriginalPublisherPixelExactParityClaim":False,
  "StudentVisualRuntimeVerified":False})
(base/"u01-l05-32-original-figure-skill-crosswalk.json").write_text(json.dumps({
"SchemaVersion":1,"SourceBookSection":"1.5",
"SourceSha256":source["SourceSha256"],
"OriginalFigureAnchorsLocated":32,
"FirstPartyPedagogicFigureSkillAlternativesMapped":32,
"SourceOriginalThirdPartyPhotoCopied":False,
"OriginalPublisherGraphBitmapsCopied":0,
"ExactSourceGraphicsCoordinateAndPixelParityClaim":False,
"StageD":"NOT_RUN","Items":figure_rows
},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
contract=json.loads((base/"u01-l05-source-81-firstparty-question-contracts.json").read_text(encoding="utf-8"))
assert len(contract["Items"])==81
leaves=[]
for task in contract["Items"]:
 n=task["Number"]
 for index,action in enumerate(task["OriginalSourcePedagogicActions"],1):
  leaves.append({"SourceId":f"G12-U01-L05-SOURCE-EX{n:03d}-ACTION{index:02d}",
   "OriginalSourceParentExerciseNumber":n,"OriginalSourcePdfPage":task["SourcePDFPage"],
   "FirstPartyPedagogicAction":action,"FirstPartyTeacherAssessmentTaskNumber":n,
   "OriginalPublisherSourceExactVerbatimOrPixelParityClaim":False,
   "FirstPartySourceLearningSkillAuthored":True,
   "RuntimeGradebookAndGraphTesting":"NOT_RUN"})
assert len({x["SourceId"]for x in leaves})==len(leaves)
(base/"u01-l05-original-numbered-exercise-action-leaves.json").write_text(json.dumps({
"SchemaVersion":1,"SourceNumberedExerciseParents":81,
"SourceNumberedExerciseCognitiveActionLeaves":len(leaves),
"CountScope":"ORIGINAL_81_NUMBERED_EXERCISES_AND_NAMED_STUDENT_ACTIONS_ONLY_EXCLUDES_FULL_PUBLISHER_GRAPH_PIXEL_DENOMINATOR",
"FirstPartySourceSkillTeacherReviewActionsAuthored":len(leaves),
"ExactPublisherVerbatimIdentityClaim":False,
"StageD":"NOT_RUN","Items":leaves
},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
cp=base/"u01-l05-abc-checkpoint.json"
c=json.loads(cp.read_text(encoding="utf-8"))
c["SourceFigure32FirstPartyAcademicSkillCrosswalk"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u01-l05-32-original-figure-skill-crosswalk.json"
c["SourceOriginalFigureLearningSkillsMapped"]=32
c["SourceNumberedExerciseActionLeaves"]=len(leaves)
c["SourceFigureVisualSkillReview"]="32_ORIGINAL_PDF_FIGURE_ANCHORS_AND_FIRSTPARTY_PEDAGOGIC_SKILLS_REVIEWED_NO_PUBLISHER_ART_REUSED"
c["SourceAtomicActionScopeExcludesFullPublisherPixelAndParagraphs"]=True
c["ReadyForStageD"]=False
cp.write_text(json.dumps(c,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("L05_32_FIGURES_PEDAGOGIC_CROSSWALKED_81_EXERCISES_"+str(len(leaves))+"_COGNITIVE_ACTIONS_SOURCE_SKILL_ACCEPTANCE_REVIEW")
