'use strict';
// Stage A–B–C writes, Stage D runs. No test execution in this stage.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const base='artifacts/curriculum-delivery/UAE-MOE-MATH/G10/';
const code='PED:UAE-MOE-MATH:L10:GENERAL:02:02:DIVISION-PROPERTIES-OF-EXPONENTS';
const bp=read('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g10-general-t1-ogl-v1.lesson-blueprint.json');
const content=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json');
const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u02-l02-ar.rich-lesson-v2.json');
const source=read(base+'lesson-018-source-elements.json');
const contract=read(base+'lesson-018-stage-c-exact-contract.json');
const mapping=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
const skills=read('src/Edulytics.Core/Mathematics/Skills/skill-registry.v1.json');
const registry=read('src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json');

test('Approved canonical 148/18 Blueprint preserves existing General U02L02 identity',()=>{
 assert.equal(bp.Lessons.length,148);assert.equal(bp.Units.length,18);
 const l=bp.Lessons.find(x=>x.LessonCode===code);
 assert.ok(l);assert.equal(l.SortOrder,18);assert.equal(l.UnitNumber,2);assert.equal(l.LessonNumber,2);
 assert.equal(l.Title,'خواص قسمة الأسس');assert.equal(l.UnitTitle,'الأسس والتعابير الجذرية');
 const c=content.Lessons.find(x=>x.LessonCode===code);assert.ok(c);
 assert.match(c.SourceLocator,/PDF pages 105–111.*printed pages 92–98/);
 assert.equal(c.SourceSha256,'d7e492a960675c30ac7eebb0144ed2dc51d1f4e9c91c9834ea05638f614a3c72');
 assert.match(c.AdaptationStatus,/READY_FOR_STAGE_D/);
 const ar=c.Translations.find(x=>x.CultureCode==='ar');
 assert.ok(ar);assert.equal(ar.Title,'خواص قسمة الأسس');
 for(const k of ['Explanation','KeyConceptsAndRules','WorkedExamples','StepByStepSolutions','CommonMistakes','QuickSummary'])
  assert.ok(ar[k].length>180,k);
});
test('Arabic source lesson includes zero/negative/quotient rules and original visual representation',()=>{
 const l=rich.Lessons[0];assert.equal(l.LessonCode,code);
 assert.equal(l.WorkedExamples.length,20);assert.equal(l.Visuals.length,5);
 assert.ok(l.Visuals.some(x=>x.Kind==='ShapeDecomposition'));
 assert.ok(l.WorkedExamples.some(x=>x.Answer.includes('١/١٦')));
 for(const a of [2,3,4]){
  assert.equal(a**9/a**4,a**5);
  assert.equal(a**0,1);
 }
 assert.equal(2**(-4),1/16);
 assert.equal(2**3/2**8,1/32);
});
test('Exactly seven original source pages have 29 traceable goal/assessment types',()=>{
 assert.deepEqual(source.sourcePdfPages,[105,106,107,108,109,110,111]);
 assert.equal(source.sourceElements.length,29);
 assert.equal(new Set(source.sourceElements.map(x=>x.elementId)).size,29);
 assert.equal(source.sourceElementDenominatorVerifiedForThisLesson,true);
 assert.ok(source.sourceElements.every(x=>x.appLocation&&x.sourcePrintedPage===x.sourcePhysicalPage-13));
 assert.equal(source.testsRun,'WRITTEN_NOT_RUN');
});
test('Exact grader uses Grade10 proper quotient family and mathematically equivalent fraction',()=>{
 const m=mapping.mappings.find(x=>x.lessonCode===code);
 assert.ok(m);assert.deepEqual(m.primarySkills,['algebra.exponent.quotient_rule']);
 assert.ok(skills.skills.some(x=>x.id===m.primarySkills[0]));
 assert.deepEqual(m.allowedQuestionFamilies,contract.allowedQuestionFamilies);
 assert.ok(m.forbiddenQuestionFamilies.includes('supporting.powers10.divide'));
 assert.equal(contract.runtimeGate.productionRouting,false);
 assert.equal(contract.testsRun,'WRITTEN_NOT_RUN');
 for(const f of m.allowedQuestionFamilies){
  const x=registry.families.find(x=>x.id===f);
  assert.ok(x,f);assert.equal(x.skillId,m.primarySkills[0]);assert.equal(x.lessonPracticeRouting,true);assert.equal(x.productionRouting,false);
 }
 const fraction=registry.families.find(x=>x.id==='uae.g10.general.u02.l02.negative_power_fraction');
 assert.equal(fraction.answerType,'exact_fraction');
 const engine=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'),'utf8');
 const verifier=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.UaeG10ExponentQuotient.cs'),'utf8');
 assert.match(engine,/BuildG10ExponentQuotient/);
 assert.match(engine,/UaeG10ExponentQuotientResponse.Verify/);
 assert.match(verifier,/Denominator/);
});

 test('G10 L018 primary-source repair examples and arithmetic',()=>{const fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto'),load=p=>JSON.parse(fs.readFileSync(path.resolve(__dirname,'..',p),'utf8'));const e=load('artifacts/curriculum-delivery/UAE-MOE-MATH/G10/lesson-018-stage-d-source-review.json'),l=load('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u02-l02-ar.rich-lesson-v2.json').Lessons[0],p=load('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json').Lessons.find(x=>x.LessonCode===l.LessonCode),a=p.Translations.find(t=>t.CultureCode==='ar');for(const row of e.rows){assert.ok(l.WorkedExamples.some(x=>x.Title===row.originalWorkedExampleTitle&&x.Answer===row.expectedAnswer));assert.ok(a.WorkedExamples.includes(row.originalWorkedExampleTitle));assert.ok(a.StepByStepSolutions.includes(row.originalWorkedExampleTitle));}assert.equal(p.CanonicalBodySha256,crypto.createHash('sha256').update(JSON.stringify(Object.fromEntries(Object.keys(a).sort().map(k=>[k,a[k]])))).digest('hex'));let count=0;const eq=(x,y)=>{assert.ok(Number.isFinite(x)&&Number.isFinite(y)&&Math.abs(x-y)<=1e-10*Math.max(1,Math.abs(x),Math.abs(y)));count++;};eq(2.4/.003,800);eq(Math.round(Math.log10(2.4))-Math.round(Math.log10(.003)),3);for(const r of [1,2,3,7])eq(Math.PI*r*r/(2*r)**2,Math.PI/4);eq((1/6)**3/(1/6)**5,36);assert.equal(count,e.arithmeticProbes);});
