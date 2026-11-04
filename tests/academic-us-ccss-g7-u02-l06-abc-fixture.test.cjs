'use strict';
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=x=>fs.readFileSync(path.join(root,x),'utf8');
test('Grade7 source tasks and original Cool-down have first-party authoring',()=>{
 const v=read('src/Edulytics.Web/Views/Shared/_USCCSSG7Unit2And7GeometryFigures.cshtml');
 const h=read('src/Edulytics.Web/Views/Shared/_RichLessonContentV2.cshtml');
 for(const item of ['data-g7-u02-l06','6.1 Reason','6.2 Tickets','6.3 Recycled',
 'Cool-down','Teacher review'])assert.ok(v.includes(item),item);
 assert.ok(h.includes('Model.LessonCode == "PED:US-CCSS-MATH:G7:U02:L06"'));
});
test('Source-math and discrete-boundary independent oracles',()=>{
 assert.equal(311000/5000,62.2);
 assert.equal(404300/62.2,6500);
 assert.equal(Math.ceil(5000000/62.2),80386);
 assert.equal(80386*62.2,5000009.2);
 assert.equal(2.4/0.16*10,150);
 assert.equal(150/10*0.14,2.1);
 assert.equal(65*1.5,97.5);
});
test('A-C authoring closure is not runtime verification',()=>{
 const c=JSON.parse(read('artifacts/curriculum-delivery/US-CCSS-MATH/G7/grade7-u02-l06-abc-cursor.json'));
 assert.equal(c.READY_FOR_STAGE_D,true);
 assert.equal(c.testExecution,'AUTHORED_NOT_RUN');assert.equal(c.D_STATUS,'NOT_RUN');
});
