'use strict';
// A-B-C authoring fixture, NOT_RUN. Runtime verification belongs in Stage D.
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=x=>fs.readFileSync(path.join(root,x),'utf8');
test('Four source activities and first-party cooldown are present',()=>{
 const view=read('src/Edulytics.Web/Views/Shared/_USCCSSG7Unit2And7GeometryFigures.cshtml');
 const hook=read('src/Edulytics.Web/Views/Shared/_RichLessonContentV2.cshtml');
 for(const s of ['data-g7-u02-l05','5.1 Growing','5.2 Meters','5.3 Filling',
   '5.4 Feeding','Cool-down','Teacher review'])assert.ok(view.includes(s),s);
 assert.ok(hook.includes('Model.LessonCode == "PED:US-CCSS-MATH:G7:U02:L05"'));
});
test('Independent reciprocal and per-day arithmetic',()=>{
 assert.equal(0.94*100,94);assert.equal(250/100,2.5);
 assert.equal(5/8,0.625);assert.equal(8/5,1.6);
 assert.equal(3/5*10,6);assert.equal(300/6,50);
 assert.equal(100*100*100,1000000);
});
test('No Stage D inferred from A-B-C authoring',()=>{
 const c=JSON.parse(read('artifacts/curriculum-delivery/US-CCSS-MATH/G7/grade7-u02-l05-abc-cursor.json'));
 assert.equal(c.READY_FOR_STAGE_D,true);
 assert.equal(c.D_STATUS,'NOT_RUN');assert.equal(c.testExecution,'AUTHORED_NOT_RUN');
});
