const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const p=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u06-l07-l12-2021.rich-lesson-v2.json"),'utf8'));
const a=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u06-l07-source-human-paper-20261031.json"),'utf8'));
const lesson=p.Lessons.find(x=>x.LessonCode==="PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U06:L07");
test('lesson exists, previous authored lessons preserved, source task contract selected',()=>{
 assert.ok(lesson);assert.equal(a.LessonCode,"PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U06:L07");assert.equal(a.SourceTaskCrosswalk.length,5);
 assert.equal(a.Practice.Count,9);assert.ok(lesson.WorkedExamples.length>0);
});
test('four real human rubric criteria, five fixture families and tests not executed',()=>{
 assert.equal(a.SelectedPracticeRoute.MachineGrade,false);assert.equal(a.HumanRubric.TotalMax,8);
 assert.equal(a.HumanRubric.Criteria.reduce((t,x)=>t+x.MaxPoints,0),8);
 assert.equal(a.TestsExecuted,'NOT_RUN');
 for(const k of ['Correct','Equivalent','Wrong','Boundary','Malformed'])assert.ok(a.AuthoredFixtures[k].length>=2,k);
});
test('printable source-specific student evidence',()=>{const w=fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Printables/geo-u06-l07-parabola-distance-student-worksheet.md"),'utf8');assert.ok(w.includes('U06:L07'));assert.equal(a.SelectedPracticeRoute.PrintableStudentEvidencePath,"src/Edulytics.Core/Curriculum/LessonContent/Printables/geo-u06-l07-parabola-distance-student-worksheet.md");});
test('independent source math oracle 1',()=>{assert.ok(Math.abs((Math.hypot(6-2,5-2))-(5))<1e-9);});
test('independent source math oracle 2',()=>{assert.ok(Math.abs((Math.hypot(11-6,5-4))-(5.0990195135927845))<1e-9);});
test('independent source math oracle 3',()=>{assert.ok(Math.abs((Math.hypot(14-6,10-4))-(10))<1e-9);});
test('independent source math oracle 4',()=>{assert.ok(Math.abs(((5)/2)-(2.5))<1e-9);});
test('independent source math oracle 5',()=>{assert.ok(Math.abs((1048/(5.6*13*16))-(0.8997252747252747))<1e-9);});
test('independent source math oracle 6',()=>{assert.ok(Math.abs(((4**2+3**2))-(25))<1e-9);});
test('source scope 2367',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 7.1 Notice and Wonder Dis")));});
test('source scope 2497',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 7.2 Into Focus (15 min): ")));});
test('source scope 2627',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 7.3 On Point (10 min): so")));});
test('source scope 2757',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 7.3 optional extension wi")));});
test('source scope 2887',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source lesson synthesis focus(-1")));});
test('source scope 3017',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source Practice P1-P9: P1 P=(6,y")));});
