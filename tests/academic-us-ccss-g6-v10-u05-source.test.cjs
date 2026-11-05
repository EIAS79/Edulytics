'use strict';
// Grade 6 source/first-party candidate tests. Full repository checkout needed.
// Not a runtime selected Practice, authenticated content or teacher-workflow PASS.
const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const root = path.resolve(__dirname,'..');
const evidence = JSON.parse(fs.readFileSync(path.join(root,
  'artifacts/curriculum-delivery/US-CCSS-MATH/G6/g6-v10-u05-l01-l05-public-source-task-evidence.json'),'utf8'));
const pack = JSON.parse(fs.readFileSync(path.join(root,
  'src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g6-phase29-v1.lesson-content-pack.json'),'utf8'));
const byCode = new Map(pack.lessons.map(x=>[x.lessonCode,x]));
const lessonCode = n=>`PED:US-CCSS-MATH:G6:U05:L${String(n).padStart(2,'0')}`;
const scale = 10000000n;
function fixed(s) {
  if(typeof s !== 'string'||! /^-?\d+(?:\.\d{1,7})?$/.test(s))throw new SyntaxError('malformed decimal');
  const sign=s.startsWith('-') ? -1n:1n;
  const [whole,frac='']=s.replace(/^-/,'').split('.');
  return sign*(BigInt(whole)*scale+BigInt((frac+'0000000').slice(0,7)));
}
const eq=(v,s)=>assert.equal(v,fixed(s));
test('Grade6 U05 L01-L05 decimal mathematical oracles handle exact cents and sub-millimeter units',()=>{
  eq(2n*(fixed('3.25')+fixed('1.85')+fixed('0.99')),'12.18');
  eq(fixed('3.25')+fixed('1.85')+5n*fixed('0.99'),'10.05');
  eq(fixed('50')/8n,'6.25');
  eq(4n*fixed('6.69'),'26.76');
  eq(2n*fixed('2.99'),'5.98');
  eq(fixed('50')-4n*fixed('6.69')-2n*fixed('2.99'),'17.26');
  eq(fixed('0.03')+fixed('0.05'),'0.08');
  eq(fixed('0.06')+fixed('0.07'),'0.13');
  eq(fixed('0.4')+fixed('0.7'),'1.1');
  eq(fixed('0.26')+fixed('0.07'),'0.33');
  eq(fixed('0.38')+fixed('0.69'),'1.07');
  eq(fixed('1.009')+fixed('0.391'),'1.4');
  eq(fixed('0.007')+fixed('0.004'),'0.011');
  eq(fixed('0.008')+fixed('0.07'),'0.078');
  eq(fixed('0.2')+fixed('0.05'),'0.25');
  assert.notEqual(fixed('2.405'),fixed('2.45'));
  eq(fixed('0.11')+fixed('0.005'),'0.115');
  eq(fixed('0.209')+fixed('0.01'),'0.219');
  eq(fixed('10.2')+fixed('1.1456'),'11.3456');
  eq(fixed('0.4')-fixed('0.03'),'0.37');
  eq(fixed('0.02')-fixed('0.007'),'0.013');
  eq(fixed('5')-fixed('0.17'),'4.83');
  eq(fixed('11.3')-fixed('9.5'),'1.8');
  eq(fixed('318.8')-fixed('94.63'),'224.17');
  eq(fixed('0.02')-fixed('0.0116'),'0.0084');
  eq(fixed('0.6875')-fixed('0.3125'),'0.375');
  eq(fixed('1')-fixed('0.162'),'0.838');
  eq(fixed('0.00007')-fixed('0.0000075'),'0.0000625');
  eq(fixed('1')-fixed('0.404'),'0.596');
  eq(fixed('0.7')-fixed('0.012'),'0.688');
  eq(fixed('7')-fixed('3.4567'),'3.5433');
  eq(fixed('70')-fixed('0.0089'),'69.9911');
  assert.equal(fixed('0.6')*fixed('0.5')/scale,fixed('0.3'));
  assert.equal(fixed('0.3')*fixed('0.02')/scale,fixed('0.006'));
  assert.equal(fixed('0.7')*fixed('0.05')/scale,fixed('0.035'));
  assert.throws(()=>fixed(''),/malformed decimal/);
  assert.throws(()=>fixed('0.2.4'),/malformed decimal/);
  assert.throws(()=>fixed('NaN'),/malformed decimal/);
  assert.throws(()=>fixed('0.12345678'),/malformed decimal/);
});
test('Grade6 U05 L01-L05 first-party source and unresolved A-D gate flags',()=>{
  assert.equal(pack.lessons.length,147);
  assert.equal(evidence.summary.nowReviewed,137);
  assert.equal(evidence.summary.remaining,10);
  assert.equal(evidence.summary.academicAccepted,0);
  assert.deepEqual(evidence.lessons.map(x=>x.lessonCode),[1,2,3,4,5].map(lessonCode));
  for(const l of evidence.lessons){
    const lesson=byCode.get(l.lessonCode);
    assert.ok(lesson);
    const en=lesson.translations.find(x=>x.cultureCode==='en');
    assert.match(en.workedExamples,/Example 3:/);
    assert.match(en.workedExamples,/Description: Ratio table/);
    assert.equal(l.publicStudentTaskBodiesCompared,true);
    assert.equal(l.publicTeacherActivityAndGoalsCompared,true);
    assert.equal(l.originalSourceAppletOrMediaVerified,false);
    assert.equal(l.teacherRubricDraft.implemented,false);
    assert.equal(l.teacherRubricDraft.persisted,false);
    assert.equal(l.sourceProtectedTeacherCooldownRead,false);
    assert.equal(l.effectiveAuthenticatedStudentVisualVerified,false);
    assert.equal(l.actualPracticeSelectionAndVerifierTested,false);
    assert.equal(l.gradingCorrectWrongMalformedEquivalentTested,false);
    assert.equal(l.twoSchoolIsolationVerified,false);
    assert.equal(l.academicAcceptance,'NOT_ACCEPTED');
  }
});
