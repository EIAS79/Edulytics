'use strict';
// Coder 9 proposal-level mathematical oracle. This tests fixtures, NOT the application's shared generator/verifier.
// Run: node --test tools/curriculum/coder-9/UsCcssHsADVTRIGGEOParameterizedPractice.test.cjs
const {test}=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../../..');
const spec=JSON.parse(fs.readFileSync(path.join(root,'artifacts/curriculum-delivery/US-CCSS-MATH/coder-9/ADV-TRIG-GEO-parameterized-practice-acceptance.v1.json'),'utf8'));
const near=(a,b,label)=>assert.ok(Number.isFinite(a)&&Number.isFinite(b)&&Math.abs(a-b)<=1e-9*Math.max(1,Math.abs(b)),label+': '+a+' != '+b);
const correctLessons=['trig.unit_circle_reflection_exact','trig.principal_inverse','trig.model_all_time_solutions','solid_geometry.cavalieri_sphere_volume','coordinates.focal_distance_conics'];
test('proposal-only status, unique canonical lessons, and diverse source-appropriate families',()=>{
 assert.equal(spec.meta.liveGeneratorRegistered,false);
 assert.equal(spec.meta.approved,false);
 assert.equal(spec.meta.exactCourseRoutingVerified,false);
 assert.equal(spec.lessons.length,5);
 assert.deepEqual(spec.lessons.map(x=>x.family),correctLessons);
 let total=0;
 for(let i=0;i<5;i++){
   const l=spec.lessons[i];
   assert.equal(l.lessonCode,'PED:US-CCSS-MATH:HS:FOURTH:ADV-TRIG-GEO:U01:L0'+(i+1));
   assert.ok(l.cases.length>=4);
   assert.equal(new Set(l.cases.map(c=>c.id)).size,l.cases.length);
   assert.ok(l.normalization.length>=3);
   assert.ok(l.humanReview.length>=2);
   total+=l.cases.length;
 }
 assert.equal(total,28);
});
function transform(theta,name){if(name==='pi-theta')return Math.PI-theta;if(name==='pi+theta')return Math.PI+theta;if(name==='2pi-theta')return 2*Math.PI-theta;throw Error(name);}
for(const c of spec.lessons[0].cases)test('L01 '+c.id,()=>{
 const theta=Math.PI*c.thetaPiNumerator/c.thetaPiDenominator;
 const angle=transform(theta,c.transform);
 if(c.expectedStatus==='OUT_OF_DOMAIN'){assert.equal(c.function,'tan');assert.ok(Math.abs(Math.cos(angle))<1e-12);return;}
 const value=Math[c.function](angle);near(value,c.valueNumeric,c.id);
 assert.ok(c.answerVariants.length>=2);
 assert.ok(c.incorrect.length>=1);
});
for(const c of spec.lessons[1].cases)test('L02 '+c.id,()=>{
 if(c.expectedStatus==='OUT_OF_DOMAIN'){
   assert.ok(Number.isNaN(Math[c.function==='arcsin'?'asin':'acos'](c.argument)));return;
 }
 if(c.expectedStatus==='WRONG_BRANCH'){
   assert.equal(c.function,'arctan');
   assert.equal(c.candidateAngleExact,'pi/2');
   const proposed=Math.PI/2;
   assert.ok(!(proposed>-Math.PI/2 && proposed<Math.PI/2),c.id);
   for(const input of [-10,-1,0,1,10]){
     const principal=Math.atan(input);
     assert.ok(principal>-Math.PI/2 && principal<Math.PI/2,c.id);
   }
   return;
 }
 const arg=c.argumentKind==='sin-angle'?Math.sin(Math.PI*c.anglePiNumerator/c.anglePiDenominator):c.argument;
 const fn={arcsin:'asin',arccos:'acos',arctan:'atan'}[c.function];
 const value=Math[fn](arg);
 near(value,c.valueNumeric,c.id);
 if(c.function==='arcsin')assert.ok(value>=-Math.PI/2&&value<=Math.PI/2);
 if(c.function==='arccos')assert.ok(value>=0&&value<=Math.PI);
 if(c.function==='arctan')assert.ok(value>-Math.PI/2&&value<Math.PI/2);
});
function signal(t,c){return c.offset+c.amplitude*Math.sin(2*Math.PI*t/c.period);}
for(const c of spec.lessons[2].cases)test('L03 '+c.id,()=>{
 const [lo,hi]=c.interval;
 assert.ok(c.amplitude>0&&c.period>0);
 if(Math.abs(c.target-c.offset)>c.amplitude){assert.equal(c.expectedSeconds.length,0);return;}
 const points=c.expectedSeconds;
 assert.deepEqual(points,[...points].sort((a,b)=>a-b));
 assert.equal(new Set(points.map(x=>x.toFixed(12))).size,points.length);
 for(const t of points){
  assert.ok(t>lo||t===lo&&c.closedStart);
  assert.ok(t<hi||t===hi&&c.closedEnd);
  near(signal(t,c),c.target,c.id);
 }
 // Exhaustively search one-period roots analytically using the two inverse sine branches.
 const fraction=(c.target-c.offset)/c.amplitude;
 const reference=Math.asin(fraction);
 const hits=[];
 for(let k=-3;k<=3;k++)for(const theta of [reference+2*k*Math.PI,Math.PI-reference+2*k*Math.PI]){
  const t=theta*c.period/(2*Math.PI);
  if((t>lo||t===lo&&c.closedStart)&&(t<hi||t===hi&&c.closedEnd)&&!hits.some(x=>Math.abs(x-t)<1e-8))hits.push(t);
 }
 hits.sort((a,b)=>a-b);
 assert.equal(hits.length,points.length,c.id);
 hits.forEach((t,i)=>near(t,points[i],c.id));
 for(const wrong of c.incorrectSeconds)assert.notDeepEqual(wrong,points);
});
for(const c of spec.lessons[3].cases)test('L04 '+c.id,()=>{
 if(c.expectedStatus==='OUT_OF_DOMAIN'){assert.ok(c.radius<=0);return;}
 assert.ok(c.radius>0);
 near(4*Math.PI*c.radius**3/3,c.volumeNumeric,c.id);
 for(const z of c.comparisonHeights){
   assert.ok(z>=0&&z<=c.radius);
   near(Math.PI*(c.radius**2-z**2),Math.PI*c.radius**2-Math.PI*z**2,c.id);
 }
});
function foci(c){
 const [h,k]=c.center;const delta=c.axis==='horizontal'?[c.c,0]:[0,c.c];
 return [[h-delta[0],k-delta[1]],[h+delta[0],k+delta[1]]];
}
function distance(p,q){return Math.hypot(p[0]-q[0],p[1]-q[1]);}
for(const c of spec.lessons[4].cases)test('L05 '+c.id,()=>{
 const a2=c.a**2,c2=c.c**2;
 if(c.expectedStatus==='OUT_OF_DOMAIN'){
  assert.equal(c.bSquared,0);assert.equal(c.a,c.c);return;
 }
 assert.ok(c.a>0&&c.c>0);
 if(c.kind==='ellipse'){assert.ok(c.a>c.c);near(c.bSquared,a2-c2,c.id);}
 if(c.kind==='hyperbola'){assert.ok(c.c>c.a);near(c.bSquared,c2-a2,c.id);}
 const [h,k]=c.center,[x,y]=c.testPoint;
 const dx=x-h,dy=y-k;
 const major=c.axis==='horizontal'?dx:dy,minor=c.axis==='horizontal'?dy:dx;
 const value=c.kind==='ellipse'?(major**2/a2+minor**2/c.bSquared):(major**2/a2-minor**2/c.bSquared);
 near(value,1,c.id);
 const [f1,f2]=foci(c),d1=distance([x,y],f1),d2=distance([x,y],f2);
 const measure=c.kind==='ellipse'?d1+d2:Math.abs(d1-d2);
 near(measure,c.expectedValue,c.id);
});
test('all Practice contracts keep negative auth and answer-parsing tests under shared owner',()=>{
 assert.match(spec.meta.unsafeExpressionPolicy,/No eval/);
 assert.match(spec.meta.persistencePolicy,/authenticated/);
 assert.match(spec.meta.persistencePolicy,/tenant/);
 assert.ok(spec.meta.questionDiversityRequirement.includes('3 distinct'));
 assert.equal(spec.issue,587);
});
