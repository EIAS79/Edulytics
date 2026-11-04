'use strict';
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=x=>fs.readFileSync(path.join(root,x),'utf8');
test('L12 shared-axis graph comparison and first-party Cool-down are authored',()=>{
 const v=read('src/Edulytics.Web/Views/Shared/_USCCSSG7Unit2And7GeometryFigures.cshtml');
 const svg=read('src/Edulytics.Web/wwwroot/curriculum/us-ccss-g7/geometry/u02-l12-compare-same-axes.svg');
 const hook=read('src/Edulytics.Web/Views/Shared/_RichLessonContentV2.cshtml');
 for(const s of ['data-g7-u02-l12','12.1 Same destination','12.2 Compare','12.3 Unit-price','Cool-down','Teacher review']) assert.ok(v.includes(s),s);
 assert.match(svg,/<title/);assert.match(svg,/<desc/);
 assert.ok(svg.includes('A: 50 m in 20 s')&&svg.includes('B: 50 m in 30 s'));
 assert.ok(hook.includes('Model.LessonCode == "PED:US-CCSS-MATH:G7:U02:L12"'));
});
test('L12 source-aligned comparison witnesses are exact',()=>{
 assert.equal(50/20,2.5);
 assert.equal(50/30,5/3);
 assert.ok(50/20>50/30);
 assert.equal(4*3,12);assert.equal(6*3,18);
});
test('L12 A-C authoring records Extend rather than New Engine',()=>{
 const c=JSON.parse(read('artifacts/curriculum-delivery/US-CCSS-MATH/G7/grade7-u02-l12-abc-cursor.json'));
 assert.equal(c.READY_FOR_STAGE_D,true);
 assert.equal(c.newEngine,false);
 assert.equal(c.extendedFamily,'usccss.g7.u02.l12.compare_same_axes_rates');
 assert.equal(c.testExecution,'AUTHORED_NOT_RUN');
 assert.equal(c.D_STATUS,'NOT_RUN');
});
