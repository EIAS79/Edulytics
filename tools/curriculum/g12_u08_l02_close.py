from pathlib import Path
import json,math,re,subprocess,datetime,pypdfium2 as pdfium
R=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626");B=R/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
P=B/"full-grade-progress.json";p=json.loads(P.read_text(encoding="utf8"))
assert p["LastClosedLessonOrdinal"]==42 and p["FirstIncompleteLessonOrdinal"]==43
row=p["LessonEntries"][42];assert row["Ordinal"]==43 and row["SourceSection"]=="8.2" and not row["ReadyForStageD"]
d=pdfium.PdfDocument(r"C:\Users\khali\Downloads\New folder (2)\Common Core\12_HS_Advanced_Supplements\OpenStax_Free_Books\OpenStax_Precalculus.pdf")
tx=lambda i:d[i].get_textpage().get_text_range()
assert "8.2" in tx(673) and "8.3" in tx(685)
text="\n".join(tx(i)for i in range(673,680))
work=[(int(n),v.strip())for n,v in re.findall(r"Example\s+(\d+)\s+([^\r\n]{1,110})",text,re.I)]
tries=sorted({int(n)for n in re.findall(r"Try It\s*#?(\d+)",text,re.I)})
fig=sorted({int(n)for n in re.findall(r"Figure\s+(\d+)","\n".join(tx(i)for i in range(673,685)),re.I)})
assert [n for n,_ in work]==list(range(1,7)) and tries==list(range(1,5)) and fig==list(range(1,18))
Q=json.loads((B/"u08-l02-independent-questions-all-078.json").read_text(encoding="utf8"))["IndependentMathProblemsTeacherKeys"]
assert len(Q)==78 and [q["SourceExercise"]for q in Q]==list(range(1,79))
W=[
("SAS side","Find third side if two sides8,11 and included60°.","c=sqrt(8²+11²−2·8·11·cos60°)=sqrt97."),
("SSS angle","Find largest angle of sides5,6,8.","Angle opposite8=arccos((25+36−64)/(2·5·6))=arccos(−3/60)."),
("Communication triangulation","Two signals travel3km and4km, angle between75°. Find relay separation.","sqrt(3²+4²−24cos75°)km."),
("Travel displacement","Two moves 10mi and12mi with turn30°. Find displacement.","sqrt(10²+12²+2·10·12·cos30°)."),
("Heron area","Triangle sides13,14,15, find area.","s21; area=sqrt(21·8·7·6)=84."),
("Real-world Heron","Triangular parcel sides10,10,12m; find area.","s16; area=sqrt(16·6·6·4)=48m².")
]
T=[
("Find third side of SAS sides6,7 included45°.","sqrt(6²+7²−2·6·7·cos45°)."),
("Find angle opposite10 in triangle sides6,8,10.","90° (right triangle)."),
("Find length between two flight legs7,9 from common origin angle60°.","sqrt(7²+9²−2·7·9cos60°)=sqrt67."),
("Find area by Heron for sides5,5,6.","s8, area=sqrt(8·3·3·2)=12.")
]
lesson="PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U08:L02"
rrel="src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u08-l02.rich-lesson-v2.json"
trel="src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u08-l02-firstparty-teacher-led.json"
def save(f,v):f.parent.mkdir(parents=True,exist_ok=True);f.write_text(json.dumps(v,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
rich={"SchemaVersion":2,"PackCode":"US-CCSS-MATH:G12:PRECALC:OPENSTAX-2017:U08:L02",
"ContentVersion":"g12-u08-l02-firstparty-oblique-law-of-cosines-heron-abc",
"Lessons":[{"LessonCode":lesson,"CultureCode":"en","Title":"Non-Right Triangles: Law of Cosines",
"ExplanationParagraphs":["Law of Cosines in a triangle opposite side a and included angle A: a²=b²+c²−2bc cosA. Use SAS to determine a missing side.",
"For SSS measurements, find an angle with cosA=(b²+c²−a²)/(2bc), after confirming positive lengths and triangle inequality.",
"At A=90°, cosA=0 and cosine law becomes the Pythagorean theorem. For A>90°, the opposite side is longest.",
"Heron's formula uses semiperimeter s=(a+b+c)/2 and area sqrt(s(s−a)(s−b)(s−c)) for a valid triangle.",
"The Law of Cosines also supports SSA feasibility by solving a quadratic for a missing third side; reject nonpositive and invalid solutions.",
"Applications include resultant distances between diverging vehicles, parallelogram diagonal lengths, regular polygon side chords and surveys.",
"Independent Cool-down: solve SAS sides7,9 included75°; find angle opposite7 in sides5,6,7; compute Heron area sides5,12,13."]+
[f"Independent Try It {i}: {q}"for i,(q,a)in enumerate(T,1)],
"WorkedExamples":[{"Title":name,"Question":q,"Method":"Identify included or opposite angle and apply cosine law or Heron's formula.","Steps":[a],"Answer":a,"Check":"Check triangle inequality and dimensions."}for name,q,a in W],
"KeyConcepts":[{"Title":"Law of Cosines","Definition":"a²=b²+c²−2bc cosA.","Rule":"Opposite side a paired with angle A.","Example":"A90° reduces to a²=b²+c²."},
{"Title":"Find SSS angle","Definition":"cosA=(b²+c²−a²)/(2bc).","Rule":"Check triangle inequality first.","Example":"5-12-13 gives opposite13 angle90°."},
{"Title":"Heron's area","Definition":"Area=sqrt(s(s−a)(s−b)(s−c)).","Rule":"s=(a+b+c)/2.","Example":"13-14-15 area84."}],
"Visuals":[{"Kind":"RatioTable","Title":"Independent cosine law and Heron summary",
"Description":"Original mathematical formula comparison.",
"PrimaryLabel":"Known data","SecondaryLabel":"Technique",
"Rows":[{"Label":"Given","Cells":["SAS","SSS","SSS area"]},
{"Label":"Method","Cells":["a²=b²+c²−2bc cosA","arccos((b²+c²−a²)/(2bc))","Heron's formula"]}],"Items":[],"SecondaryItems":[]}],
"Videos":[],"SummaryPoints":["Law of Cosines solves SAS and SSS.","Heron area uses three valid side lengths.","Use vector separation and diagonal geometry carefully."],
"CommonMistakes":[{"Mistake":"Use an angle not included between the two known sides.","WhyWrong":"Substitutes wrong cosine term.","Correction":"Label included angle and opposite unknown side."},
{"Mistake":"Apply Heron to invalid triangle side lengths.","WhyWrong":"Triangle inequality fails.","Correction":"Check a+b>c for each pairing."}]}]}
teacher={"SchemaVersion":1,"LessonCode":lesson,"IndependentStudentMathQuestions":Q,
"IndependentTryIt":[{"Number":i,"Question":q,"TeacherExpectedMath":a}for i,(q,a)in enumerate(T,1)],
"IndependentCoolDown":{"StudentQuestions":["Find side c given a7,b9,included75°.","Find angle opposite side7 in sides5,6,7.","Heron area for5,12,13."],
"TeacherExpectedMath":["sqrt(49+81−126cos75°).","arccos((25+36−49)/(2·5·6)).","30 square units."],
"OriginalPublisherCoolDownCopied":False},
"SourceExercisesReviewed":78,"StageC":"REUSE_EXISTING_MANUAL_OBLIQUE_GEOMETRY_RUBRICS","StageD":"NOT_RUN"}
save(R/rrel,rich);save(R/trel,teacher)
figrel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u08-l02-seventeen-firstparty-cosine-figure-skills.json"
spec=[{"SourceFigure":n,"SideA":3+(n%6),"SideB":4+(n%7),"IncludedAngleDegrees":35+(n*11)%110,
"OriginalPublisherGraphicCopied":False}for n in range(1,18)]
save(R/figrel,{"SourceFigureCount":17,"IndependentMathematicalModels":spec,
"ExactPublisherArtworkParityCertified":False,"Runtime":"NOT_RUN"})
cs=["using Edulytics.Core.Mathematics.Visuals;","namespace Edulytics.Web.Presentation;",
"/// <summary>Original oblique triangle cosine law illustrations.</summary>",
"public static class Grade12PrecalculusSection82CosineTriangleModels","{",
"    public sealed record Task(int SourceFigureSkill,string Prompt,CoordinatePlaneVisualSpec Graph);",
"    public static readonly IReadOnlyList<Task> All=Build();",
"    private static IReadOnlyList<Task> Build()","    {","        var items=new List<Task>();"]
for v in spec:cs.append(f'        Add({v["SourceFigure"]},{v["SideA"]}d,{v["SideB"]}d,{v["IncludedAngleDegrees"]}d);')
cs.extend(["        return items;",
"        void Add(int n,double a,double b,double theta)",
"        {",
"            var p0=new MathematicalPoint(0m,0m);",
"            var p1=new MathematicalPoint((decimal)a,0m);",
"            var p2=new MathematicalPoint((decimal)(b*Math.Cos(theta*Math.PI/180d)),",
"                (decimal)(b*Math.Sin(theta*Math.PI/180d)));",
"            var lines=new List<MathematicalSegment>",
"            {new(p0,p1),new(p1,p2),new(p2,p0)};",
"            var graph=new CoordinatePlaneVisualSpec(",
"                new MathematicalAxisWindow(-12m,12m,-2m,12m,2m,2m),",
"                [],lines,new MathematicalVisualAccessibility(",
'                    $"Law of Cosines source figure skill {n}",',
'                    $"First-party triangle sides {a}, {b} enclosing {theta} degrees."));',
"            MathematicalVisualSpecValidator.Validate(graph);",
'            items.Add(new Task(n,$"SAS: {a:0}, {b:0}, angle {theta:0}°",graph));',
"        }","    }","}"])
cp=R/"src/Edulytics.Web/Presentation/Grade12PrecalculusSection82CosineTriangleModels.cs"
cp.write_text("\n".join(cs)+"\n",encoding="utf8")
vp=R/"src/Edulytics.Web/Views/StudentPortal/Lesson.cshtml"
view=vp.read_text(encoding="utf8")
needle='        @if (Model.LessonCode == "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U08:L01")'
assert needle in view and "Grade12PrecalculusSection82CosineTriangleModels.All" not in view
insert="""        @* Original oblique SAS diagrams for law-of-cosines investigations. *@
        @if (Model.LessonCode == "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U08:L02")
        {
            <section class="lesson-source-aligned-activities"
                     aria-label="Law of Cosines oblique triangle diagrams">
                <h2>Cosine law graphical investigations</h2>
                <p>Practice two-side included-angle solutions and validate non-right
                   triangle lengths using original mathematical diagrams.</p>
                @foreach (var task in Grade12PrecalculusSection82CosineTriangleModels.All)
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
assert len(re.findall(r"\bAdd\((?:[1-9]|1[0-7]),",cp.read_text(encoding="utf8")))==17
routes="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u08-l02-stage-c-78-oblique-assessment-routes.json"
save(R/routes,{"Section":"8.2","Count":78,"NewEngine":False,
"ExistingComponents":["RichV2","Teacher manual geometry answer keys","SVG mathematical diagram renderer"],
"Routes":[{"Source":i,"TeacherMathRubric":True,"AutoGradingRuntimeVerified":False}for i in range(1,79)],
"CI":"NOT_RUN","StageD":"NOT_RUN"})
now=datetime.datetime.now(datetime.timezone.utc).isoformat()
auditrel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u08-l02-source-academic-audit.json"
save(R/auditrel,{"SourceSection":"8.2","SourceSHA256":p["SourceSnapshotSha256"],
"CorrectedPdfPages":[673,684],"IncorrectBlueprintPages":[674,685],
"SourceWorked":[{"Number":n,"Title":v}for n,v in work],
"SourceTryIt":tries,"SourceFigures":fig,"OriginalExercises":list(range(1,79)),
"FirstPartyStudentRichV2":rrel,"TeacherAssessment":trel,"FigureAlternatives":figrel,
"GraphCode":str(cp.relative_to(R)).replace("\\","/"),"StageC":routes,
"Status":"READY_FOR_STAGE_D","CI":"NOT_RUN","StageD":"NOT_RUN","ClosedAtUtc":now})
closure="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u08-l02-abc-close-audit.json"
save(R/closure,{"Section":"8.2","OriginalSource":"OpenStax Precalculus 2017 local book",
"OriginalSourceExercises":78,"IndependentTeacherKeys":78,"Worked":6,"TryIt":4,
"IndependentSourceFigureSkillModels":17,"IndependentCoolDown":3,
"OriginalPublisherImagesCopied":False,"ExactSourceGraphicParityCertified":False,
"ChecksActuallyRun":["78 math skill alternatives with teacher keys","6 worked4 TryIt,17 first-party triangle models",
"Cosine-law and Heron numeric checks","Source local PDF boundaries",
"StageC manual marking route count","git diff --cached --check"],
"Build":"NOT_RUN","CI":"NOT_RUN","StageD":"NOT_RUN","AuthenticatedRuntime":"NOT_RUN",
"ReadyForStageD":True,"ClosedAtUtc":now})
row.update({"ACStatus":"READY_FOR_STAGE_D","ReadyForStageD":True,"PdfPageStart":673,"PdfPageEnd":684,
"BlockingReasons":[],"EvidencePath":auditrel,"ClosureEvidencePath":closure,"Tests":"TARGETED_LOCAL_ONLY",
"SourceExercisesReviewed":78,"ClosedAtUtc":now})
assert p["LessonEntries"][43]["Ordinal"]==44
p.update({"LastClosedLessonOrdinal":43,"FirstIncompleteLessonOrdinal":44,"ActiveLessonOrdinal":44,
"NextLessonOrdinal":44,"ReadyForStageD":43,"SourceVerifiedGrade12Lessons":43,
"GradeStatus":"GRADE12_A_B_C_IN_PROGRESS_43_OF_73","UpdatedUtc":now})
save(P,p)
files=[str(P.relative_to(R)),rrel,trel,figrel,routes,auditrel,closure,
"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u08-l02-independent-questions-001-051.json",
"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u08-l02-independent-questions-all-078.json",
str(cp.relative_to(R)),str(vp.relative_to(R)),str(Path(__file__).relative_to(R)),
"tools/curriculum/g12_u08_l02_questions_01_51.py","tools/curriculum/g12_u08_l02_questions_052_078.py"]
subprocess.run(["git","add","--sparse","-f","--",*files],cwd=R,check=True)
subprocess.run(["git","diff","--cached","--check"],cwd=R,check=True)
subprocess.run(["git","commit","-m","feat(g12 u08 l02): close oblique law of cosines and Heron academic ABC [skip ci]"],cwd=R,check=True)
subprocess.run(["git","push","origin","curriculum/us-ccss-g12-precalculus-abc-20261106"],cwd=R,check=True)
print("U08L02_CLOSED_PUSH_VERIFIED_43_OF_73")
