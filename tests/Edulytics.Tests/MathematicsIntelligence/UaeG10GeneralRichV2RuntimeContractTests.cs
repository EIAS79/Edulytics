using Edulytics.Core.Curriculum;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class UaeG10GeneralRichV2RuntimeContractTests
{
    [Fact]
    public void All_148_grade10_Arabic_lessons_load_through_strict_compiled_registry()
    {
        var blueprint = Assert.Single(PedagogicalLessonBlueprintRegistry.LoadEmbeddedDocuments(),
            x => x.PackCode == "UAE-MOE-MATH" && x.LogicalLevel == 10 && x.Pathway == "General");
        Assert.Equal(148, blueprint.Lessons.Count);
        var rows = RichLessonContentV2Registry.AllDocuments
            .SelectMany(d => d.Lessons)
            .Where(l => l.LessonCode.StartsWith("PED:UAE-MOE-MATH:L10:GENERAL:", StringComparison.Ordinal)
                && l.CultureCode == "ar").ToArray();
        Assert.Equal(148, rows.Length);
        Assert.Equal(148, rows.Select(x => x.LessonCode).Distinct(StringComparer.Ordinal).Count());
        foreach (var b in blueprint.Lessons)
        {
            var l = RichLessonContentV2Registry.Find(b.LessonCode, "ar");
            Assert.NotNull(l);
            Assert.Equal("ar", l.CultureCode);
            Assert.Equal(b.Title, l.Title);
            Assert.All(l.WorkedExamples, e => Assert.True(e.Steps.Count >= 3, e.Title));
            Assert.All(l.CommonMistakes, m => Assert.False(string.IsNullOrWhiteSpace(m.Mistake)));
        }
        var tables = rows.SelectMany(x => x.Visuals)
            .Where(v => v.Kind == RichLessonVisualKind.ComparisonTable).ToArray();
        Assert.True(tables.Length >= 340);
        Assert.All(tables, t => Assert.NotEmpty(t.Rows));
        Assert.Equal(18, (int)RichLessonVisualKind.CoordinatePlane);
        Assert.Equal(19, (int)RichLessonVisualKind.ComparisonTable);
    }
}
