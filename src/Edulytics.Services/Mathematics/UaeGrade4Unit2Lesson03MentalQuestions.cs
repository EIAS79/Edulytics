using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;

// UAE Grade 4 Common U02 L03: student-book mental addition/subtraction strategies.
// Reuses the existing ExactSkillContractQuestionEngine.
internal static class UaeGrade4Unit2Lesson03MentalQuestions
{
    internal const string MakeFriendly = "uae.g4.u02.l03.make_friendly_ten";
    internal const string Compensate = "uae.g4.u02.l03.compensate_addition";
    internal const string BreakApart = "uae.g4.u02.l03.break_apart_subtraction";
    internal const string Thousands = "uae.g4.u02.l03.subtract_thousands";
    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f) =>
        f is MakeFriendly or Compensate or BreakApart or Thousands;

    internal static Item Build(string f,Random rng)
    {
        if(!Supports(f))throw new InvalidOperationException("Unsupported G4 mental arithmetic family");
        int a,b;
        if(f==MakeFriendly) {a=rng.Next(35,990);b=rng.Next(15,100);}
        else if(f==Compensate) {a=rng.Next(200,9000);b=99+rng.Next(0,5)*100;}
        else if(f==BreakApart) {a=rng.Next(200,9000);b=rng.Next(15,Math.Min(150,a));}
        else {a=rng.Next(120,900)*1000;b=rng.Next(15,Math.Min(100,a/1000))*1000;}
        string question=f switch
        {
            MakeFriendly=>$"اجمع ذهنيًا {a} + {b}. كوّن عشرة أو مئة لتسهيل الحساب.",
            Compensate=>$"اجمع ذهنيًا {a} + {b} باستراتيجية التعويض.",
            BreakApart=>$"اطرح ذهنيًا {a} − {b} بتجزئة العدد أو تكوين عشرة.",
            _=>$"اطرح ذهنيًا {a} − {b} باستخدام قيم الألوف."
        };
        return new Item(f,question,"اشرح كيف استخدمت عددًا مناسبًا ثم عوّضت أو جزأت الحدود.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"a",a},{"b",b}});
    }
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out int result)
    {
        result=0;
        if(!Supports(f)||p.Count!=2||!p.TryGetValue("a",out int a)||
           !p.TryGetValue("b",out int b))return false;
        bool okay=f switch
        {
            MakeFriendly=>a is >=35 and <990 && b is >=15 and <100,
            Compensate=>a is >=200 and <9000 && b is 99 or 199 or 299 or 399 or 499,
            BreakApart=>a is >=200 and <9000 && b>=15 && b<Math.Min(150,a),
            Thousands=>a>=120000 && a<900000 && a%1000==0 && b>=15000 &&
                b<=99000 && b%1000==0,
            _=>false
        };
        if(!okay)return false;
        result=f==MakeFriendly||f==Compensate?a+b:a-b;
        return true;
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p,out int n))throw new InvalidOperationException("Invalid G4 mental question");
        return n.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(f,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
        string normalized=new(answer.Trim().Where(c=>c!=','&&c!='٬'&&c!=' '&&c!='\u00a0')
           .Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
                     c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(normalized,NumberStyles.None,CultureInfo.InvariantCulture,out int n)
           &&n==expected;
    }
}
