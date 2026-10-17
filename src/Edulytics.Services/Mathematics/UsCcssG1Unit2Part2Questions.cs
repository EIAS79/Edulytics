using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Original IM2021 G1 Unit 2 L04-L05 story and choice center,
/// no-action total and two-color partition tasks.
/// Integer responses are exact and verified against constrained source parameters.
/// </summary>
internal static class UsCcssG1Unit2Part2Questions
{
    public const string MixedStory="usccss.g1.u02.story.mixed_result_or_change";
    public const string CenterStory="usccss.g1.u02.center.picture_story_equation";
    public const string WriteMixedEquation="usccss.g1.u02.story.write_result_or_change_equation";
    public const string WritePictureEquation="usccss.g1.u02.center.write_picture_equation";
    private static readonly string[] Things=["books","cards","stickers"];
    public sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family)=>
        family is MixedStory or CenterStory or WriteMixedEquation or WritePictureEquation;

    public static Problem Build(string family,Random rng)
    {
        if(!Supports(family))throw new InvalidOperationException("Unknown G1 U02 Part2 family");
        string prompt = string.Empty, solution = string.Empty;
        var p=new Dictionary<string,int>(StringComparer.Ordinal);
        if(family is MixedStory or WriteMixedEquation)
        {
            var start=rng.Next(3,8);
            var change=rng.Next(1,4);
            var mode=rng.Next(3);
            var context=rng.Next(Things.Length);
            p["start"]=start;p["change"]=change;p["mode"]=mode;p["context"]=context;
            var noun=Things[context];
            if(mode==0)
                prompt=$"A child has {start} {noun}. A friend gives {change} more. "+
                    "How many are there now? Draw the starting group and the joining group. "+
                    $"Complete {start} + {change} = [ ? ]. Enter the result.";
            else if(mode==1)
                prompt=$"A child has {start} {noun}. A friend gives some more. "+
                    $"Now the child has {start+change} {noun}. How many new {noun} were given? "+
                    $"Complete {start} + [ ? ] = {start+change}. Enter the missing change, not the final total.";
            else
                prompt=$"A child has {start} {noun} and gives away {change}. "+
                    "How many remain? Show a picture where some leave and complete "+
                    $"{start} - {change} = [ ? ]. Enter the result.";
            if(family==WriteMixedEquation)
            {
                var question=mode switch
                {
                    0=>"Write a full equation including the ending number.",
                    1=>"The change was missing; show how many joined in your full equation.",
                    _=>"Write a full subtraction equation showing how many remain."
                };
                prompt=mode==0
                    ? $"A child starts with {start} {noun}; {change} more arrive. "+question
                    : mode==1
                    ? $"A child starts with {start} {noun}, then has {start+change}. "+question
                    : $"A child starts with {start} {noun} and gives away {change}. "+question;
                prompt+=" Type the numbers, operation and equals sign, not just the answer.";
            }
            solution="Show the story with objects; distinguish the result from the change. "+
                "Box the number that actually answers the question and verify your equation.";
        }
        else if(family is CenterStory or WritePictureEquation)
        {
            var start=rng.Next(4,9);
            var change=rng.Next(1,Math.Min(4,11-start));
            var action=rng.Next(2);
            var flip=rng.Next(2);
            p["start"]=start;p["change"]=change;p["action"]=action;p["flip"]=flip;
            // Student-visible original dot diagram: the question must actually
            // supply the picture instead of merely claiming one exists.
            // Text labels accompany the dots for screen readers and speech.
            var beforeDots=new string('●',start);
            var joiningDots=new string('○',change);
            var remainingDots=new string('●',start-change);
            var removedDots=new string('×',change);
            var story=action==0
                ? $"Dot picture (before): {beforeDots} ({start} balls). "+
                    $"Dot picture (change): {joiningDots} ({change} joining). "+
                    $"Dot picture (after): {beforeDots}{joiningDots}. "+
                    "The open circles are the newly arrived balls."
                : $"Dot picture (before): {beforeDots} ({start} balls). "+
                    $"Dot picture (change): {removedDots} ({change} removed). "+
                    $"Dot picture (after): {remainingDots}. "+
                    "The crossed-out symbols mean the balls rolled away.";
            var addition=$"{start} + {change} = {start+change}";
            var subtraction=$"{start} - {change} = {start-change}";
            var first=flip==0?addition:subtraction;
            var second=flip==0?subtraction:addition;
            prompt=family==WritePictureEquation
                ? story+" Tell your partner a complete math story from the picture. "+
                    "Write its addition or subtraction equation yourself, "+
                    "including the equals sign and final number."
                : story+" Tell your partner a complete math story and question. "+
                    $"Which equation matches your story? Enter 1 or 2: 1) {first}; 2) {second}.";
            solution="Explain what the picture's action means. More joining needs addition; "+
                "some leaving needs subtraction. Point to each amount in the chosen equation.";
        }
        return new Problem(family,prompt,solution,
            family is WriteMixedEquation or WritePictureEquation
                ? AssessmentItemType.ShortAnswer : AssessmentItemType.Numeric,p);
    }

    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))
            throw new InvalidOperationException("Invalid G1 U02 Part2 original source parameters");
        if(family==WriteMixedEquation)
        {
            var a=p["start"];var b=p["change"];
            return p["mode"]==2?$"{a}-{b}={a-b}":$"{a}+{b}={a+b}";
        }
        if(family==WritePictureEquation)
        {
            var a=p["start"];var b=p["change"];
            return p["action"]==0?$"{a}+{b}={a+b}":$"{a}-{b}={a-b}";
        }
        int answer=family switch
        {
            MixedStory=>p["mode"]==1?p["change"]:
                p["mode"]==2?p["start"]-p["change"]:p["start"]+p["change"],
            CenterStory=>(p["action"]==p["flip"])?1:2,
            _=>throw new InvalidOperationException("Unknown family")
        };
        return answer.ToString(CultureInfo.InvariantCulture);
    }
    private static string NormalizeEquation(string? answer)=>
        string.Concat((answer??string.Empty).Where(c=>!char.IsWhiteSpace(c)))
            .Replace('−','-').Replace('–','-').Replace('＋','+').Replace('＝','=');

    public static bool Verify(string family,IReadOnlyDictionary<string,int> parameters,string answer)=>
        Valid(family,parameters) &&
        string.Equals(
            family is WriteMixedEquation or WritePictureEquation
                ? NormalizeEquation(answer) : answer,
            Solve(family,parameters),StringComparison.Ordinal);

    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family))return false;
        if(family is MixedStory or WriteMixedEquation)
            return p.Count==4 &&
                p.TryGetValue("start",out var start) &&
                p.TryGetValue("change",out var change) &&
                p.TryGetValue("mode",out var mode) &&
                p.TryGetValue("context",out var context) &&
                start is >=3 and <=7 && change is >=1 and <=3 &&
                start+change<=10 && mode is >=0 and <=2 &&
                context is >=0 and <=2;
        if(family is CenterStory or WritePictureEquation)
            return p.Count==4 &&
                p.TryGetValue("start",out var cstart) &&
                p.TryGetValue("change",out var cchange) &&
                p.TryGetValue("action",out var action) &&
                p.TryGetValue("flip",out var flip) &&
                cstart is >=4 and <=8 && cchange is >=1 and <=3 &&
                cstart+cchange<=10 && action is 0 or 1 && flip is 0 or 1;
        return false;
    }
}
