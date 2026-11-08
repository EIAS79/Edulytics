from pathlib import Path
import json,math,datetime
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
ap=base/"u04-l02-source-inventory-corrected-boundary.json"
tp=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u04-l02-firstparty-teacher-led.json"
a=json.loads(ap.read_text(encoding="utf8"));t=json.loads(tp.read_text(encoding="utf8"))
assert a["OriginalExerciseMathVerified"]==28 and len(t["FirstPartyIndependentStudentMathQuestions"])==28
q=[
(29,"Describe both end limits and asymptote of y=−3·2^x−4.","As x→+∞, y→−∞; as x→−∞, y→−4 from below. Horizontal asymptote y=−4.","Source f=−5·4^x−1 tends−∞ as x→∞ and−1 from below as x→−∞."),
(30,"Describe end behavior of f(x)=4(0.25)^x−3.","As x→+∞, f→−3 from above; as x→−∞, f→+∞.","Source 3(1/2)^x−2 tends−2 as x→∞, +∞ as x→−∞."),
(31,"Describe end behavior of f(x)=5·3^(−x)+2.","As x→+∞, f→2 from above; as x→−∞, f→∞.","Source 3·4^(−x)+2 tends2 from above for x→∞ and∞ for x→−∞."),
(32,"Shift y=5^x up2.","g(x)=5^x+2, horizontal asymptote y=2.","Source 4^x shifted up4 gives 4^x+4."),
(33,"Shift y=5^x down3.","g(x)=5^x−3, asymptote y=−3.","Source 4^x shifted down3 gives 4^x−3."),
(34,"Shift y=5^x two units left.","g(x)=5^(x+2), asymptote0.","Source shift 4^x left2 gives 4^(x+2)."),
(35,"Shift y=5^x three units right.","g(x)=5^(x−3).","Source shift 4^x right5 gives 4^(x−5)."),
(36,"Reflect y=5^x across x-axis.","g(x)=−5^x, negative range.","Source reflection of4^x across x-axis gives−4^x."),
(37,"Reflect y=5^x across y-axis.","g(x)=5^(−x)=(1/5)^x, positive decay.","Source 4^(−x)=(1/4)^x."),
(38,"A newly plotted curve is y=−2^x+1. Describe parent transformations.","Reflect 2^x across x-axis, translate up1; y-intercept0; horizontal asymptote1, range(−∞,1).","Source figure original #38 is an exponential transform with negative branches; exact graphic-to-formula identity not claimed."),
(39,"An independently plotted curve has y=−3^(x−1)+2. Identify transformations and asymptote.","Reflect across x-axis, shift right1, up2; horizontal y=2; y-intercept 2−1/3.","Source original graph #39 requires reading negative exponential branch and vertical translation; no exact source plot reproduction."),
(40,"An independent plot displays y=4−2^(−x). Identify transformations and asymptote.","Reflect y=2^x across y-axis and x-axis, shift up4; asymptote y=4; range (−∞,4).","Source original graph #40 shows transformed exponential rising towards a positive horizontal limit; distinct first-party model."),
(41,"Identify an exponential function from its independent graph with points (0,−2),(1,−4),(2,−8).","f(x)=−2·2^x, negative and decreasing, horizontal y=0.","Source original graph #41 is a graph-reading function reconstruction requiring distinct marked coordinates."),
(42,"Identify an exponential function from its independent graph with points (0,−1),(1,−2),(2,−2.5) and asymptote −3.","f(x)=2(1/2)^x−3: f(0)=−1, f(1)=−2, f(2)=−2.5.","Source original graph #42 requires reconstructing negative translated decay from its graph."),
(43,"Find g(5) for g(x)=2·3^(x−2).","g(5)=2·3³=54.","Source g(x)=(1/3)7^(x−2) at x6 gives 2401/3."),
(44,"Find f(4) for f(x)=5·2^(x−1)−3.","f(4)=5·8−3=37.","Source f(5)=4·2^(5−1)−2=62."),
(45,"Find h(−5) for h(x)=−(1/4)(1/2)^x+3.","h(−5)=−(1/4)·32+3=−5.","Source −(1/2)(1/2)^(−7)+6=−58."),
(46,"Solve −16=−2^x numerically.","2^x=16, x=4.","Source −50=−(1/2)^(−x)=−2^x, so x=ln50/ln2≈"+f"{math.log(50,2):.3f}"+"."),
(47,"Solve 36=(1/4)·(1/3)^x for x.","(1/3)^x=144; x=ln144/ln(1/3)≈"+f"{math.log(144)/math.log(1/3):.3f}"+".","Source 116=(1/4)(1/8)^x gives x=ln464/ln(1/8)≈"+f"{math.log(464)/math.log(1/8):.3f}"+"."),
(48,"Solve 19=2·3^x+1.","3^x=9, x=2.","Source 12=2·3^x+1 yields x=ln(11/2)/ln3≈"+f"{math.log(5.5,3):.3f}"+"."),
(49,"Solve 7=4·(1/2)^(x−1)−1.","(1/2)^(x−1)=2 ->x=0.","Source 5=3(1/2)^(x−1)−2 ->x=1+ln(7/3)/ln(1/2)≈"+f"{1+math.log(7/3)/math.log(.5):.3f}"+"."),
(50,"Solve −18=−2·2^(x+2)+2.","−20=−2·2^(x+2), 2^(x+2)=10 -> x=log₂10−2≈"+f"{math.log(10,2)-2:.3f}"+".","Source −30=−4·2^(x+2)+2 ->2^(x+2)=8 ->x=1."),
(51,"Relate graphs of y=3^x and y=(1/3)^x.","(1/3)^x=3^(−x)=f(−x), reflection of f across y-axis.","Source b^x and (1/b)^x=b^(−x) reflect over y-axis for b>0,b≠1."),
(52,"Prove y=b^(−x) is the y-axis reflection of f(x)=b^x.","For every (x,b^x), reflected point (−x,b^x) satisfies g(−x)=b^(−(−x))=b^x; g(x)=f(−x).","Source same identity for any b>0,b≠1."),
(53,"Compare f(x)=3^x, g(x)=3^(x−2), h(x)=(1/9)·3^x.","g(x)=3^(x−2)=3^x·3^(−2)=(1/9)3^x=h(x). Both g,h shift f right2.","Source 4^(x−2)=(1/16)4^x."),
(54,"Prove b^(x−n)=(1/b^n)b^x for b>0.","Exponent rule b^(x−n)=b^x/b^n=b^(−n)b^x. Both are right-translation by n.","Source same general identity.")
]
assert [v[0] for v in q]==list(range(29,55))
assert len(q)==26 and math.isclose(2*0.5**2-3,-2.5)
for n,question,answer,original in q:
 row=a["OriginalNumberedExerciseLedger"][n-1]
 assert row["Number"]==n
 row.update({"OriginalSourceMathematicsVerified":True,"IndependentStudentExerciseAuthored":True,"TeacherExpectedMathVerified":True})
 t["FirstPartyIndependentStudentMathQuestions"].append({"SourceSkillReferenceExercise":n,"IndependentStudentQuestion":question,
 "TeacherExpectedMath":answer,"OriginalSourceMathematicsReviewed":original,
 "TeacherMarkingMode":"MANUAL_GRAPH_RECONSTRUCTION_AND_EXPONENTIAL_EQUATION","OriginalPublisherGraphicCopied":False,"Runtime":"NOT_RUN"})
assert len(t["FirstPartyIndependentStudentMathQuestions"])==54
t["IndependentSourceSkillExercisesAuthored"]=54
tp.write_text(json.dumps(t,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
a["OriginalExerciseMathVerified"]=54
a["FirstPartyIndependentExercisesAuthored"]=54
a["BlockingReasons"]=["13_ORIGINAL_FIGURES_REQUIRE_SKILL_ALTERNATIVES","STAGE_C_TEACHER_MARKING_ROUTING_PENDING"]
a["UpdatedUtc"]=datetime.datetime.now(datetime.timezone.utc).isoformat()
ap.write_text(json.dumps(a,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
print("U04L02_ALL_54_SOURCE_SKILL_EXERCISES_AND_TEACHER_KEYS_AUTHORED")
