using Edulytics.Core.Mathematics.Visuals;
namespace Edulytics.Web.Presentation;
/// <summary>19 independently authored unit-circle mathematical models for Section5.2.</summary>
public static class Grade12PrecalculusSection52UnitCircleModels
{
    public sealed record CircleTask(int SourceFigureSkill,string Prompt,CoordinatePlaneVisualSpec Graph);
    public static readonly IReadOnlyList<CircleTask> All=Build();
    private static IReadOnlyList<CircleTask> Build()
    {
        var result=new List<CircleTask>();
        Add(1,0d,"Unit circle definition");
        Add(2,30d,"Point (cos,sin) interpretation");
        Add(3,45d,"Sine and cosine graph axes");
        Add(4,60d,"Pythagorean identity triangle on unit circle");
        Add(5,90d,"Radian rotations and unit point");
        Add(6,120d,"Quadrantal angle positions");
        Add(7,135d,"Reference triangle");
        Add(8,150d,"Special anglesπ/4");
        Add(9,180d,"Special anglesπ/6");
        Add(10,210d,"Special anglesπ/3");
        Add(11,225d,"Reference angle in QII");
        Add(12,240d,"Reference angle in QIII");
        Add(13,270d,"Reference angle in QIV");
        Add(14,300d,"Unit circle ordered pairs");
        Add(15,315d,"Cyclic point positions");
        Add(16,330d,"Sine and cosine sign chart");
        Add(17,45d,"Radius scaling on circle");
        Add(18,75d,"Carousel rotation in unit circle");
        Add(19,60d,"Independent first quadrant reference");
        return result;
        void Add(int number,double angleDegrees,string label)
        {
            var lines=new List<MathematicalSegment>();
            for(int n=1;n<=120;n++)
            {
                var t0=2*Math.PI*(n-1)/120d;
                var t1=2*Math.PI*n/120d;
                lines.Add(new MathematicalSegment(
                    new MathematicalPoint((decimal)Math.Cos(t0),(decimal)Math.Sin(t0)),
                    new MathematicalPoint((decimal)Math.Cos(t1),(decimal)Math.Sin(t1))));
            }
            var theta=angleDegrees*Math.PI/180d;
            var origin=new MathematicalPoint(0m,0m);
            var x=(decimal)Math.Cos(theta);var y=(decimal)Math.Sin(theta);
            lines.Add(new MathematicalSegment(origin,new MathematicalPoint(x,y)));
            lines.Add(new MathematicalSegment(new MathematicalPoint(x,0m),new MathematicalPoint(x,y)));
            var model=new CoordinatePlaneVisualSpec(
                new MathematicalAxisWindow(-1.5m,1.5m,-1.5m,1.5m,0.5m,0.5m),
                [],lines,new MathematicalVisualAccessibility(
                    $"First-party unit circle figure skill {number}",
                    label+". Independent unit-circle diagram with coordinates (cos theta, sin theta)."));
            MathematicalVisualSpecValidator.Validate(model);
            result.Add(new CircleTask(number,label,model));
        }
    }
}
