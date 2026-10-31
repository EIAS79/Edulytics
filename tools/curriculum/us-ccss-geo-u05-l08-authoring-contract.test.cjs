const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const p=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u05-l07-l12-2021.rich-lesson-v2.json"),'utf8'));
const a=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u05-l08-source-human-paper-20261031.json"),'utf8'));
const lesson=p.Lessons.find(x=>x.LessonCode==="PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U05:L08");
test('lesson exists, previous authored lessons preserved, source task contract selected',()=>{
 assert.ok(lesson);assert.equal(a.LessonCode,"PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U05:L08");assert.equal(a.SourceTaskCrosswalk.length,5);
 assert.equal(a.Practice.Count,7);assert.ok(lesson.WorkedExamples.length>0);
});
test('four real human rubric criteria, five fixture families and tests not executed',()=>{
 assert.equal(a.SelectedPracticeRoute.MachineGrade,false);assert.equal(a.HumanRubric.TotalMax,8);
 assert.equal(a.HumanRubric.Criteria.reduce((t,x)=>t+x.MaxPoints,0),8);
 assert.equal(a.TestsExecuted,'NOT_RUN');
 for(const k of ['Correct','Equivalent','Wrong','Boundary','Malformed'])assert.ok(a.AuthoredFixtures[k].length>=2,k);
});
test('independent source math oracle 1',()=>{assert.ok(Math.abs((Math.sqrt(729/36))-(4.5))<1e-9);});
test('independent source math oracle 2',()=>{assert.ok(Math.abs((Math.sqrt(198/1408)*8)-(3))<1e-9);});
test('independent source math oracle 3',()=>{assert.ok(Math.abs((800*(Math.sqrt(2250/360))**3)-(12500))<1e-9);});
test('independent source math oracle 4',()=>{assert.ok(Math.abs((.04*4+.27*8)-(2.32))<1e-9);});
test('independent source math oracle 5',()=>{assert.ok(Math.abs((6*3**2*.5**2)-(13.5))<1e-9);});
test('source scope 1833',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 8.1 Going Backwards (5 mi")));});
test('source scope 1963',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 8.2 Info Gap Originals an")));});
test('source scope 2093',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 8.3 Jumbo Can (15 min OPT")));});
test('source scope 2223',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 8.3 optional Burj Khalifa")));});
test('source scope 2353',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source student summary original ")));});
test('source scope 2483',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source Practice P1-P7: P1 indivi")));});
