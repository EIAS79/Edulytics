using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssG1Unit6ExactWarmupBraceletTests
{
    [Theory]
    [InlineData(5,24,54,"TRUE=NO;ACTUAL=54")]
    [InlineData(6,24,64,"TRUE=YES;ACTUAL=64")]
    [InlineData(7,30,75,"TRUE=YES;ACTUAL=75")]
    [InlineData(8,32,77,"TRUE=NO;ACTUAL=77")]
    public void Lesson112AllFourOriginalMentalTruthStatementsAreCorrect(
        int mode,int lhsBase,int computed,string answer)
    {
        const string family="usccss.g1.u06.l10.center_measure_and_add";
        var p=new Dictionary<string,int>{{"a",lhsBase},{"b",computed},{"c",0},{"mode",mode}};
        Assert.True(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(family,p));
        Assert.Equal(answer,UsCcssG1FullGradeUnit6MeasurementQuestions.Solve(family,p));
        Assert.True(UsCcssG1FullGradeUnit6MeasurementQuestions.Verify(family,p,answer));
        Assert.False(UsCcssG1FullGradeUnit6MeasurementQuestions.Verify(family,p,"TRUE=YES;ACTUAL=999"));
        p["b"]++;
        Assert.False(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(family,p));
    }

    [Theory]
    [InlineData(4,14,4,"LENGTH=10;DIFFERENCE=4")]
    [InlineData(5,10,4,"LENGTH=14;DIFFERENCE=4")]
    public void Lesson114BothSourceBraceletUnknownsAreRepresented(
        int mode,int known,int difference,string correct)
    {
        const string family="usccss.g1.u06.l12.compare_measurements";
        var p=new Dictionary<string,int>{{"a",known},{"b",difference},{"c",0},{"mode",mode}};
        Assert.True(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(family,p));
        Assert.Equal(correct,UsCcssG1FullGradeUnit6MeasurementQuestions.Solve(family,p));
        Assert.True(UsCcssG1FullGradeUnit6MeasurementQuestions.Verify(family,p,correct));
        Assert.False(UsCcssG1FullGradeUnit6MeasurementQuestions.Verify(family,p,"LENGTH=4;DIFFERENCE=4"));
        p["a"]++;
        Assert.False(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(family,p));
    }

    [Theory]
    [InlineData(6,65,20,"RESULT=85")]
    [InlineData(7,85,1,"RESULT=86")]
    [InlineData(8,65,21,"RESULT=86")]
    [InlineData(9,65,21,"RESULT=86")]
    public void Lesson118AllFourOriginalTensExpressionResultsAreRequired(
        int mode,int a,int b,string answer)
    {
        const string family="usccss.g1.u06.l16.center_mixed_math";
        var p=new Dictionary<string,int>{{"a",a},{"b",b},{"c",0},{"mode",mode}};
        Assert.True(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(family,p));
        Assert.Equal(answer,UsCcssG1FullGradeUnit6MeasurementQuestions.Solve(family,p));
        Assert.True(UsCcssG1FullGradeUnit6MeasurementQuestions.Verify(family,p,answer));
        Assert.False(UsCcssG1FullGradeUnit6MeasurementQuestions.Verify(family,p,"RESULT=87"));
        p["a"]++;
        Assert.False(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(family,p));
    }

    [Fact]
    public void SourceMathVariantsAreReachableAcrossDistinctGeneratorSeeds()
    {
        foreach(var (family,modes) in new[]
        {
            ("usccss.g1.u06.l10.center_measure_and_add",new[]{5,6,7,8}),
            ("usccss.g1.u06.l12.compare_measurements",new[]{4,5}),
            ("usccss.g1.u06.l16.center_mixed_math",new[]{6,7,8,9})
        })
        {
            var reached=new HashSet<int>();
            for(int seed=0;seed<600;seed++)
            {
                var q=UsCcssG1FullGradeUnit6MeasurementQuestions.Build(
                    family,new Random(seed*31+27));
                reached.Add(q.Parameters["mode"]);
                Assert.True(UsCcssG1FullGradeUnit6MeasurementQuestions.Verify(
                    family,q.Parameters,
                    UsCcssG1FullGradeUnit6MeasurementQuestions.Solve(family,q.Parameters)));
            }
            Assert.All(modes,m=>Assert.Contains(m,reached));
        }
    }
}
