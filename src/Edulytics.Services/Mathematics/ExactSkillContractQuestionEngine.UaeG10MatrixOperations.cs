using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Canonical Grade10 General U03L05 — matrix fundamentals, addition, subtraction,
/// scalar multiplication (original Advanced U01L05 and U01L06, PDF57–71).
/// Exact cell-value questions only, not full matrix algebra or matrix product.
/// Stage D execution NOT RUN; no production routing.
/// </summary>
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10MatrixEntryOperation(
        string family,Random random,ExactSkillQuestionDifficulty difficulty)
    {
        int bound=difficulty==ExactSkillQuestionDifficulty.Challenge?8:6;
        int[] a=new int[4],b=new int[4];
        for(int i=0;i<4;i++)
        {
            a[i]=random.Next(-bound,bound+1);
            b[i]=random.Next(-bound,bound+1);
        }
        int k;
        do k=random.Next(-4,5);while(k==0);
        int row=random.Next(1,3),col=random.Next(1,3);
        string first=$"[ {a[0]}  {a[1]} ؛ {a[2]}  {a[3]} ]";
        string second=$"[ {b[0]}  {b[1]} ؛ {b[2]}  {b[3]} ]";
        string op=family switch
        {
            UaeG10MatrixOperationsResponse.SumFamily=>$"مجموع أ + ب، أ={first}، ب={second}",
            UaeG10MatrixOperationsResponse.DiffFamily=>$"فرق أ − ب، أ={first}، ب={second}",
            _=>$"حاصل ضرب الكمية العددية {k} في أ={first}"
        };
        string prompt=$"المصفوفات المعطاة من الرتبة ٢×٢؛ افصل الصفين بعلامة «؛». احسب مدخلة الصف {row} والعمود {col} في {op}. اكتب عددًا صحيحًا فقط.";
        return Problem(family,prompt,
            "تحقق من تساوي الأبعاد في الجمع والطرح، ثم اجمع أو اطرح المدخلتين المناظرتين. في الضرب القياسي اضرب كل مدخلة في الكمية العددية، دون تبديل الصف والعمود.",
            AssessmentItemType.Numeric,
            ("a11",a[0]),("a12",a[1]),("a21",a[2]),("a22",a[3]),
            ("b11",b[0]),("b12",b[1]),("b21",b[2]),("b22",b[3]),
            ("k",k),("row",row),("col",col));
    }
}

internal static class UaeG10MatrixOperationsResponse
{
    internal const string SumFamily="uae.g10.general.u03.l05.matrix_sum_entry";
    internal const string DiffFamily="uae.g10.general.u03.l05.matrix_difference_entry";
    internal const string ScalarFamily="uae.g10.general.u03.l05.matrix_scalar_entry";
    internal static bool Supports(string? f)=>f is SumFamily or DiffFamily or ScalarFamily;

    private static bool TryExpected(string family,IReadOnlyDictionary<string,int> p,out int value)
    {
        value=0;
        if(!Supports(family)||p.Count!=11)return false;
        string[] keys=["a11","a12","a21","a22","b11","b12","b21","b22","k","row","col"];
        if(keys.Any(x=>!p.ContainsKey(x)))return false;
        if(keys.Take(8).Any(x=>p[x] is < -8 or > 8)||
           p["k"] is < -4 or > 4 or 0||
           p["row"] is < 1 or > 2||
           p["col"] is < 1 or > 2)return false;
        string suffix=p["row"].ToString(CultureInfo.InvariantCulture)+
                      p["col"].ToString(CultureInfo.InvariantCulture);
        int a=p["a"+suffix],b=p["b"+suffix];
        value=family switch
        {
            SumFamily=>a+b,
            DiffFamily=>a-b,
            _=>p["k"]*a
        };
        return true;
    }
    internal static string Format(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!TryExpected(family,p,out var expected))
            throw new InvalidOperationException("Invalid matrix operation parameters.");
        return expected.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? response)
    {
        if(!TryExpected(family,p,out int expected)||
            string.IsNullOrWhiteSpace(response)||response.Length>20)return false;
        string normalized=string.Concat(response.Trim().Select(c=>c switch
        {
            >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
            '−' or '–'=>'-',_=>c
        }));
        return int.TryParse(normalized,NumberStyles.AllowLeadingSign,
            CultureInfo.InvariantCulture,out var actual)&&actual==expected;
    }
}
