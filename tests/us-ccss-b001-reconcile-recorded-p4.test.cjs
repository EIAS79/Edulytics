'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const {reconcile}=require('../tools/curriculum/us-ccss-b001-reconcile-recorded-p4.cjs');
const {loadCatalogue,evaluateEvidence}=require('../tools/curriculum/us-ccss-b001-acceptance-gate.cjs');
const catalogue=loadCatalogue();
const sha='a'.repeat(40);
const meta={sha,engineRun:12345,contentRun:12346,httpsRun:12347,httpsRunHeadSha:sha};
function fixture(){
  const engine={batchId:'US-CCSS-B001',candidateSha:sha,lessonCount:500,
    testType:'B001_ENGINE_SAMPLING_NOT_FINAL_ACADEMIC_CERTIFICATION',
    lessons:catalogue.lessons.map(x=>({lessonCode:x.lessonCode,
      status:'ENGINE_SAMPLES_PASSED',familiesTested:1,generatedCases:3,reasons:[]}))};
  const content={batchId:'US-CCSS-B001',candidateSha:sha,
    expectedLessons:500,rowsExamined:500,
    testScope:'SOURCE_LINKED_EFFECTIVE_IN_REPOSITORY_TEXT_AND_RICH_V2_RUNTIME_COMPOSITION',
    lessons:catalogue.lessons.map(x=>({lessonCode:x.lessonCode,
      sourceUrl:x.sourceUrl,sourceEdition:x.sourceEdition,
      technicalStatus:'SOURCE_AND_RICH_COMPOSABLE',
      richV2ExampleCount:3,richV2ExplanationCount:2,issues:[],
      academicAcceptance:'NOT_GRANTED'}))};
  const https={batchId:'US-CCSS-B001',checked:500,
    generationAndSelfGradingPassed:500,malformedAnswersRejectedAndPersisted:500,
    malformedAnswersNotVerified:0,productionAuthenticatedUatApproved:0,
    releaseDecision:'NO_GO',lessons:catalogue.lessons.map((x,i)=>({
      lessonCode:x.lessonCode,
      status:i<2?'SYNTHETIC_SURVEY_TALLY_PERSISTED_NEEDS_REAL_TEACHER_VERIFICATION':
        'SELF_CONSISTENT_GRADE_PERSISTED',
      negativeStatus:'MALFORMED_ANSWER_REJECTED_AND_PERSISTED'
    }))};
  return {engine,content,https};
}
test('Reconciles 500 genuine-shaped P4 rows, preserving only real technical fields; P5 remains NO_GO',()=>{
  const {engine,content,https}=fixture();
  const x=reconcile(catalogue,engine,content,https,meta);
  assert.equal(x.lessons.length,500);
  assert.equal(x.technicalVerifiedLessons,500);
  assert.equal(x.technicalIncompleteLessons,0);
  assert.equal(x.lessonsWithNewInternalAcademicAcceptance,0);
  assert.equal(x.releaseDecision,'NO_GO');
  assert.equal(x.lessons[0].technicalP4.syntheticSurveyFixture,true);
  assert.equal(x.lessons[2].technicalP4.syntheticSurveyFixture,false);
  assert.equal(x.lessons[0].evidence.question_family_generation.runUrl,
    'https://github.com/EIAS79/Edulytics/actions/runs/12345');
  for(const row of x.lessons){
    assert.equal(row.decision,'TESTED_PENDING_INDEPENDENT_ACADEMIC_VERIFICATION');
    assert.equal(row.historicalT001DriftVerified,false);
    assert.equal(row.evidence.question_family_generation.result,'pass');
    assert.equal(row.evidence.authenticated_practice_persistence.result,'pass');
    assert.equal(row.evidence.wrong_and_malformed_answer_rejection.result,'pass');
    assert.equal(row.evidence.source_provenance_and_licence,undefined);
    assert.equal(row.evidence.effective_learner_visible_content,undefined);
    assert.equal(row.evidence.lesson_specific_mathematics_oracle,undefined);
    assert.equal(row.evidence.student_and_school_isolation,undefined);
    assert.equal(row.effectiveBodySha256,undefined);
  }
  assert.equal(evaluateEvidence(catalogue,x,sha).state,'NO_GO');
  assert.equal(evaluateEvidence(catalogue,x,sha).acceptedCount,0);
});
test('Fails closed on mixed commit SHAs, absent lesson rows and spoofed HTTPS provenance',()=>{
  const {engine,content,https}=fixture();
  assert.throws(()=>reconcile(catalogue,engine,content,https,{
    ...meta,httpsRunHeadSha:'b'.repeat(40)}),/HTTPS workflow run/);
  engine.candidateSha='f'.repeat(40);
  assert.throws(()=>reconcile(catalogue,engine,content,https,meta),/Engine artifact/);
  engine.candidateSha=sha;
  content.lessons.pop();
  assert.throws(()=>reconcile(catalogue,engine,content,https,meta),/exactly 500/);
});
test('Incorrect persisted or generated lesson stays unverified, never certifies the whole batch',()=>{
  const {engine,content,https}=fixture();
  engine.lessons[12].status='ENGINE_SAMPLES_FAILED';
  content.lessons[78].technicalStatus='TECHNICAL_REVIEW_REQUIRED';
  https.lessons[321].negativeStatus='MALFORMED_ANSWER_INCORRECTLY_ACCEPTED';
  const r=reconcile(catalogue,engine,content,https,meta);
  assert.equal(r.technicalVerifiedLessons,497);
  assert.equal(r.technicalIncompleteLessons,3);
  assert.equal(r.technicalFailures.length,3);
  assert.equal(r.lessonsWithNewInternalAcademicAcceptance,0);
  assert.equal(evaluateEvidence(catalogue,r,sha).state,'NO_GO');
  assert.equal(r.lessons[12].evidence.question_family_generation,undefined);
  assert.equal(r.lessons[321].evidence.authenticated_practice_persistence,undefined);
});
