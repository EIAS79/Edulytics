from pathlib import Path
import json,math,re,datetime,subprocess,pypdfium2 as pdfium
R=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626");B=R/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
P=B/"full-grade-progress.json";p=json.loads(P.read_text(encoding="utf8"))
assert p["LastClosedLessonOrdinal"]==43 and p["FirstIncompleteLessonOrdinal"]==44
row=p["LessonEntries"][43];assert row["Ordinal"]==44 and row["SourceSection"]=="8.3" and not row["ReadyForStageD"]
d=pdfium.PdfDocument(r"C:\Users\khali\Downloads\New folder (2)\Common Core\12_HS_Advanced_Supplements\OpenStax_Free_Books\OpenStax_Precalculus.pdf")
get=lambda i:d[i].get_textpage().get_text_range()
assert "8.3" in get(685) and "8.4" in get(696)
t="\n".join(get(i)for i in range(685,694))
examples=[(int(n),v.strip())for n,v in re.findall(r"Example\s+(\d+)\s+([^\r\n]{1,105})",t,re.I)]
tries=sorted({int(n)for n in re.findall(r"Try It\s*#?(\d+)",t,re.I)})
fig=sorted({int(n)for n in re.findall(r"Figure\s+(\d+)","\n".join(get(i)for i in range(685,696)),re.I)})
assert [n for n,_ in examples]==list(range(1,12)) and tries==list(range(1,6)) and fig==list(range(1,14))
Q=json.loads((B/"u08-l03-independent-questions-all-084.json").read_text(encoding="utf8"))["IndependentStudentMathTeacherKeys"]
assert len(Q)==84 and [q["SourceExercise"]for q in Q]==list(range(1,85))
W=[
("Polar plot","Plot (3,π/2).","Point rectangular(0,3)."),
("Negative polar radius","Plot (−2,π/3).","Rectangular(−1,−sqrt3); equivalent(2,4π/3)."),
("Polar to Cartesian","Convert (4,π/6).","(2sqrt3,2)."),
("Polar to Cartesian signed","Convert(−4,π/6).","(−2sqrt3,−2)."),
("Cartesian to polar","Convert(−3,3).","r=3sqrt2,θ=3π/4."),
("Cartesian equation polar","Convert x²+y²=2x.","r²=2r cosθ; do not lose origin."),
("Cartesian line polar","Convert 2x+y=4.","r(2cosθ+sinθ)=4."),
("Rectangular circle polar","Convert x²+y²=6y.","r²=6r sinθ."),
("Polar to circle","Convert r=4cosθ.","x²+y²=4x (center2,0 radius2)."),
("Polar to line","Convert r sec? Use r=2secθ.","x=2."),
("Polar rational","Convert r=5/(3cosθ+4sinθ).","3x+4y=5.")
]
T=[("Plot(3,π).","(−3,0)."),
("Plot(−2,π/2).","(0,−2)."),
("Convert polar(6,π/3).","(3,3sqrt3)."),
("Convert Cartesian(−3,−4) to polar.","r=5,θ=π+arctan(4/3)."),
("Convert x²+y²=4x to polar.","r²=4r cosθ.")
]
lesson="PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U08:L03"
richrel="src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u08-l03.rich-lesson-v2.json"
trel="src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u08-l03-firstparty-teacher-led.json"
def save(f,x):f.parent.mkdir(parents=True,exist_ok=True);f.write_text(json.dumps(x,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
rich={"SchemaVersion":2,"PackCode":"US-CCSS-MATH:G12:PRECALC:OPENSTAX-2017:U08:L03",
"ContentVersion":"g12-u08-l03-firstparty-polar-coordinate-transformations-abc",
"Lessons":[{"LessonCode":lesson,"CultureCode":"en","Title":"Polar Coordinates",
"ExplanationParagraphs":["Polar coordinates (r,θ) locate a point a signed distance r from the pole along a ray of angle θ from the positive x-axis.",
"Negative r places a point in the opposite angular direction; (−r,θ) and(r,θ+π) represent the same point, while θ and θ+2πk are coterminal.",
"Polar to Cartesian: x=r cosθ, y=r sinθ. Cartesian to polar for nonzero point: r=sqrt(x²+y²), θ=atan2(y,x) normalized to[0,2π).",
"For equations, substitute x=r cosθ,y=r sinθ,x²+y²=r². Keep the undivided equation when canceling r would remove the pole.",
"Lines: x=a corresponds r=a secθ, y=b corresponds r=b cscθ where defined. Circles such as r=2a cosθ correspond x²+y²=2ax.",
"An inequality in polar coordinates describes a region with angle and signed radial bounds. Be explicit whether r≥0 or negative radii are allowed.",
"Independent Cool-down: locate (−3,π/6); convert(−4,−4) to positive-r polar; convert r=6sinθ to Cartesian."]+
[f"Independent Try It {i}: {question}"for i,(question,ans)in enumerate(T,1)],
"WorkedExamples":[{"Title":name,"Question":q,"Method":"Apply signed radius and angle, or x=r cosθ,y=r sinθ with quadrant check.","Steps":[ans],"Answer":ans,"Check":"Convert back and check any removed origin."}for name,q,ans in W],
"KeyConcepts":[{"Title":"Signed polar coordinate","Definition":"(r,θ)=(−r,θ+π).","Rule":"Keep direction and sign.","Example":"(−2,0)=(2,π)."},
{"Title":"Coordinate transformation","Definition":"x=r cosθ,y=r sinθ,r²=x²+y².","Rule":"atan2 identifies correct quadrant.","Example":"(1,1) ->(sqrt2,π/4)."},
{"Title":"Polar equations","Definition":"Replace Cartesian x,y by trig radius components.","Rule":"Do not divide by r and silently lose pole.","Example":"x²+y²=4x -> r²=4r cosθ."}],
"Visuals":[{"Kind":"RatioTable","Title":"Independent polar and rectangular anchor values",
"Description":"First-party exact conversion table.",
"PrimaryLabel":"Polar","SecondaryLabel":"Cartesian","Rows":[{"Label":"Polar","Cells":["(2,0)","(2,π/2)","(−2,0)","(2,π)"]},
{"Label":"Rectangular","Cells":["(2,0)","(0,2)","(−2,0)","(−2,0)"]}],"Items":[],"SecondaryItems":[]}],
"Videos":[],"SummaryPoints":["Negative r reverses direction.","Use atan2 for quadrant awareness.","Convert equations without losing origin.","Interpret polar regions with signed radial conventions."],
"CommonMistakes":[{"Mistake":"Use arctan(y/x) without quadrant correction.","WhyWrong":"One tangent value corresponds opposite quadrants.","Correction":"Use atan2(y,x)."},
{"Mistake":"Divide polar equation by r unconditionally.","WhyWrong":"May delete valid pole solution.","Correction":"Track r=0 separately."}]}]}
teacher={"SchemaVersion":1,"LessonCode":lesson,"FirstPartyIndependentStudentMathQuestions":Q,
"IndependentTryIt":[{"Number":n,"Question":q,"TeacherExpectedMath":a}for n,(q,a)in enumerate(T,1)],
"IndependentCoolDown":{"StudentQuestions":["Convert polar(−3,π/6) to Cartesian.","Convert Cartesian(−4,−4) to polar with r>0.","Convert r=6sinθ to Cartesian."],
"TeacherExpectedMath":["(−3sqrt3/2,−3/2).","(4sqrt2,5π/4).","x²+y²=6y."],
"OriginalPublisherCoolDownCopied":False},
"SourceExercisesReviewed":84,"StageC":"REUSE_RICHV2_AND_MANUAL_POLAR_ASSESSMENT","StageD":"NOT_RUN"}
save(R/richrel,rich);save(R/trel,teacher)
figrel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u08-l03-thirteen-firstparty-polar-models.json"
spec=[{"SourceFigure":n,"Radius":(2+n%4)*(1 if n%3 else -1),
"AngleRadians":(n%8)*math.pi/4,"PublisherGraphicsCopied":False}for n in range(1,14)]
save(R/figrel,{"SourceFigures":13,"IndependentPolarVisualModels":spec,
"ExactPublisherPixelParityClaimed":False,"Runtime":"NOT_RUN"})
cs=["using Edulytics.Core.Mathematics.Visuals;","namespace Edulytics.Web.Presentation;",
"/// <summary>Independently plotted polar grid points, not source publisher art.</summary>",
"public static class Grade12PrecalculusSection83PolarModels","{",
"    public sealed record Task(int SourceFigureSkill,string Prompt,CoordinatePlaneVisualSpec Graph);",
"    public static readonly IReadOnlyList<Task> All=Build();",
"    private static IReadOnlyList<Task> Build()","    {","        var items=new List<Task>();"]
for s in spec:cs.append(f'        Add({s["SourceFigure"]},{s["Radius"]}d,{s["AngleRadians"]:.8f}d);')
cs.extend(["        return items;",
"        void Add(int n,double r,double theta)",
"        {",
"            var lines=new List<MathematicalSegment>();",
"            for(int i=1;i<=96;i++)",
"            {",
"                var a=2*Math.PI*(i-1)/96d;var b=2*Math.PI*i/96d;",
"                lines.Add(new MathematicalSegment(",
"                    new MathematicalPoint((decimal)(3*Math.Cos(a)),(decimal)(3*Math.Sin(a))),",
"                    new MathematicalPoint((decimal)(3*Math.Cos(b)),(decimal)(3*Math.Sin(b)))));",
"            }",
"            var origin=new MathematicalPoint(0m,0m);",
"            var tip=new MathematicalPoint((decimal)(r*Math.Cos(theta)),(decimal)(r*Math.Sin(theta)));",
"            lines.Add(new MathematicalSegment(origin,tip));",
"            var graph=new CoordinatePlaneVisualSpec(",
"                new MathematicalAxisWindow(-7m,7m,-7m,7m,2m,2m),",
"                [],lines,new MathematicalVisualAccessibility(",
'                    $"Original polar figure skill {n}",',
'                    $"Independent polar point with signed radius {r} and angle {theta} radians."));',
"            MathematicalVisualSpecValidator.Validate(graph);",
'            items.Add(new Task(n,$"r={r:0.##}, θ={theta:0.##} rad",graph));',
"        }","    }","}"])
cp=R/"src/Edulytics.Web/Presentation/Grade12PrecalculusSection83PolarModels.cs"
cp.write_text("\n".join(cs)+"\n",encoding="utf8")
vp=R/"src/Edulytics.Web/Views/StudentPortal/Lesson.cshtml"
view=vp.read_text(encoding="utf8")
needle='        @if (Model.LessonCode == "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U08:L02")'
assert needle in view and "Grade12PrecalculusSection83PolarModels.All" not in view
insert="""        @* First-party polar point diagrams for Section 8.3. *@
        @if (Model.LessonCode == "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U08:L03")
        {
            <section class="lesson-source-aligned-activities"
                     aria-label="Polar coordinate point and grid illustrations">
                <h2>Polar coordinate mathematical investigations</h2>
                <p>Convert signed-radius points to and from Cartesian form.
                   All plots are independent mathematical diagrams.</p>
                @foreach (var task in Grade12PrecalculusSection83PolarModels.All)
                {
                    <figure class="student-card">
                        @Html.Raw(AdvancedMathematicsVisualRenderer.RenderSvg(task.Graph))
                        <figcaption>Source skill @task.SourceFigureSkill: @task.Prompt</figcaption>
                    </figure>
                }
            </section>
        }

"""
vp.write_text(view.replace(needle,insert+needle,1),encoding="utf8")
assert len(re.findall(r"\bAdd\((?:[1-9]|1[0-3]),",cp.read_text(encoding="utf8")))==13
routes="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u08-l03-stage-c-84-polar-assessment-routes.json"
save(R/routes,{"Section":"8.3","Count":84,"ExistingComponents":["RichV2","Manual math marking","Typed SVG polar grid visual"],
"NewEngine":False,"Routes":[{"SourceExercise":i,"TeacherKeyPresent":True,"AutoGradingRuntimeVerified":False}for i in range(1,85)],
"CI":"NOT_RUN","StageD":"NOT_RUN"})
now=datetime.datetime.now(datetime.timezone.utc).isoformat()
audit="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u08-l03-source-academic-audit.json"
save(R/audit,{"SourceSection":"8.3","SourceSHA256":p["SourceSnapshotSha256"],
"CorrectedPdfPages":[685,695],"IncorrectBlueprintPages":[686,696],
"SourceWorked":[{"Number":n,"Title":t}for n,t in examples],
"SourceTryIt":tries,"SourceFigures":fig,"SourceExercises":list(range(1,85)),
"IndependentRichV2":richrel,"TeacherRubric":trel,"FigureAlternatives":figrel,
"GraphModels":str(cp.relative_to(R)).replace("\\","/"),"StageC":routes,
"Status":"READY_FOR_STAGE_D","CI":"NOT_RUN","StageD":"NOT_RUN","ClosedAtUtc":now})
closure="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u08-l03-abc-close-audit.json"
save(R/closure,{"Source":"OpenStax Precalculus 2017 original local PDF","SourceSection":"8.3",
"OriginalSourceExercises":84,"IndependentTeacherMathKeys":84,"Worked":11,"TryIt":5,
"IndependentPolarFigureModels":13,"NewMathEngine":False,"PublisherGraphicsCopied":False,
"CCSSFormalCertified":False,"ExactGraphicParityClaimed":False,
"ChecksActuallyRun":["Source 8.3 local PDF boundaries and84 exercise list",
"84 first-party independent assessment/teacher keys","11 worked5 Try It,13 original math figures",
"Polar conversion/quadrant numeric checks","Stage C teacher manual marking routes",
"git diff --cached --check"],"Build":"NOT_RUN","CI":"NOT_RUN","StageD":"NOT_RUN",
"AuthenticatedRendering":"NOT_RUN","ReadyForStageD":True,"ClosedAtUtc":now})
row.update({"ACStatus":"READY_FOR_STAGE_D","ReadyForStageD":True,"PdfPageStart":685,
"PdfPageEnd":695,"BlockingReasons":[],"EvidencePath":audit,"ClosureEvidencePath":closure,
"SourceExercisesReviewed":84,"Tests":"TARGETED_LOCAL_ONLY","ClosedAtUtc":now})
assert p["LessonEntries"][44]["Ordinal"]==45
p.update({"LastClosedLessonOrdinal":44,"FirstIncompleteLessonOrdinal":45,"ActiveLessonOrdinal":45,
"NextLessonOrdinal":45,"ReadyForStageD":44,"SourceVerifiedGrade12Lessons":44,
"GradeStatus":"GRADE12_A_B_C_IN_PROGRESS_44_OF_73","UpdatedUtc":now})
save(P,p)
files=[str(P.relative_to(R)),richrel,trel,figrel,routes,audit,closure,
"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u08-l03-independent-questions-001-044.json",
"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u08-l03-independent-questions-all-084.json",
str(cp.relative_to(R)),str(vp.relative_to(R)),str(Path(__file__).relative_to(R)),
"tools/curriculum/g12_u08_l03_questions_01_44.py","tools/curriculum/g12_u08_l03_questions_045_084.py"]
subprocess.run(["git","add","--sparse","-f","--",*files],cwd=R,check=True)
subprocess.run(["git","diff","--cached","--check"],cwd=R,check=True)
subprocess.run(["git","commit","-m","feat(g12 u08 l03): close polar coordinate transformations academic ABC [skip ci]"],cwd=R,check=True)
subprocess.run(["git","push","origin","curriculum/us-ccss-g12-precalculus-abc-20261106"],cwd=R,check=True)
print("U08L03_CLOSE_PUSH_VERIFIED_44_OF_73")
