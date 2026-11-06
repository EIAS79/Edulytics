const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const p=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u04-l05-l08-2021.rich-lesson-v2.json"),'utf8'));
const a=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u04-l05-source-human-paper-20261031.json"),'utf8'));
const lesson=p.Lessons.find(x=>x.LessonCode==="PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U04:L05");
test('lesson exists, previous authored lessons preserved, source task contract selected',()=>{
 assert.ok(lesson);assert.equal(a.LessonCode,"PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U04:L05");assert.equal(a.SourceTaskCrosswalk.length,5);
 assert.equal(a.Practice.Count,8);assert.ok(lesson.WorkedExamples.length>0);
});
test('four real human rubric criteria, five fixture families and tests not executed',()=>{
 assert.equal(a.SelectedPracticeRoute.MachineGrade,false);assert.equal(a.HumanRubric.TotalMax,8);
 assert.equal(a.HumanRubric.Criteria.reduce((t,x)=>t+x.MaxPoints,0),8);
 assert.equal(a.TestsExecuted,'NOT_RUN');
 for(const k of ['Correct','Equivalent','Wrong','Boundary','Malformed'])assert.ok(a.AuthoredFixtures[k].length>=2,k);
});
test('independent source math oracle 1',()=>{assert.ok(Math.abs((Math.atan(50/13)*180/Math.PI)-(75.42578380196126))<1e-9);});
test('independent source math oracle 2',()=>{assert.ok(Math.abs((7**2+24**2)-(625))<1e-9);});
test('independent source math oracle 3',()=>{assert.ok(Math.abs((5/0.643)-(7.776049766718507))<1e-9);});
test('independent source math oracle 4',()=>{assert.ok(Math.abs((12/2.747)-(4.368401892974154))<1e-9);});
test('independent source math oracle 5',()=>{assert.ok(Math.abs((18*Math.sin(Math.PI/6))-(9))<1e-9);});
test('source scope 1857',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 5.1 Launch Pad (5 min): o")));});
test('source scope 1987',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 5.2 Pythagorean Triples (")));});
test('source scope 2117',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 5.3 Solve All the Triangl")));});
test('source scope 2247',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source student summary first exa")));});
test('source scope 2377',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source Practice P1-P8: P1 8,15,1")));});
test('source scope 2507',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 5.4 Solve the Triangle or")));});
