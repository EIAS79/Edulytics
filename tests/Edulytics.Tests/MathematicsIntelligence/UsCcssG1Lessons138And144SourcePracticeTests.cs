using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssG1Lessons138And144SourcePracticeTests
{
    [Theory]
    [InlineData(4,5,4,9)]
    [InlineData(5,4,5,9)]
    [InlineData(6,9,4,5)]
    [InlineData(7,9,5,4)]
    public void OriginalRelatedAdditionSubtractionFactsAreExact(int mode,int a,int b,int answer)
    {
        const string family="usccss.g1.u08.l02.inverse_add_subtract";
        var p=new Dictionary<string,int>{{"a",a},{"b",b},{"c",0},{"mode",mode}};
        Assert.True(UsCcssG1FullGradeUnit8ReviewQuestions.Valid(family,p));
        Assert.Equal($"RESULT={answer}",UsCcssG1FullGradeUnit8ReviewQuestions.Solve(family,p));
        Assert.True(UsCcssG1FullGradeUnit8ReviewQuestions.Verify(family,p,$"RESULT={answer}"));
        Assert.False(UsCcssG1FullGradeUnit8ReviewQuestions.Verify(family,p,$"RESULT={answer+1}"));
        p["a"]++;
        Assert.False(UsCcssG1FullGradeUnit8ReviewQuestions.Valid(family,p));
    }

    [Fact]
    public void NumberBookSourceRequires20To100AndThreeDistinctValidExpressions()
    {
        const string family="usccss.g1.u08.l08.same_number_tens_ones";
        foreach(var a in new[]{20,25,99,100})
        {
            var p=new Dictionary<string,int>{{"a",a},{"b",0},{"c",0},{"mode",5}};
            Assert.True(UsCcssG1FullGradeUnit8ReviewQuestions.Valid(family,p));
            var expected=$"NUMBER={a};E1=0+{a};E2=5+{a-5};E3=10+{a-10}";
            Assert.Equal(expected,UsCcssG1FullGradeUnit8ReviewQuestions.Solve(family,p));
            Assert.True(UsCcssG1FullGradeUnit8ReviewQuestions.Verify(family,p,expected));
            // Teacher-verified authored expressions can differ from the scaffold.
            Assert.True(UsCcssG1FullGradeUnit8ReviewQuestions.Verify(family,p,
                $"NUMBER={a};E1=1+{a-1};E2=2+{a-2};E3=3+{a-3}"));
            Assert.False(UsCcssG1FullGradeUnit8ReviewQuestions.Verify(family,p,
                $"NUMBER={a};E1=1+{a-1};E2={a-1}+1;E3=3+{a-3}"));
            Assert.False(UsCcssG1FullGradeUnit8ReviewQuestions.Verify(family,p,expected+";EXTRA=0"));
        }
        foreach(var invalidNumber in new[]{1,10,19,101,110,120,121})
        {
            var p=new Dictionary<string,int>{{"a",invalidNumber},{"b",0},{"c",0},{"mode",5}};
            Assert.False(UsCcssG1FullGradeUnit8ReviewQuestions.Valid(family,p));
        }
        // This automated numerical check does not certify actual student handwriting or a physical number book.
    }
    [Fact]
    public void OriginalNewModesAreSeededAndExistingModesStillGenerate()
    {
        foreach(var (family,maxMode) in new[]
        {
            ("usccss.g1.u08.l02.inverse_add_subtract",7),
            ("usccss.g1.u08.l08.same_number_tens_ones",4)
        })
        {
            var seen=new HashSet<int>();
            for(int i=0;i<1000;i++)
            {
                var q=UsCcssG1FullGradeUnit8ReviewQuestions.Build(
                    family,new Random(i*17+20261020));
                seen.Add(q.Parameters["mode"]);
                Assert.True(UsCcssG1FullGradeUnit8ReviewQuestions.Verify(
                    family,q.Parameters,UsCcssG1FullGradeUnit8ReviewQuestions.Solve(family,q.Parameters)));
            }
            foreach(var mode in Enumerable.Range(0,maxMode+1))Assert.Contains(mode,seen);
        }
    }
}
