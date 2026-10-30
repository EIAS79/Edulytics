'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const pack=JSON.parse(fs.readFileSync(path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-hs-alg2-phase29-v1.lesson-content-pack.json'),'utf8'));
const ledger=JSON.parse(fs.readFileSync(path.join(root,'artifacts/curriculum-delivery/US-CCSS-MATH/coder-7/algebra2-120-lesson-progress.json'),'utf8'));
const near=(a,b,tol=1e-8)=>assert.ok(Number.isFinite(a)&&Math.abs(a-b)<tol,String(a)+' != '+String(b));
test('U05 L04 reflection maps input vs output and source coordinates',()=>{
 const f=x=>x*x+x;
 for(const x of [-3,-1,0,1,2,3]){near(-f(x),-x*x-x);near(f(-x),x*x-x);}
 near(-(-4.3),4.3);near((-1.5),-1.5);
 assert.notEqual(-f(2),f(-2));
});
test('U05 L05 Ferris wheel mirror and function parity',()=>{
 const H=new Map([[-80,0],[-60,31],[-40,106],[-20,181],[0,212],[20,181],[40,106],[60,31],[80,0]]);
 for(const t of [20,40,60,80])near(H.get(-t),H.get(t));
 const even=x=>x*x+1,odd=x=>x*x*x;
 for(const x of [-2,-1,0,1,2]){near(even(-x),even(x));near(odd(-x),-odd(x));}
});
test('U05 L06 parity from equations with negative mixed polynomial case',()=>{
 const odd=x=>x**5+x,even=x=>3*x**6-4*x*x+7,mixed=x=>x**3+x*x;
 for(const x of [-3,-1,0,1,2]){
  near(odd(-x),-odd(x));near(even(-x),even(x));
 }
 assert.notEqual(mixed(-2),mixed(2));assert.notEqual(mixed(-2),-mixed(2));
});
test('U05 L07 square root transformations and domain exclusion',()=>{
 const g=x=>Math.sqrt(x),p=x=>Math.sqrt(x+5)-3,q=x=>Math.sqrt(5-x)-3;
 near(g(0),0);near(p(-5),-3);near(q(5),-3);
 for(const x of [-4,-1,0,2,5])near(q(x),p(-x));
 assert.ok(Number.isNaN(q(6)));
 const parabola=x=>(x+3)**2+5;
 near(parabola(-3),5);
});
test('U05 L08 Hulme arch vertical scale preserves zeros and peak',()=>{
 const H=x=>x*(52-x),k=25/676,scaled=x=>k*H(x);
 near(H(26),676);near(scaled(0),0);near(scaled(52),0);
 near(scaled(26),25);assert.ok(k>0&&k<1);
 near(4*Math.sqrt(9),12);
});
test('U05 L09 scaling input affects time but not walking distance',()=>{
 const f=t=>t<=1?2*t:4-2*t,g=t=>f(2*t),slow=t=>f(t/2);
 near(g(0),0);near(g(.25),1);near(g(.5),2);near(g(.75),1);near(g(1),0);
 near(slow(2),2);near(slow(4),0);
 assert.notEqual(g(.75),2*f(.75));
});
test('U05 L10 previously authored function arithmetic and composition',()=>{
 const bookPerPerson=2530/309.35;
 near(bookPerPerson,8.178438661710036,1e-4);
 const f=x=>2*x+1,g=x=>3*x-4;
 near(f(g(2)),5);near(g(f(2)),11);
 assert.notEqual(f(g(2)),g(f(2)));
 assert.ok(!Number.isFinite((2*1+3)/(1-1)));
 const r=ledger.lessons.find(x=>x.lessonCode.endsWith(':U05:L10'));
 assert.equal(r.contentStatus,'CONTENT_EDITED_NEEDS_RUNTIME_VALIDATION');
});
test('U05 L11 physically bounded temperature trend vs misleading quadratic',()=>{
 const T=t=>23-15*.6**t,Q=t=>8+6*t-.8*t*t;
 near(T(0),8);near(T(1),14);near(T(2),17.6);near(T(3),19.76);
 near(Q(2),16.8);near(Q(3.75),19.25);
 assert.ok(Q(20)<8);assert.ok(T(20)>22&&T(20)<23);
});
test('new original instructional content and exact next cursor remain uncertified',()=>{
 const codes=['U05:L04','U05:L05','U05:L06','U05:L07','U05:L08','U05:L09','U05:L11'];
 for(const code of codes){
  const t=pack.lessons.find(x=>x.lessonCode.endsWith(':'+code)).translations[0];
  assert.doesNotMatch(t.explanation,/^Learning goal: You will /);
  for(const k of ['explanation','keyConceptsAndRules','workedExamples','stepByStepSolutions','commonMistakes','quickSummary'])assert.ok(t[k].length>120);
  const row=ledger.lessons.find(x=>x.lessonCode.endsWith(':'+code));
  assert.equal(row.academicApproval,'NOT_VERIFIED');assert.equal(row.sourceToStudentCompared,true);
 }
 const firstUnfinished=ledger.lessons.find(x=>['CONTENT_DEFECT_TRIAGE_PENDING','EXISTING_AUTHORED_REVIEW_PENDING'].includes(x.contentStatus));
 assert.equal(ledger.nextCurriculumCursor.ordinal,firstUnfinished?firstUnfinished.ordinal:ledger.lessons.length+1);
 assert.equal(ledger.lessons.filter(x=>x.contentStatus==='CONTENT_EDITED_NEEDS_RUNTIME_VALIDATION').length,ledger.checkedStaticCounts.sourceGroundedContentEdits);
});
