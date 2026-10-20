'use strict';

/**
 * One continuous full-grade execution worklist, not a 5-lesson chunk scheduler.
 * This is a SCOPE/RESUMPTION planner, not automatic lesson generation or
 * academic acceptance. It cannot and must not publish data to production.
 *
 * node tools/curriculum/us-ccss-full-grade-worklist.cjs
 * node tools/curriculum/us-ccss-full-grade-worklist.cjs --remaining (all lacking academic acceptance)
 * node tools/curriculum/us-ccss-full-grade-worklist.cjs --new (only not-yet-started implementation)
 */
const fs=require('node:fs');
const path=require('node:path');
const ROOT=path.resolve(__dirname,'../..');
const MANIFEST='artifacts/curriculum-delivery/US-CCSS-MATH/G1/grade-manifest.json';
const LEDGER='artifacts/curriculum-delivery/US-CCSS-MATH/G1/full-grade-progress.json';
const PLAN='EDULYTICS-GRADE-FIRST-FULL-GRADE-V2';
const APPROVED='INTERNALLY_ACCEPTED';
const knownStates=new Set([
  'NOT_STARTED','IN_PROGRESS','READY_REUSE','CONTENT_REPAIR','PRACTICE_REPAIR',
  'NEW_SHARED_CAPABILITY','BLOCKED_SOURCE','BLOCKED','TESTED',APPROVED
]);

const read=p=>JSON.parse(fs.readFileSync(path.join(ROOT,p),'utf8'));

function buildWorklist(manifest,ledger,{remainingOnly=false,newOnly=false,checkpointOrdinals=null}={}) {
  // Checkpoint presence is source-review RESUMPTION data, never academic approval.
  // Null means not inspected, not "missing"; CLI passes real filenames on disk.
  if(checkpointOrdinals!==null && (!Array.isArray(checkpointOrdinals) ||
      checkpointOrdinals.some(n=>!Number.isInteger(n)||n<51||n>146) ||
      new Set(checkpointOrdinals).size!==checkpointOrdinals.length))
    throw new Error('Checkpoint snapshot must contain unique Grade 1 new ordinals 51-146');
  const checkpointSet=checkpointOrdinals===null?null:new Set(checkpointOrdinals);
  if(remainingOnly&&newOnly)throw new Error('Use either academic --remaining or implementation --new, never both');
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
    const hasV2Checkpoint=checkpointSet===null?null:checkpointSet.has(ordinal);
    return {
      ordinal,lessonCode:canonical.lessonCode,
      unitNumber:canonical.unitNumber,lessonNumber:canonical.lessonNumber,
      title:canonical.title,sourceTeacherUrl:canonical.sourceTeacherUrl,
      sourceEdition:canonical.sourceEdition,
      status:row.status,needsNewAcademicEvidence:!accepted,
      // 62 existing checkpoints represent preserved authored A-D work, NOT 62
      // complete lessons; non-checkpoint rows still have prior source indexes,
      // implementations and Practice candidates. Never reauthor them by default.
      hasV2Checkpoint,
      resumptionClass:accepted?'PRESERVE_VERIFIED_ACCEPTANCE':
        hasV2Checkpoint===null?'CHECKPOINT_INVENTORY_NOT_SUPPLIED':
        hasV2Checkpoint?'CONTINUE_EXISTING_A_D_CHECKPOINT':
        'REUSE_INDEXED_SOURCE_AND_EXISTING_CANDIDATE_THEN_COMPLETE_A_D',
      nextAction:accepted?'PRESERVE_ACCEPTANCE_AND_SOURCE_EVIDENCE':
        row.status==='BLOCKED_SOURCE'?'RESOLVE_SOURCE_RIGHTS_OR_PROVENANCE':
        row.status==='BLOCKED'?'RESOLVE_VERIFIED_BLOCKER':
        row.status==='NOT_STARTED'?'START_SOURCE_STUDENT_TASK_AND_PRACTICE_REVIEW':
        'CONTINUE_A_TO_D_AND_RECORD_REAL_VERIFICATION'
    };
  });
  const unfinished=workItems.filter(x=>x.needsNewAcademicEvidence);
  const sourceReviewResumption=checkpointSet===null?null:{
    checkpointFiles:checkpointSet.size,
    acceptedWithCheckpoint:workItems.filter(x=>!x.needsNewAcademicEvidence&&x.hasV2Checkpoint).length,
    pendingWithCheckpoint:unfinished.filter(x=>x.hasV2Checkpoint).length,
    pendingWithoutCheckpoint:unfinished.filter(x=>!x.hasV2Checkpoint).length,
    pendingWithoutCheckpointOrdinals:unfinished.filter(x=>!x.hasV2Checkpoint)
      .map(x=>x.ordinal),
    note:'Checkpoint presence is progress, NOT an academic acceptance or proof that original V1 Stage D passed. Missing checkpoint does not mean no source, candidate teaching, tests or Practice exist.'
  };
  const firstUnfinished=unfinished[0]?.ordinal??147;
  if(ledger.nextOrdinal!==firstUnfinished)
    throw new Error('Resume cursor disagrees with actual pending work; expected '+firstUnfinished);
  // Two different cursors are essential: content creation resumes at the
  // first NOT_STARTED item (currently 81), while A-D academic verification
  // still includes the 30 partially implemented 51–80 (currently 51).
  // Neither cursor imposes any five-lesson delivery boundary.
  const notStarted=workItems.filter(x=>x.status==='NOT_STARTED');
  const nextUnstartedOrdinal=notStarted[0]?.ordinal??147;
  const inProgressCount=workItems.filter(x=>x.status!=='NOT_STARTED'&&x.needsNewAcademicEvidence).length;
  const items=newOnly?notStarted:remainingOnly?unfinished:workItems;
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
    expectedItems:items.length,
    implementation:{nextUnstartedOrdinal,notStartedCount:notStarted.length,inProgressCount,
      instruction:'PRESERVE EXISTING IN_PROGRESS WORK; start new-content work at nextUnstartedOrdinal; individually verify earlier started lessons'},
    academicVerification:{firstPendingOrdinal:firstUnfinished,pendingCount:unfinished.length},
    queueSelection:newOnly?'NOT_STARTED_IMPLEMENTATION_ONLY':remainingOnly?'PENDING_ACADEMIC_EVIDENCE':'ALL_96_GRADE_ITEMS',
    acceptedNew:96-unfinished.length,
    sourceReviewResumption,
    firstUnfinishedOrdinal:firstUnfinished,
    nextGradeAllowed:false,
    status:unfinished.length?'NO_GO':'ELIGIBLE_FOR_SEPARATE_FULL_GRADE_EVIDENCE_GATE',
    warning:'WORKLIST ONLY: items are tasks, not implemented content, test passes, academic approvals or permission to deploy.',
    items
  };
}

const checkpointFiles=()=>fs.readdirSync(path.join(ROOT,
  'artifacts/curriculum-delivery/US-CCSS-MATH/G1'))
  .filter(name=>/^lesson-\d+-v2-stage-a-d-checkpoint\.json$/.test(name))
  .map(name=>Number(name.match(/^lesson-(\d+)-/)[1]));

if(require.main===module){
  try {
    const worklist=buildWorklist(read(MANIFEST),read(LEDGER),
      {remainingOnly:process.argv.includes('--remaining'),
       newOnly:process.argv.includes('--new'),
       checkpointOrdinals:checkpointFiles()});
    console.log(JSON.stringify(worklist,null,2));
  }catch(e){console.error(e.stack||String(e));process.exitCode=1;}
}
module.exports={buildWorklist};
