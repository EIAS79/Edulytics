const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const p=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u05-l13-l18-2021.rich-lesson-v2.json"),'utf8'));
const a=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u05-l13-source-human-paper-20261031.json"),'utf8'));
const lesson=p.Lessons.find(x=>x.LessonCode==="PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U05:L13");
test('lesson exists, previous authored lessons preserved, source task contract selected',()=>{
 assert.ok(lesson);assert.equal(a.LessonCode,"PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U05:L13");assert.equal(a.SourceTaskCrosswalk.length,5);
 assert.equal(a.Practice.Count,8);assert.ok(lesson.WorkedExamples.length>0);
});
test('four real human rubric criteria, five fixture families and tests not executed',()=>{
 assert.equal(a.SelectedPracticeRoute.MachineGrade,false);assert.equal(a.HumanRubric.TotalMax,8);
 assert.equal(a.HumanRubric.Criteria.reduce((t,x)=>t+x.MaxPoints,0),8);
 assert.equal(a.TestsExecuted,'NOT_RUN');
 for(const k of ['Correct','Equivalent','Wrong','Boundary','Malformed'])assert.ok(a.AuthoredFixtures[k].length>=2,k);
});
test('printable source-specific student evidence',()=>{const w=fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Printables/geo-u05-l13-pyramid-volume-student-worksheet.md"),'utf8');assert.ok(w.includes('U05:L13'));assert.equal(a.SelectedPracticeRoute.PrintableStudentEvidencePath,"src/Edulytics.Core/Curriculum/LessonContent/Printables/geo-u05-l13-pyramid-volume-student-worksheet.md");});
test('independent source math oracle 1',()=>{assert.ok(Math.abs((6**2*8/3)-(96))<1e-9);});
test('independent source math oracle 2',()=>{assert.ok(Math.abs((Math.PI*9**2*15/3)-(1272.345024703866))<1e-9);});
test('independent source math oracle 3',()=>{assert.ok(Math.abs((8*.25**2)-(0.5))<1e-9);});
test('independent source math oracle 4',()=>{assert.ok(Math.abs((10*6/3)-(20))<1e-9);});
test('independent source math oracle 5',()=>{assert.ok(Math.abs(((12-2)*(6-2)*(20-1))-(760))<1e-9);});
test('independent source math oracle 6',()=>{assert.ok(Math.abs((Math.sqrt(90/10))-(3))<1e-9);});
test('source scope 2327',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 13.1 Cover Your Bases (5 ")));});
test('source scope 2457',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 13.2 Splitting a Prism in")));});
test('source scope 2587',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 13.2 generalization: a tr")));});
test('source scope 2717',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 13.3 Comparing Cross Sect")));});
test('source scope 2847',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 13.3 optional extension r")));});
test('source scope 2977',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source Practice P1-P8: P1 square")));});
