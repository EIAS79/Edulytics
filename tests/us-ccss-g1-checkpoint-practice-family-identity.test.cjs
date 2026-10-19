'use strict';

const {test}=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');

const ROOT=path.resolve(__dirname,'..');
const DIR=path.join(ROOT,'artifacts/curriculum-delivery/US-CCSS-MATH/G1');
const read=p=>JSON.parse(fs.readFileSync(path.join(DIR,p),'utf8'));
const map=read('full-grade-practice-candidate-map.json');
const families=new Map(map.records.map(row=>[row.ordinal,row]));

function sourceFamilyValues(value,result=[]) {
  if(typeof value==='string'){
    if(value.startsWith('usccss.g1.'))result.push(value);
  }else if(Array.isArray(value)){
    for(const x of value)sourceFamilyValues(x,result);
  }else if(value&&typeof value==='object'){
    for(const x of Object.values(value))sourceFamilyValues(x,result);
  }
  return result;
}

test('all source-reviewed G1 V1 checkpoint Practice references use the real registered family',()=>{
  assert.equal(map.records.length,96);
  const files=fs.readdirSync(DIR).filter(name=>/^lesson-\d+-v2-stage-a-d-checkpoint\.json$/.test(name));
  assert.ok(files.length>=58,'Earlier Grade-1 source checkpoints must not disappear');
  for(const file of files){
    const row=read(file);
    const num=Number(file.match(/^lesson-(\d+)-/)[1]);
    const official=families.get(num);
    assert.ok(official,'Unknown G1 review checkpoint ordinal '+num);
    assert.equal(row.ordinal,num,'Wrong ordinal in '+file);
    assert.equal(row.lessonCode,official.lessonCode,'Wrong canonical code in '+file);
    for(const candidate of sourceFamilyValues(row.stageC))
      assert.equal(candidate,official.candidateFamily,
        'Source-task checkpoint references a nonexistent/wrong Practice family in '+file);
  }
});

test('source-authored G1 teacher rubrics never invent a digital Practice family',()=>{
  const files=fs.readdirSync(DIR).filter(name=>/^lesson-\d+-.+-rubric\.json$/.test(name));
  assert.ok(files.length>=9,'Existing Grade-1 authoring rubrics must be retained');
  for(const file of files){
    const row=read(file);
    const num=row.lessonOrdinal??row.ordinal;
    const official=families.get(num);
    assert.ok(official,'Unknown G1 original learner-authoring rubric '+file);
    assert.equal(row.lessonCode,official.lessonCode,'Wrong canonical code in '+file);
    const registered=row.digitalPracticeFamily??row.practiceFamily;
    if(registered!==undefined&&registered!==null)
      assert.equal(registered,official.candidateFamily,
        'Teacher review rubric digital family differs from the actual source map: '+file);
    // Original real pupil work, teacher observation and academic signoff are
    // not proven simply by a prepared rubric or by this metadata identity check.
  }
});
