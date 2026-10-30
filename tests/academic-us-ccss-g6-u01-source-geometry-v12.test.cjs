'use strict';

// Grade 6 source/math authoring regressions. These assertions are NOT runtime
// Practice, authenticated rendering, publisher applet or teacher scoring tests.
const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');

const root = path.resolve(__dirname, '..');
const load = p => JSON.parse(fs.readFileSync(path.join(root, p), 'utf8'));
const rich = load('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g6-u01-l01-l03-area-20261028.rich-lesson-v2.json');
const evidence = load('artifacts/curriculum-delivery/US-CCSS-MATH/G6/g6-u01-l01-l02-source-geometry-v12-evidence-20261030.json');
const ledger = load('artifacts/curriculum-delivery/US-CCSS-MATH/G6/grade6-147-academic-authoring-ledger.json');
const byCode = n => rich.Lessons.find(x => x.LessonCode === 'PED:US-CCSS-MATH:G6:U01:L' + String(n).padStart(2, '0'));
const polygonArea = points => Math.abs(points.reduce((sum, p, i) => {
  const q = points[(i + 1) % points.length];
  return sum + p[0] * q[1] - q[0] * p[1];
}, 0) / 2);

test('U01:L01 source text evidence records figure/app and acceptance boundaries honestly', () => {
  assert.equal(evidence.evidence.length, 2);
  assert.equal(evidence.summary.sourceTextTasksScoped, 2);
  for (const row of evidence.evidence) {
    assert.equal(row.sourceStudentTaskTextCompared, true);
    assert.equal(row.sourceTeacherGoalAndPreparationCompared, true);
    assert.equal(row.sourceFigurePixelsIndependentlyInspected, false);
    assert.equal(row.sourceAppletOperated, false);
    assert.equal(row.teacherProtectedResponsesAccessed, false);
    assert.match(row.stages.A, /^OPEN__/);
    assert.match(row.stages.C, /^NOT_RUN__/);
    assert.match(row.stages.D, /^NOT_RUN__/);
  }
  assert.equal(evidence.summary.stageAClosed, 0);
  assert.equal(evidence.summary.academicAccepted, 0);
  assert.equal(evidence.summary.released, false);
});

test('U01:L01 source-faithful eight-hexagon lesson has counted units and explanation', () => {
  const l1 = byCode(1);
  assert.equal(l1.WorkedExamples.length, 5);
  assert.match(l1.ExplanationParagraphs.join(' '), /eight.*hexagon/i);
  const ex = l1.WorkedExamples.find(x => /Eight hexagons/.test(x.Title));
  assert.ok(ex);
  assert.match(ex.Question, /each hexagon contains 3 red trapezoids, 4 blue rhombi and 7 green triangles/);
  assert.match(ex.Answer, /72 triangular area units/);
  assert.match(ex.Answer, /64/);
  assert.match(ex.Answer, /56/);
  assert.match(ex.Check, /NOT|not.*recreate|does not recreate/i);
  assert.match(l1.Visuals[0].Description, /eight-hexagon/);
  assert.match(l1.Visuals[0].Description, /NOT square units/);
});

test('Independent eight-hexagon oracle rejects comparing only number of colored pieces', () => {
  const n = evidence.evidence[0].exactMathematicalOracle;
  assert.equal(n.hexagons, 8);
  assert.deepEqual(n.perHex, {trapezoids:3, rhombi:4, triangles:7});
  const counts = {
    red: n.hexagons * n.perHex.trapezoids,
    blue: n.hexagons * n.perHex.rhombi,
    green: n.hexagons * n.perHex.triangles
  };
  assert.deepEqual(counts, {red:24, blue:32, green:56});
  const byArea = {
    red: counts.red * n.relativeTileArea.trapezoids,
    blue: counts.blue * n.relativeTileArea.rhombi,
    green: counts.green * n.relativeTileArea.triangles
  };
  assert.deepEqual(byArea, {red:72, blue:64, green:56});
  assert.deepEqual(byArea, n.coloredAreas);
  assert.equal(Object.values(byArea).reduce((a,b) => a+b), 192);
  assert.equal(Object.values(counts).reduce((a,b) => a+b), 112);
  assert.notEqual(counts.red, Math.max(...Object.values(counts))); // largest area, not most pieces
  assert.equal(byArea.red > byArea.blue && byArea.blue > byArea.green, true);
});

test('Tiling area is invariant under piece reordering, but missing tiles are rejected', () => {
  const inventory = [ ...Array(24).fill(3), ...Array(32).fill(2), ...Array(56).fill(1) ];
  const sum = x => x.reduce((a,b) => a+b, 0);
  assert.equal(sum(inventory),192);
  assert.equal(sum([...inventory].reverse()),192);
  assert.equal(sum([...inventory.slice(56), ...inventory.slice(0,56)]),192);
  assert.equal(sum(inventory.slice(1)),189); // missing one trapezoid
  assert.equal(sum(inventory.concat([2])),194); // added rhombus
});

test('U01:L01 qualitative warm-up is explicitly teacher-scored, not a single number', () => {
  const l1 = byCode(1);
  const warmup = l1.WorkedExamples.find(x => /Four original pattern descriptions/.test(x.Title));
  assert.ok(warmup);
  assert.match(warmup.Answer, /uncovered/);
  assert.match(warmup.Answer, /different/);
  assert.match(warmup.Check, /teacher review/);
  assert.ok(l1.Visuals.some(v => /uncovered region/.test(v.Description)));
  assert.match(evidence.evidence[0].studentActivities[0].unresolved, /teacher/);
});

test('Four congruent half-square triangles make two square units, not six', () => {
  const l2 = byCode(2);
  const ex = l2.WorkedExamples.find(x => /Two non-square assemblies/.test(x.Title));
  assert.ok(ex);
  assert.match(ex.Steps.join(' '), /4 × 1\/2 = 2 square units/);
  assert.match(ex.Answer, /2 square units/);
  assert.equal(4 * (1 * 1 / 2), 2);
  assert.notEqual(4 * (1 * 1 / 2), 4 * 3 / 2); // obsolete unrelated 6-unit path
  assert.doesNotMatch(ex.Answer, /6 square units/);
  assert.match(l2.Visuals[0].SecondaryLabel, /4 × 1\/2 = 2 square units/);
});

test('Geometric proof: four half-unit triangles tile either a rectangle or slanted parallelogram', () => {
  const rectangle = [
    [[0,0],[1,0],[1,1]], [[0,0],[0,1],[1,1]],
    [[1,0],[2,0],[2,1]], [[1,0],[1,1],[2,1]]
  ];
  const slanted = [
    [[0,0],[1,0],[1,1]], [[1,0],[2,1],[1,1]],
    [[1,0],[2,0],[2,1]], [[2,0],[3,1],[2,1]]
  ];
  for (const tiling of [rectangle, slanted]) {
    assert.equal(tiling.length, 4);
    assert.deepEqual(tiling.map(polygonArea), [0.5,0.5,0.5,0.5]);
    assert.equal(tiling.map(polygonArea).reduce((a,b)=>a+b,0),2);
  }
  assert.equal(polygonArea([[0,0],[2,0],[2,1],[0,1]]),2);
  assert.equal(polygonArea([[0,0],[2,0],[3,1],[1,1]]),2);
  assert.notDeepEqual(rectangle, slanted);
});

test('A source-style unit checker rejects wrong, malformed and mixed-unit numerical claims', () => {
  // This is a test oracle, NOT the actual Edulytiks selected Practice verifier.
  const accepts = (submission, area, unit) =>
    submission !== null && typeof submission === 'object' &&
    typeof submission.value === 'number' && Number.isFinite(submission.value) &&
    submission.value === area && submission.unit === unit;
  assert.equal(accepts({value:2,unit:'square units'},2,'square units'),true);
  assert.equal(accepts({value:6,unit:'square units'},2,'square units'),false);
  assert.equal(accepts({value:2,unit:'triangular units'},2,'square units'),false);
  assert.equal(accepts({value:'2',unit:'square units'},2,'square units'),false);
  assert.equal(accepts({value:Infinity,unit:'square units'},2,'square units'),false);
  assert.equal(accepts(null,2,'square units'),false);
  assert.equal(accepts({value:72,unit:'triangular units'},72,'triangular units'),true);
  assert.equal(accepts({value:72,unit:'square units'},72,'triangular units'),false);
});

test('Grade 6 unresolved academic gates remain fail-closed after candidate edits', () => {
  assert.equal(ledger.lessons.length,147);
  assert.equal(ledger.summary.fullStageAReviews,0);
  assert.equal(ledger.summary.selectedPracticeRuntimeContractsVerified,0);
  assert.equal(ledger.summary.stageDVerified,0);
  assert.equal(ledger.summary.academicallyAccepted,0);
  assert.equal(ledger.summary.releaseReady,false);
  for (const n of [1,2]) {
    const row=ledger.lessons.find(x=>x.lessonCode === byCode(n).LessonCode);
    assert.ok(row);
    assert.equal(row.academicAcceptance,'NOT_ACCEPTED');
    assert.equal(row.stageA.publisherImageAndAppletParityVerified,false);
    assert.equal(row.stageC.exactPracticeAccepted,false);
  }
});
