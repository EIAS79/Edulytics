using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Original, accessible IM 2021 G1 U04 L12 teacher Student Facing mental number talk.
/// Written during A-C authoring only. Execution and acceptance remain deferred to Stage D.
/// </summary>
public sealed class UsCcssG1Lesson77Original2021WarmupTests
{
    [Fact]
    public void AllFourOriginalMentalExpressionsHaveExactAndNegativeOracles()
    {
        const string family=UsCcssG1FullGradeTwoDigitOperationsQuestions.TenMoreLess;
        foreach(var (mode,a,b,answer,wrong) in new[]
        {
            (11,3,10,"RESULT=13","RESULT=12"),
            (12,10,5,"RESULT=15","RESULT=5"),
            (13,13,10,"RESULT=3","RESULT=23"),
            (14,15,10,"RESULT=5","RESULT=25")
        })
        {
            var p=new Dictionary<string,int>
                {["a"]=a,["b"]=b,["mode"]=mode,["unused"]=0};
            Assert.True(UsCcssG1FullGradeTwoDigitOperationsQuestions.Valid(family,p));
            Assert.Equal(answer,UsCcssG1FullGradeTwoDigitOperationsQuestions.Solve(family,p));
            Assert.True(UsCcssG1FullGradeTwoDigitOperationsQuestions.Verify(family,p,answer));
            Assert.False(UsCcssG1FullGradeTwoDigitOperationsQuestions.Verify(family,p,wrong));
            Assert.False(UsCcssG1FullGradeTwoDigitOperationsQuestions.Verify(family,p,answer+";EXTRA=0"));
            Assert.False(UsCcssG1FullGradeTwoDigitOperationsQuestions.Verify(family,p,""));
            p["a"]=a+1;
            Assert.False(UsCcssG1FullGradeTwoDigitOperationsQuestions.Valid(family,p));
            p["a"]=a;
            p["b"]=b+1;
            Assert.False(UsCcssG1FullGradeTwoDigitOperationsQuestions.Valid(family,p));
        }
    }

    [Fact]
    public void GeneratorExposesAllFour2021WarmupsWhilePreservingOlderModes()
    {
        const string family=UsCcssG1FullGradeTwoDigitOperationsQuestions.TenMoreLess;
        var items=Enumerable.Range(0,1200)
            .Select(i=>UsCcssG1FullGradeTwoDigitOperationsQuestions.Build(
                family,new Random(i*31+11))).ToArray();
        foreach(int mode in new[]{11,12,13,14})
        {
            var matches=items.Where(x=>x.Parameters["mode"]==mode).ToArray();
            Assert.NotEmpty(matches);
            Assert.All(matches,x=>
            {
                Assert.Contains("ORIGINAL 2021 MENTAL WARM-UP",x.Prompt);
                var expected=UsCcssG1FullGradeTwoDigitOperationsQuestions.Solve(family,x.Parameters);
                Assert.True(UsCcssG1FullGradeTwoDigitOperationsQuestions.Verify(family,x.Parameters,expected));
                Assert.False(UsCcssG1FullGradeTwoDigitOperationsQuestions.Verify(family,x.Parameters,"RESULT=-1"));
            });
        }
        foreach(int historicalMode in Enumerable.Range(0,11))
            Assert.Contains(items,x=>x.Parameters["mode"]==historicalMode);
    }

    [Fact]
    public void WarmupModesNeverValidateForDifferentLessonFamilyOrUnexpectedShape()
    {
        var p=new Dictionary<string,int>
            {["a"]=3,["b"]=10,["mode"]=11,["unused"]=0};
        Assert.False(UsCcssG1FullGradeTwoDigitOperationsQuestions.Valid(
            UsCcssG1FullGradeTwoDigitOperationsQuestions.CompareWords,p));
        p["unexpected"]=1;
        Assert.False(UsCcssG1FullGradeTwoDigitOperationsQuestions.Valid(
            UsCcssG1FullGradeTwoDigitOperationsQuestions.TenMoreLess,p));
    }
}
