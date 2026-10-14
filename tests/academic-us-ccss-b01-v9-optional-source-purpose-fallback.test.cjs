'use strict';
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const root=path.join(__dirname,'..');
const load=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const prefix='artifacts/academic-us-ccss/batches/';
test('One source-verified optional Lesson Purpose fills 500 original-purpose sources without inventing Learning Goals',()=>{
 const manifest=load(prefix+'B01-lesson-manifest.json'),
  sourceIndex=load(prefix+'B01-book-source-reference-index-500.json'),
  memory=load(prefix+'B01-v9-source-goal-signal-memory-500.json'),
  fallback=load(prefix+'B01-v9-optional-source-purpose-fallback-reviewed-metadata.json');
 assert.equal(manifest.lessons.length,500);
 assert.equal(sourceIndex.rows.length,500);
 assert.equal(memory.rows.length,500);
 assert.equal(memory.coverage.teacherGoalSectionsFound,499);
 assert.equal(memory.coverage.sourceGoalSectionsUnresolved,1);
 assert.equal(memory.coverage.alternativeLessonPurposeHeadingsVerified,1);
 assert.equal(memory.coverage.sourcePurposeAvailableViaEitherHeading,500);
 assert.equal(memory.coverage.sourcePurposeUnavailable,0);
 assert.equal(fallback.rows.length,1);
 assert.equal(fallback.populationScope,500);
 const alt=memory.rows.filter(x=>x.alternativePurposeHeadingFound);
 assert.equal(alt.length,1);
 assert.equal(alt[0].lessonCode,'PED:US-CCSS-MATH:G2:U02:L16');
 assert.equal(alt[0].teacherGoalSectionFound,false);
 assert.equal(alt[0].originalTeacherGoalStillAbsent,true);
 assert.equal(alt[0].alternativePurposeHeading,'Lesson Purpose');
 assert.ok(alt[0].teacherFacingPurposeSignals.includes('ADDITION'));
 assert.ok(alt[0].teacherFacingPurposeSignals.includes('SUBTRACTION'));
 assert.equal(memory.rows.filter(x=>x.teacherGoalSectionFound).length,499);
 assert.ok(memory.rows.every((x,i)=>{
  const m=manifest.lessons[i],s=sourceIndex.rows[i];
  return x.ordinal===i+1&&m.lessonCode===x.lessonCode&&
   s.lessonCode===m.lessonCode&&x.sourceSemanticSha256===m.sourceSemanticSha256&&
   x.sourceFileInventorySha256===s.localFileByteSha256&&
   (x.teacherGoalSectionFound||x.alternativePurposeHeadingFound);
 }));
 assert.equal(alt[0].sourceSemanticSha256,fallback.rows[0].sourceSemanticSha256);
 assert.equal(alt[0].originalTeacherSourceUrl,fallback.rows[0].sourceUrl);
 assert.equal(fallback.reviewedPurposeOnlyNotL1L6,true);
 assert.equal(fallback.academicDecision,'BATCH_NO-GO');
});
