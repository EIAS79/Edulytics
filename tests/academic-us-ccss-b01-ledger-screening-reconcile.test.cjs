'use strict';
const test=require('node:test'),assert=require('node:assert/strict');
const {createInitialLedger}=require('../tools/us-ccss-lesson-certification-records.cjs');
const {reconcile}=require('../tools/us-ccss-b01-ledger-screening-reconcile.cjs');
const sha='abcdef0123456789abcdef0123456789abcdef01';
const date='2026-10-15T12:00:00Z';
function fixture(){
 const source={records:Array.from({length:1560},(_,i)=>({
   lessonCode:'PED:US-CCSS-MATH:G1:U01:L'+String(i+1).padStart(4,'0'),
   title:'Test '+i,gradeOrCourse:'Grade 1',sourceUrl:'https://im.kendallhunt.com/k5/teachers/test',
   sourceSemanticSha256:'a'.repeat(64),officialOutcomeCodes:[],formalTargetCodes:[]}))};
 const ledger=createInitialLedger(source);
 const manifest={batchId:'B01',lessons:source.records.slice(0,500).map((x,i)=>({
   ordinal:i+1,lessonCode:x.lessonCode,sourceSemanticSha256:x.sourceSemanticSha256}))};
 const runtime={kind:'B01_500_EFFECTIVE_CONTENT_AND_REAL_PRACTICE_RUNTIME_PROBE',
   inspected:500,unique:500,runtimeFailures:0,
   lessonRows:source.records.slice(0,500).map((x,i)=>({
    ordinal:i+1,lessonCode:x.lessonCode,
    effectiveContentSha256:i.toString(16).padStart(64,'0'),
    effectiveContentVersion:'sample-v1',generatedCases:6,engineIssues:[],
    sourcePreparationUrl:'https://im.kendallhunt.com/k5/teachers/test'
   }))};
 return {source,ledger,manifest,runtime,headSha:sha,runId:'38732225517',atUtc:date};
}
test('500 audited runtime rows become SCREENED, not certified; 1060 other lessons untouched',()=>{
 const input=fixture(),old=structuredClone(input.ledger);
 const result=reconcile(input);
 assert.equal(result.summary.screened,500);
 assert.equal(result.summary.refusedToAwardPass,true);
 assert.equal(result.summary.academicDecision,'NOT_500_BATCH_CERTIFIED');
 assert.deepEqual(input.ledger,old);
 assert.equal(result.ledger.records[0].status,'AUTOMATED_SCREENED');
 assert.equal(result.ledger.records[499].contentVersion,'sample-v1');
 assert.equal(result.ledger.records[500].status,'QUEUED');
 assert.equal(result.ledger.records[499].gates.L6.status,'NOT_STARTED');
 assert.equal(result.ledger.records[499].academicDecision,'NOT_CERTIFIED');
 assert.equal(result.ledger.records[499].reviewDimensions,null);
 assert.deepEqual(result.ledger.records[500],old.records[500]);
});
test('Running the exact same runtime twice cannot change certified states',()=>{
 const a=fixture(),first=reconcile(a).ledger;
 const after=reconcile({...a,ledger:first});
 assert.deepEqual(after.ledger,first);
 assert.equal(after.summary.screened,0);
 assert.equal(after.summary.alreadyScreened,500);
 assert.equal(after.summary.stillNeedFullL1L6AndEightDimensions,500);
});
test('Certified pilot is never overwritten',()=>{
 const a=fixture();a.ledger.records[0].status='INTERNALLY_CERTIFIED';
 a.ledger.records[0].academicDecision='INTERNALLY_CERTIFIED';
 a.ledger.records[0].contentSha256=a.runtime.lessonRows[0].effectiveContentSha256;
 const result=reconcile(a);
 assert.equal(result.summary.preservedCertified,1);
 assert.equal(result.summary.screened,499);
 assert.equal(result.ledger.records[0].status,'INTERNALLY_CERTIFIED');
 a.runtime.lessonRows[0].effectiveContentSha256='1'.repeat(64);
 assert.throws(()=>reconcile(a),/Historic certified lesson/);
});
test('Source order, content hashes and runtime failures fail closed',()=>{
 const a=fixture();a.runtime.lessonRows[8].ordinal=9;
 a.runtime.lessonRows[8].lessonCode=a.runtime.lessonRows[9].lessonCode;
 assert.throws(()=>reconcile(a),/Unordered/);
 const b=fixture();b.manifest.lessons[1].sourceSemanticSha256='b'.repeat(64);
 assert.throws(()=>reconcile(b),/source SHA drift/);
 const c=fixture();c.runtime.lessonRows[5].engineIssues=['incorrect grading'];
 assert.throws(()=>reconcile(c),/Incomplete actual student-content/);
 const d=fixture();d.runtime.runtimeFailures=1;
 assert.throws(()=>reconcile(d),/Successful complete/);
});

test('A repaired AUTOMATED_SCREENED lesson is re-screened against the new exact HEAD without academic PASS',()=>{
 const a=fixture(),first=reconcile(a);
 const old=structuredClone(first.ledger);
 a.runtime.lessonRows[2].effectiveContentSha256='f'.repeat(64);
 a.runtime.lessonRows[2].effectiveContentVersion='lesson-repair-v2';
 const next=reconcile({...a,ledger:old,runId:'38740000002',
   headSha:'1234567890abcdef1234567890abcdef12345678'});
 assert.equal(next.summary.screened,0);
 assert.equal(next.summary.rescreened,1);
 assert.equal(next.summary.alreadyScreened,499);
 assert.equal(next.summary.stillNeedFullL1L6AndEightDimensions,500);
 assert.equal(next.ledger.records[2].contentSha256,'f'.repeat(64));
 assert.equal(next.ledger.records[2].contentVersion,'lesson-repair-v2');
 assert.equal(next.ledger.records[2].screeningProvenance.githubRunId,'38740000002');
 assert.equal(next.ledger.records[2].screeningProvenance.auditedHeadSha,
   '1234567890abcdef1234567890abcdef12345678');
 assert.equal(next.ledger.records[2].academicDecision,'NOT_CERTIFIED');
 assert.equal(next.ledger.records[2].reviewDimensions,null);
 assert.ok(Object.values(next.ledger.records[2].gates).every(x=>x.status==='NOT_STARTED'));
 assert.equal(next.ledger.records[3].contentSha256,old.records[3].contentSha256);
 assert.equal(old.records[2].contentVersion,'sample-v1');
});
test('Re-screen refuses corrupted previous provenance or started academic gates',()=>{
 const a=fixture(),first=reconcile(a).ledger;
 a.runtime.lessonRows[2].effectiveContentSha256='f'.repeat(64);
 const invalid=structuredClone(first);
 invalid.records[2].screeningProvenance.learnerContentSha256='a'.repeat(64);
 assert.throws(()=>reconcile({...a,ledger:invalid}),/Previously screened runtime provenance/);
 const active=structuredClone(first);
 active.records[2].gates.L4.status='IN_PROGRESS';
 assert.throws(()=>reconcile({...a,ledger:active}),/cannot overwrite started academic review/);
});

test('Previously reviewed academic gates and changed body fingerprints never get overwritten',()=>{
 const a=fixture();a.ledger.records[2].gates.L4.status='IN_PROGRESS';
 assert.throws(()=>reconcile(a),/cannot overwrite started/);
 const b=fixture();b.ledger.records[2].contentSha256='f'.repeat(64);
 assert.throws(()=>reconcile(b),/stale learner-body fingerprint/);
});
