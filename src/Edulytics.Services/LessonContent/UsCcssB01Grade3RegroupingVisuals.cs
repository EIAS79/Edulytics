using System.Globalization;
using Edulytics.Core.Curriculum;
using Edulytics.Services.Mathematics;

namespace Edulytics.Services.LessonContent;

/// <summary>
/// Grade 3 U03 source-exact three-digit written algorithms. Columns are
/// rebuilt from independently verified real question operands and their
/// signed mathematical relationship, not a title-based guessed diagram.
/// This is L4 repair evidence, NOT independent browser/academic approval.
/// </summary>
public static class UsCcssB01Grade3RegroupingVisuals
{
    private static readonly IReadOnlySet<string> LessonCodes = new HashSet<string>(
        new[] { "L03","L05","L07","L08","L09","L10","L11","L12" }
            .Select(code=>$"PED:US-CCSS-MATH:G3:U03:{code}"),
        StringComparer.Ordinal);

    private static RichLessonVisualRow Row(string label,int value) => new()
    {
        Label=label,
        Cells=[
            (value/100).ToString(CultureInfo.InvariantCulture),
            (value/10%10).ToString(CultureInfo.InvariantCulture),
            (value%10).ToString(CultureInfo.InvariantCulture)
        ]
    };

    public static IReadOnlyList<RichLessonVisual> Build(
        string lessonCode,IReadOnlyList<ExactSkillGeneratedQuestion> questions)
    {
        if(!LessonCodes.Contains(lessonCode)) return [];
        foreach(var q in questions)
        {
            if(!q.Family.StartsWith("usccss.b01.g3.",StringComparison.Ordinal) ||
               !ExactSkillContractQuestionEngine.Verify(
                   q.Family,q.Parameters,q.CorrectAnswer)) continue;
            var p=q.Parameters;
            if(!p.TryGetValue("left",out var left) ||
               !p.TryGetValue("right",out var right) ||
               !p.TryGetValue("operation",out var operation) ||
               !p.TryGetValue("expected",out var expected) ||
               operation is not (0 or 1) ||
               left<100 || left>999 || right<100 || right>999 ||
               expected<0 || expected>999 ||
               expected!=(operation==0?left+right:left-right) ||
               q.CorrectAnswer!=expected.ToString(CultureInfo.InvariantCulture))
                continue;
            var acrossZero = operation==1 && left%100==0 &&
                right%100!=0;
            var suffix=operation==0?" + ":" − ";
            return [new RichLessonVisual
            {
                Kind=RichLessonVisualKind.PlaceValueChart,
                Title="Read and regroup hundreds, tens and ones",
                PrimaryLabel="Hundreds | Tens | Ones",
                Description=
                    $"The values {left}{suffix}{right} = {expected} are exact. " +
                    (acrossZero
                        ? "Decompose one hundred into ten tens, then one ten " +
                          "into ten ones to subtract across the zero places. "
                        : operation==0
                            ? "Combine units of the same place value; if ten units " +
                              "are composed, exchange them for the next place. "
                            : "Subtract by place value, decomposing a larger " +
                              "unit only when the lower place needs it. ") +
                    "Verify by the inverse operation.",
                Rows=[
                    Row("First number",left),
                    Row(operation==0?"Addend":"Subtrahend",right),
                    Row(operation==0?"Sum":"Difference",expected)
                ]
            }];
        }
        return [];
    }
}
