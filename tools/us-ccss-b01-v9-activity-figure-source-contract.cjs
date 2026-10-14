#!/usr/bin/env node
'use strict';
// V9 B01 immutable source/original ACTIVITY-FIGURE provenance gate.
// Metadata proves presence and lesson position of figure frames, but NOT that
// a remotely linked image corresponds to any particular activity prompt.
// A visual is an academic pass only after source-data, independently authored
// math contract, accessible browser rendering AND asset reuse rights are proved.
const fs=require('node:fs');
const assert=require('node:assert/strict');

function verifyOriginalActivityFigures({manifest,sourceIndex,activity,context,graph,effective}){
 assert.equal(activity?.planId,'EDULYTICS-US-CCSS-UNIFIED-V9');
 assert.equal(activity?.batchId,'B01');
 assert.equal(activity.coverage?.batchLessons,500);
 assert.equal(activity.coverage?.sourceFigureCandidateLessons,40);
 assert.equal(activity.coverage?.sourcePageHtmlRetrieved,40);
 assert.equal(activity.coverage?.uniqueLessonCodes,40);
 assert.equal(activity.rows?.length,40);
 assert.equal(context?.planId,activity.planId);
 assert.equal(context?.batchId,'B01');
 assert.equal(context.coverage?.batchManifestSize,500);
 assert.equal(context.coverage.figureReferenceGroup,40);
 assert.equal(context.rows?.length,40);
 assert.equal(manifest?.batchId,'B01');
 assert.equal(manifest.lessons?.length,500);
 assert.equal(sourceIndex?.rows?.length,500);
 assert.equal(graph?.planId,activity.planId);
 assert.equal(graph?.batchId,'B01');
 assert.equal(graph.coverage.observed,500);
 assert.equal(graph.coverage.unique,500);
 assert.equal(graph.nodes?.length,500);
 assert.equal(effective?.planId,activity.planId);
 assert.equal(effective?.batchId,'B01');
 assert.equal(effective.coverage.joined,500);
 assert.equal(effective.coverage.unique,500);
 assert.equal(effective.rows?.length,500);
 // The source figure subset cannot conceal a missing or swapped OTHER lesson.
 for(let i=0;i<500;i++){
  const m=manifest.lessons[i],source=sourceIndex.rows[i],
   g=graph.nodes[i],e=effective.rows[i];
  assert.equal(m.ordinal,i+1,'Manifest ordinal drift');
  assert.equal(source.ordinal,i+1,'Source inventory ordinal drift');
  assert.equal(g.ordinal,i+1,'Root graph ordinal drift');
  assert.equal(e.ordinal,i+1,'Effective graph ordinal drift');
  assert.equal(m.lessonCode,source.lessonCode,'Source identity drift');
  assert.equal(m.lessonCode,g.lessonCode,'Raw graph identity drift');
  assert.equal(m.lessonCode,e.lessonCode,'Effective identity drift');
 }
 const blocked=effective.sharedPatternRoots?.SOURCE_FIGURE_CAPTION_PLACEHOLDER;
 assert.ok(blocked,'Effective figure-root-cause group missing');
 assert.equal(blocked.effectiveMarkerStillPresent,40);
 assert.equal(blocked.effectiveMarkerLessonCodes.length,40);
 const activities=new Map(), contexts=new Map();
 for(const [i,row] of activity.rows.entries()){
  assert.ok(!activities.has(row.lessonCode),'Duplicate activity row');
  assert.equal(row.teacherActivityPageRead,true,'Activity page not retrieved');
  assert.ok(row.sourceFigureFrames>0,'Unexpected source figure-less member: '+row.lessonCode);
  assert.equal(row.sourceFigureFrames,row.sourceFigureContexts.length);
  assert.equal(row.sourceFigureFramesWithSemanticImageVerified,0,
   'Invented checked source graphic without independent source asset proof');
  assert.equal(row.sourceFigureFramesWithoutLinkedGraphic,row.sourceFigureFrames,
   'Unexpected figure-asset binding state; review extraction version');
  assert.equal(row.sourceImageAssetReferences,row.unlinkedSourceCMSAssetReferences.length,
   'Source asset list count does not match exact deduplicated source references');
  assert.ok(row.unlinkedSourceCMSAssetReferences.every(u=>
   /^https:\/\/cms-assets\.illustrativemathematics\.org\/[a-z0-9]+(?:[/?#].*)?$/i.test(u)),
   'Untrusted publisher media locator');
  assert.ok(row.sourceFigureContexts.every((f,j)=>f.order===j+1&&
   f.embeddedImageTagPresent===false&&
   f.mediaAssetToFigureIndexProven===false&&
   f.studentTaskDiagramMathematicsVerified===false),
   'Source figure evidence cannot be silently minted by frame ordering');
  activities.set(row.lessonCode,row);
 }
 for(const row of context.rows){
  assert.ok(!contexts.has(row.lessonCode),'Duplicate activity context');
  contexts.set(row.lessonCode,row);
 }
 assert.equal(contexts.size,40);
 assert.equal(activities.size,40);
 const seen=new Set(),rows=[];
 let frames=0,assets=0,studFacing=0;
 const candidates=new Set(blocked.effectiveMarkerLessonCodes);
 for(const code of candidates){
  const a=activities.get(code),ctx=contexts.get(code);
  assert.ok(a&&ctx,'Figure activity source identity missing '+code);
  const idx=manifest.lessons.findIndex(x=>x.lessonCode===code);
  assert.ok(idx>=0,'B01 manifest does not contain figure reference '+code);
  const m=manifest.lessons[idx],source=sourceIndex.rows[idx];
  const raw=graph.nodes[idx],eff=effective.rows[idx];
  assert.ok(!seen.has(code));seen.add(code);
  assert.equal(m.ordinal,idx+1);
  assert.equal(source.lessonCode,code);
  assert.equal(raw.lessonCode,code);
  assert.equal(eff.lessonCode,code);
  assert.equal(a.ordinal,idx+1);
  assert.equal(ctx.ordinal,idx+1);
  assert.equal(m.sourceSemanticSha256,a.sourceSemanticSha256);
  assert.equal(ctx.sourceSemanticSha256,m.sourceSemanticSha256);
  assert.equal(a.originalPreparationUrl,m.sourceUrl);
  assert.equal(ctx.originalPreparationUrl,m.sourceUrl);
  assert.equal(a.teacherActivityUrl,ctx.activityPageUrl);
  assert.equal(a.teacherActivityUrl,m.sourceUrl.replace(/\/preparation\.html$/,'/lesson.html'));
  assert.equal(ctx.teacherActivityPageFetched,true);
  assert.ok(ctx.studentFacingSectionsFound>0);
  assert.equal(eff.effectivePatternSignals.figureCaptionPlaceholder,true);
  assert.equal(eff.certificationState,'NOT_CERTIFIED_BY_THIS_JOIN');
  assert.equal(a.rightsReview,'NOT_REVIEWED_PER_ASSET');
  frames+=a.sourceFigureFrames;
  assets+=a.sourceImageAssetReferences;
  studFacing+=ctx.studentFacingSectionsFound;
  rows.push({lessonCode:code,ordinal:idx+1,sourceSemanticSha256:m.sourceSemanticSha256,
   sourcePreparationUrl:m.sourceUrl,teacherActivityUrl:a.teacherActivityUrl,
   studentFacingActivitySections:ctx.studentFacingSectionsFound,
   originalFigureFrames:a.sourceFigureFrames,
   unboundPublisherAssetReferences:a.sourceImageAssetReferences,
   sourceFigureAssetIndexJoin:'NOT_PROVEN',
   sourceSemanticFigureMathematics:'NOT_VERIFIED',
   independentDrawingMathOracle:'NOT_VERIFIED',
   publisherAssetReuseRight:'NOT_VERIFIED',
   renderedDesktopMobileAccessibilityL6:'NOT_VERIFIED',
   renderedRichV2VisualKinds:eff.richV2VisualKinds,
   academicDecision:'BLOCKED_SOURCE_FIGURE_AND_RENDER_EVIDENCE'});
 }
 assert.equal(rows.length,40);
 assert.equal(frames,activity.summary.totalFigureFrames);
 assert.equal(assets,activity.summary.totalCmsMediaReferences);
 assert.equal(studFacing,context.rows.reduce((n,x)=>n+x.studentFacingSectionsFound,0));
 return {
  schemaVersion:1,planId:activity.planId,batchId:'B01',
  kind:'B01_V9_40_FIGURE_SOURCE_ASSET_TO_RENDER_FAIL_CLOSED_CONTRACT',
  coverage:{manifestLessons:500,sourceActivityLessons:40,
   lessonIdentitiesJoined:seen.size,
   sourceFigureFrames:frames,sourceCMSAssetLocators:assets,
   sourceStudentFacingSections:studFacing},
  independentFigureMathValidations:0,verifiedSourceAssetToFigureMappings:0,
  verifiedAccessibleRenderedStudentVisuals:0,
  eligibleSourceFigureLessons:0,blockedSourceFigureLessons:rows.length,
  figureAssetReuseAuthorization:false,academicDecision:'BATCH_NO-GO',
  cause:'Unbound original activity figure frames and renderer source/task equivalence; CMS URL count cannot substitute for verified label-to-asset linkage',
  nextRequiredEvidence:'Verify activity assets and exact numerical/visual semantics within source and independent authored display; prove licensing and perform authenticated L6 desktop/mobile/accessible browser test for all 40',
  rows
 };
}
function readArg(k,otherwise){
 const found=process.argv.find(s=>s.startsWith('--'+k+'='));
 return found?found.slice(k.length+3):otherwise;
}
if(require.main===module){
 const read=p=>JSON.parse(fs.readFileSync(readArg(p,({
  manifest:'artifacts/academic-us-ccss/batches/B01-lesson-manifest.json',
  sourceIndex:'artifacts/academic-us-ccss/batches/B01-book-source-reference-index-500.json',
  activity:'artifacts/academic-us-ccss/batches/B01-v9-original-activity-figure-frames-40.json',
  context:'artifacts/academic-us-ccss/batches/B01-v9-source-lesson-activity-figure-context-40.json',
  graph:'b01-v9-global-root-cause-500.json',
  effective:'b01-v9-global-effective-root-causes-500.json'
 })[p]),'utf8'));
 const report=verifyOriginalActivityFigures({manifest:read('manifest'),sourceIndex:read('sourceIndex'),
  activity:read('activity'),context:read('context'),graph:read('graph'),effective:read('effective')});
 const out=readArg('out',null);
 if(out)fs.writeFileSync(out,JSON.stringify(report,null,2)+'\n');
 else process.stdout.write(JSON.stringify(report,null,2)+'\n');
 process.stderr.write('V9 B01 original figure source-asset guard '+report.blockedSourceFigureLessons+
  '/40 BLOCKED; '+report.coverage.sourceFigureFrames+
  ' frames; '+report.coverage.sourceCMSAssetLocators+
  ' unlinked CMS media pointers; certification NO-GO\n');
 if(process.argv.includes('--strict')&&report.blockedSourceFigureLessons>0)process.exitCode=2;
}
module.exports={verifyOriginalActivityFigures};
