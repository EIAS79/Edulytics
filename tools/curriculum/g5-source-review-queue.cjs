#!/usr/bin/env node
'use strict';
// Deterministic 2021 source review queue: source offsets/hashes are NOT academic parity.
const fs = require('node:fs');
const path = require('node:path');
const {
  inventory, analyzeElementInventory, grade5EditionManifests,
  original2021StudentFacingLocator, original2021StudentFacingSectionProvenance
} = require('./g5-stage-d-diagnostic.cjs');

const root = path.resolve(__dirname, '../..');
const sourceRoot = 'artifacts/curriculum-delivery/US-CCSS-MATH/G5/';
const richDir = path.join(root, 'src/Edulytics.Core/Curriculum/LessonContent/RichV2');
const read = file => JSON.parse(fs.readFileSync(path.join(root, file), 'utf8'));
function richV2Index() {
  const byCode = new Map(), duplicates = [];
  const filenames = fs.readdirSync(richDir).filter(name =>
    /^us-ccss-g5-.*\.rich-lesson-v2\.json$/.test(name)).sort();
  for (const filename of filenames) {
    const relative = 'src/Edulytics.Core/Curriculum/LessonContent/RichV2/' + filename;
    const pack = read(relative);
    for (const lesson of pack.Lessons || []) {
      if (lesson.CultureCode !== 'en') continue;
      const code = lesson.LessonCode;
      if (byCode.has(code)) {
        duplicates.push({lessonCode: code, files: [byCode.get(code).path, relative]});
        continue;
      }
      byCode.set(code, {
        path: relative,
        contentVersion: pack.ContentVersion,
        title: lesson.Title || null,
        paragraphCount: (lesson.ExplanationParagraphs || []).length
      });
    }
  }
  return {byCode, duplicates};
}
function buildReviewQueue() {
  const baseline = inventory();
  if (baseline.errors.length) throw Error('Grade5 manifest failed: ' + baseline.errors.slice(0, 5));
  const editions = grade5EditionManifests();
  const firstEdition = original2021StudentFacingSectionProvenance(
    editions, original2021StudentFacingLocator());
  const rich = richV2Index();
  const lessons = [], errors = [...rich.duplicates.map(x => 'DUPLICATE_RICH_' + x.lessonCode)];
  const observedElements = new Set(), observedOriginalSha = new Set();
  let studentFacingSectionCount = 0, figureReferences = 0;
  const uniqueFigureIds = new Set();
  for (const record of baseline.records) {
    const original = firstEdition.get(record.code);
    const source = read(record.evidence);
    const diagnostic = analyzeElementInventory(source, record.code);
    const richLesson = rich.byCode.get(record.code) || null;
    if (!richLesson) errors.push('MISSING_EFFECTIVE_RICH_V2_' + record.code);
    if (!original) {
      errors.push('MISSING_2021_SOURCE_' + record.code);
      continue;
    }
    if (observedOriginalSha.has(record.code)) errors.push('DUPLICATE_ORIGINAL_' + record.code);
    observedOriginalSha.add(record.code);
    const elements = original.studentFacingSections.map((section, i) => {
      const id = record.code + ':2021_STUDENT_FACING_SECTION_' + String(i + 1).padStart(2, '0');
      if (observedElements.has(id)) errors.push('DUPLICATE_ORIGINAL_SECTION_' + id);
      observedElements.add(id);
      if (!/^[a-f0-9]{64}$/.test(section.sourceSectionSha256) ||
          !Number.isInteger(section.sourceHtmlStartOffset) ||
          !Number.isInteger(section.sourceHtmlCharacterLength) ||
          section.sourceHtmlCharacterLength <= 0) errors.push('INVALID_SOURCE_SECTION_' + id);
      const figures = section.embeddedFigureRecordIds;
      figureReferences += figures.length;
      figures.forEach(x => uniqueFigureIds.add(x));
      return {
        sourceElementId: id,
        edition: 'Illustrative Mathematics IM K-5 Grade 5 First Edition 2021',
        sourceHtmlRelativePath: original.sourceRelativePath,
        teacherSourceHtmlSha256: original.teacherSourceHtmlSha256,
        sourceSectionSha256: section.sourceSectionSha256,
        sourceSectionOrdinal: section.ordinal,
        sourceHtmlStartOffset: section.sourceHtmlStartOffset,
        sourceHtmlCharacterLength: section.sourceHtmlCharacterLength,
        sourceEmbeddedFigureRecordIds: figures,
        candidateRichV2ArtifactPath: richLesson?.path || null,
        mappingStatus: 'SOURCE_LOCATED_SEMANTIC_MAPPING_NOT_VERIFIED',
        originalArtRightsStatus: figures.length ? 'REVIEW_REQUIRED' : 'NOT_APPLICABLE_TO_SECTION',
        originalStudentPdfStatus: 'NOT_VERIFIED',
        humanSourceApproval: null
      };
    });
    studentFacingSectionCount += elements.length;
    const unknownFullDenominator = diagnostic.sourceElements === null ||
      String(source.sourceCoverageDenominator || '').startsWith('UNKNOWN_FULL_SOURCE_') ||
      source.sourceCoverage100Percent !== true;
    lessons.push({
      ordinal: record.ordinal,
      lessonCode: record.code,
      handoffEvidencePath: record.evidence,
      richV2: richLesson,
      identifiedLegacyElementCount: diagnostic.identified,
      legacyStructuredElementCount: diagnostic.sourceElements?.length ?? null,
      legacyMatchedElementCountClaim: diagnostic.matched,
      legacyItemizationBlockers: diagnostic.blockers,
      sourceCompleteness: unknownFullDenominator ? 'FULL_SOURCE_DENOMINATOR_NOT_VERIFIED' :
        'LEGACY_100_PERCENT_CLAIM_REQUIRES_INDEPENDENT_VALIDATION',
      original2021StudentFacingSectionCount: elements.length,
      original2021StudentFacingSections: elements,
      activeACExplicitGap: source.status === 'A_C_EXPLICIT_GAP',
      reviewerRequired: true,
      sourceRightsReviewRequired: true,
      academicApproval: 'PENDING_INDEPENDENT_SOURCE_REVIEW',
      dVerified: false
    });
  }
  if (lessons.length !== 148 || observedElements.size !== 483) errors.push('2021_SECTION_COUNT_OR_LESSON_SCOPE_MISMATCH');
  if (studentFacingSectionCount !== 483) errors.push('2021_SECTION_TOTAL_MISMATCH');
  return {
    schemaVersion: 1,
    artifactKind: '2021_SOURCE_SECTION_REVIEW_QUEUE_NOT_SOURCE_PARITY_CERTIFICATION',
    sourceEdition: 'IM K-5 Grade 5 First Edition 2021 - TEACHER Student Facing',
    sourceCoverageLimit: 'Only original teacher Student Facing sections; warm-ups, teacher moves, PDFs, cool-downs, assets, and nested tasks require separate itemization and verification.',
    fullSourceElementDenominator: null,
    sourceParityPercent: null,
    independentlyAcademicallyApproved: 0,
    summary: {
      lessons: lessons.length,
      originalStudentFacingSections: studentFacingSectionCount,
      originalFigureReferences: figureReferences,
      distinctFigureRecordIds: uniqueFigureIds.size,
      linkedRichV2Lessons: lessons.filter(x => x.richV2).length,
      legacyStructuredElementLessons: lessons.filter(x => x.legacyStructuredElementCount !== null).length,
      unresolvedFullSourceDenominatorLessons: lessons.filter(x =>
        x.sourceCompleteness === 'FULL_SOURCE_DENOMINATOR_NOT_VERIFIED').length,
      explicitLegacyACGaps: lessons.filter(x => x.activeACExplicitGap).map(x => x.lessonCode),
      mappedAndIndependentlyVerifiedSections: 0
    },
    errors, lessons
  };
}
if (require.main === module) {
  const queue = buildReviewQueue();
  const output = path.join(root, 'stage-d-results/grade5-original-2021-source-review-queue.json');
  fs.mkdirSync(path.dirname(output), {recursive: true});
  fs.writeFileSync(output, JSON.stringify(queue, null, 2) + '\n');
  console.log('G5_SOURCE_REVIEW_QUEUE ' + JSON.stringify(queue.summary));
  console.log('G5_SOURCE_REVIEW_QUEUE_ARTIFACT stage-d-results/grade5-original-2021-source-review-queue.json');
  if (queue.errors.length) {
    console.error('G5_SOURCE_REVIEW_QUEUE_INVALID ' + JSON.stringify(queue.errors.slice(0, 20)));
    process.exitCode = 2;
  }
}
module.exports = {buildReviewQueue, richV2Index};
