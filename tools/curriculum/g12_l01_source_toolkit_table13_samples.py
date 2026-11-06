from pathlib import Path
import json
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
richpath=root/"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u01-l01.rich-lesson-v2.json"
packpath=root/"src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g12-precalculus-u01-l01-abc-draft.lesson-content-pack.json"
j=json.loads(richpath.read_text(encoding="utf-8"))
lesson=j["Lessons"][0]
table=next(x for x in lesson["Visuals"] if x["Title"]=="Source-skill table 13: Basic toolkit functions")
assert [r["Label"] for r in table["Rows"]]==["Family","Formula","Real domain"]
expected={
"Constant":("−2→2; 0→2; 2→2","f(x)=c"),
"Identity":("−2→−2; 0→0; 2→2","f(x)=x"),
"Absolute value":("−2→2; 0→0; 2→2","f(x)=|x|"),
"Quadratic":("−2→4; −1→1; 0→0; 1→1; 2→4","f(x)=x²"),
"Cubic":("−1→−1; −0.5→−0.125; 0→0; 0.5→0.125; 1→1","f(x)=x³"),
"Reciprocal":("−2→−0.5; −1→−1; −0.5→−2; 0.5→2; 1→1; 2→0.5","f(x)=1/x"),
"Reciprocal squared":("−2→0.25; −1→1; −0.5→4; 0.5→4; 1→1; 2→0.25","f(x)=1/x²"),
"Square root":("0→0; 1→1; 4→2","f(x)=√x"),
"Cube root":("−1→−1; −0.125→−0.5; 0→0; 0.125→0.5; 1→1","f(x)=∛x")
}
families=table["Rows"][0]["Cells"]
formulas=table["Rows"][1]["Cells"]
assert len(families)==len(formulas)==9
assert all(expected[f][1]==formulas[i] for i,f in enumerate(families))
table["Rows"].append({"Label":"Sample x→f(x) pairs","Cells":[expected[f][0] for f in families]})
table["Description"]=(
    "Nine basic function families with source-verified algebraic sample input/output pairs "
    "read from the locally authorized 2017 Section 1.1 Table 13; Edulytics independently "
    "draws the nine function graphs with explicit real domains. The underlying source "
    "images were not reused."
)
j["ContentVersion"]="g12-u01-l01-source-toolkit-table13-numeric-samples-20261106"
richpath.write_text(json.dumps(j,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
pack=json.loads(packpath.read_text(encoding="utf-8"))
t=pack["Lessons"][0]["Translations"][0]
note=("Independent example input-output samples for nine basic families: constant 2: D(-2)=D(0)=D(2)=2; "
      "identity maps -2,0,2 to -2,0,2; absolute value maps -2,0,2 to 2,0,2; "
      "quadratic -2,-1,0,1,2 to 4,1,0,1,4; cubic -1,-0.5,0,0.5,1 to -1,-0.125,0,0.125,1; "
      "reciprocal -2,-1,-0.5,0.5,1,2 to -0.5,-1,-2,2,1,0.5; "
      "reciprocal squared to 0.25,1,4,4,1,0.25; "
      "square root 0,1,4 to 0,1,2; and cube root -1,-0.125,0,0.125,1 to -1,-0.5,0,0.5,1. "
      "Exclude input 0 from reciprocal families; use nonnegative inputs for real square roots.")
if note not in t["Explanation"]:
    t["Explanation"]+="\n\n"+note
pack["ContentVersion"]="g12-u01-l01-source-toolkit-table13-numeric-samples-20261106"
packpath.write_text(json.dumps(pack,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
coverage=base/"u01-l01-element-coverage.json"
ledger=json.loads(coverage.read_text(encoding="utf-8"))
row=next(x for x in ledger["Elements"] if x["SourceElementId"]=="G12-U01-L01-TABLE013")
row["FirstPartyTable13SourceSampleValuesVerified"]=9
row["FirstPartyBasicToolkitFamilyGraphCount"]=9
row["FirstPartyTable13SourceSampleValuesDeliverable"]="FOURTH_RATIOTABLE_ROW_SOURCE_VERIFIED_NUMBERS"
row["OriginalSourceToolkitImageCopied"]=False
row["OriginalFigurePixelParityCertified"]=False
row["Status"]="SOURCE_VERIFIED"
ledger["OriginalSourceToolkitTable13NineFamilySampleRowsVerified"]=True
ledger["SourceAtomicDenominatorVerified"]=False
coverage.write_text(json.dumps(ledger,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
assessment=base/"u01-l01-assessment.json"
a=json.loads(assessment.read_text(encoding="utf-8"))
a["VisualRepresentationDrafts"]["OriginalToolkitTable13SampleFamiliesVerified"]=9
a["VisualRepresentationDrafts"]["SourceToolkitTable13OriginalImageCopied"]=False
a["VisualRepresentationDrafts"]["SourceToolkitGraphFigurePixelIdentityClaim"]=False
assessment.write_text(json.dumps(a,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
check=base/"u01-l01-abc-checkpoint.json"
c=json.loads(check.read_text(encoding="utf-8"))
c["SourceToolkitTable13NineFamilySampleValuesVerified"]=True
c["OriginalSourceToolkitTablesAndGraphSamplesFullyMatched"]="NINE_SOURCE_NUMERIC_SAMPLES_PLUS_NINE_FIRSTPARTY_GRAPH_FAMILIES_AUTHORED_NOT_PIXEL_IDENTICAL"
c["ReadyForStageD"]=False
check.write_text(json.dumps(c,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
tests=root/"tests/Edulytics.Tests/MathematicsIntelligence/Grade12PrecalculusSection11SourceContractsTests.cs"
s=tests.read_text(encoding="utf-8")
anchor='''    [Fact]
    public void IndependentWorkedExamplesHaveCorrectMathGoldenValues()'''
assert s.count(anchor)==1
new='''    [Fact]
    public void AllNineSourceToolkitFamiliesRetainTheirOriginalNumericalSamples()
    {
        var lesson = RichLessonContentV2Registry.Find(Code, "en");
        Assert.NotNull(lesson);
        var toolkit = lesson.Visuals.Single(x =>
            x.Title == "Source-skill table 13: Basic toolkit functions");
        Assert.Equal(4, toolkit.Rows.Count);
        Assert.Equal(9, toolkit.Rows[3].Cells.Count);
        Assert.Contains("−0.5→−2", toolkit.Rows[3].Cells[5]);
        Assert.Contains("−0.5→4", toolkit.Rows[3].Cells[6]);
        Assert.Contains("4→2", toolkit.Rows[3].Cells[7]);
        Assert.Contains("0.125→0.5", toolkit.Rows[3].Cells[8]);
        Assert.Equal(9, Grade12PrecalculusSection11ToolkitGraphs.All.Count);
    }

'''
tests.write_text(s.replace(anchor,new+anchor),encoding="utf-8",newline="\n")
doc=base/"u01-l01-original-table13-all-nine-function-samples.md"
doc.write_text("""# Table 13 — nine original toolkit formula and sample-value families

Authorized local source: OpenStax Precalculus (2017), Section 1.1,
PDF pages 32–33. These are mathematical identities and data points,
not copied publisher bitmap graphics.

The previously authored Edulytics table contained all nine family
names, formulas and real domains, **but omitted the publisher's
numerical example samples**. This is a real pedagogic omission,
because the original uses sample input/output values to teach
function notation and graph recognition.

Corrected the independently authored RatioTable by adding a fourth
row of all nine source-verified input/output sample groups. Family
graphs were already independently drawn and remain reused.
Canonical pack also describes the nine sample sets. The graded
tests are WRITTEN_NOT_RUN; no original images copied and source
figure pixel equality not claimed. A–B–C remains in progress.
""",encoding="utf-8",newline="\n")
print("TABLE13_NINE_SOURCE_SAMPLE_FAMILIES_RESTORED_TO_RICH_AND_PACK")
