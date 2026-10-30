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
  assert.equal(l1.WorkedExamples.length, 8);
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
  assert.equal(ledger.summary.fullStageAReviews,4);
  assert.equal(ledger.summary.sourceStudentTaskParityAccepted,4);
  assert.equal(ledger.lessons[0].stageA.academicGoalTaskFullParityVerified,true);
  assert.equal(ledger.lessons[0].stageB.fullLessonParityAccepted,true);
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


test('G6 U01:L01 Edulytiks-original gated-source cool-down: area comparisons and human-review boundary', () => {
  const l1 = byCode(1);
  const cd = l1.WorkedExamples.find(x => /Edulytiks-original cool-down/.test(x.Title));
  assert.ok(cd, 'original check remains visible in grade-owned Rich V2');
  const [red, blue, green] = [2 * 3, 3 * 2, 5 * 1];
  assert.deepEqual([red, blue, green], [6, 6, 5]);
  assert.equal(red + blue + green, 17);
  assert.equal(red === blue && blue > green, true);
  assert.notEqual(green, Math.max(red, blue, green));
  const validNumeric = v => typeof v === 'number' && Number.isFinite(v) && v === red;
  assert.equal(validNumeric(6), true);
  assert.equal(validNumeric(6.0), true); // mathematically equivalent JavaScript number
  assert.equal(validNumeric(5), false); // tile-count trap
  assert.equal(validNumeric('6'), false); // malformed representation for a numeric contract
  assert.equal(validNumeric(NaN), false);
  assert.match(cd.Check, /NOT the gated publisher Cool-down/);
  assert.match(cd.Check, /teacher/i);
  assert.match(cd.Check, /not evidence of a live Practice/i);
  assert.match(cd.Answer, /17 units/);
});


test('G6 U01:L01 first-party four-pattern graphic and drawable alternatives are represented in Rich V2', () => {
  const l1 = byCode(1);
  const p = 'src/Edulytics.Web/wwwroot/images/public/us-ccss-g6-u01-l01-first-party-tilings.svg';
  const svg = fs.readFileSync(path.join(root, p), 'utf8');
  assert.match(svg, /Not publisher imagery/);
  assert.match(svg, /A, B, C each cover 4|A, B, C cover all 4/);
  assert.match(svg, /V and H|same pieces/i);
  assert.equal(l1.WorkedExamples.length, 8);
  assert.equal(l1.Visuals.length, 3);
  assert.ok(l1.Visuals.every(v => v.Kind === 'ShapeDecomposition' ? v.Rows.length > 0 : true));
  const a = l1.WorkedExamples.find(x => /four-pattern drawing comparison/.test(x.Title));
  const b = l1.WorkedExamples.find(x => /Two distinct equal-area constructions/.test(x.Title));
  assert.ok(a && b);
  assert.match(a.Answer, /D is not a full tiling/);
  assert.match(b.Answer, /4 square units/);
  assert.match(b.Check, /teacher/i);
  assert.ok(l1.Visuals[1].Rows.some(x => /D — mixed pattern with a gap/.test(x.Label)));
});

test('G6 U01:L01 independent 2x2 mosaic partition checker accepts equivalent V/H, rejects gaps and overlaps', () => {
  const S = 'square', T = 'two half-square triangles', G = 'gap';
  const V = [[S,T],[S,T]], H = [[S,S],[T,T]];
  const covered = cell => cell === S || cell === T ? 1 : 0;
  const total = grid => grid.flat().reduce((sum,cell) => sum + covered(cell), 0);
  const count = (grid,k) => grid.flat().filter(v => v === k).length;
  const valid = grid => Array.isArray(grid) && grid.length===2 && grid.every(row => Array.isArray(row) && row.length===2) &&
    grid.flat().every(cell => cell===S || cell===T) && count(grid,S)===2 && count(grid,T)===2 && total(grid)===4;
  assert.equal(valid(V),true);
  assert.equal(valid(H),true);
  assert.notDeepEqual(V,H);
  assert.equal(count(V,S),2);
  assert.equal(count(V,T)*2,4); // four individually cut half-square triangles
  assert.equal(2*1+4*0.5,4);
  assert.equal(2*1,4*0.5); // each shape type covers equal square area
  assert.equal(valid([[S,T],[S,G]]),false);
  assert.equal(valid([[S,T],[S,'overlap']]),false);
  assert.equal(valid([[S,T],[S,null]]),false);
  assert.equal(valid([[S,T],[S,4]]),false);
  assert.equal(valid([[S,T],[S,T],[S,T]]),false);
  assert.equal(total([[S,T],[S,G]]),3);
});

test('G6 U01:L01 source-derived math and authoring-only C blocked gate coexist', () => {
  const l1 = byCode(1);
  const info = JSON.parse(fs.readFileSync(path.join(root, 'artifacts/curriculum-delivery/US-CCSS-MATH/G6/g6-u01-l01-abc-source-alternatives-and-practice-handoff-20261030.json'),'utf8'));
  assert.equal(info.status.A_CLOSED,1);
  assert.equal(info.status.B_CLOSED,1);
  assert.equal(info.status.C_PREPARED,0);
  assert.equal(info.status.READY_FOR_STAGE_D,0);
  assert.equal(info.C.selectedContractProof,undefined); // proof is nested in proposedContract
  assert.equal(info.C.proposedContract.selectedContractProof,'NOT_RUN_UNASSIGNED_SINGLE_WRITER');
  assert.equal(info.B.authenticatedRender,'NOT_RUN_STAGE_D');
  assert.equal(info.firstPartyAsset.endsWith('.svg'),true);
  assert.equal(info.assetEmbeddedInExistingRichV2,false);
  assert.ok(l1.ExplanationParagraphs.some(p => /SQUARE units/.test(p) && /TRIANGLE/.test(p)));
  assert.equal(ledger.lessons.length,147);
  assert.equal(ledger.summary.abcFirstOrder.A_CLOSED,4);
  assert.equal(ledger.summary.abcFirstOrder.B_CLOSED,4);
  assert.equal(ledger.summary.abcFirstOrder.C_PREPARED,1);
  assert.equal(ledger.summary.abcFirstOrder.READY_FOR_STAGE_D,1);
});
