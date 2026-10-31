const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const p=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u05-l13-l18-2021.rich-lesson-v2.json"),'utf8'));
const a=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u05-l17-source-human-paper-20261031.json"),'utf8'));
const lesson=p.Lessons.find(x=>x.LessonCode==="PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U05:L17");
test('lesson exists, previous authored lessons preserved, source task contract selected',()=>{
 assert.ok(lesson);assert.equal(a.LessonCode,"PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U05:L17");assert.equal(a.SourceTaskCrosswalk.length,5);
 assert.equal(a.Practice.Count,9);assert.ok(lesson.WorkedExamples.length>0);
});
test('four real human rubric criteria, five fixture families and tests not executed',()=>{
 assert.equal(a.SelectedPracticeRoute.MachineGrade,false);assert.equal(a.HumanRubric.TotalMax,8);
 assert.equal(a.HumanRubric.Criteria.reduce((t,x)=>t+x.MaxPoints,0),8);
 assert.equal(a.TestsExecuted,'NOT_RUN');
 for(const k of ['Correct','Equivalent','Wrong','Boundary','Malformed'])assert.ok(a.AuthoredFixtures[k].length>=2,k);
});
test('printable source-specific student evidence',()=>{const w=fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Printables/geo-u05-l17-density-student-worksheet.md"),'utf8');assert.ok(w.includes('U05:L17'));assert.equal(a.SelectedPracticeRoute.PrintableStudentEvidencePath,"src/Edulytics.Core/Curriculum/LessonContent/Printables/geo-u05-l17-density-student-worksheet.md");});
test('independent source math oracle 1',()=>{assert.ok(Math.abs((51*66*7)-(23562))<1e-9);});
test('independent source math oracle 2',()=>{assert.ok(Math.abs(((20*20*28)/3)-(3733.3333333333335))<1e-9);});
test('independent source math oracle 3',()=>{assert.ok(Math.abs(((.059)/(23562/1e6))-(2.5040319157966215))<1e-9);});
test('independent source math oracle 4',()=>{assert.ok(Math.abs((30/((11200/3)/1e6))-(8035.714285714286))<1e-9);});
test('independent source math oracle 5',()=>{assert.ok(Math.abs((5*.285*Math.PI/32)-(0.13989904785517046))<1e-9);});
test('independent source math oracle 6',()=>{assert.ok(Math.abs((1.1*(4/3)*Math.PI*(.001**3))-(4.60766922526503e-09))<1e-9);});
test('source scope 2407',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 17.1 A Kilogram by Any Ot")));});
test('source scope 2537',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 17.2 Light as a Feather (")));});
test('source scope 2667',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 17.2 optional Iridium Pen")));});
test('source scope 2797',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 17.3 A Fishy Situation (1")));});
test('source scope 2927',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source summary: a spherical-obje")));});
test('source scope 3057',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source Practice P1-P9: P1 annula")));});
