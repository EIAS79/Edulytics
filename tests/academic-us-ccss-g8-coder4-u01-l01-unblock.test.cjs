'use strict';
const test=require('node:test'), assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const r=path.resolve(__dirname,'..');
const txt=p=>fs.readFileSync(path.join(r,p),'utf8');
const j=p=>JSON.parse(txt(p));
const sha=p=>crypto.createHash('sha256').update(fs.readFileSync(path.join(r,p))).digest('hex');
const ev=j('docs/curriculum/us-ccss-g8-coder4-u01-l01-unblock-evidence-20261030.json');
const lesson=j('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u01-l01-l08-2017.rich-lesson-v2.json').Lessons[0];
const near=(a,b,t=.002)=>assert.ok(Math.abs(a-b)<t, String(a)+' != '+String(b));
const attrs=s=>Object.fromEntries([...s.matchAll(/([\w-]+)="([^"]*)"/g)].map(m=>[m[1],m[2]]));
test('L01 first-party candidate preserves all prior learning and has native fallback',()=>{
 assert.equal(ev.lessonCode,'PED:US-CCSS-MATH:G8:U01:L01');
 assert.equal(lesson.LessonCode,ev.lessonCode);
 assert.equal(lesson.WorkedExamples.length,3);
 assert.equal(lesson.Visuals.length,5);
 assert.deepEqual(lesson.Visuals.slice(1).map(x=>x.Kind),['ShapeDecomposition','RatioTable','RatioTable','RatioTable']);
 assert.deepEqual(ev.sourceStandardRole.source.map(x=>x.role),['BuildingOn','BuildingTowards']);
 assert.equal(ev.sourceStandardRole.approved,false);
 assert.equal(ev.gates.READY_FOR_STAGE_D,false);
 assert.equal(ev.nativeVisuals.renderActuallyObservedWithStudentAuthentication,false);
 const h=j('docs/curriculum/us-ccss-g8-coder4-course-evidence-20261029.json').hashes.richCandidates['us-ccss-g8-u01-l01-l08-2017.rich-lesson-v2.json'];
 assert.equal(sha('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u01-l01-l08-2017.rich-lesson-v2.json'),h);
});
test('L01 new SVG angle construction has exact straight/right/acute/obtuse rays and safe semantics',()=>{
 const a=ev.assets[0],s=txt(a.file);
 assert.equal(sha(a.file),a.sha256);
 assert.match(s,/role="img" aria-labelledby="title desc"/);
 assert.match(s,/<title id="title">/);assert.match(s,/<desc id="desc">/);
 assert.doesNotMatch(s,/<script|<foreignObject|onload=|onerror=|<iframe|<image|href=|<a\s/i);
 for(const fig of a.angles){
  const g=s.match(new RegExp('<g id="angle-'+fig.label+'" data-degrees="(\\d+)">'));assert.ok(g);
  assert.equal(+g[1],fig.degrees);
  const x=attrs(s.match(new RegExp('<line id="ray-'+fig.label+'-0" ([^>]+)\\/>'))[1]);
  const y=attrs(s.match(new RegExp('<line id="ray-'+fig.label+'-1" ([^>]+)\\/>'))[1]);
  near(+x.x1,+y.x1);near(+x.y1,+y.y1);near(+x.y1,+x.y2);
  assert.ok(+x.x2>+x.x1);
  const theta=Math.atan2(+y.y1-(+y.y2),+y.x2-(+y.x1))*180/Math.PI;
  near(theta,fig.degrees,.01);
 }
});
test('L01 three independent six-frame SVG dances preserve triangle side lengths and exact rotations',()=>{
 const a=ev.assets[1],s=txt(a.file);assert.equal(sha(a.file),a.sha256);
 assert.match(s,/role="img" aria-labelledby="title desc"/);
 assert.doesNotMatch(s,/<script|<foreignObject|onload=|onerror=|<iframe|<image|href=|<a\s/i);
 const direction={north:0,east:1,south:2,west:3},arrows=[[0,-29],[29,0],[0,29],[-29,0]];
 for(const name of ['A','B','C']){
  const seq=a.states[name];assert.equal(seq.length,6);let first=null;
  for(let i=0;i<6;i++){
   const hit=s.match(new RegExp('<g id="dance-'+name+'-frame-'+(i+1)+'" ([^>]+)>'));assert.ok(hit);
   const t=attrs(hit[1]);
   assert.equal(+t['data-x'],seq[i].x);assert.equal(+t['data-y'],seq[i].y);assert.equal(t['data-direction'],seq[i].direction);
   const poly=s.match(new RegExp('<polygon id="triangle-'+name+'-'+(i+1)+'" points="([^"]+)"'));assert.ok(poly);
   const p=poly[1].split(' ').map(x=>x.split(',').map(Number));
   assert.equal(p.length,3);
   const [q,v,w]=p,expected=arrows[direction[seq[i].direction]];
   near(v[0]-q[0],expected[0]);near(v[1]-q[1],expected[1]);
   const dist=(u,v)=>Math.hypot(u[0]-v[0],u[1]-v[1]);
   const sides=[dist(q,v),dist(q,w),dist(v,w)];
   if(!first)first=sides;else for(let k=0;k<3;k++)near(sides[k],first[k]);
   if(i){
    const prev=seq[i-1],current=seq[i];
    const slide=prev.x!==current.x||prev.y!==current.y;
    const turn=prev.direction!==current.direction;
    assert.notEqual(slide,turn);
    if(turn)assert.ok([1,3].includes((direction[current.direction]-direction[prev.direction]+4)%4));
   }
  }
  assert.equal(lesson.Visuals.find(x=>x.Title.endsWith('Dance '+name)).Rows.length,6);
 }
 assert.deepEqual(a.states.A.slice(0,3),a.states.B.slice(0,3));
 assert.notDeepEqual(a.states.C[2],a.states.A[2]);
 assert.notDeepEqual(a.states.A[3],a.states.B[3]);
});
test('L01 candidate final-coordinate assessment accepts equivalents, rejects wrong and invalid, never claims runtime grading',()=>{
 const c=ev.assessmentContract.machineCandidate;
 assert.equal(c.productionGeneratorConnected,false);
 assert.equal(ev.assessmentContract.actualFamilyAndNegativeGradingVerified,false);
 const parse=x=>{
  if(typeof x!=='string')return null;
  const m=x.trim().match(/^\(?\s*(-?\d+(?:\.\d+)?)\s*,\s*(-?\d+(?:\.\d+)?)\s*\)?$/);
  return m?[Number(m[1]),Number(m[2])]:null;
 };
 const correct=x=>{const p=parse(x);return p!==null&&p[0]===4&&p[1]===1};
 for(const x of [c.correct,...c.equivalentAccepted])assert.equal(correct(x),true);
 for(const x of c.wrongRejected)assert.equal(correct(x),false);
 for(const x of c.invalidRejected)assert.equal(parse(x),null);
 assert.equal(ev.assessmentContract.human.savedTeacherScoresObserved,false);
});
