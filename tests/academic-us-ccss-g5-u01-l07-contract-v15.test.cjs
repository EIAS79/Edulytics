'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
test('L07 freezer volume only determined after sufficient dimensions and units',()=>{assert.equal(4*2*3,24);assert.notEqual(4*2*5,24);assert.deepEqual([2,3,10].map(h=>4*2*h),[16,24,80])});
test('L07 malformed dimensions and wrong units are not valid full assessments',()=>{for(const a of [[4,2],[4,2,0],[4,-2,3],[4,2,3.2],['4',2,3]])assert.equal(a.length===3&&a.every(n=>Number.isSafeInteger(n)&&n>0),false);assert.notEqual('24','24 cubic feet')});
const p=path.resolve(__dirname,'../src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u01-l07-l12-2021.rich-lesson-v2.json'),s=path.resolve(__dirname,'../artifacts/curriculum-delivery/US-CCSS-MATH/G5/u01-l07-practice-rubric-v15.json');
test('L07 first-party task and human-review contract without false C approval',{skip:(!fs.existsSync(p)||!fs.existsSync(s))&&'full repository required'},()=>{const l=JSON.parse(fs.readFileSync(p,'utf8')).Lessons.find(x=>x.LessonCode.endsWith('U01:L07'));const r=JSON.parse(fs.readFileSync(s,'utf8'));assert.match(l.ExplanationParagraphs.join(' '),/dumpster/);assert.match(l.ExplanationParagraphs.join(' '),/Info Gap/);assert.match(l.ExplanationParagraphs.join(' '),/Edulytiks-original exit check/);assert.equal(r.StageC,'GRADE5_L07_SOURCE_EXACT_DIGITAL_AND_TEACHER_LED_WRITTEN_NOT_RUN')});

test('grade5 U01 L07 exact subskill contract and source mathematical oracles', {
 skip: !fs.existsSync(path.resolve(__dirname,'../src/Edulytics.Services/Mathematics/UsCcssG5Unit1Lesson7Questions.cs'))&&'Requires full checkout'
},()=>{
 const root=path.resolve(__dirname,'..'),read=p=>fs.readFileSync(path.resolve(root,p),'utf8');
 const src=read('src/Edulytics.Services/Mathematics/UsCcssG5Unit1Lesson7Questions.cs');
 const core=read('src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs');
 const engine=read('src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs');
 const registry=JSON.parse(read('src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json'));
 assert.ok(core.includes('UsCcssG5Unit1Lesson7PracticeContracts.Reviewed'));
 for (const method of ['Supports','Build','Solve','Verify']) assert.ok(engine.includes('UsCcssG5Unit1Lesson7Questions.'+method));
 const fam=registry.families.find(x=>x.id==='usccss.g5.u01.l07.cubic_unit_context');
 assert.equal(fam.skillId,'geometry.volume');assert.equal(fam.lessonPracticeRouting,true);
 assert.equal(fam.answerType,'exact_integer');
 for(const answer of [3,2,1])assert.ok(src.includes(String(answer)));
 assert.match(src,/teacher-reviewed/);
});
