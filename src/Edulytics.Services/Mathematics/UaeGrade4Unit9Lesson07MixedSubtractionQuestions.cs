using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit9Lesson07MixedSubtractionQuestions
{
    internal const string Whole="uae.g4.u09.l07.difference_whole";
    internal const string Fraction="uae.g4.u09.l07.difference_fraction_numerator";
    internal const string Improper="uae.g4.u09.l07.difference_improper_numerator";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? family)=>family is Whole or Fraction or Improper;
    internal static Item Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported mixed subtraction");
        int d=random.Next(2,11),w1=random.Next(2,9),w2=random.Next(1,w1),n1=random.Next(1,d),n2=random.Next(1,d);
        var p=new Dictionary<string,int>
        {{"denominator",d},{"whole1",w1},{"whole2",w2},{"numerator1",n1},{"numerator2",n2}};
        string prefix=$"احسب {w1} و{n1}/{d} − {w2} و{n2}/{d}.";
        string prompt=family switch
        {
            Whole=>prefix+" كم عدد الوحدات الكاملة في الناتج بعد إعادة التجميع؟",
            Fraction=>prefix+" ما بسط الجزء الكسري المتبقي؟ (اكتب ٠ إذا كان الناتج كليًا.)",
            _=>prefix+" ما بسط الكسر المعتل المكافئ للفرق؟"
        };
        return new Item(family,prompt,"حوّل الأعداد الكسرية إلى كسور معتلة، ثم اطرح البسطين وأعد التجميع.",
            AssessmentItemType.Numeric,p);
    }
    private static bool Valid(string family,IReadOnlyDictionary<string,int> p,out int answer)
    {
        answer=0;
        if(!Supports(family)||p.Count!=5||!p.TryGetValue("denominator",out int d)||
            !p.TryGetValue("whole1",out int w1)||!p.TryGetValue("whole2",out int w2)||
            !p.TryGetValue("numerator1",out int n1)||!p.TryGetValue("numerator2",out int n2)||
            d is <2 or >10||w1 is <2 or >8||w2<1||w2>=w1||n1<1||n2<1||n1>=d||n2>=d)return false;
        int numerator=(w1-w2)*d+n1-n2;
        if(numerator<=0)return false;
        answer=family switch{Whole=>numerator/d,Fraction=>numerator%d,_=>numerator};
        return true;
    }
    internal static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p,out int a))throw new InvalidOperationException("Invalid mixed subtraction parameters");
        return a.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(family,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
        string normalized=new(answer.Trim().Select(c=>c switch
        {
            >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
            _=>c
        }).Where(c=>c is not ' ' and not ',' and not '٬' and not '\u00a0').ToArray());
        return int.TryParse(normalized,NumberStyles.None,CultureInfo.InvariantCulture,out int actual)&&actual==expected;
    }
}
