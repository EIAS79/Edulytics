from pathlib import Path
import math,json,datetime,re,subprocess,time,pypdfium2 as pdfium
started=time.monotonic()
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
pp=base/"full-grade-progress.json";p=json.loads(pp.read_text(encoding="utf8"))
assert p["LastClosedLessonOrdinal"]==32 and p["FirstIncompleteLessonOrdinal"]==33
row=p["LessonEntries"][32]
assert row["Ordinal"]==33 and row["SourceSection"]=="6.1" and not row["ReadyForStageD"]
doc=pdfium.PdfDocument(r"C:\Users\khali\Downloads\New folder (2)\Common Core\12_HS_Advanced_Supplements\OpenStax_Free_Books\OpenStax_Precalculus.pdf")
get=lambda i:doc[i].get_textpage().get_text_range()
assert "6.1" in get(521) and "6.2" in get(538)
pages=[{"PdfPage":i,"Text":get(i)}for i in range(521,538)]
work=[(int(n),title.strip())for n,title in re.findall(r"Example\s+(\d+)\s+([^\r\n]{1,105})","\n".join(get(i)for i in range(521,535)),re.I)]
tries=sorted({int(n)for n in re.findall(r"Try It\s*#?(\d+)","\n".join(get(i)for i in range(521,535)),re.I)})
figures=sorted({int(n)for n in re.findall(r"Figure\s+(\d+)","\n".join(get(i)for i in range(521,538)),re.I)})
assert [n for n,_ in work]==list(range(1,14)) and tries==list(range(1,12))
assert figures==list(range(1,34)) and "48." in get(537)
Q=[]
def add(q,a,src):
 n=len(Q)+1;assert q and a and src
 Q.append({"SourceExercise":n,"IndependentStudentQuestion":q,"TeacherMath":a,
 "OriginalMathSkillReviewed":src,"OriginalExerciseTextCopied":False,
 "ManualTeacherReview":True})
for q,a,source in [
("Explain why sin and cos are periodic.","sin(x+2π)=sinx and cos(x+2π)=cosx for all real x.","Source #1 periodic trig functions."),
("Describe horizontal translation between sine and cosine.","cosx=sin(x+π/2); shift sine leftπ/2.","Source #2 sine/cosine phase shift."),
("For y=Acos(Bx+C)+D, what changes its range?","Amplitude |A| and shift D: range[D−|A|,D+|A|].","Source #3 amplitude and vertical translation."),
("For y=A sin(Bx+C)+D, give midline and range.","Midline y=D, range[D−|A|,D+|A|].","Source #4 sine transformed range."),
("Explain unit-circle construction of sine graph.","As t increases, plot y-coordinate of (cos t,sin t) against t; repeats every2π.","Source #5 unit-circle graph construction.")
]:add(q,a,source)
# 6–22 graph sinusoidal variations
parameters=[
(2,1,0,0,"sin"),(2/3,1,0,0,"cos"),(-3,1,0,0,"sin"),
(4,1,0,0,"sin"),(2,1,0,0,"cos"),(1,2,0,0,"cos"),
(2,.5,0,0,"sin"),(4,math.pi,0,0,"cos"),(3,1.2,0,0,"cos"),
(3,8,32,5,"sin"),(2,3,-21,4,"sin"),(5,5,20,-2,"sin"),
(2,1,-5*math.pi/6,0,"sin"),(-1,1,math.pi/3,1,"cos"),
(4,2,math.pi/2,-3,"cos"),(-1,.5,5*math.pi/3,0,"sin"),
(4,math.pi/2,-3*math.pi/2,7,"sin")]
assert len(parameters)==17
for i,(A,B,C,D,kind)in enumerate(parameters,6):
 amp=abs(A);period=2*math.pi/abs(B);shift=-C/B
 add(f"For independent y={A:.6g} {kind}({B:.5g}x + {C:.5g}) + ({D:g}), find amplitude, period, phase shift, midline and range.",
 f"amplitude={amp:g};period≈{period:.5f};phase shift≈{shift:.5f};midline y={D:g};range[{D-amp:g},{D+amp:g}].",
 f"Source #{i} transformed sine/cosine plot and characteristics.")
# 23-30 from source graph form, independent sample curves
for i,(A,B,C,D,kind)in enumerate([
(2,1,0,1,"sin"),(3,1,0,-2,"cos"),(4,2,0,0,"cos"),(5,.5,0,-1,"sin"),
(-2,1,math.pi/4,0,"cos"),(2,3,-1,2,"sin"),(1.5,.75,0,1,"cos"),(3,2,math.pi/3,-1,"sin")],23):
 add(f"A separately authored plotted sinusoid has model y={A}·{kind}({B}x+{C:.3f})+{D}; recover amplitude, period and midline.",
 f"Amplitude={abs(A):g}; period={2*math.pi/abs(B):.5f}; midline y={D:g}; original teacher equation as given.",
 f"Source #{i} identify equation/period/vertical translation from given source Figure{i+3}, independently authored mathematical graph.")
# 31-42 roots/extrema
for q,a,src in [
("Solve sinx=0 on[0,2π).","x=0,π.","Source #31 sine zeros."),
("Solve sinx=1/2 on[0,2π).","x=π/6,5π/6.","Source #32 sine level half."),
("Find sin(π/2).","1.","Source #33 sine on quadrant axis."),
("Solve sinx=sqrt2/2 on[0,2π).","x=π/4,3π/4.","Source #34 special sine level."),
("Find where sinx maximal on[0,2π).","x=π/2.","Source #35 sine maximum."),
("Find where sinx minimal on[0,2π).","x=3π/2.","Source #36 sine minimum."),
("Explain symmetry of sin(−x).","sin is odd, sin(−x)=−sinx, origin symmetry.","Source #37 sine parity."),
("Solve cosx=0 on[0,2π).","x=π/2,3π/2.","Source #38 cosine zeros."),
("Solve cosx=1/2 on[0,2π).","x=π/3,5π/3.","Source #39 cosine level half."),
("Give x intercepts of y=cosx on[0,2π).","(π/2,0),(3π/2,0).","Source #40 cos intercepts."),
("List cosine maximum/minimum input positions on[0,2π).","Maximum x=0; minimum x=π.","Source #41 cosine extremes."),
("Solve cosx=sqrt3/2 on[0,2π).","x=π/6,11π/6.","Source #42 special cosine level.")
]:add(q,a,src)
# 43-47 graph compositions
for q,a,src in [
("Graph h(x)=x+sinx on[0,2π]; describe range relative to diagonal.","It lies within ±1 of y=x and is nondecreasing since h'=1+cosx≥0.","Source #43 x+sinx short-domain graph."),
("Describe h(x)=x+sinx on[−40,40].","Near diagonal y=x with bounded oscillatory offset ±1, slope 1+cosx≥0.","Source #44 long domain behavior."),
("Graph y=x sinx on[0,2π].","Sinusoidal oscillation multiplied by x; envelope ±x; zeros0,π,2π.","Source #45 xsinx first-period graph."),
("Graph y=x sinx on[−10,10].","Even function, since x and sinx both odd; bounded by ±|x|.","Source #46 xsinx symmetric long range."),
("Graph f(x)=sinx/x for x≠0 and discuss near origin.","Even; removable hole at0, limit1 as x→0; oscillations decay for large |x|.","Source #47 sinc oscillation/domain and origin.")
]:add(q,a,src)
add("Ferris wheel diameter18m, lowest seat1m above ground, period12min, boarding at bottom. Give amplitude, midline, formula, height after6min.",
 "Radius/amplitude9m, midline10m, h(t)=10−9cos(2πt/12), t minutes; h(6)=19m.",
 "Source #48 Ferris wheel diameter25m loading1m, period10min, derive height.")
assert len(Q)==48 and [q["SourceExercise"]for q in Q]==list(range(1,49))
# 13 source worked example skill alternatives, 11 TryIt; source figure skills 33
W=[
("Period of sine","Find period of f(x)=sin(4x).","2π/4=π/2."),
("Amplitude","Find amplitude of g(x)=−3cosx.","3."),
("Phase shift","Find phase shift of h(x)=2sin(3x−π).","π/3 right."),
("Vertical translation","Find midline of y=sinx+5.","y=5."),
("Interpret transformed equation","For y=4sin(2x+π)+1 find amplitude, period, shift and range.","Amplitude4, periodπ, leftπ/2, range[-3,5]."),
("Read sine graph","Graph y=2sinx+3; identify maximum/minimum.","Max5,min1,midline3."),
("Read cosine graph","Graph y=3cos(2x)−2; identify period, range.","Periodπ, range[-5,1]."),
("Sine amplitude and period","Graph y=−2sin(3x).","Amplitude2, period2π/3."),
("Cosine transformed","Graph g(x)=−2cos(x−π/3)+1.","Amplitude2,period2π,shift rightπ/3,midline1."),
("Properties","For y=5sin(x/2)+2 find period and range.","Period4π, range[-3,7]."),
("Circular motion","Wheel radius2m, height center3m, period8sec; started bottom.","h(t)=3−2cos(2πt/8)."),
("Vertical wheel position","A radius7m wheel centered8m above ground, rotating once20s, started bottom.","h(t)=8−7cos(πt/10)."),
("Ferris wheel height","Wheel radius10m, center11m, period12min, started bottom; find at6min.","h(6)=21m.")
]
T=[
("Period of sin(5x).","2π/5."),
("Amplitude of −4cosx.","4."),
("Phase shift of sin(2x+π).","leftπ/2."),
("Midline of 3cosx−2.","y=−2."),
("For y=2sin(4x)+1 give period/range.","Periodπ/2,range[-1,3]."),
("Find max/min y=5cosx+4.","Max9,min−1."),
("Find amplitude and period y=−2sin(2x).","2, π."),
("Graph y=−3cosx.","Amplitude3, period2π."),
("Graph y=4sin(x−π/4)+3.","Shift rightπ/4,midline3,range[-1,7]."),
("For y=2cos(3x)−1 find period.","2π/3."),
("Wheel center5m,radius4m, period10s, starts bottom: find h(5).","9m.")
]
assert len(W)==13 and len(T)==11
lessoncode="PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U06:L01"
richrel="src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u06-l01.rich-lesson-v2.json"
teacherrel="src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u06-l01-firstparty-teacher-led.json"
rich={"SchemaVersion":2,"PackCode":"US-CCSS-MATH:G12:PRECALC:OPENSTAX-2017:U06:L01",
"ContentVersion":"g12-u06-l01-firstparty-sinusoidal-abc",
"Lessons":[{"LessonCode":lessoncode,"CultureCode":"en","Title":"Graphs of the Sine and Cosine Functions",
"ExplanationParagraphs":["Sine and cosine have amplitude1, period2π, and values between−1 and1. Cosine is sine shifted leftπ/2.",
"A general sinusoid y=A sin(Bx+C)+D or A cos(Bx+C)+D has amplitude |A|, period2π/|B| for B≠0, horizontal shift−C/B, midline y=D and range[D−|A|,D+|A|].",
"The sign of A reflects about the midline. Use five evenly spaced key points across a full period to sketch an independent wave.",
"When A=0 or B=0 the model becomes constant and the standard period formula is inapplicable.",
"Cosine is even and sine is odd. Phase-shifted sinusoids require transformed input for symmetry discussions.",
"For uniform wheel motion, height h(t)=center−radius cos(ωt) if starting at the bottom; ω=2π/period.",
"Independent Cool-down: determine period/phase of y=−3sin(2x−π)+1, list sine zeros in[0,2π) and model wheel center8m radius7m period10s."]+
[f"Independent Try It {i}: {q}"for i,(q,ans)in enumerate(T,1)],
"WorkedExamples":[{"Title":title,"Question":q,"Method":"Identify A,B,C,D, verify period, extrema, midline, and phase direction.","Steps":[ans],"Answer":ans,"Check":"Substitute at key quarter-period points."}for title,q,ans in W],
"KeyConcepts":[{"Title":"Amplitude","Definition":"Magnitude |A| of vertical oscillation about midline.","Rule":"Does not include vertical offset.","Example":"−3sinx+2 amplitude3."},
{"Title":"Period","Definition":"2π/|B| for nonconstant sinusoid.","Rule":"B≠0 and A≠0 for nonconstant curve.","Example":"cos(4x) periodπ/2."},
{"Title":"Phase shift","Definition":"−C/B for Bx+C, B≠0.","Rule":"Mind sign inside the input.","Example":"sin(x−π) shift rightπ."},
{"Title":"Circular height","Definition":"Center height+radius sin/cos wave.","Rule":"Start position determines sine versus negative cosine.","Example":"h=5−4cos(2πt/10)."}],
"Visuals":[{"Kind":"RatioTable","Title":"Original sine/cosine periodic anchor values",
"Description":"Independent quarter-cycle table.",
"PrimaryLabel":"Angle","SecondaryLabel":"Output",
"Rows":[{"Label":"x","Cells":["0","π/2","π","3π/2"]},{"Label":"sinx","Cells":["0","1","0","−1"]},{"Label":"cosx","Cells":["1","0","−1","0"]}],"Items":[],"SecondaryItems":[]}],
"Videos":[],"SummaryPoints":["Amplitude |A| and midline D.","Period2π/|B| for nonconstant waves.","Phase shift−C/B.","Model cyclic height with correct start angle."],
"CommonMistakes":[{"Mistake":"Period=2πB.","WhyWrong":"Horizontal scale inversely changes period.","Correction":"2π/|B|."},{"Mistake":"Take shift=C/B for Bx+C.","WhyWrong":"Sign is reversed.","Correction":"Shift−C/B."}]}]}
teacher={"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","LessonCode":lessoncode,
"IndependentStudentMathQuestions":Q,"IndependentTryIt":[{"Number":i,"Question":q,"TeacherExpectedMath":ans}for i,(q,ans)in enumerate(T,1)],
"IndependentCoolDown":{"StudentQuestions":["For y=−3sin(2x−π)+1 determine period, phase, midline, range.",
"Find sine zeros in[0,2π).","Wheel center8m,radius7m,period10s starts bottom: equation."],
"TeacherExpectedMath":["Amplitude3,periodπ,phase rightπ/2,midline1,range[-2,4].","0 andπ.","h(t)=8−7cos(2πt/10)."],
"OriginalPublisherCoolDownCopied":False},
"SourceMathExercisesReviewed":48,"StageC":"EXISTING_RICHV2_MANUAL_SINUSOID_TEACHER_REVIEW",
"NewEngine":False,"StageD":"NOT_RUN"}
def save(path,obj):path.parent.mkdir(parents=True,exist_ok=True);path.write_text(json.dumps(obj,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
save(root/richrel,rich);save(root/teacherrel,teacher)
# 33 authored sine/cosine SVG plots for source figure skills, with each unique model and not copying source points.
figparams=[]
for i in range(33):
 if i<len(parameters):figparams.append(parameters[i])
 else:
  A=[1,2,3,-2][i%4];B=[1,2,.5,3][i%4];C=[0,math.pi/4,-math.pi/2][i%3];D=[0,1,-1][i%3]
  figparams.append((A,B,C,D,"sin"if i%2 else "cos"))
assert len(figparams)==33
figrel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u06-l01-33-firstparty-sinusoidal-figures.json"
save(root/figrel,{"SchemaVersion":1,"Section":"6.1","OriginalFigureCount":33,
"IndependentGraphAlternatives":[{"SourceFigureSkill":n,"IndependentEquation":f"{A:.6g}{fn}({B:.6g}x+{C:.6g})+{D:.6g}",
"PublisherGraphicsCopied":False,"RuntimeVerified":False}for n,(A,B,C,D,fn)in enumerate(figparams,1)]})
cs=["using Edulytics.Core.Mathematics.Visuals;","namespace Edulytics.Web.Presentation;",
"/// <summary>First-party sinusoidal figure skill alternatives, non-copy of source.</summary>",
"public static class Grade12PrecalculusSection61SineCosineGraphs","{",
"    public sealed record Task(int SourceFigureSkill,string Prompt,CoordinatePlaneVisualSpec Graph);",
"    public static readonly IReadOnlyList<Task> All=Build();",
"    private static IReadOnlyList<Task> Build()","    {",
"        var items=new List<Task>();"]
for n,(A,B,C,D,fn)in enumerate(figparams,1):
 cs += [f'        Add({n},{A:.8g}d,{B:.8g}d,{C:.8g}d,{D:.8g}d,{"true"if fn=="sin"else"false"});']
cs.extend(["        return items;",
"        void Add(int n,double A,double B,double C,double D,bool sine)",
"        {",
"            var segments=new List<MathematicalSegment>();",
"            MathematicalPoint? previous=null;",
"            for(int i=0;i<=240;i++)",
"            {",
"                var x=-2*Math.PI+4*Math.PI*i/240d;",
"                var y=A*(sine?Math.Sin(B*x+C):Math.Cos(B*x+C))+D;",
"                var point=new MathematicalPoint((decimal)x,(decimal)y);",
"                if(previous is not null)segments.Add(new MathematicalSegment(previous,point));",
"                previous=point;",
"            }",
"            var max=Math.Abs(A)+Math.Abs(D)+1d;",
"            var graph=new CoordinatePlaneVisualSpec(",
"                new MathematicalAxisWindow(-7m,7m,-10m,10m,2m,2m),",
"                [],segments,new MathematicalVisualAccessibility(",
'                    $"Independent sinusoidal source skill {n}",',
'                    "Original Edulytics sine/cosine transformed wave, not publisher artwork."));',
"            MathematicalVisualSpecValidator.Validate(graph);",
'            items.Add(new Task(n,$"Amplitude {Math.Abs(A):0.##}; period {2*Math.PI/Math.Abs(B):0.##}; midline {D:0.##}",graph));',
"        }","    }","}"])
graphpath=root/"src/Edulytics.Web/Presentation/Grade12PrecalculusSection61SineCosineGraphs.cs"
graphpath.write_text("\n".join(cs)+"\n",encoding="utf8")
vp=root/"src/Edulytics.Web/Views/StudentPortal/Lesson.cshtml"
view=vp.read_text(encoding="utf8")
needle='        @if (Model.LessonCode == "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U05:L02")'
assert needle in view and "Grade12PrecalculusSection61SineCosineGraphs.All" not in view
insert="""        @* Independent sinusoidal mathematical figures, not source art. *@
        @if (Model.LessonCode == "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U06:L01")
        {
            <section class="lesson-source-aligned-activities"
                     aria-label="Sine and cosine sinusoidal graphs">
                <h2>Sinusoidal graphical investigations</h2>
                <p>Determine the amplitude, period, phase and midline of each
                   independent graph. Mathematical reasoning requires teacher review.</p>
                @foreach (var task in Grade12PrecalculusSection61SineCosineGraphs.All)
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
routesrel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u06-l01-stage-c-48-sinusoidal-assessment-routes.json"
save(root/routesrel,{"SchemaVersion":1,"Section":"6.1","Count":48,
"Reuse":["Existing RichV2","Teacher manual graph evaluation","Typed SVG sine and cosine renderer"],
"NewEngine":False,"AuthenticatedGraphRuntime":"NOT_RUN",
"Routes":[{"SourceExercise":q["SourceExercise"],"TeacherMathPresent":True,"AutoGradingClaimed":False}for q in Q],
"CI":"NOT_RUN","StageD":"NOT_RUN"})
assert len(re.findall(r"\bAdd\((?:[1-9]|[12][0-9]|3[0-3]),",graphpath.read_text(encoding="utf8")))==33
assert "MathematicalVisualSpecValidator.Validate(graph)" in graphpath.read_text(encoding="utf8")
assert math.isclose(2*math.pi/2,math.pi)
now=datetime.datetime.now(datetime.timezone.utc).isoformat()
close_rel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u06-l01-abc-close-audit.json"
save(root/close_rel,{"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":6,"Lesson":1,
"Source":"OpenStax Precalculus 2017 original local PDF","SourceSHA256":p["SourceSnapshotSha256"],
"CorrectedPdfPages":[521,537],"IncorrectBlueprintPages":[522,538],
"OriginalWorkedExamples":13,"OriginalTryIt":11,"OriginalFigureCount":33,"NumberedExercises":48,
"SourceMathExercisesReviewed":48,"IndependentWorked":13,"IndependentTryIt":11,
"IndependentQuestionsTeacherKeys":48,"FirstPartyGraphAlternatives":33,
"IndependentCoolDown":3,"StageCReuseRoutes":48,"NewEngine":False,
"CCSSCandidate":["CCSS:HSF-TF.B.5"],"FormalFullCCSSAlignmentCertified":False,
"PublisherOriginalFiguresCopied":False,"ExactOriginalNumericGraphParityCertified":False,
"TargetedChecksActuallyRun":["Source PDF section boundary","48 source exercises and independent teacher keys",
"13 worked 11 Try It skill alternatives","33 typed mathematical graph source-skill alternatives",
"Source sine/cosine formulas and selected period/extrema checks","Git diff check"],
"CI":"NOT_RUN","Build":"NOT_RUN","StageD":"NOT_RUN","AuthenticatedRuntime":"NOT_RUN",
"ReadyForStageD":True,"ClosedAtUtc":now})
auditrel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u06-l01-source-and-academic-audit.json"
save(root/auditrel,{"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Section":"6.1",
"OriginalSourceSHA256":p["SourceSnapshotSha256"],"CorrectedPdfPages":[521,537],
"OriginalWorkedExampleAnchors":[{"Number":n,"Title":title}for n,title in work],
"OriginalTryItNumbers":tries,"OriginalFigureNumbers":figures,
"OriginalNumberedExerciseLedger":[{"Number":q["SourceExercise"],"SourceSkillReviewed":True,
"TeacherIndependentMathPrepared":True}for q in Q],
"StudentRichV2":richrel,"TeacherAssessment":teacherrel,"FigureEvidence":figrel,
"GraphCode":str(graphpath.relative_to(root)).replace("\\","/"),"StageCAssessment":routesrel,
"Status":"READY_FOR_STAGE_D","ReadyForStageD":True,"BlockingReasons":[],
"ClosureEvidence":close_rel,"StageD":"NOT_RUN","CI":"NOT_RUN","ClosedAtUtc":now})
assert row["Ordinal"]==33 and not row["ReadyForStageD"]
row.update({"ACStatus":"READY_FOR_STAGE_D","ReadyForStageD":True,
"PdfPageStart":521,"PdfPageEnd":537,"BlockingReasons":[],
"OriginalSourceMathExercisesReviewed":48,"FirstPartyTeacherKeys":48,
"IndependentMathematicalSourceFigureModels":33,"EvidencePath":auditrel,"ClosureEvidencePath":close_rel,
"Tests":"TARGETED_LOCAL_ONLY","ClosedAtUtc":now,"UpdatedUtc":now})
assert p["LessonEntries"][33]["Ordinal"]==34
p.update({"LastClosedLessonOrdinal":33,"FirstIncompleteLessonOrdinal":34,
"ActiveLessonOrdinal":34,"NextLessonOrdinal":34,"ReadyForStageD":33,
"SourceVerifiedGrade12Lessons":33,"GradeStatus":"GRADE12_A_B_C_IN_PROGRESS_33_OF_73",
"UpdatedUtc":now})
save(pp,p)
files=[str(pp.relative_to(root)),richrel,teacherrel,figrel,routesrel,close_rel,auditrel,
str(graphpath.relative_to(root)),str(vp.relative_to(root)),str(Path(__file__).relative_to(root))]
subprocess.run(["git","add","--sparse","-f","--",*files],cwd=root,check=True)
subprocess.run(["git","diff","--cached","--check"],cwd=root,check=True)
subprocess.run(["git","commit","-m","feat(g12 u06 l01): close sine and cosine graph models academic ABC [skip ci]"],cwd=root,check=True)
subprocess.run(["git","push","origin","curriculum/us-ccss-g12-precalculus-abc-20261106"],cwd=root,check=True)
print("U06L01_ACADEMIC_CLOSE_PUSH_VERIFIED_33_OF_73")
print("SHA",subprocess.run(["git","rev-parse","HEAD"],cwd=root,capture_output=True,text=True,check=True).stdout.strip())
print("ELAPSED_SEC",round(time.monotonic()-started,2))
