using System.Text.Json;
using Edulytics.Core.Mathematics.Curriculum;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class SupportingCatalogueOntologyR5Tests
{
    [Theory]
    [InlineData("Pythagoras theorem", "geometry")]
    [InlineData("Sine cosine and tangent", "trigonometry")]
    [InlineData("Add and subtract fractions", "fractions")]
    [InlineData("Linear equations", "algebra")]
    [InlineData("Mean, median and range", "statistics")]
    public void ResolverClassifiesCanonicalTargetDomain(string title, string domain)
    {
        var target = SupportingCatalogueTargetResolver.Resolve(title);

        Assert.NotNull(target);
        Assert.Equal(domain, target!.Domain);
        Assert.StartsWith($"supporting.{domain}.", target.TargetId, StringComparison.Ordinal);
    }

    [Fact]
    public void BuildApplyAdvancedAndConsolidatingVariantsReuseSameCanonicalTarget()
    {
        var a = SupportingCatalogueTargetResolver.Resolve("Pythagoras theorem");
        var b = SupportingCatalogueTargetResolver.Resolve("Pythagoras theorem — advanced reasoning");
        var c = SupportingCatalogueTargetResolver.Resolve("Consolidating Pythagoras theorem");
        var d = SupportingCatalogueTargetResolver.Resolve("Pythagoras theorem: Build the Idea");
        var e = SupportingCatalogueTargetResolver.Resolve("Pythagoras theorem: Reason and Apply");

        Assert.NotNull(a);
        Assert.Equal(a, b);
        Assert.Equal(a, c);
        Assert.Equal(a, d);
        Assert.Equal(a, e);
    }

    [Fact]
    public void DifferentMathematicalTargetsNeverCollapseToSameIdentity()
    {
        var pythagoras = SupportingCatalogueTargetResolver.Resolve("Pythagoras theorem");
        var surfaceArea = SupportingCatalogueTargetResolver.Resolve("Surface area and volume");
        var similarity = SupportingCatalogueTargetResolver.Resolve("Congruence and similarity");

        Assert.NotNull(pythagoras);
        Assert.NotNull(surfaceArea);
        Assert.NotNull(similarity);
        Assert.NotEqual(pythagoras!.TargetId, surfaceArea!.TargetId);
        Assert.NotEqual(pythagoras.TargetId, similarity!.TargetId);
        Assert.NotEqual(surfaceArea.TargetId, similarity.TargetId);
    }

    [Fact]
    public void EverySupportingCatalogueLessonHasAReusableCanonicalTargetIdentity()
    {
        var assembly = typeof(SupportingCatalogueTargetResolver).Assembly;
        var resources = assembly
            .GetManifestResourceNames()
            .Where(x => x.EndsWith(".lesson-content-pack.json", StringComparison.Ordinal))
            .ToArray();

        var supportingCount = 0;
        var targetIds = new HashSet<string>(StringComparer.Ordinal);

        foreach (var resource in resources)
        {
            using var stream = assembly.GetManifestResourceStream(resource)
                ?? throw new InvalidOperationException($"Cannot open {resource}.");
            using var document = JsonDocument.Parse(stream);

            if (!TryGetProperty(document.RootElement, "Lessons", "lessons", out var lessons) ||
                lessons.ValueKind != JsonValueKind.Array)
            {
                continue;
            }

            foreach (var lesson in lessons.EnumerateArray())
            {
                if (HasOfficialOutcomes(lesson))
                    continue;

                var title = EnglishTitle(lesson);
                var target = SupportingCatalogueTargetResolver.Resolve(title);

                supportingCount++;
                Assert.False(string.IsNullOrWhiteSpace(title));
                Assert.NotNull(target);
                targetIds.Add(target!.TargetId);
            }
        }

        Assert.True(supportingCount >= 1349);
        Assert.True(targetIds.Count > 100);
        Assert.True(targetIds.Count < supportingCount);
    }

    private static bool HasOfficialOutcomes(JsonElement lesson)
    {
        if (!TryGetProperty(lesson, "OutcomeCodes", "outcomeCodes", out var outcomes) ||
            outcomes.ValueKind != JsonValueKind.Array)
        {
            return false;
        }

        return outcomes.GetArrayLength() > 0;
    }

    private static string EnglishTitle(JsonElement lesson)
    {
        if (TryGetProperty(lesson, "Translations", "translations", out var translations) &&
            translations.ValueKind == JsonValueKind.Array)
        {
            JsonElement? fallback = null;
            foreach (var translation in translations.EnumerateArray())
            {
                fallback ??= translation;
                if (TryGetProperty(translation, "CultureCode", "cultureCode", out var culture) &&
                    culture.ValueKind == JsonValueKind.String &&
                    (culture.GetString() ?? string.Empty).StartsWith("en", StringComparison.OrdinalIgnoreCase))
                {
                    return StringProperty(translation, "Title", "title");
                }
            }

            if (fallback.HasValue)
                return StringProperty(fallback.Value, "Title", "title");
        }

        return StringProperty(lesson, "Title", "title");
    }

    private static string StringProperty(JsonElement element, string upper, string lower)
    {
        if (TryGetProperty(element, upper, lower, out var value) &&
            value.ValueKind == JsonValueKind.String)
        {
            return value.GetString()?.Trim() ?? string.Empty;
        }

        return string.Empty;
    }

    private static bool TryGetProperty(
        JsonElement element,
        string upper,
        string lower,
        out JsonElement value)
    {
        if (element.TryGetProperty(upper, out value))
            return true;
        return element.TryGetProperty(lower, out value);
    }
}
