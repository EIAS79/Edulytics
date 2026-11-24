'use strict';
// Coder 9: scope-only offline checks, no database, auth, or network.
// Run: node --test tools/curriculum/coder-9/UsCcssHsADVTRIGGEO5LESSONS.test.cjs
const { test } = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const root = path.resolve(__dirname, '../../..');
function load(rel) { return JSON.parse(fs.readFileSync(path.join(root, rel), 'utf8')); }
const pack = load('src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-hs-adv-trig-geo-phase29-v1.lesson-content-pack.json');
const bp = load('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/us-ccss-math-hs-fourth-advanced-trigonometry-geometry.lesson-blueprint.json');
function near(a, b) { assert.ok(Math.abs(a-b)<1e-9, String(a) + ' != ' + String(b)); }
const outcomes = [
  ['CCSS:HSF-TF.A.3','CCSS:HSF-TF.A.4'], ['CCSS:HSF-TF.B.6'],
  ['CCSS:HSF-TF.B.7'], ['CCSS:HSG-GMD.A.2'], ['CCSS:HSG-GPE.A.3']
];
test('course identity, ordered manifest and immutable source provenance', () => {
  assert.equal(pack.packCode,'US-CCSS-MATH');
  assert.equal(pack.versionCode,'CCSSM-2010');
  assert.equal(bp.BlueprintCode,'US-CCSS-MATH:HS:FOURTH:ADV-TRIG-GEO');
  assert.equal(bp.CourseCode,'ADV-TRIG-GEO');
  assert.equal(bp.Pathway,'Fourth Courses');
  assert.equal(bp.Units.length,1);
  assert.equal(bp.Lessons.length,5);
  assert.equal(pack.lessons.length,5);
  for (let i=0;i<5;i++) {
    const id='PED:US-CCSS-MATH:HS:FOURTH:ADV-TRIG-GEO:U01:L0'+(i+1);
    assert.equal(bp.Lessons[i].LessonCode,id);
    assert.equal(pack.lessons[i].lessonCode,id);
    assert.deepEqual(pack.lessons[i].outcomeCodes,outcomes[i]);
    assert.deepEqual(bp.Lessons[i].FormalTargets.map(x=>x.OutcomeCode),outcomes[i]);
    assert.match(pack.lessons[i].sourceSha256,/^[a-f0-9]{64}$/);
    assert.equal(pack.lessons[i].translations[0].cultureCode,'en');
  }
});
test('no observed PDF extraction, navigation or teacher-logistics contamination',()=>{
  const bad=/READY, SET, GO|Homework:|The Teaching Cycle|Launch \(Whole Class\)|Common mistake to avoid: ignoring|ed to the volume of the cube\?|Step 1: The amusement park|Explore \(Small Group\)/i;
  const keys=['explanation','keyConceptsAndRules','workedExamples','stepByStepSolutions','commonMistakes','quickSummary'];
  for(const lesson of pack.lessons) {
    assert.equal(lesson.translations.length,1);
    const tr=lesson.translations[0];
    for(const key of keys) {
      assert.ok(tr[key].length>250,lesson.lessonCode+':'+key);
      assert.doesNotMatch(tr[key],bad,lesson.lessonCode+':'+key);
      assert.doesNotMatch(tr[key],/[\u0000-\u0008\u000B\u000C\u000E-\u001F]/);
    }
  }
});
test('special-angle values and transformed unit-circle identities',()=>{
  near(Math.sin(Math.PI/6),0.5);
  near(Math.cos(Math.PI/6),Math.sqrt(3)/2);
  near(Math.tan(Math.PI/4),1);
  for(const x of [-14,-4,-.4,0,.2,2.1,9]) {
    near(Math.sin(Math.PI-x),Math.sin(x));
    near(Math.cos(Math.PI-x),-Math.cos(x));
    near(Math.sin(Math.PI+x),-Math.sin(x));
    near(Math.cos(Math.PI+x),-Math.cos(x));
    near(Math.sin(2*Math.PI-x),-Math.sin(x));
    near(Math.cos(2*Math.PI-x),Math.cos(x));
    near(Math.sin(-x),-Math.sin(x));
    near(Math.cos(-x),Math.cos(x));
    near(Math.sin(x+2*Math.PI),Math.sin(x));
  }
});
test('tangent excludes poles and repeats after pi',()=>{
  for(const x of [-2.3,-.8,0,.8,2.3]) near(Math.tan(x+Math.PI),Math.tan(x));
  near(Math.cos(Math.PI/2),0);
});
test('inverse principal-value and real-domain counterexamples',()=>{
  near(Math.asin(Math.sin(5*Math.PI/6)),Math.PI/6);
  near(Math.acos(-.5),2*Math.PI/3);
  near(Math.atan(-1),-Math.PI/4);
  assert.equal(Number.isNaN(Math.asin(1.2)),true);
  assert.equal(Number.isNaN(Math.acos(-1.3)),true);
});
test('inverse solving with identity and contextual time model',()=>{
  for(const theta of [Math.PI/6,5*Math.PI/6]){
    near(Math.cos(Math.PI/2-theta),.5);
    const t=6*theta/Math.PI;
    near(10+4*Math.sin(Math.PI*t/6),12);
  }
});
test('Cavalieri matching cross sections and sphere volume',()=>{
  const r=3;
  for(const z of [0,.1,.5,1,1.5,2.5,3])
    near(Math.PI*(r*r-z*z),Math.PI*r*r-Math.PI*z*z);
  near(4/3*Math.PI*r*r*r,36*Math.PI);
});
test('ellipse and hyperbola obey original focal distance conditions',()=>{
  const distance=(x,y,fx)=>Math.hypot(x-fx,y);
  for(const [x,y] of [[5,0],[-5,0],[0,4]]) {
    near(x*x/25+y*y/16,1);
    near(distance(x,y,-3)+distance(x,y,3),10);
  }
  for(const [x,y] of [[3,0],[-3,0],[5,16/3],[-5,-16/3]]) {
    near(x*x/9-y*y/16,1);
    near(Math.abs(distance(x,y,-5)-distance(x,y,5)),6);
  }
});
test('five specific topics appear in their own lesson, not an unrelated lesson',()=>{
  const words=[['unit circle','periodicity'],['arcsin','principal'],
    ['inverse','model'],['Cavalieri','sphere'],['ellipse','hyperbola']];
  pack.lessons.forEach((lesson,i)=>{
    const body=Object.values(lesson.translations[0]).join(' ').toLowerCase();
    for(const word of words[i]) assert.ok(body.includes(word.toLowerCase()),lesson.lessonCode+':'+word);
  });
});
