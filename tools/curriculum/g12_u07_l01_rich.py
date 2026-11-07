from pathlib import Path
import json,math,datetime,subprocess,re
r=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
b=r/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
pfile=b/"full-grade-progress.json";p=json.loads(pfile.read_text(encoding="utf8"))
a=b/"u07-l01-source-inventory.json";audit=json.loads(a.read_text(encoding="utf8"))
q=json.loads((b/"u07-l01-independent-math-questions.json").read_text(encoding="utf8"))
items=q["IndependentMathQuestionTeacherKeys"]
assert p["LastClosedLessonOrdinal"]==35 and p["FirstIncompleteLessonOrdinal"]==36
assert len(items)==42 and [x["SourceExercise"]for x in items]==list(range(1,43))
# Correct a rational expression and check select identity numerically.
items[19]["TeacherAnswer"]="2csc²x−1, sinx≠0 (since (1+cos²x)/sin²x)"
x=.73;s=math.sin(x);c=math.cos(x)
assert math.isclose(1/(1-c)-c/(1+c),2/(s*s)-1)
assert math.isclose((1+s)/c,c/(1-s))
assert math.isclose((math.tan(x)+c/s)/(1/c),1/s)
assert math.isclose((c*c-s*s)/(1-(s/c)**2),c*c)
q["IndependentMathQuestionTeacherKeys"]=items
(b/"u07-l01-independent-math-questions.json").write_text(json.dumps(q,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
W=[
("Pythagorean","Prove sin²t+cos²t=1","Unit-circle equation x²+y²=1."),
("Reciprocal","Simplify sec t cos t","1, for cos t≠0."),
("Odd-even","Simplify sin(-t)cos(-t)","−sin t cos t."),
("Use sec identity","Simplify sec²t−tan²t","1, where cos t≠0."),
("Combine quotient","Simplify tan t+cot t","1/(sin t cos t), both nonzero."),
("Parity algebra","Simplify cos(-t)−sin(-t)","cos t+sin t."),
("Multiply reciprocal ratios","Simplify cot t tan t","1 on common domain."),
("Square identity","Rewrite cos²t in terms of sine","1−sin²t."),
("Difference of squares","Simplify (sec t−tan t)(sec t+tan t)","1 on common domain."),
("Substitution","Simplify 1/(1+tan²t)","cos²t where cos t≠0.")
]
T=[("Simplify sin²t+cos²t","1"),("Simplify csc t sin t","1 for sin t≠0"),
("Simplify sin(-t)+cos(-t)","−sin t+cos t"),
("Simplify tan²t−sec²t","−1 where cos t≠0"),
("Express sec²t with tan²t","1+tan²t")]
code="PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U07:L01"
rrel="src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u07-l01.rich-lesson-v2.json"
trel="src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u07-l01-firstparty-teacher-led.json"
def save(path,obj):path.parent.mkdir(parents=True,exist_ok=True);path.write_text(json.dumps(obj,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
expl=[
"Identities hold for all inputs in the common domain of the two expressions. Equations may instead hold only for particular inputs.",
"Fundamental Pythagorean: sin²t+cos²t=1; 1+tan²t=sec²t where cos t≠0; 1+cot²t=csc²t where sin t≠0.",
"Reciprocal: sec=1/cos, csc=1/sin; quotient tan=sin/cos, cot=cos/sin. Preserve original denominator exclusions when simplifying.",
"Cos and sec are even; sin, tan, cot, csc are odd, on their domains. Use these identities to transform negative angles.",
"Algebraic proof uses factoring, expanding, common denominators, and conjugates; checking individual numeric inputs only disproves false identities.",
"Independent Cool-down: simplify (1−cos²t)/sin²t, simplify sec²t−tan²t and disprove sin t+cos t=1 universally."
]+[f"Independent Try It {i}: {question}"for i,(question,answer)in enumerate(T,1)]
rich={"SchemaVersion":2,"PackCode":"US-CCSS-MATH:G12:PRECALC:OPENSTAX-2017:U07:L01",
"ContentVersion":"g12-u07-l01-firstparty-identity-abc",
"Lessons":[{"LessonCode":code,"CultureCode":"en","Title":"Solving Trigonometric Equations with Identities",
"ExplanationParagraphs":expl,
"WorkedExamples":[{"Title":title,"Question":question,"Method":"Use algebraic identity and check denominators.",
"Steps":[answer],"Answer":answer,"Check":"Verify on common domain."}for title,question,answer in W],
"KeyConcepts":[{"Title":"Pythagorean identities","Definition":"sin²+cos²=1; 1+tan²=sec²; 1+cot²=csc²","Rule":"Specify denominator domain.","Example":"sec²−tan²=1"},
{"Title":"Function parity","Definition":"Cosine/secant even, sine/tangent/cotangent/cosecant odd","Rule":"Replace negative input with correct sign.","Example":"sin(-t)=-sin t"},
{"Title":"Proof vs example","Definition":"Identity holds throughout common domain.","Rule":"Numerical counterexample disproves but numerical agreement doesn't prove.","Example":"sin t+cos t=1 fails at π/4"}],
"Visuals":[{"Kind":"RatioTable","Title":"Independent trigonometric identity validity table",
"Description":"Mathematically authored table of identities and domains.",
"PrimaryLabel":"Identity","SecondaryLabel":"Required domain",
"Rows":[{"Label":"Equation","Cells":["sin²+cos²=1","sec²−tan²=1","csc²−cot²=1"]},
{"Label":"Domain","Cells":["all real","cos t≠0","sin t≠0"]}],"Items":[],"SecondaryItems":[]}],
"Videos":[],"SummaryPoints":["Preserve domain in every trig cancellation.","Algebra proves identities; a counterexample disproves.","Use reciprocal, Pythagorean and parity laws."],
"CommonMistakes":[{"Mistake":"Cancel denominators without domain restriction","WhyWrong":"Original expression undefined at zeros","Correction":"Retain original domain."}]}]}
teacher={"SchemaVersion":1,"LessonCode":code,"FirstPartyIndependentStudentMathQuestions":items,
"IndependentTryIt":[{"Number":i,"Question":question,"TeacherExpectedMath":answer}for i,(question,answer)in enumerate(T,1)],
"IndependentCoolDown":{"StudentQuestions":["Simplify (1−cos²t)/sin²t.","Simplify sec²t−tan²t.","Disprove sin t+cos t=1 universally."],
"TeacherExpectedMath":["1 for sin t≠0.","1 for cos t≠0.","At t=π/4 sum=sqrt2."],"OriginalPublisherCoolDownCopied":False},
"SourceExercisesReviewed":42,"StageC":"REUSE_EXISTING_TEACHER_MANUAL_MATH"}
save(r/rrel,rich);save(r/trel,teacher)
routes="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u07-l01-stage-c-42-teacher-math-routes.json"
save(r/routes,{"Section":"7.1","Count":42,"Decision":"Reuse RichV2 and existing teacher manual scoring","NewEngine":False,
"Routes":[{"Source":i,"TeacherRubric":True,"RuntimeVerified":False}for i in range(1,43)],"StageD":"NOT_RUN","CI":"NOT_RUN"})
print("U07L01_STUDENT_TEACHER_RICH_CONTENT_CREATED_READY_FOR_FIGURE_REVIEW")
