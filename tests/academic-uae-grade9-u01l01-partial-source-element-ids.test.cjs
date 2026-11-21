'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const b=path.resolve(__dirname,'../artifacts/curriculum-delivery/UAE-MOE-MATH/G9');
const read=x=>JSON.parse(fs.readFileSync(path.join(b,x),'utf8'));
test('Known U01L01 partial source element inventory retains 61 numbered + 21 instructional IDs with physical-page traceability',()=>{
 const m=read('u01l01-preliminary-source-element-ids-NO_GO.json');
 const ids=[...m.numberedExerciseContainers,...m.knownNonNumberedInstruction];
 assert.equal(m.knownDistinctSourceSubsetIds,82);
 assert.equal(m.numberedExerciseContainers.length,61);
 assert.equal(m.knownNonNumberedInstruction.length,21);
 assert.equal(new Set(ids.map(x=>x.SourceElementId)).size,82);
 assert.deepEqual(m.knownSubsetByPrintedPage,{'5':14,'6':7,'7':34,'8':8,'9':19});
 for(const item of ids) {
  assert.ok([5,6,7,8,9].includes(item.SourcePage.printed));
  assert.equal(item.SourcePage.pdf,item.SourcePage.printed+11);
  assert.ok(item.SourceElementId.startsWith('01.pdf:1-1:'));
 }
 assert.equal(m.SourcePdfSha256,read('grade9-82-target-index.json').records[0].SourcePdfSha256);
 assert.equal(m.sourceSnapshotId,read('source-selection.json').sourceSnapshotId);
});
test('Known source exercise containers are matched to independent analog IDs, never falsely certified as exact textbook atomic parity',()=>{
 const m=read('u01l01-preliminary-source-element-ids-NO_GO.json');
 const analog=read('u01l01-original-61-parallel-skill-practice.json').records;
 const audit=read('u01l01-61-source-visual-skill-audit.json').records;
 for(let i=0;i<61;i++){
  const r=m.numberedExerciseContainers[i];
  assert.equal(r.SourceElementId,'01.pdf:1-1:Q'+String(i+1).padStart(2,'0'));
  assert.equal(r.analogTaskId,analog[i].independentTaskId);
  assert.equal(audit[i].sourceItem,analog[i].sourceItem);
  assert.equal(r.publisherExactTask1to1Certified,false);
  assert.equal(r.fullAtomicSubpartInventory,false);
  assert.equal(r.teacherGradingEvidence,false);
 }
 assert.deepEqual(m.numberedExerciseContainerSubpartRisks,[34,35,36,37,48]);
});
test('Partial source subset cannot change unknown total element denominator or D_PENDING academic gate',()=>{
 const m=read('u01l01-preliminary-source-element-ids-NO_GO.json');
 const d=read('grade9-stage-d-execution-ledger.json');
 const c=read('lesson-1-stage-d-checkpoint.json');
 const v=read('grade9-release-validation.json');
 assert.equal(m.separatelyReviewedStructuralGroupsNotDoubleCounted,7);
 assert.ok(m.openUnreviewedGroups.length>=4);
 assert.equal(m.rawTotalAtomicSourceElementDenominator,null);
 assert.equal(m.isExhaustiveSourceElementInventory,false);
 assert.equal(m.sourceSemanticParity,'UNKNOWN');
 assert.equal(m.fullRawSourceLessonAuditPassed,false);
 assert.equal(m.canonicalLessonDVerified,false);
 assert.equal(m.releaseDecision,'NO_GO_SOURCE_100');
 assert.equal(c.partialSourceElementManifest,'u01l01-preliminary-source-element-ids-NO_GO.json');
 assert.equal(c.sourceTotalAtomicElementDenominatorStillUnknown,true);
 assert.equal(c.sourceElementCount,null);
 assert.equal(c.dStatus,'D_PENDING');
 assert.equal(v.firstLessonRawAtomicDenominator,null);
 assert.equal(v.rawLessonsFullyAudited,0);
 assert.equal(v.targetLessonsOneToOneVerified,0);
 assert.equal(d.sourceSemanticLock.rawLessonAtomicAuditPassed,0);
 assert.equal(d.sourceSemanticLock.targetLessonElementParityPassed,0);
 assert.equal(d.stageDStatus,'D_PENDING_A_C_EVIDENCE');
});
