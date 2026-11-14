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
    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? family)=>family is Position or Opposite or GraphSetExtremum or AbsoluteDistance;
    internal static Item Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported signed-number family.");

        ArgumentNullException.ThrowIfNull(random);
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
    private static bool Valid(string family,IReadOnlyDictionary<string,int>? p,out int answer)
    {

        answer=0;
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
        if(!Valid(family,p,out int signed))
            throw new InvalidOperationException("Invalid signed-number parameters.");
        return signed.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
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
