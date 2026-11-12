using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;

/// <summary>Distinct Grade 4 algorithmic representations not covered by the shared
/// numeric place-value / division families. These remain non-production until Stage D
/// learner input, Arabic visuals, and saved-attempt acceptance have passed.</summary>
internal static class UaeGrade4DistinctAlgorithmsQuestions
{
    internal sealed record Item(string Family, string Prompt, string Solution,
        AssessmentItemType ItemType, IReadOnlyDictionary<string,int> Parameters);

    internal static readonly HashSet<string> Families =
    [
        "uae.g4.u04.l04.array_product", "uae.g4.u04.l04.array_partial_tens",
        "uae.g4.u04.l05.vertical_product", "uae.g4.u04.l05.units_carry",
        "uae.g4.u04.l06.regroup_tens", "uae.g4.u04.l06.regroup_product",
        "uae.g4.u04.l07.distributive_tens", "uae.g4.u04.l07.distributive_sum",
        "uae.g4.u04.l08.vertical_three_digit_product", "uae.g4.u04.l08.vertical_units_carry",
        "uae.g4.u04.l11.zero_place_value_product", "uae.g4.u04.l11.added_zero_places",
        "uae.g4.u05.l04.two_digit_column_product", "uae.g4.u05.l04.two_digit_tens_partial",
        "uae.g4.u06.l07.first_quotient_digit", "uae.g4.u06.l07.first_quotient_place_value",
        "uae.g4.u06.l08.partial_quotient_sum", "uae.g4.u06.l08.partial_quotient_remainder",
        "uae.g4.u06.l09.large_quotient", "uae.g4.u06.l09.large_remainder",
        "uae.g4.u06.l10.zero_middle_quotient", "uae.g4.u06.l10.zero_middle_remainder"
    ];
    internal static bool Supports(string? f) => f is not null && Families.Contains(f);

    internal static Item Build(string f, Random random)
    {
        if (!Supports(f)) throw new InvalidOperationException("Unsupported Grade4 algorithm family.");
        int a=random.Next(3,10), b=random.Next(2,9), c=random.Next(4,10), d=random.Next(4,10);
        if (f.Contains(".u04.l11.")) c=random.Next(1,4);
        if (f.Contains(".u06."))
        {
            a=random.Next(2,10); b=random.Next(1,10);
            c=random.Next(1,10); d=random.Next(a);
        }
        var p=new Dictionary<string,int>{{"a",a},{"b",b},{"c",c},{"d",d}};
        int two=10*b+c;
        int triple=100*b+10*c+d;
        int firstQuotient=100*c+10*b+1;
        int partialQ=10*b+c;
        int largeQ=1000*b+100*c+10*d+1;
        int zeroQ=100*b+c;
        int zeros=(int)Math.Pow(10,c);
        string prompt=f switch
        {
            "uae.g4.u04.l04.array_product" =>
                $"ارسم نموذج مساحة من {a} صفوف و{b} عشرات و{c} آحاد في كل صف. كم مربع وحدة؟",
            "uae.g4.u04.l04.array_partial_tens" =>
                $"قسّم مستطيلًا إلى {a} صفوف، وفي كل صف {b} عشرات و{c} آحاد. كم مربعًا في جزء العشرات؟",
            "uae.g4.u04.l05.vertical_product" =>
                $"رتّب {two} × {a} عموديًا؛ اضرب الآحاد ثم احمل إلى العشرات. ما الناتج النهائي؟",
            "uae.g4.u04.l05.units_carry" =>
                $"في عملية الضرب العمودية {two} × {a}، كم عشرة نحمل بعد ضرب الآحاد {c} × {a}؟",
            "uae.g4.u04.l06.regroup_tens" =>
                $"في نموذج القيمة المكانية، لدينا {a} مجموعات من {c} آحاد. كم عشرة جديدة تتكون بإعادة التجميع؟",
            "uae.g4.u04.l06.regroup_product" =>
                $"نمذج {a} مجموعات من {b} عشرات و{c} آحاد ثم أعد التجميع. ما العدد الكلي؟",
            "uae.g4.u04.l07.distributive_tens" =>
                $"طبّق خاصية التوزيع: {a} × ({b}×10 + {c}). ما ناتج جزء العشرات {a}×({b}×10)؟",
            "uae.g4.u04.l07.distributive_sum" =>
                $"طبّق خاصية التوزيع: {a}×({b}×10)+{a}×{c}. ما مجموع الناتجين الجزئيين؟",
            "uae.g4.u04.l08.vertical_three_digit_product" =>
                $"اضرب عموديًا {triple} × {a} مع تسجيل الحمل من الآحاد ثم العشرات. ما الناتج؟",
            "uae.g4.u04.l08.vertical_units_carry" =>
                $"في الضرب العمودي {triple} × {a}، كم عشرة تحمل بعد ضرب رقم الآحاد {d} في {a}؟",
            "uae.g4.u04.l11.zero_place_value_product" =>
                $"اضرب {a} × {b} × 10^{c}؛ ضع الأصفار في المنازل الصحيحة. ما الناتج؟",
            "uae.g4.u04.l11.added_zero_places" =>
                $"عند ضرب {a}×{b} في 10^{c}، كم منزلة إضافية يزاح إليها الناتج؟",
            "uae.g4.u05.l04.two_digit_column_product" =>
                $"اضرب عموديًا ({10*a+b}) × ({10*c+d})، مع ناتجي الآحاد والعشرات الجزئيين. ما المجموع؟",
            "uae.g4.u05.l04.two_digit_tens_partial" =>
                $"في ({10*a+b}) × ({10*c+d})، ما الناتج الجزئي لضرب العدد الأول في {10*c} عشرة؟",
            "uae.g4.u06.l07.first_quotient_digit" =>
                $"في القسمة المطولة للعدد {firstQuotient*a+d} على {a}، ما الرقم الأول من يسار خارج القسمة؟",
            "uae.g4.u06.l07.first_quotient_place_value" =>
                $"في القسمة المطولة {firstQuotient*a+d} ÷ {a}، ما القيمة المكانية للرقم الأول في المئات؟",
            "uae.g4.u06.l08.partial_quotient_sum" =>
                $"قسّم {partialQ*a+d} على {a} بطريقة النواتج الجزئية: ابدأ بـ {10*b} ثم {c}. ما الخارج؟",
            "uae.g4.u06.l08.partial_quotient_remainder" =>
                $"بعد طرح النواتج الجزئية {10*b*a} و{c*a} من {partialQ*a+d}، ما الباقي؟",
            "uae.g4.u06.l09.large_quotient" =>
                $"قسّم العدد الكبير {largeQ*a+d} على {a} قسمة مطولة مع فحص كل منزلة. ما الخارج؟",
            "uae.g4.u06.l09.large_remainder" =>
                $"في القسمة المطولة {largeQ*a+d} ÷ {a}، ما الباقي الأصغر من المقسوم عليه؟",
            "uae.g4.u06.l10.zero_middle_quotient" =>
                $"اقسم {zeroQ*a+d} على {a}، ولا تسقط الصفر في منزلة العشرات من خارج القسمة. ما الخارج؟",
            _ =>
                $"اقسم {zeroQ*a+d} على {a}؛ بعد تسجيل الصفر في وسط الخارج، ما الباقي؟"
        };
        return new Item(f,prompt,
          "تحقق من الخوارزمية بالتمثيل بالمنازل أو الأجزاء والمضاعفات؛ راجع الحمل والباقي والصفر الوسيط.",
          AssessmentItemType.Numeric,p);
    }

    private static bool TryAnswer(string f, IReadOnlyDictionary<string,int> p, out int answer)
    {
        answer=0;
        if (!Supports(f) || p.Count!=4 || !p.TryGetValue("a",out int a) ||
            !p.TryGetValue("b",out int b) || !p.TryGetValue("c",out int c) ||
            !p.TryGetValue("d",out int d) || a is <2 or >9 ||
            b is <1 or >9 || c is <0 or >9 || d is <0 or >9)
            return false;
        if (f.Contains(".u04.l11.") && c is <1 or >3) return false;
        if (f.Contains(".u06.") && d>=a) return false;
        if (f.Contains(".u04.") && !f.Contains(".u04.l11.") && (c<1 || d<1)) return false;
        int two=10*b+c;
        int triple=100*b+10*c+d;
        int zeroMul=f.Contains(".u04.l11.") ? (int)Math.Pow(10,c) : 0;
        answer=f switch
        {
            "uae.g4.u04.l04.array_product" => a*two,
            "uae.g4.u04.l04.array_partial_tens" => a*b*10,
            "uae.g4.u04.l05.vertical_product" => a*two,
            "uae.g4.u04.l05.units_carry" => a*c/10,
            "uae.g4.u04.l06.regroup_tens" => a*c/10,
            "uae.g4.u04.l06.regroup_product" => a*two,
            "uae.g4.u04.l07.distributive_tens" => a*b*10,
            "uae.g4.u04.l07.distributive_sum" => a*two,
            "uae.g4.u04.l08.vertical_three_digit_product" => a*triple,
            "uae.g4.u04.l08.vertical_units_carry" => a*d/10,
            "uae.g4.u04.l11.zero_place_value_product" => a*b*zeroMul,
            "uae.g4.u04.l11.added_zero_places" => c,
            "uae.g4.u05.l04.two_digit_column_product" => (10*a+b)*(10*c+d),
            "uae.g4.u05.l04.two_digit_tens_partial" => (10*a+b)*10*c,
            "uae.g4.u06.l07.first_quotient_digit" => c,
            "uae.g4.u06.l07.first_quotient_place_value" => 100*c,
            "uae.g4.u06.l08.partial_quotient_sum" => 10*b+c,
            "uae.g4.u06.l08.partial_quotient_remainder" => d,
            "uae.g4.u06.l09.large_quotient" => 1000*b+100*c+10*d+1,
            "uae.g4.u06.l09.large_remainder" => d,
            "uae.g4.u06.l10.zero_middle_quotient" => 100*b+c,
            _ => d
        };
        return true;
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if (!TryAnswer(f,p,out int value)) throw new InvalidOperationException("Invalid Grade4 algorithm parameters");
        return value.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!TryAnswer(f,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
        string norm=new(answer.Trim().Select(ch=>ch switch {
            >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),_=>ch
        }).Where(ch=>ch is not ' ' and not '٬' and not ',' and not '\u00a0').ToArray());
        return int.TryParse(norm,NumberStyles.None,CultureInfo.InvariantCulture,out int actual)
            && actual==expected;
    }
}
