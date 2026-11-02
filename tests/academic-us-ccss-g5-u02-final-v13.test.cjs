'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const file=path.resolve(__dirname,'../src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u02-l14-l17-2021.rich-lesson-v2.json');
function mixed(n,whole,part,den){if(![n,whole,part,den].every(Number.isSafeInteger))throw Error('malformed');if(n<=0||whole<0||part<0||den<=0)throw Error('invalid');return {num:n*(whole*den+part),den};}
const equal=(a,b)=>a.num*b.den===b.num*a.den;
test('L14 exact source warm-up 15,15,20,20 and optional missing factor cases',()=>{
 assert.deepEqual([3*(10/2),3/2*10,14/7*10,14*(10/7)],[15,15,20,20]);
 assert.equal(18/3,6);assert.equal(21/7,3);assert.equal(2*15,30);
 assert.ok(equal(mixed(9,6,2,3),{num:60,den:1}));
 assert.ok(equal(mixed(10,14,99,100),{num:1499,den:10}));
 assert.ok(equal(mixed(6,7,3,5),{num:228,den:5}));
 assert.ok(equal(mixed(4,6,9,10),{num:138,den:5}));
});
test('L15 original bank used once for rug puzzle and whiteboard with correct unit areas',()=>{
 const bank=['4','5','11/2','3','23/4','2'];
 const filled=['3','11/2','2','5','4','23/4'];
 assert.equal(new Set(filled).size,6);assert.deepEqual([...filled].sort(),[...bank].sort());
 assert.ok(equal(mixed(3,5,1,2),{num:33,den:2}));
 assert.equal(2*(5/2),5);assert.ok(equal(mixed(4,5,3,4),{num:23,den:1}));
 assert.notEqual(new Set(['3','11/2','2','5','4','4']).size,6);
});
test('L15 original all four equivalent expressions equal 68 thirds',()=>{
 const target=mixed(4,5,2,3);assert.ok(equal(target,{num:68,den:3}));
 assert.ok(equal({num:4*5*3+4*2,den:3},target));
 assert.ok(equal({num:4*6*3-4,den:3},target));
 assert.ok(equal({num:4*17,den:3},target));
 assert.ok(equal({num:68,den:3},target));
});
test('L16 source garden 36 threshold: budget does not imply near-target acceptance',()=>{
 const areas=[mixed(9,4,2,3),mixed(9,3,8,9),mixed(12,2,11,12),mixed(9,2,2,3)];
 assert.deepEqual(areas.map(a=>a.num/a.den),[42,35,35,24]);
 assert.deepEqual(areas.map(a=>a.num<=36*a.den),[false,true,true,true]);
 assert.deepEqual(areas.map(a=>Math.abs(a.num/a.den-36)<=1),[false,true,true,false]);
});
test('L16 exact source large estimates and illustrative expression near twenty',()=>{
 assert.ok(equal(mixed(7,12,8,9),{num:812,den:9}));
 assert.ok(equal(mixed(9,4,2,29),{num:1062,den:29}));
 assert.ok(equal(mixed(6,3,2,5),{num:102,den:5}));
 assert.ok(84<812/9&&812/9<91);assert.ok(36<1062/29&&1062/29<37);
});
test('L17 original material prices $5,$3,$2 per square unit retain open geometry',()=>{
 const piece=mixed(3,1,1,2);assert.ok(equal(piece,{num:9,den:2}));
 const area=4*piece.num/piece.den;assert.equal(area,18);
 assert.deepEqual([5,3,2].map(rate=>rate*area),[90,54,36]);
 assert.notDeepEqual([5,3,2].map(rate=>rate*area),[90,90,90]);
});
test('Reject malformed, impossible geometry and disallowed zero divisions',()=>{
 assert.throws(()=>mixed(0,1,1,2),/invalid/);assert.throws(()=>mixed(3,1,1,0),/invalid/);
 assert.throws(()=>mixed(3,1,-1,2),/invalid/);assert.throws(()=>mixed(3,1,'1',2),/malformed/);
});
test('L14-L17 source fixture on real checkout',{skip:!fs.existsSync(file)?'exact branch fixtures not mounted':false},()=>{
 const d=JSON.parse(fs.readFileSync(file,'utf8'));assert.equal(d.Lessons.length,4);
 for(const n of [14,15,16,17])assert.ok(d.Lessons.find(x=>x.LessonCode==='PED:US-CCSS-MATH:G5:U02:L'+n));
 const t=d.Lessons.map(x=>x.ExplanationParagraphs.join(' ')).join(' ');
 assert.match(t,/Info Gap: Area source cards/);assert.match(t,/16 1\/2-square-foot rug/);
 assert.match(t,/36 square feet/);assert.match(t,/Cost of Mosaic/);
});
