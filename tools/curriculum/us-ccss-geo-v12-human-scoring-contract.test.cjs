'use strict';
const {test}=require('node:test');
const assert=require('node:assert/strict');
const {readFileSync}=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'..','..');
const raw=file=>readFileSync(path.join(root,file),'utf8');
const doc=JSON.parse(raw('src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-geo-v12-human-scored-source-activity-candidates.json'));
const mapping=JSON.parse(raw('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json'));
const prefix='PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:';

test('four source-specific teacher-assessed authoring contracts are NOT active runtime grading',()=>{
  assert.equal(doc.Contracts.length,4);
  assert.equal(doc.Status,'COURSE_LOCAL_AUTHORED_EVIDENCE_NOT_RUNTIME_CONNECTED');
  assert.equal(doc.TeacherSubmissionPersistence,'NOT_IMPLEMENTED');
  assert.equal(doc.NotReleaseApproval,true);
  const expected=new Set(['U06:L03','U07:L02','U07:L12','U08:L04']);
  for(const task of doc.Contracts){
    assert.ok(task.LessonCode.startsWith(prefix));
    assert.ok(expected.delete(task.LessonCode.slice(prefix.length)),task.LessonCode);
    assert.equal(task.MachineScoringAllowed,false,task.TaskId);
    assert.equal(task.TeacherAssessmentRequired,true,task.TaskId);
    assert.equal(task.TaskLifecycle,'AUTHORING_ONLY_NOT_REGISTERED_NOT_PERSISTED');
    assert.equal(task.Rubric.Criteria.length,4);
    assert.equal(task.Rubric.TotalMax,8);
    assert.equal(task.Rubric.AutoGradeFromScalarProhibited,true);
    assert.equal(task.Rubric.Criteria.reduce((sum,x)=>sum+x.MaxPoints,0),8);
    assert.deepEqual(Object.values(task.Acceptance),[false,false,false,false,false]);
    assert.ok(task.SourceStudentUrl.startsWith('https://im.kendallhunt.com/HS/students/2/'));
    assert.ok(task.EvidenceModes.includes('WRITTEN_PROOF_OR_EXPLANATION'));
  }
  assert.equal(expected.size,0);
});

test('manual cases include positive independent oracle, wrong, malformed, boundary and equivalent evidence',()=>{
  for(const task of doc.Contracts){
    assert.ok(task.IndependentEvidenceOracles.length>=2);
    assert.ok(task.IndependentEvidenceOracles.every(x=>x.Input&&x.Expected&&x.Scope==='NUMERICAL_SUBCOMPONENT_ONLY_NOT_OVERALL_SCORE'));
    const cases=new Set(task.NegativeAndEdgeReviewCases.map(x=>x.Type));
    assert.deepEqual(cases,new Set(['WRONG','MALFORMED','BOUNDARY','EQUIVALENT']));
    assert.ok(task.NegativeAndEdgeReviewCases.every(x=>x.Trigger&&x.ExpectedHandling));
    assert.ok(task.AccessAndPersistence.SchoolTenantIsolationRequired);
    assert.ok(task.AccessAndPersistence.SameLessonCodeRequired);
    assert.equal(task.AccessAndPersistence.Status,'NOT_TESTED');
  }
});

test('current central exact Geometry Practice projection needs separately approved mapping and READY_VERIFIED',()=>{
  const geoMappings=mapping.mappings.filter(x=>x.lessonCode?.startsWith(prefix));
  assert.equal(geoMappings.length,0,'snapshot gap must be separately reviewed before activation');
  const source=raw('src/Edulytics.Core/Mathematics/Practice/LessonPracticeContractProjection.cs');
  const resolver=raw('src/Edulytics.Core/Mathematics/Practice/LessonPracticeCapabilityResolver.cs');
  const registry=raw('src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs');
  assert.ok(source.includes('"READY_VERIFIED"'));
  assert.ok(source.includes('lessonPracticeRouting'));
  assert.ok(source.includes('verificationPolicy'));
  assert.ok(resolver.includes('ReadyVerified'));
  assert.ok(registry.includes('ByLessonCode.TryGetValue'));
  // This is source-level evidence, not an authenticated generator/grade/persistence test.
});
