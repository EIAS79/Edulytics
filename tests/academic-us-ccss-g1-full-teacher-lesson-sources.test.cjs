'use strict';
const {test}=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),get=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const authentic=get('artifacts/academic-us-ccss/grade-reports/G01-146-full-kendallhunt-teacher-lesson-pages.json');
const bad=get('artifacts/academic-us-ccss/grade-reports/G01-146-student-mirror-404-disposition.json');
const originals=get('artifacts/academic-us-ccss/grade-reports/G01-146-local-teacher-source-evidence.json');
const checkpoint=get('artifacts/academic-us-ccss/grade-reports/G01-execution-checkpoint.json');
test('all Grade 1 teacher activity pages are verified as genuine non-404 HTML sources',()=>{
 assert.equal(authentic.uniqueLessonHtml,146);
 assert.equal(authentic.non404SourcePages,146);
 assert.equal(authentic.records.length,146);
 assert.equal(new Set(authentic.records.map(x=>x.lessonCode)).size,146);
 assert.deepEqual(authentic.records.map(x=>x.lessonCode),originals.records.map(x=>x.lessonCode));
 assert.deepEqual(authentic.records.map(x=>x.lessonCode),checkpoint.records.map(x=>x.lessonCode));
 assert.equal(authentic.records.reduce((a,x)=>a+x.rawImgElements,0),2377);
 assert.equal(authentic.records.reduce((a,x)=>a+x.rawTableElements,0),32);
 for(let i=0;i<146;i++){
  const r=authentic.records[i],original=originals.records[i],checkpointRow=checkpoint.records[i];
  const m=r.lessonCode.match(/:G1:U(\d+):L(\d+)/);
  assert.ok(m,r.lessonCode);
  assert.equal(r.fullTeacherLessonHtmlPath,'unit-'+Number(m[1])+'/lesson-'+Number(m[2])+'/lesson.html');
  assert.match(r.fullTeacherLessonSha256,/^[a-f0-9]{64}$/);
  assert.ok(r.visibleContentChars>=4000,r.lessonCode);
  assert.equal(r.verifiedNon404LessonContent,true);
  assert.equal(r.teacherPreparationSha256,original.teacherPreparationSha256);
  assert.equal(r.studentExercisesVerified,false);
  assert.equal(r.imageRenderOrMathAccuracyVerified,false);
  assert.equal(checkpointRow.fullTeacherLessonSourceAvailable,true);
  assert.equal(checkpointRow.fullTeacherLessonSourceSha256,r.fullTeacherLessonSha256);
 }
});
test('full teacher lesson source does not erase invalid student mirror or fabricate approvals',()=>{
 assert.equal(bad.archivedPagesShowing404,146);
 assert.equal(checkpoint.counts.localStudentHtml404Shells,146);
 assert.equal(checkpoint.counts.authenticFullKendallHuntTeacherLessonPages,146);
 assert.equal(checkpoint.counts.studentOriginalProblemAnswerProofFromTheseTeacherFiles,0);
 assert.equal(checkpoint.counts.contentReviewComplete,0);
 assert.equal(checkpoint.counts.independentlyL7Signed,0);
 assert.equal(checkpoint.gradeContentReviewDecision,'NO-GO');
});