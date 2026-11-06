// G8 U01:L01 first-party offline teacher paper route authoring fixtures. AUTHORED_NOT_RUN.
'use strict';
const {test}=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const a=read('docs/curriculum/us-ccss-g8-coder4-u01-l01-academic-assessment-contract-20261030.json');
const r=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u01-l01-l08-2017.rich-lesson-v2.json');
const s=a.selectedOfflinePaperRoute20261031;
test('exact G8 lesson has printed handoff referenced from student-selected Rich V2',()=>{
 assert.equal(s.lessonCode,'PED:US-CCSS-MATH:G8:U01:L01');
 assert.equal(s.machineGenerated,false);
 const lesson=r.Lessons.find(x=>x.LessonCode===s.lessonCode);
 assert(lesson.ExplanationParagraphs.some(x=>x.includes(s.learnerWorksheetUrl)));
 const page=fs.readFileSync(path.join(root,s.learnerWorksheetPath),'utf8');
 for(const token of ['id="odd"','id="dance"','id="cool"','id="danceinv"'])assert(page.includes(token));
 assert(page.includes('This worksheet itself has no online submission'));
});
test('lesson rubric and paper student tasks include all source modalities',()=>{
 assert.equal(s.privateTeacherRubric.Criteria.reduce((v,c)=>v+c.MaxPoints,0),8);
 assert.deepEqual(s.activityMap.map(x=>x.source),['1.1','1.2','extension','Edulytiks Cool-down']);
 assert.equal(a.coordinateQuestionCandidate.runtimeEnabled,false);
});
test('teacher qualitative responses preserve alternative valid angle choices',()=>{
 assert(s.fixtures.correct.some(x=>x.startsWith('A')));
 assert(s.fixtures.equivalent.some(x=>x.startsWith('B')));
 assert(s.fixtures.equivalent.some(x=>x.startsWith('D')));
 assert(s.fixtures.wrong.some(x=>x.includes('choice letter alone')));
});
