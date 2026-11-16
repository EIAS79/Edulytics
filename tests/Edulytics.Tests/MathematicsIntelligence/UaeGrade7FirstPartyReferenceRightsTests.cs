using Edulytics.Core.Curriculum;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Do not misclassify reference-only textbooks as open-licensed source text.</summary>
public sealed class UaeGrade7FirstPartyReferenceRightsTests
{
    [Fact]
    public void Grade7OwnerAuthoredBlueprint_ValidatesWithoutPretendingPublisherBookIsOpenLicensed()
    {
        var g7 = PedagogicalLessonBlueprintRegistry.LoadEmbeddedDocuments()
            .Single(x => x.PackCode == MathematicsCurriculumPackRegistry.UaeCode &&
                         x.LogicalLevel == 7 && x.Pathway == "General");
        Assert.Equal(78, g7.Lessons.Count);
        Assert.Equal("Reference-only local copyrighted student text; original Edulytics content, no reuse license presumed", g7.SourceLicense);
        Assert.False(PedagogicalSourceLicensePolicy.IsApproved(g7.SourceLicense));
        Assert.Contains("not copied", g7.SourceRightsNote);
        PedagogicalLessonBlueprintContract.Validate(g7);
    }

    [Fact]
    public void RemovingTheFirstPartyNoCopyRepresentation_FailsClosed()
    {
        var g7 = PedagogicalLessonBlueprintRegistry.LoadEmbeddedDocuments()
            .Single(x => x.PackCode == MathematicsCurriculumPackRegistry.UaeCode &&
                         x.LogicalLevel == 7 && x.Pathway == "General");
        g7.SourceRightsNote = "Publisher artwork was copied without permission";
        var ex = Assert.Throws<InvalidOperationException>(
            () => PedagogicalLessonBlueprintContract.Validate(g7));
        Assert.Contains("source license is not approved", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void UnknownSourceLicenseCannotBeApprovedWithOnlyAnOwnershipAssertion()
    {
        var g7 = PedagogicalLessonBlueprintRegistry.LoadEmbeddedDocuments()
            .Single(x => x.PackCode == MathematicsCurriculumPackRegistry.UaeCode &&
                         x.LogicalLevel == 7 && x.Pathway == "General");
        g7.SourceLicense = "Unverified publisher reproduction rights";
        Assert.Throws<InvalidOperationException>(
            () => PedagogicalLessonBlueprintContract.Validate(g7));
    }
}
