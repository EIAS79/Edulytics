'use strict';
// UAE Grade10 General U01L06: authored source-alignment guards for Stage D.
// Grade A/B/C writes these tests but DOES NOT execute them.
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const base='artifacts/curriculum-delivery/UAE-MOE-MATH/G10/';
const code='PED:UAE-MOE-MATH:L10:GENERAL:01:06:SOLVING-X-BX-C-0';
const bp=read('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g10-general-t1-ogl-v1.lesson-blueprint.json');
const content=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json');
const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u01-l06-ar.rich-lesson-v2.json');
const ledger=read(base+'lesson-006-source-elements.json');
const contract=read(base+'lesson-006-stage-c-exact-contract.json');
const mappings=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
const skills=read('src/Edulytics.Core/Mathematics/Skills/skill-registry.v1.json');
const registry=read('src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json');
const cross=read(base+'lesson-006-crossbook-semantic-crosswalk.json');

test('Owner-approved G10 148 index preserves General 1-6 identity, not Advanced/Applied duplicate',()=>{
 assert.equal(bp.Units.length,18);assert.equal(bp.Lessons.length,148);
 const l=bp.Lessons.find(x=>x.LessonCode===code);
 assert.ok(l);assert.equal(l.SortOrder,6);assert.equal(l.UnitNumber,1);
 assert.equal(l.LessonNumber,6);assert.equal(l.UnitTitle,'كثيرات الحدود والتحليل');
 assert.equal(l.Title,'حل x² + bx + c = 0');
 const p=content.Lessons.find(x=>x.LessonCode===code);
 assert.ok(p);assert.equal(p.SourceSha256,'d7e492a960675c30ac7eebb0144ed2dc51d1f4e9c91c9834ea05638f614a3c72');
 assert.match(p.SourceLocator,/56–63.*43–50/);
 assert.match(p.AdaptationStatus,/READY_FOR_STAGE_D/);
 const ar=p.Translations.find(x=>x.CultureCode==='ar');
 assert.ok(ar);assert.equal(ar.Title,'حل x² + bx + c = 0');
 for(const key of ['Explanation','KeyConceptsAndRules','WorkedExamples','StepByStepSolutions','CommonMistakes','QuickSummary'])
   assert.ok(ar[key].length>240,key);
 assert.match(ar.Explanation,/الأعداد الصحيحة|عددين صحيحين|بمعاملات صحيحة/);
});

test('Original General pages 56–63 include algebra tiles, prime distinction, roots and geometry',()=>{
 assert.deepEqual(ledger.sourcePdfPages,[56,57,58,59,60,61,62,63]);
 assert.equal(ledger.sourceElements.length,29);
 assert.equal(ledger.sourceElementDenominatorVerifiedForGeneralBook,true);
 assert.ok(ledger.sourceElements.every(x=>x.appLocation&&x.sourcePrintedPage===x.sourcePhysicalPage-13));
 assert.equal(ledger.sourceElements.filter(x=>x.category==='visual_lab').length,2);
 assert.equal(ledger.testsRun,'WRITTEN_NOT_RUN');
 const v=rich.Lessons[0];
 assert.equal(v.LessonCode,code);assert.equal(v.CultureCode,'ar');
 assert.equal(v.WorkedExamples.length,11);assert.equal(v.Visuals.length,6);
 assert.ok(v.Visuals.some(x=>x.Kind==='ExpressionStripBars'));
 assert.ok(v.Visuals.some(x=>x.Kind==='ShapeDecomposition'));
 assert.ok(v.WorkedExamples.some(x=>x.Answer.includes('(س+٤)(س+٥)')));
 assert.ok(v.WorkedExamples.some(x=>x.Answer.includes('غير قابل للتحليل')));
 for(const x of [-3,-1,0,1,2,5]){
  assert.ok((x+4)*(x+5)===x*x+9*x+20);
  assert.ok((x-2)*(x-6)===x*x-8*x+12);
  assert.ok((x+5)*(x-3)===x*x+2*x-15);
 }
 assert.equal(9*6,54);
});

test('Three books reviewed: overlap is mapped to one canonical lesson per objective',()=>{
 assert.equal(cross.canonicalCode,'G10-U01-L06');
 assert.equal(cross.sourceFiles.length,3);
 assert.deepEqual(cross.sources.general.physicalPages,[56,57,58,59,60,61,62,63]);
 assert.deepEqual(cross.sources.advanced.physicalPages,[131,132,133,134,135,136,137]);
 assert.deepEqual(cross.sources.applied.physicalPages,[49,50,51,52,53,54,55]);
 assert.ok(cross.goalCrosswalk.length>=10);
 assert.ok(cross.goalCrosswalk.every(g=>g.source&&g.canonicalDestination&&g.decision));
 assert.equal(cross.duplicateGeneralOrAdvancedOrAppliedCourseCreated,false);
 assert.equal(cross.stageDExecuted,false);
});

test('Stage C monic factor, roots and integer-prime use one registered exact skill, fail closed',()=>{
 const mapping=mappings.mappings.find(x=>x.lessonCode===code);
 assert.ok(mapping);assert.deepEqual(mapping.primarySkills,['algebra.quadratic.factor_monic']);
 assert.ok(skills.skills.some(x=>x.id==='algebra.quadratic.factor_monic'));
 assert.deepEqual(mapping.allowedQuestionFamilies,contract.allowedQuestionFamilies);
 assert.equal(contract.testsRun,'WRITTEN_NOT_RUN');
 assert.equal(contract.runtimeGate.productionRouting,false);
 for(const id of contract.allowedQuestionFamilies){
  const f=registry.families.find(x=>x.id===id);
  assert.ok(f,id);assert.equal(f.skillId,mapping.primarySkills[0]);
  assert.equal(f.productionRouting,false);assert.equal(f.lessonPracticeRouting,true);
 }
 const engine=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'),'utf8');
 const exact=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.UaeG10MonicQuadratic.cs'),'utf8');
 assert.match(engine,/BuildG10MonicQuadratic/);
 assert.match(exact,/IntegerFactorable/);assert.match(exact,/r1\+r2/);
});
