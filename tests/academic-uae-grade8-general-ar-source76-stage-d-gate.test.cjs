'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g8-general-t1-ogl-v1.lesson-content-pack.json');
const bp=read('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g8-general-t1-ogl-v1.lesson-blueprint.json');
const manifest=read('docs/curriculum/UAE_G8_GENERAL_OFFICIAL_MANIFEST.json');
const progress=read('artifacts/curriculum-delivery/UAE-MOE-MATH/G8/full-grade-progress.json');
const pad=n=>String(n).padStart(2,'0');
test('12 units and 76 selected numbered Arabic lessons with no old English fallback',()=>{
 assert.equal(pack.AcademicLanguage,'ar');assert.equal(bp.Lessons.length,76);
 assert.equal(pack.Lessons.length,76);assert.equal(manifest.LessonCount,76);
 assert.equal(bp.Units.length,12);assert.equal(manifest.Units.length,12);
 assert.equal(bp.Units.reduce((a,u)=>a+u.LessonCount,0),76);
 assert.equal(manifest.Units.reduce((a,u)=>a+u.Lessons.length,0),76);
 assert.equal(bp.Units[11].LessonCount,4);assert.equal(manifest.Units[11].LessonCount,4);
 assert.equal(progress.closedABCCount,76);assert.equal(progress.gradeWideReady,false);
 assert.equal(progress.testsRun,'NOT_RUN');
 const codes=new Set();
 for(let i=0;i<76;i++){const p=pack.Lessons[i],b=bp.Lessons[i];
   assert.equal(p.LessonCode,b.LessonCode);assert.equal(p.Translations.length,1);
   assert.equal(p.Translations[0].CultureCode,'ar');assert.ok(!codes.has(p.LessonCode));codes.add(p.LessonCode);
   assert.equal(b.UnitTitle,bp.Units[b.UnitNumber-1].Title);
   for(const k of ['Explanation','KeyConceptsAndRules','WorkedExamples','StepByStepSolutions','CommonMistakes','QuickSummary'])
     assert.ok(p.Translations[0][k]?.length>0,k+' '+p.LessonCode);
 }
 assert.ok(!codes.has('PED:UAE-MOE-MATH:L8:GENERAL:12:05:INFINITE-SETS'));
});
test('each completed source lesson has recorded checkpoint, local evidence and RichV2 Arabic body',()=>{
 for(let i=0;i<76;i++){
  const b=bp.Lessons[i],n=String(i+1).padStart(3,'0');
  const prefix='artifacts/curriculum-delivery/UAE-MOE-MATH/G8/lesson-'+n;
  const cp=read(prefix+'-stage-a-c-checkpoint.json');
  const evidence=read(prefix+'-source-evidence.json');
  assert.equal(cp.lessonCode,b.LessonCode,'checkpoint '+n);
  assert.equal(cp.status,'READY_FOR_STAGE_D','closure '+n);
  assert.equal(cp.stageC.testsRun,'NOT_RUN','not run '+n);
  assert.ok(evidence,'evidence '+n);
  const f='src/Edulytics.Core/Curriculum/LessonContent/RichV2/uae-g8-general-u'+pad(b.UnitNumber)+'-l'+pad(b.LessonNumber)+'-ar.rich-lesson-v2.json';
  const rich=read(f);
  assert.equal(rich.Lessons[0].LessonCode,b.LessonCode,'rich '+n);
  assert.equal(rich.Lessons[0].CultureCode,'ar','language '+n);
 }
});
