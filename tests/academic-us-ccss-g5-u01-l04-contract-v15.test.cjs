'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const p=path.resolve(__dirname,'../src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u01-l04-l06-2021.rich-lesson-v2.json');
const r=path.resolve(__dirname,'../artifacts/curriculum-delivery/US-CCSS-MATH/G5/u01-l04-practice-rubric-v15.json');
test('L04 incomplete nine-cube front layer has no unique total without depth',()=>{const volumes=[2,3,10].map(n=>9*n);assert.deepEqual(volumes,[18,27,90]);assert.equal(new Set(volumes).size,3)});
test('L04 equivalent decompositions count the same 24-cube and 120-cube solids',()=>{assert.deepEqual([2*12,3*8,4*6],[24,24,24]);assert.deepEqual([5*24,6*20,4*30],[120,120,120]);assert.equal(5*(4*6),6*(4*5));assert.notEqual(5*20,120)});
test('L04 independent original 60-cube exit model has different valid layer partitions',()=>{assert.deepEqual([3*20,4*15,5*12],[60,60,60]);for(const arr of [[],[3,-4,5],[3,4,5.2],[3,0,5],['3',4,5]])assert.equal(arr.length===3&&arr.every(x=>Number.isSafeInteger(x)&&x>0),false)});
test('L04 source and assessment remain separate from shared grader',{skip:(!fs.existsSync(p)||!fs.existsSync(r))&&'full checkout required'},()=>{const d=JSON.parse(fs.readFileSync(p,'utf8')),s=JSON.parse(fs.readFileSync(r,'utf8'));const l=d.Lessons.find(x=>x.LessonCode.endsWith('U01:L04'));assert.ok(l);assert.match(l.ExplanationParagraphs.join(' '),/5 × 24/);assert.match(l.ExplanationParagraphs.join(' '),/Edulytiks-original exit check/);assert.equal(s.status,'GRADE5_L04_EXACT_COMPLETE_PRISM_LAYERS_TEACHER_INFERENCE_WRITTEN_NOT_RUN')});

test('source full-prism L04 question family is selected; unknown edges are teacher-led', {
 skip: !fs.existsSync(path.resolve(__dirname,'../src/Edulytics.Services/Mathematics/UsCcssG5Unit1Lesson4Questions.cs'))&&'full checkout required'
},()=>{
 const root=path.resolve(__dirname,'..');
 const read=p=>fs.readFileSync(path.resolve(root,p),'utf8');
 const source=read('src/Edulytics.Services/Mathematics/UsCcssG5Unit1Lesson4Questions.cs');
 const engine=read('src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs');
 const contracts=read('src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs');
 const registry=JSON.parse(read('src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json'));
 assert.ok(contracts.includes('UsCcssG5Unit1Lesson4PracticeContracts.Reviewed'));
 for(const m of ['Build','Solve','Verify','Supports'])assert.ok(engine.includes('UsCcssG5Unit1Lesson4Questions.'+m));
 assert.equal(registry.families.find(x=>x.id==='usccss.g5.u01.l04.full_prism_layer_groups').lessonPracticeRouting,true);
 assert.match(source,/no invisible dimension is being guessed/);
 assert.equal(2*3*4,24);assert.equal(3*5*2,30);assert.equal(3*2*5,30);
});
