'use strict';
const {test}=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const ROOT=path.resolve(__dirname,'..');
const {initial,evaluate,validSourceAssessmentEvidence,validRecordedAuthorshipDecision}=require('../tools/curriculum/us-ccss-full-grade-gate.cjs');
const manifest=JSON.parse(fs.readFileSync(path.join(ROOT,'artifacts/curriculum-delivery/US-CCSS-MATH/G1/grade-manifest.json'),'utf8'));
const fresh=()=>structuredClone(initial(manifest));
test('146 unique canonical lessons, first new ordinal 51, old 50 historical only',()=>{
  const ledger=fresh();
  assert.equal(ledger.historical.count,50);
  assert.equal(ledger.newLessons.length,96);
  assert.equal(ledger.newLessons[0].ordinal,51);
  assert.equal(ledger.newLessons[95].ordinal,146);
  assert.equal(ledger.newLessons[0].lessonCode,'PED:US-CCSS-MATH:G1:U03:L14');
  assert.equal(ledger.historical.status,'HISTORICALLY_RELEASED_NO_RETEST');
});
test('real incomplete Grade 1 is NO_GO, not a 146 pass',()=>{
  const a=evaluate(manifest,fresh());
  assert.equal(a.outcome,'NO_GO');
  assert.equal(a.newAccepted,0);
  assert.equal(a.newNotAccepted,96);
  assert.equal(a.historicRetestedCount,0);
  assert.equal(a.productionLive,false);
});
test('cannot change historical acceptance SHA or relabel 50 as new',()=>{
  const d=fresh();d.historical.mainSha='a'.repeat(40);
  assert.equal(evaluate(manifest,d).outcome,'NO_GO');
  assert.match(evaluate(manifest,d).issues.join(','),/Historical/);
});
test('cannot re-order, duplicate or omit an unaccepted new lesson',()=>{
  const d=fresh();d.newLessons[1]=d.newLessons[0];
  assert.equal(evaluate(manifest,d).outcome,'NO_GO');
  assert.ok(evaluate(manifest,d).issues.some(x=>x.includes('52')));
  d.newLessons.pop();assert.equal(evaluate(manifest,d).outcome,'NO_GO');
});
test('labels without real source/Practice/test proof cannot pass',()=>{
  const d=fresh();
  d.newLessons[0].status='INTERNALLY_ACCEPTED';
  d.newLessons[0].evidence={candidateSha:'2c7c43a8f3af7151d422acc6e8e9e8fa5348833c'};
  assert.equal(evaluate(manifest,d).newAccepted,0);
  assert.equal(evaluate(manifest,d).outcome,'NO_GO');
});
test('rejected source or wrong candidate SHA cannot be inferred as accepted',()=>{
  const d=fresh();
  const result=evaluate(manifest,d,'bad');
  assert.equal(result.outcome,'NO_GO');
  assert.ok(result.issues.some(x=>x.includes('Candidate SHA')));
});

test('original lesson 65 authentic authoring can be assessed without inventing an auto-graded numeric family',()=>{
  const authored={
    allowedFamilies:[],authoredTaskContentVerified:true,teacherLedOutcomeNotClaimed:true,
    authoredTaskContentProofPath:'source-teaching-and-review-evidence',
    authoredTaskContentProofSha256:'a'.repeat(64)
  };
  const simulatedVerifiedProof=()=>true;
  assert.equal(validSourceAssessmentEvidence(65,authored,simulatedVerifiedProof),true);
  assert.equal(validSourceAssessmentEvidence(65,{...authored,authoredTaskContentVerified:false},simulatedVerifiedProof),false);
  assert.equal(validSourceAssessmentEvidence(65,{...authored,allowedFamilies:['fake.numeric.quiz']},simulatedVerifiedProof),false);
  // Authentic school submissions are correctly PENDING; this is a valid proposed teacher rubric.
  assert.equal(validRecordedAuthorshipDecision(65),true);
});

test('lesson 146 numeric check never substitutes for student-authored riddle',()=>{
  const digital={
    allowedFamilies:['usccss.g1.u08.l10.write_number_riddles'],
    realPracticeGradingPersisted:true,independentCorrectAndWrongAnswersVerified:true,
    independentGradingProofPath:'math',independentGradingProofSha256:'a'.repeat(64),
    runtimePracticeProofPath:'attempt',runtimePracticeProofSha256:'b'.repeat(64)
  };
  const simulatedVerifiedProof=()=>true;
  assert.equal(validSourceAssessmentEvidence(146,digital,simulatedVerifiedProof),false);
  const authored={...digital,authoredTaskContentVerified:true,
    teacherLedOutcomeNotClaimed:true,
    authoredTaskContentProofPath:'reviewed-riddle-teaching-and-rubric',
    authoredTaskContentProofSha256:'c'.repeat(64)};
  assert.equal(validSourceAssessmentEvidence(146,authored,simulatedVerifiedProof),true);
  assert.equal(validRecordedAuthorshipDecision(146),true);
});

test('ordinary Grade 1 digital lesson still requires real graded answer and persistent runtime evidence',()=>{
  const complete={
    allowedFamilies:['math.family'],
    realPracticeGradingPersisted:true,independentCorrectAndWrongAnswersVerified:true,
    independentGradingProofPath:'independent-grading',
    runtimePracticeProofPath:'persisted-attempt'
  };
  const simulatedVerifiedProof=()=>true;
  assert.equal(validSourceAssessmentEvidence(51,complete,simulatedVerifiedProof),true);
  assert.equal(validSourceAssessmentEvidence(51,{...complete,realPracticeGradingPersisted:false},simulatedVerifiedProof),false);
  assert.equal(validSourceAssessmentEvidence(51,{...complete,allowedFamilies:[]},simulatedVerifiedProof),false);
  assert.equal(validSourceAssessmentEvidence(51,complete,()=>false),false);
});
