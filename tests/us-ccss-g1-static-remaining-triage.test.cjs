'use strict';
const {test}=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const j=read('artifacts/curriculum-delivery/US-CCSS-MATH/G1/G1-L71-L146-STATIC-SOURCE-CONTENT-TRIAGE.json');
const m=read('artifacts/curriculum-delivery/US-CCSS-MATH/G1/grade-manifest.json');
test('76 exact remaining new ordinals statically triaged without claiming approval',()=>{
 assert.equal(j.rows,76);assert.equal(j.records.length,76);
 assert.deepEqual(j.records.map(x=>x.lessonCode),m.lessons.slice(70).map(x=>x.lessonCode));
 assert.equal(j.records[0].ordinal,71);assert.equal(j.records[75].ordinal,146);
 assert.ok(j.flaggedRows>=1);
 for(const r of j.records){
   assert.equal(r.academicAccepted,false);
   assert.equal(r.originalStudentTasksFullyVerified,false);
   assert.equal(r.practiceVerified,false);
   assert.equal(r.authenticatedRenderingVerified,false);
   assert.equal(r.status,'PENDING_IMPLEMENTATION_AND_REVIEW');
 }
});
