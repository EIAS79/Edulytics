using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;

// Specialized variant family; reuses ExactSkillContractQuestionEngine, no new engine.
// Guarantees regrouping across one or more zeros. Stage D tests NOT_RUN.
internal static class UaeGrade3Unit3Lesson07Questions
{
    internal const string Family = "uae.g3.u03.l07.subtract_across_zeros_exact";
    internal sealed record Item(string Family, string Prompt, string Solution,
        AssessmentItemType ItemType, IReadOnlyDictionary<string,int> Parameters);

    public static bool Supports(string? family) => family == Family;

    public static Item Build(string family, Random rng)
    {
        if (!Supports(family)) throw new InvalidOperationException("Unknown G3 U03 L07 family");
        int mode = rng.Next(3);
        int a, b;
        if (mode == 0) // whole thousands: 7000 - 3456
        {
            a = rng.Next(2,10)*1000;
            b = rng.Next(101,a-1);
            if (b%10==0) b++;
        }
        else if (mode == 1) // whole hundreds: 400 - 175
        {
            a = rng.Next(3,10)*100;
            b = rng.Next(11,a-1);
            if (b%10==0) b++;
        }
        else // four-digit with internal double zero: 6004 - 2815
        {
            a = rng.Next(3,10)*1000 + rng.Next(1,9);
            b = rng.Next(100,a/10-1)*10 + rng.Next(a%10+1,10);
        }
        int result = a-b;
        string prompt = mode switch
        {
            0 => $"اطرح مع إعادة التجميع عبر الأصفار ثم تحقق بالجمع: {a} − {b} = ؟",
            1 => $"باستخدام مخطط المئات والعشرات والآحاد، أوجد الفرق: {a} − {b} = ؟",
            _ => $"أوجد الفرق الدقيق بين {a} و{b} مع شرح عبور منازل الصفر أثناء إعادة التجميع."
        };
        return new Item(family, prompt,
            "أبحث عن أول منزلة غير صفرية للاستلاف، ثم أعيد التجميع وأتحقق: الفرق + المطروح = العدد الأصلي.",
            AssessmentItemType.Numeric,
            new Dictionary<string,int> { ["a"]=a, ["b"]=b, ["mode"]=mode, ["result"]=result });
    }

    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p,out int answer)
    {
        answer=0;
        if(!Supports(family)||p.Count!=4||
           !p.TryGetValue("a",out int a)||a is <100 or >9999||
           !p.TryGetValue("b",out int b)||b<=0||b>=a||
           !p.TryGetValue("mode",out int mode)||mode is <0 or >2||
           !p.TryGetValue("result",out int claimed))return false;
        bool targetPattern = mode switch
        {
            0 => a>=2000 && a%1000==0 && b%10!=0,
            1 => a>=300 && a<1000 && a%100==0 && b%10!=0,
            _ => a>=3001 && a%1000 is >=1 and <=8 && b>=1000 && b%10>a%10
        };
        answer=a-b;
        return targetPattern && claimed==answer;
    }

    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p,out int answer))throw new InvalidOperationException("Invalid zero-regrouping parameters");
        return answer.ToString(CultureInfo.InvariantCulture);
    }

    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)
    {
        if(!Valid(family,p,out int correct)||string.IsNullOrWhiteSpace(answer))return false;
        var normalized=new string(answer.Trim().Select(c =>
           c is >= '٠' and <= '٩' ? (char)('0'+c-'٠') :
           c is >= '۰' and <= '۹' ? (char)('0'+c-'۰') : c).ToArray());
        return int.TryParse(normalized,NumberStyles.Integer,CultureInfo.InvariantCulture,out int entered)&&entered==correct;
    }
}
