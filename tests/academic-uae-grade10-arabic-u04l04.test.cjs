'use strict';
// Authoring-only Stage A-B-C regression. Execution reserved for Stage D.
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const load=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const base='artifacts/curriculum-delivery/UAE-MOE-MATH/G10/';
const lessonCode='PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U04:L04:C037';
const blueprint=load('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g10-general-t1-ogl-v1.lesson-blueprint.json');
const content=load('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json');
const rich=load('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u04-l04-ar.rich-lesson-v2.json');
const source=load(base+'lesson-037-source-elements.json');
const contract=load(base+'lesson-037-stage-c-exact-contract.json');
const mappings=load('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
const registry=load('src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json');

test('Owner-approved catalog G10-U04L04 preserves unique 148-lesson General identity',()=>{
 const bp=blueprint.Lessons.find(x=>x.LessonCode===lessonCode);
 assert.ok(bp);
 assert.equal(blueprint.Lessons.length,148);
 assert.equal(bp.SortOrder,37);
 assert.equal(bp.UnitNumber,4);assert.equal(bp.LessonNumber,4);
 assert.equal(bp.Title,'حل المعادلات التربيعية بإكمال المربع');
 assert.equal(bp.UnitTitle,'الدوال والمعادلات التربيعية');
 const l=content.Lessons.find(x=>x.LessonCode===lessonCode);
 assert.ok(l,'new student content for canonical37 must be authored');
 assert.match(l.SourceLocator,/149–154/);
 assert.equal(l.SourceSha256,'0a554589f4cc72537007f3229098c1ab46775d7bf2301bae59f38fe6861a9815');
 assert.match(l.AdaptationStatus,/READY_FOR_STAGE_D/);
 const ar=l.Translations.find(x=>x.CultureCode==='ar');
 assert.ok(ar);assert.ok(ar.Explanation.includes('س²+ب س') && ar.Explanation.includes('(ب/٢)²') && ar.Explanation.includes('(س+ب/٢)²'));
 for(const k of ['Explanation','KeyConceptsAndRules','WorkedExamples','StepByStepSolutions','CommonMistakes','QuickSummary'])assert.ok(ar[k].length>150,k);
});
test('Completing square student RichV2 has original examples, CartesianPlot and visual geometry',()=>{
 assert.equal(rich.Lessons.length,1);
 const x=rich.Lessons[0];
 assert.equal(x.LessonCode,lessonCode);
 assert.equal(x.WorkedExamples.length,12);
 assert.equal(x.Visuals.length,5);
 assert.ok(x.Visuals.some(v=>v.Kind==='CartesianPlot'));
 assert.ok(x.Visuals.some(v=>v.Kind==='ExpressionStripBars'));
 assert.ok(x.WorkedExamples.some(v=>v.Answer.includes('٥/٢')));
 for(const t of [1,-11])assert.equal(t*t+10*t-11,0);
 for(const t of [-3,-1,0,1,4])assert.equal((t-3)**2-7,t*t-6*t+2);
 const y=t=>-16*t*t+25*t;
 assert.equal(y(25/32),625/64);
});
test('All original Advanced source lesson/exercise categories and technology extension get a mapped owner source',()=>{
 assert.deepEqual(source.sourcePdfPages,[149,150,151,152,153,154]);
 assert.equal(source.sourceElements.length,25);
 assert.deepEqual(source.sourceTechnologyLabPages,[156]);
 assert.equal(source.sourceSha256,'0a554589f4cc72537007f3229098c1ab46775d7bf2301bae59f38fe6861a9815');
 assert.equal(new Set(source.sourceElements.map(x=>x.elementId)).size,25);
 assert.ok(source.sourceElements.every(x=>x.appLocation&&x.coverageStatus));
 assert.equal(source.testStatus,'WRITTEN_NOT_RUN');
});
test('Complete-square exact families have unique primary skill, source-correct answer contracts and fail-closed production',()=>{
 const m=mappings.mappings.find(x=>x.lessonCode===lessonCode);
 assert.ok(m);
 assert.deepEqual(m.primarySkills,['algebra.quadratic.complete_square']);
 assert.deepEqual(m.allowedQuestionFamilies,contract.allowedQuestionFamilies);
 assert.equal(contract.testsRun,'WRITTEN_NOT_RUN');
 assert.equal(contract.productionRouting,false);
 for(const id of contract.allowedQuestionFamilies){
  const f=registry.families.find(x=>x.id===id);
  assert.ok(f,id);assert.equal(f.productionRouting,false);
  assert.equal(f.skillId,m.primarySkills[0]);
 }
 const engine=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'),'utf8');
 assert.match(engine,/BuildG10CompletingSquare/);
 const helper=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.UaeG10CompletingSquare.cs'),'utf8');
 assert.match(helper,/UaeG10CompletingSquareResponse/);
 assert.match(helper,/Verify/);
});
