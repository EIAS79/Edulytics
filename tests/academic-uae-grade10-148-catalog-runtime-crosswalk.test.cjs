'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const load=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const cross=load('artifacts/curriculum-delivery/UAE-MOE-MATH/G10/grade10-approved-148-to-runtime-48-crosswalk.json');
const manifest=load('docs/curriculum/UAE_G10_GENERAL_OFFICIAL_MANIFEST.json');
const blueprint=load('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g10-general-t1-ogl-v1.lesson-blueprint.json');
test('Approved unified 148 index has a traceable collision-free crosswalk to all 48 current Grade10 General identities',()=>{
 assert.equal(cross.canonicalLessonCount,148);assert.equal(cross.canonicalUnitCount,18);
 assert.equal(cross.matchedExistingLessonCount,48);assert.equal(cross.missingRuntimeLessons,100);
 assert.equal(cross.canonicals.length,148);
 assert.equal(manifest.LessonCount,148);
 assert.deepEqual(cross.canonicals.map(x=>x.ordinal),Array.from({length:148},(_,i)=>i+1));
 assert.equal(new Set(cross.canonicals.map(x=>x.canonicalCode)).size,148);
 const old=cross.canonicals.filter(x=>x.legacyLessonCode);
 assert.equal(old.length,48);
 assert.equal(new Set(old.map(x=>x.legacyLessonCode)).size,48);
 assert.equal(blueprint.Lessons.length,148);
 assert.equal(blueprint.Units.length,18);
 assert.equal(new Set(blueprint.Lessons.map(x=>x.LessonCode)).size,148);
 for(const lesson of blueprint.Lessons.filter(x=>!x.LessonCode.includes(':UNIFIED:'))){
   const row=old.find(x=>x.legacyLessonCode===lesson.LessonCode);
   assert.ok(row,'never orphan existing student-linked lesson code: '+lesson.LessonCode);
   assert.equal(row.title,lesson.Title);
 }
 const current=cross.canonicals[0];
 assert.equal(current.canonicalCode,'G10-U01-L01');
 assert.equal(current.title,'جمع كثيرات الحدود وطرحها');
 assert.equal(current.legacyLessonCode,'PED:UAE-MOE-MATH:L10:GENERAL:01:01:ADDING-AND-SUBTRACTING-POLYNOMIALS');
});
test('Catalog alignment and individual lesson approval are separate from migration and Stage D',()=>{
 assert.equal(cross.canonicalBlueprintAligned,true);
 assert.equal(cross.contentPackAligned,false);
 assert.equal(cross.currentRuntimeCatalogAligned,false);
 assert.equal(cross.contentStageABCClosureCount,0);
 assert.equal(cross.testsRun,'WRITTEN_NOT_RUN');
 assert.equal(manifest.FullLessonLevelSemanticAuditComplete,false);
 assert.equal(manifest.RuntimeMigrationStatus.startsWith('NOT_APPLIED'),true);
});
