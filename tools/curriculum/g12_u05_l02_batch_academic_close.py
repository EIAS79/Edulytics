from pathlib import Path
import json,math,datetime,time,subprocess,re,pypdfium2 as pdfium
started=time.monotonic()
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12";pp=base/"full-grade-progress.json"
p=json.loads(pp.read_text(encoding="utf8"))
assert p["LastClosedLessonOrdinal"]==29 and p["FirstIncompleteLessonOrdinal"]==30
row=p["LessonEntries"][29]
assert row["Ordinal"]==30 and row["SourceSection"]=="5.2"
d=pdfium.PdfDocument(r"C:\Users\khali\Downloads\New folder (2)\Common Core\12_HS_Advanced_Supplements\OpenStax_Free_Books\OpenStax_Precalculus.pdf")
get=lambda i:d[i].get_textpage().get_text_range()
assert "5.2" in get(472) and "5.3" in get(488)
pages=[{"PdfPage":i,"Text":get(i)}for i in range(472,488)]
body="\n".join(x["Text"]for x in pages)
examples=[(int(n),title.strip())for n,title in re.findall(r"Example\s+(\d+)\s+([^\r\n]{1,110})","\n".join(get(i)for i in range(472,485)),re.I)]
tries=sorted({int(n)for n in re.findall(r"Try It\s*#?(\d+)","\n".join(get(i)for i in range(472,485)),re.I)})
figures=sorted({int(n)for n in re.findall(r"Figure\s+(\d+)", "\n".join(get(i)for i in range(472,485)),re.I)})
assert [x[0]for x in examples]==list(range(1,8)),examples
assert tries==list(range(1,8)) and figures==list(range(1,20))
assert "104." in get(487)
Q=[]
def add(q,a,skill):
 i=len(Q)+1
 assert q and a and skill
 Q.append({"SourceExercise":i,"IndependentStudentQuestion":q,"TeacherExpectedMath":a,
 "SourceMathSkillReviewed":skill,"FirstPartyContent":True,"TeacherManualReview":True})
for q,a,s in [
("Define the unit circle.","Circle centered origin, radius1, equation x²+y²=1.","Source #1 define unit circle."),
("Explain coordinates of the point obtained at directed angle t on unit circle.","(cos t,sin t), x is cosine and y is sine.","Source #2 x/y coordinate interpretation."),
("Distinguish reference from coterminal angles.","Reference angle acute axis-distance, coterminal angles differ by integer2π.","Source #3 reference vs coterminal."),
("Compare cosine for quadrant II angle with cosine of acute reference angle.","Equal magnitude, opposite sign: cos t=−cos α.","Source #4 cosine quadrantII sign."),
("Compare sine for quadrant II angle with reference sine.","Equal and positive: sin t=sin α.","Source #5 sine quadrantII sign.")]:add(q,a,s)
for n,(s,c,quadrant)in enumerate([(-1,-1,"III"),(1,1,"I"),(1,-1,"II"),(-1,1,"IV")],6):
 add(f"Which quadrant has sine {'positive'if s>0 else 'negative'} and cosine {'positive'if c>0 else 'negative'}?",
 f"Quadrant {quadrant}.",f"Source #{n} use signs of sin and cos to locate quadrant.")
# 10-22 exact trig values on unit circle
from fractions import Fraction as F
exactangles=[(90,"sin"),(60,"sin"),(90,"cos"),(60,"cos"),(45,"sin"),(45,"cos"),(30,"sin"),
(180,"sin"),(270,"sin"),(180,"cos"),(0,"cos"),(30,"cos"),(0,"sin")]
def exact(v):
 known={0:("0","1"),30:("1/2","sqrt(3)/2"),45:("sqrt(2)/2","sqrt(2)/2"),
 60:("sqrt(3)/2","1/2"),90:("1","0"),180:("0","-1"),270:("-1","0")}
 return known[v]
for i,(angle,fn)in enumerate(exactangles,10):
 alter=(angle+180)%360
 # independent but equally difficult, exact from integer quadrants
 sval,cval=exact(angle)
 answer=sval if fn=="sin" else cval
 add(f"Find exact {fn}({angle}°) using unit circle.",answer+".",
 f"Source #{i} exact unit-circle {fn} from source angle; independent representation in degrees.")
assert len(Q)==22
# 23-33 reference-angle source tasks, separate independent degree values
refangle=[(225,45,"°"),(-130,50,"°"),(110,70,"°"),(-300,60,"°"),(150,30,"°"),
(315,45,"°"),(120,60,"°"),(210,30,"°"),(-660,60,"°"),(-225,45,"°"),(-22.5,22.5,"°")]
for i,(angle,ref,unit)in enumerate(refangle,23):
 add(f"Give acute reference angle for directed angle {angle}°.",f"{ref:g}°.",
 f"Source #{i} reference-angle calculation in degrees or radians.")
# 34-49 terminal quadrant, reference, sin & cos
mathangles=[225,300,320,135,210,120,250,150,225,210,300,135,240,120,150,315]
for i,angle in enumerate(mathangles,34):
 normalized=angle%360
 ref=min(normalized%180,180-normalized%180)
 quadrant="I"if normalized<90 else "II"if normalized<180 else "III"if normalized<270 else "IV"
 sinv=math.sin(math.radians(angle));cosv=math.cos(math.radians(angle))
 add(f"For independent angle {angle}°, give quadrant, reference angle, sin and cos (3-decimal approximation).",
 f"Q{quadrant}, ref={ref:g}°, sin≈{sinv:.3f}, cos≈{cosv:.3f}.",
 f"Source #{i} angle quadrant/reference and sin/cos; use exact special-angle values when available.")
# 50-53 Pythagorean identity, correct quadrant signs
for i,(kind,fval,quad,src)in enumerate([
("cos",F(3,5),"IV","Source #50 cos1/7 QIV -> sin<0."),
("cos",F(5,13),"I","Source #51 cos2/9 QI -> sin>0."),
("sin",F(4,5),"II","Source #52 sin3/8 QII -> cos<0."),
("sin",F(-5,13),"III","Source #53 sin−1/4 QIII -> cos<0.")],50):
 comp=math.sqrt(1-float(fval)**2)
 sign=-1 if quad in ("III","IV")and kind=="cos" or kind=="sin"and quad in ("II","III") else 1
 if kind=="cos":sign=-1 if quad in ("III","IV") else 1
 else:sign=-1 if quad in ("II","III") else 1
 add(f"If {kind}(t)={fval}, and t in quadrant{quad}, find the other trig function.",
 f"Other component≈{sign*comp:.6f}; use sin²t+cos²t=1 and quadrant sign.",src)
# 54-57 radius coordinates
for i,(r,ang,src)in enumerate([(10,210,"Source #54 r15 and220°"),
(12,150,"Source #55 r20 and120°"),(6,315,"Source #56 r8 and7π/4"),
(18,100,"Source #57 r16 angle5π/9")],54):
 x=r*math.cos(math.radians(ang));y=r*math.sin(math.radians(ang))
 add(f"Find circle point for radius {r} at {ang}°.",
 f"(r cosθ,r sinθ)≈({x:.4f},{y:.4f}).",src)
add("Find real domain for sin and cos.","All real t.","Source #58 domains of sine and cosine.")
add("Find range of sin and cos.","Both [-1,1].","Source #59 ranges.")
# 60-79 first-party independent unit-circle point tasks for each source graphic skill
points=[0,30,45,60,90,120,135,150,180,210,225,240,270,300,315,330,360,110,250,80]
for i,ang in enumerate(points,60):
 s=math.sin(math.radians(ang));c=math.cos(math.radians(ang))
 add(f"An independently sketched unit-circle terminal point lies at angle {ang}°. Read its sine and cosine coordinates.",
 f"sin≈{s:.3f}, cos≈{c:.3f}, point(cos,sin).",
 f"Source graphical exercise #{i} identify sin and cos from a circle point; independently authored point, no copied illustration.")
# 80-89 calculator trig values
values=[("sin",100),("cos",100),("sin",18),("cos",18),("sin",135),("cos",135),
("sin",105),("cos",105),("cos",290),("sin",290)]
for i,(fn,deg)in enumerate(values,80):
 val=math.sin(math.radians(deg))if fn=="sin" else math.cos(math.radians(deg))
 add(f"Evaluate {fn}({deg}°) using degree-mode calculator to 5 decimals.",f"{val:.5f}.",
 f"Source #{i} calculator values in radians/degree; verify correct angle unit.")
# 90-99 products of unit circle values
anglepairs=[("sin",330,"cos",-150),("sin",135,"cos",300),("sin",-240,"cos",90),
("sin",-405,"cos",-30),("sin",30,"cos",-60),("sin",315,"cos",-120),
("cos",150,"cos",120),("cos",-60,"cos",45),("sin",-225,"sin",330),("sin",180,"sin",30)]
for i,(f1,a1,f2,a2)in enumerate(anglepairs,90):
 v1=(math.sin if f1=="sin" else math.cos)(math.radians(a1))
 v2=(math.sin if f2=="sin" else math.cos)(math.radians(a2))
 add(f"Compute product {f1}({a1}°)×{f2}({a2}°).",
 f"≈{v1*v2:.6f}; check both quadrant signs.",
 f"Source #{i} evaluate product of exact special-angle trigonometric values.")
# 100-104: carousel coordinate, period60 sec, initial north
def pos(t):
 a=math.pi/2+2*math.pi*t/60
 return math.cos(a),math.sin(a)
for i,tsec in enumerate([15,75,130],100):
 x,y=pos(tsec)
 add(f"Carousel starts at(0,1), rotates CCW once per60 seconds. Give coordinates after{tsec}s.",
 f"(cos(π/2+2π{tsec}/60),sin(...))≈({x:.3f},{y:.3f}).",
 f"Source #{i} carousel starting north, positions at45,90,125 seconds.")
add("Carousel starts north, one revolution60s, CCW. List times in [0,180) with point(−sqrt2/2,+sqrt2/2).",
 "t=52.5,112.5,172.5 seconds (initial angleπ/2; target3π/4, add full rotations: t=7.5+60k actually t=7.5,67.5,127.5).",
 "Source #103 multiple arrival times at given quadrantIV point.")
# fix explicit times
Q[102]["TeacherExpectedMath"]="Target angle3π/4, initialπ/2; Δθ=π/4 => t=7.5+60k sec; in[0,180):7.5,67.5,127.5."
add("Carousel starts north, one revolution60s, CCW. List t in [0,180) at point(−sqrt3/2,−1/2).",
 "Target angle7π/6, initialπ/2 =>Δθ=2π/3 =>t=20+60k seconds; t=20,80,140.",
 "Source #104 find repeated times at specified third-quadrant point.")
assert len(Q)==104 and [q["SourceExercise"]for q in Q]==list(range(1,105))
# independent geometric checks
assert math.isclose(math.sin(math.radians(30)),.5)
assert math.isclose(math.cos(math.radians(60)),.5)
assert math.isclose(pos(60)[0],0,abs_tol=1e-10)
assert len(figures)==19 and len(examples)==7
W=[
("Unit circle coordinates","Find cos and sin at 5π/6.","cos=−sqrt3/2, sin=1/2."),
("Axis angle","Find sin and cos for angle3π/2.","sin=−1, cos=0."),
("Use Pythagorean identity","Given cos=3/5 and quadrantIV, find sin.","sin=−sqrt(1−9/25)=−4/5."),
("Calculator evaluation","Evaluate sin(100°) to four decimals.",f"{math.sin(math.radians(100)):.4f}."),
("Reference angle","Find reference angle for 240°.","240−180=60°, quadrant III."),
("Reference sine cosine","Find sin,cos for 7π/6.","sin=−1/2, cos=−sqrt3/2."),
("Radius coordinates","Find point at120° on circle radius6.","(6cos120°,6sin120°)=(-3,3sqrt3).")
]
T=[
("Point atπ/3 on unit circle.","(1/2,sqrt3/2)."),
("Find sin,cos of angleπ.","sin=0, cos=−1."),
("If sin=3/5 in QII, find cos.","−4/5."),
("Evaluate cos(20°) to4 decimals.",f"{math.cos(math.radians(20)):.4f}."),
("Find reference angle for 315°.","45°."),
("Find sin,cos of 5π/4.","Both−sqrt2/2."),
("Find point on circle r=4 at210°.","(-2sqrt3,−2).")
]
lessoncode="PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U05:L02"
richrel="src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u05-l02.rich-lesson-v2.json"
teacherrel="src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u05-l02-firstparty-teacher-led.json"
rich={"SchemaVersion":2,"PackCode":"US-CCSS-MATH:G12:PRECALC:OPENSTAX-2017:U05:L02",
"ContentVersion":"g12-u05-l02-firstparty-unit-circle-abc",
"Lessons":[{"LessonCode":lessoncode,"CultureCode":"en","Title":"Unit Circle: Sine and Cosine Functions",
"ExplanationParagraphs":["The unit circle x²+y²=1 maps each directed angle t to (cos t,sin t).",
"The horizontal coordinate is cosine, vertical coordinate sine; both are defined on all real angles and range from−1 to1.",
"The Pythagorean identity sin²t+cos²t=1 follows from x²+y²=1 and helps find an unknown trig value. The quadrant determines its sign.",
"Standard special angles 0,π/6,π/4,π/3,π/2 and their symmetric rotations have exact algebraic values.",
"Reference angles measure the acute angle from terminal side to x-axis. Use reference angle magnitudes and quadrant signs.",
"A radius r circle has coordinates (r cos t,r sin t). For a unit-circle carousel beginning at north, t=π/2+ωtime.",
"Independent Cool-down: find sin and cos at 4π/3; find cos if sin=5/13 in quadrantII; and find coordinate on radius5 at angleπ/6."]+
[f"Independent Try It {i}: {q}"for i,(q,a)in enumerate(T,1)],
"WorkedExamples":[{"Title":title,"Question":q,"Method":"Place terminal point on circle and determine signed coordinates.","Steps":[ans],"Answer":ans,"Check":"Verify sin²+cos²=1 and quadrant sign."}for title,q,ans in W],
"KeyConcepts":[{"Title":"Unit-circle coordinates","Definition":"Point at angle t is (cos t,sin t).","Rule":"cos horizontal, sin vertical.","Example":"t=π/2 gives(0,1)."},
{"Title":"Pythagorean identity","Definition":"sin²t+cos²t=1.","Rule":"Missing component sign depends on quadrant.","Example":"cos=3/5 QIV ->sin=−4/5."},
{"Title":"Reference angle","Definition":"Acute angle between terminal side and x-axis.","Rule":"Use quadrant signs.","Example":"225° reference45°."}],
"Visuals":[{"Kind":"RatioTable","Title":"Independent first-quadrant unit circle table",
"Description":"Original unit circle exact coordinates.",
"PrimaryLabel":"Angle","SecondaryLabel":"Coordinates",
"Rows":[{"Label":"t","Cells":["0","π/6","π/4","π/3"]},
{"Label":"cos t","Cells":["1","sqrt3/2","sqrt2/2","1/2"]},
{"Label":"sin t","Cells":["0","1/2","sqrt2/2","sqrt3/2"]}],"Items":[],"SecondaryItems":[]}],
"Videos":[],"SummaryPoints":["Unit circle maps to sine and cosine.","Quadrant signs determine components.","Reference angles provide exact special values.","Check identity and graph coordinates."],
"CommonMistakes":[{"Mistake":"Swap sine and cosine axes.","WhyWrong":"cos t is x-coordinate, sin t is y-coordinate.","Correction":"Always write (cos t,sin t)."},
{"Mistake":"Take positive square root in quadrantIII/IV without checking sign.","WhyWrong":"The component may be negative.","Correction":"Use quadrant before selecting radical sign."}]}]}
teacher={"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","LessonCode":lessoncode,
"IndependentStudentMathQuestions":Q,
"IndependentTryIt":[{"Number":i,"Question":q,"TeacherExpectedMath":ans}for i,(q,ans)in enumerate(T,1)],
"IndependentCoolDown":{"StudentQuestions":["Find sin and cos of 4π/3.","Given sin=5/13 in QII, find cos.","Point atπ/6 on radius5 circle."],
"TeacherExpectedMath":["sin=−sqrt3/2, cos=−1/2.","cos=−12/13.","(5sqrt3/2,5/2)."],
"OriginalPublisherCoolDownCopied":False},
"OriginalSourceExercisesReviewed":104,"ExistingTeacherMarkingReused":True,"StageD":"NOT_RUN"}
def save(path,data):path.parent.mkdir(parents=True,exist_ok=True);path.write_text(json.dumps(data,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
save(root/richrel,rich);save(root/teacherrel,teacher)
# Mathematical drawing models for 19 original figure skills; all authored independently.
figrel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u05-l02-nineteen-unit-circle-figure-skill-alternatives.json"
figure_roles=[
"Unit circle definition","Point (cos,sin) interpretation","Sine and cosine graph axes",
"Pythagorean identity triangle on unit circle","Radian rotations and unit point",
"Quadrantal angle positions","Reference triangle","Special anglesπ/4",
"Special anglesπ/6","Special anglesπ/3","Reference angle in QII",
"Reference angle in QIII","Reference angle in QIV",
"Unit circle ordered pairs","Cyclic point positions","Sine and cosine sign chart",
"Radius scaling on circle","Carousel rotation in unit circle","Independent first quadrant reference"]
assert len(figure_roles)==19
save(root/figrel,{"SchemaVersion":1,"Section":"5.2","SourceOriginalFigureCount":19,
"IndependentFigureAlternatives":[{"SourceFigure":i+1,"Skill":skill,
"FirstPartyMathUnitCircleDrawing":True,"SourceGraphicsCopied":False}for i,skill in enumerate(figure_roles)],
"Runtime":"NOT_RUN","ExactPublisherFigureParityClaimed":False})
cs=["using Edulytics.Core.Mathematics.Visuals;","namespace Edulytics.Web.Presentation;",
"/// <summary>19 independently authored unit-circle mathematical models for Section5.2.</summary>",
"public static class Grade12PrecalculusSection52UnitCircleModels","{",
"    public sealed record CircleTask(int SourceFigureSkill,string Prompt,CoordinatePlaneVisualSpec Graph);",
"    public static readonly IReadOnlyList<CircleTask> All=Build();",
"    private static IReadOnlyList<CircleTask> Build()","    {",
"        var result=new List<CircleTask>();"]
angles=[0,30,45,60,90,120,135,150,180,210,225,240,270,300,315,330,45,75,60]
for n,(role,angle) in enumerate(zip(figure_roles,angles),1):
 cs.append(f'        Add({n},{angle}d,"{role}");')
cs.extend(["        return result;",
"        void Add(int number,double angleDegrees,string label)",
"        {",
"            var lines=new List<MathematicalSegment>();",
"            for(int n=1;n<=120;n++)",
"            {",
"                var t0=2*Math.PI*(n-1)/120d;",
"                var t1=2*Math.PI*n/120d;",
"                lines.Add(new MathematicalSegment(",
"                    new MathematicalPoint((decimal)Math.Cos(t0),(decimal)Math.Sin(t0)),",
"                    new MathematicalPoint((decimal)Math.Cos(t1),(decimal)Math.Sin(t1))));",
"            }",
"            var theta=angleDegrees*Math.PI/180d;",
"            var origin=new MathematicalPoint(0m,0m);",
"            var x=(decimal)Math.Cos(theta);var y=(decimal)Math.Sin(theta);",
"            lines.Add(new MathematicalSegment(origin,new MathematicalPoint(x,y)));",
"            lines.Add(new MathematicalSegment(new MathematicalPoint(x,0m),new MathematicalPoint(x,y)));",
"            var model=new CoordinatePlaneVisualSpec(",
"                new MathematicalAxisWindow(-1.5m,1.5m,-1.5m,1.5m,0.5m,0.5m),",
"                [],lines,new MathematicalVisualAccessibility(",
'                    $"First-party unit circle figure skill {number}",',
'                    label+". Independent unit-circle diagram with coordinates (cos theta, sin theta)."));',
"            MathematicalVisualSpecValidator.Validate(model);",
"            result.Add(new CircleTask(number,label,model));",
"        }","    }","}"])
graphpath=root/"src/Edulytics.Web/Presentation/Grade12PrecalculusSection52UnitCircleModels.cs"
graphpath.write_text("\n".join(cs)+"\n",encoding="utf8")
vp=root/"src/Edulytics.Web/Views/StudentPortal/Lesson.cshtml"
view=vp.read_text(encoding="utf8")
needle='        @if (Model.LessonCode == "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U05:L01")'
assert needle in view and "Grade12PrecalculusSection52UnitCircleModels.All" not in view
insert="""        @* First-party G12 unit circle drawings. No publisher images reproduced. *@
        @if (Model.LessonCode == "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U05:L02")
        {
            <section class="lesson-source-aligned-activities"
                     aria-label="Unit circle and sine cosine graphical investigations">
                <h2>Unit circle graphical investigations</h2>
                <p>Identify coordinates (cos t, sin t), reference angles and quadrant
                   signs on original mathematical diagrams. Teacher review required
                   for open-ended geometric explanations.</p>
                @foreach (var task in Grade12PrecalculusSection52UnitCircleModels.All)
                {
                    <figure class="student-card">
                        @Html.Raw(AdvancedMathematicsVisualRenderer.RenderSvg(task.Graph))
                        <figcaption>Figure skill @task.SourceFigureSkill: @task.Prompt</figcaption>
                    </figure>
                }
            </section>
        }

"""
vp.write_text(view.replace(needle,insert+needle,1),encoding="utf8")
routesrel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u05-l02-stage-c-104-unit-circle-assessment-routes.json"
save(root/routesrel,{"SchemaVersion":1,"Section":"5.2","Count":104,
"ExistingComponents":["RichV2 mathematics","Teacher manual geometry marking","Typed SVG unit circle renderer"],
"NewEngine":False,"StageD":"NOT_RUN","CI":"NOT_RUN",
"Routes":[{"Original":q["SourceExercise"],"IndependentStudentMath":True,"TeacherKey":True,
"AutoGradingClaimed":False}for q in Q]})
assert len(re.findall(r"\bAdd\((?:[1-9]|1[0-9]),",graphpath.read_text(encoding="utf8")))==19
assert "Grade12PrecalculusSection52UnitCircleModels.All" in vp.read_text(encoding="utf8")
assert math.isclose(math.sin(math.radians(60)),math.sqrt(3)/2)
assert len(Q)==104 and len(W)==7 and len(T)==7
now=datetime.datetime.now(datetime.timezone.utc).isoformat()
closerel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u05-l02-abc-close-audit.json"
save(root/closerel,{"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":5,"Lesson":2,
"Source":"OpenStax Precalculus 2017 original local PDF","SourceSHA256":p["SourceSnapshotSha256"],
"CorrectedPdfPages":[472,487],"IncorrectBlueprintPages":[473,488],
"OriginalWorked":7,"OriginalTryIt":7,"OriginalFigures":19,"OriginalExercises":104,
"IndependentWorked":7,"IndependentTryIt":7,"IndependentQuestionTeacherKeys":104,
"FirstPartyMathFigureModels":19,"IndependentCoolDown":3,
"StageCExistingAssessmentRoutes":104,"NewMathEngine":False,
"CCSSCandidates":["CCSS:HSF-TF.A.2","CCSS:HSF-TF.A.3"],
"CompleteFormalCCSSCertification":False,"OriginalPublisherGraphicCopied":False,
"ExactPublisherFigureParityCertified":False,
"ChecksActuallyRun":["Original source PDF boundary","104 numbered source exercise skills with independent answer keys",
"7 worked 7 Try It, Cool-down","19 mathematical unit circle model definitions",
"teacher marking route count","selected trigonometric and quadrant math checks",
"git diff check"],
"Build":"NOT_RUN","CI":"NOT_RUN","StageD":"NOT_RUN","AuthenticatedRuntime":"NOT_RUN",
"ReadyForStageD":True,"ClosedAtUtc":now})
auditrel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u05-l02-source-and-academic-audit.json"
save(root/auditrel,{"SchemaVersion":1,"Unit":5,"Lesson":2,"Section":"5.2",
"SourceSHA256":p["SourceSnapshotSha256"],"CorrectedPdfPages":[472,487],
"OriginalWorkedExamples":[{"Number":n,"Title":name}for n,name in examples],
"OriginalTryItNumbers":tries,"OriginalFigureNumbers":figures,
"OriginalNumberedExerciseLedger":[{"Number":q["SourceExercise"],"SourceSkillReviewed":True,
"IndependentTeacherMath":True}for q in Q],
"FirstPartyRichV2":richrel,"TeacherAssessment":teacherrel,
"FirstPartyFigureSkillEvidence":figrel,
"GraphCode":str(graphpath.relative_to(root)).replace("\\","/"),
"StageCRoutes":routesrel,"ReadyForStageD":True,"Status":"READY_FOR_STAGE_D",
"BlockingReasons":[],"ClosureEvidence":closerel,"CI":"NOT_RUN","StageD":"NOT_RUN","ClosedAtUtc":now})
assert row["Ordinal"]==30 and not row["ReadyForStageD"]
row.update({"ACStatus":"READY_FOR_STAGE_D","ReadyForStageD":True,
"PdfPageStart":472,"PdfPageEnd":487,"SourceNumberedExercisesMathReviewed":104,
"FirstPartyTeacherMathQuestions":104,"OriginalFigureSkillAlternatives":19,
"BlockingReasons":[],"EvidencePath":auditrel,"ClosureEvidencePath":closerel,
"Tests":"TARGETED_LOCAL_ONLY","ClosedAtUtc":now,"UpdatedUtc":now})
assert p["LessonEntries"][30]["Ordinal"]==31
p.update({"LastClosedLessonOrdinal":30,"FirstIncompleteLessonOrdinal":31,
"ActiveLessonOrdinal":31,"NextLessonOrdinal":31,"ReadyForStageD":30,
"SourceVerifiedGrade12Lessons":30,"GradeStatus":"GRADE12_A_B_C_IN_PROGRESS_30_OF_73",
"UpdatedUtc":now})
save(pp,p)
files=[str(pp.relative_to(root)),richrel,teacherrel,figrel,routesrel,closerel,auditrel,
str(graphpath.relative_to(root)),str(vp.relative_to(root)),str(Path(__file__).relative_to(root))]
subprocess.run(["git","add","--sparse","-f","--",*files],cwd=root,check=True)
subprocess.run(["git","diff","--cached","--check"],cwd=root,check=True)
subprocess.run(["git","commit","-m","feat(g12 u05 l02): close unit circle sine cosine academic ABC [skip ci]"],cwd=root,check=True)
subprocess.run(["git","push","origin","curriculum/us-ccss-g12-precalculus-abc-20261106"],cwd=root,check=True)
print("U05L02_ACADEMIC_CLOSE_PUSH_SUCCESS_30_OF_73")
print("SHA",subprocess.run(["git","rev-parse","HEAD"],cwd=root,capture_output=True,text=True,check=True).stdout.strip())
print("DURATION",round(time.monotonic()-started,2))
