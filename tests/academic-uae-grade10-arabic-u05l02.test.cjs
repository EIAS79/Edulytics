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
 assert.equal(cp.selectedPrintedSourceItemsMapped,100);
 assert.equal(cp.allowedQuestionFamilies.length,17);
 assert.equal(cp.sourcePdfPages[0],150);assert.equal(cp.sourcePdfPages.at(-1),157);
 assert.equal(cp.productionRouting,false);
 assert.equal(cp.status,'A_C_EXPLICIT_GAP');
 assert.equal(cp.practiceReadiness,'PARTIAL_REUSE_ONLY_NOT_READY_FOR_STAGE_D');
 assert.equal(cp.sourceSha256,'96df18c4fb47e45fc7a98f876d80e00e3cbdc080e5acfbdc9674a25c85f8cb9e');
 const entry=pack.Lessons.find(x=>x.LessonCode===code);
 assert.ok(entry);
 assert.equal(entry.Translations.find(x=>x.CultureCode==='ar').Title,cp.lessonTitle);
 assert.ok(entry.Translations.find(x=>x.CultureCode==='ar').Explanation.length>1000);
 assert.equal(rich.Lessons[0].WorkedExamples.length,26);
 assert.equal(rich.Lessons[0].Visuals.length,8);
});
test('Independent algebra oracles preserve equation and inequality directions',()=>{
 for(const x of [-2,-1,0,1,2,3,4,5]){
  assert.equal(Math.pow(2,x-1)>8,x>4);
  // Compare exact powers of 3; reciprocal floating-point bases can misclassify equality.
  assert.equal(Math.pow(3,2-x)>=9,x<=0);
  assert.equal(Math.pow(1/2,2*x-1)<8,x>-1);
  assert.equal(Math.pow(9,x)>27,x>1.5);
 }
 for(const x of [0,2])assert.equal(Math.pow(2,2*x)-5*Math.pow(2,x)+4,0);
 assert.equal(Math.pow(4,1.5),8);
 assert.equal(Math.pow(3,3),27);
});

test('Growth model and compounding examples have independent numerical checks',()=>{
 assert.equal(640*Math.pow(1.25,3),1250);
 assert.ok(Math.abs(5000*Math.pow(1.02,4)-5412.1608)<1e-8);
 assert.equal(75*Math.pow(2,2),300);
 assert.equal(75*Math.pow(2,4),1200);
 assert.ok(Math.abs(1000*Math.pow(1.05,4)-1215.50625)<1e-8);
 assert.equal(200*Math.pow(2,3)>1600,false);
 assert.equal(200*Math.pow(2,4)>1600,true);
});

test('Original targeted source repair models and cooldown have independent numerical checks',()=>{
 assert.equal(800*Math.pow(0.25,2),50);
 assert.equal(18+64*Math.pow(0.5,1),50);
 assert.equal(18+64*Math.pow(0.5,2),34);
 assert.ok(Math.pow(1.005,12)>1.06);
 assert.equal(1.5*Math.pow(81,0.75),40.5);
 assert.equal(2*Math.pow(8,4),Math.pow(2,13));
 assert.equal(2**5,32);assert.equal(0.02*32,0.64);
 assert.equal(240*Math.pow(0.5,3),30);
 assert.equal(Math.pow(4,3),Math.pow(8,2));
 assert.equal(Math.pow(0.5,4)>1/16,false);
});

test('Selected-source approximate factor and cooling / compounding graph oracles',()=>{
 assert.ok(Math.abs(Math.pow(1.6,0.25)-1.1246826503806981)<1e-10);
 assert.equal(Math.round(450*Math.pow(1.6,1.5)),911);
 assert.ok(Math.abs(50*Math.pow((88*.454)/20,1.3)-122.92241738622963)<1e-9);
 assert.ok(Math.abs(5000*Math.pow(1.02,4)-5412.1608)<1e-8);
});

test('General cooling uses the unrounded value for threshold decisions',()=>{
 const t=x=>19+66*Math.pow(1.05,-x);
 assert.equal(Math.round(t(12)*10)/10,55.8);
 assert.equal(Math.round(t(24)*10)/10,39.5);
 assert.ok(t(24)<40&&t(24)>19);
});
