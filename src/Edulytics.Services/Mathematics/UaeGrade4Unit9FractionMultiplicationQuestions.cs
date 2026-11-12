using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit9FractionMultiplicationQuestions
{
    internal const string ModelRepeatedSum="uae.g4.u09.l08.model_repeated_sum";
    internal const string UnitFractionCount="uae.g4.u09.l08.model_unit_fraction_count";
    internal const string ProductNumerator="uae.g4.u09.l09.product_numerator";
    internal const string ProductWhole="uae.g4.u09.l09.product_whole";
    internal const string ProductRemainder="uae.g4.u09.l09.product_remainder";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is ModelRepeatedSum or UnitFractionCount or ProductNumerator or ProductWhole or ProductRemainder;
    internal static Item Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported fraction multiplication");
        int d=random.Next(2,13),n=random.Next(1,d),count=random.Next(2,8);
        var p=new Dictionary<string,int>{{"denominator",d},{"numerator",n},{"repeatCount",count}};
        string prompt=family switch
        {
            ModelRepeatedSum=>$"مثل {count} مجموعات متطابقة، في كل واحدة {n}/{d} من الوحدة. ما بسط المجموع قبل التبسيط؟",
            UnitFractionCount=>$"كم كسرًا واحديًا من فئة ١/{d} في {count} × {n}/{d}؟",
            ProductNumerator=>$"أوجد بسط ناتج الضرب {count} × {n}/{d} قبل التبسيط.",
            ProductWhole=>$"احسب {count} × {n}/{d}. ما عدد الوحدات الكاملة في الناتج؟",
            _=>$"احسب {count} × {n}/{d}. ما بسط الجزء الكسري بعد فصل الوحدات الكاملة؟"
        };
        return new Item(family,prompt,"الضرب في عدد كلي يعادل جمع الكسر نفسه عدة مرات، ثم أعد تجميع الوحدات الكاملة.",
            AssessmentItemType.Numeric,p);
    }
    private static bool Valid(string family,IReadOnlyDictionary<string,int> p,out int answer)
    {
        answer=0;
        if(!Supports(family)||p.Count!=3||!p.TryGetValue("denominator",out int d)||
           !p.TryGetValue("numerator",out int n)||!p.TryGetValue("repeatCount",out int times)||
           d is <2 or >12||n<1||n>=d||times is <2 or >7)return false;
        int product=n*times;
        answer=family switch
        {
            ModelRepeatedSum or UnitFractionCount or ProductNumerator=>product,
            ProductWhole=>product/d,
            _=>product%d
        };
        return true;
    }
    internal static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p,out int result))throw new InvalidOperationException("Invalid fraction multiplication parameters");
        return result.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(family,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
        string normalized=new(answer.Trim().Select(c=>c switch
        {
            >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
            _=>c
        }).Where(c=>c is not ' ' and not ',' and not '٬' and not '\u00a0').ToArray());
        return int.TryParse(normalized,NumberStyles.None,CultureInfo.InvariantCulture,out int actual)&&actual==expected;
    }
}
