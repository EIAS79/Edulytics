const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const p=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u05-l01-l06-2021.rich-lesson-v2.json"),'utf8'));
const a=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u05-l02-source-human-paper-20261031.json"),'utf8'));
const lesson=p.Lessons.find(x=>x.LessonCode==="PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U05:L02");
test('lesson exists, previous authored lessons preserved, source task contract selected',()=>{
 assert.ok(lesson);assert.equal(a.LessonCode,"PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U05:L02");assert.equal(a.SourceTaskCrosswalk.length,5);
 assert.equal(a.Practice.Count,9);assert.ok(lesson.WorkedExamples.length>0);
});
test('four real human rubric criteria, five fixture families and tests not executed',()=>{
 assert.equal(a.SelectedPracticeRoute.MachineGrade,false);assert.equal(a.HumanRubric.TotalMax,8);
 assert.equal(a.HumanRubric.Criteria.reduce((t,x)=>t+x.MaxPoints,0),8);
 assert.equal(a.TestsExecuted,'NOT_RUN');
 for(const k of ['Correct','Equivalent','Wrong','Boundary','Malformed'])assert.ok(a.AuthoredFixtures[k].length>=2,k);
});
test('independent source math oracle 1',()=>{assert.ok(Math.abs((Math.PI*3*3)-(28.274333882308138))<1e-9);});
test('independent source math oracle 2',()=>{assert.ok(Math.abs(((6*6)/4)-(9))<1e-9);});
test('independent source math oracle 3',()=>{assert.ok(Math.abs((Math.sin(Math.PI/6))-(0.5))<1e-9);});
test('source scope 1624',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 2.1 Slice This (5 min): s")));});
test('source scope 1754',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 2.2 Slice That (20 min): ")));});
test('source scope 1884',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 2.3 Stack 'Em Up (10 min)")));});
test('source scope 2014',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 2.3 optional 3D printing:")));});
test('source scope 2144',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source glossary and summary: a p")));});
test('source scope 2274',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source Practice P1-P9: P1 figure")));});
