#!/usr/bin/env node
'use strict';

/**
 * Re-use actual already-reconciled 500-scope P4 technical rows for the first 20
 * WITHOUT executing any tests or upgrading technical data to academic acceptance.
 * A 500-row legacy report is validated as a whole, and the 20 lesson IDs are
 * matched in canonical order. GitHub provenance remains the original P4 SHA/run.
 */
const fs=require('node:fs');
const path=require('node:path');
const assert=require('node:assert/strict');
const {loadManifest}=require('./us-ccss-b001-20-acceptance-gate.cjs');
const {loadCatalogue}=require('./us-ccss-b001-acceptance-gate.cjs');
const fullSha=/^[0-9a-f]{40}$/i;
function projectRecordedP4(manifest,legacyCatalogue,recorded,sha) {
  assert(fullSha.test(sha||''),'Exact 40-character tested checkout SHA required');
  assert.equal(manifest.planId,'EDULYTICS-US-CCSS-20-V2');
  assert.equal(manifest.total,20);
  assert.equal(manifest.lessons.length,20);
  assert.equal(legacyCatalogue.total,500);
  assert.equal(legacyCatalogue.lessons.length,500);
  assert.deepEqual(manifest.lessons.map(x=>x.lessonCode),
    legacyCatalogue.lessons.slice(0,20).map(x=>x.lessonCode));
  assert.equal(recorded.schemaVersion,1);
  assert.equal(recorded.batchId,'US-CCSS-B001');
  assert.equal(recorded.candidateSha,sha,'Legacy P4 SHA differs from tested SHA');
  assert.equal(recorded.lessons?.length,500,'Only full original 500-row P4 reports can be sliced');
  assert.equal(recorded.releaseDecision,'NO_GO','Legacy report must preserve its recorded state');
  assert.equal(recorded.lessonsWithNewInternalAcademicAcceptance,0);
  assert.equal(recorded.technicalVerifiedLessons+recorded.technicalIncompleteLessons,500);
  assert(recorded.evidenceRuns?.engine?.id>0 &&
    recorded.evidenceRuns?.learnerComposer?.id>0 &&
    recorded.evidenceRuns?.authenticatedDisposableHttps?.id>0,
    'Missing original authentic P4 run provenance');
  const originalCodes=legacyCatalogue.lessons.map(x=>x.lessonCode);
  const rows=new Map();
  for(const [i,row] of recorded.lessons.entries()){
    assert.equal(row.lessonCode,originalCodes[i],'Legacy row order/identity mismatch');
    assert.equal(row.candidateSha,sha,'Legacy per-lesson SHA mismatch');
    assert.equal(row.sourceSemanticSha256,legacyCatalogue.lessons[i].semanticSha256);
    assert.equal(row.sourceEdition,legacyCatalogue.lessons[i].sourceEdition);
    assert.equal(row.catalogueContentVersion,legacyCatalogue.lessons[i].contentVersion);
    assert(!rows.has(row.lessonCode));
    rows.set(row.lessonCode,row);
  }
  const lessons=manifest.lessons.map(x=>{
    const old=rows.get(x.lessonCode);
    return {
      ...old,
      historicalReleaseDriftVerified:false,
      decision:'TESTED_PENDING_INDEPENDENT_ACADEMIC_VERIFICATION',
      // These legacy tests did not establish any of these new academic P5 facts.
      effectiveBodySha256:undefined,
      missingAcademicRequirements:old.missingAcademicRequirements ||
        ['SOURCE_GOAL_FIDELITY','ACADEMIC_MATH_ORACLE','HISTORICAL_DRIFT_IF_APPLICABLE','OWNER_UAT']
    };
  });
  const verified=lessons.filter(x=>
    x.technicalP4?.realEngineSampling==='PASS' &&
    x.technicalP4?.effectiveRichComposition==='PASS_TECHNICAL_ONLY' &&
    x.technicalP4?.authenticatedGradingAndMalformedPersistence==='PASS_DISPOSABLE_CI_ONLY'
  ).length;
  return {
    schemaVersion:1,planId:manifest.planId,batchId:manifest.batchId,total:20,
    candidateSha:sha,
    source:'PROJECTED_FROM_VERIFIED_500_SCOPE_RECORDED_P4_NOT_NEW_TESTS',
    sourceP4RunIds:recorded.evidenceRuns,
    testedSourceKind:recorded.testedSourceKind,
    technicalVerifiedLessons:verified,technicalIncompleteLessons:20-verified,
    lessonsWithNewInternalAcademicAcceptance:0,
    releaseDecision:'NO_GO',
    note:'No P5 PASS: original source/task pedagogy, independent math, historical release drift and owner UAT remain separate proofs.',
    lessons
  };
}
function main(argv=process.argv.slice(2)){
  const args={};
  for(const item of argv){const m=item.match(/^--([^=]+)=(.+)$/);if(!m)throw Error('Use --key=value');
    args[m[1]]=m[2];}
  if(!args['legacy-report'] || !args.sha || !args.out)
    throw Error('Provide --legacy-report, --sha, --out; no test execution occurs');
  const input=JSON.parse(fs.readFileSync(path.resolve(args['legacy-report']),'utf8'));
  const output=projectRecordedP4(loadManifest(),loadCatalogue(),input,args.sha);
  const target=path.resolve(args.out);
  fs.mkdirSync(path.dirname(target),{recursive:true});
  fs.writeFileSync(target,JSON.stringify(output,null,2)+'\n');
  console.log(JSON.stringify({projectedLessons:20,technicalVerified:output.technicalVerifiedLessons,
    academicAccepted:0,decision:'NO_GO',out:target}));
}
if(require.main===module){try{main();}catch(e){console.error('[20-scope recorded P4 projection] '+e.message);process.exitCode=1;}}
module.exports={projectRecordedP4};
