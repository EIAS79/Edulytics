const test=require('node:test'); const assert=require('node:assert/strict');
const fs=require('node:fs');const path=require('node:path');const root=path.resolve(__dirname,'../..');
const l=JSON.parse(fs.readFileSync(path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u03-l01-l03-2021.rich-lesson-v2.json'),'utf8')).Lessons;
const a=JSON.parse(fs.readFileSync(path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u03-l03-dilation-two-ratio-tables-paper-20261031.json'),'utf8'));
test('source activity 3.1 through 3.4 and old worked examples preserved',()=>{
 assert.deepEqual(l.map(x=>x.LessonCode.slice(-7)),['U03:L01','U03:L02','U03:L03']);
 assert.equal(l[2].WorkedExamples.length,3);
 for(const k of ['Source 3.1 Dilating Out','Source 3.2 All the Scale Factors','Source 3.3 What Stays the Same','Source 3.4 teacher-login'])
 assert.ok(l[2].ExplanationParagraphs.some(x=>x.includes(k)),k);
});
test('centre ratios are definition; source side relation assertion only',()=>{
 assert.match(l[2].ExplanationParagraphs.join(' '),/NOT a theorem proved here/);
 assert.equal(a.IndependentOracles.SideProportionalityLessonStatus,'SOURCE_ASSERTION_NOT_PROVED_IN_THIS_LESSON');
 assert.equal(a.IndependentOracles.P1ScaleFactor,'5/3');
 assert.equal(a.IndependentOracles.P2Perimeter,'9');
 assert.equal(a.IndependentOracles.P3Enlargement,'4/3');
});
test('seven practice items and human rubric not run',()=>{
 assert.equal(a.Practice.Count,7);
 assert.equal(a.HumanRubric.Criteria.reduce((s,x)=>s+x.MaxPoints,0),8);
 assert.equal(a.SelectedPracticeRoute.MachineGrade,false);
 assert.equal(a.TestsExecuted,'NOT_RUN');
 for(const k of ['Correct','Equivalent','Wrong','Boundary','Malformed']) assert.ok(a.AuthoredFixtures[k].length>=2,k);
});
