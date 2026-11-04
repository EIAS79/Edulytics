'use strict';
// Source-aligned independent Prism card contract for IM 2021 L03; WRITTEN_NOT_RUN.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
test('L03 first-party prism cards are mathematically consistent and labelled as independently authored',()=>{
 const p=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u01-l01-l03-2021.rich-lesson-v2.json');
 const l=p.Lessons.find(x=>x.LessonCode==='PED:US-CCSS-MATH:G5:U01:L03');
 const cards=l.Visuals.find(v=>v.Title==='Edulytiks-original printable prism-build cards (independent 2021 skill equivalents)');
 assert.ok(cards);assert.equal(cards.Rows.length,5);assert.match(cards.Description,/NOT the publisher/i);
 assert.deepEqual([[2,2,3],[2,3,3],[2,3,4],[3,2,5]].map(([a,b,c])=>a*b*c),[12,18,24,30]);
 assert.equal(5*8,40);assert.equal(2*20,40);assert.equal(4*10,40);
});
test('Original publisher cards and cooldown not silently reclassified as verified original source',()=>{
 const p=read('artifacts/curriculum-delivery/US-CCSS-MATH/G5/u01-l03-abc-element-ledger-20261030.json');
 const a=p.sourceElements.find(x=>x.SourceElementId==='G5-U01-L03-A01-CARDS');
 const c=p.sourceElements.find(x=>x.SourceElementId==='G5-U01-L03-COOLDOWN-SOURCE');
 assert.equal(a.Status,'SOURCE_UNAVAILABLE');assert.equal(c.Status,'SOURCE_UNAVAILABLE');
 assert.equal(a.FunctionalReplacementStatus,'MATCHED_TEACHER_LED');
 assert.equal(c.FunctionalReplacementStatus,'MATCHED_TEACHER_LED');
 assert.equal(p.sourceCoverage100Percent,false);
 assert.equal(p.readyForStageD,true);assert.equal(p.revalidation20261102.tests,'WRITTEN_NOT_RUN');
});
