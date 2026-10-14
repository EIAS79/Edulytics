'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const {screen}=require('../tools/us-ccss-b01-original-authorship-screen.cjs');
const mk=()=>{
 const lessons=Array.from({length:500},(_,i)=>({lessonCode:'PED:B01:T'+String(i).padStart(3,'0'),sourceSemanticSha256:'a'.repeat(64)}));
 const packRows=lessons.map((entry,i)=>({
   lessonCode:entry.lessonCode,
   canonicalBodySha256:'b'.repeat(64),
   translations:[{cultureCode:'en',explanation:i===0?
    'In previous lessons, students studied shapes.':i===1?
    'We invented this new number puzzle ourselves.':'Count the number of dots.',
    workedExamples:i===2?'Example: A Caption: B Caption: C':'Draw 5 apples and then count them.',
    quickSummary:'Review.',stepByStepSolutions:'Count once.'}]
 }));
 return {manifest:{batchId:'B01',expectedLessonCount:500,lessons},
  packs:[0,1,2,3].map(i=>({packCode:'US-CCSS-MATH',versionCode:'CCSSM-2010',lessons:packRows.slice(i*125,(i+1)*125)}))};
};
test('Screens exactly 500 as one batch and always refuses legal or academic PASS',()=>{
 const x=screen(mk());
 assert.equal(x.coverage.found,500);
 assert.equal(x.flagged+x.unflagged,500);
 assert.equal(x.batchCertification,'NOT_CERTIFIED');
 assert.equal(x.removePublicAttributionAuthorized,false);
 assert.equal(x.rows[0].originalAuthorshipReview,'CANDIDATE_NOT_CLEARED');
 assert.equal(x.rows[1].originalAuthorshipReview,'NO_HEURISTIC_CUE_NOT_RIGHTS_PROOF');
 assert.equal(x.rows[1].licenceDisposition,'UNDECIDED');
 assert.equal(x.rows[0].sourceTextCompared,'NO_SOURCE_TEXT_IN_THIS_SCAN');
});
test('A heuristic cue is not treated as proof that copying took place',()=>{
 const x=screen(mk());
 assert.equal(x.rows[0].cues[0].disposition,'REVIEW_REQUIRED_NOT_COPY_PROOF');
 assert.equal(x.rows[0].licenceDisposition,'UNDECIDED');
});
test('Missing, duplicated or fingerprint-drifted rows are rejected',()=>{
 const x=mk();x.manifest.lessons.pop();
 assert.throws(()=>screen(x),/Exactly 500/);
 const d=mk();d.packs[0].lessons[1].lessonCode=d.packs[0].lessons[0].lessonCode;
 assert.throws(()=>screen(d),/Duplicate learner lesson/);
 const e=mk();e.manifest.lessons[20].sourceSemanticSha256='';
 assert.throws(()=>screen(e),/Invalid source semantic SHA/);
});
