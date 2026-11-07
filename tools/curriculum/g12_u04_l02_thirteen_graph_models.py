from pathlib import Path
import json,datetime
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
ap=base/"u04-l02-source-inventory-corrected-boundary.json"
a=json.loads(ap.read_text(encoding="utf8"))
assert a["OriginalExerciseMathVerified"]==54
data=[
(1,"Parent exponential growth and y=0 end level","Independent graph g(x)=3^x, y-intercept1, all-real domain, positive range and horizontal y=0.",-3,3,0,30,["(decimal)Math.Pow(3d,(double)x)"],1),
(2,"Parent exponential decay","Independent graph g(x)=(1/3)^x, horizontal y=0, y-intercept1.",-3,3,0,30,["(decimal)Math.Pow(1d/3d,(double)x)"],26),
(3,"Compare exponential growth and decay families","Independent graphs y=3^x and y=3^(−x), both through (0,1), related by reflection about y-axis.",-3,3,0,30,["(decimal)Math.Pow(3d,(double)x)","(decimal)Math.Pow(3d,(double)(-x))"],8),
(4,"Sketch decay from anchor points","Independent g(x)=(1/4)^x, points (−1,4),(0,1),(1,1/4).",-3,3,0,50,["(decimal)Math.Pow(.25d,(double)x)"],24),
(5,"Vertical shift comparison of exponential curves","Independent parent 2^x and translated 2^x+2, 2^x−2; asymptotes y=0,2,−2.",-3,4,-5,35,["(decimal)Math.Pow(2d,(double)x)","(decimal)Math.Pow(2d,(double)x)+2m","(decimal)Math.Pow(2d,(double)x)-2m"],28),
(6,"Horizontal translation of exponential graph","Independent 2^x,2^(x+2),2^(x−2) showing left/right translations.",-4,4,0,70,["(decimal)Math.Pow(2d,(double)x)","(decimal)Math.Pow(2d,(double)(x+2m))","(decimal)Math.Pow(2d,(double)(x-2m))"],34),
(7,"Combined horizontal and vertical shifts","Independent y=2^(x+1)−3, asymptote y=−3, points (−1,−2),(0,−1),(1,1).",-3,4,-5,20,["(decimal)Math.Pow(2d,(double)(x+1m))-3m"],27),
(8,"Vertical stretch and compression","Independent y=3·2^x and y=(1/3)·2^x, y-intercepts3 and1/3.",-3,4,0,60,["3m*(decimal)Math.Pow(2d,(double)x)","(1m/3m)*(decimal)Math.Pow(2d,(double)x)"],12),
(9,"Scaled decreasing exponential with positive range","Independent y=4·(1/2)^x, intercept4, decay factor1/2.",-3,4,0,40,["4m*(decimal)Math.Pow(.5d,(double)x)"],9),
(10,"Reflections about coordinate axes","Independent parent y=3^x, x-axis reflection −3^x and y-axis reflection 3^(−x).",-3,3,-30,30,["(decimal)Math.Pow(3d,(double)x)","-(decimal)Math.Pow(3d,(double)x)","(decimal)Math.Pow(3d,(double)(-x))"],23),
(11,"Graph of negative exponential with x-axis asymptote","Independent y=−(1/3)^x, negative range and y-intercept−1.",-3,3,-30,0,["-(decimal)Math.Pow(1d/3d,(double)x)"],25),
(12,"Compare six independently labelled exponential factors","Independent A=2(.6)^x,B=4(.6)^x,C=2(.85)^x,D=2(1.3)^x,E=4(1.3)^x,F=2(1.7)^x. Compare a at x=0 and b ratios.",-2,3,0,24,["2m*(decimal)Math.Pow(.6d,(double)x)","4m*(decimal)Math.Pow(.6d,(double)x)","2m*(decimal)Math.Pow(.85d,(double)x)","2m*(decimal)Math.Pow(1.3d,(double)x)","4m*(decimal)Math.Pow(1.3d,(double)x)","2m*(decimal)Math.Pow(1.7d,(double)x)"],13),
(13,"Distinguish initial multiplier from exponential ratio","Independent A=2(.5)^x,B=6(.8)^x,C=(.3)^x,D=3(1.8)^x,E=4(1.3)^x,F=.5(1.1)^x. Largest b:D, smallest b:C, largest a:B, smallest a:F.",-2,3,0,40,["2m*(decimal)Math.Pow(.5d,(double)x)","6m*(decimal)Math.Pow(.8d,(double)x)","(decimal)Math.Pow(.3d,(double)x)","3m*(decimal)Math.Pow(1.8d,(double)x)","4m*(decimal)Math.Pow(1.3d,(double)x)",".5m*(decimal)Math.Pow(1.1d,(double)x)"],19)
]
assert [n for n,*_ in data]==list(range(1,14))
figure_path=base/"u04-l02-thirteen-firstparty-graph-figure-skill-alternatives.json"
figure_path.write_text(json.dumps({"SchemaVersion":1,"Section":"4.2","SourceSha256":a["SourceSHA256"],
"OriginalFigureCount":13,"FirstPartyAlternatives":[{"SourceFigureNumber":n,"OriginalSkill":title,"IndependentMathematicalAlternative":desc,"RelatedFirstPartyExercise":exercise,"PublisherGraphicCopied":False,"ExactPublisherGraphCoordinateParityCertified":False}for n,title,desc,*_,exercise in data],
"StudentGraphsRuntime":"NOT_RUN"},ensure_ascii=False,indent=2)+"\n",encoding="utf8")
head="""using Edulytics.Core.Mathematics.Visuals;

namespace Edulytics.Web.Presentation;

/// <summary>Independent source-skill alternatives for all 13 Section 4.2 figures.
/// SVG graph rendering reuses the existing mathematics renderer. This is not
/// a textbook illustration reproduction; tests reserved for Stage D.</summary>
public static class Grade12PrecalculusSection42GraphModels
{
    public sealed record GraphTask(int SourceFigureSkill, string Prompt,
        CoordinatePlaneVisualSpec Graph);
    public static readonly IReadOnlyList<GraphTask> All=Build();
    private static IReadOnlyList<GraphTask> Build()
    {
        var items=new List<GraphTask>();
"""
def esc(v):return v.replace("\\","\\\\").replace('"','\\"')
calls=[]
for n,title,desc,xmin,xmax,ymin,ymax,funcs,exercise in data:
 calls.append(f'        Add({n},{xmin}m,{xmax}m,{ymin}m,{ymax}m,')
 calls.append(f'            "{esc(desc)}",')
 calls.append('            ['+', '.join('x => '+z for z in funcs)+']);')
tail="""
        return items;
        void Add(int number, decimal xmin, decimal xmax, decimal ymin,
            decimal ymax, string description, Func<decimal,decimal>[] functions)
        {
            var segments=new List<MathematicalSegment>();
            foreach (var func in functions)
            {
                MathematicalPoint? previous=null;
                for (var i=0; i<=120; i++)
                {
                    var x=xmin+(xmax-xmin)*i/120m;
                    var y=func(x);
                    if (y<ymin || y>ymax)
                    {
                        previous=null;
                        continue;
                    }
                    var point=new MathematicalPoint(x,y);
                    if (previous is not null)
                        segments.Add(new MathematicalSegment(previous,point));
                    previous=point;
                }
            }
            var graph=new CoordinatePlaneVisualSpec(
                new MathematicalAxisWindow(xmin,xmax,ymin,ymax,1m,5m),
                [],segments,new MathematicalVisualAccessibility(
                    $"Original Edulytics exponential graph skill {number}",
                    description+" Independently authored; no original source pixels copied."));
            MathematicalVisualSpecValidator.Validate(graph);
            items.Add(new GraphTask(number,description,graph));
        }
    }
}
"""
cp=root/"src/Edulytics.Web/Presentation/Grade12PrecalculusSection42GraphModels.cs"
cp.write_text(head+"\n".join(calls)+"\n"+tail,encoding="utf8")
vp=root/"src/Edulytics.Web/Views/StudentPortal/Lesson.cshtml"
v=vp.read_text(encoding="utf8")
needle='        @if (Model.LessonCode == "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U04:L01")'
assert v.count(needle)==1 and "Grade12PrecalculusSection42GraphModels.All" not in v
insert="""        @* Grade 12 Section 4.2: original first-party typed math graphs.
           The publisher figure skills are covered, not replicated as pixels. *@
        @if (Model.LessonCode == "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U04:L02")
        {
            <section class="lesson-source-aligned-activities"
                     aria-label="Exponential function transformations and graph reasoning">
                <h2>Exponential graph reasoning: thirteen figure-skill alternatives</h2>
                <p>Investigate each independently authored graph, its intercepts,
                   asymptote, growth or decay ratio, translations and reflections.
                   For multi-curve comparisons, use the explicitly named functions
                   and explain their distinct initial values and ratios.
                   Drawing and explanations require teacher review.</p>
                @foreach (var task in Grade12PrecalculusSection42GraphModels.All)
                {
                    <figure class="student-card">
                        @Html.Raw(AdvancedMathematicsVisualRenderer.RenderSvg(task.Graph))
                        <figcaption>Figure skill @task.SourceFigureSkill:
                            @task.Prompt</figcaption>
                    </figure>
                }
            </section>
        }

"""
vp.write_text(v.replace(needle,insert+needle,1),encoding="utf8")
a["OriginalFigureSkillAlternatives"]=13
a["FigureSkillAlternativeEvidence"]=figure_path.relative_to(root).as_posix()
a["IndependentTypedGraphCodePath"]=cp.relative_to(root).as_posix()
a["GraphRuntimeVerified"]=False
a["BlockingReasons"]=["STAGE_C_54_TEACHER_REVIEW_ROUTES_PENDING","FINAL_TARGETED_CLOSURE_GATE_AND_GITHUB_PUSH_PENDING"]
a["UpdatedUtc"]=datetime.datetime.now(datetime.timezone.utc).isoformat()
ap.write_text(json.dumps(a,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
print("U04L02_13_FIRSTPARTY_FIGURE_SKILL_SVG_GRAPH_MODELS_AUTHORED")
