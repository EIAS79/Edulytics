'use strict';
// Stage D: do not hide duplicate Rich V2 lesson/culture registrations by
// preferring one arbitrary embedded resource. Runtime index is strictly unique.
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'..');
const dir=path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/RichV2');
test('all embedded rich V2 lessons have unique lessonCode:cultureCode',()=>{
 const files=fs.readdirSync(dir).filter(n=>n.endsWith('.rich-lesson-v2.json')).sort();
 assert.ok(files.length>300,'unexpectedly incomplete Rich V2 source inventory');
 const seen=new Map(),duplicates=[];
 for(const name of files){
  const doc=JSON.parse(fs.readFileSync(path.join(dir,name),'utf8'));
  assert.ok(Array.isArray(doc.Lessons)&&doc.Lessons.length>0,name);
  for(const lesson of doc.Lessons){
   assert.ok(lesson.LessonCode&&lesson.CultureCode,name);
   const key=lesson.LessonCode+':'+lesson.CultureCode.toLowerCase();
   if(seen.has(key))duplicates.push({key,first:seen.get(key),second:name});
   else seen.set(key,name);
  }
 }
 assert.deepEqual(duplicates,[],'Runtime would throw Duplicate Rich Lesson Content V2 entry');
 assert.equal(seen.get('PED:US-CCSS-MATH:G5:U02:L01:en'),
   'us-ccss-g5-u02-l01-2021.rich-lesson-v2.json',
   'preserve the richer canonical dedicated original-source G5 U02:L01');
 assert.equal(seen.get('PED:US-CCSS-MATH:G5:U03:L01:en'),
   'us-ccss-g5-fractions-coordinates-source-lessons-2021.rich-lesson-v2.json');
 assert.equal(seen.get('PED:US-CCSS-MATH:G5:U07:L01:en'),
   'us-ccss-g5-fractions-coordinates-source-lessons-2021.rich-lesson-v2.json');
});
