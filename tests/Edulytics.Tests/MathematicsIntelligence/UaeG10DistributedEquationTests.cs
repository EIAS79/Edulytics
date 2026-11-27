using System.Globalization;
using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UaeG10DistributedEquationTests
{
    private const string Code="PED:UAE-MOE-MATH:L10:GENERAL:01:02:MULTIPLYING-A-POLYNOMIAL-BY-A-MONOMIAL";
    private const string Family="uae.g10.general.u01.l02.distributed_linear_equation_integer_root";
    private static Dictionary<string,int> P(int mode,int a,int b,int d,int g,int c,int e) =>
        new() { ["mode"]=mode, ["a"]=a, ["b"]=b, ["d"]=d, ["g"]=g, ["c"]=c, ["e"]=e };

    private static bool IndependentSubstitution(IReadOnlyDictionary<string,int> p,int x)
    {
        long k = p["mode"] == 1 ? x : 1;
        return (long)p["a"]*k*(x+p["b"])+p["c"] == (long)p["d"]*k*(x+p["g"])+p["e"];
    }

    [Fact]
    public void FamilyHasIntegerInputAndSupportsBothDistributionModes()
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(Family,out var interaction));
        Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,interaction!.InteractionKind);
        var engine=new ExactSkillContractQuestionEngine();
        var modes=new HashSet<int>();
        foreach(int seed in Enumerable.Range(20261127,40))
        foreach(var difficulty in new[]{ExactSkillQuestionDifficulty.Standard,ExactSkillQuestionDifficulty.Stretch,ExactSkillQuestionDifficulty.Challenge})
        {
            var tasks=engine.Generate(Code,Code,[Family],difficulty,4,seed,[]);
            Assert.Equal(4,tasks.Count);
            foreach(var task in tasks)
            {
                var p=task.Parameters;
                Assert.Equal(7,p.Count);
                modes.Add(p["mode"]);
                if(p["mode"]==1) Assert.Equal(p["a"],p["d"]);
                else Assert.NotEqual(p["a"],p["d"]);
                var roots=Enumerable.Range(-8,17).Where(x=>IndependentSubstitution(p,x)).ToArray();
                int expected=Assert.Single(roots);
                Assert.Equal(AssessmentItemType.Numeric,task.ItemType);
                Assert.Contains("حل المعادلة",task.Prompt);
                Assert.Equal(expected,int.Parse(task.CorrectAnswer,CultureInfo.InvariantCulture));
                Assert.True(ExactSkillContractQuestionEngine.Verify(Family,p,task.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,(expected+1).ToString(CultureInfo.InvariantCulture)));
                Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,"س="+task.CorrectAnswer));
                Assert.True(ExactSkillContractQuestionEngine.Verify(Family,p,string.Concat(task.CorrectAnswer.Select(ch=>ch is >= '0' and <= '9'?(char)('٠'+ch-'0'):ch))));
                Assert.True(ExactSkillContractQuestionEngine.Verify(Family,p,string.Concat(task.CorrectAnswer.Select(ch=>ch is >= '0' and <= '9'?(char)('۰'+ch-'0'):ch))));
            }
        }
        Assert.Equal(new[]{0,1},modes.OrderBy(x=>x).ToArray());
    }
    [Fact]
    public void GoldenEquationsAndMalformedParametersAreFailClosed()
    {
        // 2x(x+3)+4 = 2x(x-1)+20 => x=2.
        var valid=P(1,2,3,2,-1,4,20);
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family,valid,"٢"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,valid,"−2"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,valid,"2.0"));
        // 3(x+2)+1 = -2(x+1)-6 => x=-3.
        var linear=P(0,3,2,-2,1,1,-6);
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family,linear,"-3"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family,linear,"−٣"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(1,2,3,3,-1,4,20),"2"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(1,2,3,2,3,4,4),"2"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(1,2,3,2,-1,4,21),"2"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(0,2,3,2,-1,4,20),"2"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(0,0,3,-2,-1,4,20),"2"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,P(1,2,3,2,-1,4,1001),"2"));
        var extra=new Dictionary<string,int>(valid){["answer"]=2};
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,extra,"2"));
        valid.Remove("b");
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,valid,"2"));
    }
}
