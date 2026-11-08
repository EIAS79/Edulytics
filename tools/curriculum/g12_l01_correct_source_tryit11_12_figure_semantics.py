from pathlib import Path
import json
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
def edit(relative, old, new):
    file=root/relative
    s=file.read_text(encoding="utf-8")
    assert s.count(old)==1,(relative,s.count(old),old[:90])
    file.write_text(s.replace(old,new),encoding="utf-8",newline="\n")

visual_rel="src/Edulytics.Web/Presentation/Grade12PrecalculusSection11FigureAlternatives.cs"
visual=root/visual_rel
s=visual.read_text(encoding="utf-8")
anchor="    public static readonly IReadOnlyList<Figure> All =\n"
assert s.count(anchor)==1
addition='''    // Independent analytic circle model: unlike the graph of a function,
    // this relation has two real outputs at the same x. Neither the source
    // artwork nor its pixel layout is reused.
    private static CoordinatePlaneVisualSpec CircleCounterexample()
    {
        const int samples = 96;
        var segments = new List<MathematicalSegment>();
        var previous = new MathematicalPoint(2m, 0m);
        for (var i = 1; i <= samples; i++)
        {
            var theta = 2d * Math.PI * i / samples;
            var next = new MathematicalPoint(
                (decimal)(2d * Math.Cos(theta)),
                (decimal)(2d * Math.Sin(theta)));
            segments.Add(new MathematicalSegment(previous, next));
            previous = next;
        }
        return new CoordinatePlaneVisualSpec(
            Window(-3m, 3m, -3m, 3m),
            [
                new MathematicalPoint(0m, 2m, "(0,2)"),
                new MathematicalPoint(0m, -2m, "(0,-2)")
            ],
            segments,
            new MathematicalVisualAccessibility(
                "First-party circle: not a function of x",
                "The independent relation x squared plus y squared equals 4. At x=0, " +
                "the graph has two outputs y=2 and y=-2. It fails the vertical-line test " +
                "and cannot be classified as a one-to-one function of x."));
    }

'''
s=s.replace(anchor,addition+anchor)
old='''        new(13,"Vertical-line failure highlighted",
            "The relation x=y² contains (1,1) and (1,-1). A vertical line at x=1 intersects twice.",
            41,null),
        new(14,"Independent vertical-line Try It alternative",
            "Sketch a closed ellipse and test x=0 on an ellipse centered at the origin; " +
            "two y-values show it is not a function. Original drawing remains unverified.",
            44,null),'''
new='''        new(13,"Source Figure 13: circle fails the vertical-line test",
            "An independent circle x²+y²=4 has points (0,2) and (0,-2). " +
            "The same x has two real outputs, so the relation is not a function of x.",
            null,CircleCounterexample()),
        new(14,"Source Figure 14: absolute-value V is a function",
            "The independent V-shaped graph y=|x| has vertex (0,0) and exactly " +
            "one output for every real x. Every vertical line hits at most once: " +
            "this relation IS a function, as required by the original Try It 11 skill.",
            null,Grade12PrecalculusSection11ToolkitGraphs.All
                .Single(family => family.Title == "Absolute value y=|x|").Graph),'''
assert s.count(old)==1
visual.write_text(s.replace(old,new),encoding="utf-8",newline="\n")
exercise="src/Edulytics.Web/Presentation/Grade12PrecalculusSection11Exercises.cs"
old='''"Graph y=x^2 on all real inputs. Is it one-to-one? Provide a horizontal-line counterexample."'''
new='''"Consider the independently drawn circle x²+y²=4 on real coordinates. Is this a one-to-one FUNCTION of x? Check the vertical-line test at x=0 BEFORE applying any horizontal-line argument."'''
edit(exercise,old,new)
guidance="src/Edulytics.Web/Presentation/Grade12PrecalculusSection11TeacherGuidance.cs"
old='''"No; y=4 meets the graph at x=-2 and x=2."'''
new='''"No. The original-skill FIRST-PARTY circle x²+y²=4 is not even a function of x: at x=0 it has outputs 2 and -2. The vertical-line test fails; one-to-one requires first being a function."'''
edit(guidance,old,new)
workbook="tools/curriculum/g12_u01_l01_tryit_workbook.py"
p=root/workbook
s=p.read_text(encoding="utf-8")
assert s.count("Graph y=x^2 on all real inputs. Is it one-to-one? Provide a horizontal-line counterexample.")==1
s=s.replace("Graph y=x^2 on all real inputs. Is it one-to-one? Provide a horizontal-line counterexample.",
    "Consider the independently drawn circle x²+y²=4 on real coordinates. Is this a one-to-one FUNCTION of x? Check the vertical-line test at x=0 BEFORE applying any horizontal-line argument.")
assert s.count("No; y=4 meets the graph at x=-2 and x=2.")==1
s=s.replace("No; y=4 meets the graph at x=-2 and x=2.",
    "No. The first-party circle fails the vertical-line test at x=0, with y=2 and y=-2; therefore it is not a one-to-one function.")
p.write_text(s,encoding="utf-8",newline="\n")
richrel="src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u01-l01.rich-lesson-v2.json"
packrel="src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g12-precalculus-u01-l01-abc-draft.lesson-content-pack.json"
oldnote=("Independent Try It 12 — Horizontal-line test for a function. Graph y=x^2 "
         "on all real inputs. Is it one-to-one? Provide a horizontal-line counterexample.")
newnote=("Independent Try It 12 — A prerequisite to injectivity. Inspect the "
         "independent circle x²+y²=4: at x=0 the relation has outputs y=2 and y=−2, "
         "so it fails the vertical-line test and cannot be a one-to-one function.")
rpath=root/richrel
r=json.loads(rpath.read_text(encoding="utf-8"))
rp=r["Lessons"][0]["ExplanationParagraphs"]
idx=next(i for i,x in enumerate(rp) if "Independent Try It 12" in x)
assert oldnote in rp[idx]
rp[idx]=rp[idx].replace(oldnote,newnote)
r["ContentVersion"]="g12-u01-l01-correct-source-tryit11-12-figures-20261106"
rpath.write_text(json.dumps(r,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
ppath=root/packrel
pp=json.loads(ppath.read_text(encoding="utf-8"))
trans=pp["Lessons"][0]["Translations"][0]
assert oldnote in trans["Explanation"]
trans["Explanation"]=trans["Explanation"].replace(oldnote,newnote)
pp["ContentVersion"]=r["ContentVersion"]
ppath.write_text(json.dumps(pp,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
tryfile=base/"u01-l01-tryit-first-party-teacher-review.json"
t=json.loads(tryfile.read_text(encoding="utf-8"))
q=t["Items"][11]
assert q["SourceId"]=="G12-U01-L01-TRY012"
q["LearningSkill"]="Function prerequisite for one-to-one: test vertical uniqueness before horizontal injectivity."
q["EdulyticsTitle"]="Check functionhood before testing one-to-one"
q["EdulyticsIndependentPrompt"]=new.strip('"')
q["ExpectedTeacherAnswer"]="No. A circle has two outputs at x=0, namely y=2 and y=-2, and is not a function of x; one-to-one is not applicable."
q["SourceTryItOriginalImageInspected"]=True
q["OriginalCircleNonfunctionCognitiveSkillPreserved"]=True
q["IndependentFirstPartyCircleModelFile"]=visual_rel
t["OriginalTryIt12SourceCircleSemanticMismatchCorrected"]=True
tryfile.write_text(json.dumps(t,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
# The student must see the exact independently drawn model close to the
# questions rather than being told to infer a circle from prose alone.
view="src/Edulytics.Web/Views/StudentPortal/Lesson.cshtml"
old='''                                <strong>Try It @(task.Number - 93):</strong> @task.Prompt
                                <a asp-controller="Grade12PrecalculusRubricStudent"'''
new='''                                <strong>Try It @(task.Number - 93):</strong> @task.Prompt
                                @if (task.Number is 104 or 105)
                                {
                                    var sourceFigure = task.Number == 104 ? 14 : 13;
                                    if (Grade12PrecalculusSection11FigureAlternatives.Find(sourceFigure)?.AddedDrawing is { } drawn)
                                    {
                                        <figure class="student-lesson-graph">
                                            @Html.Raw(AdvancedMathematicsVisualRenderer.RenderSvg(drawn))
                                            <figcaption>@drawn.Accessibility.Description</figcaption>
                                        </figure>
                                    }
                                }
                                <a asp-controller="Grade12PrecalculusRubricStudent"'''
edit(view,old,new)
ledpath=base/"u01-l01-element-coverage.json"
ledger=json.loads(ledpath.read_text(encoding="utf-8"))
fig14=next(x for x in ledger["Elements"] if x["SourceElementId"]=="G12-U01-L01-FIG014")
fig14["OriginalSourceFigure14Mathematics"]="V-shaped absolute value graph y=|x| is a function of x."
fig14["FirstPartyAlternativeSourceSkillCorrected"]=True
fig14["FirstPartyAlternativeGraphRendered"]=True
fig14["OriginalSourceFigureImageCopied"]=False
fig13=next(x for x in ledger["Elements"] if x["SourceElementId"]=="G12-U01-L01-FIG013")
fig13["OriginalSourceFigure13Mathematics"]="Original circle fails the vertical-line test."
fig13["FirstPartyIndependentCircleDiagramAuthored"]=True
fig13["OriginalSourceFigureImageCopied"]=False
try12=next(x for x in ledger["Elements"] if x["SourceElementId"]=="G12-U01-L01-TRY012")
try12["SourceLearningTaskMath"]="Original circle graph is not a function of x, so it cannot be one-to-one."
try12["FirstPartySemanticMatchRestored"]="INDEPENDENT_CIRCLE_REQUIRES_VERTICAL_TEST_BEFORE_INJECTIVITY"
try12["OriginalSourceFigureImageCopied"]=False
ledger["OriginalTryIt11And12VisualSourceSkillsCorrected"]=True
ledger["SourceAtomicDenominatorVerified"]=False
ledpath.write_text(json.dumps(ledger,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
check=base/"u01-l01-abc-checkpoint.json"
c=json.loads(check.read_text(encoding="utf-8"))
c["SourceTryIt11And12SourceImageVisualSemanticReinspection"]="PASS_SOURCE_11_V_FUNCTION_12_CIRCLE_NONFUNCTION"
c["SourceFigure14FalseEllipseStatementRemoved"]=True
c["SourceTryIt12FalseQuadraticSubstitutionRemoved"]=True
c["SourceOriginalFigureImagesCopied"]=0
c["ReadyForStageD"]=False
check.write_text(json.dumps(c,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
doc=base/"u01-l01-tryit11-12-original-graphic-semantic-correction.md"
doc.write_text("""# Original source graph cross-check: Figures 13, 14 and Try Its 11, 12

Local original PDF 2017, PDF pages 30–31 (1-based) visually rendered
and inspected. The original Figure 14 for Try It 11 is a V-shaped
absolute value graph: **it IS a function of x**. The previous
first-party Figure 14 alternative incorrectly described an ellipse
and concluded it was not a function. This was an actual pedagogic
error, not a cosmetic image difference.

The original Try It 12 shows a CIRCLE with a vertical line meeting
the graph twice. This relation is not a function and thus cannot be
a one-to-one function. The earlier first-party Try It 12 replaced it
with the quadratic y=x², which IS a function but not injective. That
replacement got the yes/no answer right for a different mathematical
reason, and so was *not* a faithful source-skill comparison.

Corrected independently authored materials: Figure 14 now shows an
accessible original Edulytics absolute-value graph that passes the
vertical-line test. Figure 13 and first-party Try It 12 now use an
accessible independent circle x²+y²=4 and require citing y=±2
at x=0. The diagrams are displayed beside the two first-party Try It
questions, not just in a distant gallery. Teacher rubric, RichV2,
canonical pack and source ledger are reconciled. No original source
publisher images were copied. Isolated C# compile and Stage D
testing are separate, and READY_FOR_STAGE_D is still blocked.
""",encoding="utf-8",newline="\n")
print("SOURCE_FIGURE14_V_FUNCTION_AND_TRYIT12_CIRCLE_NONFUNCTION_SEMANTIC_ERRORS_CORRECTED")
