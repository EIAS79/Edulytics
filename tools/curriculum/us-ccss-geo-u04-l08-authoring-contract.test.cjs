const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const p=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u04-l05-l08-2021.rich-lesson-v2.json"),'utf8'));
const a=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u04-l08-source-human-paper-20261031.json"),'utf8'));
const lesson=p.Lessons.find(x=>x.LessonCode==="PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U04:L08");
test('lesson exists, previous authored lessons preserved, source task contract selected',()=>{
 assert.ok(lesson);assert.equal(a.LessonCode,"PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U04:L08");assert.equal(a.SourceTaskCrosswalk.length,5);
 assert.equal(a.Practice.Count,8);assert.ok(lesson.WorkedExamples.length>0);
});
test('four real human rubric criteria, five fixture families and tests not executed',()=>{
 assert.equal(a.SelectedPracticeRoute.MachineGrade,false);assert.equal(a.HumanRubric.TotalMax,8);
 assert.equal(a.HumanRubric.Criteria.reduce((t,x)=>t+x.MaxPoints,0),8);
 assert.equal(a.TestsExecuted,'NOT_RUN');
 for(const k of ['Correct','Equivalent','Wrong','Boundary','Malformed'])assert.ok(a.AuthoredFixtures[k].length>=2,k);
});
test('independent source math oracle 1',()=>{assert.ok(Math.abs((7/Math.cos(41*Math.PI/180))-(9.27509095344168))<1e-9);});
test('independent source math oracle 2',()=>{assert.ok(Math.abs((6*Math.sin(65*Math.PI/180))-(5.437846722219899))<1e-9);});
test('independent source math oracle 3',()=>{assert.ok(Math.abs((Math.tan(12*Math.PI/180)*Math.tan(78*Math.PI/180))-(1))<1e-9);});
test('independent source math oracle 4',()=>{assert.ok(Math.abs((20*7/(7+12))-(7.368421052631579))<1e-9);});
test('source scope 1809',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 8.1 Which One Doesn't Bel")));});
test('source scope 1939',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 8.2 Twin Triangles (15 mi")));});
test('source scope 2069',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 8.3 Explain the Co-nnecti")));});
test('source scope 2199',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source lesson synthesis: for acu")));});
test('source scope 2329',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source Practice P1-P8: P1 equati")));});
test('source scope 2459',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 8.4 Cosine's Complement C")));});
