using Edulytics.Services.Mathematics;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>G5 U03:L10 source pretzel and divide-by-three AUTHORED_NOT_RUN.</summary>
public sealed class UsCcssG5U03L10SharingQuestionsTests
{
    private static Dictionary<string,int> P(int c,int s=0)=>
        new(StringComparer.Ordinal){["case"]=c,["slot"]=s};
    [Theory]
    [InlineData(0,0,"1")]
    [InlineData(1,1,"2")]
    [InlineData(2,2,"3")]
    [InlineData(3,0,"1")]
    [InlineData(4,1,"2")]
    [InlineData(5,2,"3")]
    [InlineData(6,0,"1")]
    public void OriginalAllSevenSharingOrDivisionCases(int c,int s,string expected)
    {
        foreach(var f in new[]{UsCcssG5U03L10SharingQuestions.PretzelFamily,
                                UsCcssG5U03L10SharingQuestions.PatternFamily})
        {
            Assert.Equal(expected,UsCcssG5U03L10SharingQuestions.Solve(f,P(c,s)));
            Assert.True(UsCcssG5U03L10SharingQuestions.Verify(f,P(c,s),expected));
            Assert.False(UsCcssG5U03L10SharingQuestions.Verify(f,P(c,s),"0"));
        }
    }
    [Fact]
    public void MalformedAndTeacherReview()
    {
        foreach(var f in new[]{UsCcssG5U03L10SharingQuestions.PretzelFamily,
                                UsCcssG5U03L10SharingQuestions.PatternFamily})
        {
            foreach(var p in new[]{P(-1),P(7),P(0,-1),P(0,3)})
                Assert.False(UsCcssG5U03L10SharingQuestions.Verify(f,p,"1"));
            var extra=P(0);extra["foreign"]=5;
            Assert.False(UsCcssG5U03L10SharingQuestions.Verify(f,extra,"1"));
            Assert.False(UsCcssG5U03L10SharingQuestions.Verify(f,null,"1"));
            Assert.False(UsCcssG5U03L10SharingQuestions.Verify(f,P(0)," 1"));
            Assert.Contains("TEACHER_REVIEW_REQUIRED",
                UsCcssG5U03L10SharingQuestions.Build(f,new Random(6)).Prompt);
        }
    }
}
