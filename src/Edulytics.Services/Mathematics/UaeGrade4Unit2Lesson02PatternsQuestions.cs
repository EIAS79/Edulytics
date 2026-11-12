using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
// Grade 4 U02 L02: number patterns in addition/subtraction, using the existing exact-engine route.
internal static class UaeGrade4Unit2Lesson02PatternsQuestions
{
    internal const string AddStep="uae.g4.u02.l02.add_place_step";
    internal const string SubStep="uae.g4.u02.l02.subtract_place_step";
    internal const string MissingStep="uae.g4.u02.l02.find_missing_step";
    internal const string NextTerm="uae.g4.u02.l02.continue_pattern";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is AddStep or SubStep or MissingStep or NextTerm;
    private static readonly int[] Steps=[1,10,100,1_000,10_000,100_000];

    internal static Item Build(string family,Random rng)
    {
        if(!Supports(family))throw new InvalidOperationException("Unknown G4 U02 L02 number-pattern family");
        int step=Steps[rng.Next(Steps.Length)],start=rng.Next(200_000,850_000);
        // Ensure both positive and negative steps remain within Grade 4 whole numbers.
        string prompt=family switch
        {
            AddStep=>$"ما العدد الأكبر بمقدار {step} من {start}؟",
            SubStep=>$"ما العدد الأصغر بمقدار {step} من {start}؟",
            MissingStep=>$"أكمل: {start} + □ = {start+step}.",
            _=>$"أكمل النمط: {start}، {start+step}، {start+2*step}، □."
        };
        return new Item(family,prompt,
            "حدد مقدار التغير الثابت في كل خطوة، ثم اجمع أو اطرح مع مراعاة المنازل.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"start",start},{"step",step}});
    }

    private static bool Valid(string family,IReadOnlyDictionary<string,int> p,out int expected)
    {
        expected=0;
        if(!Supports(family)||p.Count!=2||!p.TryGetValue("start",out int a)||
           !p.TryGetValue("step",out int step)||
           a<200_000||a>=850_000||!Steps.Contains(step))return false;
        expected=family switch
        {
            AddStep=>a+step,
            SubStep=>a-step,
            MissingStep=>step,
            _=>a+3*step
        };
        return true;
    }

    internal static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p,out int n))throw new InvalidOperationException("Bad G4 number pattern");
        return n.ToString(CultureInfo.InvariantCulture);
    }

    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(family,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
        string t=new(answer.Trim().Where(c=>c!=','&&c!='٬'&&c!=' '&&c!='\u00a0')
            .Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
                        c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(t,NumberStyles.None,CultureInfo.InvariantCulture,out int v)&&v==expected;
    }
}
