using System.Globalization;

namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// G2 U01 L11 from original 2021: exactly FOUR questions about the TWO
/// graph data sets a learner used in the previous lesson, two per graph,
/// at least one combined or compared category question. Open wording,
/// partner response provenance and explanation remain teacher-scored.
/// </summary>
public static class UsCcssG2Unit1EleventhLessonGraphQuestions
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U01:L11";
    public const string Version="edulytics-g2-u01-l11-original-question-writing-v1";
    public static readonly IReadOnlyDictionary<string,int> PictureTable=
        UsCcssG2Unit1TenthLessonGraphConstruction.TableA;
    public static readonly IReadOnlyDictionary<string,int> BarTable=
        UsCcssG2Unit1TenthLessonGraphConstruction.TableB;

    public sealed class Draft
    {
        public string? Graph {get;set;}
        public string? Question {get;set;}
        public string? Operation {get;set;}
        public string? CategoryOne {get;set;}
        public string? CategoryTwo {get;set;}
        public string? PartnerAnswer {get;set;}
    }

    public sealed record Review(bool MachineScorable,bool Correct,
        bool TeacherReviewNeeded,string Feedback);

    public static Review CheckSourceQuestion(Draft? draft)
    {
        if(draft is null || string.IsNullOrWhiteSpace(draft.Question) ||
            draft.Question.Trim().Length is <8 or >500)
            return new(true,false,true,"Write a meaningful student-generated mathematical question.");
        var data=draft.Graph switch
        {
            "Picture"=>PictureTable,
            "Bar"=>BarTable,
            _=>null
        };
        if(data is null)return new(true,false,true,"Choose the graph that actually contains the category data.");
        if(!data.TryGetValue(draft.CategoryOne??"",out var a))
            return new(true,false,true,"The first category does not exist in that graph.");
        var expected=draft.Operation switch
        {
            "Count"=>a,
            "Combine" when data.TryGetValue(draft.CategoryTwo??"",out var b)
                && draft.CategoryOne!=draft.CategoryTwo=>a+b,
            "Compare" when data.TryGetValue(draft.CategoryTwo??"",out var c)
                && draft.CategoryOne!=draft.CategoryTwo=>Math.Abs(a-c),
            _=>-1
        };
        if(expected<0)return new(true,false,true,
            "Count one existing category, or combine/compare two DIFFERENT categories from this SAME graph.");
        if(!int.TryParse(draft.PartnerAnswer,NumberStyles.None,
            CultureInfo.InvariantCulture,out var answered))
            return new(true,false,true,"Ask a partner to provide one whole-number answer.");
        return new(true,answered==expected,true,answered==expected
            ?"Arithmetic matches this graph. Teacher reviews if the question text genuinely asks for the selected operation and whether a partner answered it."
            :"Compare the question with the correct graph and category counts; revise the proposed answer.");
    }

    public static Review CheckAll(IReadOnlyList<Draft>? questions)
    {
        if(questions is null||questions.Count!=4)
            return new(true,false,true,"Write exactly FOUR questions, not fewer or more.");
        if(questions.Count(x=>x.Graph=="Picture")!=2 ||
            questions.Count(x=>x.Graph=="Bar")!=2)
            return new(true,false,true,
                "Write exactly two questions for EACH graph, then mix their order.");
        if(!questions.Any(x=>x.Operation is "Combine" or "Compare"))
            return new(true,false,true,
                "At least one question must combine or compare TWO categories.");
        if(questions.Select(x=>x.Question?.Trim())
            .Distinct(StringComparer.OrdinalIgnoreCase).Count()!=4)
            return new(true,false,true,"Make the four original questions different.");
        var reviews=questions.Select(CheckSourceQuestion).ToArray();
        return new(true,reviews.All(r=>r.Correct),true,
            reviews.All(r=>r.Correct)
                ?"Four distinct graph questions and proposed numerical answers agree with the source tables. Open wording and genuine partner exchange still need teacher review."
                :"One or more selected graph operations or partner answers do not match their chart.");
    }

    public static Review CheckEdulyticsCoolDown(string? comparePicture,
        string? combineBar,string? threeAddends)
    {
        var raw=new[]{comparePicture,combineBar,threeAddends};
        if(raw.Any(x=>!int.TryParse(x,NumberStyles.None,
            CultureInfo.InvariantCulture,out _)))
            return new(true,false,false,"Enter three non-negative integer answers.");
        var values=raw.Select(x=>int.Parse(x!,CultureInfo.InvariantCulture)).ToArray();
        var correct=values.SequenceEqual(new[]{4,11,12});
        return new(true,correct,false,correct
            ?"Comics 6 minus Poetry 2 gives four; Music 4 plus Reading 7 gives eleven; 3+7+2 makes twelve."
            :"Use the picture graph for Comics versus Poetry, the bar graph for Music plus Reading, and make ten in 3+7+2.");
    }

    public static readonly IReadOnlyList<string> TeacherRubric=
    [
        "Four learner-authored DIFFERENT mathematical questions are recorded: two tied to the picture graph and two to the bar graph created in the previous lesson.",
        "At least one student-generated question asks to COMBINE or COMPARE two different categories; questions genuinely match selected operation metadata, not just numerical answers.",
        "Learner swaps these questions and both earlier diagrams with a different partner, who identifies the matching graph and explains how each answer was found.",
        "Teacher checks student question clarity, partner answers, revisions, mathematical justifications, and actual prior-lesson graph artifacts; no automated grade asserts a partner interaction occurred.",
        "Use prior graphs/tables or ask students to reproduce their own originals. Example counts from independently authored Edulytics Lesson 10 are fictional teaching data, not personal class observations."
    ];
}
