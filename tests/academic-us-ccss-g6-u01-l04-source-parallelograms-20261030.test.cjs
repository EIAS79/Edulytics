'use strict';
const {test}=require('node:test');const assert=require('node:assert/strict');const fs=require('node:fs');const path=require('node:path');const root=path.resolve(__dirname,'..');const load=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const pack=load('src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g6-phase29-v1.lesson-content-pack.json');const ledger=load('artifacts/curriculum-delivery/US-CCSS-MATH/G6/grade6-147-academic-authoring-ledger.json');const e=load('artifacts/curriculum-delivery/US-CCSS-MATH/G6/g6-u01-l04-abc-first-party-parallelograms-20261030.json');const l=pack.lessons.find(x=>x.lessonCode==='PED:US-CCSS-MATH:G6:U01:L04');const tr=l.translations.find(x=>x.cultureCode==='en');
const area=pts=>Math.abs(pts.reduce((a,p,i)=>{let n=pts[(i+1)%pts.length];return a+p[0]*n[1]-n[0]*p[1];},0))/2;
test('two original first-party slants preserve equal area at fixed base and height',()=>{
 const p=[[0,0],[4,0],[5,3],[1,3]],q=[[0,0],[4,0],[7,3],[3,3]],higher=[[0,0],[4,0],[5,4],[1,4]];
 assert.equal(area(p),12);assert.equal(area(q),12);assert.equal(area(higher),16);
 assert.equal(8*4-2*(2*4/2),24);assert.equal(11*4-2*(5*4/2),24);
 assert.equal(6*4,24);assert.notEqual(6*5,24);
});
test('classification correct, incorrect, incomplete student structures',()=>{
 const classify=s=>s&&typeof s==='object'&&s.sideCount===4&&s.parallelOppositePairs===2;
 assert.equal(classify({sideCount:4,parallelOppositePairs:2}),true);
 assert.equal(classify({sideCount:4,parallelOppositePairs:1}),false);
 assert.equal(classify({sideCount:5,parallelOppositePairs:2}),false);
 assert.equal(classify({sideCount:4,parallelOppositePairs:0}),false);
 assert.equal(classify(null),false);
 assert.match(tr.workedExamples,/Edulytiks-original Cool-down/);
 assert.match(e.C_STATUS,/BLOCKED/);
 assert.equal(e.READY_FOR_STAGE_D,false);
 assert.equal(e.testExecution,'AUTHORED_NOT_RUN');
 assert.equal(ledger.lessons.find(x=>x.lessonCode==='PED:US-CCSS-MATH:G6:U01:L04').academicAcceptance,'NOT_ACCEPTED');
});
