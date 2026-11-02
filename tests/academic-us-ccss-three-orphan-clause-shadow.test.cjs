'use strict';
const test=require('node:test'), assert=require('node:assert/strict');
const path=require('node:path'),cp=require('node:child_process'),fs=require('node:fs');
const {casesFor,independentlyVerify,buildEvidence}=
 require('../tools/us-ccss-three-orphan-clause-shadow.cjs');
const root=path.resolve(__dirname,'..');
const artifact=require('../artifacts/academic-us-ccss/05-three-orphan-child-math-shadow-candidates.json');
test('Phase 05 orphan clauses: reproducible 3-parent-only/184-case ledger with NO-GO guard',()=>{
 const run=cp.spawnSync(process.execPath,
  [path.join(root,'tools/us-ccss-three-orphan-clause-shadow.cjs'),'--check'],
  {cwd:root,encoding:'utf8'});
 assert.equal(run.status,0,run.stderr||run.stdout);
 assert.deepEqual(artifact,buildEvidence());
 assert.equal(artifact.mathematicalCaseCount,184);
 assert.equal(artifact.newShadowFamilies,3);
 assert.equal(artifact.studentPracticeRouting,false);
 assert.equal(artifact.skillFamilyProductionReady,false);
 assert.equal(artifact.independentHumanAcademicReviews,0);
 assert.deepEqual(artifact.sourcePublisherExactCoverageBefore,
  artifact.sourcePublisherExactCoverageAfter);
 const registry=require('../artifacts/academic-us-ccss/05-clause-publisher-reference-reconciliation.json');
 assert.equal(registry.summary.publisherUnresolved,15);
 assert.deepEqual(artifact.clauses.map(x=>x.clause),
  ['3.MD.C.7.a','5.MD.C.3.a','6.SP.B.5.a']);
 for(const row of artifact.clauses){
  assert.equal(row.rawPublisherChildAddressingCount,0);
  assert.equal(row.releaseStatus,'SHADOW_ONLY_NOT_STUDENT_ROUTED');
  assert.equal(row.humanAcademicReviewerId,null);
  assert.ok(row.candidateLesson.sourceUrl.startsWith('https://im.kendallhunt.com/'));
 }
});
test('3.MD.C.7.a: all 100 rectangles satisfy independent tiled counts and products',()=>{
 const a=casesFor('rectangle_tiling_product');
 assert.equal(a.length,100);
 for(const c of a){
  assert.equal(independentlyVerify(c,'rectangle_tiling_product'),true);
  assert.throws(()=>independentlyVerify({...c,answerArea:c.answerArea+1},
   'rectangle_tiling_product'));
  assert.throws(()=>independentlyVerify({...c,tileRows:['#'.repeat(c.columns-1),...c.tileRows.slice(1)]},
   'rectangle_tiling_product'));
 }
});
test('5.MD.C.3.a: unit cube is EXACTLY one-by-one-by-one, not any solid',()=>{
 const a=casesFor('unit_cube_recognition');
 assert.equal(a.length,24);
 assert.equal(a.filter(c=>c.isUnitCube).length,4);
 for(const c of a){
  assert.equal(independentlyVerify(c,'unit_cube_recognition'),true);
  assert.throws(()=>independentlyVerify({...c,isUnitCube:!c.isUnitCube},
   'unit_cube_recognition'));
 }
 const wrong=a.find(x=>x.edgeDimensions.join(',')==='1,1,2');
 assert.equal(wrong.isUnitCube,false);
});
test('6.SP.B.5.a: all 60 datasets count zeros and duplicate observations',()=>{
 const a=casesFor('number_of_observations');
 assert.equal(a.length,60);
 for(const c of a){
  assert.equal(independentlyVerify(c,'number_of_observations'),true);
  assert.throws(()=>independentlyVerify({...c,answerCount:c.answerCount-1},
    'number_of_observations'));
 }
 const allZeros=a.find(x=>x.id==='count-10-pattern-0');
 assert.equal(allZeros.measurements.length,10);
 assert.equal(new Set(allZeros.measurements).size,1);
 assert.equal(allZeros.answerCount,10);
});
test('No user-facing, official curriculum, or school metadata is mutated by this shadow tool',()=>{
 const read=p=>fs.readFileSync(path.join(root,p),'utf8');
 const proof=read('tools/us-ccss-three-orphan-clause-shadow.cjs');
 assert.doesNotMatch(proof,/UPDATE\s+"?SchoolCurriculumAdoptions/i);
 assert.equal(artifact.studentPracticeRouting,false);
 assert.equal(artifact.clauses.every(x=>x.rawPublisherChildAddressingCount===0),true);
});
