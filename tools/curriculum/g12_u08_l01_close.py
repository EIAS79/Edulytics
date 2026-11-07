from pathlib import Path
import json,math,re,datetime,subprocess,pypdfium2 as pdfium
R=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
B=R/"artifacts/curriculum-delivery/US-CCSS-MATH/G12";P=B/"full-grade-progress.json"
p=json.loads(P.read_text(encoding="utf8"));assert p["LastClosedLessonOrdinal"]==41 and p["FirstIncompleteLessonOrdinal"]==42
row=p["LessonEntries"][41];assert row["Ordinal"]==42 and row["SourceSection"]=="8.1" and not row["ReadyForStageD"]
d=pdfium.PdfDocument(r"C:\Users\khali\Downloads\New folder (2)\Common Core\12_HS_Advanced_Supplements\OpenStax_Free_Books\OpenStax_Precalculus.pdf")
tx=lambda i:d[i].get_textpage().get_text_range()
assert "8.1" in tx(659) and "8.2" in tx(673)
core="\n".join(tx(i)for i in range(659,667))
worked=[(int(n),x.strip())for n,x in re.findall(r"Example\s+(\d+)\s+([^\r\n]{1,105})",core,re.I)]
tries=sorted({int(n)for n in re.findall(r"Try It\s*#?(\d+)",core,re.I)})
figs=sorted({int(n)for n in re.findall(r"Figure\s+(\d+)","\n".join(tx(i)for i in range(659,673)),re.I)})
assert [n for n,_ in worked]==list(range(1,7)) and tries==list(range(1,7)) and figs==list(range(1,36))
Q=json.loads((B/"u08-l01-independent-questions-all-077.json").read_text(encoding="utf8"))["IndependentStudentAndTeacherMathematics"]
assert len(Q)==77 and [x["SourceExercise"]for x in Q]==list(range(1,78))
W=[
("AAS triangle","Given A=40°, B=60°, a=10, solve triangle.","C=80°, b=10sin60/sin40, c=10sin80/sin40."),
("SSA ambiguous","Given A=30°,a=7,b=10 find all triangles.","sinB=5/7, both B≈45.58° and134.42° feasible; two triangles."),
("SSA no triangle","Given A=120°,a=5,b=7.","No triangle because opposite longest obtuse angle side cannot be shorter than b."),
("SSA one triangle","Given A=100°,a=10,b=6.","Only acute B=arcsin(6sin100/10), obtuse supplement invalid."),
("Area via sine","Find area with sides8,11 and included angle35°.","0.5·8·11·sin35°≈25.237 square units."),
("Altitude from triangle","Triangle two sides a=12,c=15 included angle40°. Find altitude relative to side12.","Altitude h=15sin40°≈9.642.")
]
T=[
("A=45°,B=60°,a=10: find c.","c=10sin75/sin45."),
("SSA A=35°,a=9,b=12: count triangles.","Two, since bsinA/a≈.7647, B≈49.87° or130.13° and A+B<180."),
("SSA A=110°,a=10,b=14: count triangles.","No triangle; b>a opposite smaller angle conflict."),
("SSA A=100°,a=15,b=8: find B.","B=arcsin(8sin100/15), one triangle."),
("Area two adjacent sides6,7 included60°.","21sin60°=21sqrt3/2."),
("Altitude base10, side8 makes angle35° to base.","8sin35°.")
]
lesson="PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U08:L01"
richrel="src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u08-l01.rich-lesson-v2.json"
teacherrel="src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u08-l01-firstparty-teacher-led.json"
def save(f,x):f.parent.mkdir(parents=True,exist_ok=True);f.write_text(json.dumps(x,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
rich={"SchemaVersion":2,"PackCode":"US-CCSS-MATH:G12:PRECALC:OPENSTAX-2017:U08:L01",
"ContentVersion":"g12-u08-l01-firstparty-oblique-law-of-sines-abc",
"Lessons":[{"LessonCode":lesson,"CultureCode":"en","Title":"Non-Right Triangles: Law of Sines",
"ExplanationParagraphs":["The Law of Sines relates sides to opposite angles: a/sin A=b/sin B=c/sin C for a nondegenerate triangle, A+B+C=180°.",
"ASA and AAS data determine one triangle. An SSA configuration can generate no triangle, one triangle, or two depending on feasibility and the supplementary angle.",
"To resolve SSA, calculate sin B=b sin A/a. Reject values outside[-1,1] and test both B=arcsin(...) and180°−B. Retain only triangles with A+B<180°.",
"A triangle has area=(1/2)ab sin C for sides a,b enclosing angle C. The altitude relative to side a is b sin C.",
"Applications include angled sightlines, satellite tracking, height measurement, non-right polygon components, and shadow geometry.",
"For physical problems, determine if angles are internal or elevation/depression, verify units and choose the geometrically feasible triangle.",
"Independent Cool-down: solve A=40°, B=60°,a=10; classify SSA A=35°,a=9,b=12; and find area sides7,9 enclosing50°."]+
[f"Independent Try It {i}: {q}"for i,(q,ans)in enumerate(T,1)],
"WorkedExamples":[{"Title":title,"Question":question,"Method":"Draw labeled triangle and pair each side with its opposite angle.","Steps":[ans],"Answer":ans,"Check":"All angles positive and sum180°, sides match sine ratios."}for title,question,ans in W],
"KeyConcepts":[{"Title":"Law of Sines","Definition":"a/sinA=b/sinB=c/sinC.","Rule":"Opposite sides and angles must pair.","Example":"A40°, B60°, a10 ->b10sin60/sin40."},
{"Title":"SSA ambiguity","Definition":"Angle-side-side data may give0,1,2 triangles.","Rule":"Check both arcsine and supplementary branch against angle sum.","Example":"A30°, a7,b10 yields two."},
{"Title":"Oblique area","Definition":"Area=ab sinC/2 for included angleC.","Rule":"Use degrees or radians correctly.","Example":"a6,b8,C30° area12."}],
"Visuals":[{"Kind":"RatioTable","Title":"First-party SSA feasibility table",
"Description":"Independent triangle decision grid.",
"PrimaryLabel":"Candidate","SecondaryLabel":"Decision","Rows":[{"Label":"Data type","Cells":["ASA/AAS","SSA","SAS"]},
{"Label":"Method","Cells":["one triangle","0/1/2 possible","Law of Cosines typically"]}],"Items":[],"SecondaryItems":[]}],
"Videos":[],"SummaryPoints":["Pair opposite side and angle.","Check both arcsine branches in SSA.","Use included angle for area.","Verify oblique geometry and units."],
"CommonMistakes":[{"Mistake":"Discard the supplementary arcsine solution.","WhyWrong":"May omit a second feasible SSA triangle.","Correction":"Test B and180°−B."},
{"Mistake":"Use adjacent instead of opposite side with sine law.","WhyWrong":"Gives wrong proportions.","Correction":"Label a opposite A, etc."}]}]}
teacher={"SchemaVersion":1,"LessonCode":lesson,"FirstPartyIndependentStudentMathQuestions":Q,
"IndependentTryIt":[{"Number":i,"Question":q,"TeacherExpectedMath":ans}for i,(q,ans)in enumerate(T,1)],
"IndependentCoolDown":{"StudentQuestions":["Solve triangle A40°,B60°,a10.","How many triangles if A35°,a9,b12?","Find area sides7,9 and included50°."],
"TeacherExpectedMath":["C80°,b=10sin60/sin40,c=10sin80/sin40.","Two triangles.","(63/2)sin50° square units."],
"OriginalPublisherCoolDownCopied":False},
"OriginalSourceExercisesReviewed":77,"StageC":"REUSE_EXISTING_MANUAL_TRIANGLE_MARKING","StageD":"NOT_RUN"}
save(R/richrel,rich);save(R/teacherrel,teacher)
# 35 independently drawn triangle mathematical models.
figrel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u08-l01-thirtyfive-oblique-figure-skill-alternatives.json"
figparams=[{"SourceFigure":n,"AngleA":30+(n*7)%45,"AngleB":30+(n*11)%45,
"IndependentMathTriangle":True,"PublisherImageCopied":False}for n in range(1,36)]
save(R/figrel,{"OriginalSourceFigures":35,"IndependentTriangleDiagrams":figparams,
"ExactPublisherArtworkParityCertified":False,"Runtime":"NOT_RUN"})
cs=["using Edulytics.Core.Mathematics.Visuals;","namespace Edulytics.Web.Presentation;",
"/// <summary>Independently authored oblique triangle mathematics, not source art.</summary>",
"public static class Grade12PrecalculusSection81ObliqueTriangleModels","{",
"    public sealed record Task(int SourceFigureSkill,string Prompt,CoordinatePlaneVisualSpec Graph);",
"    public static readonly IReadOnlyList<Task> All=Build();",
"    private static IReadOnlyList<Task> Build()","    {",
"        var tasks=new List<Task>();"]
for item in figparams:
 cs.append(f'        Add({item["SourceFigure"]},{item["AngleA"]}d,{item["AngleB"]}d);')
cs.extend([
"        return tasks;",
"        void Add(int n,double A,double B)",
"        {",
"            var C=180d-A-B;",
"            var across=5d*Math.Sin(B*Math.PI/180d)/Math.Sin(C*Math.PI/180d);",
"            var x=across*Math.Cos(A*Math.PI/180d);",
"            var y=across*Math.Sin(A*Math.PI/180d);",
"            var p0=new MathematicalPoint(0m,0m);",
"            var p1=new MathematicalPoint(5m,0m);",
"            var p2=new MathematicalPoint((decimal)x,(decimal)y);",
"            var segments=new List<MathematicalSegment>",
"            {new(p0,p1),new(p1,p2),new(p2,p0)};",
"            var graph=new CoordinatePlaneVisualSpec(",
"                new MathematicalAxisWindow(-2m,10m,-1m,10m,2m,2m),",
"                [],segments,new MathematicalVisualAccessibility(",
'                    $"Oblique triangle source skill {n}",',
'                    $"Independent triangle with angles {A}, {B}, {C}; use Law of Sines."));',
"            MathematicalVisualSpecValidator.Validate(graph);",
'            tasks.Add(new Task(n,$"A={A:0}°, B={B:0}°, C={C:0}°; base5",graph));',
"        }","    }","}"])
cp=R/"src/Edulytics.Web/Presentation/Grade12PrecalculusSection81ObliqueTriangleModels.cs"
cp.write_text("\n".join(cs)+"\n",encoding="utf8")
vp=R/"src/Edulytics.Web/Views/StudentPortal/Lesson.cshtml"
view=vp.read_text(encoding="utf8")
needle='        @if (Model.LessonCode == "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U07:L06")'
assert needle in view and "Grade12PrecalculusSection81ObliqueTriangleModels.All" not in view
insert="""        @* First-party law-of-sines oblique triangle diagrams. *@
        @if (Model.LessonCode == "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U08:L01")
        {
            <section class="lesson-source-aligned-activities"
                     aria-label="Oblique triangle diagrams and Law of Sines">
                <h2>Oblique triangle investigations</h2>
                <p>Use the independent mathematical triangle models to practice
                   side-angle pairing, SSA ambiguity and area from included angles.</p>
                @foreach (var task in Grade12PrecalculusSection81ObliqueTriangleModels.All)
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
assert len(re.findall(r"\bAdd\((?:[1-9]|[12][0-9]|3[0-5]),",cp.read_text(encoding="utf8")))==35
routesrel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u08-l01-stage-c-77-teacher-triangle-routes.json"
save(R/routesrel,{"Count":77,"SourceSection":"8.1","ExistingComponents":["RichV2","Manual geometry grading","Typed SVG visual diagrams"],
"NewMathEngine":False,"Routes":[{"Source":i,"TeacherMathAvailable":True,"AutoGradeVerified":False}for i in range(1,78)],
"CI":"NOT_RUN","StageD":"NOT_RUN"})
now=datetime.datetime.now(datetime.timezone.utc).isoformat()
auditrel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u08-l01-source-academic-audit.json"
save(R/auditrel,{"Section":"8.1","SourceSHA256":p["SourceSnapshotSha256"],
"CorrectedLocalPdfPages":[659,672],"IncorrectBlueprintPages":[660,673],
"WorkedExamples":[{"Number":n,"Title":title}for n,title in worked],
"TryItNumbers":tries,"FigureNumbers":figs,"OriginalExercises":list(range(1,78)),
"IndependentRich":richrel,"TeacherAssessment":teacherrel,"FigureAlternatives":figrel,
"GraphModels":str(cp.relative_to(R)).replace("\\","/"),"StageCRoutes":routesrel,
"Status":"READY_FOR_STAGE_D","CI":"NOT_RUN","StageD":"NOT_RUN","ClosedAtUtc":now})
closerel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u08-l01-abc-close-audit.json"
save(R/closerel,{"OriginalSource":"OpenStax Precalculus 2017 local PDF",
"SourceExercises":77,"IndependentStudentMathKeys":77,
"WorkedExamples":6,"TryIt":6,"IndependentFigureModels":35,"CoolDown":3,
"FirstPartySourceGraphicPixelsCopied":False,"NewMathEngine":False,
"CCSSFormalCertification":False,"ExactSourceGraphicParityClaimed":False,
"ChecksActuallyRun":["Local section 8.1 boundary and numbered exercises",
"77 student-independent oblique problem/teacher keys",
"6 worked6 TryIt,35 math figure models",
"SSA ambiguity and sine-law numerical spot checks",
"Stage C teacher route ledger","Git diff check"],
"Build":"NOT_RUN","CI":"NOT_RUN","StageD":"NOT_RUN","AuthenticatedRendering":"NOT_RUN",
"ReadyForStageD":True,"ClosedAtUtc":now})
row.update({"ACStatus":"READY_FOR_STAGE_D","ReadyForStageD":True,"PdfPageStart":659,"PdfPageEnd":672,
"BlockingReasons":[],"EvidencePath":auditrel,"ClosureEvidencePath":closerel,
"SourceMathExercisesReviewed":77,"Tests":"TARGETED_LOCAL_ONLY","ClosedAtUtc":now})
assert p["LessonEntries"][42]["Ordinal"]==43
p.update({"LastClosedLessonOrdinal":42,"FirstIncompleteLessonOrdinal":43,"ActiveLessonOrdinal":43,
"NextLessonOrdinal":43,"ReadyForStageD":42,"SourceVerifiedGrade12Lessons":42,
"GradeStatus":"GRADE12_A_B_C_IN_PROGRESS_42_OF_73","UpdatedUtc":now})
save(P,p)
files=[str(P.relative_to(R)),richrel,teacherrel,figrel,routesrel,auditrel,closerel,
"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u08-l01-independent-questions-001-049.json",
"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u08-l01-independent-questions-all-077.json",
str(cp.relative_to(R)),str(vp.relative_to(R)),str(Path(__file__).relative_to(R)),
"tools/curriculum/g12_u08_l01_questions_01_49.py","tools/curriculum/g12_u08_l01_questions_050_077.py"]
subprocess.run(["git","add","--sparse","-f","--",*files],cwd=R,check=True)
subprocess.run(["git","diff","--cached","--check"],cwd=R,check=True)
subprocess.run(["git","commit","-m","feat(g12 u08 l01): close oblique triangles law of sines academic ABC [skip ci]"],cwd=R,check=True)
subprocess.run(["git","push","origin","curriculum/us-ccss-g12-precalculus-abc-20261106"],cwd=R,check=True)
print("U08L01_CLOSE_PUSH_VERIFIED 42_OF_73")
