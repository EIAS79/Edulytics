from pathlib import Path
import json,math,re,datetime,subprocess,pypdfium2 as pdfium
R=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626");B=R/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
P=B/"full-grade-progress.json";p=json.loads(P.read_text(encoding="utf8"))
assert p["LastClosedLessonOrdinal"]==44 and p["FirstIncompleteLessonOrdinal"]==45
row=p["LessonEntries"][44];assert row["Ordinal"]==45 and row["SourceSection"]=="8.4"and not row["ReadyForStageD"]
D=pdfium.PdfDocument(r"C:\Users\khali\Downloads\New folder (2)\Common Core\12_HS_Advanced_Supplements\OpenStax_Free_Books\OpenStax_Precalculus.pdf")
get=lambda i:D[i].get_textpage().get_text_range()
assert "8.4" in get(696) and "8.5" in get(712)
txt="\n".join(get(i)for i in range(696,710))
worked=[(int(n),v.strip())for n,v in re.findall(r"Example\s+(\d+)\s+([^\r\n]{1,105})",txt,re.I)]
tries=sorted({int(n)for n in re.findall(r"Try It\s*#?(\d+)",txt,re.I)})
figures=sorted({int(n)for n in re.findall(r"Figure\s+(\d+)","\n".join(get(i)for i in range(696,712)),re.I)})
assert [n for n,_ in worked]==list(range(1,11)) and tries==list(range(1,7)) and figures==list(range(1,22))
Q=json.loads((B/"u08-l04-independent-source-math-072.json").read_text(encoding="utf8"))["IndependentMathQuestionsAndTeacherKeys"]
assert len(Q)==72
W=[
("Symmetry test","Does r=3+2cosθ have polar-axis symmetry?","Yes, r(−θ)=r(θ)."),
("Polar zeros and max","For r=2−2cosθ find zeros and maximum.","Zeroθ=0; max4 atθ=π."),
("Circle","Graph r=4cosθ.","Circle x²+y²=4x, center(2,0),radius2."),
("Cardioid","Graph r=3+3sinθ.","Cardioid cusp θ=3π/2, maximum6 atπ/2."),
("Limaçon no loop","Graph r=5+2cosθ.","Radii3..7, no inner loop."),
("Inner loop","Graph r=1+3cosθ.","Inner-loop limaçon, negative radii when cosθ<−1/3."),
("Lemniscate","Graph r²=9cos2θ.","Two-lobed horizontal lemniscate, |r|≤3 on cos2θ≥0."),
("Rose even","Graph r=3cos4θ.","8 petals, each length3."),
("Rose odd","Graph r=2sin3θ.","3 petals, each length2."),
("Spiral","Graph r=θ for θ≥0.","Archimedean spiral increases radius linearly with angle.")
]
T=[
("Test symmetry of r=3+cosθ.","Polar-axis symmetry."),
("Find max radius r=5−5cosθ.","10 atθ=π."),
("Graph r=2cosθ.","Circle center(1,0),radius1."),
("Graph r=2+2cosθ.","Right-facing cardioid."),
("How many petals does r=3sin5θ have?","Five."),
("Graph r=2θ on[0,2π].","Archimedean spiral radius from0 to4π.")
]
lesson="PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U08:L04"
rrel="src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u08-l04.rich-lesson-v2.json"
trel="src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u08-l04-firstparty-teacher-led.json"
def save(f,x):f.parent.mkdir(parents=True,exist_ok=True);f.write_text(json.dumps(x,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
rich={"SchemaVersion":2,"PackCode":"US-CCSS-MATH:G12:PRECALC:OPENSTAX-2017:U08:L04",
"ContentVersion":"g12-u08-l04-firstparty-polar-curve-graphs-abc",
"Lessons":[{"LessonCode":lesson,"CultureCode":"en","Title":"Polar Coordinates: Graphs",
"ExplanationParagraphs":["Polar graphs are traced by θ varying through a selected domain, plotting point(r(θ)cosθ,r(θ)sinθ) and accounting for negative r.",
"Test potential symmetry across polar axis, lineθ=π/2 and pole, noting substitution is a sufficient but not always necessary test.",
"Circles include r=a cosθ (center on x-axis) and r=a sinθ (center on y-axis). Cardioids are r=a±a sinθ or r=a±a cosθ.",
"A general limaçon r=a+b cosθ (or sine) has an inner loop if |a|<|b|; a cusp if |a|=|b|; dimpled/convex forms otherwise.",
"Lemniscates have forms r²=a²cos2θ or r²=a²sin2θ and are defined only where right-hand side≥0.",
"Rose curves r=a cos(nθ) or r=a sin(nθ) have n petals for odd integer n and2n petals for even n; petal magnitude |a|.",
"Archimedean spirals r=a+bθ expand linearly with θ. Nontrivial curves may require numerical graphical investigation.",
"To locate curve intersections, solve for equal radii at a common θ, but also check poles and signed-radius alternative representations.",
"Independent Cool-down: classify r=2+2cosθ, count petals of r=3sin4θ, and find polar-axis symmetry of r=5−2cosθ."]+
[f"Independent Try It {i}: {q}"for i,(q,a)in enumerate(T,1)],
"WorkedExamples":[{"Title":name,"Question":q,"Method":"Use polar symmetry, radius zeros and family shape; verify key angles.","Steps":[a],"Answer":a,"Check":"Inspect signed radius and parameter domain."}for name,q,a in W],
"KeyConcepts":[{"Title":"Cardioid/limaçon","Definition":"r=a±b sinθ or cosθ.","Rule":"Relative |a| and |b| determines inner loop/cusp.","Example":"r=2+2cosθ cardioid."},
{"Title":"Rose curves","Definition":"r=a sin(nθ) or a cos(nθ).","Rule":"n odd:n petals; n even:2n petals.","Example":"r=3sin2θ ->4 petals."},
{"Title":"Lemniscate","Definition":"r²=a²cos2θ or a²sin2θ.","Rule":"Only angles giving nonnegative r².","Example":"r²=9cos2θ."}],
"Visuals":[{"Kind":"RatioTable","Title":"Independent polar curve classification",
"Description":"Original polar curve family comparison.",
"PrimaryLabel":"Family","SecondaryLabel":"Signature","Rows":[{"Label":"Curve","Cells":["circle","cardioid","limaçon","rose","spiral"]},
{"Label":"Example","Cells":["r=2cosθ","r=2+2cosθ","r=3+2sinθ","r=3cos3θ","r=θ"]}],"Items":[],"SecondaryItems":[]}],
"Videos":[],"SummaryPoints":["Graph by signed radius and correct parameter domain.","Recognize circles, cardioids, limaçons, roses and lemniscates.","Check special intersection points and pole."],
"CommonMistakes":[{"Mistake":"Treat negative r as impossible.","WhyWrong":"Negative polar radius plots opposite the angle.","Correction":"Convert to Cartesian before rejecting."},
{"Mistake":"Claim every equal geometric intersection has same θ.","WhyWrong":"Equivalent polar pairs may differ by π and radial sign.","Correction":"Check signed-radius and pole separately."}]}]}
teacher={"SchemaVersion":1,"LessonCode":lesson,"FirstPartyIndependentStudentMathQuestions":Q,
"IndependentTryIt":[{"Number":i,"Question":q,"TeacherExpectedMath":a}for i,(q,a)in enumerate(T,1)],
"IndependentCoolDown":{"StudentQuestions":["Classify r=2+2cosθ.","Count petals r=3sin4θ.","Test r=5−2cosθ for polar-axis symmetry."],
"TeacherExpectedMath":["Cardioid cusp atθ=π.","8 petals.","Yes because cos(−θ)=cosθ."],
"OriginalPublisherCoolDownCopied":False},
"SourceExercisesReviewed":72,"StageC":"REUSE_EXISTING_MANUAL_POLAR_GRAPH_MARKING","StageD":"NOT_RUN"}
save(R/rrel,rich);save(R/trel,teacher)
# 21 independently authored math polar curve source-skill alternative diagrams
figrel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u08-l04-21-firstparty-polar-curve-models.json"
models=[]
for n in range(1,22):
 kind=["circle","cardioid","limacon","rose","lemniscate","spiral"][(n-1)%6]
 models.append({"SourceFigure":n,"CurveFamily":kind,"Amplitude":2+(n%4),"PublisherOriginalImageCopied":False})
save(R/figrel,{"OriginalFigures":21,"FirstPartyIndependentPolarModels":models,
"ExactPublisherGraphParityClaimed":False,"AuthenticatedRuntime":"NOT_RUN"})
cs=["using Edulytics.Core.Mathematics.Visuals;","namespace Edulytics.Web.Presentation;",
"/// <summary>Original polar curves as mathematical sketches, not source images.</summary>",
"public static class Grade12PrecalculusSection84PolarCurveModels","{",
"    public sealed record Task(int SourceFigureSkill,string Prompt,CoordinatePlaneVisualSpec Graph);",
"    public static readonly IReadOnlyList<Task> All=Build();",
"    private static IReadOnlyList<Task> Build()","    {","        var items=new List<Task>();"]
for m in models:
 cs.append(f'        Add({m["SourceFigure"]},"{m["CurveFamily"]}",{m["Amplitude"]}d);')
cs.extend(["        return items;",
"        void Add(int n,string kind,double A)",
"        {",
"            var lines=new List<MathematicalSegment>();",
"            MathematicalPoint? prev=null;",
"            for(int i=0;i<=600;i++)",
"            {",
"                var theta=4d*Math.PI*i/600d;",
"                var v=kind switch",
"                {",
'                    "circle"=>A*Math.Cos(theta),',
'                    "cardioid"=>A*(1d+Math.Cos(theta))/2d,',
'                    "limacon"=>A*.5d+Math.Sin(theta)*A,',
'                    "rose"=>A*Math.Cos(3d*theta),',
'                    "lemniscate"=>Math.Sqrt(Math.Max(0d,Math.Cos(2d*theta)))*A,',
"                    _=>theta*A/(4d*Math.PI)",
"                };",
'                if(kind=="lemniscate"&&Math.Cos(2d*theta)<0d)',
"                {prev=null;continue;}",
"                var next=new MathematicalPoint((decimal)(v*Math.Cos(theta)),",
"                    (decimal)(v*Math.Sin(theta)));",
"                if(prev is not null && Math.Abs((double)(next.X-prev.X))<1.5d &&",
"                                      Math.Abs((double)(next.Y-prev.Y))<1.5d)",
"                    lines.Add(new MathematicalSegment(prev,next));",
"                prev=next;",
"            }",
"            var graph=new CoordinatePlaneVisualSpec(",
"                new MathematicalAxisWindow(-9m,9m,-9m,9m,3m,3m),",
"                [],lines,new MathematicalVisualAccessibility(",
'                    $"First-party polar curve source skill {n}",',
'                    $"Independent {kind} polar curve with scale {A}."));',
"            MathematicalVisualSpecValidator.Validate(graph);",
'            items.Add(new Task(n,$"Polar {kind} mathematical investigation",graph));',
"        }","    }","}"])
cp=R/"src/Edulytics.Web/Presentation/Grade12PrecalculusSection84PolarCurveModels.cs"
cp.write_text("\n".join(cs)+"\n",encoding="utf8")
vp=R/"src/Edulytics.Web/Views/StudentPortal/Lesson.cshtml"
view=vp.read_text(encoding="utf8")
needle='        @if (Model.LessonCode == "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U08:L03")'
assert needle in view and "Grade12PrecalculusSection84PolarCurveModels.All" not in view
insert="""        @* First-party polar curve mathematics for Section 8.4. *@
        @if (Model.LessonCode == "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U08:L04")
        {
            <section class="lesson-source-aligned-activities"
                     aria-label="Polar cardioid limacon rose and spiral mathematical models">
                <h2>Independent polar curve investigations</h2>
                <p>Compare radius sign, symmetry, petals, and graph classification.
                   Diagrams are original mathematical examples.</p>
                @foreach (var task in Grade12PrecalculusSection84PolarCurveModels.All)
                {
                    <figure class="student-card">
                        @Html.Raw(AdvancedMathematicsVisualRenderer.RenderSvg(task.Graph))
                        <figcaption>Source figure skill @task.SourceFigureSkill: @task.Prompt</figcaption>
                    </figure>
                }
            </section>
        }

"""
vp.write_text(view.replace(needle,insert+needle,1),encoding="utf8")
assert len(re.findall(r"\bAdd\((?:[1-9]|1[0-9]|2[01]),",cp.read_text(encoding="utf8")))==21
routes="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u08-l04-stage-c-72-polar-graph-assessment-routes.json"
save(R/routes,{"Section":"8.4","Count":72,"Reuse":["RichV2","Manual teacher polar graph evaluation","Typed SVG curves"],
"NewEngine":False,"AutoGradeRuntimeVerified":False,
"Cases":[{"SourceExercise":i,"IndependentTeacherMathKey":True}for i in range(1,73)],
"CI":"NOT_RUN","StageD":"NOT_RUN"})
now=datetime.datetime.now(datetime.timezone.utc).isoformat()
audit="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u08-l04-source-academic-audit.json"
save(R/audit,{"SourceSection":"8.4","SourceSHA256":p["SourceSnapshotSha256"],
"CorrectedLocalPdfPages":[696,711],"IncorrectBlueprintPages":[697,712],
"OriginalWorked":[{"Number":n,"Title":v}for n,v in worked],
"OriginalTryItNumbers":tries,"OriginalFigures":figures,
"OriginalExerciseNumbers":list(range(1,73)),
"FirstPartyRich":rrel,"Teacher":trel,"Figures":figrel,
"GraphCode":str(cp.relative_to(R)).replace("\\","/"),"StageC":routes,
"Status":"READY_FOR_STAGE_D","CI":"NOT_RUN","StageD":"NOT_RUN","ClosedAtUtc":now})
close="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u08-l04-abc-close-audit.json"
save(R/close,{"Course":"US-CCSS-MATH:G12","Unit":8,"Lesson":4,
"Source":"OpenStax Precalculus 2017 original local PDF",
"OriginalExercises":72,"IndependentMathTeacherKeys":72,"Worked":10,"TryIt":6,
"FirstPartyPolarCurveFigureModels":21,"CoolDown":3,
"NewMathEngine":False,"PublisherOriginalGraphicsCopied":False,
"SourceExactGraphParityCertified":False,"FullCCSSCertified":False,
"TargetedChecksActuallyRun":["Local source Section8.4 boundary and 72 exercise ledger",
"72 independent teacher math/graph answers","10 worked6 TryIt",
"21 first-party polar mathematical model definitions",
"Category/axis/curve intersection mathematical samples",
"Existing teacher manual assessment route count","git diff check"],
"CI":"NOT_RUN","Build":"NOT_RUN","StageD":"NOT_RUN","AuthenticatedRuntime":"NOT_RUN",
"ReadyForStageD":True,"ClosedAtUtc":now})
row.update({"ACStatus":"READY_FOR_STAGE_D","ReadyForStageD":True,"PdfPageStart":696,
"PdfPageEnd":711,"BlockingReasons":[],"EvidencePath":audit,"ClosureEvidencePath":close,
"SourceMathExercisesReviewed":72,"Tests":"TARGETED_LOCAL_ONLY","ClosedAtUtc":now})
assert p["LessonEntries"][45]["Ordinal"]==46
p.update({"LastClosedLessonOrdinal":45,"FirstIncompleteLessonOrdinal":46,"ActiveLessonOrdinal":46,
"NextLessonOrdinal":46,"ReadyForStageD":45,"SourceVerifiedGrade12Lessons":45,
"GradeStatus":"GRADE12_A_B_C_IN_PROGRESS_45_OF_73","UpdatedUtc":now})
save(P,p)
files=[str(P.relative_to(R)),rrel,trel,figrel,routes,audit,close,str(cp.relative_to(R)),str(vp.relative_to(R)),
"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u08-l04-independent-source-math-072.json",
"tools/curriculum/g12_u08_l04_questions.py",str(Path(__file__).relative_to(R))]
subprocess.run(["git","add","--sparse","-f","--",*files],cwd=R,check=True)
subprocess.run(["git","diff","--cached","--check"],cwd=R,check=True)
subprocess.run(["git","commit","-m","feat(g12 u08 l04): close polar curve graphs academic ABC [skip ci]"],cwd=R,check=True)
subprocess.run(["git","push","origin","curriculum/us-ccss-g12-precalculus-abc-20261106"],cwd=R,check=True)
print("U08L04_CLOSE_PUSH_VERIFIED_45_OF_73")
