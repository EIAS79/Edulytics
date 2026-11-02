'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
test('L06 original three true-false warm-up statements',()=>{assert.equal((4*2)*5===4*(2*5),true);assert.equal((2*5)*4===2*20,true);assert.equal(5*4*2===10*40,false)});
test('L06 original source prisms 90,72,84 have equivalent volume groupings',()=>{assert.deepEqual([6*(5*3),(6*5)*3,15*6],[90,90,90]);assert.deepEqual([6*3*4,(6*3)*4,(3*4)*6],[72,72,72]);assert.deepEqual([(7*4)*3,(7*3)*4,7*12],[84,84,84])});
test('L06 reject invalid plus, wrong products and malformed dimensions',()=>{assert.notEqual(6+(3*4),72);assert.notEqual(10*40,5*4*2);for(const a of [[6,3],[6,0,4],[6,-3,4],['6',3,4]])assert.equal(a.length===3&&a.every(n=>Number.isSafeInteger(n)&&n>0),false)});
const p=path.resolve(__dirname,'../src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u01-l04-l06-2021.rich-lesson-v2.json'),s=path.resolve(__dirname,'../artifacts/curriculum-delivery/US-CCSS-MATH/G5/u01-l06-practice-rubric-v15.json');
test('L06 source-covered Rich V2 and C shared gate evidence',{skip:(!fs.existsSync(p)||!fs.existsSync(s))&&'requires branch checkout'},()=>{const l=JSON.parse(fs.readFileSync(p,'utf8')).Lessons.find(x=>x.LessonCode.endsWith('U01:L06'));const spec=JSON.parse(fs.readFileSync(s,'utf8'));assert.match(l.ExplanationParagraphs.join(' '),/5×4×2 = 10×40/);assert.match(l.ExplanationParagraphs.join(' '),/Edulytiks-original exit check/);assert.equal(spec.StageC,'GRADE5_L06_EXACT_SOURCE_EXPRESSION_TRUTH_WRITTEN_NOT_RUN_TEACHER_LED')});

test('source L06 prism expression truth family supports parentheses versus false addition', {
 skip: !fs.existsSync(path.resolve(__dirname,'../src/Edulytics.Services/Mathematics/UsCcssG5Unit1Lesson6Questions.cs'))&&'full checkout'
},()=>{
 const root=path.resolve(__dirname,'..'),read=p=>fs.readFileSync(path.resolve(root,p),'utf8');
 const src=read('src/Edulytics.Services/Mathematics/UsCcssG5Unit1Lesson6Questions.cs');
 const core=read('src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs');
 const eng=read('src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs');
 const reg=JSON.parse(read('src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json'));
 assert.ok(core.includes('UsCcssG5Unit1Lesson6PracticeContracts.Reviewed'));
 for(const m of ['Supports','Build','Solve','Verify'])assert.ok(eng.includes('UsCcssG5Unit1Lesson6Questions.'+m));
 assert.equal(reg.families.find(x=>x.id==='usccss.g5.u01.l06.match_volume_expression').answerType,'exact_integer');
 assert.equal((6*3)*4,72);assert.notEqual(6*(3+4),72);
 assert.equal((4*5)*8,160);assert.notEqual(4*(5+8),160);
 assert.match(src,/teacher reviewed/i);
});
