'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const {verifyDomLinks}=require('../tools/us-ccss-b01-v9-source-figure-dom-linkage-gate.cjs');
const root=path.join(__dirname,'..'),dir='artifacts/academic-us-ccss/batches/';
const load=k=>JSON.parse(fs.readFileSync(path.join(root,dir+k),'utf8'));
function inputs(){return {
 manifest:load('B01-lesson-manifest.json'),
 index:load('B01-book-source-reference-index-500.json'),
 activityMap:load('B01-v9-original-teacher-figure-dom-asset-map-40.json'),
 figureContext:load('B01-v9-source-lesson-activity-figure-context-40.json')
}}
test('All 397 original HTML figures have source media DOM identifiers but exactly zero visual academic approvals',()=>{
 const r=verifyDomLinks(inputs());
 assert.equal(r.coverage.manifestLessons,500);
 assert.equal(r.coverage.sourceFigureLessons,40);
 assert.equal(r.coverage.sourceFigureFrames,397);
 assert.equal(r.coverage.exactFrameToOriginalMediaDOMPointers,397);
 assert.equal(r.coverage.uniquePublisherMediaURLs,383);
 assert.equal(r.coverage.unlinkedFigureFrames,0);
 assert.equal(r.rows.length,40);
 assert.equal(r.independentlyVerifiedVisualMathematics,0);
 assert.equal(r.copyrightReusableSourceFigureCount,0);
 assert.equal(r.authenticatedAccessibleProductionVisualsVerified,0);
 assert.equal(r.blockedLessonsForMathRightsAndL6,40);
 assert.equal(r.releaseEligible,false);
 assert.equal(r.academicDecision,'BATCH_NO-GO');
});
test('Tampered source SHAs, fabricated media IDs, fake rights and unsourced content fail closed',()=>{
 const mutate=[
  o=>{o.activityMap.rows[0].sourceSemanticSha256='0'.repeat(64);},
  o=>{o.activityMap.rows[1].frames[0].mediaSrc='https://attacker.invalid/malware';},
  o=>{o.activityMap.rows[2].frames[0].publisherRecordId='fake';},
  o=>{o.activityMap.rows[3].frames[0].rightsToRepublishVerified=true;},
  o=>{o.activityMap.rows[4].frames[0].mathSemanticsVerified=true;},
  o=>{o.activityMap.rows[5].frames[0].authenticatedRichV2GraphicMatchesSource=true;},
  o=>{o.activityMap.rows[6].frames[0].altTextSha256='unknown';},
  o=>{o.activityMap.rows.pop();},
  o=>{o.manifest.lessons[445].lessonCode='SWAPPED';},
  o=>{o.figureContext.rows[0].activityPageUrl='https://example.invalid/replaced';},
  o=>{o.activityMap.academicPassGranted=397;},
 ];
 for(const fn of mutate){const o=inputs();fn(o);assert.throws(()=>verifyDomLinks(o));}
});
