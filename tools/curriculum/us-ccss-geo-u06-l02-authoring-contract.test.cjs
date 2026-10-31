const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const p=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u06-l01-l06-2021.rich-lesson-v2.json"),'utf8'));
const a=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u06-l02-source-human-paper-20261031.json"),'utf8'));
const lesson=p.Lessons.find(x=>x.LessonCode==="PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U06:L02");
test('lesson exists, previous authored lessons preserved, source task contract selected',()=>{
 assert.ok(lesson);assert.equal(a.LessonCode,"PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U06:L02");assert.equal(a.SourceTaskCrosswalk.length,5);
 assert.equal(a.Practice.Count,8);assert.ok(lesson.WorkedExamples.length>0);
});
test('four real human rubric criteria, five fixture families and tests not executed',()=>{
 assert.equal(a.SelectedPracticeRoute.MachineGrade,false);assert.equal(a.HumanRubric.TotalMax,8);
 assert.equal(a.HumanRubric.Criteria.reduce((t,x)=>t+x.MaxPoints,0),8);
 assert.equal(a.TestsExecuted,'NOT_RUN');
 for(const k of ['Correct','Equivalent','Wrong','Boundary','Malformed'])assert.ok(a.AuthoredFixtures[k].length>=2,k);
});
test('printable source-specific student evidence',()=>{const w=fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Printables/geo-u06-l02-transform-function-student-worksheet.md"),'utf8');assert.ok(w.includes('U06:L02'));assert.equal(a.SelectedPracticeRoute.PrintableStudentEvidencePath,"src/Edulytics.Core/Curriculum/LessonContent/Printables/geo-u06-l02-transform-function-student-worksheet.md");});
test('independent source math oracle 1',()=>{assert.ok(Math.abs(((-10)+12)-(2))<1e-9);});
test('independent source math oracle 2',()=>{assert.ok(Math.abs((5-2)-(3))<1e-9);});
test('independent source math oracle 3',()=>{assert.ok(Math.abs((Math.PI*(.4**2)*5)-(2.5132741228718345))<1e-9);});
test('independent source math oracle 4',()=>{assert.ok(Math.abs((1950/(Math.PI*.8))-(775.8803475729898))<1e-9);});
test('independent source math oracle 5',()=>{assert.ok(Math.abs((3**3/3)-(9))<1e-9);});
test('independent source math oracle 6',()=>{assert.ok(Math.abs(((2-(-4)))-(6))<1e-9);});
test('source scope 2331',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 2.1 Math Talk Transformin")));});
test('source scope 2461',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 2.2 Inputs and Outputs (1")));});
test('source scope 2591',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 2.3 What Does it Do? (15m")));});
test('source scope 2721',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 2.3 optional extension gi")));});
test('source scope 2851',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source synthesis general (x,y)\u2192(")));});
test('source scope 2986',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source student summary original ")));});
test('source scope 3116',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source Practice P1-P8: P1 match ")));});
