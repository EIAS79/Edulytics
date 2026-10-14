'use strict';
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const root=path.resolve(__dirname,'..'),base='artifacts/academic-us-ccss/batches/';
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const j=read(base+'B01-v9-subtraction-55-source-goal-activity-purpose-reviewed.json');
const manifest=read(base+'B01-lesson-manifest.json'),inventory=read(base+'B01-book-source-reference-index-500.json');
const packs=[1,2,3,4].map(g=>read('src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g'+g+'-phase29-v1.lesson-content-pack.json'));
const byCode=new Map(manifest.lessons.map(x=>[x.lessonCode,x]));
const bySource=new Map(inventory.rows.map(x=>[x.lessonCode,x]));
const byBody=new Map(packs.flatMap(x=>x.lessons).map(x=>[x.lessonCode,x]));
const sha=/^[0-9a-f]{64}$/;
function validate(src) {
 assert.equal(src.planId,'EDULYTICS-US-CCSS-UNIFIED-V9');
 assert.equal(src.batchId,'B01'); assert.equal(src.academicDecision,'BATCH_NO-GO');
 assert.equal(src.newAcademicCertifications,0);
 assert.equal(src.metadataOnlyNoAutoRepair,true);
 assert.equal(src.sourceReviewedRows.length,55);assert.equal(src.rows.length,51);
 const seen=new Set(),added=new Set(),families=new Set();
 let original=0,stepCount=0;
 for(const r of src.sourceReviewedRows) {
   assert(!seen.has(r.lessonCode),'duplicate source identity');seen.add(r.lessonCode);
   const m=byCode.get(r.lessonCode),s=bySource.get(r.lessonCode),l=byBody.get(r.lessonCode);
   assert(m&&s&&l,'cross manifest boundaries');
   assert.equal(r.ordinal,m.ordinal);
   assert.equal(r.originalTeacherSourceUrl,m.sourceUrl);
   assert.equal(r.originalTeacherActivityUrl,m.sourceUrl.replace('preparation.html','lesson.html'));
   assert.equal(r.sourceSemanticSha256,m.sourceSemanticSha256);
   assert.equal(r.originalArchivedSourceFileByteSha256,s.localFileByteSha256);
   assert.equal(r.originalTeacherLocalRelativePath,s.localRelativePath);
   assert.equal(r.learnerRawBodySha256,l.canonicalBodySha256);
   assert(sha.test(r.liveOfficialPreparationHtmlSha256));
   assert(sha.test(r.liveOfficialActivityHtmlSha256));
   assert.equal(r.originalTeacherLearningGoalsRead,true);
   assert.equal(r.officialTeacherActivityHeadingsInspected,true);
   assert(r.readableOfficialActivityHeadingCount>=2);
   assert.equal(r.sourceGoalAndActivityPurposeReviewed,true);
   assert.equal(r.sharedCauseCorrectionAuthorizedByThisMetadata,false);
   assert.equal(r.lessonAcademicL1toL6CertifiedByThisEvidence,false);
   assert.equal(r.procedureCorrectnessCertified,false);
   assert.equal(r.repeatedMistakesRootMember,true);
   if(r.repeatedStepsRootMember)stepCount++;
   if(r.alreadyReviewedSourcePurposeInPrevious134)original++;
   else added.add(r.lessonCode);
   families.add(r.mathematicalPurposeFamilyFromSourceReview);
 }
 assert.equal(stepCount,53); assert.equal(original,4);assert.equal(added.size,51);assert.equal(families.size,43);
 assert.deepEqual(new Set(src.rows.map(x=>x.lessonCode)),added);
 for(const row of src.rows) {
  const orig=src.sourceReviewedRows.find(x=>x.lessonCode===row.lessonCode);
  assert.equal(row.sourceSemanticSha256,orig.sourceSemanticSha256);
  assert.equal(row.sourceFileInventorySha256,orig.originalArchivedSourceFileByteSha256);
  assert.equal(row.sourcePurposeFamily,orig.mathematicalPurposeFamilyFromSourceReview);
 }
 const repeated=field=> {
  const by=new Map();
  for(const lesson of packs.flatMap(x=>x.lessons)) {
   if(!byCode.has(lesson.lessonCode)) continue;
   const t=lesson.translations.find(x=>x.cultureCode==='en');
   const content=t?.[field];if(typeof content!=='string'||content.length<80) continue;
   if(!by.has(content))by.set(content,[]);
   by.get(content).push(lesson.lessonCode);
  }
  const row=src.sourceReviewedRows[0],t=byBody.get(row.lessonCode).translations.find(x=>x.cultureCode==='en');
  const match=[...by.entries()].find(([text,members])=>members.length===(field==='commonMistakes'?55:53)&&members.every(x=>seen.has(x)));
  assert(match,'missing exact original repeated template');
  const id='REPEATED_'+field.toUpperCase()+'_'+crypto.createHash('sha256').update(field+'\0'+match[0]).digest('hex').slice(0,20);
  assert.equal(src.rootCauseIds[field==='commonMistakes'?'repeatedMistakes':'repeatedSteps'],id);
  assert.deepEqual(new Set(match[1]),new Set(src.sourceReviewedRows.filter(x=>field==='commonMistakes'||x.repeatedStepsRootMember).map(x=>x.lessonCode)));
 };
 repeated('commonMistakes'); repeated('stepByStepSolutions');
 assert.equal(src.scope.totalB01DistinctSourcePurposesReviewed,185);
 assert.equal(src.scope.remainingB01SourcePurposes,315);
 assert.equal(src.scope.repeatedMistakesRawMembers,55);
 assert.equal(src.scope.repeatedStepsRawMembers,53);
 return true;
}
test('One 55-lesson original subtraction root reviewed, exactly 51 new purpose-only sources and no certification',()=>assert.equal(validate(j),true));
test('Guard teacher-source identity, source SHA, family, review state and no counterfeit academic PASS',()=>{
 for(const change of [
  x=>x.sourceReviewedRows[0].sourceSemanticSha256='0'.repeat(64),
  x=>x.sourceReviewedRows[0].learnerRawBodySha256='0'.repeat(64),
  x=>x.sourceReviewedRows[0].liveOfficialActivityHtmlSha256='bad',
  x=>x.sourceReviewedRows[0].sourceGoalAndActivityPurposeReviewed=false,
  x=>x.sourceReviewedRows[0].lessonAcademicL1toL6CertifiedByThisEvidence=true,
  x=>x.sourceReviewedRows[0].sharedCauseCorrectionAuthorizedByThisMetadata=true,
  x=>x.sourceReviewedRows.push(x.sourceReviewedRows[0]),
  x=>x.rows.pop()
 ]){const bad=structuredClone(j);change(bad);assert.throws(()=>validate(bad));}
});