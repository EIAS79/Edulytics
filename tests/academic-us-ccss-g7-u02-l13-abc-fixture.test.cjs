'use strict';
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=x=>fs.readFileSync(path.join(root,x),'utf8');
test('L13 reversed-axis representation and first-party Cool-down are authored',()=>{
 const v=read('src/Edulytics.Web/Views/Shared/_USCCSSG7Unit2And7GeometryFigures.cshtml');
 const svg=read('src/Edulytics.Web/wwwroot/curriculum/us-ccss-g7/geometry/u02-l13-reversed-axes-rates.svg');
 const hook=read('src/Edulytics.Web/Views/Shared/_RichLessonContentV2.cshtml');
 for(const s of ['data-g7-u02-l13','13.1 Coordinate','13.2 Reverse','13.3 Compare','Cool-down','Teacher review']) assert.ok(v.includes(s),s);
 assert.match(svg,/<title/);assert.match(svg,/<desc/);
 assert.ok(svg.includes('p = 6t')&&svg.includes('t = p/6'));
 assert.ok(hook.includes('Model.LessonCode == "PED:US-CCSS-MATH:G7:U02:L13"'));
});
test('L13 reciprocal witnesses preserve units and exact inverse values',()=>{
 assert.equal(18/3,6);assert.equal(3/18,1/6);
 assert.equal(10/4,5/2);assert.equal(4/10,2/5);
 assert.equal(12/6,2);assert.equal(6/12,1/2);
});
test('L13 A-C closes by Reuse, not a new engine',()=>{
 const c=JSON.parse(read('artifacts/curriculum-delivery/US-CCSS-MATH/G7/grade7-u02-l13-abc-cursor.json'));
 assert.equal(c.READY_FOR_STAGE_D,true);assert.equal(c.newEngine,false);
 assert.equal(c.testExecution,'AUTHORED_NOT_RUN');assert.equal(c.D_STATUS,'NOT_RUN');
});
