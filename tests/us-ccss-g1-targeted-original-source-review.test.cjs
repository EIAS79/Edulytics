'use strict';
const {test}=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'..');
const dir='artifacts/curriculum-delivery/US-CCSS-MATH/G1/';
const read=p=>JSON.parse(fs.readFileSync(path.join(root,dir,p),'utf8'));
const manifest=read('grade-manifest.json');
const audit=read('G1-SOURCE-STUDENT-TASK-AUDIT-51-52-53-65-146.json');
test('genuine source-task audit covers only five explicitly inspected new Grade 1 lessons',()=>{
  assert.deepEqual(audit.records.map(x=>x.ordinal),[51,52,53,65,146]);
  assert.equal(audit.academicApprovalCount,0);
  assert.equal(audit.historicalGrade1Ordinals1To50,'EXCLUDED_UNCHANGED');
  for(const row of audit.records){
    const original=manifest.lessons[row.ordinal-1];
    assert.equal(row.lessonCode,original.lessonCode);
    assert.equal(row.teacherPreparationSha256,original.teacherPreparationSha256);
    assert.equal(row.originalStudentFacingHtmlSha256,original.studentSourceHtmlSha256);
    assert.match(row.fullOriginalTeacherLessonHtmlSha256,/^[0-9a-f]{64}$/);
    assert.ok(row.teacherAndStudentActivity.length>80);
    assert.ok(row.assessmentGap.length>50);
    assert.equal(row.internalAcademicAcceptance,'NOT_CLAIMED');
  }
});
test('original authored source task distinctions survive without false digital acceptance',()=>{
  const by=new Map(audit.records.map(x=>[x.ordinal,x]));
  assert.match(by.get(51).teacherAndStudentActivity,/digit cards/i);
  assert.match(by.get(52).teacherAndStudentActivity,/three addends/i);
  assert.match(by.get(53).teacherAndStudentActivity,/equivalent 10\+box/i);
  assert.equal(by.get(65).candidateDigitalFamily,null);
  assert.match(by.get(146).teacherAndStudentActivity,/1-120/);
  assert.match(by.get(146).teacherAndStudentActivity,/FOUR clues/);
});
