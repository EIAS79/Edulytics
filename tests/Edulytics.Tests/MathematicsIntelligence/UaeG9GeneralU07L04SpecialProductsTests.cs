using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Stage D prepared acceptance tests for special square and conjugate formulas.</summary>
public sealed class UaeG9GeneralU07L04SpecialProductsTests
{
    private const string Square="uae.g9.general.u07.l04.square_binomial_coefficient";
    private const string Conjugate="uae.g9.general.u07.l04.conjugate_product_coefficient";
    private const string Lesson="PED:UAE-MOE-MATH:L9:GENERAL:07:04:SPECIAL-PRODUCTS";

    [Theory]
    [InlineData(true,2,"9","6")]
    [InlineData(true,1,"12","0")]
    [InlineData(true,0,"4","-4")]
    [InlineData(false,2,"9","6")]
    [InlineData(false,1,"0","12")]
    [InlineData(false,0,"-4","4")]
    public void IndependentVerifierChecksSquareAndConjugateCoefficients(bool square,int degree,string correct,string wrong)
    {
        var p=new Dictionary<string,int>{{"a",3},{"b",2},{"degree",degree}};
        var family=square?Square:Conjugate;
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,correct));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,wrong));
    }

    [Fact]
    public void ParameterIntegrityAndArabicMinusAreSupported()
    {
        var p=new Dictionary<string,int>{{"a",3},{"b",2},{"degree",0}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Conjugate,p,"−٤"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Conjugate,p,"٤"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Conjugate,
            new Dictionary<string,int>{{"a",3},{"degree",0}},"-4"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Conjugate,
            new Dictionary<string,int>{{"a",3},{"b",2},{"degree",3}},"-4"));
    }

    [Theory]
    [InlineData(Square)]
    [InlineData(Conjugate)]
    public void OriginalSpecialProductGeneratorMatchesIndependentVerifier(string family)
    {
        var engine=new ExactSkillContractQuestionEngine();
        for(int seed=1;seed<=24;seed++)
        {
            var q=Assert.Single(engine.Generate("UAE-G9-GENERAL",Lesson,
                new[]{family},ExactSkillQuestionDifficulty.Standard,1,seed,Array.Empty<string>()));
            Assert.True(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,q.CorrectAnswer));
            var wrong=(int.Parse(q.CorrectAnswer,System.Globalization.CultureInfo.InvariantCulture)+1)
                .ToString(System.Globalization.CultureInfo.InvariantCulture);
            Assert.False(ExactSkillContractQuestionEngine.Verify(q.Family,q.Parameters,wrong));
        }
    }
}
