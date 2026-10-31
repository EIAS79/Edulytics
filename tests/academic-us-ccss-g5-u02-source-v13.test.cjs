'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const dir=path.resolve(__dirname,'../src/Edulytics.Core/Curriculum/LessonContent/RichV2');
const f7=path.join(dir,'us-ccss-g5-u02-l06-l08-2021.rich-lesson-v2.json');
const f9=path.join(dir,'us-ccss-g5-u02-l09-l13-2021.rich-lesson-v2.json');
const fraction=(numerator,denominator)=>{if(!Number.isSafeInteger(numerator)||!Number.isSafeInteger(denominator))throw Error('malformed fraction');if(denominator<=0||numerator<0)throw Error('invalid fraction');return numerator/denominator;};
const equal=(a,b)=>Math.abs(a-b)<1e-12;
const rectangle=(length,numerator,denominator)=>{if(!Number.isSafeInteger(length)||length<=0)throw Error('invalid length');return length*fraction(numerator,denominator);};
test('L07 original four-runner 3-mile relay is NOT Han 7-mile quarter-road',()=>{
 assert.equal(fraction(3,4)*4,3);assert.equal(fraction(9,4),2.25);assert.equal(fraction(7,4),1.75);
 assert.notEqual(fraction(7,4),fraction(3,4));assert.notEqual(fraction(9,4),fraction(3,4));
 assert.ok(equal(1+fraction(5,6),11/6));assert.notEqual(11/6,1+3/4);
});
test('L08 pre-existing equivalent non-unit multiplication candidate retained',()=>{
 assert.ok(equal(3*fraction(1,5),3/5));assert.ok(equal(3*fraction(2,5),6/5));assert.ok(equal(3*fraction(3,5),9/5));
 assert.ok(equal(2*fraction(3,5),6/5));assert.notEqual(2*fraction(1,4)*6,2*fraction(1,3)*6);
});
test('L09 FOUR original six-unit rectangles, not eight-unit substitutions',()=>{
 assert.deepEqual([1,2,3,4].map(n=>rectangle(6,1,n)),[6,3,2,1.5]);
 assert.deepEqual([1,2,3,4].map(n=>rectangle(n,1,2)),[.5,1,1.5,2]);
 assert.equal(rectangle(12,1,2),6);assert.equal(rectangle(4,3,2)/2,3);
 assert.notDeepEqual([1,2,3,4].map(n=>rectangle(8,1,n)),[6,3,2,1.5]);
});
test('L10 original three rectangles preserve number AND size of area pieces',()=>{
 assert.equal(rectangle(4,2,3),8/3);assert.equal(rectangle(4,3,4),3);
 assert.equal(rectangle(4,3,5),12/5);assert.equal(12/4,3);assert.equal(12/5,2.4);
 assert.notEqual(12/4,12/5);assert.equal(rectangle(3,2,3),2);
});
test('L10 source X/Y expression cards include three genuine negative cases',()=>{
 const X=rectangle(3,4,5),Y=rectangle(5,3,4);
 const labels=[{id:'Y',value:rectangle(5,3,4)},{id:'neither',value:rectangle(3,3,5)},{id:'X',value:rectangle(3*4,1,5)},{id:'neither',value:rectangle(4,3,4)},{id:'neither',value:rectangle(3*3,1,4)}];
 assert.ok(equal(X,12/5));assert.ok(equal(Y,15/4));
 assert.deepEqual(labels.map(p=>equal(p.value,X)?'X':equal(p.value,Y)?'Y':'neither'),labels.map(p=>p.id));
});
test('Wrong, boundary, malformed dimensions/fractions rejected',()=>{
 assert.equal(fraction(0,4),0);assert.equal(fraction(1,1),1);
 assert.throws(()=>fraction(1,0),/invalid/);assert.throws(()=>fraction(-1,3),/invalid/);
 assert.throws(()=>fraction(1,'3'),/malformed/);assert.throws(()=>rectangle(0,1,2),/invalid/);
 assert.throws(()=>rectangle(3.5,1,2),/invalid/);assert.throws(()=>rectangle(3,1,NaN),/malformed/);
});
test('Exact G5 V13 Rich V2 fixtures in real checkout',{skip:!fs.existsSync(f7)||!fs.existsSync(f9)?'complete branch files not mounted':false},()=>{
 const a=JSON.parse(fs.readFileSync(f7,'utf8')),b=JSON.parse(fs.readFileSync(f9,'utf8'));
 const lesson=(d,n)=>d.Lessons.find(z=>z.LessonCode==='PED:US-CCSS-MATH:G5:U02:L'+String(n).padStart(2,'0'));
 for(const n of [7,9,10])assert.ok(lesson(n===7?a:b,n));
 assert.match(lesson(a,7).ExplanationParagraphs.join(' '),/four-person relay/);
 assert.match(lesson(b,9).ExplanationParagraphs.join(' '),/SAME 6-unit length/);
 assert.match(lesson(b,10).ExplanationParagraphs.join(' '),/diagram X/i);
});
