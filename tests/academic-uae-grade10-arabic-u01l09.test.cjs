'use strict';
// Grade10 General U01L09 Stage A/B/C source tests; written but NOT RUN until Stage D.
const test=require('node:test'),assert=require('node:assert/strict');
const path=require('node:path'),fs=require('node:fs');
const root=path.resolve(__dirname,'..'),load=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const base='artifacts/curriculum-delivery/UAE-MOE-MATH/G10/';
const code='PED:UAE-MOE-MATH:L10:GENERAL:01:09:PERFECT-SQUARES';
const blueprint=load('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g10-general-t1-ogl-v1.lesson-blueprint.json');
const pack=load('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json');
const rich=load('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u01-l09-ar.rich-lesson-v2.json');
const source=load(base+'lesson-009-source-elements.json');
const contract=load(base+'lesson-009-stage-c-exact-contract.json');
const mapping=load('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
const families=load('src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json');

test('Owner-approved 148/18 catalogue keeps grade10 lesson9 legacy identity and original source page boundary',()=>{
 assert.equal(blueprint.Units.length,18);assert.equal(blueprint.Lessons.length,148);
 const bp=blueprint.Lessons.find(x=>x.LessonCode===code);
 assert.ok(bp);assert.equal(bp.UnitNumber,1);assert.equal(bp.LessonNumber,9);
 assert.equal(bp.UnitTitle,'كثيرات الحدود والتحليل');assert.equal(bp.Title,'المربعات الكاملة');
 const l=pack.Lessons.find(x=>x.LessonCode===code);
 assert.ok(l);assert.match(l.SourceLocator,/PDF pages 77–84.*printed pages 64–71/);
 assert.equal(l.SourceSha256,'d7e492a960675c30ac7eebb0144ed2dc51d1f4e9c91c9834ea05638f614a3c72');
 assert.match(l.AdaptationStatus,/READY_FOR_STAGE_D/);
 const ar=l.Translations.find(x=>x.CultureCode==='ar');
 assert.ok(ar);assert.match(ar.Explanation,/ثلاثية الحدود المربعة الكاملة/);
 assert.ok(ar.WorkedExamples.length>2000);
});

test('Arabic source-authored RichV2 has square, near miss, rational/irrational roots and fall context',()=>{
 const l=rich.Lessons[0];
 assert.equal(rich.Lessons.length,1);assert.equal(l.LessonCode,code);
 assert.equal(l.WorkedExamples.length,11);assert.equal(l.Visuals.length,6);
 assert.ok(l.Visuals.some(x=>x.Kind==='ExpressionStripBars'));
 assert.ok(l.WorkedExamples.some(x=>x.Answer==='(٤س+٣)²'));
 assert.ok(l.WorkedExamples.some(x=>x.Answer.includes('٢√٣')));
 assert.ok(l.WorkedExamples.some(x=>x.Answer.includes('٣ ثوان')));
 for(const x of [-3,-1,0,1,2,5]){
  assert.equal(16*x*x+24*x+9,(4*x+3)**2);
  assert.equal(25*x*x-30*x+9,(5*x-3)**2);
  assert.equal(3*x*x+30*x+75,3*(x+5)**2);
  assert.notEqual(9*x*x-6*x+4,(3*x-2)**2);
 }
});

test('All 27 original book source element categories mapped and not claimed to be tests run',()=>{
 assert.deepEqual(source.sourcePdfPages,[77,78,79,80,81,82,83,84]);
 assert.equal(source.sourceElements.length,27);
 assert.equal(new Set(source.sourceElements.map(x=>x.elementId)).size,27);
 assert.equal(source.sourceElementDenominatorVerifiedForThisLesson,true);
 assert.ok(source.sourceElements.every(x=>x.appLocation&&x.sourcePrintedPage===x.sourcePhysicalPage-13));
 assert.equal(source.testsRun,'WRITTEN_NOT_RUN');
});

test('Perfect-square skill uses three correct inverse/exact families, not other course multiplication/difference',()=>{
 const row=mapping.mappings.find(x=>x.lessonCode===code);
 assert.ok(row);assert.deepEqual(row.primarySkills,['algebra.factor.perfect_square_trinomial']);
 assert.deepEqual(row.allowedQuestionFamilies,contract.allowedQuestionFamilies);
 assert.equal(contract.runtimeGate.productionRouting,false);assert.equal(contract.testsRun,'WRITTEN_NOT_RUN');
 assert.ok(row.forbiddenQuestionFamilies.includes('uae.g10.general.u01.l04.full_special_product'));
 for(const id of contract.allowedQuestionFamilies){
  const found=families.families.find(x=>x.id===id);
  assert.ok(found,id);assert.equal(found.skillId,'algebra.factor.perfect_square_trinomial');
  assert.equal(found.productionRouting,false);assert.equal(found.lessonPracticeRouting,true);
 }
 const helper=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.UaeG10PerfectSquares.cs'),'utf8');
 const main=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'),'utf8');
 assert.match(helper,/internal static bool Verify\(string family/);
 assert.match(main,/UaeG10PerfectSquaresResponse\.Verify\(family,parameters,answer\)/);
 assert.match(main,/BuildG10PerfectSquares\(family,random,difficulty\)/);
});
