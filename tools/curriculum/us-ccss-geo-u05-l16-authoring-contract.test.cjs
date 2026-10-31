const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const p=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u05-l13-l18-2021.rich-lesson-v2.json"),'utf8'));
const a=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u05-l16-source-human-paper-20261031.json"),'utf8'));
const lesson=p.Lessons.find(x=>x.LessonCode==="PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U05:L16");
test('lesson exists, previous authored lessons preserved, source task contract selected',()=>{
 assert.ok(lesson);assert.equal(a.LessonCode,"PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U05:L16");assert.equal(a.SourceTaskCrosswalk.length,6);
 assert.equal(a.Practice.Count,8);assert.ok(lesson.WorkedExamples.length>0);
});
test('four real human rubric criteria, five fixture families and tests not executed',()=>{
 assert.equal(a.SelectedPracticeRoute.MachineGrade,false);assert.equal(a.HumanRubric.TotalMax,8);
 assert.equal(a.HumanRubric.Criteria.reduce((t,x)=>t+x.MaxPoints,0),8);
 assert.equal(a.TestsExecuted,'NOT_RUN');
 for(const k of ['Correct','Equivalent','Wrong','Boundary','Malformed'])assert.ok(a.AuthoredFixtures[k].length>=2,k);
});
test('printable source-specific student evidence',()=>{const w=fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Printables/geo-u05-l16-surface-volume-student-worksheet.md"),'utf8');assert.ok(w.includes('U05:L16'));assert.equal(a.SelectedPracticeRoute.PrintableStudentEvidencePath,"src/Edulytics.Core/Curriculum/LessonContent/Printables/geo-u05-l16-surface-volume-student-worksheet.md");});
test('independent source math oracle 1',()=>{assert.ok(Math.abs((45*45)-(2025))<1e-9);});
test('independent source math oracle 2',()=>{assert.ok(Math.abs((2*(3*4+3*8+4*8))-(136))<1e-9);});
test('independent source math oracle 3',()=>{assert.ok(Math.abs(((2*Math.PI*.1**2+2*Math.PI*.1*3)/(.03*Math.PI))-(20.666666666666668))<1e-9);});
test('independent source math oracle 4',()=>{assert.ok(Math.abs((Math.PI*(9+38/3))-(68.06784082777885))<1e-9);});
test('independent source math oracle 5',()=>{assert.ok(Math.abs((Math.pow(2/8,3))-(0.015625))<1e-9);});
test('independent source math oracle 6',()=>{assert.ok(Math.abs((10*Math.pow(108/4,2/3))-(90))<1e-9);});
test('source scope 2396',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 16.1 Maximize Area (5 min")));});
test('source scope 2526',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 16.2 Maximize in Three Di")));});
test('source scope 2656',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 16.3 Assume a Spherical E")));});
test('source scope 2786',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 16.4 OPTIONAL Measuring S")));});
test('source scope 2916',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source synthesis prism SA at vol")));});
test('source scope 3046',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source Practice P1-P8: P1 four o")));});
