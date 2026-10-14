'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.join(__dirname,'..'),prefix='artifacts/academic-us-ccss/batches/';
const load=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
function data(){
 return {
  manifest:load(prefix+'B01-lesson-manifest.json'),
  index:load(prefix+'B01-book-source-reference-index-500.json'),
  review:load(prefix+'B01-v9-shared-comparison-procedure-44-source-reviewed-metadata.json'),
  packs:[1,2,3,4].map(g=>load('src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g'+g+'-phase29-v1.lesson-content-pack.json'))
 };
}
function validate({manifest,index,review,packs}){
 assert.equal(review.planId,'EDULYTICS-US-CCSS-UNIFIED-V9');
 assert.equal(review.batchId,'B01');
 assert.equal(review.academicDecision,'BATCH_NO-GO');
 assert.equal(review.newAcademicCertifications,0);
 assert.equal(review.replacementsAuthorizedBeforeIndependentMathExamples,false);
 assert.equal(manifest.lessons.length,500);
 assert.equal(index.rows.length,500);
 assert.equal(review.fullCohortCount,44);
 assert.equal(review.sourceAnalysisRows.length,44);
 assert.equal(review.rows.length,38);
 assert.equal(review.classificationTotals.comparisonRelevant,22);
 assert.equal(review.classificationTotals.mixed,1);
 assert.equal(review.classificationTotals.nonComparison,21);
 assert.equal(review.classificationTotals.stillEffectiveGenericComparisonNeedsReviewAndRepair,20);
 assert.equal(review.classificationTotals.newSourcePurposeReviews,38);
 const originals=new Map(packs.flatMap(p=>p.lessons.map(l=>[l.lessonCode,l])));
 const group=new Set(),newPurposes=new Set();
 for(const r of review.sourceAnalysisRows){
  assert.ok(!group.has(r.lessonCode),'Repeated original source purpose review');
  group.add(r.lessonCode);
  const m=manifest.lessons[r.ordinal-1],source=index.rows[r.ordinal-1],learner=originals.get(r.lessonCode);
  assert.ok(m&&source&&learner);
  assert.equal(m.lessonCode,r.lessonCode);
  assert.equal(source.lessonCode,r.lessonCode);
  assert.equal(m.sourceSemanticSha256,r.sourceSemanticSha256);
  assert.equal(source.localFileByteSha256,r.sourceFileInventorySha256);
  assert.equal(source.localRelativePath,r.sourceOriginalTeacherRelativePath);
  assert.equal(m.sourceUrl,r.sourceUrl);
  assert.equal(learner.canonicalBodySha256.length,64);
  const en=learner.translations.find(t=>t.cultureCode==='en');
  assert.ok(en.stepByStepSolutions.startsWith('Step 1: Identify the quantities or expressions to compare.'));
  assert.equal(r.academicCertification,false);
  if(r.needsSharedCauseRepair)
   assert.equal(r.reviewedGoalFamily,'NON_COMPARISON_GOAL');
  if(!r.reviewedEarlier)newPurposes.add(r.lessonCode);
 }
 assert.equal(group.size,44);
 assert.equal(newPurposes.size,38);
 assert.deepEqual(new Set(review.rows.map(x=>x.lessonCode)),newPurposes);
 assert.equal(review.sourceAnalysisRows.filter(x=>x.reviewedGoalFamily==='NON_COMPARISON_GOAL').length,21);
 assert.equal(review.sourceAnalysisRows.filter(x=>x.needsSharedCauseRepair).length,20);
 assert.equal(review.sourceAnalysisRows.filter(x=>x.reviewedGoalFamily==='COMPARISON_GOAL_RELEVANT').length,22);
 assert.equal(review.sourceAnalysisRows.filter(x=>x.reviewedGoalFamily.startsWith('MIXED_GOAL')).length,1);
}
test('44 source goals are classified together; 20 effective non-comparison procedural mismatches remain blocked',()=>{
 validate(data());
});
test('A missing lesson, swapped source digest, fabricated source evidence, or fake certification fails',()=>{
 const changes=[
  x=>x.review.sourceAnalysisRows[0].sourceSemanticSha256='f'.repeat(64),
  x=>x.review.sourceAnalysisRows[1].sourceOriginalTeacherRelativePath='other.html',
  x=>x.review.rows.pop(),
  x=>x.review.newAcademicCertifications=44,
  x=>x.review.sourceAnalysisRows[5].needsSharedCauseRepair=true,
  x=>x.manifest.lessons.pop()
 ];
 for(const modify of changes){const x=data();modify(x);assert.throws(()=>validate(x));}
});
