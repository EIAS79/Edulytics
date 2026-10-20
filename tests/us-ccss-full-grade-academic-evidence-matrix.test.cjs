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

test('full-grade 96-lesson source/evidence trace checks actual progress',()=>{
  assert.equal(x.records.length,96);
  assert.deepEqual(x.records.map(z=>z.lessonCode),m.lessons.slice(50).map(y=>y.lessonCode));
  assert.deepEqual(x.records.map(z=>z.ordinal),Array.from({length:96},(_,i)=>i+51));
  assert.equal(x.releaseDecision,state.release);
  for(const z of x.records){
    const lesson=state.newLessons[z.ordinal-51];
    assert.equal(z.internalEvidenceBasedAcademicAcceptance,lesson.status==='INTERNALLY_ACCEPTED');
    assert.ok(Array.isArray(z.evidenceReferences));
    if(lesson.status==='INTERNALLY_ACCEPTED'){
      assert.equal(z.originalStudentTasksFullyReviewed,true);
      assert.equal(z.effectiveAuthenticatedLearnerRenderingVerified,true);
      assert.ok(z.evidenceReferences.length>0);
    }
  }
});
test('student-authored riddle readiness requires teacher rubric without fake classroom results',()=>{
  assert.equal(r.lessonOrdinal,146);
  assert.equal(r.lessonCode,'PED:US-CCSS-MATH:G1:U08:L10');
  assert.ok(r.teacherRubric.some(x=>x.criterion==='Authorship'));
  assert.equal(r.status,'TEACHER_REVIEW_REQUIRED_NOT_CERTIFIED');
  assert.ok(r.teacherRubric.some(x=>x.criterion==='Solution and consistency'));
  if(r.authenticatedStudentWorkVerified!==true){
    assert.equal(r.independentTeacherAcceptance,false);
    assert.equal(r.studentSubmission,null);
  }
  assert.equal(x.records[95].authoredOriginalWorkRubricRequired,true);
  assert.equal(x.records[142-51].authoredOriginalWorkRubricRequired,true);
  assert.equal(x.records[142-51].potentialNonNumericTeacherEvidence,true);
  assert.equal(x.records[14].authoredOriginalWorkRubricRequired,true);
});


test('historic source/HTTPS CI links remain technical-only as individual V1 approvals progress',()=>{
  assert.equal(x.releaseDecision,state.release);
  assert.equal(state.newLessons.length,96);
  const technicalSha='1aabc3a2821d68858fd337b86bbec87673cec2f9';
  for(const row of x.records){
    const historic=row.evidenceReferences.filter(ref=>ref.kind==='HISTORIC_EXACT_SHA_CI_TECHNICAL_ONLY');
    for(const ref of historic){
      assert.equal(ref.testedCodeSha,technicalSha);
      assert.equal(ref.lessonOrdinal,row.ordinal);
      assert.equal(ref.approval,'NOT_CLAIMED');
      assert.match(ref.url,/^https:\/\/github\.com\/EIAS79\/Edulytics\/actions\/runs\/\d+$/);
    }
    if(row.internalEvidenceBasedAcademicAcceptance){
      assert.equal(row.originalStudentTasksFullyReviewed,true);
      assert.equal(row.effectiveAuthenticatedLearnerRenderingVerified,true);
      assert.ok(row.evidenceReferences.some(ref=>ref.kind!=='HISTORIC_EXACT_SHA_CI_TECHNICAL_ONLY'),
        'historic CI-only evidence cannot certify a new lesson');
    }
  }
  if(state.newLessons.some(row=>row.status!=='INTERNALLY_ACCEPTED')){
    assert.equal(state.release,'NO_GO');
  }
});

test('read-only three-file cache source checks cannot be silently forged into lesson approval',()=>{
  const source=read(dir+'G1-SOURCE-ORIGINAL-TASK-PARITY-REMAINING-RISKS.json');
  for(const ordinal of [66,67,68,69,70]){
    const item=source.rows.find(z=>z.ordinal===ordinal);
    const stamp=item.readOnlyOriginal2021SourceIntegrityReverified;
    assert.equal(stamp.status,'THREE_LOCAL_SHA256_MATCH_EXISTING_REGISTRY');
    assert.equal(stamp.verificationDate,'2026-10-19');
    assert.equal(stamp.algorithm,'SHA-256');
    assert.equal(stamp.fullTeacherLessonSha256,item.original2021FullTeacherLessonHtmlSha256);
    assert.equal(stamp.teacherPreparationSha256,item.original2021TeacherPreparationSha256);
    assert.equal(stamp.studentFacingSha256,item.original2021StudentFacingHtmlSha256);
    assert.equal(Object.keys(stamp.originalFiles).length,3);
    assert.equal(item.sourcePublisherArtworkFullyReviewed,false);
    assert.equal(item.actualAuthenticatedTaskRenderingAtExactHeadConfirmed,false);
    assert.equal(item.internalAcademicApproval,'NOT_CLAIMED');
    assert.equal(x.records[ordinal-51].internalEvidenceBasedAcademicAcceptance,
      state.newLessons[ordinal-51].status==='INTERNALLY_ACCEPTED');
  }
});
