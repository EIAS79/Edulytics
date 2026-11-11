using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
// Grade 4 U03 L02 division represented by repeated equal subtractions.
internal static class UaeGrade4Unit3Lesson02RepeatedSubtractionQuestions
{
    internal const string NumberJumps="uae.g4.u03.l02.count_subtraction_jumps";
    internal const string EqualGroups="uae.g4.u03.l02.equal_groups_quotient";
    internal const string MissingDividend="uae.g4.u03.l02.reconstruct_dividend";
    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is NumberJumps or EqualGroups or MissingDividend;
    internal static Item Build(string f,Random rng)
    {
        if(!Supports(f))throw new InvalidOperationException("Unknown repeated subtraction family");
        int divisor=rng.Next(2,11),quotient=rng.Next(2,11),total=divisor*quotient;
        string prompt=f switch
        {
            NumberJumps=>$"ابدأ بالعدد {total}، ثم اطرح {divisor} في كل قفزة حتى الصفر. كم قفزة؟",
            EqualGroups=>$"عندنا {total} قطعة، نضع {divisor} قطع في كل مجموعة متساوية. كم مجموعة؟",
            _=>$"اطرح عددًا يساوي {divisor} بالتكرار {quotient} مرات حتى الصفر. ما العدد الذي بدأت به؟"
        };
        return new Item(f,prompt,"يمثل خارج القسمة عدد مرات طرح المقسوم عليه حتى الوصول إلى الصفر.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"divisor",divisor},{"quotient",quotient}});
    }
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out int expected)
    {
        expected=0;
        if(!Supports(f)||p.Count!=2||!p.TryGetValue("divisor",out int d)||
           !p.TryGetValue("quotient",out int q)||d is <2 or >10||q is <2 or >10)return false;
        expected=f==MissingDividend?d*q:q;
        return true;
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p,out var n))throw new InvalidOperationException("Invalid division parameters");
        return n.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(f,p,out var expected)||string.IsNullOrWhiteSpace(answer))return false;
        string s=new(answer.Trim().Where(c=>c!=','&&c!='٬'&&c!=' '&&c!='\u00a0')
            .Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
                      c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out int actual)&&actual==expected;
    }
}
