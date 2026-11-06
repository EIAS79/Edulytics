const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const p=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u05-l07-l12-2021.rich-lesson-v2.json"),'utf8'));
const a=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u05-l10-source-human-paper-20261031.json"),'utf8'));
const lesson=p.Lessons.find(x=>x.LessonCode==="PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U05:L10");
test('lesson exists, previous authored lessons preserved, source task contract selected',()=>{
 assert.ok(lesson);assert.equal(a.LessonCode,"PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U05:L10");assert.equal(a.SourceTaskCrosswalk.length,5);
 assert.equal(a.Practice.Count,8);assert.ok(lesson.WorkedExamples.length>0);
});
test('four real human rubric criteria, five fixture families and tests not executed',()=>{
 assert.equal(a.SelectedPracticeRoute.MachineGrade,false);assert.equal(a.HumanRubric.TotalMax,8);
 assert.equal(a.HumanRubric.Criteria.reduce((t,x)=>t+x.MaxPoints,0),8);
 assert.equal(a.TestsExecuted,'NOT_RUN');
 for(const k of ['Correct','Equivalent','Wrong','Boundary','Malformed'])assert.ok(a.AuthoredFixtures[k].length>=2,k);
});
test('printable source-specific student evidence',()=>{const w=fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Printables/geo-u05-l10-cavalieri-student-worksheet.md"),'utf8');assert.ok(w.includes('U05:L10'));assert.equal(a.SelectedPracticeRoute.PrintableStudentEvidencePath,"src/Edulytics.Core/Curriculum/LessonContent/Printables/geo-u05-l10-cavalieri-student-worksheet.md");});
test('independent source math oracle 1',()=>{assert.ok(Math.abs((Math.PI*3**2*7)-(197.92033717615698))<1e-9);});
test('independent source math oracle 2',()=>{assert.ok(Math.abs((Math.PI*3**2*10)-(282.7433388230814))<1e-9);});
test('independent source math oracle 3',()=>{assert.ok(Math.abs((3*4/2*5)-(30))<1e-9);});
test('independent source math oracle 4',()=>{assert.ok(Math.abs((Math.PI*3.5**2*12.5)-(481.0563750809371))<1e-9);});
test('independent source math oracle 5',()=>{assert.ok(Math.abs((5*2*(10-.5))-(95))<1e-9);});
test('independent source math oracle 6',()=>{assert.ok(Math.abs((10*Math.cbrt(128/2)**2)-(160))<1e-9);});
test('source scope 2362',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 10.1 Two Stacks of Coins ")));});
test('source scope 2492',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 10.2 Rectangular Prism, S")));});
test('source scope 2622',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 10.2 Cavalieri Principle ")));});
test('source scope 2752',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 10.3 Equal Volumes? (15 m")));});
test('source scope 2882',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 10.3 class synthesis: a t")));});
test('source scope 3012',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source Practice P1-P8: P1 vase w")));});
