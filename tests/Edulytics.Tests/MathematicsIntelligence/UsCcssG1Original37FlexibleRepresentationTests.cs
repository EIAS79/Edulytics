using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Original 2021 Grade 1 U04 L19 asks for any different ways of
/// composing 37 with complete tens and loose ones.</summary>
public sealed class UsCcssG1Original37FlexibleRepresentationTests
{
    [Fact]
    public void StudentMayChooseAnyTwoDistinctEquivalentFormsInEitherOrder()
    {
        const string family="usccss.g1.u04.l19.regroup_one_ten";
        var source=new Dictionary<string,int>
        {
            ["a"]=37,["b"]=0,["c"]=0,["mode"]=1
        };
        var representations=new[]
        {
            (Tens:3, Ones:7),
            (Tens:2, Ones:17),
            (Tens:1, Ones:27),
            (Tens:0, Ones:37)
        };
        foreach(var first in representations)
            foreach(var second in representations)
            {
                var answer=$"TENS={first.Tens};ONES={first.Ones};ALT_TENS={second.Tens};ALT_ONES={second.Ones}";
                Assert.Equal(first.Tens!=second.Tens,
                    UsCcssG1FullGradeUnit4FinalQuestions.Verify(family,source,answer));
            }
        foreach(var invalid in new[]
        {
            "TENS=2;ONES=18;ALT_TENS=1;ALT_ONES=27",
            "TENS=0;ONES=37;ALT_TENS=1;ALT_ONES=26",
            "TENS=-1;ONES=47;ALT_TENS=3;ALT_ONES=7",
            "TENS=3;ONES=7;ALT_TENS=1;ALT_ONES=27;EXTRA=1"
        })
            Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Verify(family,source,invalid));
    }
}
