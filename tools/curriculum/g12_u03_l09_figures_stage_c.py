from pathlib import Path
import json, datetime, math
from fractions import Fraction as F
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
ap=base/"u03-l09-source-inventory-and-gap-audit.json"
rp=root/"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u03-l09.rich-lesson-v2.json"
tp=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u03-l09-firstparty-teacher-led.json"
a=json.loads(ap.read_text(encoding="utf8"));r=json.loads(rp.read_text(encoding="utf8"));t=json.loads(tp.read_text(encoding="utf8"));lesson=r["Lessons"][0]
assert a["OriginalExerciseMathVerified"]==60 and len(t["FirstPartyIndependentStudentMathQuestions"])==60 and not a["ReadyForStageD"]
figures=[
(1,"Direct linear sales commission, straight line through the origin","Original publisher Figure 1 illustrates earnings y=0.16s; a first-party alternative uses e=0.1s, s∈{0,200,400,600}, e∈{0,20,40,60}.","Constant positive rate leads to origin line and proportional increases.",41),
(2,"Direct cubic variation graph with positive input growth","Original source Figure 2 illustrates y=(25/8)x³ at x=2 and x=6; independent first-party graph uses y=2x³ with points (0,0),(1,2),(2,16),(3,54).","Higher-power direct variation curves even though multiplier k remains constant.",42),
(3,"Reciprocal variation graph emphasizing decrease with positive x","Original Figure 3 illustrates a reciprocal form. Independent alternative y=24/x for x>0 with (1,24),(2,12),(3,8),(6,4).","Doubling x halves y for inverse first-power variation.",44),
(4,"Inverse cube relationship and change under scaling","Original Figure 4 illustrates inverse cube y=200/x³. Independent alternative y=64/x³ for x>0, points (2,8),(4,1),(8,1/8).","Doubling x divides output by 8 and the x=0 domain exclusion remains.",45)
]
def table(n,title,description,rows):
 return {"Kind":"RatioTable","Title":title,"Description":description,
         "PrimaryLabel":"Input","SecondaryLabel":"Output",
         "Rows":[{"Label":label,"Cells":cells}for label,cells in rows],"Items":[],"SecondaryItems":[]}
vis=[
table(1,"Independent diagram for direct variation: commission rate",
"Original mathematical skill from source Figure 1; no publisher diagram copied.",
[("Sale s ($)",["0","200","400","600"]),("Commission e=0.1s ($)",["0","20","40","60"])]),
table(2,"Independent diagram for cubic variation",
"Original Figure 2 source skill; coordinates can be plotted as an increasing cubic.",
[("x",["0","1","2","3"]),("y=2x³",["0","2","16","54"])]),
table(3,"Independent inverse variation table",
"Graphing y=24/x on x>0 shows a decreasing reciprocal branch.",
[("x",["1","2","3","6"]),("y=24/x",["24","12","8","4"])]),
table(4,"Independent inverse cube variation table",
"Graph y=64/x³ on x>0 and discuss change factor under doubling.",
[("x",["2","4","8"]),("y=64/x³",["8","1","1/8"])])
]
assert F(64,8**3)==F(1,8) and F(25,8)*6**3==675
assert len(a["OriginalFigureNumbers"])==4
asset=base/"u03-l09-four-original-figure-firstparty-skill-alternatives.json"
asset.write_text(json.dumps({"SchemaVersion":1,"Section":"3.9","SourceSha256":a["SourceSha256"],
"OriginalFigureCount":4,"FirstPartyFigureAlternatives":[{"SourceFigureNumber":n,"SourceSkill":title,"FirstPartyIndependentAlternative":desc,"StudentReasoning":reason,"LinkedIndependentExercise":exercise,"PublisherImageCopied":False}for n,title,desc,reason,exercise in figures],
"ExactPublisherImageParityClaimed":False},ensure_ascii=False,indent=2)+"\n",encoding="utf8")
lesson["Visuals"].extend(vis)
lesson["ExplanationParagraphs"].extend([f"Figure-skill alternative {n}: {desc} {reason}"for n,title,desc,reason,exercise in figures])
r["ContentVersion"]="g12-u03-l09-firstparty-variation-abc-four-figure-alternatives"
rp.write_text(json.dumps(r,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
families={row["Family"]for row in a["OriginalNumberedExerciseLedger"]}
routes=[{"Exercise":row["Number"],"Family":row["Family"],"EngineDecision":"REUSE_EXISTING_TEACHER_REVIEW_AND_RICHV2",
"GraphAndTableReview":"TEACHER_MANUAL_GRAPH_SIGN_DOMAIN"if row["Number"] in range(41,46) else "TEACHER_MANUAL_SYMBOLIC",
"FirstPartyIndependentQuestion":row["IndependentStudentQuestionVerified"],
"TeacherMath":row["IndependentTeacherMathVerified"],"RuntimeVerified":False,
"AutogradingClaimed":False}for row in a["OriginalNumberedExerciseLedger"]]
assert len(routes)==60 and all(x["TeacherMath"]and x["FirstPartyIndependentQuestion"]for x in routes)
route_path=base/"u03-l09-stage-c-reuse-60-teacher-assessment-routes.json"
route_path.write_text(json.dumps({"SchemaVersion":1,"Section":"3.9","RouteCount":60,"NoNewMathEngine":True,
"ExistingComponentsReused":["RichV2 student display","Manual teacher math rubric","RatioTable diagrams"],
"DrawingReview":"TEACHER_MANUAL","StageD":"NOT_RUN","CI":"NOT_RUN",
"Routes":routes},ensure_ascii=False,indent=2)+"\n",encoding="utf8")
t["StageCAssessmentRoutes"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u03-l09-stage-c-reuse-60-teacher-assessment-routes.json"
t["FirstPartySourceFigureSkillAlternatives"]=4
tp.write_text(json.dumps(t,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
a["OriginalFigureSkillAlternatives"]=4
a["OriginalFigureSkillEvidence"]=asset.relative_to(root).as_posix()
a["StageC"]="60_MANUAL_TEACHER_RUBRIC_ROUTES_REUSE_RICHV2"
a["StageCRoutesEvidence"]=route_path.relative_to(root).as_posix()
a["BlockingReasons"]=["FINAL_TARGETED_ACADEMIC_CLOSURE_CHECK_AND_GITHUB_PUSH_PENDING"]
a["UpdatedUtc"]=datetime.datetime.now(datetime.timezone.utc).isoformat()
ap.write_text(json.dumps(a,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
print("U03L09_60_EXERCISES_FOUR_FIGURES_STAGE_C_REUSE_COMPLETE_PENDING_CLOSE")
