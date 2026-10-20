'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const {build,STATUS}=require('../tools/curriculum/us-ccss-g1-a-to-c-status.cjs');
const fs=require('node:fs');
const path=require('node:path');
const dir=path.resolve(__dirname,'../artifacts/curriculum-delivery/US-CCSS-MATH/G1');

test('exact 96 lesson identities reconcile historic acceptance and A–C evidence without jumping to D',()=>{
  const {lessons,summary}=build();
  assert.equal(lessons.length,96);
  assert.equal(summary.historicallyPublishedOutsideScope,50);
  assert.equal(summary.scopeNewLessons,96);
  assert.equal(summary.releaseDecision,'NO_GO');
  const groups=new Map(Object.values(STATUS).map(s=>[s,0]));
  for(const [index,row] of lessons.entries()){
    assert.equal(row.ordinal,51+index);
    assert.match(row.lessonCode,/^PED:US-CCSS-MATH:G1:/);
    assert.ok(groups.has(row.status));
    groups.set(row.status,groups.get(row.status)+1);
    if(row.status===STATUS.ready){
      assert.equal(row.aToCReady,true);
      const checkpoint=JSON.parse(fs.readFileSync(path.join(dir,row.evidenceFile),'utf8'));
      assert.equal(checkpoint.aToCClosure?.originalStudentAndTeacherTasksCompared,true);
      assert.equal(checkpoint.aToCClosure?.learnerTeachingSourceParityReviewed,true);
      assert.equal(checkpoint.aToCClosure?.practiceGeneratorAndVerifierIndependentlyReviewed,true);
      assert.deepEqual(checkpoint.aToCClosure?.openACDefects,[]);
      assert.ok(checkpoint.aToCClosure?.evidenceReference?.length>15);
      assert.notEqual(checkpoint.stageA.allOriginalFigureAndStudentTasksFullyAudited,false);
      assert.notEqual(checkpoint.stageC.originalSourceTaskToPracticeParity,'PARTIAL_OR_UNPROVEN');
    }else if(row.status!==STATUS.accepted){
      assert.equal(row.aToCReady,false);
      assert.ok(row.evidenceFile.endsWith('-v2-stage-a-d-checkpoint.json'));
    }
  }
  assert.equal(groups.get(STATUS.accepted),summary.previouslyInternallyAccepted);
  assert.equal(groups.get(STATUS.ready),summary.explicitlyReadyForFinalD);
  assert.equal(groups.get(STATUS.gap),summary.knownACGaps);
  assert.equal(groups.get(STATUS.unproven),summary.workedButNotProvenACClosed);
  assert.equal(groups.get(STATUS.ready)+groups.get(STATUS.gap)+
    groups.get(STATUS.unproven)+groups.get(STATUS.accepted),96);
  assert.equal(summary.newLessonsNotYetAccepted,96-summary.previouslyInternallyAccepted);
});

test('prepared A–D files, source hashes, reused code and technical tests are not automatic A–C certification',()=>{
  const {lessons}=build();
  // This assertion must remain valid as genuine work moves from gaps to D-ready.
  assert.equal(lessons.length,96);
  for(const row of lessons.filter(x=>x.status===STATUS.gap)){
    const cp=JSON.parse(fs.readFileSync(path.join(dir,row.evidenceFile),'utf8'));
    assert.ok(cp.stageA.allOriginalFigureAndStudentTasksFullyAudited===false||
      cp.stageA.sourceGoalAgainstEffectiveAuthenticatedContentApproved===false||
      cp.stageC.originalSourceTaskToPracticeParity==='PARTIAL_OR_UNPROVEN');
  }
});
