'use strict';
const {test}=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const root=path.resolve(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const sha=p=>crypto.createHash('sha256').update(fs.readFileSync(path.join(root,p))).digest('hex');
const doc=p=>'docs/curriculum/'+p;
const led=read(doc('us-ccss-g8-coder4-p0-current-execution-ledger-20261030.json'));
const unit=read(doc('us-ccss-g8-coder4-u01-strict-unit-gate-20261030.json'));
const aud=read(doc('us-ccss-g8-coder4-u01-final-academic-unit-audit-20261030.json'));
const parse=(s,kind,unit)=>{
 if(typeof s!=='string')return null;s=s.trim();
 if(kind==='pair'){
  let m=s.match(/^\(?\s*([+-]?\d+(?:\.\d+)?)\s*,\s*([+-]?\d+(?:\.\d+)?)\s*\)?$/);
  return m?m.slice(1).map(Number):null;
 }
 let m=s.match(unit==='degrees'?/^[+-]?\d+(?:\.\d+)?°?$/:/^[+-]?\d+(?:\.\d+)?$/);
 return m?Number(s.replace('°','')):null;
};
test('U01 preserves 17 historical authoring records; independent selected offline L01-L09 C routes closed',()=>{
 assert.equal(led.rows.length,131);assert.equal(aud.lessonCommits.length,17);
 assert.equal(unit.unitTotal,17);assert.equal(unit.unitStatus,'UNIT_IN_PROGRESS');
 assert.equal(unit.unitReadyForStageD,false);assert.equal(unit.newlyReadyForStageD,9);
 assert.equal(led.abcCounters.ABC_READY_FOR_D,9);assert.equal(led.abcCounters.D_VERIFIED,0);
 assert.equal(led.abcCounters.UNITS_ABC_CLOSED,0);assert.equal(led.abcCounters.REMAINING,122);
 assert.equal(led.currentSequentialABCCursor.lessonCode,'PED:US-CCSS-MATH:G8:U01:L10');
 assert.equal(led.currentSequentialABCCursor.advanceAllowed,true);
 assert.equal(led.abcCounters.CURRENT_UNIT,'U01');
 assert.ok(led.rows.slice(0,9).every(x=>x.readyForStageD===true));assert.ok(led.rows.slice(9,17).every(x=>x.readyForStageD===false));
 assert.ok(led.rows.slice(0,17).every(x=>x.previousReadyForStageDClaim===true));
 assert.ok(led.rows.slice(17).every(x=>x.readyForStageD!==true));
 const manifest=read(doc('us-ccss-g8-coder4-course-evidence-20261029.json'));
 for(const [pack,hash] of Object.entries(aud.richContentSha256)){
  const f='src/Edulytics.Core/Curriculum/LessonContent/RichV2/'+pack;
  if(pack.includes('u01-l01-l08')) { assert.notEqual(sha(f),hash,'selected paper links change original audit content hash'); } else { assert.equal(sha(f),hash); } assert.equal(sha(f),manifest.hashes.richCandidates[pack]);
 }
});
for(let n=1;n<=17;n++)test('U01 L'+String(n).padStart(2,'0')+' source-scoped A-B-C contract and exact oracles',()=>{
 const num=String(n).padStart(2,'0'),id='PED:US-CCSS-MATH:G8:U01:L'+num;
 const entry=aud.lessonCommits[n-1],row=led.rows[n-1];
 const s=read(doc('us-ccss-g8-coder4-u01-l'+num+'-academic-assessment-contract-20261030.json'));
 assert.equal(entry.lessonCode,id);assert.equal(row.lessonCode,id);
 assert.equal(entry.academicABCCommitSHA,n>=2&&n<=9?row.historicalABCCommitSHA:row.latestABCCommitSHA);
 assert.match(entry.academicABCCommitSHA,/^[a-f0-9]{40}$/);
 assert.equal(entry.sourceFiles.length,3);
 for(const f of entry.sourceFiles)assert.match(f.sha256,/^[a-f0-9]{64}$/);
 const gates=s.gates||s.stageGates;
 assert.equal(gates.READY_FOR_STAGE_D,true);
 assert.equal(gates.C,n<=9?'C_CLOSED_ACADEMIC_SELECTED_FIRST_PARTY_PAPER_TEACHER_ROUTE':'C_CLOSED_ACADEMIC_CONTRACT');
 assert.equal(gates.D,'NOT_RUN');assert.equal(row.readyForStageD,n<=9);
 assert.equal(row.independentCReaudit20261031,n<=9?'C_CLOSED_SELECTED_LESSON_PAPER_TEACHER_ROUTE':'HOLD_EARLIER_CANONICAL_C_GATE');
 let m,kind,unit,correct,wrong,invalid,expected;
 if(n===1){
  m=s.coordinateQuestionCandidate;kind='pair';unit='coordinate';expected=m.key;
  correct=m.correct;wrong=m.wrong;invalid=m.malformed;
  assert.equal(s.studentModes.length,4);assert.equal(s.studentModes[3].rubric.maxPoints,8);
  assert.equal(s.futureStageD.exactPracticeContractReadiness,'NOT_READY_VERIFIED');
 }else if(n<=6){
  m=s.coordinateQuestion.case;kind='pair';unit='coordinate';expected=m.expected;
  correct=m.correct;wrong=m.wrong;invalid=m.invalid;
  assert.equal(s.humanScoredCoolDown.maxPoints,8);
  assert.equal(s.stageDDeferred.readiness,'RUNTIME_NOT_VERIFIED');
 }else{
  m=s.machineCandidate;kind=m.responseType==='coordinate_pair'?'pair':'scalar';unit=m.unit;
  expected=m.expected;correct=m.acceptedEquivalentInputs;wrong=m.wrongRejected;invalid=m.malformedRejected;
  assert.equal(s.humanScoredCooldown.maxPoints,8);
  assert.equal(s.humanScoredCooldown.criteria.length,4);
  assert.equal(s.stageD.lessonPracticeContractReadiness,'NOT_READY_VERIFIED');
  assert.ok(s.sourceActivityToCourseAssessment.length>=3);
  assert.ok(s.sourceActivityToCourseAssessment.every(a=>a.assessmentOwner==='HUMAN_TEACHER'));
 }
 for(const x of correct)assert.deepEqual(parse(x,kind,unit),expected,'correct '+id+': '+x);
 for(const x of wrong)assert.notDeepEqual(parse(x,kind,unit),expected,'wrong '+id+': '+x);
 for(const x of invalid)assert.equal(parse(x,kind,unit),null,'malformed '+id+': '+x);
});