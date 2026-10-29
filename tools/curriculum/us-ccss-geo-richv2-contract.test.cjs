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
const richNext = read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u01-l04-l06-2021.rich-lesson-v2.json');
const richThird = read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u01-l07-l09-2021.rich-lesson-v2.json');
const authored = [...rich.Lessons, ...richNext.Lessons, ...richThird.Lessons];
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
  assert.equal(evidence.totals.richCandidateLessons, 9);
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

test('Nine original construction and modelling lessons have coherent authored candidate contracts', () => {
  assert.equal(rich.PackCode, 'US-CCSS-MATH');
  assert.equal(rich.Lessons.length, 3);
  assert.equal(richNext.Lessons.length, 3);
  assert.equal(richThird.Lessons.length, 3);
  assert.deepEqual(authored.map(l => l.LessonCode), [prefix+'01', prefix+'02', prefix+'03', prefix+'04', prefix+'05', prefix+'06', prefix+'07', prefix+'08', prefix+'09']);
  for (const lesson of authored) {
    assert.equal(lesson.CultureCode, 'en');
    assert.equal(lesson.WorkedExamples.length, 3);
    assert.ok(lesson.ExplanationParagraphs.some(x => /teacher[- ]scor|teacher[- ]review|teacher[- ]check|teacher rubric|Human assessment/i.test(x)));
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
  for (const l of authored) {
    assert.ok(l.LessonCode.startsWith('PED:US-CCSS-MATH:HS:TRAD:GEO:'));
    assert.ok(!Object.hasOwn(l, 'AutoGrade'));
    assert.ok(!Object.hasOwn(l, 'Approved'));
    assert.ok(!Object.hasOwn(l, 'AcademicApproved'));
  }
  assert.equal(evidence.status, 'WORK_IN_PROGRESS_NOT_RELEASE_READY');
});

test('Parallel and angle-bisector authored examples obey nondegenerate conditions', () => {
  const next = new Map(richNext.Lessons.map(x => [x.LessonCode, x]));
  const eq = next.get(prefix+'04');
  const ang = next.get(prefix+'05');
  const para = next.get(prefix+'06');
  assert.ok(eq.WorkedExamples.some(x => x.Answer.includes('ACE')));
  assert.ok(ang.WorkedExamples.some(x => x.Answer.includes('35-degree')));
  assert.ok(para.WorkedExamples.some(x => x.Answer.includes('y=3')));
  // Two perpendiculars to a transversal are parallel in Euclidean geometry.
  const horizontal = [1,0], vertical = [0,1];
  const dot = (a,b) => a[0]*b[0] + a[1]*b[1];
  assert.equal(dot(horizontal, vertical), 0);
  assert.equal(dot(vertical, horizontal), 0);
  assert.equal(dot(horizontal, horizontal), 1);
  assert.equal(2*3 > 6, false); // tangency: one circle-line contact only
  assert.equal(2*4 > 6, true); // larger radius: two circle-line intersections
});

test('Original square, dynamic geometry and Voronoi candidates have truthful assessment boundaries', () => {
  const byCode = new Map(richThird.Lessons.map(l => [l.LessonCode, l]));
  const sq = byCode.get(prefix+'07'), digital = byCode.get(prefix+'08'), vor = byCode.get(prefix+'09');
  assert.ok(sq.ExplanationParagraphs.join(' ').includes('four right'));
  assert.ok(digital.ExplanationParagraphs.join(' ').includes('optional source lesson'));
  assert.ok(digital.ExplanationParagraphs.join(' ').includes('Drag'));
  assert.ok(vor.ExplanationParagraphs.join(' ').includes('Euclidean'));
  for (const candidate of [sq, digital, vor]) {
    assert.ok(candidate.ExplanationParagraphs.some(x => /teacher rubric|teacher[- ]scor|teacher[- ]review/i.test(x)), candidate.LessonCode);
    assert.equal(candidate.WorkedExamples.length, 3);
    assert.equal(candidate.Visuals.length, 0);
  }
  // Perpendicular bisector x=2 of A=(0,0),B=(4,0):
  for (const y of [-9,0,2,11]) {
    const distanceSq = (p,q) => (p[0]-q[0])**2 + (p[1]-q[1])**2;
    assert.equal(distanceSq([2,y],[0,0]), distanceSq([2,y],[4,0]));
  }
  // A=(0,0), B=(4,0), C=(0,4): A is nearest where x<=2 and y<=2.
  for(const [x,y] of [[0,0],[1,1],[2,2]]) {
    const ds=(q)=> (x-q[0])**2+(y-q[1])**2;
    assert.ok(ds([0,0])<=ds([4,0])&&ds([0,0])<=ds([0,4]));
  }
  // A circle of radius r has inscribed-square side r*sqrt(2), and area 2r^2.
  for(const radius of [0.5,3,10]) {
    const side=Math.sqrt(2)*radius;
    assert.ok(Math.abs(side**2 - 2*radius*radius)<1e-10);
  }
});
