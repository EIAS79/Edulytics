from pathlib import Path
import json
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
file=root/"src/Edulytics.Web/Presentation/Grade12PrecalculusSection11FigureAlternatives.cs"
s=file.read_text(encoding="utf-8")
old='''        new(15,"Horizontal-line test of injectivity",
            "On first-party y=x², horizontal y=4 meets x=-2 and x=2. " +
            "Thus it is a function but not one-to-one over all real inputs.",
            null,Grade12PrecalculusSection11Visuals.Build()[0])'''
new='''        new(15,"Horizontal-line test with three repeated outputs",
            "The original source illustrates that a horizontal line can meet " +
            "a function curve at two or even three different inputs. " +
            "In this independent cubic y=x³−4x, the horizontal level y=0 " +
            "meets x=−2, x=0 and x=2, so the function is not one-to-one.",
            null,new FunctionGraphVisualSpec(
                Window(-3m,3m,-6m,6m),
                new MathematicalCurveSpec("g12-s11-fig15-three-horizontal-roots",
                    MathematicalCurveKind.Polynomial,[0m,-4m,0m,1m],"y=x³−4x"),
                [new MathematicalPoint(-2m,0m,"(-2,0)"),
                 new MathematicalPoint(0m,0m,"(0,0)"),
                 new MathematicalPoint(2m,0m,"(2,0)")],
                new MathematicalVisualAccessibility(
                    "Continuous cubic is a function but not one-to-one",
                    "The cubic graph y=x cubed minus four x passes the vertical-line " +
                    "test, but horizontal y=0 crosses three times at x=-2,0,2. " +
                    "This recreates the source's three-intersection skill without " +
                    "copying source curve artwork.")))'''
assert s.count(old)==1
file.write_text(s.replace(old,new),encoding="utf-8",newline="\n")
test=root/"tests/Edulytics.Tests/MathematicsIntelligence/Grade12PrecalculusSection11SourceContractsTests.cs"
s=test.read_text(encoding="utf-8")
old='''        Assert.Equal(4, figures.Count(x => x.AddedDrawing is not null));'''
new='''        Assert.Equal(5, figures.Count(x => x.AddedDrawing is not null));
        Assert.Contains("three different inputs", figures.Single(x =>
            x.OriginalFigureOrdinal == 15).FirstPartyAlternative);
        Assert.Contains("x=-2,0,2", figures.Single(x =>
            x.OriginalFigureOrdinal == 15).AddedDrawing!.Accessibility.Description);'''
assert s.count(old)==1
test.write_text(s.replace(old,new),encoding="utf-8",newline="\n")
ledgerpath=base/"u01-l01-element-coverage.json"
j=json.loads(ledgerpath.read_text(encoding="utf-8"))
row=next(x for x in j["Elements"] if x["SourceElementId"]=="G12-U01-L01-FIG015")
row["OriginalFigure15SourceSkill"]="A continuous cubic-like function passes vertical tests yet can intersect a horizontal level in 2 or 3 distinct points."
row["FirstPartyFigure15ThreeInputCounterexample"]="y=x³−4x meets horizontal y=0 at x=-2,0,2."
row["FirstPartyFigure15SourceExtraSkillPreserved"]=True
row["FirstPartyFigure15IndependentlyDrawnGraph"]=True
row["OriginalPublisherSourceImageCopied"]=False
row["OriginalExactShapeIdentityNotClaimed"]=True
j["SourceAtomicDenominatorVerified"]=False
ledgerpath.write_text(json.dumps(j,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
check=base/"u01-l01-abc-checkpoint.json"
c=json.loads(check.read_text(encoding="utf-8"))
c["SourceFigure15HorizontalThreeIntersections"]="INDEPENDENT_CUBIC_EXACT_THREE_ROOTS_SOURCE_PEDAGOGIC_NUANCE_RESTORED"
c["FirstPartyNamedSourceFigureGraphicsWithIndependentDrawing"]=5
c["ReadyForStageD"]=False
check.write_text(json.dumps(c,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
evidence=base/"u01-l01-original-figure15-three-horizontal-intersection-equivalence.md"
evidence.write_text("""# Figure 15 — horizontal line may meet a function at three inputs

Original local OpenStax Precalculus 2017 PDF, printed Section 1.1,
page 15 / PDF page 31: Figure 15 is a two-turning-point curve that
passes the vertical-line test, but a horizontal line can meet it at
two points, and a different horizontal level can meet it at THREE.

Previous first-party Figure15 illustration used y=x² and showed
only two repeated outputs. That was sufficient to disprove
injectivity but omitted the source's explicit three-input nuance.

Corrected independent first-party model: y=x³−4x, which is a genuine
function on all real x. The exact horizontal zero level meets the
graph at (-2,0), (0,0), and (2,0). The rubric and text teach why
this fails one-to-one even though every vertical line meets once.
Source artwork was not copied and pixel identity is not claimed.
Targeted compile and Stage D are separate; xUnit specs written only.
""",encoding="utf-8",newline="\n")
print("FIGURE15_SOURCE_THREE_HORIZONTAL_INTERSECTIONS_RESTORED_FIRST_PARTY_CUBIC")
