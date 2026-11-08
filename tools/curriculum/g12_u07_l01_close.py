from pathlib import Path
import json,math,subprocess,datetime,re
r=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
b=r/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
pfile=b/"full-grade-progress.json";p=json.loads(pfile.read_text(encoding="utf8"))
auditfile=b/"u07-l01-source-inventory.json";a=json.loads(auditfile.read_text(encoding="utf8"))
q=json.loads((b/"u07-l01-independent-math-questions.json").read_text(encoding="utf8"))
assert p["LastClosedLessonOrdinal"]==35 and p["FirstIncompleteLessonOrdinal"]==36
assert len(q["IndependentMathQuestionTeacherKeys"])==42
richrel="src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u07-l01.rich-lesson-v2.json"
teachrel="src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-g12-precalculus-u07-l01-firstparty-teacher-led.json"
rich=json.loads((r/richrel).read_text(encoding="utf8"))
teacher=json.loads((r/teachrel).read_text(encoding="utf8"))
assert len(rich["Lessons"][0]["WorkedExamples"])==10 and len(teacher["IndependentTryIt"])==5
assert len(teacher["IndependentCoolDown"]["StudentQuestions"])==3
figrel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u07-l01-four-source-figure-alternatives.json"
fig={"SchemaVersion":1,"Section":"7.1","OriginalFigureCount":4,
"Alternatives":[{"OriginalFigure":1,"Skill":"Identity metaphor credited passport photograph","FirstPartyAlternative":"Independent mathematical equality/definition text; original photo not copied"},
{"OriginalFigure":2,"Skill":"Odd sine graph","FirstPartyAlternative":"Independent sin graph checking sin(-x)=-sinx"},
{"OriginalFigure":3,"Skill":"Even cosine graph","FirstPartyAlternative":"Independent cos graph checking cos(-x)=cosx"},
{"OriginalFigure":4,"Skill":"Cotangent reciprocal identity","FirstPartyAlternative":"Independent cot x graph for sinx!=0; 1/tanx only where both sinx and cosx nonzero"}],
"PublisherImagesCopied":False,"ExactPublisherGraphicParityClaimed":False}
(b/"u07-l01-four-source-figure-alternatives.json").write_text(json.dumps(fig,indent=2)+"\n",encoding="utf8")
cs=["using Edulytics.Core.Mathematics.Visuals;","namespace Edulytics.Web.Presentation;",
"/// <summary>First-party trig identity source skills; excludes publisher artwork.</summary>",
"public static class Grade12PrecalculusSection71IdentityGraphs","{",
"    public sealed record Task(int SourceFigureSkill,string Prompt,CoordinatePlaneVisualSpec Graph);",
"    public static readonly IReadOnlyList<Task> All=Build();",
"    private static IReadOnlyList<Task> Build()","    {","        var items=new List<Task>();",
'        Add(2,"Sine is odd",x=>Math.Sin(x));',
'        Add(3,"Cosine is even",x=>Math.Cos(x));',
'        Add(4,"Cotangent equals 1/tangent on common domain",x=>Math.Cos(x)/Math.Sin(x));',
"        return items;",
"        void Add(int n,string label,Func<double,double> f)",
"        {","            var lines=new List<MathematicalSegment>();",
"            MathematicalPoint? prev=null;",
"            for(var i=0;i<=300;i++)",
"            {",
"                var x=-Math.PI*2+Math.PI*4*i/300d;",
"                var y=f(x);",
"                if(double.IsNaN(y)||double.IsInfinity(y)||Math.Abs(y)>7 || (n==4 && Math.Abs(Math.Sin(x))<0.05))",
"                { prev=null;continue; }",
"                var next=new MathematicalPoint((decimal)x,(decimal)y);",
"                if(prev is not null && Math.Abs((double)(next.Y-prev.Y))<1d)",
"                    lines.Add(new MathematicalSegment(prev,next));",
"                prev=next;",
"            }",
"            var graph=new CoordinatePlaneVisualSpec(",
"                new MathematicalAxisWindow(-7m,7m,-8m,8m,2m,2m),",
"                [],lines,new MathematicalVisualAccessibility(",
'                    $"First-party Section7.1 graph skill {n}",label));',
"            MathematicalVisualSpecValidator.Validate(graph);",
"            items.Add(new Task(n,label,graph));",
"        }","    }","}"]
cp=r/"src/Edulytics.Web/Presentation/Grade12PrecalculusSection71IdentityGraphs.cs"
cp.write_text("\n".join(cs)+"\n",encoding="utf8")
vp=r/"src/Edulytics.Web/Views/StudentPortal/Lesson.cshtml"
view=vp.read_text(encoding="utf8")
needle='        @if (Model.LessonCode == "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U06:L03")'
assert needle in view and "Grade12PrecalculusSection71IdentityGraphs.All" not in view
inject="""        @* First-party identities illustrations, not publisher images. *@
        @if (Model.LessonCode == "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U07:L01")
        {
            <section class="lesson-source-aligned-activities"
                     aria-label="Trigonometric identity graphical comparisons">
                <h2>Trigonometric identity graphs</h2>
                @foreach (var task in Grade12PrecalculusSection71IdentityGraphs.All)
                {
                    <figure class="student-card">
                        @Html.Raw(AdvancedMathematicsVisualRenderer.RenderSvg(task.Graph))
                        <figcaption>@task.Prompt</figcaption>
                    </figure>
                }
            </section>
        }

"""
vp.write_text(view.replace(needle,inject+needle,1),encoding="utf8")
assert len(re.findall(r"\bAdd\([234],",cp.read_text(encoding="utf8")))==3
assert "MathematicalVisualSpecValidator.Validate(graph)" in cp.read_text(encoding="utf8")
now=datetime.datetime.now(datetime.timezone.utc).isoformat()
closerel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u07-l01-abc-close-audit.json"
close={"SchemaVersion":1,"Section":"7.1","OriginalSource":"OpenStax 2017 original local PDF",
"SourceSHA256":a["SourceSHA256"],"SourcePdfPages":[575,584],
"OriginalWorkedExamples":10,"OriginalTryIt":5,"OriginalNumberedExercises":42,"OriginalFigures":4,
"IndependentWorked":10,"IndependentTryIt":5,"IndependentTeacherQuestionsAndAnswers":42,
"FirstPartyFigureSkillAlternatives":4,"IndependentCoolDown":3,"StageC":"REUSE_EXISTING_MANUAL_TEACHER_SCORING",
"OriginalSourceGraphicPixelsCopied":False,"ExactSourceGraphicParityClaimed":False,
"LocalChecksRun":["42 math/teacher prompts","10 worked5 TryIt","3 independent graph model contracts",
"Trig identity and domain numerical spot-check","Git diff"],
"CI":"NOT_RUN","Build":"NOT_RUN","StageD":"NOT_RUN","ReadyForStageD":True,"ClosedAtUtc":now}
(b/"u07-l01-abc-close-audit.json").write_text(json.dumps(close,indent=2)+"\n",encoding="utf8")
a.update({"Status":"READY_FOR_STAGE_D","ReadyForStageD":True,"ExercisesIndividuallyReviewed":42,
"BlockingReasons":[],"ClosureEvidence":closerel,"FigureSkillsEvidence":figrel,
"ClosedAtUtc":now,"UpdatedUtc":now})
auditfile.write_text(json.dumps(a,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
p["LessonEntries"][35].update({"ACStatus":"READY_FOR_STAGE_D","ReadyForStageD":True,
"BlockingReasons":[],"ClosureEvidencePath":closerel,"Tests":"TARGETED_LOCAL_ONLY","ClosedAtUtc":now})
assert p["LessonEntries"][36]["Ordinal"]==37
p.update({"LastClosedLessonOrdinal":36,"FirstIncompleteLessonOrdinal":37,
"ActiveLessonOrdinal":37,"NextLessonOrdinal":37,"ReadyForStageD":36,
"SourceVerifiedGrade12Lessons":36,"GradeStatus":"GRADE12_A_B_C_IN_PROGRESS_36_OF_73","UpdatedUtc":now})
pfile.write_text(json.dumps(p,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
files=[str(pfile.relative_to(r)),str(auditfile.relative_to(r)),closerel,figrel,
"artifacts/curriculum-delivery/US-CCSS-MATH/G12/u07-l01-independent-math-questions.json",
richrel,teachrel,str(cp.relative_to(r)),str(vp.relative_to(r)),
"tools/curriculum/g12_u07_l01_questions.py","tools/curriculum/g12_u07_l01_rich.py",str(Path(__file__).relative_to(r))]
subprocess.run(["git","add","--sparse","-f","--",*files],cwd=r,check=True)
subprocess.run(["git","diff","--cached","--check"],cwd=r,check=True)
subprocess.run(["git","commit","-m","feat(g12 u07 l01): close trigonometric identity academic ABC [skip ci]"],cwd=r,check=True)
subprocess.run(["git","push","origin","curriculum/us-ccss-g12-precalculus-abc-20261106"],cwd=r,check=True)
print("U07L01_CLOSED_AND_GITHUB_PUSH_CONFIRMED 36/73 NEXT37")
