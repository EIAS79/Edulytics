using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Grade10 General U02L06: exact bounded operations on real radicals.
/// Existing Grade9 like-radical sum family is reused separately. These two
/// new families evaluate products and conjugates, not unrestricted CAS.
/// </summary>
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10RadicalOperation(
        string family,Random random,ExactSkillQuestionDifficulty difficulty)
    {
        int a=random.Next(2,difficulty==ExactSkillQuestionDifficulty.Challenge?8:7);
        int b=random.Next(2,8);
        int[] residuals=[2,3,5,6,7,10,11];
        int r=residuals[random.Next(residuals.Length)];
        bool conjugate=family==UaeG10RadicalOperationsResponse.Conjugate;
        string prompt=conjugate
            ? $"أوجد القيمة الصحيحة لحاصل ضرب المترافقين ({a}√{r}+{b})({a}√{r}−{b}) بالضبط. اكتب العدد الصحيح بعد إلغاء الحدود الوسطى."
            : $"أوجد القيمة الصحيحة لحاصل ضرب ({a}√{r})({b}√{r}) بالضبط، مستعملًا √{r}×√{r}={r}. اكتب العدد الصحيح فقط.";
        return Problem(family,prompt,
            conjugate?"استخدم فرق المربعين: (أ+ب)(أ−ب)=أ²−ب² مع أ مضاعفًا لجذر موجب.":"اضرب المعاملات ثم جذري العدد نفسه، ثم بسّط الناتج.",
            AssessmentItemType.Numeric,("a",a),("b",b),("radicand",r));
    }
}

internal static class UaeG10RadicalOperationsResponse
{
    internal const string Product="uae.g10.general.u02.l06.same_root_product_integer";
    internal const string Conjugate="uae.g10.general.u02.l06.radical_conjugate_integer";

    internal static bool Supports(string? family)=>family is Product or Conjugate;
    private static bool Valid(IReadOnlyDictionary<string,int> p)
    {
        string[] keys=["a","b","radicand"];
        return p.Count==3&&keys.All(p.ContainsKey)&&
            p["a"] is >=2 and <=7&&p["b"] is >=2 and <=7&&
            new[]{2,3,5,6,7,10,11}.Contains(p["radicand"]);
    }
    private static int Expected(string family,IReadOnlyDictionary<string,int> p)
    {
        int a=p["a"],b=p["b"],r=p["radicand"];
        return family==Conjugate?a*a*r-b*b:a*b*r;
    }
    internal static string Format(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family)||!Valid(p))throw new InvalidOperationException("Invalid radical operation parameters.");
        return Expected(family,p).ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Supports(family)||!Valid(p)||string.IsNullOrWhiteSpace(answer)||answer.Length>32)return false;
        var s=string.Concat(answer.Trim().Select(c=>c switch
        {
            >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
            '−' or '–'=>'-',_=>c
        }));
        return int.TryParse(s,NumberStyles.AllowLeadingSign,CultureInfo.InvariantCulture,out int actual)&&
               actual==Expected(family,p);
    }
}
