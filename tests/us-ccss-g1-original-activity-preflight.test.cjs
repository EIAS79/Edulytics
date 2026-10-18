'use strict';
// Trace source metadata gathered from the READ-ONLY original teacher HTML;
// academic claims require separate per-activity source, Practice and runtime proof.
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const dir=path.resolve(__dirname,'../artifacts/curriculum-delivery/US-CCSS-MATH/G1');
const data=JSON.parse(fs.readFileSync(path.join(dir,'full-grade-original-teacher-activity-preflight.json'),'utf8'));
const manifest=JSON.parse(fs.readFileSync(path.join(dir,'grade-manifest.json'),'utf8'));

test('all 96 original teacher full-lesson pages are independently traceable and ordered',()=>{
  assert.equal(data.planId,'EDULYTICS-GRADE-FIRST-FULL-GRADE-V1');
  assert.equal(data.records.length,96);
  assert.deepEqual(data.records.map(r=>r.ordinal),Array.from({length:96},(_,i)=>i+51));
  for(const r of data.records){
    const original=manifest.lessons[r.ordinal-1];
    assert.equal(r.lessonCode,original.lessonCode);
    assert.equal(r.sourceTeacherPreparationSha256,original.teacherPreparationSha256);
    assert.equal(r.sourceStudentHtmlSha256,original.studentSourceHtmlSha256);
    assert.match(r.sourceFullTeacherLessonSha256,/^[a-f0-9]{64}$/);
    assert.ok(r.sourceActivityCount>=1);
    assert.equal(r.sourceActivityHeadings.length,r.sourceActivityCount);
    assert.ok(r.teacherEmbeddedStudentFacingSections>=1);
  }
});

test('preflight activity index is never mistaken for educational or production approval',()=>{
  assert.equal(data.academicAccepted,0);
  assert.equal(data.release,'NO_GO');
  assert.equal(data.activityCount,data.records.reduce((s,r)=>s+r.sourceActivityCount,0));
  assert.equal(data.teacherEmbeddedStudentFacingSections,
    data.records.reduce((s,r)=>s+r.teacherEmbeddedStudentFacingSections,0));
  assert.equal(data.digitalCandidateFamilyCount,
    data.records.reduce((s,r)=>s+r.candidateDigitalFamilies.length,0));
  assert.deepEqual(data.withoutDigitalFamily,[65]);
  assert.ok(data.records.every(r=>r.preflightStatus==='SOURCE_ACTIVITY_INDEXED_NOT_SOURCE_TASK_PARITY_APPROVED'));
});
