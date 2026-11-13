using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Source: Grade 5 local Arabic 02.pdf, printed 687–692. Stage D not run.
public sealed class UaeGrade5Unit09Lesson08ScalingTests
{
    private const string Family="uae.g5.u09.l08.classify_scaling";
    [Theory]
    [InlineData(2,5,8,"2")]
    [InlineData(2,7,4,"1")]
    [InlineData(6,5,5,"3")]
    public void ScalingClassifiesWithoutComputingProduct(int whole,int num,int den,string expected)
    {
        var p=new Dictionary<string,int>{{"whole",whole},{"num",num},{"den",den}};
        Assert.True(UaeGrade4Unit9FractionMultiplicationQuestions.Supports(Family));
        Assert.Equal(expected,UaeGrade4Unit9FractionMultiplicationQuestions.Solve(Family,p));
        Assert.True(UaeGrade4Unit9FractionMultiplicationQuestions.Verify(Family,p,expected));
        Assert.False(UaeGrade4Unit9FractionMultiplicationQuestions.Verify(Family,p,"9"));
        p["den"]=0;
        Assert.False(UaeGrade4Unit9FractionMultiplicationQuestions.Verify(Family,p,expected));
    }
}
