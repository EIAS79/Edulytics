using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssG1Lessons89To91OriginalEquationTests
{
    private static readonly (int a,int b)[] Unit5Lesson1=
    {
        (43,5),(43,50),(51,3),(51,30),(2,75),(20,75),(93,6),(60,28),(5,74)
    };
    private static readonly (int a,int b)[] Unit5Lesson2=
    {
        (23,45),(63,25)
    };
    private static readonly (int a,int b)[] Unit5Lesson3=
    {
        (13,21),(13,31),(23,31),(33,41),(32,54),(23,74)
    };

    [Fact]
    public void AllNineOriginal2021TensOrOnesStudentEquationsAreActuallyGradeable()
    {
        const string family="usccss.g1.u05.l01.tens_or_ones_no_compose";
        for(var i=0;i<Unit5Lesson1.Length;i++)
        {
            var (a,b)=Unit5Lesson1[i];
            var p=new Dictionary<string,int>{{"a",a},{"b",b},{"mode",i+4}};
            var answer=$"NEW_TEN=NO;TOTAL={a+b}";
            Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(family,p));
            Assert.Equal(answer,UsCcssG1FullGradeUnit5AdditionQuestions.Solve(family,p));
            Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(family,p,answer));
            Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(family,p,
                $"NEW_TEN=YES;TOTAL={a+b}"));
            Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(family,p,answer+";EXTRA=1"));
            p["a"]++;
            Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(family,p));
        }
    }

    [Theory]
    [InlineData("usccss.g1.u05.l02.explain_no_compose_equations",2)]
    [InlineData("usccss.g1.u05.l03.write_no_compose_equations",3)]
    public void OriginalStrategiesTeachPlaceValueButMathGradingRequiresAllComponents(
        string family,int lesson)
    {
        var cases=lesson==2?Unit5Lesson2:Unit5Lesson3;
        for(var i=0;i<cases.Length;i++)
        {
            var (a,b)=cases[i];
            var p=new Dictionary<string,int>{{"a",a},{"b",b},{"mode",i+4}};
            var ans=$"TENS={(a/10+b/10)*10};ONES={a%10+b%10};TOTAL={a+b}";
            Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(family,p));
            Assert.Equal(ans,UsCcssG1FullGradeUnit5AdditionQuestions.Solve(family,p));
            Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(family,p,ans));
            Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(family,p,
                $"TENS={(a/10+b/10)*10};ONES={a%10+b%10};TOTAL={a+b+1}"));
            p["b"]++;
            Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(family,p));
        }
    }

    [Fact]
    public void NewModesActuallyGenerateTogetherWithPreservedGenericModes()
    {
        foreach(var (family,lastMode) in new[]
        {
            ("usccss.g1.u05.l01.tens_or_ones_no_compose",12),
            ("usccss.g1.u05.l02.explain_no_compose_equations",5),
            ("usccss.g1.u05.l03.write_no_compose_equations",9)
        })
        {
            var counts=new Dictionary<int,int>();
            for(var seed=0;seed<2600;seed++)
            {
                var q=UsCcssG1FullGradeUnit5AdditionQuestions.Build(
                    family,new Random(seed*17+31));
                var generatedMode=q.Parameters["mode"];
                counts[generatedMode]=counts.GetValueOrDefault(generatedMode)+1;
                Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(
                    family,q.Parameters,UsCcssG1FullGradeUnit5AdditionQuestions.Solve(family,q.Parameters)));
                if(q.Parameters["mode"]>=4)Assert.Contains("ORIGINAL 2021",q.Prompt);
            }
            for(var mode=0;mode<=lastMode;mode++)
                Assert.True(counts.GetValueOrDefault(mode)>35);
        }
    }
}
