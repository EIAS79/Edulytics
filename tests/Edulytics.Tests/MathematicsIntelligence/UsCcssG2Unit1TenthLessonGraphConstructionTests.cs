using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Stage C source tests written only, no execution until D.</summary>
public sealed class UsCcssG2Unit1TenthLessonGraphConstructionTests
{
    [Fact]
    public void OriginalPictureAndBarTasksMatchTheirOwnIndependentTables()
    {
        Assert.True(UsCcssG2Unit1TenthLessonGraphConstruction.VerifyPictureGraph(
            1,"Our favorite books","●","1",["6","3","5","2"]).Correct);
        Assert.True(UsCcssG2Unit1TenthLessonGraphConstruction.VerifyBarGraph(
            2,"Our club choices","0","1",["4","6","3","7"]).Correct);
        Assert.False(UsCcssG2Unit1TenthLessonGraphConstruction.VerifyBarGraph(
            2,"Our club choices","0","1",["6","3","5","2"]).Correct);
    }

    [Fact]
    public void FalseKeysAndUnequalScalesAreRejected()
    {
        Assert.False(UsCcssG2Unit1TenthLessonGraphConstruction.VerifyPictureGraph(
            1,"Favorite books","●","2",["6","3","5","2"]).Correct);
        Assert.False(UsCcssG2Unit1TenthLessonGraphConstruction.VerifyBarGraph(
            2,"Favorite clubs","1","1",["4","6","3","7"]).Correct);
        Assert.False(UsCcssG2Unit1TenthLessonGraphConstruction.VerifyBarGraph(
            2,"Favorite clubs","0","2",["4","6","3","7"]).Correct);
    }

    [Fact]
    public void IndependentExitTicketUsesSameTableForBothRepresentations()
    {
        var data=UsCcssG2Unit1TenthLessonGraphConstruction.ExitTicketTable;
        Assert.Equal(15,data.Values.Sum());
        Assert.True(UsCcssG2Unit1TenthLessonGraphConstruction.VerifyPictureGraph(
            3,"School lunch choices","★","1",["4","6","3","2"]).Correct);
        Assert.True(UsCcssG2Unit1TenthLessonGraphConstruction.VerifyBarGraph(
            3,"School lunch choices","0","1",["4","6","3","2"]).Correct);
        Assert.False(UsCcssG2Unit1TenthLessonGraphConstruction.VerifyBarGraph(
            3,"School lunch choices","0","1",["4","5","3","3"]).Correct);
    }
}
