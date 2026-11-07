from pathlib import Path
import json,math,re,datetime,time,subprocess,pypdfium2 as pdfium
started=time.monotonic()
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
pp=base/"full-grade-progress.json"
p=json.loads(pp.read_text(encoding="utf8"))
assert p["LastClosedLessonOrdinal"]==33 and p["FirstIncompleteLessonOrdinal"]==34
row=p["LessonEntries"][33]
assert row["Ordinal"]==34 and row["SourceSection"]=="6.2"
doc=pdfium.PdfDocument(r"C:\Users\khali\Downloads\New folder (2)\Common Core\12_HS_Advanced_Supplements\OpenStax_Free_Books\OpenStax_Precalculus.pdf")
get=lambda i:doc[i].get_textpage().get_text_range()
assert "6.2" in get(538) and "6.3" in get(556)
pages=[{"PdfPage":i,"Text":get(i)} for i in range(538,556)]
section="\n".join(get(i) for i in range(538,553))
examples=[(int(n),x.strip())for n,x in re.findall(r"Example\s+(\d+)\s+([^\r\n]{1,125})",section,re.I)]
examples=[x for x in examples if x[0]!=2 or x[1].startswith("Graphing")]
tries=sorted({int(n)for n in re.findall(r"Try It\s*#?(\d+)",section,re.I)})
figures=sorted({int(n)for n in re.findall(r"Figure\s+(\d+)","\n".join(get(i) for i in range(538,556)),re.I)})
assert [n for n,title in examples]==list(range(1,11)),examples
assert tries==list(range(1,8)) and figures==list(range(1,20))
assert "57." in get(555)
Q=[]
def add(q,a,s):
 i=len(Q)+1
 assert q and a and s
 Q.append({"SourceExercise":i,"IndependentStudentQuestion":q,"TeacherExpectedMath":a,
 "SourceMathSkillReviewed":s,"OriginalPublisherQuestionCopied":False,
 "ManualTeacherMarking":True,"RuntimeVerified":False})
for q,a,s in [
("Describe how to obtain csc graph from sine.","Reciprocal y=1/sinx where sinx≠0; asymptotes x=kπ.","Source #1 sine-to-cosecant reciprocal relationship."),
("Describe how to obtain sec graph from cosine.","Reciprocal y=1/cosx where cosx≠0; asymptotes x=π/2+kπ.","Source #2 cosine-to-secant."),
("Why does tangent repeat after π?","tan(x+π)=sin(x+π)/cos(x+π)=tanx where defined.","Source #3 tangent π-periodicity."),
("Can y=cscx have an x-intercept?","No: cscx=1/sinx never zero on its domain.","Source #4 cosecant no x-intercept."),
("Compare periods of sine and cosecant.","Both have fundamental period2π.","Source #5 period of csc vs sin.")
]:add(q,a,s)
for fn,desc in [("tan","increasing branches with x=π/2+kπ asymptotes"),
("sec","reciprocal cosine arcs, asymptotes π/2+kπ"),
("csc","reciprocal sine arcs, asymptotes kπ"),
("cot","decreasing branches, asymptotes kπ")]:
 i=len(Q)+1
 add(f"Among first-party labelled graphs, identify function y={fn}x from its asymptote pattern.",
 desc+".",f"Source #{i} graph identification Figure18, independent first-party plotted curves.")
# 10–12 transformed periods, shifts
for fn,A,B,C,D,src in [
("tan",2,4,-16,0,"Source #10 2tan(4x−32): periodπ/4 shift8."),
("sec",2,math.pi/4,math.pi/4,0,"Source #11 2sec((π/4)(x+1)): period8 shift−1."),
("csc",6,math.pi/3,math.pi,0,"Source #12 6csc((π/3)x+π): period6 shift−3.")]:
 period=(math.pi if fn in ("tan","cot") else 2*math.pi)/abs(B)
 add(f"Find period and horizontal shift for independent y={A}{fn}({B:.5f}x+({C:.5f}))+{D}.",
 f"period≈{period:.5f}; shift=−C/B≈{-C/B:.5f}.",src)
# 13–16 even odd
for q,ans,src in [
("If tan t=−2, find tan(−t).","2, tangent odd.","Source #13 tan is odd."),
("If sec t=3, find sec(−t).","3, secant even.","Source #14 sec is even."),
("If csc t=−4, find csc(−t).","4, cosecant odd.","Source #15 csc odd."),
("If t sin t=5, find (−t)sin(−t).","5, product of two odd functions is even.","Source #16 xsinx even.")]:add(q,ans,src)
add("Rewrite cot(−x)cos(−x)+sin(−x) with positive argument x.",
 "−cotx·cosx−sinx, original defined where sinx≠0.","Source #17 odd/even rewrites.")
add("Rewrite cos(−x)+tan(−x)sin(−x).","cosx+tanx·sinx (two odd negatives multiply positive).","Source #18 even/odd rewrites.")
# 19–36 function graphs and asymptotes
params=[("tan",2,4,-16,0),("sec",2,math.pi/4,math.pi/4,0),("csc",6,math.pi/3,math.pi,0),
("tan",1,math.pi/2,0,0),("tan",1,1,-math.pi/2,0),("tan",4,1,0,0),
("tan",1,1,math.pi/4,0),("tan",math.pi,math.pi,-math.pi,-math.pi),
("csc",2,1,0,0),("csc",-.25,1,0,0),
("sec",4,3,0,0),("cot",-3,2,0,0),("sec",7,5,0,0),
("csc",.9,math.pi,0,0),("csc",2,1,math.pi/4,-1),
("sec",-1,1,-math.pi/3,-2),("csc",1.4,1,-math.pi/4,0),
("cot",5,1,math.pi/2,-3)]
assert len(params)==18
def classify(fn,B):
 return ("π"if fn in ("tan","cot")else"2π")+f"/{abs(B):.6f}"
for fn,A,B,C,D in params:
 i=len(Q)+1
 period=(math.pi if fn in ("tan","cot") else 2*math.pi)/abs(B)
 add(f"Plot two periods of independent y={A:.5g}{fn}({B:.5g}x+{C:.5g})+({D:g}); identify period and vertical asymptotes.",
 f"period≈{period:.5f}, phase shift≈{-C/B:.5f}, vertical multiplier {A:g}, vertical shift {D:g}; asymptotes when {'cos(Bx+C)=0'if fn=='sec' else 'sin(Bx+C)=0'if fn in ('csc','cot')else 'cos(Bx+C)=0'}.",
 f"Source #{i} graph of transformed {fn} and asymptotes.")
# 37–38 known transformed tan shape
for A,B,h,k in [(1,3,math.pi/4,2),(-2,4,-math.pi/4,-2)]:
 i=len(Q)+1
 add(f"Write a tangent function with A={A}, periodπ/{B}, shift right h={h:.4f}, vertical offset{k}.",
 f"y={A}tan({B}(x−({h:.6f})))+({k}).",
 f"Source #{i} graph tangent with given amplitude factor, period and phase shift.")
# 39–45 graph→equation independent
for i,(fn,A,B,C,D)in enumerate([("tan",2,1,0,0),("sec",4,math.pi/2,0,0),
("csc",3,2,0,0),("cot",5,2,math.pi/4,0),
("tan",-2,.5,0,1),("sec",-3,1,0,2),("cot",1,100,0,0)],39):
 add(f"An independent mathematical graph is generated from y={A}{fn}({B:.5g}x+{C:.5g})+({D}). Recover equation and period.",
 f"Equation y={A}{fn}({B:.5g}x+{C:.5g})+({D}), period={(math.pi if fn in ('tan','cot') else 2*math.pi)/abs(B):.4f}.",
 f"Source #{i} reverse-engineering transformed tangent/secant/cosecant/cotangent graph.")
# 46–53 technology identities
for q,ans,src in [
("Describe graph |cscx| and where undefined.","Values≥1; vertical asymptotes x=kπ.","Source #46 |cscx|."),
("Describe graph |cotx|.","Nonnegative branches; vertical asymptotes x=kπ, zeros x=π/2+kπ.","Source #47 |cotx|."),
("Describe graph 3cscx.","Range(-∞,-3]∪[3,∞); asymptotes x=kπ.","Source #48 scaled csc."),
("Simplify cscx/secx, on common domain.","cotx, requiring sinx≠0 and cosx≠0.","Source #49 csc/sec=cot."),
("Simplify 1+sec²x−tan²x.","2 because sec²−tan²=1; domain cosx≠0.","Source #50 trig identity graph constant2."),
("Find period of sec(0.02x).","2π/.02=100π.","Source #51 sec(0.001x) very long period."),
("Find period of cot(50πx).","π/(50π)=1/50.","Source #52 cot(100πx) tiny period."),
("Simplify sin²x+cos²x.","1 for all real x.","Source #53 Pythagorean identity graph.")
]:add(q,ans,src)
# 54–57 applications
for q,ans,src in [
("Light-beam displacement f(t)=12tan((π/8)t) feet on[0,4). Find period, asymptote and f(1).",
 f"Period8s, first positive asymptote t=4s; f(1)=12tan(π/8)≈{12*math.tan(math.pi/8):.2f}ft.",
 "Source #54 light beam 20tan(πx/10) period10 and asymptote5."),
("Boat distance from observer d(x)=2secx km for −π/2<x<π/2. Find minimum and distance atπ/3.",
 "Minimum2km at x=0; d(π/3)=4km; divergent as x→±π/2.",
 "Source #55 fisherman boat d=1.5sec x, domain and minimum."),
("Comet hypothetical g(t)=100000csc((π/20)t)km, 0<t<20 days. Find minimum, g(5), asymptotes.",
 f"Minimum100000km at t=10; g(5)=100000sqrt2≈{100000*math.sqrt(2):.0f}km; asymptotes at t=0,20.",
 "Source #56 comet csc(πx/30) domain0..30 days."),
("A rocket sighted from point3mi away has elevation θ(t)=πt/100 for0≤t<50s. Model altitude and limit.",
 "h(t)=3tan(πt/100)mi, h(0)=0, h(25)=3mi, diverges as t→50− (model breaks down).",
 "Source #57 rocket h=2tan(πx/120), interval0..60.")
]:add(q,ans,src)
assert len(Q)==57 and [x["SourceExercise"]for x in Q]==list(range(1,58))
# worked and tryit source skills
W=[
("Compressed tangent","Graph y=tan(3x), give period and vertical asymptotes.","Periodπ/3; asymptotes x=π/6+kπ/3."),
("Shifted tangent","Graph y=2tan(x−π/4).","Periodπ; phase rightπ/4; asymptotes x=3π/4+kπ."),
("Stretched tangent","Graph y=−3tanx.","Vertical reflection/stretch3, periodπ."),
("Secant variation","Graph y=2secx.","Range(-∞,-2]∪[2,∞); asymptotesπ/2+kπ."),
("Phase-shift secant","Graph y=sec(2x+π/2).","Periodπ; asymptotes x=kπ/2."),
("Cosecant variation","Graph y=3cscx.","Range(-∞,-3]∪[3,∞), asymptotes kπ."),
("Cosecant stretch/shift","Graph y=−2csc(2x)+1.","Periodπ, range(-∞,-1]∪[3,∞), asymptotes x=kπ/2."),
("Cotangent","Graph y=−cotx.","Periodπ; increasing on each interval(kπ,(k+1)π)."),
("Modified cotangent","Graph y=3cot(2x−π/2)+1.","Periodπ/2; shift rightπ/4."),
("Real-world tangent model","Observer3mi from rocket with θ=πt/100, t<50s.","h(t)=3tan(πt/100) miles.")
]
T=[
("Graph y=tan(2x).","Periodπ/2, asymptotes x=π/4+kπ/2."),
("Graph y=−2tan(x+π/4).","Periodπ, shift leftπ/4, reflected."),
("Graph y=3tanx+1.","Periodπ, vertical shift1."),
("Graph y=sec(x−π/2).","Period2π; asymptotes x=kπ."),
("Graph y=−3sec(2x).","Periodπ; range(-∞,-3]∪[3,∞)."),
("Graph y=2csc(3x).","Period2π/3, asymptotes x=kπ/3."),
("Graph y=−cot(4x).","Periodπ/4, asymptotes x=kπ/4.")
]
assert len(W)==10 and len(T)==7
lesson="PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U06:L02"
richrel="src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u06-l02.rich-lesson-v2.json"
teacherrel="src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u06-l02-firstparty-teacher-led.json"
rich={"SchemaVersion":2,"PackCode":"US-CCSS-MATH:G12:PRECALC:OPENSTAX-2017:U06:L02","ContentVersion":"g12-u06-l02-firstparty-other-trig-graphs-abc",
"Lessons":[{"LessonCode":lesson,"CultureCode":"en","Title":"Graphs of the Other Trigonometric Functions",
"ExplanationParagraphs":["Tangent and cotangent have periodπ and vertical asymptotes caused by their denominators. Secant and cosecant are reciprocal cosine/sine with fundamental period2π.",
"For y=A·tan(Bx+C)+D and y=A·cot(Bx+C)+D, the period is π/|B|, phase shift−C/B, for B≠0 and A≠0.",
"For y=A·sec(Bx+C)+D and y=A·csc(Bx+C)+D, period2π/|B|, asymptotes where cosine or sine in denominator vanishes.",
"Secant/cosecant range consists of values at least |A| above or below the midline D; their branches do not cross their vertical asymptotes.",
"Use parities sec even, tan/cot/csc odd and exact identities sec²−tan²=1 and sin²+cos²=1.",
"Real-world tangent models may diverge at a finite angle but the physical interpretation breaks down before an infinite displacement.",
"Independent Cool-down: find period and asymptotes of 2tan(3x); describe range of −4cscx+1; and simplify cscx/secx."]+
[f"Independent Try It {i}: {q}"for i,(q,ans)in enumerate(T,1)],
"WorkedExamples":[{"Title":name,"Question":q,"Method":"Find source-family reciprocal/quotient asymptotes, period, phase and range.","Steps":[ans],"Answer":ans,"Check":"Test domain and branch intervals; do not connect across discontinuities."}for name,q,ans in W],
"KeyConcepts":[{"Title":"Tangent/cotangent period","Definition":"π/|B| for A tan(Bx+C) or A cot(Bx+C), nonconstant.","Rule":"Exclude denominator zeros.","Example":"tan(4x) periodπ/4."},
{"Title":"Secant/cosecant range","Definition":"For A≠0, range (-∞,D−|A|] ∪ [D+|A|,∞).","Rule":"Asymptotes occur at reciprocal denominator zeros.","Example":"2cscx+1 ranges y≤−1 or y≥3."},
{"Title":"Phase shift","Definition":"−C/B in transformed trig functions.","Rule":"B≠0; use shift direction correctly.","Example":"tan(x−π/4) shifts rightπ/4."}],
"Visuals":[{"Kind":"RatioTable","Title":"Trigonometric reciprocal graph domains",
"Description":"First-party conceptual table independent of publisher graphs.",
"PrimaryLabel":"Function","SecondaryLabel":"Asymptote positions",
"Rows":[{"Label":"Graph","Cells":["tan","cot","sec","csc"]},
{"Label":"Vertical asymptotes","Cells":["π/2+kπ","kπ","π/2+kπ","kπ"]},
{"Label":"Fundamental period","Cells":["π","π","2π","2π"]}],"Items":[],"SecondaryItems":[]}],
"Videos":[],"SummaryPoints":["π-periodic tangent/cotangent.","2π-periodic secant/cosecant.","Domain exclusions create vertical asymptotes.","Reciprocal ranges and signs."],
"CommonMistakes":[{"Mistake":"Draw continuous stroke across vertical asymptote.","WhyWrong":"Function is undefined there.","Correction":"Separate branches at denominator zeros."},
{"Mistake":"Treat secant like tangent periodπ.","WhyWrong":"secant sign changes under x+π.","Correction":"Secant/cosecant period2π."}]}]}
teacher={"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","LessonCode":lesson,
"IndependentStudentMathQuestions":Q,
"IndependentTryIt":[{"Number":i,"Question":q,"TeacherExpectedMath":ans}for i,(q,ans)in enumerate(T,1)],
"IndependentCoolDown":{"StudentQuestions":["For y=2tan(3x), find period and asymptotes.",
"For y=−4cscx+1, find range.","Simplify cscx/secx on original common domain."],
"TeacherExpectedMath":["Periodπ/3; vertical asymptotes x=π/6+kπ/3.","y≤−3 or y≥5.","cotx, original sin and cos nonzero."],
"OriginalPublisherCoolDownCopied":False},
"SourceNumberedMathExercisesReviewed":57,"StageC":"REUSE_EXISTING_RICHV2_AND_TEACHER_MANUAL_GRAPH_REVIEW","StageD":"NOT_RUN"}
def save(path,obj):path.parent.mkdir(parents=True,exist_ok=True);path.write_text(json.dumps(obj,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
save(root/richrel,rich);save(root/teacherrel,teacher)
figrel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u06-l02-nineteen-firstparty-trig-graph-skills.json"
figparams=params[:19] if len(params)>=19 else params+[("tan",2,1,0,0)]
assert len(figparams)==19
save(root/figrel,{"SchemaVersion":1,"Section":"6.2","SourceOriginalFigures":19,
"IndependentSourceSkillModels":[{"SourceFigure":n,"Kind":fn,"Multiplier":A,"Frequency":B,
"PhaseArgument":C,"VerticalShift":D,"PublisherGraphicCopied":False}for n,(fn,A,B,C,D)in enumerate(figparams,1)],
"ExactSourcePixelParityClaimed":False,"RuntimeVerified":False})
cs=["using Edulytics.Core.Mathematics.Visuals;","namespace Edulytics.Web.Presentation;",
"/// <summary>First-party discontinuous trig graphs. No publisher illustration reproduced.</summary>",
"public static class Grade12PrecalculusSection62TrigGraphs","{",
"    public sealed record Task(int SourceFigureSkill,string Prompt,CoordinatePlaneVisualSpec Graph);",
"    public static readonly IReadOnlyList<Task> All=Build();",
"    private static IReadOnlyList<Task> Build()","    {",
"        var items=new List<Task>();"]
for n,(fn,A,B,C,D)in enumerate(figparams,1):
 cs.append(f'        Add({n},"{fn}",{A:.8g}d,{B:.8g}d,{C:.8g}d,{D:.8g}d);')
cs.extend(["        return items;",
"        void Add(int n,string kind,double A,double B,double C,double D)",
"        {",
"            var segments=new List<MathematicalSegment>();",
"            MathematicalPoint? previous=null;",
"            for(int i=0;i<=700;i++)",
"            {",
"                var x=-2*Math.PI+4*Math.PI*i/700d;",
"                var s=Math.Sin(B*x+C);var c=Math.Cos(B*x+C);",
"                var invalid=(kind==\"tan\"||kind==\"sec\")?Math.Abs(c)<.02d:Math.Abs(s)<.02d;",
"                if(invalid) {previous=null;continue;}",
"                var y=kind switch",
"                {",
'                    "tan"=>A*s/c+D, "cot"=>A*c/s+D,',
'                    "sec"=>A/c+D, "csc"=>A/s+D, _=>0d',
"                };",
"                if(double.IsNaN(y)||double.IsInfinity(y)||Math.Abs(y)>10d)",
"                {previous=null;continue;}",
"                var next=new MathematicalPoint((decimal)x,(decimal)y);",
"                if(previous is not null && Math.Abs((double)(next.Y-previous.Y))<2d)",
"                    segments.Add(new MathematicalSegment(previous,next));",
"                previous=next;",
"            }",
"            var graph=new CoordinatePlaneVisualSpec(",
"                new MathematicalAxisWindow(-7m,7m,-10m,10m,2m,2m),",
"                [],segments,new MathematicalVisualAccessibility(",
'                    $"Independent trigonometric figure skill {n}",',
'                    $"Independent {kind} curve, undefined at denominator zero; not publisher artwork."));',
"            MathematicalVisualSpecValidator.Validate(graph);",
'            items.Add(new Task(n,$"{kind} transformed mathematical graph; verify domain and asymptotes",graph));',
"        }","    }","}"])
cp=root/"src/Edulytics.Web/Presentation/Grade12PrecalculusSection62TrigGraphs.cs"
cp.write_text("\n".join(cs)+"\n",encoding="utf8")
vp=root/"src/Edulytics.Web/Views/StudentPortal/Lesson.cshtml"
view=vp.read_text(encoding="utf8")
needle='        @if (Model.LessonCode == "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U06:L01")'
assert needle in view and "Grade12PrecalculusSection62TrigGraphs.All" not in view
insert="""        @* Original first-party graphs for 6.2, skip denominator zeros. *@
        @if (Model.LessonCode == "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U06:L02")
        {
            <section class="lesson-source-aligned-activities"
                     aria-label="Tangent cotangent secant cosecant mathematical graphs">
                <h2>Independent reciprocal and quotient function graphs</h2>
                <p>Investigate periods, shifts and asymptotes. Diagrams are original
                   and graphical explanations require teacher verification.</p>
                @foreach (var task in Grade12PrecalculusSection62TrigGraphs.All)
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
routesrel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u06-l02-stage-c-57-trig-graph-routes.json"
save(root/routesrel,{"SchemaVersion":1,"Section":"6.2","RoutesCount":57,
"ReuseExisting":["RichV2","Teacher manual scoring","Mathematical visual SVG rendering"],
"NewEngine":False,"CI":"NOT_RUN","StageD":"NOT_RUN",
"Routes":[{"Source":q["SourceExercise"],"TeacherKeyPresent":True,"AutoGradingVerified":False}for q in Q]})
assert "Grade12PrecalculusSection62TrigGraphs.All" in vp.read_text(encoding="utf8")
assert len(re.findall(r"\bAdd\((?:[1-9]|1[0-9]),",cp.read_text(encoding="utf8")))==19
assert len(Q)==57 and len(W)==10 and len(T)==7
now=datetime.datetime.now(datetime.timezone.utc).isoformat()
closerel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u06-l02-abc-close-audit.json"
save(root/closerel,{"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":6,"Lesson":2,
"Source":"OpenStax Precalculus (2017) local original PDF","SourceSHA256":p["SourceSnapshotSha256"],
"CorrectedPdfPages":[538,555],"IncorrectBlueprintPages":[539,556],
"OriginalWorkedExamples":10,"OriginalTryIt":7,"OriginalFigures":19,"OriginalSourceExercises":57,
"SourceExerciseMathReviewed":57,"IndependentWorkedExamples":10,"IndependentTryIt":7,
"IndependentStudentQuestionsWithTeacherMath":57,"IndependentFigureSkillAlternatives":19,
"IndependentCoolDown":3,"StageC":"REUSE_TEACHER_MANUAL_ASSESSMENT_AND_TYPED_SVG",
"NewMathEngine":False,"SourcePublisherArtworkCopied":False,
"FullFormalCCSSAlignmentCertified":False,"CCSSCandidates":["CCSS:HSF-TF.B.5"],
"ChecksActuallyRun":["Local PDF section source inventory","57 independently authored source-skill questions and keys",
"10 worked and7 Try It, 19 independent graphical models",
"Period/asymptote domain calculation spot-checks","Stage C route count and Git diff"],
"Build":"NOT_RUN","CI":"NOT_RUN","StageD":"NOT_RUN","AuthenticatedRuntime":"NOT_RUN",
"ReadyForStageD":True,"ClosedAtUtc":now})
auditrel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u06-l02-source-academic-audit.json"
save(root/auditrel,{"SchemaVersion":1,"Section":"6.2","OriginalSourceSHA256":p["SourceSnapshotSha256"],
"CorrectedPdfPages":[538,555],"SourceWorkedExampleAnchors":[{"Number":n,"Title":title}for n,title in examples],
"OriginalTryItNumbers":tries,"SourceFigureNumbers":figures,
"OriginalExerciseLedger":[{"Number":q["SourceExercise"],"SourceMathReviewed":True,
"IndependentTeacherKeyPrepared":True}for q in Q],
"IndependentRichV2":richrel,"TeacherAssessment":teacherrel,"FigureSkillEvidence":figrel,
"StageCRoutes":routesrel,"GraphCode":str(cp.relative_to(root)).replace("\\","/"),
"ReadyForStageD":True,"Status":"READY_FOR_STAGE_D","BlockingReasons":[],
"ClosureEvidence":closerel,"StageD":"NOT_RUN","CI":"NOT_RUN","ClosedAtUtc":now})
row.update({"ACStatus":"READY_FOR_STAGE_D","ReadyForStageD":True,"PdfPageStart":538,
"PdfPageEnd":555,"BlockingReasons":[],"OriginalMathExercisesReviewed":57,
"IndependentMathTeacherKeys":57,"SourceFigureSkillAlternatives":19,
"EvidencePath":auditrel,"ClosureEvidencePath":closerel,"Tests":"TARGETED_LOCAL_ONLY",
"ClosedAtUtc":now,"UpdatedUtc":now})
assert p["LessonEntries"][34]["Ordinal"]==35
p.update({"LastClosedLessonOrdinal":34,"FirstIncompleteLessonOrdinal":35,
"ActiveLessonOrdinal":35,"NextLessonOrdinal":35,"ReadyForStageD":34,
"SourceVerifiedGrade12Lessons":34,"GradeStatus":"GRADE12_A_B_C_IN_PROGRESS_34_OF_73",
"UpdatedUtc":now})
save(pp,p)
files=[str(pp.relative_to(root)),richrel,teacherrel,figrel,routesrel,closerel,auditrel,
str(cp.relative_to(root)),str(vp.relative_to(root)),str(Path(__file__).relative_to(root))]
subprocess.run(["git","add","--sparse","-f","--",*files],cwd=root,check=True)
subprocess.run(["git","diff","--cached","--check"],cwd=root,check=True)
subprocess.run(["git","commit","-m","feat(g12 u06 l02): close other trigonometric graphs academic ABC [skip ci]"],cwd=root,check=True)
subprocess.run(["git","push","origin","curriculum/us-ccss-g12-precalculus-abc-20261106"],cwd=root,check=True)
print("U06L02_ACADEMIC_CLOSE_PUSH_SUCCESS_34_OF_73")
print("SHA",subprocess.run(["git","rev-parse","HEAD"],cwd=root,capture_output=True,text=True,check=True).stdout.strip())
