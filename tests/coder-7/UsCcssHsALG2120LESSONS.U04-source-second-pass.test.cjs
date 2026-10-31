'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const pack=JSON.parse(fs.readFileSync(path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-hs-alg2-phase29-v1.lesson-content-pack.json'),'utf8'));
const ledger=JSON.parse(fs.readFileSync(path.join(root,'artifacts/curriculum-delivery/US-CCSS-MATH/coder-7/algebra2-120-lesson-progress.json'),'utf8'));
const near=(x,y,eps=1e-6)=>assert.ok(Math.abs(x-y)<eps,String(x)+' differs from '+String(y));
test('U04 01 bank/photo and half-day geometric algae values',()=>{
 assert.deepEqual([0,1,2,3].map(n=>120+12*n),[120,132,144,156]);
 near(150*.6**3,32.4);near(100/Math.sqrt(2),70.71067811865476);
});
test('U04 02 quarter of remaining debt and calendar tuition model',()=>{
 near(160*.75**3,67.5);near(90*.25,22.5);near(30000*1.04**3,33745.92);
});
test('U04 03 exact fractional years and population',()=>{
 near(54500*1.6,87200);near(54500*1.6**2,139520);
 near(54500*Math.sqrt(1.6),68937.652984,0.02);
});
test('U04 04 decadal and annual factors equivalent',()=>{
 near(5000*1.5**(.5),5000*(1.5**(.1))**5);
 near(5000*(1.5**(.1))**10,7500);
 near(Math.sqrt(95.3*122.4),108.0,0.02);
});
test('U04 05 fractional interval multiplier and machine depreciation',()=>{
 near(4**.5,2);near(16000*.85**2,11560);
 near(16000*.85**3,9826);
 near(13600*Math.sqrt(.85),12538.58,.1);
});
test('U04 06 two points determine exponent over input gap',()=>{
 const a=3,k=2;near(a*k**1,6);near(a*k**3,24);
 near(64**(1/3),4);near(8**(2/3),4);
});
test('U04 07 radiocarbon half-life and reciprocal growth factor',()=>{
 near(3*.5**(5730/5730),1.5);near(3*.5**(11460/5730),.75);
 near(3*.5**(1910/5730),2.3811,.0002);
 near((2**(1/6))**6,2);
});
test('U04 08 fourfold tessellation and noninteger log estimate',()=>{
 assert.equal(4**9,262144);assert.equal(4**10,1048576);
 near(3**(Math.log(20)/Math.log(3)),20);
});
test('U04 09 logs are inverse of base-ten exponentiation',()=>{
 near(Math.log10(50),1.6989700043,1e-9);near(Math.log10(9),.9542425,1e-6);
 assert.ok(Number.isNaN(Math.log10(-1)));assert.equal(Math.log10(1000),3);
});
test('U04 10 equivalent base-two and base-five log statements',()=>{
 near(Math.log2(15),3.9068905956,1e-8);near(Math.log2(40),5.3219280948,1e-8);
 near(5**(Math.log(25)/Math.log(5)),25,1e-5);
 assert.equal(2**4,16);assert.equal(5**2,25);
});
test('U04 11 logs can be negative when the argument is positive',()=>{
 near(Math.log10(.1),-1);near(Math.log10(.001),-3);
 near(Math.log2(10),3.3219280949,1e-8);
 assert.ok(Math.log2(40)>Math.log2(15));
});
test('U04 12 e is a limit of (1+1/n)^n not its exact finite value',()=>{
 near((1+1/10)**10,2.59374246,1e-7);
 near((1+1/100)**100,2.7048138294,1e-7);
 assert.ok((1+1/100)**100<Math.E);
});
test('U04 13 e-based growth continuous differs from annual compounding',()=>{
 near(10*Math.exp(1.1),30.04166,1e-5);
 near(5*Math.exp(1.1*7),11041.73996,.001);
 near(400*Math.exp(-.1*5),242.6122639,.001);
 assert.ok(Math.exp(.05)>1.05);
});
test('U04 14 natural log solution substitution and positivity',()=>{
 let a=Math.log(18)/3,t=Math.log(50)/.4;
 near(5*Math.exp(3*a),90);near(2*Math.exp(.4*t),100);
 assert.ok(t>0);assert.ok(Number.isNaN(Math.log(-18)));
});
test('U04 15 graph intersection equals logarithmic time',()=>{
 const t=Math.log(30);near(Math.exp(t),30);assert.ok(t>3&&t<4);
 const doubling=Math.log(2)/.03;near(100*Math.exp(.03*doubling),200);
});
test('15 reviewed first-edition U04 bodies preserve official source identities and no approval claim',()=>{
 for(let n=1;n<=15;n++){
  const suffix=':U04:L'+String(n).padStart(2,'0');
  const lesson=pack.lessons.find(x=>x.lessonCode.endsWith(suffix));
  const row=ledger.lessons.find(x=>x.lessonCode.endsWith(suffix));
  assert.ok(lesson&&row);
  assert.equal(lesson.sourceUrl,row.teacherPreparationUrl);
  assert.equal(lesson.sourceSha256,row.sourceSha256);
  assert.equal(lesson.canonicalBodySha256,row.baselineCanonicalBodySha256);
  assert.ok(!lesson.translations[0].explanation.startsWith('Learning goal: You will '));
  assert.equal(row.academicApproval,'NOT_VERIFIED');
 }
});
