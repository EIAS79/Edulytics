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
test('G1 96 historical exact-source scopes are unchanged except seven SHA-pinned audited Grade 3/4 additions',()=>{
  const {execFileSync}=require('node:child_process');
  const {sourceUnchangedSinceProof}=require('../tools/curriculum/us-ccss-full-grade-gate.cjs');
  const ledger=JSON.parse(fs.readFileSync(path.join(ROOT,
    'artifacts/curriculum-delivery/US-CCSS-MATH/G1/full-grade-progress.json'),'utf8'));
  const head=execFileSync('git',['rev-parse','HEAD'],{cwd:ROOT,encoding:'utf8'}).trim();
  const proof='e702b36328ff90cbb01e7ac81a4a90c06e165989';
  const paths=new Set();
  assert.equal(ledger.newLessons.length,96);
  for(const row of ledger.newLessons){
    assert.equal(row.status,'INTERNALLY_ACCEPTED');
    assert.equal(row.evidence.testedCodeSha,proof);
    for(const p of row.evidence.affectedCodePaths)paths.add(p);
    assert.equal(sourceUnchangedSinceProof(proof,head,row.evidence.affectedCodePaths),true,
      'A previously accepted G1 lesson has an unaudited shared-code delta: '+row.ordinal);
  }
  assert.equal(paths.size,124);
  const changed=execFileSync('git',['diff','--name-only',proof,head,'--',...paths],
    {cwd:ROOT,encoding:'utf8'}).trim().split(/\\r?\\n/).filter(Boolean);
  assert.equal(changed.length,7, 'Unexpected change in a Grade 1 proof-affecting code path');
  assert.deepEqual(changed.filter(p=>p.includes('/G1')).sort(),[],
    'Grade 1-specific source code itself changed');
});

test('the ACTUAL progress ledger counts every internally accepted lesson only with original V1 source, Practice, tests and SHA proofs',()=>{
  const ledger=JSON.parse(fs.readFileSync(
    path.join(ROOT,'artifacts/curriculum-delivery/US-CCSS-MATH/G1/full-grade-progress.json'),'utf8'));
  const candidateSha=require('node:child_process').execFileSync('git',['rev-parse','HEAD'],
    {cwd:ROOT,encoding:'utf8'}).trim();
  const report=evaluate(manifest,ledger,candidateSha);
  const declared=ledger.newLessons.filter(x=>x.status==='INTERNALLY_ACCEPTED').length;
  const stale=report.missing.filter(row=>
    ledger.newLessons[row.ordinal-51]?.status==='INTERNALLY_ACCEPTED');
  // An earlier lesson approval remains a historical fact, but an updated
  // shared math/grading dependency invalidates its OLD exact-SHA runtime
  // proof until that lesson is genuinely revalidated. Fail closed instead of
  // pretending the declared approval is verified on this candidate.
  assert.equal(report.newAccepted+stale.length,declared);
  // Current source and authenticated grading evidence must revalidate every
  // declared approval; historical statuses alone do NOT make the release gate green.
  assert.equal(stale.length,0,
    'Approved V1 lesson has stale exact-candidate source/grading evidence');
  assert.equal(report.newAccepted,declared,
    'Original V1 acceptance count must be independently verified at candidate SHA');
  assert.equal(report.newNotAccepted,96-report.newAccepted);
  assert.equal(report.declaredNewAccepted,declared);
  assert.deepEqual(report.unverifiedDeclaredAcceptances,
    stale.map(row=>row.ordinal));
  assert.equal(report.firstVerifiedUnfinishedOrdinal,
    report.missing[0]?.ordinal??147);
  if(stale.length>0){
    assert.equal(report.outcome,'NO_GO');
    assert.ok(report.issues.some(s=>s.includes('Progress nextOrdinal disagrees')),
      'The old academic cursor must not hide stale accepted proofs');
    assert.ok(stale.every(row=>row.reason.includes('Invalid/missing verifiable')));
  }else{
    assert.equal(report.issues.length,0,report.issues.join('; '));
  }
  if(declared<96)assert.equal(report.outcome,'NO_GO');
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


test('original U04 L09 own-collection authorship and four-gallery task cannot be approved from digital place-value grading alone',()=>{
  const digital={
    allowedFamilies:['usccss.g1.u04.l09.show.expanded_form'],
    realPracticeGradingPersisted:true,
    independentCorrectAndWrongAnswersVerified:true,
    independentGradingProofPath:'math',
    independentGradingProofSha256:'a'.repeat(64),
    runtimePracticeProofPath:'attempt',
    runtimePracticeProofSha256:'b'.repeat(64)
  };
  assert.equal(validLessonAssessment(74,digital,()=>true),false);
  const both={...digital,originalAuthorshipTaskReviewed:true,
    learnerAuthoredTaskRendered:true,teacherReviewRubricAvailable:true,
    actualTeacherObservationNotClaimed:true};
  assert.equal(validLessonAssessment(74,both,()=>true),true);
  assert.equal(validLessonAssessment(74,{...both,learnerAuthoredTaskRendered:false},()=>true),false);
  assert.equal(validLessonAssessment(74,{...both,teacherReviewRubricAvailable:false},()=>true),false);
  assert.equal(validLessonAssessment(74,{...both,realPracticeGradingPersisted:false},()=>true),false);
  assert.equal(validLessonAssessment(74,{...both,actualTeacherObservationNotClaimed:false},()=>true),false);
  assert.equal(validLessonAssessment(74,both,()=>false),false);
});

test('ordinal 142 requires BOTH authentic two-question ride authorship and independently graded comparison Practice',()=>{
  const digital={
    allowedFamilies:['usccss.g1.u08.l06.compare_difference_story'],
    realPracticeGradingPersisted:true,
    independentCorrectAndWrongAnswersVerified:true,
    independentGradingProofPath:'math',
    independentGradingProofSha256:'a'.repeat(64),
    runtimePracticeProofPath:'attempt',
    runtimePracticeProofSha256:'b'.repeat(64)
  };
  // Numerically correct comparison tasks alone never evidence two
  // personally-authored and solved ride-table questions.
  assert.equal(validLessonAssessment(142,digital,()=>true),false);
  const complete={...digital,
    originalAuthorshipTaskReviewed:true,
    learnerAuthoredTaskRendered:true,
    teacherReviewRubricAvailable:true,
    actualTeacherObservationNotClaimed:true};
  assert.equal(validLessonAssessment(142,complete,()=>true),true);
  assert.equal(validLessonAssessment(142,
    {...complete,learnerAuthoredTaskRendered:false},()=>true),false);
  assert.equal(validLessonAssessment(142,
    {...complete,teacherReviewRubricAvailable:false},()=>true),false);
  assert.equal(validLessonAssessment(142,
    {...complete,realPracticeGradingPersisted:false},()=>true),false);
  assert.equal(validLessonAssessment(142,
    {...complete,actualTeacherObservationNotClaimed:false},()=>true),false);
  assert.equal(validLessonAssessment(142,complete,()=>false),false);
});

test('original class-number-book lesson 144 requires real authored page and three-expression rubric in addition to numeric Practice',()=>{
  const numeric={
    allowedFamilies:['usccss.g1.u08.l08.same_number_tens_ones'],
    realPracticeGradingPersisted:true,
    independentCorrectAndWrongAnswersVerified:true,
    independentGradingProofPath:'math',
    independentGradingProofSha256:'a'.repeat(64),
    runtimePracticeProofPath:'student',
    runtimePracticeProofSha256:'b'.repeat(64)
  };
  assert.equal(validLessonAssessment(144,numeric,()=>true),false);
  const authored={...numeric,
    originalAuthorshipTaskReviewed:true,
    learnerAuthoredTaskRendered:true,
    teacherReviewRubricAvailable:true,
    actualTeacherObservationNotClaimed:true};
  assert.equal(validLessonAssessment(144,authored,()=>true),true);
  assert.equal(validLessonAssessment(144,
    {...authored,learnerAuthoredTaskRendered:false},()=>true),false);
  assert.equal(validLessonAssessment(144,
    {...authored,originalAuthorshipTaskReviewed:false},()=>true),false);
  assert.equal(validLessonAssessment(144,
    {...authored,teacherReviewRubricAvailable:false},()=>true),false);
  assert.equal(validLessonAssessment(144,authored,()=>false),false);
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


test('all 11 source-authored rubrics preserve each required ORIGINAL task; generic rubrics must fail closed',()=>{
  const {validSourceAuthorshipRubric}=require('../tools/curriculum/us-ccss-full-grade-gate.cjs');
  const dir='artifacts/curriculum-delivery/US-CCSS-MATH/G1/';
  const mapped=JSON.parse(fs.readFileSync(path.join(ROOT,dir,'full-grade-practice-candidate-map.json'),'utf8'));
  const authored=mapped.records.filter(row=>row.assessmentMode!=='DIGITAL_PRACTICE');
  assert.deepEqual(authored.map(row=>row.ordinal),
    [65,74,119,123,124,125,126,135,142,144,146]);
  const essential=new Map([
    [65,'Sufficient data'],[74,'Original four-collection gallery'],
    [119,'Peer work'],[123,'Source task specificity'],
    [124,'Three source nonexamples'],
    [125,'Five source squares and three source non-squares'],
    [126,'Same-outline alternate construction'],
    [135,'Schedule consistency'],[142,'Two original questions'],
    [144,'Three distinct expressions'],[146,'Valid clues']
  ]);
  for(const candidate of authored){
    const ordinal=candidate.ordinal;
    const rubric=JSON.parse(fs.readFileSync(path.join(ROOT,dir,candidate.teacherOnlyRubric),'utf8'));
    assert.equal(validSourceAuthorshipRubric(ordinal,candidate,rubric),true,
      'Current original-task rubric must be usable: '+ordinal);
    const sourceSpecific=essential.get(ordinal);
    const reduced=structuredClone(rubric);
    reduced.teacherRubric=reduced.teacherRubric.filter(x=>x.criterion!==sourceSpecific);
    assert.equal(validSourceAuthorshipRubric(ordinal,candidate,reduced),false,
      'Removing original source task must block: '+ordinal);
    const blank=structuredClone(rubric);
    blank.teacherRubric[0].required=' ';
    assert.equal(validSourceAuthorshipRubric(ordinal,candidate,blank),false);
    const duplicate=structuredClone(rubric);
    duplicate.teacherRubric.push({...duplicate.teacherRubric[0]});
    assert.equal(validSourceAuthorshipRubric(ordinal,candidate,duplicate),false);
    assert.equal(validSourceAuthorshipRubric(ordinal,candidate,
      {...rubric,academicAcceptance:'INTERNALLY_ACCEPTED'}),false);
    assert.equal(validSourceAuthorshipRubric(ordinal,candidate,
      {...rubric,gradeRelease:'GO'}),false);
    assert.equal(validSourceAuthorshipRubric(ordinal,
      {...candidate,lessonCode:'PED:FORGED'},rubric),false);
  }
  assert.equal(validSourceAuthorshipRubric(74,authored[1],null),false);
  assert.equal(validSourceAuthorshipRubric(73,authored[1],
    JSON.parse(fs.readFileSync(path.join(ROOT,dir,authored[1].teacherOnlyRubric),'utf8'))),false);
});
