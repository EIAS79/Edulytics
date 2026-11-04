'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');

const root=path.resolve(__dirname,'..');
const shared=fs.readFileSync(path.join(root,
 'src/Edulytics.Web/Views/Shared/_RichLessonContentV2.cshtml'),'utf8');

test('student and lesson-detail views share a provenance-free SummaryPoints renderer',()=>{
 const student=fs.readFileSync(path.join(root,'src/Edulytics.Web/Views/StudentPortal/Lesson.cshtml'),'utf8');
 const detail=fs.readFileSync(path.join(root,'src/Edulytics.Web/Views/LessonContent/Detail.cshtml'),'utf8');
 assert.match(student,/partial name="_RichLessonContentV2"/);
 assert.match(detail,/partial name="_RichLessonContentV2"/);
 assert.match(shared,/RichLessonSummaryVisibility\.ForReader\(Model\.LessonCode(?: \?\? string\.Empty)?, Model\.SummaryPoints\)/);
 assert.doesNotMatch(shared,/@foreach\s*\(var point in Model\.SummaryPoints\)/);
 assert.doesNotMatch(shared,/@Model\.SummaryPoints/);
});

test('148 source records stay in code, with private provenance separate from mathematical summary',()=>{
 const dir=path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/RichV2');
 const files=fs.readdirSync(dir).filter(x=>/^us-ccss-g5-.*\.rich-lesson-v2\.json$/.test(x));
 let lessons=0,urls=0;
 const codes=new Set();
 for(const file of files){
  for(const lesson of JSON.parse(fs.readFileSync(path.join(dir,file),'utf8')).Lessons){
   assert.ok(!codes.has(lesson.LessonCode),lesson.LessonCode);
   codes.add(lesson.LessonCode);
   lessons++;
   assert.ok(lesson.SummaryPoints.length>=3,lesson.LessonCode);
   urls+=lesson.SummaryPoints.filter(x=>/https?:\/\//i.test(x)).length;
  }
 }
 assert.equal(lessons,148);
 assert.ok(urls>=95,'original source references must remain in code');
});
