'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const base='src/Edulytics.Core/Curriculum/LessonContent/';
const source=read(base+'Packs/uae-g9-general-ar-2025-student-u01l01.lesson-content-pack.json');
for(const [n,slug] of [[2,'ORDER-OF-OPERATIONS'],[3,'PROPERTIES-OF-NUMBERS'],[4,'DISTRIBUTIVE-PROPERTY']]){
test('U01L0'+n+' retains canonical identity and Arabic source objectives in new RichV2',()=>{
 const lessonCode='PED:UAE-MOE-MATH:L9:GENERAL:01:0'+n+':'+slug;
 const pack=source.Lessons.find(x=>x.LessonCode===lessonCode);
 const rich=read(base+'RichV2/uae-g9-general-u01-l0'+n+'-ar.rich-lesson-v2.json');
 assert.ok(pack,'canonical pack lesson must exist');
 assert.equal(rich.SchemaVersion,1);
 assert.equal(rich.PackCode,'UAE-MOE-MATH');
 assert.equal(rich.Lessons.length,1);
 const l=rich.Lessons[0];
 assert.equal(l.LessonCode,lessonCode);
 assert.equal(l.CultureCode,'ar');
 assert.equal(l.Title,pack.Translations.find(x=>x.CultureCode==='ar').Title);
 assert.ok(l.ExplanationParagraphs.length>=3);
 assert.ok(l.KeyConcepts.length>=3);
 assert.ok(l.WorkedExamples.length>=4);
 assert.ok(l.Visuals.length>=2);
 for(const e of l.WorkedExamples){
 assert.ok(e.Question);
 assert.ok(e.Method);
 assert.ok(e.Steps.length>=2);
 assert.ok(e.Answer);
 assert.ok(e.Check);
 }
});
}
test('U01L02 respects equal precedence of multiplication and division',()=>{
 const x=read(base+'RichV2/uae-g9-general-u01-l02-ar.rich-lesson-v2.json').Lessons[0];
 assert.ok(x.WorkedExamples.some(e=>e.Question.includes('٢٤÷٣×٢')&&e.Answer==='١٦'));
});
test('U01L03 forbids inverse of zero and noncommutative subtraction',()=>{
 const x=read(base+'RichV2/uae-g9-general-u01-l03-ar.rich-lesson-v2.json').Lessons[0];
 assert.ok(x.KeyConcepts.some(k=>k.Title==='المقلوب الضربي'&&k.Rule.includes('أ≠٠')));
 assert.ok(x.WorkedExamples.some(k=>k.Question.includes('١٢−٥')&&k.Answer.includes('−٧')));
});
test('U01L04 preserves signed distribution and avoids squaring sum error',()=>{
 const x=read(base+'RichV2/uae-g9-general-u01-l04-ar.rich-lesson-v2.json').Lessons[0];
 assert.ok(x.WorkedExamples.some(e=>e.Question.includes('−٢(س−٦)')&&e.Answer==='−٢س+١٢'));
 assert.ok(x.CommonMistakes.some(e=>e.Mistake.includes('(أ+ب)²')));
});
