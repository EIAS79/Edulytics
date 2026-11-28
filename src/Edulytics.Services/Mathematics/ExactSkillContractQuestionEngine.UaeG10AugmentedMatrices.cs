using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Canonical Grade10 U03L09: first-party exact bounded numeric subsystem of
/// the source's augmented-matrix technology laboratory (PDF98, printed81).
/// Authored A/B/C; NOT RUN or routed to production until Stage D.
/// </summary>
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10AugmentedMatrix(
        string family,Random random,ExactSkillQuestionDifficulty difficulty)
    {
        int bound=difficulty==ExactSkillQuestionDifficulty.Challenge?5:4;
        int a,b,c,d,det;
        do
        {
            a=random.Next(-bound,bound+1);b=random.Next(-bound,bound+1);
            c=random.Next(-bound,bound+1);d=random.Next(-bound,bound+1);
            det=a*d-b*c;
        }while(det==0);
        int x=random.Next(-5,6),y=random.Next(-5,6);
        int rhs1=a*x+b*y,rhs2=c*x+d*y;
        int k;
        do k=random.Next(-3,4);while(k==0);
        int col=random.Next(1,4);
        string baseSystem=$"{a}س+({b})ص={rhs1}، {c}س+({d})ص={rhs2}";
        string prompt=family switch
        {
            UaeG10AugmentedMatrixResponse.RowFamily =>
                $"كوّن المصفوفة الموسعة [ {a} {b} | {rhs1} ؛ {c} {d} | {rhs2} ] للنظام {baseSystem}. نفّذ عملية الصف R₂ ← R₂+({k})R₁. ما المدخلة الجديدة في الصف الثاني والعمود {col}؟ الأعمدة ١ و٢ للمعاملات، والعمود ٣ للثوابت.",
            UaeG10AugmentedMatrixResponse.XFamily =>
                $"كوّن المصفوفة الموسعة للنظام {baseSystem}. بعد الاختزال إلى صورة الصف المختزل (RREF) تكون معاملات المتغيرين مصفوفة الوحدة. ما قيمة س؟ اكتب عددًا صحيحًا فقط.",
            _ =>
                $"كوّن المصفوفة الموسعة للنظام {baseSystem}. بعد الاختزال إلى صورة الصف المختزل (RREF)، ما قيمة ص؟ اكتب عددًا صحيحًا فقط."
        };
        return Problem(family,prompt,
            "انقل المعاملات والثوابت إلى [أ ب | هـ؛ ج د | و]. عمليات الصفوف تؤثر في العمود الموسع أيضًا. مع محدد غير صفري، RREF=[١ ٠ | س؛ ٠ ١ | ص].",
            AssessmentItemType.Numeric,
            ("a",a),("b",b),("c",c),("d",d),("r1",rhs1),("r2",rhs2),("k",k),("col",col));
    }
}

internal static class UaeG10AugmentedMatrixResponse
{
    internal const string RowFamily="uae.g10.general.u03.l09.row_operation_entry";
    internal const string XFamily="uae.g10.general.u03.l09.rref_solution_x";
    internal const string YFamily="uae.g10.general.u03.l09.rref_solution_y";
    internal static bool Supports(string? family)=>family is RowFamily or XFamily or YFamily;

    private static bool TryExpected(string family,IReadOnlyDictionary<string,int> p,out int answer)
    {
        answer=0;
        var keys=new[]{"a","b","c","d","r1","r2","k","col"};
        if(!Supports(family)||p.Count!=keys.Length||keys.Any(k=>!p.ContainsKey(k))||
           keys.Take(4).Any(k=>p[k] is < -5 or > 5)||
           p["r1"] is < -50 or > 50||p["r2"] is < -50 or > 50||
           p["k"] is 0 or < -3 or > 3||p["col"] is <1 or >3)
            return false;
        int a=p["a"],b=p["b"],c=p["c"],d=p["d"];
        int D=a*d-b*c;
        if(D==0)return false;
        if(family==RowFamily)
        {
            int[] r1=[a,b,p["r1"]],r2=[c,d,p["r2"]];
            answer=r2[p["col"]-1]+p["k"]*r1[p["col"]-1];
            return true;
        }
        int numerator=family==XFamily?p["r1"]*d-b*p["r2"]:a*p["r2"]-p["r1"]*c;
        if(numerator%D!=0)return false;
        answer=numerator/D;
        return true;
    }

    internal static string Format(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!TryExpected(family,p,out var n))
            throw new InvalidOperationException("Invalid G10 augmented matrix assessment parameters.");
        return n.ToString(CultureInfo.InvariantCulture);
    }

    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? response)
    {
        if(!TryExpected(family,p,out var correct)||string.IsNullOrWhiteSpace(response))
            return false;
        var text=string.Concat(response.Trim().Select(ch=>ch switch
        {
            >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
            '−' or '–'=>'-',_=>ch
        }));
        return int.TryParse(text,NumberStyles.AllowLeadingSign,
            CultureInfo.InvariantCulture,out var got)&&got==correct;
    }
}
