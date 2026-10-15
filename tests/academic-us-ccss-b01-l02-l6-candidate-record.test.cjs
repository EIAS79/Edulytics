'use strict';
const {test}=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const file='artifacts/academic-us-ccss/certification-evidence/B01-G1-U01-L02-authenticated-l6-candidate-20261015.json';
const sha=s=>typeof s==='string'&&/^[a-f0-9]{64}$/.test(s);
test('Actual authenticated L02 witness binds to original manifest and screened body without awarding PASS',()=>{
 const a=read(file),m=read('artifacts/academic-us-ccss/batches/B01-lesson-manifest.json'),
  ledger=read('artifacts/academic-us-ccss/04-lesson-certification-records.json');
 const code='PED:US-CCSS-MATH:G1:U01:L02';
 const src=m.lessons.filter(x=>x.lessonCode===code),r=ledger.records.filter(x=>x.lessonCode===code);
 assert.equal(src.length,1);assert.equal(r.length,1);
 assert.equal(a.schemaVersion,1);
 assert.equal(a.lessonCode,code);
 assert.equal(a.sourceSemanticSha256,src[0].sourceSemanticSha256);
 assert.equal(a.effectiveContentSha256,r[0].contentSha256);
 assert.equal(a.originalTeacherPreparationUrl,src[0].sourceUrl);
 assert.equal(r[0].academicDecision,'NOT_CERTIFIED');
 assert.equal(r[0].status,'AUTOMATED_SCREENED');
 assert.equal(r[0].gates.L6.status,'NOT_STARTED');
 assert.equal(a.academicDecision,'NOT_CERTIFIED');
 assert.equal(a.approvedL1L6,false);assert.equal(a.approvedEightDimensions,false);
 assert.equal(a.batchReleaseEligible,false);
 assert.equal(a.decision,'REVIEWABLE_L6_CANDIDATE_NOT_ACADEMIC_CERTIFICATION');
 assert.equal(a.independentVerificationDecision,'BROWSER_EVIDENCE_REVIEWABLE_NOT_ACADEMIC_L6_PASS');
 assert.match(a.githubRunUrl,/^https:\/\/github\.com\/EIAS79\/Edulytics\/actions\/runs\/\d+$/);
 assert.equal(a.githubRunId,a.githubRunUrl.split('/').at(-1));
 assert.match(a.checkedOutCommit,/^[0-9a-f]{40}$/);
 assert(sha(a.originalLessonBodySha256));
 assert.deepEqual(a.screenshots.desktop.dotCounts,a.screenshots.mobile.dotCounts);
 assert.deepEqual([a.screenshots.desktop.width,a.screenshots.desktop.height],[1440,900]);
 assert.deepEqual([a.screenshots.mobile.width,a.screenshots.mobile.height],[390,844]);
 assert(sha(a.screenshots.desktop.sha256));
 assert(sha(a.screenshots.mobile.sha256));
 assert.deepEqual(a.verifiedScoring.map(x=>[x.case,x.persistedCorrectItems,x.persistedScore]),
 [['correct',3,3],['wrong',0,0]]);
 assert.equal(a.exactSourceFamilies.length,3);
 assert.equal(new Set(a.exactSourceFamilies).size,3);
 assert.deepEqual(Object.keys(a.artifactIds).sort(),
 ['authenticatedDesktopMobileScreenshots','authenticatedSourceShaBrowserGrading','independentL6Verification']);
 assert(a.pendingBeforeCertification.length>=4);
 assert(a.pendingBeforeCertification.some(x=>x.includes('CI artifacts expire')));
});
