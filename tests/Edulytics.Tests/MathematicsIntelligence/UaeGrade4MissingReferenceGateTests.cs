using Edulytics.Core.Curriculum;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4MissingReferenceGateTests
{
    [Fact] public void MissingExternalReferenceNodesDoNotProduceInventedOfficialLinks()
    {
        var doc=PedagogicalLessonBlueprintRegistry.LoadEmbeddedDocuments()
            .Single(x=>x.BlueprintCode=="UAE-G4-COMMON-T1-OGL-V1:OGL-V1");
        Assert.Equal(132,doc.Lessons.Count);
        Assert.Equal(49,doc.Lessons.Count(x=>string.IsNullOrWhiteSpace(x.OfficialReferenceCode)));
        Assert.Equal(83,doc.Lessons.Count(x=>!string.IsNullOrWhiteSpace(x.OfficialReferenceCode)));
        Assert.All(doc.Lessons.Where(x=>string.IsNullOrWhiteSpace(x.OfficialReferenceCode)),
            x=>Assert.Empty(x.Alignments));
    }
}
