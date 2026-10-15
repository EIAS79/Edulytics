#!/usr/bin/env node
'use strict';
const fs=require('node:fs'),path=require('node:path'),assert=require('node:assert/strict');
const isSha=x=>typeof x==='string'&&/^[0-9a-f]{64}$/.test(x);
const commit=x=>typeof x==='string'&&/^[0-9a-f]{40}$/.test(x);
const scope=x=>typeof x==='string'&&x.startsWith('PED:US-CCSS-MATH:G1:');
function validate({manifest,ledger,inventory,headSha}){
 assert(commit(headSha),'Actual checked-out commit required');
 assert.equal(manifest.batchId,'B01');
 assert.equal(inventory.kind,'B01_G1_146_AUTHENTICATED_STUDENT_HTTPS_CONTENT_INVENTORY');
 assert.equal(inventory.decision,'ROUTE_AND_STUDENT_CONTENT_SCREENED_NO_L6_OR_ACADEMIC_PASS');
 assert.equal(inventory.checkedOutCommit,headSha);
 assert.equal(inventory.schoolCode,'REHEARSAL-US-PRIMARY');
 assert.equal(inventory.lessonRows?.length,146);
 const manifestG1=manifest.lessons.filter(x=>scope(x.lessonCode));
 assert.equal(manifestG1.length,146);
 const records=new Map(ledger.records.map(x=>[x.lessonCode,x]));
 let visualCount=0;
 for(let i=0;i<146;i++){
  const m=manifestG1[i],r=inventory.lessonRows[i],cert=records.get(m.lessonCode);
  assert.equal(r.ordinal,i+1);
  assert.equal(r.lessonCode,m.lessonCode);
  assert(isSha(r.authenticatedHtmlSha256),'Missing response byte digest');
  assert.equal(r.sourceSemanticSha256,m.sourceSemanticSha256);
  assert(cert&&isSha(cert.contentSha256)&&r.effectiveContentSha256===cert.contentSha256);
  assert.equal(r.learnerTitleMatched,true);
  assert.equal(r.richV2HtmlPresent,true);
  assert.equal(cert.academicDecision,
    m.lessonCode==='PED:US-CCSS-MATH:G1:U01:L01'?'INTERNALLY_CERTIFIED':'NOT_CERTIFIED');
  if(m.lessonCode!=='PED:US-CCSS-MATH:G1:U01:L01'){
   assert.equal(cert.gates.L6.status,'NOT_STARTED');
  }
  assert.equal(typeof r.mathVisualMarkupPresent,'boolean');
  visualCount+=Number(r.mathVisualMarkupPresent);
 }
 return {kind:'B01_G1_AUTHENTICATED_STUDENT_146_HTTP_SOURCE_SCREEN_VERIFIED',
  inspected:146,originalLessonCodes:146,richV2HttpPresent:146,
  mathVisualMarkupObserved:visualCount,actualChromiumScreenshotsForAll146:false,
  everyStudentPracticeGradingVerified:false,
  evidenceBoundToCommit:headSha,
  decision:'G1_146_HTTP_TRIAGE_ONLY_NOT_L6_ACADEMIC_APPROVAL',
  note:'Authenticated route/title/HTML smoke is not a guarantee of visual correctness or full academic lesson certification.'};
}
function main(){
 const root=path.resolve(__dirname,'..');
 const read=x=>JSON.parse(fs.readFileSync(path.isAbsolute(x)?x:path.join(root,x),'utf8'));
 const result=validate({manifest:read('artifacts/academic-us-ccss/batches/B01-lesson-manifest.json'),
 ledger:read('artifacts/academic-us-ccss/04-lesson-certification-records.json'),
 inventory:read('/tmp/g1-all146-authenticated-student-html-inventory.json'),
 headSha:process.env.GITHUB_SHA});
 fs.writeFileSync('/tmp/g1-all146-authenticated-html-independent-verification.json',
 JSON.stringify(result,null,2)+'\n');
 console.log(JSON.stringify(result));
}
if(require.main===module){try{main()}catch(e){console.error('G1 all146 HTTP evidence: '+e.message);process.exitCode=1}}
module.exports={validate};
