'use strict';
const { test } = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');

const root = path.resolve(__dirname, '..');
const at = relative => JSON.parse(fs.readFileSync(path.join(root, relative), 'utf8'));
const evidencePath = 'artifacts/academic-us-ccss/grade-reports/G01-source-screening.json';
const screen = at(evidencePath);
const pack = at('src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g1-phase29-v1.lesson-content-pack.json');
const prior = at('artifacts/academic-us-ccss/04-grade1-source-verified-instructional-repair-candidates.json');
const gradeReport = fs.readFileSync(path.join(root, 'artifacts/academic-us-ccss/grade-reports/G01-progress.md'), 'utf8');

const eight = ['title','standard','explanation','examples','solutions',
  'visualRepresentations','commonMistakes','questionsAndGrading'];
const sourceFlags = [
  ['SOURCE_QUOTED_TEACHER_PROMPT_AS_EXAMPLE', t => /^Example 1:\s*[“"']/.test(t.workedExamples || '')],
  ['GENERIC_SOLUTION_TEMPLATE', t => /Step 1: (?:Represent the quantities or objects|Look at the defining properties|Read the category labels)/i.test(t.stepByStepSolutions || '')],
  ['RAW_HTML_SVG_FRAGMENT', t => /<(?:path|svg)\b|&(?:nbsp|amp|lt|gt);/i.test(t.workedExamples || '')]
];

test('entire G1 inventory is unique, exact, and still not falsely certified', () => {
  assert.equal(screen.schemaVersion, 1);
  assert.equal(screen.scope, 'US-CCSS-MATH/CCSSM-2010/G1');
  assert.equal(pack.lessons.length, 146);
  assert.equal(screen.records.length, 146);
  assert.deepEqual(screen.records.map(r => r.lessonCode), pack.lessons.map(r => r.lessonCode));
  assert.equal(new Set(screen.records.map(r => r.lessonCode)).size, 146);
  assert.equal(screen.counts.fullEightDimensionReviewsConfirmed, 0);
  assert.equal(screen.counts.academicallyCertified, 0);
  assert.equal(screen.notAcademicSignoff, true);
  assert.equal(screen.notRuntimeVerification, true);
  assert.equal(screen.notFinalReport, true);
  assert.ok(gradeReport.includes('NOT A FINAL ACADEMIC CERTIFICATION REPORT'));
  assert.ok(!gradeReport.includes('GRADE_PASS'));
  for (const r of screen.records) {
    assert.equal(r.grade, 'G1');
    assert.deepEqual(Object.keys(r.eightDimensionSourceScreen), eight);
    assert.equal(r.academicDecision, 'NOT_CERTIFIED');
    assert.equal(r.humanSignoff, 'NOT_PRESENT_IN_SOURCE_SCREEN');
    assert.ok(['BLOCKED_MISSING_EVIDENCE','REWORK_REQUIRED_SOURCE_EVIDENCE'].includes(r.editorialTriage));
    assert.notEqual(r.eightDimensionSourceScreen.visualRepresentations.screen, 'PASS');
    assert.notEqual(r.eightDimensionSourceScreen.questionsAndGrading.screen, 'PASS');
    for (const axis of eight) {
      assert.notEqual(r.eightDimensionSourceScreen[axis].screen, 'PASS');
      assert.ok(typeof r.eightDimensionSourceScreen[axis].screen === 'string');
    }
  }
});

test('checked-in original source and officially mapped identities match each G1 evidence row', () => {
  for (let i = 0; i < pack.lessons.length; i++) {
    const lesson = pack.lessons[i], row = screen.records[i];
    const en = lesson.translations.find(x => x.cultureCode === 'en');
    assert.ok(en, lesson.lessonCode);
    assert.equal(row.title, en.title);
    assert.deepEqual(row.officialOutcomeCodes, lesson.outcomeCodes);
    assert.equal(row.sourceUrl, lesson.sourceUrl);
    assert.equal(row.sourceSha256, lesson.sourceSha256);
    assert.equal(row.canonicalBodySha256, lesson.canonicalBodySha256);
    assert.equal(row.sourcePackPath, screen.sourcePackPath);
    assert.ok(/^https:\/\/im\.kendallhunt\.com\//.test(row.sourceUrl), row.lessonCode);
    for (const [flag, predicate] of sourceFlags) {
      assert.equal(row.sourceSignals.includes(flag), predicate(en), row.lessonCode + ': ' + flag);
    }
    assert.equal(
      gradeReport.includes('|' + ' ' + (i+1) + ' | ' + '\`' + lesson.lessonCode.replace('PED:US-CCSS-MATH:', '') + '\`'),
      true, 'Missing grade report row: '+lesson.lessonCode
    );
  }
});

test('prior Grade 1 fixes are never misreported as certs or unmodified live source', () => {
  const pinned = new Set(prior.lessons.map(x => x.lessonCode));
  assert.equal(pinned.size, 3);
  const reuses = screen.records.filter(r => r.previousRepairCandidate);
  assert.equal(reuses.length, 3);
  assert.deepEqual(new Set(reuses.map(r => r.lessonCode)), pinned);
  for (const row of reuses) {
    assert.equal(row.editorialTriage, 'BLOCKED_MISSING_EVIDENCE');
    assert.equal(row.previousRepairCandidate.academicApproval, 'NOT_GRANTED');
    assert.equal(row.evidenceBasis, 'RAW_EMBEDDED_PUBLISHER_DERIVED_PACK_NOT_VERIFIED_AS_RUNTIME_RENDERED_BODY');
  }
});

test('source defects are specifically flagged, not rubber-stamped or invented', () => {
  const byCode = new Map(screen.records.map(x => [x.lessonCode, x]));
  const check = (short, flag) => {
    const row = byCode.get('PED:US-CCSS-MATH:' + short);
    assert.ok(row, short);
    assert.ok(row.sourceSignals.includes(flag), short + '/' + flag);
    assert.equal(row.editorialTriage, 'REWORK_REQUIRED_SOURCE_EVIDENCE');
  };
  check('G1:U01:L04', 'SUBTRACTION_LESSON_USES_ADDITION_ONLY_EXAMPLES');
  check('G1:U01:L10', 'RAW_HTML_SVG_FRAGMENT');
  check('G1:U06:L17', 'OUT_OF_GRADE_NEGATIVE_RESULT_IN_STORY_EXAMPLE');
  const tally = {};
  for (const row of screen.records) {
    for (const flag of row.sourceSignals) tally[flag] = (tally[flag] || 0) + 1;
  }
  assert.deepEqual(tally, screen.counts.sourceSignalOccurrences);
  assert.equal(tally.SOURCE_QUOTED_TEACHER_PROMPT_AS_EXAMPLE, 28);
  assert.equal(tally.GENERIC_SOLUTION_TEMPLATE, 39);
  assert.equal(tally.RAW_HTML_SVG_FRAGMENT, 1);
  assert.equal(screen.counts.editorialReworkFromSource, 3);
  assert.equal(screen.counts.otherBlocked, 143);
  assert.equal(screen.counts.automatedSourceScreened, 146);
});
