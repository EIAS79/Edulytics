// G8 U02:L03 teacher-selected offline assessment source math fixtures. AUTHORED_NOT_RUN.
'use strict';
const {test}=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const c=read('docs/curriculum/us-ccss-g8-coder4-u02-l03-academic-assessment-contract-20261031.json'),r=c.selectedOfflinePaperRoute20261031,k=r.privateAnswerKey;
const led=read('docs/curriculum/us-ccss-g8-coder4-p0-current-execution-ledger-20261030.json'),unit=read('docs/curriculum/us-ccss-g8-coder4-u02-strict-unit-gate-20261031.json');
const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u02-l01-l13-2017.rich-lesson-v2.json'),html=fs.readFileSync(path.join(root,r.learnerWorksheetPath),'utf8');
const dilate=(p,origin,f)=>p.map((v,i)=>origin[i]+f*(v-origin[i])),close=(a,b)=>a.every((v,i)=>Math.abs(v-b[i])<1e-9);
test('exact L03 RichV2 selects paper for three source activities and six Practice families',()=>{
 assert.equal(c.lessonCode,'PED:US-CCSS-MATH:G8:U02:L03');assert.equal(c.sourceActivityToFirstPartyCrosswalk.length,3);
 assert.equal(c.sourcePracticeToFirstPartyCrosswalk.length,6);
 assert(rich.Lessons.some(x=>x.LessonCode===r.lessonCode&&x.ExplanationParagraphs.some(y=>y.includes(r.learnerWorksheetUrl))));
 assert.equal(crypto.createHash('sha256').update(Buffer.from(html,'utf8')).digest('hex'),r.learnerWorksheetSha256);
 for(const sec of ['ray','obstacle','perspective','practice','cool'])assert(html.includes('id="'+sec+'"'));
 assert.equal(r.privateTeacherRubric.Criteria.reduce((s,x)=>s+x.MaxPoints,0),8);
 assert.equal(r.studentSubmissionOnline,false);assert.equal(r.sourceAppletIntegrated,false);
});
test('A-B ray factor2 and factor1/2 images preserve positive ray and correct distances',()=>{
 assert.deepEqual(dilate(k.ray.B,k.ray.A,2),k.ray.C);
 assert.deepEqual(dilate(k.ray.B,k.ray.A,0.5),k.ray.D);
});
test('obstacle course tests nontrivial forward, inverse, fixed-point cases',()=>{
 const q=k.obstacle;
 assert.deepEqual(dilate(q.B,q.A,5),q.C);
 assert.deepEqual(dilate(q.G,q.H,1/3),q.E);
 assert.deepEqual(dilate(q.E,q.H,3),q.G);
 assert.deepEqual(dilate(q.F,q.A,0.5),q.B);
 assert(close(dilate(q.H,q.A,1/3),q.J));
 assert.deepEqual(dilate(q.F,q.A,5/4),q.H);
 assert.deepEqual(dilate(q.H,q.B,1),q.H);
});
test('perspective 3/2 without grid dilates every polygon vertex about K, not origin',()=>{
 const q=k.perspective;
 assert.deepEqual(dilate(q.P,q.center,4),q.PImage);
 assert.deepEqual(dilate(q.Q,q.center,1/2),q.QImage);
 assert.deepEqual(q.polygonOriginal.map(p=>dilate(p,q.polygonCenter,q.polygonFactor)),q.polygonImage);
});
test('all six practice skill families and boundary cases mathematically authored, no Stage D run',()=>{
 assert.deepEqual(k.practice1.scaled,[15,11.1,0.6]);
 const q=k.practice2;
 assert.deepEqual(dilate(q.B,q.A,2),q.C);assert.deepEqual(dilate(q.A,q.B,2),q.D);
 assert.deepEqual(dilate(q.B,q.A,0.5),q.E);assert.deepEqual(dilate(q.A,q.B,0.5),q.F);
 assert.equal(k.practice4.forward*k.practice4.inverse,1);
 assert.equal(k.practice5.vertical+k.practice5.adjacent,180);
 assert.deepEqual(k.practice6.original.map(p=>dilate(p,[0,0],1).map((v,i)=>v+[2,-1][i])),k.practice6.image);
 assert.deepEqual(k.cooldown.original.map(p=>dilate(p,k.cooldown.center,k.cooldown.factor)),k.cooldown.image);
 for(const key of ['correct','equivalent','wrong','malformed','boundary'])assert(r.fixtures[key].length>=4);
 assert.equal(c.D,'NOT_RUN');assert.equal(led.abcCounters.ABC_READY_FOR_D,20);assert.equal(led.abcCounters.D_VERIFIED,0);
 assert.equal(unit.academicABCReadyForD,3);
 assert.equal(led.currentSequentialABCCursor.lessonCode,'PED:US-CCSS-MATH:G8:U02:L04');
});