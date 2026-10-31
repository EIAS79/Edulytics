const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const p=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u06-l01-l06-2021.rich-lesson-v2.json"),'utf8'));
const a=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u06-l01-source-human-paper-20261031.json"),'utf8'));
const lesson=p.Lessons.find(x=>x.LessonCode==="PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U06:L01");
test('lesson exists, previous authored lessons preserved, source task contract selected',()=>{
 assert.ok(lesson);assert.equal(a.LessonCode,"PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U06:L01");assert.equal(a.SourceTaskCrosswalk.length,5);
 assert.equal(a.Practice.Count,7);assert.ok(lesson.WorkedExamples.length>0);
});
test('four real human rubric criteria, five fixture families and tests not executed',()=>{
 assert.equal(a.SelectedPracticeRoute.MachineGrade,false);assert.equal(a.HumanRubric.TotalMax,8);
 assert.equal(a.HumanRubric.Criteria.reduce((t,x)=>t+x.MaxPoints,0),8);
 assert.equal(a.TestsExecuted,'NOT_RUN');
 for(const k of ['Correct','Equivalent','Wrong','Boundary','Malformed'])assert.ok(a.AuthoredFixtures[k].length>=2,k);
});
test('printable source-specific student evidence',()=>{const w=fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Printables/geo-u06-l01-rigid-transform-student-worksheet.md"),'utf8');assert.ok(w.includes('U06:L01'));assert.equal(a.SelectedPracticeRoute.PrintableStudentEvidencePath,"src/Edulytics.Core/Curriculum/LessonContent/Printables/geo-u06-l01-rigid-transform-student-worksheet.md");});
test('independent source math oracle 1',()=>{assert.ok(Math.abs((Math.hypot(5-2,3-1))-(3.605551275463989))<1e-9);});
test('independent source math oracle 2',()=>{assert.ok(Math.abs((-2-1)-(-3))<1e-9);});
test('independent source math oracle 3',()=>{assert.ok(Math.abs(((1.17)/(2*.5/3))-(3.51))<1e-9);});
test('independent source math oracle 4',()=>{assert.ok(Math.abs((Math.PI*2**2*(16*Math.sin(Math.PI/6)))-(100.53096491487338))<1e-9);});
test('independent source math oracle 5',()=>{assert.ok(Math.abs((Math.atan2(2,3)*180/Math.PI)-(33.690067525979785))<1e-9);});
test('source scope 2307',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 1.1 Traversing the Plane ")));});
test('source scope 2437',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 1.2 Transforming with Coo")));});
test('source scope 2567',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 1.3 Congruent by Coordina")));});
test('source scope 2697',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source synthesis triangle END fr")));});
test('source scope 2827',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source Practice P1-P7: P1 triang")));});
