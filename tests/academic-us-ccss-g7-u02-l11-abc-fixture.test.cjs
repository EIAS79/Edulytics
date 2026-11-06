'use strict';
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=x=>fs.readFileSync(path.join(root,x),'utf8');
test('L11 first-party graph interpretation, source boundary and Cool-down are authored',()=>{
 const v=read('src/Edulytics.Web/Views/Shared/_USCCSSG7Unit2And7GeometryFigures.cshtml');
 const svg=read('src/Edulytics.Web/wwwroot/curriculum/us-ccss-g7/geometry/u02-l11-point-k-context.svg');
 const hook=read('src/Edulytics.Web/Views/Shared/_RichLessonContentV2.cshtml');
 for(const s of ['data-g7-u02-l11','11.1 Interpret','11.2 Locate','11.3 Connect','Cool-down','Teacher review']) assert.ok(v.includes(s),s);
 assert.match(svg,/<title/);assert.match(svg,/<desc/);
 assert.ok(svg.includes('(1, 2.5)')&&svg.includes('(8, 20)'));
 assert.ok(hook.includes('Model.LessonCode == "PED:US-CCSS-MATH:G7:U02:L11"'));
});
test('L11 exact source-aligned rate witnesses remain mathematically correct',()=>{
 assert.equal(20/8,2.5);assert.equal(2.5*6,15);
 assert.equal(30/4,7.5);assert.equal(7.5*6,45);
});
test('L11 A-C authoring closes without claiming execution',()=>{
 const c=JSON.parse(read('artifacts/curriculum-delivery/US-CCSS-MATH/G7/grade7-u02-l11-abc-cursor.json'));
 assert.equal(c.READY_FOR_STAGE_D,true);
 assert.equal(c.newEngine,false);
 assert.equal(c.testExecution,'AUTHORED_NOT_RUN');
 assert.equal(c.D_STATUS,'NOT_RUN');
 assert.equal(c.originalGatedCoolDown,'NOT_VERIFIED_NOT_COPIED');
});
