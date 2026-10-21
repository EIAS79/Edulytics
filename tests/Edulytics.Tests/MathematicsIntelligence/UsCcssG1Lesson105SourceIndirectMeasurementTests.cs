using Edulytics.Data.Seeding;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Public 2021 U06 L03 source physical route and six furniture tasks are
/// teacher-assessed. Numeric problems below represent FIRST-PARTY
/// hypothetical measures, not original map/furniture measurements.
/// Tests are authored, not executed until deferred Stage D.
/// </summary>
public sealed class UsCcssG1Lesson105SourceIndirectMeasurementTests
{
    private const string Family="usccss.g1.u06.l03.choose_indirect_comparison";
    private static Dictionary<string,int> P(int a,int b,int mode)=>new()
    {
        ["a"]=a,["b"]=b,["c"]=0,["mode"]=mode
    };

    [Fact]
    public void OriginalSixFurniturePromptsArePresentInGuardedLearnerTeachingText()
    {
        var concepts=CommonCoreG1SourceGroundedInstructionalCorrections
            .ChooseIndirectLengthComparatorConcepts;
        var steps=CommonCoreG1SourceGroundedInstructionalCorrections
            .ChooseIndirectLengthComparatorSteps;
        Assert.Contains("Mai and Clare",concepts);
        Assert.Contains("TEACHER desk fit through the DOOR",concepts);
        Assert.Contains("STUDENT desk fit through the DOOR",concepts);
        Assert.Contains("BOOKSHELF or RUG",concepts);
        Assert.Contains("FILE CABINET or BOOKSHELF",concepts);
        Assert.Contains("BOOKSHELF or TEACHER DESK",concepts);
        Assert.Contains("NEXT TO the BOOKSHELF",concepts);
        Assert.Contains("SIX source furniture questions",steps);
        Assert.Contains("original graphics have not been copied",concepts);
    }

    [Theory]
    [InlineData(14,9,0,"LONGER=A")]
    [InlineData(9,14,0,"LONGER=B")]
    [InlineData(9,9,0,"LONGER=EQUAL")]
    [InlineData(14,9,1,"SHORTER=CLARE")]
    [InlineData(9,14,1,"SHORTER=MAI")]
    [InlineData(14,9,2,"FITS=YES")]
    [InlineData(9,14,2,"FITS=NO")]
    [InlineData(14,9,3,"FITS=YES")]
    [InlineData(9,14,3,"FITS=NO")]
    public void AuthoredSameReferenceUnitModelHasMathematicallyTrueComparisons(
        int referenceA,int referenceB,int mode,string expected)
    {
        var p=P(referenceA,referenceB,mode);
        Assert.True(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(Family,p));
        Assert.Equal(expected,UsCcssG1FullGradeUnit6MeasurementQuestions.Solve(Family,p));
        Assert.True(UsCcssG1FullGradeUnit6MeasurementQuestions.Verify(Family,p,expected));
        Assert.False(UsCcssG1FullGradeUnit6MeasurementQuestions.Verify(
            Family,p,expected+";ACTUAL_CLASSROOM=VERIFIED"));
    }

    [Fact]
    public void SeededQuestionsKeepActualTeacherMeasurementBoundaryClear()
    {
        var tasks=Enumerable.Range(0,1400).Select(i=>
            UsCcssG1FullGradeUnit6MeasurementQuestions.Build(
                Family,new Random(i*43+9))).ToArray();
        foreach(int mode in Enumerable.Range(0,4))
            Assert.Contains(tasks,x=>x.Parameters["mode"]==mode);
        Assert.All(tasks,q=>
        {
            Assert.True(UsCcssG1FullGradeUnit6MeasurementQuestions.Valid(Family,q.Parameters));
            Assert.True(UsCcssG1FullGradeUnit6MeasurementQuestions.Verify(Family,q.Parameters,
                UsCcssG1FullGradeUnit6MeasurementQuestions.Solve(Family,q.Parameters)));
            Assert.Contains("SOURCE-INSPIRED",q.Prompt);
        });
    }
}
