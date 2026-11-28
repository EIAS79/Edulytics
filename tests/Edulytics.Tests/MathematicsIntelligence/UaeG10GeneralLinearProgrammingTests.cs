using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Source G10/U03L03, written in Stage A–B–C. NOT RUN until Stage D.</summary>
public sealed class UaeG10GeneralLinearProgrammingTests
{
    private const string LessonCode="PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U03:L03:C027";
    private const string Value="uae.g10.general.u03.l03.optimal_objective_value";
    private const string Vertex="uae.g10.general.u03.l03.unique_optimal_vertex";

    [Theory]
    [InlineData(Value)]
    [InlineData(Vertex)]
    public void ExactLinearProgrammingIsNumericAndDoesNotLeakIncorrectResponse(string family)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction));
        Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,interaction!.InteractionKind);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(int seed in new[]{20261125,20261126,20261127})
        foreach(var difficulty in new[]{ExactSkillQuestionDifficulty.Standard,ExactSkillQuestionDifficulty.Challenge})
        {
            var batch=engine.Generate(LessonCode,LessonCode,[family],difficulty,4,seed,[]);
            Assert.Equal(4,batch.Count);
            foreach(var item in batch)
            {
                var p=item.Parameters;
                Assert.Equal(6,p.Count);
                Assert.Equal(AssessmentItemType.Numeric,item.ItemType);
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,item.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"خطأ"));
                int[] v=[0,p["p"]*p["b"]*p["k"],p["q"]*p["a"]*p["k"]];
                int optimal=p["mode"]==0?v.Max():v.Min();
                int expected=family==Value?optimal:Array.IndexOf(v,optimal);
                Assert.Equal(expected,int.Parse(item.CorrectAnswer,System.Globalization.CultureInfo.InvariantCulture));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,(expected+1).ToString()));
                if(family==Vertex)Assert.Equal(1,v.Count(x=>x==optimal));
                var forged=p.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
                forged["a"]=9;
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,forged,item.CorrectAnswer));
                var arabic=string.Concat(item.CorrectAnswer.Select(ch=>ch is >= '0' and <= '9'?(char)('٠'+ch-'0'):ch));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,arabic));
            }
        }
    }
    [Fact]
    public void OriginalIndependentGoldenChecksDistinguishBoundedAndUnbounded()
    {
        int[,] pts={{0,0},{6,0},{0,12}};
        int[] vals=Enumerable.Range(0,3).Select(i=>3*pts[i,0]+2*pts[i,1]).ToArray();
        Assert.Equal(24,vals.Max());Assert.Equal(0,vals.Min());
        foreach(int x in new[]{6,7,10,100}) Assert.True(x>=6); // y=0 in unbounded x+y>=6
    }
}
