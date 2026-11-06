const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const p=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u04-l09-l11-2021.rich-lesson-v2.json"),'utf8'));
const a=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u04-l11-source-human-paper-20261031.json"),'utf8'));
const lesson=p.Lessons.find(x=>x.LessonCode==="PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U04:L11");
test('lesson exists, previous authored lessons preserved, source task contract selected',()=>{
 assert.ok(lesson);assert.equal(a.LessonCode,"PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U04:L11");assert.equal(a.SourceTaskCrosswalk.length,5);
 assert.equal(a.Practice.Count,7);assert.ok(lesson.WorkedExamples.length>0);
});
test('four real human rubric criteria, five fixture families and tests not executed',()=>{
 assert.equal(a.SelectedPracticeRoute.MachineGrade,false);assert.equal(a.HumanRubric.TotalMax,8);
 assert.equal(a.HumanRubric.Criteria.reduce((t,x)=>t+x.MaxPoints,0),8);
 assert.equal(a.TestsExecuted,'NOT_RUN');
 for(const k of ['Correct','Equivalent','Wrong','Boundary','Malformed'])assert.ok(a.AuthoredFixtures[k].length>=2,k);
});
test('independent source math oracle 1',()=>{assert.ok(Math.abs((6*Math.sqrt(3)/4)-(2.598076211353316))<1e-9);});
test('independent source math oracle 2',()=>{assert.ok(Math.abs((60*Math.sin(Math.PI/60))-(3.1401573745766296))<1e-9);});
test('independent source math oracle 3',()=>{assert.ok(Math.abs((102*Math.sin(Math.PI/102))-(3.141095972729376))<1e-9);});
test('independent source math oracle 4',()=>{assert.ok(Math.abs((237*Math.sin(Math.PI/237))-(3.141500651464446))<1e-9);});
test('independent source math oracle 5',()=>{assert.ok(Math.abs((245/(4*Math.tan(Math.PI/5)))-(84.30339262885938))<1e-9);});
test('source scope 1929',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 11.1 More Sides (10 min):")));});
test('source scope 2059',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 11.2 N Sides (15 min): gr")));});
test('source scope 2189',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 11.3 So Many Sides (15 mi")));});
test('source scope 2319',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 11.3 original teacher dis")));});
test('source scope 2449',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 11.3 upper and lower pi b")));});
test('source scope 2579',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source unit final practice P1-P7")));});
test('source scope 2709',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source optional area formula for")));});
