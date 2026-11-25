using System.Globalization;
using System.Numerics;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>Source G10 U03L08: exact 2x2 matrix inverse rational entry or existence. StageD NOT RUN.</summary>
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10InverseMatrix(string family,Random random,ExactSkillQuestionDifficulty difficulty)
    {
        int bound=difficulty==ExactSkillQuestionDifficulty.Challenge?5:4;
        int a=0,b=0,c=0,d=0,D=0;
        bool entry=family==UaeG10InverseMatrixResponse.EntryFamily;
        for(int i=0;i<256;i++)
        {
            a=random.Next(-bound,bound+1);b=random.Next(-bound,bound+1);
            c=random.Next(-bound,bound+1);d=random.Next(-bound,bound+1);
            D=a*d-b*c;
            if(!entry||D!=0)break;
            if(i==255)throw new InvalidOperationException("No invertible matrix generated.");
        }
        int row=random.Next(1,3),col=random.Next(1,3);
        string prompt=entry
            ? $"للمصفوفة أ=[ {a}  {b} ؛ {c}  {d} ] أوجد المدخلة في الصف {row} والعمود {col} من معكوسها أ⁻¹. اكتب كسرًا دقيقًا مبسطًا مثل ٣/٢ أو عددًا صحيحًا، دون تقريب."
            : $"هل للمصفوفة أ=[ {a}  {b} ؛ {c}  {d} ] معكوس ضرب؟ اكتب ١ إذا كان لها معكوس، و٠ إذا لم يكن. اعتمد على المحدد.";
        return Problem(family,prompt,
            "احسب D=ad−bc. إذا كان D=0 فالمصفوفة غير قابلة للعكس. إذا D≠0 فإن أ⁻¹=(1/D)[d,−b;−c,a]. تحقق من أأ⁻¹=I.",
            entry?AssessmentItemType.ShortAnswer:AssessmentItemType.Numeric,
            ("a",a),("b",b),("c",c),("d",d),("row",row),("col",col));
    }
}
internal static class UaeG10InverseMatrixResponse
{
    internal const string EntryFamily="uae.g10.general.u03.l08.inverse2_entry";
    internal const string ExistsFamily="uae.g10.general.u03.l08.inverse2_exists";
    internal static bool Supports(string? f)=>f is EntryFamily or ExistsFamily;
    private static bool Params(string f,IReadOnlyDictionary<string,int> p,out int num,out int den)
    {
        num=0;den=1;string[] keys=["a","b","c","d","row","col"];
        if(!Supports(f)||p.Count!=6||keys.Any(x=>!p.ContainsKey(x))||
           keys.Take(4).Any(x=>p[x] is < -5 or > 5)||
           p["row"] is <1 or >2||p["col"] is <1 or >2)return false;
        int a=p["a"],b=p["b"],c=p["c"],d=p["d"],D=a*d-b*c;
        if(f==ExistsFamily){num=D==0?0:1;return true;}
        if(D==0)return false;
        num=p["row"]==1?(p["col"]==1?d:-b):(p["col"]==1?-c:a);den=D;return true;
    }
    internal static string Format(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Params(f,p,out int num,out int den))throw new InvalidOperationException("Invalid inverse matrix parameters.");
        if(den<0){num=-num;den=-den;}
        int gcd=Gcd(Math.Abs(num),den);num/=gcd;den/=gcd;
        return den==1?num.ToString(CultureInfo.InvariantCulture):num.ToString(CultureInfo.InvariantCulture)+"/"+den.ToString(CultureInfo.InvariantCulture);
    }
    private static int Gcd(int a,int b){while(b!=0){int t=a%b;a=b;b=t;}return Math.Max(1,a);}
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? response)
    {
        if(!Params(family,p,out int num,out int den)||string.IsNullOrWhiteSpace(response)||response.Length>64)return false;
        var normalized=string.Concat(response.Trim().Select(ch=>ch switch
        {
            >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
            '−' or '–'=>'-',_=>ch
        }));
        var parts=normalized.Split('/');
        if(parts.Length is <1 or >2 ||
           !BigInteger.TryParse(parts[0],NumberStyles.AllowLeadingSign,CultureInfo.InvariantCulture,out var n))return false;
        var d=BigInteger.One;
        if(parts.Length==2&&(!BigInteger.TryParse(parts[1],NumberStyles.AllowLeadingSign,CultureInfo.InvariantCulture,out d)||d.IsZero))return false;
        return n*den==num*d;
    }
}