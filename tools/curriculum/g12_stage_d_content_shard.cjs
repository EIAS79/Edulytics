'use strict';
const fs=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'../../');
const base=path.join(root,'artifacts/curriculum-delivery/US-CCSS-MATH/G12');
const progress=JSON.parse(fs.readFileSync(path.join(base,'full-grade-progress.json'),'utf8'));
const intervals={'G12-D1':[1,28],'G12-D2':[29,49],'G12-D3':[50,73]};
const shard=process.argv[2];
if(!intervals[shard]){console.error('Unknown shard '+shard);process.exit(2);}
const [first,last]=intervals[shard];const errors=[];let checked=0;
function fail(n,message){errors.push('ordinal '+n+': '+message);}
for(let n=first;n<=last;n++){
 const row=progress.LessonEntries[n-1];const u=String(row.Unit).padStart(2,'0');const l=String(row.LessonNumber).padStart(2,'0');
 const prefix='us-ccss-g12-precalculus-u'+u+'-l'+l;
 const contentPath=path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/RichV2',prefix+'.rich-lesson-v2.json');
 const teacherPath=path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/Assessment',prefix+'-firstparty-teacher-led.json');
 if(!fs.existsSync(contentPath)){fail(n,'MISSING_RICHV2_JSON');continue;}
 if(!fs.existsSync(teacherPath)){fail(n,'MISSING_TEACHER_JSON');continue;}
 let rich,teacher;
 try{rich=JSON.parse(fs.readFileSync(contentPath,'utf8'));teacher=JSON.parse(fs.readFileSync(teacherPath,'utf8'));}
 catch(e){fail(n,'INVALID_JSON '+e.message);continue;}
 checked++;
 if(rich.SchemaVersion!==2)fail(n,'RICHV2_SCHEMA_NOT_2');
 if(!Array.isArray(rich.Lessons)||rich.Lessons.length!==1){fail(n,'RICHV2_NOT_EXACT_ONE_LESSON');continue;}
 const student=rich.Lessons[0];
 if(student.LessonCode!==row.LessonCode)fail(n,'RICHV2_LESSON_CODE_MISMATCH');
 if(teacher.LessonCode!==row.LessonCode)fail(n,'TEACHER_LESSON_CODE_MISMATCH');
 if(student.CultureCode!=='en')fail(n,'RICHV2_NOT_ENGLISH');
 if(!Array.isArray(student.ExplanationParagraphs)||student.ExplanationParagraphs.length<3)fail(n,'EXPLANATION_TOO_THIN');
 if(!Array.isArray(student.WorkedExamples)||!student.WorkedExamples.length)fail(n,'NO_WORKED_EXAMPLES');
 if(!Array.isArray(student.KeyConcepts)||!student.KeyConcepts.length)fail(n,'NO_KEY_CONCEPTS');
 if(!Array.isArray(student.CommonMistakes)||!student.CommonMistakes.length)fail(n,'NO_MISTAKES_GUIDANCE');
 if(!Array.isArray(student.Visuals))fail(n,'NO_VISUAL_COLLECTION');
 if(!Array.isArray(student.SummaryPoints)||!student.SummaryPoints.length)fail(n,'NO_SUMMARY');
 if(!rich.PackCode||!rich.PackCode.includes('U'+u+':L'+l))fail(n,'RICHV2_PACK_CODE_MISMATCH');
 if(!rich.ContentVersion)fail(n,'CONTENT_VERSION_MISSING');
 if(Array.isArray(student.WorkedExamples))for(let i=0;i<student.WorkedExamples.length;i++){
  const w=student.WorkedExamples[i];
  if(!w.Question||!w.Answer||!Array.isArray(w.Steps)||!w.Steps.length)fail(n,'WORKED_EXAMPLE_INCOMPLETE_'+(i+1));
 }
 // This is static content-shape inspection only; it cannot prove source equivalence,
 // mathematical correctness, runtime binding, teacher approval or visual accessibility.
}
console.log(JSON.stringify({shard,first,last,expected:last-first+1,checked,errors,scope:'STATIC_AUTHORING_SHAPE_ONLY_NOT_RUNTIME_OR_ACADEMIC_ACCEPTANCE'},null,2));
if(errors.length||checked!==last-first+1)process.exitCode=1;
