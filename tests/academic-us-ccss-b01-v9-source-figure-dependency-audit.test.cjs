'use strict';
const test=require('node:test'),assert=require('node:assert/strict');
const {buildFigureDependencies}=require('../tools/us-ccss-b01-v9-source-figure-dependency-audit.cjs');
function fixture(){
 const names=Array.from({length:500},(_,i)=>'PED:US-CCSS-MATH:G2:U01:L'+String(i).padStart(3,'0'));
 const graph={planId:'EDULYTICS-US-CCSS-UNIFIED-V9',batchId:'B01',
  coverage:{expected:500,observed:500,unique:500},nodes:names.map((name,i)=>({
   ordinal:i+1,lessonCode:name,sourceSemanticSha256:'a'.repeat(64),
   originalTeacherActivitySignals:i===0?['DATA_GRAPH']:i===1?['RECTANGULAR_ARRAY']:[]
  }))};
 const effective={planId:graph.planId,batchId:'B01',
  coverage:{expected:500,joined:500,unique:500},academicDecision:'BATCH_NO-GO',
  rows:names.map((name,i)=>({ordinal:i+1,lessonCode:name,
   effectiveLearnerFingerprint:'b'.repeat(64),
   effectivePatternSignals:{figureCaptionPlaceholder:i<40},
   richV2VisualKinds:i===0?['BarGraph']:i===1?['EquationSet']:['EquationSet'],
   richV2Source:'RUNTIME_COMPOSER'})),
  sharedPatternRoots:{SOURCE_FIGURE_CAPTION_PLACEHOLDER:{
   rawCandidates:44,effectiveMarkerStillPresent:40,effectiveMarkerAbsent:4,
   effectiveMarkerLessonCodes:names.slice(0,40)}}
 };
 return {graph,effective};
}
test('Exactly forty unbound figure labels remain L6-blocked despite generated visual objects',()=>{
 const x=fixture(),r=buildFigureDependencies(x.graph,x.effective);
 assert.equal(r.referencePopulation.batchLessons,500);
 assert.equal(r.cohorts.length,40);
 assert.equal(r.structuralCorrelationTotals.RENDER_KIND_FAMILY_COMPATIBLE_SEMANTICS_UNVERIFIED,1);
 assert.equal(r.structuralCorrelationTotals.SOURCE_CUE_VISUAL_KIND_FAMILY_NOT_MATCHED,1);
 assert.equal(r.structuralCorrelationTotals.ORIGINAL_ACTIVITY_FIGURE_SEMANTICS_UNINDEXED,38);
 assert.equal(r.cohorts[0].structurallyCompatibleSourceCueFamilies[0],'DATA_GRAPH');
 assert.equal(r.cohorts[1].uncorrelatedSourceCueFamilies[0],'RECTANGULAR_ARRAY');
 assert.ok(r.cohorts.every(x=>x.exactSourceActivityDiagramRetrieved===false));
 assert.ok(r.cohorts.every(x=>x.automaticFigureReplacementAllowed===false));
 assert.equal(r.academicCertificationAwarded,0);
 assert.equal(r.academicDecision,'BATCH_NO-GO');
});
test('Identity drift, false certification, missing image cohort and changed source hashes fail closed',()=>{
 const a=fixture();a.effective.rows[100].lessonCode='WRONG';
 assert.throws(()=>buildFigureDependencies(a.graph,a.effective));
 const b=fixture();b.effective.academicDecision='BATCH_PASS';
 assert.throws(()=>buildFigureDependencies(b.graph,b.effective));
 const c=fixture();c.effective.sharedPatternRoots.SOURCE_FIGURE_CAPTION_PLACEHOLDER.effectiveMarkerLessonCodes.pop();
 assert.throws(()=>buildFigureDependencies(c.graph,c.effective));
 const d=fixture();d.graph.nodes[0].sourceSemanticSha256='INVALID';
 assert.throws(()=>buildFigureDependencies(d.graph,d.effective));
});
