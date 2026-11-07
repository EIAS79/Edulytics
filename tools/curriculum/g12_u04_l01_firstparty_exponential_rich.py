from pathlib import Path
import json,datetime,math
from fractions import Fraction as F
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
ap=base/"u04-l01-source-inventory-and-corrected-boundary.json"
a=json.loads(ap.read_text(encoding="utf8"))
assert len(a["OriginalSourceWorkedExamples"])==12
worked=[
("Recognize an exponential function","Which is exponential: 5x², 5(2)^x, or 5x+2?","The function 5(2)^x is exponential: x is in the exponent and base 2 is positive and not 1."),
("Evaluate powers and order","Evaluate g(x)=3·2^(x+1)−4 at x=2.","g(2)=3·2³−4=20."),
("Interpret a growth model","A colony starts at 500 cells and grows 8% per hour. Model and predict after 3 hours.","N(t)=500(1.08)^t; N(3)=500(1.08)³≈629.856 cells. Counts may be rounded to 630."),
("Find model with known initial value","An investment is worth 800 initially and rises by factor 1.1 annually. Write value function.","V(t)=800(1.1)^t for t≥0. Initial value V(0)=800."),
("Find exponential function from two points","An exponential f(x)=ab^x satisfies f(1)=6 and f(3)=54. Find f.","f(3)/f(1)=b²=9 so b=3; a=6/3=2. f(x)=2·3^x."),
("Model graph intercept and ratio","Exponential graph has y-intercept 4 and points (1,8),(2,16). Find function.","f(x)=4·2^x, with positive range and horizontal asymptote y=0."),
("Fit exponential sequence from data","Observed readings x=0,1,2,3 are 5,15,45,135. Fit model.","Ratio 3 per unit, f(x)=5·3^x. Check all four observations."),
("Evaluate compound interest","Deposit $1,200 at APR 6% compounded quarterly for 3 years. Find amount.","A=1200(1+0.06/4)^(4·3)=1200(1.015)^12≈$1434.74."),
("Solve compound interest for principal","What principal yields $2,000 after 2 years at 4% compounded monthly?","P=2000/(1+0.04/12)^(24)≈$1846.3 (calculate before rounding)."),
("Evaluate natural exponential","Evaluate 2e^(−1) to four decimal places.","2/e≈0.7358."),
("Continuous compounding growth","Deposit $3,500 at 5% compounded continuously for 4 years.","A=3500e^(0.05·4)=3500e^0.2≈$4274.91."),
("Continuous decay","A 100 g sample decays continuously at rate 0.07 per hour. Find 6-hour mass.","M(t)=100e^(−0.07t), so M(6)=100e^(−0.42)≈65.7047 g.")
]
tries=[
("Identify form","Classify x^4, (3/4)^x, and 7x.","Only (3/4)^x is exponential; base 0.75 implies decay."),
("Evaluation","Find 2·3^(x−1)+1 at x=3.","2·3²+1=19."),
("Population growth","A city has 20,000 people growing 2% annually. Model 5-year population.","P(t)=20000(1.02)^t; at t=5 ≈22081.6."),
("Initial value","A bacteria culture starts at 200 and doubles every 3 hours. Express count at t hours.","N(t)=200·2^(t/3)."),
("Two observations","Exponential f(0)=7 and f(2)=63. Find model.","a=7, b²=9, b=3; f(x)=7·3^x."),
("Graph ratios","Given graph points (0,6),(1,18),(2,54), write f.","f(x)=6·3^x."),
("Table fitting","Table y=4,12,36 at x=1,2,3. Find f=ab^x.","b=3, a=4/3; f(x)=(4/3)·3^x."),
("Periodic compounding","Find amount of $1,000 at 8% APR compounded twice annually for 2 years.","1000(1+0.08/2)^4≈$1169.86."),
("Solve principal","What deposit gives $5000 after 1 year at 12% compounded monthly?","P=5000/(1.01)^12≈$4437.25."),
("Natural base","Evaluate e²/2.","≈3.694528."),
("Continuous growth","Find $2,000 after 3 years at 3% continuous interest.","2000e^0.09≈$2188.35."),
("Continuous decay","Mass starts at 60 and decays at 10% continuous rate. Find after 5 units.","60e^(−0.5)≈36.3918.")
]
assert len(worked)==12 and len(tries)==12
# Check symbolic examples against independent numerical evaluation instead of unverifiable rounded guesses.
for i in (7,8,10,11):
 print("NUMERIC_REFERENCE",i,[1200*1.015**12,2000/(1+0.04/12)**24,3500*math.exp(.2),100*math.exp(-.42)][[7,8,10,11].index(i)])
print("OTHER",1000*1.04**4,5000/1.01**12,2000*math.exp(.09))
lesson={"LessonCode":"PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U04:L01","CultureCode":"en","Title":"Exponential Functions",
"ExplanationParagraphs":["An exponential function has the form f(x)=a·b^x (a≠0, b>0, b≠1). Its base is a repeated ratio, unlike constant differences in linear models.",
"Evaluate powers first, then products and translations. With positive a, b>1 means exponential growth and 0<b<1 means exponential decay.",
"To fit a model, infer b from output ratios over equal changes in x and solve a from one given point. A table with constant differences is linear; constant nonzero ratios can be exponential.",
"Compound interest uses A=P(1+r/n)^(nt), where the APR r is a decimal, n is compoundings per year and t is measured in years.",
"Continuous compounding A=Pe^(rt), while continuous growth or decay models are f(t)=ae^(kt) with k positive for growth and negative for decay.",
"Currency results should be rounded only after evaluating the full expression; for population and physical quantities, include units and context.",
"Independent Edulytics Cool-down: identify whether 5(1.2)^x is growth, fit exponential with f(0)=3 and f(2)=48, and calculate continuous interest on $1000 for 2 years at 4%."]+[f"Independent Try It {i}: {q}"for i,(_,q,ans) in enumerate(tries,1)],
"WorkedExamples":[{"Title":name,"Question":q,"Method":"Use exact exponential model, ratio, domain and appropriate units.","Steps":[answer],"Answer":answer,"Check":"Check initial value and exponential ratio; evaluate formula and units independently."}for name,q,answer in worked],
"KeyConcepts":[{"Title":"Exponential function","Definition":"f(x)=a·b^x, a≠0, b>0 and b≠1.","Rule":"An exponential has constant output ratio for equal input increments.","Example":"f(x)=4·2^x."},{"Title":"Growth and decay","Definition":"With positive initial amount, exponential base b>1 grows, 0<b<1 decays.","Rule":"B=1+r for growth by fraction r, B=1−r for decay.","Example":"200(1.08)^t or 200(0.92)^t."},{"Title":"Compound interest","Definition":"A=P(1+r/n)^(nt).","Rule":"Nominal APR r as decimal; n annual periods, t years; P and A share currency.","Example":"1200(1+0.06/4)^12."},{"Title":"Natural exponential","Definition":"e≈2.718281828; continuous compounding or natural exponential model.","Rule":"For A=Pe^(rt), rate r=0.05 means 5% per year.","Example":"3500e^(0.2)."}],
"Visuals":[{"Kind":"RatioTable","Title":"Linear differences versus exponential ratios","Description":"Independent table demonstrating the source's comparison skill.","PrimaryLabel":"Input x","SecondaryLabel":"Output y","Rows":[{"Label":"x","Cells":["0","1","2","3"]},{"Label":"Linear 5+3x","Cells":["5","8","11","14"]},{"Label":"Exponential 5·2^x","Cells":["5","10","20","40"]}],"Items":[],"SecondaryItems":[]}],
"Videos":[],"SummaryPoints":["Identify exponential ratio, not additive difference.","Use positive bases not 1.","Distinguish growth from decay.","Fit initial value and base from pairs.","Use compound versus continuous interest correctly."],
"CommonMistakes":[{"Mistake":"Treat APR 6% as 6 in an exponent.","WhyWrong":"APR must be a decimal (0.06).","Correction":"Convert percentage to decimal before applying A=P(1+r/n)^(nt)."},{"Mistake":"Assume constant increases imply exponential data.","WhyWrong":"Constant differences indicate linear, not exponential, behavior.","Correction":"Check ratios for equally spaced inputs."}]}
rel="src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u04-l01.rich-lesson-v2.json"
rp=root/rel
rp.write_text(json.dumps({"SchemaVersion":2,"PackCode":"US-CCSS-MATH:G12:PRECALC:OPENSTAX-2017:U04:L01","ContentVersion":"g12-u04-l01-firstparty-exponential-authoring-draft","Lessons":[lesson]},ensure_ascii=False,indent=2)+"\n",encoding="utf8")
t_rel="src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u04-l01-firstparty-teacher-led.json"
t=root/t_rel
t.write_text(json.dumps({"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","LessonCode":lesson["LessonCode"],
"IndependentTryIts":[{"Number":i,"Prompt":q,"TeacherExpectedMath":answer,"Marking":"TEACHER_MANUAL"}for i,(_,q,answer)in enumerate(tries,1)],
"IndependentCoolDown":{"StudentQuestions":["Classify 5(1.2)^x as growth or decay and justify.","Find exponential model f(x)=ab^x with f(0)=3,f(2)=48.","Find amount at 4% interest compounded continuously for 2 years on $1000."],
"TeacherExpectedMath":["Growth; base 1.2>1.","a=3, b=4, f(x)=3·4^x.","1000e^0.08≈1083.29 dollars."],"PublisherRestrictedCoolDownCopied":False},
"FirstPartyIndependentStudentMathQuestions":[],"RuntimeVerified":False,"StageD":"NOT_RUN"},ensure_ascii=False,indent=2)+"\n",encoding="utf8")
a.update({"FirstPartyWorkedExamplesAuthored":12,"IndependentTryItsAuthored":12,"FirstPartyRichV2":rel,"FirstPartyTeacherAssessment":t_rel,"BlockingReasons":["68_NUMBERED_EXERCISES_REQUIRE_INDIVIDUAL_MATH_REVIEW","6_FIGURE_SKILL_ALTERNATIVES_PENDING","STAGE_C_TEACHER_ASSESSMENT_REUSE_ROUTING_PENDING"],"UpdatedUtc":datetime.datetime.now(datetime.timezone.utc).isoformat()})
ap.write_text(json.dumps(a,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
print("U04L01_12_INDEPENDENT_WORKED_AND_12_TRYITS_CREATED_OPEN")
