using Edulytics.Services.Mathematics;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>IM 2021 G5 U04:L01 original source products and estimates; AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG5U04L01ProductsTests
{
    private static Dictionary<string,int> P(int c,int slot=0)=>
        new(StringComparer.Ordinal){["case"]=c,["slot"]=slot};
    [Theory]
    [InlineData(0,0,"1")]
    [InlineData(1,1,"2")]
    [InlineData(2,2,"3")]
    [InlineData(3,0,"1")]
    [InlineData(4,1,"2")]
    [InlineData(5,2,"3")]
    [InlineData(6,0,"1")]
    public void SevenOriginalProducts(int c,int slot,string expected)
    {
        var f=UsCcssG5U04WholeNumberSourceProducts.L01Product;
        Assert.Equal(expected,UsCcssG5U04WholeNumberSourceProducts.Solve(f,P(c,slot)));
        Assert.True(UsCcssG5U04WholeNumberSourceProducts.Verify(f,P(c,slot),expected));
        Assert.False(UsCcssG5U04WholeNumberSourceProducts.Verify(f,P(c,slot),"0"));
    }
    [Fact]
    public void OriginalFourEstimateOptionsAndInvalidParams()
    {
        var f=UsCcssG5U04WholeNumberSourceProducts.L01Estimate;
        Assert.Equal("3",UsCcssG5U04WholeNumberSourceProducts.Solve(f,P(0,2)));
        Assert.False(UsCcssG5U04WholeNumberSourceProducts.Verify(f,P(0,1),"2"));
        Assert.True(UsCcssG5U04WholeNumberSourceProducts.Verify(f,P(0,2),"3"));
        var original=Enumerable.Range(1,200)
            .Select(seed=>UsCcssG5U04WholeNumberSourceProducts.Build(f,new Random(seed)))
            .First(item=>item.Parameters["case"]==0);
        Assert.Contains("4: 1500",original.Prompt);
        Assert.Contains("TEACHER_REVIEW_REQUIRED",original.Prompt);
        var supplemental=Enumerable.Range(1,200)
            .Select(seed=>UsCcssG5U04WholeNumberSourceProducts.Build(f,new Random(seed)))
            .First(item=>item.Parameters["case"]>0);
        Assert.Contains("Edulytiks-original",supplemental.Prompt);
        Assert.Equal("3",UsCcssG5U04WholeNumberSourceProducts.Solve(f,supplemental.Parameters));
        Assert.True(UsCcssG5U04WholeNumberSourceProducts.Verify(f,supplemental.Parameters,"3"));
        foreach(var p in new[]{P(-1),P(13,2),P(0,-1),P(0,3)})
            Assert.False(UsCcssG5U04WholeNumberSourceProducts.Verify(f,p,"1"));
        var extra=P(0,2);extra["foreign"]=2;
        Assert.False(UsCcssG5U04WholeNumberSourceProducts.Verify(f,extra,"3"));
        Assert.False(UsCcssG5U04WholeNumberSourceProducts.Verify(f,null,"3"));
    }
}
