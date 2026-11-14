using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;

/// <summary>Bounded Grade6 Arabic positive/negative reference-position adapter.
/// The existing exact practice pipeline performs numeric grading. Freehand
/// number-line drawings and contextual explanations remain teacher-reviewed.</summary>
internal static class UaeG6U5L01SignedNumbersQuestions
{
    internal const string Position="uae.g6.u05.l01.signed_position";
    internal const string Opposite="uae.g6.u05.l01.opposite_integer";
    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? family)=>family is Position or Opposite;
    internal static Item Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported signed-number family.");
        ArgumentNullException.ThrowIfNull(random);
        int magnitude=random.Next(family==Opposite?0:1,501);
        int sign=random.Next(2)==0?-1:1;
        var p=new Dictionary<string,int>{{"magnitude",magnitude},{"sign",sign}};
        if(family==Position)
        {
            string place=sign==1?"فوق مستوى سطح البحر":"تحت مستوى سطح البحر";
            return new Item(family,$"يقع موضع على بعد {magnitude} قدم {place}. اكتب العدد الصحيح بإشارته بالنسبة إلى المستوى المرجعي صفر.",
                "الموضع الأعلى من المرجع موجب، والأدنى منه سالب. الصفر يمثّل مستوى المرجع.",
                AssessmentItemType.ShortAnswer,p);
        }
        int original=sign*magnitude;
        return new Item(family,$"اكتب العدد الصحيح المقابل للعدد {original}. ما موقعه من الصفر؟",
            "العددان المتقابلان على بعد متساوٍ من الصفر وفي اتجاهين متعاكسين؛ مقابل الصفر هو الصفر.",
            AssessmentItemType.ShortAnswer,p);
    }
    private static bool Valid(string family,IReadOnlyDictionary<string,int>? p,out int answer)
    {
        answer=0;
        if(!Supports(family)||p is null||p.Count!=2||
           !p.TryGetValue("magnitude",out int magnitude)||
           !p.TryGetValue("sign",out int sign)||sign is not (-1 or 1)||
           magnitude<0||magnitude>500||
           (family==Position&&magnitude==0))return false;
        int signed=checked(sign*magnitude);
        answer=family==Position?signed:-signed;
        return true;
    }
    internal static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p,out int signed))
            throw new InvalidOperationException("Invalid signed-number parameters.");
        return signed.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(family,p,out int expected)||
           string.IsNullOrWhiteSpace(answer)||answer.Length>36)return false;
        string normalized=new(answer.Trim().Where(ch=>!char.IsWhiteSpace(ch))
            .Select(ch=>ch switch{
                >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
                >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
                '−' or '–'=>'-',_=>ch
            }).ToArray());
        return int.TryParse(normalized,NumberStyles.AllowLeadingSign,
            CultureInfo.InvariantCulture,out int actual)&&actual==expected;
    }
}
