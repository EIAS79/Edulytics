'use strict';
const {test}=require('node:test');
const assert=require('node:assert/strict');
const {readFileSync}=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'..','..'),read=p=>JSON.parse(readFileSync(path.join(root,p),'utf8'));
const teacher=read('src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-geo-v13-human-scored-source-activity-candidates.json');
const oldTeacher=read('src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-geo-v12-human-scored-source-activity-candidates.json');
const fixture=read('artifacts/curriculum-delivery/US-CCSS-MATH/GEO/geo-v13-synthetic-two-school-negative-matrix-20261029.json');
test('V13 adds two independent human-scored source tasks without promoting four existing tasks to runtime',()=>{
 assert.equal(oldTeacher.Contracts.length,4);
 assert.equal(teacher.Contracts.length,2);
 assert.equal(teacher.Status,'COURSE_LOCAL_AUTHORING_ONLY_NOT_RUNTIME_CONNECTED');
 assert.equal(teacher.TeacherSubmissionPersistence,'NOT_IMPLEMENTED');
 const expected=new Set(['U06:L15','U07:L13']);
 for(const t of teacher.Contracts){
   const short=t.LessonCode.slice(-7);assert.ok(expected.delete(short));
   assert.equal(t.TeacherAssessmentRequired,true);
   assert.equal(t.MachineScoringAllowed,false);
   assert.equal(t.TaskLifecycle,'AUTHORING_ONLY_NOT_REGISTERED_NOT_PERSISTED');
   assert.equal(t.Rubric.TotalMax,8);
   assert.equal(t.Rubric.Criteria.reduce((s,r)=>s+r.MaxPoints,0),8);
   assert.ok(t.Rubric.AutoGradeFromScalarProhibited);
   assert.equal(t.IndependentEvidenceOracles.length,3);
   assert.deepEqual(new Set(t.NegativeAndEdgeReviewCases.map(x=>x.Type)),
     new Set(['WRONG','MALFORMED','BOUNDARY','EQUIVALENT']));
   assert.equal(t.AccessAndPersistence.Status,'NOT_TESTED');
   assert.equal(t.Acceptance.StudentSubmit,false);
   assert.equal(t.Acceptance.TeacherReview,false);
   assert.equal(t.Acceptance.SavedGrade,false);
   assert.equal(t.Acceptance.TwoSchoolAuthorization,false);
   assert.equal(t.Acceptance.Approved,false);
 }
 assert.equal(expected.size,0);
 assert.equal(new Set([...teacher.Contracts,...oldTeacher.Contracts].map(t=>t.LessonCode)).size,6);
});
test('synthetic two-school fixture is pure disposable contract and covers required positive/negative cases',()=>{
 assert.equal(fixture.DataClassification,'DISPOSABLE_SYNTHETIC_ONLY');
 assert.match(fixture.ExecutionStatus,/^NOT_RUN/);
 assert.equal(fixture.RequiredEnvironment.Blocked,true);
 assert.equal(fixture.Schools.length,2);
 assert.equal(fixture.Users.length,4);
 assert.equal(fixture.CourseAdoptions.length,3);
 assert.equal(fixture.SyntheticAttempts.length,2);
 assert.ok(fixture.PositiveCases.length>=4);
 assert.ok(fixture.DenialCases.length>=8);
 assert.equal(fixture.PersistenceCases.length,3);
 assert.ok(fixture.PersistenceCases.every(x=>x.Status==='NOT_RUN'));
 assert.equal(new Set(fixture.Users.map(x=>x.Id)).size,4);
 assert.equal(new Set(fixture.CourseAdoptions.map(x=>x.Id)).size,3);
 const ids=new Set([...Object.values(fixture.Ids)]);
 assert.equal(ids.size,Object.keys(fixture.Ids).length);
 const tests=new Set(fixture.DenialCases.map(x=>x.Id));
 for(const id of ['CROSS_SCHOOL_STUDENT_ADOPTION','CROSS_SCHOOL_ATTEMPT_READ','CROSS_SCHOOL_TEACHER_SCORE',
 'WRONG_ALG1_ADOPTION','WRONG_LESSON_B_IN_ATTEMPT_A','WRONG_STUDENT_SAME_SCHOOL',
 'OTHER_STUDENT_OWNERSHIP','OTHER_STUDENT_ATTEMPT_REPLAY'])
 assert.ok(tests.has(id),id);
 for(const c of fixture.DenialCases)assert.equal(c.Expected,'DENY',c.Id);
 // This validates the fixture's policy *intent*, not real server denial.
});
