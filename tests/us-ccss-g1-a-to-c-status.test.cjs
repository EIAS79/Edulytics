'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const {build,buildRangeReport,main,STATUS}=require('../tools/curriculum/us-ccss-g1-a-to-c-status.cjs');
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

test('ordered ten-lesson range report reads existing evidence without running D or inventing A-C closure',()=>{
  const snapshot=build();
  const report=buildRangeReport(57,66,snapshot);
  assert.equal(report.scope,'G1 57-66');
  assert.equal(report.lessons.length,10);
  assert.equal(report.summary.total,10);
  assert.equal(report.summary.ciTriggeredByReport,false);
  assert.equal(report.summary.releaseDecision,'NO_GO');
  assert.deepEqual(report.lessons.map(x=>x.ordinal),
    [57,58,59,60,61,62,63,64,65,66]);
  for(const item of report.lessons){
    const original=snapshot.lessons.find(x=>x.ordinal===item.ordinal);
    assert.equal(item.lessonCode,original.lessonCode);
    assert.equal(item.stageA,original.stageA);
    assert.equal(item.stageB,original.stageB);
    assert.equal(item.stageC,original.stageC);
    assert.equal(item.readyForStageD,original.aToCReady);
    assert.equal(item.aToCStatus,original.status);
    if(item.readyForStageD){
      assert.equal(item.nextAction,'HOLD_AT_STAGE_D');
      assert.equal(item.stageA,'REVIEW_EVIDENCED');
      assert.equal(item.stageB,'REVIEW_EVIDENCED');
      assert.equal(item.stageC,'REVIEW_EVIDENCED');
      assert.deepEqual(item.openACDefects,[]);
      assert.ok(item.evidenceReference?.length>15);
    }
    if(item.stageA==='NOT_PROVEN'||item.stageB==='NOT_PROVEN'||
       item.stageC==='NOT_PROVEN'||item.stageA==='EXPLICIT_GAP'||
       item.stageC==='EXPLICIT_GAP')
      assert.equal(item.readyForStageD,false);
  }
  assert.equal(report.summary.previouslyInternallyAccepted+
    report.summary.readyForStageD+report.summary.knownACGaps+
    report.summary.reviewClosureUnproven,10);
  assert.deepEqual(main(['--range=57-66']),report);
  assert.deepEqual(main(['--range=57-66','--summary']),report.summary);
});

test('range report rejects gaps and ranges outside 51–146, without guessed completion',()=>{
  const snapshot=build();
  for(const [from,to] of [[0,66],[50,60],[57,147],[66,57],[58.1,66]])
    assert.throws(()=>buildRangeReport(from,to,snapshot),/valid Grade 1/);
  for(const option of ['--range=xx','--range=50-66','--range=57-147','--range=57-66-67'])
    assert.throws(()=>main([option]));
});
