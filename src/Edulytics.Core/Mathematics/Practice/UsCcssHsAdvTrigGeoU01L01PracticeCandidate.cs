using System;
using System.Collections.Generic;
using System.Linq;

namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// Course-owned candidate for MVP Secondary Mathematics III 7.5.
/// Metadata maps L01 source examples into the existing typed #600 engine.
/// NOT registered or eligible for learner Practice without approvals.
/// </summary>
public static class UsCcssHsAdvTrigGeoU01L01PracticeCandidate
{
    public const string LessonCode = "PED:US-CCSS-MATH:HS:FOURTH:ADV-TRIG-GEO:U01:L01";
    public const string SourceFamily = "trig.unit_circle_reflection_exact";
    public const string ExistingKernelFamily = "trig.unit_circle_source_exact_angles";
    public const string SkillId = "supporting.trigonometry.core";
    public const string Version = "usccss.hs.adv_trig_geo.u01.l01.pending.v1";
    public const string Readiness = "INTEGRATION_PENDING";
    public const int RequiredScorableQuestions = 3;
    public const int TeacherProofRubricMaxPoints = 6;

    public static bool StudentPracticeRoutingAuthorized => false;
    public static bool AcademicApprovalRecorded => false;
    public static bool ContentPredecessorApproved => false;
    public static bool MayAutomaticallyScoreHumanProof => false;

    public sealed record CandidateCase(
        string Id, int Twelfths, int FunctionCode, int TransformCode,
        string ExpectedStatus, string? Exact,
        IReadOnlyList<string> EquivalentForms,
        IReadOnlyList<string> IncorrectForms,
        IReadOnlyList<string> MalformedForms,
        bool ApproximationAllowedByDefault)
    {
        public bool IsScorable => ExpectedStatus == "OK" && Exact is not null;
    }

    // function sin=0, cos=1, tan=2; transform pi-theta=0,
    // pi+theta=1, 2pi-theta=2. All input units are radians.
    private static readonly CandidateCase[] Cases =
    [
        new("l01-reflection-cos", 2, 1, 0, "OK", "-sqrt(3)/2",
            ["-0.8660254037844386"], ["sqrt(3)/2", "-1/2"], ["cos(", "banana"], false),
        new("l01-rotation-sin", 4, 0, 1, "OK", "-sqrt(3)/2",
            ["-0.8660254037844386"], ["sqrt(3)/2"], ["sin(", "banana"], false),
        new("l01-fullturn-cos", 3, 1, 2, "OK", "sqrt(2)/2",
            ["1/sqrt(2)", "0.7071067811865476"], ["-sqrt(2)/2"], ["cos(", "banana"], false),
        new("l01-tangent-pole", 6, 2, 2, "OUT_OF_DOMAIN", null,
            [], ["0", "1"], ["tan(", "banana"], false)
    ];

    public static IReadOnlyList<CandidateCase> ReviewCases => Array.AsReadOnly(Cases);

    public static bool TryGetReviewCase(string? lessonCode, string? caseId,
        out CandidateCase? candidate)
    {
        candidate = null;
        if (!string.Equals(lessonCode, LessonCode, StringComparison.Ordinal)
            || string.IsNullOrWhiteSpace(caseId)) return false;
        candidate = Cases.FirstOrDefault(c =>
            string.Equals(c.Id, caseId, StringComparison.Ordinal));
        return candidate is not null;
    }

    /// <summary>For isolated reviewer use, not student generation or grading.</summary>
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
}
