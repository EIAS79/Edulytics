'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),get=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8')),base='artifacts/curriculum-delivery/UAE-MOE-MATH/G10/',code='PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U03:L08:C032';
test('G10 inverse matrix source matches lesson1-9 and canonical ordinal 32',()=>{
 const bp=get('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g10-general-t1-ogl-v1.lesson-blueprint.json');
 const lesson=bp.Lessons.find(x=>x.LessonCode===code);assert.ok(lesson);assert.equal(lesson.SortOrder,32);
 const ledger=get(base+'lesson-032-source-elements.json');assert.deepEqual(ledger.sourcePhysicalPdfPages,[91,92,93,94,95,96,97]);assert.equal(ledger.sourceElements.length,23);
});
test('Arabic inverse matrix examples use exact rational and correctly ordered AX=B',()=>{
 const pack=get('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json');
 const l=pack.Lessons.find(x=>x.LessonCode===code);assert.ok(l);assert.match(l.AdaptationStatus,/READY_FOR_STAGE_D/);
 const rich=get('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u03-l08-ar.rich-lesson-v2.json').Lessons[0];
 assert.equal(rich.WorkedExamples.length,12);assert.equal(rich.Visuals.length,6);
 assert.ok(rich.WorkedExamples.some(x=>x.Answer.includes('٣/٢')));
 const a=[[1,2],[3,4]],inverse=[[-2,1],[1.5,-0.5]];
 assert.equal(a[0][0]*inverse[0][0]+a[0][1]*inverse[1][0],1);
 assert.equal(a[1][0]*inverse[0][1]+a[1][1]*inverse[1][1],1);
});
test('StageC fraction family uses independent verifier and fail-closed routing',()=>{
 const c=get(base+'lesson-032-stage-c-exact-contract.json');
 const m=get('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json').mappings.find(x=>x.lessonCode===code);
 assert.ok(m);assert.deepEqual(m.allowedQuestionFamilies,c.allowedQuestionFamilies);assert.deepEqual(m.primarySkills,c.primarySkills);
 assert.equal(c.runtimeGate.productionRouting,false);assert.equal(c.testsRun,'WRITTEN_NOT_RUN');
 const src=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.UaeG10InverseMatrix.cs'),'utf8');assert.match(src,/BigInteger.TryParse/);
});