'use strict';
const fs=require('node:fs'),path=require('node:path'),root=path.resolve(__dirname,'../..');
const lesson=(n,title,body,ideas,examples,errors,summary)=>({
 LessonCode:'PED:US-CCSS-MATH:G8:U09:L'+String(n).padStart(2,'0'),CultureCode:'en',Title:title,
 ExplanationParagraphs:[...body,'Independently authored Edulytiks curriculum material informed by Open Up Resources / Illustrative Mathematics Grade 8 (2017–2019), CC BY 4.0. Exact publisher preparation source: https://im.kendallhunt.com/MS/teachers/3/9/'+n+'/preparation.html . No original figures, data sets or assessments are reproduced.'],
 KeyConcepts:ideas.map(([Title,Definition,Rule,Example])=>({Title,Definition,Rule,Example})),
 WorkedExamples:examples.map(([Title,Question,Method,Steps,Answer,Check])=>({Title,Question,Method,Steps,Answer,Check})),
 CommonMistakes:errors.map(([Mistake,WhyWrong,Correction])=>({Mistake,WhyWrong,Correction})),
 SummaryPoints:summary,Visuals:[],Videos:[]
});
const rows=[
lesson(1,'Tessellations of the Plane',[
 'Goal: characterize a tessellation as a gapless, overlap-free cover of a region by repeated shapes, and justify how translations, rotations or reflections position tiles.',
 'A rectangular grid tessellates because copies meet at whole edges and four right angles meet at each interior vertex. Merely placing many identical shapes close together does not guarantee no gaps.',
 'Draw an original irregular quadrilateral on squared paper, make rotated and translated copies, and test edge-to-edge fit. A teacher evaluates whether the actual drawing covers the selected region without gaps or overlaps, not just arithmetic.'
],[
 ['Tessellation','Tiling of the plane without gaps or overlaps.','Tiles must cover each region point consistently.','Square lattice tessellates.'],
 ['Vertex angle sum','Angles of tiles around an interior point total 360°.','Measure all touching corners.','Four 90° corners fill a turn.'],
 ['Rigid-copy tile','Tile retains original side lengths and angles when moved.','Use translation, rotation or reflection.','A reflected quadrilateral remains congruent.']
],[
 ['Square corners','How many 90° square corners fill a vertex?','Divide a full turn by each corner.', ['Full angular measure is 360°.','Each square angle is 90°.','360/90=4.'],'Four.','Four copies leave no angular gap.'],
 ['Triangle corners','How many 60° equilateral triangle corners fill a point?','Use full-turn ratio.', ['Angle at equilateral vertex is 60°.','360/60=6.','Six sectors meet without angular gap.'],'Six.','6×60=360.'],
 ['Reject gap','Three square corners meet at a vertex. What angle is unfilled?','Subtract from full turn.', ['Three corners total 270°.','Full turn is 360°.','Gap angle 360−270=90°.'],'90° gap.','Three squares alone cannot fill the point.']
],[
 ['Assuming equal tile areas alone imply tessellation.','Shape and edge fit matter.','Check boundaries and vertices.'],
 ['Counting tile overlap as extra coverage.','Overlap violates the tessellation condition.','Inspect all edges and interiors.']
],['Tessellations have no gaps or overlaps.','Rigid moves preserve tile shape.','Analyze vertex angles and edge matching.']),
lesson(2,'Regular Tessellations',[
 'Goal: investigate which congruent regular polygons alone make an edge-to-edge regular tessellation, using interior-angle reasoning.',
 'A regular n-gon has interior angle ((n−2)×180°)/n. Equilateral triangles (60°), squares (90°) and regular hexagons (120°) fit integer numbers around a point; regular pentagons (108°) do not.',
 'Construct original samples from paper or a grid. Test hexagonal honeycomb and explain why three 120° corners close. Teacher review assesses regularity, edge matching and explanation.'
],[
 ['Regular polygon','All sides and all interior angles equal.','Interior angle=(n−2)×180°/n.','Regular hexagon has 120° interior.'],
 ['Vertex divisibility','Number of corners around point must be a whole number.','360°/interior angle must be integer for regular single-polygon tiling.','360/120=3 hexagons.'],
 ['Excluded pentagon','Regular pentagon interior is 108°.','360/108 is not integer.','Cannot make a regular pentagon-only edge-to-edge tessellation.']
],[
 ['Hexagon tiling','How many 120° regular-hexagon corners meet at a point?','Divide full turn.', ['360° total.','Each corner 120°.','360/120=3.'],'Three.','Three angles sum to 360°.'],
 ['Pentagon obstacle','Can 108° pentagons fill a point with only identical corners?','Check integer ratio.', ['3×108=324°, leaving gap.','4×108=432°, overlapping.','No integer count makes 360°.'],'No.','Regular pentagons alone cannot form a regular tessellation.'],
 ['Triangle regular tiling','How many equilateral triangles around a vertex?','Use 60° angle.', ['Equilateral angles are 60°.','360/60=6.','Six triangles fill the turn.'],'Six.','They can form a regular triangular tiling.']
],[
 ['Claiming every regular polygon tessellates.','Angle divisibility restricts possibilities.','Test full-turn condition.'],
 ['Using exterior angle instead of interior.','Tiles meet by their interior corners.','Calculate (n−2)180/n.']
],['Regular triangle, square and hexagon tilings are possible.','Polygon interior angle matters.','No regular pentagon-only tessellation.']),
lesson(3,'Tessellating Polygons',[
 'Goal: design a tessellation with polygons that may be different or irregular, showing explicitly how tile edges meet and whether a repeat pattern covers the plane.',
 'Every triangle tessellates the plane by pairing rotated copies into parallelograms, and every quadrilateral can tile by 180° rotations around side midpoints in an appropriate arrangement. Not every arbitrary mixture will tile.',
 'Make an original triangle tile with unequal sides and demonstrate a paired copy. Ask a partner to explain how translated parallelograms extend the pattern. Teacher review assesses the actual edge-matching diagram and reasoning.'
],[
 ['Triangle tiling','Two copies of a triangle can form a parallelogram.','Rotate one triangle 180° about a side midpoint.','Pair congruent scalene triangles.'],
 ['Parallelogram tiling','Opposite edges match under translation.','Translate repeated copies along two edge vectors.','A nonrectangular parallelogram covers plane in rows.'],
 ['Mixed polygon tiling','Multiple polygon shapes can meet if vertex angles and edges fit.','Check all boundaries.','Triangles and hexagons can form patterns in some configurations.']
],[
 ['Triangle corner sum','Two triangles each contribute angle sum 180°. What total angles?','Add triangle sums.', ['One triangle interior sum is 180°.','Two sums make 360°.','A suitable arrangement can close a full angular turn.'],'360°.','Actual edge placement must still be checked.'],
 ['Parallelogram area','A tiling tile has base 5 and perpendicular height 3. What area?','Use base×height.', ['Base b=5.','Height h=3.','Area=15 square units.'],'15 square units.','A translated tile preserves area.'],
 ['Area of tiled patch','Twelve nonoverlapping 15-unit² tiles fill a patch. What area?','Add equal tile areas.', ['Each tile contributes 15 units².','Twelve do not overlap.','Total 12×15=180 units².'],'180 square units.','No gaps assumed for exact patch coverage.']
],[
 ['Assuming angle sum alone proves edge fit.','Edges may mismatch even when corner totals align.','Check a full repeat cell.'],
 ['Using a distorted copy instead of a rigid tile.','A tessellation by congruent copies preserves dimensions.','Use only permitted rigid movements.']
],['Triangles and parallelograms can tessellate.','Tile arrangements must match at edges and vertices.','Verify periodic coverage with a repeat cell.']),
lesson(4,'What Influences Temperature?',[
 'Goal: identify candidate predictors for temperature data, distinguish measurements from explanatory claims and examine how geographical and seasonal variables could confound a model.',
 'Latitude, elevation, distance from ocean, season and time of day may influence observed temperature. A bivariate graph of latitude and temperature may show an association, but a single predictor cannot account for all geographic variation.',
 'Use independent fictional city observations (latitude 20°, temperature 30°C),(40°,20°C),(60°,10°C). Predict a trend, then discuss limitations such as elevation. A teacher assesses model assumptions and causal reasoning.'
],[
 ['Predictor','Input used to estimate an output variable.','Specify measured x and y.','Latitude x, temperature y.'],
 ['Confounding variable','Another factor influencing the observed relation.','Consider season and elevation.','Mountain cities may be cooler at same latitude.'],
 ['Association versus causation','A trend does not isolate its physical cause.','Avoid unsupported causal conclusions.','Higher latitude often accompanies lower temperature in some seasonal samples.']
],[
 ['Temperature trend','Fictional pairs (20,30),(40,20),(60,10) have what slope?','Compute output change over input.', ['From 20° to 40°, temperature change 20−30=−10°C.','Latitude change is 20°.','Slope −10/20=−0.5°C per degree latitude.'],'−0.5°C per latitude degree.','The fictional samples lie on a decreasing line.'],
 ['Predict mid-latitude','Use model T=40−0.5L for L=50°.','Substitute latitude.', ['0.5×50=25.','40−25=15.','Prediction 15°C.'],'15°C.','Model is illustrative, not a weather forecast.'],
 ['Find contextual intercept','For T=40−0.5L, predicted T at zero latitude?','Evaluate L=0.', ['0.5×0=0.','T=40−0.','Intercept 40°C.'],'40°C under the fictional model.','Not a measured universal equator temperature.']
],[
 ['Treating a simplified sample as real global climate data.','Many explanatory factors are omitted.','Label fictional model and caveats.'],
 ['Using different months to compare locations without noting season.','Season can confound latitude association.','Align or account for observation times.']
],['Models depend on predictor and context.','Temperature has multiple influences.','Use association responsibly.']),
lesson(5,'Plotting the Weather',[
 'Goal: construct a scatter plot from paired climate-like measurements and choose a reasonable linear model with a stated numerical slope, intercept and uncertainty.',
 'Given original fictional observations of latitude L and temperature T: (20,31),(30,24),(40,21),(50,14), one possible trend line is T≈40−0.5L. Some observations fall above or below the line; its predictions are estimates.',
 'Draw the points and fitted line with labeled units. Compare residuals at each observation and explain why the line is not a universal physical law. A teacher reviews graph quality and justification.'
],[
 ['Weather scatter','Paired measurements with geographic x and temperature y.','Each point is one paired observation.','(30,24) is latitude 30°, temperature 24°C.'],
 ['Fitted model','A line summarizing data rather than exactly passing through every dot.','T≈mL+b.','T≈40−0.5L.'],
 ['Residual','Observed output minus modeled output.','r=Tobserved−Tpredicted.','At L=20, prediction 30 and observation 31 gives +1.']
],[
 ['Model prediction','T≈40−0.5L at latitude 30. Find prediction.','Substitute L=30.', ['Multiply 0.5×30=15.','40−15=25.','Prediction 25°C.'],'Approximately 25°C.','Measured fictional example 24°C has residual −1.'],
 ['Residual above line','At L=20, measured T=31 and model predicts 30. Residual?','Subtract predicted.', ['Predicted T=40−10=30.','Observed T=31.','Residual 31−30=+1°C.'],'+1°C.','Observation is one degree above model.'],
 ['Residual below line','At L=50, measured 14 and predicted 15. Residual?','Compute observed minus predicted.', ['Prediction 40−25=15.','Observation 14.','14−15=−1°C.'],'−1°C.','The dot lies below the model line.']
],[
 ['Connecting points as chronological weather series.','City latitudes are independent subjects rather than one timeline.','Use scatter plot and fitted line.'],
 ['Pretending model predictions are exact observations.','Residuals show differences.','Label estimated values and uncertainty.']
],['Scatter plots preserve paired weather observations.','A fitted line is approximate.','Residuals reveal imperfect fit.']),
lesson(6,'Using and Interpreting a Mathematical Model',[
 'Goal: use a linear model of paired data for contextual predictions, interpret slope and intercept, and explicitly state plausible limits and uncertainty.',
 'An illustrative model T(L)=40−0.5L for temperature T in degrees Celsius versus latitude L in degrees has negative slope, but its intercept describes only what the model predicts for L=0. It is not an asserted universal equatorial measurement, nor necessarily suitable for southern latitudes, other seasons, or extreme altitudes.',
 'Student task: use a fictional fitted model to predict temperatures at two unmeasured latitudes and defend whether the model can support the inference. A teacher checks argumentation, context, extrapolation, units and the model’s limitations, not just numeric predictions.'
],[
 ['Model prediction','Evaluate fitted rule at new x.','Use a stated input domain.','At L=45, T≈17.5°C.'],
 ['Slope interpretation','Estimated change in output for one unit increase of input.','m=−0.5°C/latitude degree.','Increasing L by 10 predicts a 5°C decrease.'],
 ['Intercept limitation','Predicted output when input zero, not necessarily an observed value.','Explain context and evidence range.','T(0)=40°C under hypothetical rule.']
],[
 ['Predict one city','Model T=40−0.5L predicts what at L=45°?','Substitute.', ['0.5×45=22.5.','Subtract from 40.','Predicted T=17.5°C.'],'Approximately 17.5°C.','Not an observed or live weather forecast.'],
 ['Interpret slope','What does slope −0.5 mean for an increase of 10 latitude degrees?','Multiply change by slope.', ['Rate is −0.5°C per latitude degree.','Input change +10°.','Estimated temperature change is −0.5×10=−5°C.'],'Predicted decrease of 5°C.','A fitted trend is not a causal statement.'],
 ['Limit extrapolation','Model fitted only for L=20° to 50°. Is predicting L=90° an interpolation?','Compare input with evidence range.', ['Observed latitudes in fictional data are 20°–50°.','90° lies outside this interval.','Using model there is extrapolation with potentially large error.'],'No; it is extrapolation and should be treated cautiously.','Other weather and geographic factors matter.']
],[
 ['Calling the intercept a physically verified temperature at latitude zero.','A fitted model’s intercept may lie outside measured data.','State it is a model prediction.'],
 ['Claiming causal effect of latitude from a scatter plot alone.','Confounding variables may explain part of association.','Distinguish prediction and causation.']
],['Models are approximate and contextual.','Interpret slope with units and relevant domain.','State limits of extrapolation and alternative factors.'])
];
if(rows.length!==6||new Set(rows.map(x=>x.LessonCode)).size!==6)throw Error('Unit9 count mismatch');
for(const r of rows)if(r.ExplanationParagraphs.length<2||r.KeyConcepts.length<3||r.WorkedExamples.length<3||r.CommonMistakes.length<2||r.SummaryPoints.length<3)throw Error('Bad Rich V2 '+r.LessonCode);
const out=path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u09-l01-l06-2017.rich-lesson-v2.json');
fs.writeFileSync(out,JSON.stringify({SchemaVersion:1,ContentVersion:'us-ccss-g8-u09-first-party-rich-v2-20261029-v1',PackCode:'US-CCSS-MATH',Lessons:rows},null,2)+'\n');
console.log('G8_U09 '+JSON.stringify({lessons:rows.length,examples:rows.reduce((a,r)=>a+r.WorkedExamples.length,0),out}));
