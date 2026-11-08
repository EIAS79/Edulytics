using Edulytics.Core.Mathematics.Visuals;
namespace Edulytics.Web.Presentation;
/// <summary>First-party periodic, damped and seasonal mathematical models.</summary>
public static class Grade12PrecalculusSection76PhysicalModelGraphs
{
    public sealed record Task(int SourceFigureSkill,string Prompt,CoordinatePlaneVisualSpec Graph);
    public static readonly IReadOnlyList<Task> All=Build();
    private static IReadOnlyList<Task> Build()
    {
        var items=new List<Task>();
        Add(1,2d,0.500000d,0.000000d,false);
        Add(2,3d,0.750000d,0.000000d,false);
        Add(3,4d,1.000000d,0.000000d,false);
        Add(4,5d,0.500000d,0.000000d,false);
        Add(5,6d,0.750000d,0.000000d,false);
        Add(6,2d,1.000000d,0.000000d,false);
        Add(7,3d,0.500000d,0.000000d,false);
        Add(8,4d,0.750000d,0.000000d,false);
        Add(9,5d,1.000000d,0.080000d,false);
        Add(10,6d,0.500000d,0.080000d,false);
        Add(11,2d,0.750000d,0.080000d,false);
        Add(12,3d,1.000000d,0.080000d,false);
        Add(13,4d,0.500000d,0.080000d,false);
        Add(14,5d,0.750000d,0.080000d,false);
        Add(15,6d,1.000000d,0.080000d,false);
        Add(16,2d,0.500000d,0.080000d,false);
        Add(17,3d,0.750000d,0.000000d,true);
        Add(18,4d,1.000000d,0.000000d,true);
        Add(19,5d,0.500000d,0.000000d,true);
        Add(20,6d,0.750000d,0.000000d,true);
        Add(21,2d,1.000000d,0.000000d,true);
        Add(22,3d,0.500000d,0.000000d,true);
        Add(23,4d,0.750000d,0.000000d,true);
        return items;
        void Add(int n,double amplitude,double frequency,double decay,bool trend)
        {
            var lines=new List<MathematicalSegment>();
            MathematicalPoint? previous=null;
            for(int j=0;j<=120;j++)
            {
                var x=12d*j/120d;
                var y=amplitude*Math.Exp(-decay*x)*Math.Cos(2d*Math.PI*frequency*x)
                    +(trend ? .2d*x : 0d);
                var next=new MathematicalPoint((decimal)x,(decimal)y);
                if(previous is not null)
                    lines.Add(new MathematicalSegment(previous,next));
                previous=next;
            }
            var graph=new CoordinatePlaneVisualSpec(
                new MathematicalAxisWindow(0m,12m,-9m,9m,2m,2m),
                [],lines,new MathematicalVisualAccessibility(
                    $"Original physical modeling skill {n}",
                    "Independently plotted periodic or damped harmonic curve."));
            MathematicalVisualSpecValidator.Validate(graph);
            items.Add(new Task(n,trend?"Seasonal trend":decay>0d?"Damped motion":"Undamped periodic",graph));
        }
    }
}
