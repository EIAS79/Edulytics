using System.Globalization;
using System.Numerics;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG10ProbabilityOddsTests
{
    private const string Code="PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U16:L05:C137";
    private const string Prefix="uae.g10.general.u16.l05.";
    [Theory]
    [InlineData("single_event_boundary",LessonPracticeInteractionKind.FractionEntry)]
    [InlineData("odds_from_counts",LessonPracticeInteractionKind.StructuredEntry)]
    [InlineData("probability_from_odds",LessonPracticeInteractionKind.FractionEntry)]
    [InlineData("odds_from_decimal",LessonPracticeInteractionKind.StructuredEntry)]
    [InlineData("hypergeometric_probability",LessonPracticeInteractionKind.FractionEntry)]
    [InlineData("hypergeometric_odds",LessonPracticeInteractionKind.StructuredEntry)]
    [InlineData("at_least_one_probability",LessonPracticeInteractionKind.FractionEntry)]
    [InlineData("three_category_probability",LessonPracticeInteractionKind.FractionEntry)]
    [InlineData("three_category_odds",LessonPracticeInteractionKind.StructuredEntry)]
    [InlineData("different_pair_odds",LessonPracticeInteractionKind.StructuredEntry)]
    [InlineData("two_suit_pattern_odds",LessonPracticeInteractionKind.StructuredEntry)]
    [InlineData("repeated_result_probability",LessonPracticeInteractionKind.FractionEntry)]
    [InlineData("ordered_pattern_probability",LessonPracticeInteractionKind.FractionEntry)]
    [InlineData("ordered_pattern_odds",LessonPracticeInteractionKind.StructuredEntry)]
    [InlineData("conditional_independent_probability",LessonPracticeInteractionKind.FractionEntry)]
    [InlineData("uniform_cut_probability",LessonPracticeInteractionKind.FractionEntry)]
    [InlineData("data_union_probability",LessonPracticeInteractionKind.FractionEntry)]
    [InlineData("data_category_odds",LessonPracticeInteractionKind.StructuredEntry)]
    public void FamilyDispatchRendererIndependentCounts(string suffix, LessonPracticeInteractionKind kind)
    {
        string family=Prefix+suffix;Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction));Assert.Equal(kind,interaction!.InteractionKind);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(int seed in new[]{137,20261126,20261127,723})
        {
            var tasks=engine.Generate(Code,Code,[family],ExactSkillQuestionDifficulty.Standard,4,seed,[]);Assert.Equal(4,tasks.Count);
            foreach(var task in tasks)
            {
                var p=task.Parameters;
                int a=p["a"],b=p["b"],c=p["c"],r=p["r"],u=p["u"],v=p["v"],k=p["k"],mode=p["mode"],n=p["n"];
                BigInteger s=0,t=0;
                switch(suffix)
                {
                    case "single_event_boundary":t=a+b+c;s=mode switch{0 or 1=>a+c,2=>0,3=>a+b+c,_=>a};break;
                    case "odds_from_counts":case "probability_from_odds":s=mode==2?0:a;t=s+(mode==3?0:b);break;
                    case "odds_from_decimal":s=p["pct"];t=1000;break;
                    case "hypergeometric_probability":case "hypergeometric_odds":t=C(a+b,r);s=C(a,k)*C(b,r-k);break;
                    case "at_least_one_probability":t=C(a+b,r);s=t-C(b,r);break;
                    case "three_category_probability":case "three_category_odds":t=C(a+b+c,r);s=C(a,u)*C(b,v)*C(c,r-u-v);break;
                    case "different_pair_odds":t=C(a+b+c,2);s=a*b+a*c+b*c;break;
                    case "two_suit_pattern_odds":int g=3+c%3,h=n+3;t=C(g*h,5);s=g*(g-1)*C(h,3)*C(h,2);break;
                    case "repeated_result_probability":s=1;t=BigInteger.Pow(n,2+mode%2);break;
                    case "ordered_pattern_probability":case "ordered_pattern_odds":s=1;t=mode%2==0?n*(n-1)*(n-2):n*n*n;break;
                    case "conditional_independent_probability":s=p["dir"]==0?a*a:a;t=p["dir"]==0?(a+b)*(a+b):a+b;break;
                    case "uniform_cut_probability":s=2;t=n+1;break;
                    case "data_union_probability":s=a+b;t=a+b+c;break;
                    case "data_category_odds":s=c;t=a+b+c;break;
                    default:throw new InvalidOperationException();
                }
                bool odds=kind==LessonPracticeInteractionKind.StructuredEntry;
                if((odds||suffix=="probability_from_odds")&&p["dir"]==1)s=t-s;
                var parts=task.CorrectAnswer.Split(odds?':':'/');BigInteger num=BigInteger.Parse(parts[0]),den=parts.Length==2?BigInteger.Parse(parts[1]):1;
                Assert.Equal(s*den,num*(odds?t-s:t));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,task.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,odds?$"{num+1}:{den+1+(num==den?1:0)}":"2/1"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,odds?"0:0":"1/0"));Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"wrong"));
                string arabic=string.Concat(task.CorrectAnswer.Select(ch=>ch is >= '0' and <= '9'?(char)('٠'+ch-'0'):ch));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,arabic));
                var invalid=p.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);invalid["r"]=999;Assert.False(ExactSkillContractQuestionEngine.Verify(family,invalid,task.CorrectAnswer));
            }
        }
    }
    private static BigInteger C(int n,int k){if(k<0||k>n)return 0;BigInteger result=1;for(int i=1;i<=k;i++)result=result*(n-i+1)/i;return result;}
    [Fact]
    public void GoldenOddsAndHypergeometricBoundaries()
    {
        var p=new Dictionary<string,int>{{"a",4},{"b",5},{"c",3},{"r",3},{"u",2},{"v",1},{"k",2},{"dir",0},{"mode",0},{"pct",375},{"n",6}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"hypergeometric_probability",p,"5/14"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"hypergeometric_odds",p,"5:9"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Prefix+"hypergeometric_odds",p,"5:14"));
        p["dir"]=1;Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"hypergeometric_odds",p,"9:5"));
        p["dir"]=0;p["mode"]=2;Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"single_event_boundary",p,"0"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"odds_from_counts",p,"0:1"));
        p["mode"]=3;Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"odds_from_counts",p,"1:0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Prefix+"odds_from_counts",p,"0:0"));
        p["pct"]=375;Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"odds_from_decimal",p,"٣:٥"));
    }
}
