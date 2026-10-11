#!/usr/bin/env node
'use strict';
// PHASE 04: deterministic, source-risk-based review ordering.
// This is a queue of unreviewed lessons, not a mathematics correctness verdict.
const fs = require('node:fs');
const path = require('node:path');

const root = path.resolve(__dirname, '..');
const dir = path.join(root, 'artifacts/academic-us-ccss');
const input = JSON.parse(fs.readFileSync(path.join(dir,
  '04-automated-content-risk-audit.json'), 'utf8'));
if (input.scope !== 'US-CCSS-MATH/CCSSM-2010/G1-G12' ||
    input.lessons.length !== 1560 ||
    input.summary.humanReviewed !== 0 ||
    input.lessons.some(l => l.academicReviewStatus !== 'PENDING_INDEPENDENT_REVIEW')) {
  throw Error('Refusing to prioritize an unexpected or academically signed audit');
}

const repeatedCodes = new Set(input.repeatedProcedures
  .filter(g => g.identicalLessonCount >= 4)
  .flatMap(g => g.sampleLessonCodes));
// Full membership is not available in the report's sample groups. Reconstruct
// it from source content in the upstream audit, rather than infer from samples.
const crypto = require('node:crypto');
const packCache = new Map();
const groups = new Map();
for (const l of input.lessons) {
  if (!l.contentSourceFile) continue;
  let pack = packCache.get(l.contentSourceFile);
  if (!pack) {
    pack = JSON.parse(fs.readFileSync(path.join(root, l.contentSourceFile), 'utf8'));
    packCache.set(l.contentSourceFile, pack);
  }
  const body = pack.lessons.find(x => x.lessonCode === l.lessonCode);
  if (!body) throw Error('Content source missing: ' + l.lessonCode);
  const en = (body.translations || []).find(x => x.cultureCode === 'en');
  const steps = (en?.stepByStepSolutions || '').trim();
  if (steps.length < 40) continue;
  const key = crypto.createHash('sha256').update(steps).digest('hex');
  if (!groups.has(key)) groups.set(key, []);
  groups.get(key).push(l.lessonCode);
}
const repeated = new Set([...groups.values()].filter(g => g.length >= 4).flat());
if (repeated.size !== input.summary.lessonsSharingOneOfThoseProcedures)
  throw Error('Repeated-procedure source population drift');
if (input.summary.riskFlagCounts.PUBLISHER_HTML_ENTITY_LEAK_IN_WORKED_EXAMPLES !== 20 ||
    input.summary.riskFlagCounts.MATH_TOPIC_PROCEDURE_CONFLICT_MULTIPLES_VS_FIGURE_CLASSIFICATION !== 1)
  throw Error('Raw-source risk counts drift: regenerate and review the input audit');

const codes = new Set();
const rows = input.lessons.map(l => {
  if (codes.has(l.lessonCode)) throw Error('Duplicate lesson in audit');
  codes.add(l.lessonCode);
  const flags = l.contentRiskFlags;
  const topicConflict = flags.includes('MATH_TOPIC_PROCEDURE_CONFLICT_MULTIPLES_VS_FIGURE_CLASSIFICATION');
  const sourceEntities = flags.includes('PUBLISHER_HTML_ENTITY_LEAK_IN_WORKED_EXAMPLES');
  const repeatedProcedure = repeated.has(l.lessonCode);
  const priority = topicConflict ? 'P0' : sourceEntities ? 'P1' : repeatedProcedure ? 'P2' : 'P3';
  const guardedCorrectionCandidate = l.lessonCode === 'PED:US-CCSS-MATH:G4:U01:L01';
  if (guardedCorrectionCandidate !== topicConflict)
    throw Error('Source conflict/candidate identity drift: ' + l.lessonCode);
  return {lessonCode:l.lessonCode, priority, publisherSourceUrl:l.publisherSourceUrl,
    contentSourceFile:l.contentSourceFile, rawSourceRiskFlags:flags.join(';'),
    repeatedProcedure, guardedCorrectionCandidate,
    academicReviewStatus:l.academicReviewStatus};
}).sort((a,b) => a.priority.localeCompare(b.priority) ||
  a.lessonCode.localeCompare(b.lessonCode));
const counts = Object.fromEntries(['P0','P1','P2','P3'].map(p =>
  [p, rows.filter(x => x.priority === p).length]));
if (rows.length !== 1560 || counts.P0 !== 1 || counts.P1 !== 19 ||
    Object.values(counts).reduce((a,b) => a+b,0) !== 1560)
  throw Error('Review queue priority partition is inconsistent');
const csv = v => '"' + String(v??'').replaceAll('"','""') + '"';
const headings = ['lessonCode','priority','publisherSourceUrl','contentSourceFile',
  'rawSourceRiskFlags','repeatedProcedure','guardedCorrectionCandidate','academicReviewStatus'];
const csvText = [headings.join(','), ...rows.map(x =>
  headings.map(h => csv(x[h])).join(','))].join('\n')+'\n';
const flagged = rows.filter(x => x.priority === 'P0' || x.priority === 'P1');
const md = [
  '# US-CCSS Phase 04 — prioritized independent lesson review queue',
  '',
  '**Status: NO-GO — 0/1,560 independently reviewed.** This queue is an',
  'engineering triage artifact, not a source-based academic sign-off.',
  '',
  '| Priority | Lesson count | Reason |',
  '|---|---:|---|',
  '| P0 | '+counts.P0+' | Original source shows explicit skill/procedure conflict |',
  '| P1 | '+counts.P1+' | Original source has encoded HTML entities; verify actual learner presentation before editing |',
  '| P2 | '+counts.P2+' | Identical long step procedures shared by four or more lessons; manual specificity review |',
  '| P3 | '+counts.P3+' | No current machine flag; still requires academic lesson review |',
  '',
  'These categories are **review priorities, not defect counts or approvals**.',
  'P0 is the historical G4 U01 L01 raw-source mismatch: a guarded correction',
  'candidate exists on PR #464, but this does not establish deployed content,',
  'real-user presentation, independent sign-off, or full lesson certification.',
  'P1 includes source encoding warnings even where learner UI may already decode',
  'entities. Do not rewrite canonical source or alter ContentVersion just to',
  'eliminate these warnings.',
  '',
  '## Immediate P0/P1 reviewer queue',
  '',
  '| Priority | Lesson | Publisher reference | Required next check |',
  '|---|---|---|---|',
  ...flagged.map(x => '| '+x.priority+' | `'+x.lessonCode+'` | '+(
    /^https:\/\//.test(x.publisherSourceUrl) ? '[Source]('+x.publisherSourceUrl+')' : 'Source URL needs verification')+
    ' | '+(x.guardedCorrectionCandidate ?
      'Verify guarded runtime correction, student-facing render and independent academic review' :
      'Inspect source notation vs rendered text; do not assume learner-visible failure')+' |'),
  '',
  '## Remaining review queue',
  '',
  '- All 1,560 individual lessons, including P2 and P3, are enumerated in',
  '  `04-prioritized-lesson-review-queue.csv` with source file, publisher URL,',
  '  raw-source flags and review status.',
  '- The exact repeated-procedure population is re-derived from canonical source',
  '  strings and checked against the existing 1,420-lesson source audit.',
  '- A reviewer must validate content, examples, representations, exact skill',
  '  alignment, question-family suitability, math correctness, and provenance.',
  '- No row is approved automatically. Human sign-off and a candidate-SHA',
  '  release check remain mandatory before declaring any phase gate PASS.',
  '',
  'Source baseline SHA: `'+input.sourceCommitSha+'`.',
  ''
].join('\n');
for(const [name,text] of [
  ['04-prioritized-lesson-review-queue.csv',csvText],
  ['04-review-queue.md',md]]) {
  const target=path.join(dir,name);
  if (process.argv.includes('--write')) fs.writeFileSync(target,text);
  else if (!fs.existsSync(target) || fs.readFileSync(target,'utf8') !== text)
    throw Error('Review queue missing or drift: '+name);
}
console.log(JSON.stringify({lessons:rows.length,counts,
  highPriority:flagged.map(x=>x.lessonCode),humanReviewed:0},null,2));
