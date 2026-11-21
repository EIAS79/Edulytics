'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'..');
const packPath='src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json';
const read=()=>JSON.parse(fs.readFileSync(path.join(root,packPath),'utf8'));
test('UAE Grade9 draft canonical content uses defined .NET source and title provenance enums',()=>{
 const p=read();
 assert.equal(p.SourceResolution,'OwnerSelectedStudentBookCrosscheck');
 assert.equal(p.PedagogicalSourceType,'OwnerSelectedLocalArabicStudentBookReference');
 assert.equal(p.SourcePolicyVersion,2);
 assert.equal(p.Status,'Draft');
 assert.equal(p.Lessons.length,82);
 assert.equal(new Set(p.Lessons.map(x=>x.LessonCode)).size,82);
 assert.ok(p.Lessons.every(x=>x.TitleProvenance==='PedagogicalSource'));
 assert.ok(p.Lessons.every(x=>x.TitleSourceReference && x.TitleSourceReference.length>8));
 assert.ok(p.Lessons.every(x=>x.Translations.length===1 && x.Translations[0].CultureCode==='ar'));
 assert.equal(p.Lessons[0].TitleProvenance,'PedagogicalSource');
 assert.equal(p.Lessons[0].LessonCode,'PED:UAE-MOE-MATH:L9:GENERAL:01:01:VARIABLES-AND-EXPRESSIONS');
 assert.equal(p.AcademicLanguage,'ar');
 assert.equal(p.CurriculumTranslationRequired,false);
 assert.equal(p.Lessons[0].Translations[0].CultureCode,'ar');
});
test('Owner-local Grade9 source policy v2 supplies required checked timestamps, selection and rights evidence without asserting 2026-27 parity',()=>{
 const p=read();
 for(const key of ['FallbackReason','SourceAuthority','SourceCheckedAtUtc','PedagogicalSourceTitle','PedagogicalSourcePublisher','PedagogicalSourceEdition','PedagogicalSourceUrl','PedagogicalSourceCheckedAtUtc','PedagogicalSourceSelectionReason','PedagogicalSourceRightsNote']){
  assert.ok(typeof p[key]==='string'&&p[key].trim().length>3,key);
 }
 for(const key of ['SourceCheckedAtUtc','PedagogicalSourceCheckedAtUtc']){
  assert.match(p[key],/^\d{4}-\d\d-\d\dT\d\d:\d\d:\d\dZ$/);
  assert.ok(Number.isFinite(Date.parse(p[key])));
 }
 assert.ok(p.SourceVersionLabel.includes('2025-2026'));
 assert.ok(p.SourceVersionLabel.includes('2019-2020'));
 assert.ok(p.FallbackReason.includes('2026-2027'));
 assert.ok(p.PedagogicalSourceRightsNote.includes('Edulytics'));
 assert.ok(p.ReviewEvidence.includes('SOURCE_INDEX_COMPLETE=false'));
 assert.ok(p.Lessons[0].SourceRights.includes('Original independent Edulytics'));
 assert.equal(p.Status,'Draft');
});
