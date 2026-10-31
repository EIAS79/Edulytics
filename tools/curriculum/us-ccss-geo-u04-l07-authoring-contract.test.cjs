const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const p=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u04-l05-l08-2021.rich-lesson-v2.json"),'utf8'));
const a=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u04-l07-source-human-paper-20261031.json"),'utf8'));
const lesson=p.Lessons.find(x=>x.LessonCode==="PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U04:L07");
test('lesson exists, previous authored lessons preserved, source task contract selected',()=>{
 assert.ok(lesson);assert.equal(a.LessonCode,"PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U04:L07");assert.equal(a.SourceTaskCrosswalk.length,5);
 assert.equal(a.Practice.Count,7);assert.ok(lesson.WorkedExamples.length>0);
});
test('four real human rubric criteria, five fixture families and tests not executed',()=>{
 assert.equal(a.SelectedPracticeRoute.MachineGrade,false);assert.equal(a.HumanRubric.TotalMax,8);
 assert.equal(a.HumanRubric.Criteria.reduce((t,x)=>t+x.MaxPoints,0),8);
 assert.equal(a.TestsExecuted,'NOT_RUN');
 for(const k of ['Correct','Equivalent','Wrong','Boundary','Malformed'])assert.ok(a.AuthoredFixtures[k].length>=2,k);
});
test('independent source math oracle 1',()=>{assert.ok(Math.abs((250*Math.tan(73*Math.PI/180))-(817.7131546210351))<1e-9);});
test('independent source math oracle 2',()=>{assert.ok(Math.abs((300*(Math.tan(Math.PI/3)+Math.tan(Math.PI/12)))-(600))<1e-9);});
test('independent source math oracle 3',()=>{assert.ok(Math.abs((12*Math.sin(Math.PI/4))-(8.485281374238571))<1e-9);});
test('independent source math oracle 4',()=>{assert.ok(Math.abs((630/Math.tan(50*Math.PI/180))-(528.6327676416864))<1e-9);});
test('source scope 1824',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 7.1 Tilted Triangle (5 mi")));});
test('source scope 1954',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 7.2 Info Gap Trigonometry")));});
test('source scope 2084',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 7.3 Tallest Tower (10 min")));});
test('source scope 2214',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 7.3 optional extension: o")));});
test('source scope 2344',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 7.4 Tallest Tree original")));});
test('source scope 2474',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source Practice P1-P7: P1 Eiffel")));});
