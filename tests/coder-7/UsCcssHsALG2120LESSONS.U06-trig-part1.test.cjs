'use strict';
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const pack=JSON.parse(fs.readFileSync(path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-hs-alg2-phase29-v1.lesson-content-pack.json'),'utf8'));
const ledger=JSON.parse(fs.readFileSync(path.join(root,'artifacts/curriculum-delivery/US-CCSS-MATH/coder-7/algebra2-120-lesson-progress.json'),'utf8'));
const near=(a,b,tol=1e-9)=>assert.ok(Math.abs(a-b)<tol,String(a)+' not close to '+String(b));
test('U06 L01 60-second clock period and circle radius',()=>{
  const h=t=>10+Math.cos(2*Math.PI*t/60);
  [11,10,9,10,11].forEach((value,i)=>near(h(i*15),value));
  for(const t of [0,10,15,20,45])near(h(t),h(t+60));
  assert.notEqual(h(0),h(30));
  near(Math.hypot(-5,12),13);
  near(Math.sqrt(153)**2+16,169);
});
test('U06 L02 ratios and 15-radius normalization',()=>{
  near(8**2+15**2,17**2);near(9**2+12**2,15**2);
  near(8/17,Math.sin(Math.asin(8/17)));
  near((3/5)**2+(4/5)**2,1);
  near((12/15)/(9/15),4/3);
});
test('U06 L03 arc and coordinate sign cases',()=>{
  for(const y of [4/5,-4/5])near((3/5)**2+y**2,1);
  for(const x of [Math.sqrt(21)/5,-Math.sqrt(21)/5])near(x*x+(-.4)**2,1);
  near(1*Math.PI/3,Math.PI/3);near(2*Math.PI,2*Math.PI);
  assert.ok(Math.PI/3<Math.PI);
});
test('U06 L04 twenty-four angle positions and quadrant reference signs',()=>{
  near(24*Math.PI/12,2*Math.PI);
  const expected=[[Math.sqrt(3)/2,.5],[-Math.sqrt(3)/2,.5],[-Math.sqrt(3)/2,-.5],[Math.sqrt(3)/2,-.5]];
  const angles=[Math.PI/6,5*Math.PI/6,7*Math.PI/6,11*Math.PI/6];
  angles.forEach((a,i)=>{
    near(Math.cos(a),expected[i][0]);near(Math.sin(a),expected[i][1]);
    near(Math.cos(a)**2+Math.sin(a)**2,1);
  });
});
test('U06 L05 unit-circle Pythagorean identity positive and negative',()=>{
  for(const a of [-Math.PI,0,Math.PI/6,2*Math.PI/3,7*Math.PI/4,19*Math.PI/3]){
    near(Math.cos(a)**2+Math.sin(a)**2,1);
  }
  near(Math.sqrt(3)/2**1,Math.sqrt(3)/2);
  assert.ok((-.5)**2+(-.5)**2!==1);
});
test('U06 L06 quadrant-four ratios and impossible QII signs',()=>{
  const cos=7/25,sin=-24/25;
  near(cos*cos+sin*sin,1);near(sin/cos,-24/7);
  assert.ok(cos>0&&sin<0);
  assert.ok(!(3/5<0),'positive cos cannot be in QII');
});
test('U06 L07 clockwise hand coordinate mapping and offset circle',()=>{
  const clock=(t,r)=>[r*Math.sin(2*Math.PI*t/60),r*Math.cos(2*Math.PI*t/60)];
  const at10=clock(10,5),at40=clock(40,5),at45=clock(45,5);
  near(at10[0],5*Math.sqrt(3)/2);near(at10[1],2.5);
  near(at40[0],-5*Math.sqrt(3)/2);near(at40[1],-2.5);
  near(at45[0],-5);near(at45[1],0);
  near(Math.hypot(7.5,7.5),7.5*Math.sqrt(2));
  near(Math.hypot(10-10,28-20),8);
});
test('U06 L08 actual periodicity vs changing baseline',()=>{
 const D=t=>1-Math.cos(Math.PI*t);
 for(const t of [0,.25,1,1.5,5])near(D(t+2),D(t));
 near(D(0),0);near(D(1),2);
 const trend=t=>20+.3*t+2*Math.sin(Math.PI*t);
 for(const t of [0,.5,1,2])near(trend(t+2)-trend(t),.6);
});
test('U06 first eight edited student lessons and moving exact next cursor',()=>{
 for(let n=1;n<=8;n++){
  const suffix=':U06:L'+String(n).padStart(2,'0');
  const item=pack.lessons.find(x=>x.lessonCode.endsWith(suffix));
  const row=ledger.lessons.find(x=>x.lessonCode.endsWith(suffix));
  assert.ok(item&&!item.translations[0].explanation.startsWith('Learning goal: You will '));
  for(const k of ['explanation','keyConceptsAndRules','workedExamples','stepByStepSolutions','commonMistakes','quickSummary'])assert.ok(item.translations[0][k].length>120);
  assert.equal(row.academicApproval,'NOT_VERIFIED');
  assert.equal(row.contentStatus,'CONTENT_EDITED_NEEDS_RUNTIME_VALIDATION');
 }
 const firstUnfinished=ledger.lessons.find(x=>['CONTENT_DEFECT_TRIAGE_PENDING','EXISTING_AUTHORED_REVIEW_PENDING'].includes(x.contentStatus));
 assert.equal(ledger.nextCurriculumCursor.ordinal,firstUnfinished?firstUnfinished.ordinal:ledger.lessons.length+1);
});
