#!/usr/bin/env node
'use strict';
// Tests embedded source MEDIA URLs, not ownership or mathematical correctness.
// A teacher-page DOM reference never supplies image-byte inspection or permission.
const assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(p,'utf8'));
function auditOriginalImageReuse({figureMap,gradePacks}){
 assert.equal(figureMap.batchId,'B01');
 assert.equal(figureMap.rows.length,40);
 assert.equal(figureMap.coverage.sourceFigureFramesFound,397);
 assert.equal(figureMap.actualImageBytesInspected,0);
 assert.equal(figureMap.reusedPublisherImages,0);
 const canonical=new Map(),checkedGrades=[];
 for(const grade of [2,3,4]){
  const pack=gradePacks[grade];
  assert.ok(pack&&Array.isArray(pack.lessons),'Missing grade pack '+grade);
  checkedGrades.push({grade,lessons:pack.lessons.length});
  for(const lesson of pack.lessons){
   assert.ok(!canonical.has(lesson.lessonCode),'Duplicate canonical lesson');
   canonical.set(lesson.lessonCode,lesson);
  }
 }
 const lessonRows=[],imageFileCandidates=[],seen=new Set(),urls=new Set();
 let frames=0,tikz=0,img=0,exact=0;
 for(const r of figureMap.rows){
  const lesson=canonical.get(r.lessonCode);
  assert.ok(lesson,'Missing canonical lesson '+r.lessonCode);
  assert.ok(!seen.has(r.lessonCode),'Duplicate source lesson');seen.add(r.lessonCode);
  const body=JSON.stringify(lesson),embedded=[];
  for(const f of r.frames){
   assert.equal(f.mathSemanticsVerified,false);
   assert.equal(f.rightsToRepublishVerified,false);
   assert.equal(f.authenticatedRichV2GraphicMatchesSource,false);
   if(f.publisherAssetKind==='TikzFile')tikz++;
   else if(f.publisherAssetKind==='ImageFile'){
    img++;
    imageFileCandidates.push({lessonCode:r.lessonCode,publisherRecordId:f.publisherRecordId,
     activity:f.teacherActivitySection,sourceMediaUrl:f.mediaSrc,
     legalStatus:'NOT_DETERMINED',sourceImageBytesInspected:false,
     flaggedReason:'Inspect actual expressive artwork and its terms before copying this source asset'});
   } else assert.fail('Unrecognized media type');
   urls.add(f.mediaSrc);
   if(body.includes(f.mediaSrc)){exact++;embedded.push(f.publisherRecordId);}
   frames++;
  }
  const publisherHostInCanonicalBody=body.includes('cms-assets.illustrativemathematics.org');
  lessonRows.push({lessonCode:r.lessonCode,sourceFigureCount:r.frames.length,
   sourceImageFileCount:r.frames.filter(f=>f.publisherAssetKind==='ImageFile').length,
   publisherHostInCanonicalBody,embeddedPublisherRecordIds:embedded,
   canonicalPackDecision:publisherHostInCanonicalBody||embedded.length
    ? 'SOURCE_MEDIA_IN_LESSON_NEEDS_RIGHTS_AND_VISUAL_REVIEW'
    : 'NO_PUBLISHER_MEDIA_URL_IN_CANONICAL_LESSON',
   rightsDetermination:'NOT_DETERMINED',productionL6:'NOT_DETERMINED'});
 }
 assert.equal(frames,397);assert.equal(tikz,352);assert.equal(img,45);
 assert.equal(urls.size,383);
 return {schemaVersion:1,batchId:'B01',
  kind:'REFERENCE_VS_CANONICAL_GRADE_PACK_IMAGE_REUSE_AUDIT',
  coverage:{batchSize:500,sourceFigureLessons:40,
   gradePackLessonsChecked:checkedGrades.reduce((s,x)=>s+x.lessons,0),
   sourceFrames:397,tikzRefs:tikz,imageFileRefs:img,
   distinctSourceMediaUrls:urls.size,exactMediaUrlsEmbeddedInMatchingLessons:exact,
   actualImageBytesInspected:0,verifiedCopyrightClaims:0,verifiedL6:0},
  checkedGrades,confirmedProtectedSourceImages:[],
  legalReviewStatus:'ALL_397_NOT_DETERMINED',
  note:'This is only direct URL scanning of canonical grade packs. RichV2/runtime and each publisher image must be audited separately before a rights claim or academic release.',
  releaseEligible:false,imageFileCandidates,lessonRows};
}
function run(){
 const batch=path.join(root,'artifacts/academic-us-ccss/batches');
 const packsRoot=path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/Packs');
 const figureMap=read(path.join(batch,'B01-v9-original-teacher-figure-dom-asset-map-40.json'));
 const gradePacks={};
 for(const grade of [2,3,4])gradePacks[grade]=read(path.join(packsRoot,'us-ccss-math-g'+grade+'-phase29-v1.lesson-content-pack.json'));
 const result=auditOriginalImageReuse({figureMap,gradePacks});
 const arg=process.argv.indexOf('--out');
 if(arg>=0)fs.writeFileSync(process.argv[arg+1],JSON.stringify(result,null,2)+'\n');
 process.stdout.write(JSON.stringify(result.coverage)+'\n');
 return result;
}
if(require.main===module)run();
module.exports={auditOriginalImageReuse,run};
