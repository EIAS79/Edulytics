#!/usr/bin/env node
import fs from 'node:fs';
import path from 'node:path';
import { fileURLToPath } from 'node:url';

const __filename = fileURLToPath(import.meta.url);
const __dirname = path.dirname(__filename);
const ROOT = path.resolve(__dirname, '../..');
const PACK_DIR = path.join(
  ROOT,
  'src/Edulytics.Core/Curriculum/LessonContent/Packs'
);

const files = fs.readdirSync(PACK_DIR)
  .filter((name) => /^us-ccss-math-.*\.lesson-content-pack\.json$/i.test(name))
  .sort();

const requiredFields = [
  'explanation',
  'keyConceptsAndRules',
  'workedExamples',
  'stepByStepSolutions',
  'commonMistakes',
  'quickSummary'
];

const forbiddenLearnerPatterns = [
  /\bblackline master\b/i,
  /\bformative assessment\b/i,
  /\bsupports accessibility\b/i,
  /\baccess for english learners\b/i,
  /\bactivity synthesis\b/i
];

const errors = [];
let lessons = 0;
let supporting = 0;
let official = 0;

for (const file of files) {
  const full = path.join(PACK_DIR, file);
  const doc = JSON.parse(fs.readFileSync(full, 'utf8'));

  if (doc.packCode !== 'US-CCSS-MATH') {
    errors.push(`${file}: unexpected packCode ${doc.packCode}`);
  }

  for (const lesson of doc.lessons || []) {
    lessons++;
    if (lesson.isSupporting) supporting++;
    else official++;

    const english = (lesson.translations || [])
      .find((translation) =>
        String(translation.cultureCode || '').toLowerCase().startsWith('en'));

    if (!english) {
      errors.push(`${lesson.lessonCode}: missing English translation`);
      continue;
    }

    for (const field of requiredFields) {
      const value = String(english[field] || '').trim();
      if (!value) {
        errors.push(`${lesson.lessonCode}: empty ${field}`);
      }
    }

    const learnerBody = requiredFields
      .map((field) => String(english[field] || ''))
      .join('\n');

    for (const pattern of forbiddenLearnerPatterns) {
      if (pattern.test(learnerBody)) {
        errors.push(
          `${lesson.lessonCode}: learner body contains teacher-facing marker ${pattern}`
        );
      }
    }

    if (!String(lesson.canonicalBodySha256 || '').match(/^[0-9a-f]{64}$/i)) {
      errors.push(`${lesson.lessonCode}: invalid canonicalBodySha256`);
    }
  }
}

if (lessons !== 1560) {
  errors.push(`expected 1560 Common Core lessons, found ${lessons}`);
}

console.log(JSON.stringify({
  files: files.length,
  lessons,
  official,
  supporting,
  errors: errors.length
}, null, 2));

if (errors.length) {
  console.error(errors.slice(0, 100).join('\n'));
  process.exit(1);
}