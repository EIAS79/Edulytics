using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>Grade10 General U02L05: bounded perfect-factor extraction
/// from square and nth roots; actual operation/rationalization proof stays
/// teacher scored until Stage D. Extends shared exact contract engine.</summary>
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10RadicalCoefficient(
        string family,Random random,ExactSkillQuestionDifficulty difficulty)
    {
        int degree=family==UaeG10RadicalSimplificationResponse.Square?2:random.Next(3,5);
        int coefficient=random.Next(2,difficulty==ExactSkillQuestionDifficulty.Challenge?8:7);
        int[] residuals=[2,3,5,6,7,10,11,13];
        int residual=residuals[random.Next(residuals.Length)];
        int original=1;for(int i=0;i<degree;i++)original*=coefficient;
        original*=residual;
        string prompt=degree==2
            ? $"بسّط √{original}=م√{residual} مع الجذر الرئيسي. ما العدد الصحيح الموجب م الذي خرج من تحت الجذر؟"
            : $"بسّط الجذر النوني لدليل {degree} للعدد {original}. يساوي م في الجذر النوني للعدد {residual}. ما قيمة م الصحيحة الموجبة؟";
        return Problem(family,prompt,
            "ابحث عن عامل قوة نونية كاملة، واستخرج جذره الصحيح. الباقي تحت الجذر لا يحتوي عاملًا هو قوة كاملة من الدليل نفسه.",
            AssessmentItemType.Numeric,
            ("degree",degree),("coefficient",coefficient),
            ("residual",residual),("radicand",original));
    }
}

internal static class UaeG10RadicalSimplificationResponse
{
    internal const string Square="uae.g10.general.u02.l05.square_root_extract_coefficient";
    internal const string Nth="uae.g10.general.u02.l05.nth_root_extract_coefficient";
    internal static bool Supports(string? family)=>family is Square or Nth;

    private static bool Valid(string family,IReadOnlyDictionary<string,int> p)
    {
        string[] keys=["degree","coefficient","residual","radicand"];
        if(!Supports(family)||p.Count!=4||!keys.All(p.ContainsKey))return false;
        if(p["degree"] is <2 or >4||p["coefficient"] is <2 or >7||
            !new[]{2,3,5,6,7,10,11,13}.Contains(p["residual"]))return false;
        if(family==Square&&p["degree"]!=2)return false;
        if(family==Nth&&p["degree"]<3)return false;
        int expected=1;for(int i=0;i<p["degree"];i++)expected*=p["coefficient"];
        return p["radicand"]==expected*p["residual"];
    }
    internal static string Format(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid Grade10 radical coefficient parameters.");
        return p["coefficient"].ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(family,p)||string.IsNullOrWhiteSpace(answer)||answer.Length>24)return false;
        var normalized=string.Concat(answer.Trim().Select(c=>c switch
        {
            >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
            '−' or '–'=>'-',_=>c
        }));
        return int.TryParse(normalized,NumberStyles.AllowLeadingSign,
            CultureInfo.InvariantCulture,out int c)&&c==p["coefficient"];
    }
}
