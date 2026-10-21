using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssG1Lesson87And88OriginalTaskTests
{
    [Theory]
    [InlineData(4,92,92,"TRUE=YES;LEFT=92;RIGHT=92")]
    [InlineData(5,92,92,"TRUE=NO;LEFT=92;RIGHT=92")]
    [InlineData(6,33,43,"TRUE=YES;LEFT=33;RIGHT=43")]
    public void Lesson87ThreeOriginalPlaceValueTruthStatementsAreStrict(
        int mode,int left,int right,string expected)
    {
        const string family="usccss.g1.u04.l22.center_place_value";
        var p=new Dictionary<string,int>
        {
            ["a"]=left,["b"]=right,["c"]=0,["mode"]=mode
        };
        Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Valid(family,p));
        Assert.Equal(expected,UsCcssG1FullGradeUnit4FinalQuestions.Solve(family,p));
        Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Verify(family,p,expected));
        Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Verify(family,p,
            expected.Replace("TRUE=YES","TRUE=NO").Replace("LEFT=92","LEFT=93")));
        Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Verify(family,p,expected+";SPURIOUS=1"));
        p["b"]++;
        Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Valid(family,p));
    }

    [Fact]
    public void Lesson88NumericFirstPartyProxiesRepresentEachSourceQuantityBand()
    {
        const string family="usccss.g1.u04.l23.estimate_and_count";
        foreach(var (mode,total,band) in new[]
        {
            (4,17,"LESS_THAN_20"),(5,20,"TWENTY_TO_FIFTY"),
            (5,50,"TWENTY_TO_FIFTY"),(6,51,"MORE_THAN_50"),
            (6,98,"MORE_THAN_50")
        })
        {
            var p=new Dictionary<string,int>
                {["a"]=total,["b"]=42,["c"]=0,["mode"]=mode};
            var answer=$"COUNT={total};CATEGORY={band};TENS={total/10};ONES={total%10}";
            Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Valid(family,p));
            Assert.Equal(answer,UsCcssG1FullGradeUnit4FinalQuestions.Solve(family,p));
            Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Verify(family,p,answer));
            Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Verify(family,p,answer+";EXTRA=1"));
            p["mode"]=mode==4?6:mode==5?4:5;
            Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Valid(family,p));
        }
    }

    [Fact]
    public void SourceMathAndFirstPartyQuantitiesAreSampledAcrossManySeeds()
    {
        foreach(var (family,maxMode) in new[]
        {
            ("usccss.g1.u04.l22.center_place_value",6),
            ("usccss.g1.u04.l23.estimate_and_count",6)
        })
        {
            var counts=new int[maxMode+1];
            for(var seed=0;seed<1200;seed++)
            {
                var q=UsCcssG1FullGradeUnit4FinalQuestions.Build(family,new Random(53*seed+7));
                counts[q.Parameters["mode"]]++;
                Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Valid(family,q.Parameters));
                Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Verify(
                    family,q.Parameters,UsCcssG1FullGradeUnit4FinalQuestions.Solve(family,q.Parameters)));
                if(q.Parameters["mode"]>=4&&family.EndsWith("estimate_and_count"))
                    Assert.Contains("NOT THE PUBLISHER PICTURE",q.Prompt);
            }
            foreach(var mode in new[]{4,5,6})Assert.True(counts[mode]>70);
        }
    }
}
