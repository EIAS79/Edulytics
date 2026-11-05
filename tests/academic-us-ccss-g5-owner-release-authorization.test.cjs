'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const crypto=require('node:crypto');
const cp=require('node:child_process');
const root=path.resolve(__dirname,'..');
const artifact=JSON.parse(fs.readFileSync(path.join(root,
 'artifacts/curriculum-delivery/US-CCSS-MATH/G5/grade5-owner-academic-release-authorization-20261103.json'),'utf8'));
const review=JSON.parse(fs.readFileSync(path.join(root,
 'artifacts/curriculum-delivery/US-CCSS-MATH/G5/g5-2021-second-pass-review-ledger-v1.json'),'utf8'));

test('owner academic and deployment authorization is explicit and exact-grade scoped',()=>{
 assert.equal(artifact.schemaVersion,1);
 assert.equal(artifact.recordType,'GRADE5_PROJECT_OWNER_ACADEMIC_APPROVAL_AND_RELEASE_AUTHORIZATION');
 assert.equal(artifact.scope.repository,'EIAS79/Edulytics');
 assert.equal(artifact.scope.pullRequest,596);
 assert.equal(artifact.scope.units,8);
 assert.equal(artifact.scope.lessons,148);
 assert.equal(artifact.ownerDecision.academicApprovalOfEdulyticsAuthoredMathematics,true);
 assert.equal(artifact.ownerDecision.productionPublicationAuthorized,true);
 assert.equal(artifact.ownerDecision.originalMathContentAuthorshipAssertedByOwner,true);
 assert.equal(artifact.decisionSeparation.academicOwnerApproval,'GRANTED');
 assert.equal(artifact.decisionSeparation.ownerReleasePermission,'GRANTED');
 assert.equal(artifact.notCryptographicOwnerSignature,true);
 assert.match(artifact.publicAuditNote,/^https:\/\/github\.com\/EIAS79\/Edulytics\/pull\/596#issuecomment-\d+$/);
 assert.equal(review.reviewComplete2021Count,148);
 assert.equal(review.existingStageDReadyCount,148);
 assert.equal(review.lessons.length,148);
 assert.ok(review.lessons.every(x=>x.reviewState==='CLOSED'&&x.REVIEW_COMPLETE_2021===true&&
   x.readyForStageDPreviousPass===true));
});

test('owner decision is pinned to authored content bytes, not a blanket publisher-source approval',()=>{
 const p=artifact.authorizedContentPack.path;
 assert.equal(p,'src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g5-phase29-v1.lesson-content-pack.json');
 const bytes=fs.readFileSync(path.join(root,p));
 const actual=crypto.createHash('sha1').update('blob '+bytes.length+'\0').update(bytes).digest('hex');
 assert.equal(actual,artifact.authorizedContentPack.gitBlobSha);
 for(const value of Object.values(artifact.nonClaims))assert.equal(value,false);
 assert.equal(artifact.decisionSeparation.independentSourceParity,'UNVERIFIED_NOT_CLAIMED');
 assert.equal(artifact.decisionSeparation.thirdPartyAssetLicenseCompliance,'NOT_YET_INDEPENDENTLY_AUDITED');
 assert.equal(artifact.decisionSeparation.stageDFinalCI,'MUST_PROVE_ON_CANDIDATE_SHA');
 assert.equal(artifact.decisionSeparation.actualDeploy,'NOT_EXECUTED');
 assert.match(artifact.rightsAndAttributionPolicy,/attribution must be met/);
});

test('Stage D reports owner academic signoff separately from unverified publisher parity',()=>{
 const cmd=cp.spawnSync(process.execPath,['tools/curriculum/g5-stage-d-diagnostic.cjs','source-evidence'],{cwd:root,encoding:'utf8',timeout:30000});
 assert.equal(cmd.status,2);
 const result=JSON.parse(fs.readFileSync(path.join(root,'stage-d-results/grade5-source-evidence.json'),'utf8'));
 assert.equal(result.ownerAcademicApprovedOriginalLessons,148);
 assert.equal(result.ownerReleasePermissionGranted,true);
 assert.equal(result.reviewed2021SecondPassLessons,148);
 assert.equal(result.independentlySourceApprovedLessons,0);
 assert.equal(result.blockedLessons,148);
 assert.ok(result.rows.every(x=>x.ownerContentApproved===true));
 assert.ok(result.rows.every(x=>x.academicApproval===
   'OWNER_ACADEMICALLY_APPROVED_EDULYTICS_ORIGINAL_CONTENT_SOURCE_PARITY_UNVERIFIED'));
});
