'use strict';
// Grade10 General owner seven-book canonical U01L10: authored in A–B–C;
// Stage D Node/.NET/CI/E2E tests intentionally not executed here.
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const base='artifacts/curriculum-delivery/UAE-MOE-MATH/G10/';
const lessonCode='PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U01:L10:C010';
const bp=read('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g10-general-t1-ogl-v1.lesson-blueprint.json');
const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json');
const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u01-l10-ar.rich-lesson-v2.json');
const ledger=read(base+'lesson-010-source-elements.json');
const mapping=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
const registry=read('src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json');
const contract=read(base+'lesson-010-stage-c-exact-contract.json');

test('Grade10 canonical row ten is new unique General catalog identity, not Advanced pathway',()=>{
 assert.equal(bp.Units.length,18);assert.equal(bp.Lessons.length,148);
 const b=bp.Lessons.find(x=>x.LessonCode===lessonCode);
 assert.ok(b);
 assert.equal(b.UnitNumber,1);assert.equal(b.LessonNumber,10);
 assert.equal(b.Title,'قسمة كثيرات الحدود');assert.equal(b.UnitTitle,'كثيرات الحدود والتحليل');
 assert.equal(pack.Lessons.filter(x=>x.LessonCode===lessonCode).length,1);
 assert.equal(pack.Lessons.length,49);
 const l=pack.Lessons.find(x=>x.LessonCode===lessonCode);
 assert.equal(l.OfficialReferenceCode,null);
 assert.ok(l.SourceLocator.includes('01متقدم_.pdf physical PDF pages 204–212'));
 assert.equal(l.SourceSha256,'0a554589f4cc72537007f3229098c1ab46775d7bf2301bae59f38fe6861a9815');
 assert.equal(l.Translations.filter(t=>t.CultureCode==='ar').length,1);
 assert.equal(l.Translations.filter(t=>t.CultureCode==='en').length,1);
 assert.match(l.AdaptationStatus,/READY_FOR_STAGE_D/);
});
test('Arabic 10 worked examples and original 5 visuals cover division goals',()=>{
 assert.equal(rich.Lessons.length,1);
 const l=rich.Lessons[0];
 assert.equal(l.LessonCode,lessonCode);
 assert.equal(l.WorkedExamples.length,10);
 assert.equal(l.Visuals.length,5);
 assert.ok(l.Visuals.some(x=>x.Kind==='ShapeDecomposition'));
 assert.ok(l.Visuals.some(x=>x.Kind==='ExpressionStripBars'));
 for(const x of [-3,-1,0,1,2,4]){
  assert.equal(x*x+3*x-40,(x-5)*(x+8));
  assert.equal(2*x**3-3*x*x+4*x+1,(x-2)*(2*x*x+x+6)+13);
  assert.equal(x*x*x-5*x+6,(x-2)*(x*x+2*x-1)+4);
 }
});
test('Source Advanced 3-2 spans pp204–212 and all 27 identified types mapped',()=>{
 assert.deepEqual(ledger.sourcePdfPages,[204,205,206,207,208,209,210,211,212]);
 assert.equal(ledger.sourceElementCount,27);
 assert.equal(ledger.sourceElements.length,27);
 assert.equal(new Set(ledger.sourceElements.map(x=>x.elementId)).size,27);
 assert.ok(ledger.sourceElements.every(x=>x.sourcePrintedPage===x.sourcePhysicalPage-17&&x.appLocation));
 assert.equal(ledger.testsRun,'WRITTEN_NOT_RUN');
 assert.equal(ledger.sourceElementDenominatorVerifiedForThisLesson,true);
});
test('Exact source families reuse linear solver and extend monomial once, Stage D fail closed',()=>{
 const m=mapping.mappings.find(x=>x.lessonCode===lessonCode);
 assert.ok(m);assert.deepEqual(m.primarySkills,contract.primarySkills);
 assert.deepEqual(m.allowedQuestionFamilies,contract.allowedQuestionFamilies);
 assert.equal(contract.testsRun,'WRITTEN_NOT_RUN');
 assert.equal(contract.runtimeGate.productionRouting,false);
 const ids=contract.allowedQuestionFamilies;
 assert.equal(new Set(ids).size,3);
 for(const id of ids){
  const a=registry.families.filter(x=>x.id===id);
  assert.equal(a.length,1,id);
  assert.equal(a[0].skillId,m.primarySkills[0]);
  assert.equal(a[0].productionRouting,false);
 }
 const exact=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'),'utf8');
 const linear=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.UaeG10PolynomialDivision.cs'),'utf8');
 const monomial=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.UaeG10Division.cs'),'utf8');
 assert.match(exact,/BuildG10PolynomialDivision/);
 assert.match(exact,/BuildG10Division/);
 assert.match(linear,/UaeG10PolynomialDivisionResponse\.Verify/);
 assert.match(monomial,/UaeG10DivisionResponse/);
});
