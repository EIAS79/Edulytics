'use strict';
const test=require('node:test'),assert=require('node:assert/strict');
const crypto=require('node:crypto'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const packPath='src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-hs-alg2-phase29-v1.lesson-content-pack.json';
const packBytes=fs.readFileSync(path.join(root,packPath));
const pack=JSON.parse(packBytes);
const audit=JSON.parse(fs.readFileSync(path.join(root,'artifacts/curriculum-delivery/US-CCSS-MATH/coder-7/algebra2-120-canonical-body-lineage-audit.json'),'utf8'));
const fields=['title','explanation','keyConceptsAndRules','workedExamples','stepByStepSolutions','commonMistakes','quickSummary'];
const digest=tr=>crypto.createHash('sha256').update(fields.map(k=>k+':'+tr[k]).join('\n'),'utf8').digest('hex');
const blob=b=>crypto.createHash('sha1').update('blob '+b.length+'\0').update(b).digest('hex');
test('all 120 baseline/current body digests are independently reproducible with source blob binding',()=>{
 assert.equal(audit.sourceContentBlobSha,blob(packBytes));
 assert.equal(audit.sourceContentPackPath,packPath);
 assert.equal(audit.reviewType,'READ_ONLY_CANONICAL_BODY_DIGEST_COMPARISON_NOT_APPROVAL');
 assert.equal(pack.lessons.length,120);assert.equal(audit.lessons.length,120);
 let changed=0,unchanged=0;
 for(let i=0;i<120;i++){
   const lesson=pack.lessons[i],row=audit.lessons[i];
   assert.equal(row.ordinal,i+1);assert.equal(row.lessonCode,lesson.lessonCode);
   assert.equal(row.predecessorCanonicalBodySha256,lesson.canonicalBodySha256);
   assert.equal(row.sourceSha256,lesson.sourceSha256);
   const t=lesson.translations.find(x=>x.cultureCode==='en');
   assert.equal(row.authoredCandidateBodySha256,digest(t));
   const same=row.authoredCandidateBodySha256===row.predecessorCanonicalBodySha256;
   assert.equal(row.lineageState,same?'UNCHANGED_BASELINE_BODY':'CHANGED_BODY_SUCCESSOR_NOT_APPROVED');
   assert.equal(row.academicAcceptance,'NOT_VERIFIED');assert.equal(row.runtimeAcceptance,'NOT_VERIFIED');
   same?unchanged++:changed++;
 }
 assert.deepEqual([changed,unchanged],[118,2]);
 assert.equal(audit.counts.changedBodiesRequiringSuccessorDecision,changed);
 assert.equal(audit.counts.unchangedReusedBodies,unchanged);
 for(const stage of ['academicallyAccepted','renderedAccepted','practiceAccepted','humanAccepted'])assert.equal(audit.counts[stage],0);
 assert.equal(audit.release,'NO_GO');assert.equal(audit.sharedVersionOwnerStatus,'PENDING');
});
test('two unchanged authored predecessors remain U02 L17 and U05 L10, not academic signoffs',()=>{
 const same=audit.lessons.filter(x=>x.lineageState==='UNCHANGED_BASELINE_BODY').map(x=>x.lessonCode.slice(-7)).sort();
 assert.deepEqual(same,['U02:L17','U05:L10']);
 const l18=audit.lessons.find(x=>x.lessonCode.endsWith('U02:L18'));
 const fixture=JSON.parse(fs.readFileSync(path.join(root,'artifacts/curriculum-delivery/US-CCSS-MATH/coder-7/U02-L18-PRACTICE-RUBRIC-AND-VERSION-TRANSITION.json'),'utf8'));
 assert.equal(l18.predecessorCanonicalBodySha256,fixture.hashTransition.historicalApprovedCanonicalBodySha256);
 assert.equal(l18.authoredCandidateBodySha256,fixture.hashTransition.recomputedAuthoredCanonicalBodySha256);
 assert.equal(fixture.academicApproval,'NOT_VERIFIED');
});
