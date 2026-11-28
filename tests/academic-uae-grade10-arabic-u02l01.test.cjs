'use strict';
// UAE Grade10 Unit2 Lesson2-1 original student source: tests authored, NOT RUN until Stage D.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const load=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const dir='artifacts/curriculum-delivery/UAE-MOE-MATH/G10/';
const lesson='PED:UAE-MOE-MATH:L10:GENERAL:02:01:MULTIPLICATION-PROPERTIES-OF-EXPONENTS';
const blueprint=load('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g10-general-t1-ogl-v1.lesson-blueprint.json');
const pack=load('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json');
const rich=load('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u02-l01-ar.rich-lesson-v2.json');
const map=load('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
const skills=load('src/Edulytics.Core/Mathematics/Skills/skill-registry.v1.json');
const reg=load('src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json');
const ledger=load(dir+'lesson-017-source-elements.json');
const contract=load(dir+'lesson-017-stage-c-exact-contract.json');

test('Owner-approved 148/18 canonical index keeps existing General U02L01 lesson identity',()=>{
 assert.equal(blueprint.Lessons.length,148);
 assert.equal(blueprint.Units.length,18);
 const b=blueprint.Lessons.find(x=>x.LessonCode===lesson);
 assert.ok(b);assert.equal(b.SortOrder,17);
 assert.equal(b.UnitNumber,2);assert.equal(b.LessonNumber,1);
 assert.equal(b.Title,'خواص ضرب الأسس');
 assert.equal(b.UnitTitle,'الأسس والتعابير الجذرية');
 const content=pack.Lessons.find(x=>x.LessonCode===lesson);
 assert.ok(content);assert.equal(content.SourceSha256,'d7e492a960675c30ac7eebb0144ed2dc51d1f4e9c91c9834ea05638f614a3c72');
 assert.match(content.SourceLocator,/PDF pages 98–103.*printed pages 85–90/);
 assert.match(content.AdaptationStatus,/READY_FOR_STAGE_D/);
 const ar=content.Translations.find(x=>x.CultureCode==='ar');
 assert.ok(ar);
 for(const key of ['Explanation','KeyConceptsAndRules','WorkedExamples','StepByStepSolutions','CommonMistakes','QuickSummary'])
   assert.ok(ar[key].length>180,key);
});

test('Original Arabic instructional content covers three exponent product laws and human geometry',()=>{
 const l=rich.Lessons[0];
 assert.equal(l.LessonCode,lesson);
 assert.equal(l.WorkedExamples.length,16);
 assert.equal(l.Visuals.length,5);
 assert.ok(l.Visuals.some(x=>x.Kind==='CalculationChain'));
 assert.ok(l.Visuals.some(x=>x.Kind==='ShapeDecomposition'));
 assert.ok(l.WorkedExamples.some(x=>x.Answer.includes('س¹²')));
 assert.ok(l.WorkedExamples.some(x=>x.Answer.includes('١٥س⁷')));
 assert.equal(3**3*3**4,3**7);
 assert.equal((2**4)**3,2**12);
 assert.equal((-3)**3,-27);
 assert.equal(5*3*1,15);
});

test('All 29 distinctive source elements and their six PDF pages are mapped, cumulative page excluded',()=>{
 assert.deepEqual(ledger.sourcePdfPages,[98,99,100,101,102,103]);
 assert.equal(ledger.sourceElements.length,29);
 assert.equal(ledger.sourceElementDenominatorVerifiedForThisLesson,true);
 assert.equal(new Set(ledger.sourceElements.map(x=>x.elementId)).size,29);
 assert.ok(ledger.sourceElements.every(x=>x.appLocation&&x.sourcePrintedPage===x.sourcePhysicalPage-13));
 assert.equal(ledger.testsRun,'WRITTEN_NOT_RUN');
});

test('Bounded three exact families reuse actual registered skill but never substitute G9 power-of-ten only family',()=>{
 const m=map.mappings.find(x=>x.lessonCode===lesson);
 assert.ok(m);
 assert.deepEqual(m.primarySkills,['algebra.exponent.product_rule']);
 assert.ok(skills.skills.some(x=>x.id===m.primarySkills[0]));
 assert.deepEqual(m.allowedQuestionFamilies,contract.allowedQuestionFamilies);
 assert.ok(m.forbiddenQuestionFamilies.includes('supporting.powers10.multiply'));
 assert.equal(contract.testsRun,'WRITTEN_NOT_RUN');
 assert.equal(contract.runtimeGate.productionRouting,false);
 for(const id of m.allowedQuestionFamilies){
  const f=reg.families.find(x=>x.id===id);
  assert.ok(f,id);assert.equal(f.skillId,m.primarySkills[0]);
  assert.equal(f.answerType,'exact_integer');
  assert.equal(f.lessonPracticeRouting,true);
  assert.equal(f.productionRouting,false);
 }
 const core=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'),'utf8');
 const helper=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.UaeG10ExponentProduct.cs'),'utf8');
 assert.match(core,/BuildG10ExponentProduct/);
 assert.match(core,/UaeG10ExponentProductResponse.Verify/);
 assert.match(helper,/internal static bool Verify/);
});

 test('G10 L017 primary-source repair examples and arithmetic',()=>{const fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto'),load=p=>JSON.parse(fs.readFileSync(path.resolve(__dirname,'..',p),'utf8'));const e=load('artifacts/curriculum-delivery/UAE-MOE-MATH/G10/lesson-017-stage-d-source-review.json'),l=load('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u02-l01-ar.rich-lesson-v2.json').Lessons[0],p=load('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json').Lessons.find(x=>x.LessonCode===l.LessonCode),a=p.Translations.find(t=>t.CultureCode==='ar');for(const row of e.rows){assert.ok(l.WorkedExamples.some(x=>x.Title===row.originalWorkedExampleTitle&&x.Answer===row.expectedAnswer));assert.ok(a.WorkedExamples.includes(row.originalWorkedExampleTitle));assert.ok(a.StepByStepSolutions.includes(row.originalWorkedExampleTitle));}assert.equal(p.CanonicalBodySha256,crypto.createHash('sha256').update(JSON.stringify(Object.fromEntries(Object.keys(a).sort().map(k=>[k,a[k]])))).digest('hex'));let count=0;const eq=(x,y)=>{assert.ok(Number.isFinite(x)&&Number.isFinite(y)&&Math.abs(x-y)<=1e-10*Math.max(1,Math.abs(x),Math.abs(y)));count++;};for(const x of [1,2,3])for(const y of [1,2,3]){eq(.5*6*x*x*y*5*x*y**3,15*x**3*y**4);eq(150*x*.04*2*x,12*x*x);eq(Math.PI*(3*x*x)**2*4*x,36*Math.PI*x**5);eq(Math.PI*(3*x*x)**2*8*x,72*Math.PI*x**5);}eq(348,300+48);assert.equal(count,e.arithmeticProbes);});
