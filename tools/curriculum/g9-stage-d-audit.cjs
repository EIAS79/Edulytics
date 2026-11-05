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
 for(const r of ledger.lessons){
  assert.equal(r.dStatus,'D_VERIFIED',r.lessonCode+': no D proof');
  assert(Number.isInteger(r.sourceElementCount)&&r.sourceElementCount>0,r.lessonCode+': unknown denominator');
  assert.equal(r.verifiedEquivalentElementCount,r.sourceElementCount,r.lessonCode+': no 1:1 evidence');
  assert(r.actionsRunIds.length>0,r.lessonCode+': no GitHub run');
 }
}else if(['D1','D2','D3'].includes(batch)){
 const lo={D1:1,D2:45,D3:89}[batch],hi=lo+43;
 for(const r of ledger.lessons.slice(lo-1,hi)) assert.equal(r.aCStatus,'READY_FOR_STAGE_D');
 console.log(batch+' static preflight: lessons '+lo+'-'+hi+'; not runtime verified');
}else console.log('132 unique lessons / 7 units; all RichV2 and closure files present');
