using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Source-derived Grade 9 General polynomial coefficient checks.
/// Prepared in Stage A–B–C; Stage D will execute these tests.
/// Full polynomial expressions and algebra-tile reasoning require teacher review.
/// </summary>
public sealed class UaeG9GeneralU07L01PolynomialTests
{
    private const string Prefix="uae.g9.general.u07.l01.";
    private const string Lesson="PED:UAE-MOE-MATH:L9:GENERAL:07:01:POLYNOMIAL-ADD-SUBTRACT";

    private static readonly Dictionary<string,int> Fixture=new()
    {
        ["a2"]=5,["a1"]=-3,["a0"]=2,
        ["b2"]=2,["b1"]=4,["b0"]=-7,["degree"]=2
    };

    [Theory]
    [InlineData("add_coefficient", 2, "7", "3")]
    [InlineData("add_coefficient", 1, "1", "-7")]
    [InlineData("add_coefficient", 0, "-5", "9")]
    [InlineData("subtract_coefficient", 2, "3", "7")]
    [InlineData("subtract_coefficient", 1, "-7", "1")]
    [InlineData("subtract_coefficient", 0, "9", "-5")]
    public void IndependentVerifierChecksLikeTermCoefficients(string suffix,int degree,string good,string bad)
    {
        var p=new Dictionary<string,int>(Fixture) { ["degree"]=degree };
        var family=Prefix+suffix;
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,good));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,bad));
    }

    [Fact]
    public void ArabicDigitsAreAcceptedButTamperedParametersAreRejected()
    {
        Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"subtract_coefficient",Fixture,"٣"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Prefix+"subtract_coefficient",Fixture,"٤"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Prefix+"subtract_coefficient",
            new Dictionary<string,int>{{"a2",5},{"b2",2},{"degree",2}},"3"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Prefix+"subtract_coefficient",
            new Dictionary<string,int>(Fixture) { ["degree"]=3 },"3"));
    }

    [Theory]
    [InlineData("add_coefficient")]
    [InlineData("subtract_coefficient")]
    public void GeneratorAndIndependentVerifierAgreeForRepeatedSourceCases(string suffix)
    {
        var engine=new ExactSkillContractQuestionEngine();
        for(var seed=1;seed<=32;seed++)
        {
            var q=Assert.Single(engine.Generate("UAE-G9-GENERAL",Lesson,
                new[]{Prefix+suffix},ExactSkillQuestionDifficulty.Standard,
                1,seed,Array.Empty<string>()));
            Assert.Contains("كثيرتي الحدود",q.Prompt);
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            var invalid=(int.Parse(q.CorrectAnswer,System.Globalization.CultureInfo.InvariantCulture)+1)
                .ToString(System.Globalization.CultureInfo.InvariantCulture);
            Assert.False(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,invalid));
        }
    }
}
