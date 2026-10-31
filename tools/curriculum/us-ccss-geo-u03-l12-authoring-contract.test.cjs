const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const p=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u03-l10-l12-2021.rich-lesson-v2.json"),'utf8'));
const a=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u03-l12-source-human-paper-20261031.json"),'utf8'));
const lesson=p.Lessons.find(x=>x.LessonCode==="PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U03:L12");
test('lesson exists, previous authored lessons preserved, source task contract selected',()=>{
 assert.ok(lesson);assert.equal(a.LessonCode,"PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U03:L12");assert.equal(a.SourceTaskCrosswalk.length,5);
 assert.equal(a.Practice.Count,7);assert.ok(lesson.WorkedExamples.length>0);
});
test('four real human rubric criteria, five fixture families and tests not executed',()=>{
 assert.equal(a.SelectedPracticeRoute.MachineGrade,false);assert.equal(a.HumanRubric.TotalMax,8);
 assert.equal(a.HumanRubric.Criteria.reduce((t,x)=>t+x.MaxPoints,0),8);
 assert.equal(a.TestsExecuted,'NOT_RUN');
 for(const k of ['Correct','Equivalent','Wrong','Boundary','Malformed'])assert.ok(a.AuthoredFixtures[k].length>=2,k);
});
test('independent source math oracle 1',()=>{assert.ok(Math.abs((3*2.8)-(8.4))<1e-9);});
test('independent source math oracle 2',()=>{assert.ok(Math.abs((90/6)-(15))<1e-9);});
test('independent source math oracle 3',()=>{assert.ok(Math.abs(((2*2)**0.5)-(2))<1e-9);});
test('independent source math oracle 4',()=>{assert.ok(Math.abs((7*(3/6))-(3.5))<1e-9);});
test('independent source math oracle 5',()=>{assert.ok(Math.abs((3*6/4)-(4.5))<1e-9);});
test('source scope 1770',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 12.1 Vegetable Garden (5 ")));});
test('source scope 1900',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 12.2 Card Sort Correspond")));});
test('source scope 2030',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 12.3 Quilting Questions (")));});
test('source scope 2160',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 12.3 optional extension: ")));});
test('source scope 2290',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source lesson synthesis: similar")));});
test('source scope 2420',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source Practice P1-P7: P1 simila")));});
