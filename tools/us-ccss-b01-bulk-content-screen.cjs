#!/usr/bin/env node
'use strict';
// Screening of all 500 embedded, learner-facing source packs as one unit.
// The screening itself is NOT certification: L2-L6 require deeper verified evidence.
const fs = require('node:fs');
const path = require('node:path');
const root = path.resolve(__dirname, '..');
const read = p => JSON.parse(fs.readFileSync(path.join(root, p), 'utf8'));
const manifestFile = 'artifacts/academic-us-ccss/batches/B01-lesson-manifest.json';
const auditFile = 'artifacts/academic-us-ccss/04-lesson-academic-audit.json';
const contentDir = 'src/Edulytics.Core/Curriculum/LessonContent/Packs/';
const packPaths = [1,2,3,4].map(g =>
  contentDir + 'us-ccss-math-g' + g + '-phase29-v1.lesson-content-pack.json');
const fields = ['explanation','keyConceptsAndRules','workedExamples',
  'stepByStepSolutions','commonMistakes','quickSummary'];
const sha = x => typeof x === 'string' && /^[a-f0-9]{64}$/i.test(x);
// Match complete chained expressions instead of misreading `3+2 = 2+3` as `3+2=2`.
const arithmetic = /(?:\d[\d,]*\s*[+\-−×*÷]\s*)+\d[\d,]*\s*=\s*(?:\d[\d,]*\s*[+\-−×*÷]\s*)*\d[\d,]*/g;
function evaluateArithmeticSide(input) {
  const text = input.replace(/\s/g, '').replace(/,/g, '').replace(/−/g, '-').replace(/×/g, '*').replace(/÷/g, '/');
  const tokens = text.match(/\d+(?:\.\d+)?|[+\-*/]/g);
  if (!tokens || tokens.join('') !== text) return null;
  let i = 0;
  const factor = () => {
    if (tokens[i] === '-') { i++; return -factor(); }
    const token = tokens[i++];
    return /^\d+(?:\.\d+)?$/.test(token || '') ? Number(token) : NaN;
  };
  const term = () => {
    let n = factor();
    while (tokens[i] === '*' || tokens[i] === '/') {
      const op = tokens[i++], v = factor();
      n = op === '*' ? n * v : n / v;
    }
    return n;
  };
  let v = term();
  while (tokens[i] === '+' || tokens[i] === '-') {
    const op = tokens[i++], n = term();
    v = op === '+' ? v + n : v - n;
  }
  return i === tokens.length && Number.isFinite(v) ? v : null;
}
const visualCue = /dot|cube|draw|diagram|picture|chart|graph|ten.frame|number.line|shape|fraction|array|strip|base.ten/i;

function scanBatch({ manifest, audit, packs }) {
  if (manifest.batchId !== 'B01' || manifest.lessons?.length !== 500 ||
      audit.records?.length !== 1560 || packs.length !== 4)
    throw Error('B01 scope/source count mismatch');
  const provenance = new Map(audit.records.map(l => [l.lessonCode, l]));
  const content = new Map();
  const issues = [];
  for (const pack of packs) {
    if (pack.packCode !== 'US-CCSS-MATH' || pack.versionCode !== 'CCSSM-2010')
      throw Error('Incorrect Common Core content pack');
    for (const lesson of pack.lessons) {
      if (content.has(lesson.lessonCode)) throw Error('Duplicate content identity');
      content.set(lesson.lessonCode, lesson);
    }
  }
  let representationsToReview = 0;
  let equationsChecked = 0;
  let lessonsWithMathChecks = 0;
  let falseStatementsForSemanticReview = 0;
  const reviewCandidates = [];
  let covered = 0;
  const evidenceMatrix = [];
  for (const [i, item] of manifest.lessons.entries()) {
    const rowIssues = [];
    const origin = provenance.get(item.lessonCode);
    const learner = content.get(item.lessonCode);
    if (item.ordinal !== i + 1) rowIssues.push('ORDINAL_DRIFT');
    if (!origin || origin.sourceSemanticSha256 !== item.sourceSemanticSha256)
      rowIssues.push('PUBLISHER_SEMANTIC_DRIFT');
    if (!learner) rowIssues.push('MISSING_LEARNER_CONTENT');
    let localEquations = 0;
    if (learner) {
      if (origin && (learner.sourceUrl !== origin.sourceUrl ||
            JSON.stringify(learner.outcomeCodes) !== JSON.stringify(origin.officialOutcomeCodes)))
        rowIssues.push('SOURCE_OR_STANDARD_MISMATCH');
      if (learner.sourceRights !== 'CC BY 4.0' || !sha(learner.canonicalBodySha256) ||
          !sha(learner.sourceSha256)) rowIssues.push('MISSING_RIGHTS_OR_SOURCE_HASH');
      const english = learner.translations?.filter(x => x.cultureCode === 'en') ?? [];
      if (english.length !== 1) rowIssues.push('ENGLISH_CONTENT_COUNT');
      if (english.length === 1) {
        const t = english[0];
        if (t.title !== item.title) rowIssues.push('TITLE_DRIFT');
        for (const field of fields)
          if (typeof t[field] !== 'string' || !t[field].trim())
            rowIssues.push('MISSING_' + field.toUpperCase());
        const joined = fields.map(k => t[k] || '').join('\n');
        // Evaluate the COMPLETE addition/subtraction equality, including
        // associative and commutative expressions on BOTH sides.
        for (const field of fields) {
          const fieldText = (t[field] || '').replace(/\\!/g, '');
          const isTrueFalseTask = /true or false|equation is false|statement is false/i.test(fieldText);
          for (const match of fieldText.matchAll(arithmetic)) {
            const equation = match[0];
            const sides = equation.split('=');
            const lhs = evaluateArithmeticSide(sides[0]);
            const rhs = evaluateArithmeticSide(sides[1]);
            if (lhs === null || rhs === null) continue;
            localEquations++;
            if (Math.abs(lhs - rhs) > 1e-9) {
              if (isTrueFalseTask) {
                falseStatementsForSemanticReview++;
                reviewCandidates.push({ ordinal: item.ordinal,
                  lessonCode: item.lessonCode, field,
                  kind: 'TRUE_FALSE_EXPRESSION_SEMANTIC_REVIEW', equation });
              } else rowIssues.push('FALSE_EQUATION_NOT_EXPLICITLY_MARKED:' + equation);
            }
          }
        }
        if (visualCue.test(item.title) || visualCue.test(joined))
          representationsToReview++;
      }
    }
    equationsChecked += localEquations;
    if (localEquations > 0) lessonsWithMathChecks++;
    covered++;
    const uniqueIssues = [...new Set(rowIssues)];
    for (const problem of uniqueIssues)
      issues.push({ ordinal: item.ordinal, lessonCode: item.lessonCode, problem });
    evidenceMatrix.push({
      ordinal: item.ordinal, lessonCode: item.lessonCode,
      sourceMatched: !uniqueIssues.some(p => p.includes('SOURCE') || p.includes('SEMANTIC')),
      staticFieldsPresent: !uniqueIssues.some(p => p.startsWith('MISSING_') || p === 'ENGLISH_CONTENT_COUNT'),
      arithmeticEquationsChecked: localEquations, screeningIssues: uniqueIssues,
      L1toL6: 'NOT_PROVEN_BY_STATIC_SCREENING',
      status: uniqueIssues.length ? 'STRUCTURAL_ISSUES' : 'SCREENED_NOT_CERTIFIED'
    });
  }
  const report = {
    reportKind: 'B01_500_LESSON_BULK_CONTENT_SCREEN',
    coverage: { expected: 500, observed: covered, unique: new Set(evidenceMatrix.map(x => x.lessonCode)).size },
    counts: { sourceContentPacks: packs.length, equationsChecked,
      lessonsWithArithmeticSpotChecks: lessonsWithMathChecks,
      lessonsWithVisualReviewCues: representationsToReview,
      falseStatementsForSemanticReview,
      blockingStructuralIssues: issues.length },
    issues, reviewCandidates, evidenceMatrix,
    academicDecision: 'NOT_CERTIFIED',
    warning: 'Content presence/source mapping and spot-check equations are NOT sufficient to certify math, visual instruction, practice grading or Rich V2. L1-L6 + eight verified dimensions must be evidenced for all 500.'
  };
  return report;
}

function main(args = process.argv.slice(2)) {
  const result = scanBatch({
    manifest: read(manifestFile), audit: read(auditFile),
    packs: packPaths.map(read)
  });
  const output = args.find(x => x.startsWith('--output='));
  if (output) fs.writeFileSync(output.slice('--output='.length),
    JSON.stringify(result, null, 2) + '\n');
  process.stdout.write(JSON.stringify({
    reportKind: result.reportKind, coverage: result.coverage,
    counts: result.counts, issuePreview: result.issues.slice(0, 15),
    academicDecision: result.academicDecision
  }, null, 2) + '\n');
  if (args.includes('--check') &&
    (result.coverage.observed !== 500 || result.coverage.unique !== 500 ||
      result.counts.blockingStructuralIssues > 0))
    throw Error('B01 whole-batch structural/math screen found blocking issues.');
  return result;
}
if (require.main === module) {
  try { main(); }
  catch (e) { console.error('[B01 bulk content screen] ' + e.message); process.exitCode = 1; }
}
module.exports = { scanBatch, main };
