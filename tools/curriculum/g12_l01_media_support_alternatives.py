from pathlib import Path
import json
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
resources=[
("Functions from relations","A relation assigns ordered-pair outputs. A function permits at most one output per input; compare (2,4) and (2,5), which cannot both appear in a function.","1,6"),
("Vertical-line reasoning","A vertical line x=1 crosses the relation x=y² at both (1,1) and (1,−1). This is a precise counterexample to a single-valued function.","3,10,41"),
("Meaning of function notation","For f(x)=x², the input x=−2 produces output 4. Each x has one output even though two different inputs may share the output 4.","2,9,27"),
("Graphical test of functionhood","A circle x²+y²=4 fails the vertical-line test at x=0 because y=±2. The nonvertical line y=2x+1 passes.","40,44,47,50"),
("One-to-one depends on domain","f(x)=x² is not one-to-one on all real inputs because f(−2)=f(2)=4. Restricted to nonnegative x, it becomes one-to-one.","4,5,9,92"),
("Horizontal-line evidence on graphs","The horizontal level y=4 cuts y=x² at two distinct x-values, −2 and 2. Therefore this function is not one-to-one on its full real domain.","55,56,57,58,59")
]
assert len(resources)==6
codes=[
"namespace Edulytics.Web.Presentation;",
"",
"/// <summary>",
"/// Independent Edulytics text alternatives for the six original Section 1.1",
"/// optional web resources. No third-party video, images or link destinations",
"/// are embedded or asserted to remain available.",
"/// </summary>",
"public static class Grade12PrecalculusSection11SupportResources",
"{",
"    public sealed record Resource(int OriginalMediaOrdinal,",
"        string Title, string FirstPartyExplanation, string RelatedPracticeNumbers);",
"",
"    public static readonly IReadOnlyList<Resource> All =",
"    ["
]
for i,(title,explanation,refs) in enumerate(resources,1):
    codes.append(f"        new({i}, {json.dumps(title,ensure_ascii=False)}, {json.dumps(explanation,ensure_ascii=False)}, {json.dumps(refs)}),")
codes.extend(["    ];","}",""])
path=root/"src/Edulytics.Web/Presentation/Grade12PrecalculusSection11SupportResources.cs"
path.write_text("\n".join(codes),encoding="utf-8",newline="\n")
vpath=root/"src/Edulytics.Web/Views/StudentPortal/Lesson.cshtml"
view=vpath.read_text(encoding="utf-8")
anchor='''                <section class="student-card" aria-label="Edulytics original Cool-down">'''
assert view.count(anchor)==1
addition='''                <section class="student-card" aria-label="Edulytics original concept extensions">
                    <h3>Additional function concept explanations</h3>
                    <p>Six independent Edulytics explanations and practice references
                       address the source section's six optional media topics.
                       They are not embedded third-party videos.</p>
                    <ol>
                        @foreach (var resource in Grade12PrecalculusSection11SupportResources.All)
                        {
                            <li>
                                <strong>@resource.Title</strong>
                                <p>@resource.FirstPartyExplanation</p>
                                <p>Related practice task numbers: @resource.RelatedPracticeNumbers</p>
                            </li>
                        }
                    </ol>
                </section>
'''
vpath.write_text(view.replace(anchor,addition+anchor),encoding="utf-8",newline="\n")
testpath=root/"tests/Edulytics.Tests/MathematicsIntelligence/Grade12PrecalculusSection11SourceContractsTests.cs"
test=testpath.read_text(encoding="utf-8")
anchor='''    [Fact]
    public void IndependentWorkedExamplesHaveCorrectMathGoldenValues()'''
assert test.count(anchor)==1
testadd='''    [Fact]
    public void SixSourceMediaSkillsHaveIndependentFirstPartyLearningAlternatives()
    {
        Assert.Equal(6, Grade12PrecalculusSection11SupportResources.All.Count);
        Assert.Equal(Enumerable.Range(1, 6),
            Grade12PrecalculusSection11SupportResources.All.Select(x => x.OriginalMediaOrdinal));
        Assert.All(Grade12PrecalculusSection11SupportResources.All,
            resource => Assert.False(string.IsNullOrWhiteSpace(resource.FirstPartyExplanation)));
        // Alternative explanations do not prove third-party media rights or exact parity.
    }

'''
testpath.write_text(test.replace(anchor,testadd+anchor),encoding="utf-8",newline="\n")
coveragepath=base/"u01-l01-element-coverage.json"
coverage=json.loads(coveragepath.read_text(encoding="utf-8"))
media=[x for x in coverage["Elements"] if x["Category"]=="MEDIA"]
assert len(media)==6
for i,item in enumerate(media,1):
    assert item["SourceElementId"]==f"G12-U01-L01-MEDIA{i:03d}"
    item["FirstPartyInstructionalAlternative"]="src/Edulytics.Web/Presentation/Grade12PrecalculusSection11SupportResources.cs"
    item["FirstPartyAlternativeOrdinal"]=i
    item["EdulyticsArtifactPath"]="src/Edulytics.Web/Presentation/Grade12PrecalculusSection11SupportResources.cs"
    item["FirstPartyAlternativeStatus"]="AUTHORED_NOT_AUTHENTICATED_RUNTIME_VERIFIED"
    item["ImplementationType"]="DIGITAL_FIRST_PARTY_SUPPORT_ALTERNATIVE_NOT_SOURCE_VIDEO"
    item["RightsStatus"]="FIRST_PARTY_TEXT_ONLY_NO_SOURCE_MEDIA_EMBEDDED; ORIGINAL_EXTERNAL_MEDIA_RIGHTS_UNREVIEWED"
    item["SourceSemanticParity"]="SOURCE_SUPPORT_MEDIA_NOT_CERTIFIED_AS_FULLY_EQUIVALENT"
coverage["AuthoringEvidence"]["SixFirstPartyMediaSkillAlternativesAuthored"]=6
coverage["SourceMediaResourceCoverageVerified"]=False
coveragepath.write_text(json.dumps(coverage,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
cp_path=base/"u01-l01-abc-checkpoint.json"
cp=json.loads(cp_path.read_text(encoding="utf-8"))
cp["FirstPartyMediaSkillSupportAlternativesAuthored"]=6
cp["FirstPartyMediaStudentDelivery"]="AUTHORED_NOT_RUNTIME_VERIFIED"
cp["SourceMediaParityVerified"]=False
cp["ReadyForStageD"]=False
cp_path.write_text(json.dumps(cp,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
pgpath=base/"full-grade-progress.json"
pg=json.loads(pgpath.read_text(encoding="utf-8"))
pg["LessonEntries"][0]["FirstPartyMediaSkillSupportAlternativesAuthored"]=6
pg["LessonEntries"][0]["ReadyForStageD"]=False
pgpath.write_text(json.dumps(pg,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
print("SIX_SOURCE_MEDIA_TOPICS_HAVE_FIRST_PARTY_LEARNER_ALTERNATIVES")
