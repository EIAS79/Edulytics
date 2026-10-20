'use strict';
// Independent Grade 1 A–C status inventory. Never equate a checkpoint,
// code candidate or D's acceptance ledger with academic A–C completion.
const fs=require('node:fs');
const path=require('node:path');
const DIR=path.resolve(__dirname,'../../artifacts/curriculum-delivery/US-CCSS-MATH/G1');
const read=name=>JSON.parse(fs.readFileSync(path.join(DIR,name),'utf8'));
const check=(condition,reason)=>{if(!condition)throw new Error(reason);};
const STATUS={
  accepted:'PRESERVED_INTERNAL_ACCEPTANCE',
  gap:'A_C_EXPLICIT_GAP',
  unproven:'A_C_WORK_RECORDED_CLOSURE_UNPROVEN',
  ready:'READY_FOR_STAGE_D'
};
function build(){
  const progress=read('full-grade-progress.json');
  const risk=read('G1-SOURCE-ORIGINAL-TASK-PARITY-REMAINING-RISKS.json').rows;
  const matrix=read('full-grade-academic-evidence-matrix.json').records;
  check(progress.newLessons.length===96&&risk.length===96&&matrix.length===96,
    'Expected 96 original source, candidate and academic evidence rows');
  const riskByOrdinal=new Map(risk.map(x=>[x.ordinal,x]));
  const matrixByOrdinal=new Map(matrix.map(x=>[x.ordinal,x]));
  const lessons=progress.newLessons.map((lesson,index)=>{
    const ordinal=index+51;
    const source=riskByOrdinal.get(ordinal);
    const evidence=matrixByOrdinal.get(ordinal);
    check(lesson.ordinal===ordinal&&source?.lessonCode===lesson.lessonCode&&
      evidence?.lessonCode===lesson.lessonCode,'Canonical source identity drift at '+ordinal);
    check(lesson.status===evidence.contentImplementationStatus,
      'Progress/evidence status conflict at '+ordinal);
    if(lesson.status==='INTERNALLY_ACCEPTED')
      return {ordinal,lessonCode:lesson.lessonCode,status:STATUS.accepted,aToCReady:true,
        reason:'Already individually accepted; preserve previous independent evidence'};
    const file='lesson-'+ordinal+'-v2-stage-a-d-checkpoint.json';
    const exists=fs.existsSync(path.join(DIR,file));
    check(exists,'Missing source/teaching/Practice checkpoint for '+ordinal);
    const cp=read(file);
    check(cp.ordinal===ordinal&&cp.lessonCode===lesson.lessonCode&&
      cp.stageA&&cp.stageB&&cp.stageC,'Stage checkpoint incomplete/mismatched at '+ordinal);
    const a=cp.stageA,c=cp.stageC;
    const explicitGap=a.allOriginalFigureAndStudentTasksFullyAudited===false||
      a.sourceGoalAgainstEffectiveAuthenticatedContentApproved===false||
      c.originalSourceTaskToPracticeParity==='PARTIAL_OR_UNPROVEN';
    // New affirmative contract: no inference from explanatory prose, hashes,
    // a prepared checklist, or historical CI on a different SHA.
    const proof=cp.aToCClosure;
    const complete=proof?.originalStudentAndTeacherTasksCompared===true&&
      proof?.learnerTeachingSourceParityReviewed===true&&
      proof?.practiceGeneratorAndVerifierIndependentlyReviewed===true&&
      Array.isArray(proof?.openACDefects)&&proof.openACDefects.length===0&&
      typeof proof?.evidenceReference==='string'&&proof.evidenceReference.length>15;
    check(!complete||!explicitGap,'Contradictory A–C closure at '+ordinal);
    const status=complete?STATUS.ready:explicitGap?STATUS.gap:STATUS.unproven;
    return {ordinal,lessonCode:lesson.lessonCode,status,aToCReady:complete,
      sourceTasksFullyAudited:a.allOriginalFigureAndStudentTasksFullyAudited===true,
      originalSourceToPracticeProven:c.originalSourceTaskToPracticeParity==='VERIFIED',
      evidenceFile:file,reason:complete?'Affirmative source/teaching/Practice evidence on file':
        explicitGap?'Stage A source-task audit or Stage C parity explicitly pending':
        'A/B/C work exists, but no independently evidenced phase-closure decision'};
  });
  const count=s=>lessons.filter(x=>x.status===s).length;
  const summary={
    historicallyPublishedOutsideScope:50,scopeNewLessons:96,
    previouslyInternallyAccepted:count(STATUS.accepted),
    explicitlyReadyForFinalD:count(STATUS.ready),
    knownACGaps:count(STATUS.gap),
    workedButNotProvenACClosed:count(STATUS.unproven),
    newLessonsNotYetAccepted:96-count(STATUS.accepted),
    releaseDecision:'NO_GO',
    note:'This counts proved closure, not inferred completion. Stage D CI, actual learner grading and publication are independent gates.'
  };
  check(summary.previouslyInternallyAccepted+summary.explicitlyReadyForFinalD+
    summary.knownACGaps+summary.workedButNotProvenACClosed===96,
    'Status counts must reconcile to the 96 new lessons');
  return {schemaVersion:1,scope:'G1 ordinals 51–146',summary,lessons};
}
if(require.main===module)
  process.stdout.write(JSON.stringify(process.argv.includes('--summary')?build().summary:build(),null,2)+'\n');
module.exports={build,STATUS};
