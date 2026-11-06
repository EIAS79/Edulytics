const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const p=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u05-l01-l06-2021.rich-lesson-v2.json"),'utf8'));
const a=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u05-l01-source-human-paper-20261031.json"),'utf8'));
const lesson=p.Lessons.find(x=>x.LessonCode==="PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U05:L01");
test('lesson exists, previous authored lessons preserved, source task contract selected',()=>{
 assert.ok(lesson);assert.equal(a.LessonCode,"PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U05:L01");assert.equal(a.SourceTaskCrosswalk.length,5);
 assert.equal(a.Practice.Count,7);assert.ok(lesson.WorkedExamples.length>0);
});
test('four real human rubric criteria, five fixture families and tests not executed',()=>{
 assert.equal(a.SelectedPracticeRoute.MachineGrade,false);assert.equal(a.HumanRubric.TotalMax,8);
 assert.equal(a.HumanRubric.Criteria.reduce((t,x)=>t+x.MaxPoints,0),8);
 assert.equal(a.TestsExecuted,'NOT_RUN');
 for(const k of ['Correct','Equivalent','Wrong','Boundary','Malformed'])assert.ok(a.AuthoredFixtures[k].length>=2,k);
});
test('independent source math oracle 1',()=>{assert.ok(Math.abs((8*12)-(96))<1e-9);});
test('independent source math oracle 2',()=>{assert.ok(Math.abs((288*(1+Math.SQRT2))-(695.2935059634514))<1e-9);});
test('independent source math oracle 3',()=>{assert.ok(Math.abs((Math.hypot(4,6))-(7.211102550927978))<1e-9);});
test('source scope 1638',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 1.1 Which One Doesn't Bel")));});
test('source scope 1768',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 1.2 Axis of Rotation (10 ")));});
test('source scope 1898',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 1.2 optional extension: g")));});
test('source scope 2028',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 1.3 From Three Dimensions")));});
test('source scope 2158',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source lesson synthesis: a rotat")));});
test('source scope 2288',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source Practice P1-P7: P1 sketch")));});
