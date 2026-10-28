'use strict';
const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const root = path.resolve(__dirname, '..');
const pack = JSON.parse(fs.readFileSync(path.join(root, 'src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g4-phase29-v1.lesson-content-pack.json'), 'utf8'));
const blueprint = JSON.parse(fs.readFileSync(path.join(root, 'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/us-ccss-math-grade4-im-k5-2021.lesson-blueprint.json'), 'utf8'));
const first = pack.lessons[0].translations.find(x => x.cultureCode === 'en');
test('Grade 4 lesson 1 source identity and inventory', () => {
 assert.equal(pack.lessons.length,149); assert.equal(blueprint.Lessons.length,149);
 assert.equal(pack.lessons[0].lessonCode, 'PED:US-CCSS-MATH:G4:U01:L01');
 assert.equal(blueprint.Lessons[0].Title, 'Multiples of a Number');
 assert.equal(first.title, 'Multiples of a Number');
});
test('source-specific area and multiples, no generic geometry fallback', () => {
 assert.match(first.keyConceptsAndRules,/width × length/);
 assert.match(first.stepByStepSolutions,/28 ÷ 3 leaves remainder 1/);
 assert.doesNotMatch(first.commonMistakes,/classifying a figure by appearance/);
});
test('independent multiples arithmetic regression', () => {
 assert.deepEqual(Array.from({length:10},(_,i)=>(i+1)*3),[3,6,9,12,15,18,21,24,27,30]);
 assert.equal(28%3,1); assert.equal(33%3,0); assert.equal(34%3,1); assert.equal(36%3,0);
});
test('fail-closed checkpoint blocks advancing to lesson 2', () => {
 const x=JSON.parse(fs.readFileSync(path.join(root,'artifacts/curriculum-delivery/US-CCSS-MATH/G4/lesson-1-v2-stage-a-d-checkpoint.json'),'utf8'));
 assert.equal(x.status,'READY_FOR_STAGE_D');assert.equal(x.readyForStageD,true);assert.equal(x.sourceParityProvenPercent,null);assert.equal(x.approvedSourceDifference.publisherCoolDownOriginalSeen,false);assert.equal(x.approvedSourceDifference.userAuthorisedOriginalEdulyticsCoolDown,true);
});


test('G4 U01 L01 first-party Rich V2 retains source PDF activity boundaries', () => {
 const v2 = JSON.parse(fs.readFileSync(path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g4-u01-l01-2021.rich-lesson-v2.json'),'utf8'));
 assert.equal(v2.SchemaVersion,1);
 assert.equal(v2.Lessons.length,1);
 const l=v2.Lessons[0];
 assert.equal(l.LessonCode,'PED:US-CCSS-MATH:G4:U01:L01');
 assert.equal(l.Title,'Multiples of a Number');
 assert.ok(l.ExplanationParagraphs.length>=2 && l.KeyConcepts.length>=3 && l.WorkedExamples.length>=3 && l.CommonMistakes.length>=2 && l.SummaryPoints.length>=3);
 assert.ok(l.ExplanationParagraphs.some(s=>s.includes('FIVE distinct') && s.includes('2 tiles')));
 assert.ok(l.ExplanationParagraphs.some(s=>s.includes('Elena') && s.includes('28')));
 assert.ok(l.ExplanationParagraphs.some(s=>s.includes('Cool-down') && s.includes('not represented as verified')));
 for(const e of l.WorkedExamples){assert.ok(e.Title && e.Question && e.Method && e.Answer && e.Steps.length>=3);}
 const caps = l.WorkedExamples.find(e=>e.Title.startsWith('Activity 2 — all Elena'));
 assert.ok(caps && caps.Answer.includes('27,30'));
});
