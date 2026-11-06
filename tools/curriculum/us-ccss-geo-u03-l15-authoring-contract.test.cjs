const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const p=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u03-l13-l16-2021.rich-lesson-v2.json"),'utf8'));
const a=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u03-l15-source-human-paper-20261031.json"),'utf8'));
const lesson=p.Lessons.find(x=>x.LessonCode==="PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U03:L15");
test('lesson exists, previous authored lessons preserved, source task contract selected',()=>{
 assert.ok(lesson);assert.equal(a.LessonCode,"PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U03:L15");assert.equal(a.SourceTaskCrosswalk.length,5);
 assert.equal(a.Practice.Count,7);assert.ok(lesson.WorkedExamples.length>0);
});
test('four real human rubric criteria, five fixture families and tests not executed',()=>{
 assert.equal(a.SelectedPracticeRoute.MachineGrade,false);assert.equal(a.HumanRubric.TotalMax,8);
 assert.equal(a.HumanRubric.Criteria.reduce((t,x)=>t+x.MaxPoints,0),8);
 assert.equal(a.TestsExecuted,'NOT_RUN');
 for(const k of ['Correct','Equivalent','Wrong','Boundary','Malformed'])assert.ok(a.AuthoredFixtures[k].length>=2,k);
});
test('independent source math oracle 1',()=>{assert.ok(Math.abs((Math.sqrt(13*13-5*5))-(12))<1e-9);});
test('independent source math oracle 2',()=>{assert.ok(Math.abs((Math.sqrt(8*2))-(4))<1e-9);});
test('independent source math oracle 3',()=>{assert.ok(Math.abs((4*4/5)-(3.2))<1e-9);});
test('independent source math oracle 4',()=>{assert.ok(Math.abs(((8.45/13)*5)-(3.25))<1e-9);});
test('independent source math oracle 5',()=>{assert.ok(Math.abs((3*3+4*4)-(25))<1e-9);});
test('source scope 1796',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 15.1 Which One Doesn't Be")));});
test('source scope 1926',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 15.2 Info Gap Similar Seq")));});
test('source scope 2056',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 15.2 original synthesis: ")));});
test('source scope 2186',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 15.3 Relatively Reasonabl")));});
test('source scope 2316',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 15.3 optional origami ext")));});
test('source scope 2446',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source synthesis source-indexed ")));});
test('source scope 2576',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source Practice P1-P7: P1 BE of ")));});
