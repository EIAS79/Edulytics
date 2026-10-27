#!/usr/bin/env node
'use strict';

/**
 * B001 deterministic source/CANONICAL CONTENT inventory.
 * This deliberately does NOT certify lesson pedagogy, mathematical question
 * generation, source permissions, DB state, student-visible rendering or release.
 *
 * Usage:
 *   node tools/curriculum/us-ccss-b001-inventory.cjs --write
 *   node tools/curriculum/us-ccss-b001-inventory.cjs --check
 *   node tools/curriculum/us-ccss-b001-inventory.cjs --summary
 */
const fs = require('node:fs');
const path = require('node:path');
const assert = require('node:assert/strict');
const root = path.resolve(__dirname, '../..');
const outputPath = path.join(root, 'docs/curriculum/batches/US-CCSS-B001-CATALOGUE.json');
const inventoryPath = 'docs/curriculum/us-ccss-g1-g12-structural-audit.json';
const gradeInputs = [
  { grade: 1, count: 146, take: 146, blueprint: 'us-ccss-math-grade1-im-k5-2021.lesson-blueprint.json', content: 'us-ccss-math-g1-phase29-v1.lesson-content-pack.json' },
  { grade: 2, count: 146, take: 146, blueprint: 'us-ccss-math-grade2-im-k5-2021.lesson-blueprint.json', content: 'us-ccss-math-g2-phase29-v1.lesson-content-pack.json' },
  { grade: 3, count: 143, take: 143, blueprint: 'us-ccss-math-grade3-im-k5-2021.lesson-blueprint.json', content: 'us-ccss-math-g3-phase29-v1.lesson-content-pack.json' },
  { grade: 4, count: 149, take: 65, blueprint: 'us-ccss-math-grade4-im-k5-2021.lesson-blueprint.json', content: 'us-ccss-math-g4-phase29-v1.lesson-content-pack.json' }
];
const englishFields = ['title', 'explanation', 'keyConceptsAndRules',
  'workedExamples', 'stepByStepSolutions', 'commonMistakes', 'quickSummary'];
const isNonempty = x => typeof x === 'string' && x.trim() !== '';
const isSha256 = x => typeof x === 'string' && /^[0-9a-f]{64}$/i.test(x);
const g4IndexAliasesPath = 'artifacts/curriculum-delivery/US-CCSS-MATH/G4/g4-b001-index-display-title-aliases.json';
const json = relativePath => JSON.parse(fs.readFileSync(path.join(root, relativePath), 'utf8'));

function buildCatalogue() {
  const snapshot = json(inventoryPath);
  assert.equal(snapshot.lessonInventory.lessons, 1560, 'Source snapshot total drift');
  const seen = new Set();
  const lessons = [];
  const segments = [];
  const titleAliases = json(g4IndexAliasesPath);
  assert.equal(titleAliases.entries.length, 13, 'Grade 4 source-index title alias denominator drift');
  const expectedTitleAliasCodes = new Set(["PED:US-CCSS-MATH:G4:U01:L01","PED:US-CCSS-MATH:G4:U01:L06","PED:US-CCSS-MATH:G4:U02:L04","PED:US-CCSS-MATH:G4:U02:L08","PED:US-CCSS-MATH:G4:U02:L12","PED:US-CCSS-MATH:G4:U03:L02","PED:US-CCSS-MATH:G4:U03:L03","PED:US-CCSS-MATH:G4:U03:L10","PED:US-CCSS-MATH:G4:U03:L15","PED:US-CCSS-MATH:G4:U04:L03","PED:US-CCSS-MATH:G4:U04:L06","PED:US-CCSS-MATH:G4:U04:L16","PED:US-CCSS-MATH:G4:U04:L20"]);
  const byAliasCode = new Map(titleAliases.entries.map(row => [row.lessonCode,row]));
  assert.equal(byAliasCode.size, 13, 'Duplicate Grade 4 source-index alias');
  assert([...byAliasCode.keys()].every(code => expectedTitleAliasCodes.has(code)), 'Unreviewed Grade 4 title alias');

  for (const input of gradeInputs) {
    const blueprintPath = 'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/' + input.blueprint;
    const contentPath = 'src/Edulytics.Core/Curriculum/LessonContent/Packs/' + input.content;
    const blueprint = json(blueprintPath);
    const pack = json(contentPath);
    const label = 'Grade ' + input.grade;
    assert.equal(snapshot.lessonInventory.grades[label], input.count, 'Snapshot grade count drift ' + label);
    assert.equal(blueprint.PackCode, 'US-CCSS-MATH');
    assert.equal(pack.packCode, 'US-CCSS-MATH');
    assert.equal(blueprint.NativeLevel, label);
    assert.equal(blueprint.Lessons.length, input.count, 'Blueprint count drift ' + label);
    assert.equal(pack.lessons.length, input.count, 'Content count drift ' + label);
    assert(isNonempty(blueprint.SourceLicense), 'Missing blueprint license ' + label);
    assert(isNonempty(blueprint.SourceEdition), 'Missing blueprint edition ' + label);
    assert(isNonempty(pack.contentVersion), 'Missing content version ' + label);

    const ordered = [...blueprint.Lessons].sort((a, b) => a.SortOrder - b.SortOrder);
    const contentLookup = new Map();
    for (const row of pack.lessons) {
      assert(isNonempty(row.lessonCode) && !contentLookup.has(row.lessonCode), 'Duplicate content lesson code ' + label);
      contentLookup.set(row.lessonCode, row);
    }
    assert.equal(contentLookup.size, ordered.length, 'Content count mismatch ' + label);
    for (const [position, lesson] of ordered.entries()) {
      assert.equal(lesson.SortOrder, position + 1, 'Noncontinuous source sort order ' + label);
      assert(isNonempty(lesson.LessonCode) && !seen.has(lesson.LessonCode),
        'Duplicate/missing canonical lesson code ' + lesson.LessonCode);
      assert(new RegExp('^PED:US-CCSS-MATH:G' + input.grade + ':U\\d+:L\\d+$').test(lesson.LessonCode),
        'Grade/code mismatch ' + lesson.LessonCode);
      seen.add(lesson.LessonCode);
      const record = contentLookup.get(lesson.LessonCode);
      assert(record, 'Missing content record ' + lesson.LessonCode);
      const english = (record.translations || []).find(t => t.cultureCode === 'en');
      assert(english, 'Missing English content record ' + lesson.LessonCode);
      if (english.title !== lesson.Title) {
        const alias = byAliasCode.get(lesson.LessonCode);
        assert(alias && alias.historicalCatalogueTitle === lesson.Title &&
          alias.firstPartyDisplayTitle === english.title &&
          alias.sourceUrl === lesson.SourceUrl &&
          alias.recordedSourceSha256 === record.sourceSha256 &&
          lesson.Title === english.title + ' PLC Activity',
          'Title mismatch ' + lesson.LessonCode);
      }
      assert.equal(record.sourceUrl, lesson.SourceUrl, 'Source URL mismatch ' + lesson.LessonCode);
      assert.equal(JSON.stringify([...(lesson.OutcomeCodes || [])].sort()),
        JSON.stringify([...(record.outcomeCodes || [])].sort()), 'Outcome mismatch ' + lesson.LessonCode);
      assert(isSha256(lesson.SemanticSha256), 'Missing semantic SHA256 ' + lesson.LessonCode);
      assert(isSha256(record.canonicalBodySha256), 'Missing stored content SHA256 ' + lesson.LessonCode);
      assert(isNonempty(record.sourceRights) && isNonempty(record.sourceEdition),
        'Missing stored content provenance ' + lesson.LessonCode);

      if (position >= input.take) continue;
      const missingContentFields = englishFields.filter(name => !isNonempty(english[name]));
      const historicalT001 = input.grade === 1 && position < 10;
      lessons.push({
        batchOrdinal: lessons.length + 1,
        gradeOrCourse: label,
        gradeOrdinal: position + 1,
        lessonCode: lesson.LessonCode,
        sourceLessonCode: lesson.SourceLessonCode,
        unitNumber: lesson.UnitNumber,
        unitTitle: lesson.UnitTitle,
        lessonNumber: lesson.LessonNumber,
        title: lesson.Title,
        outcomeCodes: lesson.OutcomeCodes || [],
        sourceUrl: lesson.SourceUrl,
        sourceEdition: blueprint.SourceEdition,
        recordedLicense: blueprint.SourceLicense,
        semanticSha256: lesson.SemanticSha256,
        storedContentSha256: record.canonicalBodySha256,
        contentVersion: pack.contentVersion,
        missingContentFields,
        contentRecordStatus: missingContentFields.length ? 'INCOMPLETE_FIELDS' : 'FIELDS_PRESENT_NOT_REVIEWED',
        historicalT001,
        academicAcceptanceStatus: historicalT001 ? 'HISTORICAL_T001_DRIFT_CHECK_REQUIRED' : 'NOT_REVIEWED',
        practiceVerification: 'NOT_RUN',
        studentVisibleRendering: 'NOT_TESTED',
        productionDbReconciliation: 'NOT_RUN'
      });
    }
    segments.push({
      gradeOrCourse: label,
      plannedGradeLessonCount: input.count,
      includedGradeOrdinals: [1, input.take],
      includedLessonCount: input.take,
      blueprintPath,
      contentPath,
      contentVersion: pack.contentVersion
    });
  }

  assert.equal(lessons.length, 500, 'B001 must include exactly 500 lessons');
  assert.equal(new Set(lessons.map(x => x.lessonCode)).size, 500, 'B001 has duplicate lesson codes');
  assert.equal(lessons.filter(x => x.historicalT001).length, 10, 'Historical T001 count changed');
  assert.equal(lessons[10].lessonCode, 'PED:US-CCSS-MATH:G1:U01:L11',
    'First newly targeted lesson unexpectedly changed');
  assert.equal(lessons[499].lessonCode, 'PED:US-CCSS-MATH:G4:U04:L20',
    'Last B001 lesson unexpectedly changed');

  const present = lessons.filter(x => x.missingContentFields.length === 0).length;
  return {
    schemaVersion: 1,
    kind: 'US_CCSS_B001_SOURCE_AND_STORED_CONTENT_INVENTORY',
    planId: 'EDULYTICS-US-CCSS-500-V2',
    batchId: 'US-CCSS-B001',
    snapshotPath: inventoryPath,
    snapshotGeneratedUtc: snapshot.generatedUtc,
    authoritativeRuntimeInventoryVerified: false,
    productionDbReconciliation: 'NOT_RUN',
    mathematicalGenerationVerification: 'NOT_RUN',
    lessonAcademicAcceptance: 'NOT_RUN',
    gradeSourcesCheckedAgainstActualPublishers: false,
    total: lessons.length,
    summary: {
      canonicalSourceCodesIndexed: lessons.length,
      uniqueCanonicalSourceCodes: seen.size >= 500 ? lessons.length : seen.size,
      storedContentRowsCrossReferenced: lessons.length,
      storedEnglishContentFieldsPresent: present,
      storedEnglishContentFieldsIncomplete: lessons.length - present,
      previouslyPublishedT001LessonPositions: 10,
      newLessonPositionsToReview: 490,
      independentlyAcademicallyAcceptedInThisRun: 0,
      newLessonsReleasedInThisRun: 0
    },
    segments,
    lessons
  };
}

function main(args = process.argv.slice(2)) {
  const catalogue = buildCatalogue();
  const serialized = JSON.stringify(catalogue, null, 2) + '\n';
  if (args.includes('--write')) {
    fs.mkdirSync(path.dirname(outputPath), { recursive: true });
    fs.writeFileSync(outputPath, serialized);
  } else if (args.includes('--check')) {
    assert(fs.existsSync(outputPath), 'B001 catalogue missing; run --write');
    assert.equal(fs.readFileSync(outputPath, 'utf8'), serialized,
      'B001 catalogue does not match current canonical source/content; run --write and review diff');
  } else if (!args.includes('--summary')) {
    throw Error('Pass --write, --check or --summary');
  }
  process.stdout.write(JSON.stringify({ status: 'CATALOGUE_STRUCTURALLY_RECONCILED_ONLY',
    total: catalogue.total, summary: catalogue.summary, firstNewCode: catalogue.lessons[10].lessonCode,
    lastCode: catalogue.lessons.at(-1).lessonCode, database: 'NOT_RUN',
    practice: 'NOT_RUN', academicAcceptance: 'NOT_RUN' }) + '\n');
}

if (require.main === module) main();
module.exports = { buildCatalogue, main };
