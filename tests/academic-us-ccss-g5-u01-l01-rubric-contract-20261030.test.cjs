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

test('U01:L01 selected family must not be misrepresented as source-equivalent construction assessment',()=>{
 const mappingPath = path.resolve(__dirname, '../src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
 const familyPath = path.resolve(__dirname, '../src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json');
 const maps = JSON.parse(fs.readFileSync(mappingPath, 'utf8')).mappings;
 const families = JSON.parse(fs.readFileSync(familyPath, 'utf8')).families;
 const current = maps.find(row => row.lessonCode === 'PED:US-CCSS-MATH:G5:U01:L01');
 assert.ok(current, 'Exact lesson-code mapping must exist');
 const only = current.allowedQuestionFamilies.map(id => families.find(f => f.id===id));
 assert.ok(only.every(Boolean), 'Every allowed family must exist');
 const isOnlySymbolic = only.every(f => f.representations.every(rep=>rep==='symbolic'));
 // Intentionally RED until designated shared integration provides actual teacher-scored
 // geometry-construction path or removes a misleading source-equivalent readiness claim.
 assert.ok(!isOnlySymbolic || current.practiceReadiness !== 'READY_VERIFIED' ||
   current.humanReviewCapability === 'TEACHER_REVIEW_REQUIRED',
   'Symbolic-only volume multiplication cannot certify original teacher-reviewed constructions');
});
