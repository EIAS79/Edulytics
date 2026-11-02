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

// Grade 5 first-edition licensing: educational mathematics can be independently
// re-illustrated, but never silently import third-party artwork into learner UI.
test('Grade5 first-party visual inventory and 2021 CC BY source attribution are intact',()=>{
 const files=fs.readdirSync(dir).filter(n=>/^us-ccss-g5-.*\.rich-lesson-v2\.json$/.test(n));
 const codes=new Set();
 let figures=0, videos=0;
 for(const name of files){
  const doc=JSON.parse(fs.readFileSync(path.join(dir,name),'utf8'));
  for(const lesson of doc.Lessons||[]){
   assert.match(lesson.LessonCode,/^PED:US-CCSS-MATH:G5:U\d{2}:L\d{2}$/);
   assert.equal(lesson.CultureCode,'en');
   assert.ok(!codes.has(lesson.LessonCode),'Duplicate Grade 5 lesson '+lesson.LessonCode);
   codes.add(lesson.LessonCode);
   assert.ok(Array.isArray(lesson.Visuals),name+' missing visuals array');
   assert.ok(Array.isArray(lesson.Videos),name+' missing videos array');
   figures+=lesson.Visuals.length;
   videos+=lesson.Videos.length;
   for(const visual of lesson.Visuals){
    const raw=JSON.stringify(visual);
    assert.doesNotMatch(raw,/https?:\/\/|data:image\/|<img\b|<image\b|cms-assets\./i,
      lesson.LessonCode+' visual must not embed or link to external publisher artwork');
   }
  }
 }
 assert.equal(codes.size,148,'Grade 5 first-edition lesson count');
 assert.equal(figures,282,'current first-party Grade5 visual inventory');
 assert.equal(videos,0,'no unreviewed external Grade5 video embeds');

 const metadata=JSON.parse(fs.readFileSync(path.join(root,
  'src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g5-phase29-v1.lesson-content-pack.json'),'utf8'));
 assert.equal(metadata.lessons.length,148);
 assert.match(metadata.pedagogicalSourceRightsNote,/CC BY 4\.0.*attribution/i);
 assert.match(metadata.pedagogicalSourceRightsNote,/independently authored/i);
 assert.equal(metadata.pedagogicalSourceEdition,'1st Edition (2021)');
 for(const row of metadata.lessons){
  assert.ok(codes.has(row.lessonCode),'Original source metadata missing a tested lesson '+row.lessonCode);
  assert.equal(row.sourceRights,'CC BY 4.0');
  assert.match(row.sourcePublisher,/Illustrative Mathematics/);
  assert.match(row.sourceUrl,/^https:\/\/im\.kendallhunt\.com\//);
 }
});
