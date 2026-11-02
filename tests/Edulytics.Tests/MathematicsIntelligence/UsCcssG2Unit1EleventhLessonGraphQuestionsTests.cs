using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Stage C tests authored but NOT executed until Stage D.</summary>
public sealed class UsCcssG2Unit1EleventhLessonGraphQuestionsTests
{
    private static UsCcssG2Unit1EleventhLessonGraphQuestions.Draft Q(
        string graph,string question,string op,string a,string? b,string answer)=>
        new(){Graph=graph,Question=question,Operation=op,CategoryOne=a,
            CategoryTwo=b,PartnerAnswer=answer};

    [Fact]
    public void FourDistinctSourceQuestionsCoverTwoGraphsAndComparison()
    {
        var drafts=new[]{
            Q("Picture","How many selected Comics?","Count","Comics",null,"6"),
            Q("Bar","How many selected Music and Reading?","Combine","Music","Reading","11"),
            Q("Picture","How many more selected Animals than Poetry?","Compare","Animals","Poetry","3"),
            Q("Bar","How many selected Drawing?","Count","Drawing",null,"6")
        };
        var result=UsCcssG2Unit1EleventhLessonGraphQuestions.CheckAll(drafts);
        Assert.True(result.Correct);
        Assert.True(result.TeacherReviewNeeded);
    }

    [Fact]
    public void FourCountOnlyQuestionsDoNotFulfillOriginalLearningTask()
    {
        var drafts=new[]{
            Q("Picture","How many Comics?","Count","Comics",null,"6"),
            Q("Bar","How many Music?","Count","Music",null,"4"),
            Q("Picture","How many Science?","Count","Science",null,"3"),
            Q("Bar","How many Drawing?","Count","Drawing",null,"6")
        };
        Assert.False(UsCcssG2Unit1EleventhLessonGraphQuestions.CheckAll(drafts).Correct);
    }

    [Fact]
    public void QuestionCannotImportOtherGraphDataOrInventCategories()
    {
        Assert.False(UsCcssG2Unit1EleventhLessonGraphQuestions.CheckSourceQuestion(
            Q("Picture","How many chose Music?","Count","Music",null,"4")).Correct);
        Assert.False(UsCcssG2Unit1EleventhLessonGraphQuestions.CheckSourceQuestion(
            Q("Bar","How many liked Rain?","Count","Rain",null,"2")).Correct);
        Assert.False(UsCcssG2Unit1EleventhLessonGraphQuestions.CheckSourceQuestion(
            Q("Picture","How many selected Comics and Comics?","Combine","Comics","Comics","12")).Correct);
    }

    [Fact]
    public void FirstPartyCoolDownAndMakeTenFactAreExact()
    {
        Assert.True(UsCcssG2Unit1EleventhLessonGraphQuestions
            .CheckEdulyticsCoolDown("4","11","12").Correct);
        Assert.False(UsCcssG2Unit1EleventhLessonGraphQuestions
            .CheckEdulyticsCoolDown("4","10","12").Correct);
        Assert.Equal(5,UsCcssG2Unit1EleventhLessonGraphQuestions.TeacherRubric.Count);
    }
}
