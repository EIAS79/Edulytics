'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs');
const read=p=>JSON.parse(fs.readFileSync(p,'utf8'));
const a=()=>read('artifacts/academic-us-ccss/batches/B01-v9-identified-third-party-image-rights.json');
const m=()=>read('artifacts/academic-us-ccss/batches/B01-v9-original-teacher-figure-dom-asset-map-40.json');
test('Identified third-party rights belong to exact real publisher image references',()=>{
 const rights=a(),map=m(),all=map.rows.flatMap(r=>r.frames.map(f=>({lessonCode:r.lessonCode,...f})));
 assert.equal(rights.batchId,'B01');
 assert.equal(rights.entries.length,5);
 for(const x of rights.entries){
  const frame=all.find(f=>f.lessonCode===x.lessonCode&&f.publisherRecordId===x.publisherRecordId);
  assert.ok(frame,'Missing original figure '+x.publisherRecordId);
  assert.equal(frame.mediaSrc,x.mediaUrl);
  assert.equal(frame.publisherAssetKind,'ImageFile');
  assert.equal(frame.rightsToRepublishVerified,false,
   'The historical DOM map itself does not clear this asset');
 }
});
test('Actual third-party authors, attribution licenses, CC0 and unknown license remain distinct',()=>{
 const rights=a().entries;
 assert.deepEqual(rights.filter(x=>x.status==='LICENSE_CONFIRMED_ATTRIBUTION_REQUIRED_IF_COPIED')
  .map(x=>x.publisherRecordId),['38252','38253']);
 assert.equal(rights.filter(x=>x.license==='CC0').length,2);
 assert.deepEqual(rights.filter(x=>x.status==='THIRD_PARTY_SOURCE_CREDIT_ASSET_IDENTITY_UNVERIFIED')
  .map(x=>x.publisherRecordId),['39878']);
 assert.ok(rights.filter(x=>x.status.includes('ATTRIBUTION_REQUIRED'))
  .every(x=>x.licenseEvidence&&x.actionForNoCreditsUI.startsWith('USE_INDEPENDENT')));
 assert.ok(!rights.some(x=>x.status==='SAFE_TO_COPY_ANYWHERE'));
});
