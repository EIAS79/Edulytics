using System.Globalization;
using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Owner-supplied G10 General U02L08, Advanced 4-7 and Applied 6-2.
/// Exact isolated square-root equations and extraneous root rejection.
/// Written in A–B–C; NOT RUN before Stage D.
/// </summary>
public sealed class UaeG10GeneralRadicalEquationTests
{
    private const string LessonCode =
        "PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U02:L08:C024";
    private const string Solution =
        "uae.g10.general.u02.l08.radical_equation_integer_solution";
    private const string Candidate =
        "uae.g10.general.u02.l08.squared_candidate_is_valid";

    [Theory]
    [InlineData(Solution)]
    [InlineData(Candidate)]
    public void SourceExactFamiliesHaveIntegerKeypadAndNoImplicitGraphRouting(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
        Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
    }

    [Theory]
    [InlineData(Solution)]
    [InlineData(Candidate)]
    public void GeneratedQuestionsUseOriginalRadicalEquationToRejectExtraRoots(string family)
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(int seed in new[]{20261125,20261126,20261127,20261128})
        foreach(var difficulty in new[]{
            ExactSkillQuestionDifficulty.Standard,ExactSkillQuestionDifficulty.Challenge})
        {
            var tasks=engine.Generate(LessonCode,LessonCode,[family],difficulty,4,seed,[]);
            Assert.Equal(4,tasks.Count);
            foreach(var q in tasks)
            {
                var p=q.Parameters;
                Assert.Equal(4,p.Count);
                int a=p["a"],b=p["b"],c=p["c"],negative=p["negative"];
                Assert.InRange(a,-8,8);Assert.InRange(b,1,7);
                Assert.InRange(c,-5,5);Assert.InRange(negative,0,1);
                Assert.Equal(AssessmentItemType.Numeric,q.ItemType);
                int candidate=b*b-a;
                int leftRadicand=candidate+a;
                Assert.Equal(b*b,leftRadicand);
                Assert.Equal(b,(int)Math.Sqrt(leftRadicand));
                int expected=family==Solution
                    ?candidate
                    :(Math.Sqrt(leftRadicand)==(negative==1?-b:b)?1:0);
                Assert.Equal(expected,int.Parse(q.CorrectAnswer,CultureInfo.InvariantCulture));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,p,(expected+1).ToString(CultureInfo.InvariantCulture)));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"غير صحيح"));
                var arabic=string.Concat(q.CorrectAnswer.Select(ch=>
                    ch is >= '0' and <= '9'?(char)('٠'+ch-'0'):ch));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,arabic));
                var invalid=p.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
                invalid["b"]=0;
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,invalid,q.CorrectAnswer));

                if(family==Solution)
                {
                    Assert.Equal(0,negative); // No mathematically irrelevant random flag.
                    Assert.Equal(b+c,(int)Math.Sqrt(candidate+a)+c);
                }
            }
        }
    }

    [Fact]
    public void SquaringCanCreateExtraneousCandidateThatMustFailOriginalEquation()
    {
        // sqrt(x+5)=-4 -> squaring yields x=11, but sqrt(16) != -4.
        foreach(int x in new[]{-4,0,5,11,16})
            Assert.False(x>=-5&&Math.Sqrt(x+5)==-4);
        // sqrt(x+5)=+4 -> x=11 is the unique real solution.
        Assert.Equal(4,Math.Sqrt(11+5));
        // sqrt(x+4)=x-2 -> x=0 is extraneous, x=5 is valid.
        Assert.NotEqual(Math.Sqrt(0+4),0-2);
        Assert.Equal(Math.Sqrt(5+4),5-2);
        // Radical inequality: sqrt(x+1)<3 requires -1<=x<8.
        foreach(int x in new[]{-1,0,1,7})
            Assert.True(x>=-1&&Math.Sqrt(x+1)<3);
        Assert.False(Math.Sqrt(8+1)<3);
    }
}
