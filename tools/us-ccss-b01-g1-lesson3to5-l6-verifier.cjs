#!/usr/bin/env node
'use strict';
// Source-bound authenticated learner witness verification only. Never signs
// an academic L6 gate, approves source adaptation, or certifies the 500 batch.
const fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const assert=require('node:assert/strict');
const sha=x=>typeof x==='string'&&/^[a-f0-9]{64}$/.test(x);
const commit=x=>typeof x==='string'&&/^[a-f0-9]{40}$/.test(x);
const prefix='PED:US-CCSS-MATH:G1:U01:';
const titles=new Map([
 [prefix+'L03','Add 1 or 2'],
 [prefix+'L04','More Work with 1 and 2'],
 [prefix+'L05','Explore Addition and Subtraction PLC Activity']
]);
function verify({manifest,ledger,witness,headSha,readScreenshot}){
 assert(commit(headSha),'Immutable checked-out commit required');
 assert.equal(witness.schemaVersion,1);
 assert.equal(witness.kind,'B01_G1_U01_L03_L05_AUTHENTICATED_STUDENT_RENDER_CANDIDATES');
 assert.equal(witness.environment,'DISPOSABLE_US_PRIMARY_HTTPS_POSTGRES');
 assert.equal(witness.checkedOutCommit,headSha);
 assert.equal(witness.academicDecision,'NOT_CERTIFIED');
 assert.equal(manifest.batchId,'B01');
 assert.equal(witness.lessonRecords?.length,3,'Exactly three independent learner pages required');
 const found=new Set(),out=[];
 const expectedViewports={desktop:[1440,900],mobile:[390,844]};
 for(const record of witness.lessonRecords){
  const code=record.lessonCode;
  assert(titles.has(code)&&!found.has(code),'Wrong or duplicate lesson');
  found.add(code);
  assert.equal(record.title,titles.get(code));
  const m=manifest.lessons.filter(x=>x.lessonCode===code),l=ledger.records.filter(x=>x.lessonCode===code);
  assert.equal(m.length,1);assert.equal(l.length,1);
  assert.equal(record.sourceSemanticSha256,m[0].sourceSemanticSha256);
  assert.equal(record.effectiveContentSha256,l[0].contentSha256);
  assert(sha(record.sourceSemanticSha256)&&sha(record.effectiveContentSha256));
  assert.equal(l[0].academicDecision,'NOT_CERTIFIED');
  assert.equal(l[0].status,'AUTOMATED_SCREENED');
  assert.equal(l[0].gates.L6.status,'NOT_STARTED');
  assert.equal(record.learnerPageHttpVerified,true);
  assert.deepEqual(Object.keys(record.screenshots).sort(),['desktop','mobile']);
  for(const [screen,viewport] of Object.entries(expectedViewports)){
   const snap=record.screenshots[screen],buf=readScreenshot(code,screen);
   assert.deepEqual([snap.viewportWidth,snap.viewportHeight],viewport);
   assert(Number.isInteger(snap.visibleMathVisualCount)&&snap.visibleMathVisualCount>=1);
   assert(Buffer.isBuffer(buf)&&buf.length>1000,'Missing original Chromium bytes');
   assert.equal(buf.subarray(0,8).toString('hex'),'89504e470d0a1a0a','Invalid PNG');
   assert.equal(snap.imageSha256,crypto.createHash('sha256').update(buf).digest('hex'),'Screenshot SHA mismatch');
  }
  out.push({lessonCode:code,sha:record.effectiveContentSha256,authenticatedViewports:2});
 }
 assert.equal(found.size,3);
 return {kind:'B01_G1_U01_L03_L05_INDEPENDENT_L6_RENDER_WITNESSES',
    checkedOutCommit:headSha,lessonsVerified:3,viewportsVerified:6,sourceBoundEvidence:out,
    decision:'AUTHENTICATED_L6_RENDER_EVIDENCE_ONLY_NO_ACADEMIC_PASS',
    note:'Correctness of every source activity, assessment, visual and live production behavior still needs academic review. CI screenshots alone are not a certificate.'};
}
function main(){
 const root=path.resolve(__dirname,'..');
 const read=x=>JSON.parse(fs.readFileSync(path.isAbsolute(x)?x:path.join(root,x),'utf8'));
 const proof=read('/tmp/g1-u01-l03-l05-authenticated-student-evidence.json');
 const result=verify({manifest:read('artifacts/academic-us-ccss/batches/B01-lesson-manifest.json'),
 ledger:read('artifacts/academic-us-ccss/04-lesson-certification-records.json'),
 witness:proof,headSha:process.env.GITHUB_SHA,
 readScreenshot:(code,screen)=>{
   const lesson=code.rsplit?code.rsplit(':',1):code.split(':').at(-1);
   return fs.readFileSync('/tmp/g1-u01-'+lesson.toLowerCase()+'-'+screen+'.png');
 }});
 fs.writeFileSync('/tmp/g1-u01-l03-l05-independent-verification.json',
   JSON.stringify(result,null,2)+'\n');
 console.log(JSON.stringify(result));
}
if(require.main===module){try{main()}catch(e){console.error('G1 L03-L05 L6 verifier: '+e.message);process.exitCode=1}}
module.exports={verify};
