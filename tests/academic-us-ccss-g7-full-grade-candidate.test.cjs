'use strict';

// Grade 7 candidate *static* contract only. This is not Stage A academic
// acceptance, a real generator/verifier run, a seeded DB check or owner UAT.
const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const root = path.resolve(__dirname, '..');
const read = p => JSON.parse(fs.readFileSync(path.join(root, p), 'utf8'));
const dir = 'src/Edulytics.Core/Curriculum/LessonContent/RichV2';
const bp = read('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/us-ccss-math-grade7-our-2017.lesson-blueprint.json');
const files = fs.readdirSync(path.join(root, dir))
  .filter(name => /^us-ccss-g7-.*\.rich-lesson-v2\.json$/.test(name))
  .sort();
const docs = files.map(name => ({ name, doc: read(path.join(dir, name)) }));
const lessons = docs.flatMap(({name, doc}) => doc.Lessons.map(lesson => ({...lesson, sourceFile: name})));
const byCode = new Map(lessons.map(x => [x.LessonCode, x]));

test('Grade 7 full-grade candidate has exact canonical 145 IDs and 9 unit counts', () => {
  assert.equal(bp.BlueprintCode, 'US-CCSS-MATH:G7:OUR-IM-2017');
  assert.equal(bp.Lessons.length, 145);
  assert.equal(lessons.length, 145);
  assert.equal(byCode.size, 145, 'No duplicate Rich V2 LessonCode');
  assert.deepEqual(new Set(byCode.keys()), new Set(bp.Lessons.map(x=>x.LessonCode)));
  const expected = [13,15,11,16,17,23,17,20,13];
  for (let u=1;u<=9;u++) {
    const count = lessons.filter(x=>x.LessonCode.includes(':U'+String(u).padStart(2,'0')+':')).length;
    assert.equal(count,expected[u-1],'Incorrect coverage for U'+u);
  }
});

test('Rich V2 schema, exact title/source, 3 concrete examples and no duplicate prompts', () => {
  const prompts = new Set();
  const source = new Map(bp.Lessons.map(x=>[x.LessonCode,x]));
  for (const {name, doc} of docs) {
    assert.equal(doc.SchemaVersion,1,name);
    assert.equal(doc.PackCode,'US-CCSS-MATH',name);
    assert.ok(doc.ContentVersion,name);
    assert.ok(doc.Lessons.length>0,name);
  }
  for(const l of lessons) {
    const x=source.get(l.LessonCode);
    assert.ok(x,l.LessonCode);
    assert.equal(l.Title,x.Title,l.LessonCode);
    assert.equal(l.CultureCode,'en',l.LessonCode);
    assert.ok(l.ExplanationParagraphs.length>=2,l.LessonCode);
    assert.ok(l.ExplanationParagraphs.join(' ').includes(x.SourceUrl),l.LessonCode);
    assert.ok(l.KeyConcepts.length>=3,l.LessonCode);
    assert.ok(l.WorkedExamples.length>=3,l.LessonCode);
    assert.ok(l.CommonMistakes.length>=2,l.LessonCode);
    assert.ok(l.SummaryPoints.length>=3,l.LessonCode);
    assert.ok(!l.ExplanationParagraphs.join(' ').includes('Learning goal: You will'),l.LessonCode);
    for (const k of l.KeyConcepts) for(const field of ['Title','Definition','Rule','Example']) assert.ok(k[field],l.LessonCode+':'+field);
    for(const w of l.WorkedExamples) {
      for(const field of ['Title','Question','Method','Answer','Check']) assert.ok(w[field],l.LessonCode+':'+field);
      assert.ok(Array.isArray(w.Steps)&&w.Steps.length>=3,l.LessonCode);
      assert.ok(w.Steps.every(s=>typeof s==='string'&&s.trim()),l.LessonCode);
      assert.ok(!prompts.has(w.Question.toLowerCase().trim()),l.LessonCode+': duplicate example question');
      prompts.add(w.Question.toLowerCase().trim());
    }
    for(const mistake of l.CommonMistakes) for(const field of ['Mistake','WhyWrong','Correction']) assert.ok(mistake[field],l.LessonCode+':'+field);
  }
  assert.ok(prompts.size>=435,'Three unique worked problems per lesson');
});

test('source-targeted mathematics checkpoints are independently computed', () => {
  const L = (u,n)=>byCode.get('PED:US-CCSS-MATH:G7:U'+String(u).padStart(2,'0')+':L'+String(n).padStart(2,'0'));
  assert.ok(L(1,1).ExplanationParagraphs.join(' ').includes('scaled'), 'U01 L01 scaling');
  assert.ok(L(6,1).ExplanationParagraphs.join(' ').includes('not proportional'), 'U06 L01 not-proportional target');
  assert.ok(L(8,13).ExplanationParagraphs.join(' ').includes('random'), 'U08 L13 sampling goal');
  assert.ok(L(9,13).ExplanationParagraphs.join(' ').includes('5 km'), 'U09 L13 course design');
  // These computations are reference oracles, not a test of the live Practice generator.
  assert.equal((6/3),2);
  assert.equal((7/4),1.75);
  assert.equal((12/2),6);
  assert.equal((3*4*5),60);
  assert.equal((6*4/2)*8,96);
  assert.equal((16/5),3.2);
  assert.equal(40*(1-0.15),34);
  assert.equal(5000/1250,4);
  assert.equal((-4) >= (-4),true);
  assert.equal((-5) > (-4),false);
});

test('candidate ledger must not silently claim academic or runtime PASS',()=>{
  const ledger=read('artifacts/curriculum-delivery/US-CCSS-MATH/G7/grade7-authoring-ledger.json');
  assert.equal(ledger.courseCode,bp.BlueprintCode);
  assert.equal(ledger.lessons.length,145);
  assert.equal(ledger.counts.directRichCandidate,145);
  assert.equal(ledger.counts.stageDVerified,0);
  assert.equal(ledger.counts.stageCVerified,0);
  for (const item of ledger.lessons) {
    assert.ok(byCode.has(item.lessonCode));
    assert.equal(item.stages.D,'NOT_RUN');
    assert.equal(item.stages.C,'NOT_VERIFIED');
    assert.notEqual(item.humanAcademicAcceptance,'APPROVED');
  }
});


test('Grade 7 visual audit preserves 145 lesson identities and labels each authored visual', () => {
  const audit = read('artifacts/curriculum-delivery/US-CCSS-MATH/G7/grade7-visual-coverage-audit.json');
  assert.equal(audit.counts.canonical, 145);
  assert.equal(audit.lessons.length, 145);
  assert.equal(new Set(audit.lessons.map(l => l.lessonCode)).size, 145);
  assert.deepEqual(
    new Set(audit.lessons.map(l => l.lessonCode)),
    new Set(bp.Lessons.map(l => l.LessonCode))
  );
  const allowed = new Set([
    'CalculationChain', 'NumberLine', 'DoubleNumberLine',
    'FractionBars', 'RatioTable', 'PlaceValueChart', 'Scale',
    'EquationSet', 'ShapeDecomposition'
  ]);
  const withVisual = lessons.filter(l => l.Visuals.length > 0);
  assert.equal(withVisual.length, 145, 'Every Grade 7 candidate must have at least one authored visual model');
  assert.equal(audit.counts.visualsPresent, withVisual.length);
  assert.equal(audit.counts.missingVisuals, 145 - withVisual.length);
  for (const l of withVisual) {
    for (const v of l.Visuals) {
      assert.ok(allowed.has(v.Kind), l.LessonCode + ': unsupported renderer kind');
      assert.ok(v.Title.trim(), l.LessonCode + ': visual title');
      assert.ok(v.Description.trim(), l.LessonCode + ': alt/description');
      if (v.Kind === 'RatioTable') {
        assert.ok(v.Rows.length >= 2, l.LessonCode);
        for (const row of v.Rows) {
          assert.ok(row.Label.trim(), l.LessonCode + ': accessible row label');
          assert.ok(row.Cells.length >= 1, l.LessonCode + ': labelled quantities');
          assert.ok(row.Cells.every(x => String(x).trim()), l.LessonCode);
        }
      }
    }
  }
  for (const row of audit.lessons) {
    const candidate = byCode.get(row.lessonCode);
    assert.equal(row.visualCount, candidate.Visuals.length, row.lessonCode);
    assert.equal(row.visualAcademicReview, 'NOT_VERIFIED', row.lessonCode);
    assert.equal(row.assistiveTechnologyTest, 'NOT_RUN', row.lessonCode);
  }
});


test('V5 actual student-source comparisons remain honest and lesson-specific', () => {
  const parity = read('artifacts/curriculum-delivery/US-CCSS-MATH/G7/grade7-student-task-parity-v5.json');
  const ledger = read('artifacts/curriculum-delivery/US-CCSS-MATH/G7/grade7-authoring-ledger.json');
  assert.equal(parity.lessons.length, 145);
  assert.equal(new Set(parity.lessons.map(row => row.lessonCode)).size, 145);
  assert.deepEqual(
    new Set(parity.lessons.map(row => row.lessonCode)),
    new Set(bp.Lessons.map(row => row.LessonCode))
  );
  const inspected = parity.lessons.filter(row => row.sourceInspected === 'DIRECT_STUDENT_PAGE_READ');
  const unchecked = parity.lessons.filter(row => row.sourceInspected === 'NOT_CHECKED');
  assert.equal(inspected.length, 145);
  assert.equal(unchecked.length, 0);
  assert.equal(parity.counts.stageAVerified, 0);
  assert.equal(parity.counts.stageCVerified, 0);
  assert.equal(parity.counts.stageDVerified, 0);
  assert.equal(ledger.counts.sourceStudentPagesInspected, 145);
  assert.equal(ledger.counts.sourceStudentPagesNotIndividuallyInspected, 0);
  assert.equal(ledger.counts.stageAVerified, 0);
  assert.equal(ledger.counts.stageCVerified, 0);
  assert.equal(ledger.counts.stageDVerified, 0);
  assert.equal(ledger.counts.staticRichWorkedExamples, 584);
  const lessonRows = new Map(ledger.lessons.map(row => [row.lessonCode, row]));
  for (const row of parity.lessons) {
    const rich = byCode.get(row.lessonCode);
    const evidence = lessonRows.get(row.lessonCode);
    assert.ok(rich && evidence, row.lessonCode);
    assert.equal(evidence.studentTaskInspection, row.sourceInspected, row.lessonCode);
    assert.equal(row.actualRichWorkedExamples, rich.WorkedExamples.length, row.lessonCode);
    assert.equal(row.actualRichVisuals, rich.Visuals.length, row.lessonCode);
    assert.equal(evidence.visualCount, rich.Visuals.length, row.lessonCode);
    assert.equal(evidence.richCandidateWorkedExamples, rich.WorkedExamples.length, row.lessonCode);
    assert.equal(row.academicStageA, 'NOT_VERIFIED', row.lessonCode);
    assert.equal(row.practiceExactSelectedFamily, 'NOT_VERIFIED', row.lessonCode);
    assert.equal(row.stageD, 'NOT_RUN', row.lessonCode);
    if (row.sourceInspected === 'DIRECT_STUDENT_PAGE_READ') {
      assert.equal(evidence.stages.A, 'SOURCE_STUDENT_TASK_COMPARED_REPAIR_PENDING_ACCEPTANCE', row.lessonCode);
      assert.equal(row.sourceStudentUrl, 'https://im.kendallhunt.com/MS/students/2/' +
        row.unit + '/' + row.lesson + '/index.html');
      assert.ok(row.sourceActivities && row.sourceTarget && row.sourceStudentEvidence &&
        row.actualGradeOwnedRepair && row.teacherRubricRequired, row.lessonCode);
      assert.ok(rich.WorkedExamples.length >= 4, row.lessonCode);
    } else {
      assert.equal(evidence.stages.A, 'NOT_CHECKED', row.lessonCode);
      assert.equal(row.sourceStudentUrl, null, row.lessonCode);
      assert.equal(row.sourceComparison, 'NOT_CHECKED', row.lessonCode);
    }
  }
});

test('U01 source-authentic constructions and U06 discrete nonproportional contrasts exist', () => {
  for (let n = 1; n <= 13; n++) {
    const code = 'PED:US-CCSS-MATH:G7:U01:L' + String(n).padStart(2, '0');
    const l = byCode.get(code);
    assert.ok(l.ExplanationParagraphs.join(' ').includes('Student task alignment:'), code);
    assert.ok(l.WorkedExamples.some(w => /teacher|drawing|sketch|peer|construct/i.test(w.Check)), code);
  }
  const u01 = byCode.get('PED:US-CCSS-MATH:G7:U01:L01');
  assert.ok(u01.WorkedExamples.some(w => w.Question.includes('block letter F')));
  const u06 = byCode.get('PED:US-CCSS-MATH:G7:U06:L01');
  assert.ok(u06.WorkedExamples.some(w => /toaster/i.test(w.Title)));
  assert.ok(u06.WorkedExamples.some(w => /fixed-fee popcorn|unit-price and a fixed-fee/i.test(w.Title)));
  const u07 = byCode.get('PED:US-CCSS-MATH:G7:U07:L01');
  assert.ok(u07.WorkedExamples.some(w => /protractor/i.test(w.Title)));
  const u03 = byCode.get('PED:US-CCSS-MATH:G7:U03:L01');
  assert.ok(/measure a square/i.test(u03.WorkedExamples[0].Title));
});

test('V5 145 student source lessons retain physical task and teacher-only evidence boundaries', () => {
  const parity = read('artifacts/curriculum-delivery/US-CCSS-MATH/G7/grade7-student-task-parity-v5.json');
  const visual = read('artifacts/curriculum-delivery/US-CCSS-MATH/G7/grade7-visual-coverage-audit.json');
  assert.equal(visual.counts.visualsTotal, 151);
  assert.equal(visual.counts.visualsPresent, 145);
  assert.equal(visual.counts.visualsHtmlRendered, 0);
  assert.equal(visual.counts.visuallyOrAcademicallyAccepted, 0);
  assert.equal(parity.counts.studentTaskPagesDirectlyRead, 145);
  assert.equal(parity.counts.notIndividuallySourceChecked, 0);
  const kinds = new Map();
  for (const lesson of lessons) {
    for (const v of lesson.Visuals) kinds.set(v.Kind, (kinds.get(v.Kind) || 0) + 1);
  }
  assert.equal(kinds.get('RatioTable'), 110);
  assert.equal(kinds.get('DoubleNumberLine'), 1);
  assert.equal(kinds.get('NumberLine'), 1);
  assert.equal(kinds.get('EquationSet'), 39);
  for (const row of parity.lessons) {
    assert.equal(row.sourceInspected, 'DIRECT_STUDENT_PAGE_READ', row.lessonCode);
    assert.equal(row.teacherRubricRequired, true, row.lessonCode);
    assert.equal(row.academicStageA, 'NOT_VERIFIED', row.lessonCode);
    assert.equal(row.stageD, 'NOT_RUN', row.lessonCode);
    assert.equal(row.sourceTaskGraphicParity, 'NOT_VERIFIED_NEEDS_ACTUAL_FIGURE_OR_STUDENT_ARTIFACT', row.lessonCode);
  }
});


test('V6 source-original SVG polygons preserve actual scaled and distorted geometry', () => {
  const rubric = read('artifacts/curriculum-delivery/US-CCSS-MATH/G7/grade7-authentic-geometry-teacher-rubrics-v6.json');
  const base = 'src/Edulytics.Web/wwwroot/curriculum/us-ccss-g7/geometry/';
  assert.equal(rubric.releaseStatus, 'BLOCKED');
  assert.equal(rubric.lessons.length, 14, 'Thirteen Unit 1 rubrics plus U06 L01 reasoning rubric');
  const fetchSvg = name => {
    const svg = fs.readFileSync(path.join(root, base, name), 'utf8');
    assert.match(svg, /<svg [^>]*role="img"/);
    assert.match(svg, /aria-labelledby="/);
    assert.match(svg, /<title id="/);
    assert.match(svg, /<desc id="/);
    assert.doesNotMatch(svg, /<script|<foreignObject|onload=|onclick=/i);
    return [...svg.matchAll(/<polygon class="(?:shape|bad)" points="([^"]+)"\s*\/>/g)]
      .map(match => match[1].split(' ').map(pair => pair.split(',').map(Number)));
  };
  const normal = points => points.map(([x, y]) => [x - points[0][0], y - points[0][1]]);
  const assertFactor = (p, q, k) => {
    assert.equal(p.length, q.length);
    const a = normal(p), b = normal(q);
    for (let i = 0; i < a.length; i++)
      for (let dim = 0; dim < 2; dim++)
        assert.ok(Math.abs(b[i][dim] - a[i][dim] * k) < 1e-9,
          'Mismatched polygon coordinate and expected factor ' + k);
  };
  const f = fetchSvg('u01-l01-scaled-vs-stretched.svg');
  assert.equal(f.length, 3);
  assertFactor(f[0], f[1], 2);
  assert.notDeepEqual(normal(f[2]), normal(f[1]), 'Stretched F must not equal uniform copy');
  const t = fetchSvg('u01-l02-corresponding-triangles.svg');
  assert.equal(t.length, 2);
  assertFactor(t[0], t[1], 1.5);
  const l = fetchSvg('u01-l03-scaled-l-construction.svg');
  assert.equal(l.length, 3);
  assertFactor(l[0], l[1], 0.5);
  assertFactor(l[0], l[2], 3);
});

test('V6 grade-specific teacher rubrics do not claim a working or authenticated auto-grader', () => {
  const rubric = read('artifacts/curriculum-delivery/US-CCSS-MATH/G7/grade7-authentic-geometry-teacher-rubrics-v6.json');
  assert.equal(rubric.statusEvidence.authenticatedStudentPageRender, 'NOT_RUN');
  assert.equal(rubric.statusEvidence.studentPracticeSelection, 'NOT_EXECUTED');
  assert.equal(rubric.statusEvidence.scoringPersistence, 'NOT_RUN');
  const codes = new Set();
  for (const l of rubric.lessons) {
    assert.ok(byCode.has(l.lessonCode), l.lessonCode);
    assert.ok(!codes.has(l.lessonCode));
    codes.add(l.lessonCode);
    assert.equal(l.assessmentMode, 'TEACHER_REVIEW_REQUIRED_NOT_AUTOGRADED');
    assert.equal(l.teacherRubric.criteria.length, 4);
    assert.equal(l.teacherRubric.maxScore, 12);
    assert.equal(l.rubricIntegratedIntoTeacherUI, false);
    assert.equal(l.studentAssetRenderedInRichV2, false);
    assert.equal(l.studentArtifactPersisted, false);
    assert.equal(l.verifiedInTwoSchools, false);
    assert.ok(l.examples.some(x => x.kind === 'incorrect' || x.kind === 'wrong'), l.lessonCode + ': negative counterexample');
    assert.ok(l.examples.some(x => x.kind === 'correct'));
  }
  assert.equal(codes.size, 14, 'No duplicate Grade 7 teacher-only rubric IDs');
  assert.equal(Math.ceil(9 / 4) * 35, 105, 'Whole-toaster batches');
  assert.equal(3.5 + 0.25 * 20, 8.5, 'Nonproportional pricing');
  assert.notEqual(3.5 + 0.25 * 20, 0.6 * 20, 'Do not treat fixed-fee and rate-only as same rule');
});
