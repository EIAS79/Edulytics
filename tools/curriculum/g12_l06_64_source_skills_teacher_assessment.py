from pathlib import Path
import json
from fractions import Fraction
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
source=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit01\lesson006.json").read_text(encoding="utf-8"))
assert source["Section"]=="1.6"
tasks={}
def put(n,skill,prompt,answer,parts=None,mode="TEACHER_REVIEW_REQUIRED"):
 assert n not in tasks,n
 tasks[n]={"Number":n,"OriginalPdfPage":114 if n<=36 else 115,
 "SourceLearningSkill":skill,"FirstPartyStudentPrompt":prompt,
 "TeacherExpectedAnswerOrScoringGuidance":answer,
 "StudentLearningActions":parts or ["show mathematical reasoning and final set/graph"],
 "GradeMode":mode,"OriginalSourceGraphicsCopied":False,
 "ExactPublisherGraphicOrTextParityClaim":False,"StageDRuntime":"NOT_RUN"}
verbal=[
("Explain a reliable three-case method for solving |A(x)|=c. Include a no-solution case.","If c<0 no real solution; if c=0 solve A=0; if c>0 solve A=±c."),
("Explain how a V-shaped function can have two, one or no x-intercepts by comparing vertex height and opening direction.","For f=a|x−h|+k: solve |x−h|=−k/a and classify sign."),
("If isolating absolute value gives |A|=−3, what does it say about solutions and the corresponding graphs?","No real solution; nonnegative distance cannot equal negative output."),
("How can a graph of a V-shaped absolute-value function identify intervals where f(x)<0?","Locate x-intercepts and select strictly below x-axis segments."),
("Explain how to solve |A|≤c versus |A|≥c, including open/closed endpoints.","For c≥0 inside interval simultaneous inequalities; outside union of two rays. Negative c requires special handling.")
]
for n,(q,a)in enumerate(verbal,1):put(n,"VERBAL",q,a)
distance={
6:("|x−8|=4","{4,12}"),
7:("|x+4|=1/2","{−9/2,−7/2}"),
8:("|x−10|≥15","(−∞,−5]∪[25,∞)"),
9:("|f(x)−8|<0.03","7.97<f(x)<8.03")
}
for n,(expr,answer)in distance.items():
 put(n,"DISTANCE_FROM_TARGET",
    f"Express the corresponding original source skill in absolute-distance notation {expr}; derive the exact permitted real set or output bound and explain whether equality is included.",
    answer,["express distance","solve set or range"])
equations={
10:("|x+3|=9","{−12,6}"),
11:("|6−x|=5","{1,11}"),
12:("|5x−2|=11","{−9/5,13/5}"),
13:("|4x−2|=11","{−9/4,13/4}"),
14:("2|4−x|=7","{1/2,15/2}"),
15:("3|5−x|=5","{10/3,20/3}"),
16:("3|x+1|−4=5","{−4,2}"),
17:("5|x−4|−7=2","{11/5,29/5}"),
18:("−|x−3|+2=0","{1,5}"),
19:("2|x−3|+1=2","{5/2,7/2}"),
20:("|3x−2|=7","{−5/3,3}"),
21:("|3x−2|=−7","∅ (no real solution)"),
22:("|x/2−5|=11","{−12,32}"),
23:("|x/3+5|=14","{−57,27}"),
24:("−|x/3+5|+14=0","{−57,27}")
}
for n,(expr,answer)in equations.items():
 put(n,"SOLVE_ABSOLUTE_VALUE_EQUATION",
     f"Solve the real absolute-value equation {expr}. Show the two linear cases only when valid and report all exact solutions in set notation.",
     answer,["isolate absolute value","analyze nonnegative bound","solve and verify candidates"])
for n,(formula,intercepts,y_intercept)in{
25:("2|x+1|−10","x=−6 or 4","y=−8"),
26:("4|x−3|+4","none","y=16"),
27:("−3|x−2|−1","none","y=−7"),
28:("−2|x+1|+6","x=−4 or 2","y=4")
}.items():
 put(n,"ABSOLUTE_GRAPH_INTERCEPTS",
     f"For f(x)={formula}, find all real x-intercepts and the unique y-intercept without relying on a publisher graph.",
     f"x-intercepts: {intercepts}; y-intercept: {y_intercept}.",
     ["solve f(x)=0","evaluate f(0)"])
inequalities={
29:("|x−2|>10","(−∞,−8)∪(12,∞)"),
30:("2|v−7|−4≥42","(−∞,−16]∪[30,∞)"),
31:("|3x−4|≤8","[−4/3,4]"),
32:("|x−4|≥8","(−∞,−4]∪[12,∞)"),
33:("|3x−5|≥13","(−∞,−8/3]∪[6,∞)"),
34:("|3x−5|≥−13","(−∞,∞)"),
35:("|3x/4−5|≥7","(−∞,−8/3]∪[16,∞)"),
36:("|3x/4−5|+1≤16","[−40/3,80/3]")
}
for n,(expr,answer)in inequalities.items():
 put(n,"ABSOLUTE_VALUE_INEQUALITY",
     f"Solve {expr} over ℝ, reporting the entire solution in correct interval notation and marking equality endpoints.",
     answer,["isolate nonnegative absolute term","split inside/outside conditions","state included endpoints"])
# Every plotted five-point exercise is independently recreated from the
# mathematical formula rather than publisher pixels. Teacher key is retained
# separately and actual student graphic runtime belongs to Stage D.
graph={
37:("1,1,−1,0","|x−1|"),38:("1,1,1,0","|x+1|"),
39:("1,1,0,1","|x|+1"),40:("1,1,0,−2","|x|−2"),
41:("−1,1,0,0","−|x|"),42:("−1,1,0,−2","−|x|−2"),
43:("−1,1,−3,−2","−|x−3|−2"),44:("−1,1,−1,−2","−|x−1|−2"),
45:("−1,1,3,4","−|x+3|+4"),46:("2,1,3,1","2|x+3|+1"),
47:("3,1,−2,3","3|x−2|+3"),48:("1,2,−4,−3","|2x−4|−3"),
49:("1,3,9,2","|3x+9|+2"),50:("−1,1,−1,−3","−|x−1|−3"),
51:("−1,1,4,−3","−|x+4|−3"),52:("1/2,1,4,−3","(1/2)|x+4|−3")
}
assert len(graph)==16
for n,(coefs,formula)in graph.items():
 a,b,c,k=map(Fraction,coefs.replace("−","-").split(","))
 vertex=-c/b
 ys=[a*abs(b*(vertex+dx)+c)+k for dx in (-2,-1,0,1,2)]
 pp=[(str(vertex+dx),str(y))for dx,y in zip((-2,-1,0,1,2),ys)]
 put(n,"FIVE_POINT_ABSOLUTE_GRAPH",
     f"Independently graph y={formula} by computing at least five correctly labelled points spanning both arms. State the vertex, domain, range and whether the graph opens up or down.",
     f"Vertex ({vertex},{k}); five valid labelled points {pp}; domain ℝ; range {'['+str(k)+',∞)' if a>0 else '(−∞,'+str(k)+']'}.",
     ["evaluate five points","mark vertex","state domain/range"])
put(53,"WINDOWED_ABSOLUTE_GRAPH",
    "Graph the first-party formula f(x)=10|x−2| only on the source-skill window 0≤x≤4 and identify the attained range.",
    "Range [0,20], min zero at x=2, max 20 at x=0 and x=4.",
    ["render bounded viewing window","check attained output range"])
put(54,"WINDOWED_ABSOLUTE_GRAPH",
    "Graph f(x)=−100|x|+100 on the bounded window −5≤x≤5 and determine the exact displayed range.",
    "Range [−400,100], max 100 at x=0 and min −400 at x=±5.",
    ["render bounded viewing window","check attained output range"])
put(55,"TECHNOLOGY_SCALE_GRAPH",
    "Graph f(x)=−0.1|0.1(0.2−x)|+0.3 in a viewing window including x=0.2 and at least five nearby inputs. Label vertex and both slopes.",
    "f(x)=−0.01|x−0.2|+0.3; vertex (0.2,0.3); downward V; choose and label independent window.")
put(56,"TECHNOLOGY_LARGE_MAGNITUDE_GRAPH",
    "Graph f(x)=4·10^9|x−5·10^9|+2·10^9 using a window centered on x=5·10^9 wide enough to see both arms. State its vertex and axis labels.",
    "Vertex (5·10^9,2·10^9); upward V, slopes ±4·10^9; an adequate first-party window includes x=5·10^9±1.")
put(57,"ABSOLUTE_INEQUALITY_ALWAYS_TRUE",
    "Determine all x satisfying |−2x−(2/3)(x+1)|+3>−1 and justify without unnecessary case splitting.",
    "All real x, because the left side is always at least 3.")
put(58,"ABSOLUTE_ZERO_INTERCEPT_PARAMETER",
    "For f(x)=2|x+1|+a with a real parameter, determine all a for which no x-intercepts occur.",
    "a>0; a=0 gives one zero and a<0 gives two.")
put(59,"ABSOLUTE_Y_INTERCEPT_PARAMETER",
    "For f(x)=2|x+1|+a determine whether any real a removes the y-intercept; explain.",
    "No such a: f(0)=2+a always exists.")
for n,target,tolerance,unit in[
 (60,"0",100,"miles for two towns on a signed east-west line"),
 (61,"0.08",0.015,"proportion of respondents"),
 (62,"82",18,"test score points"),
 (63,"5",0.01,"inches diameter"),
 (64,"2",0.01,"inches diameter")]:
 sign="≥"if n==60 else "≤"
 variable="x"if n!=61 else "p"
 if n==61:variable="p"
 expr=f"|{variable}−({target})|{sign}{tolerance}"
 put(n,"CONTEXT_ABSOLUTE_TOLERANCE",
     f"Translate an independent real-world distance or tolerance into absolute-value notation: target {target} {unit}, allowed {'at least'if n==60 else 'within'} {tolerance} of the target. Use variable {variable} and interpret the boundary.",
     expr,["define target and deviation units","write appropriate absolute comparison"])
assert sorted(tasks)==list(range(1,65))
native=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u01-l06-firstparty-teacher-led.json"
native.parent.mkdir(parents=True,exist_ok=True)
assert not native.exists()
native.write_text(json.dumps({
"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","LessonCode":"PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U01:L06",
"OriginalSourceNumberedExercises":64,"FirstPartyIndependentSkillQuestions":64,
"StudentQuestions":[{"Number":n,"SourceSkill":v["SourceLearningSkill"],"Prompt":v["FirstPartyStudentPrompt"],"Scoring":"TEACHER_REVIEW_REQUIRED"}for n,v in sorted(tasks.items())],
"TeacherExpectedMath":[{"Number":n,"Answer":v["TeacherExpectedAnswerOrScoringGuidance"],"SourceSkillActions":v["StudentLearningActions"]}for n,v in sorted(tasks.items())],
"OriginalPublisherVisualsCopied":False,
"OriginalPublisherExactVerbatimParityCertified":False,
"StudentTeacherRuntimeAndGradebook":"NOT_RUN","StageD":"NOT_RUN"
},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
(base/"u01-l06-64-original-numbered-exercise-skill-contracts.json").write_text(json.dumps({
"SchemaVersion":1,"SourceSha256":source["SourceSha256"],
"OriginalNumberedExercises":64,"FirstPartyIndependentSourceSkillQuestions":64,
"OriginalPublisherGraphicsCopied":0,"OriginalSourceExactArtworkTextPixelParityClaim":False,
"OriginalQuestionGraphCognitiveSkillsRecreatedWithIndependent5PointModels":True,
"StageD":"NOT_RUN","Items":[tasks[n]for n in sorted(tasks)]
},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
cp=base/"u01-l06-abc-checkpoint.json"
c=json.loads(cp.read_text(encoding="utf-8"))
c["Source64NumberedExerciseSkillRoutes"]="64_FIRSTPARTY_MANUAL_TEACHER_REVIEW_QUESTIONS_AUTHORING_COMPLETE"
c["FirstPartyQuestionCount"]=64
c["FirstPartyTeacherExpectedMathCount"]=64
c["NativeFirstPartyAssessmentResource"]=native.relative_to(root).as_posix()
c["SourceGraphFigureSkills"]="PENDING_FIGURE_CROSSWALK"
c["ReadyForStageD"]=False
cp.write_text(json.dumps(c,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("G12_U01_L06_64_ORIGINAL_SECTION_SKILL_QUESTIONS_WITH_INDEPENDENT_MATH_RUBRICS_AUTHORED")
