const fs=require('fs'),path=require('path');const root=path.resolve(__dirname,'..');
const mappings=require(path.join(root,'src/Edulytics.Core/Mathematics/Curriculum/official-outcome-practice-map.v1.json')).entries;
const rules=require(path.join(root,'src/Edulytics.Core/Mathematics/Curriculum/supporting-practice-target-rules.v1.json')).rules;
const cases=[
 {code:'CCSS:5.G.A.1',required:'Locate and interpret points using ordered pairs in a first-quadrant coordinate system.',issue:'Midpoint and generic analytic-geometry questions go beyond the Grade 5 standard; a first-quadrant coordinate family is needed.',risky:['supporting.geometry.midpoint','supporting.geometry.analytic.mixed']},
 {code:'CCSS:5.NF.B.7',required:'Divide unit fractions by nonzero whole numbers, and whole numbers by unit fractions; solve corresponding contextual problems.',issue:'The generic fraction multiplication/simplification families do not specifically implement the two required unit-fraction division forms.',risky:['supporting.fractions.multiply','supporting.fractions.simplify']},
 {code:'CCSS:6.EE.A.2',required:'Write expressions, identify terms/factors/coefficients, evaluate expressions and formulas including whole-number exponents.',issue:'Simplification/substitution provides partial coverage; identifying expression components and formula evaluation need distinct proven families.',risky:[]},
 {code:'CCSS:7.NS.A.2',required:'Multiply and divide signed rational numbers, apply the operation properties, and convert rationals to terminating/repeating decimals.',issue:'Generic fraction-only multiplication/division/simplification does not establish full signed-rational operation and decimal-conversion coverage.',risky:['supporting.fractions.divide_whole','supporting.fractions.simplify']}
];
const findings=cases.map(x=>{const map=mappings.find(y=>y.outcomeCode===x.code);if(!map)throw Error('Missing official map '+x.code);
const rule=rules.find(y=>y.id===map.targetRuleId);if(!rule)throw Error('Missing target rule '+x.code);
return {...x,targetRule:rule.id,currentFamilies:rule.families,riskyPresent:x.risky.filter(f=>rule.families.includes(f)),status:'NOT_ACADEMICALLY_CERTIFIED'};});
const doc={version:1,scope:'Explicit high-risk CCSS standard-to-Practice semantic spot check; not an exhaustive 363-standard certification.',productionReady:false,findings};
const out=path.join(root,'docs/curriculum/us-ccss-practice-semantic-blockers.json');fs.writeFileSync(out,JSON.stringify(doc,null,2)+'\n');
console.log(JSON.stringify({productionReady:false,highRiskStandards:findings.length,findings:findings.map(x=>({code:x.code,currentFamilies:x.currentFamilies,riskyPresent:x.riskyPresent}))},null,2));