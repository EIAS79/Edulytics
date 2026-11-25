using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;

/// <summary>
/// Grade10 General U03L06: source-exact row-times-column matrix product and
/// matrix multiplication shape compatibility. PDF72-79, original Advanced U01L07.
/// Machine scope 2x2 integer entry and size rules only; Stage D NOT RUN.
/// </summary>
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10MatrixProduct(string family,Random random,
        ExactSkillQuestionDifficulty difficulty)
    {
        if(family==UaeG10MatrixProductResponse.ShapeFamily)
        {
            int ar=random.Next(1,4),ac=random.Next(1,4),
                br=random.Next(1,4),bc=random.Next(1,4);
            return Problem(family,
                $"المصفوفة أ رتبتها {ar}×{ac} والمصفوفة ب رتبتها {br}×{bc}. هل الضرب أ×ب مُعرَّف؟ اكتب ١ إذا كان ممكنًا، و٠ إذا كان غير معرف.",
                "عدد أعمدة المصفوفة الأولى يجب أن يساوي عدد صفوف الثانية. رتبة الناتج عند التعريف = صفوف الأولى × أعمدة الثانية.",
                AssessmentItemType.Numeric,("ar",ar),("ac",ac),("br",br),("bc",bc));
        }
        int bound=difficulty==ExactSkillQuestionDifficulty.Challenge?6:4;
        int[] a=new int[4],b=new int[4];
        for(int i=0;i<4;i++){a[i]=random.Next(-bound,bound+1);b[i]=random.Next(-bound,bound+1);}
        int row=random.Next(1,3),col=random.Next(1,3);
        string p=$"أ=[ {a[0]}  {a[1]} ؛ {a[2]}  {a[3]} ] و ب=[ {b[0]}  {b[1]} ؛ {b[2]}  {b[3]} ]؛ كلاهما ٢×٢. احسب مدخلة الصف {row} والعمود {col} في أ×ب بالجداء القياسي للصف والعمود. اكتب عددًا صحيحًا فقط.";
        return Problem(family,p,
            "اضرب صف أ المطلوب في عمود ب المطلوب: المدخلة = العنصر الأول×العنصر الأول + العنصر الثاني×العنصر الثاني، ولا تضرب الموضعين المتناظرين مباشرة.",
            AssessmentItemType.Numeric,
            ("a11",a[0]),("a12",a[1]),("a21",a[2]),("a22",a[3]),
            ("b11",b[0]),("b12",b[1]),("b21",b[2]),("b22",b[3]),
            ("row",row),("col",col));
    }
}
internal static class UaeG10MatrixProductResponse
{
    internal const string EntryFamily="uae.g10.general.u03.l06.matrix_product_entry";
    internal const string ShapeFamily="uae.g10.general.u03.l06.matrix_product_compatible";
    internal static bool Supports(string? family)=>family is EntryFamily or ShapeFamily;
    private static bool TryExpected(string family,IReadOnlyDictionary<string,int> p,out int result)
    {
        result=0;if(!Supports(family))return false;
        if(family==ShapeFamily)
        {
            string[] names=["ar","ac","br","bc"];
            if(p.Count!=4||names.Any(x=>!p.ContainsKey(x))||
               names.Any(x=>p[x] is <1 or >3))return false;
            result=p["ac"]==p["br"]?1:0;return true;
        }
        string[] keys=["a11","a12","a21","a22","b11","b12","b21","b22","row","col"];
        if(p.Count!=10||keys.Any(x=>!p.ContainsKey(x))||
           keys.Take(8).Any(x=>p[x] is < -6 or > 6)||
           p["row"] is <1 or >2||p["col"] is <1 or >2)return false;
        string row=p["row"].ToString(CultureInfo.InvariantCulture);
        string col=p["col"].ToString(CultureInfo.InvariantCulture);
        result=p["a"+row+"1"]*p["b"+"1"+col]+p["a"+row+"2"]*p["b"+"2"+col];
        return true;
    }
    internal static string Format(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!TryExpected(family,p,out var answer))throw new InvalidOperationException("Invalid Grade10 matrix product parameters.");
        return answer.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!TryExpected(family,p,out var expected)||string.IsNullOrWhiteSpace(answer)||
           answer.Length>20)return false;
        var norm=string.Concat(answer.Trim().Select(c=>c switch
        {
            >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
            '−' or '–'=>'-',_=>c
        }));
        return int.TryParse(norm,NumberStyles.AllowLeadingSign,CultureInfo.InvariantCulture,out var x)&&x==expected;
    }
}
