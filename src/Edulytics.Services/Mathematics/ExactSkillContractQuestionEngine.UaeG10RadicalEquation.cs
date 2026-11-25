using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>Grade10 General U02L08 bounded equation sqrt(x+a)+c=b+c.
/// Candidate squared solutions must be rechecked in the original equation;
/// extraneous roots from negative RHS are explicitly rejected.
/// Stage C authored; tests NOT RUN until Stage D.</summary>
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10RadicalEquation(string family,Random random,ExactSkillQuestionDifficulty difficulty)
    {
        int a=random.Next(-8,9),c=random.Next(-5,6);
        int b=random.Next(1,difficulty==ExactSkillQuestionDifficulty.Challenge?8:7);
        bool candidate=family==UaeG10RadicalEquationResponse.CheckCandidate;
        int negative=candidate?random.Next(0,2):0;
        int solution=b*b-a;
        string prompt=candidate
            ? $"بعد تربيع طرفي المعادلة √(س+({a}))=({negative==1?"−":""}{b}) ظهر الحل المرشح س={solution}. هل يحقق هذا المرشح المعادلة الأصلية؟ اكتب 1 إذا تحقق، أو 0 إذا كان حلاً دخيلاً."
            : $"حل المعادلة √(س+({a}))+({c})={b+c} في الأعداد الحقيقية. اعزل الجذر ثم ربّع، وتأكد من الناتج بالتعويض في المعادلة الأصلية. اكتب قيمة س الصحيحة.";
        return Problem(family,prompt,
            "اعزل الجذر. إذا كان الطرف الآخر سالبًا فلا حل حقيقي. ارفع الطرفين للقوة الثانية، ثم تحقق من جميع المرشحين في المعادلة الأصلية لأن التربيع قد يولد حلًا دخيلاً.",
            AssessmentItemType.Numeric,("a",a),("b",b),("c",c),("negative",negative));
    }
}
internal static class UaeG10RadicalEquationResponse
{
    internal const string Solution="uae.g10.general.u02.l08.radical_equation_integer_solution";
    internal const string CheckCandidate="uae.g10.general.u02.l08.squared_candidate_is_valid";
    internal static bool Supports(string? family)=>family is Solution or CheckCandidate;
    private static bool Valid(IReadOnlyDictionary<string,int> p)
    {
        string[] keys=["a","b","c","negative"];
        return p.Count==keys.Length&&keys.All(p.ContainsKey)&&
            p["a"] is >=-8 and <=8&&p["b"] is >=1 and <=7&&
            p["c"] is >=-5 and <=5&&p["negative"] is 0 or 1;
    }
    private static int Expected(string family,IReadOnlyDictionary<string,int> p)
    {
        int candidate=p["b"]*p["b"]-p["a"];
        if(family==Solution)return candidate;

        // Compare with the ORIGINAL principal-square-root equation, not
        // the squared equation or the hidden generator's "negative" flag.
        int radicand=candidate+p["a"];
        int signedRightSide=p["negative"]==1?-p["b"]:p["b"];
        if(radicand<0||signedRightSide<0)return 0;
        int root=(int)Math.Sqrt(radicand);
        return root*root==radicand&&root==signedRightSide?1:0;
    }
    internal static string Format(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family)||!Valid(p))throw new InvalidOperationException("Invalid radical equation parameters.");
        return Expected(family,p).ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Supports(family)||!Valid(p)||string.IsNullOrWhiteSpace(answer)||answer.Length>32)return false;
        var s=string.Concat(answer.Trim().Select(ch=>ch switch
        {
            >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
            '−' or '–'=>'-',_=>ch
        }));
        return int.TryParse(s,NumberStyles.AllowLeadingSign,CultureInfo.InvariantCulture,out int val)&&
               val==Expected(family,p);
    }
}
