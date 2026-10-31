'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'../..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const c=read('artifacts/curriculum-delivery/US-CCSS-MATH/coder-7/U01-L03-ABC-SOURCE-PRACTICE-CONTRACT.json');
const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-hs-alg2-phase29-v1.lesson-content-pack.json');
const lesson=pack.lessons[2];
const textBody=Object.values(lesson.translations[0]).join(' ');
const b=c.stageB.correctExamples;
test('L03 local source activities and historic hashes are represented',()=>{
 assert.equal(lesson.lessonCode,c.lessonCode);
 assert.equal(lesson.sourceSha256,'ac9f54b862342e3f8a4d7de9a9906838bb9203679be912833ce4b4977c0ba4fc');
 assert.equal(lesson.canonicalBodySha256,'9c6356a716caa3dca8e2905afd8518425f9caf5ca5d6e60b64efd13d8a58a4c7');
 assert.equal(c.stageA.activities.length,10);
 for(const v of ['Remembering Function Notation','3.3','Elena','225','-1296','-0.75','Practice 7'])
  assert.ok(textBody.includes(v),'missing '+v);
});
test('Function notation f(n)=3n-7 distinguishes input from output changes',()=>{
 const f=n=>3*n-7;
 assert.deepEqual([f(10),f(10)-1,f(9),f(5)-f(4)],b.warmup);
 assert.notEqual(f(10)-1,f(9));
 assert.notEqual(f(5)-f(4),f(5-4));
});
test('Three sequences tenth-term rank and nonconsecutive table mathematics',()=>{
 const A=n=>30+10*(n-1),B=n=>5*n*(n-1)/2,C=n=>2**(n-1);
 assert.deepEqual([A(10),B(10),C(10)],[b.tenthTerms.A,b.tenthTerms.B,b.tenthTerms.C]);
 assert.ok(C(10)>B(10)&&B(10)>A(10));
 assert.equal((18-6)/(5-2),4);
 assert.deepEqual([1,2,5].map(n=>4*n-2),b.table.values);
});
test('Arithmetic and geometric source Practice arrays are verified independently',()=>{
 for(const seq of b.arithmeticPractice1){
  const delta=seq[1]-seq[0];
  for(let i=2;i<seq.length;i++)assert.ok(Math.abs(seq[i]-seq[i-1]-delta)<1e-10);
 }
 for(const seq of b.arithmeticPractice3){
  const delta=seq[1]-seq[0];
  for(let i=2;i<seq.length;i++)assert.ok(Math.abs(seq[i]-seq[i-1]-delta)<1e-10);
 }
 const ratios=[5,-6,0.5,-3,1.5];
 b.geometricPractice5.forEach((seq,i)=>seq.slice(1).forEach((x,j)=>assert.ok(Math.abs(x-seq[j]*ratios[i])<1e-10)));
 assert.deepEqual(b.practice2Classifications,['geometric','neither','arithmetic','geometric','arithmetic']);
});
test('Open source Practice 7 remains human-scored because phrase grouping ambiguous',()=>{
 const a=b.practice7AdaptedInterpretation;
 const f=x=>2*(x-3);
 function seq(x){const s=[x];while(s.length<5)s.push(f(s.at(-1)));return s;}
 assert.deepEqual(seq(0),a.start0);
 assert.deepEqual(seq(3),a.start3);
 assert.deepEqual(seq(6),a.allPositiveStart);
 assert.notEqual(f(3),2*3-3);
 assert.equal(c.status.B_CLOSED,false);
 assert.equal(c.status.C_CLOSED,false);
 assert.equal(c.stageC.sharedRegistryMapping,false);
 assert.match(c.stageB.publisherPractice7Ambiguity,/Teacher/);
});
