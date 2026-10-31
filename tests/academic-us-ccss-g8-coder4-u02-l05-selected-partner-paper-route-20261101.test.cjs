// Grade 8 U02:L05 teacher-selected separate partner information-gap and exact math fixtures — AUTHORED_NOT_RUN.
'use strict';
const {test}=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const root=path.resolve(__dirname,'..'),read=f=>JSON.parse(fs.readFileSync(path.join(root,f),'utf8'));
const c=read('docs/curriculum/us-ccss-g8-coder4-u02-l05-academic-assessment-contract-20261101.json'),r=c.selectedOfflinePaperRoute20261101,k=r.privateAnswerKey;
const led=read('docs/curriculum/us-ccss-g8-coder4-p0-current-execution-ledger-20261030.json'),unit=read('docs/curriculum/us-ccss-g8-coder4-u02-strict-unit-gate-20261031.json');
const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u02-l01-l13-2017.rich-lesson-v2.json'),html=fs.readFileSync(path.join(root,r.learnerWorksheetPath),'utf8');
const dilate=(pt,origin,f)=>pt.map((v,i)=>origin[i]+f*(v-origin[i]));
test('two-source-activity, four-practice exact selected sheet has physically separate partner cards and teacher observation',()=>{
 assert.equal(r.lessonCode,'PED:US-CCSS-MATH:G8:U02:L05');
 assert(rich.Lessons.some(x=>x.LessonCode===r.lessonCode&&x.ExplanationParagraphs.some(z=>z.includes(r.learnerWorksheetUrl))));
 assert.equal(crypto.createHash('sha256').update(Buffer.from(html,'utf8')).digest('hex'),r.learnerWorksheetSha256);
 assert.equal(c.sourceActivityToFirstPartyCrosswalk.length,2);assert.equal(c.sourcePracticeToFirstPartyCrosswalk.length,4);
 for(const id of r.stableTaskIds)assert(html.includes('id="'+id+'"'));
 assert(html.includes('break-before:page'));assert(html.includes('Partner A')&&html.includes('Partner B'));
 assert.equal(r.privateTeacherRubric.Criteria.reduce((s,x)=>s+x.MaxPoints,0),8);
 assert.equal(r.onlineStudentSubmission,false);assert.equal(r.automaticGradeForPartnerTalk,false);assert.equal(r.onlineTeacherGradePersistence,false);
});
test('5.1 one source triangle with same non-origin center scales vertex positions sides and area',()=>{
 const q=k.many;
 for(const x of q.images){
  assert.deepEqual(q.original.map(p=>dilate(p,q.center,x.k)),x.points);
  assert.equal(x.perimeter,q.originalPerimeter*x.k);
  assert.equal(x.area,q.originalArea*x.k*x.k);
 }
});
test('5.2 partner obtains center factor and all original vertices, independently verifies all 4 images',()=>{
 const q=k.infogap;
 assert.deepEqual(q.original.map(p=>dilate(p,q.center,q.factor)),q.images);
 assert.deepEqual(q.requestedFacts,['center','scaleFactor','vertexA','vertexB','vertexC','vertexD']);
 assert.equal(q.partnerObserved,true);
});
test('all four Practice types and first-party Cool-down have exact mathematical oracle and negative boundary',()=>{
 const p=k.practice1;assert.deepEqual(p.original.map(v=>dilate(v,p.center,p.factor)),p.image);assert.deepEqual(p.image[1],p.knownBImage);
 const q=k.practice2;
 assert.deepEqual(q.A.map(pt=>dilate(pt,q.center,q.AtoB)),q.B);
 assert.deepEqual(q.B.map(pt=>dilate(pt,q.center,q.BtoA)),q.A);
 assert.deepEqual(q.C.map(pt=>dilate(pt,q.center,q.CtoB)),q.B);
 const t=k.practice3;
 assert.deepEqual(t.original.map(pt=>dilate(pt,t.center,2)),t.double);
 assert.deepEqual(t.original.map(pt=>dilate(pt,t.center,0.5)),t.half);
 assert.deepEqual(t.double.map(pt=>dilate(pt,t.center,t.doubleToHalf)),t.half);
 assert.equal(k.practice4.givenAngle+k.practice4.rightAngle+k.practice4.otherAcuteAngle,180);
 assert.deepEqual(k.cooldown.original.map(pt=>dilate(pt,k.cooldown.center,k.cooldown.factor)),k.cooldown.image);
 for(const category of ['correct','equivalent','wrong','missing','malformed','boundary'])assert(r.fixtures[category].length>=4);
 assert.equal(c.StageD,'NOT_RUN');assert.equal(led.abcCounters.D_VERIFIED,0);assert.equal(led.abcCounters.ABC_READY_FOR_D,22);
 assert.equal(unit.academicABCReadyForD,5);assert.equal(led.currentSequentialABCCursor.lessonCode,'PED:US-CCSS-MATH:G8:U02:L06');
});