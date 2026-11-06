from pathlib import Path
import json,re
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
source=Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit01\lesson002.json")
j=json.loads(source.read_text(encoding="utf-8"))
assert j["Section"]=="1.2" and len(j["Pages"])==16
source_hash=j["SourceSha256"]
code="PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U01:L02"
rich=root/"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u01-l02.rich-lesson-v2.json"
assert not rich.exists(),"Refuse to overwrite existing lesson"
def worked(title,question,steps,answer,check,method="Identify inputs, mathematical restrictions, endpoint inclusions and output values."):
    return {"Title":title,"Question":question,"Method":method,"Steps":steps,"Answer":answer,"Check":check}
examples=[
 worked("Finite domain of a tabulated relation","A function has the finite pairs {(1,11),(3,11),(5,15),(8,20)}. List its domain as a set.",
 ["Keep only first coordinates, since inputs define the domain.","Remove duplicates without changing the input values."],
 "{1,3,5,8}","The repeated output 11 does not change the domain."),
 worked("All-real polynomial domain","Find the domain of the polynomial F(x)=2x³−x+7 in interval notation.",
 ["Powers with whole-number exponents exist at every real input.","No denominator or even radical restricts x."],
 "(−∞,∞)","Substitute any real number; the polynomial is defined."),
 worked("Domain excluding a denominator zero","Determine the real domain of R(x)=(x+4)/(x−3).",
 ["Solve the forbidden denominator condition x−3=0.","Remove 3 from the real line and use an open endpoint on both sides."],
 "(−∞,3)∪(3,∞)","x=3 would divide by zero."),
 worked("Domain of an even root","Find the domain of S(x)=√(11−2x).",
 ["Enforce 11−2x≥0.","Solve −2x≥−11, reversing the inequality when dividing by a negative."],
 "(−∞,11/2]","The endpoint x=11/2 gives zero and is included."),
 worked("Open and closed interval unions","Describe the independent real set −1≤x≤2 or x>4 using set-builder and interval notation.",
 ["The first component includes both −1 and 2.","The second component excludes 4 and has no finite upper endpoint."],
 "{x∈ℝ | −1≤x≤2 or x>4}; [−1,2]∪(4,∞)","Brackets include, parentheses exclude; infinity always uses a parenthesis."),
 worked("Domain and range from plotted V","Graph the first-party model f(x)=|x−1|−2 for all real x. Give its real domain and range.",
 ["The vertex is (1,−2), a minimum attained at x=1.","Both arms extend without bound horizontally and upward."],
 "Domain (−∞,∞); range [−2,∞)","The vertex output −2 is included; no output below −2 occurs."),
 worked("Observed production graph","A city's independently collected waste metric has observed points (2020,120),(2021,170),(2022,140),(2023,200), measured in tons/day. Give observed domain and range; don't assume years outside the sample.",
 ["The observed input years are 2020 through 2023 only.","The observed smallest and largest readings are 120 and 200."],
 "Observed domain {2020,2021,2022,2023}; observed range {120,140,170,200} tons/day.",
 "A finite sample cannot prove behavior outside the observed years."),
 worked("Domain and range of a cubic","Find domain and range of f(x)=x³−4x.",
 ["The polynomial is defined for all real x.","As x→∞ the cubic grows without bound; as x→−∞ it decreases without bound."],
 "Domain (−∞,∞); range (−∞,∞)","Continuity and unbounded opposite end behavior yield every real output."),
 worked("Reciprocal domain and range","For f(x)=4/(x−2), find the real domain and range.",
 ["The denominator x−2 cannot equal zero, so x≠2.","Because the numerator is nonzero, the output cannot equal zero; every other real y occurs for x=2+4/y."],
 "Domain (−∞,2)∪(2,∞); range (−∞,0)∪(0,∞)",
 "Check x=2 undefined and f(x)=0 impossible."),
 worked("Radical domain and range","Find domain and range of f(x)=3√(x+1).",
 ["Require x+1≥0, so x≥−1.","The principal square root is nonnegative, so multiplying by 3 preserves outputs y≥0."],
 "Domain [−1,∞); range [0,∞)","At x=−1 the value is zero and larger inputs yield arbitrarily large outputs."),
 worked("Construct an integer-input piecewise cost rule","A tour costs $6 per participant for groups of 1–7 and a flat $48 for groups of 8 or more. Write C(n).",
 ["People counts n must be positive integers.","For 1≤n≤7, use C(n)=6n.","For n≥8, use C(n)=48."],
 "C(n)=6n for integers 1≤n≤7; C(n)=48 for integers n≥8.",
 "C(7)=42; C(8)=48. The real-line curve must not imply fractional people."),
 worked("Evaluate two piecewise pricing intervals","Let P(g)=20 for 0<g<3 and P(g)=20+8(g−3) for g≥3. Find P(2) and P(5).",
 ["Since 2<3, P(2)=20.","Since 5≥3, P(5)=20+8(2)=36."],
 "P(2)=20; P(5)=36","At g=3 the second branch applies and gives 20."),
 worked("Graph a three-piece definition with endpoints","Let f(x)=x² for x≤0, f(x)=2 for 0<x≤3, and f(x)=x−1 for x>3. Identify open and closed endpoints, domain, and whether the graph is a function.",
 ["At x=0 use the quadratic branch, with filled (0,0) and open (0,2).","At x=3 use the constant branch, with filled (3,2); the last segment begins just right of 3.","Each x belongs to exactly one branch, and the combined domain covers every real x."],
 "Domain (−∞,∞); the graph is a function. At x=0, f(0)=0; at x=3, f(3)=2.",
 "Open and closed dots distinguish which branch includes each boundary.")
]
assert len(examples)==13

tries=[
("Finite domain set","A mapping includes (−6,2),(0,5),(4,2),(9,8). Write its domain.","{−6,0,4,9}"),
("Polynomial domain","Find the real domain of F(x)=x⁵−2x²+4.","(−∞,∞)"),
("Rational restriction","Find the domain of f(x)=(3x+1)/(5x−4).","(−∞,4/5)∪(4/5,∞)"),
("Root restriction","Find the domain of g(x)=√(3x+12).","[−4,∞)"),
("Real interval descriptions","Write −3<x≤1 or x≥5 in interval notation.","(−3,1]∪[5,∞)"),
("Domain and range of a sampled function","At discrete years 2019,2020,2021,2022 the observed metric is 20,45,30,50. Give the observed input and output sets.","Domain {2019,2020,2021,2022}; range {20,30,45,50}."),
("Radical range","For h(x)=−√(8−x), find real domain and range.","Domain (−∞,8]; range (−∞,0]."),
("Piecewise with missing endpoints","Graph f(x)=x² if x<−1, f(x)=1 if −1<x<2 and f(x)=√(x−2) if x>2. State the domain and open endpoint behavior.","Domain (−∞,−1)∪(−1,2)∪(2,∞); neither −1 nor 2 included.")
]
assert len(tries)==8

row=lambda label,cells:{"Label":label,"Cells":list(cells)}
visuals=[
 {"Kind":"RatioTable","Title":"Finite set domain and range","Description":"Repeated outputs do not delete input values from the domain.","PrimaryLabel":"Input","SecondaryLabel":"Output","Rows":[row("Input",[1,3,5,8]),row("Output",[11,11,15,20])],"Items":[],"SecondaryItems":[]},
 {"Kind":"RatioTable","Title":"Domain exclusions with algebraic constraints","Description":"Compare all-real polynomial inputs, a forbidden denominator and an included root boundary.","PrimaryLabel":"Restriction","SecondaryLabel":"Allowed real x","Rows":[row("Function",["x³−4x","4/(x−2)","3√(x+1)"]),row("Domain",["(−∞,∞)","(−∞,2)∪(2,∞)","[−1,∞)"] )],"Items":[],"SecondaryItems":[]},
 {"Kind":"RatioTable","Title":"Piecewise branch endpoint membership","Description":"Test branch selection before evaluation. Endpoints may be open or closed.","PrimaryLabel":"Condition","SecondaryLabel":"Value","Rows":[row("x region",["x≤0","0<x≤3","x>3"]),row("f(x)",["x²","2","x−1"])],"Items":[],"SecondaryItems":[]},
 {"Kind":"RatioTable","Title":"Discrete observed annual measurements","Description":"These represent only observed years, not the continuous production function.","PrimaryLabel":"Year","SecondaryLabel":"Observed tons/day","Rows":[row("Year",[2020,2021,2022,2023]),row("Measurement",[120,170,140,200])],"Items":[],"SecondaryItems":[]}
]
concepts=[
 ("Domain and range","The domain contains permitted input values and the range contains attained outputs.","Read allowed x-values first; then identify all outputs actually attained.","For (2,9),(3,9),(4,10), domain is {2,3,4} and range {9,10}."),
 ("Interval notation","Parentheses omit finite endpoints and brackets include them.","Infinity is never included; unions combine disjoint allowed sets.","x<1 or x≥4 corresponds to (−∞,1)∪[4,∞)."),
 ("Algebraic domain restriction","Polynomials accept all real inputs; rational denominators cannot vanish.","Even-root radicands must be nonnegative; root denominators must be strictly positive.","√(x−5) needs x≥5, while 1/√(x−5) needs x>5."),
 ("Graph-derived domain and range","Horizontal coverage yields domain, and vertical coverage yields range.","Distinguish discrete points, continuous curves, open/closed endpoints and plotted windows.","y=|x−1|−2 has all-real domain and range [−2,∞)."),
 ("Toolkit shapes","Quadratic ranges may be bounded below; cubic ranges cover ℝ; reciprocals omit a value.","Analyze each exact rule and branch rather than assume a picture's drawn window is full domain.","4/(x−2) omits x=2 and output 0."),
 ("Piecewise functions","A piecewise function uses more than one formula on disjoint input conditions.","At each boundary apply only the permitted condition; annotate included and excluded endpoint points.","f(x)=x² for x≤0 and f(x)=1 for x>0 gives f(0)=0."),
 ("Applied domain and units","Real context can constrain otherwise unrestricted mathematical formulas.","Nonnegative quantities, integer counts, dates and physical units limit contextual domains.","h(t)=−16t²+96t describes nonnegative height during t∈[0,6].")
]
lesson={
"CommonMistakes":[
 {"Mistake":"Using a bracket at a value that makes a denominator zero.","WhyWrong":"Division by zero is undefined.","Correction":"Solve the denominator-zero equation and exclude roots with open interval boundaries."},
 {"Mistake":"Treating an odd root as if it were an even root.","WhyWrong":"Odd roots accept negative real radicands.","Correction":"Only even radicals demand a nonnegative radicand."},
 {"Mistake":"Reporting domain alone when asked for range.","WhyWrong":"Range refers to attained output values, not permitted inputs.","Correction":"Study y-values using the function rule, not only its input restrictions."},
 {"Mistake":"Connecting discrete annual observations with unproved continuous values.","WhyWrong":"Recorded points do not certify intermediate or extrapolated outputs.","Correction":"Identify the observation domain separately from a supplied continuous model."},
 {"Mistake":"Applying two piecewise branches to the same boundary input.","WhyWrong":"The inequalities allocate inputs between branches.","Correction":"Choose the branch whose boundary condition actually contains the input."},
 {"Mistake":"Using a closed dot at an omitted piecewise endpoint.","WhyWrong":"A strict inequality excludes that endpoint.","Correction":"Open circles mean excluded; filled circles mean included."}
],
"CultureCode":"en",
"ExplanationParagraphs":[
 "Learning goals: determine domains for finite mappings and functions defined by formulas, convert unions and exclusions to interval and set-builder notation, read domain and range from graphs, and evaluate or graph piecewise functions.",
 "Find the domain before computing outputs. For a polynomial every real input is permitted; for a rational function solve its denominator equal to zero and exclude those inputs; for an even root enforce a nonnegative radicand.",
 "Parentheses exclude finite endpoints and brackets include them. Use ∪ to combine intervals and use a parenthesis with each infinite endpoint.",
 "An isolated graph point contributes a specific input and output. A continuous curve contributes an interval of inputs and attained outputs. A source figure may be an observed data window, not proof about values outside it.",
 "To determine range of a quadratic, identify its turning point and direction; to determine range of an odd-degree cubic with opposite end limits, recognize all real outputs; to determine range of a reciprocal, exclude any unattainable output.",
 "A piecewise rule partitions inputs by strict or inclusive inequalities. Label open/closed circles and evaluate exactly one branch at a permitted input.",
 "Context restricts mathematical domains: positive integer group sizes, a finite series of calendar years, real measurement units, or nonnegative flight height can exclude values that the algebra alone allows.",
 "Teacher-led source-skill activity: compare domain and range on independent coordinate diagrams for reciprocal, radical, and multi-branch functions. Verify units, endpoint membership and graph axes manually.",
 "Independent Try It practice contains eight independently authored tasks covering original Section 1.2 learner actions. Teachers assess graph constructions and open-ended reasoning.",
 ] + [f"Independent Try It {i} — {title}. {q} Expected result for teacher review: {answer}" for i,(title,q,answer) in enumerate(tries,1)],
"KeyConcepts":[{"Title":title,"Definition":definition,"Rule":rule,"Example":ex} for title,definition,rule,ex in concepts],
"LessonCode":code,
"SummaryPoints":[
 "A domain is the complete set of permitted inputs, whereas the range consists of attained outputs.",
 "Exclude zeros of rational denominators; include an even-root boundary when the radicand is zero unless it also lies in a denominator.",
 "Use parentheses for omitted endpoints, brackets for included endpoints, and unions for separated intervals.",
 "A graph's input and output extents can be discrete, bounded or unbounded.",
 "For piecewise rules select one branch by its inequality and mark open or filled endpoints.",
 "Use physical contexts and units to narrow an algebraically permitted domain.",
 "Independent open-ended graph reasoning is teacher-assessed, not automatically graded."
],
"Title":"Domain and Range",
"Videos":[],
"Visuals":visuals,
"WorkedExamples":examples
}
pack={"ContentVersion":"g12-u01-l02-firstparty-a-b-c-initial-content-20261106",
      "Lessons":[lesson],"PackCode":"US-CCSS-MATH:G12:PRECALC:OPENSTAX-2017:U01:L02",
      "SchemaVersion":2}
rich.write_text(json.dumps(pack,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
evidence={
"SchemaVersion":1,"Unit":1,"Lesson":2,"Ordinal":2,
"LocalOriginalSource":"OpenStax Precalculus 2017 PDF Section 1.2, source page locators 38–53",
"OriginalSourceSha256":source_hash,
"OriginalSourcePdfPageRange":[38,53],
"OriginalNamedWorkedExamples":13,
"OriginalNamedTryIts":8,
"OriginalNumberedSectionExercises":61,
"SourceOriginalFiguresReused":0,
"StudentWorkedExampleAlternatives":13,
"StudentIndependentTryItAlternatives":8,
"StudentFirstPartyStructuredVisualTables":4,
"FirstPartyRichV2Path":rich.relative_to(root).as_posix(),
"OriginalGraphOrThirdPartyImagesCopied":False,
"SourceFigureAndGraphSkillMatching":"PENDING_INDIVIDUAL_FIGURE_VISUAL_REVIEW",
"SourceExercise1To61StudentRoutes":"NOT_YET_AUTHORED",
"SourceAtomicDenominatorVerified":False,
"StageA":"FIRST_PASS_13_EXAMPLE_8_TRYIT_AND_61_EXERCISE_GROUP_LOCATORS",
"StageB":"13_INDEPENDENT_EXAMPLES_8_INDEPENDENT_TRYITS_7_KEY_CONCEPTS_4_RATIOTABLES_AUTHORED",
"StageC":"PENDING_61_SOURCE_EXERCISE_BANK_AND_ASSESSMENT_ROUTING",
"ReadyForStageD":False,
"StageD":"NOT_RUN",
"Tests":"NOT_RUN",
"UpdatedUtc":datetime.now(timezone.utc).isoformat()
}
(base/"u01-l02-abc-checkpoint.json").write_text(json.dumps(evidence,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
progress=base/"full-grade-progress.json"
p=json.loads(progress.read_text(encoding="utf-8"))
assert p["LastClosedLessonOrdinal"]==1 and p["FirstIncompleteLessonOrdinal"]==2
row=p["LessonEntries"][1]
row["ACStatus"]="A_C_IN_PROGRESS"
row["BContentStatus"]=evidence["StageB"]
row["SourceWorkedExamplesLocated"]=13
row["SourceTryItLocated"]=8
row["SourceNumberedExercisesLocated"]=61
row["RichV2Path"]=evidence["FirstPartyRichV2Path"]
row["EvidencePath"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u01-l02-abc-checkpoint.json"
row["Tests"]="NOT_RUN"
row["ReadyForStageD"]=False
row["BlockingReasons"]=[
 "Original PDF section figures and graph-dependent source exercises still require individual visualization review",
 "All 61 original numbered source assessment tasks need first-party equivalent deliverables and grading references",
 "Source element/action denominator and original Figure 1 third-party source credit must be reconciled",
 "Teacher-scored piecewise sketch endpoints and exact range outcomes are not completely mapped"
]
row["UpdatedUtc"]=evidence["UpdatedUtc"]
p["UpdatedUtc"]=evidence["UpdatedUtc"]
progress.write_text(json.dumps(p,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("G12_U01_L02_AUTHORED_13_FIRSTPARTY_EXAMPLES_8_TRYIT_7_CONCEPTS_4_TABLE_VISUALS; 61_EXERCISES_SOURCE_LOCATED_NOT_CLOSED")
