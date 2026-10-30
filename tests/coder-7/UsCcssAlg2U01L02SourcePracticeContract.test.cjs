'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'../..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const contract=read('artifacts/curriculum-delivery/US-CCSS-MATH/coder-7/U01-L02-ABC-SOURCE-PRACTICE-CONTRACT.json');
const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-hs-alg2-phase29-v1.lesson-content-pack.json');
const lesson=pack.lessons[1];
const trans=lesson.translations[0];
const all=Object.values(trans).join(' ');
const B=contract.stageB.correctExamples;
function roundCentsUnrounded(month){
  const m=BigInt(month-1), numerator=100000n*(102n**m), denominator=100n**m;
  return (2n*numerator+denominator)/(2n*denominator);
}
test('L02 First Edition goals and activities are present without rewriting source lineage fields',()=>{
 assert.equal(lesson.lessonCode,contract.lessonCode);
 assert.equal(lesson.sourceSha256,'1227711629071ad0e14885fb5fc9b1dee0d7aa0779b0bcf9ab1b3e81eec5fa13');
 assert.equal(lesson.canonicalBodySha256,'ce58b137bc455281abb466579c0b65f19879f2c329859326c42f80341f599730');
 assert.equal(contract.stageA.studentAndTeacherGoals.length,3);
 assert.equal(contract.stageA.sourceActivities.length,9);
 for(const fragment of ['256,192,144,108,81','1/7,3/7,9/7','Sierpinski','credit-card','1,-1','3/4','2.3']){
   assert.ok(all.includes(fragment),'missing source topic '+fragment);
 }
});
test('L02 paper slicing discrete pairs and area conservation independently verified',()=>{
 for(let n=0;n<=6;n++){
   const piece=2**n, area=80/piece;
   assert.equal(piece,B.paperCounts[n]);
   assert.equal(area,B.paperAreas[n]);
   assert.equal(piece*area,80);
 }
});
test('L02 geometric warm-up, completion and Practice 1/2 verified',()=>{
 const samples=[[40,120,360,1080,3240],[2,8,32,128,512],[1000,500,250,125,62.5],[256,192,144,108,81]];
 const ratios=[3,4,0.5,0.75];
 samples.forEach((seq,i)=>seq.slice(1).forEach((v,j)=>assert.equal(v/seq[j],ratios[i])));
 assert.deepEqual(B.sourceNoticeRatios,['3','4','1/2','3/4']);
 const completions=[
 {seq:[1.5,3,6,12,24,48],ratio:2},
 {seq:[40,120,360,1080,3240],ratio:3},
 {seq:[200,20,2,0.2,0.02,0.002],ratio:0.1},
 {seq:[1/7,3/7,9/7,27/7,81/7],ratio:3},
 {seq:[24,12,6,3,1.5],ratio:0.5}
 ];
 completions.forEach(o=>o.seq.slice(1).forEach((x,j)=>assert.ok(Math.abs(x-o.seq[j]*o.ratio)<1e-10)));
 assert.deepEqual(B.sourcePractice1,[8,16,32]);
 assert.deepEqual(B.sourcePractice2,['1','1/2','3','1/10','10']);
});
test('L02 finance uses exact integer cents with explicit rounding assumption',()=>{
 const cents=B.creditMonth1To8RoundedOnlyForDisplay.map(s=>BigInt(s.replace('.','')));
 assert.equal(cents.length,8);
 cents.forEach((v,i)=>assert.equal(v,roundCentsUnrounded(i+1)));
 assert.equal(cents[7],114869n);
 assert.match(contract.stageB.creditPolicyRisk,/does not prescribe/);
 assert.equal(contract.stageC.academicSignoff,'NOT_VERIFIED');
});
test('L02 Sierpinski shaded area differs from paper conservation',()=>{
 for(let n=0;n<=5;n++){
   assert.equal(B.sierpinskiCounts[n],3**n);
   assert.equal(Number(B.sierpinskiPieceAreas[n].includes('/')?1/4:B.sierpinskiPieceAreas[n]),256/(4**n));
 }
 assert.equal(B.sierpinskiCounts[5]*0.25,60.75);
 assert.notEqual(B.sierpinskiCounts[5]*0.25,256);
});
test('L02 affine recurrence and alternative rules retain human-scored gates',()=>{
 let a=10,b=1; const fromTen=[a],fromOne=[b];
 for(let i=1;i<5;i++){a=3*a-4;b=3*b-4;fromTen.push(a);fromOne.push(b);}
 assert.deepEqual(fromTen,B.affineFromTen);
 assert.deepEqual(fromOne,B.affineFromOne);
 assert.equal(contract.status.C_CLOSED,false);
 assert.equal(contract.status.D_VERIFIED,false);
 assert.equal(contract.stageC.sharedRegistryMapping,false);
});
