'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const crypto=require('node:crypto');
const root=path.resolve(__dirname,'../..');
const resource=p=>fs.readFileSync(path.join(root,p));
const read=p=>JSON.parse(resource(p).toString('utf8'));
const pathBlueprint='src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/us-ccss-math-hs-traditional-algebra2-im1e.lesson-blueprint.json';
const pathPack='src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-hs-alg2-phase29-v1.lesson-content-pack.json';
const pathMappings='src/Edulytics.Core/Mathematics/Curriculum/lesson-skill-mappings.v1.json';
const blueprint=read(pathBlueprint),pack=read(pathPack),map=read(pathMappings);
const trace=read('artifacts/curriculum-delivery/US-CCSS-MATH/coder-7/algebra2-120-practice-verification-trace.json');
const l18=read('artifacts/curriculum-delivery/US-CCSS-MATH/coder-7/U02-L18-PRACTICE-RUBRIC-AND-VERSION-TRANSITION.json');
const githubBlobSha=b=>crypto.createHash('sha1').update('blob '+b.length+'\0').update(b).digest('hex');

test('120-row trace precisely matches course blueprint, source blobs and static Practice mapping',()=>{
  assert.equal(blueprint.Lessons.length,120);
  assert.equal(pack.lessons.length,120);
  assert.equal(trace.lessons.length,120);
  assert.equal(trace.sourceBlueprintBlobSha,githubBlobSha(resource(pathBlueprint)));
  assert.equal(trace.sourceContentBlobSha,githubBlobSha(resource(pathPack)));
  assert.equal(trace.sourcePracticeMappingsBlobSha,githubBlobSha(resource(pathMappings)));
  const explicit=new Map(map.mappings.filter(x=>x.lessonCode?.startsWith('PED:US-CCSS-MATH:HS:TRAD:ALG2:IM-1E:')).map(x=>[x.lessonCode,x]));
  assert.equal(explicit.size,2);
  for(let i=0;i<120;i++){
    const source=blueprint.Lessons[i],row=trace.lessons[i],packRow=pack.lessons[i];
    assert.equal(row.ordinal,i+1);
    assert.equal(row.lessonCode,source.LessonCode);
    assert.equal(packRow.lessonCode,row.lessonCode);
    assert.deepEqual(row.staticExplicitFamilies,explicit.get(row.lessonCode)?.allowedQuestionFamilies||[]);
    assert.equal(row.stageC,'NOT_ACCEPTED');
    assert.equal(row.generationVerification,'NOT_RUN');
    assert.equal(row.authenticatedStudentAttempt,'NOT_RUN');
    assert.equal(row.scorePersistence,'NOT_RUN');
    assert.equal(row.teacherReviewAndGradebook,'NOT_RUN');
  }
});

test('explicit metadata at U02:L17 and U05:L10 is not misreported as runtime Practice',()=>{
  assert.equal(trace.explicitMappingCount,2);
  const l17=trace.lessons.find(x=>x.lessonCode.endsWith(':U02:L17'));
  const l10=trace.lessons.find(x=>x.lessonCode.endsWith(':U05:L10'));
  const l18row=trace.lessons.find(x=>x.lessonCode.endsWith(':U02:L18'));
  assert.equal(l17.staticExplicitFamilies.length,5);
  assert.equal(l10.staticExplicitFamilies.length,2);
  assert.equal(l18row.staticExplicitFamilies.length,0);
  for(const row of [l17,l10,l18row]){
    assert.equal(row.resolvedFamilyIds.length,0);
    assert.equal(row.stageC,'NOT_ACCEPTED');
    assert.equal(row.tenantIsolation,'NOT_RUN');
  }
});

test('U02:L18 version proposal gives independently recomputable successor, not forged approval',()=>{
  const lesson=pack.lessons.find(x=>x.lessonCode===l18.lessonCode);
  const t=lesson.translations.find(x=>x.cultureCode==='en');
  const fields=['title','explanation','keyConceptsAndRules','workedExamples',
    'stepByStepSolutions','commonMistakes','quickSummary'];
  const digest=crypto.createHash('sha256').update(fields.map(k=>k+':'+t[k]).join('\n'),'utf8').digest('hex');
  assert.equal(lesson.canonicalBodySha256,l18.hashTransition.historicalApprovedCanonicalBodySha256);
  assert.equal(digest,l18.hashTransition.recomputedAuthoredCanonicalBodySha256);
  assert.notEqual(digest,lesson.canonicalBodySha256);
  assert.equal(l18.academicApproval,'NOT_VERIFIED');
  assert.equal(l18.hashTransition.approvalRegisterChange,'NOT_AUTHORIZED_ON_CODER7_BRANCH');
  assert.equal(l18.sourceActivities.length,3);
});

test('publisher formula matching and first-party visual are correctly scoped and accessible',()=>{
  const a=l18.sourceActivities.find(x=>x.id==='18.3');
  assert.deepEqual(a.matching,{a:'D',b:'B',c:'A',d:'C',e:'B',f:'D',g:'C',h:'A'});
  const file=l18.originalVisualCandidate,svg=resource(file).toString('utf8');
  assert.match(svg,/<svg\s/);
  assert.match(svg,/<title id="title">/);
  assert.match(svg,/<desc id="description">/);
  assert.match(svg,/aria-labelledby="title description"/);
  assert.match(svg,/Original Edulytiks draft, not deployed/);
  assert.equal((svg.match(/<g transform=/g)||[]).length,4);
  assert.equal((svg.match(/<path /g)||[]).length,8);
  assert.doesNotMatch(svg,/<script|<image|https?:\/\//i);
  assert.equal(l18.visualIntegration,'NOT_IMPLEMENTED_IN_RENDERER');
});

test('human-scored task proposals cannot be mistaken for authenticated grades',()=>{
  assert.equal(trace.effectiveFamilyProbeCount,0);
  assert.equal(trace.status,'STATIC_METADATA_AUDIT_NOT_RUNTIME_COVERAGE');
  assert.equal(trace.release,'NO_GO');
  assert.ok(trace.lessons.every(x=>x.sourceActivityHumanJudgementAuditStatus==='NOT_YET_DECIDED_BY_REVIEWED_RUBRIC'));
  assert.equal(l18.gradingPersistence,'NOT_VERIFIED');
  assert.match(l18.humanScoringGuidance,/teacher|review/i);
});
