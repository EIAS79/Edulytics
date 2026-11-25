using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using System.Globalization;

namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Grade10 U03L05; tests written Stage A–B–C, NOT RUN until Stage D.</summary>
public sealed class UaeG10GeneralMatrixOperationsTests
{
    private const string Code="PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U03:L05:C029";
    [Theory]
    [InlineData("uae.g10.general.u03.l05.matrix_sum_entry")]
    [InlineData("uae.g10.general.u03.l05.matrix_difference_entry")]
    [InlineData("uae.g10.general.u03.l05.matrix_scalar_entry")]
    public void GeneratedMatrixOperationMatchesIndependentArithmetic(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction));
        Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,interaction!.InteractionKind);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var seed in new[]{20261125,20261126,20261127})
        {
            var batch=engine.Generate(Code,Code,[family],ExactSkillQuestionDifficulty.Standard,5,seed,[]);
            Assert.Equal(5,batch.Count);
            foreach(var q in batch)
            {
                var v=q.Parameters;
                Assert.Equal(11,v.Count);
                int row=v["row"],col=v["col"];
                Assert.InRange(row,1,2);Assert.InRange(col,1,2);
                string suffix=row.ToString(CultureInfo.InvariantCulture)+col.ToString(CultureInfo.InvariantCulture);
                int a=v["a"+suffix],b=v["b"+suffix];
                int expected=family.EndsWith("matrix_sum_entry",StringComparison.Ordinal)?a+b:
                    family.EndsWith("matrix_difference_entry",StringComparison.Ordinal)?a-b:v["k"]*a;
                Assert.Equal(AssessmentItemType.Numeric,q.ItemType);
                Assert.Equal(expected,int.Parse(q.CorrectAnswer,CultureInfo.InvariantCulture));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,v,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,v,(expected+1).ToString(CultureInfo.InvariantCulture)));
                var arabic=string.Concat(q.CorrectAnswer.Select(ch=>ch is >= '0' and <= '9'?(char)('٠'+ch-'0'):ch));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,v,arabic));
                var forged=v.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
                forged["row"]=3;
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,forged,q.CorrectAnswer));
            }
        }
    }
}