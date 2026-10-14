#!/usr/bin/env node
'use strict';
// B01 V9 source-figure DOM record gate: all 40 original teacher Lesson pages
// have direct frame -> CMS media record references, but math, licensing,
// production UI and accessibility proof have NOT been acquired.
const fs=require('node:fs');
const assert=require('node:assert/strict');
const sourceRoot='artifacts/academic-us-ccss/batches/';
function verifyDomLinks({manifest,index,activityMap,figureContext}){
 assert.equal(manifest.batchId,'B01');
 assert.equal(manifest.lessons.length,500);
 assert.equal(index.rows.length,500);
 assert.equal(activityMap.planId,'EDULYTICS-US-CCSS-UNIFIED-V9');
 assert.equal(activityMap.batchId,'B01');
 assert.equal(activityMap.coverage?.manifest,500);
 assert.equal(activityMap.coverage.teacherLessonPagesRead,40);
 assert.equal(activityMap.rows.length,40);
 assert.equal(figureContext?.planId,activityMap.planId);
 assert.equal(figureContext?.batchId,'B01');
 assert.equal(figureContext.rows?.length,40);
 const context=new Map(figureContext.rows.map(x=>[x.lessonCode,x]));
 assert.equal(context.size,40);
 const all=new Set(),figureLessons=new Set(),assetRefs=new Set();
 let frames=0,domReferences=0,unlinked=0,altWithLength=0;
 const rows=[];
 for(let i=0;i<500;i++){
  const m=manifest.lessons[i],idx=index.rows[i];
  assert.equal(m.ordinal,i+1);
  assert.equal(idx.ordinal,i+1);
  assert.equal(m.lessonCode,idx.lessonCode);
  assert.ok(!all.has(m.lessonCode),'duplicate manifest identity');
  all.add(m.lessonCode);
 }
 for(const r of activityMap.rows){
  const i=r.ordinal-1;
  const m=manifest.lessons[i],idx=index.rows[i],ctx=context.get(r.lessonCode);
  assert.ok(m&&idx&&ctx,'unmapped source teacher figure lesson');
  assert.ok(!figureLessons.has(r.lessonCode));
  figureLessons.add(r.lessonCode);
  assert.equal(r.lessonCode,m.lessonCode);
  assert.equal(idx.lessonCode,m.lessonCode);
  assert.equal(r.sourceSemanticSha256,m.sourceSemanticSha256);
  assert.equal(r.preparationLocalFileInventorySha256,idx.localFileByteSha256);
  assert.equal(r.publisherTeacherActivityUrl,m.sourceUrl.replace(/\/preparation\.html$/,'/lesson.html'));
  assert.equal(ctx.activityPageUrl,r.publisherTeacherActivityUrl);
  assert.equal(ctx.originalPreparationUrl,m.sourceUrl);
  assert.equal(r.httpStatus,200);
  assert.equal(r.originalHTMLFigureFrameCount,r.frames.length);
  assert.ok(r.frames.length>=1);
  let mapped=0;
  for(const [j,f] of r.frames.entries()){
   assert.equal(f.frameOrdinal,j+1);
   assert.match(f.publisherRecordId,/^\d+$/);
   assert.ok(['TikzFile','ImageFile'].includes(f.publisherAssetKind)||
    /^[A-Za-z][A-Za-z0-9]+$/.test(f.publisherAssetKind),
    'Unrecognized original media record class');
   assert.match(f.mediaSrc,/^https:\/\/cms-assets\.illustrativemathematics\.org\/[a-z0-9]+(?:[?#].*)?$/i);
   assert.match(f.altTextSha256,/^[a-f0-9]{64}$/);
   assert.ok(Number.isInteger(f.altTextLength)&&f.altTextLength>=0);
   assert.equal(f.rawHtmlFrameRecordLinked,true);
   assert.equal(f.mathSemanticsVerified,false);
   assert.equal(f.rightsToRepublishVerified,false);
   assert.equal(f.authenticatedRichV2GraphicMatchesSource,false);
   if(f.altTextLength>0)altWithLength++;
   assetRefs.add(f.mediaSrc);
   mapped++;
  }
  assert.equal(r.directCMSMediaRecordLinkCount,mapped);
  frames+=r.frames.length;
  domReferences+=mapped;
  unlinked+=r.frames.length-mapped;
  rows.push({ordinal:i+1,lessonCode:m.lessonCode,
   originalSourceSemanticSha256:m.sourceSemanticSha256,
   sourceActivityPage:r.publisherTeacherActivityUrl,
   sourceFigureFrames:r.frames.length,sourceFigureDOMReferences:mapped,
   distinctPublisherAssetURLs:new Set(r.frames.map(x=>x.mediaSrc)).size,
   verifiableSourceRecordAndUrlPair:true,
   sourceImageBytesAvailableToRenderer:false,
   mathDiagramOrNumericParity:'NOT_VERIFIED',
   publisherFigureReuseRights:'NOT_VERIFIED',
   authenticatedProductionAccessibleL6:'NOT_VERIFIED',
   releaseStatus:'SOURCE_RECORD_LINKED_BUT_NOT_ACADEMICALLY_APPROVED'});
 }
 assert.equal(figureLessons.size,40);
 assert.equal(frames,397);
 assert.equal(domReferences,397);
 assert.equal(unlinked,0);
 assert.equal(assetRefs.size,383);
 assert.equal(frames,activityMap.coverage.sourceFigureFramesFound);
 assert.equal(domReferences,activityMap.coverage.framesLinkedToOriginalDOMCMSImage);
 assert.equal(assetRefs.size,activityMap.coverage.distinctOriginalCMSSources);
 assert.equal(unlinked,activityMap.coverage.unlinkedOrNonCMSFrames);
 assert.ok(activityMap.actualImageBytesInspected===0);
 assert.equal(activityMap.reusedPublisherImages,0);
 assert.equal(activityMap.academicPassGranted,0);
 assert.equal(activityMap.batchDecision,'BATCH_NO-GO');
 return {schemaVersion:1,planId:activityMap.planId,batchId:'B01',
  kind:'SOURCE_PUBLISHER_FIGURE_DOM_LINKAGE_VERIFIED_NO_MATH_OR_RIGHTS_CERTIFICATION',
  coverage:{manifestLessons:500,sourceFigureLessons:40,sourceFigureFrames:frames,
   exactFrameToOriginalMediaDOMPointers:domReferences,
   uniquePublisherMediaURLs:assetRefs.size,unlinkedFigureFrames:unlinked,
   imageAltDescriptionsPresent:altWithLength},
  sourceFigureProvenanceStage:'OFFICIAL_TEACHER_HTML_DOM_RECORD_LINKED',
  sourceImageBytesAnalyzed:0,
  independentlyVerifiedVisualMathematics:0,
  copyrightReusableSourceFigureCount:0,
  authenticatedAccessibleProductionVisualsVerified:0,
  blockedLessonsForMathRightsAndL6:40,
  releaseEligible:false,academicDecision:'BATCH_NO-GO',
  warning:'Direct DOM links do not prove remote byte validity, fraction/geometry/data accuracy, source edition parity, reuse rights or a rendered student diagram.',
  rows};
}
if(require.main===module){
 const arg=(k,v)=>process.argv.find(x=>x.startsWith('--'+k+'='))?.slice(k.length+3)||v;
 const read=k=>JSON.parse(fs.readFileSync(arg(k,{
  manifest:sourceRoot+'B01-lesson-manifest.json',
  index:sourceRoot+'B01-book-source-reference-index-500.json',
  activityMap:sourceRoot+'B01-v9-original-teacher-figure-dom-asset-map-40.json',
  figureContext:sourceRoot+'B01-v9-source-lesson-activity-figure-context-40.json'}[k]),'utf8'));
 const report=verifyDomLinks({manifest:read('manifest'),index:read('index'),
  activityMap:read('activityMap'),figureContext:read('figureContext')});
 const out=arg('out','');
 if(out)fs.writeFileSync(out,JSON.stringify(report,null,2)+'\n');
 else process.stdout.write(JSON.stringify(report,null,2)+'\n');
 process.stderr.write('B01 V9 DOM source figure '+report.coverage.exactFrameToOriginalMediaDOMPointers+
  '/397 linked; math, rights & production L6 still 40/40 blocked; BATCH_NO-GO\n');
 if(process.argv.includes('--strict')&&!report.releaseEligible)process.exitCode=2;
}
module.exports={verifyDomLinks};
