'use strict';
const fs=require('node:fs'),path=require('node:path'),root=path.resolve(__dirname,'../..');
const lesson=(n,title,body,ideas,examples,errors,summary)=>({
 LessonCode:'PED:US-CCSS-MATH:G8:U06:L'+String(n).padStart(2,'0'),CultureCode:'en',Title:title,
 ExplanationParagraphs:[...body,'Independently authored Edulytiks instruction informed by Open Up Resources / Illustrative Mathematics Grade 8 (2017–2019), CC BY 4.0. Exact source: https://im.kendallhunt.com/MS/teachers/3/6/'+n+'/preparation.html . Publisher graphics, individual data sets and assessments are not copied.'],
 KeyConcepts:ideas.map(([Title,Definition,Rule,Example])=>({Title,Definition,Rule,Example})),
 WorkedExamples:examples.map(([Title,Question,Method,Steps,Answer,Check])=>({Title,Question,Method,Steps,Answer,Check})),
 CommonMistakes:errors.map(([Mistake,WhyWrong,Correction])=>({Mistake,WhyWrong,Correction})),
 SummaryPoints:summary,Visuals:[],Videos:[]
});
const rows=[
lesson(1,'Organizing Data',[
 'Goal: select useful displays for paired measurements and explain what an observed pattern does or does not establish. One-variable bar charts and bivariate scatter plots answer different questions.',
 'Use an original survey in which each of five plants has measured sunlight hours and growth height: (1,2),(2,4),(3,5),(4,7),(5,8). Each ordered pair must stay paired. Discuss whether the data appear positively associated and whether other influences, such as water, could matter.',
 'Student task: choose axes and plot the five measurement pairs, then compare a two-variable scatter plot to separate unpaired summaries. A teacher checks axis units, retained pairings, and written reasoning; no automated correctness claim for self-collected data.'
],[
 ['Bivariate observation','Two measurements recorded for one subject.','Preserve the matching of values.','Plant A: 1 sunlight hour, 2 cm growth.'],
 ['Scatter plot','A display of paired numeric observations.','Place each (x,y) at its measured coordinates.','Plot plant (3,5) as a single point.'],
 ['Association','A pattern in how two variables vary together.','Describe direction without claiming causation.','Higher recorded sunlight often coincides with greater growth.']
],[
 ['Count paired observations','A study lists five plant pairs. How many dots should a scatter plot contain?','Each paired row represents one observation.', ['Count five recorded plants.','Keep each hour-height pair intact.','Plot one point per row.'],'5 points.','Separating x and y summaries would lose pairing.'],
 ['Read one observation','What does (4,7) mean when x is sunlight hours and y is growth centimeters?','Read axis units.', ['First coordinate 4 is four hours.','Second coordinate 7 is seven centimeters.','Both belong to the same plant.'],'A plant with 4 hours sunlight and 7 cm growth.','The pair is not two different plants.'],
 ['Describe trend','For pairs (1,2),(2,4),(3,5),(4,7),(5,8), does y generally rise with x?','Compare ordered pairs.', ['As x progresses from 1 to 5, y rises from 2 to 8.','Each successive measured y is larger.','This pattern is a positive association, not proof that sunlight alone caused growth.'],'Positive association.','A confounding variable could still exist.']
],[
 ['Losing the paired structure of measurements.','Unpaired displays cannot recover which observations belong together.','Store each subject as a complete (x,y) row.'],
 ['Claiming association proves cause.','Confounding variables or selection can create association.','Describe the observed pattern without unsupported causal claims.']
],['Paired observations are the foundation of bivariate displays.','Graph both quantities on labeled axes.','Association is not necessarily causation.']),
lesson(2,'Plotting Data',[
 'Goal: gather or use bivariate data and make a mathematically readable scatter plot, selecting axes with appropriate units and scales.',
 'A class investigation might compare arm span and height, but personal measurements should not be published or shared outside the classroom without permission. An independent fictional sample is (140,139),(145,146),(150,149),(155,157) measured in centimeters.',
 'Draw an original scatter plot with both axes measured in centimeters, appropriate scales, and no connecting line through separate pupils. If a student gathers real observations, the teacher reviews ethical data collection and accurate plotted pairs; do not submit personal class data to external services.'
],[
 ['Axis meaning','An axis identifies variable and measurement unit.','Label with quantity and unit, e.g. height (cm).','Point (150,149) means height 150 cm, arm span 149 cm.'],
 ['Scale selection','Tick labels convert grid distance to measured value.','Include the full measured range and regular spacing.','x-ticks 140,145,150,155.'],
 ['Independent plotted points','Each point is one subject’s two measurements.','Do not connect subjects in an artificial time sequence.','Four subjects create four dots.']
],[
 ['Interpret measured pair','With x=height and y=arm span, interpret (150,149).','Read axis assignments.', ['First coordinate indicates 150 cm height.','Second indicates 149 cm arm span.','Both belong to one observation.'],'Height 150 cm and arm span 149 cm.','Swapping axes would change the reading.'],
 ['Count points','Four students each provide one height-span pair. How many plotted observations?','One dot per complete row.', ['Identify four paired rows.','For each, place the dot at its x and y.','Total is four plotted points.'],'4 points.','No extra dot is made for each individual measurement.'],
 ['Check swapped point','A recorded pair is height 145, span 146. A dot at (146,145) is drawn. Is it correct?','Compare coordinate order.', ['x-axis represents height and should show 145.','y-axis represents span and should show 146.','The plotted pair swaps the values.'],'No; it should be (145,146).','Read labels before plotting.']
],[
 ['Connecting unrelated subjects as a line graph.','No time/order continuity is implied.','Use individual scatter points.'],
 ['Exposing classmate measurements unnecessarily.','Some physical data are personal.','Use permission and anonymous or fictional data as appropriate.']
],['Plot each paired observation once.','Label units and use meaningful scales.','Respect privacy when measuring classmates.']),
lesson(3,'What a Point in a Scatter Plot Means',[
 'Goal: interpret individual points, comparisons and outliers in a scatter plot without confusing one observation with the trend of all points.',
 'Each dot encodes two measured variables for one subject. A dot far from others may reflect an unusual subject or an error; one exceptional point does not by itself invalidate the rest.',
 'Using fictional paired data (1,3),(2,5),(3,7),(4,9),(5,4), identify the dot that differs most from an upward linear pattern and explain what additional information should be checked. Teacher-scored reasoning addresses measurement uncertainty.'
],[
 ['Point meaning','The same observation supplies both coordinates.','Use axes and units.','(3,7) means x=3, y=7 for one case.'],
 ['Outlier candidate','A value noticeably away from a general cloud of points.','Inspect it, but do not delete automatically.','(5,4) departs from near y=2x+1.'],
 ['Pattern versus case','One observation and group association are distinct.','Use several points to identify trend.','Four rising points do not force a fifth to fit.']
],[
 ['Interpret point','For axes hours and dollars, what does (3,7) describe?','Read both dimensions.', ['First value gives three hours.','Second gives seven dollars.','Both measurements describe a single case.'],'Three hours associated with seven dollars.','Neither coordinate is the observation count.'],
 ['Spot unusual point','Given (1,3),(2,5),(3,7),(4,9),(5,4), which point is far below y=2x+1?','Compare model outputs.', ['At x=5 the pattern predicts 2×5+1=11.','The measured value at x=5 is 4.','Difference 7 is much larger than that for the first four points.'],'(5,4).','It is an outlier candidate, not automatically an error.'],
 ['Check data pair','Does (4,9) agree with the rule y=2x+1?','Substitute x.', ['For x=4, predicted y=2×4+1.','The predicted output is 9.','It matches the plotted coordinate.'],'Yes.','9=2×4+1.']
],[
 ['Using a point’s x-coordinate as its index number.','x represents the axis variable.','Read axes before interpreting the dot.'],
 ['Deleting outliers to make a trend look tidy.','Outliers may convey important valid observations.','Investigate them before any exclusion.']
],['A dot records one paired case.','Explain outliers cautiously.','Group trend differs from individual variability.']),
lesson(4,'Fitting a Line to Data',[
 'Goal: use a line as an approximate model for a scatter plot, rather than mechanically connecting all points or forcing every observation onto the line.',
 'An approximate line of fit balances the overall upward or downward cloud and helps predict values. A least-squares fit is one possible method; Grade 8 reasoning can instead choose a sensible line by eye, cite points near it, and discuss its limitations.',
 'For fictional pairs (1,4),(2,6),(3,8),(4,11),(5,12), draw y≈2x+2, compare positive and negative vertical deviations, and critique extrapolation outside measured x. A teacher reviews fit quality and justification.'
],[
 ['Line of fit','A line summarizing a roughly linear association.','It need not pass through every point.','y≈2x+2 for a rising cloud.'],
 ['Residual','Observed y minus predicted y at the same x.','A positive residual lies above a model line.','At (4,11), prediction 10 gives residual +1.'],
 ['Prediction','Use fitted relation at an x value.','An estimate, not a guaranteed outcome.','At x=6, y≈14 with the example line.']
],[
 ['Residual above line','At (4,11), line y=2x+2 predicts what, and residual?','Subtract predicted from observed.', ['Prediction is 2×4+2=10.','Observation is 11.','Residual 11−10=+1.'],'Predicted 10; residual +1.','Point is one unit above the line.'],
 ['Residual below line','At (5,12), model y=2x+2 predicts what residual?','Compute difference.', ['Predicted y=2×5+2=12.','Observed y=12.','Residual 12−12=0.'],'0; the point is on the line.','Not all other points must also lie exactly on it.'],
 ['Extend cautiously','Line y≈2x+2 predicts y at x=6.','Evaluate model and label estimate.', ['Compute 2×6=12.','Add 2.','Prediction is approximately 14.'],'Approximately 14.','x=6 lies just beyond data up to 5, so the prediction is an extrapolation.']
],[
 ['Connecting dots in collection order.','Separate observations are not successive vertices of one curve.','Draw a single trend line instead.'],
 ['Reporting modeled output as an exact measurement.','A fit includes residuals.','Use approximate notation and uncertainty.']
],['A line of fit summarizes a bivariate cloud.','Residuals quantify departures from the line.','Predictions require caution.']),
lesson(5,'Describing Trends in Scatter Plots',[
 'Goal: distinguish positive, negative, weak and absent linear associations and describe the strength using scatter rather than just slope.',
 'Association direction tells whether larger x generally coincides with larger or smaller y. Association strength concerns how tightly observations cluster around a pattern. A strong negative association is possible even though its slope is negative.',
 'Sort original fictional patterns: (1,8),(2,6),(3,4),(4,2) has strong downward trend; (1,3),(2,7),(3,2),(4,8) has no obvious simple trend. Teacher review checks explanations and uncertainty.'
],[
 ['Positive association','Larger x tends to coincide with larger y.','Describe a general upward cloud.','(1,2),(2,4),(3,6).'],
 ['Negative association','Larger x tends to coincide with smaller y.','Describe a downward cloud.','(1,8),(2,6),(3,4),(4,2).'],
 ['Strength','Degree of clustering around a pattern.','Tighter cloud means stronger apparent association.','All points on y=10−2x form perfect negative linear association.']
],[
 ['Negative slope sample','For points (1,8),(2,6),(3,4),(4,2), what is slope?','Compute first-last change.', ['Rise 2−8=−6.','Run 4−1=3.','Slope −6/3=−2.'],'−2; strong negative linear trend.','Every adjacent output falls by two.'],
 ['Positive example','Pairs (1,2),(2,4),(3,6) have what trend?','Compare changes.', ['As x increases by 1, y rises by 2.','All three points align on y=2x.','Association is perfectly positive linear.'],'Positive linear.','A fitted slope of 2 is positive.'],
 ['Trend caution','Do pairs (1,3),(2,7),(3,2),(4,8) show a clear monotonic sequence?','Inspect sequence.', ['y first rises, then falls, then rises.','There is no consistent monotone pattern across adjacent points.','Do not assert a perfect increasing trend.'],'No clear perfect monotonic trend.','More observations and a suitable model may be needed.']
],[
 ['Equating negative slope with weak association.','Direction and strength are separate properties.','Describe both.'],
 ['Inferring a universal rule from a small cloud.','Association estimates can be unstable for small samples.','State the observed sample size and limitations.']
],['Describe direction and strength separately.','Scatter around a trend matters.','Association is not a causal claim.']),
lesson(6,'The Slope of a Fitted Line',[
 'Goal: interpret the numerical slope of a fitted line as an estimated change in one variable for a unit change in another, including context and units.',
 'For a model relating study hours x to quiz score y, slope 4 points/hour means an estimated four-point increase per additional hour within the range studied; it does not promise the same gain for every student.',
 'Use a fictional line through (1,54) and (4,66). Compute slope and intercept and discuss uncertainty from scatter. Teacher review assesses contextual language and limitations.'
],[
 ['Model slope','Change in fitted output per input unit.','m=(y₂−y₁)/(x₂−x₁).','(1,54),(4,66) gives m=4 score points/hour.'],
 ['Model intercept','Predicted output at x=0.','b=y−mx.','At (1,54), b=50.'],
 ['Estimated rate','A model trend, not an individual guarantee.','Use units and approximate interpretation.','Four expected points per study hour in this model.']
],[
 ['Compute score slope','Find slope through (1,54) and (4,66).','Rise over run.', ['Score difference 66−54=12 points.','Hour difference 4−1=3.','Slope 12/3=4 points/hour.'],'4 score points/hour.','Predicts an increase of four per hour.'],
 ['Find intercept','Slope 4 and model point (1,54). Find b.','Use y=4x+b.', ['54=4×1+b.','Subtract 4.','b=50.'],'50 score points.','The model is y=4x+50.'],
 ['Predict with caution','Predict modeled score for 3 hours on y=4x+50.','Evaluate.', ['Multiply 4×3=12.','Add 50.','The fitted model predicts 62.'],'Approximately 62 points.','Individual outcomes may differ because data scatter.']
],[
 ['Treating slope as guaranteed individual improvement.','The line is a fit to data, not a deterministic law.','Say estimated trend.'],
 ['Omitting outcome/input units.','Slope is a ratio, not just a number.','State score points/hour.']
],['Slope of fit is a contextual estimated rate.','Intercept gives modeled baseline.','Use cautious predictions.']),
lesson(7,'Observing More Patterns in Scatter Plots',[
 'Goal: recognize nonlinear patterns and clusters in bivariate data and avoid imposing a straight-line model when the cloud curves.',
 'A scatter plot may form a U-shape, arc, or two separate clusters rather than a single linear trend. An overall slope near zero can hide a real nonlinear association.',
 'Graph original points (−2,4),(−1,1),(0,0),(1,1),(2,4). Explain why they form a curved pattern and why a straight line would miss their structure. Teacher feedback checks the sketch and reasoning.'
],[
 ['Nonlinear association','The variables vary systematically without constant linear slope.','Inspect shape of cloud.','y=x² produces a U-shaped pattern.'],
 ['Cluster','A subgroup of points concentrated in one region.','Describe grouping before combining averages.','Two sets of similar heights may form separate groups.'],
 ['Model mismatch','An imposed line fails to describe curvature or clusters.','Look at residual patterns.','Horizontal line y=2 misses both center and ends of a parabola.']
],[
 ['Curved point set','What pattern do (−2,4),(−1,1),(0,0),(1,1),(2,4) suggest?','Recognize squared inputs.', ['At −2 and 2 the outputs both equal 4.','At −1 and 1 outputs both equal 1.','The central output is 0; points lie on y=x².'],'U-shaped, nonlinear association.','The rate is not constant across x.'],
 ['Compute variable changes','For y=x², changes from x=0→1 and 1→2 are what?','Subtract outputs.', ['From 0 to 1, y rises 1.','From 1 to 2, y rises 4−1=3.','Unequal changes prove not linear over these equal steps.'],'1 and 3; not constant.','A constant-slope model cannot exactly fit all points.'],
 ['Reject a misleading single-line claim','Can one horizontal line exactly contain all five points above?','Check y-values.', ['The points have y-values 4,1,0,1,4.','A horizontal line requires all y values equal.','These are different.'],'No.','Curved structure must be recognized.']
],[
 ['Assuming every association is linear.','Many relationships curve or form subgroups.','Inspect the whole cloud.'],
 ['Dismissing a pattern because linear correlation looks small.','Nonlinear patterns can have low linear correlation.','Check shape and spread.']
],['Not all associations form lines.','Curves and clusters require their own description.','Match model form to data shape.']),
lesson(8,'Analyzing Bivariate Data',[
 'Goal: evaluate a bivariate data set through plots, trend, outliers and limitations, and use evidence to support a concise statistical conclusion.',
 'A strong analysis identifies both variables, sample size, general direction, unusual observations and whether the conclusion concerns association or cause. Different scales and missing measurements can distort appearance.',
 'Investigate five original points (1,3),(2,4),(3,7),(4,8),(5,9). Compute how y generally changes and identify that one rise is larger than others. A teacher reviews statistical explanation, not a single predetermined adjective.'
],[
 ['Trend summary','A statement about overall change supported by points.','Describe direction and data range.','y rises from 3 to 9 as x rises from 1 to 5.'],
 ['Variation','Observed points need not follow equal steps.','Inspect deviations and local differences.','y steps are +1,+3,+1,+1.'],
 ['Statistical caveat','An observation-based conclusion can be limited.','State sample size and confounding possibility.','Five observations alone cannot establish causation.']
],[
 ['Endpoint rate','Across (1,3) to (5,9), what average y increase per x?','Divide differences.', ['Overall y change is 9−3=6.','x difference is 5−1=4.','Average endpoint slope is 6/4=1.5.'],'1.5 y units per x unit.','This is not the same as all local steps.'],
 ['Largest local rise','For y sequence 3,4,7,8,9, which consecutive rise is greatest?','Find differences.', ['4−3=1.','7−4=3.','8−7=1 and 9−8=1; largest is 3.'],'From 4 to 7, rise +3.','Other rises are +1.'],
 ['Check sample size','How many paired observations are in the set?','Count ordered pairs.', ['Each x-y row is one observation.','The list has five pairs.','Sample size is 5.'],'5.','Ten individual numerical values make five paired cases.']
],[
 ['Confusing endpoint slope with each local slope.','Averages can conceal variation.','Show both overall and local changes.'],
 ['Claiming strong causal evidence from a tiny sample.','Other variables and sampling uncertainty remain.','Use cautious association language.']
],['Analyze direction, variation and sample size.','State evidence rather than unsupported claims.','Outliers and confounders matter.']),
lesson(9,'Looking for Associations',[
 'Goal: compare two categorical variables using organized counts and relative frequencies to investigate whether proportions differ across groups.',
 'For categorical bivariate data a two-way table preserves both classifications. If the proportion preferring an option differs substantially by group, that indicates an association; raw counts alone can mislead when groups have different sizes.',
 'Original fictional table: Group A 12 choose bus, 8 walk; Group B 5 choose bus, 15 walk. Compute within-group proportions. Teacher review checks fairness of comparison and causal caution.'
],[
 ['Two-way table','Counts by combinations of two categories.','Rows and columns reflect the same observations.','Group A has bus 12, walk 8.'],
 ['Relative frequency','Part of a group satisfying a category.','Proportion=count/within-group total.','Group A bus share 12/20=60%.'],
 ['Categorical association','Differing conditional proportions across groups.','Compare percentages, not just counts.','Bus preference 60% in A and 25% in B.']
],[
 ['Group A percentage','12 of 20 in A choose bus. What percent?','Divide count by group total.', ['Bus count is 12.','Group total 12+8=20.','12/20=0.6=60%.'],'60%.','12 is 60% of 20.'],
 ['Group B percentage','5 of 20 in B choose bus. What percent?','Use conditional denominator.', ['Bus count 5.','Total group B 5+15=20.','5/20=0.25=25%.'],'25%.','Bus share differs from A.'],
 ['Compare proportions','How many percentage points apart are A and B bus shares?','Subtract percentages.', ['A bus share is 60%.','B bus share is 25%.','Difference is 60−25=35 percentage points.'],'35 percentage points.','This is an association measure, not a causal explanation.']
],[
 ['Using total across both groups when estimating within-group rate.','That answers a different conditional question.','Divide by the selected group size.'],
 ['Equating different shares with causal effect.','Group characteristics or selection may confound.','Describe association only.']
],['Use two-way tables for categorical data.','Compare conditional percentages.','Avoid unsupported causal conclusions.']),
lesson(10,'Using Data Displays to Find Associations',[
 'Goal: read two-way tables and segmented bar displays to decide whether categorical relationships show an association.',
 'For two groups of unequal sizes, construct 100% stacked bars to normalize counts. If category segments occupy different proportions, that suggests association between group and response.',
 'Use an original table: A 30 choose fruit and 10 choose cereal; B 12 choose fruit and 28 choose cereal. Predict each bar segment and compare. Teacher review assesses correctly labeled percent axes and context.'
],[
 ['Segmented bar','Displays category shares summing to 100% within each group.','Each bar spans 100%.','A fruit 75%, cereal 25%.'],
 ['Group total','Denominator for a conditional proportion.','Sum counts in one group.','A total 30+10=40.'],
 ['Between-group comparison','Compare equal-normalized percentages.','Read same category across bars.','A fruit 75% versus B fruit 30%.']
],[
 ['Group A share','Group A fruit 30, cereal 10. Fruit percent?','Count/total.', ['Total A=40.','Fruit count=30.','30/40=75%.'],'75%.','Remaining cereal share is 25%.'],
 ['Group B share','Group B fruit 12, cereal 28. Fruit percent?','Normalize within B.', ['Total B=12+28=40.','12/40=0.3.','Fruit share 30%.'],'30%.','Cereal share 70%.'],
 ['Association magnitude','Difference in fruit share: A 75%, B 30%.','Subtract normalized shares.', ['Both percentages use within-group denominators.','75−30=45.','Difference is 45 percentage points.'],'45 percentage points.','Groups have substantially different preference proportions.']
],[
 ['Using unequal bar heights for a 100%-stacked graph.','These bars represent proportions and must each span 100%.','Normalize by group total.'],
 ['Comparing count sizes without group denominators.','Unequal group sizes can skew counts.','Compute conditional proportions.']
],['Two-way tables preserve joint categories.','Segmented bars show within-group shares.','Different distributions suggest association.']),
lesson(11,'Gone In 30 Seconds',[
 'Goal: design a short timing investigation, collect paired observations and analyze variability without claiming that a small sample yields universal conclusions.',
 'An original 30-second challenge can measure how many simple symbols a participant copies in each of two trials. Record trial 1 and trial 2 counts as a pair for each consenting learner; do not upload named class data. Discuss practice effect, measurement error and differences among learners.',
 'Use fictional counts (10,12),(15,17),(20,21),(25,24) for worked analysis. Teachers review ethical collection, the scatter plot, and whether conclusions are appropriately qualified. One numerical rate cannot certify experimental design.'
],[
 ['Repeated measures','Two values from the same participant.','Keep trial pairs matched.','(15,17) means first trial 15, second 17.'],
 ['Change between trials','Second minus first for each participant.','Positive means improvement in this metric.','(15,17) changes +2.'],
 ['Experiment limitation','A short class study with sampling and order effects.','Do not infer causation automatically.','Practice effect may raise second-trial counts.']
],[
 ['Compute pair change','Participant records (15,17) symbols in two trials. What change?','Subtract trial counts.', ['First count 15.','Second count 17.','Difference 17−15=+2.'],'+2 symbols.','An increase of two in 30 seconds.'],
 ['Mean change','Pairs (10,12),(15,17),(20,21),(25,24). Average second-first change?','Sum differences and divide count.', ['Changes are 2,2,1,−1.','Sum is 4.','Four participants give mean change 4/4=1.'],'+1 symbol per participant on average.','This average hides one participant’s decrease.'],
 ['Assess variability','Do all four participants improve?','Inspect signs.', ['Three changes are positive: 2,2,1.','One change is negative: −1.','Not every participant improves.'],'No; 3 of 4 improve.','Sample does not prove a universal practice effect.']
],[
 ['Mixing trial counts across participants.','That destroys repeated-measure pairing.','Use one paired row per participant.'],
 ['Claiming second-trial improvements prove a universal skill effect.','Small sample and practice/order confounding remain.','Discuss uncertainty and experimental design.']
],['Keep paired timing data private and labeled.','Analyze change and variability.','Teacher review is needed for experimental conclusions.'])
];
if(rows.length!==11||new Set(rows.map(r=>r.LessonCode)).size!==11)throw Error('U06 count mismatch');
for(const r of rows)if(r.ExplanationParagraphs.length<2||r.KeyConcepts.length<3||r.WorkedExamples.length<3||r.CommonMistakes.length<2||r.SummaryPoints.length<3)throw Error('Bad rich shape '+r.LessonCode);
const out=path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u06-l01-l11-2017.rich-lesson-v2.json');
fs.writeFileSync(out,JSON.stringify({SchemaVersion:1,ContentVersion:'us-ccss-g8-u06-first-party-rich-v2-20261029-v1',PackCode:'US-CCSS-MATH',Lessons:rows},null,2)+'\n');
console.log('G8_U06 '+JSON.stringify({lessons:rows.length,examples:rows.reduce((a,r)=>a+r.WorkedExamples.length,0),out}));
