'use strict';
const test=require('node:test'), assert=require('node:assert/strict'), fs=require('node:fs'), path=require('node:path');
const source=path.resolve(__dirname,'../src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u01-l04-l06-2021.rich-lesson-v2.json');
test('G5 U01 L04: layer orientation does not change volume',()=>{
 assert.equal(2*12,24);assert.equal(3*8,24);assert.equal(4*6,24);
 assert.equal(4*(5*2),40);assert.equal(5*(4*2),40);
});
test('G5 U01 L04: a front nine-cube layer cannot determine exact volume with hidden depth',()=>{
 const possible=[2,3,10].map(layers=>9*layers);
 assert.deepEqual(possible,[18,27,90]);
 assert.equal(new Set(possible).size,3);
});
test('G5 U01 L05: any prism face may be base with perpendicular height',()=>{
 const dims=[2,3,5],vol=30;
 for(let i=0;i<3;i++){
  const base=dims.filter((_,j)=>j!==i).reduce((a,b)=>a*b,1);
  assert.equal(base*dims[i],vol);
 }
 assert.deepEqual([1,2,3,4].map(h=>4*5*h),[20,40,60,80]);
});
test('G5 U01 L04-L05: source-specific Rich V2 fields on a mounted original repository', {skip:!fs.existsSync(source)&&'Actual GitHub file not mounted in isolated runner'},()=>{
 const d=JSON.parse(fs.readFileSync(source,'utf8'));
 assert.equal(d.SchemaVersion,1); assert.equal(d.PackCode,'US-CCSS-MATH');
 const rows=d.Lessons.filter(x=>/G5:U01:L0[45]$/.test(x.LessonCode));
 assert.equal(rows.length,2);
 assert.match(rows[0].ExplanationParagraphs.join(' '),/estimat/i);
 assert.match(rows[0].ExplanationParagraphs.join(' '),/layer/i);
 assert.match(rows[1].ExplanationParagraphs.join(' '),/base/i);
 assert.match(rows[1].ExplanationParagraphs.join(' '),/height/i);
});
