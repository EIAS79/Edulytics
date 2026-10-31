// Geometry U02:L04 source-aligned A/B/C fixtures. AUTHORED_NOT_RUN.
'use strict';
const {test}=require('node:test'), assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const a=read('src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u02-l04-triangle-congruence-paper-20261031.json');
const r=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u02-l04-l06-2021.rich-lesson-v2.json');
const code='PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:U02:L04';
test('exact L04 paper route and all nine practice tasks; no auto-graded drawing',()=>{
 assert.equal(a.SelectedPracticeRoute.LessonCode,code);
 assert.equal(a.SelectedPracticeRoute.MachineGrade,false);
 assert.deepEqual(a.Practice.map(x=>x.Id),Array.from({length:9},(_,i)=>'P'+(i+1)));
 assert.equal(a.HumanRubric.Criteria.reduce((s,c)=>s+c.MaxPoints,0),8);
 const l=r.Lessons.find(x=>x.LessonCode===code);
 assert(l && l.WorkedExamples.length===3);
 assert(l.ExplanationParagraphs.some(x=>x.includes('private problem/data cards')));
 assert.equal(r.Lessons.length,3);
});
test('4.1 overdetermined physical triangle has BC about 3.7',()=>{
 const bc=Math.sqrt(25+4-20*Math.cos(40*Math.PI/180));
 assert(Math.abs(bc-3.7)<0.01);
});
test('SSA A30 AB10 BC6 gives two positive noncongruent triangles',()=>{
 const roots=[(10*Math.sqrt(3)-Math.sqrt(44))/2,(10*Math.sqrt(3)+Math.sqrt(44))/2];
 assert(roots[0]>0 && roots[1]>roots[0]);
 for(const t of roots) assert(Math.abs(t*t-10*Math.sqrt(3)*t+64)<1e-10);
});
test('corresponding vertices P4 and reflection P7 accepted choices',()=>{
 assert.deepEqual(a.IndependentAnswerOracles.P4_TrueChoices,['A','D']);
 assert.deepEqual(a.IndependentAnswerOracles.P7_TrueChoices,['A','B','F']);
 assert(a.AuthoredFixtures.Wrong.some(x=>x.includes('AAA')));
});
