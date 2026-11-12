using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
// Grade 4 Unit 3 Lesson 6: associative multiplication of three factors.
internal static class UaeGrade4Unit3Lesson06AssociativeQuestions
{
    internal const string TripleProduct="uae.g4.u03.l06.triple_product";
    internal const string MissingFactor="uae.g4.u03.l06.missing_factor";
    internal const string GroupedProduct="uae.g4.u03.l06.grouped_product";
    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is TripleProduct or MissingFactor or GroupedProduct;
    internal static Item Build(string f,Random rng)
    {
        if(!Supports(f))throw new InvalidOperationException("Unknown G4 associative family");
        int a=rng.Next(2,11),b=rng.Next(2,10),c=rng.Next(2,9);
        string text=f switch
        {
            TripleProduct=>$"استخدم خاصية التجميع في الضرب لإيجاد {a} × {b} × {c}.",
            MissingFactor=>$"أكمل المعادلة {a} × {b} × □ = {a*b*c}. ما العدد المجهول؟",
            _=>$"جد قيمة ({a} × {b}) × {c} = {a} × ({b} × {c})."
        };
        return new Item(f,text,"أختار تجميع عاملين أولًا ثم أضرب الناتج في الثالث دون تغيير حاصل الضرب.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"a",a},{"b",b},{"c",c}});
    }
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out int n)
    {
        n=0;
        if(!Supports(f)||p.Count!=3||!p.TryGetValue("a",out int a)||!p.TryGetValue("b",out int b)||
           !p.TryGetValue("c",out int c)||a is <2 or >=11||b is <2 or >=10||c is <2 or >=9)return false;
        n=f==MissingFactor?c:a*b*c;
        return true;
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p,out int n))throw new InvalidOperationException("Invalid associative problem");
        return n.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(f,p,out int n)||string.IsNullOrWhiteSpace(answer))return false;
        string s=new(answer.Trim().Where(ch=>ch!=','&&ch!='٬'&&ch!=' '&&ch!='\u00a0')
            .Select(ch=>ch is >= '٠' and <= '٩'?(char)('0'+ch-'٠'):
                        ch is >= '۰' and <= '۹'?(char)('0'+ch-'۰'):ch).ToArray());
        return int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out int actual)&&actual==n;
    }
}
