using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit7AlgebraRulesQuestions
{
    internal const string Add="uae.g4.u07.l05.input_add_rule";
    internal const string Subtract="uae.g4.u07.l05.input_subtract_rule";
    internal const string Multiply="uae.g4.u07.l06.input_multiply_rule";
    internal const string Divide="uae.g4.u07.l06.input_divide_rule";
    internal const string Order="uae.g4.u07.l07.order_multiply_before_add";
    internal const string TwoStep="uae.g4.u07.l08.two_step_rule";
    internal const string MultiStep="uae.g4.u07.l09.multi_step_rule";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Add or Subtract or Multiply or Divide or Order or TwoStep or MultiStep;
    internal static Item Build(string f,Random r)
    {
        if(!Supports(f))throw new InvalidOperationException("Unsupported algebra rule family");
        int n=r.Next(15,76),change=r.Next(2,10),factor=r.Next(2,7);
        var p=new Dictionary<string,int>{{"input",n},{"change",change},{"factor",factor}};
        string question=f switch
        {
            Add=>$"في جدول المدخل والمخرج، القاعدة y = x + {change}. إذا كان x = {n}، فما قيمة y؟",
            Subtract=>$"في جدول المدخل والمخرج، القاعدة y = x − {change}. إذا كان x = {n}، فما قيمة y؟",
            Multiply=>$"في جدول المدخل والمخرج، القاعدة y = {factor} × x. إذا كان x = {n}، فما قيمة y؟",
            Divide=>$"القاعدة y = x ÷ {factor}. إذا كان x = {n*factor}، فما قيمة y؟",
            Order=>$"أوجد قيمة التعبير {n} + {change} × {factor} وفق ترتيب العمليات.",
            TwoStep=>$"أكمل جدول المعادلة y = (x + {change}) × {factor} عندما يكون x = {n}.",
            _=>$"أوجد المخرج y في المعادلة y = x × {factor} + {change}، حيث x = {n}."
        };
        return new Item(f,question,"أستبدل المدخل بقيمته وأطبق العمليات وفق ترتيبها؛ ثم أتحقق بعكس العملية أو إعادة الحساب.",
            AssessmentItemType.Numeric,p);
    }
    private static bool Valid(string? f,IReadOnlyDictionary<string,int> p,out int x)
    {
        x=0;
        if(!Supports(f)||p.Count!=3||!p.TryGetValue("input",out int n)||
           !p.TryGetValue("change",out int c)||!p.TryGetValue("factor",out int k)||
           n is <15 or >75||c is <2 or >9||k is <2 or >6)return false;
        x=f switch
        {
            Add=>n+c,
            Subtract=>n-c,
            Multiply=>n*k,
            Divide=>n,
            Order=>n+c*k,
            TwoStep=>(n+c)*k,
            _=>n*k+c
        };
        return true;
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p,out int x))throw new InvalidOperationException("Invalid input-output rule");
        return x.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(f,p,out int x)||string.IsNullOrWhiteSpace(answer))return false;
        string s=new(answer.Trim().Where(c=>c!=','&&c!='٬'&&c!=' '&&c!='\u00a0')
            .Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out int actual)&&x==actual;
    }
}
