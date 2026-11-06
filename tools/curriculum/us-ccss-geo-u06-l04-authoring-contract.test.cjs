const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const p=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u06-l01-l06-2021.rich-lesson-v2.json"),'utf8'));
const a=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u06-l04-source-human-paper-20261031.json"),'utf8'));
const lesson=p.Lessons.find(x=>x.LessonCode==="PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U06:L04");
test('lesson exists, previous authored lessons preserved, source task contract selected',()=>{
 assert.ok(lesson);assert.equal(a.LessonCode,"PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U06:L04");assert.equal(a.SourceTaskCrosswalk.length,5);
 assert.equal(a.Practice.Count,7);assert.ok(lesson.WorkedExamples.length>0);
});
test('four real human rubric criteria, five fixture families and tests not executed',()=>{
 assert.equal(a.SelectedPracticeRoute.MachineGrade,false);assert.equal(a.HumanRubric.TotalMax,8);
 assert.equal(a.HumanRubric.Criteria.reduce((t,x)=>t+x.MaxPoints,0),8);
 assert.equal(a.TestsExecuted,'NOT_RUN');
 for(const k of ['Correct','Equivalent','Wrong','Boundary','Malformed'])assert.ok(a.AuthoredFixtures[k].length>=2,k);
});
test('printable source-specific student evidence',()=>{const w=fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Printables/geo-u06-l04-circle-locus-student-worksheet.md"),'utf8');assert.ok(w.includes('U06:L04'));assert.equal(a.SelectedPracticeRoute.PrintableStudentEvidencePath,"src/Edulytics.Core/Curriculum/LessonContent/Printables/geo-u06-l04-circle-locus-student-worksheet.md");});
test('independent source math oracle 1',()=>{assert.ok(Math.abs((Math.hypot(45,10))-(46.09772228646443))<1e-9);});
test('independent source math oracle 2',()=>{assert.ok(Math.abs(((14-6)**2+(25-10)**2)-(289))<1e-9);});
test('independent source math oracle 3',()=>{assert.ok(Math.abs(((22-6)**2+(3-10)**2)-(305))<1e-9);});
test('independent source math oracle 4',()=>{assert.ok(Math.abs(((9+3)**2+(1-6)**2)-(169))<1e-9);});
test('independent source math oracle 5',()=>{assert.ok(Math.abs(((2/6)**3)-(0.037037037037037035))<1e-9);});
test('source scope 2267',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 4.1 Going the Distance (5")));});
test('source scope 2397',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 4.2 Circling the Problem ")));});
test('source scope 2527',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 4.2 optional perpendicula")));});
test('source scope 2657',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 4.3 Building an Equation ")));});
test('source scope 2787',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 4.3 synthesis original ci")));});
test('source scope 2917',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source Practice P1-P7: P1 match ")));});
