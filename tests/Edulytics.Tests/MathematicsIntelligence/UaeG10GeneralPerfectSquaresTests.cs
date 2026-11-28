using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using System.Globalization;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Owner-book UAE Grade10 General U01L09 perfect-square trinomials.
/// Authored in Stage C; these tests MUST remain WRITTEN_NOT_RUN until Stage D.
/// </summary>
public sealed class UaeG10GeneralPerfectSquaresTests
{
    private const string LessonCode="PED:UAE-MOE-MATH:L10:GENERAL:01:09:PERFECT-SQUARES";
    private const string Factor="uae.g10.general.u01.l09.full_perfect_square_factor";
    private const string Recognize="uae.g10.general.u01.l09.classify_perfect_square";
    private const string Roots="uae.g10.general.u01.l09.shifted_square_two_roots";

    [Theory]
    [InlineData(Factor)]
    [InlineData(Recognize)]
    [InlineData(Roots)]
    public void SourceExactFamiliesUseAuthorDefinedStructuredResponse(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction));
        Assert.Equal(LessonPracticeInteractionKind.StructuredEntry,interaction!.InteractionKind);
    }

    [Theory]
    [InlineData(Factor)]
    [InlineData(Recognize)]
    [InlineData(Roots)]
    public void IndependentBoundedVerificationRejectsIncorrectAnswersAndForgedParams(string family)
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(int seed in new[]{20261124,20261125,20261126})
        foreach(var difficulty in new[]{ExactSkillQuestionDifficulty.Standard,
          ExactSkillQuestionDifficulty.Challenge})
        {
            var questions=engine.Generate(LessonCode,LessonCode,[family],difficulty,4,seed,[]);
            Assert.Equal(4,questions.Count);
            foreach(var q in questions)
            {
                Assert.Equal(AssessmentItemType.ShortAnswer,q.ItemType);
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"خطأ"));
                var arabic=string.Concat(q.CorrectAnswer.Select(c=>c is >= '0' and <= '9'?
                    (char)('٠'+c-'0'):c));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,arabic));
                var parameters=q.Parameters.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
                if(family==Roots)
                {
                    int h=parameters["h"],r=parameters["r"];
                    Assert.Equal(h-r,Math.Min(int.Parse(q.CorrectAnswer.Split(',')[0],
                        CultureInfo.InvariantCulture),int.Parse(q.CorrectAnswer.Split(',')[1],CultureInfo.InvariantCulture)));
                    Assert.Equal(h+r,Math.Max(int.Parse(q.CorrectAnswer.Split(',')[0],
                        CultureInfo.InvariantCulture),int.Parse(q.CorrectAnswer.Split(',')[1],CultureInfo.InvariantCulture)));
                    Assert.True(ExactSkillContractQuestionEngine.Verify(family,parameters,
                        (h+r).ToString(CultureInfo.InvariantCulture)+","+
                        (h-r).ToString(CultureInfo.InvariantCulture)));
                    parameters["r"]=9;
                }
                else
                {
                    int a=parameters["a"],b=parameters["b"],sign=parameters["sign"],delta=parameters["delta"];
                    int mid=sign*2*a*b+delta;
                    Assert.Equal(mid,parameters["sign"]*2*parameters["a"]*parameters["b"]+parameters["delta"]);
                    if(family==Recognize)
                        Assert.Equal(delta==0?"نعم":"لا",q.CorrectAnswer);
                    else
                    {
                        Assert.Equal(0,delta);
                        Assert.EndsWith("²",q.CorrectAnswer);
                    }
                    parameters["a"]=7;
                }
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,parameters,q.CorrectAnswer));
            }
        }
    }

    [Fact]
    public void OriginalIndependentSourceGoldensRespectRootSignsAndMiddleTerms()
    {
        foreach(int x in new[]{-4,-1,0,1,2,5})
        {
            Assert.Equal(16*x*x+24*x+9,(4*x+3)*(4*x+3));
            Assert.Equal(25*x*x-30*x+9,(5*x-3)*(5*x-3));
            Assert.Equal(3*x*x+30*x+75,3*(x+5)*(x+5));
            Assert.Equal(9*x*x-48*x+64,(3*x-8)*(3*x-8));
        }
        Assert.Equal(144-16*3*3,0);
        Assert.Equal((-3-6)*(-3-6),81);
        Assert.Equal((15-6)*(15-6),81);
    }
}
