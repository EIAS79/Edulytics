'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),get=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const base='artifacts/curriculum-delivery/UAE-MOE-MATH/G10/';
const code='PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U03:L04:C028';
test('Canonical G10-U03-L04 source PDF48–54 carries its original 3-variable goals and identity',()=>{
const bp=get('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g10-general-t1-ogl-v1.lesson-blueprint.json');
const lesson=bp.Lessons.find(x=>x.LessonCode===code);
assert.ok(lesson);assert.equal(lesson.SortOrder,28);assert.equal(lesson.Title,'حل أنظمة المعادلات بثلاثة متغيرات');
const ledger=get(base+'lesson-028-source-elements.json');assert.equal(ledger.sourceElements.length,28);assert.deepEqual(ledger.sourcePhysicalPdfPages,[48,49,50,51,52,53,54]);
});
test('Arabic lesson covers unique, inconsistent, free-variable, contextual systems',()=>{
const p=get('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json');
const l=p.Lessons.find(x=>x.LessonCode===code);assert.ok(l);assert.match(l.AdaptationStatus,/READY_FOR_STAGE_D/);
const r=get('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u03-l04-ar.rich-lesson-v2.json').Lessons[0];
assert.equal(r.WorkedExamples.length,12);assert.equal(r.Visuals.length,6);
assert.ok(r.WorkedExamples.some(x=>x.Question.includes('نوع حل')));
});
test('Exact 3x3 question families never pretend to solve singular or rational cases',()=>{
const c=get(base+'lesson-028-stage-c-exact-contract.json');
const m=get('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json').mappings.find(x=>x.lessonCode===code);
assert.ok(m);assert.deepEqual(m.allowedQuestionFamilies,c.allowedQuestionFamilies);
assert.equal(c.testsRun,'WRITTEN_NOT_RUN');assert.equal(c.runtimeGate.productionRouting,false);
const engine=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.UaeG10ThreeVarSystem.cs'),'utf8');
assert.match(engine,/UaeG10ThreeVarSystemResponse/);assert.match(engine,/Dx%D/);
const examples=[[[1,1,1],[1,-1,1],[2,1,-1]],[6,4,2],[2,1,3]];
for(let i=0;i<3;i++) assert.equal(examples[0][i].reduce((a,v,j)=>a+v*examples[2][j],0),examples[1][i]);
});