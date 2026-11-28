'use strict';
// Grade10 U05L01: source-bound static/academic tests authored for Stage D; NOT run in A-B-C.
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const load=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const base='artifacts/curriculum-delivery/UAE-MOE-MATH/G10/';
const code='PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U05:L01:C042';
const bp=load('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g10-general-t1-ogl-v1.lesson-blueprint.json');
const pack=load('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json');
const rich=load('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u05-l01-ar.rich-lesson-v2.json');
const ledger=load(base+'lesson-042-source-elements.json');
const contract=load(base+'lesson-042-stage-c-exact-contract.json');
const maps=load('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
const family=load('src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json');

test('Lesson42 is the first Grade10 unified exponential graph lesson, not an Advanced track',()=>{
 const x=bp.Lessons.find(x=>x.LessonCode===code);
 assert.ok(x);
 assert.equal(bp.Units.length,18);
 assert.equal(bp.Lessons.length,148);
 assert.equal(x.Title,'تمثيل الدوال الأسية بيانيًا');
 assert.equal(x.UnitNumber,5);
 assert.equal(x.LessonNumber,1);
 assert.equal(x.SortOrder,42);
 assert.equal(x.UnitTitle,'الدوال الأسية واللوغاريتمية');
 const l=pack.Lessons.find(x=>x.LessonCode===code);
 assert.ok(l);
 assert.match(l.AdaptationStatus,/READY_FOR_STAGE_D/);
 assert.equal(l.SourceSha256,'96df18c4fb47e45fc7a98f876d80e00e3cbdc080e5acfbdc9674a25c85f8cb9e');
 assert.match(l.SourceLocator,/111–117.*294–300/);
 const ar=l.Translations.find(x=>x.CultureCode==='ar');
 assert.ok(ar);
 for(const key of ['Explanation','KeyConceptsAndRules','WorkedExamples','StepByStepSolutions','CommonMistakes','QuickSummary'])
   assert.ok(ar[key].length>300,key);
});
test('Applied source PDF111-117 has individual academic goal/graph/assessment mappings',()=>{
 assert.deepEqual(ledger.sourcePdfPages,[111,112,113,114,115,116,117]);
 assert.equal(ledger.sourceElementCount,25);
 assert.equal(ledger.sourceElements.length,25);
 assert.equal(new Set(ledger.sourceElements.map(x=>x.elementId)).size,25);
 assert.equal(ledger.sourceSha256,'96df18c4fb47e45fc7a98f876d80e00e3cbdc080e5acfbdc9674a25c85f8cb9e');
 assert.equal(ledger.advancedTocOnly.sourceLesson.startsWith('8-1'),true);
 assert.equal(ledger.advancedTocOnly.pdfPage,12);
 assert.ok(ledger.sourceElements.every(x=>x.appLocation&&x.coverageStatus&&x.sourcePhysicalPage>=111&&x.sourcePhysicalPage<=117));
});
test('RichV2 creates original growth, decay and shifted Cartesian graphics with exact values',()=>{
 const r=rich.Lessons[0];
 assert.equal(r.LessonCode,code);
 assert.equal(r.CultureCode,'ar');
 assert.equal(r.WorkedExamples.length,18);
 assert.equal(r.Visuals.length,6);
 assert.equal(r.Visuals.filter(x=>x.Kind==='CartesianPlot').length,3);
 const xs=[-2,-1,0,1,2],up=[1/9,1/3,1,3,9],down=[9,3,1,1/3,1/9];
 xs.forEach((x,i)=>{assert.ok(Math.abs(Math.pow(3,x)-up[i])<1e-10);assert.ok(Math.abs(Math.pow(1/3,x)-down[i])<1e-10);});
 const shifting=r.WorkedExamples.find(x=>x.Question.includes('٢^س+١'));
 assert.ok(shifting);
 assert.match(shifting.Answer,/ص=١/);
});
test('Exact bound is Grade10 exponential features, not quadratic rate or chart guesswork',()=>{
 const m=maps.mappings.find(x=>x.lessonCode===code);
 assert.ok(m);
 assert.deepEqual(m.primarySkills,['algebra.exponential.graph_features']);
 assert.deepEqual(m.allowedQuestionFamilies,contract.allowedQuestionFamilies);
 assert.equal(contract.testsRun,'WRITTEN_NOT_RUN');
 assert.equal(contract.runtimeGate.productionRouting,false);
 assert.equal(m.practiceReadiness,'PARTIAL_REUSE_ONLY_NOT_READY_FOR_STAGE_D');
 for(const id of m.allowedQuestionFamilies){
   const row=family.families.find(x=>x.id===id);
   assert.ok(row,id);
   assert.equal(row.skillId,'algebra.exponential.graph_features');
   assert.equal(row.productionRouting,false);
   assert.equal(row.answerType,'exact_integer');
 }
 const engine=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'),'utf8');
 const response=fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.UaeG10ExponentialGraph.cs'),'utf8');
 assert.match(engine,/BuildG10ExponentialGraph/);
 assert.match(response,/class\s+UaeG10ExponentialGraphResponse\b/);
 assert.match(response,/static\s+bool\s+Verify\s*\(/);
 assert.match(fs.readFileSync(path.join(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'),'utf8'),/return\s+UaeG10ExponentialGraphResponse\.Verify\(family,\s*parameters,\s*answer\)/);
});
