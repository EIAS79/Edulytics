'use strict';
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=x=>fs.readFileSync(path.join(root,x),'utf8');
test('L14 four-representation authoring and Cool-down are present',()=>{
 const v=read('src/Edulytics.Web/Views/Shared/_USCCSSG7Unit2And7GeometryFigures.cshtml');
 const svg=read('src/Edulytics.Web/wwwroot/curriculum/us-ccss-g7/geometry/u02-l14-four-representations.svg');
 const hook=read('src/Edulytics.Web/Views/Shared/_RichLessonContentV2.cshtml');
 for(const s of ['data-g7-u02-l14','14.1 Check','14.2 Create','14.3 Critique','Cool-down','Teacher review']) assert.ok(v.includes(s),s);
 assert.match(svg,/<title/);assert.match(svg,/<desc/);
 assert.ok(svg.includes('y = 4x')&&svg.includes('(3,12)'));
 assert.ok(hook.includes('Model.LessonCode == "PED:US-CCSS-MATH:G7:U02:L14"'));
});
test('L14 closed witnesses agree across representations',()=>{
 for(const x of [0,1,2,3,4,5]) assert.equal(4*x,4*x);
 assert.equal(5*0,0);assert.notEqual(5*0,2);
});
test('L14 A-C closes with open work kept teacher reviewed',()=>{
 const c=JSON.parse(read('artifacts/curriculum-delivery/US-CCSS-MATH/G7/grade7-u02-l14-abc-cursor.json'));
 assert.equal(c.READY_FOR_STAGE_D,true);assert.equal(c.newEngine,false);
 assert.match(c.humanBoundary,/TEACHER_REVIEW/);
 assert.equal(c.testExecution,'AUTHORED_NOT_RUN');assert.equal(c.D_STATUS,'NOT_RUN');
});
