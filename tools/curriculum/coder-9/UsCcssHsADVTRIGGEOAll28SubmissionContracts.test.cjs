'use strict';
// Independent Coder 9 acceptance-oracle suite: all 28 authored cases are EXECUTED.
// This is NOT a test of shared runtime routing, grading, teacher persistence or HTTP.
const {test}=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../../..');
const spec=JSON.parse(fs.readFileSync(path.join(root,'artifacts/curriculum-delivery/US-CCSS-MATH/coder-9/ADV-TRIG-GEO-parameterized-practice-acceptance.v1.json'),'utf8'));
const rubrics=JSON.parse(fs.readFileSync(path.join(root,'artifacts/curriculum-delivery/US-CCSS-MATH/coder-9/ADV-TRIG-GEO-human-scoring-rubrics.v1.json'),'utf8'));
const close=(a,b)=>Number.isFinite(a)&&Number.isFinite(b)&&Math.abs(a-b)<1e-8*Math.max(1,Math.abs(b));
const bad=['','Infinity','NaN','1+0','<script>','1;2','1,2<script>'];
// This intentionally bounded vocabulary checks provided submission examples;
// it MUST NOT be mistaken for a production-safe general expression parser.
function exactToken(s){
 if(typeof s!=='string'||s.length>64||!s.trim())return null;
 let v=s.replaceAll('π','pi').replace(/\s+/g,'');
 if(v.endsWith('rad'))v=v.slice(0,-3);
 if(/^[+-]?\d+(?:\.\d+)?$/.test(v))return Number(v);
 let m=/^([+-]?)sqrt\((2|3)\)\/2$/.exec(v);
 if(m)return (m[1]==='-'?-1:1)*Math.sqrt(Number(m[2]))/2;
 m=/^([+-]?)1\/sqrt\((2|3)\)$/.exec(v);
 if(m)return (m[1]==='-'?-1:1)/Math.sqrt(Number(m[2]));
 m=/^([+-]?)(\d*)pi(?:\/(\d+))?$/.exec(v);
 if(m){const denom=Number(m[3]||1);return denom>0?(m[1]==='-'?-1:1)*Number(m[2]||1)*Math.PI/denom:null;}
 return null;
}
function variantAccepts(s,expected){
 const n=exactToken(s);
 return n!==null&&Number.isFinite(n)&&close(n,expected);
}
function volumeAnswerAccepts(answer,c){
 if(typeof answer!=='string')return false;
 const m=/^(.+) (cm|m)\^3$/.exec(answer.trim());
 return !!m && m[2]===c.unit && variantAccepts(m[1],c.volumeNumeric);
}
function conicEquivalent(c){
 const [h,k]=c.center;
 const coordinate=(letter,shift)=>shift===0?letter:'('+letter+(shift>0?'-':'+')+Math.abs(shift)+')';
 const axis=c.axis==='horizontal';
 const major=coordinate(axis?'x':'y',axis?h:k),minor=coordinate(axis?'y':'x',axis?k:h);
 return c.bSquared+major+'^2'+(c.kind==='ellipse'?'+':'-')+(c.a*c.a)+minor+'^2='+(c.a*c.a*c.bSquared);
}
function conicAnswerAccepts(answer,c){
 return typeof answer==='string' && (answer.trim()===c.canonical || answer.trim()===conicEquivalent(c));
}
function modelRoots(c,closedEnd=c.closedEnd){
 const [lo,hi]=c.interval;
 if(c.amplitude<=0||c.period<=0)throw Error('invalid physical model');
 const u=(c.target-c.offset)/c.amplitude;
 if(Math.abs(u)>1)return [];
 const alpha=Math.asin(u),results=[];
 for(let k=-4;k<=4;k++){
   for(const angle of [alpha+2*k*Math.PI,Math.PI-alpha+2*k*Math.PI]){
     const t=angle*c.period/(2*Math.PI);
     if(t<lo-1e-8||t>hi+1e-8)continue;
     if((close(t,lo)&&!c.closedStart)||(close(t,hi)&&!closedEnd))continue;
     const fixed=close(t,lo)?lo:close(t,hi)?hi:t;
     if(!results.some(x=>close(x,fixed)))results.push(fixed);
   }
 }
 return results.sort((a,b)=>a-b);
}
function parseTimes(submission){
 if(typeof submission!=='string'||submission.length>128||!submission.trim())return null;
 if(submission==='none')return [];
 const chunks=submission.split(',');
 if(chunks.length>5)return null;
 const out=[];
 for(const part of chunks){
   if(!/^[+-]?\d+(?:\.\d+)?(?:\/[+-]?\d+)?$/.test(part.trim()))return null;
   const token=part.trim().split('/');
   const n=Number(token[0]),div=token.length===2?Number(token[1]):1;
   if(!div||!Number.isFinite(n/div)||Math.abs(n/div)>1e6)return null;
   out.push(n/div);
 }
 if(out.some((v,i)=>i>0&&(v<=out[i-1]||close(v,out[i-1]))))return null;
 return out;
}
function matchesTimes(answer,c){
 const p=parseTimes(answer);
 if(p===null)return false;
 const expected=modelRoots(c);
 return p.length===expected.length&&p.every((v,i)=>close(v,expected[i]));
}
const focal=(p,q)=>Math.hypot(p[0]-q[0],p[1]-q[1]);
function checkConic(c){
 const [h,k]=c.center;
 const side=c.axis==='horizontal'?[c.c,0]:[0,c.c];
 const f1=[h-side[0],k-side[1]],f2=[h+side[0],k+side[1]];
 const testPoint=c.testPoint;
 const d1=focal(testPoint,f1),d2=focal(testPoint,f2);
 const measured=c.kind==='ellipse'?d1+d2:Math.abs(d1-d2);
 assert.ok(close(measured,c.expectedValue),c.id+' focal distance');
 const major=c.axis==='horizontal'?testPoint[0]-h:testPoint[1]-k;
 const minor=c.axis==='horizontal'?testPoint[1]-k:testPoint[0]-h;
 const calculated=c.kind==='ellipse'
    ? major**2/c.a**2+minor**2/c.bSquared
    : major**2/c.a**2-minor**2/c.bSquared;
 assert.ok(close(calculated,1),c.id+' equation');
 // Exact controlled equivalent from multiplying both sides by a²b².
 const equivalentMajor=c.bSquared*major**2, equivalentMinor=c.a**2*minor**2;
 assert.ok(close(c.kind==='ellipse'?equivalentMajor+equivalentMinor:equivalentMajor-equivalentMinor,c.a**2*c.bSquared));
}
test('contracts: exactly 28 unique cases, five explicit teacher rubrics and proposal-only runtime state',()=>{
 assert.equal(spec.lessons.length,5);
 assert.equal(spec.lessons.reduce((n,l)=>n+l.cases.length,0),28);
 assert.equal(new Set(spec.lessons.flatMap(l=>l.cases.map(c=>c.id))).size,28);
 assert.equal(spec.meta.approved,false);
 assert.equal(spec.meta.liveGeneratorRegistered,false);
 assert.equal(rubrics.status,'PROPOSED_TEACHER_REVIEW_NOT_GRADED');
 assert.equal(rubrics.rubrics.length,5);
 for(let i=0;i<5;i++){
  const row=rubrics.rubrics[i],lesson=spec.lessons[i];
  assert.equal(row.lessonCode,lesson.lessonCode);
  assert.equal(row.family,lesson.family);
  assert.equal(row.scoring,'HUMAN_REQUIRED');
  assert.ok(row.criteria.length>=3);
  assert.equal(row.criteria.reduce((n,c)=>n+c.points,0),row.maxPoints);
  assert.ok(row.criteria.every(c=>c.observableEvidence.length>20));
 }
});
for(const [i,lesson] of spec.lessons.entries()){
 for(const c of lesson.cases){
  test('independent positive, wrong and malformed oracle — '+c.id,()=>{
   assert.equal(lesson.lessonCode,'PED:US-CCSS-MATH:HS:FOURTH:ADV-TRIG-GEO:U01:L0'+(i+1));
   if(i===0){
     const theta=c.thetaPiNumerator*Math.PI/c.thetaPiDenominator;
     const angle=c.transform==='pi-theta'?Math.PI-theta:c.transform==='pi+theta'?Math.PI+theta:2*Math.PI-theta;
     if(c.expectedStatus==='OUT_OF_DOMAIN'){assert.equal(c.function,'tan');assert.ok(Math.abs(Math.cos(angle))<1e-12);}
     else{
       const expected=Math[c.function](angle);
       assert.ok(close(expected,c.valueNumeric));
       for(const variant of c.answerVariants)assert.ok(variantAccepts(variant,expected),variant);
       for(const wrong of c.incorrect)assert.equal(variantAccepts(wrong,expected),false,wrong);
     }
     for(const v of bad)assert.equal(exactToken(v),null);
   }else if(i===1){
     if(c.expectedStatus==='OUT_OF_DOMAIN'){
       assert.ok(Math.abs(c.argument)>1);
       assert.ok(Number.isNaN(Math[c.function==='arcsin'?'asin':'acos'](c.argument)));
     }else if(c.expectedStatus==='WRONG_BRANCH'){
       assert.equal(c.function,'arctan');
       assert.equal(c.candidateAngleExact,'pi/2');
       assert.equal(variantAccepts('pi/2',Math.atan(1)),false);
     }else{
       const arg=c.argumentKind==='sin-angle'?Math.sin(c.anglePiNumerator*Math.PI/c.anglePiDenominator):c.argument;
       const expected=Math[{arcsin:'asin',arccos:'acos',arctan:'atan'}[c.function]](arg);
       assert.ok(close(expected,c.valueNumeric));
       for(const variant of c.answerVariants)assert.ok(variantAccepts(variant,expected),variant);
       for(const wrong of c.incorrect)assert.equal(variantAccepts(wrong,expected),false,wrong);
       // Invalid unit convention: degrees are not silently treated as radians.
       assert.equal(exactToken('30 degrees'),null);
     }
     for(const v of bad)assert.equal(exactToken(v),null);
   }else if(i===2){
     const actual=modelRoots(c);
     assert.equal(actual.length,c.expectedSeconds.length);
     actual.forEach((v,j)=>assert.ok(close(v,c.expectedSeconds[j]),c.id+' root '+j));
     const canonical=actual.length?actual.join(','):'none';
     assert.ok(matchesTimes(canonical,c));
     for(const wrong of c.incorrectSeconds)assert.equal(matchesTimes(wrong.join(','),c),false,wrong.join(','));
     for(const v of bad)assert.equal(matchesTimes(v,c),false,v);
     // Repeated root is not a new distinct solution; extra cyclic endpoint is invalid when open.
     if(c.id==='l03-zero-boundary'){
       assert.deepEqual(modelRoots(c),[0,5]);
       assert.deepEqual(modelRoots(c,true),[0,5,10]);
       assert.equal(matchesTimes('0,5,10',c),false);
       assert.equal(matchesTimes('0,0,5',c),false);
     }
   }else if(i===3){
     if(c.expectedStatus==='OUT_OF_DOMAIN'){assert.ok(c.radius<=0);return;}
     const expected=4*Math.PI*c.radius**3/3;
     assert.ok(close(expected,c.volumeNumeric));
     assert.ok(variantAccepts(c.volumeExact,expected));
     assert.ok(variantAccepts(String(c.volumeNumeric),expected));
     const unit=c.unit+'^3';
     const complete=c.volumeExact+' '+unit;
     assert.ok(volumeAnswerAccepts(complete,c));
     assert.ok(volumeAnswerAccepts(c.volumeNumeric+' '+unit,c));
     assert.equal(volumeAnswerAccepts(c.volumeExact,c),false,'missing volume unit');
     assert.equal(volumeAnswerAccepts(c.volumeExact+' '+c.unit+'^2',c),false,'area is not volume');
     assert.equal(volumeAnswerAccepts(c.volumeExact+' '+(c.unit==='cm'?'m':'cm')+'^3',c),false,'unit mismatch');
     assert.equal(volumeAnswerAccepts('Infinity '+unit,c),false);
     assert.equal(exactToken(complete),null,'units must be checked outside expression evaluator');
     for(const z of c.comparisonHeights){
       assert.ok(z>=0&&z<=c.radius);
       assert.ok(close(Math.PI*(c.radius**2-z**2),Math.PI*c.radius**2-Math.PI*z**2));
     }
     for(const v of bad)assert.equal(exactToken(v),null);
   }else{
     if(c.expectedStatus==='OUT_OF_DOMAIN'){
       assert.equal(c.a,c.c);
       assert.equal(c.bSquared,0);
       assert.ok(!(c.kind==='ellipse'?c.a>c.c:c.c>c.a));
       return;
     }
     assert.ok(c.bSquared>0);
     assert.ok(c.kind==='ellipse'?c.a>c.c:c.c>c.a);
     checkConic(c);
     assert.match(c.canonical,/=1$/);
     assert.ok(conicAnswerAccepts(c.canonical,c));
     assert.ok(conicAnswerAccepts(conicEquivalent(c),c),'controlled multiplied polynomial equivalence');
     assert.equal(conicAnswerAccepts(c.canonical.replace('+','-'),c),c.canonical.indexOf('+')<0);
     assert.equal(conicAnswerAccepts('x^2/9+y^2/16=1',c),c.canonical==='x^2/9+y^2/16=1');
     assert.ok(!/eval|script/i.test(c.canonical));
     // Wrong-axis orientation, opposite conic sign and arbitrary input cannot be automatically accepted.
     assert.notEqual(c.kind==='ellipse'?'hyperbola':'ellipse',c.kind);
     for(const v of bad)assert.equal(conicAnswerAccepts(v,c),false);
   }
  });
 }
}
