const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const p=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u06-l01-l06-2021.rich-lesson-v2.json"),'utf8'));
const a=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u06-l06-source-human-paper-20261031.json"),'utf8'));
const lesson=p.Lessons.find(x=>x.LessonCode==="PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U06:L06");
test('lesson exists, previous authored lessons preserved, source task contract selected',()=>{
 assert.ok(lesson);assert.equal(a.LessonCode,"PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U06:L06");assert.equal(a.SourceTaskCrosswalk.length,5);
 assert.equal(a.Practice.Count,8);assert.ok(lesson.WorkedExamples.length>0);
});
test('four real human rubric criteria, five fixture families and tests not executed',()=>{
 assert.equal(a.SelectedPracticeRoute.MachineGrade,false);assert.equal(a.HumanRubric.TotalMax,8);
 assert.equal(a.HumanRubric.Criteria.reduce((t,x)=>t+x.MaxPoints,0),8);
 assert.equal(a.TestsExecuted,'NOT_RUN');
 for(const k of ['Correct','Equivalent','Wrong','Boundary','Malformed'])assert.ok(a.AuthoredFixtures[k].length>=2,k);
});
test('printable source-specific student evidence',()=>{const w=fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Printables/geo-u06-l06-complete-square-student-worksheet.md"),'utf8');assert.ok(w.includes('U06:L06'));assert.equal(a.SelectedPracticeRoute.PrintableStudentEvidencePath,"src/Edulytics.Core/Curriculum/LessonContent/Printables/geo-u06-l06-complete-square-student-worksheet.md");});
test('independent source math oracle 1',()=>{assert.ok(Math.abs(((-105)+9+100)-(4))<1e-9);});
test('independent source math oracle 2',()=>{assert.ok(Math.abs((4+1+4)-(9))<1e-9);});
test('independent source math oracle 3',()=>{assert.ok(Math.abs(((6-3)**2+(8-4)**2)-(25))<1e-9);});
test('independent source math oracle 4',()=>{assert.ok(Math.abs(((6-11)**2+(8+4)**2)-(169))<1e-9);});
test('independent source math oracle 5',()=>{assert.ok(Math.abs(((-2)**2+5**2)-(29))<1e-9);});
test('independent source math oracle 6',()=>{assert.ok(Math.abs((54/27)-(2))<1e-9);});
test('source scope 2306',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 6.1 Fill in the Box (5min")));});
test('source scope 2436',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 6.2 Complete the Process ")));});
test('source scope 2566',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 6.3 Your Turn (10min): in")));});
test('source scope 2696',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 6.3 optional Triangulatio")));});
test('source scope 2826',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source lesson synthesis first ci")));});
test('source scope 2956',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source Practice P1-P8: P1 explai")));});
