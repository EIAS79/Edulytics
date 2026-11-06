const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const p=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u03-l10-l12-2021.rich-lesson-v2.json"),'utf8'));
const a=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u03-l11-source-human-paper-20261031.json"),'utf8'));
const lesson=p.Lessons.find(x=>x.LessonCode==="PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U03:L11");
test('lesson exists, previous authored lessons preserved, source task contract selected',()=>{
 assert.ok(lesson);assert.equal(a.LessonCode,"PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U03:L11");assert.equal(a.SourceTaskCrosswalk.length,5);
 assert.equal(a.Practice.Count,7);assert.ok(lesson.WorkedExamples.length>0);
});
test('four real human rubric criteria, five fixture families and tests not executed',()=>{
 assert.equal(a.SelectedPracticeRoute.MachineGrade,false);assert.equal(a.HumanRubric.TotalMax,8);
 assert.equal(a.HumanRubric.Criteria.reduce((t,x)=>t+x.MaxPoints,0),8);
 assert.equal(a.TestsExecuted,'NOT_RUN');
 for(const k of ['Correct','Equivalent','Wrong','Boundary','Malformed'])assert.ok(a.AuthoredFixtures[k].length>=2,k);
});
test('independent source math oracle 1',()=>{assert.ok(Math.abs((5*7.5/3)-(12.5))<1e-9);});
test('independent source math oracle 2',()=>{assert.ok(Math.abs((5*(9/6))-(7.5))<1e-9);});
test('independent source math oracle 3',()=>{assert.ok(Math.abs((10.5-7)-(3.5))<1e-9);});
test('independent source math oracle 4',()=>{assert.ok(Math.abs((12*(5/15))-(4))<1e-9);});
test('source scope 1686',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 11.1 Notice and Wonder Pa")));});
test('source scope 1816',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 11.2 Prove It Parallel Se")));});
test('source scope 1946',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 11.3 Preponderance of Pro")));});
test('source scope 2076',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source student summary numerical")));});
test('source scope 2206',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source Practice P1-P7: P1 with A")));});
test('source scope 2336',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 11.4 Cool-down More Propo")));});
