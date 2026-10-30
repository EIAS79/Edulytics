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
