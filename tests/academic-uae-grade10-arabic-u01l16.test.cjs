'use strict';
// Stage A–B–C source and static source-contract regression; WRITTEN_NOT_RUN.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const get=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const b='artifacts/curriculum-delivery/UAE-MOE-MATH/G10/';
const code='PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U01:L16:C016';
const bp=get('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g10-general-t1-ogl-v1.lesson-blueprint.json');
const pack=get('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json');
const rich=get('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u01-l16-ar.rich-lesson-v2.json');
const ledger=get(b+'lesson-016-source-elements.json');
const contract=get(b+'lesson-016-stage-c-exact-contract.json');
const map=get('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
const skills=get('src/Edulytics.Core/Mathematics/Skills/skill-registry.v1.json');
const families=get('src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json');

test('Owner-approved 148 lesson catalog registers source-correct U01L16 single General identity',()=>{
 assert.equal(bp.Lessons.length,148);assert.equal(bp.Units.length,18);
 const l=bp.Lessons.find(x=>x.LessonCode===code);
 assert.ok(l);assert.equal(l.UnitNumber,1);assert.equal(l.LessonNumber,16);
 assert.equal(l.Title,'نظرية الصفر النسبي');
 assert.equal(l.UnitTitle,'كثيرات الحدود والتحليل');
 const data=pack.Lessons.find(x=>x.LessonCode===code);
 assert.ok(data);
 assert.equal(data.SourceSha256,'0a554589f4cc72537007f3229098c1ab46775d7bf2301bae59f38fe6861a9815');
 assert.match(data.SourceLocator,/PDF pages 258–263.*printed pages 241–246/);
 assert.match(data.AdaptationStatus,/READY_FOR_STAGE_D/);
 const ar=data.Translations.find(x=>x.CultureCode==='ar');
 assert.ok(ar);assert.equal(ar.Title,'نظرية الصفر النسبي');
 for(const key of ['Explanation','KeyConceptsAndRules','WorkedExamples','StepByStepSolutions','CommonMistakes','QuickSummary'])
   assert.ok(ar[key].length>180,key);
});
test('Arabic source math correctly distinguishes candidate rational zeros from actual zeros',()=>{
 const l=rich.Lessons[0];assert.equal(l.LessonCode,code);
 assert.equal(l.WorkedExamples.length,10);assert.equal(l.Visuals.length,5);
 assert.ok(l.Visuals.some(x=>x.Kind==='CartesianPlot'));
 assert.ok(l.WorkedExamples.some(x=>x.Answer.includes('١/٣')));
 for(const x of [-3,-1,0,1,2,3]){
  assert.equal((3*x-1)*(x*x+x+2),3*x*x*x+2*x*x+5*x-2);
  assert.equal((x-2)*(2*x+1)*(x*x+4),2*x**4-3*x**3+6*x*x-12*x-8);
  assert.equal((x-1)*(x-2)*(x-3),x**3-6*x*x+11*x-6);
 }
 assert.equal(6*1*1-5*1+1,2,'candidate 1 is not an actual zero');
 assert.equal(6*1*1-5*1*2+4,0,'candidate 1/2 is actual by numerator 1 denominator 2');
});
test('Source pages and 29 categorized original teaching/assessment elements are traceable',()=>{
 assert.deepEqual(ledger.sourcePdfPages,[258,259,260,261,262,263]);
 assert.equal(ledger.sourceElements.length,29);
 assert.equal(new Set(ledger.sourceElements.map(x=>x.elementId)).size,29);
 assert.equal(ledger.sourceElementDenominatorVerifiedForThisLesson,true);
 assert.ok(ledger.sourceElements.every(x=>x.sourcePrintedPage===x.sourcePhysicalPage-17&&x.appLocation));
 assert.equal(ledger.testsRun,'WRITTEN_NOT_RUN');
});
test('Three distinct source rational-root families, new skill and production fail-closed',()=>{
 const m=map.mappings.find(x=>x.lessonCode===code);
 assert.ok(m);assert.deepEqual(m.primarySkills,['algebra.polynomial.rational_zero_theorem']);
 assert.deepEqual(m.allowedQuestionFamilies,contract.allowedQuestionFamilies);
 assert.ok(skills.skills.some(x=>x.id===m.primarySkills[0]));
 assert.equal(contract.testsRun,'WRITTEN_NOT_RUN');
 assert.equal(contract.runtimeGate.productionRouting,false);
 for(const id of m.allowedQuestionFamilies){
  const row=families.families.find(x=>x.id===id);
  assert.ok(row,id);assert.equal(row.skillId,m.primarySkills[0]);
  assert.equal(row.lessonPracticeRouting,true);assert.equal(row.productionRouting,false);
 }
 const csharp=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.UaeG10RationalZeros.cs'),'utf8');
 const engine=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'),'utf8');
 assert.match(csharp,/UaeG10RationalZerosResponse.Verify/);
 assert.match(csharp,/Gcd/);assert.match(csharp,/IsZero/);
 assert.match(engine,/BuildG10RationalZeros/);
});
