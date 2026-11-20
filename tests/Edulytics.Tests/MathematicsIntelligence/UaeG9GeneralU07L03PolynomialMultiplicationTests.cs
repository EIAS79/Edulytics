using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade 9 General 7-3 source-skill polynomial convolution. Tests prepared for Stage D.</summary>
public sealed class UaeG9GeneralU07L03PolynomialMultiplicationTests
{
    private const string Family="uae.g9.general.u07.l03.multiply_coefficient";
    private const string Lesson="PED:UAE-MOE-MATH:L9:GENERAL:07:03:POLYNOMIAL-MULTIPLICATION";

    [Theory]
    [InlineData(4,"2","3")]
    [InlineData(3,"7","2")]
    [InlineData(2,"14","7")]
    [InlineData(1,"11","-3")]
    [InlineData(0,"-10","10")]
    public void IndependentVerifierConvolvesAllTermPairs(int degree,string correct,string incorrect)
    {
        // (x²+2x+5)(2x²+3x−2) = 2x⁴+7x³+14x²+11x−10
        var p=new Dictionary<string,int>
        {
            ["a2"]=1,["a1"]=2,["a0"]=5,
            ["b2"]=2,["b1"]=3,["b0"]=-2,
            ["degree"]=degree
        };
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Family));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family,p,correct));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,incorrect));
    }

    [Fact]
    public void TamperedParametersAndIncorrectArabicNumeralsAreRejected()
    {
        var p=new Dictionary<string,int>
        {
            ["a2"]=1,["a1"]=2,["a0"]=5,
            ["b2"]=2,["b1"]=3,["b0"]=-2,
            ["degree"]=0
        };
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family,p,"−١٠"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,"١٠"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,
            new Dictionary<string,int>(p){["degree"]=5},"-10"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,
            new Dictionary<string,int>{{"degree",0}},"-10"));
    }

    [Fact]
    public void SourceGeneratorAndIndependentVerifierAgree()
    {
        var engine=new ExactSkillContractQuestionEngine();
        for(int seed=1;seed<=32;seed++)
        {
            var q=Assert.Single(engine.Generate("UAE-G9-GENERAL",Lesson,
                new[]{Family},ExactSkillQuestionDifficulty.Standard,1,seed,Array.Empty<string>()));
            Assert.Contains("اضرب كثيرتي الحدود",q.Prompt);
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            var wrong=(int.Parse(q.CorrectAnswer,System.Globalization.CultureInfo.InvariantCulture)+1)
                .ToString(System.Globalization.CultureInfo.InvariantCulture);
            Assert.False(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,wrong));
        }
    }
}
