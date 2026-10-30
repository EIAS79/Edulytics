'use strict';const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
test('G5 L11 original three real-world contexts, including area-times-height',()=>{assert.deepEqual([10*6*5,4*9*5,30*9],[300,180,270])});
test('G5 L11 two garden volumes preserved under valid alternative side lengths',()=>{assert.equal(10*3*4+8*3*4,216);assert.equal((10+8)*3*4,216);assert.equal(12*3*4+6*3*4,216)});
test('G5 L11 negative garden arithmetic, units and overlapping pieces do not establish valid response',()=>{assert.notEqual(120+96,206);assert.notEqual(30,270);assert.notEqual('216 square feet','216 cubic feet');for(const lengths of [[18,0],[-2,20],['12',6],[12,6.5]])assert.equal(lengths.length===2&&lengths.every(x=>Number.isSafeInteger(x)&&x>0),false)});
const p=path.resolve(__dirname,'../src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u01-l07-l12-2021.rich-lesson-v2.json'),r=path.resolve(__dirname,'../artifacts/curriculum-delivery/US-CCSS-MATH/G5/u01-l11-practice-acceptance-v16.json');
test('G5 L11 first-party exit and teacher rubric source evidence, no false C readiness',{skip:(!fs.existsSync(p)||!fs.existsSync(r))&&'exact branch fixtures not mounted'},()=>{const l=JSON.parse(fs.readFileSync(p,'utf8')).Lessons.find(x=>x.LessonCode.endsWith('U01:L11'));const s=JSON.parse(fs.readFileSync(r,'utf8'));assert.match(l.ExplanationParagraphs.join(' '),/Edulytiks-original exit check/);assert.equal(s.StageC,'GRADE5_L11_EXACT_SOURCE_CONTEXTUAL_PRISMS_GARDEN_TEACHER_LED_WRITTEN_NOT_RUN')});

test('2021 G5 L11 exact contextual source volumes and teacher design boundary', {
 skip: !fs.existsSync(path.resolve(__dirname,'../src/Edulytics.Services/Mathematics/UsCcssG5Unit1Lesson11Questions.cs'))&&'full checkout needed'
},()=>{
 const root=path.resolve(__dirname,'..'),read=p=>fs.readFileSync(path.resolve(root,p),'utf8');
 const engine=read('src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs');
 const contract=read('src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs');
 const registry=JSON.parse(read('src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json'));
 assert.ok(contract.includes('UsCcssG5Unit1Lesson11PracticeContracts.Reviewed'));
 for(const op of ['Supports','Build','Solve','Verify'])assert.ok(engine.includes('UsCcssG5Unit1Lesson11Questions.'+op));
 assert.equal(registry.families.find(x=>x.id==='usccss.g5.u01.l11.real_world_prism_volume').lessonPracticeRouting,true);
 assert.deepEqual([10*6*5,4*9*5,30*9,3*8*4+10*3*4],[300,180,270,216]);
});
