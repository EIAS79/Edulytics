'use strict';
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const {execFileSync} = require('node:child_process');
const {test} = require('node:test');
const root = path.resolve(__dirname, '..');
const grade = 'artifacts/curriculum-delivery/US-CCSS-MATH/G2/';
const parse = file => JSON.parse(fs.readFileSync(path.join(root, grade, file), 'utf8'));
const manifest = parse('grade-manifest.json');
const progress = parse('full-grade-progress.json');
const ledger = parse('stage-d-execution-ledger.json');
const isCommit = value => typeof value === 'string' && /^[a-f0-9]{40}$/i.test(value);

test('Canonical Grade 2: exactly 146 unique source-pinned original lessons in mandatory order', () => {
  assert.equal(manifest.programme, 'US-CCSS-MATH');
  assert.equal(manifest.grade, 'Grade 2');
  assert.equal(manifest.lessons.length, 146);
  assert.deepEqual(
    [1,2,3,4,5,6,7,8,9].map(u => manifest.lessons.filter(x => x.unitNumber === u).length),
    [18,16,18,15,14,21,18,13,13]);
  assert.equal(new Set(manifest.lessons.map(x => x.lessonCode)).size, 146);
  for (let i = 0; i < 146; i++) {
    const x = manifest.lessons[i];
    assert.equal(x.gradeOrdinal, i + 1);
    assert.match(x.lessonCode, /^PED:US-CCSS-MATH:G2:U0[1-9]:L\d\d$/);
    assert.ok(x.sourceTeacherUrl?.startsWith('https://im.kendallhunt.com/k5/teachers/grade-2/'),
      'Original Grade 2 teacher-source link absent: ' + x.lessonCode);
    assert.equal(x.aCStatus, 'A_C_COMPLETED');
  }
});

test('Full sequence G2 D1-D3: 146 real DIGITAL test evidence records only, never auto-approved', () => {
  assert.equal(progress.programme, 'US-CCSS-MATH');
  assert.equal(progress.grade, 'Grade 2');
  assert.equal(progress.lessonStatuses.length, 146);
  assert.equal(ledger.lessons.length, 146);
  assert.equal(ledger.aggregate.lessonsWithPassingNamedTests, 146);
  assert.equal(ledger.aggregate.lessonsWithAuthenticatedB001PracticeProof, 146);
  assert.equal(ledger.aggregate.lessonsWithOriginalCompanionGetE2E, 44);
  assert.deepEqual(ledger.batches.map(b => [b.first,b.last,b.namedLessonTestsPassed]),
    [[1,49,49],[50,98,49],[99,146,48]]);
  assert.deepEqual(progress.currentStageDVerification.sequentialBatches.map(
    b => [b.from,b.to,b.status]),
    [[1,49,'D_TESTS_PASSED'],[50,98,'D_TESTS_PASSED'],[99,146,'D_TESTS_PASSED']]);
  const codes = new Set();
  for (let i = 0; i < 146; i++) {
    const [m, p, l] = [manifest.lessons[i], progress.lessonStatuses[i], ledger.lessons[i]];
    assert.equal(p.gradeOrdinal, i+1);
    assert.equal(l.ordinal, i+1);
    assert.equal(p.lessonCode, m.lessonCode);
    assert.equal(l.lessonCode, m.lessonCode);
    codes.add(l.lessonCode);
    assert.equal(p.aCStatus,'A_C_COMPLETED');
    assert.equal(p.stageD,'D_TESTS_PASSED');
    assert.equal(l.stageD,'D_TESTS_PASSED');
    assert.equal(p.currentStageDEvidence.namedDigitalTestsPassed,true);
    assert.equal(p.currentStageDEvidence.genericAuthenticatedPracticePassed,true);
    assert.equal(l.genericAuthenticatedPractice.malformedAnswerRejectedAndPersisted,true);
    assert.ok(l.namedDigitalTests.passed > 0);
    assert.equal(l.namedDigitalTests.failed,0);
    assert.equal(l.currentCandidateDigitalValidation.academicApprovalGranted,false);
    assert.equal(p.acceptance,'APPROVAL_PENDING');
    assert.equal(l.humanAcademicReview,'APPROVAL_PENDING');
    assert.equal(l.finalAcceptance,'NOT_APPROVED');
    assert.equal(p.currentStageDEvidence.authenticatedSourceCompanionGET, i >= 102);
    assert.equal(l.originalCompanionVerification.status,
      i >= 102 ? 'AUTHENTICATED_GET_CROSS_SCHOOL_DENY_PASSED' : 'NOT_SEPARATELY_CERTIFIED');
  }
  assert.equal(codes.size,146);
});

test('Exact predecessor code SHA and Actions technical proof are pinned and in Git ancestry', () => {
  const e = ledger.candidateEvidence.exactHeadCi;
  const candidate = e.testedCandidateSHA;
  assert.ok(isCommit(candidate));
  assert.equal(ledger.sourceHeadSHA,candidate);
  assert.equal(progress.currentStageDVerification.exactCandidateSHA,candidate);
  assert.equal(e.workflows,15);
  assert.equal(e.succeeded,15);
  assert.equal(e.failed,0);
  assert.equal(e.pending,0);
  assert.equal(e.mathFoundation.passed,2869);
  assert.equal(e.mathFoundation.failed,0);
  assert.equal(e.g2LateCompanions.passedAuthenticatedStudentGet,44);
  assert.equal(e.g2LateCompanions.passedCrossSchoolDenial,44);
  assert.ok(e.g2LateCompanions.artifactId);
  const ancestor = execFileSync('git',['merge-base','--is-ancestor',candidate,'HEAD'],
    {cwd:root,stdio:'pipe'});
  assert.equal(ancestor.length,0);
  // This checks the recorded evidence and relevant ancestry. It does NOT
  // assert that CI was rerun on this current ledger-only commit.
});

test('Fail closed on Grade 2 academic/publisher signoff and Render release', () => {
  assert.equal(progress.release,'NO_GO_UNVERIFIED');
  assert.equal(ledger.academicReleaseDecision,'NO_GO_UNVERIFIED');
  assert.equal(progress.currentStageDVerification.newHumanAcademicApprovedLessonCount,0);
  assert.equal(ledger.aggregate.newAcademicApprovals,0);
  assert.equal(ledger.aggregate.lessonsDVerified,0);
  assert.equal(ledger.aggregate.merged,false);
  assert.equal(ledger.aggregate.deployed,false);
  assert.equal(progress.currentStageDVerification.firstAcademicReviewOrdinalPending,1);
  assert.equal(ledger.sequentialCursor.firstPendingAcademicApprovalOrdinal,1);
  assert.ok(ledger.outstandingGates.some(s => s.includes('academic')));
  assert.ok(ledger.outstandingGates.some(s => s.includes('UAT')));
});
