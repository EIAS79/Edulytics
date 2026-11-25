using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// UAE G10 General U03L04, original Advanced source PDF48–54:
/// exact integer unique solution of three linear equations in three variables.
/// Generation uses a known point; grading independently applies Cramer's rule.
/// Rank-deficient and non-integral solutions are teacher-reviewed, not auto-certified.
/// Stage D tests and real routing are NOT RUN.
/// </summary>
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10ThreeVarSystem(
        string family,Random random,ExactSkillQuestionDifficulty difficulty)
    {
        int bound=difficulty==ExactSkillQuestionDifficulty.Challenge?4:3;
        int[] a=new int[9];
        int det=0;
        for(int n=0;n<256;n++)
        {
            for(int i=0;i<a.Length;i++)a[i]=random.Next(-bound,bound+1);
            det=UaeG10ThreeVarSystemResponse.Det(
                a[0],a[1],a[2],a[3],a[4],a[5],a[6],a[7],a[8]);
            if(det!=0)break;
            if(n==255)throw new InvalidOperationException("Could not generate invertible 3x3 system.");
        }
        int sx=random.Next(-4,5),sy=random.Next(-4,5),sz=random.Next(-4,5);
        int b1=a[0]*sx+a[1]*sy+a[2]*sz;
        int b2=a[3]*sx+a[4]*sy+a[5]*sz;
        int b3=a[6]*sx+a[7]*sy+a[8]*sz;
        string target=family==UaeG10ThreeVarSystemResponse.XFamily?"س":
                      family==UaeG10ThreeVarSystemResponse.YFamily?"ص":"ع";
        string prompt=$"حل نظام المعادلات الخطية الثلاث: " +
            $"({a[0]})س+({a[1]})ص+({a[2]})ع={b1}؛ "+
            $"({a[3]})س+({a[4]})ص+({a[5]})ع={b2}؛ "+
            $"({a[6]})س+({a[7]})ص+({a[8]})ع={b3}. "+
            $"أوجد القيمة الدقيقة للمتغير {target}، وراجع الحل بالتعويض في المعادلات الثلاث.";
        return Problem(family,prompt,
            "احذف متغيرًا من كل زوج معادلتين لتصل إلى نظام ذي متغيرين، ثم أعد التعويض. للتحقق المستقل احسب محدد مصفوفة معاملات النظام وقاعدة كرامر وتأكد أن كل معادلة أصلية تتحقق.",
            AssessmentItemType.Numeric,
            ("a11",a[0]),("a12",a[1]),("a13",a[2]),("b1",b1),
            ("a21",a[3]),("a22",a[4]),("a23",a[5]),("b2",b2),
            ("a31",a[6]),("a32",a[7]),("a33",a[8]),("b3",b3));
    }
}

internal static class UaeG10ThreeVarSystemResponse
{
    internal const string XFamily="uae.g10.general.u03.l04.three_var_x";
    internal const string YFamily="uae.g10.general.u03.l04.three_var_y";
    internal const string ZFamily="uae.g10.general.u03.l04.three_var_z";
    internal static bool Supports(string? f)=>f is XFamily or YFamily or ZFamily;

    internal static int Det(int a,int b,int c,int d,int e,int f,int g,int h,int i) =>
        a*(e*i-f*h)-b*(d*i-f*g)+c*(d*h-e*g);

    private static bool TryExpected(string f,IReadOnlyDictionary<string,int> p,out int answer)
    {
        answer=0;
        if(!Supports(f)||p.Count!=12)return false;
        string[] names=["a11","a12","a13","b1","a21","a22","a23","b2","a31","a32","a33","b3"];
        if(names.Any(n=>!p.ContainsKey(n)))return false;
        foreach(string n in names)
        {
            int v=p[n];
            if(n.StartsWith("b",StringComparison.Ordinal))
            {
                if(v is < -48 or > 48)return false;
            }
            else if(v is < -4 or > 4)return false;
        }
        int a=p["a11"],b=p["a12"],c=p["a13"],
            d=p["a21"],e=p["a22"],h=p["a23"],
            g=p["a31"],j=p["a32"],k=p["a33"];
        int r1=p["b1"],r2=p["b2"],r3=p["b3"];
        int D=Det(a,b,c,d,e,h,g,j,k);
        if(D==0)return false;
        int Dx=Det(r1,b,c,r2,e,h,r3,j,k);
        int Dy=Det(a,r1,c,d,r2,h,g,r3,k);
        int Dz=Det(a,b,r1,d,e,r2,g,j,r3);
        if(Dx%D!=0||Dy%D!=0||Dz%D!=0)return false;
        int x=Dx/D,y=Dy/D,z=Dz/D;
        if(x is < -4 or > 4||y is < -4 or > 4||z is < -4 or > 4)return false;
        if(a*x+b*y+c*z!=r1||d*x+e*y+h*z!=r2||g*x+j*y+k*z!=r3)return false;
        answer=f==XFamily?x:f==YFamily?y:z;
        return true;
    }
    internal static string Format(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!TryExpected(f,p,out var result))
            throw new InvalidOperationException("Invalid G10 three-variable exact parameters.");
        return result.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? submitted)
    {
        if(!TryExpected(f,p,out int expected)||string.IsNullOrWhiteSpace(submitted)||submitted.Length>20)
            return false;
        var text=string.Concat(submitted.Trim().Select(c=>c switch
        {
            >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
            '−' or '–'=>'-',_=>c
        }));
        return int.TryParse(text,NumberStyles.AllowLeadingSign,CultureInfo.InvariantCulture,out int got)&&got==expected;
    }
}
