using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>
/// V9 B01 independently inspected 2021 IM G3/G4 lesson-purpose preparations.
/// Twelve fraction lessons had the SAME unscoped measurement-ruler step template
/// despite separate fraction goals. Replaces only exact source-guarded legacy
/// phrases with original Edulytics-authored fraction explanations and steps.
/// Does not constitute full L1-L6 certification or license clearance.
/// </summary>
public static class CommonCoreB01FractionPurposeCorrections
{
    public const string CorrectedVersion = "us-ccss-b01-fraction-purpose-aligned-v1";
    private sealed record Repair(
        string Code, string Url, string SourceSha, string OutcomeCodes,
        string Title, string Explanation, string Summary,
        string OldConcepts, string OldExamples, string OldSteps, string OldMistakes,
        string NewConcepts, string NewExamples, string NewSteps, string NewMistakes);
    private static readonly Repair[] Repairs =
    [
        new Repair(
            "PED:US-CCSS-MATH:G3:U05:L02",
            "https://im.kendallhunt.com/k5/teachers/grade-3/unit-5/lesson-2/preparation.html",
            "1fee6fe6cb8a9f5bf7a79af1636903f6775881d4ffc28ed3a1de8684aabbca7b",
            "CCSS:3.NF.A.1",
            "Name Parts as Fractions",
            "Learning goal: You will partition shapes into equal parts and express each equal-size part as a unit fraction.\nPreviously, you partitioned rectangles that each represented 1 into fractional parts by folding. You now draw lines to partition a shape and use the fraction notation you learned to label each part as a unit fraction and describe a shaded part as a unit fraction.",
            "You will partition shapes into equal parts and express each equal-size part as a unit fraction. Previously, you partitioned rectangles that each represented 1 into fractional parts by folding.",
            "Key ideas:\n- Express the area of each part as a unit fraction of the whole\n- Partition shapes into halves, thirds, fourths, sixths, and eighths\n- Use mathematical attributes such as sides, angles, parallelism, or length—not size or orientation—to justify a classification.",
            "Example 1: Which one doesn’t belong? A Caption: A B Caption: B C Caption: C D Caption: D\nApproach: “Can we label the parts with fractions? Why or why not?” (We can label the parts in A, B, and D with fractions because you are equal in size, but not in C because the parts aren’t the same size.) “What fractions do we use to label the parts in A, B, and D?” ( \\(\\frac{1}{2}\\) in A and D, and \\(\\frac{1}{3}\\) in B.)\nExample 2: Partition each rectangle into halves, thirds, fourths, sixths, and eighths. Then label each part with the correct fraction. halves thirds fourths sixths eighths Partition the rectangle into equal-sized parts. Shade one of the parts. Trade rectangles with a partner. If the whole rectangle is 1, what number represents the shaded part? Explain your reasoning.\nApproach: For each rectangle, ask, “How did you know what fraction of the rectangle your partner shaded?” (I counted the equal parts in the rectangle. There were four equal parts, so I knew my partner shaded a fourth.)",
            "Step 1: Identify what needs to be measured.\nStep 2: Choose the required unit and correct starting point.\nStep 3: Place or interpret equal-size units accurately.\nStep 4: Count or calculate the units and record the measure.\nStep 5: Check the endpoint, scale, and unit label.",
            "Common mistake: using unequal units, leaving gaps, overlapping units, or starting at the wrong point. Check the unit and endpoints.",
            "Key ideas:\n- A unit fraction 1/b is one of b equal-area parts of the same whole.\n- Partition a shape into equal parts before naming one part; equal means equal area, not identical orientation.",
            "Example 1: Which one doesn’t belong? A Caption: A B Caption: B C Caption: C D Caption: D\nApproach: “Can we label the parts with fractions? Why or why not?” (We can label the parts in A, B, and D with fractions because you are equal in size, but not in C because the parts aren’t the same size.) “What fractions do we use to label the parts in A, B, and D?” ( \\(\\frac{1}{2}\\) in A and D, and \\(\\frac{1}{3}\\) in B.)\nExample 2: Partition each rectangle into halves, thirds, fourths, sixths, and eighths. Then label each part with the correct fraction. halves thirds fourths sixths eighths Partition the rectangle into equal-sized parts. Shade one of the parts. Trade rectangles with a partner. If the whole rectangle is 1, what number represents the shaded part? Explain your reasoning.\nApproach: For each rectangle, ask, “How did you know what fraction of the rectangle your partner shaded?” (I counted the equal parts in the rectangle. There were four equal parts, so I knew my partner shaded a fourth.)",
            "Step 1: Identify which shape is one whole.\nStep 2: Partition the whole into b pieces with equal areas.\nStep 3: Select one equal-area piece and label it 1/b.\nStep 4: Compare parts against the same whole and verify their areas are equal.",
            "Mistake: calling any one of b unequal pieces 1/b. Correct by repartitioning the same whole into b equal-area pieces."
        ),
        new Repair(
            "PED:US-CCSS-MATH:G3:U05:L03",
            "https://im.kendallhunt.com/k5/teachers/grade-3/unit-5/lesson-3/preparation.html",
            "509d068b69d6852703c798306b4698477c438356f21bebd32ac56bbb0bc4a682",
            "CCSS:3.NF.A.1",
            "Non-unit Fractions PLC Activity",
            "Learning goal: You will understand non-unit fractions.\nPreviously, you learned how to write unit fractions, using numbers of the form \\(\\frac{1}{b}\\) . You also partitioned rectangles and used unit fractions to describe one of the parts. In this lesson, you use diagrams with multiple equal parts shaded to make sense of how non-unit fractions are made of unit fractions.",
            "You will understand non-unit fractions. Previously, you learned how to write unit fractions, using numbers of the form \\(\\frac{1}{b}\\) .",
            "Key ideas:\n- Understand a fraction $\\frac{a}{b}$ as the quantity formed by $a$ parts of size $\\frac{1}{b}$\n- Previously, you learned how to write unit fractions, using numbers of the form \\(\\frac{1}{b}\\)\n- Measurement units must be equal in size and placed or interpreted without gaps, overlaps, or a shifted starting point.",
            "Example 1: Each shape in each row of the table represents 1. Use the shaded parts to complete the missing information in the table. Be prepared to explain your reasoning. Description: Table with shapes and information to find about the related fractions. Categories: number of shaded parts, size of each part, word name for the shaded parts, number name for the...\nApproach: “Now that the table is complete, what do you notice? What do you wonder?” (The size of each part always has a 1 in the top part of the fraction. The bottom part of the fraction is the same for each part and the total amount shaded. The top part of the fraction is how many parts are shaded.)",
            "Step 1: Identify what needs to be measured.\nStep 2: Choose the required unit and correct starting point.\nStep 3: Place or interpret equal-size units accurately.\nStep 4: Count or calculate the units and record the measure.\nStep 5: Check the endpoint, scale, and unit label.",
            "Common mistake: using unequal units, leaving gaps, overlapping units, or starting at the wrong point. Check the unit and endpoints.",
            "Key ideas:\n- A non-unit fraction a/b is made from a copies of the unit fraction 1/b.\n- The denominator counts equal parts per whole; the numerator counts how many of those parts are selected.",
            "Example 1: Each shape in each row of the table represents 1. Use the shaded parts to complete the missing information in the table. Be prepared to explain your reasoning. Description: Table with shapes and information to find about the related fractions. Categories: number of shaded parts, size of each part, word name for the shaded parts, number name for the...\nApproach: “Now that the table is complete, what do you notice? What do you wonder?” (The size of each part always has a 1 in the top part of the fraction. The bottom part of the fraction is the same for each part and the total amount shaded. The top part of the fraction is how many parts are shaded.)",
            "Step 1: Identify the whole and partition it into b equal-area parts.\nStep 2: Name one part 1/b.\nStep 3: Count or shade a parts of that unit size.\nStep 4: Write a/b and verify a pieces of 1/b were combined.",
            "Mistake: treating the numerator as the number of parts per whole. The denominator defines b equal parts; the numerator counts a of them."
        ),
        new Repair(
            "PED:US-CCSS-MATH:G3:U05:L04",
            "https://im.kendallhunt.com/k5/teachers/grade-3/unit-5/lesson-4/preparation.html",
            "ab52732b69104e670fb6e2be07fc55617ea15e421880cce8699c5dc3fdd49def",
            "CCSS:3.NF.A.1|CCSS:3.OA.C.7",
            "Build Fractions from Unit Fractions",
            "Learning goal: You will build non-unit fractions and whole numbers from unit fractions.\nIn the previous lesson, you named non-unit fractions and made sense of the notation used to write them. In this lesson, you play a game in which you build non-unit fractions from unit fractions (for example, you try to collect enough cards showing \\(\\frac{1}{6}\\) to make \\(\\frac{3}{6}\\) ). You record these fractions on a fraction strip diagram.",
            "You will build non-unit fractions and whole numbers from unit fractions. In the previous lesson, you named non-unit fractions and made sense of the notation used to write them.",
            "Key ideas:\n- Build non-unit fractions and whole numbers from unit fractions\n- In the previous lesson, you named non-unit fractions and made sense of the notation used to write them\n- Measurement units must be equal in size and placed or interpreted without gaps, overlaps, or a shifted starting point.",
            "Example 1: Find the value of each expression mentally. \\(3 \\times 3\\) \\(7 \\times 3\\) \\(10 \\times 3\\) \\(3 \\times 17\\)\nApproach: Choose the required unit, align equal units from the endpoint, measure without gaps or overlaps, and record the result with its unit.\nExample 2: The goal of the game is to be the first to build 2 secret fractions with unit fractions. Make two stacks: one for secret fractions and one for unit fractions. Place all cards face down. Each player draws 2 secret fraction cards. These are the fractions you are trying to make with your unit fractions. On your turn, you can make one of these moves: Pick up...\nApproach: “What strategies did you find helpful for building your secret fractions?” (When \\(\\frac{5}{8}\\) was my secret fraction, I was keeping track of how many \\(\\frac{1}{8}\\) cards I had to make \\(\\frac{5}{8}\\) . When I had \\(\\frac{3}{4}\\) as a secret fraction, I knew I needed \\(\\frac{1}{4}\\) cards, but I had a bunch of \\(\\frac{1}{6}\\) cards, so I traded for different secret fractions.)",
            "Step 1: Identify what needs to be measured.\nStep 2: Choose the required unit and correct starting point.\nStep 3: Place or interpret equal-size units accurately.\nStep 4: Count or calculate the units and record the measure.\nStep 5: Check the endpoint, scale, and unit label.",
            "Common mistake: using unequal units, leaving gaps, overlapping units, or starting at the wrong point. Check the unit and endpoints.",
            "Key ideas:\n- Build a target non-unit fraction a/b by combining a copies of the unit fraction 1/b.\n- Keep the whole and the denominator fixed when building, trading, or checking fraction-strip representations.",
            "Example 1: Find the value of each expression mentally. \\(3 \\times 3\\) \\(7 \\times 3\\) \\(10 \\times 3\\) \\(3 \\times 17\\)\nApproach: Use multiplication facts or equal groups to evaluate each mental-math expression; then use unit-fraction cards to build the target fractions.\nExample 2: The goal of the game is to be the first to build 2 secret fractions with unit fractions. Make two stacks: one for secret fractions and one for unit fractions. Place all cards face down. Each player draws 2 secret fraction cards. These are the fractions you are trying to make with your unit fractions. On your turn, you can make one of these moves: Pick up...\nApproach: “What strategies did you find helpful for building your secret fractions?” (When \\(\\frac{5}{8}\\) was my secret fraction, I was keeping track of how many \\(\\frac{1}{8}\\) cards I had to make \\(\\frac{5}{8}\\) . When I had \\(\\frac{3}{4}\\) as a secret fraction, I knew I needed \\(\\frac{1}{4}\\) cards, but I had a bunch of \\(\\frac{1}{6}\\) cards, so I traded for different secret fractions.)",
            "Step 1: Read the target fraction a/b and identify its unit part 1/b.\nStep 2: Choose or collect a cards representing 1/b.\nStep 3: Place the unit fractions together on one fraction strip without gaps or overlaps.\nStep 4: Count the a pieces, name the total a/b, and verify it matches the target card.",
            "Mistake: combining cards with different denominators as if every piece had the same size. Use copies of 1/b from the same whole to build a/b."
        ),
        new Repair(
            "PED:US-CCSS-MATH:G3:U05:L06",
            "https://im.kendallhunt.com/k5/teachers/grade-3/unit-5/lesson-6/preparation.html",
            "2d361034e44e6bbf2f8e97a319fca32fe9bcd88db11a3b5b9192f260193ed55a",
            "CCSS:3.NF.A.2",
            "Locate Unit Fractions on the Number Line",
            "Learning goal: You will partition the interval from 0 to 1and locate unit fractions within that interval.\nIn previous lessons, you made sense of number lines that were partitioned into fractions. While this lesson focuses on partitioning and locating unit fractions within the interval from 0 to 1, the number lines in this lesson vary in length. As you partition number lines in the rest of this unit, the most important thing is that you are accurate in relatively locating fractions on the number line and you understand the partitions should be equally spaced (MP6).",
            "You will partition the interval from 0 to 1and locate unit fractions within that interval. In previous lessons, you made sense of number lines that were partitioned into fractions.",
            "Key ideas:\n- Partition the interval from 0 to 1 and locate unit fractions within that interval\n- In previous lessons, you made sense of number lines that were partitioned into fractions\n- Measurement units must be equal in size and placed or interpreted without gaps, overlaps, or a shifted starting point.",
            "Example 1: Which one doesn’t belong? A Caption: A B Caption: B C Caption: C D Caption: D\nApproach: “To make your reasoning clear while locating and labeling fractions on a number line, what are some important things to include?” (Partitions of the equal parts, a dot and label at the fraction.) “How is this different from how we labeled our fraction strips like in A?” (In diagrams of fraction strips we labeled the part that has size or length \\(\\frac{1}{3}\\) . On the number line we are labeling the number...\nExample 2: Three students are partitioning a number line into fourths. Their work is shown. Clare’s number line: Andre’s number line: Diego’s number line: Whose partitioning makes the most sense to you? Explain your reasoning.\nApproach: Choose the required unit, align equal units from the endpoint, measure without gaps or overlaps, and record the result with its unit.",
            "Step 1: Identify what needs to be measured.\nStep 2: Choose the required unit and correct starting point.\nStep 3: Place or interpret equal-size units accurately.\nStep 4: Count or calculate the units and record the measure.\nStep 5: Check the endpoint, scale, and unit label.",
            "Common mistake: using unequal units, leaving gaps, overlapping units, or starting at the wrong point. Check the unit and endpoints.",
            "Key ideas:\n- On the number line, 1/b is the point one of b equal-length steps from 0 to 1.\n- Partition the entire unit interval into b equal-length intervals before labeling fractional endpoints.",
            "Example 1: Which one doesn’t belong? A Caption: A B Caption: B C Caption: C D Caption: D\nApproach: “To make your reasoning clear while locating and labeling fractions on a number line, what are some important things to include?” (Partitions of the equal parts, a dot and label at the fraction.) “How is this different from how we labeled our fraction strips like in A?” (In diagrams of fraction strips we labeled the part that has size or length \\(\\frac{1}{3}\\) . On the number line we are labeling the number...\nExample 2: Three students are partitioning a number line into fourths. Their work is shown. Clare’s number line: Andre’s number line: Diego’s number line: Whose partitioning makes the most sense to you? Explain your reasoning.\nApproach: Use the example's own equal intervals or the stated arithmetic expression to reason, then explain the location of each fraction on the number line.",
            "Step 1: Identify 0 and 1 on the number line.\nStep 2: Divide the interval from 0 to 1 into b equal-length pieces.\nStep 3: Locate the first endpoint after 0 and label it 1/b.\nStep 4: Check that b equal steps reach 1 exactly.",
            "Mistake: making unequal tick spaces but labeling each as the same unit fraction. Redivide the entire interval into b equal lengths."
        ),
        new Repair(
            "PED:US-CCSS-MATH:G3:U05:L07",
            "https://im.kendallhunt.com/k5/teachers/grade-3/unit-5/lesson-7/preparation.html",
            "0add60463e0de86e22049f7f150ae69100cdadd646e5b36067c286e1308bc6dd",
            "CCSS:3.NF.A.2",
            "Non-unit Fractions on the Number Line PLC Activity",
            "Learning goal: You will locate non-unit fractions on the number line.\nPreviously, you built non-unit fractions from unit fractions with diagrams and fraction strips. Now, you deepen your understanding of fractions on the number line as you locate and label non-unit fractions.",
            "You will locate non-unit fractions on the number line. Previously, you built non-unit fractions from unit fractions with diagrams and fraction strips.",
            "Key ideas:\n- Locate non-unit fractions on the number line (including fractions greater than 1)\n- Previously, you built non-unit fractions from unit fractions with diagrams and fraction strips\n- Measurement units must be equal in size and placed or interpreted without gaps, overlaps, or a shifted starting point.",
            "Example 1: Locate and label \\(\\frac{3}{4}\\) and \\(\\frac{6}{4}\\) . Locate and label \\(\\frac{7}{8}\\) and \\(\\frac{12}{8}\\) . Locate and label \\(\\frac{2}{3}\\) and \\(\\frac{4}{3}\\) . Locate and label \\(\\frac{2}{6}\\) and \\(\\frac{7}{6}\\) . How did you partition the number line when you were locating the numbers \\(\\frac{7}{8}\\) and \\(\\frac{12}{8}\\) ? Explain your reasoning....\nApproach: “How is counting by fractions the same as counting by whole numbers? How is it different?” (The top part of the fraction is just like counting by whole numbers, going up one. The bottom part is different because it doesn’t change.)\nExample 2: Partition the number line into any number of equal-size parts. Locate and mark, but don’t label, a fraction of your choice. Trade number lines with a partner. How did your partner partition their number line? What number did your partner mark on their number line? Explain your reasoning. If you have time, play the game again.\nApproach: “If I rolled a 4, and chose to move \\(\\frac{4}{4}\\) , how would you count the move?” (I would count 1, 2, 3, 4.) “How did you know you have moved \\(\\frac{4}{4}\\) ?” (Because each space is \\(\\frac{1}{4}\\) , so I need to move 4 times.) “How is counting along this number line the same and different than counting along your number lines?” (On the whole number one each space is 1 so we just count 1, 2, 3, 4. On our...",
            "Step 1: Identify what needs to be measured.\nStep 2: Choose the required unit and correct starting point.\nStep 3: Place or interpret equal-size units accurately.\nStep 4: Count or calculate the units and record the measure.\nStep 5: Check the endpoint, scale, and unit label.",
            "Common mistake: using unequal units, leaving gaps, overlapping units, or starting at the wrong point. Check the unit and endpoints.",
            "Key ideas:\n- A non-unit fraction a/b is a steps of size 1/b from 0 on the number line.\n- Fractions greater than 1 continue beyond the first unit interval using the same equal-length fractional step.",
            "Example 1: Locate and label \\(\\frac{3}{4}\\) and \\(\\frac{6}{4}\\) . Locate and label \\(\\frac{7}{8}\\) and \\(\\frac{12}{8}\\) . Locate and label \\(\\frac{2}{3}\\) and \\(\\frac{4}{3}\\) . Locate and label \\(\\frac{2}{6}\\) and \\(\\frac{7}{6}\\) . How did you partition the number line when you were locating the numbers \\(\\frac{7}{8}\\) and \\(\\frac{12}{8}\\) ? Explain your reasoning....\nApproach: “How is counting by fractions the same as counting by whole numbers? How is it different?” (The top part of the fraction is just like counting by whole numbers, going up one. The bottom part is different because it doesn’t change.)\nExample 2: Partition the number line into any number of equal-size parts. Locate and mark, but don’t label, a fraction of your choice. Trade number lines with a partner. How did your partner partition their number line? What number did your partner mark on their number line? Explain your reasoning. If you have time, play the game again.\nApproach: “If I rolled a 4, and chose to move \\(\\frac{4}{4}\\) , how would you count the move?” (I would count 1, 2, 3, 4.) “How did you know you have moved \\(\\frac{4}{4}\\) ?” (Because each space is \\(\\frac{1}{4}\\) , so I need to move 4 times.) “How is counting along this number line the same and different than counting along your number lines?” (On the whole number one each space is 1 so we just count 1, 2, 3, 4. On our...",
            "Step 1: Mark 0 and 1 and partition each unit into b equal intervals.\nStep 2: Count a steps of length 1/b from 0.\nStep 3: Mark the endpoint as a/b, continuing past 1 if a is greater than b.\nStep 4: Check the interval sizes and whether the fraction is less than, equal to, or greater than 1.",
            "Mistake: stopping at 1 for every fraction. If a is greater than b, continue equal 1/b steps beyond 1."
        ),
        new Repair(
            "PED:US-CCSS-MATH:G3:U05:L08",
            "https://im.kendallhunt.com/k5/teachers/grade-3/unit-5/lesson-8/preparation.html",
            "091f1d9f7cc8d5441b1fdc520a6c7c76e8208d9c67c315ac68f42e60f57c65ac",
            "CCSS:3.NF.A.2|CCSS:3.NF.A.3|CCSS:3.OA.C.7",
            "Fractions and Whole Numbers",
            "Learning goal: You will recognize fractions that are equivalent to whole numbers and, given the location of a unit fraction on the number line, to locate whole numbers.\nIn previous lessons, you learned to partition number lines and located and labeled fractions on the number line.",
            "You will recognize fractions that are equivalent to whole numbers and, given the location of a unit fraction on the number line, to locate whole numbers. In previous lessons, you learned to partition number lines and located and labeled fractions on the number line.",
            "Key ideas:\n- Locate whole numbers on the number line given the location of a unit fraction and express them as fractions\n- Recognize that whole numbers can be written as fractions\n- Measurement units must be equal in size and placed or interpreted without gaps, overlaps, or a shifted starting point.",
            "Example 1: Find the value of each expression mentally. \\(12 \\div 4\\) \\(24 \\div 4\\) \\(60 \\div 4\\) \\(72 \\div 4\\)\nApproach: Choose the required unit, align equal units from the endpoint, measure without gaps or overlaps, and record the result with its unit.\nExample 2: Locate and label your assigned fractions on the number line. Be prepared to explain your reasoning. \\(\\frac{1}{2}, \\frac{2}{2}, \\frac{3}{2}, \\frac{4}{2}, \\frac{5}{2}, \\frac{6}{2}, \\frac{7}{2}, \\frac{8}{2}, \\frac{9}{2}, \\frac{10}{2}\\) \\(\\frac{1}{3}, \\frac{2}{3}, \\frac{3}{3}, \\frac{4}{3}, \\frac{5}{3}, \\frac{6}{3}, \\frac{7}{3}, \\frac{8}{3}, \\frac{9}{3}\\)...\nApproach: because there are 2 halves in 1, so there are \\(2\\times 2\\) or 4 halves in 2, \\(3\\times 2\\) or 6 halves in 3, and so on.) Label the number line as you share, particularly the whole numbers, like: \\(1 = \\frac{2}{2}\\) , \\(2= \\frac{4}{2}\\) , \\(3=\\frac{6}{2}\\) to highlight the idea that the number of equal parts (2, 3, or 4) in the fractions affects when you end up at a whole number.",
            "Step 1: Identify what needs to be measured.\nStep 2: Choose the required unit and correct starting point.\nStep 3: Place or interpret equal-size units accurately.\nStep 4: Count or calculate the units and record the measure.\nStep 5: Check the endpoint, scale, and unit label.",
            "Common mistake: using unequal units, leaving gaps, overlapping units, or starting at the wrong point. Check the unit and endpoints.",
            "Key ideas:\n- A whole number can equal a fraction: b/b = 1 and 2b/b = 2.\n- Use the location of 1/b to reconstruct the unit interval and then locate whole-number endpoints.",
            "Example 1: Find the value of each expression mentally. \\(12 \\div 4\\) \\(24 \\div 4\\) \\(60 \\div 4\\) \\(72 \\div 4\\)\nApproach: Use related division facts to find each quotient; then connect repeated unit-fraction steps to whole numbers on the line.\nExample 2: Locate and label your assigned fractions on the number line. Be prepared to explain your reasoning. \\(\\frac{1}{2}, \\frac{2}{2}, \\frac{3}{2}, \\frac{4}{2}, \\frac{5}{2}, \\frac{6}{2}, \\frac{7}{2}, \\frac{8}{2}, \\frac{9}{2}, \\frac{10}{2}\\) \\(\\frac{1}{3}, \\frac{2}{3}, \\frac{3}{3}, \\frac{4}{3}, \\frac{5}{3}, \\frac{6}{3}, \\frac{7}{3}, \\frac{8}{3}, \\frac{9}{3}\\)...\nApproach: because there are 2 halves in 1, so there are \\(2\\times 2\\) or 4 halves in 2, \\(3\\times 2\\) or 6 halves in 3, and so on.) Label the number line as you share, particularly the whole numbers, like: \\(1 = \\frac{2}{2}\\) , \\(2= \\frac{4}{2}\\) , \\(3=\\frac{6}{2}\\) to highlight the idea that the number of equal parts (2, 3, or 4) in the fractions affects when you end up at a whole number.",
            "Step 1: Identify the given 1/b point and use its distance from 0 as one fractional step.\nStep 2: Count b equal fractional steps to locate 1.\nStep 3: Continue another b steps for 2, and so on.\nStep 4: Name each whole as a fraction with denominator b and check the positions.",
            "Mistake: treating b/b as b wholes. It is b equal unit fractions making exactly one whole."
        ),
        new Repair(
            "PED:US-CCSS-MATH:G3:U05:L09",
            "https://im.kendallhunt.com/k5/teachers/grade-3/unit-5/lesson-9/preparation.html",
            "a4e247ce514ef3bbb6f926e0138120d766f6df7ebb0408331e9731a1f247fbd0",
            "CCSS:3.NF.A.2",
            "All Kinds of Numbers on the Number Line",
            "Learning goal: You will use your knowledge of fractions on the number line to locate 1 when given a non-unit fraction.\nPreviously, you have located fractions on the number line, including locating 1 when given a unit fraction. In the first activity, you reinforce your understanding of the numerator and denominator of a fraction as you find 1 given a fraction greater than 1. You use the numerator of the given fraction to identify the size of a unit fraction and then to locate 1.",
            "You will use your knowledge of fractions on the number line to locate 1 when given a non-unit fraction. Previously, you have located fractions on the number line, including locating 1 when given a unit fraction.",
            "Key ideas:\n- Locate 1 on the number line given the location of a non-unit fraction\n- Previously, you have located fractions on the number line, including locating 1 when given a unit fraction\n- Measurement units must be equal in size and placed or interpreted without gaps, overlaps, or a shifted starting point.",
            "Example 1: Which one doesn’t belong? A Caption: A B Caption: B C Caption: C D Caption: D\nApproach: “How did you know that the number line in A stopped at 1?” (The location of \\(\\frac{1}{2}\\) shows that the whole number line is split in half right there and I know that 2 halves is 1. I can only fit one more half next to the first one.)\nExample 2: Locate and label 1 on each number line. Use any of the number lines to explain how you located 1.\nApproach: Choose the required unit, align equal units from the endpoint, measure without gaps or overlaps, and record the result with its unit.",
            "Step 1: Identify what needs to be measured.\nStep 2: Choose the required unit and correct starting point.\nStep 3: Place or interpret equal-size units accurately.\nStep 4: Count or calculate the units and record the measure.\nStep 5: Check the endpoint, scale, and unit label.",
            "Common mistake: using unequal units, leaving gaps, overlapping units, or starting at the wrong point. Check the unit and endpoints.",
            "Key ideas:\n- When a/b is marked on a number line, each one of the a equal steps from 0 to that point represents 1/b.\n- Reconstruct the full unit by extending the same 1/b step b times from 0.",
            "Example 1: Which one doesn’t belong? A Caption: A B Caption: B C Caption: C D Caption: D\nApproach: “How did you know that the number line in A stopped at 1?” (The location of \\(\\frac{1}{2}\\) shows that the whole number line is split in half right there and I know that 2 halves is 1. I can only fit one more half next to the first one.)\nExample 2: Locate and label 1 on each number line. Use any of the number lines to explain how you located 1.\nApproach: Use the displayed number line as a scale; count equal fractional steps before deciding where 1 belongs.",
            "Step 1: Read the label a/b on the provided non-unit fraction point.\nStep 2: Partition the distance from 0 to a/b into a equal segments to find the size of 1/b.\nStep 3: Count b equal segments of that length from 0 and mark the position of 1.\nStep 4: Check that the original marked point is a/b on the resulting scale.",
            "Mistake: labeling the given a/b point as 1. First determine a equal steps to a/b, then count b such steps to locate 1."
        ),
        new Repair(
            "PED:US-CCSS-MATH:G4:U02:L01",
            "https://im.kendallhunt.com/k5/teachers/grade-4/unit-2/lesson-1/preparation.html",
            "f6b25baa309cbc9afedb662c33d76830f499daf722d953594d2c7ef13ed5f388",
            "CCSS:4.NF.A.1",
            "Representations of Fractions (Part 1)",
            "Learning goal: You will make sense of unit fractions with denominators 2, 3, 4, 5, 6, 8, 10, and 12, using physical and visual representations.\nIn grade 3, you were introduced to fractions as numbers. You learned to name and represent fractions, to recognize simple equivalent fractions, and to compare fractions with like numerators and denominators (limited to 2, 3, 4, 6, and 8). You used fraction strips, area diagrams, tape diagrams, and number lines to support your reasoning with fractions.",
            "You will make sense of unit fractions with denominators 2, 3, 4, 5, 6, 8, 10, and 12, using physical and visual representations. In grade 3, you were introduced to fractions as numbers.",
            "Key ideas:\n- Make sense of the numerator and denominator of unit fractions that have denominators 2, 3, 4, 5, 6, 8, 10, and 12\n- Use physical and visual representations to reason about fractions\n- Use mathematical attributes such as sides, angles, parallelism, or length—not size or orientation—to justify a classification.",
            "Example 1: If each full diagram represents 1, what fraction does each shaded part represent? Here are four blank diagrams. Each diagram represents 1. Partition each diagram and shade one part so that the shaded part represents the given fraction. \\(\\frac{1}{6}\\) \\(\\frac{1}{8}\\) \\(\\frac{1}{10}\\) \\(\\frac{1}{12}\\) Suppose you are creating a representation of...\nApproach: Choose the required unit, align equal units from the endpoint, measure without gaps or overlaps, and record the result with its unit.",
            "Step 1: Identify what needs to be measured.\nStep 2: Choose the required unit and correct starting point.\nStep 3: Place or interpret equal-size units accurately.\nStep 4: Count or calculate the units and record the measure.\nStep 5: Check the endpoint, scale, and unit label.",
            "Common mistake: using unequal units, leaving gaps, overlapping units, or starting at the wrong point. Check the unit and endpoints.",
            "Key ideas:\n- A unit fraction 1/b describes one of b equal-size parts of the same whole, including denominators 2, 3, 4, 5, 6, 8, 10, and 12.\n- Different pictures can represent the same fraction if each selected part is 1/b of its own equally partitioned whole.",
            "Example 1: If each full diagram represents 1, what fraction does each shaded part represent? Here are four blank diagrams. Each diagram represents 1. Partition each diagram and shade one part so that the shaded part represents the given fraction. \\(\\frac{1}{6}\\) \\(\\frac{1}{8}\\) \\(\\frac{1}{10}\\) \\(\\frac{1}{12}\\) Suppose you are creating a representation of...\nApproach: Compare the shaded parts and total equal shares in each diagram, then name one share as 1/b.",
            "Step 1: Name the entire diagram as one whole.\nStep 2: Partition it into b equal-area pieces, with b among the allowed denominators.\nStep 3: Shade or select one part and write the unit fraction 1/b.\nStep 4: Compare the shaded piece against the entire whole to verify the fraction.",
            "Mistake: calling a differently sized shaded part 1/b because there are b regions. The parts must represent equal shares of the same whole."
        ),
        new Repair(
            "PED:US-CCSS-MATH:G4:U02:L02",
            "https://im.kendallhunt.com/k5/teachers/grade-4/unit-2/lesson-2/preparation.html",
            "9a9b8e85e8af4e96f64fe534208d12f0214faa5901eae6e54e97bd7c50de0205",
            "CCSS:4.NF.A.1",
            "Representations of Fractions (Part 2)",
            "Learning goal: You will make sense of non-unit fractions (including those greater than 1) that have denominators 2, 3, 4, 5, 6, 8, 10, and 12.\nIn the previous lesson, you made sense of the meaning of numerator and denominator in unit fractions. You identified fractions represented by diagrams, and partitioned diagrams to represent given fractions. In this lesson, you reason in similar ways—using numerical and visual representations—about non-unit fractions and fractions that are greater than 1.",
            "You will make sense of non-unit fractions (including those greater than 1) that have denominators 2, 3, 4, 5, 6, 8, 10, and 12. In the previous lesson, you made sense of the meaning of numerator and denominator in unit fractions.",
            "Key ideas:\n- Make sense of the numerator and denominator of unit fractions that have denominators 2, 3, 4, 5, 6, 8, 10, and 12\n- Use diagrams to represent fractions\n- Use mathematical attributes such as sides, angles, parallelism, or length—not size or orientation—to justify a classification.",
            "Example 1: Which one doesn’t belong? A Caption: A B Caption: B C Caption: C D Caption: D\nApproach: “What does the shaded part in D represent?” ( \\(\\frac{1}{3}\\) or one-third of the shape). “Why is the shaded part not one-third of the square in C?” (The parts aren’t equal in size.) Shade one part of A. “Is it a third of the square?” (No, it is \\(\\frac{1}{4}\\) or one-fourth.)\nExample 2: Each full diagram represents 1. Match each fraction to a diagram whose shaded parts represents it. Two of the fractions are not represented. Create a representation for each of them. \\(\\frac{2}{3}: \\underline{\\hspace{0.5in}} \\qquad \\frac{3}{8}: \\underline{\\hspace{0.5in}} \\qquad \\frac{4}{10}: \\underline{\\hspace{0.5in}} \\qquad \\frac{4}{6}:...\nApproach: “What does it mean that the diagrams representing those fractions are the same?” (The fractions are the same size. The term “equivalent” may or may not come up at this point.)",
            "Step 1: Identify what needs to be measured.\nStep 2: Choose the required unit and correct starting point.\nStep 3: Place or interpret equal-size units accurately.\nStep 4: Count or calculate the units and record the measure.\nStep 5: Check the endpoint, scale, and unit label.",
            "Common mistake: using unequal units, leaving gaps, overlapping units, or starting at the wrong point. Check the unit and endpoints.",
            "Key ideas:\n- A non-unit fraction a/b contains a copies of unit fraction 1/b, even when a/b is greater than 1.\n- Use equivalent-size wholes and equal partitions to represent fractions greater than one whole.",
            "Example 1: Which one doesn’t belong? A Caption: A B Caption: B C Caption: C D Caption: D\nApproach: “What does the shaded part in D represent?” ( \\(\\frac{1}{3}\\) or one-third of the shape). “Why is the shaded part not one-third of the square in C?” (The parts aren’t equal in size.) Shade one part of A. “Is it a third of the square?” (No, it is \\(\\frac{1}{4}\\) or one-fourth.)\nExample 2: Each full diagram represents 1. Match each fraction to a diagram whose shaded parts represents it. Two of the fractions are not represented. Create a representation for each of them. \\(\\frac{2}{3}: \\underline{\\hspace{0.5in}} \\qquad \\frac{3}{8}: \\underline{\\hspace{0.5in}} \\qquad \\frac{4}{10}: \\underline{\\hspace{0.5in}} \\qquad \\frac{4}{6}:...\nApproach: “What does it mean that the diagrams representing those fractions are the same?” (The fractions are the same size. The term “equivalent” may or may not come up at this point.)",
            "Step 1: Identify the unit whole and split each whole into b equal parts.\nStep 2: Represent a pieces each of size 1/b, using another equal-size whole if needed.\nStep 3: Shade and count all a pieces; label the result a/b.\nStep 4: Check the whole-unit boundaries and whether a/b exceeds 1.",
            "Mistake: making a larger fraction by changing the whole or denominator halfway through a diagram. Keep each whole the same size and partition all into b equal pieces."
        ),
        new Repair(
            "PED:US-CCSS-MATH:G4:U03:L01",
            "https://im.kendallhunt.com/k5/teachers/grade-4/unit-3/lesson-1/preparation.html",
            "57925f68ccfeb5c62612f6318de059c5babf50d4623cedb967ab58e6104c671e",
            "CCSS:4.NF.B.4",
            "Equal Groups of Unit Fractions",
            "Learning goal: You will interpret and relate descriptions, drawings, and multiplication expressions that represent equal groups of unit fractions.\nIn grade 3, you represented multiplication of whole numbers with arrays, equal-group drawings, area diagrams, and expressions. In an earlier unit, you used diagrams to represent and compare fractions. In this unit, you extend your understanding of multiplication to include equal groups of unit fractions while using familiar representations to support your thinking.",
            "You will interpret and relate descriptions, drawings, and multiplication expressions that represent equal groups of unit fractions. In grade 3, you represented multiplication of whole numbers with arrays, equal-group drawings, area diagrams, and expressions.",
            "Key ideas:\n- Interpret and relate descriptions, drawings, and expressions that represent situations involving equal groups of fractions\n- In grade 3, you represented multiplication of whole numbers with arrays, equal-group drawings, area diagrams, and expressions\n- Use mathematical attributes such as sides, angles, parallelism, or length—not size or orientation—to justify a classification.",
            "Example 1: How many do you see? How do you see them?\nApproach: “How might you describe this image to a friend?” (There are 5 plates with \\(\\frac{1}{2}\\) orange on each plate.) “How many groups do you see?” (I see 5 plates or 5 groups.) “Besides describing the image in words, how else might you represent the quantity in this image?” (I might write \\(\\frac{1}{2}\\) five times, or write an expression with five \\(\\frac{1}{2}\\) s being added together. I might write “5 times...\nExample 2: Here are images of some crackers. A Caption: A Attribution: By King of Hearts. CC-BY-SA 3.0. Wikimedia. Source . B Caption: B How are the crackers in image A like those in B? How are they different? How many crackers are in each image? Write an expression to represent the crackers in each image. Here are more images and descriptions of food items. For...\nApproach: Choose the required unit, align equal units from the endpoint, measure without gaps or overlaps, and record the result with its unit.",
            "Step 1: Identify what needs to be measured.\nStep 2: Choose the required unit and correct starting point.\nStep 3: Place or interpret equal-size units accurately.\nStep 4: Count or calculate the units and record the measure.\nStep 5: Check the endpoint, scale, and unit label.",
            "Common mistake: using unequal units, leaving gaps, overlapping units, or starting at the wrong point. Check the unit and endpoints.",
            "Key ideas:\n- Equal groups of unit fractions can be expressed using repeated addition and multiplication.\n- n groups of 1/b total n/b when all unit parts refer to equal-size wholes.",
            "Example 1: How many do you see? How do you see them?\nApproach: “How might you describe this image to a friend?” (There are 5 plates with \\(\\frac{1}{2}\\) orange on each plate.) “How many groups do you see?” (I see 5 plates or 5 groups.) “Besides describing the image in words, how else might you represent the quantity in this image?” (I might write \\(\\frac{1}{2}\\) five times, or write an expression with five \\(\\frac{1}{2}\\) s being added together. I might write “5 times...\nExample 2: Here are images of some crackers. A Caption: A Attribution: By King of Hearts. CC-BY-SA 3.0. Wikimedia. Source . B Caption: B How are the crackers in image A like those in B? How are they different? How many crackers are in each image? Write an expression to represent the crackers in each image. Here are more images and descriptions of food items. For...\nApproach: Match the equal groups in the drawing to the repeated-addition and multiplication expressions.",
            "Step 1: Identify how many equal groups n appear in the drawing or story.\nStep 2: Determine the fraction 1/b in each group.\nStep 3: Represent the groups by repeated addition or n × 1/b.\nStep 4: Combine the unit fractions into n/b and check the drawing has n such parts.",
            "Mistake: multiplying both numerator and denominator when repeating 1/b. n equal groups of 1/b make n/b."
        ),
        new Repair(
            "PED:US-CCSS-MATH:G4:U03:L02",
            "https://im.kendallhunt.com/k5/teachers/grade-4/unit-3/lesson-2/preparation.html",
            "697a983c5ea1082c94cc56104c20891920c7be153fcae740c1b613986dd749b4",
            "CCSS:4.NF.B.4",
            "Representations of Equal Groups of Fractions PLC Activity",
            "Learning goal: You will interpret and generate diagrams and expressions that represent multiplication of a whole number and a unit fraction in order to find the value of the product.\nIn this lesson, you interpret and relate multiplication expressions and diagrams that represent products of whole numbers and fractions. After matching expressions and diagrams in a card-sort activity, you practice using diagrams and expressions to find the result of multiplying a whole number and a fraction.",
            "You will interpret and generate diagrams and expressions that represent multiplication of a whole number and a unit fraction in order to find the value of the product. In this lesson, you interpret and relate multiplication expressions and diagrams that represent products of whole numbers and fractions.",
            "Key ideas:\n- Interpret diagrams and expressions that represent multiplication of a whole number and a unit fraction\n- Use diagrams and expressions to represent and find the product of a whole number and a unit fraction\n- Measurement units must be equal in size and placed or interpreted without gaps, overlaps, or a shifted starting point.",
            "Example 1: Find the value of each expression mentally. \\(3 \\times 6\\) \\(3 \\times 9\\) \\(6 \\times 9\\) \\(12 \\times 9\\)\nApproach: “What did you notice about the factors in all of the expressions?” (You are all multiples of 3.)\nExample 2: Write a multiplication expression that represents the shaded parts of the diagram. Then, find the value of the expression. Diagram: Expression: Value: Draw a diagram that the expression \\(6 \\times \\frac{1}{3}\\) could represent. Then, find the value of the expression. Diagram: Expression: \\(6 \\times \\frac{1}{3}\\) Value: Draw a diagram and write an...\nApproach: Choose the required unit, align equal units from the endpoint, measure without gaps or overlaps, and record the result with its unit.",
            "Step 1: Identify what needs to be measured.\nStep 2: Choose the required unit and correct starting point.\nStep 3: Place or interpret equal-size units accurately.\nStep 4: Count or calculate the units and record the measure.\nStep 5: Check the endpoint, scale, and unit label.",
            "Common mistake: using unequal units, leaving gaps, overlapping units, or starting at the wrong point. Check the unit and endpoints.",
            "Key ideas:\n- A whole number multiplied by a unit fraction counts repeated equal groups of that fractional amount.\n- n × 1/b = n/b; a diagram should show n groups of equal-size 1/b pieces.",
            "Example 1: Find the value of each expression mentally. \\(3 \\times 6\\) \\(3 \\times 9\\) \\(6 \\times 9\\) \\(12 \\times 9\\)\nApproach: “What did you notice about the factors in all of the expressions?” (You are all multiples of 3.)\nExample 2: Write a multiplication expression that represents the shaded parts of the diagram. Then, find the value of the expression. Diagram: Expression: Value: Draw a diagram that the expression \\(6 \\times \\frac{1}{3}\\) could represent. Then, find the value of the expression. Diagram: Expression: \\(6 \\times \\frac{1}{3}\\) Value: Draw a diagram and write an...\nApproach: Use the source mental-multiplication warm-up as arithmetic practice, then model whole-number groups of unit fractions in the main activity.",
            "Step 1: Read the whole-number factor n and unit fraction 1/b.\nStep 2: Draw n groups, each containing a 1/b part of a consistent whole.\nStep 3: Write n × 1/b and count all selected unit pieces to get n/b.\nStep 4: Check that the drawing and multiplication expression show the same amount.",
            "Mistake: drawing n unequal fractional groups, or using n/(n×b). Each group is the same 1/b, and n such groups total n/b."
        ),
        new Repair(
            "PED:US-CCSS-MATH:G4:U03:L04",
            "https://im.kendallhunt.com/k5/teachers/grade-4/unit-3/lesson-4/preparation.html",
            "8d3bb64d4f4cb68f4272c557966497b23840a3d4d5dad28bc7b5f88345869925",
            "CCSS:4.NF.B.4",
            "Equal Groups of Non-Unit Fractions",
            "Learning goal: You will apply understandings from previous lessons to multiply a non-unit fraction by a whole number.\nPreviously, you learned that any unit fraction multiplied by a whole number results in multiples of that unit fraction. You also learned that any fraction can be written as a multiplication expression of a unit fraction by a whole number. In this lesson, you notice that you can multiply any fraction and a whole number by reasoning about the number of groups and amount in each group.",
            "You will apply understandings from previous lessons to multiply a non-unit fraction by a whole number. Previously, you learned that any unit fraction multiplied by a whole number results in multiples of that unit fraction.",
            "Key ideas:\n- Recognize that $n \\times \\frac{a}{b} =\\frac{n \\ \\times \\ a}{b}$\n- Use diagrams to represent and evaluate the product of a whole number and a non-unit fraction\n- Measurement units must be equal in size and placed or interpreted without gaps, overlaps, or a shifted starting point.",
            "Example 1: Elena fills 5 small jars with homemade jams to share with her friends. Each jar can fit \\(\\frac{3}{4}\\) cup of jam. How many cups of jam are in the jars? Explain or show your reasoning. Attribution: Jars of Jam, by TesaPhotography. Pixabay. Pixabay. Source . If you have time: Elena still has some jam left. She takes 2 large jars and puts \\(\\frac{5}{4}\\)...\nApproach: “What multiplication expression can represent the amount of jam in the jars? How do you know?” ( \\(5 \\times \\frac{3}{4}\\) or \\(5 \\times 3 \\times \\frac{1}{4}\\) , because there are 5 equal groups of \\(\\frac{3}{4}\\) .) If you mention multiplying the whole-number factor by the numerator in the expression, ask: &ldquo;Why does it make sense to multiply the numerator and the whole number to get the product?\" (Because...\nExample 2: This diagram represents \\(\\frac{2}{5}\\) . Show how you would use or adjust the diagram to represent \\(4 \\times \\frac{2}{5}\\) . What is the value of the shaded parts in your diagram? This diagram represents \\(\\frac{5}{8}\\) . Show how you would use or adjust the diagram to represent \\(3 \\times \\frac{5}{8}\\) . What is the value of the shaded parts in your...\nApproach: “How does your diagram show the value of \\(2 \\times \\frac{4}{6}\\) ?” (There are 2 groups of \\(\\frac{4}{6}\\) , so there are 8 sixths shaded, which is \\(\\frac{8}{6}\\) .) “Why did you choose to draw a diagram for some expressions and to do something else for others?” (After drawing the first two diagrams, I realized that I'd have to draw a lot of groups or parts, so I thought about the numbers instead.)",
            "Step 1: Identify what needs to be measured.\nStep 2: Choose the required unit and correct starting point.\nStep 3: Place or interpret equal-size units accurately.\nStep 4: Count or calculate the units and record the measure.\nStep 5: Check the endpoint, scale, and unit label.",
            "Common mistake: using unequal units, leaving gaps, overlapping units, or starting at the wrong point. Check the unit and endpoints.",
            "Key ideas:\n- Multiply a whole number n by a non-unit fraction a/b through n equal groups of a copies of 1/b.\n- The product is (n × a)/b, which can be modeled with fraction diagrams or repeated addition.",
            "Example 1: Elena fills 5 small jars with homemade jams to share with her friends. Each jar can fit \\(\\frac{3}{4}\\) cup of jam. How many cups of jam are in the jars? Explain or show your reasoning. Attribution: Jars of Jam, by TesaPhotography. Pixabay. Pixabay. Source . If you have time: Elena still has some jam left. She takes 2 large jars and puts \\(\\frac{5}{4}\\)...\nApproach: “What multiplication expression can represent the amount of jam in the jars? How do you know?” ( \\(5 \\times \\frac{3}{4}\\) or \\(5 \\times 3 \\times \\frac{1}{4}\\) , because there are 5 equal groups of \\(\\frac{3}{4}\\) .) If you mention multiplying the whole-number factor by the numerator in the expression, ask: &ldquo;Why does it make sense to multiply the numerator and the whole number to get the product?\" (Because...\nExample 2: This diagram represents \\(\\frac{2}{5}\\) . Show how you would use or adjust the diagram to represent \\(4 \\times \\frac{2}{5}\\) . What is the value of the shaded parts in your diagram? This diagram represents \\(\\frac{5}{8}\\) . Show how you would use or adjust the diagram to represent \\(3 \\times \\frac{5}{8}\\) . What is the value of the shaded parts in your...\nApproach: “How does your diagram show the value of \\(2 \\times \\frac{4}{6}\\) ?” (There are 2 groups of \\(\\frac{4}{6}\\) , so there are 8 sixths shaded, which is \\(\\frac{8}{6}\\) .) “Why did you choose to draw a diagram for some expressions and to do something else for others?” (After drawing the first two diagrams, I realized that I'd have to draw a lot of groups or parts, so I thought about the numbers instead.)",
            "Step 1: Identify n equal groups of fraction a/b.\nStep 2: Represent a/b in each group with a unit fractions of size 1/b.\nStep 3: Count n groups of a parts, giving n × a equal unit fractions.\nStep 4: Write (n × a)/b, and verify by repeated addition or a fraction diagram.",
            "Mistake: multiplying the denominator by the number of groups when the unit size has not changed. n × (a/b) = (n×a)/b."
        ),
    ];

    private static readonly Dictionary<string, Repair> ByCode =
        Repairs.ToDictionary(x => x.Code, StringComparer.Ordinal);

    public static bool IsTarget(CanonicalLessonContentPackDocument d,
        CanonicalLessonContentPackLesson l) =>
        d.PackCode == "US-CCSS-MATH" && d.VersionCode == "CCSSM-2010" &&
        ByCode.ContainsKey(l.LessonCode);

    private static void AssertIdentity(CanonicalLessonContentPackLesson l, Repair r)
    {
        if (l.SourceUrl != r.Url ||
            !string.Equals(l.CanonicalBodySha256, r.SourceSha,
                StringComparison.OrdinalIgnoreCase) ||
            string.Join("|", l.OutcomeCodes) != r.OutcomeCodes)
            throw new InvalidOperationException(
                "V9 B01 source/CCSS/semantic fingerprint drift: " + l.LessonCode);
        if (l.Translations.Count != 1 || l.Translations[0].CultureCode != "en")
            throw new InvalidOperationException("V9 B01 unexpected translation set: " + l.LessonCode);
    }

    private static bool Matches(
        string title, string explanation, string summary,
        string concepts, string examples, string steps, string mistakes,
        Repair r, bool corrected) =>
        title == r.Title && explanation == r.Explanation && summary == r.Summary &&
        concepts == (corrected ? r.NewConcepts : r.OldConcepts) &&
        examples == (corrected ? r.NewExamples : r.OldExamples) &&
        steps == (corrected ? r.NewSteps : r.OldSteps) &&
        mistakes == (corrected ? r.NewMistakes : r.OldMistakes);

    public static void Apply(CanonicalLessonContentPackDocument d)
    {
        if (d.PackCode != "US-CCSS-MATH" || d.VersionCode != "CCSSM-2010") return;
        foreach (var lesson in d.Lessons)
        {
            if (!ByCode.TryGetValue(lesson.LessonCode, out var r)) continue;
            AssertIdentity(lesson, r);
            var t = lesson.Translations[0];
            if (Matches(t.Title,t.Explanation,t.QuickSummary,t.KeyConceptsAndRules,
                t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,r,true)) continue;
            if (!Matches(t.Title,t.Explanation,t.QuickSummary,t.KeyConceptsAndRules,
                t.WorkedExamples,t.StepByStepSolutions,t.CommonMistakes,r,false))
                throw new InvalidOperationException(
                    "Refusing partial/unreviewed fraction-content repair: " + lesson.LessonCode);
            t.KeyConceptsAndRules = r.NewConcepts;
            t.WorkedExamples = r.NewExamples;
            t.StepByStepSolutions = r.NewSteps;
            t.CommonMistakes = r.NewMistakes;
        }
    }

    public static string GetExpectedContentVersion(
        CanonicalLessonContentPackDocument d,
        CanonicalLessonContentPackLesson l,
        string existing) => IsTarget(d,l) ? CorrectedVersion : existing;

    public static bool CanUpgradeExisting(
        CanonicalLessonContentPackDocument d,
        CanonicalLessonContentPackLesson l, string existing) =>
        IsTarget(d,l) &&
        (existing == CommonCoreRichContentCorrections.CorrectionContentVersion ||
         existing == CorrectedVersion);

    public static void ValidateExistingBodyBeforeUpgrade(
        CanonicalLessonContentPackDocument d,
        CanonicalLessonContentPackLesson l,
        string existing,
        IReadOnlyCollection<CurriculumLessonContentTranslation> persisted)
    {
        if (!CanUpgradeExisting(d,l,existing))
            throw new InvalidOperationException("V9 B01 unexpected content-version predecessor.");
        var r = ByCode[l.LessonCode];
        AssertIdentity(l,r);
        if (persisted.Count == 0) return;
        if (persisted.Count != 1 || persisted.Single().CultureCode != "en")
            throw new InvalidOperationException("V9 B01 unexpected persisted language set.");
        var p = persisted.Single();
        if (!Matches(p.Title,p.Explanation,p.QuickSummary,p.KeyConceptsAndRules,
            p.WorkedExamples,p.StepByStepSolutions,p.CommonMistakes,r,false) &&
            !Matches(p.Title,p.Explanation,p.QuickSummary,p.KeyConceptsAndRules,
            p.WorkedExamples,p.StepByStepSolutions,p.CommonMistakes,r,true))
            throw new InvalidOperationException("V9 B01 persisted-content body drift: " + l.LessonCode);
    }
}
