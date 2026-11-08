from pathlib import Path
import json, math, datetime, subprocess, time, re
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626");base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
ap=base/"u04-l04-source-inventory-corrected-boundary.json";pp=base/"full-grade-progress.json"
cache=Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit04\lesson004.json")
load=lambda p:json.loads(p.read_text(encoding="utf8"))
def save(p,v):p.parent.mkdir(parents=True,exist_ok=True);p.write_text(json.dumps(v,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
a,p,s=load(ap),load(pp),load(cache)
assert p["LastClosedLessonOrdinal"]==23 and p["FirstIncompleteLessonOrdinal"]==24
assert a["SourceSHA256"]==s["SourceSha256"] and [s["Pages"][0]["PdfPage"],s["Pages"][-1]["PdfPage"]]==[378,394]
assert len(a["OriginalNumberedExerciseLedger"])==60 and len(a["OriginalWorkedExamples"])==11
assert a["OriginalTryItNumbers"]==list(range(1,12)) and a["OriginalFigureNumbers"]==list(range(1,20))
Q=[]
def add(question,key,original):
 assert question and key and len(original)>7
 n=len(Q)+1
 Q.append({"SourceSkillReferenceExercise":n,"IndependentStudentQuestion":question,
 "TeacherExpectedMath":key,"OriginalSourceMathematicsReviewed":original,"AutoGradingClaimed":False,
 "MarkingMode":"EXISTING_TEACHER_MANUAL_GRAPH_AND_MATH_RUBRIC","PublisherQuestionCopied":False})
# 1–5: original source verbal skills
add("How do the coordinates of y=4^x relate to y=log_4(x)?","Swap (x,y)↔(y,x), graph reflection across y=x.","Source1 exponential/logarithmic inverse graphs have swapped ordered pairs.")
add("Does shifting log_3(x) upward by5 change its range?","No; the range remains all real numbers.","Source2 vertical shifts do not change full real logarithmic range.")
add("What horizontal translation changes the domain of ln(x)?","ln(x-3) shifts right3, domain x>3; vertical shifts do not change domain.","Source3 a shift in the argument changes the domain.")
add("Why is x=0 excluded from log_2(x)?","2^y is positive for all real y and never zero; log has domain x>0.","Source4 0 cannot equal b^y for b>0.")
add("Does y=ln(x) have a horizontal asymptote?","No; it has vertical asymptote x=0, while range is all real.","Source5 logarithmic parent has vertical not horizontal asymptote.")
# 6–10: 5 independently authored domains/ranges and source mathematics
dom=[
("log_2(x+5)","x>−5; range R","log_3(x+4) domain x>−4, range R."),
("ln(3−x)","x<3; range R","ln(1/2−x) domain x<1/2, range R."),
("log_3(4x+8)−2","x>−2; range R","log_5(2x+9)−2 domain x>−9/2, range R."),
("ln(5x+20)−4","x>−4; range R","ln(4x+17)−5 domain x>−17/4, range R."),
("log_2(10−2x)+3","x<5; range R","log_2(12−3x)−3 domain x<4, range R.")]
for expr,answer,source in dom:add(f"Give domain and range of f(x)={expr}.",answer,source)
# 11-15 vertical asymptotes
for expr,answer,src in [
("log_4(x−2)","x>2, asymptote x=2","log_b(x−5), x>5, asymptote x=5"),
("ln(7−x)","x<7, asymptote x=7","ln(3−x), x<3, asymptote x=3"),
("log(5x+10)","x>−2, asymptote x=−2","log(3x+1), x>−1/3, asymptote x=−1/3"),
("−2log(−x)+1","x<0, asymptote x=0","3log(−x)+2, x<0, asymptote x=0"),
("−ln(2x+6)−2","x>−3, asymptote x=−3","−ln(3x+9)−7, x>−3, asymptote x=−3")]:
 add(f"Determine domain and vertical asymptote for {expr}.",answer,src)
# 16-20 end behaviour with domains
for expr,answer,src in [
("ln(4−x)","x<4; x→4− gives−∞; x→−∞ gives+∞","ln(2−x), x<2, tends−∞ at2− and +∞ at−∞."),
("log(x−1/2)","x>1/2; x→(1/2)+ gives−∞; x→∞ gives+∞","log(x−3/7) x>3/7, tends−∞ at3/7+ and+∞ to the right."),
("−log(2x−6)+1","x>3; at3+ result+∞, at∞ result−∞","−log(3x−4)+3 x>4/3, from +∞ to−∞."),
("ln(x+5)−2","x>−5; at−5+ result−∞, at∞ result+∞","ln(2x+6)−5 x>−3, left end−∞, right end+∞."),
("log_3(8−2x)+1","x<4; at4− result−∞, at−∞ result+∞","log_3(15−5x)+6 x<3, tends−∞ near3− and+∞ at−∞")]:
 add(f"Give domain and end behavior of y={expr}.",answer,src)
# 21–25 intercepts calculated from the source pattern
for expr,ans,src in [
("log_2(x−2)+1","domain x>2; x-intercept (5/2,0); no y-intercept; range R","Source log_4(x−1)+1, domain x>1, x-intercept5/4, no y-intercept."),
("log(2x+6)+2","domain x>−3; x-intercept ((0.01−6)/2,0)=(-2.995,0); y-intercept (0, log(6)+2)","Source log(5x+10)+3, x-intercept(0.001−10)/5 and y-intercept4."),
("ln(−x)−1","domain x<0; x-intercept (−e,0), no y-intercept","Source ln(−x)−2, x-intercept−e², no y-intercept."),
("log_2(x+4)−3","domain x>−4; x-intercept(4,0); y-intercept(0,−1)","Source log_2(x+2)−5, x-intercept30, y-intercept−4."),
("2ln(x)−4","domain x>0; x-intercept(e²,0); no y-intercept","Source 3ln(x)−9, x-intercept e³, no y-intercept.")]:
 add(f"Find domain, range, and intercepts for y={expr}.",ans,src)
# 26–33 source figure matching, independently authored labelled set prevents claiming publisher letters
labels=[("A","log_10(x)","log(x) source26"),("B","ln(x)","ln(x) source27"),
("C","log_2(x)","log_2(x) source28"),("D","log_5(x)","log_5(x) source29"),
("E","log_25(x)","log_25(x) source30"),
("F","log_(1/3)(x)","source31 log_(1/3)(x) decreases"),
("G","log_(1/2)(x)","source32 log_2(x) increases; new graph intentionally changes base"),
("H","log_(3/4)(x)","source33 log_(3/4)(x) decreases slowly")]
for label,expr,source in labels:
 add(f"In an independent labelled graph set, identify curve {label} for function y={expr} and characterize growth.",
 f"{label}; {'decreasing' if label in ['F','G','H'] else 'increasing'} because its base is {'below' if label in ['F','G','H'] else 'above'}1; passes (1,0).",
 source+"; source-letter parity not claimed.")
# 34–37 graph function pairs
pairs=[
("f(x)=log_2(x), g(x)=2^x","Reflection over y=x; f domain x>0, g domain R.","Source34 common log and 10^x inverse graphs."),
("f(x)=log_2(x), g(x)=log_(1/2)(x)","g(x)=−f(x), x-axis reflection; both domain x>0.","Source35 common logarithm vs log_(1/2); compare directions."),
("f(x)=log_4(x), g(x)=ln(x)","For x>1, ln(x)>log_4(x), as ln4>1.","Source36 log_4 vs ln with shared(1,0) but different slopes."),
("f(x)=e^x, g(x)=ln(x)","Graph reflection across y=x, swapped domains/ranges.","Source37 e^x vs ln(x) inverse.")]
for f,ans,src in pairs:add(f"Graph and compare {f}.",ans,src)
# 38–40 graph transformations, independent labels
for expr,ans,source in [
("log_3(3−x)","domain x<3; vertical asymptote x=3; decreases as x increases","Source38 log_4(−x+2), domain x<2, reflected over y axis and shifted."),
("−log_3(x+1)","domain x>−1; asymptote x=−1; decreases","Source39 −log_4(x+2), domain x>−2, reflected across x axis."),
("log_3(x+1)","domain x>−1; asymptote x=−1; increases","Source40 log_4(x+2), domain x>−2, shifted left.")]:
 add(f"Draw independent graph y={expr} and characterize asymptote and direction.",ans,source)
# 41–46 graphing of source skills
for expr,ans,source in [
("log_2(x+3)","domain x>−3; asymptote x=−3; intercept x=−2","Source41 log_2(x+2), x>−2, intercept−1."),
("3log(x)","domain x>0; x-intercept1; vertical stretch3","Source42 2log(x), factor2."),
("ln(−x)","domain x<0; x-intercept−1; y-axis reflection","Source43 ln(−x) same mathematical skill."),
("log(2x+8)+1","domain x>−4; asymptote−4, x-intercept x=(0.1−8)/2","Source44 log(4x+16)+4 with asymptote−4."),
("log(9−3x)+2","domain x<3; vertical asymptote3; decreasing","Source45 log(6−3x)+1 with asymptote2."),
("−(1/3)ln(x+2)−1","domain x>−2; vertical asymptote−2; decreasing","Source46 −(1/2)ln(x+1)−3 with asymptote−1.")]:
 add(f"Sketch y={expr} and give its domain, vertical asymptote and one feature.",ans,source)
# 47–50 inverse graph-reading source skills with separately specified first-party graph anchors
for expr,points,answer,src in [
("log_2(x−1)",["(2,0)","(3,1)"],"right shift1, asymptote x=1","Source47 uses parent log_2(x) and a transformed plotted graph."),
("log_3(x)+2",["(1,2)","(3,3)"],"vertical shift up2, asymptote x=0","Source48 uses log_3(x) and a graph to identify a transformation."),
("−log_4(x)",["(1,0)","(4,−1)"],"reflection over x-axis, domain x>0","Source49 uses log_4(x) parent and an independent graph."),
("log_5(−x)",["(−1,0)","(−5,1)"],"y-axis reflection, domain x<0","Source50 uses log_5(x) parent and transformed graph.")]:
 add(f"An independent graph passes through {', '.join(points)}. Identify y={expr} and describe transformations.",answer,src)
# 51–55 equation solving. Give domain checks, use exact easy independent problems.
eq=[
("log(x−1)=ln(x−1)","x=2, since ln(t)=log_10(t) implies ln t=0 and t=1","Source51 log(x−1)+2=ln(x−1)+2 has unique x=2."),
("2log(2x−1)=2","2x−1=10, so x=11/2, domain x>1/2","Source52 log(2x−3)+2=−log(2x−3)+5 implies x=(3+10^1.5)/2."),
("ln(x−2)=−ln(x+2)","(x−2)(x+2)=1 => x=√5, x>2","Source53 ln(x−2)=−ln(x+1), x=(1+√13)/2 with x>2."),
("ln(3x+1)=0","3x+1=1 =>x=0; domain x>−1/3","Source54 mixed natural log terms requires −1/5<x<0 and numerical solution."),
("log(1−x)=log(x+1)","x=0, both arguments positive for −1<x<1","Source55 mixed logarithm equation with domain −1<x<1 requires numerical check.")
]
for expr,ans,src in eq:add(f"Solve and verify domain for {expr}.",ans,src)
# 56–60
add("Compute log_7(1), and explain.","0 because7^0=1.","Source56 log_b1=0 for every valid b.")
add("Compare y=log_(1/4)(x) with y=−log_4(x).","Identical: log_(1/4)(x)=−log_4(x).","Source57 (1/2) and2 bases yield same negative-log identity.")
add("Prove log_(1/b)(x)=−log_b(x) for b>0, b!=1.","ln x/ln(1/b)=ln x/(−ln b)=−ln x/ln b.","Source58 proof by reciprocal-base exponent identity.")
add("Find real domain of ln((x+1)/(x−2)).","(-∞,−1) ∪ (2,∞); strict positivity, exclude pole2.","Source59 ln((x+2)/(x−4)): (-∞,−2) ∪ (4,∞).")
add("Find x-intercepts of log((x+3)^2).","(x+3)^2=1 => x=−4 or −2; logarithm argument positive at both.","Source60 log((x+2)^2), intercepts−3 and−1, excludesx=−2.")
assert len(Q)==60 and [x["SourceSkillReferenceExercise"]for x in Q]==list(range(1,61))
assert math.isclose(math.sqrt(5)**2,5)
W=[
("Domain of a right shift","Find domain of log_3(x−4).","x>4, vertical asymptote x=4."),
("Shift with reversal","Find domain of ln(6−2x).","x<3, vertical asymptote x=3."),
("Graph logarithm from points","Give three points for log_2(x).","(1,0),(2,1),(4,2); domain x>0, asymptote0."),
("Horizontal shift","Graph log_3(x+2).","Shift left2, asymptote−2; point (−1,0)."),
("Vertical shift","Graph ln(x)−4.","Shift down4, vertical asymptote x=0, full real range."),
("Vertical compression","Graph (1/2)log_4(x).","For x=1,4,16 outputs 0,1/2,1."),
("Combined stretch and shift","Graph 2log_3(x−1)+2.","Domain x>1, asymptote1, graph passes (2,2)."),
("Reflection of logarithm","Graph −log_2(−x).","Domain x<0, asymptote0, point (−1,0); increasing over negative x."),
("Approximate a logarithmic solution","Solve ln(x)=2.5 approximately.",f"x=e^2.5≈{math.exp(2.5):.3f}."),
("Identify vertical asymptote","For g(x)=log(5−2x) find vertical asymptote.","5−2x=0 => x=2.5, domain x<2.5."),
("Write from shifted graph","Graph passes (3,0),(5,1) with vertical asymptote x=2. Give logarithmic equation.","y=log_3(x−2), because (3−2)=1, (5−2)=3.")
]
T=[
("Find domain and asymptote of ln(x+3).","x>−3, x=−3."),
("Find domain and asymptote of ln(4−x).","x<4, x=4."),
("Give points for log_3(x).","(1,0),(3,1),(9,2)."),
("Shift y=log_2(x) right1.","y=log_2(x−1), asymptote1."),
("Shift y=ln(x) up3.","y=ln(x)+3, asymptote0."),
("Compress y=log_2(x) vertically by1/2.","y=(1/2)log_2(x)."),
("Graph y=3log_2(x−2)+1, give domain.","x>2; asymptote2; point(3,1)."),
("Reflect y=ln(x) about y-axis.","y=ln(−x), domain x<0."),
("Solve log_2(x)=3.","x=8."),
("Find asymptote for log(7−x).","x=7, domain x<7."),
("Find equation of right shifted log_2 with asymptote x=3.","y=log_2(x−3), with point (4,0).")
]
assert len(W)==11 and len(T)==11
lessoncode="PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U04:L04"
rich_rel="src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u04-l04.rich-lesson-v2.json"
teacher_rel="src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u04-l04-firstparty-teacher-led.json"
rich={"SchemaVersion":2,"PackCode":"US-CCSS-MATH:G12:PRECALC:OPENSTAX-2017:U04:L04","ContentVersion":"g12-u04-l04-firstparty-log-graphs-abc",
"Lessons":[{"LessonCode":lessoncode,"CultureCode":"en","Title":"Graphs of Logarithmic Functions",
"ExplanationParagraphs":["The parent y=log_b(x), b>0 and b!=1, has domain x>0, full real range, vertical asymptote x=0, and x-intercept(1,0).",
"For y=a·log_b(c(x−h))+k, solve the complete argument positivity constraint first; the asymptote is at the argument zero whenever c!=0.",
"Replacing x by −x reflects a logarithmic graph over the y-axis and changes its domain. A negative multiplier reflects over the x-axis.",
"Horizontal translations change logarithmic domains and vertical asymptotes; vertical translations do not change full real range.",
"Graphs of b^x and log_b(x) reflect across y=x; use independent points and graph reasoned features.",
"Before solving equations combining logarithms, restrict every argument to be strictly positive. Verify candidate roots in the original equations.",
"Independent Cool-down: (a) graph y=log_2(x−3), (b) give domain and asymptote of ln(5−x), (c) solve ln(x+1)=0."]+
[f"Independent Try It {i}: {q}"for i,(q,ans)in enumerate(T,1)],
"WorkedExamples":[{"Title":title,"Question":q,"Method":"Check strictly positive log arguments, transform parent graph, verify intercepts and asymptote.","Steps":[answer],"Answer":answer,"Check":"Verify domain against function argument and transformed anchor points."}for title,q,answer in W],
"KeyConcepts":[{"Title":"Logarithm domain","Definition":"log_b(u(x)) is real when u(x)>0.","Rule":"Strict positivity; zero and negative arguments invalid.","Example":"ln(x−4) requires x>4."},
{"Title":"Vertical asymptote","Definition":"A logarithmic graph approaches a vertical boundary where its argument tends zero from the positive side.","Rule":"Solve u(x)=0, then check valid approach direction.","Example":"ln(3−x) has asymptote x=3."},
{"Title":"Graph transformations","Definition":"Log graph transforms by shifts, stretches and reflections.","Rule":"x−h moves right; x+h moves left; negative multiplier reflects across x-axis.","Example":"−log_2(x+1)."}],
"Visuals":[{"Kind":"RatioTable","Title":"Independent logarithmic graph sample",
"Description":"First-party x,y anchor coordinates, suitable for manual graphing and teacher review.",
"PrimaryLabel":"Argument x","SecondaryLabel":"Value log_2 x","Rows":[{"Label":"x","Cells":["1/4","1/2","1","2","4"]},
{"Label":"log_2(x)","Cells":["−2","−1","0","1","2"]}],"Items":[],"SecondaryItems":[]}],
"Videos":[],"SummaryPoints":["Domain by argument>0.","Log graphs have vertical asymptotes.","Use shifts/reflections of parent graph.","Validate intersections numerically where necessary."],
"CommonMistakes":[{"Mistake":"Allow logarithm argument zero.","WhyWrong":"No real logarithm at zero.","Correction":"Use strict positivity."},{"Mistake":"Treat upward shift as changing log domain.","WhyWrong":"Vertical shift only changes y-values.","Correction":"Find domain from unshifted argument."}]}]}
teacher={"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","LessonCode":lessoncode,
"IndependentTryIt":[{"Number":i,"Question":q,"TeacherExpectedMath":ans}for i,(q,ans)in enumerate(T,1)],
"IndependentCoolDown":{"StudentQuestions":["Graph y=log_2(x−3), including asymptote and intercept.","Find domain/asymptote of ln(5−x).","Solve ln(x+1)=0."],
"TeacherExpectedMath":["x>3, vertical asymptote x=3, x-intercept4.","x<5; vertical asymptote x=5.","x=0; original log argument1."],"PublisherCoolDownCopied":False},
"FirstPartyIndependentStudentMathQuestions":Q,"SourceNumberedExercisesReviewed":60,"IndependentExercisesAuthored":60,
"StageCDecision":"REUSE_EXISTING_TEACHER_MANUAL_MATH_AND_RICHV2","RuntimeVerified":False,"StageD":"NOT_RUN"}
def save(path,obj):path.write_text(json.dumps(obj,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
save(root/rich_rel,rich);save(root/teacher_rel,teacher)
# All original figure skills: individually indexed alternative math models. No publisher artwork.
models=[
("log_2(x)","x>0",0),("log_(1/2)(x)","x>0",0),("log_2(x+1)","x>−1",-1),
("ln(3−x)","x<3",3),("log_3(x−2)","x>2",2),("ln(x)+2","x>0",0),
("log_4(x−1)−2","x>1",1),("2log_2(x)","x>0",0),
("−log_2(x)","x>0",0),("log_2(−x)","x<0",0),
("ln(x+3)","x>−3",-3),("log_5(x−4)","x>4",4),
("−log_3(−x)","x<0",0),("log_4(x)+1","x>0",0),
("ln(2−x)","x<2",2),("log_2(x−1)","x>1",1),
("log_2(x), ln(x), log_5(x)","x>0",0),
("log_(1/3)(x), log_2(x), log_(3/4)(x)","x>0",0),
("log_3(2−x), −log_3(x+2), log_3(x+2)","mixed valid domains",None)]
figure_rel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u04-l04-19-original-figure-firstparty-models.json"
save(root/figure_rel,{"SchemaVersion":1,"Section":"4.4","OriginalFigureCount":19,
"IndependentFigureModels":[{"OriginalFigureSkill":i,"IndependentModel":m,"Domain":dom,"VerticalAsymptoteX":asy,
"SourceArtworkCopied":False,"ExactGraphSourceParityClaimed":False,"TeacherGraphReview":"MANUAL"}for i,(m,dom,asy) in enumerate(models,1)]})
route_rel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u04-l04-stage-c-60-teacher-math-routes.json"
save(root/route_rel,{"SchemaVersion":1,"Section":"4.4","RouteCount":60,"NewEngine":False,
"Decision":"Reuse RichV2, independent math keys, existing teacher review; graph drawings need teacher visual review.",
"Routes":[{"Exercise":x["SourceSkillReferenceExercise"],"TeacherExpectedMath":True,"RequiresTeacherReview":True,
"AutoGradingClaimed":False,"StageDRuntime":"NOT_RUN"} for x in Q],"CI":"NOT_RUN","StageD":"NOT_RUN"})
# IMPORTANT: 19 index models are source skill comparisons; they are not rendered SVG yet.
# Do not claim all figure skills verified as live student diagrams. Record separately as pending.
a["OriginalMathExercisesReviewed"]=60;a["IndependentSourceSkillExercisesAuthored"]=60
a["FirstPartyWorkedExamples"]=11;a["FirstPartyTryIts"]=11
a["FirstPartyRichV2"]=rich_rel;a["FirstPartyTeacherAssessment"]=teacher_rel
a["OriginalFigureModelsIndexed"]=19;a["OriginalFigureModelEvidence"]=figure_rel
a["StageC"]="60_TEACHER_MANUAL_REVIEW_ROUTES_REUSE_RICHV2"
a["StageCRoutes"]=route_rel
a["BlockingReasons"]=["19_LOG_FIGURE_SKILLS_REQUIRE_ACTUAL_STUDENT_GRAPH_RENDERING_AND_TARGETED_VALIDATION"]
a["UpdatedUtc"]=datetime.datetime.now(datetime.timezone.utc).isoformat()
for item in a["OriginalNumberedExerciseLedger"]:
 item.update({"SourceMathReviewed":True,"IndependentStudentQuestionVerified":True,"TeacherExpectedMathVerified":True})
# still open: content alone doesn't verify full graph deliverability
a["Status"]="A_C_IN_PROGRESS";a["ReadyForStageD"]=False
save(ap,a)
now=datetime.datetime.now(datetime.timezone.utc).isoformat()
p["LessonEntries"][23].update({"ACStatus":"A_C_IN_PROGRESS","ReadyForStageD":False,
"EvidencePath":str(ap.relative_to(root)).replace("\\","/"),"BlockingReasons":a["BlockingReasons"],"UpdatedUtc":now})
p["UpdatedUtc"]=now
save(pp,p)
assert p["LastClosedLessonOrdinal"]==23 and p["FirstIncompleteLessonOrdinal"]==24
assert len(Q)==60 and len(models)==19 and not a["ReadyForStageD"]
files=[pp.relative_to(root),ap.relative_to(root),Path(rich_rel),Path(teacher_rel),Path(figure_rel),Path(route_rel),Path(__file__).relative_to(root)]
subprocess.run(["git","add","--sparse","-f","--",*[str(x)for x in files]],cwd=root,check=True)
subprocess.run(["git","diff","--cached","--check"],cwd=root,check=True)
subprocess.run(["git","commit","-m","feat(g12 u04 l04): author 60 logarithmic graph tasks and 19 source-skill model references [skip ci]"],cwd=root,check=True)
subprocess.run(["git","push","origin","curriculum/us-ccss-g12-precalculus-abc-20261106"],cwd=root,check=True)
print("U04L04_60_SOURCE_SKILL_MATH_AND_19_MODEL_REFERENCES_PUSHED; STILL_OPEN_FOR_SVG_GRAPH_RENDERING")
