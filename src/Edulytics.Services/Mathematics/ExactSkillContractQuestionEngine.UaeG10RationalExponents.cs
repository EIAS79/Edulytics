using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Grade10 General unified U02L03 original General PDF113-119 and
/// Advanced 4-6 PDF311-316: bounded exact rational powers and simple
/// same-base exponential equations. No unrestricted root calculator/CAS.
/// Stage C authored; student execution and CI in Stage D only.
/// </summary>
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10RationalExponents(
        string family,Random random,ExactSkillQuestionDifficulty difficulty)
    {
        int r=random.Next(2,difficulty==ExactSkillQuestionDifficulty.Challenge?7:6);
        int denominator=random.Next(2,5);
        int numerator=random.Next(1,5);
        int baseNumber=random.Next(2,9);
        int offset=random.Next(-2,3);
        int target=random.Next(2,8);
        int perfectPower=1;
        for(int i=0;i<denominator;i++)perfectPower*=r;
        string prompt=family switch
        {
            UaeG10RationalExponentsResponse.Positive =>
                $"احسب {perfectPower}^({numerator}/{denominator}) بالضبط باستعمال الجذر النوني (الأساس قوة كاملة)، واكتب الناتج عددًا صحيحًا.",
            UaeG10RationalExponentsResponse.Negative =>
                $"اكتب {perfectPower}^(−{numerator}/{denominator}) في صورة كسر موجب مبسط دون أس سالب. يمكن إدخال كسر مكافئ.",
            _=>$"حل معادلة الأسين المتساويين {baseNumber}^(س+({offset}))={baseNumber}^{target}. اكتب قيمة س الصحيحة فقط."
        };
        return Problem(family,prompt,
            "أ^(م/ن)=الجذر النوني لأ مرفوعًا للقوة م عند المجال المسموح، والأس السالب مقلوب القوة. عند تساوي أساسين موجبين وغير 1 تتساوى الأسس.",
            family==UaeG10RationalExponentsResponse.Negative
                ?AssessmentItemType.ShortAnswer:AssessmentItemType.Numeric,
            ("root",r),("den",denominator),("num",numerator),
            ("base",baseNumber),("offset",offset),("target",target));
    }
}

internal static class UaeG10RationalExponentsResponse
{
    internal const string Positive="uae.g10.general.u02.l03.rational_positive_perfect_power";
    internal const string Negative="uae.g10.general.u02.l03.rational_negative_fraction";
    internal const string Equation="uae.g10.general.u02.l03.exponential_same_base_equation";

    internal static bool Supports(string? family)=>family is Positive or Negative or Equation;

    private static bool Valid(IReadOnlyDictionary<string,int> p)
    {
        string[] keys=["root","den","num","base","offset","target"];
        return p.Count==keys.Length&&keys.All(p.ContainsKey)&&
            p["root"] is >=2 and <=6&&p["den"] is >=2 and <=4&&
            p["num"] is >=1 and <=4&&p["base"] is >=2 and <=8&&
            p["offset"] is >=-2 and <=2&&p["target"] is >=2 and <=7;
    }

    private static int IntegerRootPower(IReadOnlyDictionary<string,int> p)
    {
        int result=1;
        for(int i=0;i<p["num"];i++)result*=p["root"];
        return result;
    }

    private static string Normalize(string raw)=>
        string.Concat(raw.Trim().Where(c=>!char.IsWhiteSpace(c)).Select(c=>c switch
        {
            >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
            '−' or '–'=>'-', '⁄'=>'/',_=>c
        }));

    internal static string Format(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family)||!Valid(p))
            throw new InvalidOperationException("Invalid rational-exponent exact family parameters.");
        int value=IntegerRootPower(p);
        return family switch
        {
            Positive=>value.ToString(CultureInfo.InvariantCulture),
            Negative=>"1/"+value.ToString(CultureInfo.InvariantCulture),
            _=>(p["target"]-p["offset"]).ToString(CultureInfo.InvariantCulture)
        };
    }

    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Supports(family)||!Valid(p)||string.IsNullOrWhiteSpace(answer)||answer.Length>64)
            return false;
        string s=Normalize(answer);
        if(family==Negative)
        {
            var parts=s.Split('/',StringSplitOptions.None);
            if(parts.Length!=2||
                !int.TryParse(parts[0],NumberStyles.AllowLeadingSign,CultureInfo.InvariantCulture,out int n)||
                !int.TryParse(parts[1],NumberStyles.AllowLeadingSign,CultureInfo.InvariantCulture,out int d)||
                n<=0||d<=0||n>1000000||d>1000000)return false;
            return (long)n*IntegerRootPower(p)==d;
        }
        if(!int.TryParse(s,NumberStyles.AllowLeadingSign,CultureInfo.InvariantCulture,out int x))
            return false;
        return x==(family==Positive?IntegerRootPower(p):p["target"]-p["offset"]);
    }
}
