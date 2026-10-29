using Edulytics.Core.Curriculum;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Verifies exact source-specific Unit 6 quadratic authoring and mathematical
/// invariants, not effective Practice, authentic learner work, or Stage D.
/// </summary>
public sealed class UsCcssAlgebra1Unit6QuadraticRichV2Tests
{
    private const string Prefix="PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U06:";

    [Theory]
    [InlineData("L01","A Different Kind of Change")]
    [InlineData("L02","How Does it Change?")]
    [InlineData("L03","Building Quadratic Functions from Geometric Patterns")]
    [InlineData("L04","Comparing Quadratic and Exponential Functions")]
    [InlineData("L05","Building Quadratic Functions to Describe Situations (Part 1)")]
    [InlineData("L06","Building Quadratic Functions to Describe Situations (Part 2)")]
    [InlineData("L07","Building Quadratic Functions to Describe Situations (Part 3)")]
    [InlineData("L08","Equivalent Quadratic Expressions")]
    [InlineData("L09","Standard Form and Factored Form")]
    [InlineData("L10","Graphs of Functions in Standard and Factored Forms")]
    [InlineData("L11","Graphing from the Factored Form")]
    [InlineData("L12","Graphing the Standard Form (Part 1)")]
    [InlineData("L13","Graphing the Standard Form (Part 2)")]
    [InlineData("L14","Graphs That Represent Situations")]
    [InlineData("L15","Vertex Form")]
    [InlineData("L16","Graphing from the Vertex Form")]
    [InlineData("L17","Changing the Vertex")]
    public void CanonicalLessonHasSourceSpecificMathAndTransparentAssessment(string code,string title)
    {
        var lesson=RichLessonContentV2Registry.Find(Prefix+code,"en");
        Assert.NotNull(lesson);
        Assert.Equal(title,lesson.Title);
        int n=int.Parse(code[1..]);
        Assert.Contains(lesson.ExplanationParagraphs,p=>
            p.Contains("/1/6/"+n+"/preparation.html",StringComparison.Ordinal));
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
        Assert.All(lesson.Visuals,x=>Assert.False(string.IsNullOrWhiteSpace(x.Description)));
        Assert.Empty(lesson.Videos);
        Assert.Contains("teacher",string.Join(" ",lesson.ExplanationParagraphs),
            StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void RegistryContainsExactlySeventeenUniqueUnit6Lessons()
    {
        var rows=RichLessonContentV2Registry.AllDocuments.SelectMany(d=>d.Lessons)
            .Where(x=>x.LessonCode.StartsWith(Prefix,StringComparison.Ordinal)).ToArray();
        Assert.Equal(17,rows.Length);
        Assert.Equal(17,rows.Select(x=>x.LessonCode).Distinct().Count());
    }

    [Fact]
    public void FixedPerimeterAndVisualSquareModelsHaveCorrectSecondDifferences()
    {
        var garden=Enumerable.Range(1,11).Select(w=>w*(12-w)).ToArray();
        Assert.Equal(36,garden.Max());
        Assert.Equal(6,Array.IndexOf(garden,36)+1);
        Assert.Equal(garden[4],garden[6]);
        var squares=Enumerable.Range(1,4).Select(n=>n*n+3).ToArray();
        Assert.Equal([4,7,12,19],squares);
        var differences=squares.Zip(squares.Skip(1),(a,b)=>b-a).ToArray();
        Assert.Equal([3,5,7],differences);
    }

    [Fact]
    public void GeometryAndExponentialRatesRemainDifferent()
    {
        foreach(var n in Enumerable.Range(1,5))
            Assert.Equal(n*(n+4),n*n+4*n);
        Assert.True(9>Math.Pow(2,3));
        Assert.Equal(16,Math.Pow(2,4));
        Assert.True(Math.Pow(2,5)>25);
    }

    [Fact]
    public void GravityProjectileAndDemandModelsRespectDomains()
    {
        Assert.Equal(180,5*6*6);
        Assert.Equal(100,180-5*4*4);
        static double Projectile(double t)=>2+12*t-2*t*t;
        Assert.Equal(20,Projectile(3));
        Assert.Equal(0,Projectile(3+Math.Sqrt(10)),8);
        static int Revenue(int p)=>p*(60-2*p);
        Assert.Equal(450,Revenue(15));
        Assert.Equal(400,Revenue(10));
        Assert.Equal(400,Revenue(20));
        Assert.Equal(0,Revenue(0));
        Assert.Equal(0,Revenue(30));
    }

    [Fact]
    public void AllThreeQuadraticFormsPreserveExactlySameOutputs()
    {
        foreach(var x in Enumerable.Range(-5,14))
        {
            Assert.Equal((x+3)*(x+5),x*x+8*x+15);
            Assert.Equal((x-3)*(x+4),x*x+x-12);
            Assert.Equal((x-2)*(x-6),x*x-8*x+12);
            Assert.Equal(-2*(x-1)*(x-5),-2*x*x+12*x-10);
            Assert.Equal(-2*x*x+8*x+10,-2*(x-5)*(x+1));
            Assert.Equal(-2*x*x+8*x+10,-2*(x-2)*(x-2)+18);
        }
    }

    [Fact]
    public void QuadraticParametersAndVertexTranslationsAreChecked()
    {
        static double A(double x)=>2*x*x+3;
        static double B(double x)=>-x*x+3;
        static double C(double x)=>.5*x*x+3;
        Assert.Equal(11,A(2));
        Assert.Equal(-1,B(2));
        Assert.Equal(5,C(2));
        static double F(double x)=>x*x-6*x;
        static double G(double x)=>x*x+2*x;
        Assert.Equal(-9,F(3));
        Assert.Equal(-1,G(-1));
        Assert.Equal(0,F(0));
        Assert.Equal(0,G(0));
        static double H(double x)=>2*Math.Pow(x-3,2)+4;
        Assert.Equal(4,H(3));
        Assert.Equal(12,H(1));
        Assert.Equal(12,H(5));
        static double J(double x)=>-.5*Math.Pow(x+2,2)+6;
        Assert.Equal(6,J(-2));
        Assert.Equal(4,J(0));
        Assert.Equal(4,J(-4));
        static double K(double x)=>2*Math.Pow(x-3,2)-2;
        Assert.Equal(0,K(2));
        Assert.Equal(0,K(4));
        Assert.Equal(-2,K(3));
    }
}
