#!/usr/bin/env node
'use strict';
// B01 collective eight-dimension disposition report. This joins EXISTING audit
// artifacts; it never re-reviews a completed lesson, certifies one, or edits data.
const fs = require('node:fs');
const path = require('node:path');
const ROOT = path.resolve(__dirname, '..');
const DIMENSIONS = ['title','standard','explanation','examples','solutions',
  'visualRepresentations','commonMistakes','questions'];
const CORRECT = 'CORRECT';
const REWORK = 'NEEDS_REPAIR';
const BLOCKED = 'BLOCKED_MISSING_EVIDENCE';
const read = (name) => JSON.parse(fs.readFileSync(path.resolve(ROOT, name), 'utf8'));
const assert = (condition, message) => { if (!condition) throw Error(message); };
function strictMap(array, source, key = 'lessonCode') {
  assert(Array.isArray(array) && array.length === 500,
    source + ': expected exactly 500 evidence rows');
  const byCode = new Map();
  for (const row of array) {
    assert(typeof row[key] === 'string' && !byCode.has(row[key]),
      source + ': null or repeated lesson identity');
    byCode.set(row[key], row);
  }
  return byCode;
}
function summarize(manifest, ledger, bulk, runtime, publisher, practice, rich, visual) {
  assert(manifest.batchId === 'B01' && manifest.lessons.length === 500,
    'B01 manifest must contain exactly 500 lessons');
  const rows = {
    bulk: strictMap(bulk.evidenceMatrix, 'static-screen'),
    runtime: strictMap(runtime.lessonRows, 'effective-runtime'),
    publisher: strictMap(publisher.rows, 'publisher-source'),
    practice: strictMap(practice.rows, 'question-source'),
    rich: strictMap(rich.rows, 'Rich-V2'),
    visual: strictMap(visual.rows, 'source-visual')
  };
  const ledgerRows = new Map(ledger.records.map(item => [item.lessonCode, item]));
  const review = [];
  const totals = { [CORRECT]: 0, [REWORK]: 0, [BLOCKED]: 0 };
  const flaggedByDimension = Object.fromEntries(DIMENSIONS.map(d => [d, 0]));
  const commonProblemCodes = {};
  const codes = new Set();
  for (const item of manifest.lessons) {
    assert(!codes.has(item.lessonCode), 'Duplicate manifest lesson ' + item.lessonCode);
    codes.add(item.lessonCode);
    const code = item.lessonCode;
    const s = Object.fromEntries(Object.entries(rows).map(([k,v]) => [k, v.get(code)]));
    assert(Object.values(s).every(Boolean), 'Missing audit evidence for ' + code);
    assert(Object.values(s).every(r => r.ordinal === item.ordinal),
      'Evidence ordinal mismatch ' + code);
    const evidence = ledgerRows.get(code);
    assert(evidence, 'No canonical ledger row ' + code);
    const dimensions = {};
    for (const d of DIMENSIONS) dimensions[d] = {
      status: BLOCKED, findings: ['NO_COMPLETE_SOURCE_BOUND_DIMENSION_PROOF']
    };
    const faults = [];
    const cues = [];
    const set = (key, reason, type = REWORK) => {
      const d = dimensions[key];
      if (type === REWORK) {
        d.status = REWORK;
        d.findings = d.findings.filter(f => f !== 'NO_COMPLETE_SOURCE_BOUND_DIMENSION_PROOF');
        d.findings.push(reason);
        faults.push({dimension:key,code:reason});
        flaggedByDimension[key]++;
        commonProblemCodes[reason] = (commonProblemCodes[reason] || 0) + 1;
      } else {
        d.findings.push(reason);
        cues.push({dimension:key,code:reason});
      }
    };
    for (const issue of s.bulk.screeningIssues || []) {
      const d = issue === 'TITLE_DRIFT' ? 'title' :
        issue === 'SOURCE_OR_STANDARD_MISMATCH' ? 'standard' :
        issue.includes('SOLUTION') || issue.startsWith('FALSE_EQUATION') ? 'solutions' :
        issue.includes('EXAMPLE') ? 'examples' :
        issue.includes('MISTAKE') ? 'commonMistakes' : 'explanation';
      set(d, 'STATIC_' + issue);
    }
    for (const issue of s.runtime.engineIssues || []) {
      const d = /SOURCE_URL|OUTCOME/.test(issue) ? 'standard' :
        /TITLE/.test(issue) ? 'title' : 'questions';
      set(d, 'RUNTIME_' + issue);
    }
    if (s.runtime.effectiveContentSha256 &&
        s.publisher.effectiveContentSha256 &&
        s.runtime.effectiveContentSha256 !== s.publisher.effectiveContentSha256)
      set('explanation', 'EFFECTIVE_SOURCE_ANALYSIS_SHA_DRIFT');
    for (const candidate of s.publisher.issues || []) {
      // These source/goal comparisons are triage, not proof of wrong mathematics.
      // Known manifest URL mismatch is a concrete identity failure.
      if (candidate === 'EFFECTIVE_SOURCE_URL_MISMATCH')
        set('standard', candidate);
      else
        set(candidate.includes('TITLE') ? 'title' :
          candidate.includes('CODES') ? 'standard' : 'explanation',
          'PUBLISHER_REVIEW_' + candidate, BLOCKED);
    }
    if (s.publisher.verdict === 'BLOCKED')
      set('standard', 'PUBLISHER_SOURCE_UNAVAILABLE', BLOCKED);
    for (const c of s.practice.conflicts || [])
      set('questions', 'PRACTICE_' + c.rule);
    if (s.rich.status === 'NO_RICH_V2_CONTENT' ||
        s.rich.status === 'NO_CANONICAL_SOURCE')
      set('explanation', s.rich.status);
    if (s.rich.visualCount === 0)
      set('visualRepresentations', 'NO_RICH_V2_VISUAL_TO_INSPECT', BLOCKED);
    for (const missing of s.visual.publisherCueVisualReviewTypes || [])
      set('visualRepresentations', 'PUBLISHER_VISUAL_CUE_' + missing, BLOCKED);
    if (s.visual.sameEffectiveContent === false)
      set('visualRepresentations', 'VISUAL_EVIDENCE_BODY_SHA_MISMATCH');
    if (s.rich.commonMistakes === 0)
      set('commonMistakes', 'NO_RICH_V2_COMMON_MISTAKES');
    if (s.rich.workedExampleCount === 0)
      set('examples', 'NO_RICH_V2_WORKED_EXAMPLES');
    if (s.rich.explanationParagraphs === 0)
      set('explanation', 'NO_RICH_V2_EXPLANATION');
    if (s.runtime.generatedCases === 0)
      set('questions', 'NO_GENERATED_QUESTIONS');
    // Earlier defect findings remain until the corrected *effective* body,
    // verified source-target question families and correctly bound Rich V2
    // dot-picture evidence all occur on this same 500-lesson candidate.
    // Even when these conditions hold, this is only a repaired DEFECT, not
    // independent source-semantic or rendered-browser L1-L6 certification.
    if (code === 'PED:US-CCSS-MATH:G1:U01:L02') {
      const originalActivities = [
        'usccss.b01.g1.match_dot_cubes_expression',
        'usccss.b01.g1.complete_missing_dots',
        'usccss.b01.g1.check_off_add_within10'
      ];
      const sourceTaskRepairReady =
        s.runtime.effectiveContentVersion ===
          'usccss-b01-g1-u01-l02-source-dot-cards-v1' &&
        JSON.stringify(s.runtime.actualQuestionFamilies) ===
          JSON.stringify(originalActivities) &&
        s.rich.visualKinds?.includes('PlaceValueChart') &&
        s.rich.workedExampleCount >= 3 &&
        s.visual.sameEffectiveContent === true &&
        s.publisher.effectiveContentSha256 ===
          s.runtime.effectiveContentSha256 &&
        s.practice.conflicts?.length === 0;
      if (sourceTaskRepairReady) {
        set('examples', 'SOURCE_DOT_CARD_REPAIR_EVIDENCED_IN_EFFECTIVE_BODY_AND_ENGINE',
          BLOCKED);
        set('visualRepresentations',
          'SOURCE_DOT_PICTURE_COMPOSED_RENDERING_AND_INDEPENDENT_L4_L6_STILL_REQUIRED',
          BLOCKED);
      } else {
        set('examples',
          'PREVIOUS_SOURCE_REVIEW_MISSING_DOT_CUBE_MATCHING_ACTIVITY');
        set('visualRepresentations',
          'PREVIOUS_SOURCE_REVIEW_MISSING_DOT_CUBE_DRAWING_EVIDENCE',
          BLOCKED);
      }
    }
    const historical = code === 'PED:US-CCSS-MATH:G1:U01:L01' &&
      evidence.status === 'INTERNALLY_CERTIFIED' &&
      evidence.academicDecision === 'INTERNALLY_CERTIFIED';
    const shaSame = s.runtime.effectiveContentSha256 &&
      s.runtime.effectiveContentSha256 === evidence.contentSha256;
    const admittedHistorical = historical && shaSame && faults.length === 0;
    let verdict = faults.length ? REWORK : BLOCKED;
    if (admittedHistorical) {
      verdict = CORRECT;
      // Preserve signed-off historical dimension evidence without reworking.
      for (const d of DIMENSIONS) {
        dimensions[d] = {
          status: CORRECT,
          findings: ['PR_488_HISTORICAL_INTERNAL_SOURCE_SHA_VERIFIED']
        };
      }
    }
    totals[verdict]++;
    review.push({
      ordinal:item.ordinal, lessonCode:code, title:item.title,
      gradeOrCourse:item.gradeOrCourse, sourceUrl:item.sourceUrl,
      officialOutcomeCodes:item.officialOutcomeCodes,
      sourceSemanticSha256:item.sourceSemanticSha256,
      effectiveContentSha256:s.runtime.effectiveContentSha256 ?? null,
      historicCertification:admittedHistorical ? 'PR_488_REUSED' :
        historical ? 'PR_488_SHA_RECHECK_REQUIRED' : null,
      dimensions, verdict,
      confirmedDefects:faults, missingEvidenceOrReviewCues:cues,
      evidenceSources:{
        staticScreen:'b01-bulk-content-screen',
        effectiveRuntime:'b01-effective-content-and-practice',
        publisher:'b01-publisher-goal-alignment-500',
        sourceQuestionMatch:'b01-500-lesson-practice-source-conflicts',
        richV2:'b01-rich-v2-composition-500',
        visualSourceMatch:'b01-rich-v2-publisher-visual-gaps',
        historicalCheckpoint:code.endsWith(':G1:U01:L02') ?
          'artifacts/academic-us-ccss/batches/B01-G1-U01-L02-review-checkpoint.md' : null
      }
    });
  }
  assert(codes.size === 500 && Object.values(totals).reduce((a,b) => a+b,0) === 500,
    'B01 disposition cardinality is not exactly 500');
  return {
    schemaVersion:1, kind:'B01_UNIFIED_EIGHT_DIMENSION_DISPOSITION',
    branch:'audit/us-ccss-b01-500-lesson-certification',
    batchId:'B01', total:500, sourceOrdered:true, dimensions:DIMENSIONS,
    decisionPolicy:'CORRECT only with unchanged PR488 verified evidence; NEEDS_REPAIR only with identified defects; BLOCKED otherwise until full L1-L6/eight-dimension SHA evidence',
    coverage:{expected:500, joined:review.length, unique:codes.size},
    counts:totals, confirmedDefectsByDimension:flaggedByDimension,
    detectedProblemCodes:commonProblemCodes,
    batchDecision:'BATCH_NO-GO',
    academicCertification:'NOT_500_BATCH_CERTIFIED',
    note:'Reported verdicts are a conservative evidence triage, NOT 500 independent certifications. Publisher overlap and visual cues are not sufficient to assert error or correctness; authenticated browser, marking, persistence and full source semantic evidence remain required.',
    lessons:review
  };
}
function main() {
  const dir = 'artifacts/academic-us-ccss/batches/';
  const report = summarize(
    read(dir+'B01-lesson-manifest.json'),
    read('artifacts/academic-us-ccss/04-lesson-certification-records.json'),
    read('b01-bulk/b01-bulk-screen.json'),
    read('b01-effective/b01-effective-content-and-practice.json'),
    read('b01-publisher/b01-publisher-alignment-500.json'),
    read('b01-practice/b01-practice-source-conflicts-500.json'),
    read('b01-rich/b01-rich-v2-500.json'),
    read('b01-visual/b01-rich-v2-source-visual-gaps-500.json')
  );
  fs.writeFileSync('b01-unified-eight-dimension-report-500.json',
    JSON.stringify(report, null, 2)+'\n');
  const lines = [
    '# B01 — Unified 500-lesson eight-dimension disposition',
    '',
    '**Decision:** BATCH_NO-GO. This is one report, not individual certification.',
    '**Source:** PR #492, historical PR #488 evidence reused unchanged.',
    '',
    '| Verdict | Lessons |','|---|---:|',
    ...Object.entries(report.counts).map(([k,v])=>'| '+k+' | '+v+' |'),
    '',
    'Detailed evidence, eight dimension findings and all 500 exact LessonCodes:',
    'b01-unified-eight-dimension-report-500.json',
    '',
    'Classifications require real SHA-backed sources. Automated screening alone never means correct.',
    'No partial B01 release or Render deployment authorized.'
  ];
  fs.writeFileSync('b01-unified-eight-dimension-summary.md',lines.join('\n')+'\n');
  process.stdout.write(JSON.stringify({
    kind:report.kind,coverage:report.coverage,counts:report.counts,
    confirmedDefectsByDimension:report.confirmedDefectsByDimension,
    academicDecision:report.batchDecision
  },null,2)+'\n');
}
if (require.main === module) {
  try { main(); } catch (e) {
    console.error('[B01 Unified] '+e.message); process.exitCode=1;
  }
}
module.exports = { summarize, DIMENSIONS, CORRECT, REWORK, BLOCKED };
