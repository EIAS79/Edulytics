'use strict';
const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const root = path.resolve(__dirname,'..');
const base = 'artifacts/curriculum-delivery/UAE-MOE-MATH/G9/';
const read = name => JSON.parse(fs.readFileSync(path.join(root,base,name),'utf8'));
const records = read('u01l01-original-61-parallel-skill-practice.json').records;
const q = n => {
  const item=records[n-1];
  assert.equal(item.sourceItem,'U01L01-Q'+String(n).padStart(2,'0'));
  assert.equal(item.gradingRoute,'TEACHER_REVIEW_REQUIRED');
  return item;
};
const normalized = text => String(text)
 .replace(/[\u0660-\u0669]/g, n=>String(n.charCodeAt(0)-0x0660))
 .replace(/[\u06f0-\u06f9]/g, n=>String(n.charCodeAt(0)-0x06f0))
 .replace(/\u066b/g,'.').replace(/\u2212/g,'-')
 .replace(/\s+/g,'');
const checkExpected = (item,output) => {
  assert.ok(normalized(q(item).teacherExpectedEvidence).includes(normalized(output)),
    'Independent teacher rubric did not retain exact result for question '+item+': '+output);
};
const gcd=(a,b)=>b===0?Math.abs(a):gcd(b,a%b);
function fraction(n,d) {
  if(!d)throw Error('zero denominator');
  if(d<0){n=-n;d=-d}
  const k=gcd(n,d);
  return [n/k,d/k];
}
const plus=(a,b)=>fraction(a[0]*b[1]+a[1]*b[0],a[1]*b[1]);
const minus=(a,b)=>fraction(a[0]*b[1]-a[1]*b[0],a[1]*b[1]);
const times=(a,b)=>fraction(a[0]*b[0],a[1]*b[1]);
const divided=(a,b)=>fraction(a[0]*b[1],a[1]*b[0]);
const order=items=>[...items].sort((a,b)=>a-b);
const median=items=>{
  const arr=order(items),i=Math.floor(arr.length/2);
  return arr.length%2 ? arr[i] : (arr[i-1]+arr[i])/2;
};
const modes=items=>{
  const freq=new Map();
  items.forEach(value=>freq.set(value,(freq.get(value)||0)+1));
  const high=Math.max(...freq.values());
  return [...freq].filter(([,n])=>n===high).map(([value])=>value).sort((a,b)=>a-b);
};

test('Independent textbook-source ambiguous verbal Q07/Q25 is preserved as ambiguity, not a fabricated official answer',()=>{
  const src=read('u01l01-source-wordorder-ambiguities-math-proof-NO_GO.json');
  const index=read('grade9-82-target-index.json');
  assert.equal(src.sourceSha256,index.records[0].SourcePdfSha256);
  assert.equal(src.sourcePrintedPage,7);
  assert.equal(src.sourcePdfPage1Based,18);
  assert.equal(src.sourceQuestionProofs.length,2);
  assert.deepEqual(src.sourceQuestionProofs.map(x=>x.item),['U01L01-Q07','U01L01-Q25']);
  assert.ok(src.sourceQuestionProofs.every(x=>x.publisherOfficialIntendedExpressionKnown===false));
  assert.ok(src.sourceQuestionProofs.every(x=>x.automaticSourceAnswerSelectionAllowed===false));
  assert.equal(src.sourceQuestionProofs[0].candidateReadingA.solution,7*(1-7));
  assert.equal(src.sourceQuestionProofs[0].candidateReadingB.solution,7*(7+1));
  assert.equal(src.sourceQuestionProofs[0].independentAnalogSolution,5*(2-3));
  assert.equal(src.sourceQuestionProofs[1].candidateReadingA.atX2,3*2+16);
  assert.equal(src.sourceQuestionProofs[1].candidateReadingB.atX2,3*(2+16));
  checkExpected(25,'22');
  checkExpected(25,'54');
  checkExpected(7,'-5');
  assert.equal(src.fullSourceAtomicAuditPassed,false);
  assert.equal(src.productionPublishAllowed,false);
  assert.equal(src.releaseDecision,'NO_GO');
});

test('Independent Arabic algebra Q34-Q37 has exact reproducible economic, proportional, sports and power results',()=>{
  checkExpected(34,'53');
  assert.equal(39+0.4*20+3*2,53);
  checkExpected(35,'15');
  assert.equal((1/2)*(3/5)*50,15);
  for (const [score,goals,extras] of [[23,4,3],[18,3,3],[6,1,1]]) {
    assert.equal(score,5*goals+extras);
    assert.ok(extras>=0 && extras<=goals);
  }
  for(const [n,e] of [[3,5],[4,6]]) {
    assert.equal(10**2*10**n,10**e);
    checkExpected(37,'10');
  }
  assert.ok(normalized(q(36).teacherExpectedEvidence).includes('23'));
});

test('Independent geometry, perimeter and three-dimensional source-aligned variants compute unambiguously',()=>{
  for (const x of [-3,0,1,2,5]) {
    const first=4*x*x+4;
    const second=4*(x*x+4);
    assert.equal(second-first,12,'Distributive non-equivalence must hold for every checked x');
    assert.notEqual(first,second);
  }
  checkExpected(40,'12');
  assert.equal(4**3,64);
  assert.equal(4**2,16);
  checkExpected(43,'64');
  for(const [l,w] of [[7,3],[5,8],[0,0]]) {
    assert.equal(2*l+2*w,2*(l+w));
  }
  checkExpected(44,'2');
  assert.equal(7*3,21);
  checkExpected(46,'21');
  assert.equal(9*4,36);
  checkExpected(52,'36');
});

test('Independent statistical graph analogs Q47-Q51 use correct total, quartiles, mean, median and all modes',()=>{
  const votes=[8,5,7,11,6,9,4];
  assert.equal(votes.length,7);
  assert.equal(votes.reduce((a,b)=>a+b),50);
  assert.equal(votes.indexOf(Math.max(...votes)),3);
  assert.equal(votes.indexOf(Math.min(...votes)),6);
  checkExpected(47,'11');
  checkExpected(47,'4');
  const times=[19,20,21,21,22,22,23,24,24,25,26,27,28,29];
  assert.equal(times.length,14);
  const lower=times.slice(0,7),upper=times.slice(7);
  const [min,q1,mid,q3,max]=[times[0],median(lower),median(times),median(upper),times.at(-1)];
  assert.deepEqual([min,q1,mid,q3,max],[19,21,23.5,26,29]);
  assert.equal(q3-q1,5);
  checkExpected(48,'23.5');
  checkExpected(48,'26');
  const data49=[3,5,5,6,7,7,7,9,10,11];
  assert.equal(data49.reduce((a,b)=>a+b,0),70);
  assert.equal(median(data49),7);
  assert.deepEqual(modes(data49),[7]);
  checkExpected(49,'70');
  const data50=[-4,-2,-2,-2,0,0,0,1,4,5];
  assert.equal(data50.reduce((a,b)=>a+b,0),0);
  assert.equal(median(data50),0);
  assert.deepEqual(modes(data50),[-2,0]);
  checkExpected(50,'-2');
  const data51=[4,9,10,12,14,18,18,27];
  assert.equal(data51.reduce((a,b)=>a+b,0),112);
  assert.equal(median(data51),13);
  assert.deepEqual(modes(data51),[18]);
  checkExpected(51,'112');
});

test('All independent fraction-result Q53-Q56 and Q58/Q60 use exact reduced rational arithmetic',()=>{
  const solutions=[
    [53,times([2,3],[3,5]),[2,5]],
    [54,divided([5,6],[10,9]),[3,4]],
    [55,times([7,6],[9,4]),[21,8]],
    [56,plus([5,6],[7,9]),[29,18]],
    [58,minus([3,5],[7,3]),[-26,15]],
    [60,plus([7,8],[5,12]),[31,24]]
  ];
  for(const [n,actual,want] of solutions) {
    assert.deepEqual(actual,want,'Wrong rational mathematical oracle for '+n);
    checkExpected(n,String(actual[0])+'/'+String(actual[1]));
  }
});

test('Independent decimal questions Q57/Q59/Q61 use integer hundredths and preserve result digits',()=>{
  for(const [n,cents,result] of [[57,843-217,626],[59,1684+2759,4443],[61,4725-1867,2858]]) {
    assert.equal(cents,result);
    const written=String(Math.floor(result/100))+'.'+String(result%100).padStart(2,'0');
    checkExpected(n,written);
  }
  // Every question stays routed for manual teacher review; numeric oracle tests
  // guard authored rubrics only and never silently enable production autograding.
  for(const record of records) {
    assert.equal(record.gradingRoute,'TEACHER_REVIEW_REQUIRED');
    assert.ok(record.teacherExpectedEvidence);
  }
});
