'use strict';
const { test } = require('node:test');
const assert = require('node:assert/strict');
const { readFileSync } = require('node:fs');
const path = require('node:path');
const root = path.resolve(__dirname, '..', '..');
const read = (p) => JSON.parse(readFileSync(path.join(root, p), 'utf8'));
const blueprint = read('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/us-ccss-math-hs-traditional-geometry-im1e.lesson-blueprint.json');
const pack = read('src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-hs-geo-phase29-v1.lesson-content-pack.json');
const rich = read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u01-l01-l03-2021.rich-lesson-v2.json');
const evidence = read('artifacts/curriculum-delivery/US-CCSS-MATH/GEO/geo-124-source-manifest-20261028.json');
const prefix = 'PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U01:L';

test('Geometry identities, source links, outcomes and order match all 124 canonical records', () => {
  assert.equal(blueprint.BlueprintCode, 'US-CCSS-MATH:HS:TRAD:GEO:IM-1E');
  assert.equal(blueprint.CourseCode, 'GEO');
  assert.equal(blueprint.Pathway, 'Traditional');
  assert.equal(blueprint.Units.length, 8);
  assert.equal(blueprint.Lessons.length, 124);
  assert.equal(pack.lessons.length, 124);
  assert.equal(new Set(blueprint.Lessons.map(x => x.LessonCode)).size, 124);
  assert.equal(new Set(blueprint.Lessons.map(x => x.SourceLessonCode)).size, 124);
  assert.equal(new Set(pack.lessons.map(x => x.lessonCode)).size, 124);
  const contentByCode = new Map(pack.lessons.map(x => [x.lessonCode, x]));
  for (const [i, lesson] of blueprint.Lessons.entries()) {
    assert.equal(lesson.SortOrder, i + 1);
    const candidate = contentByCode.get(lesson.LessonCode);
    assert.ok(candidate, lesson.LessonCode);
    assert.equal(candidate.sourceLocator, lesson.SourceLessonCode);
    assert.equal(candidate.sourceUrl, lesson.SourceUrl);
    assert.ok(lesson.SourceUrl.startsWith('https://im.kendallhunt.com/HS/teachers/2/'));
    assert.deepEqual(candidate.outcomeCodes, lesson.FormalTargets.map(o => o.OutcomeCode));
    assert.equal(candidate.isSupporting, false);
  }
});

test('Course-wide matrix records unverified runtime and no fabricated approvals', () => {
  assert.equal(evidence.lessons.length, 124);
  assert.equal(evidence.totals.blueprintLessons, 124);
  assert.equal(evidence.totals.richCandidateLessons, 3);
  assert.equal(evidence.totals.gradeAccepted, 0);
  assert.equal(evidence.totals.runtimeVerified, 0);
  const known = blueprint.Lessons.map(l => l.LessonCode);
  assert.deepEqual(evidence.lessons.map(l => l.lessonCode), known);
  for (const row of evidence.lessons) {
    assert.equal(row.stageD, 'NOT_RUN');
    assert.equal(row.release, 'NOT_ACCEPTED');
    assert.equal(row.persisted, 'NOT_CHECKED');
    assert.ok(row.sourceSemanticSha256.length === 64);
    assert.ok(row.canonicalBodySha256.length === 64);
  }
});

test('Three original construction lessons contain specific reasoning and human assessment limits', () => {
  assert.equal(rich.PackCode, 'US-CCSS-MATH');
  assert.equal(rich.Lessons.length, 3);
  assert.deepEqual(rich.Lessons.map(l => l.LessonCode), [prefix+'01', prefix+'02', prefix+'03']);
  for (const lesson of rich.Lessons) {
    assert.equal(lesson.CultureCode, 'en');
    assert.equal(lesson.WorkedExamples.length, 3);
    assert.ok(lesson.ExplanationParagraphs.some(x => /teacher[- ]scor|teacher[- ]review|teacher[- ]check|Human assessment/i.test(x)));
    assert.ok(lesson.ExplanationParagraphs.join(' ').includes('CC BY 4.0'));
    assert.ok(lesson.KeyConcepts.length >= 3);
    assert.ok(lesson.CommonMistakes.length >= 2);
    assert.ok(lesson.WorkedExamples.every(ex => ex.Steps.length >= 3 && ex.Answer && ex.Check));
    assert.ok(!JSON.stringify(lesson).includes('You will constructing Patterns'));
    assert.ok(!JSON.stringify(lesson).includes('You will build It'));
    assert.deepEqual(lesson.Visuals, []);
    assert.deepEqual(lesson.Videos, []);
  }
});

test('Two-circle constructions are geometrically consistent at boundary cases', () => {
  const distance = (p, q) => Math.hypot(p[0] - q[0], p[1] - q[1]);
  for (const d of [1, 6, 11]) {
    const r = 0.7 * d;
    const A = [0, 0], B = [d, 0];
    const y = Math.sqrt(r*r - (d/2)*(d/2));
    for (const P of [[d/2, y], [d/2, -y]]) {
      assert.ok(Math.abs(distance(A, P) - r) < 1e-10);
      assert.ok(Math.abs(distance(B, P) - r) < 1e-10);
    }
    assert.ok(y > 0);
    assert.equal(r <= d/2, false);
  }
  assert.equal(2 * 4 > 10, false);
  assert.equal(2 * 5 > 10, false);
  assert.equal(2 * 6 > 10, true);
  // With radius AB, the apex yields equilateral triangles and equal-sided rhombi.
  const d=2, r=2, y=Math.sqrt(r*r - (d/2)*(d/2));
  const A=[0,0],B=[d,0],C=[d/2,y],D=[d/2,-y];
  assert.ok([distance(A,C),distance(C,B),distance(B,D),distance(D,A)].every(x=>Math.abs(x-d)<1e-10));
});

test('Candidate remains isolated from shared Practice and cannot certify release', () => {
  for (const l of rich.Lessons) {
    assert.ok(l.LessonCode.startsWith('PED:US-CCSS-MATH:HS:TRAD:GEO:'));
    assert.ok(!Object.hasOwn(l, 'AutoGrade'));
    assert.ok(!Object.hasOwn(l, 'Approved'));
    assert.ok(!Object.hasOwn(l, 'AcademicApproved'));
  }
  assert.equal(evidence.status, 'WORK_IN_PROGRESS_NOT_RELEASE_READY');
});
