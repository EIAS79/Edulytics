'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const root=path.resolve(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const sha=p=>crypto.createHash('sha256').update(fs.readFileSync(path.join(root,p))).digest('hex');
const N=6;
const id='PED:US-CCSS-MATH:G8:U01:L'+String(N).padStart(2,'0');
const e=read('docs/curriculum/us-ccss-g8-coder4-abc-u01-l'+String(N).padStart(2,'0')+'-20261030.json');
const packPath='src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u01-l01-l08-2017.rich-lesson-v2.json';
const l=read(packPath).Lessons[N-1],man=read('docs/curriculum/us-ccss-g8-coder4-course-evidence-20261029.json');
const selected=read('docs/curriculum/us-ccss-g8-coder4-u01-l06-academic-assessment-contract-20261030.json');
test('U01 L'+N+' source roles, unique lesson, rights, fallback visuals and honest A-C gates',()=>{
 assert.equal(e.lessonCode,id);assert.equal(l.LessonCode,id);
 assert.equal(e.blueprintExtraRoles.length,0);
 assert.ok(e.sourceStandardRoles.some(x=>x.reference==='8.G.A.1'));
 assert.equal(e.sourceActivityTitles.length,[3,6].includes(N)?2:3);
 assert.equal(e.sourceToRichV2Crosswalk.length,[3,6].includes(N)?3:4);
 assert.equal(e.independentFirstPartyChanges.existingWorkedExamplesPreserved,3);
 assert.equal(l.WorkedExamples.length,3);
 assert.equal(l.Visuals[0].Kind,'RatioTable');
 assert.equal(l.Visuals.length,N===4?3:4); // L06 adds three native visuals to its preserved original table
 for(const v of l.Visuals.slice(1)){
  assert.ok(['RatioTable','ShapeDecomposition'].includes(v.Kind));
  assert.ok(v.Rows.length>=2);
  assert.ok(v.Rows.every(row=>row.Cells.length>=1&&row.Cells.every(s=>String(s).length>0)));
 }
 assert.equal(sha(packPath),man.hashes.richCandidates['us-ccss-g8-u01-l01-l08-2017.rich-lesson-v2.json']);
 assert.equal(e.gates.A,'A_CLOSED_SOURCE_PARITY');assert.equal(e.gates.B,'B_CLOSED_COURSE_OWNED');
 assert.equal(selected.gates.C,'C_CLOSED_ACADEMIC_SELECTED_FIRST_PARTY_PAPER_TEACHER_ROUTE');
 assert.equal(selected.gates.READY_FOR_STAGE_D,true);assert.equal(selected.gates.D,'NOT_RUN');
 assert.equal(e.practiceContract.exactLessonReadyVerifiedRuntime,false);
 assert.equal(e.practiceContract.teacherRubric.maxPoints,8);
 assert.equal(e.practiceContract.teacherRubric.criteria.length,4);
 assert.equal(e.independentFirstPartyChanges.publisherCoolDown,'PUBLISHER_COOLDOWN_UNVERIFIED');
 for(const k of ['student','teacher','teacherPreparation'])assert.match(e.sourcePaths[k].sha256,/^[a-f0-9]{64}$/);
});
test('U01 L'+N+' coordinate candidate accepts equivalents and rejects wrong, malformed without runtime claims',()=>{
 const c=e.practiceContract.machine;
 const p=s=>{
  if(typeof s!=='string')return null;
  const m=s.trim().match(/^\(?\s*(-?\d+(?:\.\d+)?)\s*,\s*(-?\d+(?:\.\d+)?)\s*\)?$/);
  return m?[+m[1],+m[2]]:null;
 };
 const same=s=>{const q=p(s);return q!==null&&q[0]===c.expected[0]&&q[1]===c.expected[1];};
 for(const s of c.correct)assert.equal(same(s),true,s);
 for(const s of c.wrong)assert.equal(same(s),false,s);
 for(const s of c.invalid)assert.equal(p(s),null,String(s));
 if(N===2)assert.deepEqual([c.start[0]+c.vector[0],c.start[1]+c.vector[1]],c.expected);
 if(N===3)assert.deepEqual([-c.start[1],c.start[0]],c.expected);
 if(N===4)assert.deepEqual([-(c.start[0]+c.vector[0]),c.start[1]+c.vector[1]],c.expected);
 if(N===5||N===6){const x=c.start[0]-c.center[0],y=c.start[1]-c.center[1];assert.deepEqual([c.center[0]-y,c.center[1]+x],c.expected);}
 assert.equal(e.practiceContract.exactLessonReadyVerifiedRuntime,false);
});
test('U01 L'+N+' independent mathematics is sound and original source activity purpose is represented',()=>{
 const m=e.mathOracleIndependentCases;
 const sq=(a,b)=>Math.hypot(a[0]-b[0],a[1]-b[1]);
 if(N===2){
  const a=m.rotation120;
  assert.ok(Math.abs(sq(a.original,a.center)-sq(a.image,a.center))<1e-9);
  assert.ok(Math.abs(a.image[0]-(2*Math.cos(120*Math.PI/180)))<1e-9);
  assert.ok(Math.abs(a.image[1]-(2*Math.sin(120*Math.PI/180)))<1e-9);
  const z=m.danceFrames;
  assert.equal(z.length,6);
  assert.deepEqual(z[2].slice(1,3),z[3].slice(1,3));
  assert.notEqual(z[2][3],z[3][3],'a reflection changes chirality without changing tip or P');
  assert.deepEqual(m.reflectionExample.yes,[[-1,1],[-3,1],[-2,3]]);
  assert.notDeepEqual(m.reflectionExample.yes,m.reflectionExample.no);
 }
 if(N===3){
  for(let i=0;i<3;i++){
   const p=m.sourceTriangle[i],q=m.translated[i],rot=m.rotatedCCW[i],ref=m.reflectionY[i],a=m.isometricAxial[i],b=m.isometricRotated[i];
   assert.deepEqual(q,[p[0]+2,p[1]-1]);
   assert.deepEqual(rot,[-p[1],p[0]]);
   assert.deepEqual(ref,[-p[0],p[1]]);
   assert.deepEqual(b,[-a[1],a[0]+a[1]]);
  }
  for(const key of ['translated','rotatedCCW','reflectionY'])for(let a=0;a<3;a++)for(let b=a+1;b<3;b++)
   assert.ok(Math.abs(sq(m.sourceTriangle[a],m.sourceTriangle[b])-sq(m[key][a],m[key][b]))<1e-9);
 }
 if(N===5){
  assert.deepEqual(m.translationPairs.map(([a,b])=>[b[0]-a[0],b[1]-a[1]]),[m.vector,m.vector]);
  for(const [a,b] of m.xAxisPairs)assert.deepEqual(b,[a[0],-a[1]]);
  assert.deepEqual(m.yAxisPair[1],[-m.yAxisPair[0][0],m.yAxisPair[0][1]]);
  const [a,b]=m.segment;
  const ccw=(p,c)=>[c[0]-(p[1]-c[1]),c[1]+p[0]-c[0]];
  assert.deepEqual(ccw(a,b),m.rotAboutB);assert.deepEqual(ccw(b,a),m.rotAboutA);
  assert.deepEqual(m.originCW,[a,b].map(p=>[p[1],-p[0]]));
  for(const q of [m.rotAboutB,m.rotAboutA])assert.ok(Math.hypot(q[0]-a[0],q[1]-a[1])>=0);
 }
 if(N===6){
  const c=m.center;
  const ccw=p=>[c[0]-(p[1]-c[1]),c[1]+p[0]-c[0]];
  assert.deepEqual(m.original.map(ccw),m.image);
  for(let i=0;i<3;i++){
   assert.deepEqual(m.cooldownAfterTranslation[i],[m.cooldownOriginal[i][0]+2,m.cooldownOriginal[i][1]]);
   assert.deepEqual(m.cooldownAfterReflection[i],[-m.cooldownAfterTranslation[i][0],m.cooldownAfterTranslation[i][1]]);
   const a=m.original[i],b=m.image[i];
   assert.ok(Math.abs(sq(a,c)-sq(b,c))<1e-9);
  }
  assert.deepEqual(m.twoReflectionsAnswer,m.twoReflectionsExample.map(x=>-x));
 }
 if(N===4){
  for(let i=0;i<3;i++){
   const p=m.triangle[i],q=m.afterTranslation[i],z=m.afterReflection[i];
   assert.deepEqual(q,[p[0]+2,p[1]]);
   assert.deepEqual(z,[-q[0],q[1]]);
  }
  assert.notDeepEqual(m.afterReflection[0],m.reverseOrderExample);
 }
});
