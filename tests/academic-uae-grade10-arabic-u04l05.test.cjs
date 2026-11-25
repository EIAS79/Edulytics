'use strict';
// Stage A/B/C: tests are authored; execution requires Stage D.
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),load=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const base='artifacts/curriculum-delivery/UAE-MOE-MATH/G10/';
const code='PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U04:L05:C038';
const blueprint=load('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g10-general-t1-ogl-v1.lesson-blueprint.json');
const pack=load('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json');
const rich=load('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u04-l05-ar.rich-lesson-v2.json');
const ledger=load(base+'lesson-038-source-elements.json');
const contract=load(base+'lesson-038-stage-c-exact-contract.json');
const mapping=load('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
const families=load('src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json');

test('Approved G10 148 lesson index retains unique canonical38 and student content identity',()=>{
 assert.equal(blueprint.Lessons.length,148);
 const b=blueprint.Lessons.find(x=>x.LessonCode===code);
 assert.ok(b);assert.equal(b.SortOrder,38);
 assert.equal(b.UnitNumber,4);assert.equal(b.LessonNumber,5);
 assert.equal(b.Title,'القانون العام والمميز للمعادلة التربيعية');
 const l=pack.Lessons.find(x=>x.LessonCode===code);
 assert.ok(l);assert.match(l.AdaptationStatus,/READY_FOR_STAGE_D/);
 assert.equal(l.SourceSha256,'0a554589f4cc72537007f3229098c1ab46775d7bf2301bae59f38fe6861a9815');
 assert.match(l.SourceLocator,/157–164/);
 const ar=l.Translations.find(x=>x.CultureCode==='ar');
 assert.ok(ar);assert.match(ar.Explanation,/المميز|المميّز/);
 for(const k of ['Explanation','KeyConceptsAndRules','WorkedExamples','StepByStepSolutions','CommonMistakes','QuickSummary'])
  assert.ok(ar[k].length>150,k);
});
test('First-party quadratic formula RichV2 contains 10 solved examples and 3 original graphs',()=>{
 const l=rich.Lessons[0];
 assert.equal(l.LessonCode,code);
 assert.equal(l.WorkedExamples.length,10);
 assert.equal(l.Visuals.length,5);
 assert.equal(l.Visuals.filter(x=>x.Kind==='CartesianPlot').length,3);
 for(const x of [11,-1])assert.equal(x*x-10*x-11,0);
 assert.equal(8*8-4*1*16,0);
 assert.equal(6*6-4*2*(-7),92);
 assert.equal((-6)*(-6)-4*1*10,-4);
 for(const t of [-1,5])assert.equal(-5*t*t+20*t+25,0);
});
test('Source edition PDF157-164 element ledger is unique and graph lab is routed to canonical U04L07',()=>{
 assert.deepEqual(ledger.sourcePdfPages,[157,158,159,160,161,162,163,164]);
 assert.equal(ledger.sourceElements.length,24);
 assert.equal(new Set(ledger.sourceElements.map(x=>x.elementId)).size,24);
 assert.ok(ledger.sourceElements.every(x=>x.appLocation&&x.coverageStatus&&x.sourcePrintedPage===x.sourcePhysicalPage-17));
 assert.equal(ledger.relatedGraphLabDestination,'G10-U04-L07 quadratic function transformation — NOT duplicated in 38');
 assert.equal(ledger.testStatus,'WRITTEN_NOT_RUN');
});
test('Exact families match primary formula skill and remain fail closed pending Stage D',()=>{
 const m=mapping.mappings.find(x=>x.lessonCode===code);
 assert.ok(m);assert.deepEqual(m.primarySkills,['algebra.quadratic.formula_discriminant']);
 assert.deepEqual(m.allowedQuestionFamilies,contract.allowedQuestionFamilies);
 assert.equal(contract.testsRun,'WRITTEN_NOT_RUN');
 assert.equal(contract.productionRouting,false);
 for(const id of m.allowedQuestionFamilies){
  const f=families.families.find(x=>x.id===id);
  assert.ok(f,id);assert.equal(f.skillId,m.primarySkills[0]);
  assert.equal(f.productionRouting,false);
 }
 const engine=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'),'utf8');
 const math=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.UaeG10QuadraticFormula.cs'),'utf8');
 assert.match(engine,/BuildG10QuadraticFormula/);
 assert.match(math,/UaeG10QuadraticFormulaResponse/);
});
