using System.Globalization;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Data.Seeding;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Independent oracles for three source-exact B01 task families.
/// This is mathematical/runtime evidence only; never auto-certify L1-L6.
/// </summary>
public sealed class UsCcssB01V9ThreeSourcePracticeTargetsTests
{
    private const string Prefix="PED:US-CCSS-MATH:";
    private static readonly (string Suffix,string Family,string Title,string RawSha)[] Targets =
    [
        ("G1:U08:L09","usccss.b01.g1.place_value_number_riddle",
         "Solve Number Riddles","dd74764b87e53b0d6900a35e30a905dfc05d30acab9db978ed2e9ac11626720b"),
        ("G2:U03:L13","usccss.b01.g2.target_measurement_difference_cm",
         "Center Day 2 (optional)","5fbacc4927f2d2e2ed57f65965b4ffd2503752d6bd762d2cc24759a0ae14cfa6"),
        ("G3:U08:L14","usccss.b01.g3.quarter_inch_estimate_classify",
         "Estimation Exploration","17d95d0eb810c72cc430e91ded70a2e41f293fe0ccbc904325c3808bea2eda5a")
    ];

    [Fact]
    public void SourceExactRemapsAreBoundToTheCorrectUnchangedLessons()
    {
        var source=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .Where(d=>d.PackCode=="US-CCSS-MATH" && d.VersionCode=="CCSSM-2010")
            .SelectMany(d=>d.Lessons).ToDictionary(l=>l.LessonCode);
        foreach(var x in Targets)
        {
            var id=Prefix+x.Suffix;
            var lesson=source[id];
            Assert.Equal(x.RawSha,lesson.CanonicalBodySha256);
            Assert.Equal(x.Title,Assert.Single(lesson.Translations).Title);
            Assert.StartsWith("https://im.kendallhunt.com/k5/teachers/",lesson.SourceUrl);
            Assert.True(LessonPracticeCapabilityResolver.TryResolve(id,out var practice));
            Assert.NotNull(practice);
            Assert.Equal([x.Family],practice.AllowedQuestionFamilies);
            Assert.StartsWith("IM2021_",practice.SourceType);
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(x.Family));
        }
    }

    [Fact]
    public void SourceFamiliesGenerateAndAgreeWithIndependentMathOracles()
    {
        var engine=new ExactSkillContractQuestionEngine();
        var seed=990100;
        foreach(var x in Targets)
          foreach(var difficulty in new[]{
              ExactSkillQuestionDifficulty.Standard,
              ExactSkillQuestionDifficulty.Stretch,
              ExactSkillQuestionDifficulty.Challenge})
          {
            var questions=engine.Generate("b01-source-practice-verification",
                Prefix+x.Suffix,[x.Family],difficulty,8,seed++,[]);
            Assert.Equal(8,questions.Count);
            Assert.Equal(8,questions.Select(q=>q.Prompt).Distinct(StringComparer.Ordinal).Count());
            foreach(var q in questions)
            {
                Assert.Equal(x.Family,q.Family);
                string Independent(string k)=>q.Parameters[k].ToString(CultureInfo.InvariantCulture);
                var expected=x.Family switch
                {
                    "usccss.b01.g1.place_value_number_riddle" =>
                        Independent("number"),
                    "usccss.b01.g2.target_measurement_difference_cm" =>
                        Math.Abs(q.Parameters["target"]-q.Parameters["measured"])
                            .ToString(CultureInfo.InvariantCulture),
                    "usccss.b01.g3.quarter_inch_estimate_classify" =>
                        q.Parameters["estimateQuarters"]<q.Parameters["actualQuarters"]
                        ? "too low"
                        : q.Parameters["estimateQuarters"]>q.Parameters["actualQuarters"]
                          ? "too high" : "about right",
                    _ => throw new InvalidOperationException()
                };
                Assert.Equal(expected,q.CorrectAnswer);
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    x.Family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    x.Family,q.Parameters,"definitely-wrong"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    x.Family,new Dictionary<string,int>(),q.CorrectAnswer));
            }
          }
    }

    [Fact]
    public void MathematicallyInconsistentParametersNeverPassVerification()
    {
        var tests=new[]{
            ("usccss.b01.g1.place_value_number_riddle",
              new Dictionary<string,int>{{"tens",3},{"ones",4},{"number",35}},"35"),
            ("usccss.b01.g2.target_measurement_difference_cm",
              new Dictionary<string,int>{{"target",5},{"measured",27}},"22"),
            ("usccss.b01.g3.quarter_inch_estimate_classify",
              new Dictionary<string,int>{{"actualQuarters",7},{"estimateQuarters",7},{"mode",2}},"too high")
        };
        foreach(var (family,values,answer) in tests)
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,values,answer));
    }
}
