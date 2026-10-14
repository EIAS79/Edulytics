'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const root=path.resolve(__dirname,'..');
const base='artifacts/academic-us-ccss/batches/';
const originalPath=path.join(root,base+'B01-v9-original-teacher-figure-dom-asset-map-40.json');
const original=JSON.parse(fs.readFileSync(originalPath,'utf8'));
const observed=JSON.parse(fs.readFileSync(path.join(root,base+'B01-v9-original-figure-383-media-byte-provenance.json'),'utf8'));
const sha=/^[a-f0-9]{64}$/;
const recognized=['SVG','PNG','JPEG'];
function verify(j){
 assert.equal(j.planId,'EDULYTICS-US-CCSS-UNIFIED-V9');
 assert.equal(j.batchId,'B01');
 assert.equal(j.status,'PROVENANCE_AND_BYTE_INTEGRITY_ONLY_NO_MATH_SEMANTICS_RIGHTS_OR_L6_PASS');
 assert.equal(j.academicDecision,'BATCH_NO-GO');
 assert.equal(j.releaseEligible,false);
 assert.equal(j.scope.batchManifestLessons,500);
 assert.equal(j.scope.figureSourceLessons,40);
 assert.equal(j.scope.sourceOriginalFigureFrames,397);
 assert.equal(j.scope.uniquePublisherAssetURLs,383);
 assert.equal(j.scope.HTTP200CompleteMediaFilesRead,383);
 assert.equal(j.scope.distinctContentByteSha256,361);
 assert.equal(j.scope.validIntrinsicDimensions,383);
 assert.equal(j.scope.mediaDownloadErrors,0);
 assert.deepEqual(j.scope.mediaFormatCounts,{JPEG:3,PNG:34,SVG:346});
 for(const prop of ['originalMathVisualsIndependentlyVerified','rightsVerifiedAssets',
 'realStudentBrowserFigureMatchesVerified','fullL6LessonsVerified','newAcademicLessonCertifications']){
   assert.equal(j.scope[prop],0,'Unsupported certification: '+prop);
 }
 assert.equal(j.originalMapFileSha256,crypto.createHash('sha256')
   .update(fs.readFileSync(originalPath)).digest('hex'),'Original 40-lesson source map changed');
 assert.equal(j.lessons.length,40);assert.equal(j.assets.length,383);
 assert.equal(original.rows.length,40);
 const assetByUrl=new Map(),hashes=new Set(),formats=new Map();
 let bytes=0;
 for(const a of j.assets){
  assert(a.assetUrl.startsWith('https://cms-assets.illustrativemathematics.org/'));
  assert(!assetByUrl.has(a.assetUrl),'Duplicate URL');
  assetByUrl.set(a.assetUrl,a);
  assert.equal(a.httpStatus,200);
  assert.equal(a.individualImageSourceAttributionVerified,false);
  assert.equal(a.individualAssetRepublishingRightVerified,false);
  assert.equal(a.mathematicalImageMeaningIndependentlyVerified,false);
  assert(recognized.includes(a.sniffedFormat));
  assert(sha.test(a.mediaByteSha256));
  assert(a.contentByteLength>0);
  assert(a.dimensions.width>0&&a.dimensions.height>0);
  hashes.add(a.mediaByteSha256);bytes+=a.contentByteLength;
  formats.set(a.sniffedFormat,(formats.get(a.sniffedFormat)||0)+1);
 }
 assert.equal(assetByUrl.size,383);assert.equal(hashes.size,361);
 assert.equal(bytes,j.scope.byteLengthCombined);
 for(const [format,count] of Object.entries(j.scope.mediaFormatCounts))
   assert.equal(formats.get(format),count);
 const known=new Set(),bySource=new Map(original.rows.map(x=>[x.lessonCode,x]));
 let linkedFrames=0;
 for(const l of j.lessons){
  assert(!known.has(l.lessonCode),'Duplicate source lesson');
  known.add(l.lessonCode);
  const s=bySource.get(l.lessonCode);
  assert(s,'Extra-source lesson: '+l.lessonCode);
  assert.equal(l.ordinal,s.ordinal);
  assert.equal(l.sourceSemanticSha256,s.sourceSemanticSha256);
  assert.equal(l.sourceInventoryFileSha256,s.preparationLocalFileInventorySha256);
  assert.equal(l.officialTeacherActivityUrl,s.publisherTeacherActivityUrl);
  assert.equal(l.originalFigureFrames,s.frames.length);
  assert.equal(l.verifiedHttp200Frames,s.frames.length);
  assert.equal(l.allFrameBytesIntegrityHashRecorded,true);
  assert.equal(l.allFrameImageDimensionsPositive,true);
  for(const f of ['mathSemanticsVerified','republicationRightsVerifiedPerAsset',
    'visualMatchToLiveEdulyticsVerified','L6BrowserPass','lessonAcademicPass'])
      assert.equal(l[f],false,'False academic / legal PASS for '+l.lessonCode);
  assert.equal(l.frameAssets.length,s.frames.length);
  assert.equal(l.uniqueAssetUrlsInLesson,new Set(s.frames.map(x=>x.mediaSrc)).size);
  const sourceByOrdinal=new Map(s.frames.map(x=>[x.frameOrdinal,x]));
  for(const a of l.frameAssets){
    const o=sourceByOrdinal.get(a.frameOrdinal);
    assert(o&&a.assetUrl===o.mediaSrc&&a.publisherRecordId===o.publisherRecordId);
    const media=assetByUrl.get(a.assetUrl);
    assert(media,'Frame URL no actual bytes');
    assert.equal(a.byteSha256,media.mediaByteSha256);
    assert.equal(a.magicFormat,media.sniffedFormat);
    assert.equal(a.bytes,media.contentByteLength);
    assert.equal(a.width,media.dimensions.width);
    assert.equal(a.height,media.dimensions.height);
    linkedFrames++;
  }
 }
 assert.equal(known.size,40);assert.equal(linkedFrames,397);
 assert.equal(j.scope.sourceOriginalFigureFrames,linkedFrames);
 assert(!('imageBinary' in j));
 return true;
}
test('All original 397 teacher figure frames resolve 383 HTTP200 image assets: SHA, size, intrinsic dimensions, 40 lesson provenance',()=>assert.equal(verify(observed),true));
test('Negative gates refuse fake graphic math, asset rights, L6, source drift or missing original frame',()=>{
 for(const mutate of [
   x=>x.scope.originalMathVisualsIndependentlyVerified=40,
   x=>x.scope.rightsVerifiedAssets=383,
   x=>x.scope.fullL6LessonsVerified=40,
   x=>x.academicDecision='BATCH_PASS',
   x=>x.releaseEligible=true,
   x=>x.assets[0].mediaByteSha256='0'.repeat(64), // unbound to frame
   x=>x.assets[0].individualAssetRepublishingRightVerified=true,
   x=>x.lessons[0].frameAssets[0].byteSha256='0'.repeat(64),
   x=>x.lessons[0].mathSemanticsVerified=true,
   x=>x.lessons.pop(),
   x=>x.assets.pop()
 ]){
  const bad=structuredClone(observed);mutate(bad);
  assert.throws(()=>verify(bad));
 }
});