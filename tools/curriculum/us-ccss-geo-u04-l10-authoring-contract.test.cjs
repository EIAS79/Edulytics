const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const p=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u04-l09-l11-2021.rich-lesson-v2.json"),'utf8'));
const a=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u04-l10-source-human-paper-20261031.json"),'utf8'));
const lesson=p.Lessons.find(x=>x.LessonCode==="PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U04:L10");
test('lesson exists, previous authored lessons preserved, source task contract selected',()=>{
 assert.ok(lesson);assert.equal(a.LessonCode,"PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U04:L10");assert.equal(a.SourceTaskCrosswalk.length,5);
 assert.equal(a.Practice.Count,8);assert.ok(lesson.WorkedExamples.length>0);
});
test('four real human rubric criteria, five fixture families and tests not executed',()=>{
 assert.equal(a.SelectedPracticeRoute.MachineGrade,false);assert.equal(a.HumanRubric.TotalMax,8);
 assert.equal(a.HumanRubric.Criteria.reduce((t,x)=>t+x.MaxPoints,0),8);
 assert.equal(a.TestsExecuted,'NOT_RUN');
 for(const k of ['Correct','Equivalent','Wrong','Boundary','Malformed'])assert.ok(a.AuthoredFixtures[k].length>=2,k);
});
test('independent source math oracle 1',()=>{assert.ok(Math.abs((4*Math.SQRT2)-(5.656854249492381))<1e-9);});
test('independent source math oracle 2',()=>{assert.ok(Math.abs((20*Math.sin(18*Math.PI/180))-(6.180339887498948))<1e-9);});
test('independent source math oracle 3',()=>{assert.ok(Math.abs((35000/(150*5280))-(0.04419191919191919))<1e-9);});
test('independent source math oracle 4',()=>{assert.ok(Math.abs((Math.sqrt(150**2+(35000/5280)**2))-(150.14639798790367))<1e-9);});
test('independent source math oracle 5',()=>{assert.ok(Math.abs((30*Math.sin(27*Math.PI/180))-(13.619714992186402))<1e-9);});
test('source scope 1931',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 10.1 Notice and Wonder Pr")));});
test('source scope 2061',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 10.2 Growing Regular Poly")));});
test('source scope 2191',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 10.2 optional extension: ")));});
test('source scope 2321',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 10.3 Gentle Descent (10 m")));});
test('source scope 2451',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source lesson synthesis 10.4: so")));});
test('source scope 2581',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source Practice P1-P8: P1 area o")));});
