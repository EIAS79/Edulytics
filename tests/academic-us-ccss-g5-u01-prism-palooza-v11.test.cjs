'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const sidecar=path.resolve(__dirname,'../src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u01-l07-l12-2021.rich-lesson-v2.json');
const evidence=path.resolve(__dirname,'../artifacts/curriculum-delivery/US-CCSS-MATH/G5/u01-l11-source-prisms-v11.json');
const box=(x,y,z,w,d,h,unit='ft')=>({x,y,z,w,d,h,unit});
function occupied(parts){
 if(!parts.length)throw Error('missing parts');
 const cells=new Set(), unit=parts[0].unit;
 for(const p of parts){
  if(![p.x,p.y,p.z,p.w,p.d,p.h].every(Number.isSafeInteger))throw Error('malformed dimensions');
  if([p.x,p.y,p.z].some(n=>n<0)||[p.w,p.d,p.h].some(n=>n<=0))throw Error('invalid dimensions');
  if(p.unit!==unit)throw Error('mixed units');
  for(let x=p.x;x<p.x+p.w;x++)for(let y=p.y;y<p.y+p.d;y++)for(let z=p.z;z<p.z+p.h;z++){
   const k=[x,y,z].join(':');if(cells.has(k))throw Error('overlapping volumes');cells.add(k);
  }
 }
 return cells;
}
function garden(parts){
 const cells=occupied(parts);
 if(cells.size!==216)throw Error('wrong garden volume');
 if(parts.length===2){
  const [a,b]=parts,axes=[['x','w'],['y','d'],['z','h']];
  const joined=axes.some(([coord,width])=>
   (a[coord]+a[width]===b[coord]||b[coord]+b[width]===a[coord])&&
   axes.filter(([k])=>k!==coord).every(([k,len])=>Math.min(a[k]+a[len],b[k]+b[len])>Math.max(a[k],b[k])));
  if(!joined)throw Error('disconnected garden design');
 }
 return cells;
}
test('U01:L11 Han base 10x6 with 5 complete layers is 300 cubes',()=>{
 assert.equal(10*6*5,300);assert.equal(occupied([box(0,0,0,10,6,5,'cube')]).size,300);
 assert.notEqual(10*6+5,300);assert.notEqual(10*6*4,300);
});
test('U01:L11 contextual cubic feet: storage 180 and floor-area closet 270',()=>{
 assert.equal(4*9*5,180);assert.equal(30*9,270);assert.notEqual(30+9,270);
 assert.notEqual(30*9*5,270);
});
test('U01:L11 T shaped bed 216: two face-joined parts, equal-volume straight rearrangement',()=>{
 const actual=garden([box(0,0,0,10,3,4),box(4,3,0,3,8,4)]);
 const straight=garden([box(0,0,0,18,3,4)]);
 assert.equal(actual.size,120+96);assert.equal(18*3*4,216);
 assert.notDeepEqual([...actual].sort(),[...straight].sort());
});
test('U01:L11 Noah changed lengths and still valid 216-cubic-foot connected design',()=>{
 assert.notDeepEqual([10,8],[12,6]);assert.equal(garden([box(0,0,0,12,3,4),box(5,3,0,3,6,4)]).size,216);
});
test('U01:L11 reject overlap, disconnected design, malformed dimensions, mixed units and wrong totals',()=>{
 const base=box(0,0,0,10,3,4);
 assert.throws(()=>occupied([base,base]),/overlapping/);
 assert.throws(()=>occupied([box(0,0,0,10,3,0)]),/invalid/);
 assert.throws(()=>occupied([box(0,0,0,10,'3',4)]),/malformed/);
 assert.throws(()=>occupied([base,box(4,3,0,3,8,4,'in')]),/mixed/);
 assert.throws(()=>garden([base,box(4,4,0,3,8,4)]),/disconnected/);
 assert.throws(()=>garden([base,box(4,2,0,3,8,4)]),/overlapping/);
 assert.throws(()=>garden([base,box(4,3,0,3,7,4)]),/wrong garden/);
});
test('U01:L11 actual Rich V2 teaching source fixture (isolated checkout only)',{skip:!fs.existsSync(sidecar)&&'GitHub sidecar not mounted'},()=>{
 const d=JSON.parse(fs.readFileSync(sidecar,'utf8'));const x=d.Lessons.find(z=>z.LessonCode==='PED:US-CCSS-MATH:G5:U01:L11');
 assert.equal(d.SchemaVersion,1);assert.equal(d.Lessons.length,6);assert.ok(x);
 assert.match(x.ExplanationParagraphs.join(' '),/300 unit cubes/);
 assert.match(x.ExplanationParagraphs.join(' '),/270 cubic feet/);
 assert.ok(x.WorkedExamples.some(z=>/Han/.test(z.Title)&&/300/.test(z.Answer)));
 assert.ok(x.WorkedExamples.some(z=>/Noah/.test(z.Title)&&/216/.test(z.Answer)));
 assert.ok(x.Visuals.some(z=>/garden/i.test(z.Title)&&z.Rows.length>=3));
});
test('U01:L11 exact source audit remains fail-closed for academic stage A-D',{skip:!fs.existsSync(evidence)&&'GitHub audit not mounted'},()=>{
 const x=JSON.parse(fs.readFileSync(evidence,'utf8'));
 assert.equal(x.lessonCode,'PED:US-CCSS-MATH:G5:U01:L11');
 assert.equal(x.studentTaskPdfChecked,true);assert.equal(x.gatedCooldownChecked,false);
 assert.equal(x.stageA,'NOT_COMPLETE');assert.equal(x.stageB,'NOT_VERIFIED');
 assert.equal(x.stageC,'NOT_VERIFIED');assert.equal(x.stageD,'NOT_VERIFIED');
});
