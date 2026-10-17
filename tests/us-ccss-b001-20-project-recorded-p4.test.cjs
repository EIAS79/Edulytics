'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const {loadManifest,evaluateEvidence}=
  require('../tools/curriculum/us-ccss-b001-20-acceptance-gate.cjs');
const {loadCatalogue}=require('../tools/curriculum/us-ccss-b001-acceptance-gate.cjs');
const {projectRecordedP4}=
  require('../tools/curriculum/us-ccss-b001-20-project-recorded-p4.cjs');
const manifest=loadManifest(),legacy=loadCatalogue(),sha='a'.repeat(40);
function fixture(){
  return {schemaVersion:1,batchId:'US-CCSS-B001',candidateSha:sha,
    releaseDecision:'NO_GO',lessonsWithNewInternalAcademicAcceptance:0,
    technicalVerifiedLessons:500,technicalIncompleteLessons:0,
    testedSourceKind:'GITHUB_PR_SYNTHETIC_MERGE_CHECKOUT_VERIFIED',
    evidenceRuns:{
      engine:{id:123,url:'https://github.com/EIAS79/Edulytics/actions/runs/123'},
      learnerComposer:{id:124,url:'https://github.com/EIAS79/Edulytics/actions/runs/124'},
      authenticatedDisposableHttps:{id:125,url:'https://github.com/EIAS79/Edulytics/actions/runs/125'}
    },
    lessons:legacy.lessons.map(x=>({
      lessonCode:x.lessonCode,candidateSha:sha,sourceSemanticSha256:x.semanticSha256,
      sourceEdition:x.sourceEdition,catalogueContentVersion:x.contentVersion,
      decision:'TESTED_PENDING_INDEPENDENT_ACADEMIC_VERIFICATION',
      technicalP4:{realEngineSampling:'PASS',effectiveRichComposition:'PASS_TECHNICAL_ONLY',
        authenticatedGradingAndMalformedPersistence:'PASS_DISPOSABLE_CI_ONLY'},
      evidence:{question_family_generation:{result:'pass',candidateSha:sha,
        testId:'SYNTHETIC_FIXTURE_ONLY',runUrl:'https://github.com/EIAS79/Edulytics/actions/runs/123'}}
    }))
  };
}
test('Canonical first 20 technical records project out of the full legacy P4 without academic upgrading',()=>{
  const actual=projectRecordedP4(manifest,legacy,fixture(),sha);
  assert.equal(actual.total,20);
  assert.equal(actual.technicalVerifiedLessons,20);
  assert.equal(actual.lessonsWithNewInternalAcademicAcceptance,0);
  assert.equal(actual.releaseDecision,'NO_GO');
  assert.deepEqual(actual.lessons.map(x=>x.lessonCode),manifest.lessons.map(x=>x.lessonCode));
  assert(actual.lessons.every(x=>x.historicalReleaseDriftVerified===false));
  assert(actual.lessons.every(x=>x.effectiveBodySha256===undefined));
  assert.equal(evaluateEvidence(manifest,actual,sha).state,'NO_GO');
  assert.equal(evaluateEvidence(manifest,actual,sha).acceptedCount,0);
});
test('Reject partial/tampered/duplicate/mixed-SHA original artifacts rather than invent pass',()=>{
  const partial=fixture();partial.lessons.pop();
  assert.throws(()=>projectRecordedP4(manifest,legacy,partial,sha),/Only full original/);
  const spoof=fixture();spoof.lessons[45].lessonCode=spoof.lessons[44].lessonCode;
  assert.throws(()=>projectRecordedP4(manifest,legacy,spoof,sha),/order\/identity mismatch/);
  const mixed=fixture();mixed.lessons[150].candidateSha='b'.repeat(40);
  assert.throws(()=>projectRecordedP4(manifest,legacy,mixed,sha),/per-lesson SHA mismatch/);
  const incomplete=fixture();incomplete.evidenceRuns.engine.id=0;
  assert.throws(()=>projectRecordedP4(manifest,legacy,incomplete,sha),/original authentic P4 run provenance/);
  const wrong=fixture();
  assert.throws(()=>projectRecordedP4(manifest,legacy,wrong,'c'.repeat(40)),/Legacy P4 SHA/);
});
test('A failing technical lesson is faithfully retained as unverified in its 20-lesson cohort',()=>{
  const x=fixture();x.lessons[19].technicalP4.realEngineSampling='NOT_VERIFIED';
  x.technicalVerifiedLessons=499;x.technicalIncompleteLessons=1;
  const projected=projectRecordedP4(manifest,legacy,x,sha);
  assert.equal(projected.technicalVerifiedLessons,19);
  assert.equal(projected.technicalIncompleteLessons,1);
  assert.equal(projected.releaseDecision,'NO_GO');
});
