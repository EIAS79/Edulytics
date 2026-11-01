using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Tests.Acceptance;

/// <summary>
/// Lesson-local L02 authored exact Practice checks. AUTHORED_NOT_RUN.
/// No shared engine registration or teacher rounding policy is implied.
/// </summary>
public sealed class UsCcssAlg2U01L02PracticeAdapterTests
{
    private static readonly string[] Families =
    [
        UsCcssAlg2U01L02PracticeAdapter.GeometricFamily,
        UsCcssAlg2U01L02PracticeAdapter.PaperFamily,
        UsCcssAlg2U01L02PracticeAdapter.SierpinskiFamily
    ];

    [Theory]
    [InlineData("usccss.alg2.u01.l02.geometric_fill_ratio", 0, 6, "64")]
    [InlineData("usccss.alg2.u01.l02.geometric_fill_ratio", 1, 4, "1/16")]
    [InlineData("usccss.alg2.u01.l02.geometric_fill_ratio", 2, 3, "27")]
    [InlineData("usccss.alg2.u01.l02.geometric_fill_ratio", 3, 2, "9/16")]
    [InlineData("usccss.alg2.u01.l02.paper_conservation", 0, 6, "64")]
    [InlineData("usccss.alg2.u01.l02.paper_conservation", 1, 6, "5/4")]
    [InlineData("usccss.alg2.u01.l02.sierpinski_shaded", 0, 5, "243")]
    [InlineData("usccss.alg2.u01.l02.sierpinski_shaded", 1, 5, "1/4")]
    public void SourceAlignedNumericCandidate_HasExactPrivateAnswer(
        string family, int variant, int n, string answer)
    {
        Assert.True(UsCcssAlg2U01L02PracticeAdapter.TryPrepareExactNumericQuestion(
            family, variant, n, out var prompt, out var privateAnswer, out var parameters));
        Assert.Equal(answer, privateAnswer);
        Assert.Equal(n, parameters["n"]);
        Assert.Equal(variant, parameters["variant"]);
        Assert.False(string.IsNullOrWhiteSpace(prompt));
        Assert.DoesNotContain(privateAnswer, prompt, StringComparison.Ordinal);
        Assert.True(UsCcssAlg2U01L02PracticeAdapter.VerifyExactNumericQuestion(
            family, parameters, answer));
        Assert.False(UsCcssAlg2U01L02PracticeAdapter.VerifyExactNumericQuestion(
            family, parameters, "not-mathematics"));
    }

    [Theory]
    [InlineData("usccss.alg2.u01.l02.geometric_fill_ratio", 1, 4, "0.0625")]
    [InlineData("usccss.alg2.u01.l02.geometric_fill_ratio", 3, 2, "0.5625")]
    [InlineData("usccss.alg2.u01.l02.paper_conservation", 1, 6, "1.25")]
    [InlineData("usccss.alg2.u01.l02.sierpinski_shaded", 1, 5, "0.25")]
    public void EquivalentDecimalOrFraction_CreditedExactly(
        string family, int variant, int n, string equivalent)
    {
        var parameters = new Dictionary<string, int>
        {
            ["n"] = n,
            ["variant"] = variant
        };
        Assert.True(UsCcssAlg2U01L02PracticeAdapter.VerifyExactNumericQuestion(
            family, parameters, equivalent));
    }

    [Theory]
    [InlineData("usccss.alg2.u01.l02.geometric_fill_ratio", 1, 0)]
    [InlineData("usccss.alg2.u01.l02.geometric_fill_ratio", 4, 2)]
    [InlineData("usccss.alg2.u01.l02.paper_conservation", 0, -1)]
    [InlineData("usccss.alg2.u01.l02.paper_conservation", 1, 7)]
    [InlineData("usccss.alg2.u01.l02.sierpinski_shaded", 0, 6)]
    [InlineData("usccss.alg2.u01.l02.sierpinski_shaded", 2, 2)]
    [InlineData("usccss.alg2.u01.l02.monthly_balance", 0, 3)]
    public void OutOfDomainOrUnapprovedPolicy_FailsClosed(string family, int variant, int n)
    {
        Assert.False(UsCcssAlg2U01L02PracticeAdapter.TryPrepareExactNumericQuestion(
            family, variant, n, out _, out _, out _));
        Assert.False(UsCcssAlg2U01L02PracticeAdapter.VerifyExactNumericQuestion(
            family, new Dictionary<string, int>
            {
                ["n"] = n,
                ["variant"] = variant
            }, "1"));
    }

    [Fact]
    public void ForgedParametersAndTeacherOnlyFamilies_NeverFallback()
    {
        Assert.Equal(3, Families.Length);
        Assert.False(UsCcssAlg2U01L02PracticeAdapter.SupportsFamily(
            "usccss.alg2.u01.l02.monthly_balance"));
        Assert.False(UsCcssAlg2U01L02PracticeAdapter.VerifyExactNumericQuestion(
            Families[1], new Dictionary<string, int>
            {
                ["n"] = 2,
                ["variant"] = 0,
                ["foreign"] = 1
            }, "4"));
        Assert.False(UsCcssAlg2U01L02PracticeAdapter.VerifyExactNumericQuestion(
            Families[1], new Dictionary<string, int>
            {
                ["n"] = 2,
                ["variant"] = 0
            }, "5"));
        Assert.False(UsCcssAlg2U01L02PracticeAdapter.VerifyExactNumericQuestion(
            Families[2], new Dictionary<string, int>
            {
                ["n"] = 5,
                ["variant"] = 1
            }, "1/0"));
    }
}
