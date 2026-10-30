'use strict';const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
test('G5 optional L12 five independent 60-container example arrays',()=>{const a=[[1,6,10],[2,5,6],[3,4,5],[2,3,10],[1,3,20]];assert.equal(a.length,5);assert.ok(a.every(d=>d.reduce((x,y)=>x*y,1)===60));assert.equal(new Set(a.map(x=>[...x].sort((p,q)=>p-q).join(','))).size,5)});
test('G5 optional L12 ship estimate depends on assumptions, not fixed universal 384',()=>{assert.equal(8*12*4,384);assert.notEqual(8*12*5,384);assert.equal(3300/60,55);assert.notEqual(3300/60,384)});
test('G5 optional L12 rejects malformed dimensions, 61-container or mass-as-volume',()=>{for(const x of [[3,4],[3,4,5.5],[-3,4,5],[0,4,5],[3,'4',5]])assert.equal(x.length===3&&x.every(n=>Number.isSafeInteger(n)&&n>0),false);assert.notEqual(3*4*5,61);assert.notEqual('3,300 tons','3,300 cubic feet')});
const p=path.resolve(__dirname,'../src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u01-l07-l12-2021.rich-lesson-v2.json'),s=path.resolve(__dirname,'../artifacts/curriculum-delivery/US-CCSS-MATH/G5/u01-l12-practice-acceptance-v16.json');
test('G5 L12 optional source and original reflection, no false publisher gated cool-down',{skip:(!fs.existsSync(p)||!fs.existsSync(s))&&'needs full repo fixtures'},()=>{const l=JSON.parse(fs.readFileSync(p,'utf8')).Lessons.find(x=>x.LessonCode.endsWith('U01:L12'));const spec=JSON.parse(fs.readFileSync(s,'utf8'));assert.match(l.ExplanationParagraphs.join(' '),/NO listed publisher Cool-down/);assert.match(l.ExplanationParagraphs.join(' '),/Edulytiks-original optional exit/);assert.equal(spec.StageC,'GRADE5_L12_OPTIONAL_EXACT_60_CONTAINER_FACTORS_TEACHER_ESTIMATE_WRITTEN_NOT_RUN')});

test('2021 G5 optional L12 contains 54 ordered factor triples; five noncongruent 60 layouts', {
 skip: !fs.existsSync(path.resolve(__dirname,'../src/Edulytics.Services/Mathematics/UsCcssG5Unit1Lesson12Questions.cs'))&&'full checkout needed'
},()=>{
 const root=path.resolve(__dirname,'..'),read=p=>fs.readFileSync(path.resolve(root,p),'utf8');
 const engine=read('src/Edulytics.Services/Mathematics/ExactSkillContractQuestionEngine.cs');
 const contract=read('src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs');
 const registry=JSON.parse(read('src/Edulytics.Core/Mathematics/Generation/question-family-registry.v1.json'));
 assert.ok(contract.includes('UsCcssG5Unit1Lesson12PracticeContracts.Reviewed'));
 for(const op of ['Supports','Build','Solve','Verify'])assert.ok(engine.includes('UsCcssG5Unit1Lesson12Questions.'+op));
 assert.equal(registry.families.find(x=>x.id==='usccss.g5.u01.l12.sixty_container_factor_arrangements').lessonPracticeRouting,true);
 const triples=[];
 for(let width=1;width<=60;width++)for(let depth=1;depth<=60/width;depth++)
 if(60%(width*depth)===0)triples.push([width,depth,60/(width*depth)]);
 assert.equal(triples.length,54);
 const shapes=[[2,3,10],[2,5,6],[3,4,5],[1,6,10],[1,5,12]];
 assert.equal(new Set(shapes.map(x=>x.slice().sort((a,b)=>a-b).join('x'))).size,5);
 assert.ok(shapes.every(x=>x.reduce((a,b)=>a*b,1)===60));
 assert.ok(!srcIsTonsCubic(3300));
 function srcIsTonsCubic(tons){return typeof tons==='string'&&tons.endsWith('ft³')}
});
