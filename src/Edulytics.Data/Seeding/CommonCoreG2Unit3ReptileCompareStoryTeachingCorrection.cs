using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
namespace Edulytics.Data.Seeding;
/// <summary>Local 2021 teacher source G2 U03 L06, correct reversed compare keywords.</summary>
public static class CommonCoreG2Unit3ReptileCompareStoryTeachingCorrection
{
    public const string Version="usccss-g2-u03-l06-reptile-bigger-smaller-stories-v1";
    private const string Code="PED:US-CCSS-MATH:G2:U03:L06";
    private const string Url="https://im.kendallhunt.com/k5/teachers/grade-2/unit-3/lesson-6/preparation.html";
    private const string Sha="aa47e81f4f8817f05e7dc5ca81227d40f4585d7e20618e6af4d18e91515dddf2";
    private sealed record Six(string[] Before,string[] After);
    private static readonly Six S=new(
        ["Learning goal: You will solve Compare story problems within 100 involving length.\nIn previous lessons, you measured lengths and determined how much longer one object is than another using centimeters. You have also developed strategies for adding and subtracting numbers within 100. This lesson combines these skills.",
         "Key ideas:\n- Solve addition and subtraction story problems about length\n- In previous lessons, you measured lengths and determined how much longer one object is than another using centimeters\n- Measurement units must be equal in size and placed or interpreted without gaps, overlaps, or a shifted starting point.",
         "Example 1: Find the value of each expression mentally. \\(5 + 5\\) \\(15 + 5\\) \\(15 + 15\\) \\(15 + 25\\)\nApproach: “How does \\(15 + 5\\) compare to \\(5 + 5\\) ?” (It’s 10 more) “How does \\(15 + 15\\) compare to \\(15 + 5\\) ?” (It’s 10 more) “How does \\(15 + 25\\) compare to \\(15 + 15\\) ?” (I see \\(10 + 20 = 30\\) and the \\(5 + 5\\) is 10 more.)\nExample 2: Lin's pet lizard is 62 cm long. It is 19 cm shorter than Jada's. How long is Jada's pet lizard? Whose pet is longer? ___ Circle the diagram that matches the story. Solve. Show your thinking. Jada’s pet lizard is ___ cm long. Diego and Mai have pet snakes. Mai’s snake is 17 cm longer than Diego’s. Mai’s snake is 71 cm. How long is Diego’s pet snake? Whose...\nApproach: “How does the diagram match the story problem?” (You can see that Mai’s rectangle is longer and there is a question mark for Diego’s pet.) “How did you know which pet was shorter?” (It said that Mai’s snake was 17 cm longer, so Diego’s is shorter.)",
         "Step 1: Identify what needs to be measured.\nStep 2: Choose the required unit and correct starting point.\nStep 3: Place or interpret equal-size units accurately.\nStep 4: Count or calculate the units and record the measure.\nStep 5: Check the endpoint, scale, and unit label.",
         "Common mistake: using unequal units, leaving gaps, overlapping units, or starting at the wrong point. Check the unit and endpoints.",
         "You will solve Compare story problems within 100 involving length. In previous lessons, you measured lengths and determined how much longer one object is than another using centimeters."],
        ["G2 Unit 3 Lesson 6 Compare Reptile Lengths in Story Problems (IM K–5 2021). This is a story-PROBLEM lesson about centimeters, not a repeat lesson on physically placing rulers. Warm-up Number Talk: 5+5=10,15+5=20,15+15=30,15+25=40; every subsequent expression grows by ten. Activity 1 Whose Pet Is Longer? uses MLR6 Three Reads to interpret comparison wording. Lin's lizard is 62 cm and is 19 cm SHORTER than Jada's; Jada's length is UNKNOWN BIGGER, so 62+19=81 cm despite the word 'shorter.' Mai's snake is 71 cm and 17 cm LONGER than Diego's; Diego's length is UNKNOWN SMALLER, so 71−17=54 cm despite 'longer.' Students choose matching source tape diagrams and explain which pet is longer or shorter. Activity 2 Guess My Reptiles is genuine partner A/B guessing work: Partner A's reptile list day gecko28 cm, komodo dragon98 cm, baby cobra46 cm, iguana65 cm; Partner B's list ribbon snake83 cm, gila monster55 cm, baby alligator71 cm, ringneck snake38 cm. Students SECRETLY choose one from each, construct a mathematically valid difference-unknown comparison and partner solves bigger or smaller unknown using diagrams and meaningful clues. Teacher checks actual pair work and MLR8 discussion; independent Edulytics digital comparison practice cannot prove it happened.",
         "In a compare story, write the known lengths and mark the unknown on a tape diagram. If a known smaller reptile is 19 cm SHORTER than an unknown bigger, add: bigger=smaller+19. If a known bigger reptile is 17 cm LONGER than unknown smaller, subtract: smaller=bigger−17. 'Shorter' does NOT automatically mean subtract; 'longer' does NOT automatically mean add: the unknown's role decides. For two known reptiles, difference = longer−shorter, always in centimeters. Source partner cards: A day gecko28, komodo dragon98, baby cobra46, iguana65; B ribbon snake83, gila monster55, baby alligator71, ringneck snake38 (all cm). Each pair makes genuine SECRET choices from opposite lists. For example A day gecko28 and B gila monster55 differ by27 cm, and a matching tape diagram's difference segment is27. Measuring cm is background from earlier lessons: a meter=100 cm, but this lesson's main work is numerical compare-story reasoning. Addressing 2.MD.B.5,2.NBT.B.5,2.OA.A.1,2.OA.B.2.",
         "PUBLIC SOURCE Number Talk: 5+5=10,15+5=20,15+15=30,15+25=40. PUBLIC SOURCE Activity1: Lin lizard62 cm,19 cm shorter than Jada => Jada 62+19=81 cm and Jada longer. Mai snake71 cm,17 cm longer than Diego => Diego71−17=54 cm and Diego shorter. SOURCE Activity2 A/B lists: A day gecko28, komodo dragon98, baby cobra46, iguana65; B ribbon snake83, gila monster55, baby alligator71, ringneck snake38, all cm. FIRST-PARTY optional transparent model (not a claim of an actual hidden partner choice): A gecko28 vs B gila55, gap55−28=27 cm; A iguana65 vs B ringneck38 => difference27 cm and iguana longer. INDEPENDENT Edulytics exit (not restricted Kiran and Han Compare Pets): a lizard48 cm is16 cm shorter than another => longer64 cm; a snake92 cm is27 cm longer than another => shorter65 cm; reptiles73 and39 cm have gap34 cm.",
         "1. Warm-up answer10,20,30,40, and explain why adding another ten changes consecutive expressions. 2. Use authentic teacher MLR6 Three Reads: first understand pet story, next label62 cm and19 cm difference, then reveal whose length is unknown. 3. For Lin shorter than Jada, draw TWO tape bars, mark Lin62 cm shorter and unknown Jada longer by19 cm. Solve62+19=81 cm. Check81−62=19. 4. For Mai longer than Diego, label Mai71 cm as LONGER bar and17 cm extra segment. Solve71−17=54 cm for Diego, check54+17=71. 5. Compare authentic source diagrams and explain why wording alone does not choose the operation. 6. In REAL partner guessing game, assign A and B. A keeps selection from four A reptiles secret, B from four B reptiles; use given SOURCE lengths. Choose one from each list and compute their numeric length difference. Compose a clue like 'My reptile is ___ cm and is ___ cm longer/shorter than one of yours'; check that both bars and statement truly match. 7. Exchange clues with a genuine partner and deduce the actual named reptiles with an original tape diagram or equation. Teacher checks whether it was a real secret choice and whether any clue is unambiguous. 8. Finish separately authored Edulytics numerical exit; do not treat it as publisher's teacher-only Kiran/Han assessment.",
         "Reading 'Lin is19 cm shorter' and subtracting62−19=43 to find Jada, even though Jada must be LONGER: correct81. Reading 'Mai is17 cm longer' and adding71+17=88 to find Diego, even though Diego is SHORTER: correct54. Swapping the partner lists or assigning unprovided reptile lengths. Computing the pair's sum instead of the length gap. Leaving off centimeters from numeric responses. Describing Guess My Reptiles as an automatic quiz that certifies actual hidden choices or peer conversations. Claiming correct virtual arithmetic proves measuring reptiles with rulers occurred; this lesson uses stated length measurements to solve compare problems. The original Kiran/Han cooldown requires teacher login and is not copied.",
         "Grade2 U03 L06 solves compare-length stories in cm. Number Talk10,20,30,40; Lin62 cm is19 cm shorter than Jada => Jada81 cm (ADD despite 'shorter'); Mai71 cm is17 cm longer than Diego => Diego54 cm (SUBTRACT despite 'longer'). Authentic secret partner guesses use eight listed reptiles A28/98/46/65 and B83/55/71/38. Digital independent examples and original secret partner work are assessed separately."]);
    public static bool IsTarget(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)=>
        d.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode&&
        d.VersionCode=="CCSSM-2010"&&d.ContentVersion=="phase30-common-core-rich-content-v1"&&l.LessonCode==Code;
    private static void Guard(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l)
    {
        if(!IsTarget(d,l)||l.SourceUrl!=Url||
            !string.Equals(l.CanonicalBodySha256,Sha,StringComparison.OrdinalIgnoreCase)||
            !l.OutcomeCodes.SequenceEqual(new[]{"CCSS:2.MD.B.5","CCSS:2.NBT.B.5","CCSS:2.OA.A.1","CCSS:2.OA.B.2"}))
            throw new InvalidOperationException("G2 U03 L06 source URL/SHA/ordered standards changed");
    }
    private static bool Equal(CanonicalLessonContentPackTranslation t,bool after)=>
        Matches([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],after);
    private static bool Equal(CurriculumLessonContentTranslation t,bool after)=>
        Matches([t.Explanation,t.KeyConceptsAndRules,t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,t.QuickSummary],after);
    private static bool Matches(string[] a,bool after)=>a.SequenceEqual(after?S.After:S.Before);
    public static void Apply(CanonicalLessonContentPackDocument d)
    {
        foreach(var l in d.Lessons)
        {
            if(!IsTarget(d,l))continue;Guard(d,l);
            var t=l.Translations.Single(x=>x.CultureCode=="en");
            if(Equal(t,true))continue;
            if(!Equal(t,false))throw new InvalidOperationException("G2 U03 L06 original six learner fields drifted");
            t.Explanation=S.After[0];t.KeyConceptsAndRules=S.After[1];
            t.WorkedExamples=S.After[2];t.StepByStepSolutions=S.After[3];
            t.CommonMistakes=S.After[4];t.QuickSummary=S.After[5];
        }
    }
    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string prior)=>
        IsTarget(d,l)?Version:prior;
    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string prior)=>
        IsTarget(d,l)&&prior==CommonCoreRichContentCorrections.CorrectionContentVersion;
    public static void ValidateExistingBodyBeforeUpgrade(CanonicalLessonContentPackDocument d,CanonicalLessonContentPackLesson l,string prior,IReadOnlyCollection<CurriculumLessonContentTranslation> rows)
    {
        Guard(d,l);
        if(!CanUpgradeExisting(d,l,prior))throw new InvalidOperationException("G2 U03 L06 unapproved persisted predecessor version");
        if(rows.Count==0)return;
        if(rows.Count!=1||rows.Single().CultureCode!="en")throw new InvalidOperationException("G2 U03 L06 previous rows changed");
        var stored=rows.Single(); var incoming=l.Translations.Single(x=>x.CultureCode=="en");
        if(stored.Title!=incoming.Title||!(Equal(stored,false)||Equal(stored,true)))
            throw new InvalidOperationException("G2 U03 L06 persisted six fields changed");
    }
}
