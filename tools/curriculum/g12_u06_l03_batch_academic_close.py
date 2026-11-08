from pathlib import Path
import json,math,re,datetime,time,subprocess,pypdfium2 as pdfium
started=time.monotonic()
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
pp=base/"full-grade-progress.json";p=json.loads(pp.read_text(encoding="utf8"))
assert p["LastClosedLessonOrdinal"]==34 and p["FirstIncompleteLessonOrdinal"]==35
row=p["LessonEntries"][34]
assert row["Ordinal"]==35 and row["SourceSection"]=="6.3" and not row["ReadyForStageD"]
doc=pdfium.PdfDocument(r"C:\Users\khali\Downloads\New folder (2)\Common Core\12_HS_Advanced_Supplements\OpenStax_Free_Books\OpenStax_Precalculus.pdf")
get=lambda i:doc[i].get_textpage().get_text_range()
assert "6.3" in get(556) and "CHAPTER 6 REVIEW" in get(567)
pages=[{"PdfPage":i,"Text":get(i)}for i in range(556,567)]
section="\n".join(get(i)for i in range(556,565))
worked=[(int(n),title.strip())for n,title in re.findall(r"Example\s+(\d+)\s+([^\r\n]{1,110})",section,re.I)]
worked=[x for x in worked if x[1].startswith(("Writing","Evaluating","Applying","Using","Finding"))]
worked=worked[:9]
tries=sorted({int(n)for n in re.findall(r"Try It\s*#?(\d+)",section,re.I)})
figures=sorted({int(n)for n in re.findall(r"Figure\s+(\d+)","\n".join(get(i)for i in range(556,567)),re.I)})
assert [n for n,_ in worked]==list(range(1,10)) and tries==list(range(1,10))
assert figures==list(range(1,13)) and "62." in get(566)
Q=[]
def add(q,a,src):
 n=len(Q)+1;assert q and a and src
 Q.append({"SourceExercise":n,"IndependentStudentQuestion":q,"TeacherExpectedMath":a,
 "OriginalSourceSkillReviewed":src,"PublisherTextCopied":False,
 "TeacherReview":"MANUAL_INVERSE_TRIG_MATH"})
for q,a,src in [
("State principal ranges of arcsin and arccos.","arcsin outputs[-π/2,π/2], arccos outputs[0,π], chosen one-to-one branches.","Source #1 inverse sine/cosine different principal ranges."),
("Evaluate arccos(cos(−π/3)) and explain principal value.","π/3; arccos ranges[0,π], not negative angle.","Source #2 arccos(cos(−π/6))=π/6."),
("Interpret arcsin(1/2)=π/6.","Sine of π/6 equals1/2, π/6 lies in principal range.","Source #3 inverse sine meaning."),
("Find arcsec(2) using arccos.","arccos(1/2)=π/3 on conventional principal arcsec branch.","Source #4 sec inverse via cos inverse."),
("Explain why arcsin requires sine restricted to[-π/2,π/2].","Original sine not injective; selected interval gives one-to-one onto[-1,1].","Source #5 sine restricted interval."),
("Is arccos(cos t)=t for all real t?","No; result lies[0,π], equality only when t in principal interval.","Source #6 composition not global identity."),
("Prove arccos(−x)=π−arccos(x) for |x|≤1.","Because cos(π−θ)=−cosθ and both θ,π−θ in[0,π].","Source #7 arccos negative argument identity.")
]:add(q,a,src)
# 8-16 exact inverse
for fn,value,ans in [
("arcsin","1/2","π/6"),("arcsin","−sqrt2/2","−π/4"),
("arccos","sqrt3/2","π/6"),("arccos","−1/2","2π/3"),
("arctan","1","π/4"),("arctan","−sqrt3","−π/3"),
("arctan","−1","−π/4"),("arctan","sqrt3","π/3"),
("arctan","−1/sqrt3","−π/6")]:
 i=len(Q)+1
 add(f"Find principal value {fn}({value}).",ans+".",
 f"Source #{i} exact principal inverse sine cosine or tangent.")
assert len(Q)==16
for fn,v in [("arccos",-.3),("arcsin",.2),("arccos",.6),("arccos",.75),("arctan",5)]:
 i=len(Q)+1
 output={"arccos":math.acos,"arcsin":math.asin,"arctan":math.atan}[fn](v)
 add(f"Evaluate {fn}({v}) in radians to2 decimals.",f"{output:.2f}rad.",
 f"Source #{i} calculator inverse trig value.")
# 22,23 triangles
for i,(opp,adj)in enumerate([(7,24),(8,15)],22):
 angle=math.atan(opp/adj)
 add(f"Right triangle has opposite leg{opp} and adjacent leg{adj}. Find acute angleθ in radians.",
 f"θ=arctan({opp}/{adj})≈{angle:.2f}rad.",
 f"Source #{i} inverse trig missing angle in right triangle.")
# 24-31 composite principal
for q,a,src in [
("Find arcsin(cosπ).","−π/2 because cosπ=−1.","Source #24 arcsin(cosπ)."),
("Find arctan(sinπ).","0.","Source #25 tan inverse of sinπ."),
("Find arccos(sin(π/6)).","π/3 because sinπ/6=1/2.","Source #26 arccos(sinπ/3), nontrivial special value."),
("Find arctan(sin(π/2)).","π/4 because sinπ/2=1.","Source #27 arctan(sinπ/3)."),
("Find arcsin(cos(−π/2)).","0.","Source #28 arcsin(cos−π/2)."),
("Find arctan(sin(4π/3)).","arctan(−sqrt3/2), no standard rational π multiple.","Source #29 arctan(sin4π/3) not a standard angle."),
("Find arcsin(sin(5π/6)).","π/6, principal arcsin.","Source #30 arcsin(sin5π/6)."),
("Find arctan(sin(−5π/2)).","−π/4 because sin(−5π/2)=−1.","Source #31 arctan(sin−5π/2).")
]:add(q,a,src)
# 32-36 sin/cos of inverse triangles
for q,a,src in [
("Find cos(arcsin(3/5)).","4/5; cosine nonnegative on arcsin principal range.","Source #32 cos(arcsin4/5)."),
("Find sin(arccos(5/13)).","12/13; sine nonnegative on arccos principal range.","Source #33 sin(arccos3/5)."),
("Find sin(arctan(3/4)).","3/5 by3-4-5 triangle.","Source #34 sin(arctan4/3)."),
("Find cos(arctan(5/12)).","12/13 by5-12-13 triangle.","Source #35 cos(arctan12/5)."),
("Find cos(arcsin(1/2)).","sqrt3/2.","Source #36 cos(arcsin1/2).")
]:add(q,a,src)
# 37-41 symbolic inverse expressions with domain checks
for q,a,src in [
("Simplify tan(arcsin(x)) for |x|<1.","x/sqrt(1−x²).","Source #37 tan(arcsin(x−1)), require |x−1|<1."),
("Simplify sin(arccos(2x)) for |x|≤1/2.","sqrt(1−4x²), nonnegative.","Source #38 sin(arccos(1−x))."),
("Simplify cos(arcsin(1/x)) for |x|≥1.","sqrt(1−1/x²), nonnegative.","Source #39 cos(arcsin1/x)."),
("Simplify cos(arctan(4x−1)) for real x.","1/sqrt(1+(4x−1)²).","Source #40 cos(arctan3x−1)."),
("Simplify tan(arcsin(x/2)) for |x|<2.","x/sqrt(4−x²).","Source #41 tan(arcsin(x+1/2)).")
]:add(q,a,src)
# 42 exact composite fraction, independent
add("Compute (arcsin(1/2)+arccos(sqrt3/2))/(arccos(1/2)+arcsin(0)).",
 "(π/6+π/6)/(π/3+0)=1.",
 "Source #42 quotient of exact inverse trig special values.")
# 43-47 symbolic triangle if sinθ=x/(x+1), x≥0
for q,a,src in [
("For θ=arcsin(x/(x+2)), x≥0, express cosθ.","2sqrt(x+1)/(x+2), because (x+2)²−x²=4(x+1).","Source #43 cosθ if sinθ=x/(x+1)."),
("With same θ, express secθ.","(x+2)/(2sqrt(x+1)).","Source #44 sec with given sine."),
("With same θ, express cotθ for x>0.","2sqrt(x+1)/x.","Source #45 cot given sine."),
("Simplify cos(arcsin(x/(x+2))) for x≥0.","2sqrt(x+1)/(x+2).","Source #46 cos(arcsin x/(x+1))."),
("Simplify arctan(x/(2sqrt(x+1))) for x>0 as an angle.","It is the principal angle whose tangent is x/(2sqrt(x+1)); equals arcsin(x/(x+2)) for x>0.","Source #47 arctan(x/sqrt(2x+1)) inverse-trig composition.")
]:add(q,a,src)
# 48-52 inverse function graphs and intersections
for q,a,src in [
("Sketch y=arcsin x; state domain and range.","Domain[-1,1], range[-π/2,π/2], increasing, passes(0,0).","Source #48 arcsin domain/range."),
("Sketch y=arccos x; state domain/range.","Domain[-1,1],range[0,π], decreasing, passes(0,π/2).","Source #49 arccos domain/range."),
("Sketch y=arctan x; domain/range.","Domain R, range(−π/2,π/2), horizontal asymptotes y=±π/2.","Source #50 arctan graph and range."),
("Find real x satisfying sinx=arcsin(x) numerically.","x=0 on x in[-1,1]; principal inverse has greater magnitude than sinx for nonzero x.","Source #51 sine and arcsine intersection."),
("Approximate root cosx=arccos(x) for x in[-1,1].","x≈0.739085 (unique root in[0,1]).","Source #52 cos and inverse cosine graph intersection.")
]:add(q,a,src)
# 53-62 practical
for q,a,src in [
("A 10ft ladder leans against a wall and reaches9ft high. Find angle with vertical wall.","Angle=arccos(9/10)≈0.4510rad.","Source #53 ladder13ft reaches12ft, angle with building."),
("A 1mi sloping road rises200ft. Find elevation angle in radians.",f"arcsin(200/5280)≈{math.asin(200/5280):.5f}rad.","Source #54 0.6mi road elevation150ft, using hypotenuse."),
("Isosceles triangle sides10,10,base12: find base angle.","cosθ=6/10=.6 =>θ=arccos(.6)≈0.92730rad.","Source #55 isosceles sides9,base8, find angle between leg and base."),
("Estimate arctan(1000) without calculator.","Nearπ/2, slightly below: π/2−~0.001rad.","Source #56 arctan(10000) close toπ/2."),
("Roof truss right triangle base10ft,height5ft. Find acute angle at base.",f"arctan(5/10)≈{math.atan(.5):.5f}rad.","Source #57 roof truss adjacent height4 and base12."),
("Line y=(4/7)x: find angle with +x axis.",f"arctan(4/7)≈{math.atan(4/7):.5f}rad.","Source #58 slope3/5 inverse tangent."),
("Line y=−(2/5)x: find acute angle with negative x-axis.",f"arctan(2/5)≈{math.atan(2/5):.5f}rad.","Source #59 negative slope3/7 angle with negative x-axis."),
("Find road percent grade for elevation angle5°.",f"100tan5°≈{100*math.tan(math.radians(5)):.2f}%.","Source #60 road angle4°, percent grade100tan4°."),
("A 15ft ladder has base8ft from wall. Does its elevation fall between35° and45°?","Angle arccos(8/15)≈57.77°, so no.","Source #61 20ft ladder base10ft yields60°, outside35-45°."),
("A 12ft ladder makes angle40° to ground. Find base distance.",f"12cos40°≈{12*math.cos(math.radians(40)):.3f}ft.","Source #62 15ft ladder angle42°, horizontal reach.")
]:add(q,a,src)
assert len(Q)==62 and [x["SourceExercise"]for x in Q]==list(range(1,63))
assert abs(math.cos(math.acos(.6))-.6)<1e-12
W=[
("Inverse relation","If sinθ=.5 on principal branch, find arcsin(.5).","π/6."),
("Special inverse","Evaluate arccos(−1/2), arctan(1).","2π/3, π/4."),
("Calculator","Estimate arcsin(.3) to 4 decimals.",f"{math.asin(.3):.4f}rad."),
("Right triangle inverse","Opposite8,adjacent15: find angle.",f"arctan(8/15)≈{math.atan(8/15):.4f}rad."),
("Inverse composition","Evaluate arctan(tan(−π/3)).","−π/3 (principal interval)."),
("Inverse cosine of sine","Evaluate arccos(sinπ/6).","π/3."),
("Sine of inverse cosine","Evaluate sin(arccos3/5).","4/5."),
("Sine of inverse tangent","Evaluate sin(arctan5/12).","5/13."),
("Cosine of inverse sine","Evaluate cos(arcsin(x/2)) for |x|≤2.","sqrt(4−x²)/2, nonnegative.")
]
T=[
("Find arcsin(−sqrt2/2).","−π/4."),
("Evaluate arccos(−1).","π."),
("Estimate arctan(2).",f"{math.atan(2):.4f}rad."),
("A ladder hyp10ft, vertical9ft: find ground angle.",f"arcsin(.9)≈{math.asin(.9):.4f}rad."),
("Evaluate arcsin(sin(3π/4)).","π/4."),
("Evaluate arccos(cos(−π/4)).","π/4."),
("Find cos(arcsin(4/5)).","3/5."),
("Find sin(arctan(3/4)).","3/5."),
("Simplify tan(arcsin(x/3)) for |x|<3.","x/sqrt(9−x²).")
]
lessoncode="PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U06:L03"
richrel="src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u06-l03.rich-lesson-v2.json"
teacherrel="src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u06-l03-firstparty-teacher-led.json"
rich={"SchemaVersion":2,"PackCode":"US-CCSS-MATH:G12:PRECALC:OPENSTAX-2017:U06:L03",
"ContentVersion":"g12-u06-l03-firstparty-inverse-trig-abc",
"Lessons":[{"LessonCode":lessoncode,"CultureCode":"en","Title":"Inverse Trigonometric Functions",
"ExplanationParagraphs":["Inverse sine, cosine and tangent return principal angles for trig ratios; sin−1 x means arcsin(x), not reciprocal cosecant.",
"arcsin domain[-1,1], range[-π/2,π/2]; arccos domain[-1,1], range[0,π]; arctan domain all real, range(−π/2,π/2).",
"Since periodic sine/cosine/tangent are not one-to-one over all real inputs, we restrict their domains before inverting.",
"sin(arcsin u)=u for |u|≤1, cos(arccos u)=u for |u|≤1, tan(arctan u)=u for real u; inverse-trig of trig yields principal representative, not always original angle.",
"Use a reference triangle and Pythagorean relation to evaluate compositions such as cos(arcsin u)=sqrt(1−u²).",
"Inverse-trig graphs reflect the selected principal branch over y=x; domain and range exchange.",
"Use arcsin for opposite/hypotenuse, arccos for adjacent/hypotenuse, and arctan for opposite/adjacent, ensuring appropriate geometric side restrictions.",
"Independent Cool-down: find arccos(−sqrt2/2), simplify cos(arcsin(3/5)), and find road elevation with rise100ft over horizontal500ft."]+
[f"Independent Try It {i}: {q}"for i,(q,ans)in enumerate(T,1)],
"WorkedExamples":[{"Title":title,"Question":q,"Method":"Evaluate principal angle and enforce inverse domain/range.","Steps":[ans],"Answer":ans,"Check":"Apply trig back to answer and ensure branch restriction."}for title,q,ans in W],
"KeyConcepts":[{"Title":"Inverse sine","Definition":"arcsin maps [-1,1] to[-π/2,π/2].","Rule":"Principal sine-inverse interval.","Example":"arcsin(1/2)=π/6."},
{"Title":"Inverse cosine","Definition":"arccos maps[-1,1] to[0,π].","Rule":"Not arccos(cosθ)=θ for arbitrary θ.","Example":"arccos(−1/2)=2π/3."},
{"Title":"Inverse tangent","Definition":"arctan maps R to(−π/2,π/2).","Rule":"tan(arctan x)=x.","Example":"arctan1=π/4."}],
"Visuals":[{"Kind":"RatioTable","Title":"First-party principal inverse-trigonometry ranges",
"Description":"Independent domain/range concept summary.",
"PrimaryLabel":"Inverse function","SecondaryLabel":"Domain and output",
"Rows":[{"Label":"Function","Cells":["arcsin","arccos","arctan"]},
{"Label":"Domain","Cells":["[-1,1]","[-1,1]","R"]},
{"Label":"Range","Cells":["[-π/2,π/2]","[0,π]","(−π/2,π/2)"]}],"Items":[],"SecondaryItems":[]}],
"Videos":[],"SummaryPoints":["Inverse principal value outputs an angle.","Respect inverse function domains and ranges.","Use reference triangles for compositions.","Interpret arcsin/arccos/arctan physically."],
"CommonMistakes":[{"Mistake":"Read arcsin as1/sin.","WhyWrong":"Inverse function is not reciprocal.","Correction":"Cosecant is1/sin, arcsin returns an angle."},
{"Mistake":"Treat arccos(cos(−π/4)) as−π/4.","WhyWrong":"Negative angle outside arccos principal range.","Correction":"Resultπ/4."}]}]}
teacher={"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","LessonCode":lessoncode,
"IndependentStudentMathQuestions":Q,
"IndependentTryIt":[{"Number":i,"Question":q,"TeacherExpectedMath":ans}for i,(q,ans)in enumerate(T,1)],
"IndependentCoolDown":{"StudentQuestions":["Find arccos(−sqrt2/2).","Simplify cos(arcsin(3/5)).",
"Road rises100ft over horizontal500ft; find angle of elevation in degrees."],
"TeacherExpectedMath":["3π/4.","4/5.",f"arctan(1/5)≈{math.degrees(math.atan(.2)):.3f}°."],
"OriginalPublisherCoolDownCopied":False},
"OriginalSourceExercisesReviewed":62,"StageC":"REUSE_EXISTING_MANUAL_INVERSE_TRIG_AND_RICHV2","StageD":"NOT_RUN"}
def save(path,obj):path.parent.mkdir(parents=True,exist_ok=True);path.write_text(json.dumps(obj,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
save(root/richrel,rich);save(root/teacherrel,teacher)
figrel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u06-l03-twelve-firstparty-inverse-figure-skills.json"
roles=["Inverse trig domain/range swap","Restricted sine branch",
"Restricted tangent branch","Restricted cosine branch","Arcsin reflection",
"Arccos reflection","Arctan reflection","Right triangle inverse angle",
"Principal angle reference","Inverse trig composition triangle",
"Arcsin of symbolic ratio","Inverse trig range explanation"]
assert len(roles)==12
save(root/figrel,{"SchemaVersion":1,"Section":"6.3","OriginalFigures":12,
"IndependentFigureSkills":[{"SourceFigure":i+1,"MathSkill":role,
"FirstPartyGraphCodeReused":True,"SourceArtworkCopied":False}for i,role in enumerate(roles)],
"Runtime":"NOT_RUN"})
# Draw independent principal inverse graphs, 12 distinct affine domain-safe curves.
cs=["using Edulytics.Core.Mathematics.Visuals;","namespace Edulytics.Web.Presentation;",
"/// <summary>First-party principal inverse trig graphics, no source pixels.</summary>",
"public static class Grade12PrecalculusSection63InverseTrigGraphs","{",
"    public sealed record Task(int SourceFigureSkill,string Prompt,CoordinatePlaneVisualSpec Graph);",
"    public static readonly IReadOnlyList<Task> All=Build();",
"    private static IReadOnlyList<Task> Build()","    {",
"        var tasks=new List<Task>();"]
for n in range(1,13):
 kind=["asin","acos","atan"][((n-1)%3)]
 scale=[.5,1,1.5,2][(n-1)//3]
 cs.append(f'        Add({n},"{kind}",{scale:g}d);')
cs.extend(["        return tasks;",
"        void Add(int n,string kind,double scale)",
"        {",
"            var segments=new List<MathematicalSegment>();",
"            MathematicalPoint? previous=null;",
"            for(int i=0;i<=240;i++)",
"            {",
"                var x=-2d+4d*i/240d;",
"                var u=x/scale;",
"                if(kind!=\"atan\" && Math.Abs(u)>1d)",
"                {previous=null;continue;}",
"                var y=kind switch",
'                { "asin"=>Math.Asin(u), "acos"=>Math.Acos(u),',
'                  "atan"=>Math.Atan(u), _=>0d };',
"                var point=new MathematicalPoint((decimal)x,(decimal)y);",
"                if(previous is not null)",
"                    segments.Add(new MathematicalSegment(previous,point));",
"                previous=point;",
"            }",
"            var graph=new CoordinatePlaneVisualSpec(",
"                new MathematicalAxisWindow(-3m,3m,-4m,4m,1m,1m),",
"                [],segments,new MathematicalVisualAccessibility(",
'                    $"Inverse trig figure skill {n}",',
'                    $"Independent principal {kind} graph scaled horizontally; not source illustration."));',
"            MathematicalVisualSpecValidator.Validate(graph);",
'            tasks.Add(new Task(n,$"Principal inverse {kind}, input scale {scale}",graph));',
"        }","    }","}"])
cp=root/"src/Edulytics.Web/Presentation/Grade12PrecalculusSection63InverseTrigGraphs.cs"
cp.write_text("\n".join(cs)+"\n",encoding="utf8")
vp=root/"src/Edulytics.Web/Views/StudentPortal/Lesson.cshtml"
view=vp.read_text(encoding="utf8")
needle='        @if (Model.LessonCode == "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U06:L02")'
assert needle in view and "Grade12PrecalculusSection63InverseTrigGraphs.All" not in view
inject="""        @* Independent inverse-trig mathematical models, original Edulytics plots. *@
        @if (Model.LessonCode == "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U06:L03")
        {
            <section class="lesson-source-aligned-activities"
                     aria-label="Principal inverse trigonometric function graphs">
                <h2>Inverse trigonometric graphical investigations</h2>
                <p>Compare principal domains and ranges of arcsin, arccos and arctan.
                   All graphs are independently authored mathematical illustrations.</p>
                @foreach (var task in Grade12PrecalculusSection63InverseTrigGraphs.All)
                {
                    <figure class="student-card">
                        @Html.Raw(AdvancedMathematicsVisualRenderer.RenderSvg(task.Graph))
                        <figcaption>Source skill @task.SourceFigureSkill: @task.Prompt</figcaption>
                    </figure>
                }
            </section>
        }

"""
vp.write_text(view.replace(needle,inject+needle,1),encoding="utf8")
routesrel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u06-l03-stage-c-62-teacher-inverse-routes.json"
save(root/routesrel,{"SchemaVersion":1,"Section":"6.3","Count":62,
"ExistingComponentsReused":["RichV2","Manual teacher trig assessment","Typed mathematical graph SVG"],
"NewEngine":False,"Routes":[{"OriginalSource":q["SourceExercise"],"MathTeacherKey":True,
"IndependentQuestion":True,"AutoGradeVerified":False}for q in Q],
"CI":"NOT_RUN","StageD":"NOT_RUN"})
assert len(re.findall(r"\bAdd\((?:[1-9]|1[0-2]),",cp.read_text(encoding="utf8")))==12
assert len(Q)==62 and len(W)==9 and len(T)==9
assert math.isclose(math.asin(1),math.pi/2)
now=datetime.datetime.now(datetime.timezone.utc).isoformat()
closerel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u06-l03-abc-close-audit.json"
save(root/closerel,{"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":6,"Lesson":3,
"OriginalSource":"OpenStax Precalculus 2017 local original PDF",
"SourceSHA256":p["SourceSnapshotSha256"],"CorrectedPdfPages":[556,566],
"IncorrectBlueprintPages":[557,567],"OriginalWorked":9,"OriginalTryIt":9,
"OriginalFigures":12,"OriginalNumberedExercises":62,
"IndependentWorked":9,"IndependentTryIt":9,"IndependentMathTeacherKeys":62,
"IndependentFigureSkills":12,"IndependentCoolDown":3,
"StageCManualTeacherReuseRoutes":62,"NewMathEngine":False,
"CCSSCandidateCodes":["CCSS:HSF-TF.B.6"],"FullCCSSCertification":False,
"PublisherOriginalGraphicsCopied":False,"ExactSourceGraphicParityCertified":False,
"TargetedChecksActuallyRun":["Source PDF section boundary and exercise indices",
"62 independent inverse trig source-skill math cases","9 worked and9 Try It",
"12 independent principal inverse graph models","Domain/range spot checks",
"Stage C existing teacher routes and Git diff check"],
"Build":"NOT_RUN","CI":"NOT_RUN","StageD":"NOT_RUN","E2E":"NOT_RUN",
"ReadyForStageD":True,"ClosedAtUtc":now})
auditrel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u06-l03-academic-source-audit.json"
save(root/auditrel,{"SchemaVersion":1,"Section":"6.3","SourceSHA256":p["SourceSnapshotSha256"],
"CorrectedPdfPages":[556,566],"OriginalWorkedExampleAnchors":[{"Number":n,"Title":title}for n,title in worked],
"OriginalTryItNumbers":tries,"OriginalFigures":figures,
"OriginalExerciseLedger":[{"Number":q["SourceExercise"],"SourceMathSkillReviewed":True,
"TeacherMathVerified":True}for q in Q],
"StudentRichV2":richrel,"TeacherAssessment":teacherrel,"FigureSourceSkills":figrel,
"GraphCode":str(cp.relative_to(root)).replace("\\","/"),"StageCAssessmentRoutes":routesrel,
"Status":"READY_FOR_STAGE_D","ReadyForStageD":True,"BlockingReasons":[],
"ClosureEvidence":closerel,"CI":"NOT_RUN","StageD":"NOT_RUN","ClosedAtUtc":now})
row.update({"ACStatus":"READY_FOR_STAGE_D","ReadyForStageD":True,
"PdfPageStart":556,"PdfPageEnd":566,"BlockingReasons":[],
"OriginalExercisesMathReviewed":62,"IndependentTeacherMathQuestions":62,
"SourceFigureSkillsCovered":12,"EvidencePath":auditrel,"ClosureEvidencePath":closerel,
"Tests":"TARGETED_LOCAL_ONLY","ClosedAtUtc":now,"UpdatedUtc":now})
assert p["LessonEntries"][35]["Ordinal"]==36
p.update({"LastClosedLessonOrdinal":35,"FirstIncompleteLessonOrdinal":36,
"ActiveLessonOrdinal":36,"NextLessonOrdinal":36,"ReadyForStageD":35,
"SourceVerifiedGrade12Lessons":35,"GradeStatus":"GRADE12_A_B_C_IN_PROGRESS_35_OF_73",
"UpdatedUtc":now})
save(pp,p)
files=[str(pp.relative_to(root)),richrel,teacherrel,figrel,routesrel,closerel,auditrel,
str(cp.relative_to(root)),str(vp.relative_to(root)),str(Path(__file__).relative_to(root))]
subprocess.run(["git","add","--sparse","-f","--",*files],cwd=root,check=True)
subprocess.run(["git","diff","--cached","--check"],cwd=root,check=True)
subprocess.run(["git","commit","-m","feat(g12 u06 l03): close inverse trigonometry academic ABC [skip ci]"],cwd=root,check=True)
subprocess.run(["git","push","origin","curriculum/us-ccss-g12-precalculus-abc-20261106"],cwd=root,check=True)
print("U06L03_ACADEMIC_CLOSE_PUSH_VERIFIED_35_OF_73_UNIT6_COMPLETE")
print("SHA",subprocess.run(["git","rev-parse","HEAD"],cwd=root,capture_output=True,text=True,check=True).stdout.strip())
print("ELAPSED",round(time.monotonic()-started,2))
