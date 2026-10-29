'use strict';
// Independently authored illustrative Grade 8 Unit 2; source-based candidate, NOT academic release.
const fs=require('node:fs'),path=require('node:path');
const base=path.resolve(__dirname,'../..');
const concept=(Title,Definition,Rule,Example)=>({Title,Definition,Rule,Example});
const ex=(Title,Question,Method,Steps,Answer,Check)=>({Title,Question,Method,Steps,Answer,Check});
const mistake=(Mistake,WhyWrong,Correction)=>({Mistake,WhyWrong,Correction});
const lesson=(n,Title,body,ideas,examples,errors,summary)=>({
 LessonCode:'PED:US-CCSS-MATH:G8:U02:L'+String(n).padStart(2,'0'),CultureCode:'en',Title,
 ExplanationParagraphs:[...body,
  'Independent Edulytiks teaching informed by Open Up Resources / Illustrative Mathematics Grade 8 Unit 2, 2017–2019 CC BY 4.0. Public source: https://im.kendallhunt.com/MS/teachers/3/2/'+n+'/preparation.html . No publisher graphics or assessments are reproduced.'],
 KeyConcepts:ideas,WorkedExamples:examples,CommonMistakes:errors,SummaryPoints:summary,Visuals:[],Videos:[]
});
const rows=[
lesson(1,'Projecting and Scaling',[
 'Goal: investigate how an enlarged or reduced image relates to its original through a center of projection and a scale factor. Compare the resulting distances rather than assuming all size changes are rigid.',
 'Place a point light and a paper triangle above a wall. A shadow of the triangle can become larger when it moves closer to the light, but not every physical shadow is a perfect geometric dilation; establish which idealized rays and planar placements are being modeled.',
 'Draw center O, triangle A,B,C and image A′,B′,C′ along the same rays from O. Estimate a common ratio OA′/OA for all three corners. A teacher checks the source-specific projection diagram and proportional reasoning; numeric scale ratios alone do not verify drawing quality.'
],[
 concept('Projection center','A common origin from which lines to corresponding image points extend.','Use the same center for every pair of corresponding points.','O, A and A′ lie on one ray.'),
 concept('Scale factor','The ratio of image distance to original distance from the center.','k=OA′/OA when A′ lies on the ray OA.','A point 2 units from O maps to 6 units: k=3.'),
 concept('Enlargement or reduction','An image that scales distances by the same positive factor.','k>1 enlarges; 0<k<1 reduces.','An image with k=1/2 has each distance halved.')
],[
 ex('Find the enlargement factor','A point A is 2 units from O and A′ is 6 units along the same ray. What is k?','Divide image radius by original radius.',[
 'Take the original center distance OA=2.','The new center distance is OA′=6.','Compute k=6÷2=3.'
 ],'3, an enlargement.','Multiplying 2 by 3 gives 6.'),
 ex('Calculate a reduced radius','A point is 12 cm from O. It is reduced by k=1/4. How far is the image point from O?','Multiply the original radius by k.',[
 'The original distance is 12 cm.','Multiply by 1/4: 12×1/4=3.','Place the image on the original ray at distance 3 cm.'
 ],'3 cm.','3/12=1/4.'),
 ex('Check a proposed uniform scale','A and B are 2 cm and 4 cm from O. Their images are 6 cm and 10 cm from O. Is one dilation enough?','Compare scale ratios.',[
 'A-to-image ratio is 6÷2=3.','B-to-image ratio is 10÷4=2.5.','Different ratios cannot result from one uniform dilation.'
 ],'No; the scale ratios disagree.','A single factor would have to be both 3 and 2.5.')
],[
 mistake('Calling a large shadow a rigid transformation.','Rigid transformations preserve size, unlike an ideal dilation with k≠1.','Check a corresponding distance.'),
 mistake('Using different projection centers for different vertices.','A dilation uses the same center for every point.','Draw center-to-vertex rays first.')
],['A dilation scales every center-to-point distance consistently.','k above one enlarges and between zero and one reduces.','A projection model requires explicitly stated geometric assumptions.']),
lesson(2,'Circular Grid',[
 'Goal: use a grid of concentric circles and equally spaced rays to construct dilations. The circles encode distance from a common center and the rays encode direction.',
 'For a positive scale factor k, the point and its image stay on the same ray from center O. Their radii differ by factor k. Every vertex must follow the same radial rule; otherwise the image distorts.',
 'On a circular grid draw an irregular three-vertex figure whose corners have radii 2,3,4 grid steps. Dilate by 2, label the image corners and justify corresponding radii. A teacher scores the diagram; an automated numeric radius check is a limited component.'
],[
 concept('Concentric circular grid','Circles with the same center and successive marked radii.','Each ring represents a distance from O.','Point A can lie on a radius-3 circle.'),
 concept('Ray correspondence','Point and positive-scale image lie along one ray from the center.','Preserve ray direction while changing radial distance.','On the north ray, radius 2 becomes radius 6 under factor 3.'),
 concept('Uniform radial scale','A single scale multiplier applied to all radii.','r′=kr for each vertex.','Radii 2,3,4 become 4,6,8 with k=2.')
],[
 ex('Double three radii','Original radii are 2,3,4. What are image radii for k=2?','Multiply each radius by the same factor.',[
 '2×2=4.','3×2=6.','4×2=8, retaining the original ray for each vertex.'
 ],'4, 6 and 8 units.','Each image/original ratio equals 2.'),
 ex('Reduce a circle-grid radius','A point is on radius 9. Dilation k=1/3: find its new radius.','Scale radial distance.',[
 'Read the source radius 9.','Multiply 9×1/3.','Place the point on radius 3 along the same ray.'
 ],'3 units.','3÷9=1/3.'),
 ex('Spot inconsistent scaling','Radii 2 and 6 map to 5 and 15. What common scale factor is used?','Compare ratios.',[
 'For the first point, 5÷2=2.5.','For the second, 15÷6=2.5.','The matching ratios support a single dilation centered at the grid center.'
 ],'k=2.5.','Both image radii are 2.5 times their sources.')
],[
 mistake('Moving to a different ray after scaling.','A positive dilation preserves point direction from the center.','Stay on the original center-to-point ray.'),
 mistake('Treating concentric rings as equally spaced scale factors rather than distances.','A grid ring indexes absolute radius, not multiplication.','Read the radius values and multiply them.')
],['Circular grids show radial dilation directly.','Keep the center and ray fixed.','Scale every radius by one factor.']),
lesson(3,'Dilations with no Grid',[
 'Goal: construct an image using a straightedge and measured center-to-point distances without relying on a square or circular grid.',
 'Mark an arbitrary center O and a triangle ABC. Draw rays OA, OB, OC; along each ray mark A′,B′,C′ with OA′=k·OA and so on. Join image vertices in the original order.',
 'Construct a non-grid dilation of a scalene triangle by factor 3/2 and check corresponding side ratios. A ruler/tracing activity is teacher-reviewed; do not report numeric radius multiplication as proof that the learner constructed the whole figure.'
],[
 concept('Center ray','Line from center O through one original vertex.','A positive dilation image lies on that ray.','O,A,A′ are collinear.'),
 concept('Measured scale','A distance scaled by multiplier k.','Use a ruler, compass or proportional construction.','OA=4 cm becomes OA′=6 cm for k=3/2.'),
 concept('Similarity under dilation','A dilation preserves corresponding angles but multiplies sides by k.','Every side image/original ratio equals k.','A 6-cm side becomes 9 cm at k=3/2.')
],[
 ex('Place a scaled vertex','OA=4 cm and k=1.5. Find OA′.','Multiply radius by scale factor.',[
 'The original radius is 4 cm.','Compute 4×1.5=6 cm.','Mark A′ at 6 cm from O on ray OA.'
 ],'6 cm.','6÷4=1.5.'),
 ex('Find scale from a side','Corresponding sides measure 6 cm and 9 cm. What is the dilation factor?','Divide image side by original side.',[
 'Identify original side 6 cm.','Identify corresponding image side 9 cm.','Compute 9÷6=3/2.'
 ],'3/2 or 1.5.','6×1.5=9.'),
 ex('Detect a poor construction','One original radius 5 cm becomes 10 cm and another 3 cm becomes 9 cm. Is this factor consistent?','Compute both ratios.',[
 'The first radius has factor 10÷5=2.','The second has factor 9÷3=3.','One dilation cannot use two different factors.'
 ],'No, the construction is inconsistent.','Two different factors invalidate a uniform dilation.')
],[
 mistake('Setting distances from an image vertex rather than the center.','Dilation radii are measured from the common center.','Measure OA and OA′ from O.'),
 mistake('Claiming every side remains equal.','A nonunit dilation changes side lengths.','Multiply each corresponding side by k.')
],['A grid is optional for dilation construction.','Draw center rays and use accurate scaled distances.','Check corresponding sides for a common ratio.']),
lesson(4,'Dilations on a Square Grid',[
 'Goal: use ordered pairs and the center of dilation to construct an exact image on a square coordinate grid.',
 'For center O=(0,0), the dilation by k maps (x,y) to (kx,ky). For another center C=(a,b), use C+k(P−C), not simple multiplication of global coordinates.',
 'Draw a triangle on grid paper and dilate it from O by factor 2, then repeat with a different center. Explain why the second image is placed differently. The drawing and explanation require human review.'
],[
 concept('Origin-centered dilation','Coordinate scaling about O.','(x,y)→(kx,ky).','(2,−3) at k=2 becomes (4,−6).'),
 concept('Nonorigin center','Scale the displacement from center C.','P′=C+k(P−C).','From C(1,1), P(3,2) at k=2 maps to (5,3).'),
 concept('Corresponding displacement','Each image displacement from C is proportional to the original.','Vector CP′=k·CP.','Vector (2,1) becomes (4,2) at k=2.')
],[
 ex('Double a signed coordinate','Dilate P(2,−3) around the origin by factor 2.','Multiply both coordinates.',[
 'New x is 2×2=4.','New y is −3×2=−6.','Plot (4,−6) on the same ray from O.'
 ],'P′(4,−6).','Distances from O double.'),
 ex('Halve a coordinate pair','Dilate Q(−8,6) around O by factor 1/2.','Multiply every coordinate by 1/2.',[
 'x′=−8/2=−4.','y′=6/2=3.','The new point is closer to O on the same ray.'
 ],'Q′(−4,3).','Both coordinates and radial distance halve.'),
 ex('Different center','Center C(1,1), P(3,2), scale 2. Find P′.','Scale P−C, then add the center back.',[
 'P−C=(2,1).','Twice this vector is (4,2).','Add C to obtain (5,3).'
 ],'P′(5,3).','C-to-image displacement (4,2) is twice (2,1).')
],[
 mistake('Multiplying global coordinates regardless of center.','That only works when the center is the origin.','Subtract the center before scaling, then add it back.'),
 mistake('Using an additive shift instead of multiplying displacements.','A dilation changes distances proportionally.','Compare center-to-point lengths, not a constant translation vector.')
],['Origin-centered dilation multiplies coordinates.','For other centers scale P−C.','Keep correspondence and ray directions clear.']),
lesson(5,'More Dilations',[
 'Goal: investigate dilation scale factors in the coordinate plane, including reductions, negative coordinates and the relationship between scale and transformed side lengths.',
 'Dilations with k>1 enlarge and 0<k<1 reduce. Angles stay equal while lengths multiply by k; area changes by k². An image of a shape centered at O is determined by scaling every vertex.',
 'Investigate a coordinate rectangle of side lengths 2 and 3 at different positive factors. Record old and new side lengths and compare the area ratio. Written justification about why area scales by the square needs teacher scoring.'
],[
 concept('Side length scaling','A dilation multiplies every length by k.','New side=old side×k.','Side 5 maps to side 15 at k=3.'),
 concept('Angle invariance','A positive dilation preserves angle measurements.','Corresponding angles remain equal.','A 90° rectangle corner remains 90°.'),
 concept('Area scaling','Length in two independent directions is multiplied by k.','Area image/original=k².','A 2×3 rectangle enlarged by k=2 grows from area 6 to 24.')
],[
 ex('Apply reduction','Original side length 14 and k=1/2. Find the image side.','Multiply by 1/2.',[
 'Read the original 14-unit side.','Compute 14×1/2=7.','The image side is half as long.'
 ],'7 units.','7/14=1/2.'),
 ex('New rectangle area','A 2-by-3 rectangle is dilated by factor 2. What is its image area?','Scale both side lengths.',[
 'The original area is 2×3=6.','The image dimensions are 4 and 6.','Compute area 4×6=24, which is 4 times 6.'
 ],'24 square units.','k²=2²=4.'),
 ex('Infer factor from areas','An original shape has area 8 and a similar image has area 72. Find positive scale factor k.','Take square root of area ratio.',[
 'The image/original area ratio is 72/8=9.','Area ratio equals k² for a dilation.','Positive k=√9=3.'
 ],'k=3.','Original lengths triple and areas increase ninefold.')
],[
 mistake('Multiplying area only by k.','Area has two independent dimensions that both scale.','Use k² for area.'),
 mistake('Claiming dilation preserves all segment lengths.','Only unit scale does that.','Check whether k=1.')
],['Dilation preserves angles but scales lengths.','Area scales by k squared.','Reductions and enlargements obey the same ratios.']),
lesson(6,'Similarity',[
 'Goal: identify similar figures through a sequence of rigid transformations and dilations, and distinguish similarity from congruence.',
 'Similar figures share corresponding angles and all proportional sides, even if they have different positions, orientations and sizes. Dilation handles uniform size change; a rigid movement may then align the image.',
 'Draw a 3-by-5 rectangle and a 6-by-10 rectangle. Determine the common scale factor. Compare with a 6-by-9 rectangle and explain why one uniform dilation cannot map the first to the second.',
 'A learner must justify a similarity claim rather than relying on appearances. Teacher-scored matching diagrams complement numeric side-ratio checks.'
],[
 concept('Similarity mapping','A dilation possibly followed by rigid movements.','Match angles and use one scale ratio for all sides.','3-by-5 maps to 6-by-10 at factor 2.'),
 concept('Constant side ratios','All corresponding lengths change by the same factor.','a′/a=b′/b=…=k.','6/3=10/5=2.'),
 concept('Congruence as special case','Similar figures with scale factor 1.','Congruent figures need no resizing.','Rotating a 3-by-5 rectangle preserves its dimensions.')
],[
 ex('Compare proportional rectangles','Are 3-by-5 and 6-by-10 rectangles similar?','Compare ratios for matching sides.',[
 'The first corresponding ratio is 6÷3=2.','The second is 10÷5=2.','Both rectangles have right angles and consistent factor 2.'
 ],'Yes, similar with k=2.','One dilation maps the side lengths.'),
 ex('Reject nonuniform scale','Can a 3-by-5 rectangle dilate to a 6-by-9 rectangle?','Compare both dimension ratios.',[
 'Width ratio is 6/3=2.','Height ratio is 9/5=1.8.','One dilation cannot have factors 2 and 1.8.'
 ],'No, the rectangles are not similar in that correspondence.','Rotating the second does not produce the required side ratio either.'),
 ex('Distinguish congruent','Two rectangles both measure 4 by 7 but one is rotated. Are they congruent, similar, or neither?','A rotation is rigid.',[
 'All corresponding sides and angles agree.','Rotation changes orientation but not shape or size.','They are congruent, hence also similar.'
 ],'Congruent and similar; scale factor 1.','A rigid rotation maps one to the other.')
],[
 mistake('Calling every rectangle pair similar.','Rectangles can have different length-to-width ratios.','Compare both corresponding side ratios.'),
 mistake('Confusing similarity with equal size.','Similarity permits uniform scaling.','State whether k is 1 or different.')
],['Similarity includes dilation and possibly rigid movements.','Angles agree and all side ratios are equal.','Congruence is similarity at scale 1.']),
lesson(7,'Similar Polygons',[
 'Goal: check polygon similarity using a corresponding vertex order, matching angle measures and a constant ratio of side lengths.',
 'Not every pair of polygons with equal angles is similar when there are four or more sides: side ratios also matter. Correspondence can reverse under reflection; labels must trace the intended pair of vertices.',
 'Investigate two quadrilaterals with one matching angle but inconsistent side ratios. Explain why a single angle is insufficient. Diagram correspondence and explanatory reasoning are teacher-scored.'
],[
 concept('Corresponding angles','Matched vertices in two polygons have equal angle measures under similarity.','Identify and label the vertex correspondence.','A 90° corner remains 90° after dilation.'),
 concept('Corresponding side ratios','Side lengths of one polygon are all multiplied by one positive factor.','Each image/original ratio must equal k.','Sides 2,3,4 become 4,6,8 at k=2.'),
 concept('Ordered correspondence','A↔A′, B↔B′ etc. define which sides and angles match.','Check ratios in that ordered sequence.','AB corresponds to A′B′.')
],[
 ex('Check a side pattern','Polygon sides 2,3,4,5 map to 6,9,12,15. What is k?','Calculate all four ratios.',[
 '6/2=3 and 9/3=3.','12/4=3 and 15/5=3.','Constant side factor is 3; angles still need to be checked for polygon similarity.'
 ],'k=3 for the sides; verify angles before certifying similarity.','Equal ratios alone are not sufficient for arbitrary polygons.'),
 ex('Reject one wrong side','Sides 2,3,4 map to 4,6,9. Is the given correspondence a dilation?','Compare corresponding ratios.',[
 'The first side ratio is 4/2=2.','The second is 6/3=2.','The third is 9/4=2.25, so the scale is not uniform.'
 ],'No.','One inconsistent side ratio is enough to reject this dilation correspondence.'),
 ex('Angle mismatch','One quadrilateral angle is 80° and the corresponding image angle is 85°. Can the polygons be similar under the specified correspondence?','Similarity preserves angle measures.',[
 'Corresponding angle in the original is 80°.','Corresponding image angle is 85°.','Angles are not equal, so no similarity mapping exists for that correspondence.'
 ],'No, not under that correspondence.','Dilations and rigid moves cannot change an angle by 5°.')
],[
 mistake('Checking only a few matching sides.','An unchecked side can violate uniform scaling.','Check the entire correspondence.'),
 mistake('Assuming equal side ratios alone determine any polygon.','General polygons can change angles while keeping sides in proportion.','Also check corresponding angles.')
],['Polygon similarity needs equal angles and one common side ratio.','Label correspondence before calculating.','A single mismatch disproves the proposed similarity.']),
lesson(8,'Similar Triangles',[
 'Goal: reason about similar triangles through angle correspondence and proportional sides, including the angle-angle (AA) similarity principle.',
 'Unlike general quadrilaterals, two triangles with two pairs of equal corresponding angles are similar because their third angles also agree. Once AA is established, corresponding side ratios are constant.',
 'Draw two triangles with angles 40°,60°,80°. Give them different side lengths but the same proportional ratios. Check that a rotation or reflection may be needed before a dilation aligns them.',
 'An annotated triangle drawing or AA justification is teacher-reviewed, while numeric angle and ratio calculations can be checked separately.'
],[
 concept('AA similarity','Two pairs of matching triangle angles guarantee triangle similarity.','The third angle follows from a sum of 180°.','40°/60° triangles each have remaining 80°.'),
 concept('Corresponding side factor','A similar triangle scales every side by one k.','Image side=original side×k.','3-4-5 becomes 6-8-10 at k=2.'),
 concept('Angle sum','Triangle angles total 180°.','Third angle=180°−first−second.','40°+60° leaves 80°.')
],[
 ex('Use two angles','Triangles both have corresponding angles 40° and 60°. Are they similar?','Use AA and angle sum.',[
 'The third angle in each is 180−40−60=80°.','Both triangles have corresponding angles 40°,60°,80°.','AA guarantees similarity.'
 ],'Yes, similar by AA.','All three angle measures agree.'),
 ex('Find scale from corresponding sides','A triangle side 3 corresponds to side 9 in a similar triangle. Find k.','Divide image side by original.',[
 'Identify corresponding sides 3 and 9.','Compute 9/3=3.','Every other side must triple.'
 ],'k=3.','A 4-unit side would become 12.'),
 ex('Find missing angle','Two angles measure 45° and 70°. What is the third?','Use angle sum.',[
 '45°+70°=115°.','180°−115°=65°.','The third angle is 65°, determining the full angle set.'
 ],'65°.','45+70+65=180.')
],[
 mistake('Applying AA to arbitrary four-sided figures.','AA similarity is a triangle theorem.','Check the number of sides before using AA.'),
 mistake('Pairing the wrong sides in a ratio.','Corresponding sides must oppose equal angles.','Label matched vertices and opposite sides.')
],['AA establishes triangle similarity.','Third angles follow from 180°.','Side lengths scale by one constant factor.']),
lesson(9,'Side Length Quotients in Similar Triangles',[
 'Goal: use ratios of corresponding side lengths to find unknown measurements in similar triangles, and justify a proportion before solving.',
 'For similar triangles ABC and DEF with A↔D, B↔E, C↔F, side quotients DE/AB=EF/BC=DF/AC. Ratios must compare the same direction (image/source) throughout.',
 'Set up a missing-side problem, cross multiply, then substitute back. A teacher evaluates correct correspondence on figures; a calculator-only fraction answer cannot establish the matching of sides.'
],[
 concept('Image/source ratio','One quotient for all corresponding sides.','Image side divided by source side equals k.','6/4=9/6=1.5.'),
 concept('Proportion','An equation that sets equal ratios.','Solve by multiplication while preserving corresponding order.','x/8=9/6 gives x=12.'),
 concept('Inverse factor','Source/image ratio is the reciprocal of k.','Keep the same direction for every quotient.','If image/source=3/2, then source/image=2/3.')
],[
 ex('Find a missing image side','Similar triangles have original side 8, corresponding source reference 6 and image reference 9. Find the image of 8.','Set x/8=9/6.',[
 'The known factor is 9/6=3/2.','Multiply 8×3/2=12.','Check 12/8=1.5, matching 9/6.'
 ],'12 units.','Both image/source ratios equal 1.5.'),
 ex('Find the original side','Image side 15 corresponds to unknown original side and factor k=3. Find original.','Divide image by factor.',[
 'The relation is 15=3×source.','Divide both sides by 3.','Source length is 5.'
 ],'5 units.','5×3=15.'),
 ex('Spot ratio reversal','A proposed solution uses 8/12=9/6 for corresponding sides. Is this consistent?','Keep quotient direction consistent.',[
 'The first quotient source/image is 8/12=2/3.','The second quotient image/source is 9/6=3/2.','One is reciprocal of the other, so they cannot be equated as written.'
 ],'No; use 12/8=9/6 or 8/12=6/9.','Correct proportions use the same orientation.')
],[
 mistake('Mixing original/image with image/original in one proportion.','The quotients are reciprocals.','Label every numerator and denominator.'),
 mistake('Using arbitrary same-looking sides.','Only corresponding sides form the similarity proportion.','Match the opposing equal angles first.')
],['Use consistent corresponding side quotients.','Cross-multiplication follows a justified proportion.','Check the answer by substitution.']),
lesson(10,'Meet Slope',[
 'Goal: describe the slope of a nonvertical line as a consistent vertical change divided by horizontal change, including the sign and geometric rise/run meaning.',
 'On a square grid trace two points and draw a right triangle whose legs show rise and run. The ratio is the same for any two distinct points on the same nonvertical straight line.',
 'Compare uphill, downhill and horizontal lines. A vertical line has undefined slope because run is zero; it does not have slope zero. Building accurate right triangles is a teacher-reviewed graphic task.'
],[
 concept('Slope','Rate of vertical change per horizontal change.','m=(y₂−y₁)/(x₂−x₁) for x₂≠x₁.','From (1,2) to (4,8), m=6/3=2.'),
 concept('Slope sign','Positive slope rises rightward, negative slope falls rightward.','Use a consistent left-to-right run.','From (0,5) to (2,1), m=(1−5)/2=−2.'),
 concept('Undefined vertical slope','A vertical line has run zero.','Division by zero is undefined.','x=3 has no finite slope.')
],[
 ex('Positive slope','Find the slope through (1,2) and (4,8).','Use change in y over change in x.',[
 'Rise is 8−2=6.','Run is 4−1=3.','Slope m=6/3=2.'
 ],'2.','A move right one on the line increases y by 2.'),
 ex('Negative slope','Find the slope through (0,5) and (2,1).','Take consistent coordinate differences.',[
 'Rise from first to second is 1−5=−4.','Run is 2−0=2.','m=−4/2=−2.'
 ],'−2.','The line falls by 2 per unit right.'),
 ex('Vertical slope','Find the slope of the line x=3.','Inspect the horizontal change between any two points.',[
 'Two points (3,1) and (3,7) have run 3−3=0.','Their vertical change is 6.','Slope would be 6/0, which is undefined.'
 ],'Undefined, not zero.','The line is vertical rather than horizontal.')
],[
 mistake('Reversing only the rise but not the run.','Both differences must use the same point order.','Compute y₂−y₁ and x₂−x₁.'),
 mistake('Calling a vertical line slope zero.','A zero slope is horizontal; a vertical run is zero and division is impossible.','Distinguish y=c from x=c.')
],['Slope equals rise/run on a nonvertical line.','Signs encode direction.','Vertical lines have undefined slope.']),
lesson(11,'Writing Equations for Lines',[
 'Goal: connect the slope of a line with the coordinates of its points and express a relationship in a linear equation.',
 'A line with slope m and vertical intercept b satisfies y=mx+b. If a point and slope are known, substitute to solve for b. Verify a second point to ensure the chosen equation truly represents the line.',
 'Draw a line through points (1,3) and (3,7), calculate slope and write the equation. The teacher reviews the plotted graph and intercept interpretation; numerical substitutions can be machine-checked.'
],[
 concept('Slope-intercept equation','y=mx+b describes a nonvertical line.','m is rise/run and b is y at x=0.','y=2x+1 has slope 2 and intercept 1.'),
 concept('Find intercept from point','Substitute a known point into y=mx+b.','b=y−mx.','At (1,3) and m=2, b=3−2=1.'),
 concept('Validate a line','Every coordinate pair on the line satisfies its equation.','Substitute at least two points for confidence.','For y=2x+1, x=3 gives y=7.')
],[
 ex('Equation from two points','Write the line through (1,3) and (3,7).','Find m and b.',[
 'Slope m=(7−3)/(3−1)=4/2=2.','Substitute (1,3): 3=2(1)+b, so b=1.','The equation is y=2x+1.'
 ],'y=2x+1.','At x=3 it gives y=7.'),
 ex('Known slope and point','Line has slope −3 and passes through (2,1). Find the equation.','Solve for intercept.',[
 'Start y=−3x+b.','At (2,1): 1=−3×2+b=−6+b.','Therefore b=7 and y=−3x+7.'
 ],'y=−3x+7.','At x=2, −6+7=1.'),
 ex('Check a point','Does (4,9) lie on y=2x+1?','Substitute x=4.',[
 'Compute the right side 2×4+1=9.','The given y-coordinate is 9.','Both match, so the point satisfies the line equation.'
 ],'Yes.','9=2(4)+1.')
],[
 mistake('Using y-intercept as the x-coordinate of a given point.','b is specifically the value when x=0.','Solve b from a point and slope.'),
 mistake('Using two points with reversed slope differences.','Mixed-order differences give a wrong sign.','Preserve consistent point order.')
],['For nonvertical lines use y=mx+b.','Find m from two points or a graph.','Solve b by substituting one known point.']),
lesson(12,'Using Equations for Lines',[
 'Goal: write and interpret linear equations from a given slope, point or context, and use them to predict values on a line.',
 'Convert a constant change in y for each unit of x to a slope. Anchor the equation with an actual point, then calculate the intercept or use point-slope reasoning. State variable meanings in contexts.',
 'Model a water tank starting with 30 liters and draining 2 liters per minute. Form V=30−2t and decide the valid physical time domain. A teacher checks contextual assumptions and explanations; numeric predictions are only part of the task.'
],[
 concept('Slope as rate','A constant change in output per input unit.','Slope has units of output/input.','Tank volume changes by −2 L/min.'),
 concept('Initial value','The output at input zero.','In y=mx+b, b is y(0).','Tank initially contains 30 liters.'),
 concept('Context domain','Inputs allowed by the real situation.','Physical volume cannot be negative.','30−2t≥0 for t≥0 gives 0≤t≤15 minutes.')
],[
 ex('Write tank equation','Tank starts at 30 L and loses 2 L each minute. Express V after t minutes.','Use initial value and rate.',[
 'At t=0, volume is 30.','Each minute subtracts 2 liters, so slope is −2.','Combine to obtain V=30−2t.'
 ],'V=30−2t, for 0≤t≤15.','At t=15 the volume reaches 0.'),
 ex('Predict volume','Using V=30−2t, find V after 7 minutes.','Substitute t=7.',[
 'Multiply 2×7=14 liters lost.','Subtract 30−14=16.','Volume is still physically nonnegative.'
 ],'16 liters.','V=30−2(7)=16.'),
 ex('Solve for empty time','When does the tank reach 0?','Set V=0 and solve.',[
 '0=30−2t.','Add 2t to each side to get 2t=30.','Divide by 2 to obtain t=15.'
 ],'15 minutes.','At t=15, V=0; negative times or volumes are excluded.')
],[
 mistake('Using positive slope for a decreasing amount.','A loss per unit time is negative change.','Check whether the graph rises or falls.'),
 mistake('Using a mathematically valid but physically impossible negative volume.','Model domain depends on context.','Stop the time interval when volume reaches 0.')
],['Linear equations model constant rates.','State rate and initial value with units.','Interpret solutions within realistic domains.']),
lesson(13,'The Shadow Knows',[
 'Goal: estimate the height of an object using similar right triangles created by sunlight and measured shadows, with consistent length units.',
 'Under the assumption that the sunlight rays are parallel and the ground is locally level, vertical objects and their horizontal shadows form similar right triangles. Height-to-shadow ratios agree at the same sun angle.',
 'Investigate an original Edulytiks scenario: a 1.5-meter vertical pole casts a 2-meter shadow and a tree casts a 10-meter shadow at the same time. Draw and label both triangles, solve the tree height, then discuss how uneven ground or moving sunlight could invalidate the model.',
 'The diagram, measurement procedure, and limitations require teacher judgment. The numerical proportion can be independently checked, but not substituted for actual field measurements or a validated physical model.'
],[
 concept('Similar shadow triangles','Vertical heights and horizontal shadows form right triangles with the same sunlight angle.','Under parallel rays on level ground, height/shadow is constant.','1.5/2=tree height/10.'),
 concept('Proportional height','Unknown height found through equivalent ratios.','h=reference height×target shadow/reference shadow.','h=1.5×10/2=7.5 meters.'),
 concept('Model assumptions','Conditions under which the shadow ratio is valid.','Measure both shadows at one time on comparable level ground.','An incline changes the geometry and may invalidate the simple proportion.')
],[
 ex('Find a tree height','Pole 1.5 m, pole shadow 2 m, tree shadow 10 m. Find tree height.','Set equal height-to-shadow ratios.',[
 'Pole ratio is 1.5/2=0.75.','Set h/10=0.75 for the tree.','Multiply both sides by 10 to get h=7.5 m.'
 ],'7.5 meters.','7.5/10=1.5/2.'),
 ex('Find an unknown shadow','A 4-meter post has shadow 6 meters. A 10-meter post at the same angle has what shadow?','Use proportional heights and shadow lengths.',[
 'The height ratio is 10/4=2.5.','Scale the 6-meter shadow by 2.5.','Shadow is 15 meters.'
 ],'15 meters.','10/15=4/6=2/3.'),
 ex('Assess a flawed ratio','Pole 2 m with 3 m shadow, tree with 6 m shadow. A student predicts 9 m height. Check.','Use height/shadow, not the reversed ratio.',[
 'Reference height/shadow is 2/3.','Tree height is 6×2/3=4 meters.','The predicted 9 meters uses the wrong orientation of ratios.'
 ],'4 meters, not 9.','4/6=2/3.')
],[
 mistake('Assuming shadows always represent similar triangles.','The ground slope, object tilt and sun angle can violate assumptions.','Check geometry and same-time measurements.'),
 mistake('Mixing centimeters and meters.','Ratios require consistent units.','Convert lengths before solving.')
],['Same-time sunlight can produce similar right triangles.','Equal height/shadow ratios determine missing values.','Explain measurement assumptions and uncertainty.'])
];
if(rows.length!==13||new Set(rows.map(x=>x.LessonCode)).size!==13)throw Error('Unit 2 inventory mismatch');
const doc={SchemaVersion:1,ContentVersion:'us-ccss-g8-u02-first-party-rich-v2-20261029-v1',PackCode:'US-CCSS-MATH',Lessons:rows};
for(const row of rows)if(row.ExplanationParagraphs.length<2||row.KeyConcepts.length<3||row.WorkedExamples.length<3||row.CommonMistakes.length<2||row.SummaryPoints.length<3)throw Error('Invalid '+row.LessonCode);
const out=path.join(base,'src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u02-l01-l13-2017.rich-lesson-v2.json');
fs.writeFileSync(out,JSON.stringify(doc,null,2)+'\n');
console.log('G8_U02 '+JSON.stringify({lessons:rows.length,examples:rows.reduce((n,x)=>n+x.WorkedExamples.length,0),out}));
