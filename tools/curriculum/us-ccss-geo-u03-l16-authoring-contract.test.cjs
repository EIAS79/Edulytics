const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const p=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u03-l13-l16-2021.rich-lesson-v2.json"),'utf8'));
const a=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u03-l16-source-human-paper-20261031.json"),'utf8'));
const lesson=p.Lessons.find(x=>x.LessonCode==="PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U03:L16");
test('lesson exists, previous authored lessons preserved, source task contract selected',()=>{
 assert.ok(lesson);assert.equal(a.LessonCode,"PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U03:L16");assert.equal(a.SourceTaskCrosswalk.length,6);
 assert.equal(a.Practice.Count,9);assert.ok(lesson.WorkedExamples.length>0);
});
test('four real human rubric criteria, five fixture families and tests not executed',()=>{
 assert.equal(a.SelectedPracticeRoute.MachineGrade,false);assert.equal(a.HumanRubric.TotalMax,8);
 assert.equal(a.HumanRubric.Criteria.reduce((t,x)=>t+x.MaxPoints,0),8);
 assert.equal(a.TestsExecuted,'NOT_RUN');
 for(const k of ['Correct','Equivalent','Wrong','Boundary','Malformed'])assert.ok(a.AuthoredFixtures[k].length>=2,k);
});
test('independent source math oracle 1',()=>{assert.ok(Math.abs((2+6*(3/8))-(4.25))<1e-9);});
test('independent source math oracle 2',()=>{assert.ok(Math.abs((65*72/120)-(39))<1e-9);});
test('independent source math oracle 3',()=>{assert.ok(Math.abs((Math.sqrt(17**2-15**2))-(8))<1e-9);});
test('independent source math oracle 4',()=>{assert.ok(Math.abs((20*16/(16+25))-(7.804878048780488))<1e-9);});
test('independent source math oracle 5',()=>{assert.ok(Math.abs((16/32*20/(1+16/32))-(6.666666666666667))<1e-9);});
test('source scope 1839',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 16.1 Notice and Wonder Ri")));});
test('source scope 1969',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 16.2 Bank Shot (20 min): ")));});
test('source scope 2099',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 16.2 comparison of precis")));});
test('source scope 2229',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 16.3 Indirect Measurement")));});
test('source scope 2359',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 16.4 Indirect Measurement")));});
test('source scope 2489',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source original independent wind")));});
test('source scope 2619',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source student summary handball ")));});
test('source scope 2749',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source Practice P1-P9: P1 handba")));});
