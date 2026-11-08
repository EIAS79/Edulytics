from pathlib import Path
import json,datetime,math
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
ap=base/"u04-l02-source-inventory-corrected-boundary.json"
tp=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u04-l02-firstparty-teacher-led.json"
a=json.loads(ap.read_text(encoding="utf8"));t=json.loads(tp.read_text(encoding="utf8"))
assert not t["FirstPartyIndependentStudentMathQuestions"] and a["OriginalExerciseMathVerified"]==0
qs=[
(1,"Explain the horizontal asymptote of y=2^(x+1)+3.","Horizontal asymptote y=3; as x→−∞, y approaches3 from above.","Original source: y=k describes end behavior for translated exponential y=ab^x+k."),
(2,"Why is transformation notation useful for y=−3·2^(x−1)+4?","Read shift right1, vertical scale3, reflection across x-axis and shift up4 directly; asymptote y=4.","Original: recognize sign, shift, stretch, intercept and range without graphing every point anew."),
(3,"Reflect y=4^x across y-axis, then stretch vertically by factor2.","g(x)=2·4^(−x), y-intercept2; domain R, range (0,∞).","Source f=3^x -> reflection y-axis and stretch4: g=4·3^(−x), y-intercept4."),
(4,"Reflect y=2^(−x) across y-axis and scale vertically by 1/3.","g(x)=(1/3)·2^x; intercept1/3, domain R, positive range.","Source starting (1/2)^(−x)=2^x, reflect y-axis and multiply1/5 =>(1/5)(1/2)^x."),
(5,"Reflect y=5^x across x-axis then shift up9.","g(x)=−5^x+9; y-intercept8; domain R; range (−∞,9).","Source −10^x+7; intercept6, range(−∞,7)."),
(6,"Shift y=1.5^x right2, stretch by3, reflect x-axis and shift down4.","g(x)=−3(1.5)^(x−2)−4; y-intercept −3/2.25−4=−16/3; range (−∞,−4).","Source g(x)=−2(1.68)^(x−3)−3, y-int≈−3.422, range(−∞,−3)."),
(7,"From h(x)=−(1/3)·(1/5)^(x−1)+2, shift down2, left1, stretch by3, reflect x-axis.","g(x)=(1/5)^x, y-intercept1; domain R, range (0,∞).","Source starting f=−(1/2)(1/4)^(x−2)+4 then down4, left2, stretch4, reflect x gives g=2(1/4)^x."),
(8,"Draw f(x)=2(1/3)^x and its y-axis reflection; give intercept.","Reflected g(x)=2·3^x; both have y-intercept2 and horizontal asymptote0.","Source f=3(1/2)^x reflects to3·2^x; y-int3."),
(9,"Draw f(x)=−3(0.2)^x and its y-axis reflection.","g(x)=−3·5^x; both y-intercept−3 and negative range.","Source f=−2(0.25)^x reflect to−2·4^x; intercept−2."),
(10,"Draw f(x)=5(1.5)^(−x) and its y-axis reflection.","g(x)=5(1.5)^x; intercept5 for both.","Source 6(1.75)^(−x) reflects to6(1.75)^x."),
(11,"Compare y=2(0.5)^x, y=2(1.5)^x, y=2(3)^x in the same coordinate axes.","All intersect at (0,2); base0.5 decreases; 1.5 and3 grow, with 3 faster as x increases.","Source 3(1/4)^x,3(2)^x,3(4)^x share (0,3); last grows fastest."),
(12,"Compare y=0.5(2)^x, y=2(2)^x, y=5(2)^x on one plot.","All have base2 and same horizontal asymptote y=0; vertical multipliers yield intercept 0.5,2,5.","Source (1/4)3^x,2·3^x,4·3^x share base3, different intercepts."),
(13,"Using the independent labeled set A=2(.6)^x, B=4(.6)^x, C=2(.85)^x, D=2(1.3)^x, E=4(1.3)^x, F=2(1.7)^x, choose function matching y-intercept2 and decay ratio0.6.","A; a=2 and b=0.6.","Source figure12 matching skill: identify initial height a and decay base b for 2(.69)^x; exact publisher letter not claimed."),
(14,"From independent set A-F, select intercept2 and growth factor1.3.","D; y=2(1.3)^x.","Source figure12 matching: 2(1.28)^x, positive growth and y-intercept2."),
(15,"From independent set A-F, select intercept2 and slow decay factor0.85.","C; y=2(.85)^x.","Source figure12 matching: 2(.81)^x, slower decay than b=.69."),
(16,"From independent set A-F, select intercept4 and growth factor1.3.","E; y=4(1.3)^x.","Source figure12 matching: 4(1.28)^x, larger y-intercept than 2."),
(17,"From independent set A-F, select intercept2 and fastest growth factor1.7.","F; y=2(1.7)^x.","Source figure12 matching: 2(1.59)^x, steepest positive growth among base>1 at matching intercept."),
(18,"From independent set A-F, select intercept4 and decay factor0.6.","B; y=4(.6)^x.","Source figure12 matching: 4(.69)^x, intercept4 and exponential decay."),
(19,"In a new labeled set A=2(.5)^x,B=6(.8)^x,C=1(.3)^x,D=3(1.8)^x,E=4(1.3)^x,F=.5(1.1)^x, which has largest base?","D with b=1.8.","Source figure13: compare largest positive growth ratio b, not visual height alone."),
(20,"In that independent labeled set A-F, which has smallest base?","C with b=0.3.","Source figure13: steepest decay ratio below1 is minimum b."),
(21,"In independent labeled set A-F, which has largest initial a?","B with a=6 at x=0.","Source figure13: compare y-intercepts (0,a)."),
(22,"In independent labeled set A-F, which has smallest initial a?","F with a=0.5.","Source figure13: compare y-intercepts independently of base."),
(23,"Draw y=3·2^x and its x-axis reflection.","Reflected g(x)=−3·2^x; y-int ±3; ranges positive and negative respectively.","Source 1/2·4^x reflects to−1/2·4^x."),
(24,"Draw y=2(.8)^x−1 and its x-axis reflection.","Reflected −2(.8)^x+1; asymptotes original −1, reflected+1.","Source 3(.75)^x−1 -> reflection −3(.75)^x+1."),
(25,"Draw y=−5·3^x+1 and its x-axis reflection.","Reflected y=5·3^x−1; original range(−∞,1); reflected range(−1,∞).","Source −4·2^x+2 -> reflection 4·2^x−2."),
(26,"Graph f(x)=3^(−x). Give horizontal asymptote, domain and range.","y=0; domain R, range (0,∞); exponential decay.","Source 2^(−x) same positive range and asymptote0."),
(27,"Graph g(x)=3^(x+2). Give asymptote, domain and range.","y=0; domain R, range (0,∞); shift left2.","Source 2^(x+3) shifts left3, asymptote0."),
(28,"Graph h(x)=3^x−4. Give asymptote, domain and range.","y=−4; domain R; range(−4,∞).","Source 2^x−2, asymptote y=−2 and range(−2,∞).")
]
assert [x[0]for x in qs]==list(range(1,29))
assert math.isclose(-3*(1.5)**(-2)-4,-16/3)
for n,q,key,source in qs:
 row=a["OriginalNumberedExerciseLedger"][n-1]; assert row["Number"]==n
 row.update({"OriginalSourceMathematicsVerified":True,"IndependentStudentExerciseAuthored":True,"TeacherExpectedMathVerified":True})
 t["FirstPartyIndependentStudentMathQuestions"].append({"SourceSkillReferenceExercise":n,"IndependentStudentQuestion":q,"TeacherExpectedMath":key,
 "OriginalSourceMathematicsReviewed":source,"TeacherMarkingMode":"MANUAL_TRANSFORMATION_GRAPH_INTERPRETATION","OriginalPublisherGraphicCopied":False,"Runtime":"NOT_RUN"})
assert len(t["FirstPartyIndependentStudentMathQuestions"])==28
t["IndependentSourceSkillExercisesAuthored"]=28
tp.write_text(json.dumps(t,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
a["OriginalExerciseMathVerified"]=28;a["FirstPartyIndependentExercisesAuthored"]=28
a["BlockingReasons"]=["SOURCE_EXERCISES_29_TO_54_MATH_AND_INDEPENDENT_TEACHER_KEYS_PENDING","13_ORIGINAL_FIGURES_REQUIRE_SKILL_ALTERNATIVES","STAGE_C_TEACHER_MARKING_ROUTING_PENDING"]
a["UpdatedUtc"]=datetime.datetime.now(datetime.timezone.utc).isoformat()
ap.write_text(json.dumps(a,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
print("U04L02_FIRST_28_INDEPENDENT_SOURCE_SKILL_EXERCISES_REVIEWED")
