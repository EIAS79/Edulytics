using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Source-exact G5 U03:L10 original pretzel shares and thirds.
/// WODB ordering and one-third ÷3 diagram reasoning TEACHER_REVIEW_REQUIRED.
/// Reuse shared exact engine, no new engine. Tests AUTHORED_NOT_RUN.
/// </summary>
public static class UsCcssG5U03L10SharingQuestions
{
    public const string PretzelFamily="usccss.g5.u03.l10.source_pretzel_share_per_student";
    public const string PatternFamily="usccss.g5.u03.l10.source_divide_by_three_pattern";
    public sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    private static readonly (int Pretzels,int Students,string Answer)[] Pretzels=
        [(42,3,"14"),(42,14,"3"),(24,3,"8"),(45,3,"15"),
         (42,7,"6"),(6,3,"2"),(42,6,"7")];
    private static readonly (string Expression,string Answer)[] Thirds=
        [("36 ÷ 3","12"),("12 ÷ 3","4"),("9 ÷ 3","3"),("6 ÷ 3","2"),
         ("3 ÷ 3","1"),("1 ÷ 3","1/3"),("(1/3) ÷ 3","1/9")];
    public static bool Supports(string? family)=>family is PretzelFamily or PatternFamily;
    private static bool Valid(string? family,IReadOnlyDictionary<string,int>? p,out int c)
    {
        c=0;
        if(!Supports(family)||p is null||p.Count!=2||
           !p.TryGetValue("case",out c)||c is <0 or >6||
           !p.TryGetValue("slot",out int s)||s is <0 or >2)return false;
        return true;
    }
    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Wrong G5 L10 family.");
        ArgumentNullException.ThrowIfNull(random);
        IReadOnlyDictionary<string,int> p=new Dictionary<string,int>(StringComparer.Ordinal)
        {["case"]=random.Next(7),["slot"]=random.Next(3)};
        if(!Valid(family,p,out int c))throw new InvalidOperationException("Bad sharing case.");
        string correct,wrongA,wrongB,prompt,explanation;
        if(family==PretzelFamily)
        {
            var e=Pretzels[c];correct=e.Answer;
            wrongA=(e.Students+1).ToString(CultureInfo.InvariantCulture);
            wrongB=(e.Pretzels+1).ToString(CultureInfo.InvariantCulture);
            prompt=$"SOURCE pretzel sharing: {e.Students} students equally share {e.Pretzels} pretzels. How many does ONE student get? ";
            explanation=$"{e.Pretzels}÷{e.Students}={correct}. The ordering of ALL seven original situations needs a teacher explanation.";
        }
        else
        {
            var e=Thirds[c];correct=e.Answer;
            wrongA=c==6?"1/3":c==5?"3":(c+1).ToString(CultureInfo.InvariantCulture);
            wrongB=c==6?"1/6":c==5?"1/9":(c+5).ToString(CultureInfo.InvariantCulture);
            prompt=$"Original source divide-by-three sequence: {e.Expression}. What is the quotient? ";
            explanation=$"{e.Expression} = {correct}; distinguish one third shared among three from one whole divided among three.";
        }
        string[] options=p["slot"] switch
        {
            0=>[correct,wrongA,wrongB],
            1=>[wrongA,correct,wrongB],
            _=>[wrongA,wrongB,correct]
        };
        prompt+=$"Select 1: {options[0]}; 2: {options[1]}; 3: {options[2]}. " +
            "Original diagrams, all seven comparisons and open partner justifications TEACHER_REVIEW_REQUIRED.";
        return new Problem(family,prompt,explanation,AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p,out _))throw new InvalidOperationException("Invalid source sharing case.");
        return (p["slot"]+1).ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int>? p,string? answer)=>
        Valid(family,p,out _) &&
        string.Equals(answer,(p!["slot"]+1).ToString(CultureInfo.InvariantCulture),
            StringComparison.Ordinal);
}
