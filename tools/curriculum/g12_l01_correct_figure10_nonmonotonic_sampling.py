from pathlib import Path
import json
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
visual=root/"src/Edulytics.Web/Presentation/Grade12PrecalculusSection11FigureAlternatives.cs"
s=visual.read_text(encoding="utf-8")
start=s.index('        new(10,"Discrete points versus continuous curve",')
end=s.index('        new(11,"Vertical-line comparison",',start)
old=s[start:end]
new='''        new(10,"Sampled points on a continuous non-monotonic function",
            "An original first-party continuous piecewise curve passes through " +
            "(0,2) and (6,1) but also rises and falls between sampled values. " +
            "Knowing two plotted points does not imply the curve is a straight " +
            "line, a monotonic function, or fully determined outside its graph.",
            null,new CoordinatePlaneVisualSpec(
                Window(-6m,8m,-3m,4m),
                [new MathematicalPoint(0m,2m,"(0,2)"),
                 new MathematicalPoint(6m,1m,"(6,1)")],
                [
                    new MathematicalSegment(new MathematicalPoint(-5m,-2m),
                        new MathematicalPoint(0m,2m)),
                    new MathematicalSegment(new MathematicalPoint(0m,2m),
                        new MathematicalPoint(3m,0.5m)),
                    new MathematicalSegment(new MathematicalPoint(3m,0.5m),
                        new MathematicalPoint(6m,1m)),
                    new MathematicalSegment(new MathematicalPoint(6m,1m),
                        new MathematicalPoint(8m,2m))
                ],
                new MathematicalVisualAccessibility(
                    "Function graph includes two sampled outputs but is non-monotonic",
                    "An independent continuous, piecewise-linear function rises to " +
                    "a local maximum (0,2), falls to a low point near (3,0.5), " +
                    "and rises through (6,1). Every x has one y, while the two " +
                    "highlighted samples do not determine the remaining curve."))),
'''
visual.write_text(s[:start]+new+s[end:],encoding="utf-8",newline="\n")
test=root/"tests/Edulytics.Tests/MathematicsIntelligence/Grade12PrecalculusSection11SourceContractsTests.cs"
t=test.read_text(encoding="utf-8")
anchor='''        Assert.Equal(5, figures.Count(x => x.AddedDrawing is not null));'''
assert t.count(anchor)==1
t=t.replace(anchor,anchor+'''
        Assert.Contains("rises and falls", figures.Single(x =>
            x.OriginalFigureOrdinal == 10).FirstPartyAlternative);
        var fig10 = Assert.IsType<CoordinatePlaneVisualSpec>(figures.Single(x =>
            x.OriginalFigureOrdinal == 10).AddedDrawing);
        Assert.Equal(4, fig10.Segments.Count);''')
test.write_text(t,encoding="utf-8",newline="\n")
ledgerpath=base/"u01-l01-element-coverage.json"
j=json.loads(ledgerpath.read_text(encoding="utf-8"))
row=next(x for x in j["Elements"] if x["SourceElementId"]=="G12-U01-L01-FIG010")
row["OriginalFigure10SourceMathematicalObservation"]="Graph has samples f(0)=2 and f(6)=1, and is a continuous function with non-monotonic shape."
row["FirstPartyFigure10CurveReplacement"]="CONTINUOUS_NONMONOTONIC_PIECEWISE_FUNCTION_WITH_EXACT_SAMPLES_0_2_AND_6_1"
row["FirstPartyFigure10SourceSamplingAndShapeSkillPreserved"]=True
row["FirstPartyFigure10OriginalPublisherCurveCopied"]=False
row["OriginalExactSmoothCurveNotReproduced"]=True
ledgerpath.write_text(json.dumps(j,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
checkpoint=base/"u01-l01-abc-checkpoint.json"
c=json.loads(checkpoint.read_text(encoding="utf-8"))
c["SourceFigure10SamplingAndNonMonotonicity"]="RESTORED_FIRST_PARTY_NONMONOTONIC_PIECEWISE_CURVE_EXACT_F0_2_F6_1"
c["OriginalFigure10ExactCurvePixelsCopied"]=False
c["ReadyForStageD"]=False
checkpoint.write_text(json.dumps(c,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
evidence=base/"u01-l01-original-figure10-continuity-sampling-correction.md"
evidence.write_text("""# Section 1.1 Figure 10: source graph sampling is not linear interpolation

Local 2017 original PDF page 29 (1-based) was visually inspected.
Original Figure 10 illustrates a continuous NON-MONOTONIC curve,
with two black sample points indicating f(0)=2 and f(6)=1.

Previous first-party graphic used the straight line f(x)=2−x/6;
this had the right two sample outputs but wrongly replaced the
source's rise-and-fall curve with a monotonic straight line.
It could therefore mislead learners into inferring a unique
curve from two sampled points.

First-party correction: a piecewise-linear, continuous but
non-monotonic function through the source's exact (0,2)
and (6,1) values, plus a local maximum and minimum.
The first-party diagram's shape is distinct from the original
publisher art. No source image was copied.

This independently closes the specific sampling/non-monotonicity
pedagogical gap; complete Section 1.1 source-atomic
parity and Stage D runtime remain separately unverified.
""",encoding="utf-8",newline="\n")
print("FIGURE10_INDEPENDENT_GRAPH_NOW_CONTINUOUS_NONMONOTONIC_WITH_ORIGINAL_TWO_SAMPLE_VALUES")
