using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Authored first-party formative assessment, NOT the protected original IM 2021 Cool-down.
/// Stage D has not been run; these assertions are evidence of authoring only.
/// </summary>
public sealed class UsCcssG1Lesson80IndependentComparisonExitTests
{
    [Fact]
    public void FirstPartyExitGradesTheSignAndEachPlaceValueDigit()
    {
        const string f=UsCcssG1FullGradeTwoDigitOperationsQuestions.CompareSymbols;
        var p=new Dictionary<string,int>
            {["a"]=68,["b"]=86,["mode"]=13,["unused"]=0};
        const string expected="SYMBOL=<;TENS_LEFT=6;TENS_RIGHT=8";
        Assert.True(UsCcssG1FullGradeTwoDigitOperationsQuestions.Valid(f,p));
        Assert.Equal(expected,UsCcssG1FullGradeTwoDigitOperationsQuestions.Solve(f,p));
        Assert.True(UsCcssG1FullGradeTwoDigitOperationsQuestions.Verify(f,p,expected));
        Assert.False(UsCcssG1FullGradeTwoDigitOperationsQuestions.Verify(
            f,p,"SYMBOL=>;TENS_LEFT=6;TENS_RIGHT=8"));
        Assert.False(UsCcssG1FullGradeTwoDigitOperationsQuestions.Verify(
            f,p,"SYMBOL=<;TENS_LEFT=8;TENS_RIGHT=6"));
        Assert.False(UsCcssG1FullGradeTwoDigitOperationsQuestions.Verify(
            f,p,"SYMBOL=<;TENS_LEFT=6"));
        Assert.False(UsCcssG1FullGradeTwoDigitOperationsQuestions.Verify(
            f,p,expected+";EXTRA=1"));
        p["b"]=85;
        Assert.False(UsCcssG1FullGradeTwoDigitOperationsQuestions.Valid(f,p));
    }

    [Fact]
    public void OriginalComparisonModesAndIndependentExitAllRemainGenerated()
    {
        const string f=UsCcssG1FullGradeTwoDigitOperationsQuestions.CompareSymbols;
        var generated=Enumerable.Range(0,1400)
            .Select(seed=>UsCcssG1FullGradeTwoDigitOperationsQuestions.Build(
                f,new Random(seed*29+13))).ToArray();
        foreach(int mode in Enumerable.Range(0,14))
            Assert.Contains(generated,q=>q.Parameters["mode"]==mode);
        var formative=generated.Where(q=>q.Parameters["mode"]==13).ToArray();
        Assert.NotEmpty(formative);
        Assert.All(formative,q=>
        {
            Assert.Contains("INDEPENDENT EDULYTICS FORMATIVE EXIT",q.Prompt);
            Assert.Contains("not the inaccessible original",q.Prompt);
            Assert.Equal("SYMBOL=<;TENS_LEFT=6;TENS_RIGHT=8",
                UsCcssG1FullGradeTwoDigitOperationsQuestions.Solve(f,q.Parameters));
            Assert.True(UsCcssG1FullGradeTwoDigitOperationsQuestions.Verify(f,q.Parameters,
                "SYMBOL=<;TENS_LEFT=6;TENS_RIGHT=8"));
            Assert.False(UsCcssG1FullGradeTwoDigitOperationsQuestions.Verify(f,q.Parameters,
                "SYMBOL=>;TENS_LEFT=6;TENS_RIGHT=8"));
        });
    }

    [Fact]
    public void IndependentExitCannotLeakIntoDifferentFamily()
    {
        var p=new Dictionary<string,int>
            {["a"]=68,["b"]=86,["mode"]=13,["unused"]=0};
        Assert.False(UsCcssG1FullGradeTwoDigitOperationsQuestions.Valid(
            UsCcssG1FullGradeTwoDigitOperationsQuestions.CompareWords,p));
    }
}
