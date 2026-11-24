'use strict';
// Grade10 General U01L05 first-party source-specific Stage A/B/C evidence.
// Intentionally not executed until Stage D.
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path'),root=path.resolve(__dirname,'..');
const load=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const base='artifacts/curriculum-delivery/UAE-MOE-MATH/G10/';
const lessonCode='PED:UAE-MOE-MATH:L10:GENERAL:01:05:USING-THE-DISTRIBUTIVE-PROPERTY';
const bp=load('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g10-general-t1-ogl-v1.lesson-blueprint.json');
const pack=load('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json');
const rich=load('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u01-l05-ar.rich-lesson-v2.json');
const source=load(base+'lesson-005-source-elements.json');
const contract=load(base+'lesson-005-stage-c-exact-contract.json');
const mappings=load('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
const families=load('src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json');
const skills=load('src/Edulytics.Core/Mathematics/Skills/skill-registry.v1.json');

test('Canonical G10 U01L05 matches approved index and accurately scoped Arabic source',()=>{
 assert.equal(bp.Lessons.length,148);assert.equal(bp.Units.length,18);
 const b=bp.Lessons.find(l=>l.LessonCode===lessonCode);
 assert.ok(b);assert.equal(b.UnitNumber,1);assert.equal(b.LessonNumber,5);
 assert.equal(b.Title,'استخدام خاصية التوزيع');
 assert.equal(b.UnitTitle,'كثيرات الحدود والتحليل');
 const l=pack.Lessons.find(l=>l.LessonCode===lessonCode);
 assert.ok(l);
 assert.match(l.SourceLocator,/PDF pages 49–54.*printed pages 36–41/);
 assert.equal(l.SourceSha256,'d7e492a960675c30ac7eebb0144ed2dc51d1f4e9c91c9834ea05638f614a3c72');
 assert.match(l.AdaptationStatus,/READY_FOR_STAGE_D/);
 const ar=l.Translations.find(t=>t.CultureCode==='ar');
 assert.ok(ar);assert.equal(ar.Title,'استخدام خاصية التوزيع');
 for(const f of ['Explanation','KeyConceptsAndRules','WorkedExamples','StepByStepSolutions','CommonMistakes','QuickSummary'])
  assert.ok(ar[f].length>250,f);
 assert.match(ar.Explanation,/العوامل/);
 assert.match(ar.Explanation,/الضرب الصفري/);
});
test('Original Grade10 RichV2 provides inverse-distribution and zero-product with visuals',()=>{
 const r=rich.Lessons[0];assert.equal(r.LessonCode,lessonCode);
 assert.equal(r.WorkedExamples.length,10);
 assert.equal(r.Visuals.length,5);
 assert.ok(r.WorkedExamples.some(x=>x.Answer.includes('٦س(٢س+٣)')));
 assert.ok(r.WorkedExamples.some(x=>x.Answer.includes('٠ و٣')));
 assert.ok(r.Visuals.some(x=>x.Kind==='ExpressionStripBars'));
 for(const x of [-3,-1,0,1,2,5]){
  assert.equal(12*x*x+18*x,6*x*(2*x+3));
  assert.equal(3*x*x+6*x+2*x+4,(3*x+2)*(x+2));
  assert.equal(x*x-3*x+2*x-6,(x+2)*(x-3));
 }
 assert.equal(-16*1.25**2+20*1.25,0);
});
test('Unique source lesson has 24 atomic categories, excluding cumulative unit review',()=>{
 assert.deepEqual(source.sourcePdfPages,[49,50,51,52,53,54]);
 assert.equal(source.sourceElements.length,24);
 assert.equal(new Set(source.sourceElements.map(x=>x.elementId)).size,24);
 assert.equal(source.sourceElementDenominatorVerifiedForThisLesson,true);
 assert.equal(source.supportingSourceReview.pdfPage,55);
 assert.ok(source.sourceElements.every(x=>x.sourcePrintedPage===x.sourcePhysicalPage-13&&x.appLocation));
 assert.equal(source.testsRun,'WRITTEN_NOT_RUN');
});
test('Practice families map to one registered GCF+zero-product skill, with disabled routing',()=>{
 const m=mappings.mappings.find(x=>x.lessonCode===lessonCode);
 assert.ok(m);assert.deepEqual(m.primarySkills,['algebra.polynomial.factor_gcf']);
 assert.ok(skills.skills.some(s=>s.id===m.primarySkills[0]));
 assert.deepEqual(m.allowedQuestionFamilies,contract.allowedQuestionFamilies);
 assert.equal(contract.testsRun,'WRITTEN_NOT_RUN');
 assert.equal(contract.runtimeGate.productionRouting,false);
 for(const id of m.allowedQuestionFamilies){
  const f=families.families.find(x=>x.id===id);
  assert.ok(f,id);assert.equal(f.skillId,m.primarySkills[0]);assert.equal(f.productionRouting,false);
  assert.equal(f.lessonPracticeRouting,true);
 }
 const engine=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'),'utf8');
 const source=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.UaeG10DistributiveFactor.cs'),'utf8');
 assert.match(engine,/BuildG10DistributiveFactor/);
 assert.match(source,/UaeG10DistributiveFactorResponse.Verify/);
});
