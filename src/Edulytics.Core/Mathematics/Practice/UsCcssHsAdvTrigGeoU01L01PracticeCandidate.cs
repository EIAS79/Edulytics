using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// L01 first-party source 7.5 review candidate. FAIL-CLOSED and not registered.
/// This is a course-specific answer POLICY/fixture bridge, NOT a replacement
/// for the shared typed question engine and NOT an authoritative student grader.
/// #600 currently accepts near-equal decimals; never route through that verifier
/// until the lesson-specific exact-only policy is enforced with a file lease.
/// </summary>
public static class UsCcssHsAdvTrigGeoU01L01PracticeCandidate
{
    public const string LessonCode = "PED:US-CCSS-MATH:HS:FOURTH:ADV-TRIG-GEO:U01:L01";
    public const string SourceFamily = "trig.unit_circle_reflection_exact";
    public const string ExistingKernelFamily = "trig.unit_circle_source_exact_angles";
    public const string SkillId = "supporting.trigonometry.core";
    public const string Version = "usccss.hs.adv_trig_geo.u01.l01.pending.v2";
    public const string Readiness = "INTEGRATION_PENDING";
    public const string AnswerPolicy = "EXACT_SYMBOLIC_ONLY_ALLOWLIST";
    public const int RequiredScorableQuestions = 3;
    public const int TeacherProofRubricMaxPoints = 6;

    public static bool StudentPracticeRoutingAuthorized => false;
    public static bool AcademicApprovalRecorded => false;
    public static bool ContentPredecessorApproved => false;
    public static bool SharedVerifierExactPolicyVerified => false;
    public static bool MayAutomaticallyScoreHumanProof => false;

    // Codes expected by the read-only shared #600 typed engine:
    // function sin=0, cos=1, tan=2; transform pi-theta=0, pi+theta=1, 2pi-theta=2.
    public sealed record CandidateCase(
        string Id, int Twelfths, int FunctionCode, int TransformCode,
        string ExpectedStatus, string? Exact,
        IReadOnlyList<string> ApprovedExactEquivalents,
        IReadOnlyList<string> RejectedDecimalExamples,
        IReadOnlyList<string> IncorrectForms,
        IReadOnlyList<string> MalformedForms)
    {
        public bool IsScorable => ExpectedStatus == "OK" && Exact is not null;
    }

    private static readonly CandidateCase[] Cases =
    [
        new("l01-reflection-cos", 2, 1, 0, "OK", "-sqrt(3)/2",
            ["-sqrt(12)/4"], ["-0.8660254037844386", "-0.866", "-8.660254037844386e-1"],
            ["sqrt(3)/2", "-1/2"], ["cos(", "banana", "<script>"]),
        new("l01-rotation-sin", 4, 0, 1, "OK", "-sqrt(3)/2",
            ["-sqrt(12)/4"], ["-0.8660254037844386", "-0.866"],
            ["sqrt(3)/2"], ["sin(", "banana", "NaN"]),
        new("l01-fullturn-cos", 3, 1, 2, "OK", "sqrt(2)/2",
            ["1/sqrt(2)", "sqrt(8)/4"], ["0.7071067811865476", "0.707"],
            ["-sqrt(2)/2"], ["cos(", "banana", "Infinity"]),
        new("l01-tangent-pole", 6, 2, 2, "OUT_OF_DOMAIN", null,
            [], [], ["0", "1", "undefined=0"], ["tan(", "banana"])
    ];

    public static IReadOnlyList<CandidateCase> ReviewCases => Array.AsReadOnly(Cases);

    public static bool TryGetReviewCase(string? lessonCode, string? caseId,
        out CandidateCase? candidate)
    {
        candidate = null;
        if (!string.Equals(lessonCode, LessonCode, StringComparison.Ordinal)
            || string.IsNullOrWhiteSpace(caseId))
            return false;
        candidate = Cases.FirstOrDefault(c =>
            string.Equals(c.Id, caseId, StringComparison.Ordinal));
        return candidate is not null;
    }

    /// <summary>Read-only reviewer fixture; not a learner question generator.</summary>
    public static bool TryGetReviewerKernelParameters(string? lessonCode,
        string? caseId, out IReadOnlyDictionary<string, int>? parameters)
    {
        parameters = null;
        if (!TryGetReviewCase(lessonCode, caseId, out var c) || c is null)
            return false;
        parameters = new Dictionary<string, int>(StringComparer.Ordinal)
        {
            ["twelfths"] = c.Twelfths,
            ["function"] = c.FunctionCode,
            ["transform"] = c.TransformCode
        };
        return true;
    }

    /// <summary>
    /// A deterministic reviewer-policy classification. It deliberately does
    /// not evaluate or execute arbitrary submitted expressions. Only explicitly
    /// reviewed exact symbolic aliases qualify; additional algebraically
    /// equivalent expressions require a separately reviewed shared normalizer.
    /// These statuses are NOT grades and are not wired to student Practice.
    /// </summary>
    public static string ClassifyReviewerSubmission(
        string? lessonCode, string? caseId, string? submission)
    {
        if (!TryGetReviewCase(lessonCode, caseId, out var candidate)
            || candidate is null)
            return "LESSON_OR_CASE_MISMATCH";
        if (!candidate.IsScorable)
            return "OUT_OF_DOMAIN";
        if (string.IsNullOrWhiteSpace(submission) || submission.Length > 80)
            return "MALFORMED_OR_UNSUPPORTED";
        // Formatting whitespace is permitted, but merging separate numeric
        // tokens must not manufacture an approved radical (sqrt(1 2) != sqrt(12)).
        if (Regex.IsMatch(submission, @"(?<=\d)\s+(?=\d)"))
            return "MALFORMED_OR_UNSUPPORTED";
        var answer = string.Concat(submission.Where(c => !char.IsWhiteSpace(c)));
        // Exactly-written answer only. A decimal/e-notation representation
        // is never implicitly equivalent to a required exact radical.
        if (string.Equals(answer, candidate.Exact, StringComparison.Ordinal)
            || candidate.ApprovedExactEquivalents.Contains(
                answer, StringComparer.Ordinal))
            return "EXACT_ACCEPTED";
        if (candidate.RejectedDecimalExamples.Contains(
                answer, StringComparer.Ordinal)
            || IsDecimalNumericToken(answer))
            return "DECIMAL_REJECTED_EXACT_REQUIRED";
        if (candidate.IncorrectForms.Contains(answer, StringComparer.Ordinal))
            return "INCORRECT";
        return "MALFORMED_OR_UNSUPPORTED";
    }

    private static bool IsDecimalNumericToken(string answer)
    {
        // This detects decimal and scientific *numeric tokens* only; other
        // unknown expressions fail closed in MALFORMED_OR_UNSUPPORTED.
        if (answer.Length == 0 || !answer.Any(c => c is '.' or 'e' or 'E'))
            return false;
        return double.TryParse(answer,
            System.Globalization.NumberStyles.Float,
            System.Globalization.CultureInfo.InvariantCulture, out _);
    }
}
