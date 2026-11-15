using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>UAE G6 addition equations from strip and balance models.
 /// Freehand balance diagrams and authored contexts remain teacher reviewed.</summary>
internal static class UaeG6U7L02AdditionLabQuestions
{
    internal const string Tape="uae.g6.u07.l02.write_tape_equation";
    internal const string Money="uae.g6.u07.l02.solve_decimal_money";
    internal const string SubForm="uae.g6.u07.l02.subtraction_form";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Tape or Money or SubForm;
    private static string Norm(string? s)=>new string((s??"").Where(ch=>!char.IsWhiteSpace(ch))
       .Select(ch=>ch switch{
        >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
        >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
        '−' or '–'=>'-','٫'=>'.',_=>ch}).ToArray());
    private static bool TryAnswer(string family,IReadOnlyDictionary<string,int>? p,out string result)
    {
        result="";
        if(!Supports(family)||p is null)return false;
        if(family==Money)
        {
            if(p.Count!=2||!p.TryGetValue("totalCents",out int total)||
               !p.TryGetValue("spentCents",out int spent)||
               total is <1 or >50000||spent is <1 or >50000||total<=spent)return false;
            result=((total-spent)/100m).ToString("0.00",CultureInfo.InvariantCulture);
            return true;
        }
        if(p.Count!=2||!p.TryGetValue("known",out int known)||
           !p.TryGetValue("total",out int whole)||known is <1 or >100||
           whole is <2 or >200||whole<=known)return false;
        result=family==Tape?$"س+{known}={whole}":(whole-known).ToString(CultureInfo.InvariantCulture);
        return true;
    }
    internal static Item Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported addition lab family");
        ArgumentNullException.ThrowIfNull(random);
        Dictionary<string,int> p;
        string question;
        if(family==Money)
        {
            int total=random.Next(3000,50001),spent=random.Next(100,total);
            p=new(){{"totalCents",total},{"spentCents",spent}};
            question=$"مع طالب {(total/100m).ToString("0.00",CultureInfo.InvariantCulture)} درهم، صرف {(spent/100m).ToString("0.00",CultureInfo.InvariantCulture)} درهم. ما المبلغ المتبقي لحل س + المنصرف = الكل؟ اكتب القيمة بالدرهم فقط.";
        }
        else
        {
            int known=random.Next(1,101),unknown=random.Next(1,101),total=known+unknown;
            p=new(){{"known",known},{"total",total}};
            question=family==Tape?
                $"الشريط يتكون من كمية مجهولة س وجزء {known} ويبلغ مجموعه {total}. اكتب معادلة الجمع باستخدام الحرف س (مثل س+٣=٧).":
                $"إذا كانت س+{known}={total}، أوجد س بحساب {total}−{known}.";
        }
        return new Item(family,question,"أمثل الكل وجزءًا معلومًا ومجهولًا، وأزيل الجزء المعلوم من الطرفين.",
           AssessmentItemType.ShortAnswer,p);
    }
    internal static string Solve(string family,IReadOnlyDictionary<string,int> p)=>
        TryAnswer(family,p,out var answer)?answer:
           throw new InvalidOperationException("Invalid addition lab parameters.");
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? response)
    {
        if(string.IsNullOrWhiteSpace(response)||response.Length>90||
           !TryAnswer(family,p,out string expected))return false;
        string ans=Norm(response);
        if(family==Tape&&p.TryGetValue("known",out int known)&&p.TryGetValue("total",out int whole))
            return ans==expected||ans==$"{known}+س={whole}"||ans==$"{whole}=س+{known}";
        if(family==Money)
            return decimal.TryParse(ans,NumberStyles.Number,CultureInfo.InvariantCulture,
                   out var actual)&&
                   decimal.TryParse(expected,NumberStyles.Number,CultureInfo.InvariantCulture,
                   out var target)&&actual==target;
        return ans==expected;
    }
}
