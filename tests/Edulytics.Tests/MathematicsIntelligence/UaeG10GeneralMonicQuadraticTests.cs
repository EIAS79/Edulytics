using System.Globalization;
using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Original UAE Grade 10 General U01L06 monic-trinomial questions.
/// Source examined: General 01.pdf PDF56–63, printed 43–50.
/// Test source written under A/B/C; execution reserved for Stage D.
/// </summary>
public sealed class UaeG10GeneralMonicQuadraticTests
{
    private const string LessonCode =
        "PED:UAE-MOE-MATH:L10:GENERAL:01:06:SOLVING-X-BX-C-0";
    private const string Factor="uae.g10.general.u01.l06.monic_full_factor";
    private const string Roots="uae.g10.general.u01.l06.monic_integer_roots";
    private const string Prime="uae.g10.general.u01.l06.monic_prime_over_integers";

    [Theory]
    [InlineData(Factor)]
    [InlineData(Roots)]
    [InlineData(Prime)]
    public void FamiliesRequireGrade10ExactEngineAndStructuredArabicAnswer(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction));
        Assert.NotNull(interaction);
        Assert.Equal(LessonPracticeInteractionKind.StructuredEntry,interaction!.InteractionKind);
    }

    [Theory]
    [InlineData(Factor)]
    [InlineData(Roots)]
    [InlineData(Prime)]
    public void GeneratedProblemsAreSourceScopedAndTheirOwnIndependentVerifierRejectsTampering(string family)
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var seed in new[]{20261124,20261125,20261126})
        foreach(var difficulty in new[]{ExactSkillQuestionDifficulty.Standard,
                                         ExactSkillQuestionDifficulty.Challenge})
        {
            var questions=engine.Generate(LessonCode,LessonCode,[family],difficulty,4,seed,[]);
            Assert.Equal(4,questions.Count);
            foreach(var question in questions)
            {
                var p=question.Parameters;
                Assert.Equal(AssessmentItemType.ShortAnswer,question.ItemType);
                Assert.Contains(question.Prompt,ch=>ch is >= '\u0600' and <= '\u06FF');
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,question.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"خطأ"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,question.CorrectAnswer+"+1"));

                var localized=string.Concat(question.CorrectAnswer.Select(ch=>
                    ch is >= '0' and <= '9'?(char)('٠'+ch-'0'):ch));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,localized));

                if(family==Prime)
                {
                    Assert.Equal(2,p.Count);
                    int b=p["b"],c=p["c"];
                    Assert.NotEqual(0,b);Assert.NotEqual(0,c);
                    bool independentlyFactorable=false;
                    for(int m=-20;m<=20;m++)
                    for(int n=-20;n<=20;n++)
                        if(m+n==b&&m*n==c)independentlyFactorable=true;
                    Assert.Equal(independentlyFactorable?"نعم":"لا",question.CorrectAnswer);
                    var forged=p.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
                    forged["c"]=0;
                    Assert.False(ExactSkillContractQuestionEngine.Verify(family,forged,question.CorrectAnswer));
                }
                else
                {
                    Assert.Equal(2,p.Count);
                    int r1=p["r1"],r2=p["r2"];
                    Assert.NotEqual(0,r1);Assert.NotEqual(0,r2);
                    Assert.NotEqual(0,r1+r2);
                    int b=-(r1+r2),c=r1*r2;
                    foreach(var x in new[]{-5,-1,0,1,3,5})
                        Assert.Equal((x-r1)*(x-r2),x*x+b*x+c);
                    Assert.Equal(0,r1*r1+b*r1+c);
                    Assert.Equal(0,r2*r2+b*r2+c);

                    if(family==Roots)
                    {
                        Assert.NotEqual(r1,r2);
                        var reversed=r2.ToString(CultureInfo.InvariantCulture)+","+
                            r1.ToString(CultureInfo.InvariantCulture);
                        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,reversed));
                        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,
                            r1.ToString(CultureInfo.InvariantCulture)));
                    }
                    else
                    {
                        Assert.Contains('(',question.CorrectAnswer);
                        if(r1!=r2)
                        {
                            string signed1=r1<0?$"(س+{-r1})":$"(س−{r1})";
                            string signed2=r2<0?$"(س+{-r2})":$"(س−{r2})";
                            Assert.True(ExactSkillContractQuestionEngine.Verify(
                                family,p,signed2+signed1));
                        }
                        else
                        {
                            // The same factor twice may be entered with a square.
                            string square=r1<0?$"(س+{-r1})²":$"(س−{r1})²";
                            Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,square));
                        }
                    }
                    var forged=p.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
                    forged["r1"]=0;
                    Assert.False(ExactSkillContractQuestionEngine.Verify(family,forged,question.CorrectAnswer));
                }
            }
        }
    }

    [Fact]
    public void OriginalGoldenPairsAndContextualValidityMatchMonicSource()
    {
        foreach(int x in new[]{-3,-1,0,1,3,7})
        {
            Assert.Equal((x+4)*(x+5),x*x+9*x+20);
            Assert.Equal((x-2)*(x-6),x*x-8*x+12);
            Assert.Equal((x+5)*(x-3),x*x+2*x-15);
            Assert.Equal((x-7)*(x+3),x*x-4*x-21);
            Assert.Equal((x+10)*(x-5),x*x+5*x-50);
        }
        Assert.Equal(54,(5+4)*(5+1));
        Assert.Equal(0,(-8)*(-8)+5*(-8)-24);
        Assert.Equal(0,3*3+5*3-24);
    }
}
