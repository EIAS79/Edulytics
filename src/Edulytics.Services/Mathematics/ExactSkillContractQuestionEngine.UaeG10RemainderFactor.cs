using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Canonical G10 General U01L14, original Advanced Arabic lesson3-6 pp243–248.
/// Reuses existing G10 U01L10 bounded monic divisor (x−r), polynomial
/// dividend and mathematically independent remainder verifier.
/// Stage C authored only: testing/Practice activation deferred to Stage D.
/// </summary>
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10RemainderFactor(
        string family,Random random,ExactSkillQuestionDifficulty difficulty)
    {
        var existing=BuildG10PolynomialDivision(
            UaeG10PolynomialDivisionResponse.RemainderFamily,random,difficulty);
        var parameters=existing.Parameters.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
        if(family==UaeG10RemainderFactorResponse.Factor)
        {
            // Source skill requires both actual factors and nonfactors.
            parameters["remainder"]=random.Next(0,2)==0?0:
                (random.Next(1,7)*(random.Next(0,2)==0?1:-1));
        }
        string polynomial=UaeG10PolynomialDivisionResponse.Dividend(parameters);
        int r=parameters["r"];
        string divisor=r<0?$"(س+{-r})":$"(س−{r})";
        string prompt=family==UaeG10RemainderFactorResponse.Remainder
            ?$"باستخدام نظرية الباقي، ما باقي قسمة كثيرة الحدود د(س)={polynomial} على {divisor}؟ احسب د({r}) واكتب عددًا صحيحًا فقط."
            :$"باستخدام نظرية العامل، هل {divisor} عامل لكثيرة الحدود د(س)={polynomial}؟ احسب د({r}) ثم اكتب نعم أو لا فقط.";
        return new ExactProblem(
            family,prompt,
            "بموجب نظرية الباقي يكون الباقي ر=د(أ) عند القسمة على (س−أ). بموجب نظرية العامل تكون (س−أ) عاملاً إذا وفقط إذا د(أ)=٠.",
            family==UaeG10RemainderFactorResponse.Remainder?
                AssessmentItemType.Numeric:AssessmentItemType.ShortAnswer,
            parameters);
    }
}

internal static class UaeG10RemainderFactorResponse
{
    internal const string Remainder="uae.g10.general.u01.l14.remainder_theorem_value";
    internal const string Factor="uae.g10.general.u01.l14.factor_theorem_decision";
    internal static bool Supports(string? family)=>family is Remainder or Factor;

    private static bool Valid(IReadOnlyDictionary<string,int> p)
    {
        // Delegate all original source parameters and bound checks to the
        // shared existing exact Grade10 polynomial division verifier.
        return p.ContainsKey("remainder")&&
            UaeG10PolynomialDivisionResponse.Verify(
                UaeG10PolynomialDivisionResponse.RemainderFamily,p,
                p["remainder"].ToString(CultureInfo.InvariantCulture));
    }

    internal static string Format(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family)||!Valid(p))throw new InvalidOperationException("Invalid G10 remainder/factor parameters.");
        return family==Remainder?p["remainder"].ToString(CultureInfo.InvariantCulture):
            p["remainder"]==0?"نعم":"لا";
    }

    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? response)
    {
        if(!Supports(family)||!Valid(p)||string.IsNullOrWhiteSpace(response))return false;
        if(family==Remainder)
            return UaeG10PolynomialDivisionResponse.Verify(
                UaeG10PolynomialDivisionResponse.RemainderFamily,p,response);
        string compact=string.Concat(response.Trim().Where(c=>!char.IsWhiteSpace(c)));
        return compact==(p["remainder"]==0?"نعم":"لا");
    }
}
