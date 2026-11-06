'use strict';
const test=require('node:test');const assert=require('node:assert/strict');const fs=require('node:fs');const path=require('node:path');
const base=path.resolve(__dirname,'../src/Edulytics.Core/Curriculum/LessonContent/RichV2');
const arr=['us-ccss-g5-u01-l01-l03-2021.rich-lesson-v2.json','us-ccss-g5-u01-l04-l06-2021.rich-lesson-v2.json','us-ccss-g5-u01-l07-l12-2021.rich-lesson-v2.json'];
test('G5 U01 checkpoint content version equals source pack shared root version for every lesson',()=>{
 const p=arr.map(n=>JSON.parse(fs.readFileSync(path.join(base,n),'utf8')));
 const ck=JSON.parse(fs.readFileSync(path.resolve(__dirname,'../artifacts/curriculum-delivery/US-CCSS-MATH/G5/coder1-grade5-authoring-checkpoint.json'),'utf8'));
 for(const l of ck.lessons.filter(l=>l.unitNumber===1)){
  const n=l.lessonNumber;const expected=p[n<=3?0:n<=6?1:2];
  assert.equal(l.richV2ContentVersion,expected.ContentVersion,l.lessonCode+' stale');
  assert.ok(expected.Lessons.find(x=>x.LessonCode===l.lessonCode));
 }
});
