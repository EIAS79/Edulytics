// Grade 8 U02:L06 teacher-selected similarity transformation/pair-work paper; AUTHORED_NOT_RUN.
'use strict';
const {test}=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const c=read('docs/curriculum/us-ccss-g8-coder4-u02-l06-academic-assessment-contract-20261101.json'),r=c.selectedOfflinePaperRoute20261101,k=r.privateAnswerKey;
const l=read('docs/curriculum/us-ccss-g8-coder4-p0-current-execution-ledger-20261030.json'),u=read('docs/curriculum/us-ccss-g8-coder4-u02-strict-unit-gate-20261031.json');
const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u02-l01-l13-2017.rich-lesson-v2.json'),html=fs.readFileSync(path.join(root,r.learnerWorksheetPath),'utf8');
const rot90=([x,y])=>[-y,x],reflectY=([x,y])=>[-x,y],reflectX=([x,y])=>[x,-y],t=(p,[dx,dy])=>[p[0]+dx,p[1]+dy],d=(p,ctr,f)=>p.map((v,i)=>ctr[i]+f*(v-ctr[i]));
test('L06 student-facing paper selected exactly from Rich V2; partner sets and private rubric',()=>{
 assert.equal(c.lessonCode,'PED:US-CCSS-MATH:G8:U02:L06');
 assert.equal(c.sourceActivityToFirstPartyCrosswalk.length,4);assert.equal(c.sourcePracticeToFirstPartyCrosswalk.length,3);
 assert(rich.Lessons.some(x=>x.LessonCode===c.lessonCode&&x.ExplanationParagraphs.some(p=>p.includes(r.learnerWorksheetUrl))));
 assert.equal(crypto.createHash('sha256').update(Buffer.from(html,'utf8')).digest('hex'),r.learnerWorksheetSha256);
 for(const id of r.stableTaskIds)assert(html.includes('id="'+id+'"'));
 assert.equal(r.privateTeacherRubric.criteria.reduce((v,x)=>v+x.points,0),8);
 assert.equal(r.partnerSetsDistinct,true);assert.equal(r.onlineStudentSubmission,false);assert.equal(r.automaticExplanationGrading,false);
});
test('6.1 exact equivalent expression and two first-party complete figure maps',()=>{
 assert.equal(10*(2+3)-8*3,k.warmup.value);
 assert.deepEqual(k.triangle.original.map(p=>t(d(reflectY(p),[0,0],2),[5,1])),k.triangle.target);
 assert.deepEqual(k.hexagon.original.map(p=>t(d(rot90(p),[0,0],2),[5,1])),k.hexagon.target);
});
test('6.3 prescribed four transform orders each reproduce corresponding labelled vertices',()=>{
 const f=k.four,q=f.original;
 assert.deepEqual(q.map(p=>reflectY(t(p,[3,1]))),f.B);
 assert.deepEqual(q.map(p=>d(reflectX(p),[0,0],2)),f.C);
 assert.deepEqual(q.map(p=>reflectY(rot90(p))),f.D);
 assert.deepEqual(q.map(p=>t(d(p,[0,0],0.5),[4,2])),f.E);
 assert.deepEqual(f.congruent,['B','D']);
 assert.deepEqual(f.similarNoncongruent,['C','E']);
});
test('6.4 two distinct five-card sets reach same target in different valid ordered sequences',()=>{
 const q=k.cards;
 const viaA=q.original.map(p=>t(d(p,[0,0],2),[3,1]));
 const viaB=q.original.map(p=>d(t(p,[3,1]),[3,1],2));
 assert.deepEqual(viaA,q.target);assert.deepEqual(viaB,q.target);
 assert.equal(q.cardA.length,5);assert.equal(q.cardB.length,5);
 assert.notDeepEqual(q.cardA,q.cardB);
 assert.deepEqual(q.Asequence,['A2','A1']);assert.deepEqual(q.Bsequence,['B1','B2']);
});
test('three original Practice task families and original cooldown preserve vertex-level mathematical oracles',()=>{
 assert.deepEqual(k.practice1.original.map(p=>t(d(rot90(p),[0,0],2),[4,2])),k.practice1.target);
 assert.deepEqual(k.practice2.original.map(([x,y])=>[11-2*x,2*y-9]),k.practice2.target);
 assert.deepEqual(k.practice3.large.map(p=>d(p,k.practice3.center,k.practice3.factor)),k.practice3.small);
 assert.equal(k.practice3.factor*k.practice3.inverse,1);
 assert.deepEqual(k.cooldown.original.map(p=>t(d(p,k.cooldown.center,k.cooldown.factor),k.cooldown.translation)),k.cooldown.target);
 for(const kind of ['correct','equivalent','wrong','missing','malformed','boundary'])assert(r.fixtures[kind].length>=4);
 assert.equal(c.StageD,'NOT_RUN');assert.equal(l.abcCounters.D_VERIFIED,0);assert.equal(l.abcCounters.ABC_READY_FOR_D,23);
 assert.equal(u.academicABCReadyForD,6);assert.equal(l.currentSequentialABCCursor.lessonCode,'PED:US-CCSS-MATH:G8:U02:L07');
});
