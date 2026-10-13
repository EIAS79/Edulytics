'use strict';
const test = require('node:test');
const assert = require('node:assert/strict');
const { scanBatch } = require('../tools/us-ccss-b01-bulk-content-screen.cjs');

function fixture() {
  const records = Array.from({ length: 500 }, (_, i) => ({
    lessonCode: 'TEST:LESSON:' + i, title: 'Sum practice ' + i,
    gradeOrCourse: 'Grade 1', sourceUrl: 'https://example.org/' + i,
    sourceSemanticSha256: 'a'.repeat(64), officialOutcomeCodes: ['CCSS:1.OA.C.5']
  }));
  const lessons = records.map((r,i) => ({
    ordinal: i+1, lessonCode: r.lessonCode, title: r.title,
    sourceSemanticSha256: r.sourceSemanticSha256, gradeOrCourse: r.gradeOrCourse,
    sourceUrl: r.sourceUrl, officialOutcomeCodes: r.officialOutcomeCodes
  }));
  const bodies = records.map((r) => ({
    lessonCode:r.lessonCode, sourceUrl:r.sourceUrl,
    outcomeCodes:[...r.officialOutcomeCodes], sourceRights:'CC BY 4.0',
    canonicalBodySha256:'b'.repeat(64),sourceSha256:'c'.repeat(64),
    translations:[{
      cultureCode:'en', title:r.title,
      explanation:'Combine two groups of counters.',
      keyConceptsAndRules:'Addition finds the total.',
      workedExamples:'Two groups: 2 + 3 = 5.',
      stepByStepSolutions:'Count all counters: 2 + 3 = 5.',
      commonMistakes:'Do not skip counters.',
      quickSummary:'Check the sum by counting.'
    }]
  }));
  return {
    manifest:{batchId:'B01',lessons}, audit:{records:records.concat(
      Array.from({length:1060},(_,i)=>({lessonCode:'UNUSED:'+i})))},
    packs:[0,1,2,3].map((n)=>({
      packCode:'US-CCSS-MATH',versionCode:'CCSSM-2010',
      lessons:bodies.slice(n*125,(n+1)*125)
    }))
  };
}
test('Screens 500 learner bodies as ONE batch; never outputs certification',()=>{
  const r=scanBatch(fixture());
  assert.equal(r.coverage.observed,500);
  assert.equal(r.coverage.unique,500);
  assert.equal(r.counts.blockingStructuralIssues,0);
  assert.equal(r.academicDecision,'NOT_CERTIFIED');
  assert.equal(r.evidenceMatrix.length,500);
});
test('One wrong math expression is recorded as a batch defect',()=>{
  const f=fixture();
  f.packs[2].lessons[10].translations[0].workedExamples='2 + 3 = 9.';
  const r=scanBatch(f);
  assert.ok(r.issues.some(x=>x.problem.startsWith('FALSE_EQUATION_NOT_EXPLICITLY_MARKED')));
  assert.equal(r.academicDecision,'NOT_CERTIFIED');
});
test('A missing learner source within the 500 produces a gap',()=>{
  const f=fixture();
  f.packs[1].lessons.splice(40,1);
  const r=scanBatch(f);
  assert.ok(r.issues.some(x=>x.problem==='MISSING_LEARNER_CONTENT'));
});
test('Visual screening is not a substitute for L4 or L6 certification',()=>{
  const f=fixture();
  f.packs[0].lessons[0].translations[0].explanation='Use dot cube drawings to add.';
  const r=scanBatch(f);
  assert.ok(r.counts.lessonsWithVisualReviewCues>0);
  assert.equal(r.evidenceMatrix[0].L1toL6,'NOT_PROVEN_BY_STATIC_SCREENING');
});
