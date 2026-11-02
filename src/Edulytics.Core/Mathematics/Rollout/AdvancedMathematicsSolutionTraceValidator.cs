using Edulytics.Core.Mathematics.Solving;

namespace Edulytics.Core.Mathematics.Rollout;

public sealed record AdvancedMathematicsTraceValidation(
    bool IsValid,
    IReadOnlyList<string> Diagnostics);

public static class AdvancedMathematicsSolutionTraceValidator
{
    public const int MaximumSteps = 32;
    public const int MaximumTextLength = 600;

    public static AdvancedMathematicsTraceValidation Validate(
        MathematicsSolutionTrace? trace)
    {
        if (trace is null ||
            trace.Steps.Count == 0)
        {
            return Invalid(
                "TRACE_REQUIRED");
        }

        if (trace.Steps.Count > MaximumSteps)
        {
            return Invalid(
                "TRACE_STEP_LIMIT_EXCEEDED");
        }

        var ids =
            new HashSet<string>(
                StringComparer.Ordinal);

        foreach (var step in trace.Steps)
        {
            if (step is null ||
                string.IsNullOrWhiteSpace(
                    step.StepId) ||
                step.StepId.Length > 120 ||
                !ids.Add(step.StepId))
            {
                return Invalid(
                    "TRACE_STEP_ID_INVALID");
            }

            if (step.Before is null ||
                step.After is null)
            {
                return Invalid(
                    "TRACE_STATE_MISSING");
            }

            if (string.IsNullOrWhiteSpace(
                    step.RuleId) ||
                step.RuleId.Length > 160 ||
                string.IsNullOrWhiteSpace(
                    step.Operation) ||
                step.Operation.Length >
                    MaximumTextLength ||
                string.IsNullOrWhiteSpace(
                    step.Justification) ||
                step.Justification.Length >
                    MaximumTextLength)
            {
                return Invalid(
                    "TRACE_TEXT_INVALID");
            }
        }

        return new AdvancedMathematicsTraceValidation(
            true,
            []);
    }

    private static AdvancedMathematicsTraceValidation Invalid(
        string diagnostic) =>
        new(
            false,
            [diagnostic]);
}
