using System.Globalization;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UaeG11UnifiedExpressionReuseAndExtensionTests
{
    private const string Scope = "PED:UAE-MOE-MATH:L11:UNIFIED:01:01";
    private const string Signed = "uae.g11.unified.u01.l01.signed_substitution";
    private const string Decimal = "uae.g11.unified.u01.l01.decimal_substitution";
    private const string Rational = "uae.g11.unified.u01.l01.rational_substitution";

    [Theory]
    [InlineData(Signed)]
    [InlineData(Decimal)]
    [InlineData(Rational)]
    public void ExistingExactEngineGeneratesAndVerifiesOriginalG11Expressions(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        var engine = new ExactSkillContractQuestionEngine();
        foreach (int seed in new[] { 20261128, 20261129, 20261130 })
        foreach (var item in engine.Generate(Scope, Scope, [family],
                 ExactSkillQuestionDifficulty.Standard, 12, seed, []))
        {
            var p = item.Parameters;
            int a = p["a"], b = p["b"], c = p["c"];
            Assert.Contains(item.Prompt, ch => ch is >= '\u0600' and <= '\u06ff');
            decimal n, d;
            var pieces = item.CorrectAnswer.Split('/');
            n = decimal.Parse(pieces[0], CultureInfo.InvariantCulture);
            d = pieces.Length == 2 ? decimal.Parse(pieces[1], CultureInfo.InvariantCulture) : 1m;
            Assert.NotEqual(0m, d);
            if (family == Signed) Assert.Equal((decimal)a + (decimal)Math.Pow(b-c, 2), n/d);
            else if (family == Decimal) Assert.Equal((a + b*b*(c-a))/10m, n/d);
            else Assert.Equal((a*a-2*b)*d, n*(c-a));
            Assert.True(ExactSkillContractQuestionEngine.Verify(family, p, item.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family, p, "not a number"));
            string wrong = ((n+d)/d).ToString(CultureInfo.InvariantCulture);
            Assert.False(ExactSkillContractQuestionEngine.Verify(family, p, wrong));
            string arabic = string.Concat(item.CorrectAnswer.Select(ch => ch is >= '0' and <= '9'
                ? (char)('٠' + ch - '0') : ch == '.' ? '٫' : ch == '-' ? '−' : ch));
            Assert.True(ExactSkillContractQuestionEngine.Verify(family, p, arabic));
        }
    }

    [Theory]
    [InlineData(Signed, Edulytics.Core.Mathematics.Practice.LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData(Decimal, Edulytics.Core.Mathematics.Practice.LessonPracticeInteractionKind.NumericEntry)]
    [InlineData(Rational, Edulytics.Core.Mathematics.Practice.LessonPracticeInteractionKind.FractionEntry)]
    public void ExactAnswerTypeSelectsMatchingInteraction(string family, Edulytics.Core.Mathematics.Practice.LessonPracticeInteractionKind kind)
    {
        Assert.True(Edulytics.Core.Mathematics.Practice.LessonPracticeInteractionRegistry.TryResolve(family, out var interaction));
        Assert.Equal(kind, interaction!.InteractionKind);
    }

    [Fact]
    public void BoundaryAndInvalidParametersFailClosed()
    {
        var signed = new Dictionary<string, int> { ["a"]=-3, ["b"]=-2, ["c"]=4 };
        Assert.True(ExactSkillContractQuestionEngine.Verify(Signed, signed, "33"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Signed, signed, "-39"));
        var dec = new Dictionary<string, int> { ["a"]=15, ["b"]=-2, ["c"]=-20 };
        Assert.True(ExactSkillContractQuestionEngine.Verify(Decimal, dec, "-12.5"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Decimal, dec, "−١٢٫٥"));
        var rational = new Dictionary<string, int> { ["a"]=-2, ["b"]=3, ["c"]=3 };
        Assert.True(ExactSkillContractQuestionEngine.Verify(Rational, rational, "-2/5"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Rational, rational, "-4/10"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Rational, rational, "-0.4"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Rational, rational, "-2/0"));
        var third = new Dictionary<string, int> { ["a"]=1, ["b"]=0, ["c"]=4 };
        Assert.True(ExactSkillContractQuestionEngine.Verify(Rational, third, "2/6"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Rational, third, "0.33333333"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Rational, third, "0.333333333333333333333333333333333333"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Signed, signed, "33.000000000000000000000000000000001"));
        rational["c"]=-2;
        Assert.False(ExactSkillContractQuestionEngine.Verify(Rational, rational, "0"));
        signed["b"]=10;
        Assert.False(ExactSkillContractQuestionEngine.Verify(Signed, signed, "33"));
    }
}
