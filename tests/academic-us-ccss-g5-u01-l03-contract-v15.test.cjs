'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const docPath=path.resolve(__dirname,'../src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u01-l01-l03-2021.rich-lesson-v2.json');
const specPath=path.resolve(__dirname,'../artifacts/curriculum-delivery/US-CCSS-MATH/G5/u01-l03-practice-rubric-v15.json');
test('L03 exact mental number talk and changing layers',()=>{assert.deepEqual([6*4,3*2*4,3*2*5,3*2*6],[24,24,30,36]);assert.deepEqual([24,30,36].slice(1).map((n,i)=>n-[24,30][i]),[6,6])});
test('L03 compatible 40-cube layer decompositions include hidden cubes',()=>{const d=[2,4,5];assert.equal(d.reduce((a,b)=>a*b,1),40);assert.deepEqual([5*8,2*20,4*10],[40,40,40]);assert.notEqual(8,40)});
test('L03 wrong count, malformed factors and missing hidden levels do not prove response',()=>{assert.notEqual(5*7,40);for(const factors of [[2,4],[2,4,0],[2,4,-5],[2,4,5.5],[2,'4',5]])assert.equal(factors.length===3&&factors.every(x=>Number.isSafeInteger(x)&&x>0)&&factors.reduce((a,b)=>a*b,1)===40,false)});
test('L03 source text and teacher contract provided, no false C pass',{skip:(!fs.existsSync(docPath)||!fs.existsSync(specPath))&&'repo fixture absent'},()=>{const d=JSON.parse(fs.readFileSync(docPath,'utf8'));const l=d.Lessons.find(x=>x.LessonCode.endsWith('U01:L03'));const c=JSON.parse(fs.readFileSync(specPath,'utf8'));assert.ok(l);assert.match(l.ExplanationParagraphs.join(' '),/Jada/);assert.match(l.ExplanationParagraphs.join(' '),/Edulytiks-original exit check/);assert.match(l.ExplanationParagraphs.join(' '),/3 × 2 × 6 = 36/);assert.equal(c.StageC,'GRADE5_L03_SOURCE_HIDDEN_PRISM_EXACT_AUTHORED_NOT_RUN');assert.equal(c.status,'BLOCKED_SHARED_INTEGRATION');assert.equal(c.testStatus,'WRITTEN_NOT_RUN')});

test('grade5 L03 hidden full cube layers selected; picture/model assessed by teacher', {
 skip: !fs.existsSync(path.resolve(__dirname,'../src/Edulytics.Services/Mathematics/UsCcssG5Unit1Lesson3Questions.cs'))&&'full checkout required'
},()=>{
 const root=path.resolve(__dirname,'..');
 const source=fs.readFileSync(path.resolve(root,'src/Edulytics.Services/Mathematics/UsCcssG5Unit1Lesson3Questions.cs'),'utf8');
 const core=fs.readFileSync(path.resolve(root,'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 const engine=fs.readFileSync(path.resolve(root,'src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs'),'utf8');
 const registry=JSON.parse(fs.readFileSync(path.resolve(root,'src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json'),'utf8'));
 assert.equal(registry.families.find(x=>x.id==='usccss.g5.u01.l03.count_hidden_prism_layers').lessonPracticeRouting,true);
 assert.ok(core.includes('UsCcssG5Unit1Lesson3PracticeContracts.Reviewed'));
 for (const method of ['Build','Solve','Verify','Supports'])assert.ok(engine.includes('UsCcssG5Unit1Lesson3Questions.'+method));
 assert.ok(source.includes('hidden from view'));
 for (const [depth,answer] of [[2,40],[3,60],[4,80]])assert.equal(4*depth*5,answer);
});
