'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const dir='src/Edulytics.Core/Curriculum/LessonContent/RichV2';
const p='src/Edulytics.Core/Curriculum/LessonContent/Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json';

test('Every one of the 82 UAE G9 General lessons has exactly one Arabic RichV2 file with substantive original text and examples',()=>{
 const pack=read(p);
 const catalogue=new Map(pack.Lessons.map(x=>[x.LessonCode,x]));
 assert.equal(catalogue.size,82);
 const files=fs.readdirSync(path.join(root,dir))
   .filter(x=>/^uae-g9-general-u\d\d-l\d\d-ar\.rich-lesson-v2\.json$/.test(x)).sort();
 assert.equal(files.length,82,'RichV2 file count must equal complete 13-unit/82-lesson catalog');
 const observed=new Set();
 for(const filename of files){
  const obj=read(path.join(dir,filename));
  assert.ok([1,2].includes(obj.SchemaVersion),filename);
  assert.ok(obj.ContentVersion,filename);
  assert.equal(obj.Lessons.length,1,filename);
  const l=obj.Lessons[0];
  assert.equal(l.CultureCode,'ar',filename);
  assert.ok(catalogue.has(l.LessonCode),filename+' has orphan lesson');
  assert.equal(l.Title,catalogue.get(l.LessonCode).Translations.find(t=>t.CultureCode==='ar').Title,filename);
  assert.ok(!observed.has(l.LessonCode),'Duplicate RichV2 lesson code '+l.LessonCode);
  observed.add(l.LessonCode);
  assert.ok(Array.isArray(l.ExplanationParagraphs)&&l.ExplanationParagraphs.length>=1,filename);
  assert.ok(Array.isArray(l.KeyConcepts)&&l.KeyConcepts.length>=1,filename);
  assert.ok(Array.isArray(l.WorkedExamples)&&l.WorkedExamples.length>=1,filename);
  assert.ok(Array.isArray(l.CommonMistakes)&&l.CommonMistakes.length>=1,filename);
  assert.ok(Array.isArray(l.SummaryPoints)&&l.SummaryPoints.length>=1,filename);
  assert.ok(Array.isArray(l.Visuals)&&l.Visuals.length>=1,filename);
  for(const ex of l.WorkedExamples){
   assert.ok(ex.Question&&ex.Answer&&ex.Steps?.length,filename+': incomplete worked example');
  }
 }
 assert.equal(observed.size,82);
});
