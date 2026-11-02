'use strict';
const {test}=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'..');
const dir='artifacts/curriculum-delivery/US-CCSS-MATH/G1/';
const load=n=>JSON.parse(fs.readFileSync(path.join(root,dir+n),'utf8'));
const manifest=load('grade-manifest.json');
const inventory=load('full-grade-original-student-facing-task-inventory.json');
test('96 new original 2021 source teacher/student HTML pages match canonical manifest hashes',()=>{
  assert.equal(inventory.inventoryRows,96);
  assert.equal(inventory.actualAcademicAcceptedRows,0);
  assert.equal(inventory.rows.length,96);
  for(let i=0;i<96;i++){
    const row=inventory.rows[i],known=manifest.lessons[i+50];
    assert.equal(row.ordinal,i+51);
    assert.equal(row.lessonCode,known.lessonCode);
    assert.equal(row.original2021TeacherPreparationSha256,known.teacherPreparationSha256);
    assert.equal(row.original2021StudentFacingHtmlSha256,known.studentSourceHtmlSha256);
    assert.match(row.original2021FullTeacherLessonHtmlSha256,/^[0-9a-f]{64}$/);
    assert.ok(row.studentFacingExcerptCount>0,'Missing original Student Facing activity excerpt ordinal '+row.ordinal);
    assert.ok(Array.isArray(row.teacherActivityHeadings));
    assert.equal(row.internalAcademicApproval,'NOT_CLAIMED');
    assert.equal(row.originalStudentPdfArtworkInspected,false);
  }
});
test('original source inventory never includes historic Grade1 ordinals 1-50',()=>{
  assert.equal(inventory.rows[0].ordinal,51);
  assert.equal(inventory.rows[95].ordinal,146);
  assert.equal(inventory.rows.find(x=>x.ordinal===65).lessonCode,'PED:US-CCSS-MATH:G1:U03:L28');
  assert.equal(inventory.rows.find(x=>x.ordinal===146).lessonCode,'PED:US-CCSS-MATH:G1:U08:L10');
});
