// Actual academic status for source-backed US CCSS Practice remediation.
// This report does not equate a green test suite with full a/b/c clause certification.
const fs=require('fs'),path=require('path'),root=path.resolve(__dirname,'..');
const map=require(path.join(root,'src/Edulytics.Core/Mathematics/Curriculum/official-outcome-practice-map.v1.json')).entries;
const rules=require(path.join(root,'src/Edulytics.Core/Mathematics/Curriculum/supporting-practice-target-rules.v1.json')).rules;
const lessons=require(path.join(root,'src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json')).mappings;
const clauses=require(path.join(root,'docs/curriculum/us-ccss-121-clause-publisher-evidence.json')).summary;
const decisions=require(path.join(root,'docs/curriculum/us-ccss-secondary-14-lesson-review.json')).summary;
const reviewed=[
 ['CCSS:5.G.A.1','us-ccss-g5-first-quadrant','usccss.geometry.first_quadrant_point'],
 ['CCSS:5.NF.B.7','us-ccss-g5-unit-fraction-division','usccss.fractions.unit_divide_whole'],
 ['CCSS:6.EE.A.2','us-ccss-g6-expression-structure','usccss.algebra.expression_coefficient'],
 ['CCSS:7.NS.A.2','us-ccss-g7-rational-operations','usccss.number.signed_rational_multiply']
].map(([code,id,family])=>{
const mapping=map.find(x=>x.outcomeCode===code),rule=rules.find(x=>x.id===id);
if(!mapping||mapping.targetRuleId!==id||!rule||!rule.families.includes(family))throw Error('Remediation drift for '+code);
return {standard:code,targetRule:id,questionFamilies:rule.families,status:'NARROW_OUTCOME_RULE_IMPLEMENTED'};
});
const explicit=lessons.filter(x=>x.lessonCode.startsWith('PED:US-CCSS-MATH:')&&x.sourceType==='ReviewedPublisherLessonExactPractice');
if(explicit.length!==25)throw Error('Expected 25 explicit US lesson overrides, got '+explicit.length);
if(clauses.clauseCount!==121||clauses.parentOnlyOrNoClauseEvidence!==39||decisions.explicitlyNarrowed!==5)throw Error('Source coverage drift');
const doc={version:2,productionReady:false,verifiedLocalTestCount:2212,sourceReferencedLessons:25,multiOutcomeCasesReviewed:14,officialOutcomesWithNarrowRoutes:reviewed,sourceEvidence:{subclauses:121,exactPublisherReferences:clauses.publisherExactlyReferenced,requireAdditionalPerClauseEvidence:39},remainingReleaseBlockers:['39 parent-only subclause source references require independent clause-to-question verification','Full 363-standard educational semantic review not complete','Actual Render staging application boot and role-based tests not verified','Production workspace not yet confirmed'],notice:'This is a targeted release gate. No unsupported full-academic certification or production deployment claim.'};
fs.writeFileSync(path.join(root,'docs/curriculum/us-ccss-practice-semantic-blockers.json'),JSON.stringify(doc,null,2)+'\n');
console.log(JSON.stringify({productionReady:doc.productionReady,targetStandards:reviewed.length,explicit:explicit.length,gaps:39},null,2));