using Edulytics.Core.Curriculum;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UsCcssAlgebra1Unit7SolvingQuadraticsRichV2Tests
{
    private const string Prefix="PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U07:";

    [Fact]
    public void AllTwentyFourLessonsAreSourceLinkedAndAcademicallySubstantial()
    {
        var expected=new[]{
            "Finding Unknown Inputs",
            "When and Why Do We Write Quadratic Equations?",
            "Solving Quadratic Equations by Reasoning",
            "Solving Quadratic Equations with the Zero Product Property",
            "How Many Solutions?",
            "Rewriting Quadratic Expressions in Factored Form (Part 1)",
            "Rewriting Quadratic Expressions in Factored Form (Part 2)",
            "Rewriting Quadratic Expressions in Factored Form (Part 3)",
            "Solving Quadratic Equations by Using Factored Form",
            "Rewriting Quadratic Expressions in Factored Form (Part 4)",
            "What are Perfect Squares?",
            "Completing the Square (Part 1)",
            "Completing the Square (Part 2)",
            "Completing the Square (Part 3)",
            "Quadratic Equations with Irrational Solutions",
            "The Quadratic Formula",
            "Applying the Quadratic Formula (Part 1)",
            "Applying the Quadratic Formula (Part 2)",
            "Deriving the Quadratic Formula",
            "Rational and Irrational Solutions",
            "Sums and Products of Rational and Irrational Numbers",
            "Rewriting Quadratic Expressions in Vertex Form",
            "Using Quadratic Expressions in Vertex Form to Solve Problems",
            "Using Quadratic Equations to Model Situations and Solve Problems"
        };
        for(int n=1;n<=expected.Length;n++)
        {
            var code=Prefix+"L"+n.ToString("D2");
            var lesson=RichLessonContentV2Registry.Find(code,"en");
            Assert.NotNull(lesson);
            Assert.Equal(expected[n-1],lesson.Title);
            Assert.Contains(lesson.ExplanationParagraphs,p=>
                p.Contains("/1/7/"+n+"/preparation.html",StringComparison.Ordinal));
            Assert.True(lesson.KeyConcepts.Count>=3);
            Assert.True(lesson.WorkedExamples.Count>=3);
            Assert.All(lesson.WorkedExamples,e=>
            {
                Assert.True(e.Steps.Count>=3);
                Assert.False(string.IsNullOrWhiteSpace(e.Answer));
                Assert.False(string.IsNullOrWhiteSpace(e.Check));
            });
            Assert.True(lesson.CommonMistakes.Count>=2);
            Assert.True(lesson.SummaryPoints.Count>=3);
            Assert.NotEmpty(lesson.Visuals);
        }
        var rows=RichLessonContentV2Registry.AllDocuments.SelectMany(d=>d.Lessons)
            .Where(l=>l.LessonCode.StartsWith(Prefix,StringComparison.Ordinal)).ToArray();
        Assert.Equal(24,rows.Length);
        Assert.Equal(24,rows.Select(x=>x.LessonCode).Distinct().Count());
    }

    [Fact]
    public void FactoringIdentitiesAndSolutionsMatchIndependentArithmetic()
    {
        foreach(var x in Enumerable.Range(-8,18))
        {
            Assert.Equal(x*x+10*x+24,(x+4)*(x+6));
            Assert.Equal(x*x-x-12,(x-4)*(x+3));
            Assert.Equal(x*x-49,(x-7)*(x+7));
            Assert.Equal(2*x*x+7*x+3,(2*x+1)*(x+3));
            Assert.Equal(3*x*x-14*x+8,(3*x-2)*(x-4));
            Assert.Equal(x*x+12*x+36,(x+6)*(x+6));
        }
        Assert.Equal(12,(-6)*(-6)+4*(-6));
        Assert.Equal(12,2*2+4*2);
    }

    [Fact]
    public void CompleteSquareAndQuadraticFormulaKeepBothSignedRoots()
    {
        foreach(var x in new[]{1d,-9d})
            Assert.Equal(9,x*x+8*x,8);
        foreach(var x in new[]{.25,-.75})
            Assert.Equal(3d/16,x*x+.5*x,8);
        foreach(var x in new[]{(-3+Math.Sqrt(13))/4,(-3-Math.Sqrt(13))/4})
            Assert.Equal(.25,x*x+1.5*x,8);
        foreach(var x in new[]{1d,-5d})
            Assert.Equal(0,2*x*x+8*x-10,8);
        foreach(var x in new[]{-.5*0+(-2d),.5d})
            Assert.Equal(0,2*x*x+3*x-2,8);
        Assert.True(2*2-4*3*2<0);
    }

    [Fact]
    public void QuadraticContextAndExactVertexModelsAreVerified()
    {
        static double H(double t)=>-t*t+6*t+2;
        static double Platform(double t)=>t+6;
        Assert.Equal(H(1),Platform(1));
        Assert.Equal(H(4),Platform(4));
        Assert.Equal(11,H(3));
        Assert.Equal(0,H(3+Math.Sqrt(11)),8);
        Assert.Equal(8,H(3+Math.Sqrt(3)),8);
        foreach(var x in Enumerable.Range(-5,11))
        {
            Assert.Equal(x*x-6*x+5,Math.Pow(x-3,2)-4);
            Assert.Equal(2*x*x+8*x+3,2*Math.Pow(x+2,2)-5);
        }
        Assert.Equal(100,10*(20-10));
        Assert.Equal(84,6*(20-6));
        Assert.Equal(84,14*(20-14));
    }
}
