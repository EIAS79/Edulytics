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
test('U01L01 scanned source application coverage includes same-base powers and cylinder volume',()=>{
 const lesson=read(base+'RichV2/uae-g9-general-u01-l01-ar.rich-lesson-v2.json').Lessons[0];
 const powers=lesson.KeyConcepts.find(x=>x.Title==="\u0636\u0631\u0628 \u0627\u0644\u0642\u0648\u0649 \u0630\u0627\u062a \u0627\u0644\u0623\u0633\u0627\u0633 \u0646\u0641\u0633\u0647");
 const cylinder=lesson.KeyConcepts.find(x=>x.Title==="\u062a\u0645\u062b\u064a\u0644 \u062d\u062c\u0645 \u0627\u0644\u0623\u0633\u0637\u0648\u0627\u0646\u0629 \u0628\u062a\u0639\u0628\u064a\u0631 \u062c\u0628\u0631\u064a");
 assert.ok(powers&&cylinder);
 assert.ok(powers.Rule.includes("\u0633\u00b2\u00d7\u0633\u00b3=\u0633\u2075"));
 assert.ok(cylinder.Rule.includes("\u03c0\u0646\u0642\u00b2"));
 const powerExample=lesson.WorkedExamples.find(x=>x.Title==="\u062a\u0641\u0633\u064a\u0631 \u0636\u0631\u0628 \u0627\u0644\u0642\u0648\u0649");
 const cylinderExample=lesson.WorkedExamples.find(x=>x.Title==="\u0646\u0645\u0648\u0630\u062c \u062d\u062c\u0645 \u0623\u0633\u0637\u0648\u0627\u0646\u0629");
 assert.equal(powerExample?.Answer,"\u0661\u0660\u2077");
 assert.ok(cylinderExample?.Answer.includes("\u0664\u0665\u03c0"));
 assert.equal((10**3)*(10**4),10**7);
 assert.equal(Math.PI*3**2*5,45*Math.PI);
});
