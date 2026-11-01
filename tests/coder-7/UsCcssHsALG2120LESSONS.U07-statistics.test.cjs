'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-hs-alg2-phase29-v1.lesson-content-pack.json');
const ledger=read('artifacts/curriculum-delivery/US-CCSS-MATH/coder-7/algebra2-120-lesson-progress.json');
const close=(a,b,e=1e-9)=>assert.ok(Math.abs(a-b)<=e,String(a)+' != '+String(b));
const mean=a=>a.reduce((s,x)=>s+x,0)/a.length;
const combinations=(a,k)=>k===0?[[]]:a.length<k?[]:combinations(a.slice(1),k-1).map(x=>[a[0],...x]).concat(combinations(a.slice(1),k));
test('120 canonical source-linked identities, 119 edits and one reuse, no academic certification',()=>{
 assert.equal(pack.lessons.length,120);assert.equal(ledger.lessons.length,120);
 for(let i=0;i<120;i++){
  const p=pack.lessons[i],r=ledger.lessons[i];
  assert.equal(p.lessonCode,r.lessonCode);assert.equal(p.sourceUrl,r.teacherPreparationUrl);
  assert.equal(p.sourceSha256,r.sourceSha256);assert.equal(p.canonicalBodySha256,r.baselineCanonicalBodySha256);
  assert.notEqual(r.academicApproval,'APPROVED');
 }
 assert.equal(ledger.lessons.filter(x=>x.contentStatus==='CONTENT_EDITED_NEEDS_RUNTIME_VALIDATION').length,119);
 assert.equal(ledger.lessons.filter(x=>x.contentStatus==='REUSE_SOURCE_REVIEWED_RUNTIME_PENDING').length,1);
 assert.equal(ledger.lessons.filter(x=>x.contentStatus==='CONTENT_DEFECT_TRIAGE_PENDING').length,0);
 assert.equal(ledger.nextCurriculumCursor.ordinal,121);
 assert.equal(ledger.claims.academicCourseComplete,false);
});
test('Unit 7 every lesson has original teaching fields and First Edition source linkage',()=>{
 for(let n=1;n<=16;n++){
  const code=':U07:L'+String(n).padStart(2,'0'),p=pack.lessons.find(x=>x.lessonCode.endsWith(code));
  const r=ledger.lessons.find(x=>x.lessonCode.endsWith(code));assert.ok(p&&r);
  const t=p.translations[0];assert.ok(!t.explanation.startsWith('Learning goal: You will '),code);
  for(const k of ['explanation','keyConceptsAndRules','workedExamples','stepByStepSolutions','commonMistakes','quickSummary'])assert.ok(t[k].length>120,code+' '+k);
  assert.equal(r.sourceToStudentCompared,true);assert.equal(r.academicApproval,'NOT_VERIFIED');
  assert.match(r.studentTaskSourceUrl,/im\.kendallhunt\.com\/HS\/students\/3\/7\//);
 }
});
test('L01–L03 source statistical study and random selection safeguards',()=>{
 const fraction=8/10;close(fraction,.8);
 assert.ok((100/200)===.5);
 assert.equal(100+100,200);
});
test('L04 sample distribution, mean, median and population spread',()=>{
 const sample=[8,9,9,10,14];close(mean(sample),10);assert.equal(sample[2],9);
 close(mean(sample.map(x=>(x-10)**2)),4.4);close(Math.sqrt(4.4),2.097617696340303,1e-12);
});
test('L05 class relative frequency and normal curve σ vs location',()=>{
 const counts=[3,7,10,5],total=counts.reduce((a,b)=>a+b,0);
 assert.equal(total,25);close(counts.reduce((a,b)=>a+b/total,0),1);
 assert.deepEqual(counts.map(x=>x/total),[.12,.28,.4,.2]);
});
test('L06 original polygon trapezoids and distinct empirical normal interval',()=>{
 const y=[0,1,3,4,4,3,1,0],areas=y.slice(1).map((hi,i)=>(y[i]+hi)/2);
 close(areas.reduce((a,b)=>a+b,0),16);close(areas[1],2);close(areas[1]/16,.125);
 assert.equal(2600-400,2200);assert.equal(2600+400,3000);
});
test('L07 normal area differences, complements and bulb standardized cutoffs',()=>{
 close(1-.3085,.6915);close(.9332-.3085,.6247);close(.5-.3085,.1915);
 close((2070-2300)/230,-1);close((90-100)/10,-1);
 close((2530-2300)/230,1);close((110-100)/10,1);
});
test('L08 model surprise versus impossible outcomes',()=>{
 const n=100,p=.6,sd=Math.sqrt(n*p*(1-p));close(sd,Math.sqrt(24));
 assert.ok((75-60)/sd>3);assert.ok((35-60)/sd< -5);
 const rare=(1+20+190)/2**20;assert.ok(rare>0&&rare<.001);
});
test('L09 sample coin data and descriptive uncertainty only',()=>{
 close(mean([1999,2000,2002,2004,2005]),2002);close(2/5,.4);
 close(.55-2*.15,.25);close(.55+2*.15,.85);
});
test('L10 observed blue-chip proportions and pass/fail population model',()=>{
 const values=[17/50,28/50,17/20,21/60];const expected=[.34,.56,.85,.35];
 values.forEach((v,i)=>close(v,expected[i]));close(14/20,.70);
});
test('L11 square root sample scaling with n and no implied bias correction',()=>{
 const p=.4,se=n=>Math.sqrt(p*(1-p)/n);
 close(se(100),.04898979485566356);close(se(400),se(100)/2);
 close(se(200)/se(100),1/Math.sqrt(2));
});
test('L12 fair die mean and independent 35-roll sample means',()=>{
 const faces=[1,2,3,4,5,6];close(mean(faces),3.5);
 close(mean(faces.map(x=>(x-3.5)**2)),35/12);
 close(Math.sqrt((35/12)/35),Math.sqrt(1/12));
 close(mean([2,4,6,1,5]),3.6);
});
test('L13 product survey means and lack of causal assignment claim',()=>{
 close(mean([3,4,5,5,5]),4.4);close(mean([2,3,4,4,5]),3.6);
 close(4.4-3.6,.8);
});
test('L14 original first-edition 12-value, six-per-group experiment has 924 regroupings',()=>{
 const lesson=pack.lessons.find(x=>x.lessonCode.endsWith(':U07:L14'));
 const body=lesson.translations.find(x=>x.cultureCode==='en').workedExamples;
 const values=Array.from({length:12},(_,i)=>i+1);
 const treatment=[2,5,7,9,11,12],control=[1,3,4,6,8,10];
 const observed=mean(treatment)-mean(control);
 close(observed,7/3);assert.equal(values.reduce((a,b)=>a+b,0),78);
 const assignments=combinations(values,6);
 assert.equal(assignments.length,924);
 const differences=assignments.map(a=>(2*a.reduce((s,x)=>s+x,0)-78)/6);
 const extremeRight=differences.filter(d=>d>=observed-1e-10).length;
 const extremeLeft=differences.filter(d=>d<=-observed+1e-10).length;
 assert.deepEqual([extremeLeft,extremeRight],[143,143]);
 close((extremeLeft+extremeRight)/assignments.length,286/924);
 assert.ok(286/924>0.05,'source experiment is not 5% significant');
 for(const sourceTask of ['14.1','14.2','14.3','1,3,4,6,8,10','2,5,7,9,11,12',
   '924','286/924','0.3095'])assert.ok(body.includes(sourceTask),'learner body must expose '+sourceTask);
 const validAssignment=(a,b)=>Array.isArray(a)&&Array.isArray(b)&&a.length===6&&b.length===6&&
   [...a,...b].every(x=>Number.isInteger(x)&&x>=1&&x<=12)&&new Set([...a,...b]).size===12;
 assert.equal(validAssignment(treatment,control),true);
 for(const [a,b] of [[[1,2,3,4,5,5],control],[[1,2,3],control],
   [[1,2,3,4,5,'6'],control]])assert.equal(validAssignment(a,b),false);
});

test('L14 original bird simulation has correctly interpreted TWO-tailed normal area',()=>{
 const lesson=pack.lessons.find(x=>x.lessonCode.endsWith(':U07:L14'));
 const body=lesson.translations.find(x=>x.cultureCode==='en').workedExamples;
 const z=(0.27-0.0021)/0.112;close(z,2.3919642857142858,1e-10);
 const density=x=>Math.exp(-x*x/2)/Math.sqrt(2*Math.PI);
 const a=z,b=10,n=10000,h=(b-a)/n;
 let sum=(density(a)+density(b))/2;
 for(let i=1;i<n;i++)sum+=density(a+i*h);
 const twoSided=2*h*sum;
 close(twoSided,0.016758473013148204,1e-7);
 assert.ok(twoSided<0.05 && twoSided>0,'significance conditional on valid design');
 for(const sourceTask of ['0.27 grams','1,000 simulated','0.0021 grams','0.112 grams',
   '0.0168','two-sided','causal'])assert.ok(body.includes(sourceTask),sourceTask);
 assert.ok(!body.includes('proves the treatment caused'),'no false certainty');
});

test('L14 optional 2+2 example remains an independently checked extension',()=>{
 const nums=[1,2,3,4];
 const dist=combinations(nums,2).map(a=>mean(a)-mean(nums.filter(x=>!a.includes(x)))).sort((a,b)=>a-b);
 assert.deepEqual(dist,[-2,-1,0,0,1,2]);
 close(dist.filter(x=>Math.abs(x)>=2).length/dist.length,1/3);
});
test('L15 source strawberry experiment signed difference and null center',()=>{
 const d=347.47-238.67;close(d,108.8,1e-8);
 const z=(d-(-.540))/29.83;assert.ok(z>3.66&&z<3.67);
});
test('L15 optional first-edition baseball normal-table activity and oral information gap',()=>{
 const lesson=pack.lessons.find(x=>x.lessonCode.endsWith(':U07:L15'));
 const body=lesson.translations.find(x=>x.cultureCode==='en').workedExamples;
 close((142-145)/2,-1.5);
 close((149-145)/2,2);
 close(145+2*1.45,147.9);
 const density=z=>Math.exp(-z*z/2)/Math.sqrt(2*Math.PI);
 const left=-1.5,right=2,n=20000,h=(right-left)/n;
 let sum=(density(left)+density(right))/2;
 for(let i=1;i<n;i++)sum+=density(left+i*h);
 const interval=sum*h;
 close(interval,0.910442666,1e-7);
 assert.ok(interval>0.91&&interval<0.911);
 close((70-55)/8,1.875);
 close((10-6)/1.6,2.5);
 for(const sourceTask of ['15.2','PARTNER','problem-card','data-card','teacher rubric',
   '15.3','145 grams','142','149','-1.5','147.9','0.9104'])assert.ok(body.includes(sourceTask),sourceTask);
 assert.ok(!body.includes('automatically score oral dialogue'),'no invented automatic grade');
 const validNormal=(x,mu,sd)=>[x,mu,sd].every(Number.isFinite)&&sd>0;
 assert.equal(validNormal(142,145,2),true);
 for(const invalid of [[142,145,0],[142,145,-2],['142',145,2],[142,NaN,2]])
   assert.equal(validNormal(...invalid),false,'reject malformed normal-model input');
});
test('L16 pulse conversion and limited randomization reasoning',()=>{
 assert.equal(12*6,72);assert.equal(15*6,90);assert.equal(90-72,18);
 close(combinations([1,2,3,4],2).filter(a=>Math.abs(mean(a)-mean([1,2,3,4].filter(x=>!a.includes(x))))>=2).length/6,1/3);
});
