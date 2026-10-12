'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const get=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const student=get('artifacts/academic-us-ccss/grade-reports/G01-146-local-student-html-sources.json');
const teacher=get('artifacts/academic-us-ccss/grade-reports/G01-146-local-teacher-source-evidence.json');
const pack=get('src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g1-phase29-v1.lesson-content-pack.json');
test('146 distinct Grade 1 student mirrors match official teacher source lesson inventory',()=>{
 assert.equal(student.studentPagesPresent,146);
 assert.equal(student.sourceStudentPagesHashed,146);
 assert.equal(student.originalTeacherGoalsMapped,146);
 assert.equal(student.missingStudentPages,0);
 assert.equal(student.academicApprovalCount,0);
 assert.equal(student.staticSourcePresenceDoesNotProveVisualRendering,true);
 assert.equal(student.lessons.length,146);
 assert.deepEqual(student.lessons.map(x=>x.lessonCode),teacher.records.map(x=>x.lessonCode));
 assert.deepEqual(student.lessons.map(x=>x.lessonCode),pack.lessons.map(x=>x.lessonCode));
 assert.equal(new Set(student.lessons.map(x=>x.lessonCode)).size,146);
 for(const r of student.lessons){
  const m=r.lessonCode.match(/:U(\d+):L(\d+)$/);
  assert.ok(m,r.lessonCode);
  assert.equal(r.studentHtml,'unit-'+Number(m[1])+'/lesson-'+Number(m[2])+'/lesson.html');
  assert.match(r.studentLocalSha256,/^[a-f0-9]{64}$/);
  assert.ok(r.byteLength>200,r.lessonCode);
  assert.equal(r.studentMathContentReviewed,'NO');
  assert.equal(r.visualsValidated,'NO');
  assert.equal(r.questionsAndRubricsValidated,'NO');
  assert.equal(r.edulyticsRichV2Parity,'NO');
  assert.equal(r.independentL7Approval,'PENDING');
 }
});