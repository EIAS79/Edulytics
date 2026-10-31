// Grade 8 U02:L02 selected academic-paper contract and mathematical fixtures; AUTHORED_NOT_RUN.
'use strict';
const {test}=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const c=read('docs/curriculum/us-ccss-g8-coder4-u02-l02-academic-assessment-contract-20261031.json'),r=c.selectedOfflinePaperRoute20261031,k=r.privateAnswerKey;
const l=read('docs/curriculum/us-ccss-g8-coder4-p0-current-execution-ledger-20261030.json'),u=read('docs/curriculum/us-ccss-g8-coder4-u02-strict-unit-gate-20261031.json');
const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u02-l01-l13-2017.rich-lesson-v2.json');
const html=fs.readFileSync(path.join(root,r.learnerWorksheetPath),'utf8'),sq=([x,y],[a,b])=>(x-a)**2+(y-b)**2;
test('source-aligned four original activities and four practice families selected in exact L02 RichV2',()=>{
 assert.equal(c.lessonCode,'PED:US-CCSS-MATH:G8:U02:L02');
 assert.equal(c.sourceActivityToFirstPartyCrosswalk.length,4);assert.equal(c.sourcePracticeToFirstPartyCrosswalk.length,4);
 assert(rich.Lessons.some(x=>x.LessonCode===c.lessonCode&&x.ExplanationParagraphs.some(y=>y.includes(r.learnerWorksheetUrl))));
 for(const section of ['notice','droplet','quadrilateral','third','practice','cool'])assert(html.includes('id="'+section+'"'));
 assert((html.match(/<svg /g)||[]).length>=3);
 assert.equal(crypto.createHash('sha256').update(Buffer.from(html,'utf8')).digest('hex'),r.learnerWorksheetSha256);
 assert.equal(r.privateTeacherRubric.Criteria.reduce((sum,x)=>sum+x.MaxPoints,0),8);
 assert.equal(r.studentSubmissionOnline,false);assert.equal(r.geoGebraToolsIntegrated,false);assert.equal(r.teacherGradePersistenceVerified,false);
});
test('four distinct rays on radius2 all reach radius5 with same k=5/2',()=>{
 const d=k.droplet;assert.equal(d.scaleFactor,5/2);
 for(let i=0;i<4;i++){
  assert.equal(sq(d.small[i],[0,0]),4);assert.equal(sq(d.large[i],[0,0]),25);
  assert.deepEqual(d.small[i].map(v=>v*d.scaleFactor),d.large[i]);
 }
});
test('two independent quadrilateral dilations preserve midpoint E and center ray',()=>{
 const q=k.quad;
 assert.deepEqual(q.original.map(p=>p.map(v=>2*v)),q.double);
 assert.deepEqual(q.original.map(p=>p.map(v=>v/2)),q.half);
 assert.deepEqual(q.midpointE.map(v=>2*v),q.doubleE);
 assert.deepEqual(q.midpointE.map(v=>v/2),q.halfE);
 for(let i=0;i<4;i++)assert.equal(sq(q.original[i],q.original[(i+1)%4])*4,sq(q.double[i],q.double[(i+1)%4]));
});
test('one-third dilation around off-origin Q uses center+factor*(point-center)',()=>{
 const h=k.third;const d=p=>p.map((v,i)=>h.center[i]+h.factor*(v-h.center[i]));
 assert.deepEqual(h.original.map(d),h.image);
 assert.deepEqual(h.image[1],[1,2]);
});
test('original source practice P1-P4 and independently authored cooldown have correct numerical boundaries',()=>{
 const p=k.practice1;assert.deepEqual(p.P.map(v=>3*v),p.PImage);
 assert.deepEqual(p.Q.map(v=>v/3),p.QPreimage);
 const t=k.practice2;assert.deepEqual(t.original.map(pt=>pt.map(v=>2*v)),t.double);
 assert.deepEqual(t.original.map(pt=>pt.map(v=>v/2)),t.half);
 assert.ok(Math.abs(t.originalLongest*2-t.doubleLongest)<1e-10);
 assert.ok(Math.abs(t.originalLongest/2-t.halfLongest)<1e-10);
 assert.deepEqual(t.angles,[45,45,90]);
 assert.deepEqual(k.practice3.original.map(pt=>pt.map((v,i)=>v+k.practice3.vector[i])),k.practice3.image);
 assert.equal(k.practice4.valid.reduce((a,b)=>a+b),180);
 assert.notEqual(k.practice4.invalid.reduce((a,b)=>a+b),180);
 assert.deepEqual(k.cooldown.small.map(pt=>pt.map(v=>2*v)),k.cooldown.image);
 for(const category of ['correct','equivalent','wrong','malformed','boundary'])assert(r.fixtures[category].length>=4);
 assert.equal(c.D,'NOT_RUN');assert.equal(l.abcCounters.ABC_READY_FOR_D,19);assert.equal(l.abcCounters.D_VERIFIED,0);
 assert.equal(u.academicABCReadyForD,2);
 assert.equal(l.currentSequentialABCCursor.lessonCode,'PED:US-CCSS-MATH:G8:U02:L03');
});
