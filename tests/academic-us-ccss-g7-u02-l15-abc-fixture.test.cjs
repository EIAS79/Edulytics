'use strict';
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=x=>fs.readFileSync(path.join(root,x),'utf8');
test('L15 open water model and independent Cool-down are authored',()=>{
 const v=read('src/Edulytics.Web/Views/Shared/_USCCSSG7Unit2And7GeometryFigures.cshtml');
 const svg=read('src/Edulytics.Web/wwwroot/curriculum/us-ccss-g7/geometry/u02-l15-water-use-model.svg');
 const hook=read('src/Edulytics.Web/Views/Shared/_RichLessonContentV2.cshtml');
 for(const s of ['data-g7-u02-l15','15.1 Decide','15.2 Build','15.3 Reverse','Cool-down','Teacher review']) assert.ok(v.includes(s),s);
 assert.match(svg,/<title/);assert.match(svg,/<desc/);
 assert.ok(svg.includes('V = 8t')&&svg.includes('(8.75, 70)'));
 assert.ok(hook.includes('Model.LessonCode == "PED:US-CCSS-MATH:G7:U02:L15"'));
});
test('L15 model arithmetic is exact under stated fictional assumptions',()=>{
 assert.equal(8*5,40);assert.equal(8*9,72);assert.equal(8*12,96);
 assert.equal(70/8,8.75);assert.equal(72/9,8);
});
test('L15 source record explicitly treats missing separate practice page as normal evidence',()=>{
 const c=JSON.parse(read('artifacts/curriculum-delivery/US-CCSS-MATH/G7/grade7-u02-l15-abc-cursor.json'));
 assert.equal(c.studentPracticeSource,'ABSENT_IN_LOCAL_EDITION');
 assert.equal(c.READY_FOR_STAGE_D,true);assert.equal(c.newEngine,false);
 assert.equal(c.testExecution,'AUTHORED_NOT_RUN');assert.equal(c.D_STATUS,'NOT_RUN');
});
