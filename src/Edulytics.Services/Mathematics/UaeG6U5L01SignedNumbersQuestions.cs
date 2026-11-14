using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;

/// <summary>Bounded Grade6 Arabic positive/negative reference-position adapter.
/// The existing exact practice pipeline performs numeric grading. Freehand
/// number-line drawings and contextual explanations remain teacher-reviewed.</summary>
internal static class UaeG6U5L01SignedNumbersQuestions
{
    internal const string Position="uae.g6.u05.l01.signed_position";
    internal const string Opposite="uae.g6.u05.l01.opposite_integer";
    internal const string GraphSetExtremum="uae.g6.u05.l02.signed_plot_set_extremum";
    internal const string AbsoluteDistance="uae.g6.u05.l03.absolute_distance";
    internal const string AbsoluteExpression="uae.g6.u05.l04.absolute_expression";
    internal const string SignedCompare="uae.g6.u05.l05.compare_signed_integers";
    internal const string SignedOrderSet="uae.g6.u05.l05.order_integer_set";
    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? family)=>family is Position or Opposite or GraphSetExtremum or AbsoluteDistance or AbsoluteExpression or SignedCompare or SignedOrderSet;
    internal static Item Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported signed-number family.");

        ArgumentNullException.ThrowIfNull(random);
        if(family==SignedCompare)
        {
            int a=random.Next(-40,41),b=random.Next(-40,41);
            return new Item(family,$"ضع الرمز الصحيح (< أو > أو =) بين {a} و{b}.",
                "على خط الأعداد العدد الواقع يمين العدد الآخر هو الأكبر، والأعداد الموجبة أكبر من الصفر والسالبة.",
                AssessmentItemType.ShortAnswer,new Dictionary<string,int>{{"a",a},{"b",b}});
        }
        if(family==SignedOrderSet)
        {
            var set=new HashSet<int>();
            while(set.Count<4)set.Add(random.Next(-40,41));
            var n=set.ToArray();int mode=random.Next(2);
            return new Item(family,
                $"رتب الأعداد {{{string.Join("، ",n)}}} من {(mode==0?"الأصغر إلى الأكبر":"الأكبر إلى الأصغر")}. اكتب القيم مفصولة بفواصل.",
                "مثّل الأعداد بمقياس واحد، ثم اقرأها من اليسار أو اليمين بحسب الترتيب المطلوب.",
                AssessmentItemType.ShortAnswer,
                new Dictionary<string,int>{{"a",n[0]},{"b",n[1]},{"c",n[2]},{"d",n[3]},{"mode",mode}});
        }
        if(family==AbsoluteExpression)
        {
            int a=random.Next(-25,26),b=random.Next(-25,26),operation=random.Next(2);
            var p=new Dictionary<string,int>{{"a",a},{"b",b},{"operation",operation}};
            string op=operation==0?"+":"−";
            return new Item(family,
                $"أوجد قيمة التعبير |{a}| {op} |{b}|. اكتب الناتج عددًا صحيحًا.",
                "أوجد المسافة من الصفر لكل عدد أولًا، ثم اجمع أو اطرح المقدارين حسب رمز التعبير.",
                AssessmentItemType.ShortAnswer,p);
        }
        if(family==AbsoluteDistance)
        {
            int n=random.Next(-40,41);
            return new Item(family,
                $"احسب المسافة بين العدد الصحيح {n} والصفر على خط الأعداد. اكتب المقدار فقط.",
                "القيمة المطلقة تقيس المسافة من الصفر وهي غير سالبة.",
                AssessmentItemType.ShortAnswer,
                new Dictionary<string,int>{{"n",n}});
        }
        if(family==GraphSetExtremum)
        {
            var numbers=new HashSet<int>();
            while(numbers.Count<4)numbers.Add(random.Next(-20,21));
            var entries=numbers.ToArray();
            int mode=random.Next(2);
            var pp=new Dictionary<string,int>{
                ["a"]=entries[0],["b"]=entries[1],["c"]=entries[2],
                ["d"]=entries[3],["mode"]=mode
            };
            return new Item(family,
                $"مثّل الأعداد {{{string.Join("، ",entries)}}} على خط الأعداد، وحدد العدد الموجود في أقصى {(mode==0?"اليسار":"اليمين")}. اكتب العدد فقط.",
                "ثبّت الصفر ووحدات متساوية، ووضّح موضع كل عدد. يفحص المعلم الرسم ويصحّح النظام إجابة الطرف.",
                AssessmentItemType.ShortAnswer,pp);
        }
        int magnitude=random.Next(family==Opposite?0:1,501);
        int sign=random.Next(2)==0?-1:1;
        var p=new Dictionary<string,int>{{"magnitude",magnitude},{"sign",sign}};
        if(family==Position)
        {
            string place=sign==1?"فوق مستوى سطح البحر":"تحت مستوى سطح البحر";
            return new Item(family,$"يقع موضع على بعد {magnitude} قدم {place}. اكتب العدد الصحيح بإشارته بالنسبة إلى المستوى المرجعي صفر.",
                "الموضع الأعلى من المرجع موجب، والأدنى منه سالب. الصفر يمثّل مستوى المرجع.",
                AssessmentItemType.ShortAnswer,p);
        }
        int original=sign*magnitude;
        return new Item(family,$"اكتب العدد الصحيح المقابل للعدد {original}. ما موقعه من الصفر؟",
            "العددان المتقابلان على بعد متساوٍ من الصفر وفي اتجاهين متعاكسين؛ مقابل الصفر هو الصفر.",
            AssessmentItemType.ShortAnswer,p);
    }

    // Scoped G6 U05L05 exact comparison and 4-number ordering; reuse existing signed-number adapter.
    private static bool TryCompare(IReadOnlyDictionary<string,int>? p,out string symbol)
    {
        symbol="";
        if(p is null||p.Count!=2||!p.TryGetValue("a",out int a)||
           !p.TryGetValue("b",out int b)||a is <-40 or >40||b is <-40 or >40)return false;
        symbol=a<b?"<":a>b?">":"=";
        return true;
    }
    private static bool TryOrder(IReadOnlyDictionary<string,int>? p,out string values)
    {
        values="";
        if(p is null||p.Count!=5||!p.TryGetValue("a",out int a)||
           !p.TryGetValue("b",out int b)||!p.TryGetValue("c",out int c)||
           !p.TryGetValue("d",out int d)||!p.TryGetValue("mode",out int mode)||
           mode is not (0 or 1)||new[]{a,b,c,d}.Any(n=>n is <-40 or >40))return false;
        var ordered=mode==0?new[]{a,b,c,d}.OrderBy(n=>n):new[]{a,b,c,d}.OrderByDescending(n=>n);
        values=string.Join(",",ordered.Select(n=>n.ToString(CultureInfo.InvariantCulture)));
        return true;
    }
    private static string NormalizeSignedAnswer(string answer)
    {
        return new string(answer.Where(ch=>!char.IsWhiteSpace(ch))
            .Select(ch=>ch switch{
                >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
                >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
                '−' or '–'=>'-','،'=>',',_=>ch}).ToArray());
    }

    private static bool Valid(string family,IReadOnlyDictionary<string,int>? p,out int answer)
    {

        answer=0;
        if(family==AbsoluteExpression)
        {
            if(p is null||p.Count!=3||!p.TryGetValue("a",out int a)||
               !p.TryGetValue("b",out int b)||!p.TryGetValue("operation",out int op)||
               op is not (0 or 1)||a is <-25 or >25||b is <-25 or >25)
                return false;
            answer=op==0?Math.Abs(a)+Math.Abs(b):Math.Abs(a)-Math.Abs(b);
            return true;
        }
        if(family==AbsoluteDistance)
        {
            if(p is null||p.Count!=1||!p.TryGetValue("n",out int n)||
                n is <-40 or >40)return false;
            answer=Math.Abs(n);
            return true;
        }
        if(family==GraphSetExtremum)
        {
            if(p is null||p.Count!=5||!p.TryGetValue("a",out var a)||
               !p.TryGetValue("b",out var b)||!p.TryGetValue("c",out var c)||
               !p.TryGetValue("d",out var d)||!p.TryGetValue("mode",out var mode)||
               mode is not (0 or 1)||new[]{a,b,c,d}.Any(n=>n is <-20 or >20))
                return false;
            answer=mode==0?Math.Min(Math.Min(a,b),Math.Min(c,d)):
                Math.Max(Math.Max(a,b),Math.Max(c,d));
            return true;
        }
        if(!Supports(family)||p is null||p.Count!=2||
           !p.TryGetValue("magnitude",out int magnitude)||
           !p.TryGetValue("sign",out int sign)||sign is not (-1 or 1)||
           magnitude<0||magnitude>500||
           (family==Position&&magnitude==0))return false;
        int signed=checked(sign*magnitude);
        answer=family==Position?signed:-signed;
        return true;
    }
    internal static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(family==SignedCompare)
            return TryCompare(p,out string symbol)?symbol:throw new InvalidOperationException("Invalid signed comparison.");
        if(family==SignedOrderSet)
            return TryOrder(p,out string ordered)?ordered:throw new InvalidOperationException("Invalid signed ordering.");
        if(!Valid(family,p,out int signed))
            throw new InvalidOperationException("Invalid signed-number parameters.");
        return signed.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(family is SignedCompare or SignedOrderSet)
        {
            if(string.IsNullOrWhiteSpace(answer)||answer.Length>100)return false;
            string expected;
            if(family==SignedCompare){
                if(!TryCompare(p,out expected))return false;
            }else if(!TryOrder(p,out expected))return false;
            return string.Equals(NormalizeSignedAnswer(answer.Trim()),expected,StringComparison.Ordinal);
        }
        if(!Valid(family,p,out int expected)||
           string.IsNullOrWhiteSpace(answer)||answer.Length>36)return false;
        string normalized=new(answer.Trim().Where(ch=>!char.IsWhiteSpace(ch))
            .Select(ch=>ch switch{
                >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
                >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
                '−' or '–'=>'-',_=>ch
            }).ToArray());
        return int.TryParse(normalized,NumberStyles.AllowLeadingSign,
            CultureInfo.InvariantCulture,out int actual)&&actual==expected;
    }
}
