const test=require('node:test'); const assert=require('node:assert/strict');const fs=require('node:fs');const path=require('node:path');const root=path.resolve(__dirname,'../..');
const c=JSON.parse(fs.readFileSync(path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u03-l04-l06-2021.rich-lesson-v2.json'),'utf8'));
const a=JSON.parse(fs.readFileSync(path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u03-l05-midpoint-dilation-paper-20261031.json'),'utf8'));
test('source 5.1 to 5.4 and seven practice mappings',()=>{
 assert.deepEqual(c.Lessons.map(x=>x.LessonCode.slice(-7)),['U03:L04','U03:L05','U03:L06']);
 assert.equal(c.Lessons[1].WorkedExamples.length,3);
 for(const x of ['Source 5.1 Notice and Wonder','Source 5.2 Dilation or Violation','Source 5.3 A Little Bit Farther','Source 5.4 teacher-login'])
 assert.ok(c.Lessons[1].ExplanationParagraphs.some(p=>p.includes(x)),x);
 assert.equal(a.Practice.Count,7);
});
test('direct dilation argument has the exact half and two-thirds ratios',()=>{
 assert.equal(a.IndependentOracles.MidsegmentThirdSide10,'5');
 assert.match(a.IndependentOracles.SourceRatioTwoThirds,/MN parallel BC/);
 assert.equal(a.IndependentOracles.ConverseStatus,'UNPROVED_AT_THIS_SOURCE_LESSON');
 assert.equal(a.Practice.Items[2].TeacherExpected,'image also 21 degrees');
});
test('human assessment no runtime and fixture families',()=>{
 assert.equal(a.SelectedPracticeRoute.MachineGrade,false);
 assert.equal(a.HumanRubric.Criteria.reduce((s,x)=>s+x.MaxPoints,0),8);
 assert.equal(a.TestsExecuted,'NOT_RUN');
 for(const k of ['Correct','Equivalent','Wrong','Boundary','Malformed']) assert.ok(a.AuthoredFixtures[k].length>=2,k);
});
