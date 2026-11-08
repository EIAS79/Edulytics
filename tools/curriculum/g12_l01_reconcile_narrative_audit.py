from pathlib import Path
import json
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
auditpath=base/"u01-l01-a-b-c-source-coverage-audit.json"
a=json.loads(auditpath.read_text(encoding="utf-8"))
assert a["SourceTopLevelAnchors"]==170
a["SourceTopLevelAnchors"]=187
a["SourceTopLevelSourceVerified"]=187
a["SourceTopLevelOnlyIdentified"]=0
a["OriginalSourceNarrativePassagesExplicitlyVerified"]=13
a["OriginalSourceFormalDefinitionsExplicitlyVerified"]=4
a["OriginalSource273NamedLearningActionLeafInventory"]="artifacts/curriculum-delivery/US-CCSS-MATH/G12/u01-l01-273-named-source-atomic-actions.json"
a["SourceCategoryReview"]["NARRATIVE"]={"SourceAnchors":13,"SourceVerified":13,"IdentifiedNotComplete":0}
a["SourceCategoryReview"]["DEFINITION"]={"SourceAnchors":4,"SourceVerified":4,"IdentifiedNotComplete":0}
assert sum(x["SourceAnchors"] for x in a["SourceCategoryReview"].values())==187
assert sum(x["SourceVerified"] for x in a["SourceCategoryReview"].values())==187
a["FullSourceSemanticAndAtomicDenominatorVerified"]=False
a["ReadyForStageD"]=False
auditpath.write_text(json.dumps(a,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
testpath=root/"tests/Edulytics.Tests/MathematicsIntelligence/Grade12PrecalculusSection11SourceContractsTests.cs"
test=testpath.read_text(encoding="utf-8")
anchor='''    [Fact]
    public void IndependentWorkedExamplesHaveCorrectMathGoldenValues()'''
assert test.count(anchor)==1
insert='''    [Fact]
    public void NarrativeSourceGapsHaveFirstPartyExplanationsAndNineNamedToolkitFamilies()
    {
        var lesson = RichLessonContentV2Registry.Find(Code, "en");
        Assert.NotNull(lesson);
        var narrative = string.Join(" ", lesson.ExplanationParagraphs);
        Assert.Contains("not multiplication", narrative, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("sampled ordered pairs", narrative, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("isolated plotted values", narrative, StringComparison.OrdinalIgnoreCase);
        var toolkit = lesson.KeyConcepts.Single(x => x.Title == "Basic function families");
        Assert.Contains("reciprocal squared", toolkit.Definition);
        Assert.Contains("cube root", toolkit.Definition);
        Assert.Equal(9, Grade12PrecalculusSection11ToolkitGraphs.All.Count);
    }

'''
testpath.write_text(test.replace(anchor,insert+anchor),encoding="utf-8",newline="\n")
print("187_SOURCE_PARENT_LOCATORS_AND_273_NAMED_ACTIONS_RECONCILED; NARRATIVE_REGRESSION_SPECS_AUTHORED")
