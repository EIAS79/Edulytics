from pathlib import Path
import json
from collections import Counter
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
richpath=root/"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u01-l01.rich-lesson-v2.json"
rich=json.loads(richpath.read_text(encoding="utf-8"))
worked=rich["Lessons"][0]["WorkedExamples"]
assert len(worked)==16

# Original example 5: three separate source tables, one graph classification
# question per table (not merely one repeated-pair example).
five=worked[4]
assert "A table contains (1,3)" in five["Question"]
five["Question"]=(
    "Compare three independently authored relation tables. "
    "Table A: (2,1),(5,3),(8,6). "
    "Table B: (-3,5),(0,1),(4,5). "
    "Table C: (1,0),(5,2),(5,4). "
    "Classify whether EACH table defines y as a function of x; "
    "for the functional tables decide whether one-to-one, with reasons."
)
five["Method"]="Check each input and its output independently in all three tables; then test repeated outputs."
five["Steps"]=[
    "Table A has three distinct inputs and three distinct outputs: it is a one-to-one function.",
    "Table B has distinct inputs, but -3 and 4 both map to 5: it is a function but not one-to-one.",
    "Table C maps the same input 5 to both 2 and 4: it is not a function at all."
]
five["Answer"]="Table A: one-to-one function. Table B: function but not one-to-one. Table C: not a function."
five["Check"]="A repeated output is allowed for a function; a repeated input with a conflicting output is not."

# Original example 6 has FOUR explicitly requested mathematical actions, not three.
six=worked[5]
assert "f(2), f(a+h)" in six["Question"]
six["Question"]=six["Question"].replace(
    "calculate f(2), f(a+h), and [f(a+h)−f(a)]/h",
    "calculate f(2), f(a), f(a+h), and [f(a+h)−f(a)]/h")
six["Steps"]=[
    "Evaluate at x=2: f(2)=4+6−4=6.",
    "Evaluate at symbolic input a: f(a)=a²+3a−4.",
    "At input a+h, f(a+h)=(a+h)²+3(a+h)−4=a²+2ah+h²+3a+3h−4.",
    "For h≠0, subtract f(a) from f(a+h), factor h, then simplify the difference quotient to 2a+h+3."
]
six["Answer"]="f(2)=6; f(a)=a²+3a−4; f(a+h)=a²+2ah+h²+3a+3h−4; quotient=2a+h+3."

# Original example 14 has THREE plotted relations, two functions and one not.
fourteen=worked[13]
assert "y=x² and y²=x" in fourteen["Question"]
fourteen["Question"]=(
    "Apply the vertical-line test to three independent model graphs: "
    "(A) y=x², (B) y=2x+1, and (C) y²=x. "
    "Which of A, B, C represent y as a function of x, and why?"
)
fourteen["Steps"]=[
    "A: each real x has one output x², so every vertical line meets the parabola at most once.",
    "B: each real x has exactly one output 2x+1, so a nonvertical straight line passes.",
    "C: at x=4 the sideways relation contains both (4,2) and (4,-2), so it fails."
]
fourteen["Answer"]="Graphs A and B are functions of x. Graph C is not a function of x."
fourteen["Check"]="The test concerns the number of outputs for a SINGLE x; it is not a test of injectivity."
rich["ContentVersion"]="g12-u01-l01-atomic-source-example-subparts-reconciled-20261106"
richpath.write_text(json.dumps(rich,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")

examples={
1:["a: determine if price is a function of item","b: determine if item is a function of price"],
2:["a: is GPA a function of percentage grade?","b: is percentage grade a function of GPA?"],
3:["Represent number of days as a function of month name using notation"],
4:["Interpret f(2005)=300 officers with input and output units"],
5:["Classify Table 6 as a function","Classify Table 7 as a function","Classify Table 8 as a function"],
6:["a: evaluate f(2)","b: evaluate f(a)","c: evaluate f(a+h)","d: simplify [f(a+h)-f(a)]/h for h != 0"],
7:["Evaluate h(4) for h(p)=p²+2p"],
8:["Solve h(p)=3 for ALL allowed p"],
9:["Express the linear relationship 2n+6p=12 as p=f(n)"],
10:["Determine whether circle x²+y²=1 defines y as function of x"],
11:["a: evaluate g(3) from Table 11","b: solve g(n)=6 for all listed inputs"],
12:["a: read f(2) from Figure 7","b: solve f(x)=4 from Figure 7"],
13:["a: determine if circle area is a function of radius","b: determine whether area is one-to-one for nonnegative radius"],
14:["a: apply vertical-line test to Figure 12(a)","b: apply vertical-line test to Figure 12(b)","c: apply vertical-line test to Figure 12(c)"],
15:["a: apply horizontal-line test to Figure 12(a)","b: apply horizontal-line test to Figure 12(b)"]
}
tryits={
1:["Classify functional relation between book ISBN and its title"],
2:["Write weight of a pig as a function of age in days"],
3:["Determine whether Table 9 is a function"],
4:["Evaluate g(5)=sqrt(5-4)"],
5:["Solve sqrt(m-4)=2 for m"],
6:["Express x-8y³=0 as y=f(x)"],
7:["Read the requested value from the source tabulated function"],
8:["Read the requested inverse input(s) from the plotted function"],
9:["a: is account balance a function of account number?",
   "b: is account number a function of balance?",
   "c: is balance one-to-one with account number?"],
10:["a: does percentage grade determine letter grade as a function?",
    "b: is letter grade a one-to-one function of percentage?"],
11:["Apply the vertical-line test to the source graph"],
12:["Apply the horizontal-line test to decide one-to-one status from source graph"]
}
assert sum(len(x) for x in examples.values())==28
assert sum(len(x) for x in tryits.values())==15

inventory=json.loads((base/"u01-l01-element-coverage.json").read_text(encoding="utf-8"))
parents={x["SourceElementId"]:x for x in inventory["Elements"]}
assert len(parents)==170
exercise_atoms=json.loads((base/"u01-l01-exercise-atomic-source-inventory.json").read_text(encoding="utf-8"))["Atoms"]
assert len(exercise_atoms)==162
atoms=[]
for e in exercise_atoms:
    n=e["OriginalExerciseNumber"]
    atoms.append({
      "AtomId":e["AtomicElementId"],
      "ParentId":e["ParentExerciseId"],
      "Category":"EXERCISE",
      "OriginalParentNumber":n,
      "SourcePdfPage":e["PdfPage"],
      "OriginalPartLabel":e.get("OriginalPartLabel"),
      "SourceActionScope":e.get("SourcePartScope") or "Complete the original numbered exercise",
      "FirstPartyDelivery":"src/Edulytics.Web/Presentation/Grade12PrecalculusSection11Exercises.cs",
      "FirstPartyTeachingTask":n,
      "TeacherReviewChecklist":"src/Edulytics.Web/Presentation/Grade12PrecalculusSection11SourcePartChecklist.cs",
      "SourceNumericalPrecisionUnresolved":n==52 and e.get("OriginalPartLabel")=="b",
      "SourceElementSemanticParityCertified":False
    })
for category,records,kind in [("EXAMPLE",examples,"WorkedExamples"),("TRY",tryits,"TryIts")]:
    for n,parts in records.items():
        parent=f"G12-U01-L01-{category}{n:03d}"
        assert parent in parents
        for subindex,scope in enumerate(parts,1):
            atoms.append({
              "AtomId":f"{parent}-PART{subindex:02d}",
              "ParentId":parent,
              "Category":category,
              "OriginalParentNumber":n,
              "SourcePdfPage":parents[parent]["SourceLocator"]["PdfPage"],
              "OriginalPartLabel":scope.split(":")[0] if ":" in scope else None,
              "SourceActionScope":scope,
              "FirstPartyDelivery":(
                  "src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u01-l01.rich-lesson-v2.json"
                  if category=="EXAMPLE" else
                  "src/Edulytics.Web/Presentation/Grade12PrecalculusSection11Exercises.cs"),
              "FirstPartyTeachingTask":n if category=="EXAMPLE" else 93+n,
              "SourceNumericalPrecisionUnresolved":False,
              "SourceElementSemanticParityCertified":False
            })
for p in parents.values():
    if p["Category"] in ["EXERCISE","EXAMPLE","TRY"]:
        continue
    atoms.append({
      "AtomId":p["SourceElementId"],
      "ParentId":p["SourceElementId"],
      "Category":p["Category"],
      "OriginalParentNumber":None,
      "SourcePdfPage":p["SourceLocator"]["PdfPage"],
      "OriginalPartLabel":None,
      "SourceActionScope":p["ExpectedMathematics"],
      "FirstPartyDelivery":p.get("FirstPartyAlternativePath") or p.get("FirstPartyStructuredVisualFile") or p.get("EdulyticsArtifactPath"),
      "SourceNumericalPrecisionUnresolved":False,
      "SourceElementSemanticParityCertified":False
    })
assert len(atoms)==256,len(atoms)
assert len({x["AtomId"] for x in atoms})==256
counts=Counter(x["Category"] for x in atoms)
assert counts["EXERCISE"]==162 and counts["EXAMPLE"]==28 and counts["TRY"]==15
assert sum(v for k,v in counts.items() if k not in ("EXERCISE","EXAMPLE","TRY"))==51
path=base/"u01-l01-256-named-source-atomic-actions.json"
j={
 "SchemaVersion":1,
 "Course":"US-CCSS-MATH:G12","Unit":1,"Lesson":1,
 "SourceText":"Authorized local OpenStax Precalculus 2017 PDF Section 1.1",
 "OriginalParentLocationAnchors":170,
 "OriginalNamedStudentExerciseCount":92,
 "OriginalSourceWorkedExamples":15,
 "OriginalTryItCount":12,
 "OriginalExerciseLeafActions":162,
 "OriginalWorkedExampleExplicitQuestionLeafActions":28,
 "OriginalTryItExplicitQuestionLeafActions":15,
 "OtherTopLevelSourceElementsKeptAsLeaves":51,
 "TotalNamedSourceLearningAndPresentationLeafActions":256,
 "CountMethod":"Replace every explicit multipart source exercise, worked example and Try It parent with only its leaf actions. Nonmultipart figures, tables, how-tos, objectives and media stay parent leaves. Never count parent plus children.",
 "OriginalGraph52ExactRootUnknown":True,
 "SourceNamedLearningActionDenominatorAudited":True,
 "SourceCompleteFineGrainedMediaCellAndParagraphDenominatorVerified":False,
 "SourceSemanticMatchedForAllLeaves":False,
 "FirstPartyStudentDeliveryAndTeacherContractsCompiled":"PARTIAL_TARGETED_ONLY",
 "StageD":"NOT_RUN",
 "ReadyForStageD":False,
 "Categories":dict(counts),
 "Atoms":atoms
}
path.write_text(json.dumps(j,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
ledger=base/"u01-l01-element-coverage.json"
l=json.loads(ledger.read_text(encoding="utf-8"))
l["NamedSourceLeafActionInventoryPath"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u01-l01-256-named-source-atomic-actions.json"
l["SourceNamedAtomicLeafActionCount"]=256
l["SourceExplicitExampleQuestionActions"]=28
l["SourceExplicitTryItQuestionActions"]=15
l["SourceFineGrainedSourceAtomicDenominatorVerified"]=False
l["SourceAtomicDenominatorVerified"]=False
ledger.write_text(json.dumps(l,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
for name in ("u01-l01-abc-checkpoint.json","full-grade-progress.json"):
    dest=base/name
    s=json.loads(dest.read_text(encoding="utf-8"))
    row=s if name.startswith("u01") else s["LessonEntries"][0]
    row["NamedSourceLeafActionInventoryPath"]=l["NamedSourceLeafActionInventoryPath"]
    row["SourceNamedAtomicLeafActionCount"]=256
    row["SourceExplicitExampleQuestionActions"]=28
    row["SourceExplicitTryItQuestionActions"]=15
    row["SourceAtomicDenominatorVerified"]=False
    row["ReadyForStageD"]=False
    dest.write_text(json.dumps(s,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("256_NAMED_SOURCE_LEAF_ACTIONS_INVENTORIED; EXAMPLE_6_AND_5_AND_14_SUBPART_GAPS_CORRECTED")
