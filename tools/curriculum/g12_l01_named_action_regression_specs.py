from pathlib import Path
import json
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
test=root/"tests/Edulytics.Tests/MathematicsIntelligence/Grade12PrecalculusSection11SourceContractsTests.cs"
content=test.read_text(encoding="utf-8")
anchor='''    [Fact]
    public void IndependentWorkedExamplesHaveCorrectMathGoldenValues()'''
assert content.count(anchor)==1
addition='''    [Fact]
    public void SourceExplicitMultiPartExamplesArePreservedInFirstPartyTeaching()
    {
        var lesson = RichLessonContentV2Registry.Find(Code, "en");
        Assert.NotNull(lesson);
        var tableCase = lesson.WorkedExamples[4];
        Assert.Contains("Table A", tableCase.Question);
        Assert.Contains("Table B", tableCase.Question);
        Assert.Contains("Table C", tableCase.Question);
        Assert.Contains("Table C: not a function", tableCase.Answer);
        var substitution = lesson.WorkedExamples[5];
        Assert.Contains("f(2), f(a), f(a+h)", substitution.Question);
        Assert.Contains("f(a)=a²+3a−4", substitution.Answer);
        Assert.Contains("quotient=2a+h+3", substitution.Answer);
        var vertical = lesson.WorkedExamples[13];
        Assert.Contains("(A) y=x²", vertical.Question);
        Assert.Contains("(B) y=2x+1", vertical.Question);
        Assert.Contains("(C) y²=x", vertical.Question);
        Assert.Contains("Graphs A and B are functions", vertical.Answer);
    }

'''
test.write_text(content.replace(anchor,addition+anchor),encoding="utf-8",newline="\n")
audit=base/"u01-l01-256-named-source-atomic-actions.json"
j=json.loads(audit.read_text(encoding="utf-8"))
assert j["TotalNamedSourceLearningAndPresentationLeafActions"]==256
assert len(j["Atoms"])==256
extra=base/"u01-l01-named-source-action-denominator-derivation.md"
extra.write_text("""# Precalculus Section 1.1 — named source action denominator

Source is the authorized local OpenStax Precalculus 2017 PDF, Section 1.1.
The prior 170 locator anchors are not themselves the atomic action count.

| Source item category | Original named parents | Nonoverlapping child actions |
|:--|--:|--:|
| Numbered original Section exercises | 92 | 162 |
| Original worked examples | 15 | 28 |
| Original Try Its | 12 | 15 |
| Other anchors (objectives, figures, tables, how-tos, analysis, Q&A, media) | 51 | 51 |
| **Totals** | **170** | **256** |

The child-action additions come from explicitly requested original
questions, not simply from every teacher solution step. For instance,
Example 1 has two questions, Example 5 classifies three tables, Example
6 asks for four separate substitutions/quotients, Example 11 and 12
each ask for two values, Example 14 classifies three graphs, Try It 9
asks three relationship questions and Try It 10 asks two grade questions.

Three genuine first-party example omissions were repaired: Example 5
now classifies all three table behaviors, Example 6 explicitly includes
f(a) before its difference quotient, and Example 14 compares three
vertical-line cases. The existing original-skill practice, 92 Section
student tasks and typed teacher rubric have not been replaced.

This establishes **256 named exercise/teaching action candidates**, not
full publisher-content atom coverage: source table cells, plot points,
caption details and all source-equivalent figure semantics still require
independent final coverage checks. Original source graph 52(b) has no
provable exact root from the bounded printed image. Do not mark
ready for stage D or advance to U01:L02 on this ledger alone.

No xUnit suite, CI, application E2E, database migration, Stage D or
publishing was executed.
""",encoding="utf-8",newline="\n")
print("AUTHORING_256_ACTIONS_DOCUMENTED_AND_MULTIPART_EXAMPLE_TESTS_WRITTEN_NOT_RUN")
