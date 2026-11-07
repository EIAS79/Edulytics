from pathlib import Path
import json
root=Path(r"C:\Users\khali\Desktop\edulytics-g12-pr626")
base=root/"artifacts/curriculum-delivery/US-CCSS-MATH/G12"
def patch(rel,old,new):
    path=root/rel
    raw=path.read_text(encoding="utf-8")
    assert raw.count(old)==1,(rel,old[:80],raw.count(old))
    path.write_text(raw.replace(old,new),encoding="utf-8",newline="\n")
src="src/Edulytics.Web/Presentation/Grade12PrecalculusSection11Exercises.cs"
old='''    public static Task? Find(int number) =>
        number == CoolDownNumber ? CoolDown :
        number is >= 94 and <= 105
            ? TryIts.FirstOrDefault(task => task.Number == number)
            : All.FirstOrDefault(task => task.Number == number);
}'''
new='''    public static Task? Find(int number) =>
        number == CoolDownNumber ? CoolDown :
        number is >= 94 and <= 105
            ? TryIts.FirstOrDefault(task => task.Number == number)
            : All.FirstOrDefault(task => task.Number == number);

    // Reading/plotting a graph can be assessed using a labelled coordinate
    // representation supplied as text. This is an accessible teacher-review
    // alternative; it does not silently create or assert a rendered drawing.
    public static bool SupportsGraphEvidence(int number) =>
        number is (>= 40 and <= 59) or (>= 76 and <= 87) or 104 or 105;

    // Section source exercises 76–87 explicitly require graphing the function.
    public static bool RequiresGraphEvidence(int number) =>
        number is >= 76 and <= 87;

    public static string? ComposeTeacherResponse(
        int number, string? explanation, string? graphEvidence)
    {
        if (Find(number) is null || string.IsNullOrWhiteSpace(explanation))
            return null;
        var reasoning = explanation.Trim();
        if (reasoning.Length is < 20 or > 8000)
            return null;
        var evidence = graphEvidence?.Trim() ?? string.Empty;
        if (evidence.Length > 3000 ||
            (RequiresGraphEvidence(number) && evidence.Length < 20) ||
            (!SupportsGraphEvidence(number) && evidence.Length > 0))
            return null;
        if (evidence.Length == 0)
            return reasoning;
        var combined = "Written reasoning:\\n" + reasoning +
            "\\n\\nLabelled graph / coordinate representation:\\n" + evidence;
        return combined.Length <= 8000 ? combined : null;
    }
}'''
patch(src,old,new)
ctrl="src/Edulytics.Web/Controllers/Grade12PrecalculusRubricController.cs"
old='''        Guid lessonId, int exerciseNumber,
        [FromForm] string? explanation, CancellationToken ct)'''
new='''        Guid lessonId, int exerciseNumber,
        [FromForm] string? explanation,
        [FromForm] string? graphEvidence, CancellationToken ct)'''
patch(ctrl,old,new)
old='''        if (string.IsNullOrWhiteSpace(explanation) ||
            explanation.Trim().Length is < 20 or > 8000)
            return BadRequest("An explanation between 20 and 8000 characters is required.");
        var inserted = await rubric.InsertAsync( scope.Value.School, scope.Value.Class, scope.Value.Student,
            lessonId, exerciseNumber, explanation.Trim(), ct);'''
new='''        var studentResponse = Grade12PrecalculusSection11Exercises.ComposeTeacherResponse(
            exerciseNumber, explanation, graphEvidence);
        if (studentResponse is null)
            return BadRequest("Provide complete written reasoning and, for graph-window " +
                "tasks 76-87, a labelled graph/coordinate description. " +
                "Maximum combined length is 8000 characters.");
        var inserted = await rubric.InsertAsync(scope.Value.School, scope.Value.Class, scope.Value.Student,
            lessonId, exerciseNumber, studentResponse, ct);'''
patch(ctrl,old,new)
view="src/Edulytics.Web/Views/Grade12PrecalculusRubricStudent/Index.cshtml"
old='''        <textarea id="explanation" name="explanation" required minlength="20"
                  maxlength="8000" rows="10"></textarea>
        <button type="submit">Submit this exercise to the teacher</button>'''
new='''        <textarea id="explanation" name="explanation" required minlength="20"
                  maxlength="8000" rows="10"></textarea>
        @if (Grade12PrecalculusSection11Exercises.SupportsGraphEvidence(Model.ExerciseNumber))
        {
            <label for="graphEvidence">Graph evidence — describe the x/y axes,
                interval, plotted points or horizontal/vertical intersections
                using labelled coordinates (up to 3000 characters).</label>
            <p>For graph-window tasks 76–87 this evidence is required. A text
               description supports teacher assessment but is not a drawing
               upload or an automatically scored graph.</p>
            @if (Grade12PrecalculusSection11Exercises.RequiresGraphEvidence(Model.ExerciseNumber))
            {
                <textarea id="graphEvidence" name="graphEvidence" rows="5"
                          maxlength="3000" minlength="20" required></textarea>
            }
            else
            {
                <textarea id="graphEvidence" name="graphEvidence" rows="5"
                          maxlength="3000"></textarea>
            }
        }
        <p>The combined submission must not exceed 8000 characters.
           The teacher reviews your reasoning, graph data and all source-skill
           parts. Drawn images may be inspected separately by your teacher.</p>
        <button type="submit">Submit this exercise to the teacher</button>'''
patch(view,old,new)
tests="tests/Edulytics.Tests/MathematicsIntelligence/Grade12PrecalculusSection11SourceContractsTests.cs"
anchor='''    [Fact]
    public void IndependentWorkedExamplesHaveCorrectMathGoldenValues()'''
insert='''    [Fact]
    public void Grade12GraphSubmissionsPreserveAccessibleCoordinatesAndRequireEvidence()
    {
        const string reasoning = "This input value has a single correctly labelled output.";
        const string graph = "x-domain [-10,10], marked vertex (0,0), range [0,100].";
        Assert.True(Grade12PrecalculusSection11Exercises.SupportsGraphEvidence(40));
        Assert.True(Grade12PrecalculusSection11Exercises.RequiresGraphEvidence(76));
        Assert.False(Grade12PrecalculusSection11Exercises.RequiresGraphEvidence(52));
        Assert.Null(Grade12PrecalculusSection11Exercises.ComposeTeacherResponse(76, reasoning, null));
        Assert.Null(Grade12PrecalculusSection11Exercises.ComposeTeacherResponse(76, reasoning, "x=1"));
        Assert.NotNull(Grade12PrecalculusSection11Exercises.ComposeTeacherResponse(76, reasoning, graph));
        Assert.Contains("Labelled graph / coordinate representation:",
            Grade12PrecalculusSection11Exercises.ComposeTeacherResponse(76, reasoning, graph)!);
        Assert.Null(Grade12PrecalculusSection11Exercises.ComposeTeacherResponse(27, reasoning, graph));
        Assert.Null(Grade12PrecalculusSection11Exercises.ComposeTeacherResponse(106, reasoning, null));
    }

'''
patch(tests,anchor,insert+anchor)
assessment=base/"u01-l01-assessment.json"
a=json.loads(assessment.read_text(encoding="utf-8"))
a["FirstPartySectionExerciseBank"]["AccessibleStudentGraphEvidenceFields"]="SOURCE_EXERCISES_40_TO_59_OPTIONAL; SOURCE_WINDOW_EXERCISES_76_TO_87_REQUIRED"
a["FirstPartySectionExerciseBank"]["GraphEvidenceDataContract"]="SERVER_VALIDATED_TEXT_COORDINATE_DESCRIPTION_STORED_WITH_ORIGINAL_REASONING_FOR_TEACHER"
a["FirstPartySectionExerciseBank"]["ActualStudentDrawnGraphUpload"]="NOT_IMPLEMENTED"
a["FirstPartySectionExerciseBank"]["AuthenticatedRuntimeTest"]="NOT_RUN"
assessment.write_text(json.dumps(a,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
for name in ["u01-l01-abc-checkpoint.json","full-grade-progress.json"]:
    p=base/name
    j=json.loads(p.read_text(encoding="utf-8"))
    row=j if name.startswith("u01") else j["LessonEntries"][0]
    row["FirstPartyGraphEvidenceTextRoute"]="AUTHORED_FOR_SOURCE_40_59_AND_76_87_NOT_RUNTIME_TESTED"
    row["FirstPartyWindowGraphEvidenceRequired"]=True
    row["FirstPartyActualGraphFileUpload"]="NOT_IMPLEMENTED"
    row["ReadyForStageD"]=False
    p.write_text(json.dumps(j,ensure_ascii=False,indent=2)+"\n",encoding="utf-8")
note=base/"u01-l01-accessible-student-graph-evidence-contract.md"
note.write_text("""# Section 1.1 graph-response contract

Original Precalculus 2017 graphical task families:
- Exercises **40–59**: classify/source-read given coordinate graphs, including
  vertical/horizontal line reasoning and graph values. An optional labelled
  coordinate-description field is exposed.
- Exercises **76–87**: explicitly construct the function graph within a specified
  source x-window and report its output range. A textual representation of
  labelled axes, vertex/intersections and plotted coordinates is mandatory;
  teacher reviews its adequacy together with the authored source-window graph.

The first-party student form combines a written explanation and labelled graph
coordinates in a single tenant-scoped SQL response. Validation is done on the
server: valid 1–105 activity ID; explanation length 20–8000; optional graph
evidence max 3000; evidence required for 76–87 and at least 20 characters;
combined stored payload max 8000. HTML is Razor-escaped in the teacher view.
Teacher rubric remains four binary criteria, max four marks.

This is a legitimate **accessible typed alternative**, not a drawn graph upload,
not a verified auto-grader, not source figure pixel-parity, and not a substitute
for the teacher checking the graphical construction. Stage D is intentionally
not executed in A–B–C.
""",encoding="utf-8",newline="\n")
print("GRAPH_SUBMISSION_SERVER_VALIDATION_AND_ACCESSIBLE_FIELD_AUTHORED")
