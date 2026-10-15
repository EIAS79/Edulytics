using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Original IM teacher lesson purpose checks for three independent lessons,
/// never a collective L1-L6 approval.
/// </summary>
public sealed class UsCcssB01V9OffTopicClockConceptCorrectionsTests
{
    private const string Prefix = "PED:US-CCSS-MATH:";
    private static readonly string[] Codes =
    [
        Prefix + "G3:U01:L09",
        Prefix + "G3:U03:L04",
        Prefix + "G3:U03:L20"
    ];
    private const string WrongClockText = "Read the hour and minute hands separately";

    [Fact]
    public void OnlyThreeSourceVerifiedLessonsLoseTheUnrelatedClockRule()
    {
        var docs = MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments();
        var selected = docs.SelectMany(d => d.Lessons.Select(l => (d,l)))
            .Where(x => CommonCoreB01V9OffTopicClockConceptCorrections.IsTarget(x.d,x.l))
            .OrderBy(x => x.l.LessonCode)
            .ToArray();
        Assert.Equal(Codes,selected.Select(x => x.l.LessonCode).ToArray());
        Assert.Equal(3,CommonCoreB01V9OffTopicClockConceptCorrections.ExpectedCount);

        foreach(var (d,l) in selected)
        {
            var body = Assert.Single(l.Translations);
            Assert.DoesNotContain(WrongClockText,body.KeyConceptsAndRules);
            Assert.Equal(CommonCoreB01V9OffTopicClockConceptCorrections.CorrectedVersion,
                CanonicalLessonContentMaterializer.GetEffectiveContentVersion(d,l));
            Assert.False(CommonCoreB01V9OffTopicClockConceptCorrections
                .CanUpgradeExisting(d,l,"unverified-version"));
        }

        Assert.Contains("equal-sized groups", selected[0].l.Translations.Single()
            .KeyConceptsAndRules, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("ten ones", selected[1].l.Translations.Single()
            .KeyConceptsAndRules, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("intermediate quantity", selected[2].l.Translations.Single()
            .KeyConceptsAndRules, StringComparison.OrdinalIgnoreCase);

        var actualClock = docs.SelectMany(d => d.Lessons)
            .Single(l => l.LessonCode == Prefix + "G3:U06:L09");
        Assert.Contains("minute",actualClock.Translations.Single().Explanation,
            StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void CorrectionIsIdempotentAndNeverTouchesOtherLessons()
    {
        var docs=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments();
        var before=docs.SelectMany(d => d.Lessons.Select(l => (d,l)))
            .ToDictionary(x => x.l.LessonCode,
                x => CanonicalLessonContentMaterializer.ComputeLessonFingerprint(x.d,x.l));
        foreach(var doc in docs)
            CommonCoreB01V9OffTopicClockConceptCorrections.Apply(doc);
        var after=docs.SelectMany(d => d.Lessons.Select(l => (d,l)))
            .ToDictionary(x => x.l.LessonCode,
                x => CanonicalLessonContentMaterializer.ComputeLessonFingerprint(x.d,x.l));
        Assert.Equal(before,after);
    }

    [Fact]
    public void ExactRawRichContentPredecessorsMayUpgradeWithoutSkippingSafety()
    {
        var docs = MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments();
        foreach (var code in Codes)
        {
            var pair = docs.SelectMany(d => d.Lessons.Select(l => (Doc: d, Lesson: l)))
                .Single(x => x.Lesson.LessonCode == code);
            Assert.True(CommonCoreB01V9OffTopicClockConceptCorrections
                .CanUpgradeExisting(pair.Doc, pair.Lesson,
                    CommonCoreRichContentCorrections.CorrectionContentVersion));
            Assert.True(CommonCoreB01V9OffTopicClockConceptCorrections
                .CanUpgradeExisting(pair.Doc, pair.Lesson,
                    CommonCoreB01V9OffTopicClockConceptCorrections.CorrectedVersion));
            Assert.False(CommonCoreB01V9OffTopicClockConceptCorrections
                .CanUpgradeExisting(pair.Doc, pair.Lesson, "non-reviewed-version"));
            var t=pair.Lesson.Translations.Single();
            var approved = code.EndsWith("G3:U01:L09",StringComparison.Ordinal)
                ? CommonCoreB01V9SharedComparisonProcedureCorrections
                    .MatchesOriginalOrFinalProcedure(code,t.StepByStepSolutions,t.CommonMistakes)
                : CommonCoreB01V9SharedAdditionProcedureCorrections
                    .MatchesOriginalOrFinalProcedure(code,t.StepByStepSolutions,t.CommonMistakes);
            Assert.True(approved);
        }
    }

    [Fact]
    public void SourceFingerprintAndUnexpectedTextDriftFailClosed()
    {
        var doc=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .Single(d => d.Lessons.Any(l => l.LessonCode == Prefix + "G3:U03:L04"));
        var lesson=doc.Lessons.Single(l => l.LessonCode == Prefix + "G3:U03:L04");
        var savedSha=lesson.CanonicalBodySha256;
        try
        {
            lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(() =>
                CommonCoreB01V9OffTopicClockConceptCorrections.Apply(doc));
        }
        finally { lesson.CanonicalBodySha256=savedSha; }

        var t=Assert.Single(lesson.Translations);
        var old=t.KeyConceptsAndRules;
        try
        {
            t.KeyConceptsAndRules+="\nUNVERIFIED_VALUE";
            Assert.Throws<InvalidOperationException>(() =>
                CommonCoreB01V9OffTopicClockConceptCorrections.Apply(doc));
        }
        finally { t.KeyConceptsAndRules=old; }
    }
}
