using System.Text.Json;

namespace Edulytics.Web.Presentation;

/// <summary>
/// First-party original Grade 1 Unit 6 digital Practice diagrams.
/// Illustrations represent equal-sized measurement units, directly aligned
/// endpoints ONLY in the direct-comparison lesson. Indirect lessons compare
/// measurements through a shared reference tool, not aligned endpoints.
/// They do not assert any actual classroom
/// measuring, physical manipulation, source-publisher asset reuse or teacher
/// observation.
/// </summary>
public static partial class PracticeMathVisualRenderer
{
    private static string GradeOneSourceMeasurementVisual(string family,JsonElement p)
    {
        int a=GetInt(p,"a"),b=GetInt(p,"b"),c=GetInt(p,"c"),mode=GetInt(p,"mode");
        if(mode<0||mode>3)throw new JsonException("Invalid Grade 1 measurement mode");
        bool compare=family is "usccss.g1.u06.l01.compare_lengths" or
            "usccss.g1.u06.l02.indirect_length_comparison" or
            "usccss.g1.u06.l03.choose_indirect_comparison";
        bool cubes=family=="usccss.g1.u06.l05.measure_connecting_cubes";
        bool clips=family=="usccss.g1.u06.l06.measure_paperclips";
        bool smallUnits=family=="usccss.g1.u06.l07.unit_length_matters";
        if(!compare&&!cubes&&!clips&&!smallUnits)
            throw new JsonException("Not an original Grade 1 measurement family");
        bool orderThree=family=="usccss.g1.u06.l01.compare_lengths"&&mode%2==1;
        if(compare && (a<4||a>20||b<1||b>20||
             (orderThree?(b<4||c<4||c>20||a==b||a==c||b==c):c!=0)) ||
           (cubes||clips) && (a<2||a>20||b!=0||c!=0) ||
           smallUnits && (a<2||a>14||b!=2||c!=0))
            throw new JsonException("Invalid source measurement operands");
        bool indirect=family is "usccss.g1.u06.l02.indirect_length_comparison" or
            "usccss.g1.u06.l03.choose_indirect_comparison";
        var svg=SvgStart(indirect?
            "Equal-size units: two objects cannot be placed together. Each is measured separately with the SAME reference unit.":
            "Source-based first grade visual. Equal-size units, common starting line, and endpoint comparison.");
        if(indirect)
        {
            // Separate work areas intentionally have DIFFERENT drawing origins:
            // student infers longer/shorter from matching standard unit counts,
            // not from aligning the two objects or comparing their endpoints.
            const double step=11.0;
            int[] origins=[50,95];
            int[] yPositions=[80,167];
            int[] lengths=[a,b];
            bool sourceComparison=family=="usccss.g1.u06.l03.choose_indirect_comparison";
            string[] labels=sourceComparison && mode==1
                ?["Mai route","Clare route"]
                :sourceComparison && mode>=2
                    ?["Door opening",mode==2?"Teacher desk width":"Student desk width"]
                    :["Object A","Object B"];
            for(int i=0;i<2;i++)
            {
                int x0=origins[i],y=yPositions[i];
                Text(svg,x0,y-22,labels[i],"label");
                svg.Append($"<line class='indirect-reference-{i}' x1='{x0}' y1='{y-8}' x2='{x0+220}' y2='{y-8}' stroke='currentColor' stroke-width='1'/>");
                for(int n=0;n<lengths[i];n++)
                {
                    double x=x0+n*step;
                    svg.Append($"<rect class='indirect-unit-{i}' x='{F(x)}' y='{y}' width='{F(step)}' height='28' fill='rgba(33,128,184,.21)' stroke='currentColor' stroke-width='1'/>");
                }
            }
            Text(svg,210,238,sourceComparison && mode>=2
                ?"Compare WIDTH with the SAME reference unit."
                :"Compare through the SAME reference unit.","hint");
            return SvgEnd(svg);
        }
        svg.Append("<line x1='38' y1='39' x2='38' y2='219' stroke='currentColor' stroke-width='2' stroke-dasharray='4 4'/>");
        if(compare)
        {
            double step=330d/(orderThree?Math.Max(a,Math.Max(b,c)):Math.Max(a,b));
            var lengths=orderThree?
                new[]{a,b,c}:new[]{a,b};
            string[] labels=["A","B","C"];
            for(int i=0;i<lengths.Length;i++)
            {
                int y=orderThree?69+i*63:73+i*91;
                Text(svg,52,y-13,labels[i],"label");
                for(int n=0;n<lengths[i];n++)
                {
                    double x=39+n*step;
                    svg.Append($"<rect class='length-{i}' x='{F(x)}' y='{y}' width='{F(step)}' height='24' fill='rgba(33,128,184,.24)' stroke='currentColor' stroke-width='1'/>");
                }
            }
            Text(svg,210,239,orderThree?(mode==1?"Order THREE from shortest to longest.":"Order THREE from longest to shortest."):"Compare the aligned starting endpoints.","hint");
        }
        else if(cubes||clips)
        {
            double step=330d/a;
            Text(svg,54,55,clips?"Identical paper clips":"Equal connecting cubes","hint");
            for(int n=0;n<a;n++)
            {
                double x=39+n*step;
                if(cubes)
                    svg.Append($"<rect class='measure-cube' x='{F(x)}' y='107' width='{F(step)}' height='34' fill='rgba(33,128,184,.22)' stroke='currentColor' stroke-width='1.4'/>");
                else
                    svg.Append($"<rect class='measure-clip' x='{F(x+1)}' y='107' rx='7' ry='7' width='{F(step-2)}' height='34' fill='none' stroke='currentColor' stroke-width='2'/>");
            }
            Text(svg,210,203,"Count units with NO gaps or overlap.","hint");
        }
        else
        {
            // Each large unit is exactly twice the small unit; bar lengths
            // match physically. Do not prefill the requested number of units.
            double bigStep=330d/a;
            Text(svg,52,61,"Long units","hint");
            Text(svg,52,146,"Small units","hint");
            for(int n=0;n<a;n++)
                svg.Append($"<rect class='long-unit' x='{F(39+n*bigStep)}' y='77' width='{F(bigStep)}' height='27' fill='rgba(33,128,184,.26)' stroke='currentColor' stroke-width='1'/>");
            for(int n=0;n<a*b;n++)
                svg.Append($"<rect class='short-unit' x='{F(39+n*bigStep/b)}' y='164' width='{F(bigStep/b)}' height='27' fill='rgba(204,134,41,.28)' stroke='currentColor' stroke-width='1'/>");
            Text(svg,210,236,"Same length, different unit sizes.","hint");
        }
        return SvgEnd(svg);
    }
}
