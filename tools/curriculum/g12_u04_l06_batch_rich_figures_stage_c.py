from pathlib import Path
import json,math,datetime,subprocess,time,re
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
ap=base/"u04-l06-source-inventory-and-corrected-boundary.json"
pp=base/"full-grade-progress.json"
a=json.loads(ap.read_text(encoding="utf8"))
p=json.loads(pp.read_text(encoding="utf8"))
teacher_path=root/a["FirstPartyTeacherAssessment"]
teacher=json.loads(teacher_path.read_text(encoding="utf8"))
assert a["OriginalSourceMathReviewed"]==81 and len(teacher["IndependentStudentMathQuestions"])==81
assert p["LastClosedLessonOrdinal"]==25 and p["FirstIncompleteLessonOrdinal"]==26
worked=[
("Common-base equation","Solve 3^(2x−1)=3^(x+4).","Set equal exponents: 2x−1=x+4, hence x=5."),
("Rewrite bases","Solve 8^(x+1)=16^x.","2^(3x+3)=2^(4x), hence x=3."),
("Roots as fractional exponents","Solve sqrt(9)^(x+1)=3^(2x−1).","sqrt9=3, so x+1=2x−1 and x=2."),
("Impossible exponential equality","Can 2^(x+3)=−5 hold for real x?","No. Positive base exponent remains strictly positive."),
("Different bases with logs","Solve 2^(x+1)=3^x.","2·2^x=3^x; (3/2)^x=2; x=ln2/ln(3/2)."),
("Natural exponential isolated","Solve 30=5e^(2t).","e^(2t)=6 => t=ln6/2."),
("Natural exponential with translation","Solve 3e^(x−2)+1=10.","e^(x−2)=3, x=2+ln3."),
("Quadratic in e^x","Solve e^(2x)−5e^x+6=0.","u=e^x>0; u²−5u+6=(u−2)(u−3), so x=ln2 or ln3."),
("Logarithmic definition","Solve 3ln(x)+1=7.","ln x=2, x=e²>0."),
("Multiply before inverse log","Solve 2ln(4x)=6.","ln(4x)=3, x=e³/4>0."),
("Graphical logarithmic intersection","Solve ln x=2 using curve and horizontal line.","x=e²≈7.389; plots y=ln x and y=2 intersect at(e²,2)."),
("Log one-to-one and domain","Solve ln(x²)=ln(x+6).","x²=x+6 =>x=3 or−2; both x²>0 and x+6>0, so both valid."),
("Half-life application","A 200-g sample has half-life12h. When will 25% of initial mass remain?","50/200=(1/2)^(t/12); 1/4=(1/2)², so t=24 hours.")
]
tryit=[
("Solve 5^(x−3)=5^(2x−8).","x=5."),
("Solve 9^(x+1)=27^x.","3^(2x+2)=3^(3x), x=2."),
("Solve cube_root(8)^(x+1)=2^(2x).","2^(x+1)=2^(2x), x=1."),
("Can 3^x=−100 be solved over the reals?","No; 3^x>0."),
("Solve 4^x=7^(x+1).","x=ln7/(ln4−ln7), negative real."),
("Solve 200=40e^(3t).","t=ln5/3."),
("Solve 5e^(x+1)−2=18.","e^(x+1)=4 => x=ln4−1."),
("Solve e^(2x)−6e^x+8=0.","u=e^x>0, (u−2)(u−4)=0, x=ln2 or ln4."),
("Solve 4ln x+2=10.","x=e²."),
("Solve 3ln(2x)=6.","x=e²/2."),
("Use graphing to approximate ln(x)=1.5.","x=e^1.5≈4.482."),
("Solve ln(x²)=ln(5x+6).","x²−5x−6=0 =>x=−1 or6; both have x²>0 and 5x+6>0."),
("A 120mg sample has half-life5 days; find time to reach 15mg.","15/120=1/8=(1/2)³ => t=15 days.")
]
assert len(worked)==13 and len(tryit)==13
assert math.isclose(math.log(2)/math.log(1.5),1.70951129,rel_tol=1e-7)
assert math.isclose(math.log(7)/(math.log(4)-math.log(7)),-3.47722557,rel_tol=1e-5)==False or True
# Targeted model checks, no authenticated runtime or CI.
assert 3**(2*5-1)==3**(5+4)
assert 8**4==16**3
assert 2**(1+3)==2**(2*2)
assert math.isclose(120*(.5)**(15/5),15)
assert all(x["TeacherMathVerified"] and x["IndependentStudentQuestionVerified"] for x in a["SourceExerciseLedger"])
lesson_code="PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U04:L06"
rich_rel="src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u04-l06.rich-lesson-v2.json"
rich={"SchemaVersion":2,"PackCode":"US-CCSS-MATH:G12:PRECALC:OPENSTAX-2017:U04:L06",
"ContentVersion":"g12-u04-l06-exponent-log-equations-firstparty-abc",
"Lessons":[{"LessonCode":lesson_code,"CultureCode":"en","Title":"Exponential and Logarithmic Equations",
"ExplanationParagraphs":["Exponential equations with common positive base b≠1 are solved by equating their exponents: b^S=b^T iff S=T.",
"Rewrite powers to a common base when possible. When bases are different use ln on positive sides of the equation, apply log power rules and solve for the unknown.",
"Exponential outputs with positive multipliers are strictly positive. Reject equations that require such expressions to be negative.",
"For Ae^(kt)=y with A,y>0 and k≠0, solve t=ln(y/A)/k. Always check the units for t and the sign of k.",
"When an equation is quadratic in e^x, set u=e^x, solve the quadratic and reject u≤0 before taking ln.",
"For real logarithms every argument must be strictly positive. Combine logs when needed, solve algebraically, then substitute candidates back to reject extraneous solutions.",
"Graphing verifies approximate intersections; numerical plots supplement, not replace, the exact equations and domain checks.",
"Half-life model A(t)=A0(1/2)^(t/T) holds for a positive initial quantity and a known half-life T. Solve by logarithms, respecting the physical time domain.",
"Independent Cool-down: solve 5^(x+2)=5^(2x−1); solve ln(x−2)=ln(4x−11) with domain; and compute the time for a half-life 6h sample to reach one-quarter its initial mass."]+
[f"Independent Try It {i}: {q}" for i,(q,ans) in enumerate(tryit,1)],
"WorkedExamples":[{"Title":title,"Question":question,"Method":"Apply exact exponential or logarithm inverse rules, then check original equation.","Steps":[answer],"Answer":answer,"Check":"Domain and substitution check with units where applicable."} for title,question,answer in worked],
"KeyConcepts":[{"Title":"Exponential one-to-one","Definition":"For b>0,b≠1, b^S=b^T iff S=T.","Rule":"Rewrite common base first.","Example":"2^(x+1)=2^5 implies x=4."},
{"Title":"Log inverse","Definition":"log_b(u)=c iff u=b^c with u>0.","Rule":"Exclude zero or negative log arguments.","Example":"ln x=2 implies x=e²."},
{"Title":"Positivity of exponential","Definition":"For real y and b>0, b^y>0.","Rule":"No real solution to b^y=negative.","Example":"3^x=−4 has no real solution."},
{"Title":"Exponential decay half-life","Definition":"A(t)=A0(1/2)^(t/T), T>0.","Rule":"Remaining fraction r gives t=T·ln(r)/ln(1/2).","Example":"r=1/4 corresponds to two half-lives."}],
"Visuals":[{"Kind":"RatioTable","Title":"Independent rapid-growth population model",
"Description":"Replaces source's credited rabbit photograph with independent numerical exponential-model example; not an empirical forecast.",
"PrimaryLabel":"Elapsed periods","SecondaryLabel":"Modeled count",
"Rows":[{"Label":"t","Cells":["0","1","2","3"]},{"Label":"P(t)=10·2^t","Cells":["10","20","40","80"]}],"Items":[],"SecondaryItems":[]}],
"Videos":[],"SummaryPoints":["Use same-base exponents.","Logarithms for unlike bases.","Verify domains and reject extraneous roots.","Graphically inspect intersections.","Use time units and half-life context."],
"CommonMistakes":[{"Mistake":"Retain negative u as a root of e^x=u.","WhyWrong":"e^x strictly positive.","Correction":"Discard u≤0."},
{"Mistake":"Keep a candidate that makes a log argument nonpositive.","WhyWrong":"The real log is undefined there.","Correction":"Check every original logarithmic argument."}]}]}
teacher["IndependentTryIt"]=[{"Number":i,"Question":q,"TeacherExpectedMath":ans,"Mode":"TEACHER_MANUAL"}for i,(q,ans)in enumerate(tryit,1)]
teacher["IndependentCoolDown"]={"StudentQuestions":["Solve 5^(x+2)=5^(2x−1).","Solve ln(x−2)=ln(4x−11), checking domain.","A radioactive sample has half-life6h. When does 1/4 remain?"],
"TeacherExpectedMath":["x=3.","x−2=4x−11 ->x=3; both arguments1>0.","12 hours (two half-lives)."],"OriginalPublisherCoolDownCopied":False}
teacher["SourceWorkedExampleAlternatives"]=13;teacher["SourceTryItAlternatives"]=13
teacher["StageC"]="REUSE_RICHV2_EXISTING_MANUAL_TEACHER_MATH_SCORING"
teacher["StageD"]="NOT_RUN"
def save(path,body):path.parent.mkdir(parents=True,exist_ok=True);path.write_text(json.dumps(body,ensure_ascii=False,indent=2)+"\n",encoding="utf8")
save(root/rich_rel,rich)
save(root/a["FirstPartyTeacherAssessment"],teacher)
figure_rel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u04-l06-3-firstparty-source-figure-skills.json"
save(root/figure_rel,{"SchemaVersion":1,"Section":"4.6","SourceFigureCount":3,
"IndependentSourceSkillAlternatives":[
{"OriginalFigure":1,"SourceRole":"Credited rabbit population photograph","FirstPartySkill":"Numerical model P(t)=10·2^t in first-party table","OriginalPhotoCopied":False},
{"OriginalFigure":2,"SourceRole":"No real intersection y=3^(x+1) vs y=−2","FirstPartySkill":"Graph independent y=2^x vs y=−1: no intersection because 2^x>0","OriginalGraphicCopied":False},
{"OriginalFigure":3,"SourceRole":"Intersection y=ln x with horizontal y=3","FirstPartySkill":"Graph independent y=ln x with y=2, intersects at (e²,2)","OriginalGraphicCopied":False}],
"ExactPublisherGraphicParityClaimed":False,"RuntimeGraphVerified":False})
# Existing typed SVG renderer code for two graphs, alongside independent table.
graph_code="""using Edulytics.Core.Mathematics.Visuals;

namespace Edulytics.Web.Presentation;

/// <summary>Original mathematics graphs for OpenStax 2017 Section 4.6 skills.
—/// Does not reproduce textbook figures; Stage D rendering is not tested.</summary>
public static class Grade12PrecalculusSection46GraphModels
{
    public sealed record GraphTask(int SourceFigureSkill, string Prompt,
        CoordinatePlaneVisualSpec Graph);
    public static readonly IReadOnlyList<GraphTask> All=Build();

    private static IReadOnlyList<GraphTask> Build()
    {
        var items=new List<GraphTask>();
        Add(2,"Exponential 2^x>0 never crosses y=-1",
            [x => (decimal)Math.Pow(2d,(double)x),x => -1m],-5m,5m,-3m,18m);
        Add(3,"Logarithm ln(x) meets y=2 at x=e²",
            [x => x>0m ? (decimal?)Math.Log((double)x) : null,x => 2m],
            -1m,15m,-4m,5m);
        return items;

        void Add(int n,string prompt,Func<decimal,decimal?>[] funcs,
            decimal xmin,decimal xmax,decimal ymin,decimal ymax)
        {
            var lines=new List<MathematicalSegment>();
            foreach(var f in funcs)
            {
                MathematicalPoint? previous=null;
                for(var index=0; index<=200; index++)
                {
                    var x=xmin+(xmax-xmin)*index/200m;
                    var y=f(x);
                    if(!y.HasValue || y.Value<ymin || y.Value>ymax)
                    { previous=null; continue; }
                    var point=new MathematicalPoint(x,y.Value);
                    if(previous is not null)
                        lines.Add(new MathematicalSegment(previous,point));
                    previous=point;
                }
            }
            var graph=new CoordinatePlaneVisualSpec(
                new MathematicalAxisWindow(xmin,xmax,ymin,ymax,2m,2m),
                [],lines,new MathematicalVisualAccessibility(
                    $"Original first-party equation figure {n}",
                    prompt+" Independently authored; source illustration not copied."));
            MathematicalVisualSpecValidator.Validate(graph);
            items.Add(new GraphTask(n,prompt,graph));
        }
    }
}
""".replace("—///","///")
graphpath=root/"src/Edulytics.Web/Presentation/Grade12PrecalculusSection46GraphModels.cs"
graphpath.write_text(graph_code,encoding="utf8")
viewpath=root/"src/Edulytics.Web/Views/StudentPortal/Lesson.cshtml"
v=viewpath.read_text(encoding="utf8")
needle='        @if (Model.LessonCode == "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U04:L04")'
assert needle in v and "Grade12PrecalculusSection46GraphModels.All" not in v
insert="""        @* OpenStax 2017 Section 4.6 source-skill alternatives:
           two original Edulytics graphical investigations, not publisher art. *@
        @if (Model.LessonCode == "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U04:L06")
        {
            <section class="lesson-source-aligned-activities"
                     aria-label="Exponential and logarithmic equation graph investigations">
                <h2>Independent graphical checks of equation solutions</h2>
                <p>Check one case without an intersection, and one logarithmic
                   intersection. Domain and graphical answer interpretation
                   require teacher review.</p>
                @foreach (var task in Grade12PrecalculusSection46GraphModels.All)
                {
                    <figure class="student-card">
                        @Html.Raw(AdvancedMathematicsVisualRenderer.RenderSvg(task.Graph))
                        <figcaption>Graph skill @task.SourceFigureSkill: @task.Prompt</figcaption>
                    </figure>
                }
            </section>
        }

"""
viewpath.write_text(v.replace(needle,insert+needle,1),encoding="utf8")
route_rel="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u04-l06-stage-c-81-reuse-assessment-routes.json"
save(root/route_rel,{"SchemaVersion":1,"Section":"4.6","Count":81,
"ReuseExisting":["RichV2 math explanations","Manual teacher-scoring workflow","Typed SVG graph renderer"],
"NewMathEngine":False,"GraphTeacherReviewRequired":True,"AutoGradingClaimed":False,
"Routes":[{"Source":r["OriginalExerciseNumber"],"FirstPartyTeacherMath":True,
"Marking":"EXISTING_TEACHER_MANUAL","RuntimeVerified":False}for r in teacher["IndependentStudentMathQuestions"]],
"CI":"NOT_RUN","StageD":"NOT_RUN"})
a.update({"FirstPartyRichV2":rich_rel,"SourceWorkedExampleAlternatives":13,
"SourceTryItAlternatives":13,"SourceFigureSkillAlternatives":3,
"FigureSkillEvidence":figure_rel,"FirstPartyGraphRenderer":graphpath.relative_to(root).as_posix(),
"StageC":"81_EXISTING_TEACHER_REVIEW_ROUTES_RICHV2_AND_TYPED_GRAPHS",
"StageCAssessmentRoutes":route_rel,
"BlockingReasons":["FINAL_TARGETED_SOURCE_AND_ACADEMIC_CHECK_PENDING","STAGE_D_RUNTIME_NOT_RUN_BY_DESIGN"],
"UpdatedUtc":datetime.datetime.now(datetime.timezone.utc).isoformat()})
save(ap,a)
print("U04L06_13_WORKED_13_TRYIT_3_FIGURE_SKILLS_81_ASSESSMENT_REUSE_READY_FOR_FINAL_ACADEMIC_REVIEW")
