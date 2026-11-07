'use strict';
const fs = require('node:fs');
const path = require('node:path');
const {execFileSync} = require('node:child_process');
const root = path.resolve(__dirname, '../../');
const base = path.join(root,'artifacts/curriculum-delivery/US-CCSS-MATH/G12');
const progress = JSON.parse(fs.readFileSync(path.join(base,'full-grade-progress.json'),'utf8'));
const ledger = JSON.parse(fs.readFileSync(path.join(base,'grade12-stage-d-execution-ledger.json'),'utf8'));
const errors = [];
const rows = ledger.lessonResults;
const ownerApprovalPath = path.join(base,'grade12-owner-academic-release-authorization-20261107.md');
const publishedPackPath = path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g12-precalculus-u01-l01-abc-draft.lesson-content-pack.json');
const credits = path.join(root,'src/Edulytics.Web/wwwroot/content-sources-licenses.html');

if (!Array.isArray(progress.LessonEntries) || progress.LessonEntries.length!==73 ||
    progress.LessonEntries.some(x=>x.ReadyForStageD!==true || x.ACStatus!=='READY_FOR_STAGE_D')) {
  errors.push('A_B_C_73_READY_HANDOFF_NOT_PRESENT');
}
if (!fs.existsSync(ownerApprovalPath))errors.push('OWNER_ACADEMIC_SIGNOFF_EVIDENCE_MISSING');
if (ledger.academicApprovedCount!==73 || rows.filter(x=>x.academicStatus==='APPROVED').length!==73) {
  errors.push('GRADE12_OWNER_ACADEMIC_APPROVAL_INCOMPLETE');
}
if (!fs.existsSync(publishedPackPath))errors.push('PUBLISHED_CANONICAL_G12_PACK_MISSING');
else {
  const pack=JSON.parse(fs.readFileSync(publishedPackPath,'utf8'));
  if(pack.Status!=='Published' || pack.Lessons.length!==73 || !/owner/i.test(pack.ReviewedBy||'')) {
    errors.push('CANDIDATE_CANONICAL_73_NOT_OWNER_AUTHORIZED');
  }
}
const notice = fs.existsSync(credits) ? fs.readFileSync(credits,'utf8') : execFileSync('git',['show','HEAD:src/Edulytics.Web/wwwroot/content-sources-licenses.html'],{cwd:root,encoding:'utf8'});
if (!/OpenStax[\s\S]*CC BY 4\.0/i.test(notice)) {
  errors.push('CC_BY_CREDITS_NOT_PUBLISHED');
}
// Stage D verifies technical behavior; it does not rerun A–B–C textbook
// source-element comparisons or infer copyright rights from owner consent.
// Those records retain their own evidence, unknowns, and review responsibility.
if (!Array.isArray(rows) || rows.length!==73)errors.push('D_LESSON_DENOMINATOR_NOT_73');
if (ledger.canonicalPublishedG12LessonCount!==73)errors.push('G12_73_PUBLISHED_CANONICAL_RUNTIME_NOT_VERIFIED');
if (ledger.authenticatedE2EVerifiedLessonCount!==73 ||
    ledger.lastAuthenticatedE2EStatus!=='PASS_ALL_73') {
  errors.push('G12_73_AUTHENTICATED_LEARNER_HTTPS_NOT_VERIFIED');
}
if (ledger.teacherGradebookVerifiedLessonCount!==73)errors.push('G12_TEACHER_GRADING_AND_PERSISTENCE_NOT_VERIFIED');
if (ledger.practiceVerifiedLessonCount!==73)errors.push('G12_PRACTICE_PER_LESSON_NOT_VERIFIED');
if (rows?.some(x=>x.dStatus==='D_BLOCKED'))errors.push('UNRESOLVED_STAGE_D_BLOCKED_LESSONS');
if (ledger.dVerifiedCount!==73 || rows?.filter(x=>x.dStatus==='D_VERIFIED').length!==73) {
  errors.push('GRADE12_NOT_ALL_D_VERIFIED');
}
for(const row of rows||[]){
  if (row.dStatus!=='D_VERIFIED')continue;
  if(row.unitTests!=='PASS' || row.integrationTests!=='PASS' ||
     row.authenticatedE2E!=='PASS' || row.visualAccessibility!=='PASS' ||
     !Array.isArray(row.actionsRunIds) || row.actionsRunIds.length===0) {
    errors.push('D_VERIFIED_WITHOUT_RUNTIME_EVIDENCE_'+row.lessonOrdinal);
  }
}
console.log(JSON.stringify({
  gate:'Grade12 Stage D technical, student, practice, teacher and owner approval',
  candidate:ledger.baselineCandidateSha,ownerAcademicApproved:ledger.academicApprovedCount,
  canonicalRuntime:ledger.canonicalPublishedG12LessonCount,
  studentRuntime:ledger.authenticatedE2EVerifiedLessonCount,
  verified:ledger.dVerifiedCount,errors
},null,2));
if (errors.length)process.exitCode=1;
