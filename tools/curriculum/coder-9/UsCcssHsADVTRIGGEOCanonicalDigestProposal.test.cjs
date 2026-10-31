'use strict';
// Coder 9 isolated proposed canonical digest proof. Does not authorize a new body or touch shared allowlists.
// node --test tools/curriculum/coder-9/UsCcssHsADVTRIGGEOCanonicalDigestProposal.test.cjs
const {test}=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const root=path.resolve(__dirname,'../../..');
const read=rel=>JSON.parse(fs.readFileSync(path.join(root,rel),'utf8'));
const manifest=read('artifacts/curriculum-delivery/US-CCSS-MATH/coder-9/ADV-TRIG-GEO-canonical-hash-owner-proposal.json');
const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-hs-adv-trig-geo-phase29-v1.lesson-content-pack.json');
const fields=['title','explanation','keyConceptsAndRules','workedExamples','stepByStepSolutions','commonMistakes','quickSummary'];
const digest=en=>crypto.createHash('sha256').update(fields.map(field=>field+':'+en[field]).join('\n'),'utf8').digest('hex');
test('proposed five hash changes remain blocked by academic and materialization owners',()=>{
 assert.equal(manifest.kind,'CODER9_SHARED_OWNER_ONLY_CANONICAL_BODY_DIGEST_PROPOSAL_NOT_APPROVAL');
 assert.equal(manifest.allowlistWriteAuthorized,false);
 assert.equal(manifest.academicAuthorization,'PENDING_INDEPENDENT_REVIEW');
 assert.equal(manifest.materializationAuthorization,'PENDING_SHARED_OWNER');
 assert.equal(manifest.lessons.length,5);
 assert.equal(pack.lessons.length,5);
 assert.ok(manifest.sharedValidatorConstraints.some(x=>x.includes('len(rows)==8')));
 assert.ok(manifest.sharedValidatorConstraints.some(x=>x.includes('seven candidates')));
});
for(let i=0;i<5;i++){
 const row=manifest.lessons[i];
 test('L0'+(i+1)+' corrected SHA from seven actual English learner fields, historical source unchanged',()=>{
  const lesson=pack.lessons[i],en=lesson.translations.find(x=>x.cultureCode==='en');
  assert.equal(lesson.lessonCode,row.lessonCode);
  assert.equal(lesson.canonicalBodySha256,row.historicalRecordedSha);
  assert.equal(lesson.sourceSha256,row.lockedSourceSha256);
  assert.equal(digest(en),row.proposedRevisedDigestSevenFields);
  assert.notEqual(row.proposedRevisedDigestSevenFields,row.predecessorTranslationDigestSevenFields);
  assert.notEqual(row.predecessorTranslationDigestSevenFields,row.historicalRecordedSha);
  assert.equal(row.approved,false);
 });
}

test('pending predecessor fingerprints match all five current learner fields without implying approval',()=>{
 const transition=read('artifacts/curriculum-delivery/US-CCSS-MATH/coder-9/ADV-TRIG-GEO-content-predecessor-transition.json');
 assert.equal(transition.kind,'CODER9_AUTHORIZATION_SUBMISSION_ONLY_NOT_APPROVAL');
 assert.equal(transition.lessons.length,5);
 for(let i=0;i<5;i++){
  const lesson=pack.lessons[i],en=lesson.translations.find(x=>x.cultureCode==='en');
  const row=transition.lessons[i];
  assert.equal(row.lessonCode,lesson.lessonCode);
  assert.equal(row.baselineSourceSha256,lesson.sourceSha256);
  assert.equal(row.baselineCanonicalBodySha256,lesson.canonicalBodySha256);
  assert.equal(row.revisedTranslationSha256,crypto.createHash('sha256').update(JSON.stringify(en),'utf8').digest('hex'));
  for(const [field,value] of Object.entries(row.fieldDigests)){
   assert.equal(value.afterSha256,crypto.createHash('sha256').update(en[field],'utf8').digest('hex'));
   assert.equal(value.afterLength,[...en[field]].length);
  }
 }
});
