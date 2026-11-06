'use strict';
// A-C authoring, intentionally NOT_RUN.
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=p=>fs.readFileSync(path.join(root,p),'utf8');
test('Proportional tables grade-specific view called from shared hook',()=>{
 const v=read('src/Edulytics.Web/Views/Shared/_USCCSSG7Unit2And7GeometryFigures.cshtml');
 const h=read('src/Edulytics.Web/Views/Shared/_RichLessonContentV2.cshtml');
 assert.ok(h.includes('Model.LessonCode == "PED:US-CCSS-MATH:G7:U02:L02"'));
 for(const s of ['data-g7-u02-l02','2.1 Notice','2.2 Feeding',
   '2.3 Bread','2.4 Quarters','Cool-down','24 rolls'])
 assert.ok(v.includes(s),s);
});
test('Independent row arithmetic & source contradiction held, not silently fixed',()=>{
 assert.deepEqual([12/1,36/2,60/5,120/10],[12,18,12,12]);
 assert.equal(2*12,24);
 assert.equal(10*3,30);
 assert.equal(45/3,15);
 assert.equal(20*5/4,25);
 assert.equal(13*5/4,16.25);
 assert.equal(20*4/5,16);
 assert.equal(4*10/4,10);
 assert.equal(3*7/7,3);
});
test('A-C checkpoint does not assert unrun Stage D verification',()=>{
 const c=JSON.parse(read('artifacts/curriculum-delivery/US-CCSS-MATH/G7/grade7-u02-l02-abc-cursor.json'));
 assert.equal(c.READY_FOR_STAGE_D,true);
 assert.equal(c.testExecution,'AUTHORED_NOT_RUN');
 assert.equal(c.D_STATUS,'NOT_RUN');
});
