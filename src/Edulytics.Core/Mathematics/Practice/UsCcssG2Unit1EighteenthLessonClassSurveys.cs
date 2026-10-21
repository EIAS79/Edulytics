using System.Globalization;
using System.Text.RegularExpressions;

namespace Edulytics.Core.Mathematics.Practice;

/// <summary>First-party survey representation checks only. A typed response does not
/// prove collection from classmates or the originality of student questions.</summary>
public static class UsCcssG2Unit1EighteenthLessonClassSurveys
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U01:L18";
    public const string Version="edulytics-g2-u01-l18-student-authored-class-survey-v1";
    public static readonly IReadOnlyList<string> TeacherRubric=[
        "Observe partner-created survey question with exactly three or four mutually exclusive response categories",
        "Confirm learners actually surveyed classmates, anonymously and without recording names",
        "Check each real response counted once; the data must correspond to actual participants",
        "Review student's OWN title, category labels, one-per-symbol key OR zero-based equal-tick bar graph",
        "Verify an actual other pair exchanged graphs and authored combine/compare questions",
        "Review an independent original tape drawing that compares exactly two facts from the real survey",
        "Listen to partners interpret the full graph vs two-part tape diagram; numeric check cannot certify class participation"
    ];
    public sealed record Survey(string Question,string GraphType,string Title,string[] Labels,int[] Counts)
    {
        public int Total=>Counts.Sum();
    }
    public sealed record Review(bool? Correct,bool TeacherReviewNeeded,string Feedback);
    public static bool TryReadSurvey(string? question,string? graphType,string? title,
        string? numCategories,IReadOnlyList<string?> names,IReadOnlyList<string?> values,
        out Survey? survey)
    {
        survey=null;
        if((numCategories!="3"&&numCategories!="4")||names.Count!=4||values.Count!=4||
           (graphType!="Picture"&&graphType!="Bar")||
           string.IsNullOrWhiteSpace(question)||question.Trim().Length>160||
           string.IsNullOrWhiteSpace(title)||title.Trim().Length>120)return false;
        var length=numCategories=="3"?3:4;
        if(length==3&&(!string.IsNullOrWhiteSpace(names[3])||
           (!string.IsNullOrWhiteSpace(values[3])&&values[3]!="0")))return false;
        var categories=new string[length];var counts=new int[length];
        for(var i=0;i<length;i++)
        {
            var name=names[i]?.Trim();
            if(string.IsNullOrWhiteSpace(name)||name.Length>60||!TryRead(values[i],0,60,out var count))
                return false;
            categories[i]=name;counts[i]=count;
        }
        if(categories.Distinct(StringComparer.OrdinalIgnoreCase).Count()!=length ||
           counts.Sum() is <1 or >60)return false;
        survey=new Survey(question.Trim(),graphType,title.Trim(),categories,counts);
        return true;
    }
    public static Review CheckGraph(Survey? survey,string? pictureKey,string? axisStart,string? axisStep)
    {
        if(survey is null)return new(false,true,"Provide three or four valid, uniquely labeled category counts.");
        var correct=survey.GraphType switch {
            "Picture"=>TryRead(pictureKey,1,1,out var key)&&key==1,
            "Bar"=>TryRead(axisStart,0,0,out var origin)&&origin==0&&
                   TryRead(axisStep,1,1,out var step)&&step==1,
            _=>false
        };
        return new(correct,true,correct
            ?"The submitted graph labels and scale rules are consistent with these numbers; teacher must verify the REAL survey, picture or hand-drawn bars."
            :"Picture graphs need a one-response symbol key; bar graphs need zero baseline and equal tick interval 1.");
    }
    public static Review CheckPair(Survey? survey,string? a,string? b,
        string? combineAnswer,string? combineEquation,string? compareAnswer,string? compareEquation,
        string? tapeGap,string? tapeEquation)
    {
        if(survey is null)return new(null,true,"The survey categories and counts must be valid before checking pair equations.");
        if(!TryRead(a,1,survey.Counts.Length,out var ia)||
           !TryRead(b,1,survey.Counts.Length,out var ib)||ia==ib)
            return new(false,true,"Choose two DIFFERENT existing category numbers.");
        var left=survey.Counts[ia-1];var right=survey.Counts[ib-1];
        var big=Math.Max(left,right);var small=Math.Min(left,right);
        var sum=left+right;var diff=big-small;
        var sums=new HashSet<string>(StringComparer.Ordinal){
            $"{left}+{right}={sum}",$"{right}+{left}={sum}"};
        var diffs=new HashSet<string>(StringComparer.Ordinal){
            $"{big}-{small}={diff}",$"{small}+{diff}={big}",
            $"{diff}+{small}={big}"};
        var correct=TryRead(combineAnswer,0,60,out var c)&&c==sum&&
            TryRead(compareAnswer,0,60,out var d)&&d==diff&&
            TryRead(tapeGap,0,60,out var t)&&t==diff&&
            sums.Contains(Normalize(combineEquation))&&
            diffs.Contains(Normalize(compareEquation))&&
            diffs.Contains(Normalize(tapeEquation));
        return new(correct,true,correct
            ?"The supplied two-category arithmetic, equations and numeric tape gap are consistent with the ENTERED responses. Teacher checks the original peer questions and actual tape drawing."
            :"The two selected categories must combine to their sum and compare with a gap; include true addition/subtraction equations.");
    }
    public static Review CheckCoolDown(string? total,string? combine,string? compare,string? tape)
    {
        var good=TryRead(total,0,60,out var t)&&t==22
            &&TryRead(combine,0,60,out var s)&&s==10
            &&TryRead(compare,0,60,out var d)&&d==6
            &&TryRead(tape,0,60,out var g)&&g==3;
        return new(good,false,good
            ?"Independent first-party fictional survey: 22 total; 6+4=10; 9-3=6; 9-6=3."
            :"Check the fictional four-category graph: Stories9, Comics6, Poetry4, Nonfiction3.");
    }
    private static string Normalize(string? s)=>Regex.Replace(s??"",@"\s+","")
        .Replace('−','-');
    private static bool TryRead(string? text,int min,int max,out int n)=>
        int.TryParse(text,NumberStyles.None,CultureInfo.InvariantCulture,out n)&&n>=min&&n<=max;
}
