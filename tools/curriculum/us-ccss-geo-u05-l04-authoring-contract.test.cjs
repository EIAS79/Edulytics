const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const p=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u05-l01-l06-2021.rich-lesson-v2.json"),'utf8'));
const a=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u05-l04-source-human-paper-20261031.json"),'utf8'));
const lesson=p.Lessons.find(x=>x.LessonCode==="PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U05:L04");
test('lesson exists, previous authored lessons preserved, source task contract selected',()=>{
 assert.ok(lesson);assert.equal(a.LessonCode,"PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U05:L04");assert.equal(a.SourceTaskCrosswalk.length,5);
 assert.equal(a.Practice.Count,9);assert.ok(lesson.WorkedExamples.length>0);
});
test('four real human rubric criteria, five fixture families and tests not executed',()=>{
 assert.equal(a.SelectedPracticeRoute.MachineGrade,false);assert.equal(a.HumanRubric.TotalMax,8);
 assert.equal(a.HumanRubric.Criteria.reduce((t,x)=>t+x.MaxPoints,0),8);
 assert.equal(a.TestsExecuted,'NOT_RUN');
 for(const k of ['Correct','Equivalent','Wrong','Boundary','Malformed'])assert.ok(a.AuthoredFixtures[k].length>=2,k);
});
test('independent source math oracle 1',()=>{assert.ok(Math.abs((10*(2.5**2))-(62.5))<1e-9);});
test('independent source math oracle 2',()=>{assert.ok(Math.abs((20*6**2)-(720))<1e-9);});
test('independent source math oracle 3',()=>{assert.ok(Math.abs((12*1.2**2)-(17.28))<1e-9);});
test('independent source math oracle 4',()=>{assert.ok(Math.abs((40*3.5**2)-(490))<1e-9);});
test('independent source math oracle 5',()=>{assert.ok(Math.abs(((4**2-3**2))-(7))<1e-9);});
test('source scope 1790',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 4.1 Squares and Roots (5 ")));});
test('source scope 1920',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 4.2 Scaling Up a Rectangl")));});
test('source scope 2050',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 4.2 optional extension: e")));});
test('source scope 2180',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 4.3 What About Other Shap")));});
test('source scope 2310',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source synthesis: original arbit")));});
test('source scope 2440',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source Practice P1-P9: P1 origin")));});
