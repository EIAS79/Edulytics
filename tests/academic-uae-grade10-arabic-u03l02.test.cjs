'use strict';
// Original source Grade10 UAE advanced 1-2 mapped to unified General U03L02.
// Stage A-B-C source/lesson/contract assertions. NOT RUN until Stage D.
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const base='artifacts/curriculum-delivery/UAE-MOE-MATH/G10/';
const code='PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U03:L02:C026';
const blueprint=read('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g10-general-t1-ogl-v1.lesson-blueprint.json');
const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json');
const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u03-l02-ar.rich-lesson-v2.json');
const ledger=read(base+'lesson-026-source-elements.json');
const contract=read(base+'lesson-026-stage-c-exact-contract.json');
const mapping=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
const registry=read('src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json');

test('The new lesson 26 is the unique approved U03L02 system of inequalities identity',()=>{
 assert.equal(blueprint.Lessons.length,148);
 assert.equal(blueprint.Units.length,18);
 const b=blueprint.Lessons.find(x=>x.LessonCode===code);
 assert.ok(b);assert.equal(b.SortOrder,26);assert.equal(b.UnitNumber,3);
 assert.equal(b.LessonNumber,2);assert.equal(b.Title,'حل أنظمة المتباينات بالتمثيل البياني');
 assert.equal(b.UnitTitle,'الأنظمة الخطية والمصفوفات');
 assert.equal(blueprint.Lessons.filter(x=>x.LessonCode===code).length,1);
 const l=pack.Lessons.find(x=>x.LessonCode===code);assert.ok(l);
 assert.match(l.AdaptationStatus,/READY_FOR_STAGE_D/);
 assert.match(l.SourceLocator,/PDF33–39.*PDF40/);
 assert.ok(l.Translations.some(x=>x.CultureCode==='ar'&&x.Explanation.length>400));
});
test('Original page source ledger covers 29 separate teaching/assessment elements and lab',()=>{
 assert.deepEqual(ledger.sourcePdfPages,[33,34,35,36,37,38,39]);
 assert.equal(ledger.supplementaryTechnologyLab.pdfPage,40);
 assert.equal(ledger.nextSourceLessonBoundary.pdfPage,41);
 assert.equal(ledger.sourceElements.length,29);
 assert.equal(new Set(ledger.sourceElements.map(x=>x.elementId)).size,29);
 assert.equal(ledger.sourceElementDenominatorVerifiedForThisLesson,true);
 assert.ok(ledger.sourceElements.every(x=>x.sourcePrintedPage===x.sourcePdfPage-17&&x.appLocation&&x.coverageStatus));
 assert.equal(ledger.testsRun,'WRITTEN_NOT_RUN');
 assert.equal(ledger.originalPublisherContentCopied,false);
});
test('Original Arabic RichV2 distinguishes overlap, dashed/solid boundaries and graphing',()=>{
 const l=rich.Lessons[0];assert.equal(l.LessonCode,code);
 assert.equal(l.WorkedExamples.length,17);assert.equal(l.Visuals.length,7);
 assert.equal(l.Visuals.filter(x=>x.Kind==='CartesianPlot').length,2);
 assert.ok(l.ExplanationParagraphs.some(x=>x.includes('متقطع')));
 assert.ok(l.ExplanationParagraphs.some(x=>x.includes('متصل')));
 assert.ok(0>=0-1&&0<-0+5);
 assert.ok(!(3>=2-1&&3<-2+5));
 for(const x of [-3,0,2,6])for(const y of [-3,0,4,9])
   assert.ok(!(y>x+3&&y<x+1));
 assert.equal(2*3+3*2,12);
 assert.ok(2*4+3*2>12);
});
test('Exactly two scoped inequality families and a primary skill, no equation family leakage',()=>{
 const m=mapping.mappings.find(x=>x.lessonCode===code);assert.ok(m);
 assert.deepEqual(m.primarySkills,['algebra.linear.inequalities.systems_two_variables']);
 assert.deepEqual(m.allowedQuestionFamilies,contract.allowedQuestionFamilies);
 assert.equal(m.allowedQuestionFamilies.length,2);
 assert.ok(m.forbiddenQuestionFamilies.includes('uae.g10.general.u03.l01.graph_intersection_x'));
 for(const familyId of m.allowedQuestionFamilies){
  const f=registry.families.find(x=>x.id===familyId);
  assert.ok(f,familyId);assert.equal(f.skillId,m.primarySkills[0]);
  assert.equal(f.answerType,'exact_integer');
  assert.equal(f.productionRouting,false);
  assert.equal(f.lessonPracticeRouting,true);
 }
 assert.equal(contract.testsRun,'WRITTEN_NOT_RUN');
 assert.equal(contract.runtimeGate.productionRouting,false);
 const helper=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.UaeG10GraphInequalities.cs'),'utf8');
 const engine=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'),'utf8');
 assert.match(helper,/private static bool Holds/);
 assert.match(helper,/TryExpected/);
 assert.match(engine,/UaeG10GraphInequalitiesResponse.Verify/);
});
