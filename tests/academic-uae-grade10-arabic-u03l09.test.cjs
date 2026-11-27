'use strict';
// Grade10 General U03L09 source LAB. Tests WRITTEN_NOT_RUN in A–B–C.
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const base='artifacts/curriculum-delivery/UAE-MOE-MATH/G10/';
const code='PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U03:L09:C033';
const blueprint=read('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g10-general-t1-ogl-v1.lesson-blueprint.json');
const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json');
const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u03-l09-ar.rich-lesson-v2.json');
const source=read(base+'lesson-033-source-elements.json');
const contract=read(base+'lesson-033-stage-c-exact-contract.json');
const mappings=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
const registry=read('src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json');
test('Canonical33 is sourced from separate Advanced graphing calculator LAB, not a fake numbered lesson',()=>{
 const b=blueprint.Lessons.find(x=>x.LessonCode===code);assert.ok(b);
 assert.equal(b.SortOrder,33);assert.equal(b.UnitNumber,3);assert.equal(b.LessonNumber,9);
 assert.equal(b.Title,'المصفوفات الموسعة');
 assert.deepEqual(source.sourcePhysicalPdfPages,[98]);
 assert.deepEqual(source.sourcePrintedPages,[81]);
 assert.match(source.bookClassification,/LAB.*NOT_A_SEPARATE_NUMBERED/);
 assert.equal(source.sourceElements.length,10);
 assert.ok(source.sourceElements.every(x=>x.sourcePhysicalPage===98&&x.appLocation));
});
test('Original first-party Arabic augmented lesson covers 3x4 source and clear 2x3 digital boundary',()=>{
 const l=pack.Lessons.find(x=>x.LessonCode===code);assert.ok(l);
 assert.match(l.AdaptationStatus,/READY_FOR_STAGE_D/);
 assert.equal(l.Translations.filter(x=>x.CultureCode==='ar').length,1);
 const r=rich.Lessons[0];assert.equal(r.WorkedExamples.length,11);
 assert.equal(r.Visuals.length,5);
 assert.match(r.ExplanationParagraphs.join(' '),/RREF/);
 assert.ok(r.WorkedExamples.some(x=>x.Answer.includes('−٤')));
 assert.equal(2*(-4)+3+6,1);
 assert.equal(3*(-4)+2*3+3*6,12);
 assert.equal(4*(-4)+3+2*6,-1);
});
test('Exact StageC only authentic 2x3 numeric subset and human scored full 3x4',()=>{
 const m=mappings.mappings.find(x=>x.lessonCode===code);assert.ok(m);
 assert.deepEqual(m.primarySkills,['algebra.matrices.augmented_row_reduce']);
 assert.deepEqual(m.allowedQuestionFamilies,contract.allowedQuestionFamilies);
 assert.equal(contract.testsRun,'WRITTEN_NOT_RUN');
 assert.equal(contract.runtimeGate.productionRouting,false);
 for(const id of contract.allowedQuestionFamilies){
   const entry=registry.families.find(x=>x.id===id);assert.ok(entry,id);
   assert.equal(entry.skillId,'algebra.matrices.augmented_row_reduce');
   assert.equal(entry.productionRouting,false);
   assert.equal(entry.answerType,'exact_integer');
 }
 const e=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'),'utf8');
 const exact=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.UaeG10AugmentedMatrices.cs'),'utf8');
 assert.match(e,/BuildG10AugmentedMatrix/);
 assert.match(exact,/UaeG10AugmentedMatrixResponse.Verify|internal static bool Verify/);
});
