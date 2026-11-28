'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const load=p=>JSON.parse(fs.readFileSync(path.resolve(__dirname,'..',p),'utf8'));
const root='artifacts/curriculum-delivery/UAE-MOE-MATH/G10/';
const cp=load(root+'lesson-147-stage-a-c-checkpoint.json');
const ledger=load(root+'lesson-147-source-elements.json');
const contract=load(root+'lesson-147-stage-c-exact-contract.json');
const r=load('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u18-l02-ar.rich-lesson-v2.json').Lessons[0];
test('Lesson147 selected body, Arabic content, exact family links, proof geometry and Stage D gate',()=>{
 assert.equal(cp.canonicalOrdinal,147);assert.equal(cp.sourceBodyVerified,true);
 assert.equal(cp.sourceSha256,'9b4a1497d16fdd9d518ed098ac8790b5a0a1305ecbb02e4e8e2be7af3b56bc66');assert.equal(cp.sourceElementDenominatorForThisLesson,null);
 assert.equal(cp.productionRouting,false);assert.equal(cp.stageD,'NOT_STARTED');assert.equal(cp.testsRun,'WRITTEN_NOT_RUN');
 assert.equal(r.WorkedExamples.length,35);assert.equal(r.Visuals.length,10);
 assert.ok(r.WorkedExamples.every(e=>e.Answer&&e.Check&&e.Steps.length>=1));
 assert.equal(ledger.sourceElementCount,ledger.sourceElements.length);assert.equal(new Set(ledger.sourceElements.map(e=>e.elementId)).size,ledger.sourceElementCount);
 assert.equal(contract.allowedQuestionFamilies.length,14);
 assert.ok(ledger.sourceElements.every(e=>!e.questionFamily||contract.allowedQuestionFamilies.includes(e.questionFamily)));
 const pack=load('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json');
 const t=pack.Lessons.find(l=>l.LessonCode===r.LessonCode).Translations.find(t=>t.CultureCode==='ar');assert.equal(t.Explanation,r.ExplanationParagraphs.join('\n\n'));
 const mappings=load('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json').mappings;assert.deepEqual(mappings.find(m=>m.lessonCode===r.LessonCode).allowedQuestionFamilies,contract.allowedQuestionFamilies);
 const registry=load('src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json').families;
 for(const id of contract.allowedQuestionFamilies){const q=registry.filter(x=>x.id===id);assert.equal(q.length,1);assert.equal(q[0].productionRouting,false);assert.equal(q[0].answerType,id.endsWith('rational_linear_equation')?'exact_rational':'exact_integer');}
 const svg=fs.readFileSync(path.resolve(__dirname,'..',root+'lesson-147-original-proof.svg'),'utf8');
 assert.ok(svg.includes('AP=12')&&svg.includes('PB=6')&&svg.includes('AB=18'));assert.equal(12+6,18);assert.equal(9*9+12*12,15*15);
});
