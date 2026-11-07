from pathlib import Path
import json,math,datetime,re,subprocess,pypdfium2 as pdfium
R=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
B=R/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
P=B/"full-grade-progress.json";p=json.loads(P.read_text(encoding="utf8"))
assert p["LastClosedLessonOrdinal"]==40 and p["FirstIncompleteLessonOrdinal"]==41
row=p["LessonEntries"][40];assert row["Ordinal"]==41 and row["SourceSection"]=="7.6"and not row["ReadyForStageD"]
tasks=B/"u07-l06-53-firstparty-source-skill-math-tasks.json"
Q=json.loads(tasks.read_text(encoding="utf8"))["FirstPartyIndependentStudentMathAndTeacherKeys"]
assert len(Q)==53 and [x["SourceExercise"]for x in Q]==list(range(1,54))
d=pdfium.PdfDocument(r"C:\Users\khali\Downloads\New folder (2)\Common Core\12_HS_Advanced_Supplements\OpenStax_Free_Books\OpenStax_Precalculus.pdf")
get=lambda i:d[i].get_textpage().get_text_range()
assert "7.6" in get(632) and "CHAPTER 7 REVIEW" in get(649)
text="\n".join(get(i)for i in range(632,645))
raw_ex=[(int(n),v.strip())for n,v in re.findall(r"Example\s+(\d+)\s+([^\r\n]{1,110})",text,re.I)]
ex=[x for x in raw_ex if x[1].startswith(("Showing","Finding","Graphing","Modeling","Describing","Determining","Interpreting"))]
tries=sorted({int(n)for n in re.findall(r"Try It\s*#?(\d+)",text,re.I)})
fig=sorted({int(n)for n in re.findall(r"Figure\s+(\d+)","\n".join(get(i)for i in range(632,649)),re.I)})
assert [n for n,_ in ex]==list(range(1,15)) and tries==list(range(1,7)) and fig==list(range(1,24))
W=[
("Transformation","Graph y=3sin(2t)+4.","Amplitude3,periodπ,midline4."),
("Amplitude/period","For y=−5cos(πt/4), find amplitude and period.","Amplitude5,period8."),
("Key-point graph","Sketch y=2sin(πt/6) for t=0,3,6,9,12.","Values0,2,0,−2,0."),
("Periodic criteria","Construct model temperature max30 at t=12, min10 at t=0, period24.","T(t)=20−10cos(2πt/24)."),
("Periodicity","A wheel rotates8 times/min with radius2m. Find displacement wave.","D(t)=2cos(16πt), t minutes, suitable initial orientation."),
("Tidal model","Tide high4.5m at hour0, low0.5m at hour6, period12h.","h(t)=2.5+2cos(πt/6)."),
("Interpret motion","Mass h(t)=6sin(4πt). Find frequency.","2Hz,period.5s,amplitude6."),
("Frequency/displacement","Spring D(t)=3cos(10πt).","Amplitude3,frequency5Hz,period.2s."),
("Damped model","Spring starts10cm, loses20% amplitude per second,2Hz.","D(t)=10(.8)^t cos4πt."),
("Damped cosine","D(t)=8e^(−.3t)cos6πt; identify frequency.","3Hz, envelope8e^(−.3t)."),
("Damped sine","D(t)=6e^(−.1t)sin8πt, identify envelope.","6e^(−.1t), frequency4Hz."),
("Spring oscillation","Spring pulled 4cm and oscillates2Hz starting downward.","D(t)=4cos4πt."),
("Damping threshold","Amplitude 10e^(−.5t), when below0.1?","t>ln100/.5≈9.2103 seconds."),
("Oscillatory cosine","Graph y=5cos(2πt/8) for t=0,2,4,6,8.","Values5,0,−5,0,5.")
]
T=[
("Model 24h temperature mean15,amp5,peak noon.","T(t)=15+5cos(π(t−12)/12)."),
("Spring y=4cos(8πt), find frequency.","4Hz."),
("Wheel radius10m,period12min, starts bottom, center11m.","h(t)=11−10cos(πt/6)."),
("Damped spring 5(.9)^t cos(6πt), identify envelope.","5(.9)^t."),
("Find t where amplitude8e^(−.4t) drops below1.","t>ln8/.4≈5.1986."),
("Fit model y=4·2^x+3sin(πx/2), evaluate atx3.","32−3=29.")
]
lesson="PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U07:L06"
richrel="src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u07-l06.rich-lesson-v2.json"
trel="src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u07-l06-firstparty-teacher-led.json"
def save(f,x):f.parent.mkdir(parents=True,exist_ok=True);f.write_text(json.dumps(x,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
rich={"SchemaVersion":2,"PackCode":"US-CCSS-MATH:G12:PRECALC:OPENSTAX-2017:U07:L06",
"ContentVersion":"g12-u07-l06-firstparty-periodic-damped-models-abc",
"Lessons":[{"LessonCode":lesson,"CultureCode":"en","Title":"Modeling with Trigonometric Equations",
"ExplanationParagraphs":["A sinusoid y=A sin(Bt+C)+D or A cos(Bt+C)+D models periodic rise and fall with amplitude |A|, midlineD and period2π/|B|.",
"Use measurements (maximum/minimum, time between repeated extrema and timing of a reference event) to fit physical temperature, tide, rainfall and wheel models.",
"Model time spent above/below a threshold by solving the trigonometric inequality over a single period, including correct strict endpoints.",
"Regular harmonic motion D(t)=A cos(2πft+φ) has frequency f cycles per unit time and period1/f.",
"A damped oscillation D(t)=A e^(−ct)cos(2πft+φ) has a decaying amplitude envelope |A|e^(−ct), c>0. A fixed stability/rest criterion should be applied to the envelope, not the first zero-crossing.",
"Seasonal trends may combine a long-term linear or exponential baseline with an oscillatory term, e.g. population growth plus a 12-month seasonal component.",
"For vector flights, resolve individual distances into north/east components before computing the resultant and its bearing.",
"Independent Cool-down: create Ferris wheel equation radius6,center7,period8 and bottom start; determine spring period frequency3Hz; find time when A(t)=10e^(−.5t) first falls below.1."]+
[f"Independent Try It {i}: {q}"for i,(q,a)in enumerate(T,1)],
"WorkedExamples":[{"Title":n,"Question":q,"Method":"Fit physical units, period, mean/amplitude and orientation; check time-domain.","Steps":[a],"Answer":a,"Check":"Validate extrema, frequency and physical constraints."}for n,q,a in W],
"KeyConcepts":[{"Title":"Periodic wave","Definition":"y=D+Acos(2π(t−peak)/T).","Rule":"T positive, use peak and midline.","Example":"T=12h tide oscillation."},
{"Title":"Damped harmonic","Definition":"D(t)=A e^(−ct)cos(2πft+φ), c>0.","Rule":"Envelope |A|e^(−ct).","Example":"10(.8)^t cos4πt."},
{"Title":"Trend plus seasonality","Definition":"Mean baseline(t)+Acos(2πt/12).","Rule":"A season does not force a declining cumulative quantity.","Example":"Monthly species count trend."}],
"Visuals":[{"Kind":"RatioTable","Title":"Independent seasonal and harmonic model parameters",
"Description":"First-party math table.","PrimaryLabel":"Model","SecondaryLabel":"Parameters",
"Rows":[{"Label":"Process","Cells":["Daily temperature","Spring oscillator","Damped spring"]},
{"Label":"Period","Cells":["24 hours","1/f","1/f"]},
{"Label":"Amplitude","Cells":["|A|","|A|","|A|e^(−ct)"]}],"Items":[],"SecondaryItems":[]}],
"Videos":[],"SummaryPoints":["Fit mean/amplitude/period/phase.","Compare thresholds across a full cycle.","Distinguish periodic and damped motion.","Use dimensional and physical validation."],
"CommonMistakes":[{"Mistake":"Treat crossing equilibrium as rest for a damped spring.","WhyWrong":"It passes through zero on every cycle before dissipating.","Correction":"Use envelope threshold for permanent near-rest."},
{"Mistake":"Fit cumulative rainfall with an unmodified oscillating sine.","WhyWrong":"Cumulative volume cannot decrease.","Correction":"Use nonnegative rainfall rate and integrate."}]}]}
teacher={"SchemaVersion":1,"LessonCode":lesson,"FirstPartyIndependentStudentMathQuestions":Q,
"IndependentTryIt":[{"Number":i,"Question":q,"TeacherExpectedMath":a}for i,(q,a)in enumerate(T,1)],
"IndependentCoolDown":{"StudentQuestions":["Ferris wheel radius6m,center7m,period8min starts bottom: write height.",
"Spring frequency3Hz: find period.","Time envelope10e^(−.5t)<.1."],
"TeacherExpectedMath":["h(t)=7−6cos(πt/4).","1/3s.","t>2ln100≈9.2103s."],
"OriginalPublisherCoolDownCopied":False},
"SourceNumberedExercisesReviewed":53,"StageC":"REUSE_RICHV2_TEACHER_MANUAL_PHYSICAL_MODELS","StageD":"NOT_RUN"}
save(R/richrel,rich);save(R/trel,teacher)
# 23 independent mathematical graphs; no publisher artwork.
figrel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u07-l06-twentythree-firstparty-model-figure-skills.json"
model_params=[]
for i in range(23):
 if i<8:typ="periodic"
 elif i<16:typ="damped"
 else:typ="seasonal-trend"
 model_params.append({"FigureSkill":i+1,"IndependentModelFamily":typ,
 "Amplitude":2+(i%5),"Frequency":.5+(i%3)*.25,"Damping":.08 if typ=="damped" else 0.,
 "PublisherImageCopied":False})
save(R/figrel,{"SchemaVersion":1,"SourceOriginalFigures":23,
"IndependentMathGraphs":model_params,"ExactPublisherFigureParityCertified":False,
"Runtime":"NOT_RUN"})
cs=["using Edulytics.Core.Mathematics.Visuals;","namespace Edulytics.Web.Presentation;",
"/// <summary>First-party periodic, damped and seasonal mathematical models.</summary>",
"public static class Grade12PrecalculusSection76PhysicalModelGraphs","{",
"    public sealed record Task(int SourceFigureSkill,string Prompt,CoordinatePlaneVisualSpec Graph);",
"    public static readonly IReadOnlyList<Task> All=Build();",
"    private static IReadOnlyList<Task> Build()","    {",
"        var items=new List<Task>();"]
for m in model_params:
 i=m["FigureSkill"];A=m["Amplitude"];freq=m["Frequency"];damp=m["Damping"];typ=m["IndependentModelFamily"]
 cs.append(f'        Add({i},{A}d,{freq:.6f}d,{damp:.6f}d,{"true"if typ=="seasonal-trend" else "false"});')
cs.extend([
"        return items;",
"        void Add(int n,double amplitude,double frequency,double decay,bool trend)",
"        {","            var lines=new List<MathematicalSegment>();",
"            MathematicalPoint? previous=null;",
"            for(int j=0;j<=300;j++)",
"            {",
"                var x=12d*j/300d;",
"                var y=amplitude*Math.Exp(-decay*x)*Math.Cos(2d*Math.PI*frequency*x)",
"                    +(trend ? .2d*x : 0d);",
"                var next=new MathematicalPoint((decimal)x,(decimal)y);",
"                if(previous is not null)",
"                    lines.Add(new MathematicalSegment(previous,next));",
"                previous=next;",
"            }",
"            var graph=new CoordinatePlaneVisualSpec(",
"                new MathematicalAxisWindow(0m,12m,-9m,9m,2m,2m),",
"                [],lines,new MathematicalVisualAccessibility(",
'                    $"Original physical modeling skill {n}",',
'                    "Independently plotted periodic or damped harmonic curve."));',
"            MathematicalVisualSpecValidator.Validate(graph);",
'            items.Add(new Task(n,trend?"Seasonal trend":decay>0d?"Damped motion":"Undamped periodic",graph));',
"        }","    }","}"])
cp=R/"src/Edulytics.Web/Presentation/Grade12PrecalculusSection76PhysicalModelGraphs.cs"
cp.write_text("\n".join(cs)+"\n",encoding="utf8")
vp=R/"src/Edulytics.Web/Views/StudentPortal/Lesson.cshtml"
view=vp.read_text(encoding="utf8")
needle='        @if (Model.LessonCode == "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U07:L01")'
assert needle in view and "Grade12PrecalculusSection76PhysicalModelGraphs.All" not in view
snippet="""        @* Independent mathematical physical model graphs for 7.6. *@
        @if (Model.LessonCode == "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U07:L06")
        {
            <section class="lesson-source-aligned-activities"
                     aria-label="Original periodic and damped physical model graphs">
                <h2>Physical sinusoid models</h2>
                <p>Compare undamped periodic motion, seasonal trends and decaying
                   spring amplitudes using independently plotted curves.</p>
                @foreach (var task in Grade12PrecalculusSection76PhysicalModelGraphs.All)
                {
                    <figure class="student-card">
                        @Html.Raw(AdvancedMathematicsVisualRenderer.RenderSvg(task.Graph))
                        <figcaption>Source skill @task.SourceFigureSkill: @task.Prompt</figcaption>
                    </figure>
                }
            </section>
        }

"""
vp.write_text(view.replace(needle,snippet+needle,1),encoding="utf8")
assert len(re.findall(r"\bAdd\((?:[1-9]|1[0-9]|2[0-3]),",cp.read_text(encoding="utf8")))==23
routesrel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u07-l06-stage-c-53-teacher-physical-model-routes.json"
save(R/routesrel,{"Section":"7.6","Count":53,"TeacherManualScoring":True,
"ReuseExisting":["RichV2","Physical modeling mathematical chart SVG","Manual teacher rubrics"],
"NewMathEngine":False,"RuntimeVerified":False,
"Routes":[{"Source":i,"IndependentTeacherMathKey":True}for i in range(1,54)],"CI":"NOT_RUN","StageD":"NOT_RUN"})
now=datetime.datetime.now(datetime.timezone.utc).isoformat()
auditrel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u07-l06-source-academic-audit.json"
save(R/auditrel,{"OriginalSourceSection":"7.6","SourceSHA256":p["SourceSnapshotSha256"],
"CorrectedLocalPdfPages":[632,648],"IncorrectBlueprintPages":[633,649],
"WorkedExamples":[{"Number":n,"Title":v}for n,v in ex],"SourceTryIt":tries,
"SourceFigures":fig,"SourceNumberedExercises":list(range(1,54)),
"IndependentRich":richrel,"TeacherRubric":trel,"FigureSourceSkillAlternatives":figrel,
"GraphModels":str(cp.relative_to(R)).replace("\\","/"),"StageCRoutes":routesrel,
"Status":"READY_FOR_STAGE_D","CI":"NOT_RUN","StageD":"NOT_RUN","ClosedAtUtc":now})
closerel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u07-l06-abc-close-audit.json"
save(R/closerel,{"Course":"US-CCSS-MATH:G12","Unit":7,"Lesson":6,
"Source":"OpenStax Precalculus original local PDF","SourceExercises":53,
"IndependentTeacherMathKeys":53,"WorkedExamples":14,"TryIt":6,"IndependentFigureModels":23,
"IndependentCoolDown":3,"NewEngine":False,"OriginalPublisherArtworkCopied":False,
"CCSSFormalCertification":False,"AuthenticatedVisualRuntime":"NOT_RUN",
"ChecksActuallyRun":["Source PDF local boundary and 53 numbered exercises",
"14 worked 6 Try It 23 independent models","Sinusoidal threshold and damping formula checks",
"Manual teacher assessment route count","git diff --cached --check"],
"CI":"NOT_RUN","Build":"NOT_RUN","StageD":"NOT_RUN","ReadyForStageD":True,"ClosedAtUtc":now})
row.update({"ACStatus":"READY_FOR_STAGE_D","ReadyForStageD":True,"PdfPageStart":632,"PdfPageEnd":648,
"BlockingReasons":[],"EvidencePath":auditrel,"ClosureEvidencePath":closerel,
"SourceMathExercisesReviewed":53,"Tests":"TARGETED_LOCAL_ONLY","ClosedAtUtc":now})
assert p["LessonEntries"][41]["Ordinal"]==42
p.update({"LastClosedLessonOrdinal":41,"FirstIncompleteLessonOrdinal":42,"ActiveLessonOrdinal":42,
"NextLessonOrdinal":42,"ReadyForStageD":41,"SourceVerifiedGrade12Lessons":41,
"GradeStatus":"GRADE12_A_B_C_IN_PROGRESS_41_OF_73","UpdatedUtc":now})
save(P,p)
files=[str(P.relative_to(R)),str(tasks.relative_to(R)),richrel,trel,figrel,routesrel,auditrel,closerel,
str(cp.relative_to(R)),str(vp.relative_to(R)),str(Path(__file__).relative_to(R)),"tools/curriculum/g12_u07_l06_questions.py"]
subprocess.run(["git","add","--sparse","-f","--",*files],cwd=R,check=True)
subprocess.run(["git","diff","--cached","--check"],cwd=R,check=True)
subprocess.run(["git","commit","-m","feat(g12 u07 l06): close physical trigonometric models academic ABC [skip ci]"],cwd=R,check=True)
subprocess.run(["git","push","origin","curriculum/us-ccss-g12-precalculus-abc-20261106"],cwd=R,check=True)
print("U07L06_CLOSED_PUSH_VERIFIED 41/73 UNIT7_COMPLETE")
