'use strict';const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');const rd=p=>fs.readFileSync(path.join(__dirname,'..',p),'utf8');
test('source 2021 original5 fraction targets plus common-factor cases',()=>{
 const x=JSON.parse(rd('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g4-u02-l11-2021.rich-lesson-v2.json')).Lessons[0];
 assert.equal(x.Title,'Use Factors to Find Equivalent Fractions');
 assert.deepEqual(x.Visuals[0].Rows.map(y=>y.Label),["16/8","40/10","7/6","90/100","5/4"]);
 const f=s=>s.split('/').map(Number);
 for(const y of x.Visuals[0].Rows){const [a,b]=f(y.Label);for(const v of y.Cells){const[c,d]=f(v);assert.equal(a*d,b*c);}}
 assert.ok(x.ExplanationParagraphs.some(s=>s.includes('OPTIONAL')));
});
test('existing exact reduction and multiplier reuse, StageD not run',()=>{
 const c=rd('src/Edulytics.Core/Mathematics/Practice/UsCcssG4Unit2EleventhLessonPracticeContracts.cs');
 assert.ok(c.includes('fractions.equivalent.reduce_common_factor')&&c.includes('fractions.equivalent.generate_multiple'));
 const x=JSON.parse(rd('artifacts/curriculum-delivery/US-CCSS-MATH/G4/lesson-19-v2-stage-a-d-checkpoint.json'));
 assert.equal(x.status,'READY_FOR_STAGE_D');assert.equal(x.stages.D,'NOT_RUN');
});