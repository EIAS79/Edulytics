'use strict';
const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');

test('148 teacher HTML and 148 student mirror HTML files have source hashes; mirror parity remains pending', () => {
  const root = path.resolve(__dirname, '..', 'artifacts', 'curriculum-delivery', 'US-CCSS-MATH', 'G5');
  const expectedLessons = [12, 17, 20, 21, 26, 21, 13, 18];
  const observed = new Set();
  let teacherCount = 0, studentCount = 0, teacherHeadings = 0, teacherImages = 0, wrongEditionStudentMirror = 0;
  for (let unit = 1; unit <= expectedLessons.length; unit++) {
    const file = path.join(root, 'local-teacher-student-html-source-hashes-u' + String(unit).padStart(2, '0') + '.json');
    const inventory = JSON.parse(fs.readFileSync(file, 'utf8'));
    assert.equal(inventory.unit, unit);
    assert.match(inventory.kind, /NOT_ACADEMIC_OR_RIGHTS_APPROVAL/);
    assert.equal(inventory.sourceAssetAndStudentMirrorReview, 'REQUIRED');
    assert.equal(inventory.lessons.length, expectedLessons[unit - 1] * 2);
    assert.equal(inventory.studentMirrorWrongEditionCount, expectedLessons[unit - 1]);
    assert.equal(inventory.studentMirrorAcademicUse, 'BLOCKED_WRONG_EDITION');
    for (const item of inventory.lessons) {
      assert.ok(item.lesson >= 1 && item.lesson <= expectedLessons[unit - 1]);
      assert.ok(['Teacher', 'Student'].includes(item.role));
      const identity = unit + ':' + item.lesson + ':' + item.role;
      assert.ok(!observed.has(identity), 'duplicate source file ' + identity);
      observed.add(identity);
      assert.match(item.sha256, /^[a-f0-9]{64}$/);
      assert.ok(item.bytes > 0);
      assert.ok(Number.isInteger(item.headingCount) && item.headingCount >= 0);
      assert.ok(Number.isInteger(item.imageTags) && item.imageTags >= 0);
      assert.ok(Number.isInteger(item.pdfReferences) && item.pdfReferences >= 0);
      const relative = (item.role === 'Student' ? 'Student_Public_Mirror/' : '') +
        'unit-' + unit + '/lesson-' + item.lesson + '/lesson.html';
      assert.equal(item.relativePath, relative);
      assert.equal(item.educationalParityCertified, false);
      if (item.role === 'Teacher') {
        teacherCount++;
        teacherHeadings += item.headingCount;
        teacherImages += item.imageTags;
        assert.equal(item.htmlDocumentTitle, 'Illustrative Mathematics | Kendall Hunt');
        assert.equal(item.editionMatch, 'IM2021_TEACHER_CANDIDATE_SOURCE');
        assert.ok(item.headingCount > 0);
      } else {
        studentCount++;
        wrongEditionStudentMirror++;
        assert.equal(item.htmlDocumentTitle, 'Illustrative Mathematics | v.360 Curriculum');
        assert.equal(item.editionMatch, 'WRONG_EDITION_V360_STUDENT_MIRROR');
        assert.equal(item.headingCount, 8);
        assert.equal(item.imageTags, 0);
      }
    }
  }
  assert.equal(teacherCount, 148);
  assert.equal(studentCount, 148);
  assert.equal(observed.size, 296);
  assert.equal(wrongEditionStudentMirror, 148);
  assert.equal(teacherHeadings, 4412);
  assert.equal(teacherImages, 1886);
  console.log('G5_SOURCE_FILE_SNAPSHOT 148 IM2021 teacher candidates / 148 v360 wrong-edition student mirrors; semantic and rights parity BLOCKED');
});
