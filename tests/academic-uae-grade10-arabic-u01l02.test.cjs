'use strict';
// Authored in Grade10 Stage A–B–C. Run only under Stage D.
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const load=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const p='artifacts/curriculum-delivery/UAE-MOE-MATH/G10/';
const lessonCode='PED:UAE-MOE-MATH:L10:GENERAL:01:02:MULTIPLYING-A-POLYNOMIAL-BY-A-MONOMIAL';
const pack=load('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json');
const blueprint=load('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g10-general-t1-ogl-v1.lesson-blueprint.json');
const rich=load('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u01-l02-ar.rich-lesson-v2.json');
const mapping=load('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
const families=load('src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json');
const contract=load(p+'lesson-002-stage-c-exact-contract.json');
const ledger=load(p+'lesson-002-source-elements.json');

test('Canonical U01L02 preserves existing grade10 lesson identity and source pages',()=>{
 assert.equal(blueprint.Units.length,18);
 assert.equal(blueprint.Lessons.length,148);
 const bp=blueprint.Lessons.find(x=>x.LessonCode===lessonCode);
 assert.ok(bp);
 assert.equal(bp.UnitNumber,1);assert.equal(bp.LessonNumber,2);
 assert.equal(bp.UnitTitle,'كثيرات الحدود والتحليل');
 assert.equal(bp.Title,'ضرب كثيرة حدود في أحادية حد');
 const lesson=pack.Lessons.find(x=>x.LessonCode===lessonCode);
 assert.ok(lesson);
 assert.equal(lesson.SourceSha256,'d7e492a960675c30ac7eebb0144ed2dc51d1f4e9c91c9834ea05638f614a3c72');
 assert.match(lesson.SourceLocator,/PDF pages 27–32.*printed pages 14–19/);
 assert.match(lesson.AdaptationStatus,/READY_FOR_STAGE_D/);
 assert.equal(lesson.Translations.filter(x=>x.CultureCode==='ar').length,1);
 const ar=lesson.Translations.find(x=>x.CultureCode==='ar');
 for(const f of ['Explanation','KeyConceptsAndRules','WorkedExamples','StepByStepSolutions','CommonMistakes','QuickSummary'])
   assert.ok(ar[f].length>160,f);
 assert.match(ar.Explanation,/خاصية التوزيع|قانون التوزيع/);
});

test('Arabic Grade10 original monomial lesson has source-correct examples and visuals',()=>{
 assert.equal(rich.Lessons.length,1);
 const l=rich.Lessons[0];
 assert.equal(l.LessonCode,lessonCode);
 assert.equal(l.CultureCode,'ar');
 assert.equal(l.WorkedExamples.length,39);
 assert.equal(l.Visuals.length,8);
 assert.ok(l.Visuals.every(x=>x.Items.length>=3));
 assert.ok(l.Visuals.some(x=>x.Kind==='ExpressionStripBars'));
 assert.ok(l.Visuals.some(x=>x.Kind==='ShapeDecomposition'));
 assert.ok(l.WorkedExamples.some(x=>x.Answer.includes('٦س³−١٥س²+١٢س')));
 assert.ok(l.WorkedExamples.some(x=>x.Answer.includes('٦س³+٨س²+١٠س')));
 const P=x=>3*x*(2*x*x-5*x+4),A=x=>6*x*x*x-15*x*x+12*x;
 const Q=x=>-2*x*x*(3*x*x*x-4*x+5),B=x=>-6*x**5+8*x**3-10*x*x;
 for(const x of [-3,-1,0,1,2,5]){assert.ok(P(x)===A(x));assert.ok(Q(x)===B(x));}
});

test('Distinct U01L02 source elements and Advanced crosswalk remain traceable',()=>{
 assert.deepEqual(ledger.sourcePdfPages,[27,28,29,30,31,32]);
 assert.equal(ledger.sourceElementCount,22);
 assert.equal(ledger.elementDenominatorForThisBook,22);
 assert.equal(ledger.sourceElements.length,22);
 assert.equal(ledger.sourceElementDenominatorVerifiedForThisLesson,true);
 assert.equal(new Set(ledger.sourceElements.map(e=>e.elementId)).size,22);
 assert.ok(ledger.sourceElements.every(e=>e.sourcePrintedPage===e.sourcePhysicalPage-13&&e.appLocation));
 assert.equal(ledger.advancedOverlap.relevantCrosswalkIds.length,2);
 assert.equal(ledger.testsRun,'WRITTEN_NOT_RUN');
});

test('Exact monomial-only grade10 families are authored but production disabled',()=>{
 const m=mapping.mappings.find(x=>x.lessonCode===lessonCode);
 assert.ok(m);
 assert.deepEqual(m.primarySkills,['algebra.polynomial.multiply_distribute']);
 assert.deepEqual(m.allowedQuestionFamilies,contract.allowedQuestionFamilies);
 assert.ok(m.forbiddenQuestionFamilies.includes('uae.g9.general.u07.l03.multiply_coefficient'));
 assert.equal(contract.testsRun,'WRITTEN_NOT_RUN');
 assert.equal(contract.runtimeGate.productionRouting,false);
 assert.equal(contract.rendererMatrix[0].kind,'IntegerKeypad');
 assert.equal(contract.rendererMatrix[1].kind,'StructuredEntry');
 for(const id of contract.allowedQuestionFamilies){
   const f=families.families.find(x=>x.id===id);
   assert.ok(f,id);
   assert.equal(f.skillId,m.primarySkills[0]);
   assert.equal(f.productionRouting,false);
   assert.equal(f.lessonPracticeRouting,true);
 }
 const engine=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'),'utf8');
 const verifier=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.UaeG10Monomial.cs'),'utf8');
 assert.match(engine,/BuildG10MonomialPolynomial/);
 assert.match(verifier,/UaeG10MonomialPolynomialResponse/);
});
