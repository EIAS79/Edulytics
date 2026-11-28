using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;
/// <summary>G10 U03L07 bounded exact determinant and Cramer two-variable answer.
/// Original Advanced book Cramer's U01L08 and determinant concepts from matrix unit.
/// Stage D tests are authored later, NOT RUN; route remains disabled.</summary>
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10Cramer(string family,Random random,ExactSkillQuestionDifficulty difficulty)
    {
        int bound=difficulty==ExactSkillQuestionDifficulty.Challenge?6:4;
        int a=0,b=0,c=0,d=0,D=0;
        for(int i=0;i<256;i++)
        {
            a=random.Next(-bound,bound+1);b=random.Next(-bound,bound+1);
            c=random.Next(-bound,bound+1);d=random.Next(-bound,bound+1);
            D=a*d-b*c;
            if(D!=0)break;
            if(i==255)throw new InvalidOperationException("Cannot produce nonsingular 2x2.");
        }
        int x=random.Next(-4,5),y=random.Next(-4,5);
        int r1=a*x+b*y,r2=c*x+d*y;
        string prompt=family switch
        {
            UaeG10CramerResponse.DetFamily=>$"أوجد محدد المصفوفة [ {a}  {b} ؛ {c}  {d} ] باستخدام أ د−ب ج. اكتب العدد الصحيح فقط.",
            UaeG10CramerResponse.XFamily=>$"حل النظام ({a})س+({b})ص={r1} و ({c})س+({d})ص={r2} باستخدام قاعدة كرامر. أوجد قيمة س الدقيقة.",
            _=>$"حل النظام ({a})س+({b})ص={r1} و ({c})س+({d})ص={r2} باستخدام قاعدة كرامر. أوجد قيمة ص الدقيقة."
        };
        return Problem(family,prompt,
            "احسب المحدد D=ad−bc. إن D≠٠ فالحل وحيد: Dx=r1d−br2، Dy=ar2−cr1، ثم س=Dx/D وص=Dy/D؛ تحقق بالتعويض في كلتا المعادلتين.",
            AssessmentItemType.Numeric,("a",a),("b",b),("c",c),("d",d),("r1",r1),("r2",r2));
    }
}

internal static class UaeG10CramerResponse
{
    internal const string DetFamily="uae.g10.general.u03.l07.determinant2";
    internal const string XFamily="uae.g10.general.u03.l07.cramer_x";
    internal const string YFamily="uae.g10.general.u03.l07.cramer_y";
    internal static bool Supports(string? family)=>family is DetFamily or XFamily or YFamily;
    private static bool TryExpected(string family,IReadOnlyDictionary<string,int> p,out int expected)
    {
        expected=0;
        if(!Supports(family)||p.Count!=6)return false;
        string[] keys=["a","b","c","d","r1","r2"];
        if(keys.Any(x=>!p.ContainsKey(x))||
            keys.Take(4).Any(x=>p[x] is < -6 or > 6)||
            p["r1"] is < -48 or > 48||p["r2"] is < -48 or > 48)return false;
        int a=p["a"],b=p["b"],c=p["c"],d=p["d"],r1=p["r1"],r2=p["r2"];
        int D=a*d-b*c;
        if(D==0)return false;
        int Dx=r1*d-b*r2,Dy=a*r2-c*r1;
        if(Dx%D!=0||Dy%D!=0)return false;
        int x=Dx/D,y=Dy/D;
        if(x is < -4 or > 4||y is < -4 or > 4||
           a*x+b*y!=r1||c*x+d*y!=r2)return false;
        expected=family==DetFamily?D:family==XFamily?x:y;
        return true;
    }
    internal static string Format(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!TryExpected(family,p,out var x))throw new InvalidOperationException("Invalid Grade10 Cramer source parameters.");
        return x.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? response)
    {
        if(!TryExpected(family,p,out var expected)||string.IsNullOrWhiteSpace(response)||response.Length>20)return false;
        string normalized=string.Concat(response.Trim().Select(ch=>ch switch
        {
            >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
            '−' or '–'=>'-',_=>ch
        }));
        return int.TryParse(normalized,NumberStyles.AllowLeadingSign,CultureInfo.InvariantCulture,out int submitted)&&submitted==expected;
    }
}
