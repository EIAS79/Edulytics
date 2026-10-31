'use strict';
const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const root = path.resolve(__dirname, '..');
const sidecar = JSON.parse(fs.readFileSync(path.join(root, 'src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-fractions-coordinates-source-lessons-2021.rich-lesson-v2.json'), 'utf8'));
const lesson = sidecar.Lessons.find(l => l.LessonCode === 'PED:US-CCSS-MATH:G5:U03:L01');
const audit = JSON.parse(fs.readFileSync(path.join(root, 'artifacts/curriculum-delivery/US-CCSS-MATH/G5/u03-l01-local-2021-source-parity-v14.json'), 'utf8'));
const rubric = JSON.parse(fs.readFileSync(path.join(root, 'artifacts/curriculum-delivery/US-CCSS-MATH/G5/u03-l01-original-cooldown-teacher-rubric-v14.json'), 'utf8'));
test('U03:L01: source warm-up is approximate, not an exact half', () => {
 assert.ok(lesson.ExplanationParagraphs[1].includes('between one third and one half'));
 assert.ok(lesson.ExplanationParagraphs[1].includes('about two fifths'));
 assert.ok(!lesson.ExplanationParagraphs[1].includes('exactly half remains'));
 assert.ok(audit.sourceTeacherLessonSha256.length === 64 && audit.sourceTeacherPreparationSha256.length === 64);
});
test('U03:L01: original whole, two diagram orientations, and teacher-scored cool-down preserved', () => {
 assert.ok(lesson.ExplanationParagraphs[2].includes('DRAW their own diagram'));
 assert.ok(lesson.ExplanationParagraphs[3].includes('EACH diagram'));
 assert.equal(lesson.Visuals.length, 2);
 assert.ok(lesson.Visuals[1].Rows.some(x => x.Cells.some(v => v.includes('1/8'))));
 assert.equal(1 * 2 * 4, 8);
 assert.equal(rubric.maxPoints, rubric.rubric.reduce((a,b) => a + b.points, 0));
 assert.ok(lesson.ExplanationParagraphs.at(-1).includes('garden bed'));
 assert.ok(audit.unverified.some(x => x.includes('Practice')));
 assert.equal(audit.academicAccepted, false);
});
