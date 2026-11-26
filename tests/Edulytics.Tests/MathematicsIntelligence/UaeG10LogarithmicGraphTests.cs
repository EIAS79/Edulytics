using System.Globalization;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UaeG10LogarithmicGraphTests
{
    private const string Code="PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U05:L03:C044";
    private const string Prefix="uae.g10.general.u05.l03.";
    [Theory]
    [InlineData("evaluate_rational",LessonPracticeInteractionKind.FractionEntry)]
    [InlineData("convert_exponential",LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData("convert_logarithmic",LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData("domain_boundary",LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData("domain_membership",LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData("vertical_asymptote",LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData("is_increasing",LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData("graph_anchor",LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData("graph_value",LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData("inverse_shift",LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData("scale_ratio",LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData("doubling_time",LessonPracticeInteractionKind.IntegerKeypad)]
    public void FamilyDispatchRendererIndependentOracle(string suffix, LessonPracticeInteractionKind kind)
    {
        string family=Prefix+suffix;
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction));
        Assert.Equal(kind,interaction!.InteractionKind);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(int seed in new[]{44,20261126,20261127,723})
        {
            var tasks=engine.Generate(Code,Code,[family],ExactSkillQuestionDifficulty.Standard,4,seed,[]);
            Assert.Equal(4,tasks.Count);
            foreach(var task in tasks)
            {
                var p=task.Parameters;
                double b=p["b"],a=p["a"],n=p["n"],h=p["h"],k=p["k"],period=p["period"];
                double basis=Math.Pow(b,p["dir"]==1?p["m"]:-p["m"]);
                double expected=suffix switch{
                    "evaluate_rational"=>Math.Log(Math.Pow(b,n),basis),
                    "convert_exponential" or "convert_logarithmic"=>p["slot"]+1,
                    "domain_boundary" or "vertical_asymptote" or "inverse_shift"=>h,
                    "domain_membership"=>(h+n)>h?1:0,
                    "is_increasing"=>a*Math.Log(basis)>0?1:0,
                    "graph_anchor"=>h+1,
                    "graph_value"=>a*Math.Log(Math.Pow(b,Math.Abs(n)),Math.Pow(b,p["dir"]==1?1:-1))+k,
                    "scale_ratio"=>Math.Pow(p["dir"]==1?b:2,Math.Abs(n)),
                    "doubling_time"=>p["dir"]==1?period*(Math.Abs(n)+1):period,
                    _=>throw new InvalidOperationException()
                };
                double actual=Parse(task.CorrectAnswer);
                Assert.Equal(expected,actual,9);
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,task.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,(expected+7).ToString(CultureInfo.InvariantCulture)));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,""));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"wrong"));
                string arabic=string.Concat(task.CorrectAnswer.Select(ch=>ch is >= '0' and <= '9'?(char)('٠'+ch-'0'):ch=='-'?'−':ch));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,arabic));
                var invalid=p.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);
                invalid["b"]=1;
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,invalid,task.CorrectAnswer));
            }
        }
    }
    [Fact]
    public void GoldenRationalDomainAndTransformBoundaries()
    {
        var p=new Dictionary<string,int>{{"b",2},{"m",3},{"n",2},{"h",4},{"k",1},{"a",-2},{"dir",1},{"slot",2},{"period",3}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"evaluate_rational",p,"2/3"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"evaluate_rational",p,"٤/٦"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Prefix+"evaluate_rational",p,"3/2"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Prefix+"evaluate_rational",p,"2/0"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"convert_exponential",p,"3"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Prefix+"convert_exponential",p,"2"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"graph_value",p,"-3"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"vertical_asymptote",p,"4"));
        p["n"]=0;
        Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"domain_membership",p,"0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Prefix+"domain_membership",p,"1"));
        p["dir"]=0;p["n"]=2;
        Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"evaluate_rational",p,"-2/3"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"graph_value",p,"5"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"is_increasing",p,"1"));
    }
    private static double Parse(string value)
    {
        var s=value.Split('/');
        return double.Parse(s[0],CultureInfo.InvariantCulture)/(s.Length==2?double.Parse(s[1],CultureInfo.InvariantCulture):1);
    }
}
