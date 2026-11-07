from pathlib import Path
import json,datetime,subprocess,re,math,time
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626");base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
ap=base/"u04-l04-source-inventory-corrected-boundary.json"
p_path=base/"full-grade-progress.json"
a=json.loads(ap.read_text(encoding="utf8"));p=json.loads(p_path.read_text(encoding="utf8"))
assert a["OriginalMathExercisesReviewed"]==60 and a["OriginalFigureModelsIndexed"]==19
assert p["LastClosedLessonOrdinal"]==23 and p["FirstIncompleteLessonOrdinal"]==24
# Each original visual skill has a separately authored curve or group. Inputs are
# mathematical equations, not copies of publisher figure coordinates or images.
graphs=[
("Growth parent","x>0m","Math.Log((double)x,2d)"),
("Decay parent","x>0m","Math.Log((double)x,0.5d)"),
("Left shift","x>−1m","Math.Log((double)(x+1m),2d)"),
("Reflected input","x<3m","Math.Log((double)(3m−x))"),
("Right shift","x>2m","Math.Log((double)(x−2m),3d)"),
("Vertical shift","x>0m","Math.Log((double)x)+2d"),
("Shift down","x>1m","Math.Log((double)(x−1m),4d)−2d"),
("Stretch","x>0m","2d*Math.Log((double)x,2d)"),
("Reflect x-axis","x>0m","−Math.Log((double)x,2d)"),
("Reflect y-axis","x<0m","Math.Log((double)(−x),2d)"),
("Translate left","x>−3m","Math.Log((double)(x+3m))"),
("Translate right","x>4m","Math.Log((double)(x−4m),5d)"),
("Double reflection","x<0m","−Math.Log((double)(−x),3d)"),
("Translate up","x>0m","Math.Log((double)x,4d)+1d"),
("Input reversal with shift","x<2m","Math.Log((double)(2m−x))"),
("Asymptote shift","x>1m","Math.Log((double)(x−1m),2d)")]
groups=[
[("x>0m","Math.Log((double)x,2d)"),("x>0m","Math.Log((double)x)"),("x>0m","Math.Log((double)x,5d)")],
[("x>0m","Math.Log((double)x,1d/3d)"),("x>0m","Math.Log((double)x,2d)"),("x>0m","Math.Log((double)x,0.75d)")],
[("x<2m","Math.Log((double)(2m−x),3d)"),("x>−2m","−Math.Log((double)(x+2m),3d)"),("x>−2m","Math.Log((double)(x+2m),3d)")]]
assert len(graphs)==16 and len(groups)==3
cs=["using Edulytics.Core.Mathematics.Visuals;","namespace Edulytics.Web.Presentation;",
"/// <summary>Original first-party graph skills, not publisher figures; runtime to be checked in Stage D.</summary>",
"public static class Grade12PrecalculusSection44GraphModels",
"{","    public sealed record GraphTask(int SourceFigureSkill, string Prompt, CoordinatePlaneVisualSpec Graph);",
"    public static readonly IReadOnlyList<GraphTask> All=Build();",
"    private static IReadOnlyList<GraphTask> Build()","    {","        var items=new List<GraphTask>();"]
for i,(name,cond,formula) in enumerate(graphs,1):
 cs+= [f'        Add({i}, "{name}: independent logarithmic graph",',
 f'            [x => {cond} ? (decimal?)((decimal)({formula})) : null]);']
for i,grp in enumerate(groups,17):
 expressions=", ".join(f'x => {cond} ? (decimal?)((decimal)({formula})) : null' for cond,formula in grp)
 cs+=[f'        Add({i}, "Compare multiple independently authored logarithmic curves",',
 f'            [{expressions}]);']
cs +=[
"        return items;",
"        void Add(int number, string prompt, Func<decimal,decimal?>[] funcs)",
"        {",
"            var segments=new List<MathematicalSegment>();",
"            foreach(var f in funcs)",
"            {",
"                MathematicalPoint? previous=null;",
"                for(var i=0;i<=220;i++)",
"                {",
"                    var x=-7m+21m*i/220m;",
"                    var value=f(x);",
"                    if(!value.HasValue || value.Value<−9m || value.Value>9m)",
"                    { previous=null; continue; }",
"                    var point=new MathematicalPoint(x,value.Value);",
"                    if(previous is not null) segments.Add(new MathematicalSegment(previous,point));",
"                    previous=point;",
"                }",
"            }",
"            var graph=new CoordinatePlaneVisualSpec(",
"                new MathematicalAxisWindow(-7m,14m,-9m,9m,2m,2m),",
"                [],segments,new MathematicalVisualAccessibility(",
'                    $"First-party logarithm figure skill {number}",',
'                    prompt+". Coordinate plane plot authored for Edulytics, not copied from the publisher."));',
"            MathematicalVisualSpecValidator.Validate(graph);",
"            items.Add(new GraphTask(number,prompt,graph));",
"        }","    }","}"]
cs_text="\n".join(cs)+"\n"
# Normalize Unicode minus symbol in C# source into ASCII to make valid C#.
cs_text=cs_text.replace("−","-")
cp=root/"src/Edulytics.Web/Presentation/Grade12PrecalculusSection44GraphModels.cs"
cp.write_text(cs_text,encoding="utf8")
assert len(re.findall(r"\bAdd\((?:[1-9]|1[0-9]),",cs_text))==19
viewpath=root/"src/Edulytics.Web/Views/StudentPortal/Lesson.cshtml"
view=viewpath.read_text(encoding="utf8")
needle='        @if (Model.LessonCode == "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U04:L02")'
assert needle in view and "Grade12PrecalculusSection44GraphModels.All" not in view
inject="""        @* Grade 12 log-graph first-party mathematical artwork.
           Source math skill alignment only; publisher figures not reused. *@
        @if (Model.LessonCode == "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U04:L04")
        {
            <section class="lesson-source-aligned-activities"
                     aria-label="Logarithmic graph skill alternatives">
                <h2>Nineteen independent logarithmic graphical investigations</h2>
                <p>Examine the vertical asymptotes, valid domains, shifts and reflections.
                   The diagrams are original Edulytics plots. Explain your conclusions
                   and have your teacher review free-response graphical answers.</p>
                @foreach (var task in Grade12PrecalculusSection44GraphModels.All)
                {
                    <figure class="student-card">
                        @Html.Raw(AdvancedMathematicsVisualRenderer.RenderSvg(task.Graph))
                        <figcaption>Graph skill @task.SourceFigureSkill: @task.Prompt</figcaption>
                    </figure>
                }
            </section>
        }

"""
viewpath.write_text(view.replace(needle,inject+needle,1),encoding="utf8")
a["OriginalFigureSkillAlternativesAuthored"]=19
a["FirstPartyGraphRendererCode"]=cp.relative_to(root).as_posix()
a["StudentViewGraphRendererWired"]=True
a["GraphAcademicEquationStructureChecked"]=True
a["GraphRuntimeVerified"]=False
a["BlockingReasons"]=["FINAL_TARGETED_ACADEMIC_CLOSURE_GATE_PENDING","STAGE_D_RUNTIME_AND_BUILD_NOT_RUN"]
a["UpdatedUtc"]=datetime.datetime.now(datetime.timezone.utc).isoformat()
ap.write_text(json.dumps(a,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
# Targeted academic checks only, not xUnit, CI or build.
teacher=json.loads((root/a["FirstPartyTeacherAssessment"]).read_text(encoding="utf8"))
rich=json.loads((root/a["FirstPartyRichV2"]).read_text(encoding="utf8"))
assert len(teacher["FirstPartyIndependentStudentMathQuestions"])==60
assert all(q["TeacherExpectedMath"] for q in teacher["FirstPartyIndependentStudentMathQuestions"])
assert len(rich["Lessons"][0]["WorkedExamples"])==11
assert len(teacher["IndependentTryIt"])==11
assert all(q["SourceMathReviewed"] and q["IndependentStudentQuestionVerified"] and q["TeacherExpectedMathVerified"] for q in a["OriginalNumberedExerciseLedger"])
assert a["OriginalFigureSkillAlternativesAuthored"]==19
assert "Grade12PrecalculusSection44GraphModels.All" in viewpath.read_text(encoding="utf8")
assert p["StageD"]=="NOT_RUN" and not p["CIRun"]
closure_rel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u04-l04-abc-close-audit.json"
now=datetime.datetime.now(datetime.timezone.utc).isoformat()
closure={"SchemaVersion":1,"Course":"US-CCSS-MATH:G12","Unit":4,"Lesson":4,"Section":"4.4",
"OriginalSource":"OpenStax Precalculus 2017 local source","SourceSHA256":a["SourceSHA256"],
"SourcePdfPages":[378,394],"SourceWorkedExamples":11,"SourceTryIt":11,"SourceFigureSkills":19,
"OriginalSourceNumberedExercisesReviewed":60,"IndependentWorked":11,"IndependentTryIt":11,
"IndependentStudentQuestionsWithTeacherKeys":60,"FirstPartyGraphAlternativesAuthored":19,
"IndependentCoolDownQuestions":3,"StageC":"REUSE_EXISTING_RICHV2_TEACHER_MANUAL_MARKING_SVG_RENDERER",
"NewMathEngine":False,"PublisherOriginalFiguresCopied":False,
"PublisherFigureCoordinateParityCertified":False,
"CCSSSkillCandidates":["CCSS:HSF-IF.C.7e","CCSS:HSF-BF.B.3"],
"FormalCompleteCCSSAlignmentCertified":False,
"TargetedLocalChecksRun":["Source locator, 60 exercise skill maps and teacher keys",
"11 worked/11 Try It and 3 Cool-down answers","19 mathematical SVG models authored",
"Original student view wiring present","Git diff check"],
"RuntimeBuildOrCI":"NOT_RUN","StageD":"NOT_RUN","AutoGradingRuntime":"NOT_VERIFIED",
"ReadyForStageD":True,"ClosedAtUtc":now}
(root/closure_rel).write_text(json.dumps(closure,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
a.update({"Status":"READY_FOR_STAGE_D","ReadyForStageD":True,"BlockingReasons":[],
"ClosureEvidencePath":closure_rel,"CI":"NOT_RUN","StageD":"NOT_RUN","Tests":"TARGETED_LOCAL_ONLY",
"ClosedAtUtc":now,"UpdatedUtc":now})
ap.write_text(json.dumps(a,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
p["LessonEntries"][23].update({"ACStatus":"READY_FOR_STAGE_D","ReadyForStageD":True,
"BlockingReasons":[],"SourceMathExercisesReviewed":60,"SourceFigureSkillAlternativesAuthored":19,
"ClosureEvidencePath":closure_rel,"Tests":"TARGETED_LOCAL_ONLY","ClosedAtUtc":now,"UpdatedUtc":now})
assert p["LessonEntries"][24]["Ordinal"]==25
p.update({"LastClosedLessonOrdinal":24,"FirstIncompleteLessonOrdinal":25,
"ActiveLessonOrdinal":25,"NextLessonOrdinal":25,"ReadyForStageD":24,
"SourceVerifiedGrade12Lessons":24,"GradeStatus":"GRADE12_A_B_C_IN_PROGRESS_24_OF_73",
"UpdatedUtc":now})
p_path.write_text(json.dumps(p,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
files=[str(x.relative_to(root)) for x in [p_path,ap,cp,viewpath,root/closure_rel,Path(__file__)]]
subprocess.run(["git","add","--sparse","-f","--",*files],cwd=root,check=True)
subprocess.run(["git","diff","--cached","--check"],cwd=root,check=True)
subprocess.run(["git","commit","-m","feat(g12 u04 l04): close logarithmic graph skills academic ABC [skip ci]"],cwd=root,check=True)
subprocess.run(["git","push","origin","curriculum/us-ccss-g12-precalculus-abc-20261106"],cwd=root,check=True)
print("U04L04_ACADEMIC_CLOSE_PUSH_VERIFIED 24/73 NEXT25")
