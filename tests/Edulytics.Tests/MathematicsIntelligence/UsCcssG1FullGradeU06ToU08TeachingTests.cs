using Edulytics.Core.Curriculum;
using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Whole remaining grade content integration. Mathematical/student-task
/// acceptance, original licensed figures and teacher assessed activities are
/// explicitly separate, mandatory academic release checks.</summary>
public sealed class UsCcssG1FullGradeU06ToU08TeachingTests
{
    [Fact]
    public void ThirtySourceBoundRepairsHaveUniqueNewLessonIdentifiers()
    {
        var documents=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments();
        var ids=documents.SelectMany(doc=>doc.Lessons
            .Where(x=>CommonCoreG1FullGradeUnit6To8ContentCorrections.IsTarget(doc,x))
            .Select(x=>x.LessonCode)).ToArray();
        Assert.Equal(30,ids.Length);
        Assert.Equal(30,ids.Distinct(StringComparer.Ordinal).Count());
        Assert.All(ids,id=>Assert.StartsWith("PED:US-CCSS-MATH:G1:",id));
        Assert.DoesNotContain("PED:US-CCSS-MATH:G1:U06:L02",ids);
        Assert.DoesNotContain("PED:US-CCSS-MATH:G1:U07:L14",ids);
        for(var n=1;n<=10;n++)
            Assert.Contains($"PED:US-CCSS-MATH:G1:U08:L{n:00}",ids);
    }
    [Fact]
    public void OriginalSourceBoundMaterializationPreservesMeasurementGeometryTimeAndRiddleGoals()
    {
        var docs=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments();
        var doc=docs.Single(d=>d.Lessons.Any(x=>x.LessonCode=="PED:US-CCSS-MATH:G1:U08:L10"));
        CanonicalLessonContentMaterializer.Materialize(doc);
        string text(string code)
        {
            var lesson=doc.Lessons.Single(x=>x.LessonCode==code);
            Assert.True(CommonCoreG1FullGradeUnit6To8ContentCorrections.IsTarget(doc,lesson));
            var expectedVersion=code.Contains(":U06:",StringComparison.Ordinal)
                ?CommonCoreG1FullGradeUnit6To8ContentCorrections.Unit6OriginalSourceVersion
                :code is "PED:US-CCSS-MATH:G1:U07:L01" or
                     "PED:US-CCSS-MATH:G1:U07:L03" or
                     "PED:US-CCSS-MATH:G1:U07:L09" or
                     "PED:US-CCSS-MATH:G1:U07:L10" or
                     "PED:US-CCSS-MATH:G1:U07:L11" or
                     "PED:US-CCSS-MATH:G1:U07:L12" or
                     "PED:US-CCSS-MATH:G1:U07:L17" or
                     "PED:US-CCSS-MATH:G1:U08:L01" or
                     "PED:US-CCSS-MATH:G1:U08:L02" or
                     "PED:US-CCSS-MATH:G1:U08:L03" or
                     "PED:US-CCSS-MATH:G1:U08:L04" or
                     "PED:US-CCSS-MATH:G1:U08:L05" or
                     "PED:US-CCSS-MATH:G1:U08:L06" or
                     "PED:US-CCSS-MATH:G1:U08:L07" or
                     "PED:US-CCSS-MATH:G1:U08:L10"
                ?CommonCoreG1FullGradeUnit6To8ContentCorrections.Unit7Unit8OriginalSourceVersion
                :CommonCoreG1FullGradeUnit6To8ContentCorrections.Version;
            Assert.Equal(expectedVersion,
                CanonicalLessonContentMaterializer.GetEffectiveContentVersion(doc,lesson));
            var tr=lesson.Translations.Single(t=>t.CultureCode=="en");
            return tr.Explanation+"\n"+tr.WorkedExamples+"\n"+tr.StepByStepSolutions+"\n"+tr.CommonMistakes;
        }
        Assert.Contains("SHORTER than the pupil's FOOT",text("PED:US-CCSS-MATH:G1:U06:L01"));
        Assert.Contains("LONGER than the pupil's POINTER FINGER",text("PED:US-CCSS-MATH:G1:U06:L01"));
        Assert.Contains("THREE NEW objects",text("PED:US-CCSS-MATH:G1:U06:L01"));
        Assert.Contains("Get Your Numbers in Order",text("PED:US-CCSS-MATH:G1:U06:L01"));
        Assert.Contains("no gaps or overlaps",text("PED:US-CCSS-MATH:G1:U06:L06"));
        Assert.Contains("SEVEN digit cards",text("PED:US-CCSS-MATH:G1:U06:L04"));
        Assert.Contains("TWO two-digit numbers",text("PED:US-CCSS-MATH:G1:U06:L04"));
        Assert.Contains("SIX different tape strips A–F",text("PED:US-CCSS-MATH:G1:U06:L06"));
        Assert.Contains("Jeison's foot",text("PED:US-CCSS-MATH:G1:U06:L07"));
        Assert.Contains("MORE THAN 100 same-size small cubes",text("PED:US-CCSS-MATH:G1:U06:L08"));
        Assert.Contains("BEAVER, DOG, GIANT ANTEATER",text("PED:US-CCSS-MATH:G1:U06:L09"));
        Assert.Contains("24+30=44 FALSE",text("PED:US-CCSS-MATH:G1:U06:L10"));
        Assert.Contains("Priya's length is 10",text("PED:US-CCSS-MATH:G1:U06:L12"));
        Assert.Contains("UNKNOWN starting count is 20",text("PED:US-CCSS-MATH:G1:U06:L13"));
        Assert.Contains("TWO valid equations per story",text("PED:US-CCSS-MATH:G1:U06:L14"));
        Assert.Contains("9 were TAKEN away",text("PED:US-CCSS-MATH:G1:U06:L15"));
        Assert.Contains("Counting Collections Stage 3",text("PED:US-CCSS-MATH:G1:U06:L16"));
        Assert.Contains("two equal rectangles has halves",text("PED:US-CCSS-MATH:G1:U07:L09"));
        Assert.Contains("9+8=9+1+7=17",text("PED:US-CCSS-MATH:G1:U08:L03"));
        Assert.Contains("FEELS an object hidden in a bag",text("PED:US-CCSS-MATH:G1:U07:L01"));
        Assert.Contains("GROUP 1, GROUP 2 and GROUP 3",text("PED:US-CCSS-MATH:G1:U07:L03"));
        Assert.Contains("CIRCLE and a SQUARE",text("PED:US-CCSS-MATH:G1:U07:L09"));
        Assert.Contains("NOT EQUAL PIECES",text("PED:US-CCSS-MATH:G1:U07:L10"));
        Assert.Contains("Priya says she wants HALF",text("PED:US-CCSS-MATH:G1:U07:L11"));
        Assert.Contains("Can You Draw It?",text("PED:US-CCSS-MATH:G1:U07:L12"));
        Assert.Contains("Which One? Grade 1 Shapes",text("PED:US-CCSS-MATH:G1:U07:L17"));
        Assert.Contains("PERSONAL index card",text("PED:US-CCSS-MATH:G1:U08:L01"));
        Assert.Contains("4+□=9",text("PED:US-CCSS-MATH:G1:U08:L02"));
        Assert.Contains("Heads Up",text("PED:US-CCSS-MATH:G1:U08:L03"));
        Assert.Contains("16-□=9",text("PED:US-CCSS-MATH:G1:U08:L04"));
        Assert.Contains("7 first graders",text("PED:US-CCSS-MATH:G1:U08:L05"));
        Assert.Contains("WRITE and ANSWER TWO new questions",text("PED:US-CCSS-MATH:G1:U08:L06"));
        Assert.Contains("number 103",text("PED:US-CCSS-MATH:G1:U08:L07"));
        Assert.Contains("70=24+46 is TRUE",text("PED:US-CCSS-MATH:G1:U08:L10"));
        var riddles=text("PED:US-CCSS-MATH:G1:U08:L09");
        // Original Student Facing starts with three possible numbers before
        // a fourth clue selects one. Validate the now-correct objective.
        Assert.Contains("34, 44, 54",riddles);
        Assert.Contains("final clue",riddles,StringComparison.OrdinalIgnoreCase);
        Assert.Contains("unique answer is 44",riddles);
    }
}
