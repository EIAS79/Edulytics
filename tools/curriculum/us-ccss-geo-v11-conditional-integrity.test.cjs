'use strict';

const { test } = require('node:test');
const assert = require('node:assert/strict');
const { readdirSync, readFileSync } = require('node:fs');
const path = require('node:path');

const root = path.resolve(__dirname, '..', '..');
const folder = path.join(root, 'src/Edulytics.Core/Curriculum/LessonContent/RichV2');
const names = readdirSync(folder).filter(name => /^us-ccss-hs-geo-.*\.rich-lesson-v2\.json$/.test(name));
const lessons = names.flatMap(name => JSON.parse(readFileSync(path.join(folder, name), 'utf8')).Lessons);
const byCode = new Map(lessons.map(lesson => [lesson.LessonCode, lesson]));

function leafStrings(node, location = 'lesson') {
  if (typeof node === 'string') return [{ path: location, text: node }];
  if (Array.isArray(node)) return node.flatMap((value, i) => leafStrings(value, location + '[' + i + ']'));
  if (node && typeof node === 'object') {
    return Object.entries(node).flatMap(([key, value]) => leafStrings(value, location + '.' + key));
  }
  return [];
}

function semanticDefects(node) {
  const errors = [];
  for (const {path: location, text} of leafStrings(node)) {
    // Every P( event notation must close in the SAME text field.
    // Splitting JSON properties or a Steps array cannot be rescued by later rendering.
    const positions = [...text.matchAll(/\bP\(/g)].map(match => match.index);
    for (let i = 0; i < positions.length; i++) {
      const opening = positions[i];
      const next = positions[i + 1] ?? text.length;
      const tail = text.slice(opening + 2, next);
      const closing = tail.indexOf(')');
      if (closing < 0 || tail.slice(0, closing).includes('(')) {
        errors.push(location + ': incomplete P( notation at index ' + opening + ': ' + JSON.stringify(text));
      }
    }
    // Detect lost conditional delimiter even if the last parenthesis survived.
    if (/\bP\(\s*[AB]\s+[AB]\s*\)/.test(text)) {
      errors.push(location + ': missing conditional delimiter in ' + JSON.stringify(text));
    }
    // Detect the complementary half that landed at the start of the next field.
    if (/^\s*(?:[AB]|first red|red first)\)\s*(?:=|×|P\(|[A-Z])/.test(text)) {
      errors.push(location + ': orphan conditional tail ' + JSON.stringify(text));
    }
    if (/^\s*P\([AB]\s*$/.test(text) || /^\s*P\(red second\s*$/.test(text)) {
      errors.push(location + ': truncated conditional expression ' + JSON.stringify(text));
    }
  }
  return errors;
}

function tableProbabilities([[ab, aNotB], [notAB, neither]]) {
  for (const count of [ab, aNotB, notAB, neither]) {
    if (!Number.isInteger(count) || count < 0) throw new RangeError('invalid nonnegative count');
  }
  const A = ab + aNotB;
  const B = ab + notAB;
  const total = A + notAB + neither;
  if (total <= 0) throw new RangeError('empty sample space');
  const conditional = (numerator, denominator) => {
    if (denominator <= 0) throw new RangeError('conditioning event has zero probability');
    return numerator / denominator;
  };
  return {
    joint: ab / total,
    pA: A / total,
    pB: B / total,
    pAgivenB: B ? conditional(ab, B) : undefined,
    pBgivenA: A ? conditional(ab, A) : undefined,
    conditional,
  };
}

test('all 124 Geometry lesson fields are self-contained: no conditional bar splitting', () => {
  assert.equal(lessons.length, 124);
  assert.equal(byCode.size, 124);
  const problems = lessons.flatMap(lesson =>
    semanticDefects(lesson).map(issue => lesson.LessonCode + ': ' + issue)
  );
  assert.deepEqual(problems, []);
});

test('mutation regressions detect split Rules, Steps, Corrections and lost bars', () => {
  for (const bad of [
    {KeyConcepts:[{Rule:'P(A',Example:'B)=count(A∩B)/count(B)'}]},
    {WorkedExamples:[{Steps:['Use P(A','B)=P(A∩B)/P(B)']}]},
    {CommonMistakes:[{Correction:'Use P(A'}]},
    {SummaryPoints:['P(B']},
    {ExplanationParagraphs:['For P(B)>0, P(A B)=P(A)']},
    {WorkedExamples:[{Steps:['P(red second','first red)=1/2']}]},
  ]) assert.ok(semanticDefects(bad).length > 0, JSON.stringify(bad));
  assert.deepEqual(semanticDefects({
    Rule: 'P(A|B)=P(A∩B)/P(B), for P(B)>0.',
    Statement: 'P(A∩B)=P(A|B)×P(B).',
    Example: 'P(even | result>3)=2/3.',
  }), []);
});

test('all five formerly affected lessons contain complete independently meaningful probability formulae', () => {
  const stem = 'PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U08:L';
  const l04 = byCode.get(stem+'04'), l07=byCode.get(stem+'07'),
    l08=byCode.get(stem+'08'), l09=byCode.get(stem+'09'), l10=byCode.get(stem+'10');
  for (const lesson of [l04,l07,l08,l09,l10]) {
    assert.ok(lesson, 'missing repaired lesson');
    assert.deepEqual(semanticDefects(lesson), []);
    assert.ok(lesson.WorkedExamples.every(example =>
      example.Steps.length===3 && example.Steps.every(step=>step.length>15)));
  }
  assert.match(l04.KeyConcepts[2].Rule,/P\(A\|B\)=count\(A∩B\)\/count\(B\)/);
  assert.match(l04.WorkedExamples[2].Steps.join(' '),/P\(B\|A\)=.*12\/20=0\.6/);
  assert.match(l07.KeyConcepts[0].Rule,/P\(A\|B\)=P\(A\)/);
  assert.match(l07.CommonMistakes[0].Correction,/P\(A∩B\)=P\(A\)/);
  assert.match(l08.KeyConcepts[0].Rule,/P\(A\|B\)=P\(A∩B\)\/P\(B\)/);
  assert.match(l08.KeyConcepts[1].Rule,/P\(A∩B\)=P\(A\|B\)/);
  assert.match(l08.KeyConcepts[2].Example,/undefined/);
  assert.match(l09.KeyConcepts[0].Rule,/P\(B\|A\)=/);
  assert.match(l09.KeyConcepts[1].Rule,/P\(A\|B\)=/);
  assert.match(l10.KeyConcepts[1].Rule,/P\(A\|B\)=P\(A\)/);
});

test('conditional denominators and joint-probability oracles are mathematically independent', () => {
  const near = (a,b) => Math.abs(a-b)<1e-12;
  const fair = tableProbabilities([[12,8],[18,12]]);
  assert.ok(near(fair.pA,0.4));
  assert.ok(near(fair.pB,0.6));
  assert.ok(near(fair.joint,0.24));
  assert.ok(near(fair.pAgivenB,0.4));
  assert.ok(near(fair.pBgivenA,0.6));
  assert.notEqual(fair.pAgivenB,fair.pBgivenA);
  assert.ok(near(fair.joint,fair.pA*fair.pB));
  assert.ok(near(fair.pAgivenB*fair.pB,fair.joint));

  const dependent = tableProbabilities([[10,10],[5,25]]);
  assert.ok(!near(dependent.joint,dependent.pA*dependent.pB));
  assert.ok(!near(dependent.pAgivenB,dependent.pA));
  assert.ok(near(dependent.pAgivenB,2/3));
  // Two mutually exclusive events of positive probability are NOT independent.
  const disjoint = tableProbabilities([[0,1],[1,4]]);
  assert.equal(disjoint.joint,0);
  assert.ok(disjoint.pA*disjoint.pB>0);
  assert.ok(!near(disjoint.joint,disjoint.pA*disjoint.pB));
  // Independent coin tosses: first-head and second-head.
  const coins = tableProbabilities([[1,1],[1,1]]);
  assert.ok(near(coins.joint,coins.pA*coins.pB));
  // Without replacement from 2 red + 1 blue:
  assert.ok(near(1/2,0.5));
  assert.ok(!near(1/2,2/3));
});

test('zero-probability conditioning is undefined rather than zero or fabricated', () => {
  const emptyB = tableProbabilities([[0,3],[0,2]]);
  assert.equal(emptyB.pB,0);
  assert.equal(emptyB.pAgivenB,undefined);
  assert.throws(()=>emptyB.conditional(0,0),RangeError);
  assert.throws(()=>tableProbabilities([[0,0],[0,0]]),RangeError);
  assert.throws(()=>tableProbabilities([[-1,1],[1,1]]),RangeError);
});

test('Razor renders probability notation as safely encoded text, without unsafe HTML injection', () => {
  const reader=readFileSync(path.join(root,'src/Edulytics.Web/Views/Shared/_RichLessonContentV2.cshtml'),'utf8');
  const visual=readFileSync(path.join(root,'src/Edulytics.Web/Views/Shared/_RichLessonVisual.cshtml'),'utf8');
  for(const name of ['paragraph','concept.Rule','concept.Definition','example.Question','step']) {
    assert.ok(reader.includes('@'+name), 'missing Razor-encoded text '+name);
  }
  assert.ok(visual.includes('@cell'));
  assert.ok(!reader.includes('Html.Raw('));
  assert.ok(!visual.includes('Html.Raw('));
});
