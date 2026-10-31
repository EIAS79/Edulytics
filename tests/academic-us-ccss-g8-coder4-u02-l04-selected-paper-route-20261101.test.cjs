// Grade 8 U02:L04 — exact source-aligned printable teacher-selected paper/math oracle. AUTHORED_NOT_RUN.
'use strict';
const {test}=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const root=path.resolve(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const c=read('docs/curriculum/us-ccss-g8-coder4-u02-l04-academic-assessment-contract-20261101.json'),r=c.selectedOfflinePaperRoute20261101,k=r.privateAnswerKey;
const led=read('docs/curriculum/us-ccss-g8-coder4-p0-current-execution-ledger-20261030.json');
const unit=read('docs/curriculum/us-ccss-g8-coder4-u02-strict-unit-gate-20261031.json');
const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u02-l01-l13-2017.rich-lesson-v2.json'),html=fs.readFileSync(path.join(root,r.learnerWorksheetPath),'utf8');
const sha=x=>crypto.createHash('sha256').update(x).digest('hex'),dilate=(p,o,f)=>p.map((v,i)=>o[i]+f*(v-o[i]));
test('real L04 selected first-party student paper from Rich V2 with existing immutable SVG, private rubric and five task IDs',()=>{
 assert.equal(r.lessonCode,'PED:US-CCSS-MATH:G8:U02:L04');
 assert(rich.Lessons.some(x=>x.LessonCode===r.lessonCode&&x.ExplanationParagraphs.some(y=>y.includes(r.learnerWorksheetUrl))));
 assert.equal(sha(Buffer.from(html,'utf8')),r.learnerWorksheetSha256);
 for(const id of r.stableTaskIds)assert(html.includes('id="'+id+'"'));
 for(const fig of r.existingFigures){
   const p='src/Edulytics.Web/wwwroot'+fig.url;
   assert(html.includes(fig.url));assert.equal(sha(fs.readFileSync(path.join(root,p))),fig.sha256);
 }
 assert.equal(r.teacherRubric.criteria.reduce((v,x)=>v+x.points,0),8);
 assert.equal(r.onlineSubmission,false);assert.equal(r.generatedOnlinePracticeVerified,false);assert.equal(r.savedTeacherGradeVerified,false);
});
test('4.1 correctly uses AC/AB rather than BC/AB',()=>{
 const q=k.estimate;
 assert.equal(Math.hypot(q.B[0]-q.A[0],q.B[1]-q.A[1]),q.AB);
 assert.equal(Math.hypot(q.C[0]-q.A[0],q.C[1]-q.A[1]),q.AC);
 assert.equal(q.AC/q.AB,q.factor);assert.notEqual((q.AC-q.AB)/q.AB,q.factor);
});
test('4.2 P-centered rectangle and two independent T-centered triangle images have all correspondences',()=>{
 const z=k.rectangle,t=k.triangle;
 assert.deepEqual(z.vertices.map(p=>dilate(p,z.center,z.factor)),z.image);
 assert.deepEqual(t.vertices.map(p=>dilate(p,t.center,2)),t.double);
 assert.deepEqual(t.vertices.map(p=>dilate(p,t.center,1/2)),t.half);
 assert.notDeepEqual(t.double.map(p=>dilate(p,t.center,1/2)),t.half);
});
test('4.3 six-to-five source-modality card sort has unique exact matches and one mathematically constructed unmatched card',()=>{
 const x=k.cards,counts={};
 for(const task of x.numbered){
   const img=x.original.map(p=>dilate(p,task.center,task.factor));
   if(task.letter){assert.deepEqual(img,x.lettered[task.letter]);counts[task.letter]=(counts[task.letter]||0)+1;}
   else{assert.deepEqual(img,x.unmatched);assert(!Object.values(x.lettered).some(v=>JSON.stringify(v)===JSON.stringify(img)));}
 }
 assert.deepEqual(Object.values(counts).sort(),[1,1,1,1,1]);
 assert.equal(x.numbered.filter(y=>!y.letter).length,1);
 assert.deepEqual(x.circle.originalCenter.map(v=>v*x.circle.scale),x.circle.imageCenter);
 assert.equal(x.circle.originalRadius*x.circle.scale,x.circle.imageRadius);
});
test('five source Practice families and independent Cool-down preserve exact mathematics and teacher boundaries',()=>{
 const q=k.practice2;
 assert.deepEqual(q.original.map(p=>dilate(p,q.R,2)),q.doubleR);
 assert.deepEqual(q.original.map(p=>dilate(p,q.O,1/2)),q.halfO);
 assert.deepEqual(q.original.map(p=>dilate(p,q.S,1/2)),q.halfS);
 assert.equal(k.practice1.congruent,false);assert.equal(k.practice1.sideFactor,2);
 for(let i=0;i<2;i++)assert.equal(k.practice3.vertical[i]+k.practice3.adjacent[i],180);
 assert.equal(k.practice3.parallelAssumed,false);
 assert.deepEqual(k.practice4.original.map(([x,y])=>[3-x,y+1]),k.practice4.image);
 assert.deepEqual(k.practice5.final,[2,-3]);
 assert.deepEqual(k.cooldown.vertices.map(p=>dilate(p,k.cooldown.center,k.cooldown.factor)),k.cooldown.image);
 for(const cat of ['correct','equivalent','wrong','missing','malformed','boundary'])assert(r.fixtures[cat].length>=4);
 assert.equal(c.StageD,'NOT_RUN');assert.equal(led.abcCounters.ABC_READY_FOR_D,21);
 assert.equal(led.abcCounters.D_VERIFIED,0);assert.equal(unit.academicABCReadyForD,4);
 assert.equal(led.currentSequentialABCCursor.lessonCode,'PED:US-CCSS-MATH:G8:U02:L05');
});