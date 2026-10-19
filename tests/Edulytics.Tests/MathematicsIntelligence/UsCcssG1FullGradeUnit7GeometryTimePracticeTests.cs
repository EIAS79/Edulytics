using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG1FullGradeUnit7GeometryTimePracticeTests
{
    [Fact]
    public void EveryOriginalUnit7LessonHasAnExplicitTextOnlyDigitalContract()
    {
        Assert.Equal(17,UsCcssG1FullGradeUnit7GeometryTimeQuestions.Families.Count);
        for(int n=1;n<=17;n++)
        {
            var code=$"PED:US-CCSS-MATH:G1:U07:L{n:00}";
            var contract=LessonPracticeContractRegistry.All.Single(x=>x.LessonCode==code);
            var f=Assert.Single(contract.AllowedQuestionFamilies);
            Assert.True(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Supports(f));
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var choice));
            Assert.Equal(LessonPracticeInteractionKind.ContractDriven,choice!.InteractionKind);
        }
    }
    [Fact]
    public void GeometryAndTimeChecksPassCorrectAnswersAndRejectFalseClaims()
    {
        foreach(var f in UsCcssG1FullGradeUnit7GeometryTimeQuestions.Families)
            for(int seed=0;seed<110;seed++)
            {
                var q=UsCcssG1FullGradeUnit7GeometryTimeQuestions.Build(f,new Random(seed*53+11));
                var answer=UsCcssG1FullGradeUnit7GeometryTimeQuestions.Solve(f,q.Parameters);
                Assert.True(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Valid(f,q.Parameters));
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,answer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,answer+";FAKE=1"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"TIME=99:99"));
            }
    }
    [Fact]
    public void TextualPracticePromptsDoNotProvidePreFilledCorrectAnswers()
    {
        foreach(var family in UsCcssG1FullGradeUnit7GeometryTimeQuestions.Families)
        {
            for(int seed=0;seed<60;seed++)
            {
                var q=UsCcssG1FullGradeUnit7GeometryTimeQuestions.Build(family,new Random(seed*17+5));
                var answer=UsCcssG1FullGradeUnit7GeometryTimeQuestions.Solve(family,q.Parameters);
                Assert.DoesNotContain("Write "+answer+".",q.Prompt,StringComparison.Ordinal);
            }
        }
    }
    [Fact]
    public void ClocksUseWholeHoursAndHalfHoursNotUnintroducedQuarterHours()
    {
        const string f="usccss.g1.u07.l14.half_hour_clock";
        var p=new Dictionary<string,int>{{"a",7},{"b",3},{"c",0},{"mode",1}};
        Assert.Equal("MINUTES=30;HOUR=7",UsCcssG1FullGradeUnit7GeometryTimeQuestions.Solve(f,p));
        Assert.False(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Verify(f,p,"MINUTES=15;HOUR=7"));
    }
    [Theory]
    [InlineData(7,3,0,"RESULT=7")]
    [InlineData(7,3,1,"SHAPE=RECTANGLE")]
    public void OriginalU07L17CenterUsesShapeOrArithmeticRatherThanOffTopicClock(
        int a,int b,int mode,string answer)
    {
        const string family="usccss.g1.u07.l17.center_geometry_and_time";
        var p=new Dictionary<string,int>{{"a",a},{"b",b},{"c",0},{"mode",mode}};
        Assert.Equal(answer,UsCcssG1FullGradeUnit7GeometryTimeQuestions.Solve(family,p));
        Assert.True(UsCcssG1FullGradeUnit7GeometryTimeQuestions.Verify(family,p,answer));
        Assert.DoesNotContain("minute hand",UsCcssG1FullGradeUnit7GeometryTimeQuestions.Build(
            family,new Random(17)).Prompt,StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void AllTextualGeometryAndTimePromptsOfferTenDistinctCases()
    {
        var e=new ExactSkillContractQuestionEngine();
        foreach(var f in UsCcssG1FullGradeUnit7GeometryTimeQuestions.Families)
        {
            var qs=e.Generate("g1-u07",f,[f],ExactSkillQuestionDifficulty.Standard,10,20261019,[]);
            Assert.Equal(10,qs.Count);
            Assert.Equal(10,qs.Select(x=>x.Prompt).Distinct(StringComparer.Ordinal).Count());
        }
    }
}
