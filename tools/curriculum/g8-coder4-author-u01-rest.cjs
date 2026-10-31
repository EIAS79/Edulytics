'use strict';
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const out=path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u01-l09-l17-2017.rich-lesson-v2.json');
const concept=(Title,Definition,Rule,Example)=>({Title,Definition,Rule,Example});
const ex=(Title,Question,Method,Steps,Answer,Check)=>({Title,Question,Method,Steps,Answer,Check});
const error=(Mistake,WhyWrong,Correction)=>({Mistake,WhyWrong,Correction});
const lesson=(n,Title,explanations,concepts,examples,mistakes,summaries)=>({
 LessonCode:'PED:US-CCSS-MATH:G8:U01:L'+n,CultureCode:'en',Title,
 ExplanationParagraphs:[...explanations,
  'First-party Edulytiks teaching informed by Open Up Resources / Illustrative Mathematics Grade 8 (2017–2019), CC BY 4.0; original source: https://im.kendallhunt.com/MS/teachers/3/1/'+Number(n)+'/preparation.html . No publisher artwork or branded assessment is copied.'],
 KeyConcepts:concepts,WorkedExamples:examples,CommonMistakes:mistakes,SummaryPoints:summaries,Visuals:[],Videos:[]
});
const rows=[
lesson('09','Moves in Parallel',[
 'Learning objective: reason about how translations, reflections and rotations move lines, preserving parallelism, perpendicularity and intersection angles.',
 'A translation moves every point of a line by one vector, so it sends a line to itself or a parallel line. A rotation turns all directions through the same angle; parallel input lines stay parallel. A reflection reverses orientation but still preserves the angle between lines.',
 'Draw horizontal lines y=1 and y=4, then translate both upward two units. Compare their separation and slopes. Next rotate a pair of horizontal line segments a quarter-turn about the origin and observe how their directions change together.',
 'Student explanation: Is it possible for a rigid movement to turn two parallel lines into intersecting lines? Justify with a diagram and the meaning of rigid preservation. Written proof and a student-made drawing require teacher judgment.'
],[
 concept('Parallel lines','Two distinct coplanar lines that never intersect.','A common rigid move preserves parallelism.','The translated pair y=3 and y=6 remain three units apart.'),
 concept('Perpendicular lines','Lines meeting at a right angle.','Rigid moves preserve the 90° angle between lines.','Horizontal and vertical axes stay perpendicular after a joint quarter-turn.'),
 concept('Translation of a line','A uniform displacement of all its points.','For a line y=c, an upward translation of k gives y=c+k.','y=1 shifted up 2 becomes y=3.')
],[
 ex('Translate a parallel pair','What are the images of y=1 and y=4 after moving every point up two units?','Add two to the vertical coordinate of each point.',[
 'On y=1, every point (x,1) moves to (x,3), forming y=3.','On y=4, every point (x,4) moves to (x,6), forming y=6.','Their vertical separation stays |6−3|=3 units.'
 ],'y=3 and y=6; the lines remain parallel.','Both have slope 0 and never meet.'),
 ex('Rotate a horizontal line','Rotate the line y=2 by 90° counterclockwise around O. Which line results?','Use (x,y)→(−y,x) on any point of the line.',[
 'Every point has the form (x,2).','After the rotation its image is (−2,x).','Every image x-coordinate is −2, so the whole line is x=−2.'
 ],'x=−2, a vertical line.','The distance 2 from O remains 2.'),
 ex('Preserve a right angle','Two lines meet at 90°. Both rotate by 35° about the same point. What is their new angle?','A common rigid rotation preserves the difference between line directions.',[
 'Suppose their directions originally differ by 90°.','Both directions gain the same rotation of 35°.','The difference is still 90° because the common angle cancels.'
 ],'90°.','A shared rotation does not change the angle between lines.')
],[
 error('Rotating one of two lines and claiming a common rigid image.','Moving only one line can destroy parallelism.','Apply the same transformation to both lines.'),
 error('Assuming that a translated line must have the same equation.','A vertical shift changes its intercept while preserving its direction.','Track two points and write the new equation.')
],['Common rigid movements preserve parallelism and perpendicularity.','Translate a line by transforming all its points.','Reason from angles and distances, not drawings alone.']),
lesson('10','Composing Figures',[
 'Learning objective: infer unknown measurements when shapes are composed or moved by rigid transformations, without physically remeasuring every image edge.',
 'When two figures are assembled from rigid copies, corresponding edge lengths and angles transfer exactly. Mark equal pieces on a diagram before deriving total perimeter, area or unknown angle; do not assume an overlapping edge counts twice on the outside perimeter.',
 'Original task: build a 6-by-2 rectangle from two identical 3-by-2 rectangles. Translate one piece next to the other and ask what happens to the shared interior boundary. Discuss which lengths become exterior edges.',
 'For a rotated image of a rectangle, report the same lengths and right angles regardless of orientation. A teacher evaluates how the learner tracks a composite diagram; numeric checks can test the resulting measurements.'
],[
 concept('Transferred length','A rigid image segment has the same length as its source segment.','Use correspondence instead of remeasuring.','A 6-unit edge stays 6 units after a 90° rotation.'),
 concept('Composite perimeter','The outside boundary of assembled pieces.','Do not include shared interior edges in the outside perimeter.','Two 3-by-2 rectangles joined along a 2-unit side form a 6-by-2 rectangle.'),
 concept('Right angles under movement','A rotated or reflected right corner remains 90°.','Rigid transformations preserve angles.','The four right angles of a rectangle remain right angles.')
],[
 ex('Compose two rectangles','Two 3-by-2 rectangles join along a 2-unit side. What is the perimeter of the new shape?','Determine its overall outside dimensions.',[
 'The combined width is 3+3=6 units.','The height remains 2 units; the shared boundary is inside.','The outer perimeter is 2(6+2)=16 units.'
 ],'16 units.','Adding 10+10 would double-count the shared 2-unit edge, giving an incorrect 20.'),
 ex('Transfer a rotated measurement','A 6-by-2 rectangle is turned 90°. How long is its new long side?','Use rigid preservation, not its position.',[
 'The original longer side measures 6.','A 90° rotation changes orientation only.','The corresponding rotated long side is still 6 units.'
 ],'6 units.','Rotation cannot stretch an edge.'),
 ex('Compute area after composition','Two 3-by-2 nonoverlapping rectangles share one side to form one 6-by-2 rectangle. What area does the final figure cover?','Add areas or multiply new dimensions.',[
 'Each small rectangle covers 3×2=6 square units.','They meet only at a boundary, so there is no area overlap.','The total area is 6+6=12 square units, agreeing with 6×2.'
 ],'12 square units.','Area adds when interiors do not overlap.')
],[
 error('Counting a joined edge as exterior perimeter.','The interior seam is not part of the outside boundary.','Trace the outer boundary once.'),
 error('Assuming rotation changes dimensions.','Rigid movement changes position/orientation, not side length.','Identify corresponding edges.')
],['Rigid transformations transfer measurements exactly.','Perimeter follows the outer outline, not internal seams.','Use diagrams and correspondence to explain calculations.']),
lesson('11','What Is the Same?',[
 'Learning objective: decide whether two figures are the same size and shape, even when their positions or orientations differ, and distinguish congruence from mere visual resemblance.',
 'A square and its rotated copy are congruent because a rigid movement takes one to the other. A scaled square may look similar but is not congruent because its side lengths changed. Corresponding vertices and sides help specify an exact rigid mapping.',
 'Use labeled, independently drawn triangles with side lengths 3,4,5. Mirror one triangle and shift it away from the first. Decide whether the result is congruent and explain which measurements are unchanged.',
 'Student challenge: give one example of a pair that looks alike but is not congruent and one pair whose orientation differs but is congruent. A teacher evaluates explanations and sketches, not an asserted one-word machine-graded result.'
],[
 concept('Congruent figures','Figures related by a rigid movement and having identical corresponding lengths and angles.','Size and shape must agree; position and orientation need not.','A 3-4-5 triangle reflected in a line is congruent to itself.'),
 concept('Similar but not congruent','Matching shape with different scale.','A change in side length blocks congruence.','A 2-by-2 square and a 4-by-4 square are similar, not congruent.'),
 concept('Correspondence','A matching of original to image vertices and edges.','Label matching corners consistently.','A maps to A′ and side AB maps to A′B′ with the same length.')
],[
 ex('Check a reflected triangle','Triangle sides measure 3, 4 and 5. Its mirror image has those lengths and matching angles. Are they congruent?','A reflection is rigid and preserves geometry.',[
 'Reflection preserves all side lengths.','The three reflected sides remain 3,4,5.','Angles are also preserved even though the orientation reverses.'
 ],'Yes, they are congruent.','Reflection provides the required rigid mapping.'),
 ex('Reject a scaled square','One square has side 2 and another has side 4. Are they congruent?','Compare a corresponding side measurement.',[
 'The first side length is 2.','The second is 4, double the first.','Rigid transformations cannot double lengths.'
 ],'No. They have the same shape but not the same size.','A scale factor of 2 is not rigid.'),
 ex('Rotated rectangle','A 2-by-5 rectangle is rotated 90° and moved three units right. Is it still congruent?','Use the two rigid steps.',[
 'The rotation preserves every side and angle.','The translation also preserves every side and angle.','The composition therefore maps the original to a congruent figure.'
 ],'Yes.','Both steps are rigid regardless of the final orientation.')
],[
 error('Comparing only the orientation of figures.','Congruent figures need not point the same way.','Find rigid movements and compare corresponding measures.'),
 error('Equating similarity with congruence.','A dilation changes size when the scale factor is not 1.','Test at least one side length.')
],['Congruence means equal size and shape.','Rigid movements can change orientation and position.','Scaled copies may be similar without being congruent.']),
lesson('12','Congruent Polygons',[
 'Learning objective: match the corresponding vertices, sides and angles of two polygons and justify congruence using a rigid transformation or complete measurement correspondence.',
 'Use the order of vertices to prevent a false correspondence. The same quadrilateral can appear reversed after reflection; tracking labels and the matching sides matters more than visual left-to-right order.',
 'Draw quadrilateral A(1,1), B(4,1), C(4,3), D(1,3) and translate it by (−5,2). Label A′,B′,C′,D′. Check that all four sides and right angles correspond.',
 'Investigate two visually similar polygons with one changed edge. A teacher reviews a learner-drawn correspondence diagram and explanation; calculations can check specific vertices and lengths.'
],[
 concept('Corresponding sides','Pairs of edges connected by the same vertex mapping.','If A maps to A′ and B to B′ then AB corresponds to A′B′.','AB length 3 matches A′B′ length 3.'),
 concept('Matching vertex order','An ordered correspondence between polygons.','Consistent labels determine which angles and edges to compare.','ABCD must map to A′B′C′D′ under the stated translation.'),
 concept('Congruence under a translation','All vertices shift by one vector.','The same vector preserves every side and angle.','(1,1) shifted (−5,2) becomes (−4,3).')
],[
 ex('Translate all four vertices','Translate A(1,1), B(4,1), C(4,3), D(1,3) by (−5,2).','Add −5 to x and 2 to y for every vertex.',[
 'A′=(−4,3), B′=(−1,3).','C′=(−1,5), D′=(−4,5).','The width stays 3 and height stays 2.'
 ],'A′(−4,3), B′(−1,3), C′(−1,5), D′(−4,5).','The displacement is (−5,2) at each vertex.'),
 ex('Identify corresponding edge','A maps to A′ and B maps to B′. Which image edge corresponds to AB?','Follow the labeled endpoint mapping.',[
 'First endpoint A becomes A′.','Second endpoint B becomes B′.','The edge joining their images is A′B′.'
 ],'A′B′.','The mapping respects both endpoints.'),
 ex('Spot an invalid claim','A quadrilateral has a side of 3 units but its proposed congruent image has the corresponding side of 4 units. Can that pair be congruent?','Use equality of corresponding lengths.',[
 'A rigid transformation cannot change length.','The proposed lengths 3 and 4 disagree.','Thus the stated correspondence is not congruent.'
 ],'No, not under that correspondence.','A valid congruence must pair sides of equal length.')
],[
 error('Matching vertices only by nearest location.','A transformation may carry faraway vertices to closer or farther locations.','Use the stated transformation and ordered labels.'),
 error('Ignoring one side when claiming congruence.','One mismatch disproves congruence.','Check all relevant corresponding measurements or a complete rigid mapping.')
],['Track every corresponding vertex.','A rigid map of the polygon preserves sides and angles.','Explain why the mapping is valid.']),
lesson('13','Congruence',[
 'Learning objective: test whether figures can be mapped onto one another using translations, rotations and reflections, and give sufficient mathematical evidence for the decision.',
 'A sequence of rigid moves gives a constructive explanation of congruence. Equal side lengths can support a congruence claim for triangles, but for an arbitrary quadrilateral side lengths alone may leave different angles and shapes; do not overgeneralize triangle tests.',
 'Create two 3-4-5 triangles: a right triangle with legs 3 and 4, and its shifted reflection. Trace and overlay corresponding sides and explain why the mapping exists.',
 'Challenge: two quadrilaterals each have four sides of length 2, but one is a square and the other a slanted rhombus. Compare their angles to decide whether they are congruent. A teacher reviews the logic.'
],[
 concept('Rigid-map criterion','A congruence can be established by a series of rigid transformations.','Give a complete translation/rotation/reflection sequence.','A triangle and its shifted mirror are congruent.'),
 concept('Triangle side evidence','Equal corresponding three sides determine a triangle up to rigid motions.','SSS works for triangles, not arbitrary polygons.','Triangles with sides 3,4,5 are right and congruent.'),
 concept('Side evidence limitation','Arbitrary quadrilaterals may have equal side lengths but different angles.','Compare interior angles or show a rigid mapping.','Square and non-square rhombus may each have side 2.')
],[
 ex('Use SSS on triangles','Two triangles each have side lengths 3,4,5. Are they congruent?','Use the three-side triangle criterion.',[
 'Match the 3-unit sides, 4-unit sides and 5-unit sides.','All three corresponding pairs agree.','A triangle is determined up to rigid movement by the lengths of its three sides.'
 ],'Yes, by SSS.','Both have a 90° angle between sides of lengths 3 and 4.'),
 ex('Reject a rhombus-square comparison','A square and a non-square rhombus both have four sides of length 2. Must they be congruent?','Compare angle evidence.',[
 'All four side lengths agree.','A square has four right angles; a non-square rhombus has acute and obtuse angles.','Rigid movement preserves angles, so the two polygons cannot coincide.'
 ],'No. Equal sides alone are insufficient for these quadrilaterals.','One unequal corresponding angle rules out congruence.'),
 ex('Reject a doubled triangle','A 3-4-5 triangle is enlarged to sides 6,8,10. Is the image congruent?','Test length preservation.',[
 'The scale factor is 2.','Each new side is double the corresponding original side.','No composition of rigid movements changes a length by factor 2.'
 ],'Not congruent; only similar.','A dilation with scale 2 is not rigid.')
],[
 error('Claiming SSS proves every polygon congruent.','Four-sided figures can flex while keeping side lengths fixed.','Use triangle criteria only for triangles; compare angles or mapping for other shapes.'),
 error('Demanding figures coincide before any move.','A reflection or rotation can connect congruent figures.','Show a legitimate rigid sequence.')
],['Rigid transformations establish congruence.','SSS is a triangle-specific test.','For general polygons, lengths alone may not suffice.']),
lesson('14','Alternate Interior Angles',[
 'Learning objective: explain why alternate interior angles formed by a transversal crossing parallel lines are equal, including a geometric movement explanation.',
 'Draw two parallel horizontal lines and one slanted transversal. The interior angles lie between the two lines. Alternate interior angles are on opposite sides of the transversal; they can be matched by an appropriate 180° turn around the midpoint between intersection points.',
 'Original investigation: mark a slanted line y=x meeting horizontal lines y=0 and y=3. Measure the acute angle formed with each horizontal line and compare them. Then show why a straight line and the parallel condition force equality.',
 'Student tasks include selecting correct angle pairs in a labeled diagram and explaining equality; human review should check the correspondence rather than merely one angle value.'
],[
 concept('Transversal','A line intersecting two other lines at distinct points.','Mark both intersections to locate angle pairs.','y=x crosses the horizontal lines y=0 and y=3.'),
 concept('Alternate interior angles','Angles inside the parallel lines on opposite sides of the transversal.','With parallel lines, the alternate interior angle measures agree.','Two 45° acute angles made by y=x with horizontal parallel lines.'),
 concept('Straight angle','Adjacent angles on a straight line total 180°.','Use supplementary angles to derive related measures.','If one angle is 65°, its adjacent straight-line angle is 115°.')
],[
 ex('Infer the alternate angle','Parallel lines are cut by a transversal. One alternate interior angle is 65°. Find the other.','Use the parallel-line equality.',[
 'Identify two interior angles on opposite sides of the transversal.','Their corresponding rays are related by a rigid rotation between intersections.','Parallelism makes the two alternate interior angles equal.'
 ],'65°.','The second cannot be 115° unless it is a different, supplementary angle.'),
 ex('Infer the adjacent angle','An angle at a line intersection measures 65°. Find its adjacent supplementary angle.','Use the 180° straight angle sum.',[
 'Adjacent angles along a line form one straight angle.','Subtract 65° from 180°.','The result is 115°.'
 ],'115°.','65°+115°=180°.'),
 ex('Analyze a 45° transversal','The line y=x crosses two horizontal parallel lines. What acute angle does it make with each?','Compare equal horizontal and vertical changes.',[
 'A step along y=x has horizontal change 1 and vertical change 1.','An equal-rise equal-run right triangle has acute angles 45° and 45°.','Translation along the transversal carries one intersection to the other without changing the angle.'
 ],'45° at both intersections.','The transversal has slope 1 and the horizontals have slope 0.')
],[
 error('Choosing two interior angles on the same side.','Those are co-interior angles, not alternate interior.','Check both interior position and opposite transversal sides.'),
 error('Claiming the equality for nonparallel lines.','The equal-angle theorem depends on the lines being parallel.','Verify the parallel-line hypothesis.')
],['Alternate interior angles are equal when cut by a transversal across parallel lines.','Supplementary angles add to 180°.','A labeled diagram and a justification are essential.']),
lesson('15','Adding the Angles in a Triangle',[
 'Learning objective: conjecture, verify and use the 180° angle sum of a triangle, distinguishing an interior angle from the adjacent exterior angle.',
 'Draw a scalene triangle, cut out the three corners and assemble them at a point so that the three wedges form a straight angle. This supports the fact that the angles in any Euclidean triangle add to 180°.',
 'Explore acute, right and obtuse triangles. The sum remains the same regardless of the triangle shape. Then use two given angles to calculate a missing third one.',
 'A learner should explain why their rearranged angle pieces form a straight angle. A teacher checks the physical construction and proof while numeric missing-angle answers can be graded independently.'
],[
 concept('Triangle angle sum','The three interior angle measures in a Euclidean triangle add to 180°.','A+B+C=180°.','Angles 40°,60°,80° form a valid triangle sum.'),
 concept('Exterior angle','The angle adjacent to a triangle interior angle after extending one side.','Exterior and adjacent interior add to 180°.','Exterior to a 70° angle is 110°.'),
 concept('Right triangle','A triangle with one 90° angle.','The other two angles total 90°.','Other angles 30° and 60° complete the right triangle.')
],[
 ex('Find the third angle','Two triangle angles are 50° and 60°. Find the last one.','Subtract the two known angles from 180°.',[
 'First combine the known angles: 50°+60°=110°.','Subtract 110° from the triangle total 180°.','The remaining angle is 70°.'
 ],'70°.','50°+60°+70°=180°.'),
 ex('Check a right triangle','One angle is 90° and another is 35°. Find the third.','The remaining acute angles total 90°.',[
 'The entire triangle sum is 180°.','Remove 90° for the right angle and 35° for the known acute angle.','Compute 180°−90°−35°=55°.'
 ],'55°.','90°+35°+55°=180°.'),
 ex('Use an exterior angle','A triangle interior angle at a vertex is 70°. Find its adjacent exterior angle.','Use a straight-line supplementary pair.',[
 'The interior and exterior rays lie on a straight line.','Their angle sum is 180°.','180°−70°=110°.'
 ],'110°.','70°+110°=180°.')
],[
 error('Adding angles to 360° as though a triangle were a quadrilateral.','Triangles have a 180° total.','Arrange the three corners along a straight angle.'),
 error('Confusing exterior and opposite interior angles.','Exterior is formed by prolonging one side at a vertex.','Mark the straight-line pair first.')
],['The three triangle interior angles sum to 180°.','Unknown angles follow by subtraction.','An adjacent exterior angle is supplementary to its interior angle.']),
lesson('16','Parallel Lines and the Angles in a Triangle',[
 'Learning objective: derive the 180° interior-angle sum of a triangle by constructing a parallel line and applying alternate interior angle properties.',
 'Through the top vertex C of triangle ABC draw a line parallel to base AB. The rays CA and CB form angles with this new line that equal the two base angles by alternate interior angle relationships. Those two angles and angle C form a straight angle.',
 'This creates a proof rather than relying solely on repeated measurements or cut paper. Reconstruct the diagram with a different-shaped triangle to check which hypothesis makes the correspondence valid.',
 'Learners draw their own parallel and label equal angles. A teacher assesses the logical proof, while exact given-angle calculations can be independently checked.'
],[
 concept('Parallel through a vertex','A line drawn through triangle vertex C parallel to opposite side AB.','Use parallel-line alternate interior angles to transfer the base angles to C.','The angle corresponding to A appears beside angle C.'),
 concept('Straight-angle proof','Three adjacent angles along the constructed line total 180°.','The two transferred base angles and apex angle lie on one straight line.','A+B+C=180°.'),
 concept('Hypothesis matters','Parallelism is needed to transfer equal angles.','A nonparallel line through C does not automatically create alternate equal angles.','Confirm the constructed line is parallel to AB.')
],[
 ex('Derive the general sum','What do two transferred base angles and the top angle add to along the line through C parallel to AB?','Use the straight line and alternate interior equivalence.',[
 'Draw through C a line parallel to AB.','Match the angle corresponding to A on one side and the angle corresponding to B on the other.','These two angles plus C lie adjacent on a straight line and sum to 180°.'
 ],'A+B+C=180°.','The equality follows from parallel lines and straight-angle addition.'),
 ex('Find an angle using the theorem','A triangle has base angles of 42° and 73°. Find the apex angle.','Subtract both base angles from the straight-angle sum.',[
 'Add base angles: 42°+73°=115°.','The remaining apex angle is 180°−115°.','Compute 65°.'
 ],'65°.','42°+73°+65°=180°.'),
 ex('Check a proof claim','A student draws a nonparallel line through C and claims transferred alternate angles must match. Is that justified?','Inspect the required parallel hypothesis.',[
 'Alternate interior equality requires parallel lines.','The student did not establish that the new line is parallel to AB.','Thus the equality cannot be inferred from the drawing alone.'
 ],'No; the proof needs a line through C parallel to AB.','A diagram that merely looks parallel is not a geometric justification.')
],[
 error('Using alternate-angle equality without stating parallelism.','The theorem applies only when the two lines are parallel.','Label the parallel condition explicitly.'),
 error('Relying on a roughly straight sketch as proof.','The sum theorem needs an invariant reason.','Derive it from the constructed parallel and a straight angle.')
],['Draw a parallel through the opposite vertex.','Transfer base angles with alternate-angle equality.','The three adjacent angles make a 180° straight angle.']),
lesson('17','Rotate and Tessellate',[
 'Learning objective: make a repeating planar pattern with rigid transformations, explaining when copies of tiles cover the plane without gaps or overlaps.',
 'A tessellation fills the plane by repeated copies of shapes. At a grid vertex, angles from touching tiles must add to 360°. Equilateral triangles have 60° corner angles, so six fit around a point; squares have 90°, so four fit. Repeating regular pentagons alone do not fit at a vertex because their 108° angles do not divide 360°.',
 'Draw a triangle tile and make repeated rotated or translated copies on an accessible grid. Explain why the boundaries meet without gaps and why the transformations preserve tile shape.',
 'Student constructions and explanations are teacher-reviewed; an automated count of polygons is not enough to verify no gaps or overlaps in the actual drawing.'
],[
 concept('Tessellation','A gap-free, overlap-free tiling of the plane by figures.','Copies must fit along edges and around vertices.','Equilateral triangles tile with six 60° corners around a vertex.'),
 concept('Angle around a point','A full revolution around a point has measure 360°.','Tile angles meeting at the same vertex must sum to 360°.','Four square corners contribute 4×90°=360°.'),
 concept('Rigid tile placement','Rotation or translation of a tile preserving its size and angles.','The repeated tile must remain congruent to the original.','A square can be translated across rows without resizing.')
],[
 ex('Equilateral triangle tiling','How many 60° triangle corners meet exactly around a point?','Divide the full angle by one triangle angle.',[
 'The full angle around a point is 360°.','Each equilateral triangle corner is 60°.','Compute 360°÷60°=6.'
 ],'Six triangles.','6×60°=360°, leaving no angular gap.'),
 ex('Square tiling','How many 90° corners fill a vertex?','Divide one full turn by a right angle.',[
 'The total surrounding angle is 360°.','A square corner measures 90°.','360°÷90°=4.'
 ],'Four square corners.','4×90°=360°.'),
 ex('Test regular pentagon tiles','A regular pentagon has interior angle 108°. Can copies of that single regular pentagon fit exactly around a vertex?','Test whether the vertex angle divides 360° into a whole number.',[
 'Three pentagon angles total 3×108°=324°, leaving 36° gap.','Four total 432°, causing overlap.','No whole number of 108° corners fills exactly 360°.'
 ],'No, regular pentagons alone do not tessellate edge-to-edge around a vertex.','360°÷108°=3⅓ is not an integer.')
],[
 error('Calling a pattern a tessellation despite gaps.','A tessellation requires complete coverage without gaps or overlaps.','Inspect edge fit and angles around every vertex.'),
 error('Changing tile size between copies.','A repeated rigid tile must be congruent.','Use only translations, rotations or reflections for tile copies.')
],['Tessellations require no gaps and no overlaps.','Angles around a vertex total 360°.','Explain the chosen rigid movements and check the actual tiling.'])
];
if(rows.length!==9||new Set(rows.map(x=>x.LessonCode)).size!==9)throw Error('Unexpected Unit 1 row count');
for(const l of rows)if(l.ExplanationParagraphs.length<2||l.KeyConcepts.length<3||l.WorkedExamples.length<3||l.CommonMistakes.length<2||l.SummaryPoints.length<3)throw Error('Malformed lesson '+l.LessonCode);
fs.writeFileSync(out,JSON.stringify({SchemaVersion:1,ContentVersion:'us-ccss-g8-u01-l09-l17-first-party-rich-v2-20261029-v1',PackCode:'US-CCSS-MATH',Lessons:rows},null,2)+'\n');
console.log('G8_REST_OF_U01 '+JSON.stringify({lessons:rows.length,examples:rows.reduce((n,x)=>n+x.WorkedExamples.length,0),file:out}));
