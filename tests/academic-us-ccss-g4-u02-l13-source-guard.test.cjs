'use strict';const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');const rd=p=>fs.readFileSync(path.join(__dirname,'..',p),'utf8');
test('source activity1 six, activity2 four and six exact comparisons',()=>{
 const x=JSON.parse(rd('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g4-u02-l13-2021.rich-lesson-v2.json')).Lessons[0];
 assert.equal(x.Title,'Use Equivalent Fractions to Compare');assert.deepEqual(x.Visuals.map(x=>x.Rows.length),[6,4,6]);
 const rat=t=>t.split('/').map(Number);const rel=(l,r)=>{const[a,b]=rat(l),[c,d]=rat(r);return a*d<b*c?'<':a*d>b*c?'>':'='};
 for(const x of x.Visuals[2].Rows){const [l,r]=x.Label.split(' ? ');assert.equal(rel(l,r),x.Cells[0]);}
 assert.equal(x.Visuals[1].Rows[2].Cells[0],'FALSE');
});
test('existing exact families reused and StageD unrun',()=>{
 const s=rd('src/Edulytics.Core/Mathematics/Practice/UsCcssG4Unit2ThirteenthLessonPracticeContracts.cs');
 assert.ok(s.includes('fractions.compare.unlike.select_greater')&&s.includes('fractions.compare.unlike.true_false'));
 const c=JSON.parse(rd('artifacts/curriculum-delivery/US-CCSS-MATH/G4/lesson-21-v2-stage-a-d-checkpoint.json'));
 assert.equal(c.status,'READY_FOR_STAGE_D');assert.equal(c.stages.D,'NOT_RUN');
});