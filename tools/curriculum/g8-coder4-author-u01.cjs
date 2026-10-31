'use strict';
// First-party, manually authored Grade 8 Unit 1 instructional repairs.
// Original Edulytiks explanations, math and tasks; no publisher assessment or artwork.
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const out=path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u01-l01-l08-2017.rich-lesson-v2.json');
const concept=(Title,Definition,Rule,Example)=>({Title,Definition,Rule,Example});
const ex=(Title,Question,Method,Steps,Answer,Check)=>({Title,Question,Method,Steps,Answer,Check});
const error=(Mistake,WhyWrong,Correction)=>({Mistake,WhyWrong,Correction});
const l=(i,Title,ExplanationParagraphs,KeyConcepts,WorkedExamples,CommonMistakes,SummaryPoints)=>({
 LessonCode:'PED:US-CCSS-MATH:G8:U01:L'+String(i).padStart(2,'0'),CultureCode:'en',Title,
 ExplanationParagraphs:[...ExplanationParagraphs,'This is independently authored Edulytiks instructional text informed by Open Up Resources / Illustrative Mathematics Grade 8 (2017–2019), CC BY 4.0. Source: https://im.kendallhunt.com/MS/teachers/3/1/'+i+'/preparation.html . Original textbook graphics and publisher-branded assessments are not reproduced.'],
 KeyConcepts,WorkedExamples,CommonMistakes,SummaryPoints,Visuals:[],Videos:[]
});
const rows=[
l(1,'Moving in the Plane',[
 'Learning objective: describe a slide or turn of a plane figure in everyday terms, identifying the direction and amount so another learner can repeat the motion. Formal names for transformations are introduced later.',
 'Trace the outline of an irregular triangle on transparent paper and move the tracing without stretching it. A slide sends every point equally far in the same direction; a turn moves the outline around a chosen fixed point. Compare where the corners end up, not just where the middle ends up.',
 'Partner investigation: each learner secretly draws a starting and ending position of the same triangular card, then gives verbal directions for the other to reconstruct the move. Test the explanation on an asymmetric figure; a vague instruction such as “move it over there” cannot be reproduced uniquely.',
 'Original Edulytiks odd-one-out task: compare a straight angle, acute angle, right angle and obtuse angle. Identify an odd one out and state a defensible criterion. Distinct choices with distinct correct mathematical reasons are allowed; a teacher judges the justification rather than a one-button numerical grader.'
],[
 concept('Slide','Repositioning a figure without rotating or changing its size.','All corresponding points travel the same distance and direction.','Shift triangle A(1,1),B(3,1),C(1,3) four squares to the right.'),
 concept('Turn','Moving a figure around a fixed center, changing its orientation.','Specify center, direction and fraction of a full turn.','Turn a right-pointing arrow one quarter-turn left around its tail: it points upward.'),
 concept('Rigid shape','A figure with side lengths and angles unchanged during a movement.','Sliding or turning must preserve every side and angle.','A 2-by-2 right triangle stays a 2-by-2 right triangle after a slide.')
],[
 ex('Describe the entire slide','Triangle A(1,1), B(3,1), C(1,3) slides four squares right and two squares up. Where are its new corners?','Add the same pair of movements to each corner.',[
 'A moves right four from x=1 to x=5, and up two from y=1 to y=3, giving A′(5,3).','B moves to (3+4,1+2)=(7,3).','C moves to (1+4,3+2)=(5,5).','Check that the new horizontal and vertical legs are each still two grid units long.'
 ],'A′(5,3), B′(7,3), C′(5,5).','All three points move (+4,+2); the legs retain length 2.'),
 ex('Describe a turn','An arrow points east. Rotate it a quarter-turn left around its tail. Which way does it point?','Hold its tail in place and turn counterclockwise through one fourth of a circle.',[
 'Mark the tail as the fixed center.','One fourth of 360 degrees is 90 degrees.','From east, a 90-degree left turn points north while the arrow length is unchanged.'
 ],'North (up).','The reverse clockwise quarter-turn would return the arrow to the east.'),
 ex('Find a justified odd one out','Four angles measure 35°, 90°, 125° and 180°. Give one mathematically justified odd-one-out.','Classify angles, then state the property that distinguishes a choice.',[
 '35° is acute, 90° is right, 125° is obtuse and 180° is straight.','Choose 180° as the only straight angle; alternatively another unique category can be used.','State the selected criterion explicitly so the teacher can assess your reasoning.'
 ],'Example: 180° is the only straight angle; other categories support other answers.','A single-choice automated grader is inappropriate for this open justification.')
],[
 error('Only saying “move it.”','Without direction and amount the destination cannot be reconstructed.','Give a repeatable move for every corner.'),
 error('Treating resizing as a slide.','Rigid movements keep all distances and angle measures unchanged.','Compare corresponding side lengths before deciding.'),
 error('Assuming there is only one odd one out.','Several different geometric classifications can single out different objects.','Grade the mathematical justification, not an assumed unique choice.')
],[
 'Slides move a figure without changing orientation, turns move it around a specified point.','Both moves preserve distances and angles.','Informal descriptions must still be reproducible.','Teacher review is required for alternative correct odd-one-out explanations.'
]),
l(2,'Naming the Moves',[
 'Learning objective: describe translations, rotations and reflections accurately, distinguish their effects on orientation and use clockwise and counterclockwise correctly.',
 'For a translation, match each original vertex to its image with the same displacement. For a rotation, name the fixed center and signed direction of turn. For a reflection, find a mirror line that bisects the perpendicular segment between each pair of corresponding points.',
 'Make three original paper examples with an asymmetric quadrilateral. Slide one tracing, turn a second around a chosen corner and flip a third over a drawn line. Sort the three pairs by the evidence you can show; do not infer “rotation” merely because the figures have moved.',
 'Explain the chosen action to another learner. A card-sort answer may be justified in more than one language, so use a teacher-reviewed explanation rather than an invented automatic classification of speech or drawings.'
],[
 concept('Translation','A shift sending all points through the same displacement.','Every corresponding point has the same movement vector.','A translation of (−3,2) moves left 3 and up 2.'),
 concept('Rotation','A turn of a specified angle about a chosen center.','Clockwise follows clock-hand direction; counterclockwise is the opposite.','A 90° clockwise turn of a north-pointing arrow yields an east-pointing arrow.'),
 concept('Reflection','A mirror movement across a specified line.','Paired points are perpendicular to and equidistant from the mirror line.','The reflection of (3,2) across the y-axis is (−3,2).')
],[
 ex('Uniform movement','A triangle moves every vertex three squares left and two up. Name the action.','Compare all displacement arrows.',[
 'A-to-A′, B-to-B′ and C-to-C′ all point left three and up two.','The orientation has not turned or flipped.','Therefore one translation describes the whole move.'
 ],'Translation by (−3,2).','All point-to-image displacements agree.'),
 ex('Use clock direction','An upward arrow makes a quarter-turn clockwise about its fixed tail. Which way will it point?','Hold its tail and turn in the direction clock hands move.',[
 'The tail is the center and remains fixed.','A quarter-turn has magnitude 90°.','Turning clockwise from north points east.'
 ],'East (right).','West would describe a counterclockwise rather than clockwise quarter-turn.'),
 ex('Locate a mirror image','P is three units right of a vertical mirror line and two units above a baseline. Describe P′.','Measure perpendicular distance and preserve the same height.',[
 'The point lies three horizontal units on the right side.','Place its image three horizontal units on the left side of the same line.','Retain the original height of two units above the baseline.'
 ],'Three units left of the mirror line, two units above the baseline.','Point and image are on opposite sides of the mirror at equal distance.')
],[
 error('Calling a reflection a half-turn.','A reflection reverses handedness whereas a rotation preserves it.','Track the orientation of three labeled corners.'),
 error('Omitting the rotation center.','The same angle about different centers produces different positions.','Mark center, angle and direction.'),
 error('Matching corresponding vertices by proximity.','The nearest vertex may not be the actual image of a chosen original vertex.','Track labels consistently from each starting vertex.')
],['Translations are uniform slides.','Rotations have a center, angle and direction.','Reflections require a line of symmetry.','Use corresponding points as evidence for the chosen action.']),
l(3,'Grid Moves',[
 'Learning objective: draw, label and justify the images of figures after translations, rotations and reflections on square and isometric grids.',
 'Label the original polygon A,B,C and each image A′,B′,C′. On a square coordinate grid, a translation by (a,b) changes (x,y) to (x+a,y+b). A counterclockwise quarter-turn about the origin changes (x,y) to (−y,x); reflecting across the y-axis changes (x,y) to (−x,y).',
 'On an isometric grid six 60° sectors meet around a grid point. A 60° turn about a grid vertex can be constructed with tracing paper by preserving distance from the center; do not mistake the slanted equilateral-triangle grid for perpendicular Cartesian axes.',
 'Student task: draw an irregular polygon and its translated image on a square grid, then choose a center and draw a turned copy on an isometric grid. Compare all corresponding vertices. Drawings and open construction explanations need accessible graphical/teacher review; three numerical coordinate examples alone cannot establish complete student assessment.'
],[
 concept('Image vertices','A′, B′, C′ label points corresponding to A, B, C after a movement.','Apply the same transformation to every point and preserve label correspondence.','A(1,1) shifted (4,−3) becomes A′(5,−2).'),
 concept('Square-grid rules','Coordinates describe special rigid motions about coordinate axes.','Translate by addition; rotate CCW 90° about origin by (−y,x); mirror in y-axis by (−x,y).','(2,1) rotates CCW 90° to (−1,2).'),
 concept('Isometric-grid turn','An equilateral-triangle grid gives directions separated by 60°.','Choose a grid-point center; maintain each vertex distance and rotate each center-to-vertex ray equally.','One grid edge along east rotates to the next 60° direction.')
],[
 ex('Translate and relabel','A(1,1), B(3,1), C(1,2) shift four units right and three down. Find A′,B′,C′.','Add 4 to x and subtract 3 from y for all three vertices.',[
 'A′=(1+4,1−3)=(5,−2).','B′=(3+4,1−3)=(7,−2).','C′=(1+4,2−3)=(5,−1).','Check B′−A′=(2,0) and C′−A′=(0,1), the same side vectors as before.'
 ],'A′(5,−2), B′(7,−2), C′(5,−1).','Each vertex has displacement (4,−3).'),
 ex('Turn a figure','Rotate the vertices (2,1), (3,1), (2,2) 90° counterclockwise about the origin.','Apply (x,y)→(−y,x) to every vertex.',[
 '(2,1) changes to (−1,2).','(3,1) changes to (−1,3).','(2,2) changes to (−2,2).','Their side lengths remain one unit on the two legs.'
 ],'(−1,2), (−1,3), (−2,2).','A clockwise turn would instead send (2,1) to (1,−2).'),
 ex('Reflect an individual vertex','Reflect P(3,−2) across the y-axis.','Reverse the sign of the horizontal coordinate.',[
 'The mirror line is x=0.','P lies three units to its right and has height y=−2.','Place the image three units to its left at the same height.'
 ],'P′(−3,−2).','The segment PP′ is horizontal and its midpoint is (0,−2).')
],[
 error('Moving only one vertex.','A transformation applies to every vertex of the figure.','Label each original and image corner before drawing the new edges.'),
 error('Using the clockwise formula for a counterclockwise turn.','(y,−x) and (−y,x) produce opposite rotations.','Check the unit vector (1,0) must turn to (0,1) counterclockwise.'),
 error('Using Cartesian grid assumptions on an isometric grid.','Its grid directions meet at 60°, not 90°.','Use the equilateral geometry and preserve distance from the chosen center.')
],['Each original vertex has one corresponding image vertex.','Coordinate rules can describe square-grid transformations exactly.','Isometric grid rotations can be built by 60° tracing.','Check constructions visually and preserve labels.']),
l(4,'Making the Moves',[
 'Learning objective: draw a specified translation, rotation or reflection and describe the construction so someone else can reproduce the image.',
 'A movement instruction is complete only when the image is determined. Translation requires a displacement; rotation requires center, direction and angle; reflection requires a line. Mark the same vertex labels on both figures to detect an accidental flip or shift.',
 'Construction activity: draw an asymmetric quadrilateral A(1,1), B(4,1), C(3,3), D(1,2). Create three different copies: a shift two right, a half-turn about the origin, and a mirror across the y-axis. Compare how the order of labeled vertices changes.',
 'A partner must reconstruct one of the copies from the written directions without seeing the result. A teacher evaluates completeness of directions and accurate diagram construction; a multiple-choice movement name does not substitute for this skill.'
],[
 concept('Specify a translation','A displacement vector determines a slide.','Apply (x+a,y+b) to every vertex.','Right two: (x,y)→(x+2,y).'),
 concept('Specify a half-turn','A 180° rotation reverses both coordinate signs when centered at the origin.','About O: (x,y)→(−x,−y).','B(4,1) becomes (−4,−1).'),
 concept('Specify a reflection','A mirror line divides every joining segment perpendicularly at its midpoint.','Across y-axis, reverse x and preserve y.','C(3,3) becomes (−3,3).')
],[
 ex('Complete a translation','Shift A(1,1), B(4,1), C(3,3), D(1,2) two units right.','Add two to each x-coordinate.',[
 'A′=(3,1) and B′=(6,1).','C′=(5,3) and D′=(3,2).','Connect A′B′C′D′ in original vertex order; all y-coordinates are unchanged.'
 ],'A′(3,1), B′(6,1), C′(5,3), D′(3,2).','The image and original have matching horizontal side lengths and angles.'),
 ex('Half-turn around origin','Find the half-turn image of P(4,−3) about the origin.','Change both coordinate signs.',[
 'A half-turn is 180°.','The x-coordinate 4 becomes −4.','The y-coordinate −3 becomes 3; check the origin is the midpoint of P and P′.'
 ],'P′(−4,3).','The midpoint of (4,−3) and (−4,3) is (0,0).'),
 ex('Reflect a segment','Reflect the endpoints A(1,2) and B(4,2) across the y-axis.','Reverse x for each point while keeping y.',[
 'A′=(−1,2).','B′=(−4,2).','The mirrored segment still has length 3 but points left from A′ to B′.'
 ],'A′(−1,2), B′(−4,2).','Each original-image point pair is equidistant from x=0.')
],[
 error('Providing just “rotate” in the instructions.','A center and angle are missing, so many outputs are possible.','State the center, angle and turn direction.'),
 error('Connecting transformed vertices out of order.','The drawing might cross where the original did not.','Preserve the original label sequence when connecting image vertices.')
],['Name the displacement, center or mirror line explicitly.','Transform every vertex before joining the edges.','Check a peer can reproduce written directions.']),
l(5,'Coordinate Moves',[
 'Learning objective: determine how the coordinates of corresponding points change under specified rigid movements, and justify the pattern rather than memorizing one unqualified formula.',
 'Track coordinates relative to the transformation center or mirror line. A horizontal reflection across x=0 negates x, while a vertical reflection across y=0 negates y. A quarter-turn around the origin depends on direction; the same formula cannot be used for rotations about other centers without moving the center first.',
 'Use transparent grid paper to compare P(3,2), its reflection across the x-axis, its reflection across the y-axis and a 90° counterclockwise rotation about O. Explain how each operation affects signs and coordinate order.',
 'Original task: a student supplies an incorrect image coordinate after a move. Diagnose the error using point-to-point distances or the required center/line. A teacher checks the explanation; numeric pairs can be independently checked.'
],[
 concept('Reflection across x-axis','Mirror over horizontal line y=0.','(x,y)→(x,−y).','(3,2) becomes (3,−2).'),
 concept('Reflection across y-axis','Mirror over vertical line x=0.','(x,y)→(−x,y).','(3,2) becomes (−3,2).'),
 concept('Quarter-turn around O','A 90° turn of a point around origin O=(0,0).','CCW: (x,y)→(−y,x); clockwise: (x,y)→(y,−x).','(3,2) CCW becomes (−2,3), clockwise becomes (2,−3).')
],[
 ex('Compare the two mirrors','Reflect P(3,2) across each coordinate axis.','Reverse only the coordinate perpendicular to each mirror line.',[
 'Across the x-axis, the y-coordinate changes sign: (3,−2).','Across the y-axis, the x-coordinate changes sign: (−3,2).','Both image points remain the same distance from the origin as P.'
 ],'Across x-axis: (3,−2); across y-axis: (−3,2).','All three points are at distance √13 from O.'),
 ex('Counterclockwise turn','Rotate Q(−4,1) a quarter-turn counterclockwise about O.','Exchange the coordinates and negate the old y-value.',[
 'Original coordinate pair is (−4,1).','New x is −1, new y is −4.','The squared distance 16+1=17 is preserved.'
 ],'Q′(−1,−4).','A clockwise turn would produce (1,4), not the requested image.'),
 ex('Detect an invalid reflection','A student says the x-axis reflection of (5,−3) is (−5,−3). Is that correct?','Check which coordinate is perpendicular to the x-axis.',[
 'The x-axis is the horizontal line y=0.','Mirroring changes the vertical coordinate from −3 to +3.','The horizontal coordinate must remain 5.'
 ],'No. The correct image is (5,3).','The student instead described a reflection across the y-axis.')
],[
 error('Negating both coordinates for any reflection.','That operation is a half-turn about O, not a single axis reflection.','Name the mirror line and change only its perpendicular coordinate.'),
 error('Applying an origin-centered rotation formula around a different center.','The formula assumes the center is (0,0).','Translate the center to O, rotate, and translate back.')
],['Coordinate sign changes depend on the chosen axis.','The direction of a quarter-turn changes the formula.','Verify center and distance invariance.']),
l(6,'Describing Transformations',[
 'Learning objective: use coordinate and geometric language to describe a single transformation or a sequence that maps one polygon onto another.',
 'Start with two labeled copies of a polygon. Check the displacement of every corresponding vertex before claiming it is a translation. For a rotation, compare distances from a proposed center. If the figure was mirrored, identify the reflection line using matching point pairs.',
 'An original Edulytiks investigation uses triangle A(1,1), B(3,1), C(1,2). First translate by (2,0), then reflect across the y-axis. Draw each intermediate image and name the ordered steps. Ask whether reversing the order would produce the same result.',
 'The sequence description and matching of image labels are open construction tasks. Numeric coordinate predictions can be auto-checked, but teacher review must assess whether an explanation is complete and logically justified.'
],[
 concept('Sequence of moves','A composition applies one movement to the output of another.','Track intermediate points and respect the order.','First (x,y)→(x+2,y), then mirror across y-axis to (−x−2,y).'),
 concept('Inverse movement','An inverse move restores the starting figure.','Reverse a translation vector, clockwise/counterclockwise direction or repeat a reflection.','Undo a move right two by moving left two.'),
 concept('Order matters','Different compositions can move a figure to different images.','Compute the image of one test point to compare orders.','Reflect (1,1) in y-axis then translate right two gives (1,1); reverse order gives (−3,1).')
],[
 ex('Translate then reflect','Point A(1,1) is translated right two, then mirrored in the y-axis. Find A″.','Apply each movement to the previous image.',[
 'After translating, A′=(1+2,1)=(3,1).','Reflect A′ across x=0 to get A″=(−3,1).','Keep the double prime to show two consecutive movements.'
 ],'A″(−3,1).','The x-coordinate is −(1+2)=−3.'),
 ex('Reverse the move order','Reflect A(1,1) in the y-axis, then shift right two. Find the result.','Perform reflection before translation.',[
 'Reflect A to A′=(−1,1).','Move right two to A″=(−1+2,1)=(1,1).','Compare this with (−3,1) from the other order.'
 ],'A″(1,1), different from translating then reflecting.','The two sequences differ by four horizontal units.'),
 ex('Undo a translation','A polygon moved left four and up three. Which single translation returns it?','Use the additive inverse displacement.',[
 'The forward displacement is (−4,3).','Its inverse must add 4 horizontally and subtract 3 vertically.','The total displacement (−4,3)+(4,−3)=(0,0) returns every point.'
 ],'Move right four and down three.','The composition is the identity movement.')
],[
 error('Reversing the order of a composition without checking.','Most transformation compositions do not commute.','Test a single labeled point in each order.'),
 error('Giving only the final coordinate pair.','It does not explain which sequence of movements produced the figure.','Name each intermediate move with center/line/vector.')
],['A composition uses the output of each previous movement.','Order can change the final figure.','Every step can be checked through corresponding points.']),
l(7,'No Bending or Stretching',[
 'Learning objective: test which geometric measurements remain unchanged after a rigid transformation, using matching side lengths, angles, and point-to-point distances.',
 'Rigid transformations preserve lengths and angles even though coordinates and orientation can change. Measuring an inaccurate hand sketch may suggest a tiny difference; exact coordinate calculations or tracing paper provide stronger evidence.',
 'Draw a triangle with vertices (1,1),(4,1),(1,5). Its legs have lengths 3 and 4 and its third side has length 5. Reflect it across the y-axis. Use a ruler and the Pythagorean relation to show that the image retains the same three lengths.',
 'Partner task: one learner claims a stretched copy is a rotation. Measure two corresponding edges to decide whether this is possible. Record a counterexample with explicit measurements. Teacher review is required for the argument; a single numeric distance item is not the full lesson.'
],[
 concept('Distance preservation','Corresponding segments have equal lengths after rigid transformations.','For two points, compare squared distances Δx²+Δy² before and after.','Points (1,1),(4,1) stay distance 3 apart after reflection.'),
 concept('Angle preservation','Rigid movements keep the angle between corresponding rays.','Every original angle equals its image angle.','A 90° corner remains 90° after turning or mirroring.'),
 concept('Not a rigid move','Stretching or shearing changes at least some distances or angles.','Find one measured counterexample to reject rigid congruence.','A 3-unit segment stretched to 6 units is not a translation.')
],[
 ex('Compute all side lengths','Find side lengths of triangle (1,1),(4,1),(1,5).','Use coordinate differences and the Pythagorean theorem.',[
 'The horizontal leg is 4−1=3 units.','The vertical leg is 5−1=4 units.','The slanted side length is √(3²+4²)=√25=5.'
 ],'Side lengths 3, 4 and 5.','Check 3²+4²=5².'),
 ex('Reflect without changing size','Reflect the same triangle across the y-axis and recalculate its leg lengths.','Negate x-coordinate for every vertex.',[
 'The images are (−1,1),(−4,1),(−1,5).','The horizontal leg has length |−4−(−1)|=3.','The vertical leg still spans 5−1=4, and diagonal remains 5.'
 ],'The image has side lengths 3, 4 and 5.','Reflection reverses orientation, not segment lengths.'),
 ex('Reject a stretch','An original segment is 3 units long; its proposed image is 6 units long. Could one rigid movement map them?','Compare an invariant before and after.',[
 'Rigid movements preserve length.','The original length 3 differs from image length 6.','One mismatched length is sufficient to reject any rigid transformation.'
 ],'No. The figure was resized or otherwise distorted.','A rigid map cannot change length 3 to length 6.')
],[
 error('Judging equality by how the sketch looks.','Hand-drawn figures may not be to scale.','Use corresponding coordinates or reliable geometric measurements.'),
 error('Assuming a reflection changes side lengths because orientation reverses.','Orientation and metric length are different properties.','Calculate the squared distance before and after the reflection.')
],['Translations, rotations and reflections preserve every length and angle.','A single changed distance disproves rigidity.','Coordinate proofs can support a visual observation.']),
l(8,'Rotation Patterns',[
 'Learning objective: investigate how rotations create repeating patterns, identify the center and angle of rotation, and explain rotational symmetry using known fractions of 360 degrees.',
 'Arrange several identical asymmetric cutout shapes around a common marked point. Turn each copy by the same angle. Verify that the corners follow arcs centered on the same point and remain at their original distances from the center.',
 'A pattern with six equally spaced repeats uses a rotation of 360°/6=60°. With four repeats it uses 90°; with three, 120°. The center and the exact number of turns matter; a rotation of 360° always returns the shape to its start.',
 'Make a first-party geometric rosette using a simple L-shaped figure traced around a chosen point. Label each rotated image and state the generating angle. A teacher assesses drawing accuracy, repeated figures and explanation; do not claim the rosette itself is machine-graded.'
],[
 concept('Angle of repeat','The smallest positive turn that maps a pattern to itself.','For n equally spaced positions, a natural step is 360°/n.','Six placements around a center have 60° between positions.'),
 concept('Center of rotation','The fixed point about which every copy is turned.','Each vertex follows a circular path with fixed radius from the center.','An endpoint two squares from O remains two squares from O after a turn.'),
 concept('Rotational symmetry','A figure coincides with itself after a positive turn smaller than 360°.','Check the whole labeled figure, not one matching corner.','A non-square rectangle has a 180° symmetry; a generic scalene triangle does not.')
],[
 ex('Six-step pattern','What turn separates six equally spaced images around a center?','Divide a full revolution into six equal parts.',[
 'A full turn is 360°.','Compute 360°÷6=60°.','After six 60° turns the total is 360°, returning to the starting orientation.'
 ],'60°.','6×60°=360°.'),
 ex('A quarter-turn pattern','What is the step angle for four equally spaced copies?','Divide 360° by four.',[
 'There are four identical turn intervals.','360°÷4=90°.','Four consecutive 90° turns complete exactly one revolution.'
 ],'90°.','4×90°=360°.'),
 ex('Check symmetry of a rectangle','Will a non-square 2-by-5 rectangle agree with itself after a 90° turn? What about 180°?','Compare the positions of its long and short sides.',[
 'After 90°, the original horizontal 5-unit dimension becomes vertical, so it does not match its starting location.','After 180°, both axes reverse and the 2-by-5 rectangle lies on its original outline.','A 180° rotation is therefore a symmetry, but a 90° rotation is not.'
 ],'90°: no. 180°: yes.','A square would have the additional 90° rotational symmetry.')
],[
 error('Counting any number of repeated shapes as symmetry of a single shape.','A whole pattern and a constituent asymmetric tile can have different symmetries.','Specify the figure being tested and require full overlap.'),
 error('Assuming the turn center may change between copies.','A single rotation pattern uses the same center.','Keep center fixed and check vertex distances.')
],['Equal angular spacing divides 360° into consistent steps.','Check the same center for every image.','Rotational symmetry requires complete overlap of the chosen figure.'])
];
if(rows.length!==8||new Set(rows.map(x=>x.LessonCode)).size!==8)throw Error('Bad lesson set');
for(const row of rows){
 if(row.ExplanationParagraphs.length<2||row.KeyConcepts.length<3||row.WorkedExamples.length<3||row.CommonMistakes.length<2||row.SummaryPoints.length<3)throw Error('Invalid rich contract '+row.LessonCode);
}
const doc={SchemaVersion:1,ContentVersion:'us-ccss-g8-u01-l01-l08-first-party-rich-v2-20261029-v1',PackCode:'US-CCSS-MATH',Lessons:rows};
fs.writeFileSync(out,JSON.stringify(doc,null,2)+'\n');
console.log('G8_AUTHORING '+JSON.stringify({path:out,lessons:rows.map(x=>x.LessonCode),examples:rows.reduce((n,x)=>n+x.WorkedExamples.length,0)}));
