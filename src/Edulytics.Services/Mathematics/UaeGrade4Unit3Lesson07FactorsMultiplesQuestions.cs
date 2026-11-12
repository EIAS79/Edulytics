using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
// UAE G4 U03 L07: factors and multiples from equal-sized arrays.
internal static class UaeGrade4Unit3Lesson07FactorsMultiplesQuestions
{
    internal const string IsFactor="uae.g4.u03.l07.is_factor";
    internal const string NthMultiple="uae.g4.u03.l07.nth_multiple";
    internal const string MissingFactor="uae.g4.u03.l07.factor_pair";
    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is IsFactor or NthMultiple or MissingFactor;
    internal static Item Build(string f,Random rng)
    {
        if(!Supports(f))throw new InvalidOperationException("Unknown G4 factors family");
        int a=rng.Next(2,13),b=rng.Next(2,13),n=a*b;
        string prompt=f switch
        {
            IsFactor=>$"هل العدد {a} عامل للعدد {(b % 2 == 0 ? n : n + 1)}؟ اكتب ١ لنعم و٠ للا.",
            NthMultiple=>$"ما المضاعف ذو الترتيب {b+1} للعدد {a} إذا بدأنا بالصفر؟",
            _=>$"أكمل زوج العوامل: {a} × □ = {n}."
        };
        return new Item(f,prompt,"العامل يقسم العدد دون باقٍ، والمضاعف هو حاصل ضرب العدد في عدد كلي.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"a",a},{"b",b}});
    }
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out int n)
    {
        n=0;
        if(!Supports(f)||p.Count!=2||!p.TryGetValue("a",out int a)||
           !p.TryGetValue("b",out int b)||a is <2 or >=13||b is <2 or >=13)return false;
        n=f switch {IsFactor=>b % 2 == 0 ? 1 : 0,NthMultiple=>a*b,_=>b};
        return true;
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p,out int n))throw new InvalidOperationException("Invalid factor parameters");
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
