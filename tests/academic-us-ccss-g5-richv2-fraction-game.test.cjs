'use strict';
const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const filename = path.resolve(__dirname, '../src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u03-l19-2021.rich-lesson-v2.json');
const document = JSON.parse(fs.readFileSync(filename, 'utf8'));
const item = document.Lessons[0];

test('fraction game sidecar Rich V2 structure', () => {
 assert.equal(document.SchemaVersion, 1);
 assert.equal(document.PackCode, 'US-CCSS-MATH');
 assert.equal(document.Lessons.length, 1);
 assert.equal(item.LessonCode, 'PED:US-CCSS-MATH:G5:U03:L19');
 assert.ok(item.ExplanationParagraphs.length >= 2 && item.KeyConcepts.length >= 3);
 assert.ok(item.WorkedExamples.length >= 3 && item.CommonMistakes.length >= 2);
 assert.ok(item.SummaryPoints.length >= 3 && item.Visuals.length >= 1);
 assert.ok(item.WorkedExamples.every(e => e.Steps.length >= 3 && e.Answer));
 assert.ok(item.Visuals.every(v => v.Description && v.Rows.length > 0));
});

test('greatest/smallest distinct-digit fraction division is exact', () => {
 let maximum = 0;
 for (let a = 1; a <= 6; a++) for (let b = 1; b <= 6; b++) if (a !== b) maximum = Math.max(maximum, a * b);
 assert.equal(maximum, 30);
 assert.ok(item.WorkedExamples[0].Answer.includes('30'));
 assert.ok(item.WorkedExamples[1].Answer.includes('1/30'));
 assert.equal(28 * 26, 728);
 assert.equal(728 % 9, 8);
 assert.ok(item.WorkedExamples[2].Answer.includes('80 8/9'));
});

test('written optimality and publisher source boundaries remain teacher-reviewed', () => {
 const text = item.ExplanationParagraphs.join(' ');
 assert.ok(text.includes('teacher review'));
 assert.ok(text.includes('Cool-down'));
 assert.ok(item.SummaryPoints.join(' ').includes('2021'));
});
