'use strict';
const fs = require('node:fs');
const path = require('node:path');
const assert = require('node:assert/strict');
const root = path.resolve(__dirname, '../../');
const base = path.join(root,'artifacts/curriculum-delivery/US-CCSS-MATH/G12');
const read = p => JSON.parse(fs.readFileSync(path.join(base,p),'utf8'));
const progress = read('full-grade-progress.json');
const ledger = read('grade12-stage-d-execution-ledger.json');
function inspect() {
 const errors=[];
 const entries=progress.LessonEntries;
 if (!Array.isArray(entries)||entries.length!==73)errors.push('PROGRESS_LESSON_COUNT_NOT_73');
 if (!Array.isArray(ledger.lessonResults)||ledger.lessonResults.length!==73)errors.push('D_LEDGER_LESSON_COUNT_NOT_73');
 if (progress.SourceSnapshotSha256!==ledger.sourceSnapshotId)errors.push('SOURCE_SNAPSHOT_MISMATCH');
 if (entries.length===73 && ledger.lessonResults.length===73) {
  const units=[0,7,4,9,8,4,3,6,8,8,5,7,4];
  let n=0;const seen=new Set();
  for(let u=1;u<units.length;u++){
   for(let lesson=1;lesson<=units[u];lesson++){
    const p=entries[n],d=ledger.lessonResults[n];n++;
    if (p.Ordinal!==n||d.lessonOrdinal!==n||p.Unit!==u||p.LessonNumber!==lesson||d.lessonCode!==p.LessonCode) errors.push('ORDINAL_MAPPING_'+n);
    if (seen.has(p.LessonCode))errors.push('DUPLICATE_LESSON_CODE_'+n);
    seen.add(p.LessonCode);
    if (p.SourceSection!==u+'.'+lesson ||d.section!==p.SourceSection)errors.push('SECTION_MAPPING_'+n);
    if (d.dStatus==='D_VERIFIED' && (!d.actionsRunIds.length||d.unitTests!=='PASS'||d.integrationTests!=='PASS'||d.authenticatedE2E!=='PASS')) errors.push('D_VERIFIED_WITHOUT_REQUIRED_RUNTIME_EVIDENCE_'+n);
    if (d.dStatus==='APPROVED' && d.academicStatus!=='APPROVED')errors.push('MISSING_HUMAN_APPROVAL_'+n);
    for(const proof of d.evidencePaths){
     if(!fs.existsSync(path.join(root,proof)))errors.push('MISSING_LESSON_EVIDENCE_'+n+'_'+proof);
    }
   }
  }
 }
 if(ledger.dVerifiedCount!==ledger.lessonResults.filter(x=>x.dStatus==='D_VERIFIED').length)errors.push('D_VERIFIED_COUNT_NOT_RECONCILED');
 if(ledger.academicApprovedCount!==ledger.lessonResults.filter(x=>x.academicStatus==='APPROVED').length)errors.push('APPROVED_COUNT_NOT_RECONCILED');
 if(ledger.sourceParity.percent===100 && (ledger.sourceParity.numerator==null||ledger.sourceParity.denominator==null||ledger.sourceParity.numerator!==ledger.sourceParity.denominator))errors.push('SOURCE_PARITY_100_WITH_UNKNOWN_EVIDENCE');
 if(ledger.digitalCoverage.percent===100 && (ledger.digitalCoverage.numerator==null||ledger.digitalCoverage.denominator==null||ledger.digitalCoverage.numerator!==ledger.digitalCoverage.denominator))errors.push('DIGITAL_100_WITH_UNKNOWN_EVIDENCE');
 return errors;
}
const errs=inspect();
console.log(JSON.stringify({scope:'Grade 12 Stage D structural bootstrap',lessonCount:ledger.lessonResults.length,sourceSnapshot:ledger.sourceSnapshotId,firstIncomplete:ledger.firstIncompleteLessonOrdinal,blockedLessons:ledger.blockedLessons,verified:ledger.dVerifiedCount,sourceParity:ledger.sourceParity,digitalCoverage:ledger.digitalCoverage,errors:errs},null,2));
if(errs.length) process.exitCode=1;
