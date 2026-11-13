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
    // Grade 5 Arabic bar-model numeric expressions reuse the same exact
    // parameter/solve/verify algebra engine, without changing Grade 4.
    internal const string G5Equal="uae.g5.u06.l01.equal_group_bar_total";
    internal const string G5Two="uae.g5.u06.l01.two_group_bar_total";
    internal const string G5Paired="uae.g5.u06.l01.paired_group_expression_value";
    private static bool IsG5Bar(string? family)=>family is G5Equal or G5Two or G5Paired;
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Add or Subtract or Multiply or Divide or Order or TwoStep or MultiStep || IsG5Bar(f);
    internal static Item Build(string f,Random r)
    {
        if(!Supports(f))throw new InvalidOperationException("Unsupported algebra rule family");
        if(IsG5Bar(f))return BuildG5Bar(f,r);
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

    private static Item BuildG5Bar(string family,Random rng)
    {
        int n=rng.Next(2,7),a=rng.Next(3,13);
        int m=family==G5Equal?0:family==G5Paired?n:rng.Next(2,7);
        int b=family==G5Equal?0:rng.Next(3,13);
        string prompt=family switch
        {
            G5Equal=>$"في شريط يوجد {n} أجزاء متساوية، قيمة كل جزء {a}. اكتب تعبير ضرب يمثل الشريط، ثم احسب إجمالي الأجزاء.",
            G5Two=>$"يمثل شريط {n} مجموعات كل منها {a} وأيضًا {m} مجموعات كل منها {b}. اكتب تعبيرًا باستعمال الضرب والجمع ثم احسب قيمة الشريط.",
            _=>$"يتكرر في الرسم جزء قيمته {a} وجزء قيمته {b} في {n} أزواج. احسب قيمة تعبير مكافئ: {n} × ({a} + {b})."
        };
        return new Item(family,prompt,
            "أحدد الأجزاء المتساوية وأكتب المجموع بالجمع المتكرر أو الضرب وأبرر تكافؤ التعبيرين.",
            AssessmentItemType.Numeric,
            new Dictionary<string,int>{{"n",n},{"a",a},{"m",m},{"b",b}});
    }
    private static bool ValidG5Bar(string? f,IReadOnlyDictionary<string,int> p,out int value)
    {
        value=0;
        if(!IsG5Bar(f)||p.Count!=4||!p.TryGetValue("n",out int n)||
            !p.TryGetValue("a",out int a)||!p.TryGetValue("m",out int m)||
            !p.TryGetValue("b",out int b)||n is <2 or >6||a is <3 or >12)
            return false;
        if(f==G5Equal)
        {
            if(m!=0||b!=0)return false;
        }
        else
        {
            if(m is <2 or >6||b is <3 or >12|| (f==G5Paired&&m!=n))
                return false;
        }
        value=checked(n*a+m*b);
        return true;
    }

    private static bool Valid(string? f,IReadOnlyDictionary<string,int> p,out int x)
    {
        x=0;
        if(IsG5Bar(f))return ValidG5Bar(f,p,out x);
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
