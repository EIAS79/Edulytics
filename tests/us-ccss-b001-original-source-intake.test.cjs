const test = require('node:test');
const assert = require('node:assert/strict');
const path = require('node:path');
const fs = require('node:fs');

const root = path.resolve(__dirname, '..');
const catalogue = JSON.parse(fs.readFileSync(
  path.join(root, 'docs/curriculum/batches/US-CCSS-B001-CATALOGUE.json'), 'utf8'));
const source = JSON.parse(fs.readFileSync(
  path.join(root, 'docs/curriculum/batches/US-CCSS-B001-OURCS-ORIGINAL-SOURCE-INTAKE-20261016.json'), 'utf8'));

test('Our-CS offline original source snapshot matches exactly 500 canonical B001 IDs and order', () => {
  assert.equal(source.schemaVersion, 1);
  assert.equal(source.batch, 'US-CCSS-B001');
  assert.equal(source.recordCount, 500);
  assert.equal(source.matchedCount, 500);
  assert.equal(source.missingCount, 0);
  assert.equal(source.lessons.length, 500);
  assert.equal(catalogue.lessons.length, 500);
  assert.equal(new Set(source.lessons.map(x => x.lessonCode)).size, 500);
  for (let i=0; i<500; i++) {
    const actual=source.lessons[i], expected=catalogue.lessons[i];
    assert.equal(actual.ordinal, i+1);
    assert.equal(actual.lessonCode, expected.lessonCode);
    assert.equal(actual.title, expected.title);
    assert.equal(actual.sourceUrl, expected.sourceUrl);
    assert.equal(actual.expectedEdition, expected.sourceEdition);
    const grade=Number(expected.gradeOrCourse.replace('Grade ', ''));
    const directory=`${String(grade).padStart(2,'0')}_Grade_${grade}_IM_K5/unit-${expected.unitNumber}/lesson-${expected.lessonNumber}`;
    assert.equal(actual.relativeTeacherPreparation, `${directory}/preparation.html`);
    assert.equal(actual.relativeTeacherFullLesson, `${directory}/lesson.html`);
    assert.equal(actual.teacherFilePairVerifiedOnDevice, true);
    assert.equal(actual.pedagogicalFidelityAccepted, false);
    assert.equal(actual.practiceSourceGoalAccepted, false);
    assert.equal(actual.productionStudentExperienceAccepted, false);
  }
});

test('Evidence declares only independently inspected original 2021 teaching goals', () => {
  const reviewed=source.lessons.filter(x=>x.originalLearningGoalReviewed);
  assert.deepEqual(reviewed.map(x=>x.lessonCode),[
    'PED:US-CCSS-MATH:G1:U02:L01',
    'PED:US-CCSS-MATH:G1:U02:L02',
    'PED:US-CCSS-MATH:G1:U02:L03'
  ]);
  assert.equal(source.lessonGoalReviewConfirmed, 3);
  assert.equal(source.academicApprovalCount, 0);
  for(const entry of source.lessons){
    for(const hash of [entry.teacherPreparationRawSha256, entry.teacherFullLessonRawSha256]){
      if(entry.originalLearningGoalReviewed) assert.match(hash, /^[a-f0-9]{64}$/);
      else assert.equal(hash,null);
    }
  }
  assert.match(source.assurance,/not teaching content or source rights approval/i);
});
