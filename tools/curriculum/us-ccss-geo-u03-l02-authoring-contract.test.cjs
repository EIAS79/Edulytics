const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'../..');
const pack=JSON.parse(fs.readFileSync(path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-hs-geo-u03-l01-l03-2021.rich-lesson-v2.json'),'utf8'));
const a=JSON.parse(fs.readFileSync(path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/Assessment/geo-u03-l02-solar-system-ratios-paper-20261031.json'),'utf8'));
test('source lesson 2.1-2.4 plus seven cumulative tasks authored without destroying other lessons',()=>{
 assert.deepEqual(pack.Lessons.map(x=>x.LessonCode.slice(-7)),['U03:L01','U03:L02','U03:L03']);
 for(const s of ['Source 2.1 Solar Eclipse','Source 2.2 Shrinking','Source 2.3 Shrinking','Source 2.4 original','Source practice P1-P7'])
 assert.ok(pack.Lessons[1].ExplanationParagraphs.some(p=>p.includes(s)),s);
 assert.equal(pack.Lessons[1].WorkedExamples.length,3);
 assert.equal(a.Practice.Count,7);
});
test('independent arithmetic for source eclipse, Jupiter and 0.3mm Neptune',()=>{
 const q=a.SourceData;
 const moon=q.EarthSunDistanceKm*q.MoonDiameterKm/q.SunDiameterKm;
 assert.ok(Math.abs(moon-373731.1286844)<1e-5);
 const earth=q.PlanetTable.find(x=>x.Planet==='Earth');
 const j=q.PlanetTable.find(x=>x.Planet==='Jupiter');
 const n=q.PlanetTable.find(x=>x.Planet==='Neptune');
 assert.ok(Math.abs(j.DiameterKm*q.EarthModelDiameterCm/earth.DiameterKm-22.41831295)<1e-6);
 assert.ok(Math.abs((j.OrbitRadiusKm-earth.OrbitRadiusKm)*q.EarthModelDiameterCm/earth.DiameterKm/100-986.20257134)<1e-6);
 assert.ok(Math.abs((n.OrbitRadiusKm-earth.OrbitRadiusKm)*0.03/earth.DiameterKm/100-102.19896519)<1e-6);
});
test('lesson-specific selected paper task and five fixture classes, runtime NOT_RUN',()=>{
 assert.equal(a.SelectedPracticeRoute.MachineGrade,false);
 assert.equal(a.SelectedPracticeRoute.SavedGrade,'NOT_VERIFIED_STAGE_D');
 assert.equal(a.HumanRubric.Criteria.reduce((s,x)=>s+x.MaxPoints,0),8);
 assert.equal(a.TestsExecuted,'NOT_RUN');
 for(const k of ['Correct','Equivalent','Wrong','Boundary','Malformed'])assert.ok(a.AuthoredFixtures[k].length>=2,k);
});
