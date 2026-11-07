from pathlib import Path
import json
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
source=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit03\lesson003.json").read_text(encoding="utf-8"))
assert source["Section"]=="3.3" and (source["Pages"][0]["PdfPage"],source["Pages"][-1]["PdfPage"])==(239,253)
worked=[
("Recognize a single power term","For each f(x)=3x⁴, g(x)=x³−x and h(x)=7 identify a power function or sum of powers.","f is power a·xⁿ, g is polynomial not single power, h=7·x⁰ is constant power."),
("Even positive end behavior","Describe end behavior of f(x)=4x⁶.","As x→±∞, f(x)→+∞, positive even leading power."),
("Odd negative end behavior","Describe end behavior of f(x)=−2x⁵.","As x→−∞ f→+∞ and x→+∞ f→−∞."),
("Recognize polynomials","Classify f(x)=x³−2x²+1, g(x)=√x+2, h(x)=1/x.","Only f is polynomial; nonnegative integer exponents required."),
("Read leading degree and coefficient","For f(x)=−3x⁷+2x⁴−1, determine degree, leading term, coefficient.","Degree7, leading term −3x⁷, leading coefficient −3."),
("Infer degree from end behavior","A continuous smooth polynomial graph goes down at left and up at right, with 4 turning points. Find lowest plausible degree.","Odd degree with positive leading coefficient; at least degree5."),
("Determine product degree and sign","For f(x)=−2x²(x−1)(x+3), find leading coefficient, degree and end behavior.","Leading term −2x⁴, degree4, both ends negative."),
("Intercepts from factored polynomial","For p(x)=(x−2)(x+1)(x−4), find y-intercept and all zeros.","Zeros 2,−1,4; p(0)=8."),
("Factor quadratic in x²","Find real zeros and y-intercept for f(x)=x⁴−5x²+4.","(x²−1)(x²−4); zeros ±1,±2; f(0)=4."),
("Bound turning points","What maximum number of real zeros and turning points can a nonzero degree6 polynomial have?","At most6 distinct real zeros and5 turning points."),
("Reason from plotted intercepts","A polynomial has three distinct x-intercepts and two changes of direction, crossing the axis each time. What minimum degree is possible?","Degree3 is possible; at least3 from distinct zeros, at least3 from two turning points."),
("Local behavior from simple factors","For p(x)=−3x(x+2)(x−4), classify crossings, y-intercept and end behavior.","Zeros −2,0,4 all multiplicity1 crossing; degree3 leading −3; left up right down.")
]
tries=[
("Classify polynomial and power","Is 2x²·4x³ a power function? Is x⁵−x a power function?","First simplifies 8x⁵ so yes, second polynomial not one power."),
("Even negative power tails","State end behavior of −5x⁴.","Both ends approach −∞."),
("Leading coefficient after rearranging","For 4x²−x⁶+2x−6 determine degree and leading coefficient.","Degree6, leading term −x⁶, leading coefficient −1."),
("Least degree from turning points","A smooth continuous curve changes direction 3 times and tails both rise. What minimum polynomial degree is possible?","At least degree4, even positive leading coefficient."),
("Expand factors and infer tails","Find leading term and degree of 0.2(x−2)(x+1)(x−5).","Leading term 0.2x³, odd positive: left down right up."),
("Find polynomial zeros","Find intercepts of f(x)=2x³−6x²−20x.","2x(x−5)(x+2), zeros 0,5,−2; y-intercept0."),
("Maximum turning point count","For degree12 polynomial, state max x-intercepts and turning points.","At most12 x-intercepts and11 turning points."),
("Use graph root multiplicity","A polynomial graph touches at x=2 and crosses at x=−1. Find a simple polynomial model.","p(x)=(x−2)²(x+1) has double root 2, simple root −1."),
("Crossing behavior from factors","For f(x)=0.2(x−2)(x+1)(x−5), state intercept crossing behavior.","Simple zeros −1,2,5, each crosses x-axis.")
]
concepts=[
("Power function","One term a·xⁿ with constant a and exponent n≥0 for polynomial power functions.","The exponent is power, a its coefficient.","−3x⁵."),
("Polynomial definition","Finite sum of nonnegative integer powers of x with real coefficients.","Fractional or negative x powers are not polynomials.","2x³−x+7."),
("Degree","Largest exponent with nonzero coefficient.","Product degrees add unless zero polynomial factors.","x²(x−1) degree3."),
("Leading coefficient","Coefficient of the highest-degree term.","Leading term determines both end behaviors.","−2x⁶+5 has leading coefficient −2."),
("Even-degree behavior","Both tails of a polynomial travel in the same vertical direction.","Sign of leading coefficient decides up or down.","−x⁴ → both down."),
("Odd-degree behavior","Polynomial tails travel in opposite vertical directions.","Positive leading→left down/right up.","x³ → down-left/up-right."),
("Roots and intercepts","An x-intercept has p(x)=0; y-intercept p(0).","Factored form makes zeros explicit.","(x−2)(x+1) roots 2,−1."),
("Multiplicity","Odd root multiplicity crosses x-axis, even multiplicity touches/bounces.","Graph behavior near a root depends on multiplicity parity.","(x−2)² touches at2."),
("Turning point bound","Degree n≥1 polynomial has at most n−1 turning points.","Number of distinct real roots at most n.","Degree5 max4 turns."),
("Continuity/smoothness","Polynomial graphs are continuous and smooth without corners or discontinuities.","A graph with a hole or jump cannot be a polynomial graph.","x³−4x is continuous.")
]
visual=[
{"Kind":"RatioTable","Title":"Polynomial tails from leading term","Description":"First-party summary of degree parity and leading sign.","PrimaryLabel":"Degree","SecondaryLabel":"Tails","Rows":[{"Label":"Degree parity","Cells":["Even","Even","Odd","Odd"]},{"Label":"Leading sign","Cells":["+","−","+","−"]},{"Label":"Left tail","Cells":["Up","Down","Down","Up"]},{"Label":"Right tail","Cells":["Up","Down","Up","Down"]}],"Items":[],"SecondaryItems":[]},
{"Kind":"RatioTable","Title":"Independent cubic polynomial values","Description":"Values for p(x)=x(x−2)(x+2) around roots.","PrimaryLabel":"x","SecondaryLabel":"p(x)","Rows":[{"Label":"x","Cells":["−3","−2","−1","0","1","2","3"]},{"Label":"p(x)","Cells":["−15","0","3","0","−3","0","15"]}],"Items":[],"SecondaryItems":[]}
]
cool={"StudentPrompt":"(a) Is 4x³−2x a single power function? (b) Determine tails for −2x⁷+5x². (c) Find degree and real zeros of 3x(x−1)²(x+2).","TeacherExpectedMath":"(a) No, polynomial sum of two powers. (b) Left up, right down. (c) Degree4, zeros −2,0,1; multiplicity2 touches at x1.","OriginalPublisherRestrictedCoolDownCopied":False}
lesson={"LessonCode":"PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U03:L03",
"CultureCode":"en","Title":"Power Functions and Polynomial Functions",
"ExplanationParagraphs":[
 "Learning goals: identify power functions and their end behavior; identify polynomials; find polynomial degree and leading coefficient.",
 "A power function is a single term a·x^n. Polynomial functions are finite sums of nonnegative integer powers with real coefficients.",
 "At large |x|, the leading term dominates: even-degree tails have the same sign, odd-degree tails opposite signs. For positive leading coefficients, the right tail rises; negative falls.",
 "Factors reveal real zeros. At a zero of odd multiplicity the curve crosses the horizontal axis; at even multiplicity it touches or bounces.",
 "A degree-n polynomial has at most n distinct real zeros and at most n−1 turning points, and a polynomial graph is continuous and smooth.",
 "Independent Edulytics cool-down: "+cool["StudentPrompt"]
 ]+[f"Independent Try It {i} — {title}. {question}"for i,(title,question,key)in enumerate(tries,1)],
"WorkedExamples":[{"Title":title,"Question":q,"Method":"Use polynomial definitions, factorization, parity, degree and end behavior.","Steps":[a],"Answer":a,"Check":"Verify leading term and evaluate given roots."}for title,q,a in worked],
"KeyConcepts":[{"Title":title,"Definition":definition,"Rule":rule,"Example":example}for title,definition,rule,example in concepts],
"Visuals":visual,"Videos":[],
"SummaryPoints":["Power function is one term; polynomial finite sum.","Degree is highest nonzero power and leading coefficient controls tails.","Odd/even degree determines whether tails match.","Factorization gives roots and multiplicities.","Degree n bounds real zeros and turning points."],
"CommonMistakes":[{"Mistake":"Calling x⁻² a polynomial.","WhyWrong":"Exponent is a negative integer.","Correction":"Polynomial powers must be nonnegative integers."},
{"Mistake":"Concluding root multiplicity from leading sign.","WhyWrong":"Crossing/touching needs local factor multiplicity.","Correction":"Factor around each zero."},
{"Mistake":"Claiming degree n guarantees n real zeros.","WhyWrong":"n is only the maximum; some roots can be complex or repeated.","Correction":"Use at most n distinct real zeros."}]}
rich=root/"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u03-l03.rich-lesson-v2.json"
assert not rich.exists()
rich.write_text(json.dumps({"SchemaVersion":2,"PackCode":"US-CCSS-MATH:G12:PRECALC:OPENSTAX-2017:U03:L03",
"ContentVersion":"g12-u03-l03-firstparty-source-skill-math-20261107","Lessons":[lesson]},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
(base/"u03-l03-original-nine-tryit-firstparty-cooldown.json").write_text(json.dumps({
"OriginalSourceTryItCount":9,"FirstPartyTryItCount":9,
"TeacherExpectedMath":[{"Number":i,"Expected":answer}for i,(_,_,answer)in enumerate(tries,1)],
"FirstPartyCoolDown":cool,"PublisherFiguresCopied":False,"StageD":"NOT_RUN"
},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
cp={
"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":3,"Lesson":3,
"SourceSection":"3.3","OriginalBookSHA256":source["SourceSha256"],
"OriginalPdfPages":[239,253],"SourceWorkedExamples":12,
"SourceTryItActivities":9,"SourceNumberedExercises":70,
"FirstPartyWorkedExamples":12,"FirstPartyTryIts":9,"FirstPartyConcepts":10,
"FirstPartyVisualTables":2,"IndependentCoolDown":True,
"FirstPartyRichV2Path":rich.relative_to(root).as_posix(),
"Original70ExerciseSkillTeacherRubrics":"PENDING","SourceFigureSkillCrosswalk":"PENDING",
"ReadyForStageD":False,"StageD":"NOT_RUN","CI":"NOT_RUN",
"StartedAtUtc":datetime.now(timezone.utc).isoformat()}
(base/"u03-l03-abc-checkpoint.json").write_text(json.dumps(cp,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
progress=base/"full-grade-progress.json"
p=json.loads(progress.read_text(encoding="utf-8"))
assert p["LastClosedLessonOrdinal"]==13 and p["FirstIncompleteLessonOrdinal"]==14
row=p["LessonEntries"][13]
row["ACStatus"]="A_C_IN_PROGRESS";row["FirstPartyRichV2Path"]=cp["FirstPartyRichV2Path"]
row["EvidencePath"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u03-l03-abc-checkpoint.json"
row["ReadyForStageD"]=False
row["BlockingReasons"]=["70 numbered source exercise academic skill question contracts not authored","Source figure skill crosswalk pending"]
p["UpdatedUtc"]=cp["StartedAtUtc"]
progress.write_text(json.dumps(p,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("G12_U03_L03_12_WORKED_9_TRYITS_10_CONCEPTS_2_VISUALS_COOLDOWN_AUTHORED")
