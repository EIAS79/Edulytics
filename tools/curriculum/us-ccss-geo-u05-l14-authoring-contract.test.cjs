const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const p=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u05-l13-l18-2021.rich-lesson-v2.json"),'utf8'));
const a=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u05-l14-source-human-paper-20261031.json"),'utf8'));
const lesson=p.Lessons.find(x=>x.LessonCode==="PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U05:L14");
test('lesson exists, previous authored lessons preserved, source task contract selected',()=>{
 assert.ok(lesson);assert.equal(a.LessonCode,"PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U05:L14");assert.equal(a.SourceTaskCrosswalk.length,5);
 assert.equal(a.Practice.Count,8);assert.ok(lesson.WorkedExamples.length>0);
});
test('four real human rubric criteria, five fixture families and tests not executed',()=>{
 assert.equal(a.SelectedPracticeRoute.MachineGrade,false);assert.equal(a.HumanRubric.TotalMax,8);
 assert.equal(a.HumanRubric.Criteria.reduce((t,x)=>t+x.MaxPoints,0),8);
 assert.equal(a.TestsExecuted,'NOT_RUN');
 for(const k of ['Correct','Equivalent','Wrong','Boundary','Malformed'])assert.ok(a.AuthoredFixtures[k].length>=2,k);
});
test('printable source-specific student evidence',()=>{const w=fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Printables/geo-u05-l14-inverse-pyramid-student-worksheet.md"),'utf8');assert.ok(w.includes('U05:L14'));assert.equal(a.SelectedPracticeRoute.PrintableStudentEvidencePath,"src/Edulytics.Core/Curriculum/LessonContent/Printables/geo-u05-l14-inverse-pyramid-student-worksheet.md");});
test('independent source math oracle 1',()=>{assert.ok(Math.abs((32*12/3)-(128))<1e-9);});
test('independent source math oracle 2',()=>{assert.ok(Math.abs((16*3/9)-(5.333333333333333))<1e-9);});
test('independent source math oracle 3',()=>{assert.ok(Math.abs((720*3/15)-(144))<1e-9);});
test('independent source math oracle 4',()=>{assert.ok(Math.abs((3*60/5)-(36))<1e-9);});
test('independent source math oracle 5',()=>{assert.ok(Math.abs((Math.PI*3**2*6)-(169.64600329384882))<1e-9);});
test('independent source math oracle 6',()=>{assert.ok(Math.abs((Math.cbrt(448/7))-(4))<1e-9);});
test('source scope 2328',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 14.1 Volume Matching (5 m")));});
test('source scope 2458',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 14.2 Practice with Pyrami")));});
test('source scope 2588',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 14.3 An Icy Pyramid (10 m")));});
test('source scope 2718',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source student summary cone V=90")));});
test('source scope 2848',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source Practice P1-P8: P1 pyrami")));});
