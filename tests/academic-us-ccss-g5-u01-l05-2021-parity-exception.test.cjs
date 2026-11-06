'use strict';
// 2021 L05 A-B-C parity guard; authored NOT RUN.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const read=p=>JSON.parse(fs.readFileSync(path.resolve(__dirname,'..',p),'utf8'));
test('G5 L05 original cooldown is still unavailable, independent exit and source skills retained',()=>{
 const x=read('artifacts/curriculum-delivery/US-CCSS-MATH/G5/u01-l05-abc-element-ledger-20261030.json');
 const g=read('artifacts/curriculum-delivery/US-CCSS-MATH/G5/g5-abc-lesson-ledger-v1.json');
 const item=x.elements.find(x=>x.SourceElementId==='G5-U01-L05-CD-Q01');
 assert.equal(x.sourceElementsCount,25);assert.equal(x.sourceElementsAccepted,24);
 assert.equal(item.Status,'SOURCE_UNAVAILABLE');
 assert.equal(item.FunctionalReplacementStatus,'MATCHED_TEACHER_LED');
 assert.equal(x.sourceCoverage100Percent,false);assert.equal(x.readyForStageD,true);
 assert.equal(g.lessons[4].READY_FOR_STAGE_D,true);
 const p=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u01-l04-l06-2021.rich-lesson-v2.json');
 const lesson=p.Lessons.find(x=>x.LessonCode==='PED:US-CCSS-MATH:G5:U01:L05');
 assert.match(lesson.ExplanationParagraphs.join(' '),/400 and 1,000 cubic units/);
 assert.match(lesson.ExplanationParagraphs.join(' '),/Edulytiks-original exit check/);
});
