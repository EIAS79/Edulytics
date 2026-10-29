'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'../..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-hs-alg2-phase29-v1.lesson-content-pack.json');
const blueprint=read('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/us-ccss-math-hs-traditional-algebra2-im1e.lesson-blueprint.json');
const ledger=read('artifacts/curriculum-delivery/US-CCSS-MATH/coder-7/algebra2-120-lesson-progress.json');
const near=(a,b,tol=1e-7)=>assert.ok(Number.isFinite(a)&&Math.abs(a-b)<tol,String(a)+' differs from '+String(b));
const body=code=>pack.lessons.find(x=>x.lessonCode.endsWith(':'+code)).translations[0];

test('120 lesson identities, source metadata, and exact U05 cursor',()=>{
 assert.equal(blueprint.Lessons.length,120);assert.equal(pack.lessons.length,120);assert.equal(ledger.lessons.length,120);
 for(let i=0;i<120;i++){
  const b=blueprint.Lessons[i],c=pack.lessons[i],r=ledger.lessons[i];
  assert.equal(b.LessonCode,c.lessonCode);assert.equal(c.lessonCode,r.lessonCode);assert.equal(b.SourceUrl,c.sourceUrl);
  assert.equal(c.sourceSha256,r.sourceSha256);assert.equal(c.canonicalBodySha256,r.baselineCanonicalBodySha256);
 }
 assert.equal(ledger.lessons.filter(x=>x.contentStatus==='CONTENT_EDITED_NEEDS_RUNTIME_VALIDATION').length,ledger.checkedStaticCounts.sourceGroundedContentEdits);
 const firstUnfinished=ledger.lessons.find(x=>['CONTENT_DEFECT_TRIAGE_PENDING','EXISTING_AUTHORED_REVIEW_PENDING'].includes(x.contentStatus));
 assert.equal(ledger.nextCurriculumCursor.ordinal,firstUnfinished?firstUnfinished.ordinal:ledger.lessons.length+1);
 assert.equal(ledger.claims.productionDeployed,false);
});
test('U04:L16 exponential intersection and negative-time impossibility',()=>{
 const a=h=>24*Math.exp(.4*h),b=h=>9*Math.exp(.6*h);
 const h=Math.log(8/3)/.2;near(a(h),b(h));assert.ok(h>4.9&&h<4.91);
 const c=t=>16*Math.exp(.02*t),d=t=>17.5*Math.exp(.025*t);
 for(const t of [0,1,10,100])assert.ok(d(t)>c(t));
 assert.match(body('U04:L16').workedExamples,/170/);
});
test('U04:L17 inverse logarithms, growth steps and invalid arguments',()=>{
 near(Math.log2(50),5.643856189774724);near(Math.log2(80),6.321928094887363);
 near(Math.log10(20000),4.301029995663981);
 near(2**Math.log2(50),50);near(Math.log2(100)-Math.log2(50),1);
 assert.ok(!Number.isFinite(Math.log2(0)));assert.ok(Number.isNaN(Math.log2(-5)));
});
test('U04:L18 pH and source displacement logarithm table',()=>{
 const pH=h=>-Math.log10(h),R=d=>7+Math.log10(d);
 near(pH(1e-7),7);near(pH(1e-3),3);near(pH(5.6e-11),10.2518119729938);
 near((1e-3)/(1e-6),1000);near(R(1e-6),1);near(R(1e-5),2);near(R(125),9.096910013008056);
 assert.ok(Number.isNaN(pH(-1)));assert.ok(!Number.isFinite(R(0)));
});
test('U05:L01 cooling models, initial prediction and asymptote',()=>{
 const f=h=>45+20/(h+.5),g=h=>45+33*.5**(h+.5);
 near(f(0),85);near(g(0),45+33*Math.sqrt(.5));assert.ok(g(0)<f(0));
 near(f(1),45+20/1.5);near(g(1),45+33*.5**1.5);
 assert.ok(f(100)>45&&f(100)<46);assert.ok(g(100)>=45&&g(100)<46);assert.ok(g(10)>45&&g(10)<46);
});
test('U05:L02 translated zeros and vertical table changes',()=>{
 const f=x=>x*x*(x-2),v=x=>f(x)-5,h=x=>f(x-4);
 for(const x of [-2,0,2,4,6]){near(v(x),f(x)-5);near(h(x),(x-4)**2*(x-6));}
 near(h(4),0);near(h(6),0);near(-5.8+4,-1.8);assert.notEqual(h(2),f(2));
});
test('U05:L03 earlier bakery schedule and original thawing model',()=>{
 const H=x=>65+20*Math.max(0,Math.min(1,x-5,10.5-x)),G=x=>H(x+2);
 near(G(3),H(5));near(G(6),H(8));assert.notEqual(G(6),H(4));
 const M=t=>75.1-62*.85**t;near(M(0),13.1);assert.ok(M(4.92)>40&&M(4.92)<50);
});
test('source-linked new bodies use specific teaching and are not academically certified',()=>{
 for(const code of ['U04:L16','U04:L17','U04:L18','U05:L01','U05:L02','U05:L03']){
  const t=body(code);
  for(const k of ['explanation','keyConceptsAndRules','workedExamples','stepByStepSolutions','commonMistakes','quickSummary'])assert.ok(t[k].length>120,code+' '+k);
  assert.ok(!t.explanation.startsWith('Learning goal: You will '));
  const r=ledger.lessons.find(x=>x.lessonCode.endsWith(':'+code));
  assert.equal(r.academicApproval,'NOT_VERIFIED');assert.equal(r.sourceToStudentCompared,true);
  assert.match(r.studentTaskSourceUrl,/im\.kendallhunt\.com\/HS\/students\/3/);
 }
});
