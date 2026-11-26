using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Edulytics.Core.Enums;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG10CircleArcTests
{
 const string P="uae.g10.general.u13.l02."; const string Code="PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U13:L02:C099";
 [Theory]
 [InlineData("central_remaining",LessonPracticeInteractionKind.IntegerKeypad)]
 [InlineData("major_arc",LessonPracticeInteractionKind.IntegerKeypad)]
 [InlineData("pie_angle",LessonPracticeInteractionKind.IntegerKeypad)]
 [InlineData("adjacent_arc_sum",LessonPracticeInteractionKind.IntegerKeypad)]
 [InlineData("arc_length_pi_coefficient",LessonPracticeInteractionKind.FractionEntry)]
 [InlineData("arc_length_rounded",LessonPracticeInteractionKind.NumericEntry)]
 [InlineData("angle_from_arc_fraction",LessonPracticeInteractionKind.IntegerKeypad)]
 public void FamilyDispatch(string suffix,LessonPracticeInteractionKind kind){string family=P+suffix;Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var contract));Assert.Equal(kind,contract!.InteractionKind);var engine=new ExactSkillContractQuestionEngine();foreach(var task in engine.Generate(Code,Code,[family],ExactSkillQuestionDifficulty.Standard,4,20261126,[])){Assert.True(ExactSkillContractQuestionEngine.Verify(family,task.Parameters,task.CorrectAnswer));Assert.False(ExactSkillContractQuestionEngine.Verify(family,task.Parameters,"wrong"));}}
 [Fact] public void Goldens(){var p=new Dictionary<string,int>{{"a1",100},{"a2",145},{"r",7},{"angle",135},{"percent",35}};Assert.True(ExactSkillContractQuestionEngine.Verify(P+"central_remaining",p,"١١٥"));Assert.True(ExactSkillContractQuestionEngine.Verify(P+"pie_angle",p,"126"));Assert.True(ExactSkillContractQuestionEngine.Verify(P+"arc_length_pi_coefficient",p,"21/4"));Assert.True(ExactSkillContractQuestionEngine.Verify(P+"arc_length_rounded",p,"16.49"));Assert.False(ExactSkillContractQuestionEngine.Verify(P+"arc_length_rounded",p,"16.48"));}
}
