'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const crypto=require('node:crypto');
const root=path.resolve(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const dir='artifacts/academic-us-ccss/batches/';
const meta=read(dir+'B01-v9-addition-template-25-source-goal-activity-reviewed.json');
const manifest=read(dir+'B01-lesson-manifest.json');
const inventory=read(dir+'B01-book-source-reference-index-500.json');
const packs=[1,2,3,4].map(g=>read('src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g'+g+'-phase29-v1.lesson-content-pack.json'));
const SHA=/^[a-f0-9]{64}$/;
function validate(source=meta) {
 assert.equal(source.planId,'EDULYTICS-US-CCSS-UNIFIED-V9');
 assert.equal(source.batchId,'B01');
 assert.equal(source.academicDecision,'BATCH_NO-GO');
 assert.equal(source.cohortRows.length,25);
 assert.equal(source.rows.length,21);
 const byCode=new Map(manifest.lessons.map(x=>[x.lessonCode,x]));
 const byIndex=new Map(inventory.rows.map(x=>[x.lessonCode,x]));
 const byBody=new Map(packs.flatMap(p=>p.lessons).map(x=>[x.lessonCode,x]));
 const baseline=source.cohortRows[0].lessonCode;
 const defaultSteps=byBody.get(baseline).translations.find(t=>t.cultureCode==='en').stepByStepSolutions;
 const rootId='REPEATED_STEPBYSTEPSOLUTIONS_'+crypto.createHash('sha256').update('stepByStepSolutions\0'+defaultSteps).digest('hex').slice(0,20);
 assert.equal(source.rootCauseId,rootId);
 const repetitions=[...byBody.values()].filter(l=>byCode.has(l.lessonCode)&&
     l.translations.some(t=>t.cultureCode==='en'&&t.stepByStepSolutions===defaultSteps));
 assert.equal(repetitions.length,25);
 const codes=new Set(),newCodes=new Set();
 let corrections=0,previous=0;
 for (const r of source.cohortRows) {
  assert(!codes.has(r.lessonCode),'duplicate reviewed source lesson');
  codes.add(r.lessonCode);
  const m=byCode.get(r.lessonCode),s=byIndex.get(r.lessonCode),l=byBody.get(r.lessonCode);
  assert(m&&s&&l,'unknown cohort lesson');
  assert.equal(r.ordinal,m.ordinal);
  assert.equal(r.sourceSemanticSha256,m.sourceSemanticSha256);
  assert.equal(r.originalTeacherSourceUrl,m.sourceUrl);
  assert.equal(r.sourceFileInventorySha256,s.localFileByteSha256);
  assert.equal(r.originalTeacherRelativePath,s.localRelativePath);
  assert.equal(r.canonicalOriginalBodySha256,l.canonicalBodySha256);
  assert.equal(r.originalTeacherActivityUrl,m.sourceUrl.replace('preparation.html','lesson.html'));
  assert(SHA.test(r.publisherLivePreparationHtmlSha256));
  assert(SHA.test(r.publisherLiveLessonHtmlSha256));
  assert.equal(r.sourceGoalReviewed,true);
  assert.equal(r.originalTeacherGoalSectionFound,true);
  assert.equal(r.originalTeacherActivityFetched,true);
  assert.equal(r.academicCertifiedByThisReview,false);
  assert.equal(l.translations.find(t=>t.cultureCode==='en').stepByStepSolutions,defaultSteps);
  if(r.needsSharedRootCorrection)corrections++;
  if(r.alreadyCountedInPrevious113)previous++;
  else newCodes.add(r.lessonCode);
 }
 assert.deepEqual([...codes].sort(),repetitions.map(l=>l.lessonCode).sort());
 assert.equal(corrections,21);
 assert.equal(previous,4);
 assert.equal(newCodes.size,21);
 assert.deepEqual(new Set(source.rows.map(x=>x.lessonCode)),newCodes);
 for(const r of source.rows) {
  const c=source.cohortRows.find(x=>x.lessonCode===r.lessonCode);
  assert.equal(r.sourceSemanticSha256,c.sourceSemanticSha256);
  assert.equal(r.sourceFileInventorySha256,c.sourceFileInventorySha256);
 }
 assert.equal(source.scope.verifiedPurposeOnlyTotal,134);
 assert.equal(source.scope.sourceAppropriateControls,3);
 assert.equal(source.scope.sourceVerifiedWrongOrInsufficientInstruction,21);
 assert.equal(source.scope.previouslyRepairedControls,1);
 return true;
}
test('One original 25-source teaching cohort: 21 verified procedure targets, 3 appropriate controls and one prior fix',()=>{
 assert.equal(validate(),true);
});
test('Source/body SHA, identity, source link, or fabricated certification fails closed',()=>{
 for(const mutation of [
  x=>x.cohortRows[0].canonicalOriginalBodySha256='0'.repeat(64),
  x=>x.cohortRows[0].sourceSemanticSha256='0'.repeat(64),
  x=>x.cohortRows[0].originalTeacherSourceUrl='https://example.com/',
  x=>x.cohortRows[0].academicCertifiedByThisReview=true,
  x=>x.cohortRows.push(x.cohortRows[0]),
  x=>x.rows.pop()
 ]) {
  const broken=structuredClone(meta); mutation(broken);
  assert.throws(()=>validate(broken));
 }
});
