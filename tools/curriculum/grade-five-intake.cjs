#!/usr/bin/env node
'use strict';
// Grade-first, five-lesson intake: deterministic checks against committed source.
// Read-only: never touches Neon, student records, production or GitHub APIs.
const fs=require('node:fs');
const path=require('node:path');
const assert=require('node:assert/strict');
const base=path.resolve(__dirname,'../..');
const manifestPath=path.join(base,'artifacts/curriculum-delivery/US-CCSS-MATH/G1/grade-manifest.json');
const batchPath=path.join(base,'artifacts/curriculum-delivery/US-CCSS-MATH/G1/batches/G1-003.json');
const progressPath=path.join(base,'artifacts/curriculum-delivery/US-CCSS-MATH/G1/progress.json');
function read(p){return JSON.parse(fs.readFileSync(path.join(base,p),'utf8'));}
function validate(){
 const manifest=JSON.parse(fs.readFileSync(manifestPath,'utf8'));
 const batch=JSON.parse(fs.readFileSync(batchPath,'utf8'));
 const progress=JSON.parse(fs.readFileSync(progressPath,'utf8'));
 const {blueprint,content,teacher,student}=manifest.sourcePaths;
 const b=read(blueprint),c=read(content),t=read(teacher),s=read(student);
 const sourceMap=new Map(c.lessons.map(x=>[x.lessonCode,x]));
 const teacherMap=new Map(t.records.map(x=>[x.lessonCode,x]));
 const studentMap=new Map(s.lessons.map(x=>[x.lessonCode,x]));
 assert.equal(manifest.lessons.length,b.Lessons.length);
 assert.equal(b.Lessons.length,146,'Grade 1 source inventory count drift');
 assert.equal(c.lessons.length,146);
 assert.equal(t.records.length,146);
 assert.equal(s.lessons.length,146);
 assert.equal(new Set(manifest.lessons.map(x=>x.lessonCode)).size,146);
 for(let i=0;i<146;i++){
  const m=manifest.lessons[i],q=b.Lessons[i],r=sourceMap.get(q.LessonCode),te=teacherMap.get(q.LessonCode),st=studentMap.get(q.LessonCode);
  assert.equal(m.ordinal,i+1,'Stable grade ordinal drift');
  assert.equal(m.lessonCode,q.LessonCode);
  assert.equal(m.title,q.Title);
  assert.equal(m.unit,q.UnitNumber);
  assert.equal(m.lesson,q.LessonNumber);
  assert.equal(m.publisherSourceUrl,q.SourceUrl);
  assert.equal(m.publisherSemanticSha256,q.SemanticSha256);
  assert.deepEqual(m.outcomeCodes,q.OutcomeCodes);
  assert.equal(m.existingContentSha256,r?.canonicalBodySha256);
  assert.equal(m.teacherGoalExcerpt,te?.teacherGoalExcerpt);
  assert.equal(m.teacherPreparationSha256,te?.teacherPreparationSha256);
  assert.equal(m.studentMirrorSha256,st?.studentLocalSha256);
  assert.ok(m.sourceLicense==='CC BY 4.0');
  assert.equal(m.optional,/\(optional\)/i.test(q.Title));
  if(i<10)assert.equal(m.history?.productionReleased,true);
  else assert.equal(m.history,null);
 }
 assert.equal(batch.lessons.length,5);
 assert.deepEqual(batch.ordinals,[11,12,13,14,15]);
 for(let i=0;i<5;i++){
  const l=batch.lessons[i],m=manifest.lessons[i+10];
  assert.equal(l.lessonCode,m.lessonCode);
  assert.equal(l.sourceGoal,m.teacherGoalExcerpt);
  assert.equal(l.canonicalContentSha256,m.existingContentSha256);
  assert.equal(l.internalAcademicStatus,'NOT_APPROVED');
  assert.notEqual(l.classification,'READY_REUSE','An unreviewed lesson must not be silently approved');
 }
 assert.equal(progress.historicalReleasedLessons,10);
 assert.equal(progress.nextOrdinal,11);
 assert.equal(progress.currentBatchStatus,'REVIEW_STARTED_NOT_ACCEPTED');
 assert.equal(manifest.dbObservation.exactCodeTitleReconciled,true);
 return {grade:manifest.grade,sourceCount:b.Lessons.length,dbObservedCount:manifest.dbObservation.gradeLessonCount,checkedBatch:batch.batchId,lessonCodes:batch.lessons.map(x=>x.lessonCode),status:batch.status};
}
if(require.main===module){try{process.stdout.write(JSON.stringify(validate(),null,2)+'\n');}catch(e){console.error('[G1 INTAKE FAILED]',e.message);process.exitCode=1;}}
module.exports={validate};
