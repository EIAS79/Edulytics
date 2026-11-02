using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Original 2021 U05 L03 visible student source uses Lin and Han's two
/// mathematically valid methods for 32+54. Tests authored, not executed.
/// </summary>
public sealed class UsCcssG1Lesson91OriginalLinHanEquationTests
{
    private const string Family="usccss.g1.u05.l03.write_no_compose_equations";

    [Fact]
    public void BothLinAndHanStrategiesRequireCorrectStepsAndTotal()
    {
        var p=new Dictionary<string,int>{{"a",32},{"b",54},{"mode",10}};
        const string correct="LIN_TENS=80;LIN_ONES=6;HAN_FIRST=84;HAN_SECOND=86;TOTAL=86";
        Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(Family,p));
        Assert.Equal(correct,UsCcssG1FullGradeUnit5AdditionQuestions.Solve(Family,p));
        Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(Family,p,correct));
        foreach(var incorrect in new[]
        {
            "LIN_TENS=30;LIN_ONES=6;HAN_FIRST=84;HAN_SECOND=86;TOTAL=86",
            "LIN_TENS=80;LIN_ONES=7;HAN_FIRST=84;HAN_SECOND=86;TOTAL=86",
            "LIN_TENS=80;LIN_ONES=6;HAN_FIRST=86;HAN_SECOND=86;TOTAL=86",
            "LIN_TENS=80;LIN_ONES=6;HAN_FIRST=84;HAN_SECOND=84;TOTAL=86",
            "LIN_TENS=80;LIN_ONES=6;HAN_FIRST=84;HAN_SECOND=86;TOTAL=96",
            "LIN_TENS=80;LIN_ONES=6;TOTAL=86",
            correct+";EXTRA=0"
        })
            Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(Family,p,incorrect));
        p["a"]=33;
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(Family,p));
        p["a"]=32;
        p["b"]=55;
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(Family,p));
        p["b"]=54;
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(
            "usccss.g1.u05.l02.explain_no_compose_equations",p));
    }

    [Fact]
    public void Lesson91StillEmitsAllOriginalEquationsAndTwoMethods()
    {
        var generated=Enumerable.Range(0,1600)
            .Select(i=>UsCcssG1FullGradeUnit5AdditionQuestions.Build(
                Family,new Random(i*43+7))).ToArray();
        foreach(int mode in Enumerable.Range(0,11))
            Assert.Contains(generated,q=>q.Parameters["mode"]==mode);
        Assert.All(generated.Where(q=>q.Parameters["mode"]==10),q=>
        {
            Assert.Contains("EDULYTICS FIRST-PARTY METHOD WRITING",q.Prompt);
            Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(
                Family,q.Parameters,UsCcssG1FullGradeUnit5AdditionQuestions.Solve(Family,q.Parameters)));
            Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(
                Family,q.Parameters,"LIN_TENS=80;LIN_ONES=6;HAN_FIRST=84;HAN_SECOND=86;TOTAL=85"));
        });
    }
}
