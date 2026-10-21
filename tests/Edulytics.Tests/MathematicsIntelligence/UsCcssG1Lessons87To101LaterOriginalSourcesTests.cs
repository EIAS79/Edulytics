using Edulytics.Core.Curriculum;
using Edulytics.Data.Seeding;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssG1Lessons87To101LaterOriginalSourcesTests
{
    [Fact]
    public void Lesson87TeachingContainsExactlyThreeTrueFalseSourceStatements()
    {
        var doc=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .Single(x=>x.Lessons.Any(l=>l.LessonCode=="PED:US-CCSS-MATH:G1:U04:L22"));
        CanonicalLessonContentMaterializer.Materialize(doc);
        var source=doc.Lessons.Single(l=>l.LessonCode=="PED:US-CCSS-MATH:G1:U04:L22");
        var body=source.Translations.Single(t=>t.CultureCode=="en");
        Assert.Contains("exactly THREE",body.Explanation);
        Assert.DoesNotContain("35<20",body.Explanation);
        foreach(var statement in new[]{"92=90+2","90+2>80+12","20+13<30+13"})
            Assert.Contains(statement,body.Explanation);
        Assert.Equal(CommonCoreG1FullGradeUnit5ContentCorrections.Unit4Lesson22SourceVersion,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(doc,source));
        Assert.True(CommonCoreG1FullGradeUnit5ContentCorrections.CanUpgradeExisting(
            doc,source,CommonCoreG1FullGradeUnit5ContentCorrections.Unit4EndSourceVersion));
    }

    [Theory]
    [InlineData("usccss.g1.u05.l08.center_add_within100",4,68,2)]
    [InlineData("usccss.g1.u05.l08.center_add_within100",5,3,68)]
    [InlineData("usccss.g1.u05.l08.center_add_within100",6,5,45)]
    [InlineData("usccss.g1.u05.l08.center_add_within100",7,45,7)]
    [InlineData("usccss.g1.u05.l09.two_digit_composing",4,38,2)]
    [InlineData("usccss.g1.u05.l09.two_digit_composing",5,40,3)]
    [InlineData("usccss.g1.u05.l09.two_digit_composing",6,38,5)]
    [InlineData("usccss.g1.u05.l09.two_digit_composing",7,38,15)]
    public void Lesson96And97SourceMentalArithmeticHasRealNumericAnswers(
        string family,int mode,int a,int b)
    {
        var p=new Dictionary<string,int>{{"a",a},{"b",b},{"mode",mode}};
        var yes=a%10+b%10>=10?"YES":"NO";
        var answer=$"NEW_TEN={yes};TOTAL={a+b}";
        Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(family,p));
        Assert.Equal(answer,UsCcssG1FullGradeUnit5AdditionQuestions.Solve(family,p));
        Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(family,p,answer));
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(family,p,
            $"NEW_TEN={(yes=="YES"?"NO":"YES")};TOTAL={a+b}"));
        p["a"]++;
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(family,p));
    }

    [Theory]
    [InlineData("usccss.g1.u05.l10.tens_tens_ones_ones",4,28,56)]
    [InlineData("usccss.g1.u05.l10.tens_tens_ones_ones",5,27,44)]
    [InlineData("usccss.g1.u05.l11.explain_addition_method",4,37,25)]
    [InlineData("usccss.g1.u05.l11.explain_addition_method",5,23,68)]
    [InlineData("usccss.g1.u05.l12.write_addition_equations",4,48,15)]
    [InlineData("usccss.g1.u05.l12.write_addition_equations",5,57,36)]
    public void Lesson98To100OriginalComposingExamplesRequireTensOnesAndTotal(
        string family,int mode,int a,int b)
    {
        var p=new Dictionary<string,int>{{"a",a},{"b",b},{"mode",mode}};
        var sum=a+b;
        var answer=$"TENS={sum/10};ONES={sum%10};TOTAL={sum}";
        Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(family,p));
        Assert.Equal(answer,UsCcssG1FullGradeUnit5AdditionQuestions.Solve(family,p));
        Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(family,p,answer));
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(family,p,
            $"TENS={sum/10};ONES={sum%10+1};TOTAL={sum}"));
        p["b"]++;
        Assert.False(UsCcssG1FullGradeUnit5AdditionQuestions.Valid(family,p));
    }

    [Fact]
    public void LaterSourceModesActuallyAppearInSeededQuestions()
    {
        foreach(var (family,maxMode) in new[]
        {
            ("usccss.g1.u05.l08.center_add_within100",7),
            ("usccss.g1.u05.l09.two_digit_composing",7),
            ("usccss.g1.u05.l10.tens_tens_ones_ones",5),
            ("usccss.g1.u05.l11.explain_addition_method",5),
            ("usccss.g1.u05.l12.write_addition_equations",5)
        })
        {
            var counts=new Dictionary<int,int>();
            for(int seed=0;seed<1300;seed++)
            {
                var task=UsCcssG1FullGradeUnit5AdditionQuestions.Build(
                    family,new Random(seed*41+17));
                var generatedMode=task.Parameters["mode"];
                counts[generatedMode]=counts.GetValueOrDefault(generatedMode)+1;
                Assert.True(UsCcssG1FullGradeUnit5AdditionQuestions.Verify(
                    family,task.Parameters,
                    UsCcssG1FullGradeUnit5AdditionQuestions.Solve(family,task.Parameters)));
                if(task.Parameters["mode"]>=4)
                    Assert.True(task.Prompt.Contains("ORIGINAL 2021",StringComparison.Ordinal) ||
                        task.Prompt.Contains("EDULYTICS FIRST-PARTY",StringComparison.Ordinal));
            }
            for(var mode=4;mode<=maxMode;mode++)
                Assert.True(counts.GetValueOrDefault(mode)>40);
        }
    }
}
