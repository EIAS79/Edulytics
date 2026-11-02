using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Web.ViewModels.LessonContent;

public sealed class Grade2Lesson11Answers
{
    public List<UsCcssG2Unit1EleventhLessonGraphQuestions.Draft> Questions {get;set;}=
        [new(),new(),new(),new()];
    public string? CoolPictureDifference {get;set;}
    public string? CoolBarCombined {get;set;}
    public string? CoolNumberTalk {get;set;}
    public string? PartnerReflection {get;set;}
}
public sealed record Grade2Lesson11PracticePage(
    Guid AdoptionId,Guid LessonId,string LessonTitle,bool Submitted,
    Grade2Lesson11Answers Answers,
    IReadOnlyList<Grade2Lesson11Feedback> Feedback);
public sealed record Grade2Lesson11Feedback(
    string Task,bool? Correct,bool TeacherReviewNeeded,string Feedback);
