'use strict';
const fs=require('node:fs'),path=require('node:path'),root=path.resolve(__dirname,'../..');
const lesson=(n,title,body,concepts,examples,errors,summaries)=>({
 LessonCode:'PED:US-CCSS-MATH:G8:U05:L'+String(n).padStart(2,'0'),CultureCode:'en',Title:title,
 ExplanationParagraphs:[...body,'Original first-party Edulytiks explanation based on publicly available Open Up Resources / Illustrative Mathematics 2017–2019, CC BY 4.0; source: https://im.kendallhunt.com/MS/teachers/3/5/'+n+'/preparation.html . Publisher artwork and assessment questions are not reproduced.'],
 KeyConcepts:concepts.map(([Title,Definition,Rule,Example])=>({Title,Definition,Rule,Example})),
 WorkedExamples:examples.map(([Title,Question,Method,Steps,Answer,Check])=>({Title,Question,Method,Steps,Answer,Check})),
 CommonMistakes:errors.map(([Mistake,WhyWrong,Correction])=>({Mistake,WhyWrong,Correction})),
 SummaryPoints:summaries,Visuals:[],Videos:[]
});
const rows=[
lesson(1,'Inputs and Outputs',[
 'Goal: invent, test and communicate rules that assign an output to each input, including numerical and nonnumerical examples.',
 'A rule may double an input, subtract a constant, or attach a categorical label. Specify the input set and what the rule produces; one input must not silently lead to contradictory outputs when a deterministic rule is intended.',
 'Partner task: describe a hidden rule using three input-output pairs and ask a peer to predict two more. Multiple rules may fit only three examples, so give additional evidence rather than pretending the pattern is uniquely determined. Teacher review examines rule clarity.'
],[
 ['Input','The quantity or item supplied to a rule.','Use x as an optional symbol.','Input 4 to a doubling rule.'],
 ['Output','The result assigned by the chosen rule.','Compute the rule consistently.','Doubling 4 gives 8.'],
 ['Rule description','A clear operation connecting input and output.','State its domain and operations.','f(x)=2x+1 for numeric x.']
],[
 ['Apply a rule','Rule f(x)=2x+1. Find f(4).','Substitute x=4.', ['Double 4 to obtain 8.','Add one.','Output equals 9.'],'9.','2×4+1=9.'],
 ['Find missing input','f(x)=3x−2 gives 13. What is x?','Solve 3x−2=13.', ['Add two: 3x=15.','Divide by three.','x=5.'],'5.','3×5−2=13.'],
 ['Rule ambiguity','Pairs (1,2),(2,4),(3,6) are observed. Is doubling the only possible rule?','Discuss finite observations.', ['Doubling fits all three.','Another expression can agree at x=1,2,3 but differ elsewhere.','More evidence or an explicit rule is needed.'],'No; doubling is one possible rule.','For example 2x+(x−1)(x−2)(x−3) matches these pairs but differs at x=4.']
],[
 ['Inferring a unique formula from a few pairs.','Several rules may fit finite observations.','Distinguish conjecture from established rule.'],
 ['Confusing input and output.','Reversing variables changes the mapping.','Label both sides of each pair.']
],['A rule assigns outputs to inputs.','Use clear input and output labels.','Examples alone may not uniquely determine a rule.']),
lesson(2,'Introduction to Functions',[
 'Goal: recognize a function as a relationship assigning exactly one output to each input in its domain, even though different inputs may share an output.',
 'Use a table or mapping diagram. A repeated input with two different outputs is not a function; repeated outputs for different inputs are allowed. Ask whether a learner’s original mapping can be reconstructed unambiguously.',
 'Design an original mapping using three named students and their birth month; each student has one assigned month, but multiple students can share a month. A teacher reviews the mapping’s domain and ambiguity.'
],[
 ['Function','Each allowed input has exactly one output.','Never assign two distinct outputs to one input.','f(2)=4 unambiguously.'],
 ['Domain','Set of inputs on which the function is defined.','State allowed x values.','For f(x)=1/x the input 0 is excluded.'],
 ['Shared output','Different inputs can map to the same output.','Uniqueness is required per input, not per output.','f(x)=x² maps 2 and −2 to 4.']
],[
 ['Recognize a function','Pairs (1,2),(2,3),(1,4) form a relation. Is it a function?','Check repeated inputs.', ['Input 1 occurs twice.','Outputs for input 1 are 2 and 4.','One input has two outputs, violating the definition.'],'No.','The mapping of input 1 is not unique.'],
 ['Shared output allowed','Pairs (1,4),(2,4),(3,9). Is it a function?','Inspect each input.', ['Inputs 1,2,3 are distinct.','Each has exactly one assigned output.','Two inputs happen to share output 4, which is allowed.'],'Yes.','The function requirement concerns one output per input.'],
 ['Find domain exception','Why is f(x)=1/x undefined at x=0?','Inspect the denominator.', ['Substituting 0 produces 1/0.','Division by zero is undefined.','Thus zero is not in the real domain.'],'0 is excluded from the real domain.','Every other real x gives one output.']
],[
 ['Requiring all outputs to be different.','Multiple inputs may share an output.','Check uniqueness per input.'],
 ['Ignoring inputs where an equation is undefined.','A function must have a legitimate output for every input in its stated domain.','State domain exclusions.']
],['A function assigns one output to each input.','Repeated outputs are permitted.','State the domain.']),
lesson(3,'Equations for Functions',[
 'Goal: evaluate a function from an equation for specified inputs and, when possible, solve an equation to find input(s) that produce a given output.',
 'Use notation f(3) for the output when input is 3, not multiplication by f. A linear function is one-to-one when slope is nonzero, but f(x)=x² can produce one output from two inputs.',
 'Compare f(x)=2x−5 and g(x)=x² with positive and negative inputs. Teacher review checks clear notation and explanation of why inverse output questions may have multiple answers.'
],[
 ['Function notation','f(a) denotes the output at x=a.','Substitute a into every x in the rule.','f(3)=2×3−5=1.'],
 ['Solve for input','Find x from a specified output y.','Set f(x)=y and solve.','2x−5=9 gives x=7.'],
 ['Multiple preimages','Distinct inputs may have the same output.','Reverse questions may have several answers.','x²=9 gives x=−3 or 3.']
],[
 ['Evaluate affine function','For f(x)=2x−5, find f(3).','Substitute 3.', ['Multiply 2×3=6.','Subtract 5.','f(3)=1.'],'1.','2(3)−5=1.'],
 ['Solve for input','f(x)=2x−5. Which x gives output 9?','Set f(x)=9.', ['2x−5=9.','2x=14.','x=7.'],'7.','2×7−5=9.'],
 ['Two inputs same output','For g(x)=x², find all real x with g(x)=9.','Solve square equation.', ['Set x²=9.','Both 3² and (−3)² equal 9.','Real solutions are x=−3 and x=3.'],'−3 and 3.','Squaring erases sign.']
],[
 ['Treating f(3) as f times 3.','Function notation means evaluation.','Replace x with 3 inside formula.'],
 ['Reporting only positive square root.','A square may have positive and negative preimages.','Check both signs.']
],['Evaluate by substitution.','Inverse input questions may be nonunique.','Notation identifies input and output precisely.']),
lesson(4,'Tables, Equations, and Graphs of Functions',[
 'Goal: connect a function equation with numerical table pairs and their plotted points in the coordinate plane.',
 'For y=x², table outputs at −2,−1,0,1,2 are 4,1,0,1,4. The graph is curved rather than a line; a function need not be linear. A vertical line intersects the graph at most once for each x in its domain.',
 'Plot the square rule with labeled axes, join points smoothly based on the real domain, and distinguish the exact equation from sparse observations. Teacher review checks axes and the curve.'
],[
 ['Table row','A point (x,f(x)) from a function.','Evaluate each stated input.','At x=−2, f(x)=4 for x².'],
 ['Graph','Visual representation of all allowed input-output pairs.','Plot x horizontally and f(x) vertically.','(2,4) belongs to y=x².'],
 ['Vertical-line property','A function has no two outputs at the same x.','Any vertical line intersects a function graph at most once.','A circle fails this test over most x values.']
],[
 ['Complete square table','Find x² for x=−2,−1,0,1,2.','Square each integer.', ['(−2)²=4 and (−1)²=1.','0²=0 and 1²=1.','2²=4.'],'4, 1, 0, 1, 4.','Positive and negative equal magnitudes share outputs.'],
 ['Check plotted point','Is (−3,9) on y=x²?','Evaluate at −3.', ['(−3)²=9.','The claimed output is 9.','The point satisfies the equation.'],'Yes.','Negative x is permitted.'],
 ['Reject point','Is (2,5) on y=x²?','Compare output.', ['At x=2 the rule gives 4.','Candidate y is 5.','The point is not on the graph.'],'No.','5≠4.']
],[
 ['Joining x² table points into a straight line.','The rate is not constant.','Use a smooth curve matching the rule.'],
 ['Swapping x and y when plotting.','It changes the graph.','Label axes before each point.']
],['Equation, table and graph encode the same function.','Functions need not be linear.','Use exact substitution to check plotted points.']),
lesson(5,'More Graphs of Functions',[
 'Goal: interpret values, change and meaningful coordinates from an existing function graph rather than assuming every graph is linear.',
 'A graph with time on its horizontal axis and distance on its vertical axis can rise, remain flat or fall. Interpret the coordinates and rates in context; a line segment rising twice as fast has twice the slope when the scales are identical.',
 'Compare a journey: start at 0 km, reach 6 km at 2 hours, stay until 3 hours, then return to 0 km by 5 hours. Teacher reviews the graph and story correspondence.'
],[
 ['Graph reading','A point (t,d) gives output d at input t.','Read numeric axis scales.','At (2,6), distance is 6 km after 2 h.'],
 ['Flat interval','Constant output over a range.','Slope zero shows no change in measured quantity.','Distance stays 6 km from t=2 to t=3.'],
 ['Decreasing interval','Output falls as input increases.','Slope is negative.','From (3,6) to (5,0), slope is −3 km/h.']
],[
 ['Rate outward','Distance rises from 0 to 6 km in 2 hours. Average rate?','Divide distance change by time.', ['Δd=6 km.','Δt=2 h.','Rate 6/2=3 km/h.'],'3 km/h.','A constant straight segment has that slope.'],
 ['Stop duration','Distance stays 6 km between t=2 and t=3. How long is the stop?','Subtract endpoints.', ['Starting stop time is 2.','Ending stop time is 3.','Duration 3−2=1 hour.'],'1 hour.','Graph is horizontal during the stop.'],
 ['Return rate','Distance decreases from 6 to 0 in 2 hours. Slope?','Compute signed rate.', ['Change in distance is 0−6=−6.','Time change is 5−3=2.','Slope is −6/2=−3 km/h.'],'−3 km/h.','Negative slope means returning toward the start, not physically negative speed.']
],[
 ['Confusing decreasing distance from start with negative travel speed.','Slope sign describes distance-from-start change, not necessarily speed sign.','Describe the measured variable.'],
 ['Ignoring a flat interval.','Flat distance means unchanged position with respect to start in this model.','Identify interval endpoints.']
],['Read axes and coordinates in context.','Horizontal segments show constant output.','Signed slope describes direction of output change.']),
lesson(6,'Even More Graphs of Functions',[
 'Goal: sketch a function graph from a verbal story while identifying where output increases, decreases or remains constant.',
 'A story specifies key time events, initial and final values, and possible rates. Put these anchor points on axes before joining them. A sketch may only be qualitative if no exact numerical rates are given.',
 'Draw an original tank-fill story: 0 L at minute 0, 12 L at minute 3, a pause to minute 5, then 20 L at minute 7. Explain the graph segments and why a teacher must evaluate the written interpretation.'
],[
 ['Anchor point','A known time-output pair in a story.','Place on labeled axes.','(3,12) means 12 L after 3 minutes.'],
 ['Piecewise rate','Slope can change between time intervals.','Compute rise/run separately per segment.','First fill rate 12/3=4 L/min.'],
 ['Pause','An interval with constant output.','Horizontal graph segment.','Volume remains 12 L from minute 3 to 5.']
],[
 ['First fill rate','Tank from 0 to 12 L over minutes 0 to 3. What rate?','Compute segment slope.', ['Rise is 12 L.','Run is 3 min.','Rate 12/3=4 L/min.'],'4 L/min.','The first segment joins (0,0) and (3,12).'],
 ['Pause rate','Volume stays 12 L from t=3 to t=5. What rate?','Compute signed change.', ['ΔV=12−12=0.','Δt=5−3=2.','Rate 0/2=0.'],'0 L/min.','This segment is horizontal.'],
 ['Final fill','Volume increases 12 to 20 L between t=5 and 7. Rate?','Compute rise/run.', ['Change is 20−12=8 L.','Time is 7−5=2 minutes.','Rate 8/2=4 L/min.'],'4 L/min.','The last segment has the same slope as the first.']
],[
 ['Joining all event points with one straight line.','It erases the pause.','Connect segments respecting story events.'],
 ['Not labeling time and volume units.','An unlabeled graph is ambiguous.','Label both axes before plotting.']
],['Use story events as graph anchors.','Describe each rate separately.','Some sketches need teacher-reviewed qualitative reasoning.']),
lesson(7,'Connecting Representations of Functions',[
 'Goal: connect a function’s story, table, graph and algebraic rule, with consistent inputs, outputs and units across all representations.',
 'Given a constant fee of $5 plus $2 per ticket, construct f(n)=5+2n. A table for n=0,1,2,3 produces $5,$7,$9,$11, and a graph shows these discrete points for whole-number ticket counts.',
 'Compare the plotted points with a continuous line and explain why fractional tickets are not physically meaningful. Teacher review checks the correspondence and restrictions.'
],[
 ['Story-to-rule','Translate fixed and per-unit contributions.','f(n)=b+mn.','Fee 5 + 2 per ticket yields f(n)=5+2n.'],
 ['Rule-to-table','Evaluate permitted inputs.','Compute each value.','n=2 gives f(2)=9.'],
 ['Discrete domain','Inputs count indivisible items.','Use nonnegative integers.','Ticket counts 0,1,2,… rather than arbitrary decimals.']
],[
 ['Build a table','For f(n)=5+2n, find outputs for n=0,1,2,3.','Substitute every count.', ['f(0)=5 and f(1)=7.','f(2)=9.','f(3)=11.'],'5, 7, 9, 11 dollars.','Successive outputs rise by $2.'],
 ['Find counted tickets','Total $15 under f(n)=5+2n. How many?','Solve for n.', ['15=5+2n.','10=2n.','n=5 tickets.'],'5.','5+2×5=15.'],
 ['Reject fraction-ticket point','Does n=2.5 represent a feasible ticket count?','Distinguish mathematical and practical domain.', ['The algebra yields f(2.5)=10.','Tickets are indivisible in this model.','2.5 is outside the intended input domain.'],'No, not a valid ticket count.','Discrete input domain is nonnegative integers.']
],[
 ['Drawing a continuous solid line as though half-tickets are real.','The story has discrete inputs.','Plot separate allowed points.'],
 ['Forgetting fixed cost at zero input.','Intercept is nonzero.','Check f(0).']
],['Keep all representations consistent.','Interpret intercept and per-unit change.','Match the input domain to the story.']),
lesson(8,'Linear Functions',[
 'Goal: identify linear functions from constant rates of change and distinguish them from functions whose differences vary.',
 'For equally spaced x-values, a linear function produces equal differences in f(x); a quadratic function generally does not. Interpret slope and intercept from a table, graph or equation.',
 'Compare two input-output tables: f(x)=3x+2 and g(x)=x². Write evidence based on differences. Teacher reviews the explanation and the selected representative points.'
],[
 ['Constant first difference','Equal input steps yield equal output changes.','A hallmark of linear functions.','f(x)=3x+2 increases by 3 per x unit.'],
 ['Linear equation','Function represented by y=mx+b.','m is constant slope.','y=3x+2.'],
 ['Nonlinear counterexample','First differences vary for equal input increments.','Not a linear function.','x² at x=0,1,2,3 gives 0,1,4,9.']
],[
 ['Linear table','For f(x)=3x+2, list f(0),f(1),f(2),f(3).','Evaluate.', ['f(0)=2, f(1)=5.','f(2)=8.','f(3)=11, giving differences of 3.'],'2, 5, 8, 11.','Every 1-unit input increase adds 3.'],
 ['Quadratic table','List g(x)=x² for x=0,1,2,3.','Square inputs.', ['0²=0 and 1²=1.','2²=4 and 3²=9.','Differences are 1,3,5, not constant.'],'0, 1, 4, 9; not linear.','First differences vary.'],
 ['Find intercept','A linear function has slope 3 and f(2)=8. Find b.','Substitute into y=mx+b.', ['Write 8=3×2+b.','8=6+b.','b=2.'],'2.','f(x)=3x+2.']
],[
 ['Calling a curved function linear because it rises.','Increasing alone does not imply constant rate.','Check equal input increments.'],
 ['Computing differences across unequal input steps without normalizing.','Slope is change per input unit.','Divide output difference by input difference.']
],['Constant rate gives linear behavior.','Slope and intercept describe line functions.','First differences detect nonlinear tables.']),
lesson(9,'Linear Models',[
 'Goal: construct a useful linear model for a real-world situation and distinguish its prediction from a guaranteed observed value.',
 'Real data often vary. A line may approximate measurements even if the points do not lie exactly on it. State input/output units, select a sensible rate and starting value, and discuss where the model might fail.',
 'Use independent temperature readings (0 min,20°C), (10 min,24°C), (20 min,28°C) to build T(t)=20+0.4t. Ask whether the model stays valid for many hours. A teacher judges model limitations.'
],[
 ['Linear model','A constant-rate approximation or rule for related quantities.','y≈mx+b for observed data, exact only under the stated assumption.','T=20+0.4t °C.'],
 ['Rate with units','Change in output per unit input.','m=Δy/Δx.','0.4°C per minute.'],
 ['Extrapolation','Prediction outside observed input range.','Treat cautiously; behavior may change.','Predicting temperature at 3 hours from 20 minutes of data is uncertain.']
],[
 ['Compute warming rate','Temperature rises from 20°C to 24°C in 10 min. Find rate.','Divide differences.', ['Change is 4°C.','Time change is 10 min.','Rate 4/10=0.4°C/min.'],'0.4°C/min.','At 20 min the model predicts 28°C.'],
 ['Predict within range','T=20+0.4t. Find T(15).','Substitute 15 minutes.', ['0.4×15=6.','Add initial 20.','T(15)=26°C.'],'26°C.','Between known readings 24 and 28.'],
 ['Evaluate far prediction','Model at t=180 minutes gives what result, and is it certain?','Compute, then check evidence range.', ['0.4×180=72.','Add 20 to obtain 92°C.','The reading is far outside the 0–20 min evidence range and may not be realistic.'],'92°C as a mathematical extrapolation, not a verified temperature.','Model assumptions need independent support.']
],[
 ['Treating a fitted line as an exact law for all times.','A model can fail outside its data range.','State the domain of evidence.'],
 ['Forgetting units in slope.','Rate is a ratio of measured quantities.','Label °C/min.']
],['Linear models predict with assumptions.','Rate and intercept carry units.','Extrapolation needs caution.']),
lesson(10,'Piecewise Linear Functions',[
 'Goal: interpret and construct a function built from linear segments with different rates, verifying which rule applies on each input interval.',
 'A piecewise function may switch from increasing to constant or decreasing at a boundary. Clear interval endpoints prevent contradictory outputs at the same input.',
 'Model a parking fee: $3 for up to one hour, then $3 plus $2 per additional whole hour. Compare with a continuous piecewise rule if fractional hours are permitted. Teacher review checks the selected billing policy and graph step/segment choice.'
],[
 ['Piecewise rule','Different formulas on specified input ranges.','Every allowed input must have one output.','f(t)=2t for 0≤t≤2, f(t)=4 for 2<t≤4.'],
 ['Boundary ownership','Which branch applies at a break point.','Use ≤ and < consistently.','At t=2 the first rule gives 4.'],
 ['Segment slope','Rate can differ by interval.','Compute each branch slope separately.','First branch slope 2, second slope 0.']
],[
 ['Evaluate first branch','f(t)=2t for 0≤t≤2; f(t)=4 for 2<t≤4. Find f(1).','Select interval.', ['Input 1 lies in 0≤t≤2.','Use f(t)=2t.','f(1)=2.'],'2.','The other branch does not apply at t=1.'],
 ['Evaluate boundary','Find f(2).','Read endpoint condition.', ['t=2 is included in first branch.','Compute 2×2=4.','The second branch excludes t=2 but would agree at boundary.'],'4.','Single-valued definition is maintained.'],
 ['Evaluate later interval','Find f(3.5).','Select second branch.', ['3.5 lies between 2 and 4.','Use constant rule f=4.','Output remains 4.'],'4.','Graph is flat from after t=2 to t=4.']
],[
 ['Applying one formula outside its interval.','The piecewise definition assigns different rules to different domains.','Check input interval before substitution.'],
 ['Assigning two contradictory boundary outputs.','A function cannot produce two values for one input.','Specify boundary inclusions.']
],['Piecewise functions can change slope.','Check endpoint ownership.','Interpret each segment within its interval.'])
];
if(rows.length!==10||new Set(rows.map(x=>x.LessonCode)).size!==10)throw Error('Bad U05 L01-L10 count');
for(const x of rows)if(x.ExplanationParagraphs.length<2||x.KeyConcepts.length<3||x.WorkedExamples.length<3||x.CommonMistakes.length<2||x.SummaryPoints.length<3)throw Error('Invalid '+x.LessonCode);
const file=path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u05-l01-l10-2017.rich-lesson-v2.json');
fs.writeFileSync(file,JSON.stringify({SchemaVersion:1,ContentVersion:'us-ccss-g8-u05-l01-l10-first-party-rich-v2-20261029-v1',PackCode:'US-CCSS-MATH',Lessons:rows},null,2)+'\n');
console.log('G8_U05_FUNCTIONS '+JSON.stringify({count:rows.length,examples:rows.reduce((a,b)=>a+b.WorkedExamples.length,0),file}));
