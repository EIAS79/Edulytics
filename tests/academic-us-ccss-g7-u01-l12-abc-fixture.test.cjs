'use strict';
// Scoped source-level math and authoring evidence. NOT_RUN during A-B-C.
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const read=x=>fs.readFileSync(path.join(root,x),'utf8');
test('L12 source flag table stays unambiguously assigned',()=>{
 const v=read('src/Edulytics.Web/Views/Shared/_USCCSSG7ScaleConstructionFigures.cshtml');
 assert.ok(v.includes('data-g7-u01-l12'));
 assert.ok(v.includes('whole flag height'));
 assert.ok(v.includes('13.2 cm'));
 assert.ok(v.includes('1:2000'));
});
test('L12 independent conversion and area controls',()=>{
 assert.equal(5280*12*2.54,160934.4);
 assert.equal(396*100/2000,19.8);
 assert.equal(13.2*2000/100,264);
 assert.equal(99*100/2000,4.95);
 assert.equal(396*264,104544);
 assert.equal(19.8*13.2,261.36);
 assert.equal(2000*2000,4000000);
 assert.equal(3.2*25,80);
 assert.equal(8*4*25,800);
});
test('L12 A-C authored without Stage D',()=>{
 const c=JSON.parse(read('artifacts/curriculum-delivery/US-CCSS-MATH/G7/grade7-u01-l12-abc-cursor.json'));
 assert.equal(c.READY_FOR_STAGE_D,true);
 assert.equal(c.testExecution,'AUTHORED_NOT_RUN');
 assert.equal(c.D_STATUS,'NOT_RUN');
});
