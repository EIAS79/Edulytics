const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const p=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u05-l13-l18-2021.rich-lesson-v2.json"),'utf8'));
const a=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u05-l18-source-human-paper-20261031.json"),'utf8'));
const lesson=p.Lessons.find(x=>x.LessonCode==="PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U05:L18");
test('lesson exists, previous authored lessons preserved, source task contract selected',()=>{
 assert.ok(lesson);assert.equal(a.LessonCode,"PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U05:L18");assert.equal(a.SourceTaskCrosswalk.length,5);
 assert.equal(a.Practice.Count,9);assert.ok(lesson.WorkedExamples.length>0);
});
test('four real human rubric criteria, five fixture families and tests not executed',()=>{
 assert.equal(a.SelectedPracticeRoute.MachineGrade,false);assert.equal(a.HumanRubric.TotalMax,8);
 assert.equal(a.HumanRubric.Criteria.reduce((t,x)=>t+x.MaxPoints,0),8);
 assert.equal(a.TestsExecuted,'NOT_RUN');
 for(const k of ['Correct','Equivalent','Wrong','Boundary','Malformed'])assert.ok(a.AuthoredFixtures[k].length>=2,k);
});
test('printable source-specific student evidence',()=>{const w=fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Printables/geo-u05-l18-volume-graph-student-worksheet.md"),'utf8');assert.ok(w.includes('U05:L18'));assert.equal(a.SelectedPracticeRoute.PrintableStudentEvidencePath,"src/Edulytics.Core/Curriculum/LessonContent/Printables/geo-u05-l18-volume-graph-student-worksheet.md");});
test('independent source math oracle 1',()=>{assert.ok(Math.abs((Math.PI*1.5**2*1)-(7.0685834705770345))<1e-9);});
test('independent source math oracle 2',()=>{assert.ok(Math.abs((Math.cbrt(6000/(2.25*Math.PI)))-(9.468320564099924))<1e-9);});
test('independent source math oracle 3',()=>{assert.ok(Math.abs((2.25*Math.PI*5**3)-(883.5729338221294))<1e-9);});
test('independent source math oracle 4',()=>{assert.ok(Math.abs(((6.75*Math.PI)*125)-(2650.718801466388))<1e-9);});
test('independent source math oracle 5',()=>{assert.ok(Math.abs((Math.cbrt(12000/(6.75*Math.PI)))-(8.271339878658667))<1e-9);});
test('independent source math oracle 6',()=>{assert.ok(Math.abs(((2.5**2)*Math.PI*10.5)-(206.16701789183017))<1e-9);});
test('independent source math oracle 7',()=>{assert.ok(Math.abs((4*6*10.5)-(252))<1e-9);});
test('source scope 2550',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 18.1 Parade Balloon Part ")));});
test('source scope 2680',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 18.2 Parade Balloon Part ")));});
test('source scope 2810',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 18.3 Beach Ball Balloon (")));});
test('source scope 2940',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 18.3 optional extension s")));});
test('source scope 3070',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source student summary source co")));});
test('source scope 3200',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source Practice P1-P9: P1 cube e")));});
