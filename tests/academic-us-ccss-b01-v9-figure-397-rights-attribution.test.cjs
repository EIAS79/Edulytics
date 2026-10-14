'use strict';
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),dir='artifacts/academic-us-ccss/batches/';
const read=p=>JSON.parse(fs.readFileSync(path.join(root,dir+p),'utf8'));
const original=read('B01-v9-original-teacher-figure-dom-asset-map-40.json');
const media=read('B01-v9-original-figure-383-media-byte-provenance.json');
const audit=read('B01-v9-source-figure-397-rights-attribution-triage.json');
const re=/^[a-f0-9]{64}$/;
function check(x){
 assert.equal(x.planId,'EDULYTICS-US-CCSS-UNIFIED-V9');
 assert.equal(x.batchId,'B01');
 assert.equal(x.edition,'IM_K5_FIRST_EDITION_2019_2021_KENDALL_HUNT');
 assert.equal(x.officialFirstEditionLicense,'CC BY 4.0');
 assert.equal(x.officialFirstEditionLicenseUrl,'https://creativecommons.org/licenses/by/4.0/');
 assert.equal(x.academicDecision,'BATCH_NO-GO');
 assert.equal(x.releaseEligible,false);
 assert.equal(x.originalImageAssetCopiesStored,false);
 assert.equal(x.perAssetCommercialUseApproved,false);
 const scope=x.scope;
 assert.equal(scope.batchLessons,500);
 assert.equal(scope.figureLessons,40);
 assert.equal(scope.originalDOMFrames,397);
 assert.equal(scope.distinctAssets,383);
 assert.deepEqual(scope.imageKindCounts,{ImageFile:45,TikzFile:352});
 assert.equal(scope.explicitThirdPartyCreditFrames,5);
 assert.equal(scope.originalFrameWithoutExplicitAttributionParagraph,392);
 assert.deepEqual(scope.explicitCreditLicenseCounts,
 {CC0:2,CC_BY_SA_2_0:1,CC_BY_SA_3_0:1,PIXABAY_PLATFORM_LICENSE_UNVERIFIED_IMAGE_POST_DATE:1});
 for(const field of ['rightsPassedForProductionAssets','sourceMathFigureSemanticsIndependentlyCertified',
 'realStudentFigureBrowserL6Passed','academicLessonsCertifiedByThisAudit'])
  assert.equal(scope[field],0,'No false license/math/L6 passes');
 assert.equal(x.lessons.length,40);
 const byOriginal=new Map(original.rows.map(l=>[l.lessonCode,l]));
 const byMedia=new Map(media.assets.map(a=>[a.assetUrl,a]));
 let frames=0,emptyCredit=0,third=0;
 const kinds={TikzFile:0,ImageFile:0},licenses={};
 const seen=new Set();
 for(const lesson of x.lessons){
  assert(!seen.has(lesson.lessonCode));
  seen.add(lesson.lessonCode);
  const source=byOriginal.get(lesson.lessonCode);
  assert(source,'Original B01 source lesson missing');
  assert.equal(lesson.sourceSemanticSha256,source.sourceSemanticSha256);
  assert.equal(lesson.sourceOriginalTeacherPageUrl,source.publisherTeacherActivityUrl);
  assert(re.test(lesson.originalPageSha256AtRightsAudit));
  assert.equal(lesson.figureFrameCount,source.frames.length);
  assert.equal(lesson.licenseAndMathL6Pass,false);
  assert.equal(lesson.frames.length,source.frames.length);
  const byOrdinal=new Map(source.frames.map(f=>[f.frameOrdinal,f]));
  for(const frame of lesson.frames){
    const prior=byOrdinal.get(frame.frameOrdinal);
    assert(prior,'Missing exact source figure frame');
    assert.equal(prior.publisherRecordId,frame.cmsRecordId);
    assert.equal(prior.publisherAssetKind,frame.publisherAssetKind);
    assert.equal(prior.mediaSrc,frame.originalAssetUrl);
    assert.equal(frame.assetByteSha256,byMedia.get(frame.originalAssetUrl)?.mediaByteSha256);
    assert.equal(frame.exactOriginalTeacherPageSha256,lesson.originalPageSha256AtRightsAudit);
    assert.equal(frame.commercialRightsIndividuallyVerified,false);
    assert.equal(frame.assetAttributionStatementApprovedForProduction,false);
    assert.equal(frame.publisherMathFigureCopiedIntoEdulytics,false);
    assert.equal(frame.academicMathVisualCertified,false);
    assert.equal(frame.sourceAdjacentTeacherMathTaskVerified,false);
    assert.equal(frame.studentL6AccessibilityAndRenderingVerified,false);
    const license=frame.explicitSourceLicenseCategory;
    licenses[license]=(licenses[license]||0)+1;
    kinds[frame.publisherAssetKind]++;
    if(frame.attributionTextPresentInOriginalFigureElement){
      third++;
      assert.equal(frame.publisherAssetKind,'ImageFile');
      assert(frame.originalPublisherAttributionStatement.startsWith('Attribution: '));
      assert(frame.originalPublisherAttributionSourceUrl.startsWith('https://'));
      assert.notEqual(license,'NO_EXPLICIT_PER_FRAME_CREDIT_FOUND');
    }else{
      emptyCredit++;
      assert.equal(frame.originalPublisherAttributionStatement,null);
      assert.equal(frame.originalPublisherAttributionSourceUrl,null);
      assert.equal(license,'NO_EXPLICIT_PER_FRAME_CREDIT_FOUND');
    }
    frames++;
  }
 }
 assert.equal(seen.size,40);
 assert.equal(frames,397);assert.equal(emptyCredit,392);assert.equal(third,5);
 assert.deepEqual(kinds,{ImageFile:45,TikzFile:352});
 assert.deepEqual(licenses,{CC0:2,CC_BY_SA_2_0:1,CC_BY_SA_3_0:1,
 PIXABAY_PLATFORM_LICENSE_UNVERIFIED_IMAGE_POST_DATE:1,NO_EXPLICIT_PER_FRAME_CREDIT_FOUND:392});
 assert.equal(x.rightsInterpretation.noExplicitPerFigureAttribution,
 'NOT_EVIDENCE_OF_PUBLIC_DOMAIN_OR_FIRST_PARTY_OWNERSHIP');
 return true;
}
test('All 397 original B01 image-frame attribution records link real CMS SHA and separate third-party licenses without unjustified rights claims',()=>assert(check(audit)));
test('No license, L6 or academic clearance can be inferred from an absent publisher credit',()=>{
 const mutations=[
  x=>x.perAssetCommercialUseApproved=true,
  x=>x.scope.rightsPassedForProductionAssets=383,
  x=>x.scope.sourceMathFigureSemanticsIndependentlyCertified=397,
  x=>x.scope.realStudentFigureBrowserL6Passed=40,
  x=>x.academicDecision='BATCH_PASS',
  x=>x.lessons[0].frames[0].commercialRightsIndividuallyVerified=true,
  x=>x.lessons[0].frames[0].assetByteSha256='0'.repeat(64),
  x=>x.lessons[0].frames.pop(),
  x=>x.lessons.pop(),
  x=>x.officialFirstEditionLicense='CC BY-NC 4.0',
  x=>x.scope.explicitThirdPartyCreditFrames=0
 ];
 for(const fn of mutations){
  const copy=structuredClone(audit);fn(copy);
  assert.throws(()=>check(copy));
 }
});