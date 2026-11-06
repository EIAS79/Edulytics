#!/usr/bin/env python3
"""Author first-party classroom tasks for the twelve Section 1.1 Try-It skills.
Source key PDF p1097 was checked independently, not redistributed.
All authored items remain NOT_RUNTIME_ASSESSED and NOT_MATCHED until D.
"""
from pathlib import Path
import json,datetime
repo=Path(__file__).resolve().parents[2]
base=repo/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
now=datetime.datetime.now(datetime.timezone.utc).isoformat()
tasks=[
 ("Classifying a pair of mappings",
  "In a small library, book IDs A, B, C identify titles Alpha, Beta, Gamma without duplicates. Is title a function of book ID? Is book ID a function of title?",
  "Yes in both directions for the stated dataset. Each ID has one title and each title has one ID.","Function versus one-to-one mapping."),
 ("Function notation in context",
  "A plant's measured height h in centimetres depends on the day d since planting. Express this as a named function and state what h(8)=17 means.",
  "h=f(d) or h(d); h(8)=17 means 17 centimetres on day eight.","Contextual function notation."),
 ("Classification of a table",
  "A table contains input/output pairs (1,10), (2,100), (3,1000). Decide whether this is a function and justify.",
  "Yes; every input has one output.","Table classification."),
 ("Evaluate an integer radical",
  "For f(t)=sqrt(t-9), calculate f(13) and state the real domain.",
  "f(13)=2 and t>=9.","Square-root substitution and domain."),
 ("Solve an integer radical equation",
  "For f(t)=sqrt(t-9), find the input with output four.",
  "sqrt(t-9)=4 gives t-9=16, so t=25.","Inverse function-value solving."),
 ("Isolate a cubed dependent variable",
  "Rewrite x=27y^3 as y=f(x) for real x.",
  "y=cuberoot(x/27)=cuberoot(x)/3.","Implicit relationship to explicit function."),
 ("Evaluate a table-encoded function",
  "The table shows (0,6), (1,8), (2,10). Find g(1).",
  "g(1)=8.","Read exact output from table."),
 ("Solve a function equation from a graph",
  "On coordinate paper, graph f(x)=|x-1|, then identify each x at which f(x)=1.",
  "x=0 and x=2, by inspecting horizontal line y=1.","Graph reading and multiple inputs."),
 ("Inverse direction in a contextual mapping",
  "Each employee is assigned one department; departments can contain multiple employees. Is department a function of employee? Is employee a function of department? Is the first function one-to-one?",
  "Yes, no, no under the stated data model with at least one multi-employee department.","Unique-output vs inverse and injective."),
 ("Contextual function versus injectivity",
  "A score from 0 to 100 determines exactly one letter grade, and several scores share each letter. Is letter grade a function of numeric score? Is it one-to-one?",
  "Yes, but not one-to-one.","Different inputs may share outputs."),
 ("Vertical-line test for a graph",
  "Sketch y=|x| and explain whether each vertical line meets its graph in at most one point.",
  "Yes. Every x determines exactly one y=|x|.","Vertical line test."),
 ("Horizontal-line test for a function",
  "Graph y=x^2 on all real inputs. Is it one-to-one? Provide a horizontal-line counterexample.",
  "No; y=4 meets the graph at x=-2 and x=2.","Horizontal-line test and domain.")
]
assert len(tasks)==12
source=base/"u01-l01-element-coverage.json"
s=json.loads(source.read_text(encoding="utf8"))
rich=repo/"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u01-l01.rich-lesson-v2.json"
doc=json.loads(rich.read_text(encoding="utf8"))
content=doc["Lessons"][0]
assert len(content["WorkedExamples"])==15
old=[x for x in content["ExplanationParagraphs"] if "Independent Try It" in x]
assert not old
# The final cool-down remains at the end of the student lesson.
assert "cool-down" in content["ExplanationParagraphs"][-1].lower()
newparas=[
 f"Independent Try It {i} — {title}. {question} Explain your reasoning to your teacher; diagrams and open-ended reasoning are teacher-reviewed."
 for i,(title,question,expected,skill) in enumerate(tasks,1)]
content["ExplanationParagraphs"][-1:-1]=newparas
doc["ContentVersion"]="g12-u01-l01-abc-15-worked-examples-12-tryit-alternatives-20261106"
rich.write_text(json.dumps(doc,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
cross=[]
for i,(title,question,expected,skill) in enumerate(tasks,1):
  ident=f"G12-U01-L01-TRY{i:03d}"
  element=next(e for e in s["Elements"] if e["SourceElementId"]==ident)
  assert element["Status"]=="SOURCE_IDENTIFIED",ident
  element["Status"]="SOURCE_VERIFIED"
  element["OriginalAnswerKeyPdfPage"]=1097
  element["FirstPartyPedagogicalSkill"]=skill
  element["EdulyticsArtifactPath"]="src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u01-l01.rich-lesson-v2.json"
  element["ImplementationType"]="FIRST_PARTY_STUDENT_EXERCISE_PROMPT_AUTHORED_BUT_NOT_EVALUATED"
  element["VerificationMethod"]="LOCAL_OPENSTAX_TRY_IT_ANSWER_APPENDIX_PAGE1097_AND_SOURCE_SECTION_SKILL_REVIEW; NOT_SOURCE_GRAPHIC_PARITY"
  element["DigitalDeliveryAndTeacherFeedbackVerified"]=False
  cross.append({
   "SourceId":ident,"OriginalBookPdfPage":element["SourceLocator"]["PdfPage"],
   "SourceAnswerAppendixPdfPage":1097,"LearningSkill":skill,
   "EdulyticsTitle":title,"EdulyticsIndependentPrompt":question,
   "ExpectedTeacherAnswer":expected,"GradingMode":"TEACHER_REVIEW_WITH_EXPLANATION",
   "AutoGrade":False,"SourceMathVerified":True,
   "StudentRichV2Authored":True,"StudentRuntimeVerified":False,
   "TeacherRubricPersisted":False,"Status":"AUTHORED_NOT_RUNTIME_VERIFIED"})
s["SourceTryItAnswerAppendixVerified"]=12
s["IndependentStudentTryItPromptsAuthored"]=12
s["IndependentTryItsDigitallyVerified"]=0
s["MatchedDigitally"]=s["MatchedTeacherLed"]=0
s["UpdatedUtc"]=now
source.write_text(json.dumps(s,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
dst=base/"u01-l01-tryit-first-party-teacher-review.json"
dst.write_text(json.dumps({
 "LessonCode":content["LessonCode"],
 "Author":"Edulytics first-party equivalent task authoring; no original graphics copied",
 "LocalOpenStaxPdfSha256":s["SourceSnapshotId"],
 "OriginalTryItSourcePageRange":[20,31],
 "OriginalTryItAnswersPdfPage":1097,
 "OriginalTryItCount":12,
 "IndependentPromptsAuthored":12,
 "TeacherAssessmentRoute":"INTENDED_ONLY_NOT_TESTED",
 "AutomatedPracticeCoverage":"NOT_YET_VERIFIED",
 "Items":cross,"CreatedUtc":now
},ensure_ascii=False,indent=2)+"\n",encoding="utf8")
checkpoint=base/"u01-l01-abc-checkpoint.json"
k=json.loads(checkpoint.read_text(encoding="utf8"))
k["OriginalTryItSourceAnswerChecks"]=12
k["StudentRichV2IndependentTryItItemsAuthored"]=12
k["DigitalPracticeTryItsVerified"]=0
k["A_C_Status"]="A_C_EXPLICIT_GAP"
k["UpdatedUtc"]=now
checkpoint.write_text(json.dumps(k,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
print("G12_U01_L01_TRYIT_SOURCE_12_VERIFIED_FIRST_PARTY_12_AUTHORED_RUNTIME_MATCHED_0")
