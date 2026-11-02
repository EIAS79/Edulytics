const fs=require('fs'),crypto=require('crypto'),path=require('path'),root=path.resolve(__dirname,'..');
const base=path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/Packs');
const manifestFile=path.join(root,'docs/curriculum/us-ccss-verified-body-repairs.v1.json'),manifest=JSON.parse(fs.readFileSync(manifestFile));
if(manifest.repairs.length!==8||manifest.schemaVersion!==1||manifest.allowUnlistedContentChanges!==false)throw Error('Unexpected baseline repair manifest');
const extras=[
{
file:'us-ccss-math-hs-alg1-phase29-v1.lesson-content-pack.json',
code:'PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U04:L15',
suffix:{
explanation:'Edulytics original standards extension (HSF-BF.B.4.d; not attributed to the source publisher): a many-to-one quadratic such as q(x)=(x−3)² is not invertible over all real numbers. To create an inverse function, restrict the domain to x≥3, so the parabola is one-to-one on that branch; the resulting inverse q⁻¹(y)=3+√y is defined for y≥0. Choosing x≤3 instead produces q⁻¹(y)=3−√y. These are different, valid inverse branches with explicit domain restrictions.',
keyConceptsAndRules:'Edulytics extension — restriction to obtain an inverse: q(1)=q(5)=4 for q(x)=(x−3)², so no single-valued inverse exists on all real inputs. Restrict q to x≥3 and solve y=(x−3)² to obtain x=3+√y, y≥0; alternatively restrict x≤3 and select x=3−√y. An inverse is a function only when every permitted output has exactly one original input. This is different from merely restricting the physical interval of an already one-to-one linear function.',
workedExamples:'Edulytics extension, Example 4 — noninvertible quadratic: Let q(x)=(x−3)². q(1)=4 and q(5)=4, proving it is not one-to-one on all real x. Restrict to x≥3. If y=(x−3)², then x−3=√y (the positive branch by the restriction), so q⁻¹(y)=3+√y for y≥0. Thus q⁻¹(16)=3+4=7 and q(7)=16. On the alternative domain x≤3, q⁻¹(y)=3−√y and q⁻¹(16)=−1. Both compositions return permitted inputs on their respective domains; taking ±√y together would not define a single-valued inverse function.',
stepByStepSolutions:'Edulytics extension for HSF-BF.B.4.d: 1. Check whether the original graph is one-to-one; q(x)=(x−3)² fails because q(1)=q(5). 2. State the restricted domain explicitly, such as x≥3. 3. Write y=(x−3)² and solve for x. 4. Use the branch consistent with the restriction: x=3+√y for x≥3. 5. State the inverse domain y≥0. 6. Check q(q⁻¹(16))=16 and q⁻¹(q(7))=7. 7. Reject the other square-root sign because it lies outside the chosen restricted domain.',
commonMistakes:'Edulytics extension: do not call a full-domain quadratic invertible merely because you can solve its equation with ±√y; two outputs for one inverse input do not define a function. Do not omit whether x≥3 or x≤3. Choosing the wrong square-root sign violates the restricted domain. An already invertible linear function needs no mathematical domain restriction to become one-to-one.',
quickSummary:'Edulytics extension: a many-to-one quadratic becomes invertible only after choosing a one-to-one branch. For q(x)=(x−3)² restricted to x≥3, q⁻¹(y)=3+√y for y≥0. Numeric evaluation checks only part of the graphical restriction skill.'
}
},
{
file:'us-ccss-math-hs-alg2-phase29-v1.lesson-content-pack.json',
code:'PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:U05:L10',
suffix:{
explanation:'Edulytics original standards extension (HSF-BF.A.1.c; not attributed to the source publisher): composition builds a new function by using the output of an inner function as the input of an outer function. For two functions f and g, the composition f(g(x)) generally differs from both the product f(x)g(x) and the reverse composition g(f(x)). In a real context, the inner function’s output units must match the outer function’s input units.',
keyConceptsAndRules:'Edulytics extension — composition: (f∘g)(x)=f(g(x)). Evaluate g first, then apply f. The composition domain consists of x in the domain of g for which g(x) lies in the domain of f. Do not substitute outputs from different x values or confuse f(g(x)) with (fg)(x). Composition is generally not commutative: f∘g need not equal g∘f. Units must chain: minutes→height in meters→temperature in degrees Celsius.',
workedExamples:'Edulytics extension, Example 4 — algebraic composition: f(x)=2x+1 and g(x)=3x−4. Then f(g(x))=2(3x−4)+1=6x−7, while g(f(x))=3(2x+1)−4=6x−1. At x=2, f(g(2))=5, but g(f(2))=11. This demonstrates why composition is not simply multiplication and its order matters. Example 5 — temperature of a weather balloon: its height in meters after t minutes is h(t)=1,000+2t; atmospheric temperature in degrees Celsius at height y meters is T(y)=20−0.006y. Their composition is T(h(t))=20−0.006(1,000+2t)=14−0.012t °C. At t=5 minutes, the predicted temperature is 13.94°C. The input/output units match: time→height→temperature.',
stepByStepSolutions:'Edulytics extension for HSF-BF.A.1.c: 1. Identify the outer function f and the inner function g. 2. Evaluate or simplify g(x) first. 3. Replace the input of f with the entire expression g(x), keeping parentheses. 4. Simplify f(g(x)) without changing the order. 5. Check the domain: g(x) must be defined and its value must be an allowed input to f. 6. In contexts, verify output-input units are compatible across functions. 7. Test a numeric value using separate inner and outer evaluations to confirm the formula.',
commonMistakes:'Edulytics extension: (f∘g)(x) is not f(x)g(x); it is f(g(x)). In general g(f(x)) differs from f(g(x)). A physically invalid height or a mismatched output unit makes a contextual composition meaningless. Domain restrictions propagate through the inner-to-outer calculation.',
quickSummary:'Edulytics extension: build compositions by substituting the inner output into the outer function. Example f(g(x))=6x−7 and g(f(x))=6x−1 for f(x)=2x+1, g(x)=3x−4. This extension is original Edulytics content beyond the publisher lesson’s arithmetic focus.'
}
}
];
const fields=['title','explanation','keyConceptsAndRules','workedExamples','stepByStepSolutions','commonMistakes','quickSummary'];
for(const e of extras){const f=path.join(base,e.file),pack=JSON.parse(fs.readFileSync(f));
const lesson=pack.lessons.find(x=>x.lessonCode===e.code);const entry=manifest.repairs.find(x=>x.lessonCode===e.code);if(!lesson||!entry||entry.previousCorrectedCanonicalBodySha256)throw Error('Manifest or lesson ambiguity '+e.code);
const en=lesson.translations.find(x=>x.cultureCode==='en');if(!en||lesson.canonicalBodySha256!==entry.expectedCorrectedCanonicalBodySha256||lesson.sourceSha256!==entry.expectedUnchangedSourceSha256)throw Error('Unsupported source state '+e.code);
for(const [key,value] of Object.entries(e.suffix)){if(en[key].includes('Edulytics original standards extension')||en[key].includes('Edulytics extension'))throw Error('Already enriched');en[key]+='\n\n'+value;}
const next=crypto.createHash('sha256').update(fields.map(k=>k+':'+en[k]).join('\n')).digest('hex');
entry.previousCorrectedCanonicalBodySha256=entry.expectedCorrectedCanonicalBodySha256;entry.expectedCorrectedCanonicalBodySha256=next;lesson.canonicalBodySha256=next;
fs.writeFileSync(f,JSON.stringify(pack,null,2)+'\n');
console.log('EXTENDED '+e.code+' old='+entry.previousCorrectedCanonicalBodySha256+' new='+next+' source='+lesson.sourceSha256);
}
fs.writeFileSync(manifestFile,JSON.stringify(manifest,null,2)+'\n');