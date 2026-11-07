from pathlib import Path
import json,re
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
source=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit01\lesson004.json").read_text(encoding="utf-8"))
assert source["Section"]=="1.4" and len(source["Pages"])==13
tasks={}
def put(n,skill,prompt,expected,parts=None,grade="TEACHER_MANUAL_MATH_AND_DOMAIN_REVIEW"):
 assert n not in tasks,n
 tasks[n]={
 "SourceNumber":n,"SourcePdfPage":76 if n<=21 else 77 if n<=49 else 78 if n<=79 else 79,
 "SourceLearningSkill":skill,
 "FirstPartyStudentTask":prompt,
 "TeacherExpectedMathematicsOrAssessmentMethod":expected,
 "SourceRequestedActions":parts or ["complete question with correct restrictions"],
 "ReviewMode":grade,
 "SourceExerciseExactPublisherProseCopied":False,
 "OriginalPublisherGraphicCopied":False,
 "StudentPortalBound":"NOT_VERIFIED",
 "StageDAutoGrade":"NOT_RUN"}
for n,prompt,answer in [
 (1,"Explain how to determine the real domain of a quotient of two functions with an example of a cancelled but excluded input.","Intersect the separate domains and exclude any input where the original divisor is zero."),
 (2,"Define composition (f∘g)(x) with an example specifying which operation happens first.","f(g(x)), inner g first then f; check intermediate result."),
 (3,"Can different composition orders ever yield the same result? Supply one pair which commutes and one which does not.","Yes; f(x)=x+1,g(x)=x+2 commute, while f(x)=x²,g(x)=x+1 generally do not."),
 (4,"State the domain rule for the composition f∘g, including a numerical counterexample for a missing intermediate value.","{x∈D(g):g(x)∈D(f)}.")
]:put(n,"VERBAL",prompt,answer)
operations={
5:("x²+2x","6−x²","x∈ℝ except divisor zeros x=±√6"),
6:("−3x²+x","5","all real inputs"),
7:("2x²+4x","1/(2x)","x≠0 for all four; divisor g never zero on its domain"),
8:("1/(x−4)","1/(6−x)","x≠4,6 for all four; denominator g nonzero for allowed x"),
9:("3x²","√(x−5)","x≥5 for +,−,×; x>5 for quotient f/g"),
}
for n,(f,g,domain)in operations.items():
 put(n,"POINTWISE_FOUR_OPERATIONS",f"For f(x)={f} and g(x)={g}, calculate and simplify f+g, f−g, f·g and f/g; state each original operation's exact real domain.",
     f"Apply all four pointwise operations, preserve unsimplified domain restrictions; {domain}.",
     ["addition with domain","subtraction with domain","multiplication with domain","quotient with original domain"])
put(10,"POINTWISE_QUOTIENT_DOMAIN","For f(x)=√x and g(x)=|x−3|, find g(x)/f(x) and its real domain.","|x−3|/√x with x>0, because the denominator is zero at x=0.")
put(11,"FIVE_NESTED_COMPOSITIONS",
    "For independent f(x)=2x²+1 and g(x)=3x−5, find f(g(2)), f(g(x)), g(f(x)), g(g(x)), and f(f(−2)).",
    "f(g(2))=3; f(g(x))=2(3x−5)²+1; g(f(x))=6x²−2; g(g(x))=9x−20; f(f(−2))=163.",
    ["f(g(2))","f(g(x))","g(f(x))","g(g(x))","f(f(−2))"])
pairs={
12:("x²+1","√(x+2)"),
13:("√x+2","x²+3"),
14:("|x|","5x+1"),
15:("∛x","(x+1)/x³"),
16:("1/(x−6)","7/x+6"),
17:("1/(x−4)","2/x+4")
}
for n,(f,g)in pairs.items():
 put(n,"TWO_FUNCTION_COMPOSITIONS",
     f"Compute f(g(x)) and g(f(x)) separately for f(x)={f}, g(x)={g}; simplify and state all original-composition domain restrictions.",
     "Teacher substitutes g into f and f into g, verifies the exact domain at both layers and checks all restricted denominator/root inputs.",
     ["f∘g and domain","g∘f and domain"])
for n,(f,g,h)in {
18:("x⁴+6","x−6","√x"),
19:("x²+1","1/x","x+3")
}.items():
 put(n,"THREE_FUNCTION_COMPOSITION",
     f"For f(x)={f}, g(x)={g}, h(x)={h}, compute f(g(h(x))) and state its exact real domain.",
     "Evaluate h then g then f; teacher verifies nested simplification and every intermediate domain.")
put(20,"COMPOSITION_TWO_ORDERS_AND_QUOTIENT",
    "For f(x)=1/x, g(x)=x−3: (a) find f∘g, (b) its domain, (c) find g∘f, (d) its domain, and (e) f/g with its domain.",
    "(a) 1/(x−3); (b) x≠3; (c) 1/x−3; (d) x≠0; (e) 1/[x(x−3)] with x≠0,3.",
    ["f∘g","domain f∘g","g∘f","domain g∘f","quotient/domain"])
put(21,"ROOT_AND_RECIPROCAL_COMPOSITION",
    "For f(x)=√(2−4x), g(x)=−3/x, find g(f(x)) and its exact real domain. Explain why the root must be strictly positive.",
    "−3/√(2−4x), x<1/2; radicand zero is forbidden to outer reciprocal.",
    ["composition formula","domain with strict endpoint"])
put(22,"NESTED_RATIONAL_EVALUATION",
    "For f(x)=(1−x)/x and g(x)=1/(1+x²), find g(f(x)) and evaluate at x=2.",
    "g(f(x))=x²/[x²+(1−x)²], x≠0; g(f(2))=4/5.",
    ["nested formula and domain","numeric evaluation at 2"])
for n,(inner,outer)in {
23:("p(x)=1/√x","m(x)=x²−4"),
24:("q(x)=1/√x","h(x)=x²−9"),
}.items():
 put(n,"THREE_COMPOSITE_DOMAINS",
     f"Given {inner} and {outer}, give exact domains of first function divided by second, first composed with second, and second composed with first.",
     "Determine strict positivity for square-root denominator, exclusions for the polynomial divisor, and nested-domain preimage correctly.",
     ["quotient real domain","first∘second domain","second∘first domain"])
put(25,"NESTED_RADICAL_DOMAIN","For f(x)=1/x and g(x)=√(x−1), find the exact domain of f(g(x)) in interval notation.",
    "(1,∞), because x≥1 is necessary for g, and √(x−1) cannot be zero.")
decomp={
26:"(x+2)²",27:"(x−5)³",28:"3/(x−5)",29:"4/(x+2)²",
30:"4+∛x",31:"∛(1/(2x−3))",32:"1/(3x²−4)^(-3)",
33:"fourth_root((3x−2)/(x+5))",34:"((8+x³)/(8−x³))⁴",
35:"√(2x+6)",36:"(5x−1)³",37:"∛(x−1)",
38:"|x²+7|",39:"1/(x−2)³",40:"(1/(2x−3))²",
41:"√((2x−1)/(3x+4))"
}
for n,h in decomp.items():
 put(n,"DECOMPOSE_OUTER_INNER",
     f"Find explicit inner g and outer f such that f(g(x))=H(x)={h}. Verify by substitution and preserve the correct domain.",
     "Any valid composition with consistent domains and a correct recomposition is acceptable; teacher checks original restricted input set.",
     ["identify inner map","identify outer map","substitute and verify domain"])
# For original figure-dependent exercises, author independent coordinate graph
# contracts; teacher must check the actual student's graph, no pixel reuse.
graph_tasks={
42:"f(g(3))",43:"f(g(1))",44:"g(f(1))",45:"g(f(0))",
46:"f(f(5))",47:"f(f(4))",48:"g(g(2))",49:"g(g(0))",
50:"g(f(1))",51:"g(f(2))",52:"f(g(4))",53:"f(g(1))",
54:"f(h(2))",55:"h(f(2))",56:"f(g(h(4)))",57:"f(g(f(−2)))"
}
for n,expr in graph_tasks.items():
 if n<=49:
  functions="Use independently drawn x/y plots of the finite functions f(x)=x+1 and g(x)=6−x for real x∈[−2,6], and only interpret nested inputs when they remain in both plotted domains."
 else:
  functions="Use independently drawn continuous graphs of f(x)=x+1, g(x)=x−2 and h(x)=4−x for real x∈[−2,6], evaluating only where each intermediate input is plotted."
 put(n,"COMPOSITION_FROM_INDEPENDENT_GRAPHS",
     f"{functions} Read {expr} from the coordinate graphs, showing the intermediate graph input/output values.",
     "Teacher verifies both/all successive x-y graph reads and undefined intermediate values; never assume the original publisher plotted data.",
     ["trace each graph in nested order","state output or undefined"])
table3_f=[7,6,5,8,4,0,2,1,9,3]
table3_g=[9,5,6,2,1,8,7,3,4,0]
table4_f={-3:11,-2:9,-1:7,0:5,1:3,2:1,3:-1}
table4_g={-3:-8,-2:-3,-1:0,0:1,1:0,2:-3,3:-8}
tableexpress={
58:"f(g(8))",59:"f(g(5))",60:"g(f(5))",61:"g(f(3))",
62:"f(f(4))",63:"f(f(1))",64:"g(g(2))",65:"g(g(6))",
66:"f(g(1))",67:"f(g(2))",68:"g(f(2))",69:"g(f(3))",
70:"g(g(1))",71:"f(f(3))"
}
for n,expr in tableexpress.items():
 if n<=65:
  f=table3_f;g=table3_g;allowed=set(range(10))
  def fn(x):return f[x]
  def gn(x):return g[x]
  data="x=0..9, f(x)="+str(f)+", g(x)="+str(g)
 else:
  def fn(x):return table4_f[x]
  def gn(x):return table4_g[x]
  allowed=set(table4_f)
  data="x=−3..3, f(x)="+str(list(table4_f.values()))+", g(x)="+str(list(table4_g.values()))
  # Avoid reusing source table verbatim; use first-party independent
  # affine transformation of both tables preserving composition skill.
  def fn(x):return x+1
  def gn(x):return 2-x
  data="x∈{−3,−2,−1,0,1,2,3}, f(x)=x+1, g(x)=2−x (independent first-party table)"
  allowed=set(range(-3,4))
 expr_function=re.match(r'([fg])\(([fg])\((-?\d+)\)\)',expr)
 assert expr_function,expr
 outer,inner,input_str=expr_function.groups()
 x=int(input_str)
 inner_y=fn(x)if inner=="f"else gn(x)
 answer=("undefined"if inner_y not in allowed else str(fn(inner_y)if outer=="f"else gn(inner_y)))
 if n<=65:
  # Keep source math values but recast as an independently tabulated
  # discrete lookup contract (functional values are source numerals).
  data="Independent table for source skill: "+data
 put(n,"FINITE_TABLE_NESTED_LOOKUP",
     f"Using {data}, evaluate {expr}. First show the inner table result; if the intermediate is not tabulated, state undefined.",
     f"Inner function at {x} gives {inner_y}; composite result is {answer}.",
     ["read inner tabular value","validate next input","read outer output"])
pairs={
72:("4x+8","7−x²"),
73:("5x+7","4−2x²"),
74:("√(x+4)","12−x³"),
75:("1/(x+2)","4x+3")
}
for n,(f,g)in pairs.items():
 put(n,"TWO_NUMERIC_NESTED_COMPOSITIONS",
     f"Using f(x)={f}, g(x)={g}, evaluate f(g(0)) and g(f(0)), stating when a composition is undefined.",
     "Teacher evaluates each inner result then applies outer expression; checks radical and division restrictions.",
     ["f(g(0))","g(f(0))"])
for n,prompt,answer,part in [
(76,"For f(x)=2x²+1 and g(x)=3x+5, compute f(g(2)).","g(2)=11; f(11)=243.",["nested numeric"]),
(77,"For f(x)=2x²+1 and g(x)=3x+5, simplify f(g(x)).","2(3x+5)²+1=18x²+60x+51.",["nested symbolic"]),
(78,"For f(x)=2x²+1 and g(x)=3x+5, compute g(f(−3)).","f(−3)=19, g(19)=62.",["nested numeric"]),
(79,"For f(x)=2x²+1 and g(x)=3x+5, simplify g(g(x)).","9x+20.",["nested symbolic"])
]:put(n,"SINGLE_NESTED_COMPOSITION",prompt,answer,part)
# Source 80-97: keep cognitive demand, adapt arithmetic and contextual setting
# rather than pretending original word-for-word or screenshot equivalence.
put(80,"COMPOSITION_COMPARE_SYMBOLIC","Given f(x)=x³+1,g(x)=∛(x−1), calculate f(g(x)) and g(f(x)) and compare both domains.",
    "f(g(x))=x; g(f(x))=x for all real x; both are inverses on ℝ.",
    ["f∘g","g∘f","compare domains"])
put(81,"COMPOSITION_COMPARE_NUMERIC","For the same f(x)=x³+1,g(x)=∛(x−1), calculate f(g(2)) and g(f(2)).",
    "Each equals 2.",["two nested values"])
put(82,"INVERSE_COMPOSITION_DOMAIN","Given f(x)=x³+1 and g(x)=∛(x−1), state the real domain of g(f(x)).","(−∞,∞).")
put(83,"INVERSE_COMPOSITION_DOMAIN","Given f(x)=x³+1 and g(x)=∛(x−1), state the real domain of f(g(x)).","(−∞,∞).")
put(84,"SELF_INVERSE_COUNTEREXAMPLE","For f(x)=1/x on x≠0, find f(f(x)). Is it true that every f satisfies f(f(x))=x? Prove or disprove.",
    "f(f(x))=x on x≠0. Not true for arbitrary f, e.g. f(x)=x+1 gives x+2.",
    ["compute on original domain","prove counterexample"])
put(85,"COMPOSITION_POWER_ORDER","For F(x)=(x+1)^5, f(x)=x^5 and g(x)=x+1, determine if (g∘f)(x)=F(x).","False, x^5+1≠(x+1)^5 generally.")
put(86,"COMPOSITION_POWER_ORDER","For F(x)=(x+1)^5, f(x)=x^5 and g(x)=x+1, determine if (f∘g)(x)=F(x).","True for every real x.")
for n,val in [(87,6),(89,11)]:
 put(n,"TWO_COMPOSITIONS_WITH_RESTRICTIONS",
     f"Given f(x)=x²+2 for x≥0 and g(x)=√(x−2) for x≥2, compute f(g({val})) and g(f({val})), checking each intermediate input.",
     f"f(g({val}))=({val}−2)+2={val}; g(f({val}))=√(({val}²+2)−2)={val}.",
     ["first direction","reverse direction"])
put(88,"TWO_SYMBOLIC_COMPOSITIONS_WITH_RESTRICTIONS",
    "For f(x)=x²+2 on x≥0 and g(x)=√(x−2) on x≥2, simplify g(f(a)) and f(g(a)) and give the domains.",
    "g(f(a))=a for a≥0; f(g(a))=a for a≥2. The former root yields |a| before imposing a≥0.",
    ["g∘f and domain","f∘g and domain"])
put(90,"APPLIED_UNIT_COMPOSITION_ORDER",
    "Let D(p) be items demanded at unit price p dollars and C(q) the dollars needed to produce q items. Which composition yields production cost if the price is $6? Explain.",
    "C(D(6)); inner price→units, outer units→dollars.")
put(91,"APPLIED_INVERSE_COMPOSITION_CHOICE",
    "Let S(d) be a health-safety monitoring score as a function of sensor input d, and m(t) the sensor reading after t minutes. Write the equation that finds when the score reaches 4.",
    "Solve S(m(t))=4 for t. Other expression orders have incompatible quantity meanings.")
put(92,"SUCCESSIVE_DISCOUNTS",
    "An item first receives a 30% discount and then a further 15% discount. Write the final price function of original price x with sequential composition.",
    "P(x)=0.85(0.70x)=0.595x; total reduction is 40.5%, not 45%.")
put(93,"CIRCLE_AREA_OF_COMPOSED_RADIUS",
    "A circular surface ripple's radius obeys r(t)=5√(t+2) inches for t≥0 minutes. Derive area A(t) and calculate A(2).",
    "A(t)=π(5√(t+2))²=25π(t+2) in²; A(2)=100π in².",
    ["area composition","time evaluation"])
put(94,"CIRCLE_AREA_OF_COMPOSED_LINEAR_RADIUS",
    "A growing circular field has radius r(t)=2t+1 metres. Find area A(t) for time t≥0.",
    "A(t)=π(2t+1)² m².")
put(95,"TIME_EVALUATION_OF_COMPOSED_AREA",
    "Using A(t)=π(2t+1)² for a circular field, calculate the area after five minutes.",
    "A(5)=121π m².")
put(96,"SPHERE_RADIUS_OF_VOLUME_AND_TIME",
    "A spherical balloon receives air so V(t)=10+20t cubic inches for t≥0. Its radius obeys r(V)=∛(3V/(4π)). Derive r(t) and find the exact time at which r=10 inches.",
    "r(t)=∛(3(10+20t)/(4π)); 10+20t=4000π/3 so t=200π/3−1/2 seconds.",
    ["compose volume to radius","solve target radius time"])
put(97,"MODEL_CHAIN_AND_EQUATION",
    "A model predicts N(T)=23T²−56T+1 units for temperature T in (3,33), with T(t)=5t+1.5 degrees after t hours. (a) Form N(T(t)); (b) solve N(T(t))=6752 for eligible t, rounding to 2 decimal places.",
    "N(t)=23(5t+1.5)²−56(5t+1.5)+1; solve the resulting quadratic, rejecting roots outside 3<5t+1.5<33.",
    ["compose formulas","solve and filter by contextual domain"])
assert set(tasks)==set(range(1,98)), sorted(set(range(1,98))-set(tasks))
# Do not expose teacher answer keys in the RichV2 student page.
resource=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u01-l04-firstparty-teacher-led.json"
resource.parent.mkdir(parents=True,exist_ok=True)
assert not resource.exists()
assessment={
 "SchemaVersion":1,"LessonCode":"PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U01:L04",
 "SourceBookSection":"Authorized local OpenStax Precalculus (2017), Section 1.4",
 "OriginalNamedNumberedExercises":97,
 "FirstPartyStudentSourceSkillQuestions":97,
 "SourceExactVerbatimParityClaim":False,"PublisherOriginalGraphicsEmbedded":False,
 "StudentQuestions":[{"Number":k,"Skill":v["SourceLearningSkill"],"Question":v["FirstPartyStudentTask"],
                      "ScoreMode":"TEACHER_REVIEW_REQUIRED"}for k,v in sorted(tasks.items())],
 "TeacherExpectedMathematics":[{"Number":k,"ExpectedAnswerOrReview":v["TeacherExpectedMathematicsOrAssessmentMethod"],
                                "OriginalSourceActions":v["SourceRequestedActions"]}for k,v in sorted(tasks.items())],
 "StudentPortalRuntime":"NOT_RUN",
 "TeacherAuthenticatedGradebookRuntime":"NOT_RUN",
 "CI":"NOT_RUN",
 "PreserveAttribution":"CC_BY_4_SOURCE_CREDIT_REQUIRED"
}
resource.write_text(json.dumps(assessment,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
manifest=base/"u01-l04-97-numbered-source-skill-question-contracts.json"
manifest.write_text(json.dumps({
"SchemaVersion":1,"SourceSha256":source["SourceSha256"],"SourceNumberedExercises":97,
"FirstPartySourceSkillAssessmentQuestions":97,
"TeacherManualReviewQuestions":97,"OriginalPublisherGraphicCopied":False,
"OriginalSourceExactVerbatimOrFullSourceAtomicMatchingClaim":False,
"StudentDeliveryRuntime":"NOT_VERIFIED","StageD":"NOT_RUN",
"Items":[tasks[k]for k in sorted(tasks)]},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
cp=base/"u01-l04-abc-checkpoint.json"
c=json.loads(cp.read_text(encoding="utf-8"))
c["Source97NumberedExerciseSkills"]="FIRSTPARTY_TEACHER_MANUAL_QUESTION_BANK_AUTHORED"
c["FirstPartyStudentQuestionCount"]=97
c["FirstPartyTeacherAnswerRubricCount"]=97
c["FirstPartyNativeAssessmentPath"]=resource.relative_to(root).as_posix()
c["FirstPartyAcademicQuestionManifestPath"]=manifest.relative_to(root).as_posix()
c["StageC"]="97_FIRSTPARTY_NAMED_SOURCE_SKILL_QUESTION_CONTRACTS_AUTHORING_COMPLETE; ORIGINAL_GRAPH_PIXEL_PARITY_AND_RUNTIME_NOT_CLAIMED"
c["ReadyForStageD"]=False
cp.write_text(json.dumps(c,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
progress=base/"full-grade-progress.json"
p=json.loads(progress.read_text(encoding="utf-8"))
assert p["LastClosedLessonOrdinal"]==3 and p["FirstIncompleteLessonOrdinal"]==4
entry=p["LessonEntries"][3]
entry["OriginalSourceNumberedExerciseSkillsAuthored"]=97
entry["FirstPartyNativeAssessmentPath"]=resource.relative_to(root).as_posix()
entry["StageCStatus"]=c["StageC"]
entry["ReadyForStageD"]=False
p["UpdatedUtc"]=datetime.now(timezone.utc).isoformat()
progress.write_text(json.dumps(p,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("U01_L04_97_FIRSTPARTY_INDIVIDUAL_SOURCE_SKILL_QUESTION_ITEMS_97_TEACHER_KEYS_AUTHORED_NOT_RUNTIME_TESTED")
