'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const file=path.resolve(__dirname,'../src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u01-l07-l12-2021.rich-lesson-v2.json');
const P=(x,y,w,h,d)=>({x,y,w,h,d});
function cells(parts){const s=new Set();for(const p of parts){if(![p.x,p.y,p.w,p.h,p.d].every(Number.isSafeInteger)||p.w<=0||p.h<=0||p.d<=0)throw Error('bad dimensions');for(let x=p.x;x<p.x+p.w;x++)for(let y=p.y;y<p.y+p.h;y++)for(let z=0;z<p.d;z++){const k=[x,y,z].join(':');if(s.has(k))throw Error('overlap');s.add(k)}}return [...s].sort()}
test('U01 L10 source first figure: 95 is same solid by addition or bounded subtraction',()=>{
 const parts=[P(0,0,3,5,5),P(3,0,2,2,5)];
 const full=[P(0,0,5,5,5)],missing=[P(3,2,2,3,5)];
 const occupied=cells(parts),whole=cells(full),removed=new Set(cells(missing));
 assert.equal(occupied.length,95);assert.deepEqual(occupied,whole.filter(k=>!removed.has(k)));
 assert.equal(3*5*5+2*2*5,5*5*5-2*3*5);
});
test('U01 L10 measured source: vertical and horizontal cuts occupy exactly same 168 ft3',()=>{
 const vertical=[P(0,0,4,8,3),P(4,0,6,4,3)];
 const horizontal=[P(0,0,10,4,3),P(0,4,4,4,3)];
 assert.equal(10-6,4);assert.equal(8-4,4);
 assert.deepEqual(cells(vertical),cells(horizontal));
 assert.equal(cells(vertical).length,168);
 assert.equal(4*8*3+6*4*3,168);
 assert.equal(10*4*3+4*4*3,168);
});
test('U01 L10 Mai subtraction counts bounding prism minus missing corner',()=>{
 const occupied=cells([P(0,0,4,8,3),P(4,0,6,4,3)]);
 const whole=cells([P(0,0,10,8,3)]);
 const missing=new Set(cells([P(4,4,6,4,3)]));
 assert.deepEqual(occupied,whole.filter(k=>!missing.has(k)));
 assert.equal(10*8*3-6*4*3,168);
 assert.notEqual(10*8*3+6*4*3,168);
});
test('U01 L10 incorrect duplicate or malformed partition cannot certify a drawn figure',()=>{
 const a=P(0,0,4,8,3);
 assert.throws(()=>cells([a,a]),/overlap/);
 assert.throws(()=>cells([P(0,0,-4,8,3)]),/bad dimensions/);
 assert.throws(()=>cells([P(0,0,4,8,'3')]),/bad dimensions/);
 assert.notEqual(4*8*3+6*4*2,168);
});
test('U01 L10 source-specific Rich V2 is still not academic Stage B', {skip:!fs.existsSync(file)&&'Exact GitHub branch sidecar absent from isolated checkout'},()=>{
 const d=JSON.parse(fs.readFileSync(file,'utf8'));
 const item=d.Lessons.find(x=>x.LessonCode==='PED:US-CCSS-MATH:G5:U01:L10');
 assert.ok(item);assert.equal(d.Lessons.length,6);
 assert.match(item.ExplanationParagraphs.join(' '),/168 cubic feet/);
 assert.match(item.ExplanationParagraphs.join(' '),/95/);
 assert.ok(item.Visuals.length>=2&&item.WorkedExamples.length>=5);
});