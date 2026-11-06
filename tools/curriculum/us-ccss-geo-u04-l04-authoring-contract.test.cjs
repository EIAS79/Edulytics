const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const p=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u04-l01-l04-2021.rich-lesson-v2.json"),'utf8'));
const a=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u04-l04-source-human-paper-20261031.json"),'utf8'));
const lesson=p.Lessons.find(x=>x.LessonCode==="PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U04:L04");
test('lesson exists, previous authored lessons preserved, source task contract selected',()=>{
 assert.ok(lesson);assert.equal(a.LessonCode,"PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U04:L04");assert.equal(a.SourceTaskCrosswalk.length,5);
 assert.equal(a.Practice.Count,8);assert.ok(lesson.WorkedExamples.length>0);
});
test('four real human rubric criteria, five fixture families and tests not executed',()=>{
 assert.equal(a.SelectedPracticeRoute.MachineGrade,false);assert.equal(a.HumanRubric.TotalMax,8);
 assert.equal(a.HumanRubric.Criteria.reduce((t,x)=>t+x.MaxPoints,0),8);
 assert.equal(a.TestsExecuted,'NOT_RUN');
 for(const k of ['Correct','Equivalent','Wrong','Boundary','Malformed'])assert.ok(a.AuthoredFixtures[k].length>=2,k);
});
test('independent source math oracle 1',()=>{assert.ok(Math.abs((Math.sin(42*Math.PI/180))-(0.6691306063588582))<1e-9);});
test('independent source math oracle 2',()=>{assert.ok(Math.abs((Math.sqrt(2*2))-(2))<1e-9);});
test('independent source math oracle 3',()=>{assert.ok(Math.abs((Math.sqrt(4*4+4*4)/2)-(2.8284271247461903))<1e-9);});
test('independent source math oracle 4',()=>{assert.ok(Math.abs(((8*8*Math.sqrt(3))/4)-(27.712812921102035))<1e-9);});
test('independent source math oracle 5',()=>{assert.ok(Math.abs((0.574/0.819)-(0.7008547008547008))<1e-9);});
test('source scope 1889',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 4.1 Ratio Rivalry (5 min)")));});
test('source scope 2019',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 4.2 Tons of Triangles (15")));});
test('source scope 2149',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 4.2 full source numerical")));});
test('source scope 2279',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 4.3 Tons of Ratios (15 mi")));});
test('source scope 2409',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 4.3 error inquiry: halvin")));});
test('source scope 2539',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source lesson synthesis and stud")));});
test('source scope 2669',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source Practice P1-P8: P1 opposi")));});
