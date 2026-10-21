using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Unit 4 L20 first-party mathematical representation of 2021 public student tasks.
/// Source-specific assertions authored without running Stage D or CI.
/// </summary>
public sealed class UsCcssG1Lesson85Original94WaysAndMysteryBagTests
{
    private const string Family="usccss.g1.u04.l20.equivalent_tens_ones";

    [Fact]
    public void AllTenDifferentTensCountsForOriginalNinetyFourAreMathematicallyAllowed()
    {
        var p=new Dictionary<string,int>{{"a",94},{"b",0},{"c",0},{"mode",2}};
        Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Valid(Family,p));
        for(int altTens=0;altTens<=8;altTens++)
        {
            int altOnes=94-10*altTens;
            var answer=$"NUMBER=94;TENS=9;ONES=4;ALT_TENS={altTens};ALT_ONES={altOnes};WAYS=10";
            Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Verify(Family,p,answer));
            Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Verify(Family,p,
                $"NUMBER=94;TENS=9;ONES=4;ALT_TENS={altTens};ALT_ONES={altOnes+1};WAYS=10"));
        }
        Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Verify(Family,p,
            "NUMBER=94;TENS=9;ONES=4;ALT_TENS=9;ALT_ONES=4;WAYS=10"));
        Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Verify(Family,p,
            "NUMBER=94;TENS=9;ONES=4;ALT_TENS=0;ALT_ONES=94;WAYS=9"));
    }

    [Fact]
    public void OriginalMysteryBagSourceNumbersHaveStrictRightWrongAndOperandGuards()
    {
        foreach(var (mode,a,answer,bad) in new[]{
            (4,52,"NUMBER=52","NUMBER=62"),
            (5,65,"NUMBER=65","NUMBER=45"),
            (6,49,"TENS=2","TENS=4"),
            (7,36,"ONES=16","ONES=6")
        })
        {
            var p=new Dictionary<string,int>{{"a",a},{"b",0},{"c",0},{"mode",mode}};
            Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Valid(Family,p));
            Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Verify(Family,p,answer));
            Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Verify(Family,p,bad));
            Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Verify(Family,p,answer+";EXTRA=1"));
            p["a"]=a+1;
            Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Valid(Family,p));
        }
    }
}
