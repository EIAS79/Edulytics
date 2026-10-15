'use strict';
const {test}=require('node:test'),assert=require('node:assert/strict'),crypto=require('node:crypto');
const {verify}=require('../tools/us-ccss-b01-g1-lesson3to5-l6-verifier.cjs');
const commit='d'.repeat(40),sha='a'.repeat(64);
const ids=['L03','L04','L05'].map(x=>'PED:US-CCSS-MATH:G1:U01:'+x);
const title=['Add 1 or 2','More Work with 1 and 2','Explore Addition and Subtraction PLC Activity'];
const png=Buffer.concat([Buffer.from('89504e470d0a1a0a','hex'),Buffer.alloc(1500,7)]);
const digest=crypto.createHash('sha256').update(png).digest('hex');
function fixture(){
 const manifest={batchId:'B01',lessons:ids.map(x=>({lessonCode:x,sourceSemanticSha256:sha}))};
 const ledger={records:ids.map(x=>({lessonCode:x,contentSha256:sha,status:'AUTOMATED_SCREENED',
 academicDecision:'NOT_CERTIFIED',gates:{L6:{status:'NOT_STARTED'}}}))};
 const witness={schemaVersion:1,kind:'B01_G1_U01_L03_L05_AUTHENTICATED_STUDENT_RENDER_CANDIDATES',
 environment:'DISPOSABLE_US_PRIMARY_HTTPS_POSTGRES',checkedOutCommit:commit,
 academicDecision:'NOT_CERTIFIED',lessonRecords:ids.map((lessonCode,i)=>({
 lessonCode,title:title[i],sourceSemanticSha256:sha,effectiveContentSha256:sha,
 learnerPageHttpVerified:true,
 screenshots:{
 desktop:{viewportWidth:1440,viewportHeight:900,imageSha256:digest,visibleMathVisualCount:1},
 mobile:{viewportWidth:390,viewportHeight:844,imageSha256:digest,visibleMathVisualCount:1}}
 }))};
 return {manifest,ledger,witness,headSha:commit,readScreenshot:()=>png};
}
test('Three source-bound Chromium lessons produce evidence, NEVER PASS',()=>{
 const result=verify(fixture());
 assert.equal(result.lessonsVerified,3);
 assert.equal(result.viewportsVerified,6);
 assert.match(result.decision,/NO_ACADEMIC_PASS/);
});
test('Stale body or edited screenshot bytes fail closed',()=>{
 const a=fixture();a.witness.lessonRecords[0].effectiveContentSha256='b'.repeat(64);
 assert.throws(()=>verify(a),/Expected values/);
 const b=fixture();b.readScreenshot=()=>Buffer.concat([Buffer.from('89504e470d0a1a0a','hex'),Buffer.alloc(1500,8)]);
 assert.throws(()=>verify(b),/Screenshot SHA mismatch/);
});
test('No grade drift, unauthorized certification or incomplete viewport pairs',()=>{
 const a=fixture();a.witness.lessonRecords[2].lessonCode=ids[1];
 assert.throws(()=>verify(a),/Wrong or duplicate/);
 const b=fixture();b.ledger.records[0].gates.L6.status='PASS';
 assert.throws(()=>verify(b),/Expected values/);
 const c=fixture();delete c.witness.lessonRecords[0].screenshots.mobile;
 assert.throws(()=>verify(c),/Expected values/);
});
