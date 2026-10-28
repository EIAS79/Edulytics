'use strict';
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const folder=path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/RichV2');
const files=fs.readdirSync(folder).filter(x=>x.endsWith('.rich-lesson-v2.json')&&x.startsWith('us-ccss-g4-')).sort();
let count=0,valid=0,invalid=[];
for(const name of files){
 const x=JSON.parse(fs.readFileSync(path.join(folder,name),'utf8'));
 for(const lesson of x.Lessons||[]){
  if(!lesson.LessonCode.startsWith('PED:US-CCSS-MATH:G4:'))continue;
  count++;
  const issues=[];
  for(const [k,min] of [['ExplanationParagraphs',2],['KeyConcepts',3],['WorkedExamples',3],['CommonMistakes',2],['SummaryPoints',3]]){
   if((lesson[k]||[]).length<min)issues.push(k+':'+(lesson[k]||[]).length+'/'+min);
  }
  for(const e of lesson.WorkedExamples||[])if(!e.Title||!e.Question||!e.Method||!e.Answer||(e.Steps||[]).length<3)issues.push('WorkedExampleShape');
  if(issues.length)invalid.push({lesson:lesson.LessonCode,file:name,issues});
  else valid++;
 }
}
console.log(JSON.stringify({grade4RichFiles:files.length,grade4DocumentsChecked:count,structurallyValid:valid,structurallyInvalid:invalid.length,invalid},null,2));

if(invalid.length)process.exitCode=1;
