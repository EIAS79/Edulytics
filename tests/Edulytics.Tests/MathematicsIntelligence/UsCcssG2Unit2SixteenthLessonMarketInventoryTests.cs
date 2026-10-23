using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit2SixteenthLessonMarketInventoryTests
{
    [Fact]
    public void ExactlyOneBelowTenAndTwoAboveTenAreRequired()
    {
        Assert.True(UsCcssG2Unit2SixteenthLessonMarketInventory
            .CheckOriginalInventory("7","42","51").Correct);
        Assert.False(UsCcssG2Unit2SixteenthLessonMarketInventory
            .CheckOriginalInventory("10","40","50").Correct);
        Assert.False(UsCcssG2Unit2SixteenthLessonMarketInventory
            .CheckOriginalInventory("7","10","83").Correct);
        Assert.False(UsCcssG2Unit2SixteenthLessonMarketInventory
            .CheckOriginalInventory("7","42","52").Correct);
        Assert.False(UsCcssG2Unit2SixteenthLessonMarketInventory
            .CheckOriginalInventory("abc","42","51").Correct);
    }
    [Fact]
    public void FirstPartySalesMustMatchEachRemainderAndRestock()
    {
        var result=UsCcssG2Unit2SixteenthLessonMarketInventory.FirstPartySales(
            "7","42","51","3","23","25","4","19","26","51","49","51");
        Assert.True(result.Correct);
        Assert.False(result.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit2SixteenthLessonMarketInventory.FirstPartySales(
            "7","42","51","3","23","25","4","20","26","51","49","51").Correct);
        var exit=UsCcssG2Unit2SixteenthLessonMarketInventory.CheckIndependentExit("17","19");
        Assert.True(exit.Correct);
        Assert.False(UsCcssG2Unit2SixteenthLessonMarketInventory.CheckIndependentExit("19","17").Correct);
        Assert.True(UsCcssG2Unit2SixteenthLessonMarketInventory.TeacherRubric.Count>=6);
    }
}
