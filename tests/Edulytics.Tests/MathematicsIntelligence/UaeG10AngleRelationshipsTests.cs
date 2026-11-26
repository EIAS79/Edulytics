using System.Globalization;
using Edulytics.Core.Enums;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UaeG10AngleRelationshipsTests
{
    private const string Code="PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U18:L03:C148";
    private const string Prefix="uae.g10.general.u18.l03.";
    [Theory]
    [InlineData("protractor_difference")]
    [InlineData("angle_addition")]
    [InlineData("complementary_measure")]
    [InlineData("supplementary_measure")]
    [InlineData("equal_partition")]
    [InlineData("adjacent_angle_equation")]
    [InlineData("vertical_angle_equation")]
    [InlineData("intersection_measure")]
    [InlineData("same_reference_congruence")]
    [InlineData("bisector_vertical")]
    [InlineData("around_point")]
    [InlineData("congruence_reason")]
    [InlineData("perpendicular_sufficient")]
    [InlineData("two_crossing_transfer")]
    public void FamilyDispatchRendererIndependentOracle(string suffix)
    {
        string family=Prefix+suffix;
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var interaction));
        Assert.Equal(LessonPracticeInteractionKind.IntegerKeypad,interaction!.InteractionKind);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(int seed in new[]{148,20261126,20261127,723,876})
        {
            var tasks=engine.Generate(Code,Code,[family],ExactSkillQuestionDifficulty.Standard,4,seed,[]);
            Assert.Equal(4,tasks.Count);
            foreach(var task in tasks)
            {
                var p=task.Parameters;
                int a=p["a"],b=p["b"],x=p["x"],m=p["m"],start=p["p"],v=p["variant"],t=v%2==0?90:180;
                int left=a-m*x,right=t-a-(m+1)*x;
                int first=a+b-m*x,second=a+b-(m+1)*x;
                int expected=suffix switch{
                    "protractor_difference"=>Math.Abs(start+a+b-start),
                    "angle_addition"=>v switch{0=>a+b,1=>a+b-a,2=>92+a%40+b%40-90-(1+a%40),_=>1+a%40+90+1+b%40},
                    "complementary_measure"=>90-a,
                    "supplementary_measure"=>180-(v%2==0?a:180-a),
                    "equal_partition"=>(180-(v%2==0?180-2*a:90))/2,
                    "adjacent_angle_equation"=>(t-left-right)/(2*m+1),
                    "vertical_angle_equation"=>v%2==0?(first-second):m*(first-second)+first,
                    "intersection_measure"=>v==2?a+b-b:v%2==0?a+b:180-a-b,
                    "same_reference_congruence"=>v<2?t-a:t-(t-a),
                    "bisector_vertical"=>v switch{0 or 2=>(2*a)/2,1=>a+a,_=>90/2},
                    "around_point"=>360-a-(90+b)-(90-a),
                    "congruence_reason"=>p["mode"],
                    "perpendicular_sufficient"=>v<2?1:0,
                    "two_crossing_transfer"=>v%2==0?a+b:180-a-b,
                    _=>throw new InvalidOperationException()
                };
                Assert.Equal(expected,int.Parse(task.CorrectAnswer,CultureInfo.InvariantCulture));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,task.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,""));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"wrong"));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,(expected+1).ToString(CultureInfo.InvariantCulture)));
                string arabic=string.Concat(task.CorrectAnswer.Select(ch=>ch is >= '0' and <= '9'?(char)('٠'+ch-'0'):ch));
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,arabic));
                var invalid=p.ToDictionary(y=>y.Key,y=>y.Value,StringComparer.Ordinal);invalid["a"]=0;
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,invalid,task.CorrectAnswer));
            }
        }
    }
    [Fact]
    public void GoldenSourceRelationshipsAndGeometryBoundaries()
    {
        var p=new Dictionary<string,int>{{"a",40},{"b",22},{"x",12},{"p",25},{"m",3},{"mode",8},{"variant",0}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"protractor_difference",p,"٦٢"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"angle_addition",p,"62"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"adjacent_angle_equation",p,"12"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"vertical_angle_equation",p,"12"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"intersection_measure",p,"62"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"perpendicular_sufficient",p,"1"));
        p["variant"]=1;
        Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"intersection_measure",p,"118"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Prefix+"intersection_measure",p,"62"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"vertical_angle_equation",p,"62"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"same_reference_congruence",p,"140"));
        p["variant"]=2;
        Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"intersection_measure",p,"40"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"angle_addition",p,"23"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"perpendicular_sufficient",p,"0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Prefix+"perpendicular_sufficient",p,"1"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"bisector_vertical",p,"40"));
        p["variant"]=3;
        Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"angle_addition",p,"114"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"bisector_vertical",p,"45"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"around_point",p,"158"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"congruence_reason",p,"8"));
        p["a"]=75;p["b"]=60;p["p"]=30;
        Assert.True(ExactSkillContractQuestionEngine.Verify(Prefix+"protractor_difference",p,"135"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Prefix+"protractor_difference",p,"165"));
    }
}
