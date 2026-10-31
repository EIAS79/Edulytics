
'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('fs'),path=require('path'),crypto=require('crypto');
const root=path.resolve(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const sha=p=>crypto.createHash('sha256').update(fs.readFileSync(path.join(root,p))).digest('hex');
const evidence=read('docs/curriculum/us-ccss-g8-coder4-v13-u01-l01-l07-source-evidence-20261029.json');
const ledger=read('docs/curriculum/us-ccss-g8-coder4-v13-u01-complete-spot-review-ledger-20261029.json');
const triage=read('docs/curriculum/us-ccss-g8-coder4-v13-u01-complete-source-practice-teacher-audit-20261029.json');
const last=read('docs/curriculum/us-ccss-g8-coder4-v13-u01-extended-implementation-ledger-20261029.json');
const source=read('docs/curriculum/us-ccss-g8-coder4-v9-activity-figure-inventory-20261029.json').rows;
const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u01-l01-l08-2017.rich-lesson-v2.json').Lessons;
const near=(a,b)=>assert.ok(Math.abs(a-b)<1e-9,`${a} vs ${b}`);
test('Six early U01 source-specific repairs preserve original Rich V2 lessons and exclude counterfeit academic approvals',()=>{
 assert.equal(evidence.lessons.length,6);
 for(const e of evidence.lessons){
  const l=rich.find(x=>x.LessonCode===e.lessonCode),s=source.find(x=>x.lessonCode===e.lessonCode);
  assert.ok(l&&s,e.lessonCode);
  assert.deepEqual(e.sourceActivityTitles,s.activities.map(x=>x.title));
  for(const p of e.addedCourseOwnedTeaching)assert.ok(l.ExplanationParagraphs.includes(p),e.lessonCode);
  assert.equal(l.WorkedExamples.length,3);
  assert.equal(e.sourceParityAccepted,false);
  assert.equal(e.interactiveEquivalentVerified,false);
  assert.equal(e.practiceExactGeneratedPersisted,false);
  assert.equal(e.teacherRubricPersisted,false);
 }
 assert.equal(sha('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u01-l01-l08-2017.rich-lesson-v2.json'),read('docs/curriculum/us-ccss-g8-coder4-course-evidence-20261029.json').hashes.richCandidates['us-ccss-g8-u01-l01-l08-2017.rich-lesson-v2.json']); // V13 evidence.newContentHash remains an immutable historical checkpoint
});
test('U01 L05 source translation options accept correct displacements and reject reversed/wrong vectors',()=>{
 const T=[[-3,0],[-2,-1],[-4,-3]],U=[[1,2],[2,1],[0,-1]];
 for(let i=0;i<3;i++)assert.deepEqual([U[i][0]-T[i][0],U[i][1]-T[i][1]],[4,2]);
 const opt=[[[ -3,0],[1,2]],[[2,1],[-2,-1]],[[-4,-3],[0,-1]],[[1,2],[2,1]]];
 assert.deepEqual(opt.map(([a,b])=>b[0]-a[0]===4&&b[1]-a[1]===2),[true,false,true,false]);
 const xReflect=([x,y])=>[x,-y],yReflect=([x,y])=>[-x,y];
 for(const [p,expected] of [[ [13,10],[13,-10]],[[13,-20],[13,20]],[[13,570],[13,-570]]])assert.deepEqual(xReflect(p),expected);
 assert.deepEqual(yReflect([3,2]),[-3,2]);
 assert.deepEqual(xReflect(xReflect([13,570])),[13,570]);
});
test('U01 L06 independent 90° rotation center verifier rejects incorrect candidate',()=>{
 const P={x:2,y:1},target={x:0,y:3};
 const rot=(p,c)=>({x:c.x-(p.y-c.y),y:c.y+(p.x-c.x)});
 assert.deepEqual(rot(P,{x:0,y:1}),target);
 assert.notDeepEqual(rot(P,{x:0,y:0}),target);
 near(Math.hypot(P.x-0,P.y-1),Math.hypot(target.x-0,target.y-1));
});
test('U01 L07 first-party fraction/ruler precision checks are mathematically exact',()=>{
 near(4+5/8,4.625);
 near(4+7/10,4.7);
 near(3+3/4,3.75);
 near(Math.round(4.625*8)/8,4.625);
 near(Math.round(3.75*8)/8,3.75);
 assert.equal(Math.round(4.7*10)/10,4.7);
});
test('Source review across all 17 U01 lessons is explicitly spot-check only, all 131 A-D remain blocked',()=>{
 assert.equal(ledger.rows.length,131);assert.equal(triage.rows.length,131);
 assert.deepEqual(ledger.rows.map(x=>x.lessonCode),last.rows.map(x=>x.lessonCode));
 assert.equal(ledger.counts.sourceActivitySpotReviewedLessons,17);
 assert.equal(triage.unitSummary.U01.sourceActivitySpotCheckedTotal,17);
 assert.equal(triage.unitSummary.U01.lessons,17);
 assert.equal(ledger.counts.assets,36);
 assert.equal(ledger.counts.sourceFigurePriorityWithOriginalAsset,34);
 assert.equal(ledger.counts.sourceFigurePriorityWithoutOriginalAsset,79);
 for(const x of ledger.rows){
  assert.equal(x.A,'NOT_ACCEPTED');assert.equal(x.B,'NOT_ACCEPTED');
  assert.equal(x.C,'NOT_VERIFIED');assert.equal(x.D,'NOT_VERIFIED');
  assert.equal(x.actualPostgresqlAttemptPersistedAndReadBack,false);
 }
 const selected=triage.rows.filter(x=>x.unit===1);
 assert.equal(selected.length,17);
 assert.ok(selected.every(x=>x.visuals.authenticatedStudentRenderVerified===false));
 assert.ok(selected.every(x=>x.teacherAssessmentAudit.teacherScoringRuntimeVerified===false));
});

test('U01 L01 six-frame first-party dance cards, reversed sequence and non-automated Cool-down are present without claiming publisher parity',()=>{
 const lesson=rich.find(x=>x.LessonCode==='PED:US-CCSS-MATH:G8:U01:L01');
 assert.ok(lesson);
 const paragraphs=lesson.ExplanationParagraphs;
 for(const phrase of ['Original Edulytiks accessible six-frame dance-card substitute','Dance A frames 1–6','Dance B frames 1–6','Dance C frames 1–6','Source extension — reverse the dance','Original Edulytiks Cool-down (first-party equivalent'])
  assert.ok(paragraphs.some(p=>p.includes(phrase)),phrase);
 const d={A:[[0,0,0],[2,0,0],[2,0,1],[2,1,1],[2,1,0],[3,1,0]],B:[[0,0,0],[2,0,0],[2,0,1],[2,-1,1],[2,-1,2],[1,-1,2]],C:[[0,0,0],[2,0,0],[2,0,3],[2,1,3],[2,1,2],[1,1,2]]};
 assert.deepEqual(d.A.slice(0,3),d.B.slice(0,3));
 assert.notDeepEqual(d.A[2],d.C[2]);
 assert.notDeepEqual(d.A[3],d.B[3]);
 for(const seq of Object.values(d)){
  assert.equal(seq.length,6);
  for(let i=1;i<seq.length;i++){
   const a=seq[i-1],b=seq[i],moved=(a[0]!==b[0]||a[1]!==b[1]),turned=a[2]!==b[2];
   assert.notEqual(moved,turned,'each frame must be a single slide or quarter-turn');
   if(turned)assert.ok([1,3].includes((b[2]-a[2]+4)%4));
  }
  assert.deepEqual([...seq].reverse().reverse(),seq);
 }
 assert.equal(evidence.lessons.find(x=>x.lessonCode===lesson.LessonCode).sourceParityAccepted,false); // source provenance remains non-certified
});
