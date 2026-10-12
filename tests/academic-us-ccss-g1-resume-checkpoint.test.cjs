'use strict';
const {test}=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const c=read('artifacts/academic-us-ccss/grade-reports/G01-execution-checkpoint.json');
const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g1-phase29-v1.lesson-content-pack.json');
const t=read('artifacts/academic-us-ccss/grade-reports/G01-146-local-teacher-source-evidence.json');
const s=read('artifacts/academic-us-ccss/grade-reports/G01-146-local-student-html-sources.json');
const proof=read('artifacts/academic-us-ccss/grade-reports/G01-mixed-center-source-guarded-repairs.json');
test('G1 resume ledger has all 146 exact grade-level lesson identities and content SHAs',()=>{
 assert.equal(c.currentGrade,'G1');assert.equal(c.currentCourse,null);
 assert.equal(c.counts.expectedUniqueLessons,146);assert.equal(c.counts.uniqueLessons,146);
 assert.equal(c.records.length,146);
 assert.deepEqual(c.records.map(x=>x.lessonCode),pack.lessons.map(x=>x.lessonCode));
 assert.equal(new Set(c.records.map(x=>x.lessonCode)).size,146);
 assert.equal(c.firstUnresolvedLesson,'PED:US-CCSS-MATH:G1:U01:L01');
 assert.equal(c.counts.sourceGuardedRepairedMerged,25);
 assert.equal(c.counts.sourceGuardedRepairProposalsCurrentPr,4);
 assert.equal(c.counts.completeEightAxisIndependentReviews,0);
 assert.equal(c.counts.independentlyL7Signed,0);
 const T=new Map(t.records.map(x=>[x.lessonCode,x]));
 const S=new Map(s.lessons.map(x=>[x.lessonCode,x]));
 const proposed=new Set(proof.lessons.map(x=>x.lessonCode));
 const axes=['title','standard','explanation','examples','solutions','visuals','misconceptions','questions'];
 const gates=['L1','L2','L3','L4','L5','L6','L7'];
 for(let i=0;i<c.records.length;i++){
  const r=c.records[i],p=pack.lessons[i],a=T.get(r.lessonCode),q=S.get(r.lessonCode);
  assert.ok(a&&q,r.lessonCode);
  assert.equal(r.canonicalRawBodySha256,p.canonicalBodySha256);
  assert.equal(r.originalSourceSha256,p.sourceSha256);
  assert.equal(r.localTeacherSourceSha256,a.teacherPreparationSha256);
  assert.equal(r.localStudentPageSha256,q.studentLocalSha256);
  assert.equal(r.effectiveLearnerBodySha256,null);
  assert.deepEqual(Object.keys(r.eightAxes),axes);
  assert.deepEqual(Object.keys(r.gates),gates);
  assert.equal(r.gates.L7,'PENDING');
  assert.notEqual(r.editorialDecision,'CORRECT_NO_CHANGE');
  assert.ok(Object.values(r.eightAxes).some(x=>x.status!=='PASS'));
  assert.equal(r.guardedCorrection?.state==='COMMITTED_TO_PR_CI_PENDING',proposed.has(r.lessonCode));
 }
});
test('checkpoint cannot silently assert grade completion from publisher source presence or CI',()=>{
 assert.equal(c.gradeContentReviewDecision,'NO-GO');
 assert.equal(c.gradeL7Decision,'PENDING');
 assert.equal(c.counts.contentReviewComplete,0);
 assert.equal(c.counts.blockedL1ToL6,146);
 assert.ok(c.exactNextOperation.includes('mixed-center'));
 assert.ok(c.exactNextOperation.includes('first unresolved G1:U01:L01'));
 assert.ok(c.noAutomaticProductionDeploy);
});