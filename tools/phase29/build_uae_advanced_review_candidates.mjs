#!/usr/bin/env node
// Generate review-only, source-verified lesson identity drafts from Reveal
// student-book printed contents. This does NOT create publishable pedagogy.
import { readFileSync, mkdirSync, writeFileSync } from 'node:fs';
import { dirname, resolve } from 'node:path';
import { fileURLToPath } from 'node:url';

const dir = dirname(fileURLToPath(import.meta.url));
const evidence = JSON.parse(readFileSync(resolve(dir, 'evidence/uae_advanced_student_sources_2025_2026.json'), 'utf8'));
const out = process.argv.indexOf('--out');
const outputDirectory = out >= 0 ? resolve(process.argv[out + 1]) : resolve(dir, '../../artifacts/phase29/uae-advanced-review-candidates');

function candidate(book, maxUnit) {
  const level = book.grade;
  const lessons = [];
  const units = [];
  for (const u of book.printedUnits.filter(u => u.number <= maxUnit)) {
    if (!u.verifiedNumberedLessonTitles) throw Error(`Unverified lesson titles: Grade ${level} Volume ${book.volume} Unit ${u.number}`);
    if (u.verifiedNumberedLessonTitles.length !== u.numberedLessons) throw Error('TOC count mismatch');
    const unitCode = `UAE:G${level}:ADVANCED:REVEAL:V${book.volume}:U${String(u.number).padStart(2, '0')}`;
    units.push({ unitCode, unitNumber: u.number, title: u.title, lessonCount: u.numberedLessons });
    for (let index = 0; index < u.verifiedNumberedLessonTitles.length; index++) {
      const n = index + 1;
      const code = `UAE:REF:TEXTBOOK:G${level}:ADVANCED:V${book.volume}:U${String(u.number).padStart(2, '0')}:L${String(n).padStart(2, '0')}`;
      lessons.push({
        sourceReferenceCode: code,
        sourceLessonNumber: `${u.number}-${n}`,
        sourceUnitCode: unitCode,
        unitTitle: u.title,
        title: u.verifiedNumberedLessonTitles[index],
        sourceEdition: book.sourceAcademicYear,
        sourceFileName: book.fileName,
        sourcePublisher: 'McGraw Hill',
        contentStatus: 'REVIEW_REQUIRED',
        officialStandardMapping: null,
        outcomeCode: null,
        learnerExplanation: null,
        workedExamples: null,
        authorizedForPublication: false
      });
    }
  }
  if (new Set(lessons.map(l => l.sourceReferenceCode)).size !== lessons.length) throw Error('Duplicate lesson reference');
  return {
    schemaVersion: 1, grade: level, pathway: 'Advanced', sourceVolume: book.volume,
    sourceAcademicYear: book.sourceAcademicYear,
    sourceIdentityEvidence: 'Student Edition cover and printed numbered-lesson table of contents',
    sourceRights: 'Textbook used as a reference for topic names and sequence only; no prose or exercises copied',
    isPublishable: false,
    requiresHumanMathematicsAndRightsReview: true,
    units, lessons
  };
}

const cases = [
  {book: evidence.books.find(b => b.grade === 5 && b.volume === 1), max: 7, expected: 44, name: 'uae-g5-advanced-volume1-review.json'},
  {book: evidence.books.find(b => b.grade === 5 && b.volume === 2), max: 14, expected: 48, name: 'uae-g5-advanced-volume2-review.json'},
  {book: evidence.books.find(b => b.grade === 6 && b.volume === 1), max: 5, expected: 33, name: 'uae-g6-advanced-volume1-module1to5-review.json'}
];
const results = [];
for (const c of cases) {
  if (!c.book || !c.book.coverVerified) throw Error(`Missing verified book: ${c.name}`);
  const result = candidate(c.book, c.max);
  if (result.lessons.length !== c.expected) throw Error(`Expected ${c.expected} source lessons in ${c.name}, got ${result.lessons.length}`);
  results.push({ name: c.name, result });
}
if (process.argv.includes('--write')) {
  mkdirSync(outputDirectory, { recursive: true });
  for (const {name, result} of results) writeFileSync(resolve(outputDirectory, name), JSON.stringify(result, null, 2) + '\n', 'utf8');
}
console.log('UAE_ADVANCED_REVIEW_DRAFT_PASS ' + results.map(({name, result}) => `${name}=${result.lessons.length}`).join(' '));
console.log('Review-only source identity candidates. No published content, no production seed.');
