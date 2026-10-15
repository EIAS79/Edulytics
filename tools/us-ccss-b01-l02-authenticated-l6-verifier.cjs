#!/usr/bin/env node
'use strict';
// Independent validator for authenticated HTTPS student screenshot and scoring
// evidence. This is L6 candidate verification only, not academic signoff.
const fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const assert=require('node:assert/strict');
const sha=x=>typeof x==='string'&&/^[a-f0-9]{64}$/.test(x);
const commit=x=>typeof x==='string'&&/^[a-f0-9]{40}$/.test(x);
const CODE='PED:US-CCSS-MATH:G1:U01:L02';
const FAMILIES=[
'usccss.b01.g1.check_off_add_within10',
'usccss.b01.g1.complete_missing_dots',
'usccss.b01.g1.match_dot_cubes_expression'];
function validate({manifest,ledger,proof,headSha,readScreenshot}){
 assert(commit(headSha),'Verified 40-char checked-out SHA required');
 assert.equal(proof.schemaVersion,1);
 assert.equal(proof.kind,'G1_U01_L02_AUTHENTICATED_STUDENT_L6_CANDIDATE');
 assert.equal(proof.decision,'BROWSER_AND_GRADING_PROOF_ONLY_NOT_ACADEMIC_CERTIFICATION');
 assert.equal(proof.checkedOutCommit,headSha,'Stale Chromium evidence from different code commit');
 assert.equal(proof.environment,'DISPOSABLE_US_PRIMARY_HTTPS_POSTGRES');
 assert.equal(proof.lessonCode,CODE);
 assert.equal(proof.academicDecision,'NOT_CERTIFIED');
 const matches=manifest.lessons.filter(x=>x.lessonCode===CODE);
 const rows=ledger.records.filter(x=>x.lessonCode===CODE);
 assert.equal(matches.length,1,'Exactly one source manifest lesson required');
 assert.equal(rows.length,1,'Exactly one canonical ledger lesson required');
 const m=matches[0],r=rows[0];
 assert(sha(m.sourceSemanticSha256)&&sha(r.contentSha256),'Missing immutable source/content SHAs');
 assert.equal(proof.sourceSemanticSha256,m.sourceSemanticSha256);
 assert.equal(proof.effectiveContentSha256,r.contentSha256);
 assert.equal(r.status,'AUTOMATED_SCREENED','Candidate must not claim lesson academic certification');
 assert.equal(r.academicDecision,'NOT_CERTIFIED');
 assert.equal(r.gates.L6.status,'NOT_STARTED','No unsigned L6 PASS allowed');
 assert.deepEqual(Object.keys(proof.screenshots).sort(),['desktop','mobile']);
 assert.deepEqual(Object.keys(proof.visualDotCountsByViewport).sort(),['desktop','mobile']);
 const expected={desktop:[1440,900],mobile:[390,844]};
 const desktop=proof.visualDotCountsByViewport.desktop;
 assert(Array.isArray(desktop)&&desktop.length>0,'Missing mathematical dot counts');
 assert(desktop.every(n=>Number.isInteger(n)&&n>=1&&n<=20));
 assert.deepEqual(proof.visualDotCountsByViewport.mobile,desktop,
   'Mathematical pictures changed across responsive viewports');
 for(const screen of ['desktop','mobile']){
   const p=proof.screenshots[screen];
   assert.deepEqual([p.viewportWidth,p.viewportHeight],expected[screen]);
   const buf=readScreenshot(screen);
   assert(Buffer.isBuffer(buf)&&buf.length>1000,'Missing screenshot bytes');
   assert.equal(buf.subarray(0,8).toString('hex'),'89504e470d0a1a0a',
     'Expected original PNG screenshot');
   assert.equal(p.screenshotSha256,
     crypto.createHash('sha256').update(buf).digest('hex'),
     'PNG evidence SHA mismatch: '+screen);
 }
 assert(Array.isArray(proof.grading)&&proof.grading.length===2);
 const byCase=new Map(proof.grading.map(x=>[x.case,x]));
 assert.equal(byCase.size,2);
 for(const [key,score] of [['correct',3],['wrong',0]]){
   const row=byCase.get(key);
   assert(row&&row.submitted===true);
   assert.equal(row.expectedCorrectItems,score);
   assert.equal(row.persistedCorrectItems,score);
   assert.equal(row.persistedScore,score);
   assert.deepEqual(row.familyIds,FAMILIES);
 }
 return {kind:'G1_U01_L02_INDEPENDENT_L6_CANDIDATE_VERIFIED',
   lessonCode:CODE,checkedOutCommit:headSha,
   effectiveContentSha256:r.contentSha256,sourceSemanticSha256:m.sourceSemanticSha256,
   reviewedRealScreenshots:2,matchingVisualCounts:desktop,
   persistedPositiveAndNegativePracticeAttempts:2,
   decision:'BROWSER_EVIDENCE_REVIEWABLE_NOT_ACADEMIC_L6_PASS',
   reason:'Actual student Chromium and database evidence is a prerequisite, not a full independent L1-L6 academic decision'};
}
function main(){
 const root=path.resolve(__dirname,'..'),read=(p)=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
 const input=read('/tmp/g1-u01-l02-student-l6-evidence.json');
 const result=validate({
  manifest:read('artifacts/academic-us-ccss/batches/B01-lesson-manifest.json'),
  ledger:read('artifacts/academic-us-ccss/04-lesson-certification-records.json'),
  proof:input,headSha:process.env.GITHUB_SHA,
  readScreenshot:screen=>fs.readFileSync('/tmp/g1-u01-l02-'+screen+'.png')});
 fs.writeFileSync('/tmp/g1-u01-l02-independent-l6-verification.json',JSON.stringify(result,null,2)+'\n');
 process.stdout.write(JSON.stringify(result)+'\n');
}
if(require.main===module){try{main()}catch(e){console.error('L02 L6 independent verifier: '+e.message);process.exitCode=1}}
module.exports={validate};
