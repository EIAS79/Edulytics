using Edulytics.Core.Mathematics.Visuals;

namespace Edulytics.Web.Presentation;

/// <summary>Original mathematics graphs for OpenStax 2017 Section 4.6 skills.
/// Does not reproduce textbook figures; Stage D rendering is not tested.</summary>
public static class Grade12PrecalculusSection46GraphModels
{
    public sealed record GraphTask(int SourceFigureSkill, string Prompt,
        CoordinatePlaneVisualSpec Graph);
    public static readonly IReadOnlyList<GraphTask> All=Build();

    private static IReadOnlyList<GraphTask> Build()
    {
        var items=new List<GraphTask>();
        Add(2,"Exponential 2^x>0 never crosses y=-1",
            [x => (decimal)Math.Pow(2d,(double)x),x => -1m],-5m,5m,-3m,18m);
        Add(3,"Logarithm ln(x) meets y=2 at x=e²",
            [x => x>0m ? (decimal?)Math.Log((double)x) : null,x => 2m],
            -1m,15m,-4m,5m);
        return items;

        void Add(int n,string prompt,Func<decimal,decimal?>[] funcs,
            decimal xmin,decimal xmax,decimal ymin,decimal ymax)
        {
            var lines=new List<MathematicalSegment>();
            foreach(var f in funcs)
            {
                MathematicalPoint? previous=null;
                for(var index=0; index<=120/Math.Max(1,funcs.Length); index++)
                {
                    var x=xmin+(xmax-xmin)*index/(120m/Math.Max(1,funcs.Length));
                    var y=f(x);
                    if(!y.HasValue || y.Value<ymin || y.Value>ymax)
                    { previous=null; continue; }
                    var point=new MathematicalPoint(x,y.Value);
                    if(previous is not null)
                        lines.Add(new MathematicalSegment(previous,point));
                    previous=point;
                }
            }
            var graph=new CoordinatePlaneVisualSpec(
                new MathematicalAxisWindow(xmin,xmax,ymin,ymax,2m,2m),
                [],lines,new MathematicalVisualAccessibility(
                    $"Original first-party equation figure {n}",
                    prompt+" Independently authored; source illustration not copied."));
            MathematicalVisualSpecValidator.Validate(graph);
            items.Add(new GraphTask(n,prompt,graph));
        }
    }
}
