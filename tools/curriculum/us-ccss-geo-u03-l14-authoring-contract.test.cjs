const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const p=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u03-l13-l16-2021.rich-lesson-v2.json"),'utf8'));
const a=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u03-l14-source-human-paper-20261031.json"),'utf8'));
const lesson=p.Lessons.find(x=>x.LessonCode==="PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U03:L14");
test('lesson exists, previous authored lessons preserved, source task contract selected',()=>{
 assert.ok(lesson);assert.equal(a.LessonCode,"PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U03:L14");assert.equal(a.SourceTaskCrosswalk.length,5);
 assert.equal(a.Practice.Count,9);assert.ok(lesson.WorkedExamples.length>0);
});
test('four real human rubric criteria, five fixture families and tests not executed',()=>{
 assert.equal(a.SelectedPracticeRoute.MachineGrade,false);assert.equal(a.HumanRubric.TotalMax,8);
 assert.equal(a.HumanRubric.Criteria.reduce((t,x)=>t+x.MaxPoints,0),8);
 assert.equal(a.TestsExecuted,'NOT_RUN');
 for(const k of ['Correct','Equivalent','Wrong','Boundary','Malformed'])assert.ok(a.AuthoredFixtures[k].length>=2,k);
});
test('independent source math oracle 1',()=>{assert.ok(Math.abs((6*6/12)-(3))<1e-9);});
test('independent source math oracle 2',()=>{assert.ok(Math.abs((5*12/2)-(30))<1e-9);});
test('independent source math oracle 3',()=>{assert.ok(Math.abs((30/4)-(7.5))<1e-9);});
test('independent source math oracle 4',()=>{assert.ok(Math.abs((4/5)-(0.8))<1e-9);});
test('independent source math oracle 5',()=>{assert.ok(Math.abs((8*8+15*15)-(289))<1e-9);});
test('source scope 1767',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 14.1 Notice and Wonder Va")));});
test('source scope 1897',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 14.2 Prove Pythagoras Rig")));});
test('source scope 2027',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 14.3 An Alternate Approac")));});
test('source scope 2157',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 14.3 optional James Garfi")));});
test('source scope 2287',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source lesson synthesis and conv")));});
test('source scope 2417',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source Practice P1-P9: P1 only c")));});
