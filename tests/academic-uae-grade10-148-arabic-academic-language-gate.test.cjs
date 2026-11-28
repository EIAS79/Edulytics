'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const read=p=>JSON.parse(fs.readFileSync(path.resolve(__dirname,'..',p),'utf8'));
const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g10-general-t1-ogl-v1.lesson-content-pack.json');
const blueprint=read('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/uae-g10-general-t1-ogl-v1.lesson-blueprint.json');
test('Grade10 148 canonical lessons have primary Arabic language and nonempty native titles',()=>{
 assert.equal(pack.AcademicLanguage,'ar');assert.equal(pack.CurriculumTranslationRequired,false);
 assert.equal(blueprint.Units.length,18);assert.equal(blueprint.Lessons.length,148);
 assert.equal(pack.Lessons.length,148);
 const approved=new Map(blueprint.Lessons.map(x=>[x.LessonCode,x.Title]));
 const codes=new Set();
 for(const l of pack.Lessons){
  assert.ok(approved.has(l.LessonCode),'missing canonical code '+l.LessonCode);
  assert.ok(!codes.has(l.LessonCode),'duplicate canonical lesson '+l.LessonCode);
  codes.add(l.LessonCode);
  const ar=l.Translations?.find(t=>t.CultureCode==='ar');
  assert.ok(ar,'missing Arabic body '+l.LessonCode);
  assert.ok(ar.Title && /[\u0600-\u06FF]/.test(ar.Title),'missing Arabic Title '+l.LessonCode);
  assert.ok(ar.Explanation && ar.Explanation.length>=50,'incomplete Arabic explanation '+l.LessonCode);
 }
});
