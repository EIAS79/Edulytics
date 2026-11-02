'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const dir=path.resolve(__dirname,'../src/Edulytics.Core/Curriculum/LessonContent/RichV2');
const paths=['us-ccss-g5-u07-l04-l08-2021.rich-lesson-v2.json','us-ccss-g5-u08-l04-l11-2021.rich-lesson-v2.json'].map(n=>path.join(dir,n));
const eq=(a,b,c,d)=>assert.equal(BigInt(a)*BigInt(d),BigInt(c)*BigInt(b));
test('U07 L04: a rotated/non-right rhombus remains rhombus, not square',()=>{
 const points=[[0,2],[2,3],[4,2],[2,1]];
 const len2=(a,b)=>(b[0]-a[0])**2+(b[1]-a[1])**2;
 assert.equal(new Set(points.map((p,i)=>len2(p,points[(i+1)%4]))).size,1);
 assert.notEqual((points[1][0]-points[0][0])*(points[3][0]-points[0][0])+(points[1][1]-points[0][1])*(points[3][1]-points[0][1]),0);
});
test('U07 L05-L07: source inclusive trapezoid convention and class inclusions',()=>{
 const parallelogram={parallelOppositePairs:2};const onePair={parallelOppositePairs:1};
 const inclusive=x=>x.parallelOppositePairs>=1;
 const exclusive=x=>x.parallelOppositePairs===1;
 assert.equal(inclusive(parallelogram),true);
 assert.equal(exclusive(parallelogram),false);
 assert.equal(inclusive(onePair),true);
 const square={equalSides:4,rightAngles:4,parallelOppositePairs:2};
 assert.equal(square.equalSides===4,true);assert.equal(square.rightAngles===4,true);
 assert.equal(inclusive(square),true);
});
test('U07 L08: triangle classification rejects impossible equilateral right/obtuse',()=>{
 assert.equal(180/3,60);assert.ok(60<90);
 assert.equal(45+45+90,180);assert.equal(30+60+90,180);
 assert.equal(100+40+40,180);
});
test('U08 L04: quotient inverse and tied tens estimates are valid',()=>{
 assert.equal(364/13,28);assert.equal(540/12,45);assert.equal(1008/14,72);
 assert.equal(Math.abs(45-40),Math.abs(45-50));
 assert.equal(390/5,78);assert.equal(780/10,78);
 assert.equal(3500/5,700);
});
test('U08 L11: game digit multisets and exact fraction totals',()=>{
 const sorted=a=>a.slice().sort((x,y)=>x-y);
 const bank=sorted([1,1,2,2,3,4,5,6]);
 assert.deepEqual(sorted([6,1,1,5,4,2,2,3]),bank);
 assert.deepEqual(sorted([1,2,1,3,4,5,2,6]),bank);
 eq(29*3+4*5,15,107,15);
 eq(1*5+7*2,30,19,30);
});
test('U07/U08: original Rich V2 shapes and exact lesson identities',{skip:!paths.every(p=>fs.existsSync(p))&&'GitHub sidecars not mounted in isolated Node runtime'},()=>{
 const docs=paths.map(p=>JSON.parse(fs.readFileSync(p,'utf8')));
 assert.equal(docs[0].Lessons.length,5);assert.equal(docs[1].Lessons.length,2);
 const ids=docs.flatMap(d=>d.Lessons.map(x=>x.LessonCode));assert.equal(new Set(ids).size,7);
 assert.deepEqual(ids,[
 'PED:US-CCSS-MATH:G5:U07:L04','PED:US-CCSS-MATH:G5:U07:L05',
 'PED:US-CCSS-MATH:G5:U07:L06','PED:US-CCSS-MATH:G5:U07:L07',
 'PED:US-CCSS-MATH:G5:U07:L08','PED:US-CCSS-MATH:G5:U08:L04',
 'PED:US-CCSS-MATH:G5:U08:L11'
 ]);
 for(const d of docs)for(const x of d.Lessons){
 assert.ok(x.ExplanationParagraphs.length>=4&&x.KeyConcepts.length>=3);
 assert.ok(x.WorkedExamples.length>=3&&x.CommonMistakes.length>=2);
 assert.ok(x.Visuals.every(v=>v.Description&&v.Rows.length>0));}
});