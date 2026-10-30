'use strict';
// STAGE D ONLY; WRITTEN_NOT_RUN during A–B–C.
const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const rich = path.resolve(__dirname, '../src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u01-l01-l03-2021.rich-lesson-v2.json');
const ledger = path.resolve(__dirname, '../artifacts/curriculum-delivery/US-CCSS-MATH/G5/u01-l01-abc-element-ledger-20261030.json');
test('U01:L01 source elements include original publisher cooldown as unavailable',()=>{
 const x=JSON.parse(fs.readFileSync(ledger,'utf8'));
 assert.equal(x.sourceElementStatusCounts.SOURCE_UNAVAILABLE,1);
 assert.ok(x.lessonElements.find(y=>y.SourceElementId.endsWith('CD-SOURCE-Q01')));
 assert.equal(x.assessmentSkillContract.humanReviewRequired,true);
 assert.equal(x.readyForStageD,false);
});
test('U01:L01 four manual criteria reject scalar-only answer and source requires nine connected cubes',()=>{
 const x=JSON.parse(fs.readFileSync(ledger,'utf8'));
 assert.equal(x.rubric.maxPoints,8);
 assert.equal(x.rubric.criteria.length,4);
 assert.match(x.rubric.criteria[0].reject,/Disconnected/);
 assert.match(x.rubric.digitalSupplement,/NOT evidence/);
 const l=JSON.parse(fs.readFileSync(rich,'utf8')).Lessons.find(x=>x.LessonCode==='PED:US-CCSS-MATH:G5:U01:L01');
 assert.match(l.ExplanationParagraphs.join(' '),/Math Community teacher move/);
 assert.match(l.ExplanationParagraphs.join(' '),/teacher review rubric \(8 points/);
});
