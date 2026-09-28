using System.Text.Json;
using Edulytics.Core.Entities;
using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Services.AdaptivePractice;

public static class AdaptivePracticeSemanticIdentity
{
    public static string Resolve(AssessmentItem item)
    {
        ArgumentNullException.ThrowIfNull(item);

        if (string.IsNullOrWhiteSpace(item.GenerationFamily))
            return item.ExposureFingerprint;

        if (string.IsNullOrWhiteSpace(item.GenerationParametersJson))
            return item.GenerationFamily + "|" + item.ExposureFingerprint;

        try
        {
            using var document =
                JsonDocument.Parse(
                    item.GenerationParametersJson);

            var root = document.RootElement;
            var parameterNode =
                root.TryGetProperty(
                    "parameters",
                    out var nested) &&
                nested.ValueKind ==
                    JsonValueKind.Object
                    ? nested
                    : root;

            var parameters =
                new Dictionary<string, int>(
                    StringComparer.Ordinal);

            foreach (var property in
                     parameterNode.EnumerateObject())
            {
                if (property.Value.ValueKind ==
                        JsonValueKind.Number &&
                    property.Value.TryGetInt32(
                        out var value))
                {
                    parameters[property.Name] = value;
                }
            }

            return PracticeSemanticQuestionIdentityPolicy
                .Create(
                    item.GenerationFamily,
                    parameters)
                .Key;
        }
        catch (
            Exception exception) when (
            exception is JsonException or
            InvalidOperationException)
        {
            return item.GenerationFamily +
                "|" +
                item.ExposureFingerprint;
        }
    }
}
