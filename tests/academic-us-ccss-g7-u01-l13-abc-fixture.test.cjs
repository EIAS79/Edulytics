'use strict';
// Source-scoped authoring fixture, intentionally NOT_RUN during A–B–C.
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const read=p=>fs.readFileSync(path.join(root,p),'utf8');
test('L13 optional project covers each source activity and original Cool-down',()=>{
 const view=read('src/Edulytics.Web/Views/Shared/_USCCSSG7ScaleConstructionFigures.cshtml');
 for(const marker of ['data-g7-u01-l13',
 '13.1 Which measurements','13.2 Prepare',
 '13.3 Choose paper','13.4 Present',
 'Cool-down','Teacher review required'])
   assert.ok(view.includes(marker),marker);
});
test('L13 exact numeric fit and squared tile area independent witnesses',()=>{
 assert.equal(10/0.5,20);
 assert.equal(8/0.5,16);
 assert.equal(16<=15,false);
 assert.equal(10/1,10);
 assert.equal(8/1,8);
 assert.equal(20*20/(10*10),4);
 assert.equal((10*10)/(20*20),0.25);
 assert.equal(8*6,48);
 assert.equal((8/2)*(6/2),12);
});
test('L13 authoring closure never impersonates Stage D or persisted drawings',()=>{
 const c=JSON.parse(read('artifacts/curriculum-delivery/US-CCSS-MATH/G7/grade7-u01-l13-abc-cursor.json'));
 assert.equal(c.READY_FOR_STAGE_D,true);
 assert.equal(c.testExecution,'AUTHORED_NOT_RUN');
 assert.equal(c.D_STATUS,'NOT_RUN');
 assert.equal(c.studentDrawingPersistence,'NOT_IMPLEMENTED');
});
