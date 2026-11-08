from pathlib import Path
import json,datetime
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
ap=base/"u03-l08-source-inventory-and-gap-audit.json"
a=json.loads(ap.read_text(encoding="utf8"))
assert a["FirstPartyIndependentFigureSkillAlternativesAuthored"]==9
code="""using Edulytics.Core.Mathematics.Visuals;

namespace Edulytics.Web.Presentation;

/// <summary>
/// Independently drawn inverse-function models for Section 3.8. Reuses the
/// established SVG coordinate plane renderer. None are publisher figures.
/// </summary>
public static class Grade12PrecalculusSection38InverseGraphs
{
    public sealed record InversePair(int RelatedExercise, string Title,
        string DomainConstraint, CoordinatePlaneVisualSpec Graph);

    public static readonly IReadOnlyList<InversePair> All = Build();

    private static IReadOnlyList<InversePair> Build()
    {
        var pairs = new List<InversePair>();
        Add(35, "Restricted quadratic and principal square root",
            "f(x)=(x-4)² on x≥4 and g(x)=4+√x on x≥0",
            x => (x-4m)*(x-4m), 4m, 6.3m,
            x => 4m+(decimal)Math.Sqrt((double)x), 0m, 7m);
        Add(36, "Cubic and cube-root inverse",
            "f(x)=x³+1 and g(x)=∛(x-1) on all real inputs",
            x => x*x*x+1m, -1.65m, 1.85m,
            x => (decimal)Math.Cbrt((double)(x-1m)), -5m, 8m);
        Add(32, "Radical and its restricted quadratic inverse",
            "f(x)=1+√(x-1) for x≥1 and g(x)=(x-1)²+1 for x≥1",
            x => 1m+(decimal)Math.Sqrt((double)(x-1m)), 1m, 8m,
            x => (x-1m)*(x-1m)+1m, 1m, 3.5m);
        Add(40, "Reciprocal and its inverse",
            "f(x)=4/x is its own inverse for x≠0; compare reflection in y=x",
            x => 4m/x, -6m, 6m,
            x => 4m/x, -6m, 6m, [0m], [0m]);
        return pairs;

        void Add(int exercise, string title, string condition,
            Func<decimal,decimal> f, decimal fStart, decimal fEnd,
            Func<decimal,decimal> inverse, decimal inverseStart, decimal inverseEnd,
            decimal[]? fPoles = null, decimal[]? inversePoles = null)
        {
            var segments = new List<MathematicalSegment>();
            Plot(f, fStart, fEnd, fPoles ?? [], segments);
            Plot(inverse, inverseStart, inverseEnd, inversePoles ?? [], segments);
            // y=x is the geometric reflection axis for both functions.
            segments.Add(new MathematicalSegment(
                new MathematicalPoint(-5m,-5m), new MathematicalPoint(8m,8m)));
            var spec = new CoordinatePlaneVisualSpec(
                new MathematicalAxisWindow(-6m,8m,-6m,8m,1m,1m),
                [], segments,
                new MathematicalVisualAccessibility(
                    title, condition + ". Model includes original curve, its inverse, " +
                    "and reference line y=x. Explain the swapped input-output pairs. " +
                    "Separate real-domain branches remain disconnected at poles."));
            MathematicalVisualSpecValidator.Validate(spec);
            pairs.Add(new InversePair(exercise,title,condition,spec));
        }

        static void Plot(Func<decimal,decimal> value, decimal lo, decimal hi,
            decimal[] poles, List<MathematicalSegment> segments)
        {
            MathematicalPoint? previous = null;
            decimal? priorX = null;
            var step = (hi-lo)/160m;
            for (var i=0;i<=160;i++)
            {
                var x=lo+i*step;
                if (poles.Any(p => Math.Abs(x-p)<.10m))
                {
                    previous=null;
                    priorX=null;
                    continue;
                }
                var y=value(x);
                if (y< -6m || y>8m)
                {
                    previous=null;
                    priorX=null;
                    continue;
                }
                var current = new MathematicalPoint(x,y);
                if (previous is not null && priorX is decimal px && x-px <= step*1.05m)
                    segments.Add(new MathematicalSegment(previous,current));
                previous=current;
                priorX=x;
            }
        }
    }
}
"""
rel="src/Edulytics.Web/Presentation/Grade12PrecalculusSection38InverseGraphs.cs"
(root/rel).write_text(code,encoding="utf8")
viewpath=root/"src/Edulytics.Web/Views/StudentPortal/Lesson.cshtml"
view=viewpath.read_text(encoding="utf8")
needle='        @if (Model.LessonCode == "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U03:L07")'
assert view.count(needle)==1
assert "Grade12PrecalculusSection38InverseGraphs.All" not in view
insert="""        @* Independent Grade 12 section 3.8 source-skill inverse reflections.
           Not reproductions of OpenStax artwork; runtime reserved for Stage D. *@
        @if (Model.LessonCode == "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U03:L08")
        {
            <section class="lesson-source-aligned-activities"
                     aria-label="First-party inverse functions and radical graphs">
                <h2>Inverse functions: graphs and mathematical constraints</h2>
                <p>Compare each function and inverse about the line y=x.
                   The independent graphs below are practice models. Explain
                   the original domain restrictions and the corresponding inverse
                   range. No source textbook images are reproduced.</p>
                @foreach (var graph in Grade12PrecalculusSection38InverseGraphs.All)
                {
                    <figure class="student-card">
                        @Html.Raw(AdvancedMathematicsVisualRenderer.RenderSvg(graph.Graph))
                        <figcaption>Independent graph practice @graph.RelatedExercise:
                            @graph.Title. @graph.DomainConstraint.</figcaption>
                    </figure>
                }
                <details class="student-card">
                    <summary>Nine original-source figure concepts: independent alternatives</summary>
                    <ol>
                        @foreach (var item in Grade12PrecalculusSection38FigureAlternatives.All)
                        {
                            <li>
                                <strong>Figure skill @item.Number: @item.Title</strong>
                                <p>@item.Model</p><p>@item.TeacherExplanation</p>
                                <p>Related first-party practice: @item.Exercise</p>
                            </li>
                        }
                    </ol>
                </details>
            </section>
        }

"""
viewpath.write_text(view.replace(needle,insert+needle,1),encoding="utf8")
a.update({"FirstPartyInverseGraphModelsAuthored":4,"FirstPartyInverseGraphSource":rel,
"StudentViewFigureGraphWiring":"AUTHORED_NOT_RUNTIME_VERIFIED",
"FirstPartyFigureSkillStudentView":"AUTHORED_NOT_RUNTIME_VERIFIED",
"BlockingReasons":["STAGE_C_ASSESSMENT_REUSE_CONTRACT_PENDING","TARGETED_FINAL_MATH_AND_SOURCE_EVIDENCE_GATE_PENDING"],
"UpdatedUtc":datetime.datetime.now(datetime.timezone.utc).isoformat()})
ap.write_text(json.dumps(a,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
print("U03_L08_4_TYPED_INVERSE_GRAPHS_STUDENT_AND_NINE_SOURCE_FIGURE_SKILLS_AUTHORED_NOT_CLOSED")
