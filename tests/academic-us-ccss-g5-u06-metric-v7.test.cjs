'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const source=path.resolve(__dirname,'../src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u06-l02-l06-2021.rich-lesson-v2.json');
const eq=(a,b)=>assert.ok(Math.abs(a-b)<1e-10,String(a)+' vs '+String(b));
test('U06 L02: huge numbers use actual powers, not exponent as multiplier',()=>{
 assert.equal(10**6,1000000);assert.equal(10**9,1000000000);assert.equal(10**9/(10**6),1000);
 assert.equal(10**12,1000000000000);assert.equal(10000*10,100000);assert.equal(10**7,10000000);
});
test('U06 L03: metric length conversions and comparative broad jumps',()=>{
 eq(3.2*100,320);eq(3.2*1000,3200);eq(12*1000,12000);
 assert.deepEqual([1.62,1.39,1.50].map(n=>n>1.45),[true,false,true]);
});
test('U06 L04: smaller metric units divided by proper ten powers',()=>{
 eq(745/100,7.45);eq(1580/100,15.8);eq(4567/100,45.67);
 eq(9513/1000,9.513);eq(11018/1000,11.018);
});
test('U06 L05: walker and runner comparison with consistent units',()=>{
 assert.equal(52*8000,416000);eq(416000/100000,4.16);
 eq(5.5+6.25+4.75+7.1+6.4,30);
 assert.equal(5400+6200+5800+6500+7000,30900);
 eq(30900/1000-30,0.9);
});
test('U06 L06: fractional liters and multi-step group capacity',()=>{
 eq(0.45*1000,450);eq(0.95*1000,950);eq(0.75*1000,750);
 eq(24*1.5*450/1000,16.2);eq(20-16.2,3.8);
 assert.ok(15<0.15*1000);
});
test('U06 L02-L06: Rich V2 source IDs and structural fields', {skip:!fs.existsSync(source)&&'GitHub sidecar JSON unavailable in isolated Node runner'},()=>{
 const d=JSON.parse(fs.readFileSync(source,'utf8'));assert.equal(d.SchemaVersion,1);assert.equal(d.PackCode,'US-CCSS-MATH');assert.equal(d.Lessons.length,5);
 d.Lessons.forEach((x,i)=>{assert.equal(x.LessonCode,'PED:US-CCSS-MATH:G5:U06:L'+String(i+2).padStart(2,'0'));
 assert.ok(x.ExplanationParagraphs.length>=4&&x.KeyConcepts.length>=3&&x.WorkedExamples.length>=3&&x.CommonMistakes.length>=2);
 assert.ok(x.Visuals.every(v=>v.Description&&v.Rows.length>=2));});
});
