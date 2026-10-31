'use strict';
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-hs-alg2-phase29-v1.lesson-content-pack.json');
const ledger=read('artifacts/curriculum-delivery/US-CCSS-MATH/coder-7/algebra2-120-lesson-progress.json');
const near=(a,b,tol=1e-8)=>assert.ok(Math.abs(a-b)<tol,String(a)+' != '+String(b));
test('U06 L09 sine cosine graph unit circle and identities',()=>{
 const key=[0,Math.PI/2,Math.PI,3*Math.PI/2,2*Math.PI];
 [1,0,-1,0,1].forEach((v,i)=>near(Math.cos(key[i]),v));
 [0,1,0,-1,0].forEach((v,i)=>near(Math.sin(key[i]),v));
 for(const theta of key)near(Math.cos(theta)**2+Math.sin(theta)**2,1);
 for(const theta of [Math.PI/4,5*Math.PI/4])near(Math.cos(theta),Math.sin(theta));
 near(Math.cos(0)+Math.sin(0),1);
});
test('U06 L10 coterminal windmill angle positions',()=>{
 const angles=[Math.PI/3,4*Math.PI/3,7*Math.PI/4,5*Math.PI/2,3*Math.PI];
 const target=[[.5,Math.sqrt(3)/2],[-.5,-Math.sqrt(3)/2],[Math.SQRT1_2,-Math.SQRT1_2],[0,1],[-1,0]];
 angles.forEach((x,i)=>{near(Math.cos(x),target[i][0]);near(Math.sin(x),target[i][1]);});
 for(const t of [0,.5,1.2,Math.PI/4])near(Math.sin(t),Math.sin(t+2*Math.PI));
});
test('U06 L11 negative angles and parity',()=>{
 for(const x of [Math.PI/6,Math.PI/3,Math.PI/2,7*Math.PI/4]){near(Math.cos(-x),Math.cos(x));near(Math.sin(-x),-Math.sin(x));}
 near(Math.sin(-Math.PI/2),-1);near(Math.cos(-Math.PI/2),0);
});
test('U06 L12 tangent ratio, zero, period and excluded poles',()=>{
 near(Math.tan(Math.PI/6),Math.sqrt(3)/3);near(Math.tan(Math.PI/3),Math.sqrt(3));
 near(Math.tan(2*Math.PI/3),-Math.sqrt(3));near(Math.tan(7*Math.PI/6),Math.sqrt(3)/3);
 for(const x of [-Math.PI/4,Math.PI/6,Math.PI/3])near(Math.tan(x+Math.PI),Math.tan(x));
 assert.ok(Math.abs(Math.cos(Math.PI/2))<1e-14,'cosine of excluded pole is numerically zero');
});
test('U06 L13 midpoint and amplitude',()=>{
 const values=[-6,4],b=(values[0]+values[1])/2,amp=(values[1]-values[0])/2;
 near(b,-1);near(amp,5);
 const g=x=>5*Math.sin(x)-1;
 near(g(Math.PI/2),4);near(g(3*Math.PI/2),-6);
});
test('U06 L14 transformed windmills midline and range',()=>{
 const cases=[{a:2.5,k:10,min:7.5,max:12.5},{a:.8,k:3,min:2.2,max:3.8},{a:-1.5,k:5,min:3.5,max:6.5}];
 for(const q of cases){near(q.k-Math.abs(q.a),q.min);near(q.k+Math.abs(q.a),q.max);}
 near(3*Math.cos(Math.PI)-5,-8);
});
test('U06 L15 equivalent trig expressions and shortened period',()=>{
 for(const x of [-Math.PI,0,Math.PI/6,Math.PI/2,Math.PI]){
  near(-Math.cos(x),Math.sin(x-Math.PI/2));
  near(Math.cos(x+Math.PI/2),Math.sin(x+Math.PI));
  near(Math.sin(2*(x+Math.PI)),Math.sin(2*x));
 }
});
test('U06 L16 actual three-second period, amplitude and midline',()=>{
 const f=t=>4*Math.sin(2*Math.PI*t/3)+15;
 for(const t of [0,.5,.75,1.5,2.25,3])near(f(t+3),f(t));
 near(f(0),15);near(f(.75),19);near(f(1.5),15);near(f(2.25),11);
 near(2*Math.PI/(2*Math.PI/3),3);
});
test('U06 L17 transformation order, phase shift and rubric',()=>{
 const h=x=>x*x-4,H=x=>-h(x+1)-3;
 near(H(-1),1);near(H(0),0);
 const g=x=>2*Math.cos((x-Math.PI/4)/2)+3;
 near(g(Math.PI/4),5);near(g(Math.PI/4+2*Math.PI),1);
 assert.match(pack.lessons.find(x=>x.lessonCode.endsWith(':U06:L17')).translations[0].commonMistakes,/information|partner|expression|periodic/i);
});
test('U06 L18 source carousel arc and angular speed',()=>{
 const w=Math.PI/5,r=20;
 near(r*w*2.5,10*Math.PI);near(w*2.5,Math.PI/2);
 near(r*7*Math.PI/6,70*Math.PI/3);
 near(195/10,19.5);near(19.5*40*Math.PI,780*Math.PI);
 near(Math.sin(Math.PI/2+.4),Math.cos(.4));
});
test('U06 L19 Moon amplitude, estimated month and nonzero residual',()=>{
 const M=d=>.5+.5*Math.cos(2*Math.PI*(d-2)/29);
 near(M(2),1);near(M(16.5),0);near(M(31),1);
 assert.ok(Math.abs(M(10)-.37)>.02,'source data not perfectly sinusoidal');
 for(const d of [1,5,12,17,27,31])assert.ok(M(d)>=0&&M(d)<=1);
});
test('all U06 lessons use revised teaching but remain runtime and academic unverified',()=>{
 for(let n=1;n<=19;n++){
  const suffix=':U06:L'+String(n).padStart(2,'0');
  const x=pack.lessons.find(q=>q.lessonCode.endsWith(suffix));
  const y=ledger.lessons.find(q=>q.lessonCode.endsWith(suffix));
  assert.ok(x&&!x.translations[0].explanation.startsWith('Learning goal: You will '),suffix);
  assert.equal(y.contentStatus,'CONTENT_EDITED_NEEDS_RUNTIME_VALIDATION');
  assert.equal(y.academicApproval,'NOT_VERIFIED');
  assert.equal(y.sourceToStudentCompared,true);
 }
 const firstUnfinished=ledger.lessons.find(x=>['CONTENT_DEFECT_TRIAGE_PENDING','EXISTING_AUTHORED_REVIEW_PENDING'].includes(x.contentStatus));
 assert.equal(ledger.nextCurriculumCursor.ordinal,firstUnfinished?firstUnfinished.ordinal:ledger.lessons.length+1);
});
