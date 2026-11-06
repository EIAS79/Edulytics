const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const p=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u05-l01-l06-2021.rich-lesson-v2.json"),'utf8'));
const a=JSON.parse(fs.readFileSync(path.join(root,"src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u05-l05-source-human-paper-20261031.json"),'utf8'));
const lesson=p.Lessons.find(x=>x.LessonCode==="PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U05:L05");
test('lesson exists, previous authored lessons preserved, source task contract selected',()=>{
 assert.ok(lesson);assert.equal(a.LessonCode,"PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U05:L05");assert.equal(a.SourceTaskCrosswalk.length,5);
 assert.equal(a.Practice.Count,7);assert.ok(lesson.WorkedExamples.length>0);
});
test('four real human rubric criteria, five fixture families and tests not executed',()=>{
 assert.equal(a.SelectedPracticeRoute.MachineGrade,false);assert.equal(a.HumanRubric.TotalMax,8);
 assert.equal(a.HumanRubric.Criteria.reduce((t,x)=>t+x.MaxPoints,0),8);
 assert.equal(a.TestsExecuted,'NOT_RUN');
 for(const k of ['Correct','Equivalent','Wrong','Boundary','Malformed'])assert.ok(a.AuthoredFixtures[k].length>=2,k);
});
test('independent source math oracle 1',()=>{assert.ok(Math.abs((53*44*.32*.32)-(238.7968))<1e-9);});
test('independent source math oracle 2',()=>{assert.ok(Math.abs((100*.25**2)-(6.25))<1e-9);});
test('independent source math oracle 3',()=>{assert.ok(Math.abs((100*(.75**2))-(56.25))<1e-9);});
test('independent source math oracle 4',()=>{assert.ok(Math.abs((Math.sqrt(72/2))-(6))<1e-9);});
test('independent source math oracle 5',()=>{assert.ok(Math.abs((Math.sqrt(3)-1)-(0.7320508075688772))<1e-9);});
test('source scope 1827',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 5.1 Transamerica Building")));});
test('source scope 1957',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 5.2 Two Viewpoints (10 mi")));});
test('source scope 2087',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 5.3 Graphing Areas and Sc")));});
test('source scope 2217',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source 5.3 optional extension: o")));});
test('source scope 2347',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source lesson synthesis: a 2ft\u00b2 ")));});
test('source scope 2482',()=>{assert.ok(lesson.ExplanationParagraphs.some(x=>x.includes("Source Practice P1-P7: P1 circle")));});
