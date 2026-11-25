'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const load=p=>JSON.parse(fs.readFileSync(path.resolve(__dirname,'..',p),'utf8'));
const cp=load('artifacts/curriculum-delivery/UAE-MOE-MATH/G10/lesson-044-stage-a-c-checkpoint.json');
const r=load('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u05-l03-ar.rich-lesson-v2.json').Lessons[0];
test('Lesson44 original Arabic index-only authoring and truthful source scope',()=>{
 assert.equal(cp.canonicalOrdinal,44);assert.equal(cp.sourceBodyVerified,false);assert.equal(cp.ownerApprovedIndexOnlyAuthoring,true);
 assert.equal(cp.sourceElementDenominatorForThisLesson,null);assert.equal(cp.status,'READY_FOR_STAGE_D_ORIGINAL_INDEX_ONLY');
 assert.equal(cp.productionRouting,false);assert.equal(r.WorkedExamples.length,12);assert.equal(r.Visuals.length,4);
 assert.ok(r.ExplanationParagraphs.length>=5);assert.ok(r.WorkedExamples.every(e=>e.Answer&&e.Check&&e.Steps.length===2));
});
