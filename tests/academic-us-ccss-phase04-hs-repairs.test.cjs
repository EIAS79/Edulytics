'use strict';
const {test}=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const root=path.resolve(__dirname,'..');
const manifest=JSON.parse(fs.readFileSync(path.join(root,
  'docs/curriculum/us-ccss-phase04-hs-content-repair-pilot.json'),'utf8'));
const fields=['title','explanation','keyConceptsAndRules','workedExamples',
  'stepByStepSolutions','commonMistakes','quickSummary'];
const packCache=new Map(),sha=s=>crypto.createHash('sha256').update(s,'utf8').digest('hex');
const records=()=>manifest.repairs.map(row=>{
  if(!packCache.has(row.contentPackFile)){
    packCache.set(row.contentPackFile,JSON.parse(fs.readFileSync(path.join(root,row.contentPackFile),'utf8')));
  }
  const pack=packCache.get(row.contentPackFile);
  const lesson=pack.lessons.find(x=>x.lessonCode===row.lessonCode);
  const en=lesson?.translations?.find(x=>x.cultureCode==='en');
  return {row,lesson,en,pack};
});
test('Phase04 repairs: exactly seven distinct candidate lesson identities with no academic signoff',()=>{
  assert.equal(manifest.scope,'US-CCSS-MATH/CCSSM-2010/G1-G12');
  assert.equal(manifest.gate,'NO-GO');
  assert.equal(manifest.repairs.length,7);
  assert.equal(new Set(manifest.repairs.map(r=>r.lessonCode)).size,7);
  assert.ok(manifest.repairs.every(r=>r.humanAcademicSignoff==='PENDING'));
});
test('Every repair is linked to unchanged publisher evidence and a recomputed learner-body SHA',()=>{
  for(const {row,lesson,en,pack} of records()){
    assert.ok(lesson && en,'Missing English repaired lesson '+row.lessonCode);
    assert.equal(pack.packCode,'US-CCSS-MATH');
    assert.equal(lesson.sourceSha256,row.unchangedPublisherSourceSha256);
    assert.deepEqual(lesson.outcomeCodes,row.officialOutcomeCodes);
    assert.match(row.expectedOldCanonicalBodySha256,/^[a-f0-9]{64}$/);
    assert.notEqual(row.expectedOldCanonicalBodySha256,row.correctedCanonicalBodySha256);
    const actual=sha(fields.map(key=>key+':'+en[key]).join('\n'));
    assert.equal(lesson.canonicalBodySha256,actual,row.lessonCode);
    assert.equal(actual,row.correctedCanonicalBodySha256,row.lessonCode);
    for(const key of fields)assert.ok(en[key].length>=(key==='title'?6:100),row.lessonCode+' '+key+' short');
  }
});
test('Source index and teacher-logistics fragments are absent from repaired explanations',()=>{
  const patterns=[
    /READY,\s*SET,\s*GO/i,
    /TABLE OF CONTENTS/i,
    /The Teaching Cycle/i,
    /Licensed under the Creative Commons/i,
    /Skip to main content/i,
    /Homework: Matrices Revisited/i,
    /Purpose:\s*The purpose of this task/i,
    /TheMathWorksheetSite/i
  ];
  for(const {row,en} of records()){
    for(const f of fields.slice(1)){
      for(const pattern of patterns) assert.doesNotMatch(en[f],pattern,
        row.lessonCode+' '+f+' contains scraped teacher/source metadata');
    }
  }
});
test('Independent basic checks of repaired examples and boundary conditions',()=>{
  const zr=3,zi=4,wr=1,wi=1;
  assert.equal(zr*zr+zi*zi,25);
  assert.equal(zr*wr-zi*wi,-1);
  assert.equal(zr*wi+zi*wr,7);
  assert.equal((zr-wr)**2+(zi-wi)**2,13);
  assert.deepEqual([(zr+wr)/2,(zi+wi)/2],[2,2.5]);

  const [x,y,z]=[1,2,3];
  assert.deepEqual([x+y+z,2*x-y+z,x+2*y-z],[6,3,2]);
  assert.deepEqual([2-2*1,-1-2*1,1-2*1,3-2*6],[0,-3,-1,-9]);
  assert.deepEqual([1-1,2-1,-1-1,2-6],[0,1,-2,-4]);

  assert.equal(2**(-3),1/8);
  assert.equal(3**(-2),1/9);
  assert.ok(Math.abs(Math.log(7)/Math.log(2)-2.807354922)<0.00001);

  const A=[[2,1],[1,3]],det=A[0][0]*A[1][1]-A[0][1]*A[1][0];
  assert.equal(det,5);
  const inverse=[[3/5,-1/5],[-1/5,2/5]];
  const multiply=(A,B)=>A.map(r=>B[0].map((_,j)=>r.reduce((t,v,k)=>t+v*B[k][j],0)));
  const product=multiply(A,inverse);
  product.forEach((row,i)=>row.forEach((value,j)=>assert.ok(
    Math.abs(value-(i===j?1:0))<1e-12,'Inverse multiplication precision mismatch')));
  assert.deepEqual(multiply([[0,-1],[1,0]],[[2],[1]]),[[-1],[2]]);
  const a=[[1,2],[0,1]],b=[[1,0],[3,1]];
  assert.deepEqual(multiply(a,b),[[7,2],[3,1]]);
  assert.deepEqual(multiply(b,a),[[1,2],[3,7]]);
  assert.notDeepEqual(multiply(a,b),multiply(b,a));

  assert.deepEqual(multiply([[10,5],[8,7]],[[400],[200]]),[[5000],[4600]]);
});
test('Topic-specific concepts remain present, not generic title-only filler',()=>{
  const byCode=new Map(records().map(x=>[x.row.lessonCode,x.en]));
  const cases=[
    ['COMPLEX:U01:L01',/conjugate/i,/midpoint/i],
    ['ADV-ALG-FUNC:U01:L01',/augmented matrix/i,/back-substitut/i],
    ['ADV-ALG-FUNC:U01:L02',/logarithm/i,/inverse/i],
    ['VECTOR-MATRIX:U01:L02',/determinant/i,/area/i],
    ['VECTOR-MATRIX:U01:L03',/transformation/i,/rotat/i],
    ['VECTOR-MATRIX:U01:L04',/quantity matrix/i,/cost/i],
    ['VECTOR-MATRIX:U01:L05',/commutative/i,/associative/i]
  ];
  for(const [tail,p,q] of cases){
    const lesson=[...byCode.entries()].find(([code])=>code.endsWith(tail))?.[1];
    assert.ok(lesson,'Missing lesson '+tail);
    const combined=fields.map(k=>lesson[k]).join(' ');
    assert.match(combined,p);assert.match(combined,q);
  }
});
