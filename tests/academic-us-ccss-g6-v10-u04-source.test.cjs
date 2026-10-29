'use strict';
// First-party Grade 6 V10 candidate regression only.
// Requires a full authorized checkout for native node --test execution.
// No assertion below verifies authenticated student UI, exact Practice or release.
const { test } = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const root = path.resolve(__dirname, '..');
const data = JSON.parse(fs.readFileSync(path.join(root,
  'artifacts/curriculum-delivery/US-CCSS-MATH/G6/g6-v10-u04-l11-l14-public-source-task-evidence.json'),'utf8'));
const pack = JSON.parse(fs.readFileSync(path.join(root,
  'src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g6-phase29-v1.lesson-content-pack.json'),'utf8'));
const byCode = new Map(pack.lessons.map(x=>[x.lessonCode,x]));
const code = n=>`PED:US-CCSS-MATH:G6:U04:L${n}`;
const gcd = (a,b)=>b===0n ? (a<0n?-a:a) : gcd(b,a%b);
const fraction = (n,d=1n)=>{
  n=BigInt(n);d=BigInt(d);
  if(d===0n)throw new RangeError('zero denominator');
  if(d<0n){n=-n;d=-d;}
  const g=gcd(n,d);return {n:n/g,d:d/g};
};
const parse = s=>{
  if(typeof s!=='string')throw new TypeError('fraction must be a string');
  const m=/^(-?\d+)(?:\/([1-9]\d*))?$/.exec(s.trim());
  if(!m)throw new SyntaxError('malformed fraction');
  return fraction(m[1],m[2]||1);
};
const div = (a,b)=>{
  const x=parse(a),y=parse(b);
  if(y.n===0n)throw new RangeError('division by zero');
  return fraction(x.n*y.d,x.d*y.n);
};
const eq = (actual,expected)=>assert.deepEqual(actual,parse(expected));
test('U04 L11-L14: source-targeted math oracles and positive/equivalent/negative/malformed cases',()=>{
  eq(div('3/4','1/8'),'6');eq(div('9/10','3/5'),'3/2');
  eq(div('8/9','4'),'2/9');eq(div('3/4','1/2'),'3/2');
  eq(div('10/3','2/9'),'15');eq(div('9/2','3/8'),'12');
  eq(div('32/5','3'),'32/15');eq(div('11/2','2/3'),'33/4');
  eq(div('17/3','4'),'17/12');eq(div('4','17/3'),'12/17');
  eq(div('63/10','9/5'),'7/2');eq(div('31/8','19/2'),'31/76');
  eq(div('7/2','1/4'),'14');
  eq(div('63/8','9/4'),'7/2');eq(div('95/8','19/4'),'5/2');
  eq(div('16','8/3'),'6');eq(div('108/5','18/5'),'6');
  const product=(a,b)=>{const x=parse(a),y=parse(b);return fraction(x.n*y.n,x.d*y.d);};
  eq(product('7/2','9/4'),'63/8');
  eq(product('5/2','19/4'),'95/8');
  eq(product('9/2','9/2'),'81/4');
  eq(product('3/2','9/4'),'27/8');
  eq(product('27/8','4'),'27/2');
  // Equivalent ratios reduce to the same value; wrong reciprocal does not.
  eq(parse('6/8'),'3/4');
  assert.notDeepEqual(div('4','2/5'),parse('2'));
  assert.notDeepEqual(div('31/8','19/2'),parse('76/31'));
  assert.throws(()=>div('1','0'),/division by zero/);
  assert.throws(()=>div('1','2/0'),/malformed fraction/);
  assert.throws(()=>div('1','garbage'),/malformed fraction/);
  assert.throws(()=>div('1',null),/must be a string/);
  assert.throws(()=>parse('1//2'),/malformed fraction/);
});
test('U04 L11-L14: source task artifacts, learner teaching and honest no-go boundaries',()=>{
  assert.equal(pack.lessons.length,147);
  assert.equal(data.summary.newStudentAndTeacherPublicActivityComparisons,4);
  assert.equal(data.summary.currentDirectStudentTaskComparisons,129);
  assert.equal(data.summary.remainingSourceTaskComparisons,18);
  assert.equal(data.summary.academicAccepted,0);
  assert.deepEqual(data.lessons.map(x=>x.lessonCode),[11,12,13,14].map(code));
  for(const row of data.lessons){
    assert.equal(row.publicStudentTaskBodyCompared,true,row.lessonCode);
    assert.equal(row.publicTeacherActivitiesCompared,true,row.lessonCode);
    assert.equal(row.publicTeacherPreparationGoalsChecked,true,row.lessonCode);
    assert.equal(row.originalPublisherProtectedCooldownRead,false,row.lessonCode);
    assert.equal(row.teacherRubricCandidate.implemented,false,row.lessonCode);
    assert.equal(row.teacherRubricCandidate.persisted,false,row.lessonCode);
    assert.equal(row.runtimeSelectedPracticeVerified,false,row.lessonCode);
    assert.equal(row.accessibleAuthenticatedVisualVerified,false,row.lessonCode);
    assert.equal(row.correctWrongEquivalentMalformedGradingVerified,false,row.lessonCode);
    assert.equal(row.tenantIsolationVerified,false,row.lessonCode);
    assert.equal(row.academicAcceptance,'NOT_ACCEPTED',row.lessonCode);
    const en=byCode.get(row.lessonCode).translations.find(t=>t.cultureCode==='en');
    assert.ok(en&&en.workedExamples.includes('Example 3:'),row.lessonCode);
    assert.match(en.workedExamples,/Description: (Tape diagram|Ratio table)/,row.lessonCode);
  }
  assert.match(byCode.get(code(11)).translations[0].workedExamples,/33\/4/);
  assert.match(byCode.get(code(12)).translations[0].workedExamples,/31\/76/);
  assert.match(byCode.get(code(13)).translations[0].workedExamples,/126\/16/);
  assert.match(byCode.get(code(14)).translations[0].workedExamples,/400 small cubes/);
});
