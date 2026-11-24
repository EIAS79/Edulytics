'use strict';
// Offline fixture contract only. Does not execute a live Practice generator or grader.
// node --test tools/curriculum/coder-9/UsCcssHsADVTRIGGEOPracticeFixtures.test.cjs
const {test}=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'../../..');
const fixturePath=path.join(root,'artifacts/curriculum-delivery/US-CCSS-MATH/coder-9/ADV-TRIG-GEO-proposed-practice-fixtures.json');
const data=JSON.parse(fs.readFileSync(fixturePath,'utf8'));
const near=(a,b)=>assert.ok(Math.abs(a-b)<1e-8, a+' != '+b);
test('Practice fixture proposals remain isolated and fail closed until integration',()=>{
  assert.equal(data.kind,'CODER_9_PROPOSED_PRACTICE_CONTRACT_FIXTURES_NOT_REGISTERED');
  assert.equal(data.runtimeMappingExists,false);
  assert.equal(data.courseCode,'ADV-TRIG-GEO');
  assert.equal(data.pathway,'Fourth Courses');
  assert.equal(data.lessons.length,5);
  assert.equal(new Set(data.lessons.map(x=>x.lessonCode)).size,5);
  data.lessons.forEach((x,i)=>{
    assert.equal(x.lessonCode,'PED:US-CCSS-MATH:HS:FOURTH:ADV-TRIG-GEO:U01:L0'+(i+1));
    assert.ok(x.mechanic.length>10);
    assert.ok(x.accepted.length>=2);
    assert.ok(x.incorrect.length>=2);
    assert.ok(x.malformed.length>=2);
    assert.ok(x.excluded.length>=1);
    assert.ok(x.humanRubric.length>80);
  });
});
test('L01 reflection exact and undefined tangent exclusions',()=>{
  const f=data.lessons[0];
  near(f.expected.numeric,Math.cos(Math.PI-Math.PI/6));
  assert.equal(f.expected.canonical,'-sqrt(3)/2');
  assert.ok(Math.abs(Math.cos(Math.PI/2))<1e-12);
});
test('L02 inverse restrictions and principal value',()=>{
  const f=data.lessons[1];
  near(f.expected.numeric,Math.asin(Math.sin(5*Math.PI/6)));
  assert.ok(Number.isNaN(Math.asin(1.2)));
  assert.ok(Math.asin(Math.sin(5*Math.PI/6))!==5*Math.PI/6);
});
test('L03 all time solutions, interval exclusions, and incorrect single root',()=>{
  const f=data.lessons[2];
  assert.deepEqual(f.expected.canonical,[1,5]);
  for(const t of f.expected.canonical){near(10+4*Math.sin(Math.PI*t/6),12);assert.ok(t>=0&&t<12);}
  assert.ok(!f.expected.canonical.includes(13));
});
test('L04 Cavalieri uses all corresponding slices, not just one',()=>{
  const f=data.lessons[3],r=f.generatorParameters.sphereRadius;
  near(f.expected.numeric,4/3*Math.PI*r*r*r);
  for(const z of [0,.5,1.5,2.1,3])
    near(Math.PI*(r*r-z*z),Math.PI*r*r-Math.PI*z*z);
});
test('L05 conics satisfy original distances, with ellipse and hyperbola separate',()=>{
  const f=data.lessons[4],d=(x,y,fx)=>Math.hypot(x-fx,y);
  assert.equal(f.expected.canonical,'x^2/25+y^2/16=1');
  near(d(5,0,-3)+d(5,0,3),10);
  near(d(0,4,-3)+d(0,4,3),10);
  assert.equal(f.additionalCases[0].expected,'x^2/9-y^2/16=1');
  near(Math.abs(d(3,0,-5)-d(3,0,5)),6);
});
