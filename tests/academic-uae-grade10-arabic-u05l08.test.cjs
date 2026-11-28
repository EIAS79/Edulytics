'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const load=p=>JSON.parse(fs.readFileSync(path.resolve(__dirname,'..',p),'utf8'));
const root='artifacts/curriculum-delivery/UAE-MOE-MATH/G10/';
test('Selected body, exact mapping, Arabic and Stage D gate',()=>{
 const cp=load(root+'lesson-049-stage-a-c-checkpoint.json'),ledger=load(root+'lesson-049-source-elements.json'),contract=load(root+'lesson-049-stage-c-exact-contract.json'),r=load('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u05-l08-ar.rich-lesson-v2.json').Lessons[0];
 assert.equal(cp.sourceBodyVerified,true);assert.equal(cp.sourceSha256,'2411a36f1c19f705bf1ce700d657b4cd9d2464e6506c22d8a2f1df789a59550c');assert.equal(cp.sourceElementDenominatorForThisLesson,null);assert.equal(cp.stageD,'NOT_STARTED');assert.equal(cp.testsRun,'WRITTEN_NOT_RUN');assert.equal(cp.productionRouting,false);
 assert.equal(r.WorkedExamples.length,38);assert.equal(r.Visuals.length,14);assert.ok(r.WorkedExamples.every(e=>e.Answer&&e.Check&&e.Steps.length>=2));
 assert.equal(ledger.sourceElementCount,ledger.sourceElements.length);assert.equal(new Set(ledger.sourceElements.map(e=>e.elementId)).size,ledger.sourceElementCount);assert.equal(contract.allowedQuestionFamilies.length,21);
 assert.ok(ledger.sourceElements.every(e=>!e.questionFamily||contract.allowedQuestionFamilies.includes(e.questionFamily)));
 const pack=load('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json');assert.equal(pack.Lessons.find(l=>l.LessonCode===r.LessonCode).Translations.find(t=>t.CultureCode==='ar').Explanation,r.ExplanationParagraphs.join('\n\n'));
 const mappings=load('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json').mappings;assert.deepEqual(mappings.find(m=>m.lessonCode===r.LessonCode).allowedQuestionFamilies,contract.allowedQuestionFamilies);
 const registry=load('src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json').families;for(const id of contract.allowedQuestionFamilies){const q=registry.filter(x=>x.id===id);assert.equal(q.length,1);assert.equal(q[0].productionRouting,false);}
 const plots=r.Visuals.filter(v=>v.Kind==='CartesianPlot');assert.equal(plots.length,8);
 const fns=[t=>200*Math.exp(.08*t),t=>2400/(1+5*Math.exp(-.03*t))];
 plots.slice(-2).forEach((v,i)=>v.Rows.forEach(row=>{const [t,y]=row.Cells.map(Number);assert.ok(Math.abs(fns[i](t)-y)<1e-5);}));
});
