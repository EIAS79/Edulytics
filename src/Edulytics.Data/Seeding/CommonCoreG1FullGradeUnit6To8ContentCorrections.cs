using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>
/// Single grade-wide continuation: first-party source-goal teaching corrections
/// for G1 ordinals 103–146, excluding the 14 lessons previously corrected in
/// original source-bound audit. No false certification of source student PDFs,
/// physical classroom activity, oral demonstration, drawing, or pupil authorship.
/// </summary>
public static class CommonCoreG1FullGradeUnit6To8ContentCorrections
{
    public const string Version = "us-ccss-g1-full-grade-u06-u08-source-teaching-v1";
    private sealed record Repair(string Code,string SourceUrl,string OriginalSha,string[] Outcomes,
        string Title,string Explanation,string Examples,string Steps,string Mistakes,string Summary);
    private static readonly Repair[] Repairs =
    [
        new(@"PED:US-CCSS-MATH:G1:U06:L01",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-6/lesson-1/preparation.html",@"1e7c5a4544f35d093f56b1c369e46977617cee4b14761a6972b3f9d4e78583f0",[@"CCSS:1.MD.A.1",@"CCSS:1.NBT.B.3",@"CCSS:1.NBT.C.4",@"CCSS:1.NBT.C.5",@"CCSS:1.OA.C.5",@"CCSS:1.OA.C.6"],
            @"Compare Lengths",
            @"Compare object lengths directly by aligning their starting endpoints and describing which extends farther. Order three objects shortest to longest.",
            @"Example 1: A strip starts at the same line as another strip. The red strip ends after the blue one, so red is longer. Example 2: 5 cubes, 7 cubes, 9 cubes ordered by identical unit are shortest to longest.",
            @"Step 1: Put one end of each object at the same starting line. Step 2: Compare where the other ends stop. Step 3: Order three objects. Step 4: Describe 'shorter than' or 'longer than'.",
            @"Do not compare objects from different starting positions or confuse the number of objects with their length.",
            @"I can compare and order lengths by aligning starting points."),
        new(@"PED:US-CCSS-MATH:G1:U06:L04",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-6/lesson-4/preparation.html",@"581639bdcf507873e2e385bc2c3a1699e9047f95df19faa384c0a59e18f27126",[@"CCSS:1.NBT.B.3",@"CCSS:1.NBT.C.4",@"CCSS:1.NBT.C.5",@"CCSS:1.OA.C.5",@"CCSS:1.OA.C.6"],
            @"Center Day 1",
            @"Choose between teacher-led within-100 addition games and within-20 number-expression comparisons. Only numeric answers are digitally checked; live center turns require a teacher.",
            @"Example 1: 35+24=59 using tens and ones. Example 2: 8+7=15 and 18-3=15, so those expressions are equal.",
            @"Step 1: Identify whether the center requires a within-100 sum or comparison of within-20 expressions. Step 2: Select a matching method. Step 3: Solve both sides if comparing. Step 4: Explain the reasoning.",
            @"Do not compare 8+7 and 18-3 as strings or infer equality from matching operation signs.",
            @"I can add within 100 and compare the values of expressions in center work."),
        new(@"PED:US-CCSS-MATH:G1:U06:L06",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-6/lesson-6/preparation.html",@"d2f09da62e97e4c09ef87f1d9f12b196f0739758ac79e86127959e2e8a2b449e",[@"CCSS:1.MD.A.2"],
            @"Measure with Paper Clips",
            @"Measure object length by placing paper clips end to end. The clips must be equally long and leave no gaps or overlaps; record the count with unit name.",
            @"Example 1: A pencil spans 8 equally sized paper clips in a continuous row, so it is 8 paper clips long. Example 2: Four short clips cannot be compared directly with four long clips as equal lengths.",
            @"Step 1: Choose same-size paper clips. Step 2: Put first clip at the object's start. Step 3: Place others end-to-end with no gaps or overlap. Step 4: Count the clips and record the unit.",
            @"Do not leave spaces, overlap clips, or mix clip lengths and then trust the count as a length.",
            @"I can measure an object accurately with repeated identical paper clips."),
        new(@"PED:US-CCSS-MATH:G1:U06:L07",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-6/lesson-7/preparation.html",@"f7456562f66997352ad3572adcfa3f9f579d7f1d8e07c82e52925a3ae523fc3d",[@"CCSS:1.MD.A.2"],
            @"Measure Length with Different Length Units",
            @"Measure the SAME object using different size units and explain why the numerical count changes while actual length stays the same.",
            @"Example 1: A strip measures 6 short blocks but 3 double-length blocks; the strip did not change. Example 2: 4 equal paper clips may cover the same length as 8 half-length units.",
            @"Step 1: Measure an object with repeated equal small units. Step 2: Repeat with a different unit length. Step 3: Record both numbers AND unit names. Step 4: Compare how unit size affects the count.",
            @"Do not conclude that the object became longer merely because the count using smaller units increased.",
            @"I can explain how choice of length unit changes the number in a measurement."),
        new(@"PED:US-CCSS-MATH:G1:U06:L08",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-6/lesson-8/preparation.html",@"b625fea9b79d9f98966d0d0859def6ac325d9cf01a6637900d006f2b226018ec",[@"CCSS:1.MD.A.2",@"CCSS:1.NBT.A.1"],
            @"Groups Up to 110 PLC Activity",
            @"PLC: measure a long length in equal cube lengths, then organize those counted units into full tens to read a total up to 110. Teacher discussion remains teacher-led.",
            @"Example 1: 8 full groups of ten cubes and 6 extra cubes total 86 units. Example 2: 10 groups of ten and 7 loose units total 107.",
            @"Step 1: Lay cubes in a continuous length with no gaps. Step 2: Count the equal cube units. Step 3: Bundle counts of ten to avoid losing track. Step 4: Read the final count up to 110.",
            @"Do not count a gap as a cube unit or read 10 tens and 7 ones as 17.",
            @"I can group and read up to 110 equal length units."),
        new(@"PED:US-CCSS-MATH:G1:U06:L09",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-6/lesson-9/preparation.html",@"d5eacde6dde26130eda406c988167c58bbad3d0575c7f5b817e3fd4b05c95b64",[@"CCSS:1.MD.A.2",@"CCSS:1.NBT.A.1"],
            @"Write Numbers to 120",
            @"Count to and write numbers as high as 120 while measuring lengths in equal units. Model 100 as ten tens and distinguish 111 from 101.",
            @"Example 1: After 99 come 100, 101, 102. Example 2: Eleven tens and 4 ones make 114, which is less than 120. Example 3: A length of 103 cubes is 100 plus three equal cube units.",
            @"Step 1: Count by tens to a useful landmark. Step 2: Add leftover ones without skipping. Step 3: Say and write the number up to 120. Step 4: Check digits and unit count.",
            @"Do not write 110 as 101 or jump from 109 to 200. Numbers from 100 through 120 have three digits.",
            @"I can read and write counts and measurements through 120."),
        new(@"PED:US-CCSS-MATH:G1:U06:L10",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-6/lesson-10/preparation.html",@"b0392f033a63a72b6eaa5a30d1d1ff78ffbf2ecff6759cbb44b39c2b7ddc88c1",[@"CCSS:1.MD.A.2",@"CCSS:1.NBT.C.4",@"CCSS:1.NBT.C.5",@"CCSS:1.OA.C.5",@"CCSS:1.OA.C.6"],
            @"Center Day 2",
            @"Teacher-led center: add within 100 and measure with same-size length units placed without gaps or overlaps. These are separate center objectives.",
            @"Example 1: 36+25=61 using tens and ones. Example 2: A line covered by 9 equal connecting cubes measures 9 cube-length units.",
            @"Step 1: Read which center task is selected. Step 2: For addition, combine tens/ones and regroup when needed. Step 3: For measurement, tile equal units with no gaps. Step 4: Check the answer and name the units.",
            @"Do not substitute arithmetic for actual measurement, or count units of mixed sizes as though they are interchangeable.",
            @"I can add or measure accurately in an appropriate teacher-led center."),
        new(@"PED:US-CCSS-MATH:G1:U06:L12",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-6/lesson-12/preparation.html",@"363d352cea320e9ccd94af0b40e318eaa9d62365af11578b202ceae7c15df3b1",[@"CCSS:1.NBT.A.1",@"CCSS:1.OA.A.1",@"CCSS:1.OA.C.6"],
            @"Compare Measurements",
            @"Solve Compare Bigger Unknown and Compare Smaller Unknown measurement stories, as in the original 2021 lesson. When one bracelet is four cubes fewer or longer than the other, the UNKNOWN is the second bracelet's LENGTH, not the difference already supplied. Draw or explain a same-unit bar model and solve the missing length.",
            @"Example 1 (Smaller Unknown): Han's bracelet is 14 cubes long; Priya's is 4 fewer, so Priya measures 14-4=10 cubes. Example 2 (Bigger Unknown): Priya measures 9 cubes; Han is 4 longer, so Han measures 9+4=13 cubes. The given difference is NOT the requested unknown length.",
            @"Step 1: Read whose length is known and who is longer or shorter; identify the MISSING bracelet length. Step 2: Use identical cube-length units and draw a bar showing the known length plus or minus the stated difference. Step 3: For Smaller Unknown subtract the difference; for Bigger Unknown add the difference. Step 4: Check that the two resulting lengths actually differ by the stated amount, and include cube-length units.",
            @"Do not answer 4 just because the story says one bracelet is four cubes shorter; four is the KNOWN difference, not the unknown length. Use same-sized units, and choose addition for Bigger Unknown or subtraction for Smaller Unknown.",
            @"I can find an unknown bigger or smaller bracelet length using a known length and the stated difference, and verify it with a same-unit bar model."),
        new(@"PED:US-CCSS-MATH:G1:U06:L13",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-6/lesson-13/preparation.html",@"62ab518f62ff56349f745ece86db222fb39e2d4ac3c6333665492000a8fa743c",[@"CCSS:1.OA.A.1",@"CCSS:1.OA.C.6"],
            @"Solve Take From Story Problems",
            @"Solve Take From stories with unknown starting amount, amount removed, or amount left. Use counters and related addition/subtraction.",
            @"Example 1: There were 16 cubes, 7 were taken, 9 remain: 16-7=9. Example 2: 15 were present, some taken, 6 remain: 15-□=6, so 9 taken. Example 3: Some cubes present, 4 taken, 8 remain: □-4=8, so 12 originally.",
            @"Step 1: Identify the story's original total, removed amount and remaining amount. Step 2: Place the unknown correctly. Step 3: Solve using a model or related fact. Step 4: Check the story.",
            @"Do not assume every subtraction question asks for the amount left; unknowns can occur in any position.",
            @"I can solve Take From stories with the unknown in different positions."),
        new(@"PED:US-CCSS-MATH:G1:U06:L14",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-6/lesson-14/preparation.html",@"f33bbb87cd343930eedd91a620add7ffc259147edd5bb1e301e1326770cddb06",[@"CCSS:1.OA.A.1",@"CCSS:1.OA.B.4",@"CCSS:1.OA.C.6"],
            @"Which Equation Matches? PLC Activity",
            @"PLC: match addition or subtraction equations to realistic stories, including unknowns in all positions. Discuss WHY an equation represents the story.",
            @"Example 1: 12 birds, 5 fly away, 7 left corresponds to 12-5=7. Example 2: 7 birds present and 5 join corresponds to 7+5=12. Example 3: 12 present and some depart leaving 7 corresponds to 12-□=7.",
            @"Step 1: Identify what changes in the situation. Step 2: Name the known numbers and unknown. Step 3: Match operation and blank position. Step 4: Check the proposed equation is true for the story.",
            @"Do not choose addition for a leaving story simply because it contains the same numbers, unless using inverse addition to solve a missing value.",
            @"I can explain which equation matches a story and where its unknown belongs."),
        new(@"PED:US-CCSS-MATH:G1:U06:L15",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-6/lesson-15/preparation.html",@"e571c6469550156f7be3ef50c61f02865a0872176c04123b15a9bb4f867e5632",[@"CCSS:1.NBT.A.1",@"CCSS:1.OA.A.1",@"CCSS:1.OA.C.6"],
            @"Write Equations for Story Problems",
            @"Write, not just select, an addition or subtraction equation matching a story with a missing start, change, or result.",
            @"Example 1: Nine people, five join: 9+5=14. Example 2: □ children plus 6 children makes 13, so □+6=13 and □=7. Example 3: 17-□=8, so 9 left.",
            @"Step 1: Read the whole story before calculating. Step 2: Draw or describe its quantities. Step 3: Put the box in the correct equation position. Step 4: Find the unknown and verify the equation matches the story.",
            @"Do not always place □ at the end. In □+6=13 the unknown START equals 7, not 19.",
            @"I can write accurate equations for different addition/subtraction stories."),
        new(@"PED:US-CCSS-MATH:G1:U06:L16",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-6/lesson-16/preparation.html",@"98a1d3eb2f4843aa29f09f970e8198d8bce3344bc72740319287f4fe605dad78",[@"CCSS:1.MD.A.2",@"CCSS:1.NBT.A.1",@"CCSS:1.NBT.C.4"],
            @"Center Day 3",
            @"Center review includes counting collections through 120, measuring with equal units, and addition within 100. Physical measurement and classroom turns remain teacher-facilitated.",
            @"Example 1: 9 tens and 5 ones represent 95 objects. Example 2: 42+37=79. Example 3: a strip measuring 8 identical cubes is 8 cube lengths.",
            @"Step 1: Identify which center objective the task actually uses. Step 2: Count tens/ones, measure equal units, or add two numbers as appropriate. Step 3: Record quantity, units or equation. Step 4: Verify.",
            @"Do not automatically certify paper-and-cube measurement from a digital numerical answer or omit the unit.",
            @"I can choose accurate counting, measuring, and adding strategies for center tasks."),
        new(@"PED:US-CCSS-MATH:G1:U07:L01",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-7/lesson-1/preparation.html",@"5c26bc734e52785fea17276fee37c16eda2de2b5a278a3ec8f2f5f57a506cbfa",[@"CCSS:MP.6"],
            @"Shapes That Are Solid",
            @"Sort and describe SOLID three-dimensional shapes using observable attributes. Students may invent their own sorting categories and explain them; do not require a single teacher-approved grouping.",
            @"Example 1: A sphere rolls and has a curved surface. Example 2: A cube has six flat square faces. Example 3: A cylinder has two flat circular faces and one curved side.",
            @"Step 1: Observe each object from several directions. Step 2: Choose a characteristic (flat faces, curved surfaces, rolling). Step 3: Group and explain the reason. Step 4: Check all members meet the stated rule.",
            @"Do not treat a drawing of a circle as a physical sphere or require every shape to have corners.",
            @"I can classify and describe solid shapes in my own accurate words."),
        new(@"PED:US-CCSS-MATH:G1:U07:L03",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-7/lesson-3/preparation.html",@"bc5f15aa21a4b577cb3792712de3c0d94d06ec5433583f71f44bdc4e1dc04506",[@"CCSS:MP.3",@"CCSS:MP.6"],
            @"Shapes That Are Flat",
            @"Sort FLAT two-dimensional shapes based on observable sides, curves and corners. Describe shapes in your own words before formal terminology.",
            @"Example 1: A triangle is a closed flat shape with three straight sides. Example 2: A rectangle has four straight sides and four corners. Example 3: A circle has no straight sides or corners.",
            @"Step 1: Examine each flat shape. Step 2: Count straight sides and corners, or notice curves. Step 3: Sort by the chosen shared feature. Step 4: Explain why each shape belongs.",
            @"Do not confuse a 3D cube with its flat square face. A rotated triangle remains a triangle.",
            @"I can sort and describe flat shapes using their attributes."),
        new(@"PED:US-CCSS-MATH:G1:U07:L05",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-7/lesson-5/preparation.html",@"500b3b0559d2ed9f75fc326a50b4bb6ea7d39a3cb482d183234450bd3dfcc93f",[@"CCSS:1.G.A.1",@"CCSS:1.NBT.C.4",@"CCSS:1.OA.C.6"],
            @"Some Triangles, All Triangles",
            @"ORIGINAL STUDENT FACING TASK: DRAW THREE DIFFERENT TRIANGLES, not just a single triangle. Distinguish triangles from non-triangles by their defining attributes: each of three connected straight sides closes the outline. Compare different sizes, angles and orientations. DRAW THREE other closed or open figures that are NOT triangles and explain exactly why each is not a triangle (wrong side count, curve or unclosed boundary). The teacher reviews three genuinely different valid triangles PLUS three non-triangle figures; a digital SIDES=3 answer cannot prove drawing.",
            @"Example 1: A small, tilted triangle is still a triangle because it has three connected straight sides. Example 2: Three separate sticks that do not form a closed boundary are not a triangle.",
            @"Step 1: Draw three straight line segments. Step 2: Join their endpoints into one closed shape. Step 3: Count the three sides and vertices. Step 4: Change size or rotation and identify which features stay the same.",
            @"Do not say a triangle must point up or have two matching sides. Rotation and color do not determine triangle membership.",
            @"I can draw triangles and distinguish defining from non-defining features."),
        new(@"PED:US-CCSS-MATH:G1:U07:L09",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-7/lesson-9/preparation.html",@"caddbf353a123c3ffc3030c6c2b8d84a17a3a4f0af852ad3100d7504f9fa3789",[@"CCSS:1.G.A.3"],
            @"Equal Pieces",
            @"Determine whether a circle or rectangle is partitioned into EQUAL pieces and draw partitions into two or four equal shares.",
            @"Example 1: A rectangle split down its middle into two equal rectangles has halves. Example 2: A circle split through its center by two perpendicular diameters has four equal fourths. Unequal pieces are not halves.",
            @"Step 1: Identify the complete circle or rectangle. Step 2: Divide into two equal areas for halves. Step 3: Divide into four equal areas for fourths. Step 4: Compare the piece sizes and name the equal shares.",
            @"Do not call any two pieces halves: they must have equal area. Different-looking pieces must be checked rather than assumed equal.",
            @"I can partition circles and rectangles into halves and fourths."),
        new(@"PED:US-CCSS-MATH:G1:U07:L10",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-7/lesson-10/preparation.html",@"a533c8a407a83aae674d36464cd91dac2f7e1da0c48e5f0ce787f5cf23fcd12a",[@"CCSS:1.G.A.3"],
            @"One of the Pieces, All of the Pieces",
            @"ORIGINAL STUDENT FACING TASK: split the square into two equal halves, then SORT the shaded-piece picture cards into categories and EXPLAIN how you grouped them. Distinguish true equal-area halves/fourths from visually unequal pieces, even when both pictures have the same number of regions. Two halves or four fourths make one whole only when all those shares have EQUAL area. The classroom cutting, card sorting and pupil explanations are teacher-facilitated, not proven by a single digital classification.",
            @"Example 1: Two equal halves of a rectangle make one whole. Example 2: Four equal fourths of a circle make one whole. Example 3: One of four equal squares in a partitioned rectangle is one fourth of the rectangle.",
            @"Step 1: Show the whole. Step 2: Verify pieces are equal in area. Step 3: Count two equal halves or four equal fourths. Step 4: Identify one piece using a correct part-of-whole description.",
            @"Do not call one of three unequal pieces a fourth. Names halves and fourths depend on the total number of equal pieces.",
            @"I can describe a whole and its equal halves or fourths."),
        new(@"PED:US-CCSS-MATH:G1:U07:L11",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-7/lesson-11/preparation.html",@"a199f08ddd0cb1403a645394d2d0764eefd51711383028a6d8b2aaa45c01e8c5",[@"CCSS:1.G.A.3",@"CCSS:1.NBT.C.5"],
            @"A Bigger Piece PLC Activity",
            @"Compare halves and fourths of the SAME original shape. Partitioning one whole into more equal shares makes each share smaller.",
            @"Example 1: One half of the same rectangle is larger in area than one fourth. Example 2: Two fourths cover the same area as one half of the same whole.",
            @"Step 1: Draw one rectangle and two identical copies. Step 2: Split one into two equal halves and another into four equal fourths. Step 3: Compare a single piece in each. Step 4: Explain that more equal pieces means smaller individual pieces.",
            @"Do not compare one fourth of a big shape to one half of a much smaller shape and claim a universal ordering.",
            @"I can explain why fourths are smaller than halves of the same whole."),
        new(@"PED:US-CCSS-MATH:G1:U07:L12",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-7/lesson-12/preparation.html",@"6692ba8a9610988d4524132b41a6df55517496cb28390274e50aea0dc880e4e5",[@"CCSS:1.G.A.1",@"CCSS:1.G.A.2",@"CCSS:1.NBT.C.4",@"CCSS:1.OA.C.6"],
            @"Center Day 2",
            @"Teacher-led shape center review: compose objects from solids, describe shapes in picture books, add within 100 and subtract within 10. Digital arithmetic cannot certify building or explaining an object.",
            @"Example 1: A cube and rectangular prism can be arranged into a new solid structure. Example 2: 47+28=75 by combining tens and ones. Example 3: 9-4=5.",
            @"Step 1: Identify the selected activity's exact goal. Step 2: For shapes, describe and build with appropriate solids. Step 3: For mathematics, use a suitable addition or subtraction method. Step 4: Explain the result to the teacher.",
            @"Do not replace a three-dimensional composition with an unrelated arithmetic question and claim the shape objective was assessed.",
            @"I can work accurately with solids, addition within 100 and subtraction within 10 in their respective centers."),
        new(@"PED:US-CCSS-MATH:G1:U07:L17",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-7/lesson-17/preparation.html",@"88d2973aea89e988e30f779e8d1b31e6ff5bf599a818f43706f5b029333c1fa1",[@"CCSS:1.G.A.2",@"CCSS:1.MD.B.3",@"CCSS:1.NBT.C.4",@"CCSS:1.OA.C.6"],
            @"Center Day 3",
            @"Teacher-led center review: add within 100, name and describe two- and three-dimensional shapes, or subtract within 10. Each center has a different mathematical goal.",
            @"Example 1: 35+46=81. Example 2: 8-3=5. Example 3: A flat triangle has three straight sides; a three-dimensional cone has a circular base and curved surface.",
            @"Step 1: Read the center's task and expected representation. Step 2: Use place-value addition, small subtraction or shape attributes as appropriate. Step 3: Check and explain your result. Step 4: Teacher validates hands-on shape activity and partner work.",
            @"Do not claim the hands-on shape activity is complete merely because one number was entered digitally.",
            @"I can choose correct calculation and shape language in teacher-led centers."),
        new(@"PED:US-CCSS-MATH:G1:U08:L01",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-8/lesson-1/preparation.html",@"c07592bb2a67aabeeb002747901ce73a0a6df94021c10c0552f1327d1184e56f",[@"CCSS:1.OA.C.6"],
            @"Addition Fluency Within 10",
            @"Develop reliable addition fluency for sums within TEN. Explain a helpful known fact, count on, or ten-frame strategy rather than guessing.",
            @"Example 1: 6+4=10, two parts make a ten. Example 2: 5+3=8 by starting at five and counting three more. Example 3: 4+4=8 is a known double.",
            @"Step 1: Read the addends (sum no more than 10). Step 2: Try a familiar fact, double or count-on. Step 3: State the sum accurately. Step 4: Check with counters or related fact.",
            @"Do not confuse speed with accuracy or use a sum above ten as evidence of fluency within ten.",
            @"I can find sums within ten using facts and a strategy I understand."),
        new(@"PED:US-CCSS-MATH:G1:U08:L02",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-8/lesson-2/preparation.html",@"72db439ba6f87caafe696606b4ab7db25a14b1cbb5a2f3b1ffeeafd701e57be3",[@"CCSS:1.OA.C.6",@"CCSS:1.OA.D.8"],
            @"Relate Addition and Subtraction",
            @"Relate addition and subtraction within ten as inverse number relationships. A single set of three numbers can yield a fact family.",
            @"Example 1: If 5+4=9, then 9-4=5 and 9-5=4. Example 2: 6+3=9 and 9-6=3.",
            @"Step 1: Find two parts and their whole, at most ten. Step 2: Write a true addition fact. Step 3: Begin a subtraction with the whole. Step 4: Remove one part to find the other. Step 5: Check.",
            @"Do not write 5-9=4 for a within-ten part-whole fact; start subtraction from the whole nine.",
            @"I can use addition to explain and check subtraction."),
        new(@"PED:US-CCSS-MATH:G1:U08:L03",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-8/lesson-3/preparation.html",@"e5b135f819e30a6304ea07854cfa2b0e7a7963c8f8c4a2697da2628535ec933c",[@"CCSS:1.OA.C.6",@"CCSS:1.OA.D.8"],
            @"Add and Subtract Within 20 PLC Activity",
            @"PLC: add and subtract within TWENTY, using different helpful strategies and explaining why they work. Partner discussion remains teacher-led.",
            @"Example 1: 9+8=9+1+7=17. Example 2: 15-7=15-5-2=8. Example 3: 8+6=14 and 14-6=8.",
            @"Step 1: Decide whether to add or subtract. Step 2: Choose making ten, known fact or count-on. Step 3: Show an equivalent calculation. Step 4: Check with inverse operation. Step 5: Explain the choice.",
            @"Do not lose leftover ones when making ten or treat a teacher conversation as complete merely by submitting a numeric answer.",
            @"I can use efficient addition and subtraction strategies within twenty."),
        new(@"PED:US-CCSS-MATH:G1:U08:L04",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-8/lesson-4/preparation.html",@"d06b8875654892a2317c4ce01a5cf75965ef0c5b365cc516b3a5bd52a079c51c",[@"CCSS:1.OA.A.1",@"CCSS:1.OA.C.6"],
            @"Change Unknown Story Problems PLC Activity",
            @"Solve Add To or Take From stories where the CHANGE is unknown. Identify the known start and final amount, and find what joined or left.",
            @"Example 1: Seven birds, then some join, now twelve: 7+□=12; five joined. Example 2: Fifteen counters, some removed, now nine: 15-□=9; six removed.",
            @"Step 1: State the starting count and result count. Step 2: Determine whether the story adds or removes. Step 3: Put the box in the CHANGE position. Step 4: Solve and check against the story.",
            @"Do not automatically place the box at the result: in 7+□=12 the missing number is 5.",
            @"I can find the unknown change in joining or leaving stories."),
        new(@"PED:US-CCSS-MATH:G1:U08:L05",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-8/lesson-5/preparation.html",@"514cff5609b881c3fbd91b38d4bfd318e4c64f88b1e3bb3e6c42b062786f2a83",[@"CCSS:1.NBT.C.4",@"CCSS:1.OA.A.1",@"CCSS:1.OA.C.6",@"CCSS:1.OA.D.7"],
            @"Put Together and Take Apart Story Problems",
            @"Solve Put Together/Take Apart stories with an unknown ADDEND or missing part. Use a part-whole model or addition/subtraction relation.",
            @"Example 1: There are 13 marbles; eight are blue and the rest are red. 8+□=13, so five are red. Example 2: Two groups together have 17, one group has nine: 9+□=17, missing part eight.",
            @"Step 1: Identify the whole. Step 2: Name the given part and unknown part. Step 3: Write part+part=whole. Step 4: Solve the missing addend. Step 5: Check that parts sum to the whole.",
            @"Do not add the whole to the known part; the whole already contains both parts.",
            @"I can solve a missing-part Put Together/Take Apart story."),
        new(@"PED:US-CCSS-MATH:G1:U08:L06",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-8/lesson-6/preparation.html",@"a710a0be36e7f09a9c1506eaaca2bb7b05b17ff0747ece123bb118a8c5f5e695",[@"CCSS:1.OA.A.1",@"CCSS:1.OA.A.2",@"CCSS:1.OA.C.6"],
            @"Compare Story Problems",
            @"Solve Compare stories with DIFFERENCE UNKNOWN. Identify the larger and smaller quantities and explain the difference without confusing it with their total.",
            @"Example 1: Sara has 12 stickers and Kim has 7. Sara has five more because 12-7=5. Example 2: A 16-cube tower and a 9-cube tower differ by seven cubes.",
            @"Step 1: Identify what quantities are compared and their units. Step 2: Mark which is larger. Step 3: Subtract smaller from larger or count on. Step 4: State how many more or fewer. Step 5: Check by adding difference to smaller.",
            @"Do not answer 19 stickers for 'how many more' when comparing 12 and 7; 19 is a total, not a difference.",
            @"I can solve a Compare story to find the difference."),
        new(@"PED:US-CCSS-MATH:G1:U08:L07",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-8/lesson-7/preparation.html",@"c5d06c8b7c5608bf81a5c260702dd0cc6c83e6f4a45994f1b471c2bf93a7bae9",[@"CCSS:1.NBT.A.1"],
            @"Count Large Collections",
            @"Count, organize and represent up to 120 objects starting the count from an arbitrary number, not necessarily one. Use tens groups to track large collections.",
            @"Example 1: Count on from 87: 88,89,90,91. Example 2: Eleven tens and six ones make 116. Example 3: Count from 109 to 110,111,112.",
            @"Step 1: Read the starting number. Step 2: Count on by ones, regrouping at a new ten or hundred. Step 3: Make full tens groups when counting a collection. Step 4: Record and check the total, at most 120.",
            @"Do not restart from one when a different start is specified, or skip 110 after 109.",
            @"I can count and represent large collections and start from a number other than one."),
        new(@"PED:US-CCSS-MATH:G1:U08:L08",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-8/lesson-8/preparation.html",@"fccd90f84c40b1cd77b77acd7c94f39fbdb9d78f08cf00f499de4f05d2cd2b73",[@"CCSS:1.NBT.A.1",@"CCSS:1.OA.C.6"],
            @"Show Me All the Ways PLC Activity",
            @"PLC: REPRESENT a two-digit number in different valid ways using tens, ones, base-ten drawings and expanded addition. Replacing one ten with ten ones does not change the total.",
            @"Example 1: 54=5 tens 4 ones=4 tens 14 ones. Example 2: 68=60+8=5 tens 18 ones. Example 3: 73=70+3.",
            @"Step 1: Write or build the original two-digit number. Step 2: Show tens-and-ones model. Step 3: Break one ten into ten ones for an alternative. Step 4: Compare totals to verify equivalent representations.",
            @"Do not read 4 tens 14 ones as 414; it is 40+14=54. Classroom comparison requires a real partner or teacher.",
            @"I can create and compare different representations of the same number."),
        new(@"PED:US-CCSS-MATH:G1:U08:L09",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-8/lesson-9/preparation.html",@"dd74764b87e53b0d6900a35e30a905dfc05d30acab9db978ed2e9ac11626720b",[@"CCSS:1.NBT.B.3"],
            @"Solve Number Riddles",
            @"Solve 2021 place-value NUMBER RIDDLES progressively. The FIRST clues may admit SEVERAL different numbers, and the final clue selects ONE unique number. List all candidates that fit the ones digit and the upper/lower bounds before deciding on the secret number. Then check every proposed clue. Teacher-led partner cards and oral reasoning are not digitally claimed.",
            @"Example 1 (original source task): The number is MORE than 30, LESS than 60, and has 4 ones. ALL possible numbers are 34, 44, 54; the first three clues do not yet fix one secret number. Example 2: A final clue says the number is ten more than 34, so the unique answer is 44. Example 3: A false clue saying 60=25+45 must be rejected because 25+45=70.",
            @"Step 1: Read the lower and upper bounds and identify the ones digit. Step 2: Write ALL possible two-digit numbers meeting these early clues, not just one guess. Step 3: Apply the next/final clue to remove candidates until ONE secret number remains. Step 4: Check every clue and reject contradictions. Step 5: Explain why each excluded number fails at least one clue.",
            @"Do not claim that three initial clues always identify one secret number: 34, 44, and 54 all satisfy greater than 30, less than 60, and four ones. A last clue can select 44. Do not accept a false equality: 60 is not 25+45.",
            @"I can list ALL possible numbers for initial clues, then find a single secret number using the final clue and prove why it fits."),
        new(@"PED:US-CCSS-MATH:G1:U08:L10",@"https://im.kendallhunt.com/k5/teachers/grade-1/unit-8/lesson-10/preparation.html",@"b05db505b2fc2f6fd1a6d0ef8e5e1ff62c6d3fc13bcf03e941504436f748dd59",[@"CCSS:1.NBT.C.4"],
            @"Write Number Riddles",
            @"Write an ORIGINAL four-clue number riddle about a number between 1 and 120. Include accurate place-value clues and make the FOURTH clue an addition expression. Then check each clue and ask a partner to solve. A numeric Practice check cannot prove authorship or real classroom activity.",
            @"Example 1: Choose 47. Four clues: (1) Greater than 40, (2) less than 50, (3) four tens and seven ones, (4) 40 + 7. Each clue fits 47. Example 2: Choose 112. Four clues: (1) greater than 110, (2) less than 115, (3) one hundred, one ten and two ones, (4) 100 + 12. Each clue fits 112.",
            @"Step 1: Choose a secret number from 1 through 120. Step 2: Write exactly four clues: use true ranges/place value and make the FINAL clue a matching addition expression. Step 3: Check all four clues against the same secret number; notice 100–120 can contain one hundred and extra tens/ones. Step 4: Have a partner attempt the riddle and explain the number. Step 5: Revise an unclear or inconsistent clue after a teacher checks the written work.",
            @"Do not make an incorrect final addition clue: 40 + 8 cannot describe 47. A two-digit-only limit incorrectly excludes valid targets 100 through 120. Do not claim an auto-scored answer proves that the learner authored four coherent clues.",
            @"I can write an accurate number riddle and explain its solution.")
    ];
    private static Repair? Find(CanonicalLessonContentPackDocument doc,CanonicalLessonContentPackLesson lesson)=>
        doc.PackCode==MathematicsCurriculumPackRegistry.CommonCoreCode&&
        doc.VersionCode=="CCSSM-2010"
        ? Repairs.SingleOrDefault(x=>x.Code==lesson.LessonCode):null;
    public static bool IsTarget(CanonicalLessonContentPackDocument doc,CanonicalLessonContentPackLesson lesson)=>
        Find(doc,lesson)!=null;
    private static Repair Checked(CanonicalLessonContentPackDocument doc,CanonicalLessonContentPackLesson lesson)
    {
        var r=Find(doc,lesson)??throw new InvalidOperationException("Not a full-grade G1 U06–U08 repair");
        if(lesson.SourceUrl!=r.SourceUrl||
           !string.Equals(lesson.CanonicalBodySha256,r.OriginalSha,StringComparison.OrdinalIgnoreCase)||
           !lesson.OutcomeCodes.SequenceEqual(r.Outcomes))
            throw new InvalidOperationException("Original teacher source, SHA or outcomes drift: "+lesson.LessonCode);
        return r;
    }
    public static void Apply(CanonicalLessonContentPackDocument doc)
    {
        foreach(var lesson in doc.Lessons)
        {
            if(!IsTarget(doc,lesson))continue;
            var r=Checked(doc,lesson);
            var en=lesson.Translations.Single(x=>x.CultureCode=="en");
            if(en.Explanation==r.Explanation&&en.WorkedExamples==r.Examples&&
               en.StepByStepSolutions==r.Steps&&en.CommonMistakes==r.Mistakes&&en.QuickSummary==r.Summary)continue;
            if(!en.WorkedExamples.StartsWith("Example 1:",StringComparison.Ordinal)||
               !en.StepByStepSolutions.StartsWith("Step 1:",StringComparison.Ordinal))
                throw new InvalidOperationException("G1 U06–U08 unexpected original learner content: "+lesson.LessonCode);
            en.Title=r.Title;en.Explanation=r.Explanation;en.KeyConceptsAndRules=r.Explanation;
            en.WorkedExamples=r.Examples;en.StepByStepSolutions=r.Steps;
            en.CommonMistakes=r.Mistakes;en.QuickSummary=r.Summary;
        }
    }
    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string previous)=>
        IsTarget(doc,lesson)?Version:previous;
    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string previous)=>
        IsTarget(doc,lesson)&&previous==CommonCoreRichContentCorrections.CorrectionContentVersion;
    public static void ValidateExistingBodyBeforeUpgrade(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string previous,
        IReadOnlyCollection<CurriculumLessonContentTranslation> translations)
    {
        Checked(doc,lesson);
        if(!CanUpgradeExisting(doc,lesson,previous))
            throw new InvalidOperationException("Unexpected previous G1 U06–U08 content version: "+lesson.LessonCode);
        if(translations.Count==0)return;
        if(translations.Count!=1||translations.Single().CultureCode!="en"||
           !translations.Single().WorkedExamples.StartsWith("Example 1:",StringComparison.Ordinal)||
           !translations.Single().StepByStepSolutions.StartsWith("Step 1:",StringComparison.Ordinal))
            throw new InvalidOperationException("Existing G1 U06–U08 translation drift: "+lesson.LessonCode);
    }
}
