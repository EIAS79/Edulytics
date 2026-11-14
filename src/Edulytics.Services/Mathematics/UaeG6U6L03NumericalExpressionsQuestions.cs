using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Exact bounded numeric expression templates for UAE G6. The existing
/// Grade4 multiply-before-add kernel is reused in the LessonPracticeContract.
/// Only objective calculations are auto graded; explain/draw/author steps
/// require teacher review.
/// </summary>
internal static class UaeG6U6L03NumericalExpressionsQuestions
{
    internal const string AddSub="uae.g6.u06.l03.sum_sub_left_right";
    internal const string DivMul="uae.g6.u06.l03.division_multiplication_left_right";
    internal const string Parentheses="uae.g6.u06.l03.parenthesis_first";
    internal const string Exponent="uae.g6.u06.l03.exponent_then_multiply";
    internal const string Cost="uae.g6.u06.l03.two_item_cost";
    internal const string FirstStep="uae.g6.u06.l03.first_operation";
    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? family)=>
        family is AddSub or DivMul or Parentheses or Exponent or Cost or FirstStep;
    private static decimal IntegerPower(int b,int e)
    {
        decimal value=1m;
        for(int i=0;i<e;i++)value=checked(value*b);
        return value;
    }
    private static string Format(decimal value)=>value.ToString("0.##",CultureInfo.InvariantCulture);
    private static string Normalize(string? text)
    {
        if(string.IsNullOrWhiteSpace(text))return "";
        return new string(text.Where(ch=>!char.IsWhiteSpace(ch))
            .Select(ch=>ch switch{
                >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
                >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
                '٫'=>'.','−' or '–'=>'-','،'=>',',_=>ch}).ToArray());
    }
    private static bool TryAnswer(string family,IReadOnlyDictionary<string,int>? p,out string answer)
    {
        answer="";
        if(!Supports(family)||p is null)return false;
        if(family==AddSub)
        {
            if(p.Count!=3||!p.TryGetValue("a",out int a)||!p.TryGetValue("b",out int b)||
               !p.TryGetValue("c",out int c)||a is <1 or >50||b is <1 or >50||
               c is <1 or >50)return false;
            answer=Format(a-b+c);return true;
        }
        if(family==DivMul)
        {
            if(p.Count!=3||!p.TryGetValue("q",out int q)||!p.TryGetValue("b",out int b)||
               !p.TryGetValue("c",out int c)||q is <1 or >20||b is <2 or >12||
               c is <2 or >12)return false;
            answer=Format(q*c);return true;
        }
        if(family==Parentheses)
        {
            if(p.Count!=4||!p.TryGetValue("a",out int a)||!p.TryGetValue("b",out int b)||
               !p.TryGetValue("c",out int c)||!p.TryGetValue("d",out int d)||
               a is <1 or >25||b is <1 or >12||c is <3 or >20||d is <1 or >19||
               c<=d)return false;
            answer=Format(a+b*(c-d));return true;
        }
        if(family==Exponent)
        {
            if(p.Count!=4||!p.TryGetValue("a",out int a)||!p.TryGetValue("b",out int b)||
               !p.TryGetValue("e",out int e)||!p.TryGetValue("m",out int m)||
               a is <1 or >20||b is <2 or >8||e is <2 or >4||m is <1 or >5)return false;
            answer=Format(m*IntegerPower(b,e)+a);return true;
        }
        if(family==Cost)
        {
            if(p.Count!=4||!p.TryGetValue("q1",out int q1)||!p.TryGetValue("p1",out int p1)||
               !p.TryGetValue("q2",out int q2)||!p.TryGetValue("p2",out int p2)||
               q1 is <1 or >8||q2 is <1 or >8||p1 is <25 or >600||
               p2 is <25 or >600)return false;
            answer=Format((q1*p1+q2*p2)/100m);return true;
        }
        if(family==FirstStep)
        {
            if(p.Count!=1||!p.TryGetValue("mode",out int mode)||mode is <0 or >3)return false;
            answer=mode switch{0=>"الأقواس",1=>"الأسس",2=>"الضرب والقسمة",_=>"الجمع والطرح"};
            return true;
        }
        return false;
    }
    internal static Item Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported numerical expression");
        ArgumentNullException.ThrowIfNull(random);
        Dictionary<string,int> p;
        string question;
        if(family==AddSub)
        {
            int a=random.Next(1,51),b=random.Next(1,51),c=random.Next(1,51);
            p=new(){{"a",a},{"b",b},{"c",c}};
            question=$"أوجد قيمة {a} − {b} + {c} من اليسار إلى اليمين. اكتب الناتج.";
        }
        else if(family==DivMul)
        {
            int q=random.Next(1,21),b=random.Next(2,13),c=random.Next(2,13);
            p=new(){{"q",q},{"b",b},{"c",c}};
            question=$"احسب {q*b} ÷ {b} × {c} من اليسار إلى اليمين.";
        }
        else if(family==Parentheses)
        {
            int a=random.Next(1,26),b=random.Next(1,13),c=random.Next(3,21),d=random.Next(1,c);
            p=new(){{"a",a},{"b",b},{"c",c},{"d",d}};
            question=$"احسب {a} + {b} × ({c} − {d}) وفق ترتيب العمليات.";
        }
        else if(family==Exponent)
        {
            int a=random.Next(1,21),b=random.Next(2,9),e=random.Next(2,5),m=random.Next(1,6);
            p=new(){{"a",a},{"b",b},{"e",e},{"m",m}};
            question=$"احسب {m} × {b}^{e} + {a}. ابدأ بالقوة.";
        }
        else if(family==Cost)
        {
            int q1=random.Next(1,9),q2=random.Next(1,9);
            int p1=random.Next(1,25)*25,p2=random.Next(1,25)*25;
            p=new(){{"q1",q1},{"q2",q2},{"p1",p1},{"p2",p2}};
            question=$"اشترى طالب {q1} قطع بسعر {Format(p1/100m)} درهم و{q2} قطع بسعر {Format(p2/100m)} درهم. ما التكلفة الكلية؟ اكتب المبلغ عددًا فقط.";
        }
        else
        {
            int mode=random.Next(4);
            p=new(){{"mode",mode}};
            question=mode switch{
                0=>"في التعبير ٤ + ٣ × (٧ − ٢)، أي مجموعة عمليات تنفذ أولًا؟ اكتب الأقواس أو الأسس أو الضرب والقسمة أو الجمع والطرح.",
                1=>"في التعبير ٣² × ٢ + ٥، أي مجموعة عمليات تنفذ أولًا؟ اكتب الأقواس أو الأسس أو الضرب والقسمة أو الجمع والطرح.",
                2=>"في التعبير ٨ ÷ ٢ × ٣ + ٤، أي مجموعة عمليات تنفذ أولًا؟ اكتب الأقواس أو الأسس أو الضرب والقسمة أو الجمع والطرح.",
                _=>"في التعبير ١٠ − ٢ + ٣، ما مجموعة العمليات الأولى؟ اكتب الأقواس أو الأسس أو الضرب والقسمة أو الجمع والطرح."
            };
        }
        return new Item(family,question,
            "نطبق الأقواس ثم الأسس ثم الضرب والقسمة من اليسار لليمين ثم الجمع والطرح من اليسار لليمين.",
            AssessmentItemType.ShortAnswer,p);
    }
    internal static string Solve(string family,IReadOnlyDictionary<string,int> p) =>
        TryAnswer(family,p,out var a)?a:throw new InvalidOperationException("Invalid expression parameters.");
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? response)
    {
        if(!TryAnswer(family,p,out var expected)||string.IsNullOrWhiteSpace(response)||
           response.Length>75)return false;
        string answer=Normalize(response.Trim());
        if(family==FirstStep)return string.Equals(answer,expected,StringComparison.Ordinal);
        return decimal.TryParse(answer,NumberStyles.Number,CultureInfo.InvariantCulture,out var actual)&&
            decimal.TryParse(expected,NumberStyles.Number,CultureInfo.InvariantCulture,out var target)&&actual==target;
    }
}
