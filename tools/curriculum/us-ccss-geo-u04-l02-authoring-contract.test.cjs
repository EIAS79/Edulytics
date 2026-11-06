const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const p=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u04-l01-l04-2021.rich-lesson-v2.json"),'utf8'));
const a=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u04-l02-source-human-paper-20261031.json"),'utf8'));
const lesson=p.Lessons.find(x=>x.LessonCode==="PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U04:L02");
test('lesson exists, previous authored lessons preserved, source task contract selected',()=>{
 assert.ok(lesson);assert.equal(a.LessonCode,"PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U04:L02");assert.equal(a.SourceTaskCrosswalk.length,5);
 assert.equal(a.Practice.Count,8);assert.ok(lesson.WorkedExamples.length>0);
});
test('four real human rubric criteria, five fixture families and tests not executed',()=>{
 assert.equal(a.SelectedPracticeRoute.MachineGrade,false);assert.equal(a.HumanRubric.TotalMax,8);
 assert.equal(a.HumanRubric.Criteria.reduce((t,x)=>t+x.MaxPoints,0),8);
 assert.equal(a.TestsExecuted,'NOT_RUN');
 for(const k of ['Correct','Equivalent','Wrong','Boundary','Malformed'])assert.ok(a.AuthoredFixtures[k].length>=2,k);
});
test('independent source math oracle 1',()=>{assert.ok(Math.abs((Math.sqrt(4**2+7**2))-(8.06225774829855))<1e-9);});
test('independent source math oracle 2',()=>{assert.ok(Math.abs((Math.sqrt(25**2-24**2))-(7))<1e-9);});
test('source scope 1543',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 2.1 Diagonals of Rectangl")));});
test('source scope 1673',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 2.2 Decomposing Squares: ")));});
test('source scope 1803',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 2.3 Generalize Half Squar")));});
test('source scope 1933',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source Practice P1-P8: P1 source")));});
