'use strict';
const fs=require('node:fs'),path=require('node:path'),root=path.resolve(__dirname,'../..');
const lesson=(n,title,body,concepts,examples,mistakes,summary)=>({
 LessonCode:'PED:US-CCSS-MATH:G8:U04:L'+String(n).padStart(2,'0'),
 CultureCode:'en',Title:title,
 ExplanationParagraphs:[...body,'Independently authored Edulytiks Grade 8 content informed by Open Up Resources / Illustrative Mathematics 2017–2019, CC BY 4.0. Source: https://im.kendallhunt.com/MS/teachers/3/4/'+n+'/preparation.html . No original publisher figures or assessments are duplicated.'],
 KeyConcepts:concepts.map(([Title,Definition,Rule,Example])=>({Title,Definition,Rule,Example})),
 WorkedExamples:examples.map(([Title,Question,Method,Steps,Answer,Check])=>({Title,Question,Method,Steps,Answer,Check})),
 CommonMistakes:mistakes.map(([Mistake,WhyWrong,Correction])=>({Mistake,WhyWrong,Correction})),
 SummaryPoints:summary,Visuals:[],Videos:[]
});
const rows=[
lesson(1,'Number Puzzles',[
 'Goal: interpret a verbal number puzzle as an equation, define an unknown and test a solution by substitution.',
 'Translate each clue into a relationship; a phrase such as “twice a number plus three equals fifteen” means 2x+3=15, while “twice the sum of a number and three” would mean 2(x+3). Parentheses reflect the meaning.',
 'Create two original number puzzles for classmates, exchange them, and check that each has a consistent solution. A teacher reviews the language-to-algebra translation and ambiguity, not just a numeric answer.'
],[
 ['Unknown number','A variable standing for an unspecified value.','Write one equation consistent with every clue.','If x is the number, twice it plus 3 is 2x+3.'],
 ['Equation','A statement of equality with mathematical expressions.','Only a satisfying value is a solution.','2x+3=15.'],
 ['Verification','Substituting the proposed solution into the original puzzle.','Check both sides match.','With x=6, 2(6)+3=15.']
],[
 ['Solve a puzzle','Twice a number plus three is fifteen. Find it.','Let x be the number.', ['Write 2x+3=15.','Subtract three: 2x=12.','Divide two: x=6.'],'6.','2×6+3=15.'],
 ['Different parentheses','Twice the sum of a number and three is sixteen. Find it.','Distinguish 2(x+3) from 2x+3.', ['Write 2(x+3)=16.','Divide two: x+3=8.','Subtract three: x=5.'],'5.','2(5+3)=16.'],
 ['Check a wrong guess','For 3x−2=13, does x=4 work?','Substitute into the original.', ['3×4−2=10.','Right side is 13.','10 differs from 13, so x=4 is not a solution.'],'No; the correct number is 5.','3×5−2=13.']
],[
 ['Missing parentheses in a phrase about a sum.','2x+3 is not the same as 2(x+3).','Parse grouping words first.'],
 ['Reporting a guess without checking.','An equation solution must satisfy the original equality.','Substitute the value.']
],['Define the unknown first.','Translate groups and operations exactly.','Verify a solution with substitution.']),
lesson(2,'Keeping the Equation Balanced',[
 'Goal: use a balanced hanger model to represent an equation and find unknown weights without disrupting equality.',
 'Equal hanger arms carry equal total weight. If one object x appears on each arm, remove an equal quantity from both sides. When identical unknown bags occur together, dividing equally across bags determines the weight per bag.',
 'Make an original picture with two identical bags and a three-unit weight balancing an eleven-unit weight. Interpret it as 2x+3=11. A teacher evaluates diagram alignment and explanation; one numerical result does not verify the physical model.'
],[
 ['Balanced model','Two equal-weight sides of a hanger represent equal expressions.','Apply the same permissible change to both sides.','2x+3=11.'],
 ['Remove equal weights','Subtract the same quantity from each side.','Equality remains valid after subtracting c from both.','2x+3−3=11−3.'],
 ['Share the remaining weight','Divide equal total weights across identical bags.','2x=8 implies x=4.','Two 4-unit bags balance 8 units.']
],[
 ['Two-bag hanger','Two equal bags plus three units balance eleven. Find each bag.','Model 2x+3=11.', ['Subtract three units from both hanger arms.','Two bags balance eight units.','Each bag weighs 8/2=4 units.'],'4 units per bag.','2×4+3=11.'],
 ['Three-bag hanger','Three bags balance fifteen units. Find one bag.','Divide both sides equally.', ['Write 3x=15.','Divide both sides by 3.','x=5.'],'5 units.','3×5=15.'],
 ['Reject asymmetric removal','If x+7=12, is x+7−7=12−3 a valid balance move?','Compare the operations on both sides.', ['Seven is removed from the left.','Three is removed from the right.','Different changes break equivalence.'],'No.','Subtract 7 from both sides instead.']
],[
 ['Removing weight from only one arm.','It destroys the equality of total weights.','Make each operation on both sides.'],
 ['Assuming bags can have arbitrary weights on one hanger.','Identically marked bags represent equal unknown weights.','Use one variable consistently.']
],['Balanced hangers encode equations.','Maintain identical operations on both sides.','Verify bag weights by recomputing totals.']),
lesson(3,'Balanced Moves',[
 'Goal: rewrite equations with equivalent expressions while preserving exactly the same solution set.',
 'Adding, subtracting, multiplying by a nonzero value or dividing by a nonzero value on both sides preserves equality. Distributing and combining like terms preserve an expression’s value.',
 'Write two legal chains from 3x+5=20 to x=5. Exchange with a classmate and identify each operation. Written descriptions of equivalence need teacher judgment; a correct final answer alone is not full evidence.'
],[
 ['Equivalent equations','Equations with identical solution sets.','Apply reversible operations to both sides.','3x+5=20 and 3x=15 have the same x.'],
 ['Balance operations','The same addition/subtraction or nonzero scaling on both sides.','Keep both expressions equal at every step.','Subtract 5 from both sides.'],
 ['Identity substitution check','A proposed answer must satisfy original and transformed equations.','Substitute in both.','x=5 gives 3×5+5=20.']
],[
 ['Subtract then divide','Solve 3x+5=20.','Reverse adding 5 then multiplying 3.', ['Subtract 5 on both sides: 3x=15.','Divide both by 3.','x=5.'],'x=5.','3×5+5=20.'],
 ['Distribute legally','Rewrite 2(x+4)=18 and solve.','Distribute or divide equivalently.', ['Expand to 2x+8=18.','Subtract 8: 2x=10.','Divide by 2: x=5.'],'x=5.','2(5+4)=18.'],
 ['Reject one-sided change','A learner changes x+4=9 to x=9. Is this equivalent?','Test the missing subtraction.', ['Original equation requires x=5.','The altered equation proposes x=9.','The two equations have different solutions.'],'No.','Subtract four on both sides to get x=5.']
],[
 ['Cancelling a term from only one side.','It changes the solution set.','Write the balancing operation explicitly.'],
 ['Dividing by zero.','That is not a valid reversible operation.','Only divide by a demonstrated nonzero number.']
],['Equivalent equations preserve solutions.','Reversible balanced steps are justified.','Check transformed and original equalities.']),
lesson(4,'More Balanced Moves',[
 'Goal: use the distributive property, combining like terms and balanced operations to simplify equations with expressions on both sides.',
 'When expressions occur on both sides, collect variable terms on one side and constants on the other. A move such as subtracting 2x from both sides is legal; dividing by a variable expression may not be reversible if that expression can equal zero.',
 'Construct a diagram showing equal expressions 5x+2 and 2x+14. Explain how removing two x-tiles from both sides isolates the remaining three. A teacher reviews the algebraic diagram and validity of each step.'
],[
 ['Combine like terms','Merge coefficients of the same variable.','3x+2x=5x, not 5x².','x+4x=5x.'],
 ['Variables on both sides','Keep equality while moving unknown terms.','Subtract a variable term from both sides.','5x+2=2x+14 becomes 3x+2=14.'],
 ['Distribution','Multiply each term inside parentheses.','a(b+c)=ab+ac.','3(x−2)=3x−6.']
],[
 ['Solve both sides','Solve 5x+2=2x+14.','Eliminate smaller x term.', ['Subtract 2x from both sides: 3x+2=14.','Subtract 2: 3x=12.','Divide by 3: x=4.'],'x=4.','5×4+2=22 and 2×4+14=22.'],
 ['Distribute and solve','Solve 3(x−2)=15.','Expand then balance.', ['Distribute: 3x−6=15.','Add 6: 3x=21.','Divide 3: x=7.'],'x=7.','3(7−2)=15.'],
 ['Combine and solve','Solve 2x+3x−4=16.','Combine like terms.', ['2x+3x=5x.','Then 5x−4=16 gives 5x=20.','Divide by 5: x=4.'],'x=4.','2×4+3×4−4=16.']
],[
 ['Treating x+x as x².','Repeated variable addition is coefficient addition.','Write x+x=2x.'],
 ['Distributing to only the first term.','Every parenthesized term is multiplied.','Use a visual area model for each term.']
],['Combine like terms accurately.','Balance both sides when moving terms.','Check the original equation after solving.']),
lesson(5,'Solving Any Linear Equation',[
 'Goal: use reliable symbolic strategies for one-variable affine equations with fractions or variables on both sides while recognizing exceptional solution counts.',
 'A linear equation ax+b=cx+d can have one solution, no solution, or infinitely many solutions depending on the resulting coefficients. Operations must preserve its solution set, including multiplying by the common denominator when fractional coefficients occur.',
 'Work a complete explanation rather than only a final numeric answer. A teacher assesses strategy clarity; numeric solutions can be independently checked against the original.'
],[
 ['Affine equation','Variables appear to first degree with constant coefficients.','Collect all x terms and constant terms.','4x+3=2x+11.'],
 ['Unique solution','After simplification, nonzero multiple of x equals a number.','ax=b with a≠0 gives x=b/a.','2x=8 gives x=4.'],
 ['Fraction clearing','Multiply both sides by a common nonzero denominator.','Preserves solutions when done on both sides.','x/3+2=5 becomes x+6=15.']
],[
 ['Solve variables on both sides','Solve 4x+3=2x+11.','Balance variable and constant terms.', ['Subtract 2x: 2x+3=11.','Subtract 3: 2x=8.','Divide 2: x=4.'],'x=4.','4×4+3=19 and 2×4+11=19.'],
 ['Solve fractional coefficient','Solve x/3+2=5.','Clear denominator.', ['Subtract 2: x/3=3.','Multiply both sides by 3.','x=9.'],'x=9.','9/3+2=5.'],
 ['Solve negative coefficient','Solve −2x+7=−1.','Isolate −2x.', ['Subtract 7: −2x=−8.','Divide by −2.','x=4.'],'x=4.','−8+7=−1.']
],[
 ['Dropping negative signs when dividing.','Sign determines solution direction.','Track signed coefficients.'],
 ['Clearing only one fraction term.','Multiplication applies to the entire equation.','Multiply both sides and all terms consistently.']
],['Collect variable terms and constants.','Fractions and negative coefficients require careful balance.','Verification against the original guards mistakes.']),
lesson(6,'Strategic Solving',[
 'Goal: choose an efficient order of legal operations to solve a linear equation without unnecessary expansion or arithmetic errors.',
 'An equation with a factored expression can often be simplified by dividing first; another may benefit from distributing. Compare the number of steps and the potential for sign mistakes, then verify both strategies lead to the same answer.',
 'Give students 4(x+3)=28 and ask them to solve in two ways, explain what is more economical, and critique a common distribution mistake. Teacher evaluation focuses on mathematical explanation rather than speed alone.'
],[
 ['Factor-first strategy','Isolate a product before expanding.','If a(x+b)=c and a≠0, divide by a.','4(x+3)=28 becomes x+3=7.'],
 ['Distribution strategy','Expand products when it helps collect terms.','a(x+b)=ax+ab.','4x+12=28.'],
 ['Efficiency check','Compare correct solution paths while preserving equivalence.','Choose the simpler legal steps.','Dividing 4 first avoids multiplying out.']
],[
 ['Divide first','Solve 4(x+3)=28 using a direct strategy.','Divide by 4.', ['Divide each side by 4: x+3=7.','Subtract 3.','x=4.'],'x=4.','4(4+3)=28.'],
 ['Expand instead','Solve 4(x+3)=28 by distributing.','Expand and balance.', ['Write 4x+12=28.','Subtract 12: 4x=16.','Divide by 4: x=4.'],'x=4.','Same answer as factor-first solution.'],
 ['Choose smart cancellation','Solve (5x−10)/5=6.','Use fraction simplification.', ['Multiply both sides by 5: 5x−10=30.','Add 10: 5x=40.','Divide by 5: x=8.'],'x=8.','(5×8−10)/5=30/5=6.']
],[
 ['Rewarding a shortcut without justification.','Skipping necessary equal operations can conceal errors.','State why the shortcut preserves equality.'],
 ['Choosing distribution and losing a sign.','Every term inside must be multiplied.','Check with reverse substitution.']
],['Multiple equivalent solution paths are valid.','Choose steps that reduce errors.','Explain and verify the chosen strategy.']),
lesson(7,'All, Some, or No Solutions',[
 'Goal: distinguish equations true for all real x, true for exactly one x, and true for no x using equivalent transformations.',
 'If simplifying yields 0=0, every x satisfies the original; if it yields 0=nonzero, none does. If it yields ax=b with a≠0, exactly one solution exists.',
 'Sort original independently written equations into these three cases. A teacher reviews the explanation of why a statement is always or never true; test substitutions alone are insufficient to prove an identity.'
],[
 ['Identity','An equation true for every real input.','Equivalent simplification yields 0=0.','2(x+3)=2x+6.'],
 ['Contradiction','An equation false for every real input.','Equivalent simplification yields 0=c with c≠0.','2x+1=2x+5.'],
 ['Unique solution','A linear equation determining one x.','Simplifies to ax=b with a≠0.','2x+1=9 gives x=4.']
],[
 ['Identity test','How many solutions does 2(x+3)=2x+6 have?','Expand and compare.', ['Distribute the left side to 2x+6.','Both sides are identical.','Subtract to obtain 0=0, true for all real x.'],'Infinitely many solutions (all real numbers).','Any number makes both sides equal.'],
 ['Contradiction test','How many solutions does 2x+1=2x+5 have?','Subtract matching terms.', ['Subtract 2x from both sides.','The result is 1=5, impossible.','No x repairs this contradiction.'],'No solutions.','Both sides differ by 4 for every x.'],
 ['Unique case','How many solutions does 2x+1=9 have?','Isolate x.', ['Subtract 1: 2x=8.','Divide by 2: x=4.','Nonzero coefficient guarantees one real value.'],'Exactly one: x=4.','2×4+1=9.']
],[
 ['Assuming all linear equations have one solution.','The variable can cancel, leaving an identity or contradiction.','Inspect the simplified final statement.'],
 ['Using a few sample values to prove all real x.','Finite tests cannot prove an identity.','Show equivalent algebraic simplification.']
],['Equation solution counts can be none, one, or infinitely many.','0=0 signals an identity.','0=nonzero signals contradiction.']),
lesson(8,'How Many Solutions?',[
 'Goal: classify and explain the number of solutions of unfamiliar linear equations, including those with fractions and distribution.',
 'Multiply or expand carefully, then inspect whether any nonzero x term remains. Two superficially different expressions can be identical after expansion. A different constant after cancellation indicates no solution.',
 'Sort and justify three equations drawn from separately prepared cards. A teacher evaluates equivalent-transformation arguments rather than mere labels.'
],[
 ['Coefficient comparison','Compare variable coefficients after simplification.','Equal coefficients cancel; compare constants next.','3x+2=3x+7 has none.'],
 ['Identity detection','Every x works when expressions simplify identically.','Both coefficient and constant match.','2(x+4)=2x+8.'],
 ['Unique detection','Remaining nonzero variable coefficient produces one solution.','ax=b, a≠0.','3x−2=10 gives x=4.']
],[
 ['Identity by distribution','Solutions to 2(x+4)=2x+8?','Expand.', ['Distribute left to 2x+8.','Subtract 2x and 8 from both sides.','The statement is 0=0.'],'All real x.','The expressions agree identically.'],
 ['Contradiction with fractions','Solutions to x/2+1=x/2+3?','Subtract common variable term.', ['Subtract x/2 from both sides.','Obtain 1=3.','No real x can satisfy this.'],'No solutions.','The right side always exceeds the left by 2.'],
 ['Unique solution','Solve 3x−2=10 and classify count.','Isolate x.', ['Add 2: 3x=12.','Divide by 3.','x=4, a unique real value.'],'One solution: x=4.','3×4−2=10.']
],[
 ['Stopping when variables cancel and saying “x=0.”','A variable-free statement must be checked.','Distinguish 0=0 from contradiction.'],
 ['Expanding coefficients incorrectly.','Incorrect distribution changes solution count.','Expand term by term.']
],['Solution count is an algebraic property.','Inspect the simplified constant statement.','A consistent coefficient comparison prevents guessing.']),
lesson(9,'When Are They the Same?',[
 'Goal: model when two quantities from different linear rules are equal and interpret the equation’s solution in a realistic context.',
 'For two subscription plans, matching costs are found by setting the rules equal, not by comparing one arbitrary sample. The equality point can be shown on a graph as a line intersection.',
 'Compare plan A at $6 plus $3 per month and plan B at $18 plus $1 per month. A teacher reviews what the break-even value means and whether fractional months are meaningful under the pricing rules.'
],[
 ['Break-even equation','An equation formed by setting two models equal.','A(x)=B(x).','6+3m=18+m.'],
 ['Intersection meaning','Input where both outputs agree.','Same point lies on both graphs.','m=6 months, cost $24.'],
 ['Context domain','Only input values supported by the problem.','Months may be nonnegative whole counts.','m=6 is feasible.']
],[
 ['Find equal costs','Plan A is 6+3m and B is 18+m. When are they equal?','Solve the equality.', ['6+3m=18+m.','Subtract m and 6: 2m=12.','m=6.'],'At 6 months.','Both plans cost 24.'],
 ['Check a short period','Which plan costs less after 2 months?','Evaluate both.', ['A=6+3×2=12.','B=18+2=20.','A is cheaper by 8.'],'Plan A by $8.','A(2)<B(2).'],
 ['Check a later period','Which plan costs less after 10 months?','Evaluate both.', ['A=6+30=36.','B=18+10=28.','B is cheaper by 8.'],'Plan B by $8.','After break-even, smaller monthly rate wins.']
],[
 ['Equating only initial fees.','Equal starting charges are not the same as equal total charges.','Include usage-dependent terms.'],
 ['Ignoring feasible time intervals.','Negative months are not meaningful for this contract.','State m≥0.']
],['Two linear models are equal at a break-even input.','The equality appears as a graph intersection.','Interpret the solution in its domain.']),
lesson(10,'On or Off the Line?',[
 'Goal: interpret ordered pairs as solutions or nonsolutions of a contextual linear equation, and explain which points lie on its graph.',
 'A point is on a line if its coordinates satisfy the equation exactly. A plotted point that merely looks close is not an exact solution. Negative values may be algebraically valid but contextually inappropriate.',
 'Investigate a taxi fare F=4+2d. Sort points (0,4), (3,10), (3,11) by substitution and explain which are valid fare-distance combinations. A teacher checks graph reading and context.'
],[
 ['On-line test','Substitution makes the defining equation true.','Compare y with f(x).','(3,10) lies on F=4+2d.'],
 ['Off-line test','The ordered pair fails the equation.','An approximate graph dot is not enough.','(3,11) does not satisfy F=4+2d.'],
 ['Context validity','The point must also make sense in the situation.','Distance cannot be negative.','(−2,0) satisfies equation but is not a realistic trip.']
],[
 ['Test valid fare','Is (3,10) on F=4+2d?','Substitute d=3.', ['Predicted fare is 4+2×3=10.','The point’s fare is 10.','Both values agree.'],'Yes.','10=4+2(3).'],
 ['Reject an off-line point','Is (3,11) on F=4+2d?','Substitute.', ['Predicted fare at d=3 is 10.','The point has F=11.','It does not satisfy the equation.'],'No.','11≠10.'],
 ['Algebra versus context','Does (−2,0) satisfy F=4+2d? Is it realistic?','Separate equation check from physical interpretation.', ['At d=−2, F=4−4=0.','The equation is satisfied algebraically.','A trip length cannot be negative, so the point is contextually invalid.'],'Algebraically yes; realistically no.','State d≥0 for taxi distance.']
],[
 ['Judging graph membership by closeness of a hand drawing.','The graph is an exact solution set.','Substitute.'],
 ['Conflating algebraic and context restrictions.','A solution may violate domain assumptions.','State both checks separately.']
],['Line points exactly satisfy the equation.','Nonmembers fail substitution.','Context may further restrict allowed solutions.']),
lesson(11,'On Both of the Lines',[
 'Goal: identify an ordered pair that satisfies two line equations simultaneously and interpret this as the intersection of their graphs.',
 'Two independent equations yield an intersection only where both balances hold. Testing a point in one line is insufficient; substitute into both.',
 'Plot y=x+1 and y=−x+5, identify the intersection and explain its meaning in terms of equal outputs. The teacher reviews the coordinate graph and line intersection.'
],[
 ['Common solution','One ordered pair satisfying both linear equations.','Verify each equation independently.','(2,3) satisfies y=x+1 and y=−x+5.'],
 ['Intersection','A coordinate shared by two graph lines.','Solve equal y expressions.','x+1=−x+5 gives x=2.'],
 ['Verification','A pair is common only if both substitutions are true.','Check both equalities.','3=2+1 and 3=−2+5.']
],[
 ['Find intersection','Solve y=x+1 and y=−x+5.','Set right sides equal.', ['x+1=−x+5.','2x=4 gives x=2.','Then y=2+1=3.'],'(2,3).','Both lines produce 3 at x=2.'],
 ['Reject one-line point','Does (0,1) lie on both y=x+1 and y=−x+5?','Check both equations.', ['First: y=0+1=1, so yes.','Second: y=−0+5=5, not 1.','It lies on only one.'],'No.','One satisfied line does not make a common solution.'],
 ['Check a candidate','Does (2,3) satisfy both equations?','Substitute.', ['First gives 2+1=3.','Second gives −2+5=3.','Both outputs match.'],'Yes.','The two lines meet there.']
],[
 ['Checking only one equation.','The system requires simultaneous truth.','Substitute into both.'],
 ['Assuming plotted lines meet exactly at a fuzzy grid point.','Hand plotting may be imprecise.','Verify algebraically.']
],['A shared solution lies on both lines.','Intersections can be computed by equal outputs.','Verify both equations.']),
lesson(12,'Systems of Equations',[
 'Goal: understand a system of two linear equations as simultaneous constraints and classify possible intersections.',
 'Two distinct nonparallel lines intersect once, parallel distinct lines have no common point, and coincident lines share infinitely many. A graph can illustrate these cases but exact equations establish which applies.',
 'Use an original graph of y=2x+1 and y=2x−3 to explain no intersection, then compare y=2x+1 and 2y=4x+2. A teacher checks structural and graphical reasoning.'
],[
 ['Linear system','Two or more equations required to hold simultaneously.','A solution satisfies every equation.','y=2x+1 and y=−x+4.'],
 ['Parallel distinct lines','Equal slope but different intercepts.','They have no common ordered pair.','y=2x+1 and y=2x−3.'],
 ['Coincident lines','Different equations defining the same line.','Infinitely many common points.','y=2x+1 and 2y=4x+2.']
],[
 ['No intersection','Solve y=2x+1 and y=2x−3.','Set outputs equal.', ['Equality would demand 2x+1=2x−3.','Subtract 2x to get 1=−3.','Contradiction means no common solution.'],'No solutions.','Distinct parallel lines have the same slope.'],
 ['Infinitely many','Solve y=2x+1 and 2y=4x+2.','Simplify one equation.', ['Divide second equation by 2: y=2x+1.','Both equations are identical.','Every point on that line satisfies both.'],'Infinitely many common solutions.','Two equations describe the same line.'],
 ['One intersection','Solve y=x and y=−x+4.','Equate outputs.', ['x=−x+4.','2x=4 gives x=2.','Then y=2.'],'(2,2).','Both lines contain (2,2).']
],[
 ['Treating a system as two separate unrelated answers.','The common solution must satisfy both.','Use simultaneous constraints.'],
 ['Claiming parallel lines always overlap.','Same slope with different intercepts gives distinct lines.','Compare intercepts.']
],['Systems encode simultaneous conditions.','Intersections can be zero, one or infinitely many.','Graph shape and algebra must agree.']),
lesson(13,'Solving Systems of Equations',[
 'Goal: solve two-variable linear systems using substitution and verify the ordered-pair solution in both equations.',
 'Choose substitution when one variable is already isolated. Replace that variable in the other equation, solve for the remaining variable, then back-substitute and check both originals.',
 'Solve y=2x+1 together with x+y=10. Discuss whether a plotted approximation and an exact algebraic answer agree. A teacher reviews the substitution sequence.'
],[
 ['Substitution','Replace an isolated variable by its equal expression.','If y=2x+1, use it in x+y=10.','x+(2x+1)=10.'],
 ['Back-substitution','Find the second coordinate once the first is known.','Insert solved x into either equation.','x=3 yields y=7.'],
 ['System check','Test the ordered pair in both equations.','Both must be true.','7=2×3+1 and 3+7=10.']
],[
 ['Solve by substitution','Solve y=2x+1, x+y=10.','Substitute y.', ['x+2x+1=10.','3x=9 gives x=3.','Then y=2×3+1=7.'],'(3,7).','Both equations are satisfied.'],
 ['Solve another system','Solve y=x−2 and x+y=8.','Substitute y into total.', ['x+(x−2)=8.','2x−2=8 gives x=5.','Then y=5−2=3.'],'(5,3).','5+3=8 and 3=5−2.'],
 ['Reject candidate','Is (4,6) a solution to y=2x+1, x+y=10?','Test both.', ['Second gives 4+6=10.','First predicts y=2×4+1=9.','Since 6≠9, the point fails the system.'],'No.','Meeting one equation is insufficient.']
],[
 ['Forgetting to distribute while substituting expressions.','Parentheses may change signs and coefficients.','Write the substituted expression clearly.'],
 ['Checking only the equation used last.','An algebra slip can still produce an inconsistent pair.','Substitute into both originals.']
],['Substitute an already isolated variable.','Back-substitute and obtain an ordered pair.','Check both equations.']),
lesson(14,'Solving More Systems',[
 'Goal: choose an efficient substitution or elimination strategy for linear systems, including oppositely signed variable coefficients.',
 'Elimination adds or subtracts equivalent equations to cancel one variable. You may multiply an entire equation by a nonzero constant first. An intersection must still be checked in the original system.',
 'Use x+y=7 and x−y=1. Compare elimination with substitution and explain their equivalent solutions. A teacher reviews the justification for adding whole equations.'
],[
 ['Elimination','Combine equations to remove one variable.','Add equations when coefficients are opposites.','(x+y)+(x−y)=2x.'],
 ['Equation scaling','Multiply all terms by a nonzero constant.','Preserves each equation’s solutions.','2x+y=7 may be doubled to 4x+2y=14.'],
 ['Common check','Verify the final pair in both original equations.','Do not trust an unchecked elimination result.','x=4,y=3 satisfies 7 and 1.']
],[
 ['Add equations','Solve x+y=7 and x−y=1.','Eliminate y.', ['Add equations: 2x=8.','x=4.','Then 4+y=7 gives y=3.'],'(4,3).','4+3=7 and 4−3=1.'],
 ['Subtract equations','Solve 2x+y=11 and 2x−y=5.','Add for elimination.', ['Adding gives 4x=16.','x=4.','Then y=11−8=3.'],'(4,3).','2×4−3=5.'],
 ['Use substitution','Solve x=2y and x+y=12.','Replace x.', ['2y+y=12.','3y=12 gives y=4.','Then x=2×4=8.'],'(8,4).','8=2×4 and 8+4=12.']
],[
 ['Adding only one term from each equation.','Elimination uses the entire left and right sides.','Write all terms aligned.'],
 ['Multiplying only one side during scaling.','It changes the equation.','Multiply every term on both sides.']
],['Elimination and substitution can agree.','Choose the simpler algebraic route.','Check the original equations.']),
lesson(15,'Writing Systems of Equations',[
 'Goal: translate a two-condition real-world situation into two equations with shared variables and clearly defined units.',
 'Define each unknown before composing equations. A total gives a sum equation; a price or rate condition gives a second independent equation. The pair solves only when both pieces of information hold.',
 'Invent a ticket scenario with adult tickets $8, child tickets $5, 10 tickets total and $65 total revenue. Write two equations and discuss physical restrictions to nonnegative integers. Teacher review is needed for variable definitions and consistency of the story.'
],[
 ['Total-count condition','A sum of quantities equals known count.','a+c=10.','Ten tickets total.'],
 ['Total-value condition','Multiply each count by its per-unit value.','8a+5c=65.','Adults cost 8, children cost 5.'],
 ['Discrete domain','Counts are nonnegative whole numbers.','a,c∈{0,1,2,…}.','Five adults and five children is possible.']
],[
 ['Write ticket system','Ten tickets total, adult $8 child $5, revenue $65. Define equations.','Use a and c as counts.', ['Count constraint is a+c=10.','Revenue constraint is 8a+5c=65.','Require nonnegative integer counts.'],'a+c=10; 8a+5c=65.','Both describe the same sale.'],
 ['Solve counts','Solve a+c=10 and 8a+5c=65.','Substitute c=10−a.', ['8a+5(10−a)=65.','8a+50−5a=65 gives 3a=15.','a=5 and c=5.'],'5 adults and 5 children.','40+25=65.'],
 ['Reject infeasible count','A proposed solution has 12 adults and −2 children but sums to 10. Is it acceptable?','Check domain before revenue.', ['The total condition 12+(−2)=10 holds.','Negative child ticket count is impossible.','The solution is invalid for the context.'],'No.','Ticket counts must be nonnegative integers.']
],[
 ['Using one variable for different quantities.','Counts need distinct definitions.','Define symbols before equations.'],
 ['Accepting a negative ticket count.','Algebraic solutions may violate realistic constraints.','Enforce nonnegative integers.']
],['Define variables and units.','Use independent count and value conditions.','Check both equations and practical domains.']),
lesson(16,'Solving Problems with Systems of Equations',[
 'Goal: solve contextual systems, compare feasible solutions, and interpret results including units and reasonableness.',
 'Set up both constraints, select a solution method, and validate the final pair against every condition. If a model permits fractions but the situation requires counts, only integer nonnegative solutions are feasible.',
 'Consider a mixture of apples and pears: eight fruits total, apples $2 each, pears $3 each, total cost $19. Determine both counts and explain why each condition matters. Teacher review checks context and reasoning.'
],[
 ['Independent constraints','Two equations simultaneously describing one situation.','Use both count and cost.','a+p=8 and 2a+3p=19.'],
 ['Eliminate a variable','Combine equations or substitute.','Use a=8−p.','2(8−p)+3p=19.'],
 ['Reasonableness','The pair must match equations and physical context.','Check nonnegative integer counts and amounts.','a=5,p=3.']
],[
 ['Fruit mixture','Eight fruits, apples $2, pears $3, total $19. Find counts.','Substitute the count equation.', ['a+p=8 gives a=8−p.','2(8−p)+3p=19 gives 16+p=19, so p=3.','Then a=8−3=5.'],'5 apples and 3 pears.','Count 8 and cost 10+9=19.'],
 ['Check alternate guess','Could 4 apples and 4 pears meet both conditions?','Substitute counts and prices.', ['Count 4+4=8 satisfies the first.','Cost is 2×4+3×4=8+12=20.','Since 20≠19, the guess fails.'],'No.','Only one equation holds.'],
 ['Solve a second cost system','Six items total, $3 and $5 types, total cost $24. Find quantities.','Set x+y=6 and 3x+5y=24.', ['x=6−y.','3(6−y)+5y=24 yields 18+2y=24, so y=3.','x=3.'],'3 of each type.','3×3+5×3=24 and count 6.']
],[
 ['Ignoring the total count after solving a cost equation.','One constraint alone leaves multiple possibilities.','Check both.'],
 ['Giving fractional item counts without context.','Discrete objects require integer solutions.','Apply domain restrictions.']
],['Systems combine independent real constraints.','Solve algebraically and validate context.','A complete answer states quantities and units.'])
];
if(rows.length!==16||new Set(rows.map(x=>x.LessonCode)).size!==16)throw Error('Unit 4 mismatch');
for(const l of rows)if(l.ExplanationParagraphs.length<2||l.KeyConcepts.length<3||l.WorkedExamples.length<3||l.CommonMistakes.length<2||l.SummaryPoints.length<3)throw Error('Invalid rich lesson '+l.LessonCode);
const output=path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u04-l01-l16-2017.rich-lesson-v2.json');
fs.writeFileSync(output,JSON.stringify({SchemaVersion:1,ContentVersion:'us-ccss-g8-u04-first-party-rich-v2-20261029-v1',PackCode:'US-CCSS-MATH',Lessons:rows},null,2)+'\n');
console.log('G8_U04 '+JSON.stringify({lessons:rows.length,examples:rows.reduce((a,x)=>a+x.WorkedExamples.length,0),output}));
