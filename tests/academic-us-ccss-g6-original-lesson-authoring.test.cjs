'use strict';

// US-CCSS-MATH Grade 6 candidate guards only.
// These tests verify repository JSON structure and source-specific authoring
// invariants. They do NOT certify exact Practice, runtime rendering, .NET,
// authenticated routes, teacher assessment, or production state.

const assert = require('node:assert/strict');
const test = require('node:test');
const fs = require('node:fs');
const path = require('node:path');

const root = path.resolve(__dirname, '..');
const blueprint = JSON.parse(fs.readFileSync(path.join(root,
  'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/us-ccss-math-grade6-our-2017.lesson-blueprint.json'), 'utf8'));
const pack = JSON.parse(fs.readFileSync(path.join(root,
  'src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g6-phase29-v1.lesson-content-pack.json'), 'utf8'));
const byCode = new Map(pack.lessons.map(x => [x.lessonCode, x]));
const lcode = (u, l) => `PED:US-CCSS-MATH:G6:U${String(u).padStart(2, '0')}:L${String(l).padStart(2, '0')}`;
const target = [
  [1,1, 'compare areas covered by different tiling shapes'],
  [1,2, 'define area precisely, compare same-area figures'],
  [1,3, 'reason about shaded polygon areas'],
  [2,1, 'describe two quantities with precise ratio language'],
  [3,1, 'use a rate per one unit'],
  [4,1, 'estimate how changing a positive divisor'],
  [5,1, 'estimate and calculate with decimals'],
  [6,1, 'draw and interpret tape diagrams'],
  [7,1, 'represent positive and negative quantities'],
  [8,1, 'collect and distinguish categorical and numerical data'],
  [9,1, 'construct, compare and critique Fermi estimates']
];

test('Grade 6 canonical identity and nine-unit inventory are exact', () => {
  assert.equal(blueprint.BlueprintCode, 'US-CCSS-MATH:G6:OUR-IM-2017');
  assert.equal(blueprint.NativeLevel, 'Grade 6');
  assert.equal(blueprint.LogicalLevel, 7);
  assert.equal(pack.packCode, 'US-CCSS-MATH');
  assert.equal(pack.versionCode, 'CCSSM-2010');
  assert.equal(blueprint.Units.length, 9);
  assert.equal(blueprint.Lessons.length, 147);
  assert.equal(pack.lessons.length, 147);
  assert.equal(byCode.size, 147);
  assert.deepEqual(
    [...byCode.keys()].sort(),
    blueprint.Lessons.map(x => x.LessonCode).sort()
  );
  const expected = [19,17,17,17,15,19,19,18,6];
  expected.forEach((count, i) => {
    const inUnit = blueprint.Lessons.filter(l => l.UnitNumber === i + 1);
    assert.equal(inUnit.length, count, `U${i+1} count`);
    assert.deepEqual(inUnit.map(l => l.LessonNumber), Array.from({length: count}, (_, n) => n + 1));
  });
});

test('Every English lesson has a mathematical action goal and preserved source provenance', () => {
  assert.match(pack.contentVersion, /^us-ccss-g6-full-grade-authoring-candidate-/);
  for (const canonical of blueprint.Lessons) {
    const lesson = byCode.get(canonical.LessonCode);
    const en = lesson.translations.find(t => t.cultureCode === 'en');
    assert.ok(en, canonical.LessonCode);
    const first = en.explanation.split('\n', 1)[0];
    assert.match(first, /^Learning goal: You will (?:[a-z][^\n]{15,})\.$/, canonical.LessonCode);
    assert.doesNotMatch(first, /You will (?:the |a |an |you |are |of |what |which |\w+ing[.])/i, canonical.LessonCode);
    assert.equal(lesson.sourceUrl, canonical.SourceUrl);
    assert.equal(lesson.sourceRights, 'CC BY 4.0');
    assert.ok(lesson.sourceSha256 && lesson.canonicalBodySha256, canonical.LessonCode);
    assert.equal(lesson.translations.length, 1);
  }
});

test('Eleven original, lesson-targeted examples and strategies replace template teaching', () => {
  for (const [u, l, phrase] of target) {
    const lesson = byCode.get(lcode(u, l));
    assert.ok(lesson);
    const en = lesson.translations[0];
    assert.ok(en.explanation.startsWith('Learning goal: You will ' + phrase), lesson.lessonCode);
    assert.ok(en.workedExamples.includes('Example 1'), lesson.lessonCode);
    assert.ok(en.workedExamples.includes('Example 2'), lesson.lessonCode);
    assert.ok(en.stepByStepSolutions.includes('Step 5:'), lesson.lessonCode);
    assert.doesNotMatch(en.stepByStepSolutions, /Step 1: Identify what the task asks you to find or show\./);
    assert.doesNotMatch(en.commonMistakes, /Common mistake: jumping to an answer before checking/);
    assert.ok(en.quickSummary.length >= 70);
  }
});

test('Area source lessons explicitly teach area, units, and conservation, not only classification', () => {
  for (let l = 1; l <= 3; l++) {
    const en = byCode.get(lcode(1, l)).translations[0];
    assert.match(en.explanation, /area/i);
    assert.match(en.workedExamples, /square units|square unit|triangular (?:area )?units/i);
    assert.match(en.keyConceptsAndRules, /non-overlapping|overlap/i);
    assert.doesNotMatch(en.stepByStepSolutions, /classify, construct, or reason about the figure/);
  }
});

test('Fermi modeling is explicitly marked unsuitable for single-value automatic assessment', () => {
  const en = byCode.get(lcode(9, 1)).translations[0];
  assert.match(en.explanation, /human review/i);
  assert.match(en.workedExamples, /Student-estimation prompt/i);
  assert.match(en.commonMistakes, /One numerical result cannot establish/i);
});

test('Explicit Rich V2 U01 L01-L03 area lessons take precedence over generated shape-properties examples', () => {
  const rich = JSON.parse(fs.readFileSync(path.join(root,
    'src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g6-u01-l01-l03-area-20261028.rich-lesson-v2.json'), 'utf8'));
  assert.equal(rich.SchemaVersion, 1);
  assert.equal(rich.PackCode, 'US-CCSS-MATH');
  assert.equal(rich.Lessons.length, 3);
  assert.deepEqual(rich.Lessons.map(l => l.LessonCode), [lcode(1, 1), lcode(1, 2), lcode(1, 3)]);
  rich.Lessons.forEach(lesson => {
    assert.equal(lesson.CultureCode, 'en');
    assert.ok(lesson.ExplanationParagraphs.length >= 2);
    assert.ok(lesson.KeyConcepts.length >= 3);
    assert.ok(lesson.WorkedExamples.length >= 3);
    assert.ok(lesson.CommonMistakes.length >= 2);
    assert.ok(lesson.SummaryPoints.length >= 3);
    assert.ok(lesson.Visuals.length >= 1);
    lesson.WorkedExamples.forEach(ex => {
      assert.ok(ex.Title && ex.Question && ex.Method && ex.Answer);
      assert.ok(ex.Steps.length >= 3);
    });
    assert.match(lesson.ExplanationParagraphs.join(' '), /area/i);
    assert.ok(lesson.Visuals.every(v => v.Description && v.Description.length > 40));
  });
});

test('All nineteen first-party Unit 1 lessons have individualized methods and misconception guidance', () => {
  for (let i = 1; i <= 19; i++) {
    const en = byCode.get(lcode(1, i)).translations[0];
    assert.match(en.explanation, /area|triangle|prism|cube|polygon|tent|polyhedra|nets/i, `U01:L${i}`);
    assert.match(en.workedExamples, /Example 1/, `U01:L${i}`);
    assert.match(en.workedExamples, /Example 2/, `U01:L${i}`);
    assert.doesNotMatch(en.stepByStepSolutions, /Step 1: Identify what the task asks you to find or show\./, `U01:L${i}`);
    assert.doesNotMatch(en.stepByStepSolutions, /Step 3: Use the attributes to classify, construct, or reason about the figure\./, `U01:L${i}`);
    assert.doesNotMatch(en.commonMistakes, /^Common mistake:/, `U01:L${i}`);
    assert.ok(en.quickSummary.length >= 80, `U01:L${i}`);
  }
});

test('Unit 1 numeric teaching invariants are mathematically consistent', () => {
  assert.equal(7 * 4, 32 - 2 * (1 * 4 / 2)); // parallelogram area: cut versus enclosure
  assert.equal(0.5 * 8 * 5, 20);             // matched triangle area
  assert.equal(2 * (12 + 8 + 6), 52);       // prism face areas
  assert.equal(2 * (6 + 3 + 2), 22);        // second prism net
  assert.equal(2 * (1 + 8 + 8), 34);        // 1-by-1-by-8 surface
  assert.equal(6 * 4, 24);                 // 2-by-2-by-2 surface
  assert.equal(6 * (3 ** 2), 54);          // cube surface
  assert.equal(3 ** 3, 27);                // cube volume
  assert.equal(2 * 3 * 2.5 + 2 * (0.5 * 4 * 1.5), 21); // tent model, no floor
});

test('Every one of the 147 Grade 6 lessons has original worked teaching and a five-step mathematical procedure', () => {
  const counts = new Map();
  for (const lesson of pack.lessons) {
    const en = lesson.translations.find(x => x.cultureCode === 'en');
    const code = lesson.lessonCode;
    assert.ok(en, code);
    assert.match(en.explanation, /^Learning goal: You will [a-z][^\n]+\.\n/, code);
    assert.ok(en.explanation.length >= 250, code + ': explanation');
    assert.ok((en.keyConceptsAndRules.match(/(?:^|\n)- /g) || []).length >= 3, code + ': key concepts');
    assert.match(en.workedExamples, /Example 1\b/, code);
    assert.match(en.workedExamples, /Example 2\b/, code);
    assert.equal((en.stepByStepSolutions.match(/Step [1-5]:/g) || []).length, 5, code + ': steps');
    assert.doesNotMatch(en.stepByStepSolutions, /Step 1: Identify what the task asks you to find or show\./, code);
    assert.doesNotMatch(en.stepByStepSolutions, /Step 1: Identify the figure and the attributes named in the task\./, code);
    assert.ok(en.commonMistakes.length >= 80, code + ': misconception');
    assert.doesNotMatch(en.commonMistakes, /^Common mistake: jumping/, code);
    assert.ok(en.quickSummary.length >= 60, code + ': summary');
    assert.doesNotMatch(en.explanation + en.keyConceptsAndRules + en.workedExamples,
      /so nine\?|hour and minute hands|the addends are the quantities being combined/i, code);
    const prefix = code.match(/:U(\d\d):L\d\d$/);
    assert.ok(prefix, code);
    counts.set(prefix[1], (counts.get(prefix[1]) || 0) + 1);
  }
  assert.deepEqual([...counts.entries()].sort(), [
    ['01', 19], ['02', 17], ['03', 17], ['04', 17], ['05', 15],
    ['06', 19], ['07', 19], ['08', 18], ['09', 6]
  ]);
});

test('Critical Grade 6 arithmetic and statistics examples have valid independent numeric checks', () => {
  assert.equal(2 * (4 * 3 + 4 * 2 + 3 * 2), 52);         // prism surface area
  assert.equal(3 / 4 / (2 / 3), 9 / 8);                // fraction division
  assert.equal(3 / 2 * 2 / 3, 1);                    // reciprocal quotient check
  assert.ok(Math.abs(4.32 / 0.12 - 36) < 1e-10);       // decimal-divisor scaling
  assert.equal((3 + 5 + 7) / 3, 5);                   // mean
  assert.equal((2 + 0 + 2) / 3, 4 / 3);               // MAD
  assert.equal((2 * 6 + 2 * 8 + 2 * 7) / 2, 21);     // tent fabric model
  assert.equal(120 * 0.4, 48);                       // proportional scaling
  assert.ok(Math.abs(40 * 0.18 - 7.2) < 1e-10);      // percentage
  assert.equal((30 / 120) * 100, 25);                // find percentage
});

test('Full-grade authoring is not confused with source, Practice or production approval', () => {
  const record = JSON.parse(fs.readFileSync(path.join(root,
    'artifacts/curriculum-delivery/US-CCSS-MATH/G6/grade6-147-academic-authoring-ledger.json'), 'utf8'));
  assert.equal(record.lessons.length, 147);
  assert.equal(record.summary.canonicalLessons, 147);
  assert.equal(record.summary.substantiveLessonsRepaired, 147);
  assert.equal(record.summary.fullStageAReviews, 0);
  assert.equal(record.summary.stageDVerified, 0);
  assert.equal(record.summary.academicallyAccepted, 0);
  assert.equal(record.summary.releaseReady, false);
  assert.ok(record.lessons.every(l => l.academicAcceptance === 'NOT_ACCEPTED'));
});

test('Grade 6 public-source area activity boundaries match first-party candidate teaching', () => {
  const crosswalk = JSON.parse(fs.readFileSync(path.join(root,
    'artifacts/curriculum-delivery/US-CCSS-MATH/G6/g6-u01-l01-l03-public-source-task-practice-boundaries.json'), 'utf8'));
  const rich = JSON.parse(fs.readFileSync(path.join(root,
    'src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g6-u01-l01-l03-area-20261028.rich-lesson-v2.json'), 'utf8'));
  assert.equal(crosswalk.status, 'PUBLIC_SOURCE_ACTIVITY_COMPARISON_NOT_ACADEMIC_ACCEPTANCE');
  assert.equal(crosswalk.entries.length, 3);
  assert.deepEqual(crosswalk.entries.map(e => e.lessonCode), [lcode(1,1), lcode(1,2), lcode(1,3)]);
  assert.deepEqual(rich.Lessons.map(e => e.LessonCode), [lcode(1,1), lcode(1,2), lcode(1,3)]);
  crosswalk.entries.forEach((entry, index) => {
    assert.ok(entry.student.includes('/MS/students/1/1/'));
    assert.ok(entry.teacher.includes('/MS/teachers/1/1/'));
    assert.ok(entry.practice.includes('/MS/students/1/1/'));
    assert.ok(entry.observableTasks.length >= 4);
    assert.ok(entry.practiceBoundary.teacherOnly.length >= 2);
    assert.ok(entry.practiceBoundary.forbiddenSubstitutes.length >= 2);
    assert.ok(entry.remainingGaps.length >= 3);
    assert.equal(entry.teacherOnlyRubricCandidate.criteria.length, 4);
    assert.equal(entry.teacherOnlyRubricCandidate.scoreRange.max, 8);
    assert.equal(entry.teacherOnlyRubricCandidate.teacherDecisionRequired, true);
    assert.equal(entry.teacherOnlyRubricCandidate.persistedEvaluationVerified, false);
    assert.ok(entry.teacherOnlyRubricCandidate.criteria.every(c => c.maxPoints === 2 && c.requiredEvidence));
    assert.ok(rich.Lessons[index].WorkedExamples.length >= 3);
    assert.ok(rich.Lessons[index].WorkedExamples.some(e => /teacher.review/i.test(e.Method + ' ' + e.Check)));
  });
  const a = rich.Lessons[0];
  assert.match(a.ExplanationParagraphs.join(' '), /trapezoid/i);
  assert.match(a.WorkedExamples[0].Question, /rhombi/i);
  assert.match(a.WorkedExamples[0].Answer, /9 triangular area units/);
  const b = rich.Lessons[1];
  assert.match(b.WorkedExamples[1].Answer, /1\/2 square unit/);
  const c = rich.Lessons[2];
  assert.match(c.WorkedExamples[0].Answer, /16 square units/);
  assert.equal(3 * 3, 9);
  assert.equal(4 * 2, 8);
  assert.equal(7 * 1, 7);
  assert.equal(16 - 1 + 1, 16);
});

test('All 147 historical public student lesson page titles and activity headings are indexed without academic acceptance', () => {
  const index = JSON.parse(fs.readFileSync(path.join(root,
    'artifacts/curriculum-delivery/US-CCSS-MATH/G6/g6-147-public-student-page-activity-index.json'), 'utf8'));
  assert.equal(index.status ?? index.reviewState, 'AUTOMATED_PUBLIC_STUDENT_PAGE_DISCOVERY_ONLY');
  assert.equal(index.summary.canonicalLessons, 147);
  assert.equal(index.summary.pagesRetrieved, 147);
  assert.equal(index.summary.studentTasksHumanReviewed, 0);
  assert.equal(index.summary.sourceTaskParityApproved, 0);
  assert.equal(index.lessons.length, 147);
  assert.deepEqual(index.lessons.map(x => x.lessonCode), blueprint.Lessons.map(x => x.LessonCode));
  for (let i = 0; i < 147; i++) {
    const row = index.lessons[i], source = blueprint.Lessons[i];
    assert.equal(row.expectedTitle, source.Title);
    assert.equal(row.sourceDocumentTitle, source.Title);
    assert.equal(row.fetchStatus, 'PUBLIC_TEXT_RETRIEVED');
    assert.ok(row.sourceActivityHeadings.length >= 2, row.lessonCode);
    assert.equal(row.sourceLearnerTaskActivityParityApproved, false);
    assert.equal(row.semanticAlignmentManuallyReviewed, false);
    assert.equal(row.sourceImagesVerified, false);
    assert.equal(row.sourceAppletBehaviorVerified, false);
  }
});

test('U01 L12 Rich V2 cabinet and 12-cube surface area teaching has source-specific face exclusions and teacher boundary', () => {
  const rich = JSON.parse(fs.readFileSync(path.join(root,
    'src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g6-u01-l12-exposed-cabinet-and-cubes-20261028.rich-lesson-v2.json'), 'utf8'));
  const source = JSON.parse(fs.readFileSync(path.join(root,
    'artifacts/curriculum-delivery/US-CCSS-MATH/G6/g6-u01-l12-public-source-surface-area-boundary.json'), 'utf8'));
  assert.equal(rich.SchemaVersion, 1);
  assert.equal(rich.PackCode, 'US-CCSS-MATH');
  assert.equal(rich.Lessons.length, 1);
  assert.equal(rich.Lessons[0].LessonCode, lcode(1, 12));
  assert.equal(source.lessonCode, lcode(1, 12));
  assert.equal(source.verification.selectedPracticeContractRun, false);
  assert.equal(source.verification.authStudentRichV2Rendered, false);
  assert.equal(source.verification.academicAcceptance, 'NOT_ACCEPTED');
  assert.equal(source.proposedRubric.teacherJudgmentRequired, true);
  assert.equal(source.proposedRubric.criteria.length, 4);
  const l = rich.Lessons[0];
  assert.ok(l.ExplanationParagraphs.length >= 3);
  assert.ok(l.KeyConcepts.length >= 3);
  assert.ok(l.WorkedExamples.length >= 3);
  assert.ok(l.CommonMistakes.length >= 2);
  assert.ok(l.SummaryPoints.length >= 3);
  assert.ok(l.Visuals.length >= 1);
  assert.match(l.ExplanationParagraphs.join(' '), /excluded bottom|exclude.*bottom|bottom.*excluded/i);
  assert.match(l.WorkedExamples[1].Answer, /32 and 40/);
  assert.match(l.WorkedExamples[2].Check, /Teacher reviews/);
  assert.equal(8 * 5 + 2 * (8 * 3) + 2 * (5 * 3), 118);
  assert.equal(2 * (2 * 2 + 2 * 3 + 2 * 3), 32);
  assert.equal(2 * (1 * 2 + 1 * 6 + 2 * 6), 40);
  assert.equal(2 * 2 * 3, 1 * 2 * 6);
});

test('All 147 public historical teacher pages have indexed activity headings without false source acceptance', () => {
  const teachers = JSON.parse(fs.readFileSync(path.join(root,
    'artifacts/curriculum-delivery/US-CCSS-MATH/G6/g6-147-public-teacher-activity-cooldown-index.json'), 'utf8'));
  assert.equal(teachers.summary.canonicalLessons, 147);
  assert.equal(teachers.summary.pagesRetrieved, 147);
  assert.equal(teachers.summary.titleMismatches, 0);
  assert.equal(teachers.summary.sourceTasksAcademicallyAccepted, 0);
  assert.equal(teachers.lessons.length, 147);
  assert.deepEqual(teachers.lessons.map(x => x.lessonCode), blueprint.Lessons.map(x => x.LessonCode));
  for (let i = 0; i < 147; i++) {
    const row = teachers.lessons[i];
    assert.equal(row.expectedTitle, blueprint.Lessons[i].Title);
    assert.equal(row.sourceDocumentTitle, blueprint.Lessons[i].Title);
    assert.equal(row.fetchStatus, 'PUBLIC_TEXT_RETRIEVED');
    assert.ok(row.teacherActivityHeadings.length >= 2, row.lessonCode);
    assert.equal(row.originalLessonLearningGoalManuallyCompared, false);
    assert.equal(row.teacherAssessmentImplementationVerified, false);
    assert.equal(row.protectedTeacherResponsesVerified, false);
  }
});

test('All ten MP-only Grade 6 source-heading-boundary proposals are distinct and human-scored', () => {
  const assessment = JSON.parse(fs.readFileSync(path.join(root,
    'artifacts/curriculum-delivery/US-CCSS-MATH/G6/g6-ten-mp-only-source-heading-anchored-assessment-boundaries.json'), 'utf8'));
  assert.equal(assessment.summary.mpOnlyOutcomeLessons, 10);
  assert.equal(assessment.summary.sourceActivityBodiesIndependentlyCompared, 0);
  assert.equal(assessment.summary.selectedPracticeVerified, 0);
  assert.equal(assessment.summary.fullyTeacherGradedLessonsAccepted, 0);
  assert.equal(assessment.entries.length, 10);
  const seen = new Set();
  for (const e of assessment.entries) {
    assert.ok(!seen.has(e.lessonCode), 'duplicate assessment boundary: ' + e.lessonCode);
    seen.add(e.lessonCode);
    assert.ok(byCode.has(e.lessonCode), e.lessonCode);
    assert.ok(e.publicStudentActivityHeadings.length >= 2);
    assert.ok(e.publicTeacherActivityAndCooldownHeadings.length >= 2);
    assert.equal(e.teacherRubricCandidate.maxScore, 8);
    assert.equal(e.teacherRubricCandidate.criteria.length, 4);
    assert.equal(e.teacherRubricCandidate.humanEvaluationRequired, true);
    assert.equal(e.teacherRubricCandidate.autoGradingOfFullSourceTaskProhibited, true);
    assert.equal(e.actualSelectedPracticeContract, 'NOT_TRACED');
    assert.equal(e.academicAcceptance, 'NOT_ACCEPTED');
  }
});

test('U09 L01-L02 source-aware Fermi and historical population Rich V2 candidates remain explicitly review-only', () => {
  const rich = JSON.parse(fs.readFileSync(path.join(root,
    'src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g6-u09-l01-l02-fermi-world-20261028.rich-lesson-v2.json'), 'utf8'));
  const source = JSON.parse(fs.readFileSync(path.join(root,
    'artifacts/curriculum-delivery/US-CCSS-MATH/G6/g6-u09-l01-l02-public-source-task-assessment-boundaries.json'), 'utf8'));
  assert.equal(rich.PackCode, 'US-CCSS-MATH');
  assert.equal(rich.SchemaVersion, 1);
  assert.deepEqual(rich.Lessons.map(x => x.LessonCode), [lcode(9,1),lcode(9,2)]);
  assert.deepEqual(source.entries.map(x => x.lessonCode), [lcode(9,1),lcode(9,2)]);
  for (const lesson of rich.Lessons) {
    assert.ok(lesson.ExplanationParagraphs.length >= 3);
    assert.ok(lesson.KeyConcepts.length >= 3);
    assert.ok(lesson.WorkedExamples.length >= 3);
    assert.ok(lesson.CommonMistakes.length >= 2);
    assert.ok(lesson.SummaryPoints.length >= 3);
    assert.ok(lesson.Visuals.length >= 1);
    assert.ok(lesson.WorkedExamples.every(x => x.Steps.length >= 3 && x.Question && x.Method && x.Answer && x.Check));
  }
  source.entries.forEach(e => {
    assert.equal(e.academicAcceptance, 'NOT_ACCEPTED');
    assert.equal(e.assessmentCandidate.maxScore, 8);
    assert.equal(e.assessmentCandidate.realStudentSubmissionVerified, false);
    assert.equal(e.assessmentCandidate.storedTeacherFeedbackVerified, false);
    assert.equal(e.selectedPracticeGeneratorVerifierVerified, false);
  });
  assert.match(rich.Lessons[0].ExplanationParagraphs.join(' '), /cereal/i);
  assert.match(rich.Lessons[0].WorkedExamples[2].Question, /monument/i);
  assert.match(rich.Lessons[1].ExplanationParagraphs.join(' '), /historical 7.4-billion/i);
  assert.match(rich.Lessons[1].WorkedExamples[1].Answer, /2 or 3/i);
  assert.equal(2_000_000_000 * 0.125, 250_000_000);
  assert.equal(25_000_000 * 10, 250_000_000);
  assert.equal(20_000 * 10_000 * 10, 2_000_000_000);
  assert.equal(12 / 30 * 100, 40);
});

test('U01 L15-L16 geometry compares nets, exterior area and cubic volume in both directions', () => {
  const rich = JSON.parse(fs.readFileSync(path.join(root,
    'src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g6-u01-l15-l16-nets-area-volume-20261028.rich-lesson-v2.json'), 'utf8'));
  const review = JSON.parse(fs.readFileSync(path.join(root,
    'artifacts/curriculum-delivery/US-CCSS-MATH/G6/g6-u01-l15-l16-source-nets-area-volume-boundaries.json'), 'utf8'));
  assert.equal(rich.PackCode, 'US-CCSS-MATH');
  assert.deepEqual(rich.Lessons.map(l => l.LessonCode), [lcode(1,15),lcode(1,16)]);
  assert.deepEqual(review.entries.map(l => l.lessonCode), [lcode(1,15),lcode(1,16)]);
  for (const e of review.entries) {
    assert.equal(e.academicAcceptance, 'NOT_ACCEPTED');
    assert.equal(e.teacherAssessment.maxScore, 8);
    assert.equal(e.teacherAssessment.persistedTeacherGradeVerified, false);
    assert.equal(e.selectedPracticeVerified, false);
  }
  for (const l of rich.Lessons) {
    assert.ok(l.ExplanationParagraphs.length >= 3);
    assert.ok(l.KeyConcepts.length >= 3);
    assert.ok(l.WorkedExamples.length >= 3);
    assert.ok(l.CommonMistakes.length >= 2);
    assert.ok(l.SummaryPoints.length >= 3);
    assert.ok(l.WorkedExamples.every(e => e.Steps.length >= 3 && e.Answer && e.Check));
  }
  assert.match(rich.Lessons[0].WorkedExamples[2].Check, /Teacher or peer validates/i);
  assert.match(rich.Lessons[1].ExplanationParagraphs.join(' '), /22 square units/i);
  assert.equal(2*6 + (3+4+5)*6, 84);
  assert.equal(2*(6+12+18), 72);
  assert.equal(2*(9+12+12), 66);
  assert.equal(2*3*6, 3*3*4);
  assert.equal(2*(1+5+5), 2*(2+3+6));
  assert.ok(1*1*5 !== 1*2*3);
});


test('U01 L02 exposes an exact geometric visual description to the existing presentation parser', () => {
  const en = byCode.get(lcode(1,2)).translations.find(t => t.cultureCode === 'en');
  assert.match(en.workedExamples, /\n\nDescription: Tangram area decomposition schematic\./);
  assert.match(en.workedExamples, /two congruent right triangles/);
  assert.match(en.workedExamples, /Each of those two pieces has area one-half square unit/);
  assert.match(en.workedExamples, /The illustrated tangram-style partition is a schematic and is not a measurement scale/);
  assert.doesNotMatch(en.workedExamples, /<script|onerror=|<iframe/i);
  assert.ok(en.workedExamples.includes('four copies have combined area 2 square units'));
  const source = JSON.parse(fs.readFileSync(path.join(root,
    'artifacts/curriculum-delivery/US-CCSS-MATH/G6/g6-u01-l01-l03-public-source-task-practice-boundaries.json'), 'utf8'));
  const entry = source.entries.find(x => x.lessonCode === lcode(1,2));
  assert.ok(entry);
  assert.ok(entry.practiceBoundary.teacherOnly.length >= 2);
  assert.ok(entry.practiceBoundary.forbiddenSubstitutes.length >= 2);
});


test('Unit 1 lessons 04-10 preserve direct public student activity purpose and open drawing rubric boundaries', () => {
  const source = JSON.parse(fs.readFileSync(path.join(root,
    'artifacts/curriculum-delivery/US-CCSS-MATH/G6/g6-u01-l04-l10-public-student-activity-comparisons.json'), 'utf8'));
  assert.equal(source.status, 'SEVEN_PUBLIC_STUDENT_ACTIVITY_BODY_COMPARISONS__NOT_FULL_STAGE_A');
  assert.equal(source.summary.publicStudentBodiesCompared, 7);
  assert.equal(source.summary.academicAcceptance, 0);
  assert.deepEqual(source.entries.map(e => e.lessonCode), Array.from({ length: 7 }, (_, i) => lcode(1,i+4)));
  source.entries.forEach((e, index) => {
    const lesson = byCode.get(e.lessonCode);
    const body = lesson.translations.find(x => x.cultureCode === 'en');
    assert.equal(e.publicStudentActivityBodyCompared, true, e.lessonCode);
    assert.equal(e.teacherGuideBodyCompletelyReviewed, false, e.lessonCode);
    assert.equal(e.sourceAppletFunctionReplayed, false, e.lessonCode);
    assert.equal(e.exactPracticeSelected, false, e.lessonCode);
    assert.equal(e.academicAcceptance, 'NOT_ACCEPTED', e.lessonCode);
    assert.ok(e.studentOriginalActivityPurposes.length >= 3, e.lessonCode);
    assert.ok(e.remainingActivityAndAssessmentChecks.length >= 2, e.lessonCode);
    assert.ok(body.workedExamples.length >= 400, e.lessonCode);
  });
  const en = n => byCode.get(lcode(1,n)).translations[0];
  assert.match(en(4).keyConceptsAndRules, /both pairs of parallel sides|two pairs of parallel opposite sides/);
  assert.match(en(5).workedExamples, /area is 60/);
  assert.match(en(6).explanation, /noncongruent parallelograms/);
  assert.match(en(7).keyConceptsAndRules, /not every triangle pair makes a rectangle/);
  assert.match(en(8).explanation, /SAME area as the original triangle|two entire matching copies/i);
  assert.match(en(9).keyConceptsAndRules, /need not itself pass through the opposite vertex/);
  assert.match(en(10).workedExamples, /index card/);
  const ledger = JSON.parse(fs.readFileSync(path.join(root,
    'artifacts/curriculum-delivery/US-CCSS-MATH/G6/grade6-147-academic-authoring-ledger.json'), 'utf8'));
  assert.equal(ledger.summary.v6Unit6PublicStudentTaskBodiesCompared, 19);
  assert.equal(ledger.summary.v6Unit7PublicStudentTaskBodiesCompared, 19);
  assert.equal(ledger.summary.v6Unit8StudentTaskBodiesCompared, 18);
  assert.equal(ledger.summary.v6Unit9FinalStudentBodiesCompared, 4);
  assert.equal(ledger.summary.directPublicStudentActivityBodiesCompared,
    15 + ledger.summary.v6Unit6PublicStudentTaskBodiesCompared +
    ledger.summary.v6Unit7PublicStudentTaskBodiesCompared +
    ledger.summary.v6Unit8StudentTaskBodiesCompared + ledger.summary.v6Unit9FinalStudentBodiesCompared);
  assert.equal(ledger.summary.directPublicStudentTeacherSourceTaskTextCompared, 8);
  assert.equal(ledger.summary.fullStageAReviews, 0);
  assert.equal(ledger.summary.stageDVerified, 0);
  assert.equal(ledger.summary.academicallyAccepted, 0);
  assert.equal(ledger.summary.releaseReady, false);
});


test('Original Unit 2 ratios source opening screening and eleven activity-specific supports are fail-closed', () => {
  const evidence = JSON.parse(fs.readFileSync(path.join(root,
    'artifacts/curriculum-delivery/US-CCSS-MATH/G6/g6-u02-17-public-student-source-opening-and-repair-matrix.json'), 'utf8'));
  assert.equal(evidence.status, 'PUBLIC_STUDENT_SOURCE_HEADING_AND_OPENING_SCREEN__NOT_FULL_STAGE_A');
  assert.equal(evidence.entries.length, 17);
  assert.equal(evidence.summary.sourceAlignedCandidateExtensionsAuthored, 11);
  assert.equal(evidence.summary.fullTeacherGuidesReviewed, 0);
  assert.equal(evidence.summary.academicAcceptance, 0);
  const edited = new Set([2,5,7,9,10,11,12,13,15,16,17]);
  evidence.entries.forEach((e,index) => {
    assert.equal(e.lessonCode, lcode(2,index+1));
    assert.equal(e.publicSourcePageRetrieved, true);
    assert.equal(e.fullOriginalStudentActivityBodyReviewed, false);
    assert.equal(e.fullOriginalTeacherGuideReviewed, false);
    assert.equal(e.sourceImageAppletsVerified, false);
    assert.equal(e.actualSelectedPracticeContract, 'NOT_TRACED');
    assert.equal(e.academicAcceptance, 'NOT_ACCEPTED');
    assert.equal(e.authoredSourceTargetedExtension, edited.has(index+1));
  });
  const en = n => byCode.get(lcode(2,n)).translations[0];
  assert.match(en(2).workedExamples, /white paint/);
  assert.match(en(5).workedExamples, /half a batch/);
  assert.match(en(7).workedExamples, /11,33/);
  assert.match(en(9).workedExamples, /1.25 m\/s/);
  assert.match(en(10).workedExamples, /0.2 L juice/);
  assert.match(en(11).workedExamples, /400,500/);
  assert.match(en(12).workedExamples, /\$37.80/);
  assert.match(en(13).workedExamples, /600 seconds/);
  assert.match(en(15).workedExamples, /50 mL and blue is 30 mL/);
  assert.match(en(16).workedExamples, /10 chaperones and 75 students/);
  assert.match(en(17).workedExamples, /216,000 pages\/year/);
  assert.ok(Math.abs(3.2 / 4 - 4 / 5) < 1e-10);
  assert.ok(Math.abs(4 / (5 + 1 / 3) - 3 / 4) < 1e-10);
  assert.equal(455 / 5 * 4, 364);
  assert.equal(85 / (2+15), 5);
  assert.equal(2*5+15*5, 85);
});


test('Unit 3 rate, percent above 100 and painting estimates retain sourced task limits', () => {
  const evidence = JSON.parse(fs.readFileSync(path.join(root,
    'artifacts/curriculum-delivery/US-CCSS-MATH/G6/g6-u03-17-public-source-opening-and-repair-matrix.json'), 'utf8'));
  assert.equal(evidence.status, 'PUBLIC_STUDENT_OPENING_TASK_SCREEN_ONLY__NOT_STAGE_A_COMPLETION');
  assert.equal(evidence.lessons.length, 17);
  assert.equal(evidence.summary.targetedCandidateTeachingCorrections, 10);
  assert.equal(evidence.summary.academicAcceptance, 0);
  const target = new Set([2,3,4,5,8,11,12,14,15,17]);
  evidence.lessons.forEach((e, i) => {
    assert.equal(e.lessonCode, lcode(3, i+1));
    assert.equal(e.newGradeSpecificTeachingSupplement, target.has(i+1));
    assert.equal(e.fullPublicStudentActivityBodyReviewed, false);
    assert.equal(e.fullTeacherGuideBodyReviewed, false);
    assert.equal(e.actualSelectedPracticeAndVerifierChecked, false);
    assert.equal(e.academicAcceptance, 'NOT_ACCEPTED');
  });
  const body = n => byCode.get(lcode(3,n)).translations[0];
  assert.match(body(3).workedExamples, /cubic inches/);
  assert.match(body(4).workedExamples, /40 km\/h over the limit/);
  assert.match(body(5).workedExamples, /210 m\/min/);
  assert.match(body(8).workedExamples, /3.75 hours/);
  assert.match(body(11).workedExamples, /150% = \$60/);
  assert.match(body(12).workedExamples, /100% whole is 5×9 = 45 lb/);
  assert.match(body(14).workedExamples, /125%, of the discounted amount/);
  assert.match(body(15).workedExamples, /140% has 350/);
  assert.match(body(17).workedExamples, /268 ft²/);
  assert.match(body(17).workedExamples, /NOT the complete area/);
  assert.equal(75*8/5-80, 40);
  assert.equal(24/6.4, 3.75);
  assert.equal(9/0.2, 45);
  assert.equal(250*1.4, 350);
  assert.equal(2*(9+10)*8-5*3-7*3, 268);
});


test('Unit 4 fraction and packaging source opening checks retain exact mathematical examples and release boundaries', () => {
  const source = JSON.parse(fs.readFileSync(path.join(root,
    'artifacts/curriculum-delivery/US-CCSS-MATH/G6/g6-u04-17-public-student-source-opening-and-repair-matrix.json'), 'utf8'));
  assert.equal(source.lessons.length, 17);
  assert.equal(source.summary.candidateLessonsWithSourceSpecificSupplements, 13);
  assert.equal(source.summary.academicAccepted, 0);
  const revised = new Set([1,3,4,5,6,8,10,12,13,14,15,16,17]);
  source.lessons.forEach((r,i) => {
    assert.equal(r.lessonCode, lcode(4,i+1));
    assert.equal(r.newGrade6TeachingSupplementCommitted, revised.has(i+1));
    assert.equal(r.fullOriginalStudentBodyIndependentlyReviewed, false);
    assert.equal(r.fullTeacherGuideIndependentlyReviewed, false);
    assert.equal(r.selectedExactPracticeQuestionVerified, false);
    assert.equal(r.academicAcceptance, 'NOT_ACCEPTED');
  });
  const lesson = n => byCode.get(lcode(4,n)).translations.find(t => t.cultureCode === 'en');
  assert.match(lesson(1).workedExamples, /5000÷500000=0\.01/);
  assert.match(lesson(3).workedExamples, /nine|=9 jars/);
  assert.match(lesson(4).keyConceptsAndRules, /triangle represents 1\/6/);
  assert.match(lesson(5).keyConceptsAndRules, /rhombus is 2\/3/);
  assert.match(lesson(6).workedExamples, /1½ groups/);
  assert.match(lesson(8).workedExamples, /2½ cups/);
  assert.match(lesson(10).workedExamples, /9\/8/);
  assert.match(lesson(12).workedExamples, /request the piece length/i);
  assert.match(lesson(13).workedExamples, /1\/9 in²/);
  assert.match(lesson(14).workedExamples, /1\/8 in³/);
  assert.match(lesson(15).workedExamples, /108\/27=4in³/);
  assert.match(lesson(16).workedExamples, /17\/12/);
  assert.match(lesson(17).workedExamples, /not establish optimal packing/i);
  assert.equal(9*3*4, 108);
  assert.equal(108/27, 4);
  assert.equal(3*2*4, 24);
  assert.equal(Math.ceil(270/24), 12);
  assert.equal((3/4)/(2/3), 9/8);
});


test('Unit 5 decimal source examples remain place-value accurate and physically scoped', () => {
  const source = JSON.parse(fs.readFileSync(path.join(root,
    'artifacts/curriculum-delivery/US-CCSS-MATH/G6/g6-u05-15-public-student-source-opening-and-repair-matrix.json'), 'utf8'));
  assert.equal(source.lessons.length, 15);
  assert.equal(source.summary.newTargetedSupplements, 14);
  assert.equal(source.summary.academicAccepted, 0);
  const revised = new Set([1,2,3,4,5,6,8,9,10,11,12,13,14,15]);
  source.lessons.forEach((e,i) => {
    assert.equal(e.lessonCode, lcode(5,i+1));
    assert.equal(e.v5SourceTaskCandidateSupplementAuthored, revised.has(i+1));
    assert.equal(e.fullStudentTaskImageAppletFidelityVerified, false);
    assert.equal(e.fullTeacherGuideReviewed, false);
    assert.equal(e.exactGeneratedPracticeAndFeedbackVerified, false);
    assert.equal(e.academicAcceptance, 'NOT_ACCEPTED');
  });
  const en = n => byCode.get(lcode(5,n)).translations.find(x => x.cultureCode === 'en');
  assert.match(en(1).workedExamples, /\$12\.18/);
  assert.match(en(2).workedExamples, /thirteen hundredth squares/i);
  assert.match(en(3).workedExamples, /0\.011/);
  assert.match(en(4).workedExamples, /\$4\.83/);
  assert.match(en(5).workedExamples, /720×0\.01=7\.2/);
  assert.match(en(6).workedExamples, /0\.0987/);
  assert.match(en(8).workedExamples, /11\.55/);
  assert.match(en(9).workedExamples, /657÷3/);
  assert.match(en(10).workedExamples, /1816÷4=454/);
  assert.match(en(11).workedExamples, /\$67\.75/);
  assert.match(en(12).workedExamples, /13\.45/);
  assert.match(en(13).workedExamples, /1\.8÷0\.004=450/);
  assert.match(en(14).workedExamples, /0\.2875/);
  assert.match(en(15).workedExamples, /3\.375/);
  assert.match(en(15).explanation, /cannot be inferred from sheet side lengths alone/);
  assert.ok(Math.abs(2*(3.25+1.85+0.99)-12.18)<1e-10);
  assert.ok(Math.abs(1.009+0.391-1.4)<1e-10);
  assert.ok(Math.abs(0.02-0.0116-0.0084)<1e-10);
  assert.equal(657/3,219);
  assert.ok(Math.abs(53.8/4-13.45)<1e-10);
  assert.ok(Math.abs(77.4/5-15.48)<1e-10);
  assert.ok(Math.abs(4.86/(0.945/3.5)-18)<1e-10);
});


test('Unit 6 complete nineteen original-source activity enhancements remain student-specific and unaccepted', () => {
  const matrices = [
    'g6-u06-l01-l10-student-source-opening-repair-matrix.json',
    'g6-u06-l11-l19-student-source-opening-repair-matrix.json'
  ].map(f => JSON.parse(fs.readFileSync(path.join(root,
    'artifacts/curriculum-delivery/US-CCSS-MATH/G6', f), 'utf8')));
  assert.equal(matrices[0].lessons.length, 10);
  assert.equal(matrices[1].lessons.length, 9);
  const rows = matrices.flatMap(m => m.lessons);
  assert.deepEqual(rows.map(r => r.lessonCode),
    Array.from({length:19}, (_, i) => lcode(6, i + 1)));
  for (const row of rows) {
    const en = byCode.get(row.lessonCode).translations.find(x => x.cultureCode === 'en');
    assert.ok(row.originalStudentActivityHeadings?.length >= 2 ||
              row.sourceStudentOpeningsAndSelectedTaskTextsCompared === true, row.lessonCode);
    assert.match(en.workedExamples, /Example 3\b/, row.lessonCode);
    assert.equal(row.academicAcceptance, 'NOT_ACCEPTED');
    assert.ok(row.actualGrade6StudentRenderVerified === false || row.actualSelectedPracticeContract === 'NOT_RUN');
  }
  assert.match(byCode.get(lcode(6, 2)).translations[0].workedExamples, /66 ordered assignments/);
  assert.match(byCode.get(lcode(6,12)).translations[0].workedExamples, /1,048,576/);
  assert.match(byCode.get(lcode(6,13)).translations[0].workedExamples, /3\^1000 ends in 1/);
  assert.match(byCode.get(lcode(6,16)).translations[0].workedExamples, /r=\(2\/5\)t/);
  assert.match(byCode.get(lcode(6,18)).translations[0].workedExamples, /mosquito/i);
  assert.equal(11 * 12 / 2, 66);
  assert.equal(2 ** 20, 1048576);
  assert.equal((3 ** 4) % 10, 1);
  assert.equal(36 / 9, 4);
  assert.equal(225 / 7.5, 30);
  assert.equal(13 / 4 / (13 / 5), 1.25);
  assert.equal(13 / 4 / (5 / 2), 1.3);
});


test('Unit 7 all nineteen student source-targeted rational and coordinate lessons remain review-only', () => {
  const src = JSON.parse(fs.readFileSync(path.join(root,
    'artifacts/curriculum-delivery/US-CCSS-MATH/G6/g6-u07-19-public-student-source-task-repair-matrix.json'), 'utf8'));
  assert.equal(src.lessons.length, 19);
  assert.equal(src.summary.originalStudentPageBodiesRead, 19);
  assert.equal(src.summary.academicAccepted, 0);
  assert.deepEqual(src.lessons.map(x => x.lessonCode), Array.from({length:19}, (_, i) => lcode(7, i + 1)));
  for (const row of src.lessons) {
    const lesson = byCode.get(row.lessonCode);
    assert.ok(lesson, row.lessonCode);
    assert.ok(row.originalSourceActivityHeadings.length >= 2, row.lessonCode);
    assert.equal(row.originalVisualPixelAppletParityVerified, false);
    assert.equal(row.authStudentRenderingTested, false);
    assert.equal(row.runtimeSelectedPracticeTested, false);
    assert.equal(row.academicAcceptance, 'NOT_ACCEPTED');
    assert.match(lesson.translations[0].workedExamples, /Example 3\b/, row.lessonCode);
  }
  const example = n => byCode.get(lcode(7,n)).translations[0].workedExamples;
  assert.match(example(2), /Folded number line/);
  assert.match(example(7), /information gap/i);
  assert.match(example(9), /52≤h≤72/);
  assert.match(example(10), /3t>8/);
  assert.match(example(12), /taxicab distance/i);
  assert.match(example(16), /GCF\(30,18\)=6/);
  assert.match(example(17), /LCM\(6,8\)=24/);
  assert.match(example(18), /15 kits/);
  assert.match(example(19), /coordinate house/i);
  assert.equal(Math.abs(-4 - 2) + Math.abs(3 - 1), 8);
  assert.equal(2 * (5 + 3), 16);
  assert.equal(75 / 15, 5);
  assert.equal(90 / 15, 6);
  assert.equal(100 / 12 | 0, 8);
});


test('Unit 8 eighteen data-distribution first-party source extensions preserve honest graph and grading limits', () => {
  const evidence = JSON.parse(fs.readFileSync(path.join(root,
    'artifacts/curriculum-delivery/US-CCSS-MATH/G6/g6-u08-18-public-student-source-task-repair-matrix.json'), 'utf8'));
  assert.equal(evidence.lessons.length, 18);
  assert.equal(evidence.summary.comparedStudentSourceBodies, 18);
  assert.equal(evidence.summary.fullAcademicSourceActivityParityAccepted, 0);
  assert.deepEqual(evidence.lessons.map(x => x.lessonCode), Array.from({length:18}, (_, i) => lcode(8, i+1)));
  for (const l of evidence.lessons) {
    assert.ok(l.sourceActivityHeadings.length >= 2, l.lessonCode);
    assert.equal(l.allFiguresAndAppletsRecreated, false);
    assert.equal(l.authenticatedRenderedGraphVerified, false);
    assert.equal(l.selectedPracticeVerifiersAndFeedbackVerified, false);
    assert.equal(l.academicAcceptance, 'NOT_ACCEPTED');
    assert.match(byCode.get(l.lessonCode).translations[0].workedExamples, /Example 3\b/, l.lessonCode);
  }
  const e = n => byCode.get(lcode(8,n)).translations[0].workedExamples;
  assert.match(e(3), /dot plot/i);
  assert.match(e(7), /\[40,60\)/);
  assert.match(e(9), /31÷5=6\.2/);
  assert.match(e(11), /MAD=/);
  assert.match(e(15), /Q1=/);
  assert.match(e(16), /Q3=/);
  assert.match(e(18), /MAD 2/);
  assert.equal((4+5+6+7+9)/5, 6.2);
  assert.equal((3+1+1+3)/4, 2);
  assert.equal((6+6+8+8)/4, 7);
  assert.equal((Math.abs(6-7)+Math.abs(6-7)+Math.abs(8-7)+Math.abs(8-7))/4, 1);
  assert.equal(21-1, 20);
});

test('Unit 9 final four source tasks distinguish numerical vote checks from human modeling assessments', () => {
  const evidence = JSON.parse(fs.readFileSync(path.join(root,
    'artifacts/curriculum-delivery/US-CCSS-MATH/G6/g6-u09-l03-l06-public-source-task-repair-matrix.json'), 'utf8'));
  assert.equal(evidence.lessons.length, 4);
  assert.equal(evidence.summary.studentBodiesCompared, 4);
  assert.equal(evidence.summary.fullOriginalTeacherStudentActivityParityAccepted, 0);
  assert.deepEqual(evidence.lessons.map(x => x.lessonCode), [3,4,5,6].map(n => lcode(9,n)));
  for (const l of evidence.lessons) {
    assert.ok(l.sourceActivityHeadings.length >= 3);
    assert.equal(l.originalDistrictMapsAndVoteSlipsRecreated, false);
    assert.equal(l.authenticatedExactPracticeVerified, false);
    assert.equal(l.teacherRubricPersistedAndGraded, false);
    assert.equal(l.academicAcceptance, 'NOT_ACCEPTED');
    assert.match(byCode.get(l.lessonCode).translations[0].workedExamples, /Example 3\b/);
  }
  const e = n => byCode.get(lcode(9,n)).translations[0].workedExamples;
  assert.match(e(3), /28-by-12/);
  assert.match(e(4), /153\/240=63\.75%/);
  assert.match(e(5), /7 to 5/);
  assert.match(e(6), /12 laptops/);
  assert.equal(2*12*12+3*4*4, 336); // successive 28-by-12 area (288+48)
  assert.ok(Math.abs(153/240*100 - 63.75) < 1e-10);
  assert.equal(240*2/3, 160);
  assert.equal(240*0.55, 132);
  assert.equal(5+4+3, 12);
  assert.equal(22+88, 110);
  assert.ok(Math.abs(0.25*0.20*0.80-0.04)<1e-10);
});


test('U01 L11 polygons source tasks, trapezoid proof and independent visual model retain honest assessment boundaries', () => {
  const crosswalk = JSON.parse(fs.readFileSync(path.join(root,
    'artifacts/curriculum-delivery/US-CCSS-MATH/G6/g6-u01-l11-public-source-task-rich-v2-boundary.json'), 'utf8'));
  const rich = JSON.parse(fs.readFileSync(path.join(root,
    'src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g6-u01-l11-polygons-20261029.rich-lesson-v2.json'), 'utf8'));
  const code = lcode(1, 11);
  const body = byCode.get(code).translations.find(t => t.cultureCode === 'en');
  assert.equal(crosswalk.lessonCode, code);
  assert.equal(crosswalk.studentSourceActivities.length, 5);
  assert.equal(crosswalk.practiceTasks.length, 6);
  assert.equal(crosswalk.reviewLevel, 'PUBLIC_STUDENT_TEACHER_PRACTICE_TASK_BODIES_COMPARED__ACADEMIC_ACCEPTANCE_PENDING');
  assert.equal(crosswalk.implementation.teacherRubric.persistedWorkflowVerified, false);
  assert.ok(crosswalk.practiceTasks.some(p => /teacher/i.test(p.autoGradeScope)));
  assert.equal(rich.SchemaVersion, 1);
  assert.equal(rich.Lessons.length, 1);
  assert.equal(rich.Lessons[0].LessonCode, code);
  assert.equal(rich.Lessons[0].WorkedExamples.length, 3);
  assert.equal(rich.Lessons[0].Visuals.length, 3);
  assert.ok(rich.Lessons[0].Visuals.every(v => v.Description.length > 100));
  assert.match(body.explanation, /never crossing elsewhere/);
  assert.match(body.workedExamples, /Example 3 — Choose an area strategy/);
  assert.match(body.workedExamples, /NOT the historical publisher's exact pinwheel/);
  assert.match(body.commonMistakes, /teacher judgement/);
  assert.match(rich.Lessons[0].WorkedExamples[1].Answer, /18 square centimeters/);
  assert.match(rich.Lessons[0].WorkedExamples[2].Answer, /12 square units/);
  assert.equal((8 + 4) * 3 / 2, 18); // trapezoid duplicate-area proof
  assert.equal(2 * 2 + 4 * (2 * 2 / 2), 12); // original four-wing pinwheel
  assert.equal(crosswalk.releaseStatus, 'NO_GO__NOT_ACADEMICALLY_ACCEPTED');
});


test('U01 L13 L14 L17 L18 L19 match original polyhedron, net, cube and tent-floor task boundaries', () => {
  const record = JSON.parse(fs.readFileSync(path.join(root,
    'artifacts/curriculum-delivery/US-CCSS-MATH/G6/g6-u01-l13-l14-l17-l18-l19-student-teacher-source-gap-repairs.json'), 'utf8'));
  assert.equal(record.summary.compared, 5);
  assert.equal(record.summary.fullActivityAcademicAcceptance, 0);
  assert.deepEqual(record.lessons.map(x => x.lessonCode),
    [13,14,17,18,19].map(n => lcode(1,n)));
  for (const l of record.lessons) {
    assert.ok(l.actualSourceRequirements.length >= 4, l.lessonCode);
    assert.equal(l.sourceDrawingAppletParityApproved, false);
    assert.equal(l.effectiveRichV2StudentRenderVerified, false);
    assert.equal(l.runtimeSelectedPracticeVerified, false);
    assert.equal(l.teacherRubricPersistedVerified, false);
    assert.equal(l.academicAcceptance, 'NOT_ACCEPTED');
    const example = byCode.get(l.lessonCode).translations[0].workedExamples;
    assert.match(example, /Example 3\b/, l.lessonCode);
  }
  const example = n => byCode.get(lcode(1,n)).translations[0].workedExamples;
  assert.match(example(13), /tetrahedron/i);
  assert.match(example(14), /triangular prism net/i);
  assert.match(example(17), /32 identical/);
  assert.match(example(17), /27; 5 remain/);
  assert.match(example(18), /1734 square units/);
  assert.match(example(18), /4913 cubic units/);
  const tent = byCode.get(lcode(1,19)).translations[0];
  assert.match(tent.explanation, /REQUIRES a tent floor/);
  assert.match(tent.keyConceptsAndRules, /MUST INCLUDE A FLOOR/);
  assert.match(tent.workedExamples, /INCLUDING THE FLOOR/);
  assert.doesNotMatch(tent.workedExamples, /Ignoring floor and seams/);
  assert.equal(3 ** 3, 27);
  assert.equal(4 ** 3, 64);
  assert.equal(6 * (17 ** 2), 1734);
  assert.equal(17 ** 3, 4913);
  assert.equal(8 * 6 + 2 * 6 * 5 + 2 * 0.5 * 8 * 3, 132);
  assert.equal(8 * 8 + 2 * 8 * 5 + 2 * 0.5 * 8 * 3, 168);
});


test('U02 L01–L03 original collection, ratio diagrams and recipes remain distinct and teacher-scored where needed', () => {
  const record = JSON.parse(fs.readFileSync(path.join(root,
    'artifacts/curriculum-delivery/US-CCSS-MATH/G6/g6-u02-l01-l03-student-teacher-task-comparison.json'), 'utf8'));
  assert.equal(record.lessons.length, 3);
  assert.equal(record.summary.studentTaskBodiesCompared, 3);
  assert.equal(record.summary.academicAcceptance, 0);
  assert.deepEqual(record.lessons.map(x => x.lessonCode), [1,2,3].map(n => lcode(2,n)));
  for (const r of record.lessons) {
    assert.ok(r.sourceActivities.length >= 3, r.lessonCode);
    assert.ok(r.mathematicalTaskRequirements.length >= 4, r.lessonCode);
    assert.equal(r.originalPublisherFigureOrCardAssetsReproduced, false);
    assert.equal(r.authenticatedLearnerRenderedAndAccessible, false);
    assert.equal(r.selectedPracticeContractGeneratorVerifierTested, false);
    assert.equal(r.teacherEvaluationPersisted, false);
    assert.equal(r.academicAcceptance, 'NOT_ACCEPTED');
    assert.match(byCode.get(r.lessonCode).translations[0].workedExamples, /Example 3\b/);
  }
  const a = byCode.get(lcode(2, 1)).translations[0].workedExamples;
  const b = byCode.get(lcode(2, 2)).translations[0].workedExamples;
  const c = byCode.get(lcode(2, 3)).translations[0].workedExamples;
  assert.match(a, /24-square-unit rectangle/);
  assert.match(a, /16 yellow and 8 blue/);
  assert.match(b, /4:2:1/);
  assert.match(b, /24 ÷ 4/);
  assert.match(c, /15,6/);
  assert.match(c, /0.5 mg\/mL/);
  assert.equal(16 + 8, 24);
  assert.equal(12 + 6, 18);
  assert.equal(16 / 8, 12 / 6);
  assert.equal(15 / 5, 6 / 2);
  assert.ok(150 / 300 > 200 / 500);
});


test('U02 L04 L06 L08 retain exact source contexts for color mixtures, double number lines and unit prices', () => {
  const record = JSON.parse(fs.readFileSync(path.join(root,
    'artifacts/curriculum-delivery/US-CCSS-MATH/G6/g6-u02-l04-l06-l08-student-teacher-task-comparison.json'), 'utf8'));
  assert.equal(record.lessons.length, 3);
  assert.equal(record.summary.sourceStudentBodiesCompared, 3);
  assert.equal(record.summary.fullAcademicAccepted, 0);
  assert.deepEqual(record.lessons.map(l => l.lessonCode), [4,6,8].map(n => lcode(2,n)));
  for (const r of record.lessons) {
    assert.ok(r.publicStudentTasksCompared.length >= 4, r.lessonCode);
    assert.equal(r.nonNumericTasksTeacherAssessmentRequired, true);
    assert.equal(r.originalImagesAppletOrPhysicalSourceReplicated, false);
    assert.equal(r.liveLearnerRenderingVerified, false);
    assert.equal(r.exactPracticeGeneratorVerifierChecked, false);
    assert.equal(r.stageAAccepted, false);
    assert.equal(r.academicAcceptance, 'NOT_ACCEPTED');
    assert.match(byCode.get(r.lessonCode).translations[0].workedExamples, /Example 3\b/);
  }
  const e = n => byCode.get(lcode(2,n)).translations[0].workedExamples;
  assert.match(e(4), /add another 40 mL yellow/);
  assert.match(e(4), /adding 38 mL yellow/);
  assert.match(e(6), /16 teaspoons require 4 cups/);
  assert.match(e(6), /Twelve tablespoons blue require 4 cups white/);
  assert.match(e(8), /seven bottles cost \$5\.25|7 bottles cost \$5\.25/);
  assert.match(e(8), /larger package is worse value/);
  assert.equal(25 * 3 - 35, 40);
  assert.equal(17 * 3 - 13, 38);
  assert.equal(12 / 3, 4);
  assert.equal(16 / 4, 4);
  assert.equal(9 / 12 * 7, 5.25);
  assert.ok(7 / 12 > 3 / 6);
});


test('U02 L05 L07 L09 check fractional casserole, number-line units and the 30-second spatial head start', () => {
  const evidence = JSON.parse(fs.readFileSync(path.join(root,
    'artifacts/curriculum-delivery/US-CCSS-MATH/G6/g6-u02-l05-l07-l09-student-teacher-source-comparison.json'), 'utf8'));
  assert.equal(evidence.lessons.length, 3);
  assert.equal(evidence.summary.directStudentBodiesCompared, 3);
  assert.equal(evidence.summary.fullAcademicSourceParityAccepted, 0);
  assert.deepEqual(evidence.lessons.map(r => r.lessonCode), [5,7,9].map(n => lcode(2,n)));
  for (const r of evidence.lessons) {
    assert.equal(r.originalImagesAppletOrPhysicalTaskVerified, false);
    assert.equal(r.authStudentVisualRendered, false);
    assert.equal(r.exactSelectedPracticeVerified, false);
    assert.equal(r.teacherRubricAndAttemptsPersisted, false);
    assert.equal(r.academicAcceptance, 'NOT_ACCEPTED');
    assert.match(byCode.get(r.lessonCode).translations[0].workedExamples, /Example 3\b/);
  }
  const examples = n => byCode.get(lcode(2,n)).translations[0].workedExamples;
  assert.match(examples(5), /One-fifth batch/);
  assert.match(examples(5), /324/);
  assert.match(examples(7), /Ten pints require 40 cups flour/);
  assert.match(examples(7), /Thirty ounces tuna/);
  assert.match(examples(9), /45-meter distance head start/);
  assert.match(examples(9), /race distance must be specified/);
  assert.equal(162 * 2, 324);
  assert.equal(3 / 5, 0.6);
  assert.equal(10 * 4, 40);
  assert.equal(6 * (30 / 6), 30);
  assert.equal(10 * (30 / 6), 50);
  assert.equal((55 / 10 - 40 / 10) * 30, 45);
});


test('U02 L09 parser-recognizable measured path and two aligned double number line descriptions use exact source quantities', () => {
  const en = byCode.get(lcode(2,9)).translations[0];
  const descriptions = en.workedExamples.split('\n\n').filter(t => t.startsWith('Description: '));
  assert.equal(descriptions.length, 3);
  const measured = descriptions[0];
  assert.match(measured, /line with three markings/i);
  assert.match(measured, /the first mark is labeled "Warm-up"/i);
  assert.match(measured, /the second mark is labeled "Start"/i);
  assert.match(measured, /the third mark is labeled "Finish"/i);
  assert.match(measured, /distance between the first and second mark is labeled 1 meter/i);
  assert.match(measured, /distance between the second and third mark is labeled 10 meters/i);
  for (const d of descriptions.slice(1)) {
    assert.match(d, /double number line/i);
    assert.match(d, /top number line is labeled "[^"]+"/i);
    assert.match(d, /bottom number line is labeled "[^"]+"/i);
    assert.ok((d.match(/numbers [0-9., ]+ are indicated/ig) || []).length === 2);
  }
  assert.match(descriptions[1], /0, 2, 4, 6, 8 are indicated/);
  assert.match(descriptions[1], /0, 2\.5, 5, 7\.5, 10 are indicated/);
  assert.match(descriptions[2], /0, 1, 2, 3, 4, 5 are indicated/);
  assert.match(descriptions[2], /0, 2, 4, 6, 8, 10 are indicated/);
  assert.equal(10/8, 1.25);
  assert.equal(10/5, 2);
});


test('U02 L10–L12 source-task contexts preserve comparisons, tables and human explanation gates', () => {
  const record = JSON.parse(fs.readFileSync(path.join(root,
    'artifacts/curriculum-delivery/US-CCSS-MATH/G6/g6-u02-l10-l12-original-student-teacher-task-comparison.json'), 'utf8'));
  assert.equal(record.summary.studentBodiesDirectlyCompared, 3);
  assert.equal(record.summary.teacherActivityBodiesCompared, 3);
  assert.equal(record.summary.sourceTaskFullyAcademicallyAccepted, 0);
  assert.deepEqual(record.lessons.map(l => l.lessonCode), [10, 11, 12].map(n => lcode(2, n)));
  for (const lesson of record.lessons) {
    assert.ok(lesson.sourceActivities.length >= 4, lesson.lessonCode);
    assert.ok(lesson.firstPartyEvidence.length >= 3, lesson.lessonCode);
    assert.equal(lesson.teacherRubricCandidate.implemented, false);
    assert.equal(lesson.teacherRubricCandidate.persisted, false);
    assert.equal(lesson.selectedPracticeTested, false);
    assert.equal(lesson.actualStudentVisualRendered, false);
    assert.equal(lesson.academicAcceptance, 'NOT_ACCEPTED');
  }
  const l10 = byCode.get(lcode(2,10)).translations[0];
  assert.match(l10.workedExamples, /3\.0 miles in 24 minutes/);
  assert.match(l10.workedExamples, /3\.0 miles in 30 minutes/);
  assert.match(l10.workedExamples, /24-minute 3-mile runner|A is faster/);
  assert.match(l10.workedExamples, /Description: Double number line/);
  assert.match(l10.workedExamples, /15:20 versus 16:20/);

  const l11 = byCode.get(lcode(2,11)).translations[0];
  assert.match(l11.workedExamples, /4th figure contains 28 tiles/);
  assert.match(l11.workedExamples, /5th 35 and the 10th 70/);
  assert.match(l11.workedExamples, /350,250/);
  assert.match(l11.workedExamples, /364,455/);
  assert.match(l11.workedExamples, /Description: Ratio table/);

  const l12 = byCode.get(lcode(2,12)).translations[0];
  assert.match(l12.workedExamples, /Fifty tacos for \$72 cost \$1\.44 each/);
  assert.match(l12.workedExamples, /2\.1,37\.80/);
  assert.match(l12.workedExamples, /128 GB cost \$32/);
  assert.match(l12.workedExamples, /historical memory paired halving|Halving yields/);
  assert.match(l12.workedExamples, /Description: Double number line/);
  assert.match(l12.explanation, /not current retail pricing/);
});

test('U02 L10–L12 independent numeric oracles reject common non-equivalent-rate traps', () => {
  assert.equal(3 / 24 > 3 / 30, true);             // same distance, less time => faster
  assert.equal(24 / 3, 8);                         // minutes per mile A
  assert.equal(30 / 3, 10);                        // minutes per mile B
  assert.equal(47 * 3, 141);                       // 3 to 9 ticket batch factor
  assert.equal(3 * 5, 15);                         // matching soda for first juice ratio
  assert.equal(4 * 4, 16);                         // matching soda for second juice ratio
  assert.ok(Math.abs(16 / 5 - 3 - 0.2) < 1e-12);
  assert.ok(Math.abs(16 / 3 - 5 - 1 / 3) < 1e-12);
  assert.equal(7 * 4, 28);                         // growing colored tile figure
  assert.equal(7 * 10, 70);
  assert.equal(250 * 7 / 5, 350);                  // large batch trail mix
  assert.equal(455 * 4 / 5, 364);                  // juice table
  assert.equal(15 / (6 / 4), 10);                  // tacos possible for $15
  assert.equal(72 / 50 < 6 / 4, true);             // taco prices differ
  assert.ok(Math.abs(18 * 2.1 - 37.8) < 1e-10);    // wages
  assert.equal(32 / 128, 0.25);                   // historical $/GB
});


test('U02 L13–L15 preserve public student/teacher tasks without claiming completed runtime grading', () => {
  const evidence = JSON.parse(fs.readFileSync(path.join(root,
    'artifacts/curriculum-delivery/US-CCSS-MATH/G6/g6-u02-l13-l15-original-student-teacher-task-comparison.json'), 'utf8'));
  assert.equal(evidence.summary.publicStudentAndTeacherBodiesReviewed, 3);
  assert.equal(evidence.summary.activitiesCompared, 11);
  assert.equal(evidence.summary.fullStageAAccepted, 0);
  assert.deepEqual(evidence.lessons.map(x => x.lessonCode), [13,14,15].map(n => lcode(2,n)));
  for (const lesson of evidence.lessons) {
    assert.ok(lesson.sourceTaskExpectations.length >= 3, lesson.lessonCode);
    assert.ok(lesson.firstPartyCandidateTeachingEvidence.length >= 3, lesson.lessonCode);
    assert.equal(lesson.teacherRubricCandidate.implemented, false);
    assert.equal(lesson.teacherRubricCandidate.persisted, false);
    assert.equal(lesson.renderedAccessibilityVerified, false);
    assert.equal(lesson.actualSelectedPracticeVerified, false);
    assert.equal(lesson.academicAcceptance, 'NOT_ACCEPTED');
  }
  const a = byCode.get(lcode(2,13)).translations[0].workedExamples;
  const b = byCode.get(lcode(2,14)).translations[0].workedExamples;
  const c = byCode.get(lcode(2,15)).translations[0].workedExamples;
  assert.match(a, /3,000 m requires 600 s/);
  assert.match(a, /400 seconds/);
  assert.match(a, /125 s/);
  assert.match(a, /Description: Double number line/);
  assert.match(b, /separation is 20 miles/);
  assert.match(b, /order is Diego, Elena, Lin/);
  assert.match(b, /13\.5 cups/);
  assert.match(c, /50 red and 30 blue/);
  assert.match(c, /21, 14 and 7 cups/);
  assert.match(c, /112\.5 cups/);
  assert.match(c, /Teacher reviews/);
});

test('U02 L13–L15 mathematical oracle checks preserve units and limiting resources', () => {
  assert.equal(150 / 2, 75);
  assert.equal(150 / 4, 37.5);
  assert.equal(150 / 8, 18.75);
  assert.equal(3000 / (100 / 20), 600);
  assert.equal(3000 / (150 / 20), 400);
  assert.equal(3000 / (360 / 15), 125);
  assert.equal((65 - 60) * 4, 20);
  assert.equal(9 * 3 / 2, 13.5);
  assert.equal((270 - 54) / (54 / 3), 12);
  assert.equal((325 - 100) / (100 / 4), 9);
  assert.equal((480 - 160) / (160 / 5), 10);
  assert.equal((5 + 3) * 10, 80);
  assert.equal(5 * 10, 50);
  assert.equal(3 * 10, 30);
  assert.deepEqual([3, 2, 1].map(x => x * (42/6)), [21,14,7]);
  assert.equal((4 + 3 + 2) * (30 / 3), 90);
  assert.equal(Math.min(50/4,40/3,30/2) * (4+3+2), 112.5);
});


test('U02 L16–L17 preserve aquarium and source Fermi open-response boundaries', () => {
  const data = JSON.parse(fs.readFileSync(path.join(root,
    'artifacts/curriculum-delivery/US-CCSS-MATH/G6/g6-u02-l16-l17-original-student-teacher-task-comparison.json'), 'utf8'));
  assert.equal(data.summary.directLessonBodies, 2);
  assert.equal(data.summary.StageAFullyAccepted, 0);
  assert.deepEqual(data.lessons.map(x=>x.lessonCode), [16,17].map(n=>lcode(2,n)));
  for(const l of data.lessons) {
    assert.equal(l.activities.length, 3);
    assert.equal(l.teacherReviewRubric.maxPoints, 8);
    assert.equal(l.teacherReviewRubric.teacherWorkflowVerified, false);
    assert.equal(l.teacherReviewRubric.teacherScoresPersisted, false);
    assert.equal(l.selectedPracticeGeneratorVerifierTested, false);
    assert.equal(l.academicAcceptance, 'NOT_ACCEPTED');
  }
  const a = byCode.get(lcode(2,16)).translations[0].workedExamples;
  const b = byCode.get(lcode(2,17)).translations[0].workedExamples;
  assert.match(a,/10 chaperones and 75 student tickets/);
  assert.match(a,/16 oil and 12 vinegar/);
  assert.match(a,/four hours/);
  assert.match(b,/5\/3 cups/);
  assert.match(b,/teacher approval/i);
  assert.match(b,/216,000 pages/);
  assert.equal(85 * 2 / (15+2), 10);
  assert.equal(85 * 15 / (15+2), 75);
  assert.equal(28 * 4 / 7, 16);
  assert.equal(72 / (4+5) / 2, 4);
  assert.ok(Math.abs(5 / 3 - 1 - 2 / 3) < 1e-12);
});


test('U03 L01–L03 source-specific measurements preserve estimates, unit types and teacher-only tasks', () => {
  const evidence = JSON.parse(fs.readFileSync(path.join(root,
    'artifacts/curriculum-delivery/US-CCSS-MATH/G6/g6-u03-l01-l03-public-source-task-comparison.json'), 'utf8'));
  assert.equal(evidence.summary.directStudentAndTeacherTaskBodiesCompared, 3);
  assert.equal(evidence.summary.fullStageAAccepted, 0);
  assert.deepEqual(evidence.lessons.map(l => l.lessonCode), [1,2,3].map(n => lcode(3,n)));
  for(const l of evidence.lessons) {
    assert.equal(l.originalTaskPurposes.length, 4);
    assert.equal(l.teacherRubricDraft.scoreMax, 8);
    assert.equal(l.teacherRubricDraft.implemented, false);
    assert.equal(l.teacherRubricDraft.persisted, false);
    assert.equal(l.originalMediaOrAppletVerified, false);
    assert.equal(l.selectedPracticeContractVerified, false);
    assert.equal(l.fullAcademicAcceptance, 'NOT_ACCEPTED');
  }
  const a = byCode.get(lcode(3,1)).translations[0];
  const b = byCode.get(lcode(3,2)).translations[0];
  const c = byCode.get(lcode(3,3)).translations[0];
  assert.match(a.workedExamples, /24,348/);
  assert.match(a.workedExamples, /29,217\.6 minutes/);
  assert.match(a.workedExamples, /34\.5 meters/);
  assert.match(b.workedExamples, /15\.625 cm/);
  assert.match(b.workedExamples, /0\.9144 meter/);
  assert.match(c.workedExamples, /16\.387064 cubic centimeters/);
  assert.match(c.workedExamples, /Station 5 has no supplied measured teaspoon count/);
  assert.ok(Math.abs((24348 * 18 / 15) - 29217.6) < 1e-8);
  assert.equal(828 / 6, 138);
  assert.equal(138 / 4, 34.5);
  assert.equal(2.5**3, 15.625);
  assert.equal(18/9,2);
  assert.equal(18/6,3);
  assert.ok(Math.abs(16.387064 * 3 - 49.161192) < 1e-9);
});
