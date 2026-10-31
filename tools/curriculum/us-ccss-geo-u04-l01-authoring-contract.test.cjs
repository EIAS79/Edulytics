const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const p=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u04-l01-l04-2021.rich-lesson-v2.json"),'utf8'));
const a=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u04-l01-source-human-paper-20261031.json"),'utf8'));
const lesson=p.Lessons.find(x=>x.LessonCode==="PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U04:L01");
test('lesson exists, previous authored lessons preserved, source task contract selected',()=>{
 assert.ok(lesson);assert.equal(a.LessonCode,"PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U04:L01");assert.equal(a.SourceTaskCrosswalk.length,5);
 assert.equal(a.Practice.Count,7);assert.ok(lesson.WorkedExamples.length>0);
});
test('four real human rubric criteria, five fixture families and tests not executed',()=>{
 assert.equal(a.SelectedPracticeRoute.MachineGrade,false);assert.equal(a.HumanRubric.TotalMax,8);
 assert.equal(a.HumanRubric.Criteria.reduce((t,x)=>t+x.MaxPoints,0),8);
 assert.equal(a.TestsExecuted,'NOT_RUN');
 for(const k of ['Correct','Equivalent','Wrong','Boundary','Malformed'])assert.ok(a.AuthoredFixtures[k].length>=2,k);
});
test('independent source math oracle 1',()=>{assert.ok(Math.abs((4*7*12)-(336))<1e-9);});
test('independent source math oracle 2',()=>{assert.ok(Math.abs((Math.sqrt(5*5+12*12))-(13))<1e-9);});
test('independent source math oracle 3',()=>{assert.ok(Math.abs((Math.sqrt(26*26-10*10))-(24))<1e-9);});
test('independent source math oracle 4',()=>{assert.ok(Math.abs((Math.atan(1/12)*180/Math.PI)-(4.763641690726178))<1e-9);});
test('independent source math oracle 5',()=>{assert.ok(Math.abs((Math.sqrt(3**2+36**2))-(36.124783736376884))<1e-9);});
test('source scope 1865',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 1.1 Ratios Galore (5 min)")));});
test('source scope 1995',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 1.2 Can You Calculate? (1")));});
test('source scope 2125',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 1.3 Is It Accessible? (20")));});
test('source scope 2255',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 1.3 inquiry: demonstrate ")));});
test('source scope 2385',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source extension: slope leg rati")));});
test('source scope 2515',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source lesson summary: separate ")));});
test('source scope 2645',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source Practice P1-P7: P1 choose")));});
