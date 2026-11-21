'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');

const root=path.resolve(__dirname,'..');
const richDirectory=path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/RichV2');
const labDirectory=path.join(root,'src/Edulytics.Web/wwwroot/lesson-labs/uae-g9-general');
const nonOriginalMediaExtensions=new Set(['.pdf','.png','.jpg','.jpeg','.webp','.gif','.mp4','.webm','.mov','.avif','.heic']);
const mediaLocator=/^(?:url|href|src|imageurl|imagepath|videoid|externalurl|thumbnailurl|posterurl)$/i;
const externalResource=/https?:\/\/|data:image\/|data:video\//i;

test('All 82 authored Grade 9 General Arabic RichV2 lesson assets are source-image-free at this inspected layer',()=>{
 const files=fs.readdirSync(richDirectory)
   .filter(name=>/^uae-g9-general-.*\.json$/i.test(name)).sort();
 assert.equal(files.length,82,'the complete 82-lesson target is required');
 const codes=new Set();
 let visuals=0;
 for(const name of files){
  const pack=JSON.parse(fs.readFileSync(path.join(richDirectory,name),'utf8'));
  assert.equal(pack.PackCode,'UAE-G9-GENERAL');
  assert.ok(Array.isArray(pack.Lessons));
  assert.equal(pack.Lessons.length,1,'one authored lesson per RichV2 artifact');
  for(const lesson of pack.Lessons){
   assert.equal(lesson.CultureCode,'ar');
   assert.ok(lesson.LessonCode.startsWith('PED:UAE-MOE-MATH:L9:GENERAL:'));
   assert.equal(codes.has(lesson.LessonCode),false,'duplicate source lesson identity');
   codes.add(lesson.LessonCode);
   assert.deepEqual(lesson.Videos||[],[],'the draft may not embed unreviewed third-party video');
   assert.doesNotMatch(JSON.stringify(lesson),externalResource,'external or base64 media URLs are not cleared');
   for(const visual of lesson.Visuals||[]){
    visuals++;
    assert.ok(visual.Kind,'visual must have an explicit shape kind');
    for(const k of Object.keys(visual)){
     assert.equal(mediaLocator.test(k),false,'visual references unverified external asset: '+k);
    }
   }
  }
 }
 assert.equal(codes.size,82);
 assert.ok(visuals>=82,'must retain meaningful original instructional representations');
});

test('UAE Grade 9 General unpublished HTML/JS lab directory contains no imported source PDFs or binary media',()=>{
 assert.equal(fs.existsSync(labDirectory),true);
 let htmlOrJs=0;
 const walk=directory=>{
  for(const item of fs.readdirSync(directory,{withFileTypes:true})){
   const file=path.join(directory,item.name);
   if(item.isDirectory()){walk(file);continue}
   const ext=path.extname(item.name).toLowerCase();
   assert.equal(nonOriginalMediaExtensions.has(ext),false,
     'do not silently commit publisher source image/video/PDF files: '+path.relative(root,file));
   if(ext==='.html'||ext==='.js')htmlOrJs++;
  }
 };
 walk(labDirectory);
 assert.ok(htmlOrJs>0,'student source-aligned activities must not disappear to pass the rights guard');
});

test('This narrowly scoped media audit does not turn unverified publisher source rights into release approval',()=>{
 const r=JSON.parse(fs.readFileSync(
   path.join(root,'artifacts/curriculum-delivery/UAE-MOE-MATH/G9/grade9-release-validation.json'),'utf8'));
 assert.equal(r.ownerAcademicApprovalRecorded,true);
 assert.equal(r.thirdPartyRightsVerified,false,'independent authored media is not a blanket license for book media');
 assert.equal(r.sourceIndexComplete,false);
 assert.equal(r.targetLessonsOneToOneVerified,0);
 assert.equal(r.decision,'NO_GO_SOURCE_100');
});
