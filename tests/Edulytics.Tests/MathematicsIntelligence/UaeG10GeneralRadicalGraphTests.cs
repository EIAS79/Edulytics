using System.Globalization;
using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Grade10 General U02L07 radical graph point and lower-domain boundary;
/// authored Stage A–B–C, NOT RUN until Stage D.</summary>
public sealed class UaeG10GeneralRadicalGraphTests
{
    private const string Code="PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U02:L07:C023";
    private const string Value="uae.g10.general.u02.l07.radical_graph_y_value";
    private const string Domain="uae.g10.general.u02.l07.radical_graph_domain_start";
    [Theory]
    [InlineData(Value)]
    [InlineData(Domain)]
    public void SourceExactRadicalGraphQuestionHasIntegerNumericRenderer(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
        Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,ui!.InteractionKind);
    }
    [Theory]
    [InlineData(Value)]
    [InlineData(Domain)]
    public void ExactSquareTablePointsAndDomainsAreIndependentlyVerified(string family)
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(int seed in new[]{20261125,20261126,20261127})
        foreach(var difficulty in new[]{ExactSkillQuestionDifficulty.Standard,ExactSkillQuestionDifficulty.Challenge})
        {
            var tasks=engine.Generate(Code,Code,[family],difficulty,4,seed,[]);
            Assert.Equal(4,tasks.Count);
            foreach(var q in tasks)
            {
                var p=q.Parameters;
                Assert.Equal(4,p.Count);
                int a=p["a"],h=p["h"],k=p["k"],n=p["n"];
                Assert.InRange(Math.Abs(a),1,3);
                Assert.InRange(h,-4,4);Assert.InRange(k,-4,4);Assert.InRange(n,0,5);
                int x=h+n*n,y=a*n+k;
                Assert.Equal(y,a*(int)Math.Sqrt(x-h)+k);
                int expected=family==Domain?h:y;
                Assert.Equal(expected,int.Parse(q.CorrectAnswer,CultureInfo.InvariantCulture));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,(expected+1).ToString(CultureInfo.InvariantCulture)));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"خطأ"));
                var arabic=string.Concat(q.CorrectAnswer.Select(ch=>ch is >= '0' and <= '9'?(char)('٠'+ch-'0'):ch));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,arabic));
                var bad=p.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
                bad["n"]=9;
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,bad,q.CorrectAnswer));
                Assert.Equal(AssessmentItemType.Numeric,q.ItemType);
            }
        }
    }
    [Fact]
    public void OriginalStudentGraphGoldenTableAndRangeAreAccurate()
    {
        Assert.Equal(-3,2*Math.Sqrt(0)-3);
        Assert.Equal(-1,2*Math.Sqrt(1)-3);
        Assert.Equal(1,2*Math.Sqrt(4)-3);
        Assert.Equal(2,Math.Sqrt(1+3)+0);
        Assert.Equal(4,-Math.Sqrt(0)+4);
    }
}
