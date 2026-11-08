from pathlib import Path
import json
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
source=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit03\lesson004.json").read_text(encoding="utf-8"))
assert source["Section"]=="3.4" and (source["Pages"][0]["PdfPage"],source["Pages"][-1]["PdfPage"])==(254,271)
worked=[
("Recognize smooth polynomial graphs","Among y=x³−x, y=|x| and y=1/x, classify which has a polynomial graph.","x³−x is polynomial, smooth and continuous; |x| corner and 1/x gap/asymptote are not polynomials."),
("Factor a polynomial for its real zeros","Find x-intercepts of p(x)=x⁶−5x⁴+4x².","Factor x²(x²−1)(x²−4); zeros x=0,±1,±2; x0 has double multiplicity."),
("Factor by grouping","Find zeros of p(x)=x³−3x²−4x+12.","(x−3)(x−2)(x+2); zeros −2,2,3."),
("Read intercepts from factored form","For p(x)=(x−2)²(2x+3), give x-zero multiplicities and y-intercept.","x=2 multiplicity2 (touch), x=−3/2 multiplicity1 (cross), p(0)=12."),
("Find roots by testing possible factors","Find exact roots of p(x)=x³+3x²−4x−12.","(x+3)(x−2)(x+2), roots −3,−2,2."),
("Identify root multiplicity from factors","For p(x)=(x−2)³(x+1)², list zeros and graph crossing behavior.","x=2 multiplicity3 crosses with flattening, x=−1 multiplicity2 touches/bounces."),
("Bound turning points","For degree7 polynomial, what is the maximum possible number of turning points and real x-intercepts?","At most6 turns, at most7 distinct real zeros."),
("Sketch factored polynomial","Sketch p(x)=−2(x+3)²(x−5) with correct end behavior and root behavior.","Degree3 leading−2; left up/right down; x=−3 double touches; x=5 simple crosses."),
("Intermediate Value Theorem","Show p(x)=x³−3x−1 has a zero in (1,2).","p(1)=−3, p(2)=1; by continuity a real zero lies between."),
("Fit polynomial from zeros and point","A polynomial has roots −1 simple and 3 double and passes through (0,−9). Find formula.","p(x)=−(x+1)(x−3)², because p(0)=9a=−9 gives a=−1."),
("Optimize open box volume","Cut squares of width w from corners of 12×18 sheet, fold into box. Model V(w) and its physical domain.","V(w)=w(12−2w)(18−2w), 0<w<6; check derivative or graph on this domain for max.")
]
tries=[
("Root/axis intercepts","Find all real x-intercepts of p(x)=x⁴−10x²+9 and y-intercept.","(x²−1)(x²−9), roots ±1,±3; y-intercept9."),
("Use multiplicity graph","A polynomial has simple zero x=−2, double zero x=1, another simple zero x=3. Explain crossing/touching.","Cross at −2 and3, touch at1; least possible degree4."),
("Sketch given factors","Sketch p(x)=x(x−1)⁴(x+3)³ and classify tails.","Degree8 positive leading: both tails up. Cross at0 and−3 (odd mult), touch at1 (even mult)."),
("Apply IVT with sign change","Show p(x)=x³−2x−2 has a real zero between x1 and x2.","p(1)=−3,p(2)=2, continuous so some c∈(1,2) with p(c)=0."),
("Build from a graph description","A degree3 polynomial crosses at x=−1, touches at x=2 and passes through (0,8). Find lowest-degree formula.","p(x)=2(x+1)(x−2)², because p(0)=8."),
("Local extrema on interval","For f(x)=x³−3x on [−2,2], find global max/min including endpoints.","f(−2)=−2,f(−1)=2,f(1)=−2,f(2)=2, max2 at −1 and2, min−2 at −2 and1.")
]
concepts=[
("Polynomial graph continuity","Polynomials are continuous at every real input.","No finite jumps, gaps or vertical asymptotes.","x³−x."),
("Smoothness","Polynomial graphs have derivatives of all orders.","Sharp corners suggest non-polynomial rules.","|x| is not polynomial."),
("Root factorization","p(r)=0 iff (x−r) divides p(x).","Find real x-intercepts from factored p(x).","(x−2)(x+1) zeros2,−1."),
("Multiplicity","Factor (x−r)^m establishes root multiplicity m.","Even m touches, odd m crosses.","(x−1)² touches x1."),
("Leading term tails","Degree parity and sign of leading coefficient determine end behavior.","Even: same direction, odd: opposite.","−x³ left up/right down."),
("Y-intercept","Evaluate p(0) for y-intercept.","Often use it to determine unknown scalar factor.","a(x−1)(x+2) at zero −2a."),
("Polynomial turning-point bound","Degree n polynomial has at most n−1 turning points.","No claim that maximum number must occur.","Degree5 at most4 turns."),
("Intermediate Value Theorem","A continuous function whose values at a and b have opposite signs has a zero in (a,b).","Show f(a)f(b)<0 and cite continuity.","x³−3x−1 signs change between1 and2."),
("Polynomial reconstruction","Use zeros, multiplicities and a known nonzero point to determine scale.","p(x)=a∏(x−r)^m.","Zeros simple−1, double3 lead degree3."),
("Local versus global extrema","Local maxima/minima can differ from interval-wide extrema.","Check interval endpoints and interior critical points.","x³−3x on [−2,2].")
]
cool={"StudentPrompt":"(a) Describe end behavior and root multiplicity of −(x−2)²(x+1). (b) Show x³−x−1 has a zero in (1,2). (c) Construct a quadratic with double zero at3 and y-intercept8.","TeacherExpectedMath":"(a) Degree3 leading−1: left up/right down, x2 touches, x−1 crosses. (b) f(1)=−1,f(2)=5, IVT. (c) (8/9)(x−3)².","OriginalPublisherRestrictedCoolDownCopied":False}
lesson={"CultureCode":"en","Title":"Graphs of Polynomial Functions",
"LessonCode":"PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U03:L04",
"ExplanationParagraphs":[
 "Learning goals: distinguish smooth polynomial graphs from graphs with corners or discontinuities, factor to find zeros, recognize multiplicities, determine tails and turning-point bounds, sketch graphs and apply the Intermediate Value Theorem.",
 "Polynomial p(x)=a∏(x−r)^m describes zeros and their multiplicities: odd m crosses the x-axis, even m touches it.",
 "The leading coefficient and parity of total degree govern tails. A degree n polynomial has at most n−1 turning points; these are limits, not guaranteed counts.",
 "When a continuous polynomial changes sign across an interval, the Intermediate Value Theorem guarantees at least one root inside, without claiming its exact position.",
 "Construct a formula by multiplying factors corresponding to roots and use a known y-intercept or another coordinate to determine the scale a.",
 "For optimization, compare all critical points in the physical interval plus endpoints; a local extremum alone may not be globally optimal.",
 "Independent Edulytics Cool-down: "+cool["StudentPrompt"]
 ]+[f"Independent Try It {i} — {title}. {prompt}"for i,(title,prompt,key)in enumerate(tries,1)],
"WorkedExamples":[{"Title":title,"Question":q,"Method":"Analyze factorization, zero multiplicity, tail degree, sign changes and valid domain.","Steps":[a],"Answer":a,"Check":"Check evaluated zeros and signs with exact arithmetic."}for title,q,a in worked],
"KeyConcepts":[{"Title":title,"Definition":de,"Rule":ru,"Example":ex}for title,de,ru,ex in concepts],
"Visuals":[
 {"Kind":"RatioTable","Title":"Root multiplicity graph behavior","Description":"Independent first-party root classification.","PrimaryLabel":"Multiplicity","SecondaryLabel":"Graph","Rows":[{"Label":"Power of factor","Cells":["1","2","3","4"]},{"Label":"At root","Cells":["Crosses","Touches","Crosses/flattens","Touches/flattens"]}],"Items":[],"SecondaryItems":[]},
 {"Kind":"RatioTable","Title":"IVT sign-change demonstration","Description":"Independent polynomial p(x)=x³−3x−1.","PrimaryLabel":"x","SecondaryLabel":"p(x)","Rows":[{"Label":"x","Cells":["0","1","2","3"]},{"Label":"p(x)","Cells":["−1","−3","1","17"]}],"Items":[],"SecondaryItems":[]}
 ],"Videos":[],
"SummaryPoints":["Polynomial graph is continuous and smooth.","Real zeros and multiplicities arise from factors.","Odd roots cross; even roots touch.","Leading term determines end behavior.","IVT detects at least one zero between opposite signs."],
"CommonMistakes":[{"Mistake":"Thinking every zero corresponds to a crossing.","WhyWrong":"Even-multiplicity roots touch and reverse direction.","Correction":"Identify multiplicity of each factor."},
{"Mistake":"Treating IVT as finding an exact root.","WhyWrong":"It only guarantees existence between opposite signs.","Correction":"Use factorization or numerical root method for exact/approximate value."},
{"Mistake":"Using only critical points in constrained optimization.","WhyWrong":"A boundary value may be greater/smaller.","Correction":"Check both endpoints and valid interior critical points."}]}
rich=root/"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u03-l04.rich-lesson-v2.json"
assert not rich.exists()
rich.write_text(json.dumps({"SchemaVersion":2,"PackCode":"US-CCSS-MATH:G12:PRECALC:OPENSTAX-2017:U03:L04",
"ContentVersion":"g12-u03-l04-firstparty-polynomial-graphs-20261107","Lessons":[lesson]},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
(base/"u03-l04-firstparty-six-tryit-teacher-key-and-cooldown.json").write_text(json.dumps({
"OriginalTryItSkills":6,"FirstPartyIndependentTryIts":6,
"TeacherExpectedMathematics":[{"Number":i,"Expected":answer}for i,(_,_,answer)in enumerate(tries,1)],
"IndependentCoolDown":cool,"OriginalPublisherArtworkCopied":False,"StageD":"NOT_RUN"},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
cp={"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":3,"Lesson":4,
"OriginalSourceSection":"3.4","OriginalSourceSHA256":source["SourceSha256"],
"OriginalSourcePdfPages":[254,271],
"SourceWorkedExampleSkills":11,"OriginalSourceTryIts":6,"OriginalNumberedExerciseAnchors":79,
"FirstPartyWorkedExamples":11,"FirstPartyTryItAlternatives":6,
"FirstPartyKeyConcepts":10,"FirstPartyVisualTables":2,"IndependentCoolDown":True,
"RichV2Path":rich.relative_to(root).as_posix(),
"SourceOriginal79QuestionMathSkillContracts":"PENDING",
"OriginalSourceFigureMathSkills":"PENDING",
"ReadyForStageD":False,"StageD":"NOT_RUN","CI":"NOT_RUN",
"StartedAtUtc":datetime.now(timezone.utc).isoformat()}
(base/"u03-l04-abc-checkpoint.json").write_text(json.dumps(cp,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
progress=base/"full-grade-progress.json"
p=json.loads(progress.read_text(encoding="utf-8"))
assert p["LastClosedLessonOrdinal"]==14 and p["FirstIncompleteLessonOrdinal"]==15
row=p["LessonEntries"][14]
row["ACStatus"]="A_C_IN_PROGRESS";row["FirstPartyRichV2Path"]=cp["RichV2Path"]
row["EvidencePath"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u03-l04-abc-checkpoint.json"
row["ReadyForStageD"]=False
row["BlockingReasons"]=["79 numbered exercise cognitive math actions not independently authored or rubricked","Original graph figure skills not crosswalked"]
p["UpdatedUtc"]=cp["StartedAtUtc"]
progress.write_text(json.dumps(p,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("G12_U03_L04_11_WORKED_6_TRYIT_10_CONCEPTS_2_VISUALS_AND_COOLDOWN_AUTHORED")
