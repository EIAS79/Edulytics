#!/usr/bin/env node
'use strict';

/**
 * Reconcile genuine B001 P4 artifacts for the EXISTING P5 acceptance gate.
 * Does not run tests. Does not certify educational parity, licensing,
 * independent math or production UAT. Every proof remains tied to its actual
 * GitHub Actions run and the exact tested commit.
 */
const fs=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'../..');
const catalogueDefault=path.join(root,'docs/curriculum/batches/US-CCSS-B001-CATALOGUE.json');
const fullSha=/^[a-f\d]{40}$/i;
const allowedHttpsStatuses=new Set([
  'SELF_CONSISTENT_GRADE_PERSISTED',
  'SYNTHETIC_SURVEY_TALLY_PERSISTED_NEEDS_REAL_TEACHER_VERIFICATION'
]);
const url=runId=>'https://github.com/EIAS79/Edulytics/actions/runs/'+runId;
const pass=(sha,runId,testId)=>({
  result:'pass',candidateSha:sha,runUrl:url(runId),testId
});
function strictRows(name,rows,expected){
  if(!Array.isArray(rows)||rows.length!==500)throw Error(name+': need exactly 500 real rows');
  const seen=new Map();
  for(const x of rows){
    if(!x||typeof x.lessonCode!=='string'||seen.has(x.lessonCode)||
       !expected.has(x.lessonCode))throw Error(name+': duplicate/foreign/malformed code');
    seen.set(x.lessonCode,x);
  }
  if(seen.size!==expected.size)throw Error(name+': source lesson missing');
  return seen;
}
function reconcile(catalogue,engine,content,https,meta){
  const sha=meta?.sha;
  if(!fullSha.test(sha||''))throw Error('Exact 40-character candidate SHA required');
  for(const k of ['engineRun','contentRun','httpsRun']){
    if(!Number.isSafeInteger(meta[k])||meta[k]<1)throw Error('Valid actual Actions '+k+' required');
  }
  if(catalogue.batchId!=='US-CCSS-B001'||catalogue.total!==500||
     catalogue.lessons?.length!==500)throw Error('Invalid authoritative B001 catalogue');
  const expected=new Set(catalogue.lessons.map(x=>x.lessonCode));
  if(expected.size!==500)throw Error('Catalogue lesson codes repeated');
  if(engine.batchId!=='US-CCSS-B001'||engine.candidateSha!==sha||
     engine.lessonCount!==500||
     engine.testType!=='B001_ENGINE_SAMPLING_NOT_FINAL_ACADEMIC_CERTIFICATION')
    throw Error('Engine artifact missing actual exact-SHA provenance');
  if(content.batchId!=='US-CCSS-B001'||content.candidateSha!==sha||
     content.rowsExamined!==500||content.expectedLessons!==500||
     content.testScope!=='SOURCE_LINKED_EFFECTIVE_IN_REPOSITORY_TEXT_AND_RICH_V2_RUNTIME_COMPOSITION')
    throw Error('Effective learner-composer artifact missing exact-SHA provenance');
  if(https.batchId!=='US-CCSS-B001'||https.checked!==500||
     https.generationAndSelfGradingPassed!==500||
     https.malformedAnswersRejectedAndPersisted!==500||
     https.malformedAnswersNotVerified!==0||
     https.releaseDecision!=='NO_GO'||https.productionAuthenticatedUatApproved!==0)
    throw Error('Authenticated HTTPS evidence was missing or did not pass all 500 tests');
  // HTTPS artifact does not encode a commit SHA. The associated verified
  // workflow-run head SHA MUST be checked by GitHub API before invoking this.
  if(meta.httpsRunHeadSha!==sha)throw Error('HTTPS workflow run is not from the exact candidate SHA');
  const e=strictRows('real engine',engine.lessons,expected);
  const v=strictRows('real learner composer',content.lessons,expected);
  const h=strictRows('real authenticated HTTPS',https.lessons,expected);
  const rows=[],problems=[];
  for(const source of catalogue.lessons){
    const code=source.lessonCode,a=e.get(code),b=v.get(code),c=h.get(code);
    const generated=a.status==='ENGINE_SAMPLES_PASSED'&&a.generatedCases>=3&&
      a.familiesTested>=1&&(!Array.isArray(a.reasons)||a.reasons.length===0);
    const composable=b.technicalStatus==='SOURCE_AND_RICH_COMPOSABLE'&&
      b.issues?.length===0&&b.richV2ExampleCount>0&&
      b.richV2ExplanationCount>0&&
      b.sourceUrl===source.sourceUrl&&b.sourceEdition===source.sourceEdition;
    const persisted=allowedHttpsStatuses.has(c.status)&&
      c.negativeStatus==='MALFORMED_ANSWER_REJECTED_AND_PERSISTED';
    if(!generated||!composable||!persisted)problems.push({lessonCode:code,
      engine:generated,richV2Technical:composable,realHttpsPersistence:persisted});
    const evidence={};
    if(generated)evidence.question_family_generation=pass(sha,meta.engineRun,
      'UsCcssB001FiveHundredPracticeEngineTests.EveryB001LessonGeneratesAnswersAndRejectsMalformedResponses');
    if(persisted){
      evidence.wrong_and_malformed_answer_rejection=pass(sha,meta.httpsRun,
        'acceptance_two_school_https.py:B001_500_MALFORMED_ANSWER_REJECTION');
      evidence.authenticated_practice_persistence=pass(sha,meta.httpsRun,
        'acceptance_two_school_https.py:B001_500_REAL_PRACTICE_GENERATE_AND_GRADE');
    }
    rows.push({
      lessonCode:code,candidateSha:sha,sourceSemanticSha256:source.semanticSha256,
      sourceEdition:source.sourceEdition,catalogueContentVersion:source.contentVersion,
      technicalP4:{
        realEngineSampling:generated?'PASS':'NOT_VERIFIED',
        effectiveRichComposition:composable?'PASS_TECHNICAL_ONLY':'NOT_VERIFIED',
        authenticatedGradingAndMalformedPersistence:persisted?'PASS_DISPOSABLE_CI_ONLY':'NOT_VERIFIED',
        syntheticSurveyFixture:c.status==='SYNTHETIC_SURVEY_TALLY_PERSISTED_NEEDS_REAL_TEACHER_VERIFICATION'
      },
      evidence,
      // Never manufacture an effective-body hash, independent math oracle,
      // source/publisher licence proof, historical drift or final acceptance.
      decision:'TESTED_PENDING_INDEPENDENT_ACADEMIC_VERIFICATION',
      historicalT001DriftVerified:false,
      missingAcademicRequirements:[
        'SOURCE_LICENCE_AND_LESSON_GOAL_FIDELITY',
        'EFFECTIVE_STUDENT_TEACHING_AND_VISUAL_ACTIVITY_PARITY',
        'LESSON_SPECIFIC_INDEPENDENT_MATH_ORACLE_WHERE_NOT_EVIDENCED',
        'HISTORICAL_DRIFT_IF_APPLICABLE',
        'PRODUCTION_AUTHENTICATED_STUDENT_UAT'
      ]
    });
  }
  const validated=500-problems.length;
  return {schemaVersion:1,batchId:'US-CCSS-B001',candidateSha:sha,
    originalSource:'Real prior GitHub Actions P4 artifacts on exact commit, NOT a new test run',
    evidenceRuns:{
      engine:{id:meta.engineRun,url:url(meta.engineRun)},
      learnerComposer:{id:meta.contentRun,url:url(meta.contentRun)},
      authenticatedDisposableHttps:{id:meta.httpsRun,url:url(meta.httpsRun)}
    },
    technicalVerifiedLessons:validated,technicalIncompleteLessons:problems.length,
    lessonsWithNewInternalAcademicAcceptance:0,releaseDecision:'NO_GO',
    reason:'The existing P5 gate requires original teaching/visual/source/licence and independently verified academic records, which these technical tests do not prove. Do not re-run passing tests or synthesize PASS.',
    technicalFailures:problems,lessons:rows};
}
function parseArgs(argv){
  const o={};for(const x of argv){if(!x.startsWith('--')||!x.includes('='))throw Error('Use --key=value');
    const i=x.indexOf('=');o[x.slice(2,i)]=x.slice(i+1);}
  for(const k of ['sha','engine','content','https','engine-run','content-run','https-run','out'])
    if(!o[k])throw Error('Missing --'+k);
  return o;
}
function main(args=process.argv.slice(2)){
  const a=parseArgs(args),load=p=>JSON.parse(fs.readFileSync(path.resolve(p),'utf8'));
  const report=reconcile(load(a.catalogue||catalogueDefault),load(a.engine),load(a.content),
    load(a.https),{
      sha:a.sha,engineRun:Number(a['engine-run']),contentRun:Number(a['content-run']),
      httpsRun:Number(a['https-run']),httpsRunHeadSha:a['https-run-head-sha']
    });
  const out=path.resolve(a.out);
  fs.mkdirSync(path.dirname(out),{recursive:true});
  fs.writeFileSync(out,JSON.stringify(report,null,2)+'\n');
  console.log(JSON.stringify({batchId:report.batchId,technicalVerified:report.technicalVerifiedLessons,
    technicalIncomplete:report.technicalIncompleteLessons,academicAccepted:0,
    decision:report.releaseDecision,out}));
  if(report.technicalIncompleteLessons>0)process.exitCode=1;
}
if(require.main===module){try{main();}catch(error){console.error('[B001 P4 RECORDED RESULTS] '+error.message);process.exitCode=1;}}
module.exports={reconcile,strictRows};
