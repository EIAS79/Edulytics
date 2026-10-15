#!/usr/bin/env node
'use strict';
// Real browser L6/academic PASS is deliberately NOT inferred from one HTML
// response. Screenshots, practice grading, maths and lesson-specific review
// remain independent release requirements.
const fs=require('node:fs'),path=require('node:path'),assert=require('node:assert/strict');
const sha=x=>typeof x==='string'&&/^[a-f0-9]{64}$/.test(x);
const commit=x=>typeof x==='string'&&/^[a-f0-9]{40}$/.test(x);
const counts={G1:146,G2:146,G3:143,G4:65};
function verify({manifest,ledger,proof,headSha}){
 assert(commit(headSha),'Exact immutable Git commit SHA required');
 assert.equal(proof.schemaVersion,1);
 assert.equal(proof.kind,'B01_ALL500_GRADE_SPECIFIC_AUTHENTICATED_STUDENT_HTTPS_INVENTORY');
 assert.equal(proof.decision,'500_STUDENT_HTTP_SOURCE_AND_RICHV2_SCREEN_NOT_ACADEMIC_L6_PASS');
 assert.equal(proof.checkedOutCommit,headSha,'Incorrect HEAD, stale student evidence');
 assert.equal(proof.sourceProgramme,'US-CCSS');
 assert.equal(proof.schoolCode,'REHEARSAL-US-PRIMARY');
 assert.equal(manifest.batchId,'B01');
 assert.equal(manifest.lessons?.length,500);
 assert.equal(proof.lessonRows?.length,500);
 const byCode=new Map(ledger.records.map(x=>[x.lessonCode,x]));
 const observed={G1:0,G2:0,G3:0,G4:0};let visuals=0;const ids=new Set();
 for(let i=0;i<500;i++){
  const r=proof.lessonRows[i],m=manifest.lessons[i];
  assert(m&&r&&r.ordinal===i+1&&m.ordinal===i+1,
    'Omitted, reordered or injected lesson in 500 cohort');
  assert.equal(r.lessonCode,m.lessonCode);
  assert(!ids.has(r.lessonCode),'Duplicate authenticated lesson');
  ids.add(r.lessonCode);
  const grade=r.lessonCode.match(/^PED:US-CCSS-MATH:(G[1-4]):/)?.[1];
  assert(grade&&counts[grade]!==undefined,'Unexpected grade/lesson scope');
  if(grade!=='G1')assert.equal(r.authenticatedCohort,grade);
  const cert=byCode.get(r.lessonCode);
  assert(cert&&sha(cert.contentSha256),'Original 500-row SHA ledger required');
  assert.equal(r.sourceSemanticSha256,m.sourceSemanticSha256);
  assert.equal(r.effectiveContentSha256,cert.contentSha256);
  assert.equal(r.learnerTitleMatched,true);
  assert.equal(r.richV2HtmlPresent,true);
  assert(sha(r.authenticatedHtmlSha256));
  assert.equal(typeof r.mathVisualMarkupPresent,'boolean');
  if(r.mathVisualMarkupPresent)visuals++;
  if(r.lessonCode==='PED:US-CCSS-MATH:G1:U01:L01'){
    assert.equal(cert.academicDecision,'INTERNALLY_CERTIFIED');
  }else{
    assert.equal(cert.academicDecision,'NOT_CERTIFIED');
    assert.equal(cert.gates.L6.status,'NOT_STARTED');
  }
  observed[grade]++;
 }
 assert.deepEqual(observed,counts,'Missing or duplicated grade-specific lesson');
 return {kind:'B01_ALL500_GRADE_SPECIFIC_STUDENT_HTTPS_HTTP_SCREEN',
 checkedOutCommit:headSha,gradeCounts:observed,
 inspectedAuthenticatedLessonPages:500,exactRichV2SourceTitles:500,
 mathVisualMarkupFound:visuals,
 verifiedEveryChromiumScreen:false,verifiedEveryPracticeGrading:false,
 decision:'500_HTTP_COVERAGE_ONLY_499_L1_L6_NOT_APPROVED',
 note:'Student access, title and HTML smoke is not the same as academic source teaching, visual mathematics, L6 screenshot or production acceptance.'};
}
function main(){
 const root=path.resolve(__dirname,'..');
 const read=x=>JSON.parse(fs.readFileSync(path.isAbsolute(x)?x:path.join(root,x),'utf8'));
 const result=verify({manifest:read('artifacts/academic-us-ccss/batches/B01-lesson-manifest.json'),
 ledger:read('artifacts/academic-us-ccss/04-lesson-certification-records.json'),
 proof:read('/tmp/b01-all500-authenticated-student-https-inventory.json'),
 headSha:process.env.GITHUB_SHA});
 fs.writeFileSync('/tmp/b01-all500-authenticated-student-http-verification.json',
 JSON.stringify(result,null,2)+'\n');
 console.log(JSON.stringify(result));
}
if(require.main===module){try{main()}catch(e){console.error('B01 all500 authenticated student HTTP: '+e.message);process.exitCode=1}}
module.exports={verify};
