'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs');
const {auditOriginalImageReuse}=require('../tools/us-ccss-b01-original-image-reuse-audit.cjs');
const read=p=>JSON.parse(fs.readFileSync(p,'utf8'));
const map=()=>read('artifacts/academic-us-ccss/batches/B01-v9-original-teacher-figure-dom-asset-map-40.json');
const packs=()=>{
 const result={};
 for(const grade of [2,3,4])result[grade]=read(
  'src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g'+grade+'-phase29-v1.lesson-content-pack.json');
 return result;
};
test('Exactly 397 original source pointers do not equal 397 copied learner image URLs',()=>{
 const x=auditOriginalImageReuse({figureMap:map(),gradePacks:packs()});
 assert.equal(x.coverage.sourceFrames,397);
 assert.equal(x.coverage.tikzRefs,352);
 assert.equal(x.coverage.imageFileRefs,45);
 assert.equal(x.coverage.distinctSourceMediaUrls,383);
 assert.equal(x.coverage.exactMediaUrlsEmbeddedInMatchingLessons,0);
 assert.equal(x.imageFileCandidates.length,45);
 assert.equal(x.lessonRows.length,40);
 assert.equal(x.legalReviewStatus,'ALL_397_NOT_DETERMINED');
 assert.equal(x.releaseEligible,false);
});
test('A copied source image URL becomes flagged, never silently cleared',()=>{
 const m=map(),p=packs(),row=m.rows[0];
 const lesson=p[2].lessons.find(x=>x.lessonCode===row.lessonCode);
 lesson.translations[0].workedExamples+=' <img src="'+row.frames[0].mediaSrc+'">';
 const result=auditOriginalImageReuse({figureMap:m,gradePacks:p});
 assert.equal(result.coverage.exactMediaUrlsEmbeddedInMatchingLessons,1);
 assert.equal(result.lessonRows.find(x=>x.lessonCode===row.lessonCode).canonicalPackDecision,
  'SOURCE_MEDIA_IN_LESSON_NEEDS_RIGHTS_AND_VISUAL_REVIEW');
 assert.equal(result.releaseEligible,false);
});
test('No legal ownership claim is manufactured from CMS type or metadata',()=>{
 const x=auditOriginalImageReuse({figureMap:map(),gradePacks:packs()});
 assert.equal(x.confirmedProtectedSourceImages.length,0);
 assert.equal(x.coverage.actualImageBytesInspected,0);
 assert.ok(x.imageFileCandidates.every(x=>x.legalStatus==='NOT_DETERMINED'));
 const view=fs.readFileSync('src/Edulytics.Web/Views/Shared/_RichLessonVisual.cshtml','utf8');
 assert.ok(view.includes('independentSymbol'));
 assert.ok(!view.includes('cms-assets.illustrativemathematics.org'));
});
test('Copyright lookup is not imposed on self-authored or verified trivial mathematical visuals',()=>{
 const x=auditOriginalImageReuse({figureMap:map(),gradePacks:packs()});
 assert.equal(x.visualCopyPolicy.ordinaryVerifiedMathematicalSchematicsRequirePublisherSearch,false);
 assert.equal(x.visualCopyPolicy.excludePhotosFromDirectCopy,true);
 assert.equal(x.visualCopyPolicy.directReuseOfUninspectedPublisherFile,false);
 assert.ok(x.imageFileCandidates.every(a=>a.policyBeforeVisualInspection
  ==='INSPECT_ACTUAL_VISUAL_BEFORE_COPYING_SOURCE_FILE'));
});
test('Tampered source facts or missing grade pack fail closed',()=>{
 const p=packs();delete p[3];
 assert.throws(()=>auditOriginalImageReuse({figureMap:map(),gradePacks:p}),/Missing grade pack/);
 const m=map(),pp=packs();m.rows[0].frames[0].rightsToRepublishVerified=true;
 assert.throws(()=>auditOriginalImageReuse({figureMap:m,gradePacks:pp}));
});
