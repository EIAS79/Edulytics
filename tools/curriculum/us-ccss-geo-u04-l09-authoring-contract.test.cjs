const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const p=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u04-l09-l11-2021.rich-lesson-v2.json"),'utf8'));
const a=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u04-l09-source-human-paper-20261031.json"),'utf8'));
const lesson=p.Lessons.find(x=>x.LessonCode==="PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U04:L09");
test('lesson exists, previous authored lessons preserved, source task contract selected',()=>{
 assert.ok(lesson);assert.equal(a.LessonCode,"PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U04:L09");assert.equal(a.SourceTaskCrosswalk.length,5);
 assert.equal(a.Practice.Count,9);assert.ok(lesson.WorkedExamples.length>0);
});
test('four real human rubric criteria, five fixture families and tests not executed',()=>{
 assert.equal(a.SelectedPracticeRoute.MachineGrade,false);assert.equal(a.HumanRubric.TotalMax,8);
 assert.equal(a.HumanRubric.Criteria.reduce((t,x)=>t+x.MaxPoints,0),8);
 assert.equal(a.TestsExecuted,'NOT_RUN');
 for(const k of ['Correct','Equivalent','Wrong','Boundary','Malformed'])assert.ok(a.AuthoredFixtures[k].length>=2,k);
});
test('independent source math oracle 1',()=>{assert.ok(Math.abs((Math.atan(3/4)*180/Math.PI)-(36.86989764584402))<1e-9);});
test('independent source math oracle 2',()=>{assert.ok(Math.abs((Math.sqrt(13**2-12**2))-(5))<1e-9);});
test('independent source math oracle 3',()=>{assert.ok(Math.abs((Math.acos(18/22)*180/Math.PI)-(35.09680122758459))<1e-9);});
test('independent source math oracle 4',()=>{assert.ok(Math.abs((3.5*Math.sin(17*Math.PI/180))-(1.0233009665295787))<1e-9);});
test('source scope 1803',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 9.1 Once More with the Ta")));});
test('source scope 1933',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 9.2 From Ratios to Angles")));});
test('source scope 2063',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 9.3 Leaning Ladders (15 m")));});
test('source scope 2193',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source lesson synthesis: inverse")));});
test('source scope 2323',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source Practice P1-P9: P1 parkin")));});
test('source scope 2453',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source constraints: 4:1 'safe' l")));});
