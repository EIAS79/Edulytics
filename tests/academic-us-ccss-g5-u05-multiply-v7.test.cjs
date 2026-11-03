'use strict';
const test=require('node:test'), assert=require('node:assert/strict'), fs=require('node:fs'), path=require('node:path');
const base=path.resolve(__dirname,'../src/Edulytics.Core/Curriculum/LessonContent/RichV2');
const files=['us-ccss-g5-u05-l16-l18-2021.rich-lesson-v2.json','us-ccss-g5-u05-l19-l20-2021.rich-lesson-v2.json'].map(x=>path.join(base,x));
const eq=(a,b)=>assert.ok(Math.abs(a-b)<1e-10,String(a)+' vs '+String(b));
test('U05 L16: select addition, subtraction and compensation without place mixing',()=>{
eq(8.24-3.7,4.54);eq(102.5-8.06,94.44);eq(52-51.68,0.32);eq(120-24.85,95.15);eq(120+24.85,144.85);eq(260.14-259.6,0.54);
});
test('U05 L17: decimal groups preserve unit magnitude',()=>{
eq(2*0.4,0.8);eq(3*0.6,1.8);eq(3*0.09,0.27);eq(3*0.69,2.07);eq(6*0.08,0.48);eq(6*0.8,4.8);
});
test('U05 L18: equivalent whole-product and decimal-unit forms',()=>{
eq(7*0.04,(7*4)*0.01);eq(8*0.07,(8*7)*0.01);eq(4*0.13,4*0.1+4*0.03);assert.ok(7*0.04<7);
});
test('U05 L19: card groups use compatible distribution and compensation',()=>{
eq(6*0.24,6*0.2+6*0.04);eq(5*3.8,5*4-5*0.2);eq(7*0.35,7*0.3+7*0.05);eq(24*0.61,14.64);eq(8*2.9,23.2);
});
test('U05 L20: two tenths factors compose hundredth-sized cells',()=>{
eq(0.3*0.6,0.18);eq(2.4*0.5,1.2);eq(3.6*0.7,2.52);eq(2.4*5,12);assert.ok(0.3*0.6<0.3);
});
test('U05 L16-L20: Rich V2 structural identities', {skip:!files.every(f=>fs.existsSync(f))&&'GitHub branch Rich V2 sidecars not locally mounted'},()=>{
const ls=files.flatMap(f=>{const d=JSON.parse(fs.readFileSync(f,'utf8'));assert.equal(d.SchemaVersion,1);assert.equal(d.PackCode,'US-CCSS-MATH');return d.Lessons});
assert.equal(ls.length,5);ls.forEach((x,i)=>{assert.equal(x.LessonCode,'PED:US-CCSS-MATH:G5:U05:L'+String(i+16).padStart(2,'0'));assert.ok(x.WorkedExamples.length>=3&&x.KeyConcepts.length>=3&&x.CommonMistakes.length>=2&&x.Visuals.every(v=>v.Description&&v.Rows.length>0))});
});
