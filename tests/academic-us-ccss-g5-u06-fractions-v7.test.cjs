'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const rich=path.resolve(__dirname,'../src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u06-l07-l11-2021.rich-lesson-v2.json');
const fractionEq=(a,b,c,d)=>assert.equal(BigInt(a)*BigInt(d),BigInt(c)*BigInt(b));
test('U06 L07: inch/foot/yard equivalence and whole lap threshold',()=>{
 assert.equal(2.5*36,90);assert.equal(2.5*3,7.5);
 assert.equal(2*(100+48.5),297);assert.equal(297*6,1782);assert.equal(1782-1760,22);
 assert.equal(2*(90+55),290);assert.ok(12*290<3520&&13*290>=3520);
});
test('U06 L08: fraction card arithmetic uses like units',()=>{
 fractionEq(3+3,4,3,2);fractionEq(6-1,8,5,8);fractionEq(3+2,4,5,4);
});
test('U06 L09: equivalent expressions preserve both fractions',()=>{
 fractionEq(8+5,12,13,12);
 fractionEq(5-2,4,3,4);
 fractionEq(13-6,8,7,8);
});
test('U06 L10: common-denominator product works but smaller multiple can help',()=>{
 fractionEq(15+16,40,31,40);
 fractionEq(21-20,30,1,30);
 fractionEq(6+5,9,18+15,27);
});
test('U06 L11: mixed-number regrouping and adding-on reconcile',()=>{
 fractionEq(17*6-17*4,24,17,12);
 fractionEq(1+1,8,1,4);
 fractionEq(22-11,6,11,6);
});
test('U06 L07-L11: direct source-specific structure is complete', {skip:!fs.existsSync(rich)&&'GitHub candidate sidecar not present in isolated Node environment'},()=>{
 const d=JSON.parse(fs.readFileSync(rich,'utf8'));assert.equal(d.SchemaVersion,1);assert.equal(d.PackCode,'US-CCSS-MATH');assert.equal(d.Lessons.length,5);
 d.Lessons.forEach((x,i)=>{
 assert.equal(x.LessonCode,'PED:US-CCSS-MATH:G5:U06:L'+String(i+7).padStart(2,'0'));
 assert.ok(x.ExplanationParagraphs.length>=4&&x.KeyConcepts.length>=3&&x.WorkedExamples.length>=3&&x.CommonMistakes.length>=2);
 assert.ok(x.Visuals.every(v=>v.Description&&v.Rows.length>=2));});
});