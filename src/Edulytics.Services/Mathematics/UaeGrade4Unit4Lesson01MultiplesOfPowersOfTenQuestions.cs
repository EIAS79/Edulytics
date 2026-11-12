using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit4Lesson01MultiplesOfPowersOfTenQuestions
{
    internal const string Multiply="uae.g4.u04.l01.multiply_power_of_ten";
    internal const string Unknown="uae.g4.u04.l01.unknown_factor_power_ten";
    internal const string Compare="uae.g4.u04.l01.compare_powers_ten";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Multiply or Unknown or Compare;
    internal static Item Build(string f,Random random)
    {
        if(!Supports(f))throw new InvalidOperationException("Unknown Grade 4 U04 L01 question family");
        int factor=random.Next(2,10),baseDigit=random.Next(2,10);
        int zeros=random.Next(1,4),place=(int)Math.Pow(10,zeros),quantity=baseDigit*place;
        string prompt=f switch
        {
            Multiply=>$"أوجد حاصل ضرب {factor} × {quantity} باستخدام نمط مضاعفات ١٠ و١٠٠ و١٠٠٠.",
            Unknown=>$"أكمل جملة الضرب: {factor} × □ = {factor*quantity}. ما قيمة المجهول؟",
            _=>$"أي الناتجين أكبر: {factor} × {quantity} أم {factor} × {quantity/10}؟ اكتب ١ إذا كان الأول أكبر و٠ خلاف ذلك."
        };
        return new Item(f,prompt,"أضرب الأرقام الأساسية ثم أستخدم القيمة المكانية لمضاعفات عشرة.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"factor",factor},{"baseDigit",baseDigit},{"zeros",zeros}});
    }
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out int expected)
    {
        expected=0;
        if(!Supports(f)||p.Count!=3||!p.TryGetValue("factor",out var a)||
           !p.TryGetValue("baseDigit",out var b)||!p.TryGetValue("zeros",out var z)||
           a is <2 or >9||b is <2 or >9||z is <1 or >3)return false;
        expected=f switch {Multiply=>a*b*(int)Math.Pow(10,z),Unknown=>b*(int)Math.Pow(10,z),_=>1};
        return true;
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p,out var n))throw new InvalidOperationException("Invalid powers of ten parameters");
        return n.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(f,p,out var n)||string.IsNullOrWhiteSpace(answer))return false;
        string s=new(answer.Trim().Where(c=>c!=','&&c!='٬'&&c!=' '&&c!='\u00a0')
            .Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
                      c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out var x)&&x==n;
    }
}
