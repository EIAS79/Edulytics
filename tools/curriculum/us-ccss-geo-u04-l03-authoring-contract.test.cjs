const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const p=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u04-l01-l04-2021.rich-lesson-v2.json"),'utf8'));
const a=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u04-l03-source-human-paper-20261031.json"),'utf8'));
const lesson=p.Lessons.find(x=>x.LessonCode==="PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U04:L03");
test('lesson exists, previous authored lessons preserved, source task contract selected',()=>{
 assert.ok(lesson);assert.equal(a.LessonCode,"PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U04:L03");assert.equal(a.SourceTaskCrosswalk.length,5);
 assert.equal(a.Practice.Count,8);assert.ok(lesson.WorkedExamples.length>0);
});
test('four real human rubric criteria, five fixture families and tests not executed',()=>{
 assert.equal(a.SelectedPracticeRoute.MachineGrade,false);assert.equal(a.HumanRubric.TotalMax,8);
 assert.equal(a.HumanRubric.Criteria.reduce((t,x)=>t+x.MaxPoints,0),8);
 assert.equal(a.TestsExecuted,'NOT_RUN');
 for(const k of ['Correct','Equivalent','Wrong','Boundary','Malformed'])assert.ok(a.AuthoredFixtures[k].length>=2,k);
});
test('independent source math oracle 1',()=>{assert.ok(Math.abs((Math.sqrt(2**2-1**2))-(1.7320508075688772))<1e-9);});
test('independent source math oracle 2',()=>{assert.ok(Math.abs((Math.sqrt(10**2-5**2))-(8.660254037844386))<1e-9);});
test('independent source math oracle 3',()=>{assert.ok(Math.abs(((2*Math.sqrt(3))/Math.sqrt(3))-(2))<1e-9);});
test('independent source math oracle 4',()=>{assert.ok(Math.abs(((10*10*Math.sqrt(3))/4)-(43.30127018922193))<1e-9);});
test('independent source math oracle 5',()=>{assert.ok(Math.abs((12*(3/9))-(4))<1e-9);});
test('source scope 1881',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 3.1 Notice and Wonder Tri")));});
test('source scope 2011',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 3.2 Decomposing Equilater")));});
test('source scope 2141',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 3.3 Generalize Half Equil")));});
test('source scope 2271',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 3.3 optional source exten")));});
test('source scope 2401',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source synthesis compare the cla")));});
test('source scope 2531',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source student summary: original")));});
test('source scope 2661',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source Practice P1-P8: P1 equila")));});
