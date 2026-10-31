'use strict';
// Rebuilds the Grade 8 V12 candidate ledger from immutable V11 evidence.
// This is a staging-only bookkeeping operation, not academic approval.
const fs = require('node:fs');
const path = require('node:path');
const assert = require('node:assert/strict');
const root = path.resolve(__dirname, '..');
const file = name => path.join(root, 'docs/curriculum', name);
const base = JSON.parse(fs.readFileSync(file('us-ccss-g8-coder4-v11-implementation-ledger-20261029.json'), 'utf8'));
const evidence = JSON.parse(fs.readFileSync(file('us-ccss-g8-coder4-v12-u01-l02-source-geometry-evidence-20261029.json'), 'utf8'));
const figureFile = path.join(root,'src/Edulytics.Web/wwwroot', evidence.assetPath.replace(/^\//,''));
const crypto = require('node:crypto');
assert.equal(crypto.createHash('sha256').update(fs.readFileSync(figureFile)).digest('hex'), evidence.sha256);
assert.equal(base.rows.length, 131);
assert.equal(base.counts.assets,30);
const row = base.rows.find(x=>x.lessonCode === evidence.lessonCode);
assert.ok(row);
assert.equal(row.verifiedCourseOwnedSvgFile,null);
row.verifiedCourseOwnedSvgFile=evidence.assetPath;
row.verifiedCourseOwnedSvgSha256=evidence.sha256;
row.svgHasIndependentMathUnitChecks=true; // V12 exact geometry tests must pass before committing.
row.sourceActivitySpotReviewEvidence='docs/curriculum/us-ccss-g8-coder4-v12-u01-l02-source-geometry-evidence-20261029.json';
row.nextChecks=row.nextChecks.filter(x=>x!=='SOURCE_FIGURE_ORIGINAL_REPRESENTATION_STILL_REQUIRED_OR_CLASSIFY_REUSE');
row.nextChecks.unshift('SVG_ASSET_NOT_YET_ATTACHED_TO_AUTHENTICATED_RICH_V2_RENDERER','SOURCE_6_FRAME_STUDENT_INTERACTION_AND_CARD_SORT_NEED_EXACT_FIDELITY_REVIEW');
base.purpose='V12: one Grade 8 source-specific academic correction and original geometry candidate with independent math tests; preserved V11 work, NO A-D promotion.';
base.limitations='U01:L02 course-owned SVG remains detached from authenticated student Rich V2. Source activity spot-review is not full academic parity/approval, teacher score persistence, real two-school database or runtime Practice evidence.';
base.counts.assets=31;
base.counts.sourceFigurePriorityWithOriginalAsset=29;
base.counts.sourceFigurePriorityWithoutOriginalAsset=84;
base.counts.sourceActivitySpotReviewedLessons=1;
base.counts.fullAcademicSourceParityAcceptedLessons=0;
base.counts.authenticatedSvgRenderedLessons=0;
base.counts.teacherScoredLessons=0;
assert.equal(base.counts.sourceFigurePriorityWithOriginalAsset+base.counts.sourceFigurePriorityWithoutOriginalAsset,113);
for(const r of base.rows) {
 assert.equal(r.A,'NOT_ACCEPTED');
 assert.equal(r.B,'NOT_ACCEPTED');
 assert.equal(r.C,'NOT_VERIFIED');
 assert.equal(r.D,'NOT_VERIFIED');
 assert.equal(r.svgInAuthenticatedStudentRenderer,false);
 assert.equal(r.actualPostgresqlAttemptPersistedAndReadBack,false);
}
const output=file('us-ccss-g8-coder4-v12-implementation-ledger-20261029.json');
fs.writeFileSync(output,JSON.stringify(base,null,2)+'\n','utf8');
console.log('V12 ledger written: 131 lesson rows, 31 candidate SVG assets, A/B/C/D=0');
