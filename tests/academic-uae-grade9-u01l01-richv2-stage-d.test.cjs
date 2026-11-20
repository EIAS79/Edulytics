'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const base='src/Edulytics.Core/Curriculum/LessonContent/';
const code='PED:UAE-MOE-MATH:L9:GENERAL:01:01:VARIABLES-AND-EXPRESSIONS';
test('Grade9 U01L01 RichV2 is an additive Arabic rendering of the existing authored pack, not a new lesson identity',()=>{
 const pack=read(base+'Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json');
 const src=pack.Lessons.find(x=>x.LessonCode===code);
 const rich=read(base+'RichV2/uae-g9-general-u01-l01-ar.rich-lesson-v2.json');
 const lesson=rich.Lessons.find(x=>x.LessonCode===code);
 assert.ok(src&&lesson);
 assert.equal(rich.SchemaVersion,2);
 assert.equal(lesson.CultureCode,'ar');
 assert.equal(lesson.Title,src.Translations.find(x=>x.CultureCode==='ar').Title);
 assert.ok(lesson.ExplanationParagraphs.length>=3);
 assert.ok(lesson.KeyConcepts.length>=4);
 assert.ok(lesson.WorkedExamples.length>=4);
 assert.ok(lesson.Visuals.length>=3);
 for(const e of lesson.WorkedExamples){assert.ok(e.Question);assert.ok(e.Method);assert.ok(e.Steps?.length);assert.ok(e.Answer);assert.ok(e.Check);}
 assert.ok(lesson.KeyConcepts.some(x=>x.Title==='ترجمة العبارات إلى جبر'));
 assert.ok(lesson.WorkedExamples.some(x=>x.Title==='نموذج تكلفة برسوم ثابتة'));
});
test('Algebra expression coefficient is not improperly asserted inequivalent for every value including zero',()=>{
 const lesson=read(base+'RichV2/uae-g9-general-u01-l01-ar.rich-lesson-v2.json').Lessons[0];
 const rule=lesson.KeyConcepts.find(x=>x.Title==='القوى والأقواس').Rule;
 assert.match(rule,/غير متكافئين لكل س/);
 assert.match(rule,/س=٠/);
 assert.equal(3*(0**2),(3*0)**2);
 assert.notEqual(3*(2**2),(3*2)**2);
});
