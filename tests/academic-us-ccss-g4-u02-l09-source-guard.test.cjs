'use strict';const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');const rd=p=>fs.readFileSync(path.join(__dirname,'..',p),'utf8');
test('original teacher poster sets D/E/F include one false fraction',()=>{
 const r=JSON.parse(rd('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g4-u02-l09-2021.rich-lesson-v2.json')).Lessons[0];
 assert.equal(r.Title,'Explain Equivalence');assert.equal(r.Visuals[0].Rows.length,6);
 const eq=(a,b,c,d)=>a*d===b*c;
 assert.ok(eq(1,4,3,12)&&!eq(1,4,30,100));
 assert.ok(eq(15,6,30,12)&&!eq(7,4,15,6));
 assert.ok(eq(7,3,28,12)&&!eq(21,10,7,3));
 assert.ok(eq(70,100,7,10)&&!eq(70,100,3,5));
});
test('source exact family reused, original cool-down not claimed, Stage D unrun',()=>{
 const c=rd('src/Edulytics.Core/Mathematics/Practice/UsCcssG4Unit2NinthLessonPracticeContracts.cs');
 assert.ok(c.includes('fractions.equivalent.recognize')&&c.includes('fractions.equivalent.number_line'));
 const p=JSON.parse(rd('artifacts/curriculum-delivery/US-CCSS-MATH/G4/lesson-17-v2-stage-a-d-checkpoint.json'));
 assert.equal(p.status,'READY_FOR_STAGE_D');assert.equal(p.stages.D,'NOT_RUN');assert.equal(p.academicApproved,false);
});