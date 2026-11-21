'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'..');
const contracts=fs.readFileSync(path.join(root,'src/Edulytics.Core/Curriculum/CanonicalLessonContentPack.cs'),'utf8');
function namesOfEnum(name){
 const m=contracts.match(new RegExp('public enum '+name+'\\s*\\{([^}]+)\\}'));
 assert.ok(m, name+' enum must be present');
 return new Set(m[1].split(',').map(x=>x.replace(/\/\/[^\n]*/g,'').trim().split('=')[0].trim()).filter(Boolean));
}
test('Every embedded lesson-content pack enum value must exist in actual .NET model before seeder can deserialize',()=>{
 const sources=namesOfEnum('CurriculumSourceResolutionStatus');
 const titleSources=namesOfEnum('LessonTitleProvenance');
 const packs=path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/Packs');
 const files=fs.readdirSync(packs).filter(n=>n.endsWith('.lesson-content-pack.json'));
 assert.ok(files.length>60);
 let lessons=0;
 for(const name of files){
  const data=JSON.parse(fs.readFileSync(path.join(packs,name),'utf8'));
  if(data.SourceResolution!==undefined)assert.ok(sources.has(data.SourceResolution),name+': invalid SourceResolution='+data.SourceResolution);
  for(const lesson of data.Lessons||[]){
   lessons++;
   if(lesson.TitleProvenance!==undefined)assert.ok(titleSources.has(lesson.TitleProvenance),name+': '+lesson.LessonCode+' invalid TitleProvenance='+lesson.TitleProvenance);
  }
 }
 assert.ok(lessons>82);
});
test('UAE General Grade9 cross-edition canonical pack is Draft and defines all 82 source-linked Arabic titles under Source Policy v2',()=>{
 const pack=JSON.parse(fs.readFileSync(path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json'),'utf8'));
 assert.equal(pack.Status,'Draft');
 assert.equal(pack.SourcePolicyVersion,2);
 assert.ok(namesOfEnum('CurriculumSourceResolutionStatus').has(pack.SourceResolution));
 assert.equal(pack.Lessons.length,82);
 const titles=namesOfEnum('LessonTitleProvenance');
 const seen=new Set();
 for(const lesson of pack.Lessons){
  assert.ok(titles.has(lesson.TitleProvenance));
  assert.notEqual(lesson.TitleProvenance,'LegacyUnspecified');
  assert.ok(lesson.TitleSourceReference);
  assert.equal(lesson.Translations.length,1);
  assert.equal(lesson.Translations[0].CultureCode,'ar');
  assert.ok(lesson.LessonCode.startsWith('PED:UAE-MOE-MATH:L9:GENERAL:'));
  assert.ok(!seen.has(lesson.LessonCode));
  seen.add(lesson.LessonCode);
 }
});
