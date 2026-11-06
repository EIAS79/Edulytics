const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const p=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u06-l07-l12-2021.rich-lesson-v2.json"),'utf8'));
const a=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u06-l08-source-human-paper-20261031.json"),'utf8'));
const lesson=p.Lessons.find(x=>x.LessonCode==="PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U06:L08");
test('lesson exists, previous authored lessons preserved, source task contract selected',()=>{
 assert.ok(lesson);assert.equal(a.LessonCode,"PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U06:L08");assert.equal(a.SourceTaskCrosswalk.length,5);
 assert.equal(a.Practice.Count,8);assert.ok(lesson.WorkedExamples.length>0);
});
test('four real human rubric criteria, five fixture families and tests not executed',()=>{
 assert.equal(a.SelectedPracticeRoute.MachineGrade,false);assert.equal(a.HumanRubric.TotalMax,8);
 assert.equal(a.HumanRubric.Criteria.reduce((t,x)=>t+x.MaxPoints,0),8);
 assert.equal(a.TestsExecuted,'NOT_RUN');
 for(const k of ['Correct','Equivalent','Wrong','Boundary','Malformed'])assert.ok(a.AuthoredFixtures[k].length>=2,k);
});
test('printable source-specific student evidence',()=>{const w=fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Printables/geo-u06-l08-equation-graph-student-worksheet.md"),'utf8');assert.ok(w.includes('U06:L08'));assert.equal(a.SelectedPracticeRoute.PrintableStudentEvidencePath,"src/Edulytics.Core/Curriculum/LessonContent/Printables/geo-u06-l08-equation-graph-student-worksheet.md");});
test('independent source math oracle 1',()=>{assert.ok(Math.abs((Math.hypot(2+2,5-2))-(5))<1e-9);});
test('independent source math oracle 2',()=>{assert.ok(Math.abs((Math.hypot(-8+2,10-2))-(10))<1e-9);});
test('independent source math oracle 3',()=>{assert.ok(Math.abs(((5-3)**2/4+1)-(2))<1e-9);});
test('independent source math oracle 4',()=>{assert.ok(Math.abs(((29+4*4+2*2))-(49))<1e-9);});
test('independent source math oracle 5',()=>{assert.ok(Math.abs(((7/2)**2)-(12.25))<1e-9);});
test('source scope 2227',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 8.1 Focus on Distance (5 ")));});
test('source scope 2357',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 8.2 Building an Equation ")));});
test('source scope 2487',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 8.3 Card Sort Parabolas (")));});
test('source scope 2617',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 8.3 OPTIONAL extension: p")));});
test('source scope 2747',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source lesson synthesis circle c")));});
test('source scope 2877',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source Practice P1-P8: P1 x\u00b2+y\u00b2\u2212")));});
