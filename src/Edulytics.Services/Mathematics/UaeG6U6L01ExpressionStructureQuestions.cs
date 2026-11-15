using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Grade 6 UAE Arabic expression-structure lab adapter. Exact objective checks
/// classify the outer operation, count repeated equal groups, and recognize a
/// parenthesized sum as a factor. Student bar-model construction and free
/// explanations remain teacher-reviewed.
/// </summary>
internal static class UaeG6U6L01ExpressionStructureQuestions
{
    internal const string OuterOperation="uae.g6.u06.l01.classify_outer_operation";
    internal const string RepeatedGroups="uae.g6.u06.l01.count_repeated_groups";
    internal const string SumFactor="uae.g6.u06.l01.identify_sum_factor";

    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);

    internal static bool Supports(string? family) =>
        family is OuterOperation or RepeatedGroups or SumFactor;

    internal static Item Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unknown expression structure family.");
        ArgumentNullException.ThrowIfNull(random);
        int a=random.Next(2,41),b=random.Next(2,41),n=random.Next(2,5);
        if(family==OuterOperation)
        {
            int mode=random.Next(3);
            string expression=mode switch
            {
                0=>$"{a} + {b}",
                1=>$"{n} × ({a} + {b})",
                _=>$"{a*b} ÷ {b}"
            };
            return new Item(family,
                $"حدد العملية الخارجية للتعبير {expression}. اكتب: مجموع أو حاصل ضرب أو حاصل قسمة.",
                "لاحظ العملية الرئيسية خارج الأقواس، وليس العملية الفرعية بين حدين.",
                AssessmentItemType.ShortAnswer,
                new Dictionary<string,int>{{"mode",mode},{"a",a},{"b",b},{"n",n}});
        }
        if(family==RepeatedGroups)
        {
            string groups=string.Join(" + ",Enumerable.Repeat($"({a} + {b})",n));
            return new Item(family,
                $"يمثل التعبير {groups} مجموع مجموعات متساوية. كم مرة تكررت المجموعة ({a}+{b})؟ اكتب عدد المجموعات.",
                "كل مجموعة عامل كامل له حدان. احسب عدد النسخ دون عد حدودها مفردة.",
                AssessmentItemType.ShortAnswer,
                new Dictionary<string,int>{{"a",a},{"b",b},{"n",n}});
        }
        return new Item(family,
            $"في التعبير {n} × ({a} + {b}) عاملان. اكتب العامل الذي هو مجموع حدين بصيغة جمع (مثال ٣+٤).",
            "عامل المجموع هو الجزء داخل القوسين؛ يمكن تبديل حدّي الجمع.",
            AssessmentItemType.ShortAnswer,
            new Dictionary<string,int>{{"a",a},{"b",b},{"n",n}});
    }

    private static bool TryExpected(string family,IReadOnlyDictionary<string,int>? p,out string expected)
    {
        expected="";
        if(!Supports(family)||p is null||!p.TryGetValue("a",out int a)||
           !p.TryGetValue("b",out int b)||!p.TryGetValue("n",out int n)||
           a is <2 or >40||b is <2 or >40||n is <2 or >4)return false;
        if(family==OuterOperation)
        {
            if(p.Count!=4||!p.TryGetValue("mode",out int mode)||mode is <0 or >2)return false;
            expected=mode switch{0=>"مجموع",1=>"حاصل ضرب",_=>"حاصل قسمة"};
            return true;
        }
        if(p.Count!=3)return false;
        expected=family==RepeatedGroups?n.ToString(CultureInfo.InvariantCulture):
            $"{a}+{b}";
        return true;
    }

    private static string Normalize(string? value)
    {
        if(string.IsNullOrWhiteSpace(value))return "";
        return new string(value.Where(ch=>!char.IsWhiteSpace(ch)&&ch is not ('(' or ')'))
            .Select(ch=>ch switch{
                >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
                >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
                '＋'=>'+','×'=>'*',_=>ch}).ToArray());
    }

    internal static string Solve(string family,IReadOnlyDictionary<string,int> p) =>
        TryExpected(family,p,out string result)?result:
            throw new InvalidOperationException("Invalid expression structure parameters.");

    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!TryExpected(family,p,out string expected)||string.IsNullOrWhiteSpace(answer)||
            answer.Length>80)return false;
        string actual=Normalize(answer),normalizedExpected=Normalize(expected);
        if(string.Equals(actual,normalizedExpected,StringComparison.Ordinal))return true;
        if(family==SumFactor && p.TryGetValue("a",out int a)&&p.TryGetValue("b",out int b))
            return string.Equals(actual,$"{b}+{a}",StringComparison.Ordinal);
        return false;
    }
}
