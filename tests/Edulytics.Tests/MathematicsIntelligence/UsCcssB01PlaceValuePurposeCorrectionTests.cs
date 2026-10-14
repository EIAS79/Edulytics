using Edulytics.Core.Curriculum;
using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// G2 U05 L06 source purpose: represent three-digit numbers in words,
/// expanded form, base ten numerals, and unit form. No measuring ruler steps.
/// This is content regression, not complete academic certification.
/// </summary>
public sealed class UsCcssB01PlaceValuePurposeCorrectionTests
{
    private const string Code = CommonCoreB01PlaceValuePurposeCorrection.LessonCode;

    [Fact]
    public void RealLearnerContentHasCorrectNumberRepresentationRatherThanMeasuringAdvice()
    {
        var all = MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments();
        var candidates = all.Where(d => d.PackCode=="US-CCSS-MATH" &&
            d.VersionCode=="CCSSM-2010").SelectMany(d=>d.Lessons).Where(l=>l.LessonCode==Code).ToArray();
        var l = Assert.Single(candidates);
        var t = Assert.Single(l.Translations);
        Assert.Equal("en",t.CultureCode);
        Assert.Contains("400 + 30 + 6",t.KeyConceptsAndRules);
        Assert.Contains("four hundred thirty-six",t.WorkedExamples);
        Assert.Contains("seven hundred five",t.WorkedExamples);
        Assert.Contains("Step 1: Separate the given quantity",t.StepByStepSolutions);
        Assert.Contains("no tens",t.CommonMistakes);
        Assert.DoesNotContain("In previous lessons, you learned",t.Explanation);
        Assert.DoesNotContain("In previous lessons, you learned",t.QuickSummary);
        Assert.DoesNotContain("Which one doesn’t belong?",t.WorkedExamples);
        Assert.DoesNotContain("Represent 627",t.WorkedExamples);
        Assert.DoesNotContain("Represent 248",t.WorkedExamples);
        Assert.Equal(CommonCoreB01PlaceValuePurposeCorrection.CorrectedVersion,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(
                all.Single(d => d.Lessons.Contains(l)),l));
        Assert.DoesNotContain("Choose the required unit, align equal units",t.WorkedExamples);
        Assert.DoesNotContain("Step 1: Identify what needs to be measured",t.StepByStepSolutions);
        Assert.DoesNotContain("Measurement units must be equal in size",t.KeyConceptsAndRules);
        Assert.Equal("CCSS:2.NBT.A.3",Assert.Single(l.OutcomeCodes));
    }
    [Fact]
    public void ReapplicationIsIdempotentAndUnexpectedRawFingerprintFailsClosed()
    {
        var d=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .Single(d => d.PackCode=="US-CCSS-MATH" && d.Lessons.Any(l=>l.LessonCode==Code));
        var l=d.Lessons.Single(l=>l.LessonCode==Code);
        var before=CanonicalLessonContentMaterializer.ComputeLessonFingerprint(d,l);
        CommonCoreB01PlaceValuePurposeCorrection.Apply(d);
        Assert.Equal(before,CanonicalLessonContentMaterializer.ComputeLessonFingerprint(d,l));
        var sha=l.CanonicalBodySha256;
        try
        {
            l.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>CommonCoreB01PlaceValuePurposeCorrection.Apply(d));
        }
        finally { l.CanonicalBodySha256=sha; }
    }
    [Fact]
    public void RefusesPartialChangesButDoesNotTouchOtherLessons()
    {
        var docs=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments();
        var d=docs.Single(d=>d.PackCode=="US-CCSS-MATH" && d.Lessons.Any(l=>l.LessonCode==Code));
        var l=d.Lessons.Single(l=>l.LessonCode==Code);
        var t=Assert.Single(l.Translations);
        var original=t.StepByStepSolutions;
        try
        {
            t.StepByStepSolutions+=" Tampered.";
            Assert.Throws<InvalidOperationException>(()=>CommonCoreB01PlaceValuePurposeCorrection.Apply(d));
        }
        finally{t.StepByStepSolutions=original;}
        foreach(var (document,lesson) in docs.SelectMany(doc=>doc.Lessons.Select(lesson=>(document:doc,lesson))))
            if(lesson.LessonCode!=Code)
                Assert.False(CommonCoreB01PlaceValuePurposeCorrection.IsTarget(document,lesson));
    }
}
