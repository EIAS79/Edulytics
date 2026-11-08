from pathlib import Path
import json
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
source=json.loads(Path(r"C:\Users\khali\AppData\Local\Temp\edulytics-g12-precalculus-pdf-cache\unit01\lesson007.json").read_text(encoding="utf-8"))
assert source["Section"]=="1.7"
tasks={}
def add(n,skill,prompt,answer,actions=None):
 assert n not in tasks,n
 tasks[n]={"Number":n,"SourcePdfPage":126 if n<=24 else 127 if n<=41 else 128,
 "PedagogicSourceSkill":skill,"IndependentStudentSourceSkillQuestion":prompt,
 "TeacherExpectedMathematics":answer,
 "OriginalExerciseNamedCognitiveActions":actions or ["show all required reasoning and exact result"],
 "OriginalPublisherVerbatimSourceQuestionCopied":False,
 "OriginalPublisherFigurePixelReused":False,
 "TeacherManualScoring":"AUTHORING_COMPLETE_RUNTIME_NOT_TESTED"}
verbal=[
("Explain the horizontal-line test for determining whether f has a functional inverse.","If every horizontal line meets graph at most once, each output identifies at most one input; f is one-to-one."),
("Explain why unrestricted f(x)=x² cannot be inverted as a function and give a valid domain restriction.","Inputs x and −x share output; restrict to x≥0 to get inverse √x, or x≤0 to get −√x."),
("Can a one-to-one function be its own inverse? Give an example and prove.","Yes; f(x)=5−x yields f(f(x))=x."),
("Does one-to-one require continuous monotonicity? Explain with domain caveat.","Not always; injectivity only requires distinct allowed inputs to have distinct outputs, including piecewise/disconnected domains."),
("Describe the algebraic algorithm for constructing an inverse and checking allowed domains.","Set y=f(x), solve uniquely for x, swap variables, restrict inverse input to original range, verify compositions.")
]
for i,(prompt,key)in enumerate(verbal,1):add(i,"VERBAL",prompt,key)
add(6,"SELF_INVERSE_PROOF","For arbitrary real a, prove f(x)=a−x is its own inverse.","f(f(x))=a−(a−x)=x for every real x.")
inverse={
7:("x+3","x−3","ℝ"),
8:("x+5","x−5","ℝ"),
9:("2−x","2−x","ℝ"),
10:("3−x","3−x","ℝ"),
11:("x/(x+2)","2x/(1−x)","f domain x≠−2; inverse domain x≠1"),
12:("(2x+3)/(5x+4)","(3−4x)/(5x−2)","f domain x≠−4/5; inverse domain x≠2/5")
}
for n,(f,finv,domain)in inverse.items():
 add(n,"ALGEBRAIC_INVERSE",
     f"Find and simplify the inverse of the one-to-one real function f(x)={f}; state original and inverse domains.",
     f"f⁻¹(x)={finv}; {domain}.",["solve y=f(x) for x","swap variables","verify input restrictions"])
for n,f,domain,finv,range_inv in [
(13,"(x+7)²","[−7,∞)","√x−7","inverse domain [0,∞)"),
(14,"(x−6)²","[6,∞)","√x+6","inverse domain [0,∞)"),
(15,"x²−5","[0,∞)","√(x+5)","inverse domain [−5,∞)")
]:
 add(n,"RESTRICT_SQUARE_AND_INVERT",
     f"Restrict f(x)={f} to a nondecreasing one-to-one domain, then derive the exact inverse rule and its domain.",
     f"Original domain {domain}; f⁻¹(x)={finv}; {range_inv}.",
     ["select valid increasing branch","compute inverse","state domains"])
add(16,"VERIFY_RATIONAL_INVERSE_PAIR",
    "Given f(x)=x/(x+2) and g(x)=2x/(1−x), compute both compositions and explain whether they are mutual inverses.",
    "Both compositions equal x with proper excluded denominator inputs; yes, inverse pair.",
    ["f(g(x)) with domain","g(f(x)) with domain","conclude inverses"])
add(17,"VERIFY_CUBE_ROOT_INVERSE",
    "Prove that f(x)=∛(x−1) and g(x)=x³+1 are inverse functions on the real line.",
    "f(g(x))=∛(x³)=x and g(f(x))=(∛(x−1))³+1=x.")
add(18,"VERIFY_AFFINE_INVERSE",
    "For f(x)=−3x+5 and g(x)=(5−x)/3, verify both compositions.",
    "f(g(x))=x, g(f(x))=x for all real x.")
graphs={
19:("f(x)=√x, x≥0","one-to-one: strictly increasing on [0,∞)"),
20:("f(x)=∛(3x+1)","one-to-one: strictly increasing on ℝ"),
21:("f(x)=−5x+1","one-to-one: strictly decreasing on ℝ"),
22:("f(x)=x³−27","one-to-one: strictly increasing on ℝ"),
23:("An independent strictly increasing piecewise-linear curve through (−2,−3),(0,1),(3,4)","one-to-one: each horizontal line intersects at most once"),
24:("An independent U-shaped quadratic curve y=(x−1)² on all real x","not one-to-one: e.g. x=0 and x=2 both give y=1")
}
for n,(rule,answer)in graphs.items():
 add(n,"HORIZONTAL_LINE_TEST_GRAPH",
     f"Use an independently drawn coordinate graph of {rule}. Decide if it is one-to-one and justify from horizontal-line intersections.",
     answer,["draw or inspect labelled independent model","apply horizontal-line test"])
for n,prompt,answer in [
(25,"An independent one-to-one plotted model has f(x)=x+1 for x∈[−3,5]. Find f(0).","f(0)=1."),
(26,"For the same plotted model f(x)=x+1 on [−3,5], solve f(x)=0.","x=−1."),
(27,"For the same plotted model f(x)=x+1 on [−3,5], find f⁻¹(0).","−1."),
(28,"For the same plotted model f(x)=x+1 on [−3,5], solve f⁻¹(x)=0.","x=1.")
]:
 add(n,"READ_ORIGINAL_AND_INVERSE_GRAPH",prompt,answer)
for n,prompt,answer,actions in [
(29,"On an independent full finite graph f(x)=2x−4 over [0,6], sketch the inverse across y=x.","f⁻¹(x)=(x+4)/2 for x∈[−4,8].",["reflect domain and range","draw inverse labelled"]),
(30,"On the same complete graph f(x)=2x−4 for x∈[0,6], find f(6) and f⁻¹(2).","f(6)=8; f⁻¹(2)=3.",["read original","read inverse"]),
(31,"For the complete independently drawn graph f(x)=2x−4 on [0,6], give the exact original domain.","[0,6].",["read included x window"]),
(32,"For the complete independently drawn graph f(x)=2x−4 on [0,6], give the exact original range.","[−4,8].",["read attained outputs"])
]:
 add(n,"GRAPH_INVERSIONS_AND_DOMAIN_RANGE",prompt,answer,actions)
for n,prompt,answer in [
(33,"If f(6)=7 and f is one-to-one, evaluate f⁻¹(7).","6"),
(34,"If f(3)=2 for one-to-one f, find f⁻¹(2).","3"),
(35,"If f⁻¹(−4)=−8, deduce f(−8).","−4"),
(36,"If f⁻¹(−2)=−1, deduce f(−1).","−2")
]:add(n,"INVERSE_PAIR_LOOKUP",prompt,answer)
fvalues=[8,0,7,4,2,6,5,3,9,1]
for n,question,expected in [
(37,"f(1)","0"),(38,"solve f(x)=3","x=7"),
(39,"f⁻¹(0)","1"),(40,"solve f⁻¹(x)=7","x=3")]:
 add(n,"INVERT_FINITE_TABLE",
     f"Using the finite one-to-one function table x=0..9 with f(x)={fvalues}, {question}. Show which entry is used.",
     expected)
add(41,"CREATE_INVERSE_TABLE",
    "A finite one-to-one mapping has pairs (3,1),(6,4),(9,7),(13,12),(14,16). Construct a full table of its inverse.",
    "Inverse pairs (1,3),(4,6),(7,9),(12,13),(16,14).")
add(42,"TECHNOLOGY_INVERSE_RECIPROCAL",
    "Find f⁻¹ for f(x)=3/(x−2), then graph both on correctly labelled axes. State all asymptote and domain exclusions.",
    "f⁻¹(x)=3/x+2, x≠0; original f x≠2. Graphs reflect across y=x.",
    ["inverse formula","plot original and inverse","state domains"])
add(43,"TECHNOLOGY_INVERSE_CUBIC",
    "Find inverse of f(x)=x³−1 and sketch both original and inverse.",
    "f⁻¹(x)=∛(x+1), domain all real.")
add(44,"TECHNOLOGY_INVERSE_RECIPROCAL_TRANSLATE",
    "Find inverse of f(x)=1/x−1 with its exact real domain and range; draw a source-skill graph of both functions.",
    "f⁻¹(x)=1/(x+1); inverse domain x≠−1, range y≠0; original domain x≠0, range y≠−1.",
    ["inverse algebra","domain and range","independent graphs"])
add(45,"TEMPERATURE_UNIT_INVERSE",
    "A Celsius-to-Fahrenheit conversion is F(C)=(9/5)C+32. Derive its inverse, and explain the input/output units.",
    "C(F)=(5/9)(F−32). Input °F, output °C.")
add(46,"CIRCUMFERENCE_RADIUS_INVERSE",
    "For a circle with nonnegative radius r, C(r)=2πr. Derive inverse r(C) and evaluate r(36π), explaining units.",
    "r(C)=C/(2π) for C≥0; r(36π)=18 length units.",
    ["derive physical inverse","evaluate specific circumference","state domain units"])
add(47,"CONSTANT_SPEED_TIME_INVERSE",
    "A driver moves at constant 50 miles/hour, so d(t)=50t miles for t≥0. Derive t(d) and interpret t(180).",
    "t(d)=d/50 hours for d≥0 miles; t(180)=3.6 hours.",
    ["derive inverse","evaluate travel time","explain dimensions"])
assert sorted(tasks)==list(range(1,48))
native=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u01-l07-firstparty-teacher-led.json"
native.parent.mkdir(parents=True,exist_ok=True)
assert not native.exists()
native.write_text(json.dumps({
"SchemaVersion":1,"LessonCode":"PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U01:L07",
"OriginalBookSection":"OpenStax Precalculus 2017 local Section 1.7",
"OriginalNumberedSourceExerciseSkillAnchors":47,
"FirstPartyQuestionCount":47,
"StudentQuestions":[{"Number":n,"Skill":v["PedagogicSourceSkill"],"Prompt":v["IndependentStudentSourceSkillQuestion"],"Scoring":"TEACHER_REVIEW_REQUIRED"}for n,v in sorted(tasks.items())],
"TeacherOnlyExpectedMathematics":[{"Number":n,"Answer":v["TeacherExpectedMathematics"],"OriginalActions":v["OriginalExerciseNamedCognitiveActions"]}for n,v in sorted(tasks.items())],
"OriginalPublisherFigureGraphicsReused":False,
"SourcePublisherExactVerbatimOrGraphicParityClaim":False,
"StudentTeacherRuntimeAndAutomatedGrading":"NOT_RUN","StageD":"NOT_RUN"
},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
(base/"u01-l07-47-original-numbered-skill-assessment-contracts.json").write_text(json.dumps({
"SchemaVersion":1,"SourceSHA256":source["SourceSha256"],
"OriginalSectionNumberedExercises":47,
"FirstPartyNumberedIndependentSourceSkills":47,
"PublisherOriginalFiguresCopied":False,
"OriginalSourceExactWordingOrGraphPixelParityClaim":False,
"StageD":"NOT_RUN","Items":[tasks[n]for n in sorted(tasks)]
},ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
cp=base/"u01-l07-abc-checkpoint.json"
c=json.loads(cp.read_text(encoding="utf-8"))
c["Source47NumberedExerciseSkills"]="47_FIRSTPARTY_TEACHER_MANUAL_QUESTION_ASSESSMENTS_AUTHORING_COMPLETE"
c["FirstPartyNumberedAssessmentQuestions"]=47
c["FirstPartyTeacherMathAnswerKeys"]=47
c["NativeAssessmentResourcePath"]=native.relative_to(root).as_posix()
c["SourceFigureSkillReview"]="PENDING"
c["ReadyForStageD"]=False
cp.write_text(json.dumps(c,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("U01_L07_FIRSTPARTY_47_SOURCE_SKILL_INVERSE_QUESTION_CONTRACTS_TEACHER_MATH_AUTHORED")
