using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
// UAE Grade 4 U03 L01: inverse multiplication/division fact families.
internal static class UaeGrade4Unit3Lesson01FactFamiliesQuestions
{
    internal const string Product="uae.g4.u03.l01.array_product";
    internal const string Quotient="uae.g4.u03.l01.array_quotient";
    internal const string UnknownFactor="uae.g4.u03.l01.unknown_factor";
    internal const string InverseFact="uae.g4.u03.l01.inverse_fact";
    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Product or Quotient or UnknownFactor or InverseFact;
    internal static Item Build(string f,Random rng)
    {
        if(!Supports(f))throw new InvalidOperationException("Unsupported fact-family");
        int a=rng.Next(2,13),b=rng.Next(2,13),product=a*b;
        string question=f switch
        {
            Product=>$"في مصفوفة {a} صفوف و{b} أعمدة، كم عنصرًا؟",
            Quotient=>$"يوجد {product} عنصرًا موزعًا في {a} صفوف متساوية. كم عنصرًا في الصف الواحد؟",
            UnknownFactor=>$"أكمل جملة الضرب: {a} × □ = {product}.",
            _=>$"لدى أسرة حقائق العدد {product} والعامل {a}. ما العامل الآخر الذي يعيد تكوين الناتج؟"
        };
        return new Item(f,question,
            "أرسم مصفوفة صفوف وأعمدة، ثم أكتب عمليتي ضرب وعمليتي قسمة تربط الأعداد الثلاثة.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"a",a},{"b",b}});
    }
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out int expected)
    {
        expected=0;
        if(!Supports(f)||p.Count!=2||!p.TryGetValue("a",out int a)||
           !p.TryGetValue("b",out int b)||a is <2 or >12||b is <2 or >12)return false;
        expected=f==Product?a*b:b;
        return true;
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p,out int n))throw new InvalidOperationException("Bad fact-family parameters");
        return n.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(f,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
        string t=new(answer.Trim().Where(c=>c!=','&&c!='٬'&&c!=' '&&c!='\u00a0')
            .Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
                      c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(t,NumberStyles.None,CultureInfo.InvariantCulture,out int n)&&n==expected;
    }
}
