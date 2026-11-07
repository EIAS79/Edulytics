from __future__ import annotations
import json
from pathlib import Path
from datetime import datetime, timezone

root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
route=json.loads((base/"u01-l01-exercise-routing.json").read_text(encoding="utf-8"))
records=route["SourceExerciseContracts"]
assert len(records)==92 and [r["OriginalExerciseNumber"] for r in records]==list(range(1,93))
tasks=[]
def add(number,skill,prompt,mode="TEACHER_REVIEW_REQUIRED",kind="WRITTEN_REASONING"):
    assert number==len(tasks)+1, ("Bad source order",number,len(tasks))
    assert prompt and "answer:" not in prompt.lower()
    tasks.append({"Number":number,"SourceElementId":records[number-1]["SourceElementId"],"Skill":skill,"Prompt":prompt,"Mode":mode,"AnswerKind":kind,"SourcePdfPage":records[number-1]["SourcePdfPage"],"SourceParity":"CANDIDATE_NOT_VERIFIED"})
verbal=[
"Explain how a relation differs from a function. Illustrate each with ordered pairs.",
"Distinguish an independent input from its dependent output in a function. Give an example with meaningful units.",
"Explain why the vertical-line test detects a relation that has multiple outputs for the same input.",
"State and justify a method for deciding whether a function is one-to-one on its specified domain.",
"Explain why a horizontal line crossing a function graph more than once proves it is not one-to-one."]
for i,q in enumerate(verbal,1):add(i,"Definition and conceptual reasoning",q)
add(6,"Relations and functions","Decide whether {(a,b),(c,d),(a,c)} is a function of its first entry. Justify a repeated-input case.")
add(7,"Relations and functions","Decide whether {(a,b),(b,c),(c,c)} is a function of its first entry. Explain any repeated outputs.")
equations={
8:"5x + 2y = 10",9:"y = x²",10:"x = y²",11:"3x² + y = 14",
12:"2x + y² = 6",13:"y = −2x² + 40x",14:"y = 1/x",
15:"x = (3y + 5)/(7y − 1)",16:"x = √(1 − y²)",
17:"y = (3x + 5)/(7x − 1)",18:"x² + y² = 9",
19:"2xy = 1",20:"x = y³",21:"y = x³",
22:"y = √(1 − x²)",23:"x = ±√(1 − y)",
24:"y = ±√(1 − x)",25:"y² = x²",26:"y³ = x²"
}
for n,e in equations.items():add(n,"Implicit relations and domains",f"Does the equation {e} define y as a function of x over its real domain? Justify by isolating y when possible, and state excluded inputs.", "TEACHER_REVIEW_REQUIRED","SYMBOLIC_PROOF")
f_rules={27:"f(x)=2x−5",28:"f(x)=−5x²+2x−1",29:"f(x)=√(2−x)+5",30:"f(x)=(6x−1)/(5x+2)",31:"f(x)=|x−1|−|x+1|"}
for n,rule in f_rules.items():add(n,"Function notation",f"Given {rule}, calculate f(−3), f(2), f(−a), −f(a), and f(a+h). Show substitutions and state any domain restrictions.", "TEACHER_REVIEW_REQUIRED","MULTIPART_SYMBOLIC")
add(32,"Difference quotients","For g(x)=5−x², simplify [g(x+h)−g(x)]/h with h≠0. Show the cancellation steps.")
add(33,"Difference quotients","For g(x)=x²+2x, simplify [g(x)−g(a)]/(x−a) with x≠a. State why the restriction is needed.")
paired={
34:("k(t)=2t−1","k(2)","k(t)=7"),
35:("f(x)=8−3x","f(−2)","f(x)=−1"),
36:("p(c)=c²+c","p(−3)","p(c)=2"),
37:("f(x)=x²−3x","f(5)","f(x)=4"),
38:("f(x)=√(x+2)","f(7)","f(x)=4")
}
for n,(rule,val,equation) in paired.items():add(n,"Evaluate and solve",f"Given {rule}, evaluate {val} and solve {equation} for all real permitted inputs, checking every solution.")
add(39,"Isolating function output","From the relation 3r+2t=18, (a) write r=f(t), (b) calculate f(−3), and (c) solve f(t)=2.")
graph_intro={
40:"cubic-like curve that rises and falls while still presenting one output at each x",
41:"sideways parabola with upper and lower branches",
42:"reciprocal-type two-branch hyperbola with a vertical asymptote",
43:"connected, nonoverlapping piecewise horizontal–sloped–horizontal segments",
44:"closed oval or ellipse",
45:"smooth rising S-curve",
46:"two horizontal rays whose x-domains overlap: y=3 for x>0 and y=−3 for x<2",
47:"nonvertical straight line with a negative slope",
48:"two diagonal lines crossing in an X",
49:"nonvertical straight line with a positive slope",
50:"closed circle",
51:"sine-wave-shaped continuous curve"
}
for n,desc in graph_intro.items():
    add(n,"Vertical-line test on diagrams",
        f"Draw a labelled coordinate sketch of a {desc}. Decide whether the relation is y=f(x), and justify the vertical-line intersections. A teacher evaluates the sketch and proof.",
        "TEACHER_DIAGRAM_REQUIRED","DIAGRAM_AND_EXPLANATION")
graph_values={
52:"For the teacher-supplied increasing concave-down curve, read f(−1) and determine whether the displayed graph supports a unique input solving f(x)=3. Distinguish observed intersections from extrapolation and do not invent an exact root.",
53:"For the W-shaped curve with a central value 1 and two valleys at (−2,−3) and (2,−3), (a) read f(0), (b) identify all inputs with f(x)=−3. Sketch to scale.",
54:"For the inverted V with vertex (−3,4) and linear arms through (−6,1) and (0,1), (a) read f(4) from the extended arm, (b) solve f(x)=1 from both sides. Sketch to scale."
}
for n,q in graph_values.items():add(n,"Reading values from a graph",q,"TEACHER_DIAGRAM_REQUIRED","GRAPH_INTERPRETATION")
injective={
55:"closed oval with upper and lower points for some x",
56:"upward-opening V with vertex (1,1)",
57:"strictly increasing S-shaped curve",
58:"strictly decreasing continuous curve",
59:"curve that is flat, rises to a peak, then falls"
}
for n,shape in injective.items():add(n,"Function and one-to-one tests",
f"On a clearly labelled graph of a {shape}, apply the vertical-line test first. If it defines a function, determine whether it is one-to-one by the horizontal-line test, and justify.", "TEACHER_DIAGRAM_REQUIRED","GRAPH_CLASSIFICATION")
ordered={
60:"{(−1,−1),(−2,−2),(−3,−3)}",
61:"{(3,4),(4,5),(5,6)}",
62:"{(2,5),(7,11),(15,8),(7,9)}"
}
for n,r in ordered.items():add(n,"Relations with ordered pairs",f"Is the first-coordinate relation {r} a function? Explain how repeated inputs and outputs affect classification.","EXACT_FAMILY_CANDIDATE","YES_NO_WITH_REASON")
tables={
63:"x: 5, 10, 15; y: 3, 8, 14",
64:"x: 5, 10, 15; y: 3, 8, 8",
65:"x: 5, 10, 10; y: 3, 8, 14"}
for n,r in tables.items():add(n,"Function tables",f"Interpret the table ({r}). Is y a function of x? If so, is it one-to-one? Justify both answers.","EXACT_FAMILY_CANDIDATE","FUNCTION_CLASSIFICATION")
table14="x = [0,1,2,3,4,5,6,7,8,9]; f(x) = [74,28,1,53,56,3,36,45,14,47]"
add(66,"Table lookup",f"Using the finite table {table14}, evaluate f(3).","EXACT_FAMILY_CANDIDATE","INTEGER")
add(67,"Inverse lookup",f"Using the finite table {table14}, find all tabulated x values for which f(x)=1.","EXACT_FAMILY_CANDIDATE","INTEGER_SET")
eval_rules={68:"4−2x",69:"8−3x",70:"8x²−7x+3",71:"3+√(x+3)",72:"(x−2)/(x+3)",73:"3^x"}
for n,rule in eval_rules.items():add(n,"Function evaluation",f"For f(x)={rule}, evaluate at x=−2,−1,0,1,2. Present a two-row table; show exact fractions and radicals where applicable.","EXACT_FAMILY_CANDIDATE","MULTIPART_EXACT")
add(74,"Composite function-value arithmetic","Given f(x)=3x−2 and g(x)=5−x², evaluate 3f(1)−4g(−2) with substitution.","EXACT_FAMILY_CANDIDATE","INTEGER")
add(75,"Composite function-value arithmetic","Given f(x)=3x−2 and h(x)=−2x²+3x−1, evaluate f(7/3)−h(−2) with substitution.","EXACT_FAMILY_CANDIDATE","INTEGER")
windows={
76:("x²","[−0.1,0.1]"),77:("x²","[−10,10]"),78:("x²","[−100,100]"),
79:("x³","[−0.1,0.1]"),80:("x³","[−10,10]"),81:("x³","[−100,100]"),
82:("√x","[0,0.01]"),83:("√x","[0,100]"),84:("√x","[0,10000]"),
85:("∛x","[−0.001,0.001]"),86:("∛x","[−1000,1000]"),87:("∛x","[−1000000,1000000]")
}
for n,(f,win) in windows.items():
    add(n,"Graph in specified viewing window",f"Graph y={f} on the exact x-window {win}, with labelled axes. Determine the output range attained on that window. Preserve endpoints and scale.",
        "TEACHER_DIAGRAM_REQUIRED","GRAPH_AND_RANGE")
add(88,"Units in contextual notation","A city's population p is in thousands and its weekly garbage amount G=f(p) is in tons/week. (a) Express 40,000 people producing 13 tons/week as function notation. (b) Interpret f(5)=2 with the correct units.")
add(89,"Area and volume units","A garden area a in ft² maps to dirt volume D=g(a) in yd³. (a) Write the function observation for a 5,000 ft² garden requiring 50 yd³. (b) Interpret g(100)=1 including the units.")
add(90,"Time offsets in notation","If f(t) counts ducks t years after 1990, interpret f(5)=30 and f(10)=40 with actual calendar years.")
add(91,"Function interpretation","Let h(t) be a rocket's height in feet t seconds after launch. Interpret h(1)=200 and h(2)=350 with units.")
add(92,"Counterexample to one-to-one","Show that f(x)=3(x−5)²+7 is not one-to-one on all real inputs. Give two distinct inputs with equal output, and explain the horizontal-line consequence.")
assert len(tasks)==92 and [x["Number"] for x in tasks]==list(range(1,93)),len(tasks)
assert len({x["SourceElementId"] for x in tasks})==92

# First-party questions are mathematical equivalents / practice candidates, not full visual source parity.
bank={"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":1,"Lesson":1,
      "SourceSection":"1.1","OriginalNumberedExerciseCount":92,
      "TaskCount":92,"TaskDelivery":"FIRST_PARTY_STUDENT_READER_PROMPTS_WITH_TEACHER_SCORING_REQUIRED_WHERE_INDICATED",
      "Readiness":"AUTHORING_DRAFT_NOT_PRODUCTION_VERIFIED","SourceGraphImagesEmbedded":False,
      "SourceExerciseRights":"MATH_PROMPTS_ARE_NEW_FIRST_PARTY_DERIVATIVES; SOURCE_ATTRIBUTION_IN_PUBLIC_LICENSE_PAGE",
      "StudentSubmissionAndTeacherGrading":"NOT_BOUND_TO_G12_PERSISTENCE",
      "DigitalExactScoring":"EXACT_CANDIDATES_ONLY_NOT_92_AUTO_GRADED",
      "Tests":"WRITTEN_NOT_RUN","CI":"NOT_RUN","Items":tasks}
bankpath=base/"u01-l01-first-party-exercise-bank.json"
bankpath.write_text(json.dumps(bank,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")

def cs_escape(s):return json.dumps(s,ensure_ascii=False).replace('\\u2028','')
cs=[
"namespace Edulytics.Web.Presentation;",
"",
"/// <summary>",
"/// Instructor-reviewable first-party practice tasks addressing the 92 original",
"/// Section 1.1 exercise learning demands. This bank is not automatically graded,",
"/// not the original figures, and NOT a claim of full 1:1 source parity.",
"/// </summary>",
"public static class Grade12PrecalculusSection11Exercises",
"{",
"    public sealed record Task(int Number, string Skill, string Prompt,",
"        string ReviewMode, string AnswerKind);",
"    public static readonly IReadOnlyList<Task> All =",
"    ["
]
for item in tasks:
    cs.append(f'        new({item["Number"]}, {cs_escape(item["Skill"])}, {cs_escape(item["Prompt"])}, {cs_escape(item["Mode"])}, {cs_escape(item["AnswerKind"])}),')
cs += ["    ];","}",""]
newpath=root/"src/Edulytics.Web/Presentation/Grade12PrecalculusSection11Exercises.cs"
newpath.write_text("\n".join(cs),encoding="utf-8")

viewpath=root/"src/Edulytics.Web/Views/StudentPortal/Lesson.cshtml"
view=viewpath.read_text(encoding="utf-8")
needle='                @foreach (var graph in Grade12PrecalculusSection11Visuals.Build())'
assert view.count(needle)==1
# Put exercises below the graphs within the same lesson-only section, prior to RichContent.
anchor='''        @if (Model.RichContent is not null)
        {
            <partial name="_RichLessonContentV2" model="Model.RichContent" />'''
assert view.count(anchor)==1
snippet='''        @if (Model.LessonCode == "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U01:L01")
        {
            <section class="lesson-source-aligned-activities" aria-label="First-party Section 1.1 practice">
                <h2>Function notation and relations — practice bank</h2>
                <p>92 first-party practice tasks organized by mathematical skill.
                   For diagrams and written reasoning, show work to the assigned teacher;
                   these are not automatically marked and do not replace the original
                   textbook figures or constitute a completed teacher submission.</p>
                @foreach (var group in Grade12PrecalculusSection11Exercises.All.GroupBy(x => x.Skill))
                {
                    <details class="student-card">
                        <summary>@group.Key (@group.Count() tasks)</summary>
                        <ol>
                            @foreach (var task in group)
                            {
                                <li value="@task.Number">
                                    <strong>Task @task.Number:</strong> @task.Prompt
                                    @if (task.ReviewMode == "TEACHER_DIAGRAM_REQUIRED")
                                    {
                                        <p><em>Teacher-reviewed sketch and mathematical justification required.</em></p>
                                    }
                                </li>
                            }
                        </ol>
                    </details>
                }
            </section>
        }
'''
view=view.replace(anchor,snippet+anchor)
viewpath.write_text(view,encoding="utf-8")

testpath=root/"tests/Edulytics.Tests/MathematicsIntelligence/Grade12PrecalculusSection11SourceContractsTests.cs"
test=testpath.read_text(encoding="utf-8")
assert "Grade12FirstPartyExerciseBankMapsAllOriginalNumberedTasks" not in test
anchor='''    [Fact]
    public void IndependentWorkedExamplesHaveCorrectMathGoldenValues()'''
assert test.count(anchor)==1
testadd='''    [Fact]
    public void Grade12FirstPartyExerciseBankMapsAllOriginalNumberedTasks()
    {
        var tasks = Grade12PrecalculusSection11Exercises.All;
        Assert.Equal(92, tasks.Count);
        Assert.Equal(Enumerable.Range(1, 92), tasks.Select(x => x.Number));
        Assert.All(tasks, item =>
        {
            Assert.False(string.IsNullOrWhiteSpace(item.Skill));
            Assert.False(string.IsNullOrWhiteSpace(item.Prompt));
            Assert.NotEqual("AUTOMATICALLY_GRADED", item.ReviewMode);
        });
        Assert.Equal(20, tasks.Count(x => x.Number is >= 40 and <= 59 &&
            x.ReviewMode == "TEACHER_DIAGRAM_REQUIRED"));
        Assert.Equal(12, tasks.Count(x => x.Number is >= 76 and <= 87 &&
            x.ReviewMode == "TEACHER_DIAGRAM_REQUIRED"));
        Assert.Contains("tons/week", tasks[87].Prompt, StringComparison.Ordinal);
        // Teacher submission persistence, actual figure equivalence and
        // automated skill grading remain separate A–C gaps.
    }

'''
test=test.replace(anchor,testadd+anchor)
testpath.write_text(test,encoding="utf-8")

assessment_path=base/"u01-l01-assessment.json"
ass=json.loads(assessment_path.read_text(encoding="utf-8"))
ass["FirstPartySectionExerciseBank"]={
    "ArtifactPath":"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u01-l01-first-party-exercise-bank.json",
    "StudentViewCSharp":"src/Edulytics.Web/Presentation/Grade12PrecalculusSection11Exercises.cs",
    "StudentView":"src/Edulytics.Web/Views/StudentPortal/Lesson.cshtml",
    "OriginalExercisesAddressedByNumber":92,
    "UserFacingQuestionsAuthored":92,
    "SourceMatched":0,
    "SourceFigureMediaEmbedded":0,
    "TeacherPersistence":"NOT_YET_CONNECTED",
    "MachineGrading":"NOT_YET_92_EXERCISE_COVERAGE",
    "State":"AUTHORED_NOT_RUNTIME_VERIFIED",
    "Tests":"WRITTEN_NOT_RUN"
}
assessment_path.write_text(json.dumps(ass,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")

checkpoint_path=base/"u01-l01-abc-checkpoint.json"
cp=json.loads(checkpoint_path.read_text(encoding="utf-8"))
cp["FirstPartySectionExerciseBankPromptsAuthored"]=92
cp["FirstPartySectionExerciseBankPath"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u01-l01-first-party-exercise-bank.json"
cp["StudentExerciseBankRuntimeStatus"]="92_FIRST_PARTY_TASKS_VIEW_AUTHORED_NOT_RUNTIME_TESTED"
cp["SourceExercisesMatchedInEdulytics"]=0
cp["ReadyForStageD"]=False
cp["Tests"]="WRITTEN_NOT_RUN"
checkpoint_path.write_text(json.dumps(cp,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")

progress_path=base/"full-grade-progress.json"
pg=json.loads(progress_path.read_text(encoding="utf-8"))
p=pg["LessonEntries"][0]
p["FirstPartySectionExerciseBankPromptsAuthored"]=92
p["FirstPartySectionExerciseBankPath"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u01-l01-first-party-exercise-bank.json"
p["StudentExerciseBankRuntimeStatus"]="AUTHORING_92_FIRST_PARTY_TASKS_NO_G12_TEACHER_SUBMISSION"
p["ReadyForStageD"]=False
p["UpdatedUtc"]=datetime.now(timezone.utc).isoformat()
progress_path.write_text(json.dumps(pg,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("AUTHORING_92_TASKS_COMPLETE")
print("TEACHER_DIAGRAMS",sum(x["Mode"]=="TEACHER_DIAGRAM_REQUIRED" for x in tasks))
print("SOURCE_MATCHED_0_NOT_GRADED")

# Candidate-only 1:1 exercise links: do not silently certify the 92 prompts.
coverage_path=base/"u01-l01-element-coverage.json"
coverage=json.loads(coverage_path.read_text(encoding="utf-8"))
by_id={e["SourceElementId"]:e for e in coverage["Elements"]}
for task in tasks:
    source=by_id.get(task["SourceElementId"])
    assert source is not None,task
    source["FirstPartyStudentPracticeTaskNumber"]=task["Number"]
    source["StudentPracticeAuthoringFile"]="src/Edulytics.Web/Presentation/Grade12PrecalculusSection11Exercises.cs"
    source["StudentPracticeAuthoringStatus"]="FIRST_PARTY_SKILL_CANDIDATE_REVIEW_REQUIRED"
    source["StudentPracticeSemanticParity"]="NOT_VERIFIED"
coverage["FirstPartyStudentPracticeTaskBankCount"]=92
coverage["StudentPracticeTaskSourceBankPath"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u01-l01-first-party-exercise-bank.json"
coverage["AuthoringEvidence"]["FirstPartySourceNumberedExerciseTaskPrompts"]=92
coverage["AuthoringEvidence"]["SourceSemanticMatchesCertified"]=0
coverage_path.write_text(json.dumps(coverage,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")

for task,source in zip(tasks,records):
    source["FirstPartyStudentPracticeTaskNumber"]=task["Number"]
    source["StudentPracticeAuthoringStatus"]="FIRST_PARTY_CANDIDATE_NOT_SOURCE_PARITY_CERTIFIED"
    source["StudentPracticeGrading"]="NOT_AUTOMATICALLY_GRADED_BY_92_EXERCISE_BANK"
    source["StudentPracticeReviewMode"]=task["Mode"]
route["StudentFirstPartyExerciseTaskBankPath"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u01-l01-first-party-exercise-bank.json"
route["StudentFirstPartyExerciseTaskCount"]=92
route["StudentFirstPartyExerciseBankStatus"]="WRITTEN_RUNTIME_NOT_TESTED; NO_92_EXERCISE_DIGITAL_PARITY_ASSERTION"
(base/"u01-l01-exercise-routing.json").write_text(json.dumps(route,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")

teacher_review={
"ContentVersion":"g12-section11-first-party-practice-92-tasks-v1",
"SourceSection":"1.1",
"TeacherAssessmentRoute":"EXISTING_AUTHENTICATED_GENERIC_TEACHER_REVIEW_REQUIRES_G12_INTEGRATION",
"DeliveryRoute":"STUDENT_LESSON_SECTION_1_1_READ_ONLY_PROMPTS",
"SourceExercisesAddressedByOrdinal":list(range(1,93)),
"Rules":{
    "All":"Require explanation for answer rationale; do not release answer key in student-facing page.",
    "1-5":"Concept definitions and examples; accept mathematically valid original reasoning.",
    "6-26":"Correct function classification, domain and branch restrictions; no automatic scoring claimed.",
    "27-39":"Show substitutions, all real roots, domain conditions and correct simplification.",
    "40-59":"Teacher must provide and inspect a verified plot; graph descriptions alone are not source image-equivalent. Ex.52(b) cannot require an ungrounded exact root.",
    "60-75":"Show complete tables and all requested inputs; check exact radicals, rational values and unit domains.",
    "76-87":"Graph using the exact window and report range with endpoints; tiny and enormous windows are not interchangeable.",
    "88-92":"Preserve physical units, time origin, and distinct-input counterexamples."
},
"RubricCriteria":[
    {"Label":"Mathematical accuracy and complete permitted solution set","Points":2},
    {"Label":"Representation, diagram/window and stated domain accuracy","Points":2},
    {"Label":"Proof, explanation and interpretation of the objective","Points":2},
    {"Label":"Correct units, precision, symbols and valid inputs","Points":2}
],
"ScorePersistsInGradebook":False,
"TeacherAccessRuntimeVerified":False,
"RequiresHumanJudgement":True,
"Scope":"A–C teacher review directions only; durable tenant-isolated submission integration remains a blocker",
"Status":"AUTHORING_NOT_RUNTIME_VERIFIED",
"Tests":"WRITTEN_NOT_RUN"
}
reviewpath=base/"u01-l01-first-party-exercise-bank-teacher-review.json"
reviewpath.write_text(json.dumps(teacher_review,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
ass["FirstPartySectionExerciseBank"]["TeacherGuidePath"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u01-l01-first-party-exercise-bank-teacher-review.json"
assessment_path.write_text(json.dumps(ass,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
cp["FirstPartySectionExerciseTeacherGuide"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u01-l01-first-party-exercise-bank-teacher-review.json"
checkpoint_path.write_text(json.dumps(cp,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("SOURCE_LEDGER_CANDIDATES_LINKED",len(tasks))
