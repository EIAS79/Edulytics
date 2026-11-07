from pathlib import Path
import json
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
source=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit01\lesson007.json").read_text(encoding="utf-8"))
assert source["Section"]=="1.7" and len(source["Pages"])==13
code="PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U01:L07"
examples=[
("Reverse an input-output mapping","A one-to-one mapping has f(2)=7 and f(5)=12. Find the matching inverse values.","f⁻¹(7)=2; f⁻¹(12)=5.","The inverse swaps coordinates (2,7)↔(7,2)."),
("Prove an inverse by composing in both orders","Given f(x)=1/(x+3) and g(x)=1/x−3, determine if they are inverse functions including their proper domains.","f(g(x))=x for x≠0; g(f(x))=x for x≠−3. Yes, they are inverse functions.","Respect the distinct input restrictions."),
("Distinguish a multiplier from a fractional exponent","Is g(x)=x/3 the inverse of f(x)=x³?","No. f(g(x))=x³/27. Correct inverse is g(x)=∛x.","A factor 1/3 does not undo a cube operation."),
("Restrict domain before inverting a noninjective family","Explain how to make x², |x| and 1/x² invertible by an admissible branch restriction.","For f(x)=x² choose x≥0 and inverse √x. For |x| choose x≥0; for 1/x² choose x>0 yielding 1/√y for y>0.","The original full real domains are not one-to-one."),
("Interpret a tabulated inverse","A trip log gives f(20)=12 miles, f(40)=25 miles and f(60)=50 miles, with input minutes. Interpret f⁻¹(50).","f⁻¹(50)=60 minutes; the 50-mile reading occurs after 60 minutes.","Inverse inputs carry miles, inverse outputs minutes."),
("Read a graph and its inverse","An independently plotted one-to-one linear model is g(x)=x−1. Find g(3) and g⁻¹(3) from points.","g(3)=2, g⁻¹(3)=4; the input/output roles are reversed.","g(4)=3 confirms the inverse read."),
("Invert Fahrenheit and Celsius units","Given C=(5/9)(F−32), express Fahrenheit F in terms of Celsius C.","F=(9/5)C+32.","Recomposing with C(F) returns the starting temperature."),
("Inverse of a shifted rational rule","For f(x)=3/(x−2)+5, solve for f⁻¹(x) and state both input domains.","f⁻¹(x)=3/(x−5)+2; original x≠2; inverse x≠5.","Check both compositions on their allowed domains."),
("Inverse of a shifted square root","For f(x)=3+√(x−2), find inverse rule, domain and range.","f⁻¹(x)=(x−3)²+2 for x≥3; original domain [2,∞), range [3,∞).","The inverse quadratic is restricted to x≥3."),
("Plot the reflected inverse across y=x","For f(x)=x+2 and f⁻¹(x)=x−2, plot three points from each and explain the symmetry.","Source points (−1,1),(0,2),(2,4) reflect to inverse points (1,−1),(2,0),(4,2) across y=x.","Reflected inverse swaps domain and range.")
]
tries=[
("Swap a given inverse pair","If h⁻¹(7)=3, what exact original mapping must h contain?","h(3)=7."),
("Test inverse cubic translation","For f(x)=x³−4 and g(x)=∛(x+4), verify both compositions.","Yes; both compositions are identity on ℝ."),
("Cube translated inverse","For f(x)=(x−1)³, is g(x)=∛x+1 an inverse?","Yes; composing either direction yields x."),
("Swap domain and range","A one-to-one f has domain (1,∞) and range (−∞,−2). Give inverse domain and range.","Inverse domain (−∞,−2), inverse range (1,∞)."),
("Invert finite table","For a one-to-one table f(1)=5,f(2)=7,f(3)=9, evaluate f⁻¹(7).","2."),
("Graph inverse coordinate lookup","For a source-skill graph model g(x)=x+3, read g(2) and g⁻¹(2).","g(2)=5; g⁻¹(2)=−1."),
("Solve formula for input","Solve y=(x−5)/3 for x in terms of y.","x=3y+5."),
("Inverse of a decreasing root","For f(x)=2−√x, x≥0, derive its inverse and state its allowable domain.","f⁻¹(x)=(2−x)² for x≤2, range [0,∞)."),
("Graph a function and its rational inverse","Reflect points from f(x)=3/(x−2)+5 across y=x. Derive inverse formula.","f⁻¹(x)=3/(x−5)+2, x≠5.")
]
concepts=[
("Inverse function","If f(a)=b for a one-to-one function then f⁻¹(b)=a.","Swap input/output roles and original domain/range.","f(2)=7 ↔ f⁻¹(7)=2."),
("One-to-one and horizontal line test","Inverse of a function is a function only when each original output has at most one input.","A horizontal line may intersect the graph no more than once.","Unrestricted y=x² fails the horizontal line test."),
("Composition proof","For inverse pair f,g both f∘g and g∘f act as identity on their corresponding valid domains.","Do not forget exclusions where an inner rule is undefined.","f(x)=1/(x+3),g(x)=1/x−3."),
("Algebraic inversion","Write y=f(x), solve for x in terms of y, exchange variables and preserve restrictions.","Some formulas require selecting an injective branch.","f(x)=3+√(x−2) invertible with inverse (x−3)²+2 on x≥3."),
("Domain and range interchange","Domain of f⁻¹ equals range of f and range of f⁻¹ equals domain of f.","Swapping without checking endpoints yields incorrect domains.","√x on [0,∞) has inverse x² restricted x≥0."),
("Graph symmetry","An inverse graph is the reflection of f across the identity line y=x.","Each point (a,b) maps to (b,a).","(3,5) in f becomes (5,3) in f⁻¹."),
("Units in inverse models","An inverse changes dependent output back into the input quantity.","Units of original output become inverse input units.","Distance in miles ↔ time in minutes.")
]
visuals=[
 {"Kind":"RatioTable","Title":"First-party inverse finite mapping","Description":"The inverse relation swaps input and output coordinates.","PrimaryLabel":"input","SecondaryLabel":"output","Rows":[{"Label":"f input","Cells":["2","5","8"]},{"Label":"f output","Cells":["7","12","16"]},{"Label":"f⁻¹ input","Cells":["7","12","16"]},{"Label":"f⁻¹ output","Cells":["2","5","8"]}],"Items":[],"SecondaryItems":[]},
 {"Kind":"RatioTable","Title":"Domain/range exchange on root functions","Description":"A one-to-one square-root branch inverts to a restricted quadratic branch.","PrimaryLabel":"Function","SecondaryLabel":"Domain/range","Rows":[{"Label":"Formula","Cells":["f(x)=3+√(x−2)","f⁻¹(x)=(x−3)²+2"]},{"Label":"Domain","Cells":["[2,∞)","[3,∞)"]},{"Label":"Range","Cells":["[3,∞)","[2,∞)"]}],"Items":[],"SecondaryItems":[]}
]
cool={"StudentPrompt":"(a) Find inverse of f(x)=2x−6 and verify by composition. (b) Explain why f(x)=x² requires an injective domain restriction. (c) For g(x)=4+√(x−1), determine inverse formula and its domain.","TeacherExpectedMathematics":"(a) f⁻¹(x)=(x+6)/2, both compositions identity. (b) Restrict to x≥0 or x≤0 before inverting. (c) g⁻¹(x)=(x−4)²+1 on [4,∞).","OriginalPublisherCoolDownCopied":False}
lesson={
"LessonCode":code,"CultureCode":"en","Title":"Inverse Functions",
"ExplanationParagraphs":[
 "Learning goals: identify one-to-one functions, invert table and graph coordinates, test inverse pairs by composition, compute inverse algebraically, and correctly exchange domain/range including physical units.",
 "If f(a)=b for one-to-one f, the inverse satisfies f⁻¹(b)=a. The inverse must be a function, so f must pass the horizontal-line test on its chosen domain.",
 "Graph f and f⁻¹ are symmetric across y=x: every original point (a,b) corresponds to an inverse point (b,a). Inverse is not the same as reciprocal 1/f.",
 "Algebraic inversion solves y=f(x) for the input variable, swaps symbols, and preserves exclusions and necessary branch restrictions. Verify both compositions when possible.",
 "An unrestricted quadratic or absolute-value function is not one-to-one. Restrict to a monotonic branch before claiming a functional inverse.",
 "The inverse domain equals the original range, and the inverse range equals the original domain. Physical models swap output/input units.",
 "Independent Edulytics Cool-down: "+cool["StudentPrompt"]
 ]+[f"Independent Try It {i} — {t}. {q}"for i,(t,q,ans)in enumerate(tries,1)],
"WorkedExamples":[{"Title":t,"Question":q,"Method":"Swap input/output with one-to-one restrictions; verify by composition and inspect branch/domain.","Steps":[ans],"Answer":ans,"Check":check}for t,q,ans,check in examples],
"KeyConcepts":[{"Title":t,"Definition":de,"Rule":ru,"Example":e}for t,de,ru,e in concepts],
"SummaryPoints":[
 "One-to-one functions pass the horizontal-line test.",
 "An inverse reverses inputs and outputs; it is not the reciprocal function.",
 "Composition with an inverse returns the identity on the appropriate domain.",
 "An inverse graph is the reflection of the original graph in y=x.",
 "Restrictions and included endpoints swap when domains and ranges exchange.",
 "Teacher evaluates independent source-skill graph inversions manually in Stage C."
],
"Visuals":visuals,"Videos":[],
"CommonMistakes":[
 {"Mistake":"Writing f⁻¹(x)=1/f(x).","WhyWrong":"Inverse functions reverse inputs/outputs; reciprocals divide 1 by output.","Correction":"Solve y=f(x) for x and swap variables."},
 {"Mistake":"Inverting x² on ℝ without restricting input sign.","WhyWrong":"Two distinct inputs can produce one output.","Correction":"Choose x≥0 or x≤0 before finding a functional inverse."},
 {"Mistake":"Forgetting inverse domain restrictions after squaring a radical equation.","WhyWrong":"The new quadratic formula may allow outputs the original root never attained.","Correction":"Use original range as inverse domain."},
 {"Mistake":"Reading g⁻¹(3) by finding g(3).","WhyWrong":"The inverse question asks for an input whose original output is 3.","Correction":"Read the original graph at y=3 then return its x-coordinate."}]}
rp=root/"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u01-l07.rich-lesson-v2.json"
assert not rp.exists()
rp.write_text(json.dumps({"SchemaVersion":2,"PackCode":"US-CCSS-MATH:G12:PRECALC:OPENSTAX-2017:U01:L07","ContentVersion":"g12-u01-l07-firstparty-source-skill-20261107","Lessons":[lesson]},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
(base/"u01-l07-firstparty-nine-tryit-teacher-key-and-cooldown.json").write_text(json.dumps({
"OriginalSourceTryItSkills":9,"FirstPartyIndependentTryIts":9,
"TeacherOnlyExpectedMathematics":[{"Number":i,"ExpectedMath":ans}for i,(_,_,ans)in enumerate(tries,1)],
"IndependentCoolDown":cool,"SourcePublisherGraphicsCopied":False,"StageD":"NOT_RUN"},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
cp={
"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":1,"Lesson":7,
"SourceSection":"1.7","SourceSHA256":source["SourceSha256"],
"OriginalLocalPdfPageRange":[116,128],
"SourceOriginalWorkedExampleSkills":10,
"SourceOriginalTryItSkills":9,
"SourceOriginalNumberedExercises":47,
"FirstPartyWorkedExamplesAuthored":10,
"FirstPartyTryItAlternativesAuthored":9,
"FirstPartyKeyConcepts":7,"FirstPartyTableVisuals":2,
"IndependentCoolDownAuthored":True,
"FirstPartyRichV2Path":rp.relative_to(root).as_posix(),
"Source47NumberedExerciseSkills":"PENDING",
"SourceVisualFigures":"PENDING",
"SourceFullPublisherExactPixelSemanticParityCertified":False,
"ReadyForStageD":False,"StageD":"NOT_RUN","CI":"NOT_RUN",
"UpdatedUtc":datetime.now(timezone.utc).isoformat()}
(base/"u01-l07-abc-checkpoint.json").write_text(json.dumps(cp,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
progress=base/"full-grade-progress.json"
p=json.loads(progress.read_text(encoding="utf-8"))
assert p["LastClosedLessonOrdinal"]==6 and p["FirstIncompleteLessonOrdinal"]==7
e=p["LessonEntries"][6]
e["ACStatus"]="A_C_IN_PROGRESS"
e["FirstPartyRichV2Path"]=cp["FirstPartyRichV2Path"]
e["EvidencePath"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u01-l07-abc-checkpoint.json"
e["OriginalSourceNumberedExercisesLocated"]=47
e["ReadyForStageD"]=False
e["BlockingReasons"]=["Map 47 original numbered exercise skills to first-party student questions and teacher answer guidance","Original figures and chapter-level resources require review before unit closure"]
p["UpdatedUtc"]=cp["UpdatedUtc"]
progress.write_text(json.dumps(p,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("GRADE12_U01_L07_FIRSTPARTY_10_WORKED_9_TRYIT_7_CONCEPTS_2_VISUAL_TABLES_AND_COOLDOWN")
