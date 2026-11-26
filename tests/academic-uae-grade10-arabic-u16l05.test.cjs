'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const load=p=>JSON.parse(fs.readFileSync(path.resolve(__dirname,'..',p),'utf8'));
const root='artifacts/curriculum-delivery/UAE-MOE-MATH/G10/';
test('Selected probability source, exact mappings and honest D gate',()=>{
 const cp=load(root+'lesson-137-stage-a-c-checkpoint.json'),ledger=load(root+'lesson-137-source-elements.json'),contract=load(root+'lesson-137-stage-c-exact-contract.json'),r=load('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u16-l05-ar.rich-lesson-v2.json').Lessons[0];
 assert.equal(cp.sourceBodyVerified,true);assert.equal(cp.sourceSha256,'a94ef73f58c1860ac231b6ad4e8d617465fc0734108607e4bffcbaf37bfe86d3');assert.equal(cp.sourceElementDenominatorForThisLesson,null);assert.equal(cp.stageD,'NOT_STARTED');assert.equal(cp.testsRun,'WRITTEN_NOT_RUN');assert.equal(cp.productionRouting,false);
 assert.equal(r.WorkedExamples.length,26);assert.equal(r.Visuals.length,10);assert.ok(r.WorkedExamples.every(e=>e.Answer&&e.Check&&e.Steps.length>=2));
 assert.equal(ledger.sourceElementCount,ledger.sourceElements.length);assert.equal(new Set(ledger.sourceElements.map(e=>e.elementId)).size,ledger.sourceElementCount);assert.equal(contract.allowedQuestionFamilies.length,18);
 assert.ok(ledger.sourceElements.every(e=>!e.questionFamily||contract.allowedQuestionFamilies.includes(e.questionFamily)));
 const pack=load('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json');assert.equal(pack.Lessons.find(l=>l.LessonCode===r.LessonCode).Translations.find(t=>t.CultureCode==='ar').Explanation,r.ExplanationParagraphs.join('\n\n'));
 const mappings=load('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json').mappings;assert.deepEqual(mappings.find(m=>m.lessonCode===r.LessonCode).allowedQuestionFamilies,contract.allowedQuestionFamilies);
 const registry=load('src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json').families;for(const id of contract.allowedQuestionFamilies){const q=registry.filter(x=>x.id===id);assert.equal(q.length,1);assert.equal(q[0].productionRouting,false);}
 const table=r.Visuals.find(x=>x.Title==='توزيع اختيار ثلاث عناصر');assert.equal(table.Rows.reduce((s,x)=>s+Number(x.Cells[0]),0),84);
 assert.equal(r.Visuals.find(x=>x.Title==='حالات لونين مختلفين').Rows.reduce((s,x)=>s+Number(x.Cells[0]),0),36);
});
