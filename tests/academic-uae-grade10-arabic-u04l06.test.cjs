'use strict';
// Stage A–B–C authoring only. Tests WRITTEN_NOT_RUN until Stage D.
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const base='artifacts/curriculum-delivery/UAE-MOE-MATH/G10/';
const code='PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U04:L06:C039';
const bp=read('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g10-general-t1-ogl-v1.lesson-blueprint.json');
const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json');
const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u04-l06-ar.rich-lesson-v2.json');
const source=read(base+'lesson-039-source-elements.json');
const contract=read(base+'lesson-039-stage-c-exact-contract.json');
const mapping=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
const registry=read('src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json');

test('Owner approved canonical 148 index and U04L06 learner identity have exact sources',()=>{
 assert.equal(bp.Lessons.length,148);
 const row=bp.Lessons.find(x=>x.LessonCode===code);assert.ok(row);
 assert.equal(row.SortOrder,39);assert.equal(row.UnitNumber,4);assert.equal(row.LessonNumber,6);
 assert.equal(row.Title,'المتباينات التربيعية');
 const lesson=pack.Lessons.find(x=>x.LessonCode===code);assert.ok(lesson);
 assert.match(lesson.AdaptationStatus,/READY_FOR_STAGE_D/);
 assert.match(lesson.SourceLocator,/175–180/);
 assert.equal(lesson.SourceSha256,'0a554589f4cc72537007f3229098c1ab46775d7bf2301bae59f38fe6861a9815');
 const ar=lesson.Translations.find(x=>x.CultureCode==='ar');assert.ok(ar);
 for(const k of ['Explanation','KeyConceptsAndRules','WorkedExamples','StepByStepSolutions','CommonMistakes','QuickSummary'])assert.ok(ar[k].length>170,k);
});
test('Original Arabic inequality instruction includes graphical and interval examples',()=>{
 const x=rich.Lessons[0];
 assert.equal(x.WorkedExamples.length,14);assert.equal(x.Visuals.length,5);
 assert.equal(x.Visuals.filter(y=>y.Kind==='CartesianPlot').length,3);
 assert.ok(x.WorkedExamples.some(y=>y.Answer.includes('−٢<س<٤')));
 for(const t of [-5,-3,0,1,4,5]){
  const f=t*t-2*t-8;
  assert.equal(f<0,t>-2&&t<4);
 }
 assert.equal((-2)*(-2)-2*(-2)-8,0);assert.equal(4*4-2*4-8,0);
});
test('Original Advanced Unit2 lesson2-8 source elements retain six page boundaries',()=>{
 assert.deepEqual(source.sourcePdfPages,[175,176,177,178,179,180]);
 assert.equal(source.sourceElements.length,24);
 assert.equal(new Set(source.sourceElements.map(x=>x.elementId)).size,24);
 assert.ok(source.sourceElements.every(x=>x.appLocation&&x.goal&&x.sourcePrintedPage===x.sourcePhysicalPage-17));
 assert.equal(source.testsRun,'WRITTEN_NOT_RUN');
});
test('Three source-scoped exact families have one skill, numeric responses and fail-closed routing',()=>{
 const m=mapping.mappings.find(x=>x.lessonCode===code);assert.ok(m);
 assert.deepEqual(m.primarySkills,['algebra.quadratic.inequality_sign_chart']);
 assert.deepEqual(m.allowedQuestionFamilies,contract.allowedQuestionFamilies);
 assert.equal(contract.testsRun,'WRITTEN_NOT_RUN');
 for(const f of contract.allowedQuestionFamilies){
  const x=registry.families.find(z=>z.id===f);assert.ok(x);
  assert.equal(x.answerType,'exact_integer');
  assert.equal(x.productionRouting,false);
  assert.equal(x.skillId,m.primarySkills[0]);
 }
 const engine=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'),'utf8');
 assert.match(engine,/BuildG10QuadraticInequality/);
});
