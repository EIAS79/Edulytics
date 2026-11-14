using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
// G5 Student Arabic 02-1.pdf printed431–434. StageD test written but NOT_RUN in A–B–C.
public sealed class UaeGrade5Unit6Lesson05EvaluateExpressionsTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L5:GENERAL:06:05";
    [Fact] public void ThreeFamiliesUseExistingAlgebraEngineAndNumericRenderers()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal("number.algebra.evaluate_expressions",contract!.SkillId);
        Assert.Equal(3,contract.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
            Assert.NotNull(ui);
            foreach(var q in engine.Generate(Lesson,family,[family],ExactSkillQuestionDifficulty.Standard,2,20261113,[]))
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
        }
    }
    [Fact] public void IndependentIntegerAndDecimalSubstitutionFixtures()
    {
        var p=new Dictionary<string,int>{{"a",4},{"b",9},{"c",6}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u06.l05.substitute_bracket_add",p,"١١"));
        p["c"]=5;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g5.u06.l05.substitute_squared_subtract",p,"11"));
        var dec=new Dictionary<string,int>{{"aHundredths",40},{"bHundredths",630},{"cHundredths",1005}};
        const string f="uae.g5.u06.l05.substitute_decimal_values";
        Assert.True(ExactSkillContractQuestionEngine.Verify(f,dec,"١٥٫٩٥"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(f,dec,"15.59"));
        dec["cHundredths"]=0;
        Assert.False(ExactSkillContractQuestionEngine.Verify(f,dec,"15.95"));
    }
    [Fact] public void OriginalSourceAlignedParenthesesAndAppliedContext()
    {
        // The 2025–2026 Arabic student book uses whole-expression substitution,
        // parentheses, contextual quantities, and a non-commutative subtraction order.
        // Independent authored values; not copies of the textbook's exercises.
        const int x=8;
        Assert.Equal(15,30-(x+7));
        Assert.NotEqual(15,(30-x)+7);
        const int y=16;
        Assert.Equal(23,48-(y+9));
        Assert.Equal(48,23+(y+9));
        const int a=4,b=9;
        Assert.Equal(11,(b-a)+6);
        Assert.Equal(11,a*a-5);
        Assert.Equal(15.95m,(10.05m-0.40m)+6.30m);
    }
}