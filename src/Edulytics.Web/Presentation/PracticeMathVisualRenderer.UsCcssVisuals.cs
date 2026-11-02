using System.Text;
using System.Text.Json;

namespace Edulytics.Web.Presentation;

/// <summary>
/// Learner-facing, source-parameter-grounded visual scaffolds for the reviewed
/// Common Core graph and vector question families. These visuals DO NOT
/// constitute grading of a learner-drawn graph or vector construction.
/// </summary>
public static partial class PracticeMathVisualRenderer
{
    private static string UsRationalGraph(JsonElement p, bool hasHole)
    {
        var pole = GetInt(p, "pole");
        var numerator = hasHole ? 0 : GetInt(p, "numerator");
        var removed = hasHole ? GetInt(p, "removed") : pole;
        var root = hasHole ? GetInt(p, "root") : 0;

        var left = hasHole ? Math.Min(pole, removed) - 4.0 : pole - 6.0;
        var right = hasHole ? Math.Max(pole, removed) + 4.0 : pole + 6.0;
        var span = right - left;
        // The canceled-factor hole can lie beyond the former fixed +/-8
        // plotting range. Scale the viewport to retain the open-circle mark.
        // Keep the original scale for ordinary holes and pole-only graphs.
        double Value(double x) => hasHole ? (x - root) / (x - pole) : numerator / (x - pole);
        var holeValue = hasHole ? Value(removed) : double.NaN;
        var yLimit = double.IsFinite(holeValue)
            ? Math.Max(8d, Math.Abs(holeValue) + 1d)
            : 8d;
        double X(double x) => 32 + 356 * (x - left) / span;
        double Y(double y) => 130 - y * 100d / yLimit;
        var sb = SvgStart("Graph showing the branches of the rational expression and marked excluded input locations.");
        sb.Append("<line x1='32' y1='130' x2='390' y2='130' class='axis'/>");
        if (left <= 0 && right >= 0)
            sb.Append($"<line x1='{F(X(0))}' y1='25' x2='{F(X(0))}' y2='235' class='axis'/>");

        for (var i = 0; i <= 8; i++)
        {
            var x = left + span * i / 8d;
            sb.Append($"<line x1='{F(X(x))}' y1='126' x2='{F(X(x))}' y2='134' class='tick'/>");
        }

        // Do not connect separate branches across a pole or over a removed point.
        var segment = new StringBuilder();
        var previous = double.NaN;
        void Flush()
        {
            if (segment.Length > 0)
            {
                sb.Append($"<polyline points='{segment}' class='shape'/>");
                segment.Clear();
            }
        }
        for (var i = 0; i <= 400; i++)
        {
            var x = left + span * i / 400d;
            var crossesPole = !double.IsNaN(previous) &&
                (previous - pole) * (x - pole) <= 0;
            var crossesHole = hasHole && !double.IsNaN(previous) &&
                (previous - removed) * (x - removed) <= 0;
            if (crossesPole || crossesHole)
                Flush();
            previous = x;
            if (Math.Abs(x - pole) < 0.000001 ||
                (hasHole && Math.Abs(x - removed) < 0.000001))
            {
                Flush();
                continue;
            }
            var y = Value(x);
            if (!double.IsFinite(y) || Math.Abs(y) > yLimit)
            {
                Flush();
                continue;
            }
            if (segment.Length > 0)
                segment.Append(' ');
            segment.Append($"{F(X(x))},{F(Y(y))}");
        }
        Flush();

        sb.Append($"<line x1='{F(X(pole))}' y1='25' x2='{F(X(pole))}' y2='232' class='axis dash'/>");
        if (hasHole && double.IsFinite(holeValue))
            sb.Append($"<circle cx='{F(X(removed))}' cy='{F(Y(holeValue))}' r='6' fill='white' stroke='currentColor' stroke-width='2'/>");
        Text(sb, 385, 148, "x", "hint");
        Text(sb, 30, 24, "y", "hint");
        Text(sb, 210, 252, "Dashed line: excluded-input asymptote. Open circle: removable hole.", "hint");
        return SvgEnd(sb);
    }

    private static string UsVectorHeadTailDiagram(JsonElement p)
    {
        var x0=GetInt(p,"startX");
        var y0=GetInt(p,"startY");
        var ax=GetInt(p,"ax");
        var ay=GetInt(p,"ay");
        var bx=GetInt(p,"bx");
        var by=GetInt(p,"by");
        var x1=x0+ax;
        var y1=y0+ay;
        var x2=x1+bx;
        var y2=y1+by;
        var maxAbs=new[] { Math.Abs(x0),Math.Abs(y0),Math.Abs(x1),Math.Abs(y1),Math.Abs(x2),Math.Abs(y2),4 }.Max();
        var step=Math.Min(170d,95d)/(maxAbs+2);
        double X(double x)=>210+x*step;
        double Y(double y)=>130-y*step;
        var sb=SvgStart("Coordinate plane: vector a from the start to its head; vector b begins at the head of a and ends at the final point.");
        sb.Append("<line x1='36' y1='130' x2='384' y2='130' class='axis'/>");
        sb.Append("<line x1='210' y1='20' x2='210' y2='240' class='axis'/>");
        VectorArrow(sb,X(x0),Y(y0),X(x1),Y(y1),"a");
        VectorArrow(sb,X(x1),Y(y1),X(x2),Y(y2),"b");
        sb.Append($"<circle cx='{F(X(x0))}' cy='{F(Y(y0))}' r='4' class='point'/>");
        sb.Append($"<circle cx='{F(X(x2))}' cy='{F(Y(y2))}' r='5' fill='white' stroke='currentColor' stroke-width='2'/>");
        Text(sb,X(x0)+8,Y(y0)-9,"Start","hint");
        Text(sb,X(x2)+7,Y(y2)+15,"End","hint");
        Text(sb,210,252,"Head-to-tail construction (the final coordinate is not labeled).","hint");
        return SvgEnd(sb);
    }

    private static string UsVectorScalarDiagram(JsonElement p)
    {
        var x=GetInt(p,"x");
        var y=GetInt(p,"y");
        var k=GetInt(p,"k");
        var kx=(double)k*x;
        var ky=(double)k*y;
        var maxAbs=Math.Max(4,Math.Max(Math.Max(Math.Abs(x),Math.Abs(y)),Math.Max(Math.Abs(kx),Math.Abs(ky))));
        var step=90/(maxAbs+2);
        double X(double a)=>210+a*step;
        double Y(double a)=>130-a*step;
        var sb=SvgStart("Coordinate plane showing a nonzero vector and the vector after multiplying by the given scalar.");
        sb.Append("<line x1='35' y1='130' x2='385' y2='130' class='axis'/>");
        sb.Append("<line x1='210' y1='20' x2='210' y2='240' class='axis'/>");
        VectorArrow(sb,210,130,X(x),Y(y),"v");
        if(k!=0)
            VectorArrow(sb,210,130,X(kx),Y(ky),"kv",secondary:true);
        else
            sb.Append("<circle cx='210' cy='130' r='7' fill='white' stroke='currentColor' stroke-width='2'/>");
        Text(sb,210,250,"Solid: v. Dashed: kv (zero scalar gives a point at the origin).","hint");
        return SvgEnd(sb);
    }

    private static void VectorArrow(StringBuilder sb,double x1,double y1,double x2,double y2,string label,bool secondary=false)
    {
        var dx=x2-x1;
        var dy=y2-y1;
        var length=Math.Sqrt(dx*dx+dy*dy);
        if(length<0.001)
        {
            sb.Append($"<circle cx='{F(x1)}' cy='{F(y1)}' r='4' class='point'/>");
            return;
        }
        var ux=dx/length;
        var uy=dy/length;
        var px=-uy;
        var py=ux;
        var arrow=8d;
        var baseX=x2-ux*arrow;
        var baseY=y2-uy*arrow;
        sb.Append($"<line x1='{F(x1)}' y1='{F(y1)}' x2='{F(x2)}' y2='{F(y2)}' class='vector{(secondary?" secondary":"")}'/>");
        sb.Append($"<polygon points='{F(x2)},{F(y2)} {F(baseX+px*4)},{F(baseY+py*4)} {F(baseX-px*4)},{F(baseY-py*4)}' class='arrow-head'/>");
        Text(sb,(x1+x2)/2+7,(y1+y2)/2-9,label,"label");
    }
}