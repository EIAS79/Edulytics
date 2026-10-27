'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
test('All Grade 4 L101-L149 embedded Rich V2 resources satisfy runtime minimum fields',()=>{
 const pack=read('src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g4-phase29-v1.lesson-content-pack.json');
 for(let n=101;n<=149;n++){
  const l=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g4-ordinal-'+n+'-2021.rich-lesson-v2.json').Lessons[0];
  assert.equal(l.LessonCode,pack.lessons[n-1].lessonCode);
  for(const [key,min] of [['ExplanationParagraphs',2],['KeyConcepts',3],['WorkedExamples',3],['CommonMistakes',2],['SummaryPoints',3]])assert.ok(l[key].length>=min,'L'+n+' '+key);
  for(const ex of l.WorkedExamples)assert.ok(ex.Steps.length>=3&&ex.Title&&ex.Question&&ex.Method&&ex.Answer,'L'+n+' worked example');
 }
});
test('Do not conflate Rich V2 structure with verified Practice or Stage D',()=>{
 const a=read('artifacts/curriculum-delivery/US-CCSS-MATH/G4/g4-101-149-richv2-practice-audit.json');
 assert.equal(a.structuredRichV2Count,49);assert.equal(a.lessonsWithExactPracticeMappings,13);assert.equal(a.exactPracticeMappingsUnverified.length,36);
 const mapped=new Set(a.perLesson.filter(x=>x.exactPracticeMapped).map(x=>x.ordinal));
 assert.deepEqual([...mapped],Array.from({length:13},(_,i)=>101+i));
 for(const l of a.perLesson){assert.equal(l.fullStageDVerified,false);}
 assert.equal(a.perLesson.length,49);
});
