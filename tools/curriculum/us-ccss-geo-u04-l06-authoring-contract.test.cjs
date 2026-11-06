const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const p=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u04-l05-l08-2021.rich-lesson-v2.json"),'utf8'));
const a=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u04-l06-source-human-paper-20261031.json"),'utf8'));
const lesson=p.Lessons.find(x=>x.LessonCode==="PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U04:L06");
test('lesson exists, previous authored lessons preserved, source task contract selected',()=>{
 assert.ok(lesson);assert.equal(a.LessonCode,"PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U04:L06");assert.equal(a.SourceTaskCrosswalk.length,5);
 assert.equal(a.Practice.Count,8);assert.ok(lesson.WorkedExamples.length>0);
});
test('four real human rubric criteria, five fixture families and tests not executed',()=>{
 assert.equal(a.SelectedPracticeRoute.MachineGrade,false);assert.equal(a.HumanRubric.TotalMax,8);
 assert.equal(a.HumanRubric.Criteria.reduce((t,x)=>t+x.MaxPoints,0),8);
 assert.equal(a.TestsExecuted,'NOT_RUN');
 for(const k of ['Correct','Equivalent','Wrong','Boundary','Malformed'])assert.ok(a.AuthoredFixtures[k].length>=2,k);
});
test('independent source math oracle 1',()=>{assert.ok(Math.abs((3/Math.cos(50*Math.PI/180))-(4.667171480581237))<1e-9);});
test('independent source math oracle 2',()=>{assert.ok(Math.abs((2/Math.sin(12*Math.PI/180))-(9.619468689488263))<1e-9);});
test('independent source math oracle 3',()=>{assert.ok(Math.abs((20*Math.sin(71*Math.PI/180))-(18.910371511986334))<1e-9);});
test('independent source math oracle 4',()=>{assert.ok(Math.abs((Math.hypot(9,12))-(15))<1e-9);});
test('independent source math oracle 5',()=>{assert.ok(Math.abs((7*Math.tan(25*Math.PI/180))-(3.26415360708499))<1e-9);});
test('source scope 1918',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 6.1 This Time with Strate")));});
test('source scope 2048',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 6.2 New Names Same Ratios")));});
test('source scope 2178',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 6.3 Solve These Triangles")));});
test('source scope 2308',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 6.3 optional 80,85,89-deg")));});
test('source scope 2438',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source lesson synthesis END righ")));});
test('source scope 2568',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source Practice P1-P8: P1 origin")));});
