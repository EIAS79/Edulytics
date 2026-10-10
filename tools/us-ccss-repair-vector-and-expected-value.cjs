const fs=require('fs'),crypto=require('crypto'),path=require('path');
const root=path.resolve(__dirname,'..');
const packDir=path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/Packs');
const repairs=[
{
file:'us-ccss-math-hs-vector-matrix-phase29-v1.lesson-content-pack.json',
code:'PED:US-CCSS-MATH:HS:FOURTH:VECTOR-MATRIX:U01:L01',
translation:{
title:'The Arithmetic of Vectors',
explanation:'A vector has magnitude and direction. In the coordinate plane, write v = (vₓ, vᵧ), where its components give the horizontal and vertical displacement. Two vectors can be added head-to-tail or by combining their components; both methods give the same resultant. Subtracting w means adding its opposite −w. Multiplying by a scalar k multiplies each component by k; a negative scalar reverses the direction. The magnitude of (x, y) is √(x² + y²). Distinguish vector magnitude from the sum of magnitudes. To find a resultant from magnitude-and-direction information, first resolve each vector into horizontal and vertical components. These methods address CCSS HSN-VM.B.4.a–c and HSN-VM.B.5.a–b; the graphical methods should be represented with head-to-tail and parallelogram diagrams in teacher-led work.',
keyConceptsAndRules:'Vector addition: (a,b)+(c,d)=(a+c,b+d). Head-to-tail and the parallelogram diagonal represent the same sum. Vector subtraction: (a,b)−(c,d)=(a−c,b−d)=(a,b)+(−c,−d). Scalar multiplication: k(a,b)=(ka,kb). Length: ||(a,b)||=√(a²+b²). Scalar length: ||kv||=|k|·||v||, not k·||v|| for negative k. For k>0 direction is preserved; for k<0 it is reversed; for k=0 the zero vector has no direction. If u=(r cos θ,r sin θ) is given in magnitude-and-direction form, convert to components, then add and compute the resultant length and angle. For nonzero (x,y), identify the quadrant before interpreting its direction.',
workedExamples:'Example 1 — addition by components and head-to-tail: Let u=(3,2) and v=(−1,4). Draw u from (0,0) to (3,2); from its head draw v to (2,6). The resulting diagonal from (0,0) to (2,6) is u+v=(2,6). Its magnitude is √40=2√10, not ||u||+||v||. Example 2 — subtracting and scalar multiplication: u=(4,−1), v=(2,3). Then u−v=(2,−4); −2v=(−4,−6) has twice v’s magnitude and opposite direction. Example 3 — magnitude/direction of a sum: a has magnitude 3 at 0° and b has magnitude 4 at 90°. Their components are (3,0) and (0,4). The sum is (3,4), its magnitude is 5, and its direction is arctan(4/3)≈53.13° above the positive x-axis.',
stepByStepSolutions:'1. Express the given vectors as ordered component pairs; resolve magnitude-and-direction data if necessary. 2. For addition, add corresponding x- and y-components. For subtraction, negate the second vector and add. 3. For scalar multiplication, multiply both components by the signed scalar. 4. If asked for magnitude, apply the Pythagorean formula to the resulting components. 5. If asked for direction, use the signs of the components to locate the quadrant; then find the angle from their ratio. 6. Verify graphically when possible: head-to-tail and the parallelogram rule must agree, and a negative scalar must reverse direction.',
commonMistakes:'Do not add vector magnitudes instead of components; lengths generally do not add. For u−v, negate both components of v. A negative scalar changes direction, but magnitude uses its absolute value. Do not compute the direction of the zero vector. When using inverse tangent, check the quadrant; an acute reference angle is not always the correct direction.',
quickSummary:'Vectors describe size and direction. Add and subtract components, represent addition with head-to-tail/parallelogram diagrams, scale both components, and check ||kv||=|k|·||v||. Use components to compute the resultant magnitude and direction.'
}
},
{
file:'us-ccss-math-hs-prob-decision-phase29-v1.lesson-content-pack.json',
code:'PED:US-CCSS-MATH:HS:FOURTH:PROB-DECISION:U01:L05',
translation:{
title:'Expected Value in Games and Decisions',
explanation:'An expected value is a probability-weighted average outcome over many repetitions. For outcomes xᵢ with probabilities pᵢ that sum to 1, E(X)=Σpᵢxᵢ. In a game of chance, use the NET payoff to include the price paid to play, not only the prize. A fair game has zero expected net payoff; an unfavorable game has negative expected net payoff. Expected value is not a prediction of the next single result, and it is not the most likely outcome. To compare decisions, construct a consistent payoff table for each strategy, use the same units and probability model, calculate each expected net payoff or expected cost, and then choose based on the decision objective and constraints. These activities directly address CCSS HSS-MD.B.5.a–b.',
keyConceptsAndRules:'Discrete expected value: E(X)=p₁x₁+p₂x₂+⋯+pₙxₙ, where pᵢ≥0 and Σpᵢ=1. Expected net payoff = expected gross prize − fixed playing cost. When choosing the greater long-run payoff, compare expected net payoffs; when comparing costs, prefer the lower expected cost if risk and other terms are equal. A probability-weighted average can be negative. Probability must sum to 1 and every payoff must be associated with the correct outcome; independent trials are not required merely to compute one trial’s expected value.',
workedExamples:'Example 1 — game of chance (HSS-MD.B.5.a): A ticket costs $3. You have a 1/5 chance to win a $10 gross prize and a 4/5 chance to win nothing. Net payoffs are $7 and −$3. Expected net payoff = (1/5)×7+(4/5)×(−3)=1.4−2.4=−$1 per ticket. The negative expectation does not mean every ticket loses $1. Example 2 — compare strategies (HSS-MD.B.5.b): Plan A has a certain cost of $12. Plan B costs $4 when there is no incident (probability 3/4) and $28 when there is an incident (probability 1/4). E(cost B)=(3/4)×4+(1/4)×28=$10. Plan B has $2 lower expected cost than Plan A, but the $28 outcome creates greater financial risk; choose according to the stated objective and constraints.',
stepByStepSolutions:'1. List every mutually exclusive outcome and check that probabilities sum to 1. 2. Express each outcome as a NET payoff (prize minus entry fee) or as an all-in cost. 3. Multiply each probability by its associated payoff/cost and add the products. 4. Interpret the sign, units, and long-run meaning. 5. For comparisons, compute both strategies on the same basis; bigger expected payoff is preferable when maximizing payoff, while smaller expected cost is preferable when minimizing cost, all else equal. 6. Check reasonableness: expected value should lie between the smallest and largest possible payoff/cost.',
commonMistakes:'Do not ignore ticket cost or deductible when defining a payoff. Do not confuse a prize with net profit, a likely outcome with the expected average, or costs with profits. Do not compare gross winnings for one strategy with net winnings for another. Probabilities must total 1; otherwise the calculation is incomplete. Expected value describes a long-run mean, not a guaranteed outcome.',
quickSummary:'Compute expected value by multiplying each net outcome by its probability and adding. For games subtract the ticket cost; for choices compare expected costs or payoffs consistently and acknowledge risk.'
}
}
];
for(const repair of repairs){
 const p=path.join(packDir,repair.file),pack=JSON.parse(fs.readFileSync(p,'utf8'));const entry=pack.lessons.find(l=>l.lessonCode===repair.code);
 if(!entry||entry.translations?.filter(t=>t.cultureCode==='en').length!==1)throw Error('Unsafe lesson resolution: '+repair.code);
 const tr=entry.translations.find(t=>t.cultureCode==='en');
 const original=JSON.stringify(tr);
 if(!(/Vectors/.test(repair.code)||/VECTOR-MATRIX/.test(repair.code)) && !/PROB-DECISION/.test(repair.code))throw Error('Unknown lesson');
 Object.assign(tr,repair.translation);
 const fields=['title','explanation','keyConceptsAndRules','workedExamples','stepByStepSolutions','commonMistakes','quickSummary'];
 const digest=crypto.createHash('sha256').update(fields.map(k=>k+':'+tr[k]).join('\n'),'utf8').digest('hex');
 const previousHash=entry.canonicalBodySha256;
 entry.canonicalBodySha256=digest;
 // SourceSha256 is the original publisher artifact fingerprint and must not be modified.
 if(original===JSON.stringify(tr)||digest===previousHash)throw Error('No change '+repair.code);
 fs.writeFileSync(p,JSON.stringify(pack,null,2)+'\n');
 console.log(JSON.stringify({code:repair.code,digest,previousHash,sourceHashPreserved:entry.sourceSha256}));
}