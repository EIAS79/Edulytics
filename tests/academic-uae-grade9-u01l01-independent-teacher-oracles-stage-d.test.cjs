'use strict';
// Independent mathematical oracles for the existing U01L01 original teacher-reviewed analogs.
// No publisher exercise is reproduced, no automatic grading/teacher review is claimed.
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const evidence=JSON.parse(fs.readFileSync(path.join(root,
 'artifacts/curriculum-delivery/UAE-MOE-MATH/G9/u01l01-original-61-parallel-skill-practice.json'),'utf8'));
const items=evidence.records;
const item=n=>items[n-1];
const ar=n=>String(n).replace(/[0-9]/g,d=>'٠١٢٣٤٥٦٧٨٩'[Number(d)]);
const includes=(n,s)=>assert.ok(item(n).teacherExpectedEvidence.includes(s),
 'Q'+n+' teacher rubric missing '+s);
const arabicToAscii=s=>s.replace(/[٠-٩]/g,ch=>String('٠١٢٣٤٥٦٧٨٩'.indexOf(ch))).replace(/−/g,'-');
const gcd=(a,b)=>b===0n?(a<0n?-a:a):gcd(b,a%b);
const rat=(n,d)=>{n=BigInt(n);d=BigInt(d);assert.notEqual(d,0n);
 if(d<0n){n=-n;d=-d}const g=gcd(n,d);return [n/g,d/g];};
const plus=(a,b)=>rat(a[0]*b[1]+b[0]*a[1],a[1]*b[1]);
const minus=(a,b)=>rat(a[0]*b[1]-b[0]*a[1],a[1]*b[1]);
const times=(a,b)=>rat(a[0]*b[0],a[1]*b[1]);
const divided=(a,b)=>rat(a[0]*b[1],a[1]*b[0]);
const asString=a=>a[0]+'/'+a[1];
test('U01L01 all 61 parallel student prompts remain teacher-only and non-publisher-certified',()=>{
 assert.equal(evidence.lessonCode,'PED:UAE-MOE-MATH:L9:GENERAL:01:01:VARIABLES-AND-EXPRESSIONS');
 assert.equal(items.length,61);
 assert.equal(evidence.fullSourceElementParityVerified,false);
 assert.equal(evidence.publishGateSatisfied,false);
 assert.deepEqual(items.map(x=>x.sourceItem),Array.from({length:61},(_,i)=>'U01L01-Q'+String(i+1).padStart(2,'0')));
 for(const x of items){
  assert.equal(x.gradingRoute,'TEACHER_REVIEW_REQUIRED');
  assert.equal(x.comparisonStatus,'ORIGINAL_ANALOGOUS_PRACTICE_PREPARED_NOT_INDEPENDENT_PARITY_CERTIFIED');
  assert.equal(typeof x.teacherExpectedEvidence,'string');
  assert.ok(x.teacherExpectedEvidence.trim().length>0,'Q'+x.sourceItem+' must have a nonempty teacher rubric; a short algebraic expression is valid');
 }
 assert.equal(items.slice(0,46).filter(x=>x.scope==='CORE_SKILL').length,46);
 assert.equal(items.slice(46).filter(x=>x.scope==='PRIOR_SKILL_REVIEW').length,15);
});
test('U01L01 independent variable-order and zero-denominator math oracles',()=>{
 assert.equal(40-35,5); assert.equal(35-40,-5);includes(20,'ت−٣٥');includes(20,'−٥');
 assert.equal(3*2+16,22);assert.equal(3*(2+16),54);includes(25,'٢٢');includes(25,'٥٤');
 assert.equal((-5)/5,2-3);includes(7,'ر=−٥');
 assert.equal(25/(5**3),1/5);includes(28,'ت≠٠');
 assert.ok(!Number.isFinite(25/0),'source exercise excludes zero denominator');
 for(const n of [-7,-1,0,1,2,5])assert.equal(4*(n*n+4)-(4*n*n+4),12);
 includes(40,'١٢');
 assert.equal(4**3,4*(4**2));assert.equal(0**3,4*(0**2));
 assert.ok(item(41).studentPrompt.includes('موجب'));
 includes(41,'س=٤');
});
test('U01L01 independent algebra, geometry, fraction-of-whole and game-score oracles',()=>{
 assert.equal(Math.PI*(3**2)*5,45*Math.PI);includes(29,'٩πع');
 assert.equal(39+0.4*20+3*2,53);includes(34,'٥٣');
 assert.equal(50*3/5,30);includes(35,'٣٠');
 const solve=total=>{
  const options=[];for(let goals=0;goals<=Math.floor(total/5);goals++)
   for(let extra=0;extra<=goals;extra++)
    if(5*goals+extra===total)options.push([goals,extra]);
  return options;
 };
 assert.deepEqual(solve(23),[[4,3]]);
 assert.deepEqual(solve(18),[[3,3]]);
 assert.deepEqual(solve(6),[[1,1]]);
 includes(36,'٥هـ+إ');
 assert.equal(10**2*10**3,10**5);
 assert.equal(10**2*10**4,10**6);includes(37,'١٠⁵');includes(37,'١٠⁶');
 assert.equal(4**3,64); assert.notEqual(4**2,64);assert.notEqual(3**4,64);includes(43,'٦٤');
 assert.equal(2*7+2*3,20);assert.notEqual(7+3,20);includes(44,'٢ل+٢ع');
 assert.equal(3*72/36,6);includes(45,'س/١٢');
 assert.equal(7*3,21);includes(46,'٢١');
});
test('U01L01 exact-rational and integer-cent arithmetic regression oracles',()=>{
 const samples=[
  [53,times(rat(2,3),rat(3,5)),'٢/٥'],
  [54,divided(rat(5,6),rat(10,9)),'٣/٤'],
  [55,times(rat(7,6),rat(9,4)),'٢١/٨'],
  [56,plus(rat(5,6),rat(7,9)),'٢٩/١٨'],
  [58,minus(rat(3,5),rat(7,3)),'−٢٦/١٥'],
  [60,plus(rat(7,8),rat(5,12)),'٣١/٢٤']
 ];
 const expected=['2/5','3/4','21/8','29/18','-26/15','31/24'];
 samples.forEach(([n,value,text],i)=>{assert.equal(asString(value),expected[i],'Q'+n);
  includes(n,text);});
 assert.equal(843-217,626);includes(57,'٦٫٢٦');
 assert.equal(1684+2759,4443);includes(59,'٤٤٫٤٣');
 assert.equal(4725-1867,2858);includes(61,'٢٨٫٥٨');
 assert.equal(9*4,36);includes(52,'٣٦');
});
test('U01L01 independent vote and signed statistics teacher-oracle checks',()=>{
 const votes=[8,5,7,11,6,9,4];
 assert.equal(Math.max(...votes),11);assert.equal(Math.min(...votes),4);
 includes(47,'الشطرنج ١١');includes(47,'الموسيقى ٤');
 const stats=nums=>{
  const s=nums.slice().sort((a,b)=>a-b);
  const counts=new Map();for(const n of s)counts.set(n,(counts.get(n)||0)+1);
  const max=Math.max(...counts.values());
  return {sum:s.reduce((a,b)=>a+b,0),mean:s.reduce((a,b)=>a+b,0)/s.length,
   median:s.length%2?s[(s.length-1)/2]:(s[s.length/2-1]+s[s.length/2])/2,
   modes:[...counts].filter(([,v])=>v===max).map(([k])=>k).sort((a,b)=>a-b)};
 };
 assert.deepEqual(stats([3,5,5,6,7,7,7,9,10,11]),{sum:70,mean:7,median:7,modes:[7]});
 assert.deepEqual(stats([-4,-2,-2,-2,0,0,0,1,4,5]),{sum:0,mean:0,median:0,modes:[-2,0]});
 assert.deepEqual(stats([4,9,10,12,14,18,18,27]),{sum:112,mean:14,median:13,modes:[18]});
 includes(49,'المتوسط ٧');includes(50,'المنوالان −٢ و٠');includes(51,'الوسيط (١٢+١٤)/٢=١٣');
});
test('U01L01 box plot uses duration mm:ss, not decimal-minute arithmetic',()=>{
 const raw=item(48).studentPrompt.match(/[٠-٩]{2}:[٠-٩]{2}/g);
 assert.equal(raw.length,14);
 const inSeconds=v=>{const [m,s]=arabicToAscii(v).split(':').map(Number);
  assert.ok(s>=0&&s<60);return m*60+s;};
 const s=raw.map(inSeconds).sort((a,b)=>a-b);
 const summary=[s[0],s[3],(s[6]+s[7])/2,s[10],s[13]];
 assert.deepEqual(summary,[860,945,1035,1150,1290]);
 assert.equal(summary[3]-summary[1],205);
 assert.equal(summary[4]-summary[0],430);
 for(const x of ['١٤:٢٠','١٥:٤٥','١٧:١٥','١٩:١٠','٢١:٣٠','٣:٢٥','٧:١٠'])
  includes(48,x);
});
