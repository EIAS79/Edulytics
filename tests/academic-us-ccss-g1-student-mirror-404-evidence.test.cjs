'use strict';
const {test}=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),load=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const result=load('artifacts/academic-us-ccss/grade-reports/G01-146-student-mirror-404-disposition.json');
const student=load('artifacts/academic-us-ccss/grade-reports/G01-146-local-student-html-sources.json');
const checkpoint=load('artifacts/academic-us-ccss/grade-reports/G01-execution-checkpoint.json');
test('all 146 student mirror HTML source pages are correctly rejected as 404 shells',()=>{
 assert.equal(result.matchedArchiveFiles,146);
 assert.equal(result.archivedPagesShowing404,146);
 assert.equal(result.verifiedStudentLessonPages,0);
 assert.equal(result.studentMathOrAnswerKeysVerified,0);
 assert.equal(result.richV2ParityVerified,0);
 assert.equal(result.records.length,146);
 assert.equal(new Set(result.records.map(x=>x.lessonCode)).size,146);
 assert.deepEqual(result.records.map(x=>x.lessonCode),student.lessons.map(x=>x.lessonCode));
 for(let i=0;i<146;i++){
  const r=result.records[i],s=student.lessons[i],c=checkpoint.records[i];
  assert.equal(r.studentPageSha256,s.studentLocalSha256);
  assert.equal(r.studentPageSha256,c.localStudentPageSha256);
  assert.equal(r.archivedPageShows404,true,r.lessonCode);
  assert.equal(r.mainElementCount,0,r.lessonCode);
  assert.equal(r.visibleStudentTaskMarkerCount,0,r.lessonCode);
  assert.equal(r.contentDisposition,'HTTP_404_SITE_SHELL_NOT_AUTHENTIC_STUDENT_LESSON');
  assert.equal(c.studentMirrorLessonContentAvailable,false);
  assert.equal(c.studentMirrorDisposition,r.contentDisposition);
  assert.ok(c.blockers.includes('ALL_ARCHIVED_STUDENT_HTML_404_SITE_SHELL_NOT_LESSON_TASK'));
  assert.equal(c.eightAxes.visuals.status,'BLOCKED_MISSING_EVIDENCE');
  assert.equal(c.eightAxes.questions.status,'BLOCKED_MISSING_EVIDENCE');
  assert.equal(c.gates.L5,'BLOCKED');
  assert.equal(c.gates.L6,'BLOCKED');
 }
});
test('grade content review and academic signoff remain NO-GO',()=>{
 assert.equal(checkpoint.currentGrade,'G1');
 assert.equal(checkpoint.counts.localStudentHtml404Shells,146);
 assert.equal(checkpoint.counts.localStudentHtmlFilesContainingRealStudentTasks,0);
 assert.equal(checkpoint.counts.independentlyL7Signed,0);
 assert.equal(checkpoint.counts.contentReviewComplete,0);
 assert.equal(checkpoint.gradeContentReviewDecision,'NO-GO');
 assert.equal(checkpoint.gradeL7Decision,'PENDING');
});