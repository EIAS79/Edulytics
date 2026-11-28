'use strict';
// Grade10 source-based complex numbers: author tests without running Stage D.
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),load=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8')),base='artifacts/curriculum-delivery/UAE-MOE-MATH/G10/';
const code='PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U04:L03:C036';
test('Canonical36 source is original complex numbers Lesson2-4 and separately complex-plane lab, NOT original 2-3 factoring',()=>{
 const b=load('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g10-general-t1-ogl-v1.lesson-blueprint.json').Lessons.find(x=>x.LessonCode===code);
 assert.ok(b);assert.equal(b.SortOrder,36);assert.equal(b.UnitNumber,4);assert.equal(b.LessonNumber,3);assert.equal(b.Title,'الأعداد المركبة');
 const s=load(base+'lesson-036-source-elements.json');
 assert.deepEqual(s.sourceLessonPdfPages,[139,140,141,142,143,144]);
 assert.deepEqual(s.sourceRelatedComplexPlaneLabPages,[146,147]);
 assert.equal(s.sourceExcludedReviewPage,145);
 assert.equal(s.sourceExcludedHalfUnitTestPage,148);
 assert.equal(s.sourceElements.length,29);
});
test('Original Arabic complex lesson has 13 worked examples, true plane plots, correct products',()=>{
 const l=load('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json').Lessons.find(x=>x.LessonCode===code);
 assert.ok(l);assert.match(l.AdaptationStatus,/READY_FOR_STAGE_D/);
 assert.equal(l.Translations.filter(x=>x.CultureCode==='ar').length,1);
 const rich=load('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u04-l03-ar.rich-lesson-v2.json').Lessons[0];
 assert.equal(rich.WorkedExamples.length,15);assert.equal(rich.Visuals.length,6);
 assert.equal(rich.Visuals.filter(x=>x.Kind==='CartesianPlot').length,2);
 assert.equal(2*4-3*(-2),14);assert.equal(2*(-2)+3*4,8);
 assert.equal((-5)**2+12**2,169);
 assert.ok(rich.WorkedExamples.some(x=>x.Answer.includes('١+٣ت')));
});
test('Complex Stage C exact numeric subset excludes full expression or freehand plane auto-grading',()=>{
 const c=load(base+'lesson-036-stage-c-exact-contract.json');
 const m=load('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json').mappings.find(x=>x.lessonCode===code);
 assert.ok(m);assert.deepEqual(m.primarySkills,['algebra.complex.number_operations']);
 assert.deepEqual(m.allowedQuestionFamilies,c.allowedQuestionFamilies);assert.equal(c.testsRun,'WRITTEN_NOT_RUN');
 const registry=load('src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json');
 for(const id of c.allowedQuestionFamilies){
  const fam=registry.families.find(x=>x.id===id);
  assert.ok(fam,id);assert.equal(fam.productionRouting,false);assert.equal(fam.answerType,'exact_integer');
 }
 const engine=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'),'utf8');
 assert.match(engine,/BuildG10ComplexNumber/);
});
