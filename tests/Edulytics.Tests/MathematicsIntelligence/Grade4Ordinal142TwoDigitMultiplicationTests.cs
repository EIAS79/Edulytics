using System.Collections.Generic;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// Stage C, intentionally UNRUN.
public sealed class Grade4Ordinal142TwoDigitMultiplicationTests
{
[Fact] public void OriginalSourcePartialProductsAndRiddle()
{
Assert.Equal(714,34*21);
Assert.Equal(714,34*20+34);
Assert.Equal(343,7*7*7);
}
[Fact] public void FirstPartyTwoDigitFixtureVerifiesAndRejectsWrongAnswer()
{
var p=new Dictionary<string,int>{{"caseType",0},{"variant",0},{"multiplicand",34},{"multiplier",21},{"product",714}};
Assert.True(ExactSkillContractQuestionEngine.Verify("usccss.g4.u09.l05.two_digit_partial_products",p,"714"));
Assert.False(ExactSkillContractQuestionEngine.Verify("usccss.g4.u09.l05.two_digit_partial_products",p,"713"));
}
}
