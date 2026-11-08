from pathlib import Path
import json
from collections import defaultdict
from datetime import datetime,timezone
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
source=base/"u01-l01-exercise-atomic-source-inventory.json"
d=json.loads(source.read_text(encoding="utf-8"))
atoms=d["Atoms"]
assert len(atoms)==162
group=defaultdict(list)
for atom in atoms:
    n=atom["OriginalExerciseNumber"]
    label=atom.get("OriginalPartLabel")
    scope=atom.get("SourcePartScope")
    if label is None:
        label="whole exercise"
        scope="Complete the whole original-numbered mathematical exercise; include explanations, domain restrictions and diagrams when requested."
    status=atom["SourceIdentificationStatus"]
    group[n].append((label,scope,status,atom["AtomicElementId"]))
assert sorted(group)==list(range(1,93))
assert sum(len(x) for x in group.values())==162
def cs(t):
    return json.dumps(t,ensure_ascii=False)
lines=[
"namespace Edulytics.Web.Presentation;",
"",
"/// <summary>",
"/// Instructor-only checklist of original numbered Section 1.1 exercise atoms.",
"/// 92 parent exercises => 56 singleton actions and 106 explicit parts =>",
"/// 162 atomic review actions. This DOES NOT assert authenticated grading,",
"/// complete worked-example atoms or original source image equivalence.",
"/// </summary>",
"public static class Grade12PrecalculusSection11SourcePartChecklist",
"{",
"    public sealed record Part(int ParentExercise, string Label, string RequestedSkill,",
"        bool OriginalSourceNumericalResultUnresolved, string SourceElementId);",
"",
"    private static readonly IReadOnlyDictionary<int, IReadOnlyList<Part>> Parts =",
"        new Dictionary<int, IReadOnlyList<Part>>",
"        {"]
for n,rows in sorted(group.items()):
    lines.append(f"            [{n}] = new Part[]")
    lines.append("            {")
    for label,scope,status,source_id in rows:
        unresolved=status=="SOURCE_GRAPH_EXACT_ROOT_UNRESOLVED"
        lines.append(f"                new({n}, {cs(label)}, {cs(scope)}, {str(unresolved).lower()}, {cs(source_id)}),")
    lines.append("            },")
lines+=["        };","",
    "    public static IReadOnlyList<Part> ForExercise(int number) =>",
    "        Parts.TryGetValue(number, out var items) ? items : [];",
    "",
    "    public static int OriginalSourceExerciseCount => Parts.Count;",
    "    public static int OriginalAtomicReviewActions => Parts.Values.Sum(parts => parts.Count);",
    "    public static int ExplicitSourceSubparts =>",
    '        Parts.Values.SelectMany(p => p).Count(x => x.Label != "whole exercise");',
    "}",""]
target=root/"src/Edulytics.Web/Presentation/Grade12PrecalculusSection11SourcePartChecklist.cs"
target.write_text("\n".join(lines),encoding="utf-8",newline="\n")

viewpath=root/"src/Edulytics.Web/Views/Grade12PrecalculusRubricTeacher/Review.cshtml"
s=viewpath.read_text(encoding="utf-8")
anchor='''@if (Model.Submission.TeacherScore is int score)'''
assert s.count(anchor)==1
v='''@if (Grade12PrecalculusSection11SourcePartChecklist.ForExercise(
    Model.Submission.ExerciseNumber) is { Count: > 0 } sourceParts)
{
    <section class="student-card" aria-label="Original exercise completeness checklist">
        <h2>Source-aligned exercise checklist</h2>
        <p>Review each requested original mathematical action against the
           learner's complete first-party response. This checklist supports
           teacher judgment, not a separate automatic score or a claim
           that original publisher artwork is reproduced.</p>
        <ol>
            @foreach (var part in sourceParts)
            {
                <li>
                    <strong>@part.Label:</strong> @part.RequestedSkill
                    @if (part.OriginalSourceNumericalResultUnresolved)
                    {
                        <p><strong>Precision warning:</strong> The exact original
                           graphical result cannot be confirmed from the
                           printed image. Do not award or deduct marks based on
                           an invented source-specific numerical root.</p>
                    }
                </li>
            }
        </ol>
    </section>
}
'''
viewpath.write_text(s.replace(anchor,v+anchor),encoding="utf-8",newline="\n")

testpath=root/"tests/Edulytics.Tests/MathematicsIntelligence/Grade12PrecalculusSection11SourceContractsTests.cs"
test=testpath.read_text(encoding="utf-8")
anchor='''    [Fact]
    public void IndependentWorkedExamplesHaveCorrectMathGoldenValues()'''
assert test.count(anchor)==1
testadd='''    [Fact]
    public void Grade12OriginalExercisePartsRetain162AtomicReviewActionsWithoutDoubleCounting()
    {
        Assert.Equal(92, Grade12PrecalculusSection11SourcePartChecklist.OriginalSourceExerciseCount);
        Assert.Equal(162, Grade12PrecalculusSection11SourcePartChecklist.OriginalAtomicReviewActions);
        Assert.Equal(106, Grade12PrecalculusSection11SourcePartChecklist.ExplicitSourceSubparts);
        Assert.Equal(5, Grade12PrecalculusSection11SourcePartChecklist.ForExercise(27).Count);
        Assert.Equal(3, Grade12PrecalculusSection11SourcePartChecklist.ForExercise(39).Count);
        Assert.Equal(2, Grade12PrecalculusSection11SourcePartChecklist.ForExercise(52).Count);
        Assert.Single(Grade12PrecalculusSection11SourcePartChecklist.ForExercise(92));
        Assert.Empty(Grade12PrecalculusSection11SourcePartChecklist.ForExercise(93));
        Assert.Contains(Grade12PrecalculusSection11SourcePartChecklist.ForExercise(52),
            part => part.OriginalSourceNumericalResultUnresolved);
    }

'''
testpath.write_text(test.replace(anchor,testadd+anchor),encoding="utf-8",newline="\n")
auditpath=base/"u01-l01-original-exercise-atomic-teacher-review.md"
auditpath.write_text("""# U01:L01 Section 1.1 — original atomic exercise teacher checklist

Source: authorized local OpenStax Precalculus 2017, Section 1.1 exercises 1–92.
An original numbered exercise may contain several explicitly requested inputs,
questions, graph/range actions or contextual statements; **never count the
parent and its explicit children together**.

| Source exercise structure | Count |
|:--|--:|
| Numbered original parent exercises | 92 |
| Unpartitioned singleton tasks | 56 |
| Explicit original source subparts | 106 |
| Non-overlapping exercise-level atomic tasks | **162** |
| Original exact graph answer unresolved | Exercise 52(b), 1 subpart |
| First-party student practice parent prompts | 92 |
| Teacher-only source atomic checklist actions | **162** |

The teacher-review page now provides exact part labels and source-skill demands
for all 92 numbered source exercises, allowing a teacher to review completeness
when a student submits the original-skill first-party exercise prompt. Source
exercise 52(b) explicitly warns against inventing an exact root from an ambiguous
original printed graph. The first-party graph analogue has its own rule and
its numerical answer must not be substituted for a source-specific answer.

This is **not** a source-wide atomic denominator: worked examples, Try Its,
figures, tables and chapter shared resources require their own denominators.
It is also not an automatic 162-subpart grader, a claim of independent visual
parity or a Stage D test. Grade12 source A–B–C remains open.
""",encoding="utf-8",newline="\n")
assessmentpath=base/"u01-l01-assessment.json"
a=json.loads(assessmentpath.read_text(encoding="utf-8"))
a["FirstPartySectionExerciseBank"]["TeacherOriginalPartChecklistSource"]="src/Edulytics.Web/Presentation/Grade12PrecalculusSection11SourcePartChecklist.cs"
a["FirstPartySectionExerciseBank"]["SourceExerciseAtomicTeacherReviewChecklistCount"]=162
a["FirstPartySectionExerciseBank"]["OriginalExactGraph52BSourceAnswer"]="UNRESOLVED_NOT_FABRICATED"
a["FirstPartySectionExerciseBank"]["SourceExerciseAtomicChecklistStatus"]="AUTHORED_NOT_RUNTIME_TESTED"
assessmentpath.write_text(json.dumps(a,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
for filename in ["u01-l01-abc-checkpoint.json","full-grade-progress.json"]:
    path=base/filename
    j=json.loads(path.read_text(encoding="utf-8"))
    target=j if filename.startswith("u01") else j["LessonEntries"][0]
    target["SourceExerciseAtomicTeacherReviewChecklistCount"]=162
    target["OriginalExercise52BExactRoot"]="SOURCE_UNRESOLVED"
    target["ReadyForStageD"]=False
    target["SourceAtomicDenominatorVerified"]=False
    path.write_text(json.dumps(j,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("162_ATOMIC_EXERCISE_PARTS_TIED_TO_TEACHER_REVIEW_NO_SOURCE_52B_FABRICATION")
