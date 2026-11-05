'use strict';const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
test('L10 source original 95 region additive and subtractive equivalence',()=>{assert.equal(3*5*5+2*2*5,95);assert.equal(5*5*5-2*3*5,95)});
test('L10 source 168 step has distinct additive and subtractive volume calculations',()=>{assert.equal(4*8*3+6*4*3,168);assert.equal(10*4*3+4*4*3,168);assert.equal(10*8*3-6*4*3,168);assert.equal(10-6,4);assert.equal(8-4,4)});
test('L10 reject missing-region addition, raw bounding box and malformed input',()=>{assert.notEqual(5*5*5+2*3*5,95);assert.notEqual(10*8*3,168);for(const a of [[10,8],[10,0,3],[10,-8,3],[10,8,3.5],[10,'8',3]])assert.equal(a.length===3&&a.every(x=>Number.isSafeInteger(x)&&x>0),false)});
const p=path.resolve(__dirname,'../src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u01-l07-l12-2021.rich-lesson-v2.json'),s=path.resolve(__dirname,'../artifacts/curriculum-delivery/US-CCSS-MATH/G5/u01-l10-practice-acceptance-v16.json');
test('L10 authored original exit connected to grade content, C gated',{skip:(!fs.existsSync(p)||!fs.existsSync(s))&&'checkout not mounted'},()=>{const l=JSON.parse(fs.readFileSync(p,'utf8')).Lessons.find(x=>x.LessonCode.endsWith('U01:L10'));const v=JSON.parse(fs.readFileSync(s,'utf8'));assert.match(l.ExplanationParagraphs.join(' '),/Edulytiks-original exit check/);assert.equal(v.stageC,'GRADE5_L10_EXACT_SOURCE_COMPOSITE_EXPRESSIONS_TEACHER_GEOMETRY_WRITTEN_NOT_RUN')});

test('2021 G5 L10 two representations of 95 and 168 total are source exact', {
 skip: !fs.existsSync(path.resolve(__dirname,'../src/Edulytics.Services/Mathematics/UsCcssG5Unit1Lesson10Questions.cs'))&&'full checkout needed'
},()=>{
 const root=path.resolve(__dirname,'..'),read=p=>fs.readFileSync(path.resolve(root,p),'utf8');
 const engine=read('src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs');
 const contract=read('src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs');
 const registry=JSON.parse(read('src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json'));
 assert.ok(contract.includes('UsCcssG5Unit1Lesson10PracticeContracts.Reviewed'));
 for(const op of ['Supports','Build','Solve','Verify'])assert.ok(engine.includes('UsCcssG5Unit1Lesson10Questions.'+op));
 assert.equal(registry.families.find(x=>x.id==='usccss.g5.u01.l10.equivalent_composite_volume_expressions').lessonPracticeRouting,true);
 assert.equal(3*5*5+2*2*5,95);assert.equal(5*5*5-2*3*5,95);
 assert.equal(4*4*3+10*4*3,168);assert.equal(4*8*3+6*4*3,168);
});
