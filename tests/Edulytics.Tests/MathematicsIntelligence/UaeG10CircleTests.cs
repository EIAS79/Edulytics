using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Edulytics.Core.Enums;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG10CircleTests
{
 const string P="uae.g10.general.u13.l01."; const string Code="PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U13:L01:C098";
 [Theory]
 [InlineData("diameter_from_radius",LessonPracticeInteractionKind.IntegerKeypad)]
 [InlineData("radius_from_diameter",LessonPracticeInteractionKind.FractionEntry)]
 [InlineData("circumference_pi_coefficient",LessonPracticeInteractionKind.IntegerKeypad)]
 [InlineData("circumference_rounded",LessonPracticeInteractionKind.NumericEntry)]
 [InlineData("center_line_overlap",LessonPracticeInteractionKind.IntegerKeypad)]
 [InlineData("intersection_count",LessonPracticeInteractionKind.IntegerKeypad)]
 [InlineData("circumscribed_rectangle_pi_coefficient",LessonPracticeInteractionKind.IntegerKeypad)]
 [InlineData("radius_from_circumference_pi",LessonPracticeInteractionKind.FractionEntry)]
 public void FamilyDispatch(string suffix,LessonPracticeInteractionKind kind)
 {
 string family=P+suffix;Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var contract));Assert.Equal(kind,contract!.InteractionKind);var engine=new ExactSkillContractQuestionEngine();foreach(int seed in new[]{20261125,20261126})foreach(var task in engine.Generate(Code,Code,[family],ExactSkillQuestionDifficulty.Standard,4,seed,[])){Assert.True(ExactSkillContractQuestionEngine.Verify(family,task.Parameters,task.CorrectAnswer));Assert.False(ExactSkillContractQuestionEngine.Verify(family,task.Parameters,"wrong"));}
 }
 [Fact] public void Goldens(){var p=new Dictionary<string,int>{{"a",17},{"b",5},{"distance",15},{"k",3}};Assert.True(ExactSkillContractQuestionEngine.Verify(P+"circumference_rounded",p,"٥٣٫٤١"));Assert.False(ExactSkillContractQuestionEngine.Verify(P+"circumference_rounded",p,"53.40"));Assert.True(ExactSkillContractQuestionEngine.Verify(P+"radius_from_diameter",p,"17/2"));Assert.True(ExactSkillContractQuestionEngine.Verify(P+"center_line_overlap",p,"7"));Assert.True(ExactSkillContractQuestionEngine.Verify(P+"intersection_count",p,"2"));Assert.True(ExactSkillContractQuestionEngine.Verify(P+"circumscribed_rectangle_pi_coefficient",p,"15"));p["distance"]=22;Assert.True(ExactSkillContractQuestionEngine.Verify(P+"intersection_count",p,"1"));p["distance"]=23;Assert.True(ExactSkillContractQuestionEngine.Verify(P+"intersection_count",p,"0"));}
}
