'use strict';
const {test}=require('node:test'),assert=require('node:assert/strict'),crypto=require('node:crypto');
const {validate}=require('../tools/us-ccss-b01-l02-authenticated-l6-verifier.cjs');
const sha='0123456789abcdef0123456789abcdef01234567',s='a'.repeat(64),c='b'.repeat(64);
const CODE='PED:US-CCSS-MATH:G1:U01:L02';
const families=['usccss.b01.g1.check_off_add_within10','usccss.b01.g1.complete_missing_dots','usccss.b01.g1.match_dot_cubes_expression'];
function fixture(){
 const b=Buffer.concat([Buffer.from('89504e470d0a1a0a','hex'),Buffer.alloc(1200,12)]);
 const hash=crypto.createHash('sha256').update(b).digest('hex');
 return {manifest:{lessons:[{lessonCode:CODE,sourceSemanticSha256:s}]},
 ledger:{records:[{lessonCode:CODE,status:'AUTOMATED_SCREENED',
  academicDecision:'NOT_CERTIFIED',contentSha256:c,gates:{L6:{status:'NOT_STARTED'}}}]},
 proof:{schemaVersion:1,kind:'G1_U01_L02_AUTHENTICATED_STUDENT_L6_CANDIDATE',
  decision:'BROWSER_AND_GRADING_PROOF_ONLY_NOT_ACADEMIC_CERTIFICATION',
  checkedOutCommit:sha,environment:'DISPOSABLE_US_PRIMARY_HTTPS_POSTGRES',
  lessonCode:CODE,academicDecision:'NOT_CERTIFIED',
  sourceSemanticSha256:s,effectiveContentSha256:c,
  screenshots:{desktop:{viewportWidth:1440,viewportHeight:900,screenshotSha256:hash},
   mobile:{viewportWidth:390,viewportHeight:844,screenshotSha256:hash}},
  visualDotCountsByViewport:{desktop:[5,8],mobile:[5,8]},
  grading:[{case:'correct',submitted:true,expectedCorrectItems:3,persistedCorrectItems:3,
   persistedScore:3,familyIds:families},
   {case:'wrong',submitted:true,expectedCorrectItems:0,persistedCorrectItems:0,
   persistedScore:0,familyIds:families}]},
 headSha:sha,readScreenshot:()=>b};
}
test('Valid candidate is reviewable but never confers academic PASS',()=>{
 const f=fixture(),original=structuredClone(f.ledger);
 const result=validate(f);
 assert.equal(result.decision,'BROWSER_EVIDENCE_REVIEWABLE_NOT_ACADEMIC_L6_PASS');
 assert.equal(result.reviewedRealScreenshots,2);
 assert.deepEqual(original,f.ledger);
});
test('Reject changed source, body SHA, candidate commit or ledger L6 PASS',()=>{
 for(const field of ['sourceSemanticSha256','effectiveContentSha256','checkedOutCommit']){
  const f=fixture();f.proof[field]='c'.repeat(field==='checkedOutCommit'?40:64);
  assert.throws(()=>validate(f));
 }
 const g=fixture();g.ledger.records[0].gates.L6.status='PASS';
 assert.throws(()=>validate(g),/No unsigned L6 PASS/);
});
test('Reject fake screenshots, changed responsive mathematics, or misleading grading',()=>{
 const a=fixture();a.proof.screenshots.mobile.screenshotSha256='0'.repeat(64);
 assert.throws(()=>validate(a),/SHA mismatch/);
 const b=fixture();b.proof.visualDotCountsByViewport.mobile=[5,7];
 assert.throws(()=>validate(b),/responsive viewports/);
 const c=fixture();c.proof.grading[1].persistedScore=2;
 assert.throws(()=>validate(c));
 const d=fixture();d.proof.grading[0].familyIds=['other'];
 assert.throws(()=>validate(d));
});
