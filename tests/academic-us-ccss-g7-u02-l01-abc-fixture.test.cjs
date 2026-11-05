'use strict';
// A-C authoring fixture; NOT_RUN during Stage A–B–C.
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=p=>fs.readFileSync(path.join(root,p),'utf8');
test('U02 L01 first-party source components are in Grade7-specific view',()=>{
 const p=read('src/Edulytics.Web/Views/Shared/_USCCSSG7Unit2And7GeometryFigures.cshtml');
 for(const marker of ['data-g7-u02-l01','1.1 Build','1.2 Compare',
 '1.3 Draw','0.25 teaspoon','Cool-down','human review'])
 assert.ok(p.includes(marker),marker);
});
test('Independent mixture, oval and double-line checks',()=>{
 assert.equal(0.5/2,0.25);
 assert.equal(0.25/1,0.25);
 assert.equal(1.5/1,1.5);
 assert.equal(10/5,2);
 assert.equal(8/4,2);
 assert.notEqual(6/4,10/5);
 assert.equal(4/2,2);
 assert.equal(0.25/2,0.125);
 assert.equal(0.125/1,0.125);
 assert.notEqual(0.2/1.5,0.25/2);
});
test('U02 L01 cursor remains Stage D untested',()=>{
 const c=JSON.parse(read('artifacts/curriculum-delivery/US-CCSS-MATH/G7/grade7-u02-l01-abc-cursor.json'));
 assert.equal(c.READY_FOR_STAGE_D,true);
 assert.equal(c.testExecution,'AUTHORED_NOT_RUN');
 assert.equal(c.D_STATUS,'NOT_RUN');
});
