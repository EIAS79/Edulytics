'use strict';
const {test}=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const ROOT=path.resolve(__dirname,'..');
const {initial,evaluate,validLessonAssessment}=require('../tools/curriculum/us-ccss-full-grade-gate.cjs');
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

test('authored-only original lesson 65 must not be certified by unrelated numeric Practice',()=>{
  const e={allowedFamilies:[],originalAuthorshipTaskReviewed:true,
    learnerAuthoredTaskRendered:true,teacherReviewRubricAvailable:true,
    actualTeacherObservationNotClaimed:true};
  assert.equal(validLessonAssessment(65,e,()=>true),true);
  assert.equal(validLessonAssessment(65,{...e,learnerAuthoredTaskRendered:false},()=>true),false);
  assert.equal(validLessonAssessment(65,{...e,allowedFamilies:['fake.addition']},()=>true),false);
  assert.equal(validLessonAssessment(65,{...e,actualTeacherObservationNotClaimed:false},()=>true),false);
});
test('mixed digital/authored original lesson 146 needs BOTH proofs',()=>{
  const digital={allowedFamilies:['usccss.g1.u08.l10.write_riddle_numeric_check'],
    realPracticeGradingPersisted:true,independentCorrectAndWrongAnswersVerified:true,
    independentGradingProofPath:'math',independentGradingProofSha256:'a'.repeat(64),
    runtimePracticeProofPath:'attempt',runtimePracticeProofSha256:'b'.repeat(64)};
  assert.equal(validLessonAssessment(146,digital,()=>true),false);
  const both={...digital,originalAuthorshipTaskReviewed:true,
    learnerAuthoredTaskRendered:true,teacherReviewRubricAvailable:true,
    actualTeacherObservationNotClaimed:true};
  assert.equal(validLessonAssessment(146,both,()=>true),true);
  assert.equal(validLessonAssessment(146,{...both,realPracticeGradingPersisted:false},()=>true),false);
  assert.equal(validLessonAssessment(146,both,()=>false),false);
});
test('ordinary numeric lessons keep actual grading and persisted attempts mandatory',()=>{
  const e={allowedFamilies:['usccss.g1.u03.l14.center.add_subtract'],realPracticeGradingPersisted:true,
    independentCorrectAndWrongAnswersVerified:true,
    independentGradingProofPath:'math',independentGradingProofSha256:'a'.repeat(64),
    runtimePracticeProofPath:'attempt',runtimePracticeProofSha256:'b'.repeat(64)};
  assert.equal(validLessonAssessment(51,e,()=>true),true);
  assert.equal(validLessonAssessment(51,{...e,allowedFamilies:[]},()=>true),false);
  assert.equal(validLessonAssessment(51,{...e,allowedFamilies:['math.family']},()=>true),false);
  assert.equal(validLessonAssessment(51,{...e,allowedFamilies:['usccss.g1.u03.l15.story.three_addends_ten']},()=>true),false);
  assert.equal(validLessonAssessment(51,{...e,allowedFamilies:[e.allowedFamilies[0],e.allowedFamilies[0]]},()=>true),false);
  assert.equal(validLessonAssessment(50,e,()=>true),false);
  assert.equal(validLessonAssessment(147,e,()=>true),false);
  assert.equal(validLessonAssessment(51,{...e,independentCorrectAndWrongAnswersVerified:false},()=>true),false);
  assert.equal(validLessonAssessment(51,e,()=>false),false);
});

test('original optional ordinal 119 requires source-authored comparison-story teaching AND true numeric grading',()=>{
  const digital={allowedFamilies:['usccss.g1.u06.l17.optional_compare_stories'],
    realPracticeGradingPersisted:true,independentCorrectAndWrongAnswersVerified:true,
    independentGradingProofPath:'math',independentGradingProofSha256:'a'.repeat(64),
    runtimePracticeProofPath:'https',runtimePracticeProofSha256:'b'.repeat(64)};
  assert.equal(validLessonAssessment(119,digital,()=>true),false);
  const both={...digital,originalAuthorshipTaskReviewed:true,
    learnerAuthoredTaskRendered:true,teacherReviewRubricAvailable:true,
    actualTeacherObservationNotClaimed:true};
  assert.equal(validLessonAssessment(119,both,()=>true),true);
  assert.equal(validLessonAssessment(119,{...both,realPracticeGradingPersisted:false},()=>true),false);
  assert.equal(validLessonAssessment(119,{...both,learnerAuthoredTaskRendered:false},()=>true),false);
});

test('original Grade 1 ordinal 135 teacher-reviewed Sunday schedule is required alongside validated clock Practice',()=>{
 const e={allowedFamilies:['usccss.g1.u07.l16.daily_schedule_time'],
   realPracticeGradingPersisted:true,independentCorrectAndWrongAnswersVerified:true,
   independentGradingProofPath:'math',independentGradingProofSha256:'a'.repeat(64),
   runtimePracticeProofPath:'https',runtimePracticeProofSha256:'b'.repeat(64)};
 assert.equal(validLessonAssessment(135,e,()=>true),false);
 const together={...e,originalAuthorshipTaskReviewed:true,
  learnerAuthoredTaskRendered:true,teacherReviewRubricAvailable:true,
  actualTeacherObservationNotClaimed:true};
 assert.equal(validLessonAssessment(135,together,()=>true),true);
 assert.equal(validLessonAssessment(135,{...together,actualTeacherObservationNotClaimed:false},()=>true),false);
 assert.equal(validLessonAssessment(135,{...together,realPracticeGradingPersisted:false},()=>true),false);
});

test('source-drawing original unit 7 lessons 123-126 require both numeric Practice proof and teacher shape-creation rubric',()=>{
  for(const ordinal of [123,124,125,126]){
    const mapping=require('../artifacts/curriculum-delivery/US-CCSS-MATH/G1/full-grade-practice-candidate-map.json');
    const f=mapping.records.find(x=>x.ordinal===ordinal).candidateFamily;
    const digital={allowedFamilies:[f],realPracticeGradingPersisted:true,
      independentCorrectAndWrongAnswersVerified:true,
      independentGradingProofPath:'math',independentGradingProofSha256:'a'.repeat(64),
      runtimePracticeProofPath:'https',runtimePracticeProofSha256:'b'.repeat(64)};
    assert.equal(validLessonAssessment(ordinal,digital,()=>true),false);
    const full={...digital,originalAuthorshipTaskReviewed:true,
       learnerAuthoredTaskRendered:true,teacherReviewRubricAvailable:true,
       actualTeacherObservationNotClaimed:true};
    assert.equal(validLessonAssessment(ordinal,full,()=>true),true);
    assert.equal(validLessonAssessment(ordinal,{...full,realPracticeGradingPersisted:false},()=>true),false);
    assert.equal(validLessonAssessment(ordinal,{...full,learnerAuthoredTaskRendered:false},()=>true),false);
    assert.equal(validLessonAssessment(ordinal,{...full,actualTeacherObservationNotClaimed:false},()=>true),false);
  }
});


test('V1 accepts two source-mapped Practice families without permitting arbitrary extras',()=>{
  const {sourceGroundedFamilies}=require('../tools/curriculum/us-ccss-full-grade-gate.cjs');
  assert.deepEqual(sourceGroundedFamilies({candidateFamily:'source.a'}),['source.a']);
  assert.deepEqual(sourceGroundedFamilies({
    candidateFamilies:['source.a','source.b']
  }),['source.a','source.b']);
  assert.deepEqual(sourceGroundedFamilies({candidateFamily:null}),[]);
  assert.equal(sourceGroundedFamilies({candidateFamilies:['source.a','source.a']}),null);
  assert.equal(sourceGroundedFamilies({candidateFamilies:['source.a','']}),null);
  assert.equal(sourceGroundedFamilies({candidateFamilies:[123]}),null);
});

test('V1 immutable Git evidence supports real existing source/tests, not invented URLs or fake hashes',()=>{
  const crypto=require('node:crypto');
  const {execFileSync}=require('node:child_process');
  const {validGitEvidenceReference}=require('../tools/curriculum/us-ccss-full-grade-gate.cjs');
  const head=execFileSync('git',['rev-parse','HEAD'],{cwd:ROOT,encoding:'utf8'}).trim();
  const source='docs/curriculum/plans/EDULYTICS_GRADE_FIRST_FIVE_LESSON_EXECUTION_PLAN_V1.md';
  const content=execFileSync('git',['show',head+':'+source],{cwd:ROOT});
  const real={kind:'GIT_FILE',commitSha:head,path:source,
    sha256:crypto.createHash('sha256').update(content).digest('hex')};
  assert.equal(validGitEvidenceReference(real,head),true);
  assert.equal(validGitEvidenceReference({...real,sha256:'0'.repeat(64)},head),false);
  assert.equal(validGitEvidenceReference({...real,kind:'GITHUB_URL'},head),false);
  assert.equal(validGitEvidenceReference({...real,path:'../package.json'},head),false);
  assert.equal(validGitEvidenceReference({...real,path:'-malicious'},head),false);
  assert.equal(validGitEvidenceReference({...real,commitSha:'f'.repeat(40)},head),false);
  assert.equal(validGitEvidenceReference(real,'not_a_sha'),false);
});

test('V1 code proof can be scoped to explicit impacted files but cannot omit its scope',()=>{
  const {execFileSync}=require('node:child_process');
  const {sourceUnchangedSinceProof}=require('../tools/curriculum/us-ccss-full-grade-gate.cjs');
  const head=execFileSync('git',['rev-parse','HEAD'],{cwd:ROOT,encoding:'utf8'}).trim();
  assert.equal(sourceUnchangedSinceProof(head,head),true);
  assert.equal(sourceUnchangedSinceProof(head,head,
    ['tools/curriculum/us-ccss-full-grade-gate.cjs']),true);
  assert.equal(sourceUnchangedSinceProof(head,head,[]),false);
  assert.equal(sourceUnchangedSinceProof(head,head,
    ['tools/curriculum/us-ccss-full-grade-gate.cjs','tools/curriculum/us-ccss-full-grade-gate.cjs']),false);
  assert.equal(sourceUnchangedSinceProof(head,head,['../README.md']),false);
  assert.equal(sourceUnchangedSinceProof(head,head,['src/Edulytics.Core']),false);
  assert.equal(sourceUnchangedSinceProof(head,'bad_sha',['src/Edulytics.Core/placeholder.cs']),false);
});
