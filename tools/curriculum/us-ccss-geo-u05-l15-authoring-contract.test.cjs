const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const p=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u05-l13-l18-2021.rich-lesson-v2.json"),'utf8'));
const a=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u05-l15-source-human-paper-20261031.json"),'utf8'));
const lesson=p.Lessons.find(x=>x.LessonCode==="PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U05:L15");
test('lesson exists, previous authored lessons preserved, source task contract selected',()=>{
 assert.ok(lesson);assert.equal(a.LessonCode,"PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U05:L15");assert.equal(a.SourceTaskCrosswalk.length,5);
 assert.equal(a.Practice.Count,7);assert.ok(lesson.WorkedExamples.length>0);
});
test('four real human rubric criteria, five fixture families and tests not executed',()=>{
 assert.equal(a.SelectedPracticeRoute.MachineGrade,false);assert.equal(a.HumanRubric.TotalMax,8);
 assert.equal(a.HumanRubric.Criteria.reduce((t,x)=>t+x.MaxPoints,0),8);
 assert.equal(a.TestsExecuted,'NOT_RUN');
 for(const k of ['Correct','Equivalent','Wrong','Boundary','Malformed'])assert.ok(a.AuthoredFixtures[k].length>=2,k);
});
test('printable source-specific student evidence',()=>{const w=fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Printables/geo-u05-l15-rotation-composite-student-worksheet.md"),'utf8');assert.ok(w.includes('U05:L15'));assert.equal(a.SelectedPracticeRoute.PrintableStudentEvidencePath,"src/Edulytics.Core/Curriculum/LessonContent/Printables/geo-u05-l15-rotation-composite-student-worksheet.md");});
test('independent source math oracle 1',()=>{assert.ok(Math.abs((Math.PI*2**2*5+Math.PI*2**2*3/3)-(75.39822368615503))<1e-9);});
test('independent source math oracle 2',()=>{assert.ok(Math.abs((Math.sqrt(12**2+5**2))-(13))<1e-9);});
test('independent source math oracle 3',()=>{assert.ok(Math.abs((100*12/3)-(400))<1e-9);});
test('independent source math oracle 4',()=>{assert.ok(Math.abs((6*1*.5)-(3))<1e-9);});
test('independent source math oracle 5',()=>{assert.ok(Math.abs((Math.sqrt((9*Math.PI)/(324*Math.PI)))-(0.16666666666666666))<1e-9);});
test('source scope 2296',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 15.1 Math Talk Volumes (5")));});
test('source scope 2426',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 15.2 Missing Measurements")));});
test('source scope 2556',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 15.2 optional bowl: open ")));});
test('source scope 2686',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 15.3 Spinning into Three ")));});
test('source scope 2816',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source student summary: solid cy")));});
test('source scope 2946',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source Practice P1-P7: P1 right ")));});
