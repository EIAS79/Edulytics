'use strict';
const {test}=require('node:test'),assert=require('node:assert/strict');
const cp=require('node:child_process'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
function check(name){
 const result=cp.spawnSync(process.execPath,[path.join(root,'tools',name)],{cwd:root,encoding:'utf8'});
 assert.equal(result.status,0,result.stderr||result.stdout);
}
function read(filename){return JSON.parse(fs.readFileSync(path.join(root,'artifacts/academic-us-ccss',filename),'utf8'));}
test('PHASE 00/01 provenance is 1560 sources including nine nested HS publisher claims',()=>{
 check('us-ccss-source-provenance-audit.cjs');
 const a=read('01-authority-registry.json'),p=read('01-publisher-provenance.json');
 assert.equal(a.parentStandards.length,363);
 assert.equal(a.subordinateClauses.length,121);
 assert.equal(p.lessons.length,1560);
 assert.equal(p.summary.missingLessonUrls,0);
 assert.equal(p.summary.missingDeclaredSourceRootUrls,0);
 assert.equal(p.summary.ambiguousPublisherAttribution,0);
 assert.equal(p.summary.humanApprovedLicenses,0);
 assert.equal(p.summary.academicallyCertifiedLessons,0);
 assert.equal(p.blueprints.filter(x=>x.declaredSources.length>0).length,17);
 assert.ok(p.blueprints.some(x=>x.declaredSources.length>1));
});
test('PHASE 02 keeps 98 independent dispositions open and verifies nine prior guarded repairs',()=>{
 check('us-ccss-official-adjudication-queue.cjs');
 const d=read('02-official-text-diff-dispositions.json');
 const m=read('02-reviewed-authority-repairs.json');
 const g=read('02-digest-transition-proof.json');
 assert.equal(d.unreviewedStandards.length,98);
 assert.equal(d.recordedCorrections.length,9);
 assert.ok(d.unreviewedStandards.every(x=>x.disposition==='NEEDS_EXTERNAL_REVIEW'&&!x.humanReviewerId));
 assert.equal(m.repairs.length,9);
 assert.equal(g.productionMutationApproved,false);
});
test('PHASE 05 enumerates the 63 unverified child candidate gaps and 15 parent-only references',()=>{
 check('us-ccss-clause-gaps.cjs');
 const ledger=JSON.parse(fs.readFileSync(path.join(root,'docs/curriculum/us-ccss-academic-evidence-ledger.json'),'utf8'));
 assert.equal(ledger.clauseEvidence.length,121);
 assert.equal(ledger.clauseEvidence.filter(x=>x.assessmentEvidenceStatus==='NO_CANDIDATE_READY_ROUTED_FAMILY').length,63);
 assert.equal(ledger.clauseEvidence.filter(x=>x.exactSourceReferenceCount===0).length,15);
 assert.equal(ledger.clauseEvidence.filter(x=>x.certificationStatus==='ACADEMICALLY_CERTIFIED').length,0);
});
test('PHASE 04 records content quality concerns and never awards automatic academic approval',()=>{
 check('us-ccss-content-integrity-audit.cjs');
 const d=read('04-automated-content-risk-audit.json');
 assert.equal(d.lessons.length,1560);
 assert.equal(d.summary.lessonsWithCanonicalContent,1560);
 assert.equal(d.summary.humanReviewed,0);
 assert.ok(d.summary.largestIdenticalProcedureUsage>100);
 assert.ok(d.lessons.every(x=>x.academicReviewStatus==='PENDING_INDEPENDENT_REVIEW'));
 assert.ok(d.lessons.some(x=>x.lessonCode==='PED:US-CCSS-MATH:G4:U01:L01' &&
   x.contentRiskFlags.includes('MATH_TOPIC_PROCEDURE_CONFLICT_MULTIPLES_VS_FIGURE_CLASSIFICATION')));
});
