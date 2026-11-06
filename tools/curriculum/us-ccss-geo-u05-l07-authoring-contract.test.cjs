const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const p=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u05-l07-l12-2021.rich-lesson-v2.json"),'utf8'));
const a=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u05-l07-source-human-paper-20261031.json"),'utf8'));
const lesson=p.Lessons.find(x=>x.LessonCode==="PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U05:L07");
test('lesson exists, previous authored lessons preserved, source task contract selected',()=>{
 assert.ok(lesson);assert.equal(a.LessonCode,"PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U05:L07");assert.equal(a.SourceTaskCrosswalk.length,5);
 assert.equal(a.Practice.Count,8);assert.ok(lesson.WorkedExamples.length>0);
});
test('four real human rubric criteria, five fixture families and tests not executed',()=>{
 assert.equal(a.SelectedPracticeRoute.MachineGrade,false);assert.equal(a.HumanRubric.TotalMax,8);
 assert.equal(a.HumanRubric.Criteria.reduce((t,x)=>t+x.MaxPoints,0),8);
 assert.equal(a.TestsExecuted,'NOT_RUN');
 for(const k of ['Correct','Equivalent','Wrong','Boundary','Malformed'])assert.ok(a.AuthoredFixtures[k].length>=2,k);
});
test('independent source math oracle 1',()=>{assert.ok(Math.abs((Math.cbrt(1001))-(10.003332222839095))<1e-9);});
test('independent source math oracle 2',()=>{assert.ok(Math.abs((Math.cbrt(4.05/1.2))-(1.5))<1e-9);});
test('independent source math oracle 3',()=>{assert.ok(Math.abs((Math.sqrt(21.6/5.4))-(2))<1e-9);});
test('independent source math oracle 4',()=>{assert.ok(Math.abs(((1/8)*3**2)-(1.125))<1e-9);});
test('independent source math oracle 5',()=>{assert.ok(Math.abs((576*(1/4)**2)-(36))<1e-9);});
test('source scope 1831',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 7.1 The Number That Cubes")));});
test('source scope 1961',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 7.2 Thinking Inside the B")));});
test('source scope 2091',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 7.2 double-volume 10 to20")));});
test('source scope 2221',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 7.3 Satellite Scale Facto")));});
test('source scope 2351',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 7.3 optional extension: i")));});
test('source scope 2481',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source student summary source pr")));});
test('source scope 2611',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source Practice P1-P8: P1 origin")));});
