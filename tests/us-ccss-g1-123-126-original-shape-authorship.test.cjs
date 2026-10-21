'use strict';
const {test}=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'..');
const file=p=>fs.readFileSync(path.join(root,p),'utf8');
const json=p=>JSON.parse(file(p));
const prefix='artifacts/curriculum-delivery/US-CCSS-MATH/G1/';
const source=json(prefix+'full-grade-original-student-facing-task-inventory.json').rows;
const map=json(prefix+'full-grade-practice-candidate-map.json').records;
const matrix=json(prefix+'full-grade-academic-evidence-matrix.json').records;
const content=file('src/Edulytics.Data/Seeding/CommonCoreG1SourceGroundedInstructionalCorrections.cs')
             +file('src/Edulytics.Data/Seeding/CommonCoreG1FullGradeUnit6To8ContentCorrections.cs');
test('original first-edition geometry drawing authorship cannot masquerade as a numeric answer',()=>{
 const checks={123:/EACH partner must DRAW.*dot paper/,124:/DRAW THREE DIFFERENT TRIANGLES/,
  125:/DRAW FIVE rectangles/,126:/BUILD a new flat figure with pattern blocks, TRACE/};
 for(const [number,regex] of Object.entries(checks)){
  const ordinal=Number(number);
  const input=source.find(x=>x.ordinal===ordinal);
  const practice=map.find(x=>x.ordinal===ordinal);
  const row=matrix.find(x=>x.ordinal===ordinal);
  const rubric=json(prefix+'lesson-'+ordinal+'-original-shape-creation-rubric.json');
  assert.equal(rubric.lessonOrdinal,ordinal);
  assert.equal(rubric.lessonCode,input.lessonCode);
  assert.equal(rubric.lessonCode,practice.lessonCode);
  assert.equal(rubric.originalStudentFacingSourceSha256,input.original2021StudentFacingHtmlSha256);
  assert.equal(rubric.status,'TEACHER_REVIEW_REQUIRED_NOT_CERTIFIED');
  assert.equal(rubric.digitalPracticeFamily,practice.candidateFamily);
  assert.equal(practice.assessmentMode,'DIGITAL_AND_SOURCE_AUTHORED_TASK');
  assert.equal(row.authoredOriginalWorkRubricRequired,true);
  assert.ok(rubric.teacherRubric.some(x=>x.criterion==='Authorship'));
  assert.ok(rubric.teacherRubric.some(x=>x.criterion==='Math accuracy'));
  assert.equal(rubric.academicInternalApproval,'NOT_CLAIMED');
  assert.equal(rubric.teacherObservationClaimed,false);
  assert.equal(rubric.actualAuthenticatedLearnerAuthoredWorkVerified,false);
  assert.match(content,regex);
  if(ordinal===124){
    assert.match(content,/DRAW THREE other closed or open figures that are NOT triangles/);
    assert.ok(rubric.teacherRubric.some(x=>x.criterion==='Three source nonexamples'));
  }
  if(ordinal===125){
    assert.match(content,/Draw THREE figures that are NOT rectangles/);
    assert.ok(rubric.teacherRubric.some(x=>x.criterion==='Three source nonexamples'));
  }
  if(ordinal===126){
    assert.match(content,/SAME resulting figure again with DIFFERENT pattern blocks/);
    assert.ok(rubric.teacherRubric.some(x=>x.criterion==='Same-outline alternate construction'));
  }
 }
});
