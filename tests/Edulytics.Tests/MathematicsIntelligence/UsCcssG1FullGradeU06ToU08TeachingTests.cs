using Edulytics.Core.Curriculum;
using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Whole remaining grade content integration. Mathematical/student-task
/// acceptance, original licensed figures and teacher assessed activities are
/// explicitly separate, mandatory academic release checks.</summary>
public sealed class UsCcssG1FullGradeU06ToU08TeachingTests
{
    [Fact]
    public void ThirtyOneSourceBoundRepairsHaveUniqueNewLessonIdentifiers()
    {
        var documents=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments();
        var ids=documents.SelectMany(doc=>doc.Lessons
            .Where(x=>CommonCoreG1FullGradeUnit6To8ContentCorrections.IsTarget(doc,x))
            .Select(x=>x.LessonCode)).ToArray();
        Assert.Equal(31,ids.Length);
        Assert.Equal(31,ids.Distinct(StringComparer.Ordinal).Count());
        Assert.Contains("PED:US-CCSS-MATH:G1:U06:L03",ids);
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
        var indirectComparison=text("PED:US-CCSS-MATH:G1:U06:L03");
        Assert.Contains("Mai and Clare",indirectComparison);
        Assert.Contains("SIX distinct real classroom comparisons",indirectComparison);
        Assert.Contains("TEACHER'S DESK fit through the DOOR",indirectComparison);
        Assert.Contains("STUDENT DESK fit through the DOOR",indirectComparison);
        Assert.Contains("BOOKSHELF or the RUG",indirectComparison);
        Assert.Contains("FILE CABINET or the BOOKSHELF",indirectComparison);
        Assert.Contains("BOOKSHELF or the TEACHER'S DESK",indirectComparison);
        Assert.Contains("TEACHER'S DESK fit NEXT TO",indirectComparison);
        Assert.Contains("drawings, numbers or words for EACH",indirectComparison);
        Assert.Contains("does NOT fit widthwise",indirectComparison, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("not necessarily in the original pictured map",indirectComparison);
        Assert.Contains("SAME string reference",indirectComparison, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("12",indirectComparison); // invented numeric example, NOT false original source-map value
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
        var equations=text("PED:US-CCSS-MATH:G1:U06:L14");
        Assert.Contains("SIX separate story contexts",equations);
        Assert.Contains("CIRCLE TWO matching equations for EACH",equations);
        Assert.Contains("8+□=11 and 11-8=□",equations); // Jada
        Assert.Contains("19-□=11 and 19-11=□",equations); // Kiran
        Assert.Contains("10+□=16 and 16-10=□",equations); // Han and Lin
        Assert.Contains("9+5=14 and 5+9=14",equations); // Elena and Andre
        Assert.Contains("6+□=16 and 16-6=□",equations); // Noah and Tyler
        Assert.Contains("5+9=□ and □-9=5",equations); // Clare
        Assert.Contains("partner sorting",equations);
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
        var aquarium=text("PED:US-CCSS-MATH:G1:U08:L04");
        Assert.Contains("FOUR distinct CHANGE-UNKNOWN stories",aquarium);
        Assert.Contains("8+□=15",aquarium);  // Turtles
        Assert.Contains("17-□=5",aquarium);  // Penguins
        Assert.Contains("20-□=3",aquarium);  // Fish
        Assert.Contains("6+□=14",aquarium); // Stingrays and sea stars
        Assert.Contains("16-□=9",aquarium); // Andre and bullfrogs
        Assert.Contains("20-17=3",aquarium);
        Assert.Contains("6+8=14",aquarium);
        var planetarium=text("PED:US-CCSS-MATH:G1:U08:L05");
        Assert.Contains("FOUR different missing-part stories",planetarium);
        Assert.Contains("7+□=18",planetarium);   // First/second graders
        Assert.Contains("13+□=18",planetarium);  // Bright and dim stars
        Assert.Contains("6+□=15",planetarium);   // Diego and Tyler
        Assert.Contains("12+□=20",planetarium);  // Elena stickers
        Assert.Contains("17-9=□ and 9+□=17",planetarium);  // Noah two matches
        Assert.Contains("11+□=16 and 16-11=□",planetarium); // Kiran two matches
        Assert.Contains("9+17=□ does NOT",planetarium); // Original distractor
        Assert.Contains("16+11=□ is NOT",planetarium); // Original distractor
        var rides=text("PED:US-CCSS-MATH:G1:U08:L06");
        Assert.Contains("FOUR comparison stories",rides);
        Assert.Contains("ferris wheel holds 20 people and swings 14",rides);
        Assert.Contains("20-14=6 FEWER",rides);
        Assert.Contains("Diego 7, Priya 11, Jada 3, Kiran 6 and Noah 9",rides);
        Assert.Contains("WRITE and ANSWER TWO new questions",rides);
        Assert.Contains("11-3=8",rides);
        Assert.Contains("9-6=3",rides);
        Assert.Contains("each be a coherent and mathematically answerable question",rides);
        Assert.Contains("number 103",text("PED:US-CCSS-MATH:G1:U08:L07"));
        var classBook=text("PED:US-CCSS-MATH:G1:U08:L08");
        Assert.Contains("BETWEEN 20 AND 100",classBook);
        Assert.Contains("CLASS NUMBER BOOK",classBook);
        Assert.Contains("AT LEAST THREE",classBook);
        Assert.Contains("80+4=70+14=60+24",classBook);
        Assert.Contains("GALLERY WALK",classBook);
        Assert.Contains("Compare, How Close? and Write Numbers",classBook);
        Assert.Contains("70=24+46 is TRUE",text("PED:US-CCSS-MATH:G1:U08:L10"));
        var riddles=text("PED:US-CCSS-MATH:G1:U08:L09");
        // Original Student Facing starts with three possible numbers before
        // a fourth clue selects one. Validate the now-correct objective.
        Assert.Contains("34, 44, 54",riddles);
        Assert.Contains("final clue",riddles,StringComparison.OrdinalIgnoreCase);
        Assert.Contains("unique answer is 44",riddles);
    }
}
