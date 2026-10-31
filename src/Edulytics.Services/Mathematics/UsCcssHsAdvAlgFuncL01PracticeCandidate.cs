namespace Edulytics.Services.Mathematics;

/// <summary>
/// Prepared, deterministic MVP 10.6H matrix practice bank. Not registered in
/// selected learner Practice: shared contract/generator integration requires
/// the exclusive Issue #605 lease and academic/version acceptance. This bank
/// deliberately contains prompts and answer formats, never hidden solutions.
/// </summary>
public static class UsCcssHsAdvAlgFuncL01PracticeCandidate
{
    public const string LessonCode =
        "PED:US-CCSS-MATH:HS:FOURTH:ADV-ALG-FUNC:U01:L01";

    public sealed record Question(
        string LessonCode,
        string CaseId,
        string Prompt,
        string ExpectedResponseFormat,
        bool RequiresTeacherReview);

    public enum Disposition
    {
        WrongLessonOrCase,
        IncorrectOrMalformed,
        CorrectClosedAnswer,
        TeacherReviewRequired
    }

    public sealed record Assessment(Disposition Status, string Feedback);

    private static readonly Question[] Questions =
    [
        new(LessonCode, "M01",
            "Write the complete matrix equation Ax=b for x+y+z=6, " +
            "2x-y+z=3, x+2y-z=2, keeping the variable order x,y,z.",
            "A=[[row1],[row2],[row3]];b=[constant1,constant2,constant3]", false),
        new(LessonCode, "M02",
            "For 2x+y=5 and x+y=3, provide both the exact inverse " +
            "of the coefficient matrix and the solution vector.",
            "A^-1=[[row1],[row2]];x=[x,y]", false),
        new(LessonCode, "M03",
            "Find the exact inverse and solution for x+y+z=6, " +
            "2x-y+z=3, x+2y-z=2. Explain any factored inverse separately.",
            "A^-1=[[row1],[row2],[row3]];x=[x,y,z]", false),
        new(LessonCode, "M04",
            "Solve x+y+z=6, 2x-y+z=3, x+2y-z=2 by reversible " +
            "row reduction; submit the solution vector.",
            "x=[x,y,z] (show row-operation reasoning to a teacher)", false),
        new(LessonCode, "M05",
            "Describe and justify every solution of x+y=2 and " +
            "2x+2y=4; represent the free parameter.",
            "Written parameterization and explanation (teacher assessed)", true),
        new(LessonCode, "M06",
            "Classify x+y=2 and 2x+2y=5 by number of solutions.",
            "none / inconsistent / no solution", false),
        new(LessonCode, "M07",
            "Decide whether [[1,2],[2,4]] has an inverse; state " +
            "the determinant-based reason.",
            "singular / det(A)=0; inverse undefined", false),
        new(LessonCode, "M08",
            "Can the inverse of [[2,1],[1,1]] multiply a " +
            "three-entry right-hand-side vector? Classify the issue.",
            "dimension_mismatch / invalid dimensions", false),
        new(LessonCode, "M09",
            "Give the x,y,z coefficient row of 2x-y+z=7, " +
            "including signs and zeros when applicable.",
            "[x coefficient,y coefficient,z coefficient]", false),
        new(LessonCode, "M10",
            "Classify inverse eligibility of the 2-by-3 coefficient " +
            "matrix [[1,2,3],[4,5,6]].",
            "not_square / dimension-incompatible for inversion", false),
        new(LessonCode, "M11",
            "Explain why a reversible operation such as " +
            "R2 <- R2 - 2R1 preserves the solution set of a linear system.",
            "Written equivalence proof (teacher assessed)", true),
        new(LessonCode, "SOURCE_PAINTING",
            "Three painting receipts total 30, 20 and 10 units with " +
            "coefficient rows [2,0,1], [1,4,0], [0,2,1]. " +
            "Find the exact inverse and unit-price vector for paint, brush, tape.",
            "A^-1=[[row1],[row2],[row3]];x=[paint,brush,tape]", false),
        new(LessonCode, "SOURCE_TASK6",
            "Use a matrix inverse to solve x+2y+2z=16, 2x+y+3z=17, " +
            "x+2y+z=13; supply both the full inverse and solution.",
            "A^-1=[[row1],[row2],[row3]];x=[x,y,z]", false)
    ];

    /// <summary>
    /// Source-task rotation, not a claim of a live shared question generator.
    /// Negative indices and other lesson codes fail closed.
    /// </summary>
    public static bool TryGenerate(string? lessonCode, int itemIndex, out Question? question)
    {
        question = null;
        if (!string.Equals(lessonCode, LessonCode, StringComparison.Ordinal) || itemIndex < 0)
            return false;
        question = Questions[itemIndex % Questions.Length];
        return true;
    }

    public static bool TryGetQuestion(
        string? lessonCode, string? caseId, out Question? question)
    {
        question = null;
        if (!string.Equals(lessonCode, LessonCode, StringComparison.Ordinal))
            return false;
        foreach (var candidate in Questions)
        {
            if (!string.Equals(candidate.CaseId, caseId, StringComparison.Ordinal))
                continue;
            question = candidate;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Grading is limited to closed, source-bound numeric/classification tasks.
    /// This type neither persists teacher assessments nor issues learner marks.
    /// </summary>
    public static Assessment Assess(string? lessonCode, string? caseId, string? response)
    {
        if (!TryGetQuestion(lessonCode, caseId, out _))
            return new(Disposition.WrongLessonOrCase,
                "This matrix assessment is not available for the requested lesson or task.");

        var oracle = caseId switch
        {
            "SOURCE_PAINTING" =>
                UsCcssHsAdvAlgFuncSourceResponseContract.VerifyInverseAndSolution(
                    response, new long[,] { { 2, 0, 1 }, { 1, 4, 0 }, { 0, 2, 1 } },
                    [30, 20, 10])
                    ? UsCcssHsAdvAlgFuncSourceResponseContract.MatrixCaseDisposition.CorrectClosedAnswer
                    : UsCcssHsAdvAlgFuncSourceResponseContract.MatrixCaseDisposition.IncorrectOrMalformed,
            "SOURCE_TASK6" =>
                UsCcssHsAdvAlgFuncSourceResponseContract.VerifyInverseAndSolution(
                    response, new long[,] { { 1, 2, 2 }, { 2, 1, 3 }, { 1, 2, 1 } },
                    [16, 17, 13])
                    ? UsCcssHsAdvAlgFuncSourceResponseContract.MatrixCaseDisposition.CorrectClosedAnswer
                    : UsCcssHsAdvAlgFuncSourceResponseContract.MatrixCaseDisposition.IncorrectOrMalformed,
            _ => UsCcssHsAdvAlgFuncSourceResponseContract.AssessL01MatrixCase(caseId, response)
        };
        return oracle switch
        {
            UsCcssHsAdvAlgFuncSourceResponseContract.MatrixCaseDisposition.CorrectClosedAnswer =>
                new(Disposition.CorrectClosedAnswer,
                    "Exact closed-answer check passed. Any requested explanation is teacher assessed."),
            UsCcssHsAdvAlgFuncSourceResponseContract.MatrixCaseDisposition.TeacherReviewRequired =>
                new(Disposition.TeacherReviewRequired,
                    "Submit the mathematical argument for teacher review; no proof marks were awarded."),
            _ => new(Disposition.IncorrectOrMalformed,
                "Check the complete matrix, order, dimensions and exact numeric notation.")
        };
    }
}
