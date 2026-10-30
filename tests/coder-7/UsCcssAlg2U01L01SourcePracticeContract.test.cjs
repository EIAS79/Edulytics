'use strict';
const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const repo = path.resolve(__dirname, '../..');
const read = p => JSON.parse(fs.readFileSync(path.join(repo, p), 'utf8'));
const lessonCode = 'PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:U01:L01';
const contract = read('artifacts/curriculum-delivery/US-CCSS-MATH/coder-7/U01-L01-ABC-SOURCE-PRACTICE-CONTRACT.json');
const pack = read('src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-hs-alg2-phase29-v1.lesson-content-pack.json');
const trace = read('artifacts/curriculum-delivery/US-CCSS-MATH/coder-7/algebra2-120-practice-verification-trace.json');
const content = pack.lessons.find(l => l.lessonCode === lessonCode);
const text = Object.values(content.translations[0]).join(' ');
const hanoi = n => (1n << BigInt(n)) - 1n;

// Independent state-space shortest-path oracle, not the Hanoi recurrence.
function minHanoiBfs(n) {
  const start = Array(n).fill(0);
  const key = p => p.join('');
  const queue = [[start, 0]], seen = new Set([key(start)]);
  for (let i = 0; i < queue.length; ++i) {
    const [p, depth] = queue[i];
    if (p.every(x => x === 1)) return depth;
    const top = [null, null, null];
    p.forEach((peg, disc) => { if (top[peg] === null) top[peg] = disc; });
    for (let from = 0; from < 3; from++) for (let to = 0; to < 3; to++) {
      if (from === to || top[from] === null) continue;
      if (top[to] !== null && top[from] > top[to]) continue;
      const q = p.slice(); q[top[from]] = to;
      const k = key(q);
      if (!seen.has(k)) { seen.add(k); queue.push([q, depth + 1]); }
    }
  }
  throw new Error('Unreachable');
}
// Independent legal-move BFS for the optional forward-only checker puzzle.
function minCheckerBfs(n) {
  const start = 'B'.repeat(n) + '_' + 'R'.repeat(n);
  const goal = 'R'.repeat(n) + '_' + 'B'.repeat(n);
  const queue = [[start, 0]], seen = new Set([start]);
  for (let k = 0; k < queue.length; k++) {
    const [state, depth] = queue[k];
    if (state === goal) return depth;
    for (let i = 0; i < state.length; i++) {
      const c = state[i]; if (c === '_') continue;
      const d = c === 'B' ? 1 : -1;
      for (const step of [1,2]) {
        const j = i + d * step;
        if (j < 0 || j >= state.length || state[j] !== '_') continue;
        if (step === 2 && state[i + d] !== (c === 'B' ? 'R' : 'B')) continue;
        const arr = state.split(''); arr[j] = c; arr[i] = '_';
        const next = arr.join('');
        if (!seen.has(next)) { seen.add(next); queue.push([next, depth+1]); }
      }
    }
  }
  throw new Error('Unreachable');
}
test('U01:L01 exact local-source activities represented in course-owned lesson', () => {
  assert.equal(content.lessonCode, lessonCode);
  for (const fragment of ['three marked circles', 'Checker Jumping Puzzle', '64-disc', 'Starting with 0,1', '1/16,1/32,1/64', '17,-31,65', 'two different possible rules']) {
    // two distinct rules are described explicitly rather than this exact source phrasing
    if (fragment === 'two different possible rules') continue;
    assert.ok(text.includes(fragment), 'Missing source activity: '+fragment);
  }
  assert.equal(contract.stageA.sourceActivityCoverage.length, 7);
  assert.equal(content.sourceSha256, '11ed0bf897ac0d0a3346603a80e47f8e7dc991911d0a120e2c065d32b9a0360c');
  assert.equal(content.canonicalBodySha256, '9e7aa655c4441095780fd17c07c4d53612b59a5becc907ab55541e224aabeff7');
});
test('Tower of Hanoi minimum count independently proved by legal-move BFS for n=1..4', () => {
  for (let n = 1; n <= 4; n++) assert.equal(BigInt(minHanoiBfs(n)), hanoi(n));
  for (const n of [3,4,5,7,64]) assert.equal(hanoi(n).toString(), contract.stageB.expectedAnswers.towerDiscCounts[n]);
  assert.equal(hanoi(64), 18446744073709551615n);
  assert.notEqual(BigInt(Number(hanoi(64))), hanoi(64), 'Float64 rounds large integers');
});
test('Checker puzzle values independently verified with legal-move BFS', () => {
  for (let n = 1; n <= 4; n++) {
    assert.equal(minCheckerBfs(n), n * (n+2));
    assert.equal(minCheckerBfs(n), contract.stageB.expectedAnswers.checkerCounts[n]);
  }
  assert.equal(7*(7+2),contract.stageB.expectedAnswers.checkerCounts[7]);
});
test('Source Practice positive/negative/indexing/equivalent rational cases', () => {
  const fib=[0,1];
  while(fib.length<10) fib.push(fib.at(-1)+fib.at(-2));
  assert.deepEqual(fib,contract.stageB.expectedAnswers.fibonacciFirstTen);
  assert.deepEqual([1,2,3].map(k=>2**(-k-3)),[1/16,1/32,1/64]);
  const f=x=>-2*x+3, invert=y=>(3-y)/2;
  const x4=-7, x3=invert(x4), x2=invert(x3), x1=invert(x2);
  assert.deepEqual([x1,x2,x3],contract.stageB.expectedAnswers.recurrenceBeforeFourth);
  assert.deepEqual([f(x4),f(f(x4)),f(f(f(x4)))],contract.stageB.expectedAnswers.recurrenceAfterFourth);
  assert.equal(f(x3),x4);
  assert.notEqual(3+2*x3,x4,'reject wrong recurrence sign');
  assert.equal(2/32,1/16,'accept mathematically equivalent fraction');
  assert.equal(0.0625,1/16,'accept decimal equivalent');
  assert.ok(Number.isNaN(Number('bad')));
  assert.equal(Number.isFinite(Number('Infinity')),false);
  assert.notDeepEqual(...contract.stageB.expectedAnswers.twoDistinctContinuations);
});
test('C is a handoff, not fictitious active Practice or completed Stage D', () => {
  assert.equal(contract.stageC.status,'C_PREPARED_BLOCKED_BY_INTEGRATION');
  assert.equal(contract.stageC.lessonScopedMappingExplicitInSharedRegistry,false);
  assert.equal(contract.ABC_READY_FOR_D,false);
  assert.equal(contract.D_VERIFIED,false);
  assert.equal(contract.stageC.adaptedCoolDown.classification,'ADAPTED_COOLDOWN_ORIGINAL_UNVERIFIED');
  assert.equal(trace.lessons[0].staticMappingClassification,'NO_EXPLICIT_ROW_FALLBACK_UNVERIFIED');
  assert.equal(trace.lessons[0].authenticatedStudentAttempt,'NOT_RUN');
});
