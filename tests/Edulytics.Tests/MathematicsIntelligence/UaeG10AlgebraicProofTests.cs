using System.Globalization;
using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UaeG10AlgebraicProofTests
{
    private const string Code="PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U18:L02:C147";
    private const string Prefix="uae.g10.general.u18.l02.";
    [Theory]
    [InlineData("equality_property",LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData("distributed_equation",LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData("rational_linear_equation",LessonPracticeInteractionKind.FractionEntry)]
    [InlineData("congruent_segment_equation",LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData("congruent_angle_equation",LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData("temperature_formula",LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData("motion_formula",LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData("acceleration_formula",LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData("heart_rate_formula",LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData("ideal_gas_formula",LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData("voltage_scaling",LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData("cube_volume_scaling",LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData("positive_pythagorean_root",LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData("equivalence_relation",LessonPracticeInteractionKind.IntegerKeypad)]
    public void FamilyDispatchRendererIndependentOracle(string suffix, LessonPracticeInteractionKind kind)
    {
        string family=Prefix+suffix;
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction));
        Assert.Equal(kind,interaction!.InteractionKind);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(int seed in new[]{147,20261126,20261127,723,876})
        {
            var tasks=engine.Generate(Code,Code,[family],ExactSkillQuestionDifficulty.Standard,4,seed,[]);
            Assert.Equal(4,tasks.Count);
            foreach(var task in tasks)
            {
                var p=task.Parameters;
                double a=p["a"],b=p["b"],x=p["x"],k=p["k"],m=p["m"],t=p["t"],u=p["u"],v=p["v"];
                double s=p["m"]%2==0?x:-x;
                double expected=suffix switch{
                    "equality_property"=>m,"distributed_equation"=>s,
                    "rational_linear_equation"=>p["k"]%2==0?a*s:s/a,
                    "congruent_segment_equation" or "congruent_angle_equation"=>x,
                    "temperature_formula"=>(9.0/5)*(5*x)+32,
                    "motion_formula"=>2*((u+v)*t/2)/t-v,
                    "acceleration_formula"=>2*(v*t+x*t*t/2-v*t)/(t*t),
                    "heart_rate_formula"=>220-(165-3*x)/.75,
                    "ideal_gas_formula"=>(a*.0821*(200+10*x))/(a*.0821),
                    "voltage_scaling"=>p["m"]%2==0?(2*x*v)/(2*v):(4*x*v)/v,
                    "cube_volume_scaling"=>Math.Pow(a*k,3)/Math.Pow(a,3),
                    "positive_pythagorean_root"=>Math.Sqrt(Math.Pow(5*k,2)-Math.Pow(3*k,2)),
                    "equivalence_relation"=>Equivalence(p["m"],p["a"])?1:0,
                    _=>throw new InvalidOperationException()
                };
                Assert.Equal(expected,Number(task.CorrectAnswer),9);
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,task.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,""));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"wrong"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,((int)Math.Ceiling(expected)+1).ToString(CultureInfo.InvariantCulture)));
                string arabic=string.Concat(task.CorrectAnswer.Select(ch=>ch is >= '0' and <= '9'?(char)('٠'+ch-'0'):ch=='-'?'−':ch));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,arabic));
                var invalid=p.ToDictionary(y=>y.Key,y=>y.Value,StringComparer.Ordinal);invalid["a"]=0;
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,invalid,task.CorrectAnswer));
            }
        }
    }
    [Fact]
    public void GoldenSignedFractionAndEquivalence()
    {
        var p=new Dictionary<string,int>{{"a",4},{"b",2},{"x",5},{"k",3},{"m",1},{"t",6},{"u",12},{"v",8}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"rational_linear_equation",p,"-5/4"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"rational_linear_equation",p,"−١٠/٨"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Prefix+"rational_linear_equation",p,"5/4"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Prefix+"rational_linear_equation",p,"-5/0"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"distributed_equation",p,"-5"));
        p["k"]=2;Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"rational_linear_equation",p,"-20"));
        p["m"]=5;Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"equivalence_relation",p,"0"));
        p["m"]=9;Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"equivalence_relation",p,"1"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"positive_pythagorean_root",p,"8"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Prefix+"positive_pythagorean_root",p,"-8"));
    }
    private static double Number(string s){var z=s.Split('/');return double.Parse(z[0],CultureInfo.InvariantCulture)/(z.Length==2?double.Parse(z[1],CultureInfo.InvariantCulture):1);}
    private static bool Equivalence(int m,int a)
    {
        bool R(int x,int y)=>m switch{1=>x==y,2=>x<=y,3=>x>y,4=>x!=y,5=>Math.Abs(x-y)<=a,6=>x+a==y+a,7=>a*x==a*y,8=>x-y==a,9=>x*x==y*y,_=>false};
        // Explicit witnesses for failure; equality of images under the named functions proves the true cases.
        int[] z=[-2*a,-a,0,a,2*a,1,-1];
        return z.All(x=>R(x,x))&&z.All(x=>z.All(y=>R(x,y)==R(y,x)))&&z.All(x=>z.All(y=>z.All(w=>!R(x,y)||!R(y,w)||R(x,w))));
    }
}
