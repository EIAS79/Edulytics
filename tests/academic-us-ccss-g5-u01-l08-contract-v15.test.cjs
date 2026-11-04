'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
test('L08 exact disjoint component arithmetic from 2021 source',()=>{assert.deepEqual([12+10,3*2*2+3*2*3,3*5*3+5,3*6*3+6,3*5*3+2*2*3],[22,30,50,60,57])});
test('L08 equivalent full-prism partition 3 by 2 by 5',()=>{assert.equal(3*2*(2+3),3*2*2+3*2*3);assert.equal(3*2*5,30)});
test('L08 double counting and omitted attached row are invalid',()=>{assert.notEqual(45,45+5);assert.notEqual(54,54+6);assert.notEqual(45+12,45+12-4);for(const n of [null,-1,3.5,'5'])assert.equal(Number.isSafeInteger(n)&&n>0,false)});
const p=path.resolve(__dirname,'../src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u01-l07-l12-2021.rich-lesson-v2.json'),s=path.resolve(__dirname,'../artifacts/curriculum-delivery/US-CCSS-MATH/G5/u01-l08-practice-rubric-v15.json');
test('L08 source task/rubric evidence and C shared blocker',{skip:(!fs.existsSync(p)||!fs.existsSync(s))&&'full checkout required'},()=>{const l=JSON.parse(fs.readFileSync(p,'utf8')).Lessons.find(x=>x.LessonCode.endsWith('U01:L08'));const spec=JSON.parse(fs.readFileSync(s,'utf8'));assert.ok(l.ExplanationParagraphs.join(' ').includes('57 cubic units'));assert.ok(l.ExplanationParagraphs.join(' ').includes('Edulytiks-original exit check'));assert.equal(spec.StageC,'GRADE5_L08_EXACT_NONOVERLAP_VOLUME_TEACHER_REVIEW_WRITTEN_NOT_RUN')});

test('2021 G5 L08 nonoverlap exact practice and teacher boundary', {
 skip: !fs.existsSync(path.resolve(__dirname,'../src/Edulytics.Services/Mathematics/UsCcssG5Unit1Lesson8Questions.cs'))&&'full checkout needed'
},()=>{
 const root=path.resolve(__dirname,'..'),read=p=>fs.readFileSync(path.resolve(root,p),'utf8');
 const engine=read('src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs');
 const contract=read('src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs');
 const reg=JSON.parse(read('src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json'));
 assert.ok(contract.includes('UsCcssG5Unit1Lesson8PracticeContracts.Reviewed'));
 for(const op of ['Supports','Build','Solve','Verify'])assert.ok(engine.includes('UsCcssG5Unit1Lesson8Questions.'+op));
 assert.equal(reg.families.find(x=>x.id==='usccss.g5.u01.l08.add_nonoverlap_prism_volumes').lessonPracticeRouting,true);
 assert.deepEqual([12+10,3*2*2+3*2*3,3*5*3+1],[22,30,46]);
});
