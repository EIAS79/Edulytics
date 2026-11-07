using Edulytics.Core.Mathematics.Visuals;
namespace Edulytics.Web.Presentation;
/// <summary>First-party trig identity source skills; excludes publisher artwork.</summary>
public static class Grade12PrecalculusSection71IdentityGraphs
{
    public sealed record Task(int SourceFigureSkill,string Prompt,CoordinatePlaneVisualSpec Graph);
    public static readonly IReadOnlyList<Task> All=Build();
    private static IReadOnlyList<Task> Build()
    {
        var items=new List<Task>();
        Add(2,"Sine is odd",x=>Math.Sin(x));
        Add(3,"Cosine is even",x=>Math.Cos(x));
        Add(4,"Cotangent equals 1/tangent on common domain",x=>Math.Cos(x)/Math.Sin(x));
        return items;
        void Add(int n,string label,Func<double,double> f)
        {
            var lines=new List<MathematicalSegment>();
            MathematicalPoint? prev=null;
            for(var i=0;i<=300;i++)
            {
                var x=-Math.PI*2+Math.PI*4*i/300d;
                var y=f(x);
                if(double.IsNaN(y)||double.IsInfinity(y)||Math.Abs(y)>7 || (n==4 && Math.Abs(Math.Sin(x))<0.05))
                { prev=null;continue; }
                var next=new MathematicalPoint((decimal)x,(decimal)y);
                if(prev is not null && Math.Abs((double)(next.Y-prev.Y))<1d)
                    lines.Add(new MathematicalSegment(prev,next));
                prev=next;
            }
            var graph=new CoordinatePlaneVisualSpec(
                new MathematicalAxisWindow(-7m,7m,-8m,8m,2m,2m),
                [],lines,new MathematicalVisualAccessibility(
                    $"First-party Section7.1 graph skill {n}",label));
            MathematicalVisualSpecValidator.Validate(graph);
            items.Add(new Task(n,label,graph));
        }
    }
}
