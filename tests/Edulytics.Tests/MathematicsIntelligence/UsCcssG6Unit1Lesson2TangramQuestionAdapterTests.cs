using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// G6 U01:L02 independent authoring fixtures only; NOT_RUN until Stage D.
/// This adapter is not a production-selected Practice family yet.
/// </summary>
public sealed class UsCcssG6Unit1Lesson2TangramQuestionAdapterTests
{
    private const string Area = UsCcssG6Unit1Lesson2TangramQuestions.PieceArea;
    private const string Equal = UsCcssG6Unit1Lesson2TangramQuestions.SameArea;

    [Theory]
    [InlineData(0, "0.5", "1/2")]
    [InlineData(1, "1", "2/2")]
    [InlineData(2, "2", "4/2")]
    [InlineData(3, "1", "1/1")]
    [InlineData(4, "2", "2/1")]
    [InlineData(5, "8", "16/2")]
    public void SourceKitPieceAreas_AcceptEquivalentExactAnswers(int variant,
        string expected, string fraction)
    {
        var p = new Dictionary<string,int> { ["variant"] = variant };
        Assert.Equal(expected, UsCcssG6Unit1Lesson2TangramQuestions.Solve(Area,p));
        Assert.True(UsCcssG6Unit1Lesson2TangramQuestions.Verify(Area,p,expected));
        Assert.True(UsCcssG6Unit1Lesson2TangramQuestions.Verify(Area,p,fraction));
        Assert.False(UsCcssG6Unit1Lesson2TangramQuestions.Verify(Area,p,"9"));
        Assert.False(UsCcssG6Unit1Lesson2TangramQuestions.Verify(Area,p,"0/0"));
    }

    [Theory]
    [InlineData(0, "1")]
    [InlineData(1, "1")]
    [InlineData(2, "0")]
    [InlineData(3, "0")]
    public void EqualArea_IsNotGeometricConstructionCredit(int variant, string expected)
    {
        var p = new Dictionary<string,int> { ["variant"] = variant };
        Assert.Equal(expected, UsCcssG6Unit1Lesson2TangramQuestions.Solve(Equal,p));
        Assert.True(UsCcssG6Unit1Lesson2TangramQuestions.Verify(Equal,p,expected));
        Assert.False(UsCcssG6Unit1Lesson2TangramQuestions.Verify(
            Equal,p,expected == "1" ? "0" : "1"));
        Assert.False(UsCcssG6Unit1Lesson2TangramQuestions.Verify(Equal,p,"1.0"));
    }

    [Fact]
    public void MissingForgedAndOutOfRangeParameters_FailClosed()
    {
        Assert.False(UsCcssG6Unit1Lesson2TangramQuestions.Verify(Area,null,"1"));
        Assert.False(UsCcssG6Unit1Lesson2TangramQuestions.Verify(
            Area,new Dictionary<string,int>(), "1"));
        Assert.False(UsCcssG6Unit1Lesson2TangramQuestions.Verify(
            Area,new Dictionary<string,int>{{"variant",6}}, "1"));
        Assert.False(UsCcssG6Unit1Lesson2TangramQuestions.Verify(
            Equal,new Dictionary<string,int>{{"variant",4}}, "1"));
        Assert.False(UsCcssG6Unit1Lesson2TangramQuestions.Verify(
            Area,new Dictionary<string,int>{{"variant",0},{"target",1}}, "0.5"));
        Assert.False(UsCcssG6Unit1Lesson2TangramQuestions.Verify(
            "usccss.g5.unit_cube_count", new Dictionary<string,int>{{"variant",0}}, "0.5"));
        Assert.False(UsCcssG6Unit1Lesson2TangramQuestions.Verify(
            Area,new Dictionary<string,int>{{"variant",0}}, " 0.5 "));
        Assert.False(UsCcssG6Unit1Lesson2TangramQuestions.Verify(
            Area,new Dictionary<string,int>{{"variant",0}}, "1/0"));
        Assert.False(UsCcssG6Unit1Lesson2TangramQuestions.Verify(
            Area,new Dictionary<string,int>{{"variant",0}}, "1/2/1"));
    }

    [Fact]
    public void FixedSeedConstruction_StaysWithinSourceInventory()
    {
        var rng = new Random(20261101);
        for (int i = 0; i < 40; i++)
        {
            foreach (string family in new[] { Area, Equal })
            {
                var question = UsCcssG6Unit1Lesson2TangramQuestions.Build(family,rng);
                Assert.Equal(family,question.Family);
                Assert.Contains("teacher checks constructed figures",question.Explanation);
                Assert.True(UsCcssG6Unit1Lesson2TangramQuestions.Verify(
                    family,question.Parameters,
                    UsCcssG6Unit1Lesson2TangramQuestions.Solve(family,question.Parameters)));
            }
        }
    }
}
