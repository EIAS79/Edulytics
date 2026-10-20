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
      return {ordinal,lessonCode:lesson.lessonCode,unit:lesson.unit,
        lessonNumber:lesson.lessonNumber,title:lesson.title,
        status:STATUS.accepted,aToCReady:true,
        stageA:'PRESERVED_ACCEPTANCE',stageB:'PRESERVED_ACCEPTANCE',
        stageC:'PRESERVED_ACCEPTANCE',
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
    // Each stage is evidenced ONLY by the existing lesson checkpoint's
    // affirmative aToCClosure fields. Stage records or commits alone are not proof.
    const stageA=a.allOriginalFigureAndStudentTasksFullyAudited===false||
        a.sourceGoalAgainstEffectiveAuthenticatedContentApproved===false
      ?'EXPLICIT_GAP':proof?.originalStudentAndTeacherTasksCompared===true
      ?'REVIEW_EVIDENCED':'NOT_PROVEN';
    const stageB=proof?.learnerTeachingSourceParityReviewed===true
      ?'REVIEW_EVIDENCED':'NOT_PROVEN';
    const stageC=c.originalSourceTaskToPracticeParity==='PARTIAL_OR_UNPROVEN'
      ?'EXPLICIT_GAP':proof?.practiceGeneratorAndVerifierIndependentlyReviewed===true
      ?'REVIEW_EVIDENCED':'NOT_PROVEN';
    return {ordinal,lessonCode:lesson.lessonCode,unit:lesson.unit,
      lessonNumber:lesson.lessonNumber,title:lesson.title,
      status,aToCReady:complete,stageA,stageB,stageC,
      openACDefects:Array.isArray(proof?.openACDefects)?proof.openACDefects:null,
      evidenceReference:typeof proof?.evidenceReference==='string'?proof.evidenceReference:null,
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
// A report view, not a second ledger: it reads the existing canonical Grade 1
// progress, evidence matrix and per-lesson A/B/C closure checkpoints unchanged.
function buildSelectionReport(lessons,snapshot,scope){
  const count=status=>lessons.filter(x=>x.status===status).length;
  const pendingACOrdinals=lessons.filter(x=>x.status!==STATUS.accepted&&!x.aToCReady)
    .map(x=>x.ordinal);
  const skipACReworkOrdinals=lessons.filter(x=>x.status===STATUS.accepted||x.aToCReady)
    .map(x=>x.ordinal);
  return {
    scope,
    summary:{
      total:lessons.length,
      previouslyInternallyAccepted:count(STATUS.accepted),
      readyForStageD:count(STATUS.ready),
      knownACGaps:count(STATUS.gap),
      reviewClosureUnproven:count(STATUS.unproven),
      pendingACOrdinals,
      skipACReworkOrdinals,
      nextUnclosedOrdinal:pendingACOrdinals[0]??null,
      ciTriggeredByReport:false,
      releaseDecision:snapshot.summary.releaseDecision
    },
    lessons:lessons.map(x=>({
      ordinal:x.ordinal,lessonCode:x.lessonCode,unit:x.unit,
      lessonNumber:x.lessonNumber,title:x.title,
      stageA:x.stageA,stageB:x.stageB,stageC:x.stageC,
      aToCStatus:x.status,readyForStageD:x.aToCReady,
      skipACRework:x.status===STATUS.accepted||x.aToCReady,
      openACDefects:x.openACDefects??null,
      evidenceReference:x.evidenceReference??null,
      evidenceFile:x.evidenceFile??null,
      nextAction:x.status===STATUS.accepted?'PRESERVE_PREVIOUS_ACCEPTANCE':
        x.aToCReady?'HOLD_AT_STAGE_D':
        'REVIEW_AND_CLOSE_A_TO_C_WITH_SOURCE_AND_PRACTICE_EVIDENCE'
    }))
  };
}
// A–C authoring scope is all current non-accepted candidates in canonical order,
// not an arbitrary ten-lesson slice. Lessons with authenticated final D acceptance
// are preserved, not reworked. READY_FOR_STAGE_D entries stay in the report
// until D is actually run, but are explicitly skipped by the A–C repair queue.
function buildRemainingReport(snapshot=build()){
  const pending=snapshot.lessons.filter(x=>x.status!==STATUS.accepted);
  return buildSelectionReport(pending,snapshot,'G1 all remaining candidate lessons');
}
function buildRangeReport(from,to,snapshot=build()){
  check(Number.isInteger(from)&&Number.isInteger(to)&&from>=51&&to<=146&&from<=to,
    'Expected a valid Grade 1 new-lesson range within 51-146');
  const lessons=snapshot.lessons.filter(x=>x.ordinal>=from&&x.ordinal<=to);
  check(lessons.length===to-from+1,'Incomplete canonical Grade 1 range');
  return buildSelectionReport(lessons,snapshot,`G1 ${from}-${to}`);
}
function main(argv){
  const rangeArguments=argv.filter(arg=>arg.startsWith('--range'));
  check(rangeArguments.length<=1,'Provide at most one --range=FROM-TO');
  check(!argv.includes('--remaining')||rangeArguments.length===0,
    'Choose either --remaining (entire unfinished cohort) or --range');
  const snapshot=build();
  if(argv.includes('--remaining')){
    const report=buildRemainingReport(snapshot);
    return argv.includes('--summary')?report.summary:report;
  }
  if(rangeArguments.length){
    const match=/^--range=(\d+)-(\d+)$/.exec(rangeArguments[0]);
    check(match,'Use --range=FROM-TO, for example --range=57-66');
    const report=buildRangeReport(Number(match[1]),Number(match[2]),snapshot);
    return argv.includes('--summary')?report.summary:report;
  }
  return argv.includes('--summary')?snapshot.summary:snapshot;
}
if(require.main===module){
  try{process.stdout.write(JSON.stringify(main(process.argv.slice(2)),null,2)+'\n');}
  catch(error){console.error(error.message);process.exitCode=1;}
}
module.exports={build,buildSelectionReport,buildRemainingReport,buildRangeReport,main,STATUS};
