'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const get=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
test('G4 L101 math and independent source-first area visual models',()=>{
 const r=get('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g4-ordinal-101-2021.rich-lesson-v2.json');
 const l=r.Lessons[0];
 assert.equal(l.LessonCode,'PED:US-CCSS-MATH:G4:U06:L15');
 assert.equal(l.WorkedExamples.length,3);
 assert.match(l.WorkedExamples[2].Question,/2,280 m²/);
 assert.equal(40*57,2280);assert.equal(7*27,189);assert.equal(6*32+5,197);
 assert.equal(l.Visuals.length,3);
 assert.match(l.Visuals[0].Description,/First-party symbolic support/);
 assert.match(l.Visuals[2].Items.join(' '),/6 × 33 = 198/);
});
test('Ready for Stage D is not evidence of verified figures, approval or Stage D completion',()=>{
 const cp=get('artifacts/curriculum-delivery/US-CCSS-MATH/G4/lesson-101-v2-stage-a-d-checkpoint.json');
 assert.equal(cp.readyForStageD,true);assert.equal(cp.status,'READY_FOR_STAGE_D');
 assert.equal(cp.stages.D,'NOT_RUN');assert.equal(cp.academicApproved,false);
 assert.equal(cp.bStageVisuals.sourceOriginalImagesOpened,0);
 assert.equal(cp.bStageVisuals.graphicRenderingVerified,false);
 assert.equal(cp.focusedTestsRun,true);
});
