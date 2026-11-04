'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const base=path.resolve(__dirname,'../src/Edulytics.Core/Curriculum/LessonContent/RichV2');
const files=['us-ccss-g5-u05-l21-l23-2021.rich-lesson-v2.json','us-ccss-g5-u05-l24-l26-2021.rich-lesson-v2.json'].map(n=>path.join(base,n));
const eq=(a,b)=>assert.ok(Math.abs(a-b)<1e-10,String(a)+' vs '+String(b));
test('U05 L21: scaled integer products count hundredths correctly',()=>{
 eq(6.4*2.7,(64*27)/100);eq(5.6*3.2,17.92);eq(23*0.47,10.81);
 eq(8.2*3.4,27.88);eq(47*0.36,16.92);eq(235*0.18,42.3);
});
test('U05 L22: dividing wholes by a tenth and a hundredth counts groups',()=>{
 for(const n of [3,4,7,9]){eq(n/0.1,10*n);eq(n/0.01,100*n);}
});
test('U05 L23: decimal divisor grouping inverses hold',()=>{
 for(const n of [1,2,3,4]){eq(n/0.2,5*n);eq(n/0.02,50*n);}
 eq(12/0.2,60);eq(14/0.5,28);eq(5/0.25,20);
});
test('U05 L24: fractional dividend divided by whole groups gets smaller',()=>{
 eq(0.72/3,0.24);eq(0.5/2,0.25);eq(0.75/5,0.15);eq(0.7/2,0.35);
 eq(0.45/5,0.09);eq(0.24*3,0.72);
});
test('U05 L25: rescaled decimal divisor preserves quotient and original whole',()=>{
 eq(1.8/0.1,18);eq(1.35/0.01,135);eq(1.6/0.1,160/10);
 eq(0.5/0.1,5);eq(0.5/0.01,50);eq(0.02/0.01,2);eq(1.53/0.01,153);
});
test('U05 L26: hypothetical book-sale profit is cost-subtracted and count-based',()=>{
 const marginA=1025-725,marginB=1150-725;
 assert.equal(marginA,300);assert.equal(marginB,425);
 assert.equal(36*marginA,10800);assert.equal(24*marginB,10200);assert.equal(34*marginA,10200);
 const estimated=30*(1140-840)+24*(775-525)+40*(510-360);
 assert.equal(estimated,21000);
});
test('U05 L21-L26: complete Rich V2 identity and structure', {skip:!files.every(p=>fs.existsSync(p))&&'Unpublished GitHub sidecars are not locally mounted'},()=>{
 const lessons=files.flatMap(p=>{const d=JSON.parse(fs.readFileSync(p,'utf8'));assert.equal(d.SchemaVersion,1);assert.equal(d.PackCode,'US-CCSS-MATH');return d.Lessons});
 assert.equal(lessons.length,6);
 lessons.forEach((x,i)=>{assert.equal(x.LessonCode,'PED:US-CCSS-MATH:G5:U05:L'+String(i+21).padStart(2,'0'));
 assert.ok(x.ExplanationParagraphs.length>=4&&x.KeyConcepts.length>=3&&x.WorkedExamples.length>=3&&x.CommonMistakes.length>=2);
 assert.ok(x.Visuals.every(v=>v.Description&&v.Rows.length>=2));});
});
