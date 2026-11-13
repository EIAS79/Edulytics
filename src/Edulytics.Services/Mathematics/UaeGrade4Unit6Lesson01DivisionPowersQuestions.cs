using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit6Lesson01DivisionPowersQuestions
{
    internal const string Divide="uae.g4.u06.l01.divide_powers_10_100_1000";
    internal const string Inverse="uae.g4.u06.l01.missing_quotient_place_value";
    // Extend the same inverse multiplication/division Practice mechanism for G5.
    internal const string G5Quotient="uae.g5.u03.l01.quotient_from_fact";
    internal const string G5UnknownMultiplier="uae.g5.u03.l01.missing_multiplier";
    internal const string G5UnknownDivisor="uae.g5.u03.l01.missing_divisor";
    internal const string G5Product="uae.g5.u03.l01.related_product";
    private static bool IsG5(string? f)=>f is G5Quotient or G5UnknownMultiplier or G5UnknownDivisor or G5Product;
    internal const string G5ScaleDividend="uae.g5.u03.l04.scale_dividend";
    internal const string G5CancelZeros="uae.g5.u03.l04.cancel_common_zeros";
    internal const string G5TensDivisor="uae.g5.u03.l04.quotient_with_tens_divisor";
    private static bool IsG5Scaled(string? f)=>f is G5ScaleDividend or G5CancelZeros or G5TensDivisor;
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Divide or Inverse || IsG5(f) || IsG5Scaled(f);
    internal static Item Build(string f,Random r)
    {
        if(!Supports(f))throw new InvalidOperationException("Unsupported multiplication/division family");
        if(IsG5(f))return BuildG5(f,r);
        if(IsG5Scaled(f))return BuildG5Scaled(f,r);
        int factor=r.Next(2,10),digits=r.Next(2,10),zeros=r.Next(1,4);
        int place=zeros==1?10:zeros==2?100:1000;
        int dividend=factor*digits*place;
        string prompt=f==Divide?$"استخدم حقيقة الضرب الأساسية والقيمة المكانية لإيجاد {dividend} ÷ {factor}.":
            $"أكمل الجملة: {factor} × □ = {dividend}. ما العدد الذي يمثل ناتج القسمة؟";
        return new Item(f,prompt,"أقسم حاصل ضرب الأرقام الأساسية وأحتفظ بقيمة العشرات أو المئات أو الألوف.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"factor",factor},{"digits",digits},{"zeros",zeros}});
    }

    private static Item BuildG5(string family,Random random)
    {
        int groups=random.Next(2,13),each=random.Next(2,13),total=groups*each;
        string prompt=family switch
        {
            G5Quotient=>$"في {groups} مجموعات متساوية مجموعها {total} عنصرًا. ما عدد العناصر في المجموعة؟ اكتب حقيقة القسمة.",
            G5UnknownMultiplier=>$"أكمل حقيقة الضرب: {groups} × □ = {total}.",
            G5UnknownDivisor=>$"أكمل حقيقة القسمة: {total} ÷ □ = {each}.",
            _=>$"استخدم حقيقة الضرب المرتبطة بالقسمة لإيجاد العدد في {groups} × {each} = □."
        };
        return new Item(family,prompt,
            "أكوّن عائلة حقائق من ثلاثة أعداد: حاصل الضرب والعاملين، ثم أستخدم عمليتي الضرب والقسمة العكسيتين.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"groups",groups},{"each",each}});
    }
    private static bool ValidG5(string family,IReadOnlyDictionary<string,int> p,out int expected)
    {
        expected=0;
        if(!IsG5(family)||p.Count!=2||
           !p.TryGetValue("groups",out int g)||!p.TryGetValue("each",out int n)||
           g is <2 or >12||n is <2 or >12)return false;
        expected=family switch
        {
            G5Quotient=>n,
            G5UnknownMultiplier=>n,
            G5UnknownDivisor=>g,
            _=>g*n
        };
        return true;
    }


    // Reuse the place-value fact engine for the Grade 5 tens/zeros division patterns.
    private static int Pow10G5(int zeros)=>zeros==1?10:100;
    private static Item BuildG5Scaled(string f,Random rng)
    {
        int factor=rng.Next(2,10),quotient=rng.Next(11,91),zeros=rng.Next(1,3);
        int place=Pow10G5(zeros),dividend=factor*quotient*place;
        int divisor=f==G5ScaleDividend?factor:factor*place;
        string prompt=f switch
        {
            G5ScaleDividend=>$"اعتمد على حقيقة الضرب ثم أوجد {dividend} ÷ {divisor}؛ انتبه إلى تغير قيمة المنزلة.",
            G5CancelZeros=>$"احذف الأصفار المشتركة من طرفي القسمة دون تغيير الناتج ثم أوجد {dividend} ÷ {divisor}.",
            _=>$"كم ناتج قسمة {dividend} على {divisor} باستخدام نمط العشرات أو المئات؟"
        };
        return new Item(f,prompt,
            "أختصر عاملاً مشتركًا من قوى العشرة في المقسوم والمقسوم عليه فقط، أو أضاعف الحاصل إذا زاد المقسوم وحده.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"factor",factor},{"quotient",quotient},{"zeros",zeros}});
    }
    private static bool ValidG5Scaled(string f,IReadOnlyDictionary<string,int> p,out int result)
    {
        result=0;
        if(!IsG5Scaled(f)||p.Count!=3||
           !p.TryGetValue("factor",out int factor)||!p.TryGetValue("quotient",out int q)||
           !p.TryGetValue("zeros",out int zeros)||
           factor<2||factor>9||q<11||q>90||zeros<1||zeros>2)return false;
        result=f==G5ScaleDividend?q*Pow10G5(zeros):q;
        return true;
    }

    private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out int expected)
    {
        expected=0;
        if(IsG5(f))return ValidG5(f,p,out expected);
        if(IsG5Scaled(f))return ValidG5Scaled(f,p,out expected);
        if(!Supports(f)||p.Count!=3||!p.TryGetValue("factor",out var a)||!p.TryGetValue("digits",out var b)||
           !p.TryGetValue("zeros",out var z)||a is <2 or >9||b is <2 or >9||z is <1 or >3)return false;
        expected=b*(z==1?10:z==2?100:1000);
        return true;
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p,out int answer))throw new InvalidOperationException("Invalid place-value division");
        return answer.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(f,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
        var normalized=new string(answer.Trim().Where(c=>c!=','&&c!='٬'&&c!=' '&&c!='\u00a0')
            .Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(normalized,NumberStyles.None,CultureInfo.InvariantCulture,out var actual)&&actual==expected;
    }
}
