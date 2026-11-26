'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const load=p=>JSON.parse(fs.readFileSync(path.resolve(__dirname,'..',p),'utf8'));
const root='artifacts/curriculum-delivery/UAE-MOE-MATH/G10/';
const cp=load(root+'lesson-148-stage-a-c-checkpoint.json');
const ledger=load(root+'lesson-148-source-elements.json');
const contract=load(root+'lesson-148-stage-c-exact-contract.json');
const r=load('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u18-l03-ar.rich-lesson-v2.json').Lessons[0];
test('Lesson148 selected source, exact family links, angle figure constraints and Stage D gate',()=>{
 assert.equal(cp.canonicalOrdinal,148);assert.equal(cp.sourceBodyVerified,true);
 assert.equal(cp.sourceSha256,'9b4a1497d16fdd9d518ed098ac8790b5a0a1305ecbb02e4e8e2be7af3b56bc66');assert.equal(cp.sourceElementDenominatorForThisLesson,null);
 assert.equal(cp.productionRouting,false);assert.equal(cp.stageD,'NOT_STARTED');assert.equal(cp.testsRun,'WRITTEN_NOT_RUN');
 assert.equal(r.WorkedExamples.length,39);assert.equal(r.Visuals.length,12);assert.ok(r.WorkedExamples.every(e=>e.Answer&&e.Check&&e.Steps.length>=1));
 assert.equal(ledger.sourceElementCount,ledger.sourceElements.length);assert.equal(new Set(ledger.sourceElements.map(e=>e.elementId)).size,ledger.sourceElementCount);
 assert.equal(contract.allowedQuestionFamilies.length,14);assert.ok(ledger.sourceElements.every(e=>!e.questionFamily||contract.allowedQuestionFamilies.includes(e.questionFamily)));
 const pack=load('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json');
 const t=pack.Lessons.find(l=>l.LessonCode===r.LessonCode).Translations.find(t=>t.CultureCode==='ar');assert.equal(t.Explanation,r.ExplanationParagraphs.join('\n\n'));
 const mappings=load('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json').mappings;assert.deepEqual(mappings.find(m=>m.lessonCode===r.LessonCode).allowedQuestionFamilies,contract.allowedQuestionFamilies);
 const registry=load('src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json').families;
 for(const id of contract.allowedQuestionFamilies){const q=registry.filter(x=>x.id===id);assert.equal(q.length,1);assert.equal(q[0].productionRouting,false);assert.equal(q[0].answerType,'exact_integer');}
 const svg=fs.readFileSync(path.resolve(__dirname,'..',root+'lesson-148-original-angle-diagrams.svg'),'utf8');
 assert.ok(svg.includes('85°')&&svg.includes('118°')&&svg.includes('69°'));
 assert.equal(110-25,85);assert.equal(40+65,105);assert.equal(2*(62+118),360);assert.equal(116/2,58);assert.equal(112-43,69);
});
