'use strict';
const {test}=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'..');
const read=f=>JSON.parse(fs.readFileSync(path.join(root,f),'utf8'));
const dir='artifacts/curriculum-delivery/US-CCSS-MATH/G1/';
const m=read(dir+'grade-manifest.json'),x=read(dir+'full-grade-academic-evidence-matrix.json'),
    state=read(dir+'full-grade-progress.json'),r=read(dir+'lesson-146-number-riddle-authorship-rubric.json');

test('full-grade new-lesson source-to-evidence trace is exact and NO_GO',()=>{
  assert.equal(x.records.length,96);
  assert.deepEqual(x.records.map(z=>z.lessonCode),m.lessons.slice(50).map(y=>y.lessonCode));
  assert.deepEqual(x.records.map(z=>z.ordinal),Array.from({length:96},(_,i)=>i+51));
  assert.equal(x.releaseDecision,state.release);
  for(const z of x.records){
    const current=state.newLessons[z.ordinal-51];
    assert.equal(z.lessonCode,current.lessonCode);
    if(current.status==='INTERNALLY_ACCEPTED'){
      assert.equal(z.sourceTeacherGoalFullyAudited,true);
      assert.equal(z.originalStudentTasksFullyReviewed,true);
      assert.equal(z.effectiveAuthenticatedLearnerRenderingVerified,true);
      assert.equal(z.independentAcademicAcceptance,true);
      assert.ok(z.evidenceReferences.length>0);
      if(z.ordinal!==65)assert.equal(z.genuineStudentGradingAndPersistenceVerified,true);
    }else{
      assert.notEqual(z.independentAcademicAcceptance,true);
    }
  }
});
test('original authored student riddles need independent artifact and teacher approval',()=>{
  assert.equal(r.lessonOrdinal,146);
  assert.equal(r.lessonCode,'PED:US-CCSS-MATH:G1:U08:L10');
  assert.ok(r.teacherRubric.some(x=>x.criterion==='Authorship'));
  assert.equal(r.authenticatedStudentWorkVerified,false);
  assert.equal(r.independentTeacherAcceptance,false);
  assert.equal(r.teacherEvidenceReference,null);
  assert.equal(r.studentSubmission,null);
  assert.equal(r.readyForGradeRelease,false);
  assert.equal(x.records[95].authoredOriginalWorkRubricRequired,true);
  assert.equal(x.records[14].authoredOriginalWorkRubricRequired,true);
});
