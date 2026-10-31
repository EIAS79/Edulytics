'use strict';
// Course-scoped first-party mathematical/source-task checks for ADV-TRIG-GEO.
// This is an independent oracle, NOT an authenticated Practice route, grader or academic approval.
const {test}=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../../..');
const pack=JSON.parse(fs.readFileSync(path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-hs-adv-trig-geo-phase29-v1.lesson-content-pack.json'),'utf8'));
const body=i=>Object.values(pack.lessons[i].translations[0]).join(' ');
const near=(a,b,eps=1e-9)=>assert.ok(Number.isFinite(a)&&Number.isFinite(b)&&Math.abs(a-b)<eps,`${a} != ${b}`);

test('L02 original 7.8H: three location outcomes, principal inverse, 0.75 repeated roots',()=>{
 const b=body(1);
 assert.match(b,/three independent first-party game rounds/);
 assert.match(b,/Original 0\.75 inverse-sine graph investigation/);
 const alpha=Math.asin(.75);
 assert.ok(alpha>=-Math.PI/2&&alpha<=Math.PI/2);
 for(const angle of [alpha,Math.PI-alpha,alpha+2*Math.PI,3*Math.PI-alpha])near(Math.sin(angle),.75);
 near(2*Math.cos(5*Math.PI/6),-Math.sqrt(3)); near(2*Math.sin(5*Math.PI/6),1);
 near(2*Math.cos(Math.PI/6),Math.sqrt(3)); near(2*Math.sin(Math.PI/6),1);
 assert.ok(.5>0&&-1<0,'positive-radius clue with negative y is inconsistent');
 near(Math.asin(-.5),-Math.PI/6);near(Math.acos(-.5),2*Math.PI/3);near(Math.atan(1),Math.PI/4);
 assert.ok(Number.isNaN(Math.asin(1.2)));assert.ok(Number.isNaN(Math.acos(-1.2)));
 assert.ok(Math.atan(1)<Math.PI/2);
});

test('L03 source identities, tan domain and four angle roots',()=>{
 const s=body(2);assert.match(s,/Original 7\.6 identity-based practice/);
 for(const q of [Math.PI/3,2*Math.PI/3]){near(Math.sin(-q),-Math.sqrt(3)/2);near(Math.cos(q)*Math.tan(q),Math.sqrt(3)/2);}
 for(const p of [Math.PI/2,3*Math.PI/2])assert.ok(Math.abs(Math.cos(p))<1e-10);
 const roots=[Math.PI/6,Math.PI/3,7*Math.PI/6,4*Math.PI/3];
 roots.forEach(q=>near(Math.sin(2*q),Math.sqrt(3)/2));
 assert.equal(new Set(roots.map(x=>x.toFixed(10))).size,4);
 assert.ok(roots.every(x=>x>0&&x<2*Math.PI));
});

test('L04 source triangles, parallelograms, oblique pyramid and hemisphere slices',()=>{
 const s=body(3);assert.match(s,/Source 7\.12 task-representation investigation/);
 for(const z of [0,.5,1,2,3.5,4])near(6*(1-z/4),6*(4-z)/4);
 near(6*4/2,12);near(6*4,24);
 for(const z of [0,1,3,5.5,6])near(12*(1-z/6)**2,12*((6-z)/6)**2);
 near(12*6/3,24);
 const r=2.5;for(const z of [0,.5,1.25,2.5])near(Math.PI*(r*r-z*z),Math.PI*r*r-Math.PI*z*z);
 near(4*Math.PI*r**3/3,125*Math.PI/6);
});

test('L05 original three closed string loops, focal point checks and separation',()=>{
 const s=body(4);assert.match(s,/Source 6\.10H closed-loop experiment/);
 for(const [len,bSq] of [[10,5],[12,12],[14,21]]){const c=2,a=(len-4)/2;near(a*a-c*c,bSq);assert.ok(a>c);}
 const d=(x,y,fx)=>Math.hypot(x-fx,y);
 near(d(5,0,-2)+d(5,0,2),10);
 near(d(0,Math.sqrt(21),-2)+d(0,Math.sqrt(21),2),10);
 assert.ok(d(0,6,-2)+d(0,6,2)>10);
 near(((14-6)/2)**2-(6/2)**2,7);
 assert.match(s,/not an original publisher 6\.10H activity/);
});
