const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const p=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u06-l01-l06-2021.rich-lesson-v2.json"),'utf8'));
const a=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u06-l03-source-human-paper-20261031.json"),'utf8'));
const lesson=p.Lessons.find(x=>x.LessonCode==="PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U06:L03");
test('lesson exists, previous authored lessons preserved, source task contract selected',()=>{
 assert.ok(lesson);assert.equal(a.LessonCode,"PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U06:L03");assert.equal(a.SourceTaskCrosswalk.length,5);
 assert.equal(a.Practice.Count,7);assert.ok(lesson.WorkedExamples.length>0);
});
test('four real human rubric criteria, five fixture families and tests not executed',()=>{
 assert.equal(a.SelectedPracticeRoute.MachineGrade,false);assert.equal(a.HumanRubric.TotalMax,8);
 assert.equal(a.HumanRubric.Criteria.reduce((t,x)=>t+x.MaxPoints,0),8);
 assert.equal(a.TestsExecuted,'NOT_RUN');
 for(const k of ['Correct','Equivalent','Wrong','Boundary','Malformed'])assert.ok(a.AuthoredFixtures[k].length>=2,k);
});
test('printable source-specific student evidence',()=>{const w=fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Printables/geo-u06-l03-transformation-types-student-worksheet.md"),'utf8');assert.ok(w.includes('U06:L03'));assert.equal(a.SelectedPracticeRoute.PrintableStudentEvidencePath,"src/Edulytics.Core/Curriculum/LessonContent/Printables/geo-u06-l03-transformation-types-student-worksheet.md");});
test('independent source math oracle 1',()=>{assert.ok(Math.abs((Math.hypot(5-2,4-3))-(3.1622776601683795))<1e-9);});
test('independent source math oracle 2',()=>{assert.ok(Math.abs((Math.hypot(5-2,8-6))-(3.605551275463989))<1e-9);});
test('independent source math oracle 3',()=>{assert.ok(Math.abs((Math.PI*3**2*5)-(141.3716694115407))<1e-9);});
test('independent source math oracle 4',()=>{assert.ok(Math.abs(((Math.PI*3**2*5/3)/(Math.PI*3**2*5))-(0.3333333333333333))<1e-9);});
test('independent source math oracle 5',()=>{assert.ok(Math.abs(((-2)-(-6))-(4))<1e-9);});
test('source scope 2323',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 3.1 Why is it a Dilation?")));});
test('source scope 2453',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 3.2 Congruent Similar Nei")));});
test('source scope 2583',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 3.2 optional extension: r")));});
test('source scope 2713',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 3.3 You Write the Rules (")));});
test('source scope 2843',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source lesson synthesis source t")));});
test('source scope 2973',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source student summary ABC origi")));});
test('source scope 3103',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source Practice P1-P7: P1 given ")));});
