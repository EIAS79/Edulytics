from pathlib import Path
import json
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
source=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit01\lesson004.json").read_text(encoding="utf-8"))
assert source["Section"]=="1.4" and len(source["Pages"])==13
code="PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U01:L04"
def ex(title,q,steps,answer,check):
 return {"Title":title,"Question":q,"Method":"Perform the inner function first for composition; preserve the domain of every original operation.","Steps":steps,"Answer":answer,"Check":check}
examples=[
ex("Different algebraic operations on functions","Let f(x)=x−2 and g(x)=x²−4. Find (g−f)(x) and (g/f)(x), including their original domains.",
["(g−f)(x)=x²−4−(x−2)=x²−x−2.","(g/f)(x)=(x²−4)/(x−2)=(x+2)(x−2)/(x−2).","Although the quotient simplifies, the original divisor vanishes at x=2."],
"(g−f)(x)=x²−x−2 for all real x; (g/f)(x)=x+2 for x≠2.","Removing a factor cannot restore an excluded input."),
ex("Composition order need not commute","For f(x)=3x+2 and g(x)=4−x, compare f(g(x)) and g(f(x)).",
["f(g(x))=3(4−x)+2=14−3x.","g(f(x))=4−(3x+2)=2−3x.","The outputs differ by 12 at every x."],
"f∘g=14−3x and g∘f=2−3x, not commutative.","Changing order changes meaning even when both domains are ℝ."),
ex("Interpret a composite contextual output","Let s(t) count steps walked by time t (minutes); let c(s) be estimated kilocalories expended after s steps. Interpret c(s(5)).",
["The inner function s(5) converts five minutes into step count.","That step count is a valid input to outer c.","The composite output is kilocalories expended from the steps walked during the first five minutes."],
"c(s(5)) describes estimated energy expended (kilocalories) from a five-minute walk.","The output and input units at the function boundary must agree."),
ex("Choose a meaningful function order","Let d(h) map travel hours to miles driven; let u(m) map miles driven to gallons used. Which expression predicts gallons after h hours?",
["Compute d(h) first (miles).","Feed the result as the miles input to u.","The reverse composition d(u(m)) treats gallons as hours and is physically mismatched."],
"u(d(h)) with units gallons; the reverse composition lacks the declared interface.","Physical units validate meaningful composition order."),
ex("Nested lookup from two tables","Let f(1)=5, f(2)=4, f(3)=1 and g(1)=3, g(2)=1, g(3)=2. Evaluate f(g(1)) and g(f(3)).",
["g(1)=3, so f(g(1))=f(3)=1.","f(3)=1, so g(f(3))=g(1)=3.","Every intermediate value must be in the input domain of the next table."],
"f(g(1))=1; g(f(3))=3.","Tabular functions need not commute."),
ex("Composition from graph coordinates","Independent graphs show g(1)=2 and f(2)=5. Find f(g(1)), explaining how two graph readings are combined.",
["Start on the g graph at x=1 to obtain output 2.","Use 2 as the x-input on the f graph and read output 5."],
"f(g(1))=5.","Read actual plotted coordinates, not the original input twice."),
ex("Nested formula numeric evaluation","Let f(t)=t²−2t and h(x)=2x+3. Evaluate f(h(2)) and h(f(2)).",
["h(2)=7 and f(7)=49−14=35.","f(2)=4−4=0 and h(0)=3."],
"f(h(2))=35 and h(f(2))=3.","The inner function must be evaluated before the outer function."),
ex("Domain of nested quotient functions","Let f(x)=6/(x−2) and g(x)=3/(x−1). Find domain of f(g(x)).",
["Inner denominator excludes x=1.","Outer denominator excludes input g(x)=2.","3/(x−1)=2 implies x=5/2; exclude this additional input."],
"Domain (−∞,1)∪(1,5/2)∪(5/2,∞).",
"Both restrictions are required even after algebraic simplification."),
ex("Decomposition into inner and outer maps","Write H(x)=√(7−x²) as a composition using independent functions.",
["Define inside a(x)=7−x².","Define outside b(u)=√u for u≥0.","Then H=b∘a and its real domain requires 7−x²≥0."],
"a(x)=7−x²; b(u)=√u, with H=b(a(x)), domain [−√7,√7].",
"Composition can be decomposed in more than one mathematically valid way.")
]
tries=[
("Operations and original quotient domain","With f(x)=x−2 and g(x)=x²−4, find (fg)(x), (f−g)(x), and their domains.","Product x³−2x²−4x+8; difference −x²+x+2. Both domains ℝ."),
("Unit-compatible composition","Let G(r) be gravitational force for orbital distance r and a(F) acceleration caused by force F. Form the physically meaningful composition.","a(G(r)); input distance yields force then acceleration."),
("Nested table lookups","Let f(1)=6,f(2)=5,f(3)=2,f(4)=7 and g(1)=3,g(2)=4,g(3)=1,g(4)=2. Find f(g(1)) and g(f(3)).","f(g(1))=f(3)=2; g(f(3))=g(2)=4."),
("Graph composition with labelled points","An independent plot gives f(2)=4 and g(4)=1. Find g(f(2)).","1, with inner plotted output 4."),
("Two numerical compositions","For f(x)=x²−2x and h(x)=2x+3, evaluate h(f(2)) and h(f(−2)).","h(f(2))=3; h(f(−2))=27."),
("Nested square root and reciprocal domain","Let f(x)=1/(x−3) and g(x)=√(x+2). Find domain of f(g(x)).","x≥−2 except x=7, since √(x+2)=3 iff x=7."),
("Construct inside and outside expressions","Decompose H(x)=4/(5−√(x²+2)) into an inner and outer function and state its domain.","a(x)=√(x²+2); b(u)=4/(5−u); exclude x=±√23.")
]
key=[
("Function arithmetic","Addition, subtraction, multiplication and division combine output values pointwise.","The quotient domain is the common domain of f,g with g(x)≠0.","(x²−4)/(x−2)=x+2 only for x≠2."),
("Composition","(f∘g)(x)=f(g(x)) evaluates g first then f.","Output of the inner function must belong to the outer function's domain.","f(x)=x²,g(x)=x+1 implies (f∘g)(x)=(x+1)²."),
("Composition order","In general f∘g≠g∘f.","Even when both exist, inner/outer meaning and units determine the valid order.","d(hours) gives miles and u(miles) gives gallons, so u(d(hours))."),
("Table and graph composition","Follow the first mapping to an intermediate value, then apply the second mapping.","A missing intermediate table input makes that composition undefined.","If g(1)=2 and f(2)=5, then f(g(1))=5."),
("Composite domain","The allowed x-values are in the inner domain and map to allowed inputs of the outer function.","D(f∘g)={x∈D(g):g(x)∈D(f)}.","If f(u)=1/(u−2) and g(x)=√x, exclude x<0 and x=4."),
("Decomposition","A complicated formula can be expressed as outer rule evaluated on inner rule.","Different valid choices for components can yield the same composite.","√(7−x²)=(u↦√u)∘(x↦7−x²).")
]
visuals=[
 {"Kind":"RatioTable","Title":"First-party composition lookup table","Description":"Use the output of g as a valid input of f; reverse order yields different outputs.","PrimaryLabel":"Input","SecondaryLabel":"Outputs","Rows":[{"Label":"x","Cells":["1","2","3"]},{"Label":"f(x)","Cells":["5","4","1"]},{"Label":"g(x)","Cells":["3","1","2"]}],"Items":[],"SecondaryItems":[]},
 {"Kind":"RatioTable","Title":"Domain restriction for nested operations","Description":"Different source-skill restrictions arise from inner and outer formula domains.","PrimaryLabel":"Step","SecondaryLabel":"Excluded input","Rows":[{"Label":"Operation","Cells":["g(x)=3/(x−1)","f(u)=6/(u−2)","f(g(x))"]},{"Label":"Restriction","Cells":["x≠1","g(x)≠2","x≠1 and x≠5/2"]}],"Items":[],"SecondaryItems":[]}
]
cool={"StudentPrompt":"(a) Given f(x)=1/(x−2) and g(x)=x+1, find f(g(x)) and its domain. (b) Give a pair of simple functions for which f∘g and g∘f differ. (c) Explain why a quotient's removable factor does not restore the excluded input.","TeacherExpectedMathematics":"(a) 1/(x−1), x≠1; (b) f(x)=x²,g(x)=x+1 gives (x+1)² and x²+1; (c) division by the cancelled original denominator zero is still undefined.","OriginalPublisherRestrictedCoolDownCopied":False,"TeacherReview":True}
lesson={
"CommonMistakes":[
 {"Mistake":"Applying the outer function before the inner function.","WhyWrong":"Composition f(g(x)) is nested evaluation and order matters.","Correction":"Evaluate g(x) first then feed its result to f."},
 {"Mistake":"Restoring a cancelled denominator zero to the quotient domain.","WhyWrong":"The original division remains undefined.","Correction":"Track exclusions from the unsimplified quotient."},
 {"Mistake":"Checking the domain of only the inner function.","WhyWrong":"The output may be forbidden by the outer function.","Correction":"Intersect the inner domain with g(x)∈D(f)."},
 {"Mistake":"Assuming the two composition orders always agree.","WhyWrong":"Composition is generally not commutative.","Correction":"Evaluate both orders and check their units and domains."}],
"CultureCode":"en",
"ExplanationParagraphs":[
 "Learning goals: combine function values by arithmetic, interpret composition as an ordered mapping, evaluate composite functions with tables, graphs and formulas, determine composite domains, and decompose a rule into inner and outer functions.",
 "The quotient f/g requires x in the domains of both factors and g(x)≠0. Removing a symbolic factor cannot restore a forbidden input.",
 "The composition (f∘g)(x) evaluates inner g then outer f, and generally differs from (g∘f)(x). The same procedure holds for chained tables and graphs: verify each intermediate input exists.",
 "Composite domains depend on restrictions of g and on whether its outputs lie in the allowable input domain of f. Always test both layers.",
 "Applied composition must have meaningful quantities and compatible units across the inner-output/outer-input interface.",
 "A composite can be decomposed into two functions; multiple decompositions may exist, but they must reproduce the original formula and domain.",
 "Edulytics independent cooldown: "+cool["StudentPrompt"],
 ] + [f"Independent Try It {i} — {title}. {prompt}" for i,(title,prompt,ans)in enumerate(tries,1)],
"KeyConcepts":[{"Title":t,"Definition":de,"Rule":ru,"Example":e}for t,de,ru,e in key],
"LessonCode":code,
"SummaryPoints":[
 "Arithmetic on functions is pointwise and division excludes denominator zeros.",
 "(f∘g)(x)=f(g(x)) applies the inner function first; changing order may change meaning.",
 "For table/graph composition each intermediate output must be a valid next input.",
 "A composite domain restricts both the inner rule and the outer rule's accepted inputs.",
 "Contextual unit compatibility helps identify meaningful composition order.",
 "First-party open-ended source skills are teacher-reviewed in Stage C."
 ],
"Title":"Composition of Functions","Videos":[],"Visuals":visuals,"WorkedExamples":examples}
file=root/"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u01-l04.rich-lesson-v2.json"
assert not file.exists()
file.write_text(json.dumps({"ContentVersion":"g12-u01-l04-firstparty-source-skill-a-b-c-20261106","Lessons":[lesson],
"PackCode":"US-CCSS-MATH:G12:PRECALC:OPENSTAX-2017:U01:L04","SchemaVersion":2},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
(base/"u01-l04-independent-tryit-teacher-key.json").write_text(json.dumps({"Unit":1,"Lesson":4,"SourceOriginalTryItCount":7,"TeacherOnlyFirstPartyAnswers":[{"OriginalTryItNumber":i,"IndependentStudentPrompt":prompt,"TeacherExpectedMathematics":ans}for i,(title,prompt,ans)in enumerate(tries,1)],"OriginalPublisherAssetsCopied":False,"StageD":"NOT_RUN"},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
(base/"u01-l04-firstparty-cooldown.json").write_text(json.dumps(cool,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
checkpoint={
 "SchemaVersion":1,"Course":"US-CCSS-MATH:G12",
 "Unit":1,"Lesson":4,"OriginalSection":"1.4",
 "OriginalSourceSha256":source["SourceSha256"],
 "OriginalSourcePdfPages":[67,79],
 "OriginalWorkedExampleSkillsLocated":9,
 "OriginalTryItsLocated":7,
 "OriginalNumberedSectionExercises":97,
 "FirstPartyWorkedExamplesAuthored":9,
 "FirstPartyTryItsAuthored":7,
 "FirstPartyLearningConceptsAuthored":6,
 "FirstPartyTableVisualsAuthored":2,
 "FirstPartyCoolDownAuthored":True,
 "FirstPartyRichV2Path":file.relative_to(root).as_posix(),
 "OriginalPublisherImagesCopied":0,
 "Source97NumberedExerciseSkills":"NOT_YET_RECONCILED",
 "A_B_C":"IN_PROGRESS",
 "StageD":"NOT_RUN","Tests":"NOT_RUN",
 "ReadyForStageD":False,
 "UpdatedUtc":datetime.now(timezone.utc).isoformat()}
(base/"u01-l04-abc-checkpoint.json").write_text(json.dumps(checkpoint,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
progress=base/"full-grade-progress.json"
p=json.loads(progress.read_text(encoding="utf-8"))
assert p["LastClosedLessonOrdinal"]==3 and p["FirstIncompleteLessonOrdinal"]==4
entry=p["LessonEntries"][3]
entry["ACStatus"]="A_C_IN_PROGRESS"
entry["FirstPartyRichV2Path"]=checkpoint["FirstPartyRichV2Path"]
entry["OriginalSourceWorkedExampleSkillsLocated"]=9
entry["OriginalSourceTryItsLocated"]=7
entry["OriginalSourceNumberedExercisesLocated"]=97
entry["EvidencePath"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u01-l04-abc-checkpoint.json"
entry["ReadyForStageD"]=False
entry["Tests"]="NOT_RUN"
entry["BlockingReasons"]=["Source 97 numbered exercise actions need individual skill map and teacher answers","Graph and table source figures require skill crosswalk","Native source task assessment resource and source subpart action coverage need authoring"]
p["UpdatedUtc"]=checkpoint["UpdatedUtc"]
progress.write_text(json.dumps(p,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("G12_U01_L04_9_WORKED_7_TRYIT_6_CONCEPTS_2_TABLES_FIRSTPARTY_CONTENT_AUTHORED_SOURCE_97_EXERCISES_PENDING")
