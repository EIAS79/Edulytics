using Edulytics.Core.Curriculum;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Exact-authoring and numerical oracle tests for Algebra 1 Unit 5.
/// They cannot replace exact Practice, genuine interactions or Stage D evidence.
/// </summary>
public sealed class UsCcssAlgebra1Unit5ExponentialRichV2Tests
{
    private const string Prefix="PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U05:";

    [Theory]
    [InlineData("L01","Growing and Growing")]
    [InlineData("L02","Patterns of Growth")]
    [InlineData("L03","Representing Exponential Growth")]
    [InlineData("L04","Understanding Decay")]
    [InlineData("L05","Representing Exponential Decay")]
    [InlineData("L06","Analyzing Graphs")]
    [InlineData("L07","Using Negative Exponents")]
    [InlineData("L08","Exponential Situations as Functions")]
    [InlineData("L09","Interpreting Exponential Functions")]
    [InlineData("L10","Looking at Rates of Change")]
    [InlineData("L11","Modeling Exponential Behavior")]
    [InlineData("L12","Reasoning about Exponential Graphs (Part 1)")]
    [InlineData("L13","Reasoning about Exponential Graphs (Part 2)")]
    [InlineData("L14","Recalling Percent Change")]
    [InlineData("L15","Functions Involving Percent Change")]
    [InlineData("L16","Compounding Interest")]
    [InlineData("L17","Different Compounding Intervals")]
    [InlineData("L18","Expressed in Different Ways")]
    [InlineData("L19","Which One Changes Faster?")]
    [InlineData("L20","Changes over Equal Intervals")]
    [InlineData("L21","Predicting Populations")]
    public void LessonHasCanonicalSourceAndIndependentExamples(string number,string expectedTitle)
    {
        var lesson=RichLessonContentV2Registry.Find(Prefix+number,"en");
        Assert.NotNull(lesson);
        Assert.Equal(expectedTitle,lesson.Title);
        var n=int.Parse(number[1..]);
        Assert.Contains(lesson.ExplanationParagraphs,x=>
            x.Contains("/1/5/"+n+"/preparation.html",StringComparison.Ordinal));
        Assert.True(lesson.ExplanationParagraphs.Count>=3);
        Assert.True(lesson.KeyConcepts.Count>=3);
        Assert.True(lesson.WorkedExamples.Count>=3);
        Assert.All(lesson.WorkedExamples,x=>
        {
            Assert.True(x.Steps.Count>=3);
            Assert.False(string.IsNullOrWhiteSpace(x.Answer));
            Assert.False(string.IsNullOrWhiteSpace(x.Check));
        });
        Assert.True(lesson.CommonMistakes.Count>=2);
        Assert.True(lesson.SummaryPoints.Count>=3);
        Assert.NotEmpty(lesson.Visuals);
        Assert.All(lesson.Visuals,v=>Assert.False(string.IsNullOrWhiteSpace(v.Description)));
        Assert.Empty(lesson.Videos);
    }

    [Fact]
    public void EveryUnit5CanonicalSidecarIsUnique()
    {
        var entries=RichLessonContentV2Registry.AllDocuments
            .SelectMany(d=>d.Lessons)
            .Where(l=>l.LessonCode.StartsWith(Prefix,StringComparison.Ordinal)).ToArray();
        Assert.Equal(21,entries.Length);
        Assert.Equal(21,entries.Select(x=>x.LessonCode).Distinct().Count());
    }

    [Fact]
    public void LinearVersusExponentialModelHasGenuineCrossover()
    {
        static int A(int t)=>100+30*t;
        static int B(int t)=>10*(int)Math.Pow(2,t);
        Assert.True(B(4)<A(4));
        Assert.True(B(5)>A(5));
        Assert.Equal(310,A(7));
        Assert.Equal(1280,B(7));
        Assert.Equal(45.5625,6*Math.Pow(1.5,5),8);
    }

    [Fact]
    public void ExponentialDecayUsesRetainedNotLostFraction()
    {
        Assert.Equal(640,640*Math.Pow(.75,0),8);
        Assert.Equal(480,640*Math.Pow(.75,1),8);
        Assert.Equal(360,640*Math.Pow(.75,2),8);
        Assert.Equal(270,640*Math.Pow(.75,3),8);
        Assert.Equal(16,256*Math.Pow(.5,4),8);
        Assert.True(600*Math.Pow(.6,1)>400*Math.Pow(.8,1));
        Assert.True(600*Math.Pow(.6,2)<400*Math.Pow(.8,2));
        Assert.Equal(25,100*Math.Pow(2,-2),8);
    }

    [Fact]
    public void ExponentialRatesAndRegroupingAreDimensionallyCorrect()
    {
        static double E(int t)=>10*Math.Pow(2,t);
        Assert.Equal(10,E(1)-E(0),8);
        Assert.Equal(20,E(2)-E(1),8);
        Assert.Equal(70d/3,(E(3)-E(0))/3,8);
        Assert.Equal(Math.Pow(1.01,36),Math.Pow(Math.Pow(1.01,12),3),8);
        Assert.Equal(Math.Pow(1.01,36),Math.Pow(Math.Pow(1.01,6),6),8);
        Assert.NotEqual(Math.Pow(1.01,36),Math.Pow(Math.Pow(1.01,6),3));
    }

    [Fact]
    public void CompoundingPercentAndUnitConversionPassIndependentChecks()
    {
        Assert.Equal(45,80*.75*.75,8);
        Assert.Equal(198,200*1.1*.9,8);
        Assert.Equal(484,400*Math.Pow(1.1,2),8);
        Assert.Equal(532.4,400*Math.Pow(1.1,3),8);
        Assert.Equal(1126.8250301319697,1000*Math.Pow(1.01,12),5);
        Assert.Equal(1240,1000*1.24,8);
        Assert.Equal(1268.2417945625455,1000*Math.Pow(1.02,12),5);
        Assert.Equal(1.26247696,Math.Pow(1.06,4),8);
        Assert.Equal(2.25,Math.Pow(1.5,2),8);
    }

    [Fact]
    public void NonconsecutiveSamplesNeedRootOfTimeStepFactor()
    {
        var twoStepFactor=180d/320d;
        var perStepFactor=Math.Sqrt(twoStepFactor);
        Assert.Equal(.75,perStepFactor,8);
        Assert.Equal(240,320*perStepFactor,8);
        Assert.Equal(135,320*Math.Pow(perStepFactor,3),8);
        Assert.Equal(120,60*Math.Pow(2,1));
        Assert.Equal(120,240*Math.Pow(.5,1));
    }

    [Fact]
    public void ModelsCannotClaimEmpiricalObservationsAndUnitsMustBeHonest()
    {
        int[] townA=[800,900,1000,1100];
        int[] townB=[200,300,450,675];
        int[] townC=[500,600,620,800];
        Assert.All(Enumerable.Range(0,3),i=>Assert.Equal(100,townA[i+1]-townA[i]));
        Assert.All(Enumerable.Range(0,3),i=>Assert.Equal(1.5,(double)townB[i+1]/townB[i],8));
        Assert.NotEqual(townC[1]-townC[0],townC[2]-townC[1]);
        Assert.Equal(1200,800+100*4);
        Assert.Equal(1012.5,200*Math.Pow(1.5,4),8);
        var last=RichLessonContentV2Registry.Find(Prefix+"L21","en");
        Assert.NotNull(last);
        Assert.Contains("fictional",string.Join(" ",last.ExplanationParagraphs),StringComparison.OrdinalIgnoreCase);
    }
}
