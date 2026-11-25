using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using System.Globalization;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Owner source: original 1-9 advanced calculator technology LAB, not a numbered
/// lesson. Tests authored during A–B–C; execute ONLY under Stage D.
/// </summary>
public sealed class UaeG10GeneralAugmentedMatricesTests
{
    private const string Code="PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U03:L09:C033";
    private const string Row="uae.g10.general.u03.l09.row_operation_entry";
    private const string X="uae.g10.general.u03.l09.rref_solution_x";
    private const string Y="uae.g10.general.u03.l09.rref_solution_y";

    [Theory]
    [InlineData(Row)]
    [InlineData(X)]
    [InlineData(Y)]
    public void IndependentSourceAlignedAugmentedAssessmentIsExactAndNumeric(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var contract));
        Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,contract!.InteractionKind);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var seed in new[]{20261125,20261126,20261127})
        foreach(var difficulty in new[]{ExactSkillQuestionDifficulty.Standard,
                                       ExactSkillQuestionDifficulty.Challenge})
        {
            var batch=engine.Generate(Code,Code,[family],difficulty,4,seed,[]);
            Assert.Equal(4,batch.Count);
            foreach(var q in batch)
            {
                var p=q.Parameters;
                Assert.Equal(8,p.Count);
                int a=p["a"],b=p["b"],c=p["c"],d=p["d"];
                int det=a*d-b*c;
                Assert.NotEqual(0,det);
                int expected;
                if(family==Row)
                {
                    int[] first=[a,b,p["r1"]],second=[c,d,p["r2"]];
                    expected=second[p["col"]-1]+p["k"]*first[p["col"]-1];
                    Assert.Contains("R₂",q.Prompt);
                }
                else
                {
                    int numerator=family==X?p["r1"]*d-b*p["r2"]:a*p["r2"]-c*p["r1"];
                    Assert.Equal(0,numerator%det);
                    expected=numerator/det;
                    Assert.Contains("RREF",q.Prompt);
                }
                Assert.Equal(AssessmentItemType.Numeric,q.ItemType);
                Assert.Equal(expected.ToString(CultureInfo.InvariantCulture),q.CorrectAnswer);
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"خطأ"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(
                    family,p,(expected+1).ToString(CultureInfo.InvariantCulture)));
                var arabic=string.Concat(q.CorrectAnswer.Select(ch=>ch is >= '0' and <= '9'
                    ?(char)('٠'+ch-'0'):ch));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,arabic));
                var tampered=p.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
                tampered["col"]=9;
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,tampered,q.CorrectAnswer));
            }
        }
    }

    [Fact]
    public void OriginalSourceThreeByFourCalculatorLabIsInternallyCorrect()
    {
        const int x=-4,y=3,z=6;
        Assert.Equal(1,2*x+y+z);
        Assert.Equal(12,3*x+2*y+3*z);
        Assert.Equal(-1,4*x+y+2*z);
        // Do not falsely claim a 3x4 exact student grader. This source task
        // belongs in the source example and teacher scoring rubric.
    }
}
