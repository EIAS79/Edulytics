'use strict';
const fs=require('node:fs'),path=require('node:path'),root=path.resolve(__dirname,'../..');
const lesson=(n,title,body,concepts,examples,errors,summary)=>({
 LessonCode:'PED:US-CCSS-MATH:G8:U07:L'+String(n).padStart(2,'0'),CultureCode:'en',Title:title,
 ExplanationParagraphs:[...body,
 'First-party Edulytiks adaptation informed by Open Up Resources / Illustrative Mathematics Grade 8 (2017–2019), CC BY 4.0. Exact source: https://im.kendallhunt.com/MS/teachers/3/7/'+n+'/preparation.html . No publisher illustrations or assessments are reproduced.'],
 KeyConcepts:concepts.map(([Title,Definition,Rule,Example])=>({Title,Definition,Rule,Example})),
 WorkedExamples:examples.map(([Title,Question,Method,Steps,Answer,Check])=>({Title,Question,Method,Steps,Answer,Check})),
 CommonMistakes:errors.map(([Mistake,WhyWrong,Correction])=>({Mistake,WhyWrong,Correction})),
 SummaryPoints:summary,Visuals:[],Videos:[]
});
const rows=[
lesson(1,'Exponent Review',[
 'Goal: review the meaning of whole-number exponents and distinguish exponentiation from repeated addition or multiplication of the exponent.',
 'For a nonzero base a, a³ means a×a×a, not 3a. Zero exponent is defined as a⁰=1 for nonzero a, because it preserves the quotient pattern; zero to the zeroth power is not covered by that rule.',
 'Represent powers using original square/cube diagrams and repeated-product notation. Teacher review checks whether the visual model correctly distinguishes base and exponent.'
],[
 ['Base','The number repeatedly multiplied.','In aⁿ, the base is a.','In 4³ the base is 4.'],
 ['Exponent','How many factors of the base are multiplied for positive integer n.','a³=a×a×a.','4³=64.'],
 ['Zero exponent','Any nonzero base to power zero equals one.','a⁰=1 for a≠0.','7⁰=1.']
],[
 ['Expand power','Compute 4³.','Write repeated factors.', ['4³=4×4×4.','4×4=16.','16×4=64.'],'64.','4³ is not 4×3=12.'],
 ['Square negative number','Find (−3)².','Respect parentheses.', ['Base is −3.','Multiply (−3)(−3).','Product is +9.'],'9.','The square of a real number is nonnegative.'],
 ['Zero power','Find 7⁰.','Use exponent rule for nonzero base.', ['7 is nonzero.','7⁰=7²/7² by consistent power pattern.','The quotient is 1.'],'1.','Do not generalize to 0⁰ without defining a convention.']
],[
 ['Multiplying base by exponent.','An exponent counts repeated factors, not addition.','Write an expanded product.'],
 ['Ignoring parentheses around negative base.','−3² means −(3²), unlike (−3)².','Identify what exactly is raised to the power.']
],['Exponentiation encodes repeated multiplication.','Parentheses define a negative base.','Nonzero bases to power zero equal one.']),
lesson(2,'Multiplying Powers of Ten',[
 'Goal: discover and use the product rule for powers of ten by combining repeated factors and representing place-value shifts.',
 'Multiplying 10ᵃ and 10ᵇ joins groups of factors of ten, giving 10ᵃ⁺ᵇ. The rule extends to signed integer exponents when a and b are integers, but this investigation first uses repeated products.',
 'Draw a place-value table for 10²×10³ and explain why the outcome is 10⁵. A teacher checks mathematical pattern reasoning and table alignment.'
],[
 ['Power product rule','Same-base powers multiply by adding exponents.','10ᵃ×10ᵇ=10ᵃ⁺ᵇ.','10²×10³=10⁵.'],
 ['Place-value shift','Multiplication by 10 moves digits one place left in base ten.','Repeated multiplication shifts by exponent.','100×1000=100000.'],
 ['Matching bases','The simple sum rule requires the same base.','Check both bases before combining.','10²×2³ is not 10⁵.']
],[
 ['Multiply powers','Compute 10²×10³.','Add exponents for same base.', ['Write 10²=100 and 10³=1000.','Multiply to obtain 100000.','Equivalent power is 10^(2+3)=10⁵.'],'10⁵=100000.','Five factors of 10.'],
 ['Multiply three terms','Simplify 10¹×10²×10⁴.','Add all exponents.', ['Bases all equal 10.','1+2+4=7.','Combined expression 10⁷.'],'10⁷.','Seven factors of ten.'],
 ['Correct false product','Student says 10³×10²=10⁶. What is correct?','Check rule.', ['Same-base multiplication adds exponents.','3+2=5 rather than 3×2=6.','Correct power is 10⁵.'],'10⁵.','1000×100=100000.']
],[
 ['Multiplying exponents during a product.','The product rule uses addition.','Write expanded factors to see why.'],
 ['Applying same-base law to different bases.','Common base condition matters.','Rewrite with common bases only when valid.']
],['Multiplying powers of 10 adds exponents.','Place-value reasoning supports the rule.','Check bases and operations.']),
lesson(3,'Powers of Powers of 10',[
 'Goal: reason about repeated multiplication of a power and derive the power-of-power rule (10ᵃ)ᵇ=10ᵃᵇ.',
 'When (10²)³ appears, three copies of 10² are multiplied, giving 10²×10²×10²=10⁶. This differs from adding the exponents 2+3, which would give 10⁵.',
 'Make an original exponent-tree representation grouping factors into equal blocks. Teacher review checks the explanation of grouping versus product.'
],[
 ['Power of a power','An exponent outside parentheses counts copies of the inside power.','(aᵐ)ⁿ=aᵐⁿ, a≠0 for signed exponents.','(10²)³=10⁶.'],
 ['Product contrast','Multiplying same-base powers adds exponents.','10²×10³=10⁵, not 10⁶.','Different operations require distinct rules.'],
 ['Expanded blocks','Each inner power contributes its own factor count.','Three copies of two factors give six.','(10²)³ has 2×3 total factors of ten.']
],[
 ['Power of power','Simplify (10²)³.','Multiply exponents.', ['The inside contains two factors of ten.','Repeat that block three times, for six factors.','Answer 10⁶.'],'10⁶.','100³=1000000.'],
 ['Power of fourth power','Simplify (10⁴)².','Multiply 4×2.', ['Inner exponent is 4.','Outer exponent 2 indicates two copies.','Total exponent 8.'],'10⁸.','10⁴×10⁴=10⁸.'],
 ['Distinguish operations','Which is larger: (10²)³ or 10²×10³?','Evaluate the exponent rules.', ['Power of power yields 10⁶.','Product yields 10⁵.','10⁶=10×10⁵, so first is 10 times larger.'],'(10²)³ is 10 times larger.','Operations are not interchangeable.']
],[
 ['Adding exponents in a power of a power.','Grouping repeats the inner factors multiplicatively.','Multiply the exponents.'],
 ['Ignoring parentheses.','10^(2³) is different from (10²)³.','Identify the grouping before calculating.']
],['Power-of-power multiplies exponents.','Products of powers add exponents.','Grouping determines which rule applies.']),
lesson(4,'Dividing Powers of 10',[
 'Goal: derive division of same-base powers by canceling equal nonzero factors and interpret exponents smaller than, equal to, or greater than the denominator exponent.',
 'For powers of 10, 10ᵃ/10ᵇ=10ᵃ⁻ᵇ. When exponent difference is zero, the quotient equals one; when negative, the result is a reciprocal fraction. Division by zero is never allowed.',
 'Draw factor-cancellation rows to explain 10⁵/10², then explore 10²/10⁵. A teacher assesses the cancellation model, while exact numeric answers are independently checkable.'
],[
 ['Quotient rule','Divide powers of the same nonzero base by subtracting exponents.','10ᵃ/10ᵇ=10ᵃ⁻ᵇ.','10⁵/10²=10³.'],
 ['Cancel factors','Equal nonzero factors in numerator and denominator cancel.','10/10=1.','Five factors over two leave three.'],
 ['Negative exponent','A negative signed exponent represents a reciprocal power.','10⁻³=1/1000.','10²/10⁵=10⁻³.']
],[
 ['Divide large by small','Compute 10⁵/10².','Subtract 5−2.', ['Both base values are ten.','Exponent difference is 5−2=3.','Result 10³=1000.'],'1000.','100000/100=1000.'],
 ['Divide equal powers','Compute 10⁴/10⁴.','Subtract equal exponents.', ['Difference 4−4=0.','10⁰=1.','Any nonzero value divided by itself is 1.'],'1.','Both numerator and denominator are nonzero.'],
 ['Small over large','Compute 10²/10⁵.','Subtract exponents.', ['Difference 2−5=−3.','Result is 10⁻³.','Write as 1/10³=1/1000.'],'1/1000.','100/100000=0.001.']
],[
 ['Subtracting exponents in reverse order.','Numerator exponent minus denominator exponent is the rule.','Keep quotient direction.'],
 ['Concluding a smaller numerator gives zero.','A positive fraction can be below one without equal zero.','Use reciprocal notation.']
],['Division of powers subtracts exponents.','Equal nonzero powers divide to one.','Negative result exponent represents a reciprocal.']),
lesson(5,'Negative Exponents with Powers of 10',[
 'Goal: use the exponent pattern to interpret negative powers of ten as positive rational reciprocals and convert them to decimal place values.',
 'As exponent decreases by one, divide the value by ten. From 10⁰=1, it follows 10⁻¹=0.1 and 10⁻²=0.01. These values are not negative numbers; the sign belongs to the exponent.',
 'Construct a place-value number line from 1 to 0.001 with labels. Teacher checks ordered spacing and explanation; a numerical conversion alone does not validate the drawn model.'
],[
 ['Negative power','A reciprocal power of a nonzero base.','10⁻ⁿ=1/10ⁿ for positive n.','10⁻³=1/1000.'],
 ['Decimal movement','Dividing by ten shifts decimal places rightward in place-value naming.','10⁻¹=0.1, 10⁻²=0.01.','0.001 represents one thousandth.'],
 ['Value versus sign','A negative exponent does not make a positive base negative.','10⁻³>0.','0.001 is positive.']
],[
 ['Convert reciprocal','Write 10⁻³ in decimal form.','Invert 10³.', ['10³=1000.','10⁻³=1/1000.','Decimal representation is 0.001.'],'0.001.','1000×0.001=1.'],
 ['Compare values','Which is larger, 10⁻² or 10⁻⁴?','Convert or compare reciprocals.', ['10⁻²=0.01.','10⁻⁴=0.0001.','0.01 is greater.'],'10⁻².','A more negative power of 10 is smaller in positive value.'],
 ['Check a negative claim','A learner says 10⁻¹=−10. Is that valid?','Interpret exponent sign.', ['Negative exponent means reciprocal.','10⁻¹=1/10.','Its value is positive 0.1.'],'No; 10⁻¹=0.1.','(1/10)×10=1.']
],[
 ['Treating a negative exponent as a negative number.','The exponent sign encodes reciprocal.','Apply a⁻ⁿ=1/aⁿ.'],
 ['Rounding a tiny positive value to zero without explanation.','Positive fractions remain nonzero.','Retain exact power notation.']
],['Negative powers of 10 are small positive reciprocals.','Consecutive exponents differ by factors of ten.','Use exact fractions when appropriate.']),
lesson(6,'What about Other Bases?',[
 'Goal: extend integer-exponent laws beyond powers of ten to nonzero positive or negative bases, preserving sign and nonzero denominator restrictions.',
 'The product, quotient and power-of-power laws work for any nonzero base where all terms are defined. A negative base enclosed in parentheses changes sign according to exponent parity; an unparenthesized negative sign may not be part of the base.',
 'Compare 2³×2⁻⁵ and (−2)³. Work exact rational results and discuss why any logarithm problem is unrelated to this Grade 8 objective. A teacher reviews laws and symbolic justification.'
],[
 ['Same-base product','Combine exponents when multiplying one base.','aᵐaⁿ=aᵐ⁺ⁿ for a≠0.','2³×2⁻⁵=2⁻²=1/4.'],
 ['Same-base quotient','Subtract denominator exponent.','aᵐ/aⁿ=aᵐ⁻ⁿ for a≠0.','3⁴/3²=3²=9.'],
 ['Negative-base parity','Odd powers of a negative base are negative, even powers are positive.','Keep parentheses around the base.','(−2)³=−8 and (−2)⁴=16.']
],[
 ['Combine signed exponents','Compute 2³×2⁻⁵.','Add exponents.', ['3+(−5)=−2.','2⁻²=1/2².','The result is 1/4.'],'1/4.','8×1/32=1/4.'],
 ['Power of negative base','Compute (−2)³.','Multiply three negative twos.', ['(−2)(−2)=4.','4×(−2)=−8.','Odd exponent preserves negative sign.'],'−8.','Different from −2³ only because parentheses define base, though this odd case has same value.'],
 ['Quotient law','Compute 3⁴/3².','Subtract powers.', ['4−2=2.','3²=9.','Numerical check 81/9=9.'],'9.','No logarithms required.']
],[
 ['Dropping parentheses from a negative base.','It may change the operation order.','Write (−a)ⁿ when negative is part of base.'],
 ['Applying laws to a zero denominator.','Division by zero is undefined.','Require a nonzero base for signed exponents.']
],['Exponent laws extend to any nonzero base.','Signs depend on base and parity.','Exact rationals replace unrelated logarithms.']),
lesson(7,'Practice with Rational Bases',[
 'Goal: apply integer-exponent rules to rational bases, including reciprocals and negative exponents, and simplify exact fractions.',
 'For a nonzero fraction p/q, the negative power (p/q)⁻ⁿ=(q/p)ⁿ. A negative fraction raised to an even power yields a positive result, while odd powers remain negative.',
 'Create a rational-base card sort showing two expressions with the same value, then justify without floating-point rounding. Teacher checks symbolic reasoning and equivalence.'
],[
 ['Rational base','A fraction p/q where q≠0.','Nonzero base additionally requires p≠0 for negative powers.','(2/3)⁻².'],
 ['Reciprocal rule','Negative exponent swaps numerator and denominator.','(p/q)⁻ⁿ=(q/p)ⁿ.','(2/3)⁻²=(3/2)²=9/4.'],
 ['Sign parity','Even powers of a negative fraction are positive.','(−p/q)²=p²/q².','(−2/3)²=4/9.']
],[
 ['Reciprocal squared','Evaluate (2/3)⁻².','Invert then square.', ['Negative exponent inverts 2/3 to 3/2.','Square numerator and denominator.','Value 9/4.'],'9/4.','(2/3)²×(9/4)=1.'],
 ['Negative rational squared','Evaluate (−2/3)².','Square entire negative base.', ['(−2)²=4.','3²=9.','Quotient is 4/9.'],'4/9.','Even power is positive.'],
 ['Cube a rational base','Compute (3/2)³.','Cube numerator and denominator.', ['3³=27.','2³=8.','Value is 27/8.'],'27/8.','Equivalent to 3.375 but exact fraction preferred.']
],[
 ['Negating the fraction instead of taking reciprocal.','Negative exponent is not a minus sign outside.','Flip base for negative power.'],
 ['Squaring numerator but not denominator.','Both factors are raised to the exponent.','Compute pⁿ/qⁿ.']
],['Rational bases obey integer exponent rules.','Negative powers invert nonzero fractions.','Keep answers exact when possible.']),
lesson(8,'Combining Bases',[
 'Goal: multiply and divide products with different bases when exponent laws justify combining same exponents, without inventing an invalid same-base rule.',
 'If two different bases have the same exponent, aⁿbⁿ=(ab)ⁿ. Conversely, a^m b^n cannot generally combine by adding m+n unless bases are equal. Prime factorization can sometimes expose a common base.',
 'Compare 2³×5³ with 2³×5². Create a reasoned equivalence chart. A teacher reviews the symbolic factor grouping.'
],[
 ['Same exponent product','Different bases but equal exponent can combine within parentheses.','aⁿbⁿ=(ab)ⁿ.','2³×5³=10³.'],
 ['Different exponent caution','Unlike bases and exponents cannot combine directly.','Rearrange factors or calculate separately.','2³×5²=8×25=200.'],
 ['Common base rewrite','Express composite bases as powers of one number.','4³=(2²)³=2⁶.','2³×4³=2⁹.']
],[
 ['Combine same exponent','Compute 2³×5³.','Combine bases at exponent 3.', ['2×5=10.','(2×5)³=10³.','10³=1000.'],'1000.','8×125=1000.'],
 ['Do not overcombine','Compute 2³×5².','Evaluate separate powers.', ['2³=8.','5²=25.','Product 8×25=200.'],'200.','Not 10⁵.'],
 ['Rewrite powers of two','Simplify 2³×4³.','Rewrite 4 as 2².', ['4³=(2²)³=2⁶.','Multiply with 2³ to give 2^(3+6).','2⁹=512.'],'512.','8×64=512.']
],[
 ['Adding unlike bases in exponents.','aᵐbⁿ cannot be combined with simple exponent addition.','Look for a genuine common base.'],
 ['Forgetting parentheses when combining bases.','(ab)ⁿ is not abⁿ.','Group all factors under exponent.']
],['Same exponents can combine bases multiplicatively.','Same bases can combine exponents.','Never apply either law without checking its hypotheses.']),
lesson(9,'Describing Large and Small Numbers Using Powers of 10',[
 'Goal: use powers of ten to express very large and very small quantities efficiently and compare orders of magnitude.',
 'A number such as 50,000 can be written 5×10⁴. A small decimal like 0.0007 equals 7×10⁻⁴. The exponent describes place value, not a count of decimal digits after arbitrary rounding.',
 'Arrange original length and mass measurements on a powers-of-ten place-value chart. A teacher reviews the representation and order-of-magnitude explanation.'
],[
 ['Large-number power','Multiplying by 10ⁿ shifts magnitude up n decimal places.','a×10ⁿ for n>0.','5×10⁴=50000.'],
 ['Small-number power','Negative power scales down by reciprocal of 10ⁿ.','a×10⁻ⁿ.','7×10⁻⁴=0.0007.'],
 ['Order of magnitude','Comparing powers of ten for scale.','Powers differ by factors 10 per exponent step.','10⁶ is 1000 times 10³.']
],[
 ['Write large quantity','Convert 50,000 to a one-digit factor times a power of ten.','Move decimal four places.', ['50,000=5×10,000.','10,000=10⁴.','Number is 5×10⁴.'],'5×10⁴.','5×10000=50000.'],
 ['Write small quantity','Convert 0.0007 to power notation.','Count place-value positions.', ['0.0007 is seven ten-thousandths.','One ten-thousandth is 10⁻⁴.','Therefore 7×10⁻⁴.'],'7×10⁻⁴.','7/10000=0.0007.'],
 ['Compare scales','What is 10⁶/10³?','Subtract exponents.', ['6−3=3.','Ratio=10³.','10³=1000.'],'1000.','One million is 1000 times one thousand.']
],[
 ['Counting exponent from number of digits alone.','Leading nonzero coefficient matters.','Track decimal shifts.'],
 ['Treating 10⁻⁴ as a negative quantity.','Negative exponent means reciprocal.','Check positivity.']
],['Powers of ten encode place value.','Negative powers express small positive scales.','Exponent differences compare magnitudes.']),
lesson(10,'Representing Large Numbers on the Number Line',[
 'Goal: place large numbers expressed in powers of ten on a scaled number line and justify each point’s relative position.',
 'A number line with ticks at multiples of 10⁶ can locate 3×10⁶ exactly at the third such tick. A logarithmic axis would behave differently; do not silently switch between linear and logarithmic scales.',
 'Draw a line from 0 to 5,000,000 with million-unit spacing; locate 2.5×10⁶ and explain its midpoint position. A teacher checks the scale and labeling.'
],[
 ['Linear large-number scale','Equal distances represent equal numerical increments.','Mark regular tick values.','Ticks 0,10⁶,2×10⁶,3×10⁶.'],
 ['Between-tick placement','Fractional multiples sit proportionally between ticks.','2.5×10⁶ halfway between 2×10⁶ and 3×10⁶.','Locate 2.5 million.'],
 ['Scale consistency','Do not change tick value partway through an axis.','All equal spaces use the same amount.','Each 1-cm tick gap can represent one million.']
],[
 ['Plot two million','Where is 2×10⁶ on a million-increment number line?','Read the labeled ticks.', ['Each tick gap equals one million.','2×10⁶ is two million.','Place it two gaps right of zero.'],'Second million tick.','Coordinates increase uniformly.'],
 ['Plot midpoint','Where is 2.5×10⁶ relative to 2×10⁶ and 3×10⁶?','Use proportional position.', ['Difference between ticks is 10⁶.','2.5×10⁶ exceeds 2×10⁶ by 0.5×10⁶.','Place halfway between ticks.'],'Halfway between two and three million.','Equal distance from both bounding values.'],
 ['Compare positions','Which lies farther right: 4×10⁶ or 35×10⁵?','Convert to common power.', ['35×10⁵=3.5×10⁶.','4×10⁶=4 million.','4 million exceeds 3.5 million.'],'4×10⁶.','Difference is 0.5×10⁶.']
],[
 ['Using equal spacing for powers 10²,10³,10⁴ on a linear number line.','The values are multiplicative, not equal additive steps.','Choose and state axis type.'],
 ['Omitting unit scale.','Large coordinates become visually ambiguous.','Label tick values.']
],['Large powers can be located on linear scales.','Use consistent equal numerical increments.','Fractions of a tick gap encode intermediate values.']),
lesson(11,'Representing Small Numbers on the Number Line',[
 'Goal: position small positive decimals and negative powers of ten using correct fractional tick spacing, recognizing closeness to zero but not equality with zero.',
 'A line from 0 to 0.001 marked every 0.0001 has ten equal intervals. 3×10⁻⁴ occupies the third positive tick; the negative coordinate −3×10⁻⁴ is three equal ticks left of zero on a signed line.',
 'Create an original micro-distance axis and compare 0.0002,0.0005 and 0.0008. Teacher review checks the precision and regular spacing.'
],[
 ['Small decimal location','A positive decimal lies right of zero even if tiny.','Use a fine scale.','0.0003=3×10⁻⁴.'],
 ['Tick interval','Equal steps can represent a fixed fraction.','If each tick is 10⁻⁴, third tick is 3×10⁻⁴.','0.0001,0.0002,0.0003.'],
 ['Signed small value','A negative decimal lies left of zero.','Its distance from zero equals its absolute value.','−0.0003 is left three 10⁻⁴ ticks.']
],[
 ['Third positive tick','If intervals are 0.0001, what is third tick right of zero?','Multiply spacing.', ['One step gives 0.0001.','Three steps give 3×0.0001.','Result 0.0003.'],'0.0003.','Equal spacing.'],
 ['Compare small numbers','Which is larger, 0.0002 or 0.0005?','Compare ten-thousandths.', ['0.0002 is 2 ten-thousandths.','0.0005 is 5 ten-thousandths.','5>2.'],'0.0005.','It lies farther right of zero.'],
 ['Locate a negative decimal','Where does −0.0003 lie on tick spacing 0.0001?','Count left from zero.', ['Its sign is negative.','Magnitude equals 3 increments.','Move left three ticks.'],'Third tick left of zero.','Magnitude 0.0003.']
],[
 ['Rounding all tiny values to zero.','A nonzero value remains distinguishable on an adequate scale.','Use finer ticks.'],
 ['Reversing signed point location.','Negative is left on ordinary increasing x-axis.','Mark zero and direction first.']
],['Small powers of ten can be graphed precisely.','Tiny is not zero.','Tick increment controls resolution.']),
lesson(12,'Applications of Arithmetic with Powers of 10',[
 'Goal: use exponent arithmetic to estimate and compute quantities spanning large and small scales, with appropriate units and reasonableness checks.',
 'When multiplying a large and a small power, add signed exponents. When dividing measurements, subtract exponents. Decimal coefficients must also be multiplied or divided, and physical units must be handled separately.',
 'Model the area of a microsensor rectangle of dimensions 3×10⁻³ m and 2×10⁻² m; find area and explain why the product exponent is −5. Teacher checks dimensional interpretation.'
],[
 ['Product scale','Add signed powers of ten.','10ᵃ×10ᵇ=10ᵃ⁺ᵇ.','10⁻³×10⁻²=10⁻⁵.'],
 ['Coefficient product','Multiply coefficients separately from powers.','(a×10ᵐ)(b×10ⁿ)=ab×10ᵐ⁺ⁿ.','3×2×10⁻⁵=6×10⁻⁵.'],
 ['Unit consistency','Product of lengths gives area.','m×m=m².','Microsensor area is 6×10⁻⁵ m².']
],[
 ['Micro-area','Rectangle length 3×10⁻³ m, width 2×10⁻² m. What area?','Multiply coefficients and powers.', ['Coefficients 3×2=6.','Exponent sum −3+(−2)=−5.','Area 6×10⁻⁵ m².'],'6×10⁻⁵ m².','0.003×0.02=0.00006.'],
 ['Time scale ratio','Find 10⁶/10⁻².','Subtract exponents.', ['6−(−2)=8.','Result 10⁸.','Value 100000000.'],'10⁸.','Division by 0.01 multiplies by 100.'],
 ['Large-small product','Compute (5×10⁵)(2×10⁻³).','Multiply coefficients and exponents.', ['Coefficient product 5×2=10.','Exponent sum 5−3=2.','10×10²=10³.'],'10³=1000.','500000×0.002=1000.']
],[
 ['Combining coefficients with exponent additions.','Exponent law only combines powers, not coefficient arithmetic.','Calculate separately.'],
 ['Forgetting squared units in area.','A product of two lengths is area.','Carry units through multiplication.']
],['Powers of ten simplify extreme-scale arithmetic.','Multiply coefficients separately.','Use consistent units and plausibility.']),
lesson(13,'Definition of Scientific Notation',[
 'Goal: use normalized scientific notation a×10ⁿ with 1≤|a|<10 for nonzero quantities and integer n, retaining correct sign and magnitude.',
 'The coefficient must have an absolute value at least one and less than ten. For 62000, 6.2×10⁴ is normalized; 62×10³ has the same value but is not in standard normalized form.',
 'Write measurements from an original chart into normalized notation. Teacher checks decimal-place reasoning and units, not just pattern memorization.'
],[
 ['Normalized coefficient','A real coefficient whose magnitude lies in [1,10).','1≤|a|<10.','6.2 is valid; 62 is not.'],
 ['Integer ten-exponent','An integer power tracks decimal scaling.','a×10ⁿ.','6.2×10⁴=62000.'],
 ['Negative number','Sign remains with coefficient.','Negative a is permitted.','−4.5×10²=−450.']
],[
 ['Large number','Write 62000 in scientific notation.','Move decimal to get 1≤a<10.', ['62000→6.2 by moving decimal four positions left.','Compensate with 10⁴.','Result 6.2×10⁴.'],'6.2×10⁴.','6.2×10000=62000.'],
 ['Small decimal','Write 0.00084 in scientific notation.','Move decimal right four positions.', ['Coefficient becomes 8.4.','Compensating power is 10⁻⁴.','Result 8.4×10⁻⁴.'],'8.4×10⁻⁴.','8.4/10000=0.00084.'],
 ['Normalize expression','Rewrite 45×10³ in standard notation.','Move factor of ten into exponent.', ['45=4.5×10.','Then 45×10³=4.5×10^(1+3).','Answer 4.5×10⁴.'],'4.5×10⁴.','Both expressions equal 45000.']
],[
 ['Using coefficient 45 as normalized.','Its absolute value is not below 10.','Shift decimal and adjust exponent.'],
 ['Changing the sign of a negative quantity during normalization.','Exponent changes magnitude, not original sign.','Keep negative coefficient.']
],['Normalized scientific notation requires 1≤|a|<10.','Exponent controls powers of ten.','Convert both very large and very small numbers.']),
lesson(14,'Multiplying, Dividing, and Estimating with Scientific Notation',[
 'Goal: multiply and divide numbers in scientific notation, normalize results, and estimate their magnitude before calculating.',
 'Multiply coefficients while adding exponent powers; divide coefficients while subtracting exponents. Re-normalize if the coefficient falls outside [1,10). Check for a zero denominator.',
 'Compare original contexts of microscopic cell length and astronomical distance without claiming the same measurement units. Teacher reviews the interpretation and order-of-magnitude estimate.'
],[
 ['Scientific product','Multiply coefficients and add powers.','(a×10ᵐ)(b×10ⁿ)=ab×10ᵐ⁺ⁿ.','(3×10⁴)(2×10³)=6×10⁷.'],
 ['Scientific quotient','Divide coefficients and subtract powers, denominator nonzero.','(a×10ᵐ)/(b×10ⁿ)=(a/b)×10ᵐ⁻ⁿ.','(6×10⁸)/(2×10³)=3×10⁵.'],
 ['Normalization','If |coefficient|≥10, shift one ten into exponent.','12×10⁵=1.2×10⁶.','Coefficient 1.2 is normalized.']
],[
 ['Product example','Compute (3×10⁴)(2×10³).','Multiply components.', ['3×2=6.','10⁴×10³=10⁷.','Answer 6×10⁷.'],'6×10⁷.','Six times ten million.'],
 ['Quotient example','Compute (6×10⁸)/(2×10³).','Divide coefficients and powers.', ['6/2=3.','8−3=5.','Answer 3×10⁵.'],'3×10⁵.','300000.'],
 ['Normalize a product','Compute (4×10²)(3×10³).','Multiply and normalize.', ['Coefficient product 4×3=12.','Exponent sum 2+3=5 gives 12×10⁵.','Rewrite as 1.2×10⁶.'],'1.2×10⁶.','Equals 1200000.']
],[
 ['Failing to normalize 12×10⁵.','The coefficient is at least ten.','Shift decimal one place and increase exponent.'],
 ['Dividing exponents instead of subtracting them.','Powers of same base use quotient law.','Subtract numerator minus denominator powers.']
],['Scientific notation simplifies extreme-scale products and quotients.','Normalize the final coefficient.','Estimate before trusting calculation.']),
lesson(15,'Adding and Subtracting with Scientific Notation',[
 'Goal: add or subtract scientific-notation quantities by aligning powers of ten before combining coefficients, avoiding invalid exponent combination.',
 'Addition does not add exponents: 3×10⁵+2×10⁵=5×10⁵, but if exponents differ, first rewrite one with the other power. The quantities must have compatible units.',
 'Create a place-value chart aligning 2.4×10⁶ and 3.0×10⁵; explain why 0.3×10⁶ is equal to the second term. A teacher checks representation and unit consistency.'
],[
 ['Common exponent','A shared ten-power factor before addition.','a×10ⁿ+b×10ⁿ=(a+b)×10ⁿ.','3×10⁵+2×10⁵=5×10⁵.'],
 ['Exponent alignment','Rewrite a term with a chosen power.','3×10⁵=0.3×10⁶.','2.4×10⁶+0.3×10⁶=2.7×10⁶.'],
 ['Unit requirement','Only add quantities with compatible dimensions.','Convert measurements first.','Meters plus centimeters require conversion.']
],[
 ['Same powers','Compute 3×10⁵+2×10⁵.','Add coefficients.', ['Both terms have ten-power 10⁵.','Coefficient sum 3+2=5.','Answer 5×10⁵.'],'5×10⁵.','300000+200000=500000.'],
 ['Different powers','Compute 2.4×10⁶+3×10⁵.','Align exponent.', ['Rewrite 3×10⁵=0.3×10⁶.','Add 2.4+0.3=2.7.','Answer 2.7×10⁶.'],'2.7×10⁶.','2,400,000+300,000=2,700,000.'],
 ['Subtract nearby values','Compute 5.1×10⁴−2.0×10³.','Rewrite with common power.', ['Rewrite 2×10³=0.2×10⁴.','Subtract coefficients 5.1−0.2=4.9.','Answer 4.9×10⁴.'],'4.9×10⁴.','51000−2000=49000.']
],[
 ['Adding exponents for addition.','Exponent addition is multiplication law, not sum law.','Factor a common power.'],
 ['Subtracting unlike physical units.','The resulting number lacks meaning.','Convert to compatible units first.']
],['Align ten-powers before adding or subtracting.','Combine coefficients only after alignment.','Re-normalize results.']),
lesson(16,'Is a Smartphone Smart Enough to Go to the Moon?',[
 'Goal: apply scientific notation to compare hypothetical data storage, computing or transmission scales in a clearly defined technology scenario, stating assumptions and units.',
 'A byte count and bit count differ by a factor of eight. Storage capacity and processing speed are different dimensions and cannot be compared as if both were the same quantity; a realistic moon mission also has reliability, power and radiation requirements beyond raw capacity.',
 'Original hypothetical scenario: device A stores 1.2×10¹¹ bytes, device B stores 3×10¹⁰ bytes. Compare capacity and express device A in bits. Teacher reviews modeling limitations, evidence sources and why capacity alone does not establish flight suitability.'
],[
 ['Byte conversion','Eight bits make one byte in the usual digital-data representation.','bits=8×bytes.','1.2×10¹¹ bytes=9.6×10¹¹ bits.'],
 ['Ratio of capacities','Compare measurements in the same unit.','A/B computes factor.','(1.2×10¹¹)/(3×10¹⁰)=4.'],
 ['Multiple engineering constraints','Mission readiness cannot be inferred from storage alone.','Consider fault tolerance, radiation, power and software.','More bytes does not prove moon-flight suitability.']
],[
 ['Compare capacities','A has 1.2×10¹¹ bytes and B has 3×10¹⁰ bytes. Ratio?','Divide coefficients and powers.', ['Coefficient ratio 1.2/3=0.4.','Exponent difference 11−10=1.','0.4×10¹=4.'],'A holds four times B.','Both values are byte capacities.'],
 ['Convert to bits','A has 1.2×10¹¹ bytes. How many bits?','Multiply by eight.', ['8×1.2=9.6.','Power remains 10¹¹.','Result 9.6×10¹¹ bits.'],'9.6×10¹¹ bits.','Bits and bytes are distinct units.'],
 ['Estimate larger total','Three devices each have capacity 1.2×10¹¹ bytes. Total?','Multiply coefficient by count.', ['3×1.2=3.6.','Exponent stays 10¹¹.','Total 3.6×10¹¹ bytes.'],'3.6×10¹¹ bytes.','This is just capacity, not proven mission reliability.']
],[
 ['Comparing bit counts directly with byte counts.','Units differ by eightfold.','Convert to common unit.'],
 ['Claiming a consumer device is flight-qualified because storage is large.','Environmental and reliability requirements are separate.','Identify missing engineering evidence.']
],['Scientific notation supports digital scale comparisons.','Convert bit/byte units explicitly.','Capacity is not sufficient to certify mission suitability.'])
];
if(rows.length!==16||new Set(rows.map(x=>x.LessonCode)).size!==16)throw Error('Unit7 count mismatch');
for(const r of rows)if(r.ExplanationParagraphs.length<2||r.KeyConcepts.length<3||r.WorkedExamples.length<3||r.CommonMistakes.length<2||r.SummaryPoints.length<3)throw Error('Malformed RichV2 '+r.LessonCode);
const out=path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u07-l01-l16-2017.rich-lesson-v2.json');
fs.writeFileSync(out,JSON.stringify({SchemaVersion:1,ContentVersion:'us-ccss-g8-u07-first-party-rich-v2-20261029-v1',PackCode:'US-CCSS-MATH',Lessons:rows},null,2)+'\n');
console.log('G8_U07 '+JSON.stringify({lessons:rows.length,examples:rows.reduce((a,r)=>a+r.WorkedExamples.length,0),out}));
