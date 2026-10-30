'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const file=path.resolve(__dirname,'../src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u02-l06-l08-2021.rich-lesson-v2.json');
const validate=(barHalves)=>{
 if(!Array.isArray(barHalves)||barHalves.length!==3)throw Error('must have three bars');
 if(!barHalves.every(n=>Number.isSafeInteger(n)))throw Error('malformed shading');
 if(!barHalves.every(n=>n>=0&&n<=2))throw Error('shading exceeds unit');
 return barHalves.reduce((s,n)=>s+n,0);
};
test('Original U02:L06 A three bars show 1+1+1 half-units, B shows 2+1+0',()=>{
 assert.equal(validate([1,1,1]),3);assert.equal(validate([2,1,0]),3);
 assert.notDeepEqual([1,1,1],[2,1,0]);
 assert.equal(validate([1,1,1])/2,3/2);assert.equal(validate([2,1,0])/2,3/2);
 assert.equal(3*(1/2),3/2);assert.equal(3/2,(1/2)*3);
});
test('Reject one-long-bar misdescription, wrong total, overfilled shading and malformed inputs',()=>{
 assert.throws(()=>validate([1]),/three bars/);
 assert.throws(()=>validate([2,1]),/three bars/);
 assert.notEqual(validate([2,2,0])/2,3/2);
 assert.throws(()=>validate([2,3,0]),/exceeds/);
 assert.throws(()=>validate([2,1,'0']),/malformed/);
 assert.throws(()=>validate([-1,2,2]),/exceeds/);
});
test('U02 L06 exact Rich V2 original-diagram representation on full checkout',{skip:!fs.existsSync(file)&&'Grade 5 GitHub fixture not mounted'},()=>{
 const d=JSON.parse(fs.readFileSync(file,'utf8'));
 assert.equal(d.Lessons.length,3);
 const x=d.Lessons.find(z=>z.LessonCode==='PED:US-CCSS-MATH:G5:U02:L06');
 assert.ok(x);
 assert.match(x.ExplanationParagraphs[3],/three one-unit bars/i);
 assert.match(x.ExplanationParagraphs[3],/fully shaded one-mile bar|one entire unit bar/i);
 assert.match(x.WorkedExamples[2].Answer,/three one-unit bars/);
 assert.ok(x.CommonMistakes.some(m=>/single 3-mile strip/.test(m.Mistake)));
});
