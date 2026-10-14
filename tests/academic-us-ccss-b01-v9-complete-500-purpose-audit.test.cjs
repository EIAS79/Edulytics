'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const base='artifacts/academic-us-ccss/batches/';
const manifest=read(base+'B01-lesson-manifest.json');
const source=read(base+'B01-book-source-reference-index-500.json');
const audit=read(base+'B01-v9-full-500-source-purpose-comparison-coverage.json');
const packs=[1,2,3,4].map(g=>read('src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g'+g+'-phase29-v1.lesson-content-pack.json'));
const SHA=/^[0-9a-f]{64}$/;
function check(x) {
 assert.equal(x.batchId,'B01');assert.equal(x.reviewUnit,'ONE_500_LESSON_MANIFEST');
 assert.equal(x.certificationDecision,'BATCH_NO-GO');assert.equal(x.newAcademicCertifications,0);
 assert.equal(x.scope.manifestLessons,500);
 assert.equal(x.scope.previouslySourcePurposeReviewed,185);
 assert.equal(x.scope.newlySourcePurposeCompared315,315);
 assert.equal(x.scope.sourcePurposeComparedTotal,500);
 assert.equal(x.scope.originalPreparationPagesNewlyRead,315);
 assert.equal(x.scope.originalTeacherLessonPagesNewlyRead,315);
 assert.equal(x.scope.sourcePageReadFailures,0);
 assert.equal(x.scope.fullL1toL6CertificationsGranted,0);
 assert.equal(x.newReviewedRows.length,315);
 const m=new Map(manifest.lessons.map(x=>[x.lessonCode,x]));
 const si=new Map(source.rows.map(x=>[x.lessonCode,x]));
 const l=new Map(packs.flatMap(p=>p.lessons).map(x=>[x.lessonCode,x]));
 assert.equal(m.size,500);
 let seen=new Set(),old=new Set(),warns=0;
 for(const sourcePath of x.previousReviewedEvidencePaths) {
  const j=read(sourcePath);
  for(const r of j.rows||j.reviewedObjectives||[]) {
   assert(m.has(r.lessonCode),'Earlier purpose review has out-of-scope lesson');
   old.add(r.lessonCode);
  }
 }
 assert.equal(old.size,185);
 for(const r of x.newReviewedRows) {
  assert(m.has(r.lessonCode),'Scope drift: '+r.lessonCode);
  assert(!seen.has(r.lessonCode),'Duplicate new review: '+r.lessonCode);
  assert(!old.has(r.lessonCode),'Double counted previously reviewed: '+r.lessonCode);
  seen.add(r.lessonCode);
  const mrow=m.get(r.lessonCode),srow=si.get(r.lessonCode),pack=l.get(r.lessonCode);
  assert(mrow&&srow&&pack);
  assert.equal(r.ordinal,mrow.ordinal);
  assert.equal(r.sourceSemanticSha256,mrow.sourceSemanticSha256);
  assert.equal(r.sourceFileInventorySha256,srow.localFileByteSha256);
  assert.equal(r.canonicalOriginalLearnerBodySha256,pack.canonicalBodySha256);
  assert.equal(r.originalTeacherSourceUrl,mrow.sourceUrl);
  assert.equal(r.originalTeacherActivityUrl,mrow.sourceUrl.replace('preparation.html','lesson.html'));
  assert(SHA.test(r.liveTeacherPreparationSha256));
  assert(SHA.test(r.liveTeacherActivitySha256));
  assert(r.officialGoalElementsRead>=1 && r.officialTeacherActivityHeadingsRead>=1);
  assert(Array.isArray(r.goalPurposeTags)&&Array.isArray(r.learnerPurposeTags));
  assert.equal(r.reviewScope,'ORIGINAL_GOAL_AND_ACTIVITY_STRUCTURAL_PURPOSE_COMPARISON_ONLY');
  assert.equal(r.effectiveRuntimeAndL1ToL6VerificationStillRequired,true);
  assert.equal(r.academicCertified,false);
  assert(r.goalPurposeCueStatus==='FIRST_PASS_CUE_PRESENT_NOT_ACADEMICALLY_CERTIFIED'||
         r.goalPurposeCueStatus==='REQUIRES_DEEP_SOURCE_REVIEW');
  if(r.reviewWarnings.length)warns++;
 }
 assert.equal(seen.size,315);
 assert.equal(new Set([...seen,...old]).size,500);
 assert.equal(x.priorityReviewCandidates.length,warns);
 assert.equal(x.scope.newlyScannedRowsFlaggedForExtraReview,warns);
 for(const c of x.priorityReviewCandidates) {
  assert(seen.has(c.code));assert(c.warnings.length>0);
  const row=x.newReviewedRows.find(x=>x.lessonCode===c.code);
  assert.deepEqual(row.reviewWarnings,c.warnings);
 }
 return true;
}
test('The ONE B01 manifest joins exactly 185 prior and 315 original-page source-goal/activities reviews, no academic PASS',()=>assert.equal(check(audit),true));
test('Strong negative controls reject missing 500th identity, fake certification, drifted source, duplicate prior or new row',()=>{
 for(const mutation of [
  a=>a.newReviewedRows.pop(),
  a=>a.newReviewedRows.push(a.newReviewedRows[0]),
  a=>a.newReviewedRows[0].lessonCode=manifest.lessons[1].lessonCode,
  a=>a.newReviewedRows[0].sourceSemanticSha256='0'.repeat(64),
  a=>a.newReviewedRows[0].canonicalOriginalLearnerBodySha256='0'.repeat(64),
  a=>a.newReviewedRows[0].liveTeacherActivitySha256='f',
  a=>a.newReviewedRows[0].academicCertified=true,
  a=>a.newReviewedRows[0].reviewScope='L1-L6_CERTIFIED',
  a=>a.certificationDecision='BATCH_CERTIFIED',
  a=>a.newAcademicCertifications=315
 ]) {const t=structuredClone(audit);mutation(t);assert.throws(()=>check(t));}
});