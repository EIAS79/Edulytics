using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
// UAE Grade 4 Unit 3 Lesson 3: multiplicative comparison.
internal static class UaeGrade4Unit3Lesson03MultiplicativeComparisonQuestions
{
    internal const string TimesAsMany="uae.g4.u03.l03.times_as_many";
    internal const string UnknownBase="uae.g4.u03.l03.unknown_comparison_base";
    internal const string UnknownFactor="uae.g4.u03.l03.unknown_multiplier";
    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is TimesAsMany or UnknownBase or UnknownFactor;
    internal static Item Build(string f,Random rng)
    {
        if(!Supports(f))throw new InvalidOperationException("Unknown G4 multiplicative comparison");
        int baseNumber=rng.Next(2,14),multiple=rng.Next(2,11),total=baseNumber*multiple;
        string prompt=f switch
        {
            TimesAsMany=>$"لدى ليلى {baseNumber} بطاقات، ولدى مريم {multiple} أضعاف ما لدى ليلى. كم بطاقة لدى مريم؟",
            UnknownBase=>$"لدى خالد {total} كرة، وهذا يساوي {multiple} أمثال عدد كرات أخيه. كم كرة عند أخيه؟",
            _=>$"لدى فاطمة {total} قلمًا، ولدى سارة {baseNumber} أقلام. كم ضعفًا يمثل العدد الأول العدد الثاني؟"
        };
        return new Item(f,prompt,
            "المقارنة الضربية تربط الكمية الأساسية بعامل الضرب؛ أستعمل الضرب أو القسمة لإيجاد المجهول.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"baseNumber",baseNumber},{"multiple",multiple}});
    }
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out int expected)
    {
        expected=0;
        if(!Supports(f)||p.Count!=2||!p.TryGetValue("baseNumber",out int a)||
           !p.TryGetValue("multiple",out int m)||a is <2 or >=14||m is <2 or >=11)return false;
        expected=f switch {TimesAsMany=>a*m,UnknownBase=>a,_=>m};
        return true;
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p,out int n))throw new InvalidOperationException("Bad multiplicative comparison");
        return n.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(f,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
        string normalized=new(answer.Trim().Where(c=>c!=','&&c!='٬'&&c!=' '&&c!='\u00a0')
            .Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
                      c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(normalized,NumberStyles.None,CultureInfo.InvariantCulture,out int actual)&&actual==expected;
    }
}
