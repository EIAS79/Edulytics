#!/usr/bin/env node
'use strict';
// V9: conservative, metadata-only index across the exact B01 500-lesson scope.
// Learner-body signals are candidates, NOT source-verified objectives, defects,
// mathematical proof, academic PASS, or license clearance.
const fs = require('node:fs');
const path = require('node:path');
const root = path.resolve(__dirname, '..');
const manifestPath = 'artifacts/academic-us-ccss/batches/B01-lesson-manifest.json';
const sourceIndexPath = 'artifacts/academic-us-ccss/batches/B01-book-source-reference-index-500.json';
const packPaths = [1, 2, 3, 4].map(g =>
  'src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g' + g + '-phase29-v1.lesson-content-pack.json');
const fields = ['explanation', 'keyConceptsAndRules', 'workedExamples',
  'stepByStepSolutions', 'commonMistakes', 'quickSummary'];
const sha = x => typeof x === 'string' && /^[0-9a-f]{64}$/i.test(x);
const assert = (condition, message) => { if (!condition) throw Error(message); };

// The detectors make NO claim to validate a calculation or infer the teaching
// goal. They only find conservative signals for subsequent shared audits.
const families = [
  { id: 'arithmetic', re: /\b(?:addition|subtraction|multiplication|division|sum|difference|product|quotient|add|subtract|multiply|divide)\b|(?:\d+\s*[+×÷]\s*\d+)/i },
  { id: 'fraction', re: /\b(?:fraction|fractions|numerator|denominator|mixed number)\b|(?:\b\d+\s*\/\s*\d+\b)/i },
  { id: 'decimal', re: /\b(?:decimal|tenths|hundredths|thousandths)\b|(?:\b\d+\.\d+\b)/i },
  { id: 'ratio_percent', re: /\b(?:ratio|ratios|percent|percentage|proportional)\b|(?:\d+\s*%)/i },
  { id: 'exponent', re: /\b(?:exponent|exponents|powers of|power of|squared|cubed)\b|(?:[a-z0-9)]\s*(?:\^|\*\*)\s*[-+]?\d+)/i },
  { id: 'root', re: /\b(?:square root|cube root|nth root|radical)\b|(?:[√∛∜])/i },
  { id: 'algebra_equation', re: /\b(?:equation|inequality|variable|expression|linear function|polynomial)\b/i },
  { id: 'geometry', re: /\b(?:angle|triangle|rectangle|polygon|quadrilateral|circle|perimeter|area|volume|shape|congruent|similarity)\b/i },
  { id: 'measurement_units', re: /\b(?:measure|measurement|centimeter|millimeter|kilometer|meter|inch|foot|feet|hour|minute|length|mass|weight)\b/i },
  { id: 'data_probability', re: /\b(?:probability|statistics|mean|median|mode|scatter plot|line plot|bar graph|histogram|data set)\b/i },
  { id: 'visual_number_line', re: /\b(?:number line|number-line|ten.frame|tape diagram|dot plot|array diagram)\b/i }
];

function buildIndex({ manifest, packs, sourceIndex }) {
  assert(manifest?.batchId === 'B01' && manifest.expectedLessonCount === 500 &&
    Array.isArray(manifest.lessons) && manifest.lessons.length === 500,
    'B01 must have exactly 500 manifest lessons');
  assert(sourceIndex?.batchId === 'B01' && sourceIndex.expectedLessonCount === 500 &&
    sourceIndex.matchedToManifest === 500 && Array.isArray(sourceIndex.rows) &&
    sourceIndex.rows.length === 500, 'B01 original teacher-source index must cover 500/500');
  assert(Array.isArray(packs) && packs.length === 4, 'Expected four G1-G4 pack files');
  const packLessonMap = new Map();
  for (const pack of packs) {
    assert(pack?.packCode === 'US-CCSS-MATH' && pack?.versionCode === 'CCSSM-2010' &&
      Array.isArray(pack.lessons), 'Invalid Common Core learner pack');
    for (const row of pack.lessons) {
      assert(typeof row.lessonCode === 'string' && row.lessonCode.length > 0,
        'Learner body missing identity');
      assert(!packLessonMap.has(row.lessonCode),
        'Duplicate learner content: ' + row.lessonCode);
      packLessonMap.set(row.lessonCode, row);
    }
  }
  const seen = new Set(), rows = [], affected = new Map(families.map(f => [f.id, []]));
  for (const [ordinalIndex, m] of manifest.lessons.entries()) {
    assert(typeof m.lessonCode === 'string' && !seen.has(m.lessonCode),
      'Duplicate/missing B01 manifest identity');
    seen.add(m.lessonCode);
    assert(sha(m.sourceSemanticSha256), 'Missing source semantic fingerprint: ' + m.lessonCode);
    const source = sourceIndex.rows[ordinalIndex];
    assert(source?.ordinal === ordinalIndex + 1 &&
      source.lessonCode === m.lessonCode &&
      sha(source.localFileByteSha256) &&
      Number.isInteger(source.localFileBytes) && source.localFileBytes > 0 &&
      typeof source.localRelativePath === 'string' && source.localRelativePath.length > 0 &&
      source.state === 'REFERENCE_FILE_INVENTORIED_NOT_ACADEMICALLY_CERTIFIED',
      'Source inventory provenance missing/mismatched: ' + m.lessonCode);
    const row = packLessonMap.get(m.lessonCode);
    assert(row, 'Missing learner body: ' + m.lessonCode);
    assert(sha(row.canonicalBodySha256), 'Missing learner body fingerprint: ' + m.lessonCode);
    assert(Array.isArray(row.translations), 'Missing translations: ' + m.lessonCode);
    const en = row.translations.find(t => t.cultureCode === 'en');
    assert(en, 'Missing English learner body: ' + m.lessonCode);
    const hits = [];
    for (const f of families) {
      const matched = fields.filter(field => typeof en[field] === 'string' && f.re.test(en[field]));
      if (matched.length) {
        hits.push({ family: f.id, fields: matched, classification: 'UNVERIFIED_ELEMENT_CANDIDATE' });
        affected.get(f.id).push(m.lessonCode);
      }
    }
    rows.push({
      lessonCode: m.lessonCode,
      sourceSemanticSha256: m.sourceSemanticSha256,
      canonicalBodySha256: row.canonicalBodySha256,
      originalTeacherSource: {
        relativePath: source.localRelativePath,
        rawFileSha256: source.localFileByteSha256,
        bytes: source.localFileBytes,
        state: 'HASH_INVENTORIED_NOT_ACADEMICALLY_VERIFIED'
      },
      primaryTeachingSkill: null, // MUST come from verified academic source, not keywords
      mathematicalElements: hits,
      academicSourceVerification: 'NOT_PERFORMED_BY_THIS_INDEX',
      defectDecision: 'NOT_ASSESSED',
      certificationDecision: 'NOT_CERTIFIED'
    });
  }
  assert(rows.length === 500 && seen.size === 500, 'B01 500/500 coverage failed');
  const byFamily = Object.fromEntries([...affected].map(([key, list]) => [
    key, { candidateLessons: list.length, lessonCodes: list }
  ]));
  return {
    schemaVersion: 1,
    batchId: 'B01',
    method: 'UNVERIFIED_LEARNER_CONTENT_ELEMENT_CANDIDATE_INDEX',
    coverage: { expected: 500, observed: rows.length, unique: seen.size, missing: 0 },
    source: { manifestPath, sourceIndexPath, learnerPacks: packPaths },
    classificationPolicy: 'No primary skill inferred; no repairs or certification from this index',
    byFamily,
    rows,
    academicDecision: 'NOT_CERTIFIED',
    eligibleForAutomaticRepair: false
  };
}

function load() {
  const read = p => JSON.parse(fs.readFileSync(path.join(root, p), 'utf8'));
  return { manifest: read(manifestPath), sourceIndex: read(sourceIndexPath), packs: packPaths.map(read) };
}
if (require.main === module) {
  const out = process.argv.find(x => x.startsWith('--out='));
  const result = buildIndex(load());
  const json = JSON.stringify(result, null, 2) + '\n';
  if (out) fs.writeFileSync(out.slice(6), json, 'utf8');
  else process.stdout.write(json);
  process.stderr.write('V9 B01 math element candidates indexed: 500/500; NO academic PASS.\n');
}
module.exports = { buildIndex, families };
