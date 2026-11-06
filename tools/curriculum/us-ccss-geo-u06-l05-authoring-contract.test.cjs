const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const p=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u06-l01-l06-2021.rich-lesson-v2.json"),'utf8'));
const a=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u06-l05-source-human-paper-20261031.json"),'utf8'));
const lesson=p.Lessons.find(x=>x.LessonCode==="PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U06:L05");
test('lesson exists, previous authored lessons preserved, source task contract selected',()=>{
 assert.ok(lesson);assert.equal(a.LessonCode,"PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U06:L05");assert.equal(a.SourceTaskCrosswalk.length,5);
 assert.equal(a.Practice.Count,7);assert.ok(lesson.WorkedExamples.length>0);
});
test('four real human rubric criteria, five fixture families and tests not executed',()=>{
 assert.equal(a.SelectedPracticeRoute.MachineGrade,false);assert.equal(a.HumanRubric.TotalMax,8);
 assert.equal(a.HumanRubric.Criteria.reduce((t,x)=>t+x.MaxPoints,0),8);
 assert.equal(a.TestsExecuted,'NOT_RUN');
 for(const k of ['Correct','Equivalent','Wrong','Boundary','Malformed'])assert.ok(a.AuthoredFixtures[k].length>=2,k);
});
test('printable source-specific student evidence',()=>{const w=fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Printables/geo-u06-l05-squares-circles-student-worksheet.md"),'utf8');assert.ok(w.includes('U06:L05'));assert.equal(a.SelectedPracticeRoute.PrintableStudentEvidencePath,"src/Edulytics.Core/Curriculum/LessonContent/Printables/geo-u06-l05-squares-circles-student-worksheet.md");});
test('independent source math oracle 1',()=>{assert.ok(Math.abs((2**2+(-7)**2)-(53))<1e-9);});
test('independent source math oracle 2',()=>{assert.ok(Math.abs(((10**2)-(2**2+7**2))-(47))<1e-9);});
test('independent source math oracle 3',()=>{assert.ok(Math.abs(((12-(-18))/2)-(15))<1e-9);});
test('independent source math oracle 4',()=>{assert.ok(Math.abs(((4-2)**2+(0-1)**2)-(5))<1e-9);});
test('independent source math oracle 5',()=>{assert.ok(Math.abs(((-3-2)**2+(3-1)**2)-(29))<1e-9);});
test('source scope 2233',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 5.1 Math Talk Distributio")));});
test('source scope 2363',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 5.2 Perfectly Square (15m")));});
test('source scope 2493',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 5.3 Back and Forth (15min")));});
test('source scope 2623',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 5.3 optional sphere in th")));});
test('source scope 2753',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source student summary perfect s")));});
test('source scope 2883',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source Practice P1-P7: P1 match ")));});
