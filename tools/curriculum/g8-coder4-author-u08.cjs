'use strict';
const fs=require('node:fs'),path=require('node:path'),root=path.resolve(__dirname,'../..');
const lesson=(n,title,body,ideas,examples,errors,summary)=>({
 LessonCode:'PED:US-CCSS-MATH:G8:U08:L'+String(n).padStart(2,'0'),CultureCode:'en',Title:title,
 ExplanationParagraphs:[...body,'Independently authored Edulytiks instructional content informed by Open Up Resources / Illustrative Mathematics Grade 8 (2017–2019), CC BY 4.0. Exact source: https://im.kendallhunt.com/MS/teachers/3/8/'+n+'/preparation.html . Publisher figures and assessments are not duplicated.'],
 KeyConcepts:ideas.map(([Title,Definition,Rule,Example])=>({Title,Definition,Rule,Example})),
 WorkedExamples:examples.map(([Title,Question,Method,Steps,Answer,Check])=>({Title,Question,Method,Steps,Answer,Check})),
 CommonMistakes:errors.map(([Mistake,WhyWrong,Correction])=>({Mistake,WhyWrong,Correction})),
 SummaryPoints:summary,Visuals:[],Videos:[]
});
const rows=[
lesson(1,'The Areas of Squares and Their Side Lengths',[
 'Goal: connect area of a square with the square of its side length, and determine an unknown side by using a nonnegative square root.',
 'Construct paper square tiles with side lengths 1,2,3,4 and compare the number of unit-square tiles. The relationship A=s² means a square with area 25 has side length 5 units, not ±5, because physical lengths are nonnegative.',
 'Original student task: use an accessible grid to draw squares with areas 4,9,16 and label sides; explain why 5 square units does not give a whole-number side. A teacher assesses the drawings.'
],[
 ['Area of square','Surface measurement of a square of side s.','A=s².','Side 4 has area 16.'],
 ['Square root of area','Nonnegative length whose square equals area.','s=√A for A≥0.','Area 25 gives side 5.'],
 ['Non-square area','A positive area may require irrational side length.','Area 5 gives side √5.','√5 is between 2 and 3.']
],[
 ['Area from side','Square side length 4. What area?','Multiply side by itself.', ['4×4=16.','Area unit is square units.','The area is 16 square units.'],'16 square units.','Four rows of four unit squares.'],
 ['Side from area','Square area 25. What is its side?','Take principal square root.', ['A=s²=25.','s=√25.','Physical s=5, not negative 5.'],'5 units.','5²=25.'],
 ['Estimate side','Square area 5. Is its side between 2 and 3?','Compare known squares.', ['2²=4 and 3²=9.','5 lies between 4 and 9.','Therefore √5 lies between 2 and 3.'],'Yes, between 2 and 3.','Its exact side length is √5.']
],[
 ['Adding two sides to calculate area.','Square area multiplies length by width.','Use s².'],
 ['Reporting negative physical side.','√A denotes nonnegative root.','Distinguish algebraic x²=A from length s=√A.']
],['Square area is s².','Side length is the principal square root.','Not all square areas have integer side lengths.']),
lesson(2,'Side Lengths and Areas',[
 'Goal: reason about changes in square area when the side is scaled and infer the relationship between area factors and side factors.',
 'Doubling the side length creates four times as much area, because both length and width double. If the area grows by factor 9, the side length grows by factor 3.',
 'Compare original squares of side 2 and side 6; draw an array showing nine copies of the smaller square fitting the larger. A teacher judges the visual reasoning and correspondence.'
],[
 ['Area factor','Ratio of new square area to old square area.','(ks)²/s²=k².','Side factor 3 yields area factor 9.'],
 ['Side factor','Positive square root of an area ratio.','k=√(Anew/Aold).','Area factor 16 implies side factor 4.'],
 ['Unit scale','Lengths and areas use different units.','side in cm, area in cm².','Square side 6 cm has area 36 cm².']
],[
 ['Double side','Square side 2 becomes side 4. Compare areas.','Square both lengths.', ['Original area=2²=4.','New area=4²=16.','Factor 16/4=4.'],'Area quadruples.','Side factor 2 gives area factor 2²=4.'],
 ['Triple side','Square side 2 becomes 6. Compare areas.','Square side lengths.', ['Original area 4.','New area 36.','Area factor 36/4=9.'],'Area factor 9.','A 3-fold side change gives 9-fold area.'],
 ['Recover side factor','Area increases from 9 to 144. By what factor did side grow?','Take square root of area ratio.', ['Area ratio 144/9=16.','Side factor=√16.','k=4.'],'Factor 4.','3-unit side becomes 12-unit side.']
],[
 ['Assuming area scales linearly with side.','Two dimensions change.','Square the side factor.'],
 ['Using square-area units for side length.','A side measures one-dimensional distance.','Use proper units.']
],['Side scaling k changes square area by k².','Area factor reveals side factor through square root.','Use correct dimensions.']),
lesson(3,'Rational and Irrational Numbers',[
 'Goal: distinguish rational numbers, which equal ratios of integers with nonzero denominator, from irrational numbers whose decimal expansions do not terminate or repeat.',
 'Square roots of nonsquare positive integers such as √2 are irrational. A decimal like 0.333… is rational because it repeats and equals 1/3. A measured decimal approximation does not make an exact irrational number rational.',
 'Construct a number-card sort using 1/2, √4, √2, 0.125, 0.333… and π. A teacher checks explanations and exact classifications rather than approximated display strings.'
],[
 ['Rational number','Can be written a/b with integers a,b and b≠0.','Terminating or eventually repeating decimal.','1/3=0.333… is rational.'],
 ['Irrational number','Real number with no exact integer ratio.','Its decimal does not terminate or eventually repeat.','√2 and π are irrational.'],
 ['Exact versus approximation','Rounded decimal is not equality of real values.','Write √2≈1.4142, not √2=1.4142.','Irrational cannot be fully represented by finite decimals.']
],[
 ['Classify square root','Is √4 rational?','Evaluate the root.', ['√4=2.','2=2/1.','It is rational.'],'Rational.','Exact integer 2.'],
 ['Classify √2','Is √2 rational?','Use standard nonsquare-root property.', ['2 is not a perfect square.','The positive square root √2 has no exact integer ratio.','It is irrational.'],'Irrational.','√2≈1.414, approximation only.'],
 ['Classify repeating decimal','Is 0.333… rational?','Convert repeating decimal.', ['Let x=0.333….','10x−x=3, so 9x=3.','x=1/3.'],'Rational, equal to 1/3.','Repeat pattern determines an exact fraction.']
],[
 ['Claiming an irrational rounded to 1.41 becomes rational exactly.','An approximation is not the exact value.','Keep an approximation symbol.'],
 ['Calling every infinite decimal irrational.','Repeating infinite decimals are rational.','Check repetition.']
],['Rational numbers are integer ratios.','Irrational decimals never terminate or repeat.','Use exact notation and approximation honestly.']),
lesson(4,'Square Roots on the Number Line',[
 'Goal: construct and locate square roots such as √2 on a number line, using geometric lengths and squared-distance reasoning.',
 'A right triangle with legs 1 and 1 has hypotenuse √2 by the Pythagorean relation. Transfer its length with compass or paper strip from zero to mark √2 on a positive number line.',
 'Sketch and annotate the construction rather than only writing 1.414. Teacher evaluation is necessary for a valid geometric placement and scale.'
],[
 ['Geometric square root','An irrational length may be constructed from right triangles.','Hypotenuse √(a²+b²).','Unit legs give √2.'],
 ['Number-line transfer','Mark a constructed length starting from 0.','Maintain the same scale.','√2 is between 1 and 2.'],
 ['Approximate position','Compare squares of adjacent decimals.','1.4²<2<1.5².','1.4<√2<1.5.']
],[
 ['Unit triangle','Find diagonal of a unit square.','Use 1²+1².', ['Legs are each 1.','Hypotenuse²=1²+1²=2.','Hypotenuse=√2.'],'√2 units.','Squared length is exactly 2.'],
 ['Bound √2','Show 1.4<√2<1.5.','Square positive endpoints.', ['1.4²=1.96<2.','1.5²=2.25>2.','Positive square root is between them.'],'Between 1.4 and 1.5.','No exact decimal was asserted.'],
 ['Locate √9','Where is √9 on number line?','Evaluate principal root.', ['9 is a perfect square.','3×3=9.','√9=3, so mark the third positive unit.'],'3.','Its position is exact.']
],[
 ['Plotting √2 at the integer 2.','The square root is smaller than its radicand in this range.','Compare 1² and 2².'],
 ['Using 1.4 as exact √2.','1.4²=1.96, not 2.','Use exact √2 and mark approximate location.']
],['Irrational square roots represent real lengths.','Construct with right triangles.','Keep exact and approximate values separate.']),
lesson(5,'Reasoning About Square Roots',[
 'Goal: bound and approximate square roots by comparing perfect squares and using numerical reasoning with positive values.',
 'To estimate √30, start from 25<30<36, so 5<√30<6, then refine using decimals. For nonnegative square roots, squaring preserves order, allowing useful checks.',
 'Make an original table of nearby squares to locate √30 and √50. A teacher reviews estimation and why a proposed bound is mathematically valid.'
],[
 ['Bounding squares','Bracket a radicand between known perfect squares.','a²<n<b² implies a<√n<b for a,b≥0.','25<30<36 gives 5<√30<6.'],
 ['Decimal refinement','Compare squared tenths or hundredths.','Use positive candidates.','5.4²=29.16 and 5.5²=30.25.'],
 ['Principal root','Positive or zero square root of a nonnegative radicand.','√n≥0.','√30≈5.477.']
],[
 ['Whole-number bound','Bound √30 between consecutive integers.','Find neighboring squares.', ['5²=25.','6²=36.','30 lies between 25 and 36.'],'5<√30<6.','Both endpoints are positive.'],
 ['Tenth-place bound','Is √30 between 5.4 and 5.5?','Compare squares.', ['5.4²=29.16<30.','5.5²=30.25>30.','Root is between 5.4 and 5.5.'],'Yes.','√30≈5.477 lies in interval.'],
 ['Bound √50','Find integers enclosing √50.','Compare perfect squares.', ['7²=49.','8²=64.','49<50<64.'],'7<√50<8.','√50≈7.071.']
],[
 ['Averaging bounding roots and calling it exact.','It is only an estimate.','Use inequalities or approximation sign.'],
 ['Squaring negative bounds without checking order.','Squaring is not order-preserving across negatives.','Use nonnegative root candidates.']
],['Perfect squares bound unknown roots.','Refine using decimal squares.','Always check approximation.']),
lesson(6,'Finding Side Lengths of Triangles',[
 'Goal: use right-triangle geometry to relate two perpendicular sides and the hypotenuse, and identify which sides are legs.',
 'The Pythagorean theorem applies only to right triangles. The hypotenuse is opposite the right angle and must be the longest side. A drawn slanted side is not automatically the hypotenuse unless the right angle is identified.',
 'Draw a 3-by-4 rectangle corner and connect endpoints to construct the right-triangle diagonal. Teacher review checks perpendicularity and correct side labels.'
],[
 ['Legs','The two sides that form the right angle.','Their squares add.','Leg lengths 3,4.'],
 ['Hypotenuse','Side opposite the right angle.','c²=a²+b², c largest.','For 3-4 legs, c=5.'],
 ['Right-angle condition','Angle between legs must equal 90°.','Do not use theorem for arbitrary triangle.','A triangle with sides 3,4,6 is not right.']
],[
 ['Right-triangle side','Legs 3 and 4: find hypotenuse.','Apply Pythagorean theorem.', ['c²=3²+4²=9+16=25.','Take positive square root.','c=5.'],'5 units.','5 is greater than both legs.'],
 ['Other hypotenuse','Legs 5 and 12: find hypotenuse.','Add squares.', ['5²=25 and 12²=144.','Sum 169.','√169=13.'],'13 units.','5²+12²=13².'],
 ['Identify wrong longest side','Can 3 and 4 be legs with c=6 in a right triangle?','Check squared relation.', ['3²+4²=25.','6²=36.','These differ.'],'No.','Hypotenuse would be 5, not 6.']
],[
 ['Using any triangle as right.','The theorem requires 90°.','Locate and verify the right angle.'],
 ['Labeling shorter side as hypotenuse.','The hypotenuse is longest.','Place c opposite 90°.']
],['Right-triangle legs form 90°.','Hypotenuse² equals sum of leg squares.','Use positive lengths.']),
lesson(7,'A Proof of the Pythagorean Theorem',[
 'Goal: develop an area-based proof of a²+b²=c² using four congruent right triangles in a larger square, not merely plug numbers into the formula.',
 'Arrange four congruent right triangles of legs a,b and hypotenuse c inside a square of side a+b. One arrangement leaves a central square of side c; its area is (a+b)²−4(ab/2). Expanding gives a²+b²=c².',
 'Create a first-party paper-piece rearrangement with four labeled triangles, explain why the center is a square, and account for all pieces. Teacher review of construction and proof is required; a numeric exercise alone does not certify proof understanding.'
],[
 ['Large square area','Area of enclosing square of side a+b.','(a+b)²=a²+2ab+b².','If a=3,b=4, large square area 49.'],
 ['Four right triangles','Each triangle has area ab/2.','Total area 4×ab/2=2ab.','For a=3,b=4, four total 24.'],
 ['Central square','The remaining shape has side c, the hypotenuse.','c²=(a+b)²−2ab=a²+b².','For a=3,b=4, c²=25.']
],[
 ['Derive formula','Subtract four triangles from square (a+b)².','Compute areas symbolically.', ['Large area (a+b)²=a²+2ab+b².','Four triangles have area 4(ab/2)=2ab.','Remaining area c²=a²+b².'],'a²+b²=c².','Both descriptions cover the same enclosed area.'],
 ['Check 3-4 proof','For legs 3 and 4, find c² by rearrangement.','Subtract areas.', ['Enclosing square side 3+4=7 gives area 49.','Four triangles area 4×(3×4/2)=24.','Center square area 49−24=25.'],'c²=25; c=5.','3²+4²=25.'],
 ['Check general algebra','Simplify (a+b)²−2ab.','Expand and cancel.', ['(a+b)²=a²+2ab+b².','Subtract 2ab.','Result a²+b².'],'a²+b².','The cross terms cancel exactly.']
],[
 ['Using numerical examples as a complete universal proof.','A few triangles do not establish all right triangles.','Use symbolic area reasoning.'],
 ['Forgetting the four triangles in area count.','Then the central region is miscomputed.','Count all four congruent pieces.']
],['Rearrangement proves the theorem by equal area.','Four triangles and the central square fill one larger square.','Symbolic cancellation establishes a²+b²=c².']),
lesson(8,'Finding Unknown Side Lengths',[
 'Goal: solve for an unknown leg or hypotenuse in a right triangle, recognizing when subtraction versus addition of squares is appropriate.',
 'The largest side c is the hypotenuse; if c and leg a are known, the missing leg b=√(c²−a²). Check that c>a>0 so the radicand is positive in a nondegenerate triangle.',
 'Model a ladder as a right-triangle hypotenuse and its wall distance as a leg. Teacher reviews realism of perpendicular assumptions and units.'
],[
 ['Missing hypotenuse','Both legs known.','c=√(a²+b²).','legs 8,15 give c=17.'],
 ['Missing leg','Hypotenuse and one leg known.','b=√(c²−a²).','c=13,a=5 gives b=12.'],
 ['Feasible side lengths','Hypotenuse must exceed each nonzero leg.','c>a and c>b.','c=5 and leg 6 is impossible.']
],[
 ['Find hypotenuse','Right triangle legs 8 and 15. Find c.','Add squares.', ['8²=64 and 15²=225.','Sum 289.','√289=17.'],'17 units.','17²=289.'],
 ['Find leg','Hypotenuse 13, one leg 5. Find other.','Subtract leg square.', ['13²−5²=169−25.','Difference 144.','√144=12.'],'12 units.','5²+12²=13².'],
 ['Reject impossible input','Can a right triangle have hypotenuse 5 and leg 6?','Check side-length inequality.', ['Hypotenuse must be the longest.','Proposed leg 6 exceeds 5.','The geometry is impossible.'],'No.','c²−a² would be negative.']
],[
 ['Adding squares when finding a missing leg.','That would make the leg longer than hypotenuse.','Subtract from c².'],
 ['Using negative length.','Lengths are nonnegative.','Choose principal root.']
],['Add squares for hypotenuse.','Subtract squares for unknown leg.','Check physical feasibility.']),
lesson(9,'The Converse',[
 'Goal: use the converse of the Pythagorean theorem to test whether a triangle is right, and distinguish a proof from a simple diagram appearance.',
 'For side lengths a≤b≤c, equality a²+b²=c² establishes a right triangle; if not equal, the triangle is not right. The triangle must satisfy ordinary triangle inequalities as well.',
 'Sort independently prepared side triples into right and non-right groups with exact squared sums. Teacher checks the logical converse and classification.'
],[
 ['Pythagorean converse','If a²+b²=c² for a valid triangle with c longest, it is right.','Square exact lengths.','5²+12²=13².'],
 ['Longest-side choice','c is largest length before applying converse.','Sort lengths first.','For 7,24,25, c=25.'],
 ['Triangle validity','Any two side lengths sum greater than third.','a+b>c for sorted positive lengths.','2,3,6 do not form triangle.']
],[
 ['Test right triple','Are 5,12,13 sides of right triangle?','Use converse.', ['5²+12²=25+144=169.','13²=169.','Equality holds.'],'Yes, right triangle.','Hypotenuse side length is 13.'],
 ['Test other triple','Are 4,5,6 right triangle sides?','Compare squared sums.', ['4²+5²=16+25=41.','6²=36.','41≠36.'],'No, not right.','The converse equality fails.'],
 ['Reject invalid triangle','Can sides 2,3,6 form a triangle?','Use triangle inequality.', ['Sum of shorter sides 2+3=5.','Longest side 6 exceeds 5.','The segments cannot close into a triangle.'],'No triangle exists.','Triangle inequality fails.']
],[
 ['Using any side as c.','Converse requires longest side for c.','Sort side lengths.'],
 ['Testing Pythagoras before confirming triangle exists.','Some length triples cannot close.','Check triangle inequality.']
],['Converse determines right triangles.','Square exact lengths and compare.','Ensure a valid triangle and longest side.']),
lesson(10,'Applications of the Pythagorean Theorem',[
 'Goal: apply right-triangle distances in real contexts while identifying perpendicular sides, choosing units and interpreting a calculated length.',
 'A 6-meter eastward and 8-meter northward displacement makes a right angle, giving straight-line displacement √(6²+8²)=10 m. Distinguish straight distance from travel along the two orthogonal paths (14 m).',
 'Draw an original map with scale, perpendicular axes and a destination. Explain when right-angle assumptions are justified. Teacher checks the diagram and real-world interpretation.'
],[
 ['Perpendicular displacement','Two movements at 90° form triangle legs.','Straight distance uses square sum.','6 east and 8 north gives 10 straight.'],
 ['Path length','Total length along segments.','Add traveled segment lengths.','6+8=14 along grid paths.'],
 ['Direct distance','Hypotenuse between start and end.','√(a²+b²).','√100=10.']
],[
 ['Street-grid distance','Walk 6 m east, then 8 m north. Find direct distance.','Use right triangle.', ['Leg squares 36 and 64.','Sum 100.','Direct distance √100=10 m.'],'10 m.','Distinct from walked path 14 m.'],
 ['Ladder context','Ladder 5 m long reaches wall at 4 m. Ground foot distance?','Find missing leg.', ['5²−4²=25−16.','Difference 9.','Horizontal distance √9=3 m.'],'3 m.','3²+4²=5².'],
 ['Diagonal rectangle','Room 9 m by 12 m. Floor diagonal?','Use orthogonal sides.', ['9²=81.','12²=144.','Sum 225; root 15.'],'15 m.','9-12-15 is a 3-4-5 multiple.']
],[
 ['Adding leg lengths for straight distance.','That gives path travel, not direct hypotenuse.','Use Pythagoras for straight distance.'],
 ['Using the theorem on nonperpendicular measurements.','The condition must be a right angle.','Confirm geometry.']
],['Recognize perpendicular legs.','Keep traveled and direct distances distinct.','Verify units and context.']),
lesson(11,'Finding Distances in the Coordinate Plane',[
 'Goal: derive and use the coordinate distance formula from horizontal and vertical displacements and Pythagorean theorem.',
 'Two points (x₁,y₁),(x₂,y₂) determine horizontal difference Δx and vertical difference Δy. Euclidean distance is √(Δx²+Δy²), symmetric in point order and never negative.',
 'Plot a pair of points with negative coordinates, draw its right-triangle legs, and explain why differences rather than raw coordinates enter the formula. Teacher checks the constructed visual.'
],[
 ['Horizontal change','Difference in x-coordinate.','Δx=x₂−x₁.','From (−2,1) to (4,9), Δx=6.'],
 ['Vertical change','Difference in y-coordinate.','Δy=y₂−y₁.','From (−2,1) to (4,9), Δy=8.'],
 ['Distance formula','Hypotenuse from coordinate differences.','d=√((x₂−x₁)²+(y₂−y₁)²).','With differences 6,8, d=10.']
],[
 ['Compute signed-point distance','Distance between (−2,1) and (4,9)?','Use difference squares.', ['Δx=4−(−2)=6.','Δy=9−1=8.','d=√(36+64)=10.'],'10 units.','Sign of start x is handled correctly.'],
 ['Horizontal distance','Distance between (−3,5) and (2,5)?','Zero vertical change.', ['Δx=2−(−3)=5.','Δy=0.','d=√25=5.'],'5 units.','Matches number-line difference.'],
 ['Diagonal unit','Distance between (0,0) and (1,1)?','Compute right triangle.', ['Δx=1.','Δy=1.','d=√(1+1)=√2.'],'√2 units.','Exact irrational length.']
],[
 ['Adding raw coordinates instead of differences.','Translation of both points should not change distance.','Use coordinate differences.'],
 ['Reporting negative distance.','Distance is a nonnegative magnitude.','Take principal square root.']
],['Coordinate distance comes from Pythagoras.','Use signed coordinate differences.','Distance is invariant under common translations.']),
lesson(12,'Edge Lengths and Volumes',[
 'Goal: connect the volume of a cube with its edge length and estimate how volume changes under uniform 3D scaling.',
 'A cube with edge e has V=e³. Doubling edge produces eightfold volume. Given volume 125 cubic units, the positive cube root gives edge 5 units.',
 'Build 2×2×2 and 3×3×3 cube arrays from unit cubes or sketches, and explain why their volumes are 8 and 27. Teacher review of counting layers is required.'
],[
 ['Cube volume','All three edge dimensions are e.','V=e³.','Edge 3 gives volume 27.'],
 ['Uniform scale volume','Scale each edge by k.','V changes by k³.','Doubling edge gives ×8 volume.'],
 ['Inverse edge','Positive cube root of volume.','e=∛V for nonnegative physical volume.','V=125 gives e=5.']
],[
 ['Cube edge 3','Find volume of cube edge 3.','Cube the edge.', ['Each layer contains 3×3=9 cubes.','There are three layers.','Total 9×3=27.'],'27 cubic units.','3³=27.'],
 ['Double cube edge','A cube edge grows from 2 to 4. Volume factor?','Compare cubes.', ['Old volume 2³=8.','New volume 4³=64.','Ratio 64/8=8.'],'Factor 8.','Edge factor 2 cubes to 2³.'],
 ['Inverse volume','Cube volume 125 cubic units. Find edge.','Take cube root.', ['Edge e obeys e³=125.','5×5×5=125.','Positive physical edge e=5.'],'5 units.','5³=125.']
],[
 ['Squaring edge for cube volume.','Three dimensions multiply.','Use e³.'],
 ['Applying square root instead of cube root to volume.','Inverse exponent is 1/3.','Use ∛V.']
],['Cube volume scales with third power.','Edge comes from cube root.','Use cubic units.']),
lesson(13,'Cube Roots',[
 'Goal: interpret cube roots as inverse operations of cubing, including negative radicands and perfect cubes, and compare them with square roots.',
 'The real cube root of −8 is −2 because (−2)³=−8. Unlike real square roots of negative numbers, real cube roots exist for every real input. Use exact notation ∛n and check by cubing.',
 'Sort powers and inverse roots with original cards, including −27, 0, 1, 64 and 125. A teacher checks inverse-operation explanations.'
],[
 ['Cube root','Unique real x with x³=a.','x=∛a.','∛27=3.'],
 ['Negative cube root','Odd power keeps sign.','∛(−a)=−∛a for positive a.','∛(−8)=−2.'],
 ['Cube-square distinction','Square root of negative number is not real, but cube root is.','Odd exponent is invertible over reals.','∛(−27)=−3.']
],[
 ['Negative cube root','Find ∛(−27).','Find x such that x³=−27.', ['(−3)³=(−3)(−3)(−3).','Product is −27.','Therefore real cube root is −3.'],'−3.','(−3)³=−27.'],
 ['Positive cube root','Find ∛125.','Check known cubes.', ['5³=125.','The inverse operation gives 5.','No negative alternative, since (−5)³=−125.'],'5.','5³=125.'],
 ['Compare 0 root','What is ∛0?','Find number with cube zero.', ['0³=0.','No nonzero real x has x³=0.','Unique cube root is zero.'],'0.','0³=0.']
],[
 ['Ignoring sign of negative radicand.','Odd roots retain negative sign.','Check by cubing.'],
 ['Giving ± for a cube root.','Cubic is one-to-one over real numbers.','Use the unique real root.']
],['Cube roots invert cubes.','Negative numbers have negative real cube roots.','Verify by cubing.']),
lesson(14,'Decimal Representations of Rational Numbers',[
 'Goal: connect fractions to terminating and repeating decimal expansions using division, and explain why a repeating decimal can still represent an exact rational value.',
 'Long division for 1/4 terminates at 0.25, while 1/3 repeats 0.333…. Any rational fraction in lowest terms with denominator factors only 2 and 5 has a terminating base-ten decimal.',
 'Use original long-division diagrams and compare 3/8, 2/3 and 1/7. The teacher checks reasoning about remainders and repetition.'
],[
 ['Terminating decimal','Finite decimal digits after which representation ends.','Equivalent to denominator with prime factors 2 and/or 5 in lowest terms.','3/8=0.375.'],
 ['Repeating decimal','An eventual digit cycle extending indefinitely.','Long-division remainders repeat.','2/3=0.666….'],
 ['Exact rational','A fraction defines exact value regardless of decimal length.','0.333…=1/3.','Nonterminating repeating remains rational.']
],[
 ['Convert 3/8','Write 3/8 as decimal.','Divide.', ['8×0.3=2.4, leaving 0.6.','Long division yields further digits 7 and 5.','3/8=0.375.'],'0.375.','0.375×8=3.'],
 ['Convert 2/3','Write 2/3 as decimal.','Use repeating quotient.', ['2÷3=0 remainder 2.','The same remainder produces digit 6 repeatedly.','Result 0.666….'],'0.666… repeating.','0.666…×3=2 exactly in limiting sense.'],
 ['Convert 1/4','Write 1/4 as decimal.','Divide by 4.', ['4 goes into 1.00 twenty-five hundredths.','1/4=25/100.','Decimal 0.25.'],'0.25.','0.25×4=1.']
],[
 ['Calling repeating decimal irrational.','Repeating decimals are rational.','Use long division or geometric series.'],
 ['Treating truncated 0.66 as exact 2/3.','0.66<2/3.','Use repeating bar or ellipsis.']
],['Rational decimals terminate or eventually repeat.','Long division exposes repeated remainders.','Truncation is approximation.']),
lesson(15,'Infinite Decimal Expansions',[
 'Goal: distinguish infinite repeating decimal expansions from nonrepeating ones and reason about exact values and rationality.',
 'An infinite decimal such as 0.121212… repeats and is rational. A nonterminating nonrepeating expansion can represent an irrational number; however, a finite prefix never proves the continuation pattern.',
 'Write an original repeating pattern, derive 0.121212…=12/99=4/33, and compare with irrational √2. A teacher reviews explanations about the role of the infinite tail.'
],[
 ['Infinite repeating','Digit block repeats forever.','Eventually repeating decimals are rational.','0.121212…=4/33.'],
 ['Infinite nonrepeating','No eventual periodic digit block.','Represents irrational real number.','√2 has nonrepeating expansion.'],
 ['Finite prefix limit','Known digits alone do not prove full infinite pattern.','Distinguish finite observation and defined infinite rule.','1.4142 could prefix infinitely many different numbers.']
],[
 ['Derive repeating 12','Find fraction for x=0.121212….','Use shifting decimal.', ['100x=12.121212….','Subtract x: 99x=12.','x=12/99=4/33.'],'4/33.','The decimal repeats block 12.'],
 ['Classify repeated decimal','Is 0.777… rational?','Convert to fraction.', ['Let x=0.777….','10x−x=7.','9x=7, x=7/9.'],'Yes, rational.','Its infinite expansion repeats.'],
 ['Check finite approximation','Does 1.4142 equal √2 exactly?','Compare squared values.', ['1.4142²=1.99996164.','This is not exactly 2.','The displayed finite decimal approximates √2.'],'No; it is an approximation.','√2 is irrational and nonterminating.']
],[
 ['Inferring irrationality solely from infinite digits.','An infinite repeating decimal is rational.','Check periodicity.'],
 ['Treating decimal approximation as exact root.','Finite truncations have small but nonzero error.','Use ≈ and exact radicals.']
],['Infinite repeating decimals are rational.','Irrational decimals never eventually repeat.','Exact versus approximate matters.']),
lesson(16,'When Is the Same Size Not the Same Size?',[
 'Goal: compare display areas when screen aspect ratios differ, distinguishing equal diagonal lengths from equal surface areas.',
 'A rectangular screen with width w and height h has area A=wh, diagonal d=√(w²+h²). For fixed diagonal d and aspect ratio w:h=a:b, the dimensions are w=da/√(a²+b²), h=db/√(a²+b²). Different ratios can give different areas despite the same diagonal.',
 'Compare a 3:4 screen and a 9:16 screen of equal diagonal 20 units. Compute both exact areas and explain why diagonal alone is insufficient. A teacher checks a properly labeled drawing and comparison assumptions.'
],[
 ['Aspect ratio','Width-to-height ratio of a rectangle.','w/h=a/b.','4:3 and 16:9 are different shapes.'],
 ['Diagonal relation','Pythagorean relation for rectangle.','d²=w²+h².','3-by-4 rectangle has diagonal 5.'],
 ['Area at fixed diagonal','For ratio a:b and diagonal d, A=d²ab/(a²+b²).','Area depends on ratio, not only diagonal.','With d=20 and ratio 3:4, area=192.']
],[
 ['3:4 screen area','Screen ratio w:h=3:4 and diagonal 20. Find area.','Scale 3-4-5 triangle.', ['Ratio vector 3:4 has diagonal factor 5.','To reach diagonal 20 multiply both dimensions by 4.','Width 12, height 16, area 192.'],'192 square units.','12²+16²=400.'],
 ['9:16 screen area','Ratio 9:16 and diagonal 20. Express exact area.','Use fixed-diagonal area formula.', ['a²+b²=81+256=337.','Area=20²×(9×16)/337.','Compute 57600/337≈170.92.'],'57600/337 ≈170.92 square units.','Width and height individually satisfy the 20-unit diagonal.'],
 ['Compare equal diagonals','Which has larger area for equal diagonal 20, ratios 3:4 or 9:16?','Compare calculated areas.', ['3:4 area is 192.','9:16 area is 57600/337≈170.92.','The 3:4 screen has larger area despite equal diagonal.'],'3:4 has larger area.','Diagonal alone does not determine screen area.']
],[
 ['Using diagonal as if it were the rectangle height.','Diagonal is hypotenuse, not an edge.','Derive dimensions from ratio and Pythagoras.'],
 ['Claiming equal diagonal means equal area.','Different aspect ratios change area.','Compute w×h after identifying ratio.']
],['Screen diagonal does not uniquely determine area.','Aspect ratio constrains width and height.','Use exact geometry with specified dimensions.'])
];
if(rows.length!==16||new Set(rows.map(x=>x.LessonCode)).size!==16)throw Error('Unit8 count mismatch');
for(const r of rows)if(r.ExplanationParagraphs.length<2||r.KeyConcepts.length<3||r.WorkedExamples.length<3||r.CommonMistakes.length<2||r.SummaryPoints.length<3)throw Error('Bad shape '+r.LessonCode);
const out=path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u08-l01-l16-2017.rich-lesson-v2.json');
fs.writeFileSync(out,JSON.stringify({SchemaVersion:1,ContentVersion:'us-ccss-g8-u08-first-party-rich-v2-20261029-v1',PackCode:'US-CCSS-MATH',Lessons:rows},null,2)+'\n');
console.log('G8_U08 '+JSON.stringify({lessons:rows.length,examples:rows.reduce((a,r)=>a+r.WorkedExamples.length,0),out}));
