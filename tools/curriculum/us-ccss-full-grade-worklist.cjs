'use strict';

/**
 * One continuous full-grade execution worklist, not a 5-lesson chunk scheduler.
 * This is a SCOPE/RESUMPTION planner, not automatic lesson generation or
 * academic acceptance. It cannot and must not publish data to production.
 *
 * node tools/curriculum/us-ccss-full-grade-worklist.cjs
 * node tools/curriculum/us-ccss-full-grade-worklist.cjs --remaining
 */
const fs=require('node:fs');
const path=require('node:path');
const ROOT=path.resolve(__dirname,'../..');
const MANIFEST='artifacts/curriculum-delivery/US-CCSS-MATH/G1/grade-manifest.json';
const LEDGER='artifacts/curriculum-delivery/US-CCSS-MATH/G1/full-grade-progress.json';
const PLAN='EDULYTICS-GRADE-FIRST-FULL-GRADE-V1';
const APPROVED='INTERNALLY_ACCEPTED';
const knownStates=new Set([
  'NOT_STARTED','IN_PROGRESS','READY_REUSE','CONTENT_REPAIR','PRACTICE_REPAIR',
  'NEW_SHARED_CAPABILITY','BLOCKED_SOURCE','BLOCKED','TESTED',APPROVED
]);

const read=p=>JSON.parse(fs.readFileSync(path.join(ROOT,p),'utf8'));

function buildWorklist(manifest,ledger,{remainingOnly=false}={}) {
  const all=manifest?.lessons;
  if(!Array.isArray(all)||all.length!==146||
     all.some((x,i)=>x.gradeOrdinal!==i+1||typeof x.lessonCode!=='string')||
     new Set(all.map(x=>x.lessonCode)).size!==146)
    throw new Error('Full-grade canonical inventory must be 146 uniquely ordered lesson codes');
  if(ledger?.schemaVersion!==1||ledger.planId!==PLAN||
     ledger.grade!=='G1'||ledger.programme!=='US-CCSS-MATH'||
     ledger.gradeCount!==146||ledger.historical?.count!==50||
     ledger.historical?.first!==1||ledger.historical?.last!==50||
     ledger.historical?.status!=='HISTORICALLY_RELEASED_NO_RETEST'||
     ledger.newScope?.first!==51||ledger.newScope?.last!==146||
     ledger.newScope?.count!==96||
     !Array.isArray(ledger.newLessons)||ledger.newLessons.length!==96)
    throw new Error('Full-grade progress ledger invalid; no fallback to V1 five-lesson chunks');
  const workItems=all.slice(50).map((canonical,index)=>{
    const row=ledger.newLessons[index],ordinal=index+51;
    if(row?.ordinal!==ordinal||row.lessonCode!==canonical.lessonCode||
       row.unit!==canonical.unitNumber||row.lessonNumber!==canonical.lessonNumber||
       row.sourceTeacherUrl!==canonical.sourceTeacherUrl||
       JSON.stringify(row.outcomeCodes)!==JSON.stringify(canonical.officialOutcomeCodes)||
       !knownStates.has(row.status))
      throw new Error('Lesson missing, reordered, identity changed, or unknown state at ordinal '+ordinal);
    const accepted=row.status===APPROVED;
    return {
      ordinal,lessonCode:canonical.lessonCode,
      unitNumber:canonical.unitNumber,lessonNumber:canonical.lessonNumber,
      title:canonical.title,sourceTeacherUrl:canonical.sourceTeacherUrl,
      sourceEdition:canonical.sourceEdition,
      status:row.status,needsNewAcademicEvidence:!accepted,
      nextAction:accepted?'PRESERVE_ACCEPTANCE_AND_SOURCE_EVIDENCE':
        row.status==='BLOCKED_SOURCE'?'RESOLVE_SOURCE_RIGHTS_OR_PROVENANCE':
        row.status==='BLOCKED'?'RESOLVE_VERIFIED_BLOCKER':
        row.status==='NOT_STARTED'?'START_SOURCE_STUDENT_TASK_AND_PRACTICE_REVIEW':
        'CONTINUE_A_TO_D_AND_RECORD_REAL_VERIFICATION'
    };
  });
  const unfinished=workItems.filter(x=>x.needsNewAcademicEvidence);
  const firstUnfinished=unfinished[0]?.ordinal??147;
  if(ledger.nextOrdinal!==firstUnfinished)
    throw new Error('Resume cursor disagrees with actual pending work; expected '+firstUnfinished);
  const items=remainingOnly?unfinished:workItems;
  return {
    planId:PLAN,programme:'US-CCSS-MATH',grade:'G1',
    executionUnit:'ONE_WHOLE_GRADE',
    gradeTotal:146,
    historic:{first:1,last:50,count:50,policy:'REFERENCE_ONLY_NO_RETEST',
      approvedUnder:ledger.historical.mainSha},
    currentGradeWork:{first:51,last:146,count:96},
    fixedChunkSize:null,
    stoppedEveryFiveLessons:false,
    stages:'ORIGINAL_V1_A_THROUGH_F_UNCHANGED',
    academicGate:'ONE_AGGREGATED_GRADE_GATE_FAIL_CLOSED',
    releaseGate:'EXACT_SHA_AND_REQUIRED_CI_RENDER_AND_OWNER_UAT',
    expectedItems:remainingOnly?unfinished.length:96,
    acceptedNew:96-unfinished.length,
    firstUnfinishedOrdinal:firstUnfinished,
    nextGradeAllowed:false,
    status:unfinished.length?'NO_GO':'ELIGIBLE_FOR_SEPARATE_FULL_GRADE_EVIDENCE_GATE',
    warning:'WORKLIST ONLY: items are tasks, not implemented content, test passes, academic approvals or permission to deploy.',
    items
  };
}

if(require.main===module){
  try {
    const worklist=buildWorklist(read(MANIFEST),read(LEDGER),
      {remainingOnly:process.argv.includes('--remaining')});
    console.log(JSON.stringify(worklist,null,2));
  }catch(e){console.error(e.stack||String(e));process.exitCode=1;}
}
module.exports={buildWorklist};
