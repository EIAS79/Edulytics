'use strict';
const {test}=require('node:test');
const assert=require('node:assert/strict');
const {readFileSync,readdirSync}=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'..','..');
const read=(file)=>JSON.parse(readFileSync(path.join(root,file),'utf8'));
const source=read('artifacts/curriculum-delivery/US-CCSS-MATH/GEO/geo-124-source-manifest-20261028.json');
const base='src/Edulytics.Core/Curriculum/LessonContent/RichV2';
const lessons=readdirSync(path.join(root,base)).filter(n=>/^us-ccss-hs-geo-.+\.rich-lesson-v2\.json$/.test(n)).flatMap(n=>read(base+'/'+n).Lessons);
const byCode=new Map(lessons.map(x=>[x.LessonCode,x]));
const id=(u,n)=>'PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U'+String(u).padStart(2,'0')+':L'+String(n).padStart(2,'0');
const near=(a,b,e=1e-12)=>Math.abs(a-b)<e;

test('V12 source-ledger accounts for all 33 syllabus-only and six teacher-only new reviews without academic promotions',()=>{
  assert.equal(source.lessons.length,124);
  assert.equal(byCode.size,124);
  const reviewed=source.lessons.filter(x=>x.stageA==='SOURCE_TEACHER_STUDENT_ACTIVITIES_READ_PARITY_UNVERIFIED');
  const initial=source.lessons.filter(x=>x.stageA==='SOURCE_TEACHER_STUDENT_COMPARE_INITIAL');
  assert.equal(reviewed.length,39);
  assert.equal(initial.length,85);
  assert.equal(source.lessons.filter(x=>/PENDING|PREPARATION_ONLY/.test(x.stageA)).length,0);
  assert.equal(source.totals.gradeAccepted,0);
  assert.equal(source.totals.sourceTaskPagesReadInV12,39);
  for(const x of reviewed) {
    assert.ok(x.sourceResearch.teacherPreparationUrl.endsWith('/preparation.html'),x.lessonCode);
    assert.ok(x.sourceResearch.studentLessonUrl.endsWith('/index.html'),x.lessonCode);
    assert.ok(x.sourceResearch.sourceTaskObserved?.length>18,x.lessonCode);
    assert.equal(x.sourceResearch.academicSignoff,false,x.lessonCode);
    assert.equal(x.stageB,'RICHV2_CANDIDATE_AUTHORED',x.lessonCode);
    assert.equal(x.stageC,'NOT_VERIFIED',x.lessonCode);
    assert.equal(x.stageD,'NOT_RUN',x.lessonCode);
  }
  assert.ok(source.lessons.find(x=>x.lessonCode===id(8,1)).sourceOptional);
});

test('U06 L03: independent dilation and similar-triangle oracle',()=>{
  const lesson=byCode.get(id(6,3));
  assert.ok(lesson.WorkedExamples[1].Question.includes('B=(2,3)'));
  assert.ok(lesson.WorkedExamples[1].Answer.includes('PB:PB′=1:3'));
  const P=[0,0],B=[2,3],Bprime=[6,9],M=[2,0],N=[6,0];
  const dist=(a,b)=>Math.hypot(a[0]-b[0],a[1]-b[1]);
  assert.ok(near(dist(P,Bprime)/dist(P,B),3));
  assert.ok(near(dist(P,N)/dist(P,M),3));
  assert.ok(near(dist(N,Bprime)/dist(M,B),3));
  assert.ok(near((Bprime[0]-P[0])*(B[1]-P[1])-(Bprime[1]-P[1])*(B[0]-P[0]),0));
});

test('U07 L02: inscribed angles and crossing chord triangles match by AA',()=>{
  const x=byCode.get(id(7,2));
  const example=x.WorkedExamples[2];
  assert.match(example.Question,/Chords CB and DE intersect/);
  assert.match(example.Answer,/similar by AA/);
  const arcDB=140,arcCE=100;
  const angleC=arcDB/2,angleE=arcDB/2,angleD=arcCE/2,angleB=arcCE/2;
  assert.equal(angleC,angleE);
  assert.equal(angleD,angleB);
  assert.equal(180-angleC-angleD,60);
  assert.equal(angleC+angleD+60,180);
  assert.ok(example.Steps[2].includes('triangle CFD is similar to triangle EFB'));
});

test('U07 L12: original double number line and sector shading preserve fraction',()=>{
  const x=byCode.get(id(7,12)),ex=x.WorkedExamples[2];
  assert.ok(ex.Question.includes('5π/6 radians'));
  assert.ok(ex.Answer.includes('5/12 of a full circle'));
  assert.ok(ex.Steps.some(s=>s.includes('double number line')));
  const radians=5*Math.PI/6;
  assert.ok(near(radians/(2*Math.PI),5/12));
  assert.ok(near(radians*180/Math.PI,150));
  assert.ok(radians>Math.PI/2 && radians<Math.PI);
});

test('U08 L04: independent ordered dice sample-space and conditional table oracles',()=>{
  const lesson=byCode.get(id(8,4));
  const example=lesson.WorkedExamples[0];
  assert.ok(/two distinct fair six-sided dice/i.test(example.Question));
  const sample=Array.from({length:6},(_,i)=>i+1)
   .flatMap(a=>Array.from({length:6},(_,j)=>[a,j+1]));
  const equal=sample.filter(([a,b])=>a===b);
  assert.equal(sample.length,36);
  assert.equal(equal.length,6);
  assert.ok(near(equal.length/sample.length,1/6));
  assert.ok(example.Answer.includes('1/6'));
  const joint=12,total=50,row=20,column=30;
  assert.ok(near(joint/total,0.24));
  assert.ok(near(joint/row,0.6));
  assert.ok(near(joint/column,0.4));
  assert.notEqual(joint/row,joint/column);
});

test('U07 L06: independent incenter equal-perpendicular-distance oracle',()=>{
  const lesson=byCode.get(id(7,6)),ex=lesson.WorkedExamples[2];
  assert.ok(ex.Question.includes('internal bisectors'));
  assert.ok(ex.Answer.includes('incenter'));
  // Right triangle A=(0,0),B=(3,0),C=(0,4), incenter I=(1,1).
  const I=[1,1],distAB=Math.abs(I[1]),distAC=Math.abs(I[0]);
  const distBC=Math.abs(4*I[0]+3*I[1]-12)/5;
  assert.ok(near(distAB,1)&&near(distAC,1)&&near(distBC,1));
  assert.ok(ex.Steps[2].includes('CI is the third bisector'));
});

test('U08 L02: independent physical-slip model versus hypothetical observed frequency',()=>{
  const lesson=byCode.get(id(8,2)),ex=lesson.WorkedExamples[1];
  assert.ok(ex.Question.includes('20 draws'));
  const slips=['Ana','Ana','Ben'];
  const pAna=slips.filter(s=>s==='Ana').length/slips.length;
  const observed=15/20;
  assert.ok(near(pAna,2/3));
  assert.ok(near(observed,.75));
  assert.notEqual(observed,pAna);
  assert.ok(ex.Answer.includes('2/3')&&ex.Answer.includes('0.75'));
  assert.ok(ex.Steps[2].includes('hypothetical'));
});

test('U07 L10 retains source progression: the radian name first appears in following lesson',()=>{
  const early=byCode.get(id(7,10)),next=byCode.get(id(7,11));
  assert.ok(early.KeyConcepts[2].Rule.includes('next lesson'));
  assert.ok(!early.KeyConcepts[2].Definition.includes('defines radians'));
  assert.ok(next.KeyConcepts.some(c=>c.Title==='Radian'));
  const theta=Math.PI/2, r1=3,r2=8;
  assert.ok(near(r1*theta/r1,r2*theta/r2));
});
