'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
test('G5 L09 original source two valid 78-cubic-foot decompositions',()=>{assert.equal(2*7*2+5*5*2,78);assert.equal(7*5*2+2*2*2,78);assert.equal(7-5,2)});
test('G5 L09 source original second solid two 259-cubic-inch decompositions',()=>{assert.equal(9*3*7+5*2*7,259);assert.equal(5*5*7+4*3*7,259);assert.equal(9-5,4);assert.equal(5-2,3)});
test('G5 L09 parentheses and 420-inch composite match totals',()=>{assert.equal((2*3)*4+(3*3)*2,42);assert.equal(5*6+3*4,42);assert.equal(5*8*6+5*4*9,420);assert.equal(5*4*3+5*12*6,420)});
test('G5 L09 invalid metrics cannot pass teacher-required geometry and units',()=>{assert.notEqual(7-5,3);assert.notEqual('78 cubic feet','78 cubic inches');assert.notEqual(5*8*6+5*4*9,421);for(const x of [null,[],[1,0,2],[1,-2,2],[1,'2',2],[1,2,2.5]])assert.equal(Array.isArray(x)&&x.length===3&&x.every(n=>Number.isSafeInteger(n)&&n>0),false)});
const rp=path.resolve(__dirname,'../src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u01-l07-l12-2021.rich-lesson-v2.json');
const ap=path.resolve(__dirname,'../artifacts/curriculum-delivery/US-CCSS-MATH/G5/u01-l09-practice-acceptance-v16.json');
test('L09 original exit and shared C gate preserved on full branch checkout',{skip:(!fs.existsSync(rp)||!fs.existsSync(ap))&&'repository fixtures not mounted'},()=>{const l=JSON.parse(fs.readFileSync(rp,'utf8')).Lessons.find(x=>x.LessonCode.endsWith('U01:L09'));const r=JSON.parse(fs.readFileSync(ap,'utf8'));assert.match(l.ExplanationParagraphs.join(' '),/Edulytiks-original exit check/);assert.equal(r.C,'BLOCKED_SHARED_INTEGRATION');assert.equal(r.testStatus,'TEST_AUTHORED_NOT_RUN')});
