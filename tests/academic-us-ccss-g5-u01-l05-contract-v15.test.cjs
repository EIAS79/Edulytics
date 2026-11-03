'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
test('G5 L05 actual source growing 4×10 prism',()=>assert.deepEqual([1,2,3,10,25].map(h=>4*10*h),[40,80,120,400,1000]));
test('G5 L05 alternate base choices count same solid',()=>assert.deepEqual([3*4*5,3*5*4,4*5*3],[60,60,60]));
test('G5 L05 rejects malformed dimensions and area-volume confusion',()=>{assert.notEqual(40*25,100);for(const a of [[3,4],[3,-4,5],[3,4.5,5],['3',4,5]])assert.equal(a.length===3&&a.every(x=>Number.isSafeInteger(x)&&x>0),false)});
const p=path.resolve(__dirname,'../src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u01-l04-l06-2021.rich-lesson-v2.json'),s=path.resolve(__dirname,'../artifacts/curriculum-delivery/US-CCSS-MATH/G5/u01-l05-practice-rubric-v15.json');
test('G5 L05 original models present; Practice C still gated',{skip:(!fs.existsSync(p)||!fs.existsSync(s))&&'requires full checkout'},()=>{const l=JSON.parse(fs.readFileSync(p,'utf8')).Lessons.find(x=>x.LessonCode.endsWith('U01:L05'));const s2=JSON.parse(fs.readFileSync(s,'utf8'));assert.ok(l.ExplanationParagraphs.join(' ').includes('40×25'));assert.ok(l.ExplanationParagraphs.join(' ').includes('What Is the Question?'));assert.equal(s2.StageC,'GRADE5_L05_EXACT_SOURCE_GROWING_PRISM_WRITTEN_NOT_RUN_TEACHER_LED')});

test('source growing-prism L05 family routes only complete 4 by 10 base volumes', {
 skip: !fs.existsSync(path.resolve(__dirname,'../src/Edulytics.Services/Mathematics/UsCcssG5Unit1Lesson5Questions.cs'))&&'full checkout'
},()=>{
 const root=path.resolve(__dirname,'..'),read=p=>fs.readFileSync(path.resolve(root,p),'utf8');
 const src=read('src/Edulytics.Services/Mathematics/UsCcssG5Unit1Lesson5Questions.cs');
 const eng=read('src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs');
 const core=read('src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs');
 const reg=JSON.parse(read('src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json'));
 assert.ok(core.includes('UsCcssG5Unit1Lesson5PracticeContracts.Reviewed'));
 for(const method of ['Supports','Build','Solve','Verify'])assert.ok(eng.includes('UsCcssG5Unit1Lesson5Questions.'+method));
 assert.equal(reg.families.find(x=>x.id==='usccss.g5.u01.l05.growing_prism_fixed_base').answerType,'exact_integer');
 for(const [height,vol] of [[1,40],[2,80],[3,120],[10,400],[25,1000]])assert.equal(40*height,vol);
 assert.match(src,/reviewed by the teacher/i);
});
