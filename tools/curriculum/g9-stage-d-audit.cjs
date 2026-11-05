#!/usr/bin/env node
'use strict';
const fs=require('node:fs'),path=require('node:path'),assert=require('node:assert/strict');
const root=path.resolve(__dirname,'../..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const ledger=read('artifacts/curriculum-delivery/US-CCSS-MATH/G9/grade9-stage-d-execution-ledger.json');
const counts=[16,26,10,18,21,17,24];
assert.equal(ledger.lessons.length,132,'Partial grade rejected');
const seen=new Set();
for(let i=0;i<132;i++){
 const r=ledger.lessons[i];let unit=1,prior=0;
 while(i>=prior+counts[unit-1]){prior+=counts[unit-1];unit++;}
 const num=i-prior+1,code='PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U'+String(unit).padStart(2,'0')+':L'+String(num).padStart(2,'0');
 assert.equal(r.lessonOrdinal,i+1);assert.equal(r.lessonCode,code);assert(!seen.has(code));seen.add(code);
 assert.equal(r.unit,unit);assert.equal(r.lesson,num);
 assert(r.aCEvidence && fs.existsSync(path.join(root,r.aCEvidence)),'Missing closure: '+code);
 assert(r.contentPath && fs.existsSync(path.join(root,r.contentPath)),'Missing RichV2: '+code);
 const c=read(r.contentPath);assert(c && Object.keys(c).length>0,'Empty RichV2: '+code);
 assert(c.ContentVersion && typeof c.ContentVersion==='string', 'Missing content version: '+code);
 const visible=(c.Lessons||[]).flatMap(lesson=>lesson.ExplanationParagraphs||[]).join(' ');
 assert(!/im\.kendallhunt\.com|Illustrative Mathematics|\/HS\/teachers\//i.test(visible),
   'Learner-facing source publisher reference is not separated: '+code);

}
const batch=process.argv[2]||'inventory';
if(batch==='final'){
 const evidenceRelative=process.argv[3];
 assert(evidenceRelative,'No authenticated HTTPS evidence file supplied');
 const ev=read(evidenceRelative);
 const exactSha=process.env.G9_CANDIDATE_SHA||process.env.GITHUB_SHA;
 assert(exactSha && /^[a-f0-9]{40}$/.test(exactSha),'No exact candidate SHA');
 assert.equal(ev.candidateSha,exactSha,'Candidate mismatch');
 assert.equal(String(ev.runId),String(process.env.GITHUB_RUN_ID),'GitHub run mismatch');
 assert.equal(ev.lessonsChecked,132);
 assert.equal(ev.renderedAuthenticated,132);
 assert.equal(ev.correctGradesPersisted,132);
 assert.equal(ev.malformedGradesRejectedPersisted,132);
 assert.equal(ev.crossSchoolLessonDenied,true);
 assert.equal(ev.lessons.length,132);
 const attempts=new Set();
 for(let i=0;i<132;i++){
  const expected=ledger.lessons[i];
  const row=ev.lessons[i];
  assert.equal(row.ordinal,i+1,'wrong lesson order in runtime evidence');
  assert.equal(row.lessonCode,expected.lessonCode,'wrong lesson code in runtime evidence');
  assert(row.lessonId && /^[a-f0-9-]{36}$/i.test(row.lessonId),'missing DB lesson identity');
  assert.equal(row.contentRenderedAuthenticated,true,'lesson not rendered over authenticated HTTPS');
  assert.equal(row.correct.status,'CORRECT_PERSISTED','correct answer not persisted');
  assert.equal(row.malformed.status,'MALFORMED_REJECTED_PERSISTED','malformed answer not rejected');
  assert(row.correct.family&&row.malformed.family,'missing exact practice families');
  for(const attempt of [row.correct.attemptId,row.malformed.attemptId]){
   assert(attempt && /^[a-f0-9-]{36}$/i.test(attempt),'missing Practice Attempt UUID');
   assert(!attempts.has(attempt),'duplicate Practice Attempt UUID');
   attempts.add(attempt);
  }
 }
 const ownerDoc='artifacts/curriculum-delivery/US-CCSS-MATH/G9/owner-academic-and-release-authorization-20261105.md';
 assert(fs.existsSync(path.join(root,ownerDoc)),'Missing academic owner sign-off');
 assert(fs.readFileSync(path.join(root,ownerDoc),'utf8').includes('OWNER_APPROVED_BY_EXPLICIT_CHAT_INSTRUCTION'),'Academic owner sign-off absent');
 console.log('G9_STAGE_D_TECHNICAL_EXACT_RUN_PASS: 132 authenticated lessons, 132 correct and 132 malformed persisted, same candidate, owner signoff; no duplicate publisher parity');
}else if(['D1','D2','D3'].includes(batch)){
 const lo={D1:1,D2:45,D3:89}[batch],hi=lo+43;
 for(const r of ledger.lessons.slice(lo-1,hi)) assert.equal(r.aCStatus,'READY_FOR_STAGE_D');
 console.log(batch+' static preflight: lessons '+lo+'-'+hi+'; not runtime verified');
}else console.log('132 unique lessons / 7 units; all RichV2 and closure files present');
