using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Read correct numerics from two independent Edulytics graph tables used
/// in Grade2 U01 L10. Original four STUDENT-generated questions and actual
/// partner exchange cannot be inferred from these generated numeric items.
/// </summary>
internal static class UsCcssG2Unit1Lesson11GraphQuestions
{
    public const string Count="usccss.g2.u01.l11.graph_question_count";
    public const string Combine="usccss.g2.u01.l11.graph_question_combine";
    public const string Compare="usccss.g2.u01.l11.graph_question_compare";
    public sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family)=>family is Count or Combine or Compare;

    private static readonly string[][] Categories=
    [
        ["Comics","Science","Animals","Poetry"],
        ["Music","Drawing","Soccer","Reading"],
        ["Painting","Chess","Gardening","Drama"]
    ];
    private static readonly int[][] Counts=
    [
        [6,3,5,2],
        [4,6,3,7],
        [2,5,7,4]
    ];

    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unknown G2 U01 L11 question family");
        var graph=random.Next(3);
        var first=random.Next(4);
        var second=random.Next(4);
        if(second==first)second=(second+1)%4;
        var p=new Dictionary<string,int>(StringComparer.Ordinal) {
            ["graph"]=graph,["first"]=first
        };
        string prompt;
        if(family==Count)
            prompt=$"Use the Grade2 Lesson10 {(graph==0?"PICTURE":"BAR")} graph of independent original Edulytics {Categories[graph][first]} data. " +
                $"How many chose {Categories[graph][first]}?";
        else
        {
            p["second"]=second;
            var nameA=Categories[graph][first];
            var nameB=Categories[graph][second];
            prompt=family==Combine
                ?$"Use the Grade2 Lesson10 {(graph==0?"PICTURE":"BAR")} graph. How many selected {nameA} or {nameB} altogether?"
                :$"Use the Grade2 Lesson10 {(graph==0?"PICTURE":"BAR")} graph. How many more selected {nameA} than {nameB}, or vice versa? Give the absolute difference.";
        }
        return new Problem(family,prompt,
            "Match the named graph and its category labels from previous lesson's Edulytics data table, then count, combine or compare. Student-created wording is checked separately by a teacher.",
            AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid Grade2 U01 L11 graph parameters");
        var a=Counts[p["graph"]][p["first"]];
        var value=family switch
        {
            Count=>a,
            Combine=>a+Counts[p["graph"]][p["second"]],
            Compare=>Math.Abs(a-Counts[p["graph"]][p["second"]]),
            _=>throw new InvalidOperationException("Unknown family")
        };
        return value.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)=>
        Valid(family,p) && string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family) || p.Count!=(family==Count?2:3))return false;
        if(!p.TryGetValue("graph",out var graph) || graph is <0 or >2 ||
            !p.TryGetValue("first",out var first) || first is <0 or >3)
            return false;
        return family==Count || p.TryGetValue("second",out var second) &&
            second is >=0 and <=3 && second!=first;
    }
}
