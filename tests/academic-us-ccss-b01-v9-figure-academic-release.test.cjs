'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs');
const {verifyDomLinks}=require('../tools/us-ccss-b01-v9-source-figure-dom-linkage-gate.cjs');
const {verifyFigureAcademicRelease,exactFlow}=require('../tools/us-ccss-b01-v9-figure-academic-release.cjs');
const load=f=>JSON.parse(fs.readFileSync('artifacts/academic-us-ccss/batches/'+f,'utf8'));
const product='1'.repeat(40),content='b'.repeat(64),image='c'.repeat(64);
function fixture(){
 const inputs={manifest:load('B01-lesson-manifest.json'),
  index:load('B01-book-source-reference-index-500.json'),
  activityMap:load('B01-v9-original-teacher-figure-dom-asset-map-40.json'),
  figureContext:load('B01-v9-source-lesson-activity-figure-context-40.json')};
 const domReport=verifyDomLinks(inputs),ledger={releaseCandidateSha:product,records:[]};
 const verification={schemaVersion:1,
  kind:'B01_V9_INDEPENDENT_FIGURE_MATH_RIGHTS_L6_EVIDENCE',
  batchId:'B01',releaseCandidateSha:product,records:[]};
 for(const s of inputs.activityMap.rows){
  const L1={status:'PASS',sourceSha:product,evidenceIds:[]},
   L4={status:'PASS',sourceSha:product,evidenceIds:[]},
   L6={status:'PASS',sourceSha:product,evidenceIds:[]};
  const figures=s.frames.map((f,j)=>{
   const math='math_'+s.ordinal+'_'+j,rights='rights_'+s.ordinal+'_'+j;
   L1.evidenceIds.push(rights);L4.evidenceIds.push(math);
   return {publisherRecordId:f.publisherRecordId,sourceMediaUrl:f.mediaSrc,
    originalAltTextSha256:f.altTextSha256,sourceMediaByteSha256:image,
    sourceBytesInspected:true,mathematicalEquivalence:'PASS',
    mathVerificationMethod:'SOURCE_VISUAL_AND_NUMERIC_INVARIANT',
    mathEvidenceId:math,renderedVisualId:'render_'+s.ordinal+'_'+j,
    rightsEvidenceId:rights,rightsBasis:'EDULYTICS_INDEPENDENT_NO_SOURCE_REUSE',
    publisherAssetDisplayed:false,attributionRequired:false};
  });
  const learnerL6={status:'PASS',candidateSha:product,
   effectiveContentSha256:content,completedFlow:[...exactFlow],
   githubRunId:12345,browserTraceSha256:image,screenCaptureSha256:image,
   browserEvidenceId:'browser_'+s.ordinal,accessibilityEvidenceId:'access_'+s.ordinal,
   gradingEvidenceId:'grading_'+s.ordinal,
   observedRenderedVisualIds:figures.map(f=>f.renderedVisualId)};
  L6.evidenceIds.push(learnerL6.browserEvidenceId,
   learnerL6.accessibilityEvidenceId,learnerL6.gradingEvidenceId);
  ledger.records.push({lessonCode:s.lessonCode,contentSha256:content,
   status:'EVIDENCE_READY',gates:{L1,L4,L6}});
  verification.records.push({lessonCode:s.lessonCode,
   sourceSemanticSha256:s.sourceSemanticSha256,
   effectiveContentSha256:content,figures,learnerL6});
 }
 return {domReport,activityMap:inputs.activityMap,ledger,verification};
}
test('Source DOM reference alone leaves all 40 uncertified',()=>{
 const x=fixture();assert.equal(x.domReport.releaseEligible,false);
 assert.equal(x.domReport.independentlyVerifiedVisualMathematics,0);
});
test('Complete linked fixture proves final visual gate has a reachable success state',()=>{
 const x=fixture(),r=verifyFigureAcademicRelease(x);
 assert.equal(r.releaseEligible,true);assert.equal(r.independentlyVerifiedVisualMathematics,397);
 assert.equal(r.authenticatedAccessibleProductionVisualsVerified,40);
});

test('Independently authored mathematics diagrams need no publisher credit or public source link',()=>{
 const x=fixture();
 const r=verifyFigureAcademicRelease(x);
 assert.equal(r.releaseEligible,true);
 for(const record of x.verification.records)
  for(const f of record.figures) {
   assert.equal(f.attributionRequired,false);
   assert.equal(f.publisherAssetDisplayed,false);
  }
});
test('No-credit reuse of truly public-domain or unprotected media requires a documented L1 rights finding',()=>{
 const x=fixture(),p=x.verification.records[0].figures[0];
 p.rightsBasis='PUBLIC_DOMAIN_OR_UNPROTECTED_VERIFIED';
 p.publisherAssetDisplayed=true;
 p.rightsDeterminationSha256='d'.repeat(64);
 p.rightsDeterminationEvidenceId='rights_public_domain_148_0';
 x.ledger.records[0].gates.L1.evidenceIds.push(p.rightsDeterminationEvidenceId);
 assert.equal(verifyFigureAcademicRelease(x).releaseEligible,true);
 p.rightsDeterminationEvidenceId='unlinked_reference';
 assert.throws(()=>verifyFigureAcademicRelease(x),/linked evidence/);
});
test('Copying a CC BY publisher asset without required credit is blocked',()=>{
 const x=fixture(),p=x.verification.records[0].figures[0];
 p.rightsBasis='LICENSED_PUBLISHER_ASSET_REUSE';
 p.publisherAssetDisplayed=true;
 p.licenseGrantEvidenceId='publisher_license_148_0';
 x.ledger.records[0].gates.L1.evidenceIds.push(p.licenseGrantEvidenceId);
 assert.throws(()=>verifyFigureAcademicRelease(x),/attribution/);
 p.attributionRequired=true;
 p.attributionLocationEvidenceId='credit_footer_148_0';
 x.ledger.records[0].gates.L1.evidenceIds.push(p.attributionLocationEvidenceId);
 assert.equal(verifyFigureAcademicRelease(x).releaseEligible,true);
});
test('Any fabricated or stale math/rights/L6 evidence blocks final visual release',()=>{
 const bad=[
  x=>x.verification.records.pop(),
  x=>x.verification.releaseCandidateSha='2'.repeat(40),
  x=>x.verification.records[1].sourceSemanticSha256='0'.repeat(64),
  x=>x.verification.records[1].figures[0].publisherRecordId='X',
  x=>x.verification.records[1].figures[0].sourceBytesInspected=false,
  x=>x.verification.records[1].figures[0].mathematicalEquivalence='UNKNOWN',
  x=>x.verification.records[1].figures[0].mathEvidenceId='unlinked-proof',
  x=>x.verification.records[1].figures[0].publisherAssetDisplayed=true,
  x=>x.verification.records[1].figures[0].rightsBasis='LICENSED_PUBLISHER_ASSET_REUSE',
  x=>x.verification.records[1].figures[0].sourceMediaByteSha256='',
  x=>x.verification.records[1].learnerL6.completedFlow.pop(),
  x=>x.verification.records[1].learnerL6.accessibilityEvidenceId='unlinked-proof',
  x=>x.verification.records[1].learnerL6.observedRenderedVisualIds.pop(),
  x=>x.ledger.records[1].gates.L4.status='NOT_STARTED',
  x=>x.ledger.records[1].contentSha256='0'.repeat(64)
 ];
 for(const change of bad){const x=fixture();change(x);
  assert.throws(()=>verifyFigureAcademicRelease(x));}
});
