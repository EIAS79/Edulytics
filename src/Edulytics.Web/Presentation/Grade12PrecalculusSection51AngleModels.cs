using Edulytics.Core.Mathematics.Visuals;
namespace Edulytics.Web.Presentation;
/// <summary>Independent directed-angle diagrams for Section 5.1.
/// Source graphics not copied; runtime validation reserved for Stage D.</summary>
public static class Grade12PrecalculusSection51AngleModels
{
    public sealed record AngleTask(int SourceFigureSkill,string Prompt,CoordinatePlaneVisualSpec Graph);
    public static readonly IReadOnlyList<AngleTask> All=Build();
    private static IReadOnlyList<AngleTask> Build()
    {
        var tasks=new List<AngleTask>();
        Add(1,10d,"Ray with endpoint and arrow");
        Add(2,60d,"Two rays sharing vertex");
        Add(3,45d,"Named angle theta");
        Add(4,60d,"Initial/terminal ray directed angle");
        Add(5,120d,"Standard-position vertex and +x ray");
        Add(6,90d,"Quarter and full revolutions");
        Add(7,180d,"Quadrantal terminal axes");
        Add(8,30d,"Positive 30° rotation");
        Add(9,-135d,"Negative 135° rotation");
        Add(10,360d,"Circumference divided by radius lengths");
        Add(11,57.2958d,"One radian subtends arc length equal radius");
        Add(12,114.5916d,"Radian as arc/radius");
        Add(13,45d,"45° sector independent of radius");
        Add(14,225d,"Common angles in degrees");
        Add(15,300d,"Common angles in radians");
        Add(16,140d,"Coterminal positive and negative angle");
        Add(17,135d,"Acute reference angles by quadrant");
        Add(18,800d,"Angles greater than full revolution");
        Add(19,-45d,"Negative angles and positive coterminal");
        Add(20,855d,"Radian coterminal multiple turns");
        Add(21,120d,"Arc length proportional to central angle");
        Add(22,90d,"Sector area proportional to central angle");
        Add(23,30d,"Sprinkler's angular sector");
        Add(24,72d,"Wheel rotation and angular speed");
        Add(25,140d,"Arc of circle with given radius and angle");
        Add(26,72d,"Sector of circle with radians");
        return tasks;
        void Add(int number,double degrees,string label)
        {
            var lines=new List<MathematicalSegment>();
            var origin=new MathematicalPoint(0m,0m);
            lines.Add(new MathematicalSegment(origin,new MathematicalPoint(3m,0m)));
            var a=degrees*Math.PI/180d;
            var tip=new MathematicalPoint((decimal)(3*Math.Cos(a)),(decimal)(3*Math.Sin(a)));
            lines.Add(new MathematicalSegment(origin,tip));
            for(int j=1;j<=100;j++)
            {
                var pa=a*(j-1)/100d; var pb=a*j/100d;
                var start=new MathematicalPoint((decimal)(1.25*Math.Cos(pa)),(decimal)(1.25*Math.Sin(pa)));
                var end=new MathematicalPoint((decimal)(1.25*Math.Cos(pb)),(decimal)(1.25*Math.Sin(pb)));
                lines.Add(new MathematicalSegment(start,end));
            }
            var graph=new CoordinatePlaneVisualSpec(
                new MathematicalAxisWindow(-4m,4m,-4m,4m,1m,1m),
                [],lines,new MathematicalVisualAccessibility(
                    $"Angle skill {number}: {degrees} degrees",
                    label+". Independently authored math sketch, not original publisher artwork."));
            MathematicalVisualSpecValidator.Validate(graph);
            tasks.Add(new AngleTask(number,label,graph));
        }
    }
}
