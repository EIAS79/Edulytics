'use strict';

/**
 * Course-owned independent mathematics contracts for IM First Edition ALG2 U02 L18.
 * This does NOT replace authenticated Practice, academic sign-off, or approved
 * canonical-body successor/version transition. No publisher figures are copied.
 */
const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const crypto = require('node:crypto');

const root = path.resolve(__dirname, '../..');
const pack = JSON.parse(fs.readFileSync(path.join(root,
  'src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-hs-alg2-phase29-v1.lesson-content-pack.json'), 'utf8'));
const lessonCode = 'PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:U02:L18';
const lesson = pack.lessons.find(x => x.lessonCode === lessonCode);
const t = lesson?.translations.find(x => x.cultureCode === 'en');
const near = (a,b,tol=1e-9) => assert.ok(Number.isFinite(a) && Math.abs(a-b)<tol,
  'Expected '+a+' to equal '+b);
const revisedDigest = () => {
  const fields = ['title','explanation','keyConceptsAndRules','workedExamples',
    'stepByStepSolutions','commonMistakes','quickSummary'];
  return crypto.createHash('sha256')
    .update(fields.map(k => k+':'+t[k]).join('\n'),'utf8').digest('hex');
};

test('first-edition source and immutable predecessor recorded without false approval', () => {
  assert.ok(lesson && t, 'canonical lesson and English translation');
  assert.equal(lesson.sourceUrl,'https://im.kendallhunt.com/HS/teachers/3/2/18/preparation.html');
  assert.equal(lesson.sourceEdition,'First Edition 2019-2021');
  assert.deepEqual(lesson.outcomeCodes,['CCSS:HSA-APR.D.6','CCSS:HSA-SSE.A.1']);
  assert.equal(lesson.sourceSha256,
    '3ec01ea514d3a8d8edbd6bf1d5271079d0941e66e5558f508ce233ff5391ece0');
  assert.equal(lesson.canonicalBodySha256,
    '7cbc61c03b9ab3dcd69a13aed4fd3b580850f54446795458174cfa5e3b9fc475');
  assert.match(revisedDigest(),/^[a-f0-9]{64}$/);
  // Explicit pending-successor evidence. DO NOT "fix" the test by copying this
  // digest into an academic approval field; shared owner approves the transition.
  assert.notEqual(revisedDigest(),lesson.canonicalBodySha256,
    'authored body changed since the approved historical predecessor');
  assert.match(t.explanation,/paperback/i);
  assert.match(t.workedExamples,/hole/i);
  assert.match(t.keyConceptsAndRules,/horizontal asymptote/i);
});

test('18.1: equivalent fractional rewriting and a false equality counterexample', () => {
  const correct = x => (x+7)/x;
  const rewritten = x => 1+7/x;
  for(const x of [-10,-2,-1,1,2,7,100]) near(correct(x),rewritten(x));
  assert.ok(!Number.isFinite(correct(0)));
  const falseLeft = x => x/(x+7), falseRight = x => 1+x/7;
  near(falseLeft(7),.5);near(falseRight(7),2);
  assert.notEqual(falseLeft(7),falseRight(7));
  assert.ok(!Number.isFinite(falseLeft(-7)));
  assert.ok(!Number.isFinite(falseRight(0))===false); // x=0 is valid for falseRight
});

test('18.2: printing books, positive-integer domain, break-even and boundaries', () => {
  const cost = n => (120+4*n)/n;
  const profit = n => 8*n-(120+4*n);
  near(cost(50),6.4); near(cost(100),5.2);
  near(cost(.5),244); // algebraically defined, not a physical half-book
  for(const n of [1,2,29,30,31,100,500]) {
    assert.ok(Number.isInteger(n)&&n>0);
    near(cost(n),4+120/n);
  }
  near(profit(30),0);near(profit(31),4);
  assert.ok(profit(29)<0&&profit(31)>0);
  assert.ok(!Number.isFinite(cost(0)));
  assert.ok(cost(1000)>4&&cost(1000)<cost(50));
  const physicalPrintRun = x => Number.isInteger(x)&&x>0;
  for(const invalid of [0,-1,.5,NaN,'31',null])assert.equal(physicalPrintRun(invalid),false);
  assert.equal(physicalPrintRun(31),true);
});

test('18.3: all eight publisher expressions map to four asymptote families', () => {
  const examples = [
    {key:'a',expected:-1,fn:x=>4/x-1},
    {key:'b',expected:-4,fn:x=>1/x-4},
    {key:'c',expected:4,fn:x=>(1+4*x)/x},
    {key:'d',expected:1,fn:x=>(x+4)/x},
    {key:'e',expected:-4,fn:x=>(1-4*x)/x},
    {key:'f',expected:-1,fn:x=>(4-x)/x},
    {key:'g',expected:1,fn:x=>1+4/x},
    {key:'h',expected:4,fn:x=>1/x+4}
  ];
  const groups = new Map();
  for(const item of examples) {
    for(const x of [-1e9,1e9]) assert.ok(Math.abs(item.fn(x)-item.expected)<1e-7);
    assert.ok(!Number.isFinite(item.fn(0)),item.key+' pole excluded');
    groups.set(item.expected,[...(groups.get(item.expected)||[]),item.key]);
  }
  assert.deepEqual([...groups.get(4)].sort(),['c','h']);
  assert.deepEqual([...groups.get(-4)].sort(),['b','e']);
  assert.deepEqual([...groups.get(1)].sort(),['d','g']);
  assert.deepEqual([...groups.get(-1)].sort(),['a','f']);
});

test('distinguish a removable hole from an uncanceled pole', () => {
  const rational = x => (x*x-1)/(x-1);
  for(const x of [-5,-1,0,.999,1.001,5])near(rational(x),x+1);
  assert.ok(Number.isNaN(rational(1)),'0/0 at original x=1');
  const pole = x => (2*x+3)/(x-1);
  assert.ok(!Number.isFinite(pole(1)));
  near(pole(0),-3);
  near(pole(2),7);
  for(const x of [-1e9,1e9])assert.ok(Math.abs(pole(x)-2)<1e-7);
});

test('negative and malformed student answers must not count as equivalent', () => {
  const equal=(x,y)=> typeof x==='number' && typeof y==='number' &&
    Number.isFinite(x)&&Number.isFinite(y)&&Math.abs(x-y)<1e-9;
  assert.ok(equal(6.4,4+120/50));
  for(const wrong of [6.2,5.2,244,Infinity,NaN,'6.4','',null,undefined]) {
    assert.equal(equal(wrong,6.4),false, 'reject '+String(wrong));
  }
  assert.equal(equal(4,4),true);
});

test('18.3 source-task matches occur in the actual learner body, not only in a fixture', () => {
  const body=t.workedExamples;
  const expected = [
    ['a(x)=4/x-1','D'], ['b(x)=1/x-4','B'], ['c(x)=(1+4x)/x=4+1/x','A'],
    ['d(x)=(x+4)/x=1+4/x','C'], ['e(x)=(1-4x)/x=1/x-4','B'],
    ['f(x)=(4-x)/x=4/x-1','D'], ['g(x)=1+4/x','C'], ['h(x)=1/x+4','A']
  ];
  for (const [formula, graph] of expected) {
    assert.ok(body.includes(formula+' -> '+graph),formula+' must map to source graph '+graph);
  }
  assert.match(body,/x=0/);
  assert.match(body,/x=1 is a vertical asymptote and y=1\/2/);
});
