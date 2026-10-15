'use strict';
const {test}=require('node:test'),assert=require('node:assert/strict');
const {validate}=require('../tools/us-ccss-b01-g1-all146-student-http-verifier.cjs');
const headSha='d'.repeat(40);
function fixture(){
 const lessons=Array.from({length:146},(_,i)=>({
  lessonCode:'PED:US-CCSS-MATH:G1:U01:L'+String(i+1).padStart(2,'0'),
  sourceSemanticSha256:'a'.repeat(64)
 }));
 const ledger={records:lessons.map((x,i)=>({
  lessonCode:x.lessonCode,contentSha256:'b'.repeat(64),
  academicDecision:i===0?'INTERNALLY_CERTIFIED':'NOT_CERTIFIED',
  gates:{L6:{status:i===0?'PASS':'NOT_STARTED'}}
 }))};
 const inventory={kind:'B01_G1_146_AUTHENTICATED_STUDENT_HTTPS_CONTENT_INVENTORY',
 decision:'ROUTE_AND_STUDENT_CONTENT_SCREENED_NO_L6_OR_ACADEMIC_PASS',
 checkedOutCommit:headSha,schoolCode:'REHEARSAL-US-PRIMARY',
 lessonRows:lessons.map((x,i)=>({ordinal:i+1,lessonCode:x.lessonCode,
  sourceSemanticSha256:x.sourceSemanticSha256,effectiveContentSha256:'b'.repeat(64),
  learnerTitleMatched:true,richV2HtmlPresent:true,
  mathVisualMarkupPresent:i%2===0,
  authenticatedHtmlSha256:'c'.repeat(64)}))
 };
 return {manifest:{batchId:'B01',lessons},ledger,inventory,headSha};
}
test('All 146 source bound actual student-route hashes count as triage, not academic PASS',()=>{
 const x=validate(fixture());
 assert.equal(x.inspected,146);
 assert.equal(x.mathVisualMarkupObserved,73);
 assert.equal(x.actualChromiumScreenshotsForAll146,false);
 assert.match(x.decision,/NOT_L6_ACADEMIC_APPROVAL/);
});
test('Missing lesson or source SHA and stale HEAD fail',()=>{
 const a=fixture();a.inventory.lessonRows[10].lessonCode='WRONG';
 assert.throws(()=>validate(a),/Expected values/);
 const b=fixture();b.inventory.lessonRows[11].sourceSemanticSha256='d'.repeat(64);
 assert.throws(()=>validate(b),/Expected values/);
 const c=fixture();c.inventory.checkedOutCommit='0'.repeat(40);
 assert.throws(()=>validate(c),/Expected values/);
});
test('Authenticity markers and 146-row count fail closed',()=>{
 const a=fixture();a.inventory.lessonRows[15].learnerTitleMatched=false;
 assert.throws(()=>validate(a),/Expected values/);
 const b=fixture();b.inventory.lessonRows.pop();
 assert.throws(()=>validate(b),/Expected values/);
 const c=fixture();c.ledger.records[3].gates.L6.status='PASS';
 assert.throws(()=>validate(c),/Expected values/);
});
