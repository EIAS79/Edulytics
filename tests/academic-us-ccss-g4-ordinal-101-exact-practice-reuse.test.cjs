'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const repo=path.join(__dirname,'..');
const data=JSON.parse(fs.readFileSync(path.join(repo,'artifacts/curriculum-delivery/US-CCSS-MATH/G4/lesson-101-2021-exact-practice-contract.json'),'utf8'));
test('2021 lesson 101 exact rectangle side cases',()=>{
 assert.equal(data.edition,'1st Edition (2021)');
 assert.equal(data.lessonCode,'PED:US-CCSS-MATH:G4:U06:L15');
 for(const e of data.numericPositiveExamples)assert.equal(e.area/e.width,e.answer);
 assert.equal(197,32*6+5);
 assert.equal(data.allowedQuestionFamiliesActivated.length,0);
});
test('Existing exact family covers divisible missing side, not contextual remainder',()=>{
 const s=fs.readFileSync(path.join(repo,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'),'utf8');
 const f=JSON.parse(fs.readFileSync(path.join(repo,'src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json'),'utf8'));
 const family=f.families.find(x=>x.id==='geometry.perimeter_area.rectangle_missing_side');
 assert.ok(family);assert.equal(family.lessonPracticeRouting,true);
 assert.match(s,/BuildRectangleMissingSide\(random, scale\)/);
 assert.match(s,/parameters\["area"\] % parameters\["knownSide"\] == 0/);
 for(const x of data.numericNegativeCases)assert.equal(x.shouldAccept,false);
 const cp=JSON.parse(fs.readFileSync(path.join(repo,'artifacts/curriculum-delivery/US-CCSS-MATH/G4/lesson-101-v2-stage-a-d-checkpoint.json'),'utf8'));
 assert.equal(cp.readyForStageD,false);assert.equal(cp.focusedTestsRun,false);
});
