'use strict';
const {test}=require('node:test');
const assert=require('node:assert/strict');
const {readFileSync,readdirSync}=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'..','..'),dir='src/Edulytics.Core/Curriculum/LessonContent/RichV2';
const lessons=readdirSync(path.join(root,dir)).filter(n=>/^us-ccss-hs-geo-.*\.rich-lesson-v2\.json$/.test(n))
  .flatMap(n=>JSON.parse(readFileSync(path.join(root,dir,n),'utf8')).Lessons);
const byCode=new Map(lessons.map(x=>[x.LessonCode,x]));
const id=(u,l)=>'PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U'+String(u).padStart(2,'0')+':L'+String(l).padStart(2,'0');
const near=(a,b)=>Math.abs(a-b)<1e-10*Math.max(1,Math.abs(a),Math.abs(b));
const p=(x,y)=>Math.hypot(x,y);
const lerp=(a,b,t)=>a+(b-a)*t;

test('V13 independently computes 27 named Geometry worked-example results without certifying the other 345',()=>{
 const certified=[];
 const verify=(u,l,i,actual,expected,answerRE)=>{
   const code=id(u,l),ex=byCode.get(code)?.WorkedExamples?.[i];
   assert.ok(ex,code+'#'+i);
   assert.ok(Number.isFinite(actual)&&near(actual,expected),code+'#'+i+' independent numerical check');
   assert.match(ex.Answer,answerRE,code+'#'+i+' answer');
   certified.push(code+'#'+i);
 };
 // U05 L11: base area, normal edge component and subtraction of a void.
 verify(5,11,0,0.5*3*4*9,54,/54 cubic units/);
 verify(5,11,1,14*10*Math.sin(Math.PI/6),70,/70 cm/);
 verify(5,11,2,8*6*5-2*2*5,220,/220 cubic units/);
 // U06 L07: focus/directrix x^2=4py with p=2 and independent point and vertex checks.
 verify(6,7,0,4*(2-(-2))/2,8,/x²=8y/);
 verify(6,7,1,p(4,2-2)-Math.abs(2-(-2)),0,/Yes/);
 verify(6,7,2,(5+1)/2,3,/Vertex \(3,3\)/);
 // U06 L15: 2:3 and t=1/3 partitions, one common centre of dilation.
 verify(6,15,0,lerp(0,10,2/5),4,/P=\(4,2\)/);
 verify(6,15,1,lerp(4,-2,1/3),2,/P=\(4,2\)/);
 const A=[2,3],B=[6,3],C=[8,9],D=[4,9];
 const image=[B,C,D].map(v=>v.map((n,i)=>lerp(A[i],n,1/5)));
 assert.deepEqual(image,[[14/5,3],[16/5,21/5],[12/5,21/5]]);
 verify(6,15,2,p(image[0][0]-A[0],image[0][1]-A[1])/p(B[0]-A[0],B[1]-A[1]),1/5,/centre A and factor 1\/5/);
 // U07 L02: half-arc theorems; separate open-ended proof slot not counted.
 verify(7,2,0,120/2,60,/60°/);
 verify(7,2,1,70/2,35,/35° each/);
 // U07 L06: inradius r=area/semiperimeter.
 verify(7,6,0,(3*4/2)/((3+4+5)/2),1,/Inradius 1/);
 // U07 L12: degrees, radians and full-turn fraction.
 verify(7,12,0,(3*Math.PI/4)*(180/Math.PI),135,/135°/);
 verify(7,12,1,(225*Math.PI/180)/Math.PI,5/4,/5π\/4/);
 verify(7,12,2,(5*Math.PI/6)/(2*Math.PI),5/12,/5\/12 of a full circle/);
 // U07 L13: arc, sector and source-required slope of graph, all calculated independently.
 verify(7,13,0,(4*Math.PI/3)/Math.PI,4/3,/4π\/3/);
 verify(7,13,1,(0.5*6**2*Math.PI/2)/Math.PI,9,/9π/);
 const pairs=[[2,3],[6,9],[11,16.5],[14,21]];
 pairs.forEach(([r,s])=>assert.ok(near(s/r,1.5)));
 assert.ok(near((9-3)/(6-2),1.5));
 verify(7,13,2,(21/14)*20,30,/arc length 30 m/);
 assert.match(byCode.get(id(7,13)).WorkedExamples[2].Question,/radius 20 m/);
 // U08 L04: all 36 ordered pairs, joint 12/50 and row-conditioned 12/20.
 const dice=Array.from({length:6},(_,i)=>i+1).flatMap(a=>Array.from({length:6},(_,j)=>[a,j+1]));
 verify(8,4,0,dice.filter(([a,b])=>a===b).length/dice.length,1/6,/matching probability 1\/6/);
 verify(8,4,1,12/50,0.24,/0\.24/);
 verify(8,4,2,12/(12+8),0.6,/0\.6/);
 // U08 L08: defined conditional denominator, multiplication rule and restricted die space.
 verify(8,8,0,0.2/0.4,0.5,/0\.5/);
 verify(8,8,1,0.5*0.3,0.15,/0\.15/);
 verify(8,8,2,[4,5,6].filter(v=>v%2===0).length/3,2/3,/2\/3/);
 // U08 L11: net expected value, no replacement, and actual observable token-side sample.
 verify(8,11,0,2*(1/3)-1*(2/3),0,/Fair in expected net gain/);
 verify(8,11,1,(2-1)/(3-1),0.5,/=1\/2/);
 const tokens=[['blank','blank'],['star','blank'],['star','star']];
 const observable=tokens.flatMap((faces,i)=>faces.map(face=>({i,face}))).filter(t=>t.face==='star');
 verify(8,11,2,observable.filter(t=>t.i===2).length/observable.length,2/3,/^2\/3/);
 assert.equal(lessons.length,124);
 assert.equal(certified.length,27);
 assert.equal(new Set(certified).size,27);
});
test('V13 original arc-length/radius graph SVG has accessible source data and correct plot geometry',()=>{
 const svg=readFileSync(path.join(root,'src/Edulytics.Web/wwwroot/geometry/geo/v13/u07-l13-arc-length-radius-linear-graph.svg'),'utf8');
 assert.match(svg,/role="img"/);
 assert.match(svg,/<title id="arc-graph-title">/);
 assert.match(svg,/<desc id="arc-graph-description">/);
 assert.match(svg,/aria-labelledby="arc-graph-title arc-graph-description"/);
 for(const [r,s,x,y] of [[2,3,154,414],[6,9,262,342],[11,16.5,397,252],[14,21,478,198],[20,30,640,90]]){
   assert.ok(near(s,1.5*r));
   assert.equal(x,100+27*r);
   assert.equal(y,450-12*s);
   assert.ok(svg.includes('cx="'+x+'" cy="'+y+'"'));
 }
 assert.ok(!/<(?:script|iframe|foreignObject|image)\b/i.test(svg));
 assert.ok(!/\bon[a-z]+\s*=|\b(?:href|xlink:href)\s*=|@import|url\(/i.test(svg));
});

test('V13 added quadrant SVG encodes one common dilation and both new static figures remain unlinked',()=>{
 const meta=JSON.parse(readFileSync(path.join(root,'artifacts/curriculum-delivery/US-CCSS-MATH/GEO/geo-v13-two-first-party-svg-candidates-20261029.json'),'utf8'));
 assert.equal(meta.Assets.length,2);
 assert.equal(meta.Status,'COURSE_LOCAL_STATIC_ASSETS_UNLINKED_NOT_AUTHENTICATED_RENDITION');
 for(const asset of meta.Assets){
  assert.equal(asset.RendererVerified,false);
  assert.equal(asset.SourceFigureIdentical,false);
  const svg=readFileSync(path.join(root,asset.Path),'utf8');
  assert.match(svg,/role="img"/);
  assert.match(svg,/<title id="[^"]+">/);
  assert.match(svg,/<desc id="[^"]+">/);
  assert.ok(!/<(?:script|iframe|foreignObject|image)\b/i.test(svg));
  assert.ok(!/\bon[a-z]+\s*=|\b(?:href|xlink:href)\s*=|@import|url\(/i.test(svg));
 }
 const asset=meta.Assets.find(x=>x.LessonCode.endsWith('U06:L15'));
 assert.ok(asset);
 const v=asset.MathematicalFixture,A=v.A,sc=v.factor;
 for(const [originalKey,imageKey] of [['B','BPrime'],['C','CPrime'],['D','DPrime']]){
  const original=v[originalKey],image=v[imageKey];
  for(let i=0;i<2;i++)assert.ok(near(lerp(A[i],original[i],sc),image[i]));
 }
 const svg=readFileSync(path.join(root,asset.Path),'utf8');
 assert.ok(svg.includes('points="190,355 470,355 610,85 330,85"'));
 assert.ok(svg.includes('points="190,355 246,355 274,301 218,301"'));
 // Screen-space x=50+70x and y=490-45y for each coordinate vertex.
 for(const coord of [A,v.B,v.C,v.D,v.BPrime,v.CPrime,v.DPrime]){
   const x=50+70*coord[0],y=490-45*coord[1];
   assert.ok(near(x,Math.round(x))&&near(y,Math.round(y)));
 }
});
test('V13 four source re-audits disclose actual activity gaps and explicitly forbid academic signoff',()=>{
 const record=JSON.parse(readFileSync(path.join(root,'artifacts/curriculum-delivery/US-CCSS-MATH/GEO/geo-124-source-manifest-20261028.json'),'utf8'));
 assert.equal(record.lessons.length,124);
 assert.equal(record.totals.v13SourceTasksReopened,4);
 assert.equal(record.totals.v13IndependentlyRecomputedExampleSlots,27);
 assert.equal(record.totals.v13WorkedExamplesNotIndependentlyCertified,345);
 for(const short of ['U01:L01','U05:L11','U06:L15','U07:L13']){
  const row=record.lessons.find(x=>x.lessonCode.endsWith(short));
  assert.ok(row,short);
  assert.equal(row.sourceResearch.academicSignoff,false);
  assert.equal(row.sourceResearch.v13IndependentReaudit.sourceTaskAcceptance,'NOT_VERIFIED');
  assert.equal(row.sourceResearch.v13IndependentReaudit.protectedTeacherCooldown,'NOT_INSPECTED');
  assert.ok(row.sourceResearch.v13IndependentReaudit.studentActivitiesRead.length>=3);
  assert.ok(row.sourceResearch.v13IndependentReaudit.observedParityGap.length>50);
 }
 assert.equal(record.totals.gradeAccepted,0);
});
