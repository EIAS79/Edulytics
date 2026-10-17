using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Reusable IM 2021 Grade 1 story comprehension: match expressions/equations
/// to joining/leaving action and find an unknown added amount.
/// Numeric responses 1/2 mean explicit expression choices, not arithmetic facts.
/// </summary>
public static class UsCcssG1Unit2StoryQuestions
{
    public const string Expression="usccss.g1.u02.story.choose_expression";
    public const string Equation="usccss.g1.u02.story.choose_equation";
    public const string ChangeUnknown="usccss.g1.u02.story.add_change_unknown";
    public const string WriteExpression="usccss.g1.u02.story.write_expression";
    public const string WriteEquation="usccss.g1.u02.story.write_equation";
    public static bool IsAuthoredFamily(string? family) =>
        family is WriteExpression or WriteEquation;
    // Accept spacing, not an unrelated equal-value number or a wrong operation.
    public static bool GradePersistedAuthoredAnswer(
        string? family, string expected, string? response) =>
        IsAuthoredFamily(family) && response is not null &&
        string.Equals(
            string.Concat(response.Where(ch => !char.IsWhiteSpace(ch))),
            expected, StringComparison.Ordinal);

    private static readonly string[] Nouns = ["books","blocks","pencils","stickers","counters","cards"];

    public sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);

    public static bool Supports(string? family) =>
        family is Expression or Equation or ChangeUnknown or WriteExpression or WriteEquation;

    public static Problem Build(string family,Random random)
    {
        if (!Supports(family))
            throw new InvalidOperationException("Unknown Grade 1 story family.");
        var start=random.Next(3,9);
        var amount=random.Next(1,Math.Min(4,11-start));
        var action=family==ChangeUnknown?0:random.Next(2); // 0 join, 1 leave
        var nounIndex=random.Next(Nouns.Length);
        var flip=random.Next(2);
        var p=new Dictionary<string,int>(StringComparer.Ordinal)
        {
            ["start"]=start,["amount"]=amount,["action"]=action,
            ["noun"]=nounIndex,["flip"]=flip
        };
        var result=action==0?start+amount:start-amount;
        var subject=Nouns[nounIndex];
        string prompt,solution;
        if (IsAuthoredFamily(family))
        {
            var story=action==0
                ? $"A child has {start} {subject}. {amount} more arrive."
                : $"A child has {start} {subject} and gives away {amount}.";
            prompt=story+" Show the action with counters or a drawing. "+
                (family==WriteExpression
                    ? "WRITE the matching math expression yourself (for example, 3+2 or 5-2). Type the expression, not the answer alone."
                    : "WRITE the COMPLETE matching equation yourself (for example, 3+2=5 or 5-2=3). Type the full equation, not just a number.");
            solution="Choose + for joining or - for leaving. "+
                "Use the starting group and change to make an expression, "+
                "and if asked for an equation include the correct result.";
            return new Problem(family,prompt,solution,AssessmentItemType.ShortAnswer,p);
        }
        if (family==ChangeUnknown)
        {
            prompt=$"A child has {start} {subject}. Someone gives them some more. "+
                   $"Now there are {start+amount} {subject}. How many were added? "+
                   $"Think: {start} + [ ? ] = {start+amount}. Enter the missing amount.";
            solution="Count on from the starting group to the ending total. "+
                "The missing added group, not the total, answers the question.";
        }
        else
        {
            var story=action==0
                ? $"A child has {start} {subject} and gets {amount} more."
                : $"A child has {start} {subject} and gives away {amount}.";
            var join=family==Expression?$"{start} + {amount}":
                $"{start} + {amount} = {start+amount}";
            var leave=family==Expression?$"{start} - {amount}":
                $"{start} - {amount} = {start-amount}";
            var first=flip==0?join:leave;
            var second=flip==0?leave:join;
            prompt=story+" Which "+(family==Expression?"expression":"equation")+
                " matches what happened? Enter 1 or 2. "+
                $"1) {first}; 2) {second}. "+
                (family==Equation?"Explain what each number in your chosen equation means.":
                    "Show the action with counters before choosing.");
            solution=action==0?
                "Getting more joins the groups; the matching operation is addition.":
                "Giving objects away leaves fewer; the matching operation is subtraction.";
        }
        return new Problem(family,prompt,solution,AssessmentItemType.Numeric,p);
    }

    private static bool Valid(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family) || p.Count!=5 ||
            !p.TryGetValue("start",out var start) ||
            !p.TryGetValue("amount",out var amount) ||
            !p.TryGetValue("action",out var action) ||
            !p.TryGetValue("noun",out var noun) ||
            !p.TryGetValue("flip",out var flip))
            return false;
        return start is >=3 and <=8 && amount>=1 && amount<=3 &&
            start+amount<=10 && action is 0 or 1 && noun>=0 && noun<Nouns.Length &&
            flip is 0 or 1 && (family!=ChangeUnknown || action==0);
    }

    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid Grade 1 story parameters");
        if(IsAuthoredFamily(family))
        {
            var start=p["start"];
            var amount=p["amount"];
            var join=p["action"]==0;
            var expression=$"{start}{(join?"+":"-")}{amount}";
            return family==WriteEquation
                ? $"{expression}={(join?start+amount:start-amount)}"
                : expression;
        }
        if(family==ChangeUnknown)
            return p["amount"].ToString(CultureInfo.InvariantCulture);
        // Flip=0 places addition first. Flip=1 places subtraction first.
        var correctChoice=p["action"]==p["flip"]?1:2;
        return correctChoice.ToString(CultureInfo.InvariantCulture);
    }

    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer) =>
        Valid(family,p) &&
        (IsAuthoredFamily(family)
            ? GradePersistedAuthoredAnswer(family,Solve(family,p),answer)
            : string.Equals(answer,Solve(family,p),StringComparison.Ordinal));
}