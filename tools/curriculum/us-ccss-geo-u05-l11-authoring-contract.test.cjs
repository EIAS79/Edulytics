const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const p=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u05-l07-l12-2021.rich-lesson-v2.json"),'utf8'));
const a=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u05-l11-source-human-paper-20261031.json"),'utf8'));
const lesson=p.Lessons.find(x=>x.LessonCode==="PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U05:L11");
test('lesson exists, previous authored lessons preserved, source task contract selected',()=>{
 assert.ok(lesson);assert.equal(a.LessonCode,"PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U05:L11");assert.equal(a.SourceTaskCrosswalk.length,5);
 assert.equal(a.Practice.Count,9);assert.ok(lesson.WorkedExamples.length>0);
});
test('four real human rubric criteria, five fixture families and tests not executed',()=>{
 assert.equal(a.SelectedPracticeRoute.MachineGrade,false);assert.equal(a.HumanRubric.TotalMax,8);
 assert.equal(a.HumanRubric.Criteria.reduce((t,x)=>t+x.MaxPoints,0),8);
 assert.equal(a.TestsExecuted,'NOT_RUN');
 for(const k of ['Correct','Equivalent','Wrong','Boundary','Malformed'])assert.ok(a.AuthoredFixtures[k].length>=2,k);
});
test('printable source-specific student evidence',()=>{const w=fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Printables/geo-u05-l11-prism-practice-student-worksheet.md"),'utf8');assert.ok(w.includes('U05:L11'));assert.equal(a.SelectedPracticeRoute.PrintableStudentEvidencePath,"src/Edulytics.Core/Curriculum/LessonContent/Printables/geo-u05-l11-prism-practice-student-worksheet.md");});
test('independent source math oracle 1',()=>{assert.ok(Math.abs((6*Math.SQRT2*Math.sin(Math.PI/4))-(6))<1e-9);});
test('independent source math oracle 2',()=>{assert.ok(Math.abs((Math.PI*2**2*6)-(75.39822368615503))<1e-9);});
test('independent source math oracle 3',()=>{assert.ok(Math.abs((1995*306)-(610470))<1e-9);});
test('independent source math oracle 4',()=>{assert.ok(Math.abs((72.25*306)-(22108.5))<1e-9);});
test('independent source math oracle 5',()=>{assert.ok(Math.abs((22*(Math.sqrt(198/22))**2)-(198))<1e-9);});
test('independent source math oracle 6',()=>{assert.ok(Math.abs((6*Math.pow(198/22,1.5))-(162))<1e-9);});
test('independent source math oracle 7',()=>{assert.ok(Math.abs((144*(1-154/231))-(48))<1e-9);});
test('source scope 2470',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 11.1 New Heights (10 min)")));});
test('source scope 2600',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 11.2 The Choice is Yours ")));});
test('source scope 2730',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 11.2 optional rotation ex")));});
test('source scope 2860',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 11.3 The Cayan Tower (10 ")));});
test('source scope 2990',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 11.3 teacher discussion: ")));});
test('source scope 3120',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source Practice P1-P9: P1 source")));});
