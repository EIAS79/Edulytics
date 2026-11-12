using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit11Lesson06ArithmeticTests
{
 private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:11:06";
 [Fact] public void PracticeContractGeneratesAndVerifiesBothFamilies()
 {
    Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var c));
    Assert.Equal(2,c!.AllowedQuestionFamilies.Count);
    var engine=new ExactSkillContractQuestionEngine();
    foreach(var f in c.AllowedQuestionFamilies)
    {
       Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
       Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
       Assert.NotNull(ui);
       foreach(var q in engine.Generate(Lesson,f,[f],ExactSkillQuestionDifficulty.Standard,3,20261112,[]))
       {
          Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
          Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-1"));
       }
    }
 }
 [Theory]
 [InlineData(130,50,0,"1.80","1")]
 [InlineData(129,155,0,"2.84","2")]
 [InlineData(246,113,0,"3.59","3")]
 public void ChecksEquivalentDecimalArithmetic(int a,int b,int c,string first,string second)
 {
    var p=new Dictionary<string,int>{{"aHundredths",a},{"bHundredths",b},{"cHundredths",c}};
    Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u11.l06.column_sum",p,first));
    Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u11.l06.whole_part",p,second));
 }
 [Fact] public void InvalidAmountRejected()
 {
    var p=new Dictionary<string,int>{{"aHundredths",-1},{"bHundredths",2},{"cHundredths",0}};
    Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u11.l06.column_sum",p,"0"));
 }
 [Fact] public void RichArabicContentIncludesDistinctExamples()
 {
    var l=RichLessonContentV2Registry.Find(Lesson,"ar");
    Assert.NotNull(l);
    Assert.Equal("جمع الأعداد العشرية",l!.Title);
    Assert.True(l.WorkedExamples.Count>=4);
    Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
 }
}
