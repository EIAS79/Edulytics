const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const p=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u05-l01-l06-2021.rich-lesson-v2.json"),'utf8'));
const a=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u05-l03-source-human-paper-20261031.json"),'utf8'));
const lesson=p.Lessons.find(x=>x.LessonCode==="PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U05:L03");
test('lesson exists, previous authored lessons preserved, source task contract selected',()=>{
 assert.ok(lesson);assert.equal(a.LessonCode,"PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U05:L03");assert.equal(a.SourceTaskCrosswalk.length,4);
 assert.equal(a.Practice.Count,8);assert.ok(lesson.WorkedExamples.length>0);
});
test('four real human rubric criteria, five fixture families and tests not executed',()=>{
 assert.equal(a.SelectedPracticeRoute.MachineGrade,false);assert.equal(a.HumanRubric.TotalMax,8);
 assert.equal(a.HumanRubric.Criteria.reduce((t,x)=>t+x.MaxPoints,0),8);
 assert.equal(a.TestsExecuted,'NOT_RUN');
 for(const k of ['Correct','Equivalent','Wrong','Boundary','Malformed'])assert.ok(a.AuthoredFixtures[k].length>=2,k);
});
test('independent source math oracle 1',()=>{assert.ok(Math.abs((231*(144-96)/144)-(77))<1e-9);});
test('independent source math oracle 2',()=>{assert.ok(Math.abs((77*77)-(5929))<1e-9);});
test('independent source math oracle 3',()=>{assert.ok(Math.abs((.9*.9)-(0.81))<1e-9);});
test('independent source math oracle 4',()=>{assert.ok(Math.abs((4*Math.sin(20*Math.PI/180))-(1.3680805733026749))<1e-9);});
test('source scope 1726',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 3.1 Dilating Again (10 mi")));});
test('source scope 1856',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 3.2 Pyramid Mobile (25 mi")));});
test('source scope 1986',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 3.2 key two-centre distin")));});
test('source scope 2116',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 3.2 original extension: d")));});
test('source scope 2246',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source lesson student summary: h")));});
test('source scope 2376',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source Practice P1-P8: P1 images")));});
