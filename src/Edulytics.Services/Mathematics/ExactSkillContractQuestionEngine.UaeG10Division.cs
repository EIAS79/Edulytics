using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Only the monomial divisor extension for owner-approved G10 General U01L10.
/// Existing ExactSkillContractQuestionEngine.UaeG10PolynomialDivision owns
/// monic linear long/synthetic division and its independent verifier.
/// Source: 01متقدم_.pdf 2025-2026, unit 3 lesson 3-2, PDF 204–212.
/// Authored during Stage C, NOT RUN until Stage D.
/// </summary>
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10Division(
        string family, Random random, ExactSkillQuestionDifficulty difficulty)
    {
        if(!UaeG10DivisionResponse.Supports(family))
            throw new InvalidOperationException("Unsupported G10 monomial division family.");

        int bound=difficulty==ExactSkillQuestionDifficulty.Challenge?6:4;
        int divisor=NonZeroG10Monomial(random,bound);
        int power=random.Next(0,3);
        int q0=random.Next(-bound,bound+1), q1=random.Next(-bound,bound+1);
        int q2=NonZeroG10Monomial(random,bound);
        int[] dividend=new int[5];
        dividend[power]=divisor*q0;
        dividend[power+1]=divisor*q1;
        dividend[power+2]=divisor*q2;
        int[] divisorPolynomial=new int[5];
        divisorPolynomial[power]=divisor;
        string prompt=$"اقسم كثيرة الحدود ({UaeG10MonomialPolynomialResponse.Format(dividend)}) " +
            $"على الحد الأحادي ({UaeG10MonomialPolynomialResponse.Format(divisorPolynomial)}). " +
            "اكتب خارج القسمة المبسط بالصورة القياسية، مع استبعاد القاسم الصفري.";

        return Problem(family,prompt,
            "اقسم كل حد من المقسوم على الحد الأحادي: اقسم المعاملات واطرح أسس المتغيرات. تحقق بضرب خارج القسمة في المقسوم عليه، وبأن القاسم ليس صفرًا.",
            AssessmentItemType.ShortAnswer,
            ("m",divisor),("k",power),("q0",q0),("q1",q1),("q2",q2));
    }
    private static int NonZeroG10Monomial(Random random,int bound)
    {
        int value;
        do value=random.Next(-bound,bound+1);while(value==0);
        return value;
    }
}

internal static class UaeG10DivisionResponse
{
    internal const string MonomialFamily="uae.g10.general.u01.l10.monomial_division_expression";
    internal static bool Supports(string? family)=>family==MonomialFamily;

    private static bool Valid(IReadOnlyDictionary<string,int> p)
    {
        string[] keys=["m","k","q0","q1","q2"];
        return p.Count==5&&keys.All(p.ContainsKey)&&
            p["m"] is >=-6 and <=6 and not 0&&
            p["k"] is >=0 and <=2&&
            p["q0"] is >=-6 and <=6&&p["q1"] is >=-6 and <=6&&
            p["q2"] is >=-6 and <=6 and not 0;
    }

    private static int[] Expected(IReadOnlyDictionary<string,int> p)=>
        [p["q0"],p["q1"],p["q2"],0,0];

    internal static string Format(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family)||!Valid(p))
            throw new InvalidOperationException("Invalid G10 monomial division parameters.");
        return UaeG10MonomialPolynomialResponse.Format(Expected(p));
    }

    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)=>
        Supports(family)&&Valid(p)&&
        UaeG10MonomialPolynomialResponse.TryParse(answer,out var given)&&
        given.SequenceEqual(Expected(p));
}
