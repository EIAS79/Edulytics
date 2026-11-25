using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// UAE Grade10 General U02L02 quotient property and zero/negative powers,
/// original 2025–26 Arabic Student PDF105–111/printed92–98.
/// Source-bounded signed integer exponent or rational exact answer.
/// Stage C authored; Stage D tests and real UI activation deferred.
/// </summary>
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10ExponentQuotient(
        string family,Random random,ExactSkillQuestionDifficulty difficulty)
    {
        int limit=difficulty==ExactSkillQuestionDifficulty.Challenge?8:6;
        int baseNumber=random.Next(2,9);
        int top=random.Next(0,limit+1);
        int bottom=random.Next(0,limit+1);
        int negative=random.Next(1,5);
        string prompt=family switch
        {
            UaeG10ExponentQuotientResponse.Quotient =>
                $"بافتراض أن المقام لا يساوي صفرًا، بسّط {baseNumber}^{top} ÷ {baseNumber}^{bottom} على صورة قوة واحدة للأساس نفسه. ما الأس الناتج، حتى إن كان سالبًا؟ اكتب الأس فقط.",
            UaeG10ExponentQuotientResponse.Negative =>
                $"اكتب {baseNumber}^(−{negative}) على صورة كسر مبسط ذي أس موجب. اكتب كسرًا مثل 1/8، ولا تترك أسًا سالبًا.",
            _=>$"ما قيمة {baseNumber}^0، إذا كان أساس القوة لا يساوي صفرًا؟ اكتب عددًا صحيحًا."
        };
        return Problem(family,prompt,
            "قسم قوى الأساس نفسه بطرح الأسس. كل أساس غير صفري للأس صفر يساوي 1. الأس السالب يعبر عن مقلوب قوة موجبة.",
            family==UaeG10ExponentQuotientResponse.Negative
                ?AssessmentItemType.ShortAnswer:AssessmentItemType.Numeric,
            ("base",baseNumber),("top",top),("bottom",bottom),("negative",negative));
    }
}

internal static class UaeG10ExponentQuotientResponse
{
    internal const string Quotient="uae.g10.general.u02.l02.quotient_exponent_difference";
    internal const string Negative="uae.g10.general.u02.l02.negative_power_fraction";
    internal const string Zero="uae.g10.general.u02.l02.zero_exponent_value";
    internal static bool Supports(string? family)=>family is Quotient or Negative or Zero;

    private static bool Valid(IReadOnlyDictionary<string,int> p)
    {
        string[] keys=["base","top","bottom","negative"];
        return p.Count==keys.Length&&keys.All(p.ContainsKey)&&
               p["base"] is >=2 and <=8 &&
               p["top"] is >=0 and <=8 &&
               p["bottom"] is >=0 and <=8 &&
               p["negative"] is >=1 and <=4;
    }

    private static int Denominator(IReadOnlyDictionary<string,int> p)
    {
        int result=1;
        for(int i=0;i<p["negative"];i++)result*=p["base"];
        return result;
    }

    private static string Normalize(string answer)=>
        string.Concat(answer.Trim().Where(c=>!char.IsWhiteSpace(c)).Select(c=>c switch
        {
            >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
            '−' or '–'=>'-',
            '⁄'=>'/',
            _=>c
        }));

    internal static string Format(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family)||!Valid(p))
            throw new InvalidOperationException("Invalid U02L02 source exponent quotient parameters.");
        return family switch
        {
            Quotient=>(p["top"]-p["bottom"]).ToString(CultureInfo.InvariantCulture),
            Negative=>"1/"+Denominator(p).ToString(CultureInfo.InvariantCulture),
            _=>"1"
        };
    }

    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Supports(family)||!Valid(p)||string.IsNullOrWhiteSpace(answer)||answer.Length>64)
            return false;
        string normalized=Normalize(answer);
        if(family!=Negative)
            return int.TryParse(normalized,NumberStyles.AllowLeadingSign,
                CultureInfo.InvariantCulture,out int supplied)&&supplied==
                   (family==Zero?1:p["top"]-p["bottom"]);

        var parts=normalized.Split('/',StringSplitOptions.None);
        if(parts.Length!=2||
           !int.TryParse(parts[0],NumberStyles.AllowLeadingSign,
               CultureInfo.InvariantCulture,out int numerator)||
           !int.TryParse(parts[1],NumberStyles.AllowLeadingSign,
               CultureInfo.InvariantCulture,out int denominator)||
           numerator<=0||denominator<=0||numerator>1000000||denominator>1000000)
            return false;
        return (long)numerator*Denominator(p)==denominator;
    }
}
