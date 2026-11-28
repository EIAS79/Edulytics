using System.Globalization;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UaeG10LogarithmicEquationTests
{
    private const string Code="PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U05:L04:C045";
    private const string Prefix="uae.g10.general.u05.l04.";
    [Theory]
    [InlineData("direct_rational",LessonPracticeInteractionKind.FractionEntry)]
    [InlineData("quadratic_root_set",LessonPracticeInteractionKind.StructuredEntry)]
    [InlineData("one_side_interval",LessonPracticeInteractionKind.StructuredEntry)]
    [InlineData("two_sides_interval",LessonPracticeInteractionKind.StructuredEntry)]
    [InlineData("wind_distance",LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData("rounded_amplitude_ratio",LessonPracticeInteractionKind.NumericEntry)]
    [InlineData("music_key",LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData("sound_level",LessonPracticeInteractionKind.IntegerKeypad)]
    public void FamilyDispatchRendererIndependentOracle(string suffix, LessonPracticeInteractionKind kind)
    {
        string family=Prefix+suffix;
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction));
        Assert.Equal(kind,interaction!.InteractionKind);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(int seed in new[]{45,20261126,20261127,723,876})
        foreach(var task in engine.Generate(Code,Code,[family],ExactSkillQuestionDifficulty.Standard,4,seed,[]))
        {
            var p=task.Parameters;
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,task.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,""));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"wrong"));
            string arabic=string.Concat(task.CorrectAnswer.Select(ch=>ch is >= '0' and <= '9'?(char)('٠'+ch-'0'):ch=='-'?'−':ch==','?'،':ch));
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,arabic));
            var invalid=p.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);invalid["b"]=1;
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,invalid,task.CorrectAnswer));
            int sign=p["dir"]==1?1:-1;double b=p["b"],m=p["m"],n=p["n"];
            if(suffix=="direct_rational")
            {
                double expected=p["op"] switch{0=>Math.Pow(b,n*sign),1=>n*sign/m,2=>Math.Pow(b,(1+p["z"]%3)*sign),_=>Enumerable.Range(1,5).Sum(i=>n*sign/i)};
                Assert.Equal(expected,Number(task.CorrectAnswer),9);
            }
            else if(suffix=="quadratic_root_set")
            {
                var expected=Enumerable.Range(-5,11).Where(x=>{
                    double right=p["a"]*x+p["c"];
                    double left=p["dir"]==1?m*x*x+(p["a"]-m*(p["r"]+p["s"]))*x+p["c"]+m*p["r"]*p["s"]:(p["a"]+1)*x+p["c"]-p["r"];
                    return right>0&&left>0&&left==right;
                }).ToArray();
                var actual=task.CorrectAnswer=="∅"?Array.Empty<int>():task.CorrectAnswer.Split(',').Select(int.Parse).OrderBy(x=>x).ToArray();
                Assert.Equal(expected,actual);
            }
            else if(suffix is "one_side_interval" or "two_sides_interval")
            {
                double basis=Math.Pow(b,sign);
                for(double x=-15.125;x<=15.125;x+=.25)
                {
                    double left=p["a"]*x+p["c"],right=p["d"]*x+p["e"];
                    bool valid=left>0&&(suffix=="one_side_interval"||right>0);
                    double difference=valid?Math.Log(left,basis)-(suffix=="one_side_interval"?n:Math.Log(right,basis)):double.NaN;
                    bool expected=valid&&(p["op"] switch{0=>difference<0,1=>difference<=0,2=>difference>0,_=>difference>=0});
                    if(!valid||Math.Abs(difference)>1e-10)Assert.Equal(expected,Has(task.CorrectAnswer,x));
                }
            }
            else
            {
                double expected=suffix switch{
                    "wind_distance"=>p["dir"]==1?Math.Pow(10,p["z"]):Math.Round(10*b*Math.Log10(10+13*p["z"]+7*Math.Abs(p["c"]))+10*m,MidpointRounding.AwayFromZero),
                    "rounded_amplitude_ratio"=>Math.Round(Math.Pow(10,p["dir"]==1?p["z"]/10.0:m),2),
                    "music_key"=>1+12*Math.Abs(n),
                    "sound_level"=>10*(n+6),_=>throw new InvalidOperationException()
                };
                Assert.Equal(expected,Number(task.CorrectAnswer),9);
            }
        }
    }
    [Fact]
    public void GoldenDomainBoundariesAndExtraneousRoots()
    {
        var p=new Dictionary<string,int>{{"b",2},{"m",1},{"n",2},{"dir",1},{"op",0},{"a",1},{"c",-3},{"d",-1},{"e",2},{"r",4},{"s",-3},{"z",5}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"one_side_interval",p,"(3,7)"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Prefix+"one_side_interval",p,"[3,7)"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Prefix+"one_side_interval",p,"(-inf,7)"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"two_sides_interval",p,"∅"));
        p["op"]=1;Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"one_side_interval",p,"(٣،٧]"));
        p["dir"]=0;Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"one_side_interval",p,"[13/4,inf)"));
        p["dir"]=1;p["a"]=1;p["c"]=0;
        Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"quadratic_root_set",p,"4"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Prefix+"quadratic_root_set",p,"-3,4"));
        p["c"]=8;Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"quadratic_root_set",p,"4,-3"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Prefix+"quadratic_root_set",p,"4"));
        p["a"]=1;p["c"]=-8;Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"quadratic_root_set",p,"∅"));
        p["dir"]=1;p["op"]=2;p["m"]=2;p["z"]=5;Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"direct_rational",p,"8"));
        p["dir"]=0;Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"direct_rational",p,"1/8"));
        p["dir"]=1;p["z"]=5;Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"rounded_amplitude_ratio",p,"3.16"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Prefix+"rounded_amplitude_ratio",p,"3.17"));
    }
    private static double Number(string s){var a=s.Split('/');return double.Parse(a[0],CultureInfo.InvariantCulture)/(a.Length==2?double.Parse(a[1],CultureInfo.InvariantCulture):1);}
    private static bool Has(string s,double x)
    {
        if(s=="∅")return false;var p=s[1..^1].Split(',');
        double lo=p[0]=="-∞"?double.NegativeInfinity:Number(p[0]),hi=p[1]=="∞"?double.PositiveInfinity:Number(p[1]);
        return (x>lo||s[0]=='['&&x==lo)&&(x<hi||s[^1]==']'&&x==hi);
    }
}
