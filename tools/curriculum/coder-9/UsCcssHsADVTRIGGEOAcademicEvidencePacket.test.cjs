'use strict';
// Detects stale/detached five-lesson authorization evidence without altering protected gates.
// Independent code/data checks only; no academic approval, runtime Practice or UAT implied.
const {test}=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const crypto=require('node:crypto');
const root=path.resolve(__dirname,'../../..');
const loc='artifacts/curriculum-delivery/US-CCSS-MATH/coder-9/';
const read=relative=>fs.readFileSync(path.join(root,relative),'utf8');
const json=relative=>JSON.parse(read(relative));
const pack=json('src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-hs-adv-trig-geo-phase29-v1.lesson-content-pack.json');
const proposal=json(loc+'ADV-TRIG-GEO-canonical-hash-owner-proposal.json');
const transition=json(loc+'ADV-TRIG-GEO-content-predecessor-transition.json');
const spec=json(loc+'ADV-TRIG-GEO-parameterized-practice-acceptance.v1.json');
const matrix=read(loc+'ADV-TRIG-GEO-academic-review-decision-matrix-2026-10-29.md');
const sha=s=>crypto.createHash('sha256').update(s,'utf8').digest('hex');
const fields=['title','explanation','keyConceptsAndRules','workedExamples','stepByStepSolutions','commonMistakes','quickSummary'];
const amended=['explanation','keyConceptsAndRules','workedExamples','stepByStepSolutions','commonMistakes','quickSummary'];
test('academic evidence is explicitly proposed and retains source/version guardrails',()=>{
 assert.equal(pack.contentVersion,'phase30-common-core-rich-content-v1');
 assert.equal(proposal.allowlistWriteAuthorized,false);
 assert.equal(proposal.academicAuthorization,'PENDING_INDEPENDENT_REVIEW');
 assert.equal(proposal.materializationAuthorization,'PENDING_SHARED_OWNER');
 assert.equal(transition.kind,'CODER9_AUTHORIZATION_SUBMISSION_ONLY_NOT_APPROVAL');
 assert.match(matrix,/five-body academic transition evidence packet/);
 assert.match(matrix,/changedBodies=5; newlyApproved=0/);
 assert.match(matrix,/OFFICIAL.*NOT yet been independently confirmed PASS/i);
 assert.equal(pack.lessons.length,5);
 assert.equal(proposal.lessons.length,5);
 assert.equal(transition.lessons.length,5);
 assert.equal(spec.lessons.length,5);
 assert.equal(spec.lessons.reduce((sum,lesson)=>sum+lesson.cases.length,0),28);
 assert.equal(spec.meta.liveGeneratorRegistered,false);
});
for(let i=0;i<5;i++){
 test('L0'+(i+1)+' current candidate, field diffs, source identity and reviewer gate are all linked',()=>{
  const lesson=pack.lessons[i],candidate=proposal.lessons[i],diff=transition.lessons[i];
  const english=lesson.translations.find(t=>t.cultureCode==='en');
  const heading='### L0'+(i+1)+' — ';
  const start=matrix.indexOf(heading);
  assert.ok(start>=0,'missing per-lesson evidence section: '+heading);
  const rest=matrix.slice(start);
  const end=rest.indexOf('\n### ',4);
  const section=end<0?rest:rest.slice(0,end);
  assert.equal(candidate.lessonCode,lesson.lessonCode);
  assert.equal(diff.lessonCode,lesson.lessonCode);
  assert.equal(candidate.lockedSourceSha256,lesson.sourceSha256);
  assert.equal(candidate.historicalRecordedSha,lesson.canonicalBodySha256);
  assert.equal(diff.baselineCanonicalBodySha256,lesson.canonicalBodySha256);
  assert.equal(diff.baselineSourceSha256,lesson.sourceSha256);
  assert.equal(candidate.approved,false);
  assert.equal(diff.sameSourceSha,true);
  assert.equal(diff.sameFormalTargets,true);
  assert.equal(diff.sameCode,true);
  const current=sha(fields.map(field=>field+':'+english[field]).join('\n'));
  assert.equal(candidate.proposedRevisedDigestSevenFields,current);
  assert.notEqual(current,candidate.predecessorTranslationDigestSevenFields);
  for(const value of [lesson.lessonCode,lesson.sourceSha256,lesson.canonicalBodySha256,
   candidate.proposedRevisedDigestSevenFields,candidate.predecessorTranslationDigestSevenFields,lesson.sourceUrl]){
    assert.ok(section.includes(value),'evidence section misses '+value);
  }
  assert.match(section,/\*\*PENDING\*\*/);
  assert.match(section,/Human-scored evidence/);
  assert.match(section,/Open decision/);
  assert.match(section,/metadata also unchanged/);
  assert.deepEqual(diff.changedFields,amended);
  assert.equal(Object.keys(diff.fieldDigests).length,6);
  for(const key of amended){
   const digest=diff.fieldDigests[key];
   assert.equal(digest.afterSha256,sha(english[key]));
   assert.equal(digest.afterLength,[...english[key]].length);
   assert.ok(section.includes(key),'field diff not included in section: '+key);
   assert.match(digest.beforeSha256,/^[a-f0-9]{64}$/);
  }
 });
}
