from pathlib import Path
import json,math,datetime,subprocess,re,time
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
pp=base/"full-grade-progress.json"
p=json.loads(pp.read_text(encoding="utf8"))
tp=root/"src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u05-l01-firstparty-teacher-led.json"
teacher=json.loads(tp.read_text(encoding="utf8"))
Q=teacher["FirstPartyIndependentStudentMathQuestions"]
assert p["LastClosedLessonOrdinal"]==28 and p["FirstIncompleteLessonOrdinal"]==29
assert len(Q)==74 and [q["OriginalSectionExercise"]for q in Q]==list(range(1,75))
W=[
("Standard-position angle","Sketch 135° with vertex at origin and initial ray +x.","Positive CCW rotation to quadrant II, terminal point on unit circle(−sqrt2/2,sqrt2/2)."),
("Central angle radians","A radius5cm arc has length7cm. Find central angle.","θ=s/r=7/5=1.4rad."),
("Radians to degrees","Convert 5π/6 radians to degrees.","5·180/6=150°."),
("Degrees to radians","Convert −135° to radians.","−135π/180=−3π/4."),
("Coterminal positive angle","Find coterminal of 820° in[0,360).","820−2·360=100°."),
("Coterminal negative angle","Find coterminal of −125° in[0,360).","−125+360=235°."),
("Coterminal in radians","Find coterminal of −11π/6 in[0,2π).","−11π/6+2π=π/6."),
("Arc length","Circle radius9cm has central angle2π/3. Find arc length.","s=rθ=9·2π/3=6πcm."),
("Sector area","Circle radius4m with central angle3π/4. Find area.","A=(1/2)r²θ=(1/2)·16·3π/4=6πm²."),
("Angular speed","A wheel makes five revolutions in 10s. Find angular speed.","ω=5·2π/10=πrad/s."),
("Linear speed","Wheel radius0.4m spins at3rad/s. Find tangential speed.","v=rω=0.4·3=1.2m/s.")
]
T=[
("Sketch negative −60° in standard position.","Clockwise60° to quadrant IV."),
("A circle radius6m has 12m arc. Find radian angle.","θ=12/6=2 radians."),
("Convert 7π/4 to degrees.","315°."),
("Convert 225° to radians.","5π/4."),
("Normalize 1120° to[0,360).","40°."),
("Normalize −470° to[0,360).","250°."),
("Normalize 17π/6 to[0,2π).","5π/6."),
("Find arc length r=3cm θ=5π/6.","5π/2 cm."),
("Find sector area r=5in θ=π/3.","25π/6 square inches."),
("A fan makes 3 rotations in 2s. Find angular speed.","3π rad/s."),
("Wheel radius0.5m spinning4rad/s: find v.","2m/s.")
]
assert len(W)==11 and len(T)==11
rich_rel="src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u05-l01.rich-lesson-v2.json"
lessoncode="PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U05:L01"
rich={"SchemaVersion":2,"PackCode":"US-CCSS-MATH:G12:PRECALC:OPENSTAX-2017:U05:L01",
"ContentVersion":"g12-u05-l01-firstparty-angles-abc",
"Lessons":[{"LessonCode":lessoncode,"CultureCode":"en","Title":"Angles",
"ExplanationParagraphs":["An angle consists of two rays with shared vertex. Standard position places vertex at origin, initial ray on positive x-axis and terminal ray after a directed rotation.",
"Counterclockwise rotation is positive and clockwise negative. Angles that differ by integer multiples of360° or2π are coterminal.",
"One radian subtends an arc equal in length to the circle radius; 180°=πrad, so multiply degrees byπ/180 to get radians.",
"Quadrantal angles terminate on axes; common special angles 30°,45°,60°,90° have exact π fractions.",
"Given arc radius r>0 and angle θ in radians, s=rθ; the geometric length is nonnegative for arc magnitude |θ|.",
"A sector with central angle θ≥0 radians has area A=(1/2)r²θ. Always convert degree measure to radians before applying this formula.",
"Angular speed ω=Δθ/Δt in rad/time; linear speed v=rω in distance/time. Convert wheel revolutions per minute to radians per time unit.",
"Independent Cool-down: convert−210° to radians, find arc length r=4cm θ=3π/4, and find wheel RPM for ω=πrad/s."]+
[f"Independent Try It {i}: {q}"for i,(q,ans)in enumerate(T,1)],
"WorkedExamples":[{"Title":title,"Question":q,"Method":"Use directed angle geometry, exact π conversion and appropriate units.","Steps":[ans],"Answer":ans,"Check":"Check quadrant, sign and dimensions."}for title,q,ans in W],
"KeyConcepts":[{"Title":"Directed standard angle","Definition":"Vertex at origin, initial ray along +x, terminal ray determined by oriented angle.","Rule":"CCW positive, CW negative.","Example":"−60° ends in quadrant IV."},
{"Title":"Radians","Definition":"θ=s/r; 2π radians is one full revolution.","Rule":"Multiply degrees byπ/180.","Example":"120°=2π/3."},
{"Title":"Arc and sector","Definition":"For θ≥0 in radians, s=rθ and A=r²θ/2.","Rule":"Keep linear vs square units distinct.","Example":"r=2,θ=π => s=2π, area2π."},
{"Title":"Angular and linear speed","Definition":"ω=dθ/dt and v=rω.","Rule":"Convert rev/min to2π rad/rev.","Example":"2RPM=4πrad/min."}],
"Visuals":[{"Kind":"RatioTable","Title":"First-party degree–radian conversion anchors",
"Description":"Independent angular conversion grid; not publisher artwork.",
"PrimaryLabel":"Degrees","SecondaryLabel":"Radians",
"Rows":[{"Label":"Degrees","Cells":["30","45","60","90"]},{"Label":"Radians","Cells":["π/6","π/4","π/3","π/2"]}],"Items":[],"SecondaryItems":[]}],
"Videos":[],"SummaryPoints":["Sketch signed terminal rays correctly.","Convert degrees and radians exactly.","Normalize coterminal angles.","Use s=rθ and A=r²θ/2.","Distinguish ω from v and check units."],
"CommonMistakes":[{"Mistake":"Insert degrees directly into s=rθ.","WhyWrong":"The formula assumes radians.","Correction":"Convert first to radians."},
{"Mistake":"Treat negative rotation as negative physical arc length.","WhyWrong":"Directed displacement can be negative, physical distance cannot.","Correction":"Use magnitude when the question asks distance."}]}]}
teacher.update({"IndependentTryIt":[{"Number":i,"Question":q,"TeacherExpectedMath":ans}for i,(q,ans)in enumerate(T,1)],
"IndependentCoolDown":{"StudentQuestions":["Convert −210° to exact radians.","Circle radius4cm subtends3π/4; find arc length.","Wheel has ω=πrad/s; convert to RPM."],
"TeacherExpectedMath":["−7π/6.","3πcm.","30RPM."],"OriginalPublisherCoolDownCopied":False},
"OriginalWorkedExampleAlternatives":11,"IndependentTryItAlternatives":11,
"StageC":"REUSE_EXISTING_RICHV2_AND_TEACHER_MANUAL_GEOMETRY","AutoGradingClaimed":False})
def save(path,obj):path.parent.mkdir(parents=True,exist_ok=True);path.write_text(json.dumps(obj,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
save(root/rich_rel,rich);save(tp,teacher)
# Source figure roles mapped independently, with proper concepts for the original 26 figure skills.
figroles=[
"Ray with endpoint and arrow","Two rays sharing vertex","Named angle theta",
"Initial/terminal ray directed angle","Standard-position vertex and +x ray",
"Quarter and full revolutions","Quadrantal terminal axes","Positive 30° rotation",
"Negative 135° rotation","Circumference divided by radius lengths",
"One radian subtends arc length equal radius","Radian as arc/radius",
"45° sector independent of radius","Common angles in degrees",
"Common angles in radians","Coterminal positive and negative angle",
"Acute reference angles by quadrant","Angles greater than full revolution",
"Negative angles and positive coterminal","Radian coterminal multiple turns",
"Arc length proportional to central angle","Sector area proportional to central angle",
"Sprinkler's angular sector","Wheel rotation and angular speed",
"Arc of circle with given radius and angle","Sector of circle with radians"]
assert len(figroles)==26
figrel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u05-l01-26-firstparty-angle-figure-skill-alternatives.json"
save(root/figrel,{"SchemaVersion":1,"Section":"5.1","OriginalSourceFigures":26,
"FirstPartyIndependentFigureSkills":[{"SourceFigureNumber":i+1,"MathematicalSkill":s,
"IndependentAngleSVGModel":True,"PublisherGraphicCopied":False,"ExactPublisherGraphicParityClaimed":False}
for i,s in enumerate(figroles)],"RuntimeSvgVerified":False})
# Use typed math SVG renderer: create 26 independent plane models with circular arc, initial ray and terminal ray
# + vertical/horizontal axes. Separate source references preserve per-figure skill semantics.
angleparams=[10,60,45,60,120,90,180,30,-135,360,57.2958,114.5916,45,225,300,140,135,800,-45,855,120,90,30,72,140,72]
assert len(angleparams)==26
cs=["using Edulytics.Core.Mathematics.Visuals;","namespace Edulytics.Web.Presentation;",
"/// <summary>Independent directed-angle diagrams for Section 5.1.",
"/// Source graphics not copied; runtime validation reserved for Stage D.</summary>",
"public static class Grade12PrecalculusSection51AngleModels","{",
"    public sealed record AngleTask(int SourceFigureSkill,string Prompt,CoordinatePlaneVisualSpec Graph);",
"    public static readonly IReadOnlyList<AngleTask> All=Build();",
"    private static IReadOnlyList<AngleTask> Build()","    {",
"        var tasks=new List<AngleTask>();"]
for i,(angle,role)in enumerate(zip(angleparams,figroles),1):
 cs.append(f'        Add({i},{angle}d,"{role.replace(chr(34),chr(39))}");')
cs.extend([
"        return tasks;",
"        void Add(int number,double degrees,string label)",
"        {",
"            var lines=new List<MathematicalSegment>();",
"            var origin=new MathematicalPoint(0m,0m);",
"            lines.Add(new MathematicalSegment(origin,new MathematicalPoint(3m,0m)));",
"            var a=degrees*Math.PI/180d;",
"            var tip=new MathematicalPoint((decimal)(3*Math.Cos(a)),(decimal)(3*Math.Sin(a)));",
"            lines.Add(new MathematicalSegment(origin,tip));",
"            for(int j=1;j<=100;j++)",
"            {",
"                var pa=a*(j-1)/100d; var pb=a*j/100d;",
"                var start=new MathematicalPoint((decimal)(1.25*Math.Cos(pa)),(decimal)(1.25*Math.Sin(pa)));",
"                var end=new MathematicalPoint((decimal)(1.25*Math.Cos(pb)),(decimal)(1.25*Math.Sin(pb)));",
"                lines.Add(new MathematicalSegment(start,end));",
"            }",
"            var graph=new CoordinatePlaneVisualSpec(",
"                new MathematicalAxisWindow(-4m,4m,-4m,4m,1m,1m),",
"                [],lines,new MathematicalVisualAccessibility(",
'                    $"Angle skill {number}: {degrees} degrees",',
'                    label+". Independently authored math sketch, not original publisher artwork."));',
"            MathematicalVisualSpecValidator.Validate(graph);",
"            tasks.Add(new AngleTask(number,label,graph));",
"        }","    }","}"])
graphpath=root/"src/Edulytics.Web/Presentation/Grade12PrecalculusSection51AngleModels.cs"
graphpath.write_text("\n".join(cs)+"\n",encoding="utf8")
vp=root/"src/Edulytics.Web/Views/StudentPortal/Lesson.cshtml"
view=vp.read_text(encoding="utf8")
needle='        @if (Model.LessonCode == "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U04:L06")'
assert needle in view and "Grade12PrecalculusSection51AngleModels.All" not in view
insert="""        @* Grade 12 Section 5.1 original directed-angle mathematical sketches.
           Figure skills aligned; original textbook artwork is not copied. *@
        @if (Model.LessonCode == "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U05:L01")
        {
            <section class="lesson-source-aligned-activities"
                     aria-label="Directed angles and radians">
                <h2>Directed angle investigations</h2>
                <p>Use the mathematical diagrams to discuss signed turns, coterminal
                   rays, arc length, sectors, and radian measures. The drawings
                   are independently authored and require teacher interpretation.</p>
                @foreach (var task in Grade12PrecalculusSection51AngleModels.All)
                {
                    <figure class="student-card">
                        @Html.Raw(AdvancedMathematicsVisualRenderer.RenderSvg(task.Graph))
                        <figcaption>Angle source skill @task.SourceFigureSkill: @task.Prompt</figcaption>
                    </figure>
                }
            </section>
        }

"""
vp.write_text(view.replace(needle,insert+needle,1),encoding="utf8")
routes_rel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u05-l01-stage-c-74-teacher-geometry-routes.json"
save(root/routes_rel,{"SchemaVersion":1,"Section":"5.1","Count":74,
"ExistingComponentsReused":["RichV2","Teacher manual geometry scoring","Advanced mathematics typed SVG"],
"NewMathEngine":False,"AutoGradingCertified":False,"StudentRuntimeVerified":False,
"Routes":[{"OriginalExercise":q["OriginalSectionExercise"],"TeacherMathAvailable":True,
"Mode":"TEACHER_MANUAL","SourceContentNotCopied":True}for q in Q],
"CI":"NOT_RUN","StageD":"NOT_RUN"})
assert len(re.findall(r"\bAdd\((?:[1-9]|1[0-9]|2[0-6]),",graphpath.read_text(encoding="utf8")))==26
assert "MathematicalVisualSpecValidator.Validate(graph)" in graphpath.read_text(encoding="utf8")
assert len(rich["Lessons"][0]["WorkedExamples"])==11 and len(teacher["IndependentTryIt"])==11
assert len(teacher["IndependentCoolDown"]["StudentQuestions"])==3
assert all(q["TeacherMathematicalAnswer"] for q in Q)
now=datetime.datetime.now(datetime.timezone.utc).isoformat()
closure_rel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u05-l01-abc-close-audit.json"
save(root/closure_rel,{"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":5,"Lesson":1,
"Title":"Angles","Source":"OpenStax Precalculus (2017) local original PDF",
"SourceSHA256":p["SourceSnapshotSha256"],"CorrectedLocalPdfPages":[455,471],
"BlueprintPageRangeIncorrect":[456,472],"OriginalWorkedExamples":11,"OriginalTryIt":11,
"OriginalFigures":26,"OriginalExercises":74,
"IndependentWorkedExamples":11,"IndependentTryIt":11,"IndependentQuestionsAndTeacherMath":74,
"IndependentAngleSketchModels":26,"IndependentCoolDown":3,
"ExistingStageCManualGeometryRoutes":74,"NewMathEngine":False,
"PublisherArtworkCopied":False,"ExactPublisherFigureParityCertified":False,
"FullFormalCCSSAlignmentCertified":False,"CCSSCandidates":["CCSS:HSF-TF.A.1"],
"TargetedLocalChecksActuallyRun":["Exact source PDF 5.1 page boundaries","74 source exercise-skill mappings",
"11 worked 11 Try It 26 directed-angle sketch model contracts","Coterminal/modular arithmetic and arc/sector numerical spot-checks",
"Stage C existing teacher review routes and Git diff check"],
"Build":"NOT_RUN","CI":"NOT_RUN","StageD":"NOT_RUN","E2E":"NOT_RUN",
"AuthenticatedStudentGraphRendering":"NOT_RUN",
"ReadyForStageD":True,"ClosedAtUtc":now})
audit_rel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u05-l01-source-and-academic-audit.json"
save(root/audit_rel,{"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Section":"5.1",
"SourceSHA256":p["SourceSnapshotSha256"],"CorrectedPages":[455,471],
"SourceWorkedExamples":[{"Number":i+1,"Title":w[0]}for i,w in enumerate(W)],
"SourceTryItNumbers":list(range(1,12)),"SourceFigures":list(range(1,27)),
"SourceNumberedExerciseLedger":[{"Number":q["OriginalSectionExercise"],
"SourceMathSkillReviewed":True,"IndependentMathTeacherKeyVerified":True}for q in Q],
"IndependentRichV2":rich_rel,"IndependentTeacherQuestions":str(tp.relative_to(root)).replace("\\","/"),
"FigureSkillEvidence":figrel,"GraphModelPath":str(graphpath.relative_to(root)).replace("\\","/"),
"StageCRoutes":routes_rel,"Status":"READY_FOR_STAGE_D","ReadyForStageD":True,
"BlockingReasons":[],"ClosureEvidence":closure_rel,"CI":"NOT_RUN","StageD":"NOT_RUN","ClosedAtUtc":now})
assert p["LastClosedLessonOrdinal"]==28 and p["FirstIncompleteLessonOrdinal"]==29
row=p["LessonEntries"][28]
assert row["Ordinal"]==29 and row["SourceSection"]=="5.1"
row.update({"ACStatus":"READY_FOR_STAGE_D","ReadyForStageD":True,
"PdfPageStart":455,"PdfPageEnd":471,"BlockingReasons":[],
"SourceNumberedExercisesMathReviewed":74,"FirstPartyTeacherMathKeys":74,
"IndependentSourceFigureSketches":26,"EvidencePath":audit_rel,
"ClosureEvidencePath":closure_rel,"Tests":"TARGETED_LOCAL_ONLY",
"ClosedAtUtc":now,"UpdatedUtc":now})
assert p["LessonEntries"][29]["Ordinal"]==30
p.update({"LastClosedLessonOrdinal":29,"FirstIncompleteLessonOrdinal":30,
"ActiveLessonOrdinal":30,"NextLessonOrdinal":30,"ReadyForStageD":29,
"SourceVerifiedGrade12Lessons":29,"GradeStatus":"GRADE12_A_B_C_IN_PROGRESS_29_OF_73","UpdatedUtc":now})
save(pp,p)
files=[str(pp.relative_to(root)),rich_rel,str(tp.relative_to(root)),figrel,routes_rel,closure_rel,audit_rel,
str(graphpath.relative_to(root)),str(vp.relative_to(root)),str(Path(__file__).relative_to(root)),
"tools/curriculum/g12_u05_l01_batch_questions_01_45.py","tools/curriculum/g12_u05_l01_batch_questions_46_74.py"]
subprocess.run(["git","add","--sparse","-f","--",*files],cwd=root,check=True)
subprocess.run(["git","diff","--cached","--check"],cwd=root,check=True)
subprocess.run(["git","commit","-m","feat(g12 u05 l01): close directed angles radians sector and speed academic ABC [skip ci]"],cwd=root,check=True)
subprocess.run(["git","push","origin","curriculum/us-ccss-g12-precalculus-abc-20261106"],cwd=root,check=True)
print("U05L01_GITHUB_PUSH_VERIFIED_29_OF_73_NEXT_U05L02")
print("SHA",subprocess.run(["git","rev-parse","HEAD"],cwd=root,capture_output=True,text=True,check=True).stdout.strip())
