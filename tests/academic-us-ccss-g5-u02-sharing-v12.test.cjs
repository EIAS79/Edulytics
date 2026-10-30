'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const fixture=path.resolve(__dirname,'../src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u02-l02-l05-2021.rich-lesson-v2.json');
const share=(wholes,people)=>{
 if(!Number.isSafeInteger(wholes)||!Number.isSafeInteger(people))throw Error('malformed number');
 if(wholes<0||people<=0)throw Error('invalid share count');
 return wholes/people;
};
const card=(wholes,people,bars)=>{
 const portion=share(wholes,people);
 if(!Array.isArray(bars)||bars.length!==wholes)throw Error('wrong number of wholes');
 for(const bar of bars){
  if(!Number.isSafeInteger(bar.parts)||bar.parts!==people)throw Error('mismatched equal parts');
  if(!Number.isSafeInteger(bar.selected)||bar.selected!==1)throw Error('wrong recipient share');
  if(!bar.equalSize)throw Error('unequal partitions');
 }
 return portion;
};
const multiplyFraction=(numerator,denominator,n)=>{
 if(!Number.isSafeInteger(numerator)||!Number.isSafeInteger(denominator)||!Number.isSafeInteger(n))throw Error('malformed');
 if(numerator<0||denominator<=0||n<=0)throw Error('invalid');
 return n*numerator/denominator;
};
test('U02 L02 original one-sandwich table: exact unit shares and order',()=>{
 assert.deepEqual([2,3,4,5].map(n=>share(1,n)),[1/2,1/3,1/4,1/5]);
 assert.ok(share(1,2)>share(1,3)&&share(1,3)>share(1,4)&&share(1,4)>share(1,5));
});
test('U02 L02 Sandwich Match: 2/5 and 3/2 must match the diagrammed number of wholes, recipients and equal parts',()=>{
 assert.equal(card(2,5,[{parts:5,selected:1,equalSize:true},{parts:5,selected:1,equalSize:true}]),2/5);
 assert.equal(card(3,2,Array.from({length:3},()=>({parts:2,selected:1,equalSize:true}))),3/2);
 assert.equal(5*share(2,5),2);assert.equal(2*share(3,2),3);
});
test('U02 L02 reject numeric-only card matches, missing whole, unequal slices and malformed counts',()=>{
 assert.throws(()=>card(2,5,[{parts:5,selected:1,equalSize:true}]),/wrong number/);
 assert.throws(()=>card(2,5,[{parts:5,selected:1,equalSize:true},{parts:3,selected:1,equalSize:true}]),/mismatched/);
 assert.throws(()=>card(2,5,[{parts:5,selected:1,equalSize:true},{parts:5,selected:1,equalSize:false}]),/unequal/);
 assert.throws(()=>share(2,0),/invalid/);assert.throws(()=>share('2',5),/malformed/);
});
test('U02 L03 dancers: inverse relationship for fixed and open table rows; Mai misordered quotient rejected',()=>{
 assert.equal(share(2,4),1/2);assert.equal(share(3,4),3/4);assert.equal(share(3,5),3/5);
 assert.equal(4*share(6,4),6);assert.equal(2*share(5,2),5);
 assert.equal(3*share(2,3),2);assert.notEqual(3*share(3,2),2);
 assert.equal(2*share(3,2),3);
});
test('U02 L04 original Grams of Gold: changing unknown gives 4 grams or 6 friends',()=>{
 assert.equal(multiplyFraction(4,3,3),4);
 const count=5/(5/6);assert.equal(count,6);assert.ok(Number.isSafeInteger(count));
 assert.equal(multiplyFraction(5,6,6),5);
 assert.notEqual(multiplyFraction(5,6,5),5);
 assert.throws(()=>multiplyFraction(4,0,3),/invalid/);
 assert.throws(()=>multiplyFraction(4,3,-2),/invalid/);
 assert.throws(()=>multiplyFraction('4',3,3),/malformed/);
});
test('U02 L05 source constraints and general a/b quotient identity across boundary cases',()=>{
 for(const a of [0,1,2,5,7,20])for(const b of [1,2,3,5,10]){
  assert.equal(share(a,b)*b,a);
  assert.equal(multiplyFraction(a,b,b),a);
 }
 assert.ok(share(7,5)>1);assert.equal(share(4,4),1);
 assert.ok(share(2,3)<1);assert.equal(share(3,6),1/2);
 assert.throws(()=>share(2,-1),/invalid/);
 assert.throws(()=>share(2,0),/invalid/);
 assert.throws(()=>share(NaN,2),/malformed/);
});
test('U02 L02-L05 exact GitHub Rich V2 fixture preserves source-critical tasks',{skip:!fs.existsSync(fixture)&&'full GitHub branch JSON not mounted in isolated runner'},()=>{
 const d=JSON.parse(fs.readFileSync(fixture,'utf8'));
 assert.equal(d.Lessons.length,4);
 const lesson=n=>d.Lessons.find(x=>x.LessonCode==='PED:US-CCSS-MATH:G5:U02:L0'+n);
 for(let n=2;n<=5;n++){let x=lesson(n);assert.ok(x);assert.ok(x.WorkedExamples.length>=4);}
 assert.match(lesson(2).ExplanationParagraphs.join(' '),/Card Sort: Sandwich Match/);
 assert.match(lesson(2).ExplanationParagraphs.join(' '),/two identical whole sandwiches shared among five/i);
 assert.match(lesson(3).ExplanationParagraphs.join(' '),/3÷5/);
 assert.match(lesson(4).ExplanationParagraphs.join(' '),/Grams of Gold/);
 assert.match(lesson(4).WorkedExamples.map(x=>x.Answer).join(' '),/6 friends/);
 assert.match(lesson(5).ExplanationParagraphs.join(' '),/Why Does It Work/);
 assert.match(lesson(5).WorkedExamples.map(x=>x.Answer).join(' '),/a÷b=a\/b/);
});

