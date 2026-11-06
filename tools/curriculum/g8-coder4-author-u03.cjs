'use strict';
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const lesson=(n,title,paragraphs,concepts,examples,errors,summary)=>({
 LessonCode:'PED:US-CCSS-MATH:G8:U03:L'+String(n).padStart(2,'0'),
 CultureCode:'en',Title:title,ExplanationParagraphs:[...paragraphs,
 'First-party Edulytiks adaptation informed by Open Up Resources / Illustrative Mathematics Grade 8 (2017–2019), CC BY 4.0; exact public source: https://im.kendallhunt.com/MS/teachers/3/3/'+n+'/preparation.html . No publisher artwork or protected tasks are copied.'],
 KeyConcepts:concepts.map(([Title,Definition,Rule,Example])=>({Title,Definition,Rule,Example})),
 WorkedExamples:examples.map(([Title,Question,Method,Steps,Answer,Check])=>({Title,Question,Method,Steps,Answer,Check})),
 CommonMistakes:errors.map(([Mistake,WhyWrong,Correction])=>({Mistake,WhyWrong,Correction})),
 SummaryPoints:summary,Visuals:[],Videos:[]
});
const rows=[
lesson(1,'Understanding Proportional Relationships',[
 'Goal: interpret graphs of proportional relationships and explain the significance of points on a graph, especially the origin. Determine what a relationship represents using labeled axes and a contextual unit rate.',
 'A proportion y=kx appears as a straight line through the origin when plotted against consistent numerical axes; the point (1,k) describes the unit rate. Not every line through or near a drawn origin establishes a legitimate proportional model unless quantities and units support it.',
 'Investigate a chart of notebooks at $3 each: points (0,0), (1,3), (2,6). Draw axes labeled notebooks and cost, explain the meaning of the origin, and distinguish a line with a fixed admission fee. A teacher checks explanations and accurate graph labeling.'
],[
 ['Proportional graph','A straight relationship y=kx with constant quotient y/x for x≠0.','It passes through (0,0) and (1,k).','Cost $3 per notebook is y=3x.'],
 ['Unit rate','The output for one unit of input.','Read the y-coordinate when x=1.','At (1,3) the rate is $3 per notebook.'],
 ['Contextual origin','The point (0,0) on proportional axes.','Interpret both quantities as zero, not merely the picture.','Buying zero notebooks costs zero before extra fees.']
],[
 ['Read the unit rate','Points (0,0),(1,3),(2,6) lie on a graph. What is the constant rate?','Use y/x for a positive x.', ['At (1,3), the output/input ratio is 3/1=3.','At (2,6), it is 6/2=3.','The consistent rate is 3 per unit.'],'3 output units per input unit.','Both ratios agree.'],
 ['Check a proportional pair','Is y=4x proportional? Does (3,12) lie on its graph?','Test both the intercept and substitution.', ['y=4x has intercept 0, so it passes through the origin.','At x=3, y=4×3=12.','The point (3,12) satisfies the relation.'],'Yes; (3,12) lies on a proportional graph.','The unit rate is 4.'],
 ['Detect a fixed fee','Does C=3n+5 represent a proportional cost relationship?','Inspect the value at zero input.', ['At n=0, C=5.','A proportional relationship must have zero output at zero input.','The $5 fixed fee means C is linear but not proportional.'],'No.','Its graph passes through (0,5), not the origin.']
],[
 ['Relying only on a straight-looking sketch.','A nonzero intercept may be hidden by scale.','Verify (0,0) and the constant ratio.'],
 ['Calling (0,0) meaningless.','It may encode a real zero-quantity condition.','State its contextual units and assumptions.']
],['Proportional relationships have constant unit rate and zero intercept.','Graph labels and scales determine interpretation.','Nonzero fixed fees break proportionality.']),
lesson(2,'Graphs of Proportional Relationships',[
 'Goal: interpret how axis scales affect the appearance of a proportional graph and compare equivalent representations of the same constant rate.',
 'The same mathematical line can appear visually steeper or flatter if the horizontal and vertical pixel scales change. Numerical rate comes from labeled coordinate changes, not the drawing angle on the page.',
 'Plot the relationship y=2x with one graph using y-ticks of 1 and another using y-ticks of 5. Compare the plotted points and visual slope. A teacher evaluates whether the learner distinguishes display scale from mathematical rate.'
],[
 ['Axis scale','The numerical quantity represented by equal distances on an axis.','Read tick labels before comparing slope.','One y-grid cell may stand for 1 or 5 units.'],
 ['Rate invariant','The ratio Δy/Δx determined by coordinates.','Changing plotting scale does not change the equation.','y=2x always has rate 2.'],
 ['Origin','The point where both values are zero.','A proportional graph still passes through (0,0) under rescaling.','The origin represents no output at zero input.']
],[
 ['Compare two graphs','Two graphs of y=2x use different vertical grid spacing. Has the unit rate changed?','Use coordinates rather than drawn angle.', ['On either graph, x=1 gives y=2.','On either graph, x=3 gives y=6.','The rate (6−2)/(3−1)=2 regardless of pixels.'],'No; the mathematical unit rate remains 2.','Numerical point ratios agree.'],
 ['Read sparse ticks','The y-axis labels increase by 5 per cell. A point sits 3 cells above zero. What y-value is it?','Convert cells to numerical units.', ['Each y-cell represents 5 units.','Three cells represent 3×5.','The numerical y-value is 15.'],'15.','The plotted coordinate must be read from the scale.'],
 ['Find proportional rate','A graph contains (0,0) and (4,12). Find k.','Divide y by x at a nonzero point.', ['At (4,12), output is 12.','Input is 4.','Compute k=12÷4=3.'],'k=3.','The equation is y=3x.']
],[
 ['Comparing pixel steepness across differently scaled graphs.','The apparent angle depends on axis scale.','Compare numerical coordinates and units.'],
 ['Counting tick marks as unit steps when labels skip values.','A cell may represent more than 1.','Multiply by the axis increment.']
],['Labeled scales control interpretation.','Rate is a numerical quotient, not a picture angle.','The same proportional relationship may look different.']),
lesson(3,'Representing Proportional Relationships',[
 'Goal: create and connect tables, equations and graphs of proportional relationships, deciding when a given point lies on the line.',
 'A constant rate k gives table outputs y=kx, a straight graph through (0,0), and an equation with no added constant. Input units and output units must be recorded consistently.',
 'Use a table of distance traveled at a steady 4 kilometers per hour. Extend the table and graph the points. The teacher reviews readable axes, plotted labels and discussion of continuous versus discrete inputs.'
],[
 ['Proportional equation','The algebraic representation y=kx.','Output is always k times input.','d=4t for speed 4 km/h.'],
 ['Table of values','Paired inputs and outputs linked by one rule.','Multiply each input by k.','t=0,1,2,3 gives d=0,4,8,12.'],
 ['Graph of values','Points (x,kx) on a line through the origin.','Use an appropriate domain and labeled axes.','(3,12) belongs to d=4t.']
],[
 ['Complete the distance table','At 4 km/h, find distances at 0,1,2,3 hours.','Use d=4t.', ['At zero hours, 4×0=0.','At 1 and 2 hours, distances are 4 and 8.','At 3 hours, distance is 12.'],'0, 4, 8, 12 kilometers.','Each extra hour adds 4 km.'],
 ['Test a candidate point','Does (5,20) lie on d=4t?','Substitute input t=5.', ['The predicted output is 4×5=20.','The candidate output is 20.','They agree, so the point is on the graph.'],'Yes.','20/5=4.'],
 ['Reject an inconsistent table','A table lists (1,4),(2,8),(3,11). Is it perfectly proportional?','Check output/input ratios.', ['4/1=4.','8/2=4.','11/3≠4, so one row breaks the rule.'],'No.','At x=3, y should be 12.']
],[
 ['Adding an unexplained initial value to y=kx.','That changes the origin condition.','Use y=kx for genuinely proportional relations.'],
 ['Plotting pairs with axes reversed.','The graph then represents a different rate.','Label axes and units first.']
],['Tables, graphs and equations can represent one constant unit rate.','Proportional graphs pass through the origin.','Verify every table pair.']),
lesson(4,'Comparing Proportional Relationships',[
 'Goal: compare proportional relationships given in different forms by finding their unit rates with consistent input-output units.',
 'Do not decide that a line is faster solely by visual steepness if the display scales differ. Calculate k from an equation, table or labeled point, then compare like units.',
 'Compare courier A traveling 12 km in 3 hours with courier B described by d=5t. State which covers more distance per hour, and explain why the values have the same units.'
],[
 ['Comparable unit rates','Rates expressed in the same output/input units.','Convert units before ranking.','A: 12/3=4 km/h; B: 5 km/h.'],
 ['Rate from a table','Divide nonzero output by input.','k=y/x when proportional.','20 liters in 4 minutes is 5 L/min.'],
 ['Rate from an equation','The coefficient k in y=kx.','Identify which variable is output.','d=5t has rate 5 km per hour.']
],[
 ['Two couriers','A travels 12 km in 3 h; B follows d=5t km. Who is faster?','Compare distance/time rates.', ['Courier A rate is 12/3=4 km/h.','Courier B equation gives 5 km/h.','5 exceeds 4, so B has the higher rate.'],'B is faster by 1 km/h.','Both rates use kilometers per hour.'],
 ['From a table','A proportional machine produces 28 objects in 7 minutes. Find its rate.','Divide output by time.', ['Output is 28 objects.','Input is 7 minutes.','28/7=4 objects per minute.'],'4 objects/minute.','7×4=28.'],
 ['Unit conversion','Rate A is 120 meters/minute; rate B is 3 meters/second. Which is larger?','Convert to the same time unit.', ['B is 3×60=180 meters/minute.','A is 120 meters/minute.','B exceeds A by 60 meters/minute.'],'B is larger.','Comparing 120 to 3 without conversion would be invalid.']
],[
 ['Comparing unmatched time units.','A minute and second are different quantities.','Convert before comparing.'],
 ['Comparing graph angles without axis-scale information.','Display geometry may be misleading.','Use numerical unit rates.']
],['The unit rate is the basis of comparison.','Rates must use compatible units.','Equations, tables and graphs can be reduced to a common k.']),
lesson(5,'Introduction to Linear Relationships',[
 'Goal: recognize a linear relationship as constant additive change in output per equal input step, and distinguish it from a proportional relationship.',
 'A nonproportional linear model can start above zero. A relationship such as y=3x+4 increases by 3 for each additional x while the output at x=0 is 4.',
 'Investigate a bike-rental fee: $4 booking charge plus $3 per hour. Make a table, graph and explain the intercept. The teacher reviews the interpretation; numbers alone do not verify the story.'
],[
 ['Linear relationship','Output changes by a constant amount for equal input steps.','y=mx+b for a nonvertical linear graph.','C=3h+4.'],
 ['Initial value','Output when input is zero.','b=y(0).','Bike-rental fee at h=0 is $4.'],
 ['Rate of change','Amount added to output per 1 input.','m=Δy/Δx.','Cost rises $3 per hour.']
],[
 ['Evaluate rental model','A bike costs $4 booking plus $3 per hour. Find cost at 2 hours.','Use C=3h+4.', ['Two hours cost 3×2=6 in time charges.','Add the booking fee 4.','Total is 10 dollars.'],'$10.','C(2)=10.'],
 ['Identify initial charge','For y=3x+4, what is y when x=0?','Substitute x=0.', ['Compute 3×0=0.','Add 4.','The starting output is 4.'],'4.','Graph crosses vertical axis at (0,4).'],
 ['Test proportionality','Is y=3x+4 proportional?','Test zero input.', ['A proportional graph must pass through (0,0).','Here y(0)=4.','The nonzero intercept disproves proportionality.'],'No.','Constant change does not imply proportionality.']
],[
 ['Treating any straight line as proportional.','A straight line can have nonzero intercept.','Check the origin and b.'],
 ['Confusing fixed fee with per-hour rate.','They have different units.','Label m and b with correct units.']
],['Linear relationships have constant additive change.','Proportionality adds the condition b=0.','State slope and initial value in context.']),
lesson(6,'More Linear Relationships',[
 'Goal: analyze multiple linear relationships between quantities, including decreasing values, using tables and graphs without assuming every intercept is positive.',
 'When input increases by one, output can rise, fall or remain fixed. Slope may be negative or zero; a linear rule uses one constant change for every equal input step.',
 'Analyze a tank with 18 liters losing 3 liters each minute. Build a table until empty, discuss the limited physical domain, and distinguish that from purely algebraic continuation. Teacher review checks model assumptions.'
],[
 ['Negative linear rate','Output decreases by a fixed amount for each input step.','m<0.','V=18−3t.'],
 ['Constant difference','Equal input changes give equal output differences.','Compare successive table rows.','18,15,12,9 decreases by 3.'],
 ['Practical domain','Only inputs sensible for the real situation.','An emptied tank cannot have negative volume.','V=18−3t supports 0≤t≤6 min physically.']
],[
 ['Tank at four minutes','18 liters lose 3 per minute. How much after 4 minutes?','Use V=18−3t.', ['Four minutes remove 3×4=12 liters.','Subtract from 18.','Volume is 6 liters.'],'6 liters.','18−12=6.'],
 ['Time to empty','When does V=18−3t equal zero?','Solve for t.', ['0=18−3t.','Then 3t=18.','Divide to obtain t=6.'],'6 minutes.','Volume at t=6 is 0.'],
 ['Test linear table','At times 0,1,2,3, outputs are 10,7,4,1. Is this linear?','Inspect equal successive differences.', ['7−10=−3.','4−7=−3 and 1−4=−3.','Constant difference for 1-unit input increments gives slope −3.'],'Yes, linear with slope −3.','A matching rule is y=10−3x.']
],[
 ['Extending a physical model beyond valid range.','Negative contents are not possible after emptying.','State the time domain.'],
 ['Reading a downward line as a positive rate.','Increasing x causes decreasing y.','Use signed differences.']
],['Constant change can be positive, negative or zero.','Tables reveal slope through equal differences.','A model needs its real-world domain.']),
lesson(7,'Representations of Linear Relationships',[
 'Goal: write equations from real situations and translate between a verbal description, a table of values and an equation.',
 'Identify a starting amount and a repeated increment before choosing variables. A rule is not fully communicated if units or the meaning of the input are omitted.',
 'Model a delivery service charging $6 flat plus $2 per kilometer, then explain how its graph differs from a pure $2-per-kilometer proportional cost. A teacher checks the verbal-to-algebraic translation.'
],[
 ['Fixed charge','A cost independent of quantity x.','It becomes intercept b.','$6 base charge.'],
 ['Unit charge','Cost per additional unit of quantity.','It becomes slope m.','$2 per kilometer.'],
 ['Model equation','A complete equation with defined variables.','C=mx+b.','C=2d+6, C dollars, d kilometers.']
],[
 ['Write delivery cost','Service charges $6 fixed and $2 per km. Write C(d).','Combine fixed and variable charges.', ['The unit charge contributes 2d.','The fixed fee contributes +6.','Total is C(d)=2d+6.'],'C=2d+6.','At d=0 the cost is $6.'],
 ['Predict at 5 km','Use C=2d+6 for d=5.','Substitute distance.', ['Variable charge is 2×5=10.','Add fixed 6.','Cost is $16.'],'$16.','C(5)=16.'],
 ['Recover distance','A trip costs $22. How many km under C=2d+6?','Solve for d.', ['22=2d+6.','Subtract six: 16=2d.','Divide by two: d=8.'],'8 kilometers.','2×8+6=22.']
],[
 ['Using distance instead of cost as the dependent variable.','Variables and their units are reversed.','State what each symbol measures.'],
 ['Leaving the fixed charge out.','The initial cost is not zero.','Check x=0.']
],['Use contextual units and a named variable.','Rate maps to slope and fixed charge to intercept.','Check with a table or substitution.']),
lesson(8,'Translating to $y=mx+b$',[
 'Goal: examine how moving the graph of a line changes its equation, particularly the intercept while preserving the slope under a vertical translation.',
 'If y=mx is shifted upward c units, its equation becomes y=mx+c. A horizontal shift of y=mx+b by h units right has equation y=m(x−h)+b, so its intercept changes by −mh; do not conflate horizontal and vertical shifts.',
 'Graph y=2x and y=2x+3 on the same grid and compare corresponding points. A teacher checks whether the drawn lines are parallel and the vertical displacement is consistent.'
],[
 ['Vertical translation','Add c to every y-coordinate.','y=f(x) becomes y=f(x)+c.','y=2x becomes y=2x+3.'],
 ['Unchanged slope','A vertical translation preserves rate of change.','Only b changes in y=mx+b.','Both y=2x and y=2x+3 have slope 2.'],
 ['Horizontal translation','Move graph right h input units.','New equation y=f(x−h).','y=2x becomes y=2(x−4)=2x−8.']
],[
 ['Shift upward three','Write the image of y=2x after translating up 3.','Add 3 to output.', ['Original y at x is 2x.','Vertical shift adds 3 at the same x.','New line y=2x+3.'],'y=2x+3.','Slopes agree at 2; intercept changes to 3.'],
 ['Shift right four','Translate y=2x four units right.','Replace x by x−4.', ['Original relation is y=2x.','For a right shift use y=2(x−4).','Distribute to get y=2x−8.'],'y=2x−8.','Old point (0,0) moves to (4,0).'],
 ['Compare intercepts','Parallel lines y=−3x+1 and y=−3x+7 differ by what vertical translation?','Subtract intercepts.', ['Both slopes equal −3.','At x=0 their y-values are 1 and 7.','Difference is 7−1=6 upwards.'],'Up 6 units.','Every y-value differs by 6.']
],[
 ['Changing slope during a pure translation.','A translation does not rotate a line.','Keep m constant.'],
 ['Adding h to x for a rightward shift without a coordinate check.','Right shifts require replacing x by x−h.','Verify where the origin point moves.']
],['Parallel translations keep slope.','Vertical shift adds to intercept.','Horizontal shifts need substitution f(x−h).']),
lesson(9,"Slopes Don't Have to be Positive",[
 'Goal: explain a negative slope as a decrease in y for each positive step in x and link this to a graph, table and contextual description.',
 'A line dropping rightward has negative slope. The magnitude describes how quickly y decreases; a horizontal line has zero slope and a vertical one has undefined slope.',
 'Discuss an original downhill elevation example, beginning at height 20 m and losing 4 m per horizontal meter. Relate the physical direction to negative rise. The teacher checks story interpretation and correct graphical scales.'
],[
 ['Negative slope','A negative rise divided by positive run.','m<0 when y falls as x increases.','(0,20) to (3,8) gives m=−4.'],
 ['Magnitude','Absolute slope tells change per input without direction.','|−4|=4 units of drop per x unit.','A decrease of 4 m per horizontal meter.'],
 ['Zero slope','No output change as input grows.','A horizontal line has m=0.','y=5.']
],[
 ['Find downhill slope','Elevation goes from 20 m at x=0 to 8 m at x=3. What slope?','Compute rise/run.', ['Δy=8−20=−12.','Δx=3−0=3.','Slope m=−12/3=−4.'],'−4 meters per horizontal meter.','A positive x step reduces y by four.'],
 ['Predict height','Line y=20−4x; what is height at x=2?','Substitute x=2.', ['Multiply 4×2=8.','Subtract from 20.','Predicted elevation is 12.'],'12 meters.','At x=2, y=12.'],
 ['Contrast horizontal','Line through (1,5),(9,5). Find slope.','Divide change in y by change in x.', ['Rise is 5−5=0.','Run is 9−1=8.','0/8=0.'],'0.','Both points have y=5.']
],[
 ['Taking only absolute differences.','That loses the direction of change.','Use signed coordinate differences.'],
 ['Confusing slope zero with undefined.','Horizontal and vertical lines behave differently.','Check whether run is zero or rise is zero.']
],['Negative slope means decrease for increasing x.','The magnitude controls rate of decrease.','Horizontal slope is zero.']),
lesson(10,'Calculating Slope',[
 'Goal: calculate slope from two points correctly, including negative or fractional values, and recognize that a vertical line has no finite slope.',
 'Compute m=(y₂−y₁)/(x₂−x₁), with the same point ordering in numerator and denominator. Reversing both orders changes both signs and yields the same slope.',
 'Use two independently chosen point pairs on one line to verify slope consistency. A teacher inspects labeled rise/run triangles; computed ratios receive direct numerical checks.'
],[
 ['Slope formula','Difference of outputs divided by difference of inputs.','m=(y₂−y₁)/(x₂−x₁).','For (1,2),(4,8), m=6/3=2.'],
 ['Order invariance','Reversing both point orders preserves the quotient.','Use consistent subtraction order.','(2−8)/(1−4)=−6/−3=2.'],
 ['Vertical special case','Points with equal x give denominator zero.','Slope is undefined.','Through (3,1),(3,6), x change is zero.']
],[
 ['Positive slope','Slope between (1,2) and (4,8).','Use signed changes.', ['Rise is 8−2=6.','Run is 4−1=3.','6/3=2.'],'2.','Reversing both differences yields the same answer.'],
 ['Negative fractional slope','Slope from (−2,5) to (4,2).','Compute exact fraction.', ['Rise is 2−5=−3.','Run is 4−(−2)=6.','m=−3/6=−1/2.'],'−1/2.','A six-unit right step lowers y by three.'],
 ['Vertical exception','Slope through (3,1),(3,6).','Check denominator.', ['Rise is 6−1=5.','Run is 3−3=0.','5/0 is undefined.'],'Undefined.','Vertical lines cannot be written y=mx+b.']
],[
 ['Mixing point orders.','A positive denominator with reversed numerator changes sign.','Subtract in a consistent order.'],
 ['Forcing a number when Δx=0.','Division by zero is undefined.','Report vertical slope separately.']
],['Slope uses signed rise over run.','Any two points on one nonvertical line give the same slope.','Equal x-coordinates imply undefined slope.']),
lesson(11,'Equations of All Kinds of Lines',[
 'Goal: distinguish equations of vertical and horizontal lines from nonhorizontal, nonvertical slope-intercept equations.',
 'Horizontal lines have constant y and zero slope. Vertical lines have constant x and undefined slope; they cannot be represented as y=mx+b with a finite m.',
 'Sketch x=−2 and y=3 and find their intersection. A teacher checks accurate axes and labeling; substituting coordinates can validate points independently.'
],[
 ['Horizontal line','All points share one y-coordinate.','Equation y=c and slope 0.','y=3 includes (−1,3),(2,3).'],
 ['Vertical line','All points share one x-coordinate.','Equation x=c and undefined slope.','x=−2 includes (−2,0),(−2,4).'],
 ['Intersection','Coordinates satisfying both line equations.','Solve simultaneous x=c and y=d.','x=−2 and y=3 meet at (−2,3).']
],[
 ['Equation of horizontal line','What equation passes through (2,3),(−1,3)?','Identify unchanged y.', ['Both y-coordinates are 3.','Every point along the horizontal line has y=3.','Write y=3.'],'y=3.','Slope (3−3)/(2+1)=0.'],
 ['Equation of vertical line','What equation passes through (−2,0),(−2,4)?','Identify unchanged x.', ['Both x-coordinates are −2.','All points on the vertical line have x=−2.','Write x=−2.'],'x=−2.','Vertical slope is undefined.'],
 ['Find intersection','Find the intersection of x=−2 and y=3.','Apply both independent coordinate conditions.', ['First coordinate must equal −2.','Second coordinate must equal 3.','The only common point is (−2,3).'],'(−2,3).','It satisfies both equations.']
],[
 ['Calling a vertical line y=constant.','That describes a horizontal line.','Check which coordinate remains fixed.'],
 ['Assigning zero slope to x=c.','Its run is zero, so the slope is undefined.','Write the vertical equation directly.']
],['Horizontal lines have y=c.','Vertical lines have x=c.','Their intersections satisfy both constraints.']),
lesson(12,'Solutions to Linear Equations',[
 'Goal: interpret an ordered pair (x,y) as a solution of a two-variable linear equation precisely when substituting both coordinates makes an equality true.',
 'One equation in two variables usually has infinitely many real solutions forming a straight line, not one arbitrary numeric answer. Confirm candidate pairs by direct substitution and describe a parameterized collection.',
 'Build a table for 2x+y=7, plot its points and decide whether (2,3) or (2,4) belongs. The teacher reviews the line graph and explanation; substitution is independently gradable.'
],[
 ['Solution pair','Coordinates that make the equation true.','Substitute x and y simultaneously.','(2,3) solves 2x+y=7.'],
 ['Graph of solutions','All satisfying pairs form a line for a nondegenerate linear equation.','Rewrite as y=−2x+7.','Points (0,7),(2,3) lie on one line.'],
 ['Non-solution','A pair whose substitution makes false equality.','Reject even if near the drawn line.','(2,4) gives 8, not 7.']
],[
 ['Check valid pair','Does (2,3) solve 2x+y=7?','Substitute both values.', ['Compute 2×2=4.','Add y=3 to obtain 7.','The equation equals the required right side.'],'Yes.','2(2)+3=7.'],
 ['Reject invalid pair','Does (2,4) solve 2x+y=7?','Compute left side.', ['Compute 2×2=4.','Add 4 to obtain 8.','Since 8≠7, the pair is not a solution.'],'No.','Its left side is 8.'],
 ['Generate another solution','Find y when x=−1 in 2x+y=7.','Solve for y.', ['Insert x=−1: −2+y=7.','Add two to both sides.','y=9, giving (−1,9).'],'(−1,9).','2(−1)+9=7.']
],[
 ['Only testing x and forgetting y.','A two-variable solution requires both coordinates.','Substitute the ordered pair completely.'],
 ['Assuming one equation has one ordered-pair solution.','A line usually contains infinitely many real pairs.','Use a free parameter to generate further points.']
],['Ordered pairs must satisfy the full equation.','A two-variable linear equation graphs a line.','Test both positive and negative inputs.']),
lesson(13,'More Solutions to Linear Equations',[
 'Goal: generate, verify and compare multiple ordered-pair solutions of two-variable linear equations, including equations in standard form and constraints.',
 'When one variable is selected freely, solve for the other. A pair that works for one equation may fail another, and a line can be expressed in equivalent equation forms.',
 'Use 2x+3y=12 and generate its x- and y-intercepts. Interpret their locations graphically. A teacher checks how the learner constructs a complete line from these points.'
],[
 ['Standard form','An equation Ax+By=C.','Solve one variable in terms of the other.','2x+3y=12.'],
 ['Intercepts','Where a line meets the coordinate axes.','Set y=0 for x-intercept, x=0 for y-intercept.','Intercepts of 2x+3y=12 are (6,0),(0,4).'],
 ['Equivalent equations','Equations with the same solution set.','Apply reversible algebraic operations.','2x+3y=12 equals y=4−(2/3)x.']
],[
 ['Find x-intercept','Find x-intercept of 2x+3y=12.','Set y=0.', ['With y=0, equation becomes 2x=12.','Divide by 2.','x=6, giving (6,0).'],'(6,0).','2×6+3×0=12.'],
 ['Find y-intercept','Find y-intercept of 2x+3y=12.','Set x=0.', ['With x=0, equation becomes 3y=12.','Divide by 3.','y=4, giving (0,4).'],'(0,4).','2×0+3×4=12.'],
 ['Find another pair','Choose x=3 in 2x+3y=12. Find y.','Substitute and isolate.', ['2×3+3y=12.','Subtract 6: 3y=6.','Divide by 3, so y=2.'],'(3,2).','2×3+3×2=12.']
],[
 ['Switching x-intercept and y-intercept procedures.','Intercepts require setting the opposite coordinate to zero.','Label the axis before substituting.'],
 ['Inferring solution pairs from a rough graph only.','Pixel estimates can be wrong.','Verify algebraically.']
],['Equivalent equations can represent the same line.','Intercepts give reliable plotting points.','Check every solution by substitution.']),
lesson(14,'Using Linear Relations to Solve Problems',[
 'Goal: model a real situation with a linear equation, identify meaningful variables, solve for an unknown quantity and check the solution against context.',
 'A model with constant rate and a fixed starting value can be useful, but not every real situation extends indefinitely. Identify where the rule applies and state assumptions before interpreting an answer.',
 'Original Edulytiks task: a repair shop charges $18 inspection plus $7 per labor hour. Write total cost, solve when a bill is $60, and explain why the time cannot be negative. A teacher evaluates variable definitions and contextual interpretation.'
],[
 ['Fixed-plus-variable cost','Total starts at a fixed amount and increases with usage.','C=18+7h.','At h=0, the fee is $18.'],
 ['Inverse problem','Find input from known output.','Set equation equal to measured total and solve.','If C=60, solve 18+7h=60.'],
 ['Feasible domain','Inputs meaningful in the problem.','Labor time must be nonnegative.','h≥0; predicted cost cannot be below $18 in this model.']
],[
 ['Write cost equation','Inspection $18 plus $7/hour. Write C(h).','Separate fixed fee and hourly charge.', ['Inspection contributes $18.','h hours contribute $7h.','Total is C=18+7h.'],'C(h)=18+7h dollars.','C(0)=18.'],
 ['Find labor hours','A bill is $60 under C=18+7h. How many hours?','Solve for h.', ['60=18+7h.','Subtract 18: 42=7h.','Divide by 7: h=6.'],'6 hours.','18+7×6=60.'],
 ['Reject impossible bill','Could C=11 be produced by C=18+7h with h≥0?','Check the minimum feasible cost.', ['At h=0 the bill is $18.','For h≥0, 7h≥0 so cost≥18.','An $11 bill is below the minimum.'],'No; not within the stated model.','An algebraic negative h would violate the context.']
],[
 ['Giving a negative hour count as realistic.','Labor time is nonnegative in this model.','Restrict domain before interpreting solutions.'],
 ['Ignoring model assumptions.','A flat and hourly rate may change for overtime or surcharges.','State the valid pricing rule and range.']
],['Choose clear variables and units.','A fixed fee is an intercept, an hourly charge a slope.','Verify solutions in the real context.'])
];
if(rows.length!==14||new Set(rows.map(x=>x.LessonCode)).size!==14)throw Error('Unit 3 identity mismatch');
for(const l of rows)if(l.ExplanationParagraphs.length<2||l.KeyConcepts.length<3||l.WorkedExamples.length<3||l.CommonMistakes.length<2||l.SummaryPoints.length<3)throw Error('Invalid rich V2 shape '+l.LessonCode);
const output=path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u03-l01-l14-2017.rich-lesson-v2.json');
fs.writeFileSync(output,JSON.stringify({SchemaVersion:1,ContentVersion:'us-ccss-g8-u03-first-party-rich-v2-20261029-v1',PackCode:'US-CCSS-MATH',Lessons:rows},null,2)+'\n');
console.log('G8_U03 '+JSON.stringify({lessons:rows.length,examples:rows.reduce((a,b)=>a+b.WorkedExamples.length,0),output}));
