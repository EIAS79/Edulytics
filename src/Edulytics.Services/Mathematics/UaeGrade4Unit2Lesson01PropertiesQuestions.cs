using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;

// Student-book-first Grade 4 General, U02 L01. Existing exact engine extension.
internal static class UaeGrade4Unit2Lesson01PropertiesQuestions
{
    internal const string Swap="uae.g4.u02.l01.commutative_add";
    internal const string Group="uae.g4.u02.l01.associative_add";
    internal const string AddZero="uae.g4.u02.l01.additive_identity";
    internal const string SubZero="uae.g4.u02.l01.subtract_zero";
    internal const string SubSelf="uae.g4.u02.l01.subtract_self";
    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Swap or Group or AddZero or SubZero or SubSelf;
    internal static Item Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unknown G4 U02 L01 property family");
        int a=random.Next(11,100),b=random.Next(11,100),c=random.Next(1,100);
        string prompt=family switch
        {
            Swap=>$"أكمل مستعملًا خاصية التبديل: {a} + {b} = {b} + □.",
            Group=>$"أوجد المجموع: ({a} + {b}) + {c}. استخدم خاصية التجميع لتسهيل الحساب.",
            AddZero=>$"أوجد العدد المجهول في المعادلة: □ + 0 = {a}.",
            SubZero=>$"أوجد العدد المجهول في المعادلة: {a} − 0 = □.",
            _=>$"أكمل باستخدام قاعدة طرح العدد من نفسه: {a} − {a} = □."
        };
        return new Item(family,prompt,
            "قارن طرفي المعادلة وحدد خاصية الجمع أو قاعدة الطرح ثم تحقق بالتعويض.",
            AssessmentItemType.Numeric,
            new Dictionary<string,int>{{"a",a},{"b",b},{"c",c}});
    }
    private static bool Valid(string family,IReadOnlyDictionary<string,int> p,out int expected)
    {
        expected=0;
        if(!Supports(family)||p.Count!=3||
           !p.TryGetValue("a",out int a)||a is <11 or >99||
           !p.TryGetValue("b",out int b)||b is <11 or >99||
           !p.TryGetValue("c",out int c)||c is <1 or >99)return false;
        expected=family switch
        {
            Swap=>a,Group=>a+b+c,AddZero=>a,SubZero=>a,_=>0
        };
        return true;
    }
    internal static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p,out int value))throw new InvalidOperationException("Invalid property question");
        return value.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? response)
    {
        if(!Valid(family,p,out int expected)||string.IsNullOrWhiteSpace(response))return false;
        string normalized=new(response.Trim()
           .Where(c=>c!=','&&c!='٬'&&c!=' '&&c!='\u00a0')
           .Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
                        c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(normalized,NumberStyles.None,CultureInfo.InvariantCulture,out int value)
            &&value==expected;
    }
}
