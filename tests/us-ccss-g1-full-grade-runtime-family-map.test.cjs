'use strict';
const {test}=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const ROOT=path.resolve(__dirname,'..');
const read=f=>JSON.parse(fs.readFileSync(path.join(ROOT,f),'utf8'));
const dir='artifacts/curriculum-delivery/US-CCSS-MATH/G1/';
const m=read(dir+'grade-manifest.json');
const x=read(dir+'full-grade-practice-candidate-map.json');
test('all 96 source-ordered Grade 1 new candidates have one honest assessment mode',()=>{
  assert.deepEqual(x.records.map(v=>v.lessonCode),m.lessons.slice(50).map(l=>l.lessonCode));
  assert.deepEqual(x.records.map(v=>v.ordinal),Array.from({length:96},(_,i)=>i+51));
  assert.equal(x.academicAcceptance,'NOT_CLAIMED');
  assert.equal(x.records.filter(v=>v.candidateFamily!==null).length,95);
  assert.equal(x.records[14].assessmentMode,'SOURCE_AUTHORED_TASK');
  assert.equal(x.records[14].candidateFamily,null);
  assert.equal(x.records[95].assessmentMode,'DIGITAL_AND_SOURCE_AUTHORED_TASK');
  assert.equal(x.records[119-51].assessmentMode,'DIGITAL_AND_SOURCE_AUTHORED_TASK');
  assert.equal(x.records[135-51].assessmentMode,'DIGITAL_AND_SOURCE_AUTHORED_TASK');
  for(const ordinal of [123,124,125,126])
    assert.equal(x.records[ordinal-51].assessmentMode,'DIGITAL_AND_SOURCE_AUTHORED_TASK');
});
test('each proposed family maps to one real registered C# contract, never unrelated fallback',()=>{
  const root='src/Edulytics.Core/Mathematics/Practice/';
  const files=fs.readdirSync(path.join(ROOT,root)).filter(n=>/^UsCcssG1FullGrade.*PracticeContracts\.cs$/.test(n));
  const actual=new Map();
  for(const file of files){
    const source=fs.readFileSync(path.join(ROOT,root,file),'utf8');
    const regex=/new\(\s*@?"(PED:US-CCSS-MATH:G1:U\d{2}:L\d{2})"\s*,\s*"[^"]+"\s*,\s*"[^"]+"\s*,\s*\[([^\]]*)\]/g;
    for(const match of source.matchAll(regex)){
      assert.equal(actual.has(match[1]),false,'Duplicate: '+match[1]);
      const families=Array.from(match[2].matchAll(/"([^"]+)"/g),m=>m[1]);
      assert.equal(families.length>0,true,'Empty digital Practice family: '+match[1]);
      assert.equal(new Set(families).size,families.length,'Duplicate family: '+match[1]);
      actual.set(match[1],families);
    }
  }
  assert.equal(actual.size,95);
  for(const row of x.records){
    const planned=row.candidateFamilies??
      (row.candidateFamily===null?[]:[row.candidateFamily]);
    assert.deepEqual(planned,actual.get(row.lessonCode)??[],row.lessonCode);
  }
});
