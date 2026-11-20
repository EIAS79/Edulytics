'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const code='PED:UAE-MOE-MATH:L9:GENERAL:01:01:VARIABLES-AND-EXPRESSIONS';

test('Grade 9 lesson 1 source-objectives spec and authoring checkpoint agree on readiness while D is unverified',()=>{
 const source=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/u01l01-source-objectives-practice-contract.json');
 const cp=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G9/u01l01-stage-a-c-checkpoint.json');
 const mappings=read('src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json');
 const mapping=mappings.mappings.find(x=>x.lessonCode===code);
 const contract=fs.readFileSync(path.join(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 assert.equal(source.lessonCode,code);
 assert.equal(cp.lesson.code,code);
 assert.equal(source.aBC.readyForStageD,true);
 assert.equal(cp.delivery.readyForStageD,true);
 assert.equal(cp.lesson.actualRuntimeFamilyBinding,true);
 assert.deepEqual(mapping.allowedQuestionFamilies,source.runtimeContractEvidence.sourceMappedFamilies);
 assert.ok(contract.includes('byLesson["'+code+'"]'));
 assert.equal(source.runtimeContractEvidence.realQuestionGenerationAndGradingVerified,false);
 assert.equal(source.semanticGate.testExecutionStatus,'NOT_RUN');
 assert.equal(cp.delivery.ci,'NOT_RUN');
 assert.equal(source.semanticGate.forbidReadyVerifiedUntilPassed,true);
});
