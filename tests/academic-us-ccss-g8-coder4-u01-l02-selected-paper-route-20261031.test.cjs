// Grade 8 U01:L02 selected source-aligned offline paper assessment — AUTHORED_NOT_RUN.
'use strict';
const {test}=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const contract=read('docs/curriculum/us-ccss-g8-coder4-u01-l02-academic-assessment-contract-20261030.json');
const route=contract.selectedOfflinePaperRoute20261031;
const ledger=read('docs/curriculum/us-ccss-g8-coder4-p0-current-execution-ledger-20261030.json');
const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u01-l01-l08-2017.rich-lesson-v2.json');
const paper=fs.readFileSync(path.join(root,route.learnerWorksheetPath),'utf8');
const hash=crypto.createHash('sha256').update(Buffer.from(paper,'utf8')).digest('hex');
const pts=[
 [[0,0],[1,0],[0,2]], [[2,0],[3,0],[2,2]], [[2,0],[2,-1],[4,0]],
 [[2,0],[2,1],[4,0]], [[2,2],[2,3],[4,2]], [[2,2],[1,2],[2,4]]
];
const translate=(a,dx,dy)=>a.map(([x,y])=>[x+dx,y+dy]);
const rotateCW=(a,[cx,cy])=>a.map(([x,y])=>[cx+(y-cy),cy-(x-cx)]);
const rotateCCW=(a,[cx,cy])=>a.map(([x,y])=>[cx-(y-cy),cy+(x-cx)]);
const reflectHorizontal=(a,lineY)=>a.map(([x,y])=>[x,2*lineY-y]);
const twiceArea=a=>(a[1][0]-a[0][0])*(a[2][1]-a[0][1])-(a[1][1]-a[0][1])*(a[2][0]-a[0][0]);
const squaredDistance=([x,y],[u,v])=>(x-u)**2+(y-v)**2;
test('exact U01:L02 Rich V2 selects an existing first-party student worksheet',()=>{
 assert.equal(route.lessonCode,'PED:US-CCSS-MATH:G8:U01:L02');
 assert.equal(route.teacherSelectedOffline,true);assert.equal(route.onlineSubmission,false);
 assert.equal(route.machineGenerated,false);assert.equal(route.teacherGradePersistenceVerified,false);
 const l=rich.Lessons.find(x=>x.LessonCode===route.lessonCode);
 assert(l.ExplanationParagraphs.some(x=>x.includes(route.learnerWorksheetUrl)));
 assert.equal(hash,route.learnerWorksheetSha256);
 for(const id of ['rotation','frames','cards','cool'])assert(paper.includes('id="'+id+'"'));
 assert(paper.includes('This worksheet does not send answers or grades to an online system.'));
 assert.equal(contract.gates.D,'NOT_RUN');assert.equal(contract.gates.READY_FOR_STAGE_D,true);
});
test('student stimuli contain six labelled congruent triangles and genuine reflection',()=>{
 assert.deepEqual(translate(pts[0],2,0),pts[1]);
 assert.deepEqual(rotateCW(pts[1],[2,0]),pts[2]);
 assert.deepEqual(reflectHorizontal(pts[2],0),pts[3]);
 assert.deepEqual(translate(pts[3],0,2),pts[4]);
 assert.deepEqual(rotateCCW(pts[4],[2,2]),pts[5]);
 assert.deepEqual(pts.map(twiceArea),[2,2,2,-2,-2,-2]);
 for(const t of pts)assert.deepEqual([squaredDistance(t[0],t[1]),squaredDistance(t[1],t[2]),squaredDistance(t[0],t[2])],[1,5,4]);
 for(let i=1;i<=6;i++)assert(paper.includes('<tr><td>'+i+'</td>'));
});
test('source activities, 8-point private teacher rubric and response boundaries remain separate',()=>{
 assert.deepEqual(route.activityMap.map(x=>x.worksheet),['rotation','frames','cards','cool']);
 assert.equal(route.privateTeacherRubric.Criteria.reduce((v,c)=>v+c.MaxPoints,0),8);
 assert(route.fixtures.correct.length>=4&&route.fixtures.equivalent.length>=3);
 assert(route.fixtures.wrong.length>=4&&route.fixtures.malformed.length>=4&&route.fixtures.boundary.length>=4);
 assert.equal(route.publisherCooldown,'PUBLISHER_COOLDOWN_UNVERIFIED');
 assert(paper.includes('Card 1')&&paper.includes('Card 6'));
 assert(!paper.includes('Card T1')&&!paper.includes('Card R1')&&!paper.includes('Card F1'));
 assert(!paper.includes('teacherKey'));
 assert.equal(ledger.rows[1].readyForStageD,true);
 assert.equal(ledger.currentSequentialABCCursor.lessonCode,'PED:US-CCSS-MATH:G8:U01:L03');
});
test('cool-down first-party reflection and negative example are geometrically distinct',()=>{
 const original=[[1,1],[3,1],[2,3]];
 const correct=[[-1,1],[-3,1],[-2,3]];
 const wrong=[[-1,1],[-3,1],[2,3]];
 assert.deepEqual(original.map(([x,y])=>[-x,y]),correct);
 assert.notDeepEqual(correct,wrong);
 assert.equal(ledger.abcCounters.ABC_READY_FOR_D,2);
 assert.equal(ledger.abcCounters.D_VERIFIED,0);
});
