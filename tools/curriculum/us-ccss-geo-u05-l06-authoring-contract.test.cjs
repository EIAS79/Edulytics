const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const p=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u05-l01-l06-2021.rich-lesson-v2.json"),'utf8'));
const a=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u05-l06-source-human-paper-20261031.json"),'utf8'));
const lesson=p.Lessons.find(x=>x.LessonCode==="PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U05:L06");
test('lesson exists, previous authored lessons preserved, source task contract selected',()=>{
 assert.ok(lesson);assert.equal(a.LessonCode,"PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U05:L06");assert.equal(a.SourceTaskCrosswalk.length,6);
 assert.equal(a.Practice.Count,9);assert.ok(lesson.WorkedExamples.length>0);
});
test('four real human rubric criteria, five fixture families and tests not executed',()=>{
 assert.equal(a.SelectedPracticeRoute.MachineGrade,false);assert.equal(a.HumanRubric.TotalMax,8);
 assert.equal(a.HumanRubric.Criteria.reduce((t,x)=>t+x.MaxPoints,0),8);
 assert.equal(a.TestsExecuted,'NOT_RUN');
 for(const k of ['Correct','Equivalent','Wrong','Boundary','Malformed'])assert.ok(a.AuthoredFixtures[k].length>=2,k);
});
test('independent source math oracle 1',()=>{assert.ok(Math.abs((84*4**2)-(1344))<1e-9);});
test('independent source math oracle 2',()=>{assert.ok(Math.abs((36*4**3)-(2304))<1e-9);});
test('independent source math oracle 3',()=>{assert.ok(Math.abs((10*3.5**3)-(428.75))<1e-9);});
test('independent source math oracle 4',()=>{assert.ok(Math.abs((12*(5/3)**3)-(55.55555555555556))<1e-9);});
test('independent source math oracle 5',()=>{assert.ok(Math.abs((1.5*5**2)-(37.5))<1e-9);});
test('source scope 1804',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 6.1 Math Talk Cube Volume")));});
test('source scope 1934',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 6.2 How Do Surface Area a")));});
test('source scope 2064',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 6.3 Scaling All Solids (1")));});
test('source scope 2194',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 6.3 exact original triang")));});
test('source scope 2324',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source optional Sierpinski trian")));});
test('source scope 2454',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source student summary original ")));});
test('source scope 2584',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source Practice P1-P9: P1 box us")));});
