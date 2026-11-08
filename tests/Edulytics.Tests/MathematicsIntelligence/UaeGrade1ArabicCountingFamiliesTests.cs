using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// WRITTEN_NOT_RUN in Stage A–B–C.
public sealed class UaeGrade1ArabicCountingFamiliesTests
{
 [Theory]
 [InlineData("uae.g1.u01.l01.read_write_1_5", 1, 5)]
 [InlineData("uae.g1.u01.l02.zero_cardinality", 0, 0)]
 [InlineData("uae.g1.u01.l03.read_write_6_8", 6, 8)]
 [InlineData("uae.g1.u01.l04.read_write_9_10", 9, 10)]
 [InlineData("uae.g1.u01.l05.read_write_11_15", 11, 15)]
 [InlineData("uae.g1.u01.l06.read_write_16_20", 16, 20)]
 public void Counts_AreExactAndRangeConstrained(string family, int min, int max)
 {
   foreach (var count in new[] { min, max })
   {
     var p = new Dictionary<string,int> { ["count"]=count, ["scene"]=1 };
     Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,count.ToString()));
     Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,(count+1).ToString()));
   }
   var invalid = new Dictionary<string,int> { ["count"]=max+1, ["scene"]=1 };
   Assert.False(ExactSkillContractQuestionEngine.Verify(family,invalid,(max+1).ToString()));
 }
 [Fact]
 public void Zero_AcceptsArabicIndicNumeral()
 {
    var p=new Dictionary<string,int> { ["count"]=0, ["scene"]=0 };
    Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g1.u01.l02.zero_cardinality",p,"٠"));
    Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g1.u01.l02.zero_cardinality",p,"١"));
 }

 [Fact]
 public void FirstArabicLesson_UsesOnlyItsSourceScopedExactCountingFamily()
 {
   const string lessonCode = "PED:UAE-MOE-MATH:L1:COMMON:01:01";
   const string family = "uae.g1.u01.l01.read_write_1_5";
   Assert.True(LessonPracticeContractRegistry.TryResolve(lessonCode, out var contract));
   Assert.NotNull(contract);
   Assert.Equal("READY_VERIFIED", contract.Readiness);
   Assert.Equal(new[] { family }, contract.AllowedQuestionFamilies);
   Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
   var generated = new ExactSkillContractQuestionEngine().Generate(
       "stage18", lessonCode, new[] { family },
       ExactSkillQuestionDifficulty.Standard, 3, 42, Array.Empty<string>());
   Assert.All(generated, question =>
   {
       Assert.Equal(family, question.Family);
       Assert.InRange(question.Parameters["count"], 1, 5);
       Assert.True(ExactSkillContractQuestionEngine.Verify(
           family, question.Parameters, question.CorrectAnswer));
   });
 }

 [Fact]
 public void SecondArabicLesson_ZeroOnly_UsesExactSourceScopedFamily()
 {
   const string lessonCode = "PED:UAE-MOE-MATH:L1:COMMON:01:02";
   const string family = "uae.g1.u01.l02.zero_cardinality";
   Assert.True(LessonPracticeContractRegistry.TryResolve(lessonCode, out var contract));
   Assert.NotNull(contract);
   Assert.Equal("READY_VERIFIED", contract.Readiness);
   Assert.Equal(new[] { family }, contract.AllowedQuestionFamilies);
   Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
   var generated = new ExactSkillContractQuestionEngine().Generate(
       "stage18", lessonCode, new[] { family },
       ExactSkillQuestionDifficulty.Standard, 3, 73, Array.Empty<string>());
   Assert.All(generated, question =>
   {
       Assert.Equal(0, question.Parameters["count"]);
       Assert.Equal(family, question.Family);
       Assert.True(ExactSkillContractQuestionEngine.Verify(
           family, question.Parameters, "٠"));
       Assert.False(ExactSkillContractQuestionEngine.Verify(
           family, question.Parameters, "١"));
   });
 }

 [Fact]
 public void ArabicUnitALesson3_UsesOnlyItsExactCountRange()
 {
    const string lessonCode = "PED:UAE-MOE-MATH:L1:COMMON:01:03";
    const string family = "uae.g1.u01.l03.read_write_6_8";
    Assert.True(LessonPracticeContractRegistry.TryResolve(lessonCode, out var contract));
    Assert.NotNull(contract);
    Assert.Equal(new[] { family }, contract.AllowedQuestionFamilies);
    Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
    var generated = new ExactSkillContractQuestionEngine().Generate(
        "stage18", lessonCode, new[] { family },
        ExactSkillQuestionDifficulty.Standard, 3, 159, Array.Empty<string>());
    Assert.All(generated, q =>
    {
        Assert.InRange(q.Parameters["count"], 6, 8);
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
    });
 }
}
