using System.Globalization;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG10LogarithmPropertiesTests
{
    const string Code="PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U05:L05:C046";
    [Theory]
    [InlineData("uae.g10.general.u05.l05.approx_from_given",LessonPracticeInteractionKind.NumericEntry)]
    [InlineData("uae.g10.general.u05.l05.expand_coefficients",LessonPracticeInteractionKind.StructuredEntry)]
    [InlineData("uae.g10.general.u05.l05.condense_powers",LessonPracticeInteractionKind.StructuredEntry)]
    [InlineData("uae.g10.general.u05.l05.property_truth",LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData("uae.g10.general.u05.l05.solve_property_equation",LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData("uae.g10.general.u05.l05.ph_concentration",LessonPracticeInteractionKind.NumericEntry)]
    [InlineData("uae.g10.general.u05.l05.pressure_rounded",LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData("uae.g10.general.u05.l05.benford_percent",LessonPracticeInteractionKind.NumericEntry)]
    [InlineData("uae.g10.general.u05.l05.benford_digit",LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData("uae.g10.general.u05.l05.first_decay_year",LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData("uae.g10.general.u05.l05.relative_sound",LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData("uae.g10.general.u05.l05.repayment_payment",LessonPracticeInteractionKind.NumericEntry)]
    [InlineData("uae.g10.general.u05.l05.repayment_possible",LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData("uae.g10.general.u05.l05.repayment_minimum",LessonPracticeInteractionKind.NumericEntry)]
    [InlineData("uae.g10.general.u05.l05.repayment_months",LessonPracticeInteractionKind.IntegerKeypad)]
    [InlineData("uae.g10.general.u05.l05.property_root_set",LessonPracticeInteractionKind.StructuredEntry)]
    [InlineData("uae.g10.general.u05.l05.fractional_property_value",LessonPracticeInteractionKind.FractionEntry)]
    [InlineData("uae.g10.general.u05.l04.rounded_amplitude_ratio",LessonPracticeInteractionKind.NumericEntry)]
    public void FamilyDispatchRendererAndIndependentOracle(string family,LessonPracticeInteractionKind kind)
    {
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction));Assert.Equal(kind,interaction!.InteractionKind);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(int seed in new[]{46,20261126,20261127,723,876})
        foreach(var task in engine.Generate(Code,Code,[family],ExactSkillQuestionDifficulty.Standard,4,seed,[]))
        {
            var p=task.Parameters;Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,task.CorrectAnswer));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"wrong"));
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,""));
            string arabic=string.Concat(task.CorrectAnswer.Select(ch=>ch is >= '0' and <= '9'?(char)('٠'+ch-'0'):ch=='-'?'−':ch==','?'،':ch=='.'?'٫':ch));
            Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,arabic));
            var invalid=p.ToDictionary(x=>x.Key,x=>x.Value,StringComparer.Ordinal);invalid["b"]=1;
            Assert.False(ExactSkillContractQuestionEngine.Verify(family,invalid,task.CorrectAnswer));
            string suffix=family.Split('.').Last();
            if(suffix=="rounded_amplitude_ratio"){Assert.Equal(Math.Round(Math.Pow(10,p["dir"]==1?p["z"]/10.0:p["m"]),2),Number(task.CorrectAnswer),9);continue;}
            double a=p["p"],q=p["q"],m=p["m"],rate=p["rate"]/10000.0,balance=p["balance"],digit=p["digit"];
            if(suffix is "expand_coefficients" or "condense_powers"){Assert.Equal(new[]{p["u"]+p["w"],p["v"],-p["m"]},task.CorrectAnswer.Trim('(',')').Split(',').Select(int.Parse).ToArray());continue;}
            if(suffix=="property_root_set"){
                var expected=Enumerable.Range(-50,101).Where(x=>{
                    double l,r;
                    switch(p["mode"]){
                        case 0:return (x*x+m)*q==(a*a+m)*q;
                        case 1:l=(m+1)*x-a-q*p["u"];r=x+p["u"];return l>0&&r>0&&l==m*r;
                        case 2:l=x*x+(m-a+q)*x+m*p["u"]-a*q;r=x+p["u"];return l>0&&r>0&&l==m*r;
                        case 3:return x>a&&a*a-x*x>0;
                        default:return x>0&&x+q-a>0&&x*(x+q-a)==a*q;
                    }
                }).ToArray();
                var actual=task.CorrectAnswer=="∅"?Array.Empty<int>():task.CorrectAnswer.Split(',').Select(int.Parse).OrderBy(x=>x).ToArray();
                Assert.Equal(expected,actual);continue;
            }
            double[] given=p["mode"] switch{0=>[.5,.7925,1.1610],1=>[.6826,.8614,1],2=>[1.1606,1,2],3=>[1.1292,1,2],_=>[1.465,1,2]};
            double interest=balance*rate,monthly=interest+50+10*digit;
            double expectedValue=suffix switch{
                "approx_from_given"=>p["u"]*given[0]+p["v"]*given[1]+p["w"]*given[2],
                "property_truth"=>p["mode"] switch{0 or 2=>1,1=>a+q+1==a*q?1:0,3=>a==q?1:0,_=>0},
                "solve_property_equation"=>a,"fractional_property_value"=>a*Math.Pow(q,p["u"]),
                "ph_concentration"=>Math.Round(Math.Pow(10,-p["ph"]/10.0),12),
                "pressure_rounded"=>Math.Round(Math.Pow(10,5-digit/10.0)),
                "benford_percent"=>Math.Round(100*Math.Log10(1+1/digit),1),"benford_digit"=>digit,
                "first_decay_year"=>Math.Floor(Math.Log(1/(m+1))/Math.Log(1-p["decay"]/100.0))+1,
                "relative_sound"=>p["mode"]%3==1?10*(m+digit):Math.Pow(10,m+digit),
                "repayment_payment"=>Math.Round(interest/(1-Math.Pow(1+rate,-12*m)),2),
                "repayment_possible"=>p["u"]>0?1:0,"repayment_minimum"=>interest+.01,
                "repayment_months"=>Math.Ceiling(-Math.Log(1-interest/monthly)/Math.Log(1+rate)),
                _=>throw new InvalidOperationException()
            };
            Assert.Equal(expectedValue,Number(task.CorrectAnswer),suffix=="ph_concentration"?12:8);
        }
    }
    [Fact]
    public void GoldenSourceBoundaryExamples()
    {
        var p=new Dictionary<string,int>{{"b",2},{"u",0},{"v",1},{"w",2},{"mode",0},{"p",3},{"q",4},{"m",2},{"ph",35},{"digit",4},{"rate",100},{"balance",1200},{"decay",5}};
        string f="uae.g10.general.u05.l05.";
        Assert.True(ExactSkillContractQuestionEngine.Verify(f+"property_root_set",p,"-3,3"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(f+"property_root_set",p,"3"));
        p["mode"]=3;Assert.True(ExactSkillContractQuestionEngine.Verify(f+"property_root_set",p,"∅"));
        p["mode"]=0;Assert.True(ExactSkillContractQuestionEngine.Verify(f+"benford_percent",p,"9.7"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(f+"benford_percent",p,"9.8"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(f+"ph_concentration",p,"0.000316227766"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(f+"ph_concentration",p,"0.000316227767"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(f+"repayment_possible",p,"0"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(f+"repayment_minimum",p,"12.01"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(f+"repayment_minimum",p,"12.00"));
        p["m"]=1;Assert.True(ExactSkillContractQuestionEngine.Verify(f+"repayment_payment",p,"106.62"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(f+"repayment_payment",p,"106.61"));
        p["u"]=-1;Assert.True(ExactSkillContractQuestionEngine.Verify(f+"fractional_property_value",p,"3/4"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(f+"fractional_property_value",p,"4/3"));
    }
    static double Number(string s){var t=s.Split('/');return double.Parse(t[0],CultureInfo.InvariantCulture)/(t.Length==2?double.Parse(t[1],CultureInfo.InvariantCulture):1);}
}
