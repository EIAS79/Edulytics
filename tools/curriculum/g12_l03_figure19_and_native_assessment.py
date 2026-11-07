from pathlib import Path
import json
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
source=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit01\lesson003.json").read_text(encoding="utf-8"))
question=json.loads((base/"u01-l03-source-47-firstparty-practice-contracts.json").read_text(encoding="utf-8"))
richpath=root/"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u01-l03.rich-lesson-v2.json"
rich=json.loads(richpath.read_text(encoding="utf-8"))
lesson=rich["Lessons"][0]
trykey=json.loads((base/"u01-l03-independent-tryit-teacher-key.json").read_text(encoding="utf-8"))
cool=json.loads((base/"u01-l03-firstparty-cooldown.json").read_text(encoding="utf-8"))
assert len(question["Items"])==47 and len(lesson["WorkedExamples"])==10 and len(trykey["Items"])==4
fig={
1:(55,"Original plotted endpoints (−1,4),(2,1) used for secant slope −1","Independent worked example 2 with labelled points and same secant skill"),
2:(56,"Vertical change −3/horizontal change +3, both endpoint arrows","Independent worked example 2 explicitly computes both changes"),
3:(58,"Cubic shape with rising, falling, then rising intervals","Independent concept monotonic behavior and exercises 18,22"),
4:(58,"An x³−12x-like curve with local maximum and minimum","Independent cubic x³−3x extrema and textbook skill"),
5:(59,"Defining local maximum relative to neighborhood","First-party local extrema key concept and worked example 9"),
6:(59,"Graph with increasing intervals (1,3) and (4,∞)","Independent worked example 7 with same intervals"),
7:(60,"2/x+x/3 has two finite extrema separated by x=0","Independent rational-plus-linear worked example 8 and x=0 domain exclusion"),
8:(60,"Numerical screen giving local extrema near ±√6","Independent exact calculus extrema at ±√6 for 3/x+x/2 without source image"),
9:(61,"Reading local extrema from a coordinate graph","Independent cubic worked example 9 with max (−1,2), min (1,−2)"),
10:(61,"Toolkit constant/identity/quadratic monotonic classification","First-party monotonic concept and x² examples"),
11:(62,"Toolkit absolute value, cubic and related increasing/decreasing behavior","First-party toolkit monotonic concept and examples"),
12:(62,"Toolkit reciprocal/radical monotonic behavior and domains","First-party ratio example8 and domain-aware concepts"),
13:(63,"Absolute extrema compared over entire domain","First-party bounded graph worked example 10"),
14:(63,"Complete finite graph with max16 at x±2 and min−10 at x3","Independent piecewise-linear worked example10 with same cognitive shape"),
15:(64,"Source secant average rate between two x-pairs","First-party exercise16 and17 plotted-coordinate secants"),
16:(65,"Source graph increasing/decreasing intervals and local turning points","First-party exercises22,23 with cubic graph and extrema"),
17:(65,"Source complete graph absolute extrema with bounded input","First-party exercises24,25 over complete bounded data-graph"),
18:(66,"Calculator peak near (1.333,5.185) classified as local max","First-party exercise41 identifies local maximum, not assumed global"),
19:(66,"Decay graph average negative rate between t5 andt15","First-party exercise47 independent points (5,12),(15,4) and −0.8 mg/day")
}
assert len(fig)==19
rows=[{"OriginalFigureNumber":i,"OriginalSourcePdfPage":page,
       "OriginalSourceSkillVerifiedFromLocalSource":skill,
       "FirstPartyIndependentSourceSkillAlternative":alt,
       "OriginalPublisherFigureBitmapCopied":False,
       "OriginalThirdPartyMediaEmbedded":False,
       "ExactPublisherPixelParityClaim":False,
       "OriginalMathematicalCognitiveActionRepresented":True,
       "TeacherVisualGradingRuntime":"NOT_RUN"}for i,(page,skill,alt) in fig.items()]
cross={
"SchemaVersion":1,
"SourceSection":"1.3","SourceSha256":source["SourceSha256"],
"OriginalSourceFigureCount":19,
"OriginalSourceFiguresSkillMappedToFirstPartyAlternatives":19,
"OriginalPublisherImagesReused":0,
"PublisherSourceGraphicPixelIdentityClaim":False,
"TeacherGraphRuntimeTests":"NOT_RUN",
"Figures":rows}
(base/"u01-l03-figure19-firstparty-skill-crosswalk.json").write_text(json.dumps(cross,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
leaf=[]
for x in question["Items"]:
    n=x["Number"]
    parts=["complete the mathematical source-skill question"]
    if n in (16,17):parts=["read two original-skill graph endpoint outputs","calculate average rate and units"]
    if 18<=n<=22:parts=["identify increasing input intervals","identify decreasing input intervals"]
    if n==23:parts=["identify local maximum","identify local minimum"]
    if n==24:parts=["identify increasing intervals","identify decreasing intervals"]
    if n==25:parts=["identify absolute maximum","identify absolute minimum"]
    if n in (26,27):parts=["a: first time interval average rate","b: second time interval average rate"]
    if 35<=n<=40:parts=["estimate local extrema","determine increasing intervals","determine decreasing intervals"]
    if n==41:parts=["identify meaning of plotted peak","justify local/global distinction"]
    for k,part in enumerate(parts,1):
        leaf.append({
          "SourceId":f"G12-U01-L03-EXERCISE{n:03d}-ACTION{k:02d}",
          "SourceParentExerciseNumber":n,
          "OriginalSourcePdfPage":x["SourcePDFPage"],
          "FirstPartyExerciseAssessmentNumber":n,
          "SourcePedagogicAction":part,
          "TeacherExpectedMathematicsRecorded":True,
          "FirstPartyAcademicLearningSkillAuthored":True,
          "OriginalSourceFigureCopied":False,
          "SourcePublisherPixelAndProseIdentityClaim":False
        })
assert len(leaf)>47 and len({x["SourceId"]for x in leaf})==len(leaf)
(base/"u01-l03-original-source-exercise-action-leaves.json").write_text(json.dumps({
"SchemaVersion":1,"SourceOriginalNamedNumberedExercises":47,
"OriginalNamedActionLeafCount":len(leaf),
"CountMethod":"Numbered source question parents replaced by explicit child actions for separately requested components; no double counting.",
"SourceNamedActionScopeOnlyNotEveryGraphicPixel":True,
"StageD":"NOT_RUN","Leaves":leaf},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
resource=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u01-l03-firstparty-teacher-led.json"
resource.parent.mkdir(parents=True,exist_ok=True)
assert not resource.exists()
resource.write_text(json.dumps({
"SchemaVersion":1,
"LessonCode":lesson["LessonCode"],
"Course":"US-CCSS-MATH:G12",
"OriginalBookSection":"OpenStax Precalculus 2017 local Section 1.3",
"OriginalNumberedSourceExerciseCount":47,
"FirstPartyIndependentPracticeQuestionCount":47,
"TeacherSourceActionSubpartCount":len(leaf),
"IndependentStudentQuestions":[
  {"Number":x["Number"],"Skill":x["Category"],"Prompt":x["FirstPartyStudentQuestion"],
   "TeacherScoringMode":"TEACHER_REVIEW_REQUIRED"}for x in question["Items"]],
"TeacherOnlyExpectedAnswers":[
  {"Number":x["Number"],"ExpectedMathematics":x["TeacherExpectedMathematics"],
   "TeacherReviewMethod":x["TeacherManualReviewGuidance"]}for x in question["Items"]],
"TeacherOnlyTryItAnswers":[{"Number":i+1,"ExpectedMathematics":x["FirstPartyTeacherAnswer"]}
                            for i,x in enumerate(trykey["Items"])],
"TeacherOnlyCoolDownAnswer":cool["TeacherExpectedAnswer"],
"OriginalPublisherFiguresCopied":False,
"OriginalPublisherAuthorshipAttribution":"RETAIN_CC_BY_4_SOURCE_CREDITS",
"OriginalPublisherExactVerbatimOrPixelParityClaim":False,
"StudentPortalQuestionRoutesNotYetRuntimeVerified":True,
"AutomatedGradebookNotYetRuntimeVerified":True,
"StageD":"NOT_RUN"
},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
cp=base/"u01-l03-abc-checkpoint.json"
c=json.loads(cp.read_text(encoding="utf-8"))
c["Source19FigureSkillsFirstPartyMapped"]=19
c["OriginalSource47NumberedPracticeQuestionsAuthored"]=47
c["OriginalSourceExerciseActionLeavesAuthored"]=len(leaf)
c["FirstPartyNativeAssessmentResource"]=resource.relative_to(root).as_posix()
c["FigureEvidencePath"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u01-l03-figure19-firstparty-skill-crosswalk.json"
c["SourceOriginalImagesCopied"]=0
c["StageC"]="47_INDEPENDENT_STUDENT_SOURCE_SKILL_QUESTIONS_AND_TEACHER_EXPECTED_MATH_EMBEDDED_AS_FIRSTPARTY_ASSESSMENT_RESOURCE"
c["ReadyForStageD"]=False
cp.write_text(json.dumps(c,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
progress=base/"full-grade-progress.json"
p=json.loads(progress.read_text(encoding="utf-8"))
assert p["LastClosedLessonOrdinal"]==2 and p["FirstIncompleteLessonOrdinal"]==3
e=p["LessonEntries"][2]
e["FirstPartyNativeAssessmentResource"]=resource.relative_to(root).as_posix()
e["OriginalSourceFigureSkillsMapped"]=19
e["OriginalSourceExerciseActionLeavesAuthored"]=len(leaf)
e["StageCStatus"]=c["StageC"]
e["ReadyForStageD"]=False
p["UpdatedUtc"]=datetime.now(timezone.utc).isoformat()
progress.write_text(json.dumps(p,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("G12_U01_L03_19_SOURCE_FIGURES_47_FIRSTPARTY_PRACTICE_QUESTIONS_AND_"+str(len(leaf))+"_ORIGINAL_EXERCISE_ACTIONS_AUTHORING_READY")
