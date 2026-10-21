using Edulytics.Core.Curriculum;
using Edulytics.Data.Seeding;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssG1Lesson145OriginalRiddlesTests
{
    private const string Family="usccss.g1.u08.l09.solve_number_riddle";

    [Theory]
    [InlineData(4,65,35,"TRUE=YES;LEFT=65;RIGHT=35")]
    [InlineData(5,65,65,"TRUE=YES;LEFT=65;RIGHT=65")]
    [InlineData(6,65,65,"TRUE=NO;LEFT=65;RIGHT=65")]
    [InlineData(7,34,44,"CANDIDATES=34,44,54")]
    [InlineData(8,24,20,"NUMBER=44")]
    [InlineData(9,51,32,"NUMBER=83")]
    public void EveryOriginalSourceWarmupAndBothFinalCluesAreIndependentlyCorrect(
        int mode,int a,int b,string answer)
    {
        var p=new Dictionary<string,int>{{"a",a},{"b",b},{"c",0},{"mode",mode}};
        Assert.True(UsCcssG1FullGradeUnit8ReviewQuestions.Valid(Family,p));
        Assert.Equal(answer,UsCcssG1FullGradeUnit8ReviewQuestions.Solve(Family,p));
        Assert.True(UsCcssG1FullGradeUnit8ReviewQuestions.Verify(Family,p,answer));
        Assert.False(UsCcssG1FullGradeUnit8ReviewQuestions.Verify(Family,p,answer+";EXTRA=1"));
        Assert.False(UsCcssG1FullGradeUnit8ReviewQuestions.Verify(Family,p,"NUMBER=999"));
        p["a"]++;
        Assert.False(UsCcssG1FullGradeUnit8ReviewQuestions.Valid(Family,p));
    }

    [Fact]
    public void Lesson145LearnerTeachingUsesActualPublisherArithmeticClues()
    {
        var doc=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .Single(d=>d.Lessons.Any(l=>l.LessonCode=="PED:US-CCSS-MATH:G1:U08:L09"));
        CanonicalLessonContentMaterializer.Materialize(doc);
        var lesson=doc.Lessons.Single(l=>l.LessonCode=="PED:US-CCSS-MATH:G1:U08:L09");
        Assert.True(CommonCoreG1FullGradeUnit6To8ContentCorrections.IsTarget(doc,lesson));
        var examples=lesson.Translations.Single(t=>t.CultureCode=="en").WorkedExamples;
        foreach(var source in new[]{"34, 44, 54","24 and 20","51+32","65>35","65=75-10","65>35+30"})
            Assert.Contains(source,examples);
        Assert.Equal(CommonCoreG1FullGradeUnit6To8ContentCorrections.Unit8Lesson9SourceVersion,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(doc,lesson));
        Assert.True(CommonCoreG1FullGradeUnit6To8ContentCorrections.CanUpgradeExisting(
            doc,lesson,CommonCoreG1FullGradeUnit6To8ContentCorrections.Version));
    }

    [Fact]
    public void FixedOriginalRiddlesAreGeneratedInAdditionToPriorGenericRiddlePractice()
    {
        var reached=new HashSet<int>();
        for(var seed=0;seed<1200;seed++)
        {
            var q=UsCcssG1FullGradeUnit8ReviewQuestions.Build(
                Family,new Random(seed*41+15));
            int mode=q.Parameters["mode"];
            reached.Add(mode);
            Assert.True(UsCcssG1FullGradeUnit8ReviewQuestions.Valid(Family,q.Parameters));
            Assert.True(UsCcssG1FullGradeUnit8ReviewQuestions.Verify(Family,
                q.Parameters,UsCcssG1FullGradeUnit8ReviewQuestions.Solve(Family,q.Parameters)));
            if(mode>=4)Assert.Contains("ORIGINAL 2021",q.Prompt);
        }
        foreach(var mode in Enumerable.Range(0,10))Assert.Contains(mode,reached);
    }
}
