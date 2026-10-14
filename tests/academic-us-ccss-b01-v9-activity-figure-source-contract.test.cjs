'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const {verifyOriginalActivityFigures,sourceFigureReleasePreflight}=require('../tools/us-ccss-b01-v9-activity-figure-source-contract.cjs');
function fixture(){
 const codes=Array.from({length:500},(_,i)=>'PED:US-CCSS-MATH:G2:U01:L'+String(i+1).padStart(3,'0'));
 const a='a'.repeat(64),b='b'.repeat(64);
 const prepare=i=>'https://im.kendallhunt.com/k5/teachers/grade-2/unit-1/lesson-'+(i+1)+'/preparation.html';
 const manifest={batchId:'B01',lessons:codes.map((lessonCode,i)=>({
  ordinal:i+1,lessonCode,sourceSemanticSha256:a,sourceUrl:prepare(i)}))};
 const sourceIndex={rows:codes.map((lessonCode,i)=>({ordinal:i+1,lessonCode}))};
 const graph={planId:'EDULYTICS-US-CCSS-UNIFIED-V9',batchId:'B01',
  coverage:{expected:500,observed:500,unique:500},nodes:codes.map((lessonCode,i)=>({lessonCode,ordinal:i+1}))};
 const effective={planId:graph.planId,batchId:'B01',
  coverage:{expected:500,joined:500,unique:500},
  rows:codes.map((lessonCode,i)=>({ordinal:i+1,lessonCode,
   effectivePatternSignals:{figureCaptionPlaceholder:i<40},
   certificationState:'NOT_CERTIFIED_BY_THIS_JOIN',
   richV2VisualKinds:['BarGraph']})),
  sharedPatternRoots:{SOURCE_FIGURE_CAPTION_PLACEHOLDER:{
   rawCandidates:44,effectiveMarkerStillPresent:40,
   effectiveMarkerLessonCodes:codes.slice(0,40)}}};
 const context={planId:graph.planId,batchId:'B01',
  coverage:{batchManifestSize:500,figureReferenceGroup:40},
  rows:codes.slice(0,40).map((lessonCode,i)=>({
   lessonCode,ordinal:i+1,sourceSemanticSha256:a,
   originalPreparationUrl:prepare(i),
   activityPageUrl:prepare(i).replace('/preparation.html','/lesson.html'),
   teacherActivityPageFetched:true,studentFacingSectionsFound:3}))};
 const activity={planId:graph.planId,batchId:'B01',
  coverage:{batchLessons:500,sourceFigureCandidateLessons:40,
   sourcePageHtmlRetrieved:40,uniqueLessonCodes:40},
  rows:context.rows.map((r,i)=>({
   lessonCode:r.lessonCode,ordinal:r.ordinal,sourceSemanticSha256:a,
   originalPreparationUrl:r.originalPreparationUrl,
   teacherActivityUrl:r.activityPageUrl,teacherActivityPageRead:true,
   sourceFigureFrames:2,sourceFigureFramesWithoutLinkedGraphic:2,
   sourceFigureFramesWithSemanticImageVerified:0,
   sourceImageAssetReferences:3,
   unlinkedSourceCMSAssetReferences:['https://cms-assets.illustrativemathematics.org/abc1',
    'https://cms-assets.illustrativemathematics.org/abc2',
    'https://cms-assets.illustrativemathematics.org/abc3'],
   sourceFigureContexts:Array.from({length:2},(_,j)=>({
    order:j+1,embeddedImageTagPresent:false,
    mediaAssetToFigureIndexProven:false,studentTaskDiagramMathematicsVerified:false})),
   rightsReview:'NOT_REVIEWED_PER_ASSET'})),
  summary:{totalFigureFrames:80,totalCmsMediaReferences:120}};
 return {manifest,sourceIndex,graph,effective,activity,context};
}
test('All 500 identities stay stable; forty framed activities and missing asset bindings fail closed',()=>{
 const x=verifyOriginalActivityFigures(fixture());
 assert.deepEqual(x.coverage,{
  manifestLessons:500,sourceActivityLessons:40,lessonIdentitiesJoined:40,
  sourceFigureFrames:80,sourceCMSAssetLocators:120,sourceStudentFacingSections:120});
 assert.equal(x.blockedSourceFigureLessons,40);
 assert.equal(x.independentFigureMathValidations,0);
 assert.equal(x.verifiedAccessibleRenderedStudentVisuals,0);
 assert.ok(x.rows.every(r=>r.academicDecision==='BLOCKED_SOURCE_FIGURE_AND_RENDER_EVIDENCE'));
 assert.equal(x.figureAssetReuseAuthorization,false);
 assert.equal(x.academicDecision,'BATCH_NO-GO');
});
test('Reject source digest swaps, missing lessons, invented figure mappings or unauthorized CMS origins',()=>{
 const cases=[
  f=>{f.activity.rows[0].sourceSemanticSha256='9'.repeat(64);},
  f=>{f.context.rows[1].activityPageUrl='https://attacker.example/lesson.html';},
  f=>{f.activity.rows[2].sourceFigureContexts[0].studentTaskDiagramMathematicsVerified=true;},
  f=>{f.activity.rows[3].unlinkedSourceCMSAssetReferences[0]='https://fake.invalid/foo';},
  f=>{f.effective.sharedPatternRoots.SOURCE_FIGURE_CAPTION_PLACEHOLDER.effectiveMarkerLessonCodes.pop();},
  f=>{f.manifest.lessons[0].sourceSemanticSha256='0'.repeat(64);},
  f=>{f.activity.rows.pop();},
  f=>{f.effective.rows[480].lessonCode='FORGED';}
 ];
 for(const corrupt of cases){
  let x=fixture();corrupt(x);
  assert.throws(()=>verifyOriginalActivityFigures(x));
 }
});
test('Same counts from unrelated source frames cannot be silently treated as diagram parity',()=>{
 const x=fixture();
 x.activity.rows[0].sourceImageAssetReferences=2;
 x.activity.rows[0].unlinkedSourceCMSAssetReferences.pop();
 x.activity.summary.totalCmsMediaReferences=119;
 const r=verifyOriginalActivityFigures(x);
 assert.equal(r.rows[0].sourceFigureAssetIndexJoin,'NOT_PROVEN');
 assert.equal(r.rows[0].sourceSemanticFigureMathematics,'NOT_VERIFIED');
 assert.equal(r.rows[0].publisherAssetReuseRight,'NOT_VERIFIED');
 assert.equal(r.blockedSourceFigureLessons,40);
});

test('Final collective 500 release cannot treat 383 source image pointers as 336 verified visual diagrams',()=>{
 const f=fixture(),s=sourceFigureReleasePreflight(f);
 assert.equal(s.coverage.totalLessons,500);
 assert.equal(s.coverage.sourceFigureLessons,40);
 assert.equal(s.coverage.unmappedFrames,80);
 assert.equal(s.coverage.mediaPointers,120);
 assert.equal(s.releaseEligible,false);
 assert.equal(s.noAutomaticPublisherAssetReuse,true);
 assert.equal(s.academicDecision,'BATCH_NO-GO');
});
test('Static gate rejects forged rights and false semantic image proof',()=>{
 const a=fixture();a.activity.rows[0].rightsReview='APPROVED';
 assert.throws(()=>sourceFigureReleasePreflight(a));
 const b=fixture();b.activity.rows[0].sourceFigureContexts[0].mediaAssetToFigureIndexProven=true;
 assert.throws(()=>sourceFigureReleasePreflight(b));
 const c=fixture();c.context.rows[3].sourceSemanticSha256='b'.repeat(64);
 assert.throws(()=>sourceFigureReleasePreflight(c));
});
