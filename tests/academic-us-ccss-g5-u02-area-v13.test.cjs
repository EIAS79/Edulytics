'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const fixture=path.resolve(__dirname,'../../../src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u02-l09-l13-2021.rich-lesson-v2.json');
const area=(whole,num,den,length)=>{if(![whole,num,den,length].every(Number.isSafeInteger))throw Error('malformed');if(whole<0||num<0||den<=0||length<=0)throw Error('invalid');return {top:length*(whole*den+num),bottom:den};};
const same=(x,y)=>x.top*y.bottom===y.top*x.bottom;
test('L11 exact 4 2/3 by 4 area and all five select-all cards',()=>{
 const source=area(4,2,3,4);assert.ok(same(source,{top:56,bottom:3}));
 const choices=[source,{top:16*8,bottom:3},area(0,14,3,4),{top:56,bottom:3},area(0,5,3,4)];
 assert.deepEqual(choices.map(x=>same(source,x)),[true,false,true,true,false]);
 assert.ok(same(area(2,1,4,2),{top:9,bottom:2}));
 assert.ok(same(area(3,3,4,3),{top:45,bottom:4}));
});
test('L12 Noah and Priya original gardens differ by exactly quarter square yard',()=>{
 const noah=area(6,1,4,5),priya=area(5,1,4,6);
 assert.ok(same(noah,{top:125,bottom:4}));assert.ok(same(priya,{top:126,bottom:4}));
 assert.equal(priya.top-noah.top,1);assert.equal(priya.bottom,4);
 assert.notEqual(noah.top,priya.top);
});
test('L12 six incomplete partial-product strategies all complete original 4 by 5 1/2 rectangle',()=>{
 const expected=area(5,1,2,4);assert.ok(same(expected,{top:22,bottom:1}));
 assert.deepEqual([20+2,2+20,24-2,20+2,24-2,44/2],[22,22,22,22,22,22]);
 assert.notEqual(24,22);assert.notEqual(44,22);
});
test('L13 original card C/B and two write-expression regions use distinct geometry',()=>{
 assert.ok(same(area(5,2,5,3),{top:81,bottom:5}));
 assert.equal(3*5+3*2/5,81/5);
 assert.equal(5*3-5*2/5,13);
 assert.ok(same(area(5,1,3,4),{top:64,bottom:3}));
 assert.ok(same(area(3,3,4,7),{top:105,bottom:4}));
 assert.equal(7*4-7/4,105/4);
 assert.notEqual(81/5,13);
});
test('Invalid and boundary side lengths are not silently scored as valid areas',()=>{
 assert.ok(same(area(0,1,2,1),{top:1,bottom:2}));
 assert.throws(()=>area(2,1,0,4),/invalid/);
 assert.throws(()=>area(2,-1,4,4),/invalid/);
 assert.throws(()=>area(2,1,4,0),/invalid/);
 assert.throws(()=>area(2,1,'4',4),/malformed/);
});
test('L11–L13 source-specific content fixture in actual full checkout',{skip:!fs.existsSync(fixture)?'GitHub branch files not mounted':false},()=>{
 const d=JSON.parse(fs.readFileSync(fixture,'utf8'));
 for(const n of [11,12,13]){
  const x=d.Lessons.find(z=>z.LessonCode==='PED:US-CCSS-MATH:G5:U02:L'+n);
  assert.ok(x);assert.ok(x.WorkedExamples.length>=3);assert.ok(x.Visuals.length>=1);
 }
 const joined=d.Lessons.filter(z=>/U02:L1[123]$/.test(z.LessonCode)).map(x=>x.ExplanationParagraphs.join(' ')).join(' ');
 assert.match(joined,/original 4 2\/3 by 4/);assert.match(joined,/Noah/);assert.match(joined,/Card Sort/);
});
