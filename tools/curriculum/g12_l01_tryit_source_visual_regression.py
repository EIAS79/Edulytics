from pathlib import Path
import json
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
test=root/"tests/Edulytics.Tests/MathematicsIntelligence/Grade12PrecalculusSection11SourceContractsTests.cs"
s=test.read_text(encoding="utf-8")
old='''        Assert.Equal(2, figures.Count(x => x.OriginalFigureOrdinal is 6 or 10 &&
            x.AddedDrawing is not null));'''
new='''        Assert.Equal(4, figures.Count(x => x.AddedDrawing is not null));
        Assert.Contains("circle", figures.Single(x =>
            x.OriginalFigureOrdinal == 13).Topic, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("IS a function", figures.Single(x =>
            x.OriginalFigureOrdinal == 14).FirstPartyAlternative);
        Assert.True(figures.Single(x => x.OriginalFigureOrdinal == 13)
            .AddedDrawing is CoordinatePlaneVisualSpec);
        Assert.True(figures.Single(x => x.OriginalFigureOrdinal == 14)
            .AddedDrawing is CoordinatePlaneVisualSpec);'''
assert s.count(old)==1
s=s.replace(old,new)
anchor='''    [Fact]
    public void IndependentWorkedExamplesHaveCorrectMathGoldenValues()'''
assert s.count(anchor)==1
addition='''    [Fact]
    public void OriginalTryItElevenAndTwelveDoNotInvertFunctionhoodLogic()
    {
        var v = Grade12PrecalculusSection11Exercises.TryIts;
        Assert.Equal(12, v.Count);
        Assert.Contains("y=|x|", v.Single(x => x.Number == 104).Prompt);
        Assert.Contains("x²+y²=4", v.Single(x => x.Number == 105).Prompt);
        Assert.Contains("vertical-line test at x=0",
            v.Single(x => x.Number == 105).Prompt);
        Assert.Contains("not even a function",
            Grade12PrecalculusSection11TeacherGuidance.Find(105)!.ExpectedMathematics);
    }

'''
s=s.replace(anchor,addition+anchor)
test.write_text(s,encoding="utf-8",newline="\n")
doc=base/"u01-l01-figure15-accessible-source-crosswalk.md"
body=doc.read_text(encoding="utf-8")
old="Figure 14 | 30 | Try It graph requires teacher inspection for vertical-line function status."
new="Figure 14 | 30 | Original V-shaped absolute-value curve IS a function: every vertical line meets it once."
assert body.count(old)==1
doc.write_text(body.replace(old,new),encoding="utf-8",newline="\n")
checkpoint=base/"u01-l01-abc-checkpoint.json"
c=json.loads(checkpoint.read_text(encoding="utf-8"))
c["SourceTryIt11Figure14SourceMathematics"]="ORIGINAL_V_SHAPED_Y_ABS_X_IS_A_FUNCTION"
c["SourceTryIt12OriginalCircleSourceMathematics"]="ORIGINAL_CIRCLE_IS_NOT_A_FUNCTION_AND_CANNOT_BE_ONE_TO_ONE"
c["SourceTryIt11And12FirstPartyStudentDiagramsNowRendered"]=2
c["SourceTryIt11And12TeacherAnswerCognitiveSkillsAligned"]=True
c["SourceOriginalMediaImagesCopied"]=0
c["ReadyForStageD"]=False
checkpoint.write_text(json.dumps(c,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("FIG13_14_AND_TRY11_12_WRITTEN_REGRESSIONS_AND_SOURCE_CROSSWALK_RECONCILED")
