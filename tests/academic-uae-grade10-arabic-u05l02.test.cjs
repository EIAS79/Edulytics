'use strict';
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const load=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const prefix='artifacts/curriculum-delivery/UAE-MOE-MATH/G10/';
const cp=load(prefix+'lesson-043-stage-a-c-checkpoint.json');
const pack=load('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json');
const rich=load('src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g10-general-u05-l02-ar.rich-lesson-v2.json');
const code='PED:UAE-MOE-MATH:L10:GENERAL:UNIFIED:U05:L02:C043';
test('Lesson43 has independently authored Arabic content while source body is verified using same-title Applied student book',()=>{
 assert.equal(cp.canonicalOrdinal,43);
 assert.equal(cp.sourceIndexTitleVerified,true);
 assert.equal(cp.sourceBodyVerified,true);
 assert.equal(cp.ownerApprovedIndexOnlyAuthoring,false);
 assert.equal(cp.fullSourceBookEquivalenceClaimed,false);
 assert.equal(cp.sourceElementDenominatorForThisLesson,null);
 assert.equal(cp.sourcePdfPages[0],150);assert.equal(cp.sourcePdfPages.at(-1),155);
 assert.equal(cp.productionRouting,false);
 assert.equal(cp.status,'READY_FOR_STAGE_D');
 assert.equal(cp.sourceSha256,'96df18c4fb47e45fc7a98f876d80e00e3cbdc080e5acfbdc9674a25c85f8cb9e');
 const entry=pack.Lessons.find(x=>x.LessonCode===code);
 assert.ok(entry);
 assert.equal(entry.Translations.find(x=>x.CultureCode==='ar').Title,cp.lessonTitle);
 assert.ok(entry.Translations.find(x=>x.CultureCode==='ar').Explanation.length>1000);
 assert.equal(rich.Lessons[0].WorkedExamples.length,16);
 assert.equal(rich.Lessons[0].Visuals.length,5);
});
test('Independent algebra oracles preserve equation and inequality directions',()=>{
 for(const x of [-2,-1,0,1,2,3,4,5]){
  assert.equal(Math.pow(2,x-1)>8,x>4);
  assert.equal(Math.pow(1/3,x-2)>=9,x<=0);
  assert.equal(Math.pow(1/2,2*x-1)<8,x>-1);
  assert.equal(Math.pow(9,x)>27,x>1.5);
 }
 for(const x of [0,2])assert.equal(Math.pow(2,2*x)-5*Math.pow(2,x)+4,0);
 assert.equal(Math.pow(4,1.5),8);
 assert.equal(Math.pow(3,3),27);
});
