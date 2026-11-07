from pathlib import Path
import json
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
guide=root/"src/Edulytics.Web/Presentation/Grade12PrecalculusSection11TeacherGuidance.cs"
s=guide.read_text(encoding="utf-8")
old='''[52] = new(52, "Graph reading: f(−1) is approximately 1. The exact input for f(x)=3 cannot be certified from the finite plotted window alone; do not invent a numerical root without a warranted extension.",'''
new='''[52] = new(52, "Grade the STUDENT'S FIRST-PARTY graph y=√(x+2): f(−1)=1 and the plotted level y=3 is reached at x=7. The original source graph separately shows f(−1)≈1; it does not independently justify an exact numerical root for y=3.",'''
assert s.count(old)==1
guide.write_text(s.replace(old,new),encoding="utf-8",newline="\n")
generator=root/"tools/curriculum/g12_l01_teacher_guidance_delivery.py"
gen=generator.read_text(encoding="utf-8")
old='''explanation=("Graph reading: f(−1) is approximately 1. The exact input for "
            "f(x)=3 cannot be certified from the finite plotted window alone; "
            "do not invent a numerical root without a warranted extension.")'''
new='''explanation=("Grade the STUDENT'S FIRST-PARTY graph y=√(x+2): f(−1)=1 and "
            "the plotted level y=3 is reached at x=7. The original source graph "
            "separately shows f(−1)≈1; it does not independently justify an exact "
            "numerical root for y=3.")'''
assert gen.count(old)==1
generator.write_text(gen.replace(old,new),encoding="utf-8",newline="\n")
teacher_reference=base/"u01-l01-first-party-exercise-bank-teacher-review.json"
r=json.loads(teacher_reference.read_text(encoding="utf-8"))
r["OriginalGraph52TeacherGuidance"]=(
    "Original source: graph read f(-1) approximately 1 and any f(x)=3 "
    "input is imprecise due to a graph ending near y=3. Student's independent "
    "first-party model y=sqrt(x+2) has exact f(-1)=1 and x=7 for y=3. "
    "Award student credit for accurate FIRST-PARTY answer x=7; do not "
    "misrepresent it as a demonstrated exact original-source result."
)
teacher_reference.write_text(json.dumps(r,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
test=root/"tests/Edulytics.Tests/MathematicsIntelligence/Grade12PrecalculusSection11SourceContractsTests.cs"
st=test.read_text(encoding="utf-8")
anchor='''        Assert.Contains("tons per week",
            Grade12PrecalculusSection11TeacherGuidance.Find(88)!.ExpectedMathematics,
            StringComparison.Ordinal);'''
insert='''        Assert.Contains("x=7",
            Grade12PrecalculusSection11TeacherGuidance.Find(52)!.ExpectedMathematics,
            StringComparison.Ordinal);
        Assert.Contains("original source graph",
            Grade12PrecalculusSection11TeacherGuidance.Find(52)!.ExpectedMathematics,
            StringComparison.OrdinalIgnoreCase);
'''
assert st.count(anchor)==1
test.write_text(st.replace(anchor,anchor+"\n"+insert),encoding="utf-8",newline="\n")
assessment=base/"u01-l01-assessment.json"
a=json.loads(assessment.read_text(encoding="utf-8"))
a["FirstPartySectionExerciseBank"]["Exercise52TeacherScoreSourceBoundary"]="STUDENT_FIRST_PARTY_X_EQUALS_7_VALID; ORIGINAL_EXACT_SOURCE_GRAPH_ROOT_NOT_CLAIMED"
assessment.write_text(json.dumps(a,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
check=base/"u01-l01-abc-checkpoint.json"
c=json.loads(check.read_text(encoding="utf-8"))
c["Graph52TeacherMathAnswerVsSourceBoundary"]="FIRST_PARTY_X7_CORRECT_SOURCE_ORIGINAL_EXACT_ROOT_UNKNOWN"
c["ReadyForStageD"]=False
check.write_text(json.dumps(c,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
evidence=base/"u01-l01-graph52-first-party-grading-correction.md"
evidence.write_text("""# Section 1.1 exercise 52: avoid a teacher grading false negative

Source: locally verified OpenStax Precalculus 2017, PDF p.36:
original graph approximately passes through (-1,1) and approaches y=3
near the edge of its displayed window. It has no stated algebraic rule.
An exact original numerical root for f(x)=3 is not demonstrable from
the finite image alone; accepting a graphical estimate is a teacher decision.

First-party task intentionally uses y=sqrt(x+2), with a fully specified
domain and drawn graph extending through the level y=3. On this **new**
student-assessed model, f(-1)=1 and f(x)=3 exactly when x=7.

Bug corrected: the teacher-only grading guidance previously mentioned only
the source's unresolved exact root, which risked rejecting an entirely valid
x=7 answer to the student-facing independent task. It now distinguishes
the original source graph from the Edulytics graph and explicitly accepts
x=7 for the latter.

Source image not copied. Source perfect pixel parity not claimed.
No Stage D, CI, E2E or automated tests run.
""",encoding="utf-8",newline="\n")
print("FIXED_GRADE12_52_FIRST_PARTY_TEACHER_ANSWER_MISMATCH_NO_FALSE_SOURCE_PARITY")
