'use strict';
const {test}=require('node:test'),assert=require('node:assert/strict');
const {verify}=require('../tools/us-ccss-b01-all500-student-http-verifier.cjs');
const grades=[['G1',146],['G2',146],['G3',143],['G4',65]];
const headSha='f'.repeat(40);
function fixture(){
 const lessons=grades.flatMap(([grade,n])=>Array.from({length:n},(_,i)=>({
 lessonCode:grade==='G1'&&i===0 ? 'PED:US-CCSS-MATH:G1:U01:L01' :
   'PED:US-CCSS-MATH:'+grade+':U01:L'+String(i+1).padStart(3,'0'),
 sourceSemanticSha256:'b'.repeat(64)
 })));
 const manifest={batchId:'B01',lessons:lessons.map((x,i)=>({...x,ordinal:i+1}))};
 const ledger={records:lessons.map((x,i)=>({lessonCode:x.lessonCode,contentSha256:'a'.repeat(64),
 academicDecision:i?'NOT_CERTIFIED':'INTERNALLY_CERTIFIED',
 gates:{L6:{status:i?'NOT_STARTED':'PASS'}}}))};
 const proof={schemaVersion:1,kind:'B01_ALL500_GRADE_SPECIFIC_AUTHENTICATED_STUDENT_HTTPS_INVENTORY',
 decision:'500_STUDENT_HTTP_SOURCE_AND_RICHV2_SCREEN_NOT_ACADEMIC_L6_PASS',
 checkedOutCommit:headSha,sourceProgramme:'US-CCSS',schoolCode:'REHEARSAL-US-PRIMARY',
 lessonRows:manifest.lessons.map(x=>({...x,effectiveContentSha256:'a'.repeat(64),
 learnerTitleMatched:true,richV2HtmlPresent:true,mathVisualMarkupPresent:true,
 authenticatedHtmlSha256:'c'.repeat(64),authenticatedCohort:x.lessonCode.match(/:(G[1-4]):/)[1]}))};
 return {manifest,ledger,proof,headSha};
}
test('Grade-specific 500 authenticated HTML source coverage never promotes academic PASS',()=>{
 const x=verify(fixture());
 assert.equal(x.inspectedAuthenticatedLessonPages,500);
 assert.deepEqual(x.gradeCounts,{G1:146,G2:146,G3:143,G4:65});
 assert.equal(x.verifiedEveryChromiumScreen,false);
 assert.match(x.decision,/NOT_APPROVED/);
});
test('Only the actual G1 U01 L01 identity may carry the historical certification',()=>{
 const x=fixture();
 assert.equal(x.manifest.lessons[0].lessonCode,'PED:US-CCSS-MATH:G1:U01:L01');
 const wrong=fixture();
 wrong.ledger.records[0].lessonCode='PED:US-CCSS-MATH:G1:U01:L002';
 assert.throws(()=>verify(wrong),/Original 500-row SHA ledger required/);
 const fake=fixture();
 fake.ledger.records[1].academicDecision='INTERNALLY_CERTIFIED';
 fake.ledger.records[1].gates.L6.status='PASS';
 assert.throws(()=>verify(fake),/Expected values/);
});
test('Missing, duplicate, renamed and shuffled lessons fail closed',()=>{
 const a=fixture();a.proof.lessonRows.pop();
 assert.throws(()=>verify(a),/Expected values/);
 const b=fixture();b.proof.lessonRows[5].lessonCode=b.proof.lessonRows[6].lessonCode;
 assert.throws(()=>verify(b),/Expected values/);
 const c=fixture();c.proof.lessonRows.reverse();
 assert.throws(()=>verify(c),/Omitted, reordered/);
});
test('Wrong grade, stale SHA and fake L6 academic PASS fail closed',()=>{
 const a=fixture();a.proof.lessonRows[146].authenticatedCohort='G1';
 assert.throws(()=>verify(a),/Expected values/);
 const b=fixture();b.proof.lessonRows[222].effectiveContentSha256='d'.repeat(64);
 assert.throws(()=>verify(b),/Expected values/);
 const c=fixture();c.ledger.records[5].gates.L6.status='PASS';
 assert.throws(()=>verify(c),/Expected values/);
});
