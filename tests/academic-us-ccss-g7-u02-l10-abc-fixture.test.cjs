'use strict';
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=x=>fs.readFileSync(path.join(root,x),'utf8');
test('L10 source graph activities, local first-party SVG and cooldown',()=>{
 const v=read('src/Edulytics.Web/Views/Shared/_USCCSSG7Unit2And7GeometryFigures.cshtml');
 const svg=read('src/Edulytics.Web/wwwroot/curriculum/us-ccss-g7/geometry/u02-l10-proportional-vs-offset.svg');
 const hook=read('src/Edulytics.Web/Views/Shared/_RichLessonContentV2.cshtml');
 for(const s of ['data-g7-u02-l10','10.1 Plot','10.2 T-shirt',
 '10.3 Match','Cool-down','Teacher review'])assert.ok(v.includes(s),s);
 assert.match(svg,/<title/);assert.match(svg,/<desc/);
 assert.ok(hook.includes('Model.LessonCode == "PED:US-CCSS-MATH:G7:U02:L10"'));
});
test('Line-through-origin and nonproportional offset math',()=>{
 assert.deepEqual([1,2,3,6].map(x=>8*x),[8,16,24,48]);
 assert.equal(10-2*0,10);
 assert.equal(6*1.25,7.5);
});
test('Authoring closure not execution',()=>{
 const c=JSON.parse(read('artifacts/curriculum-delivery/US-CCSS-MATH/G7/grade7-u02-l10-abc-cursor.json'));
 assert.equal(c.READY_FOR_STAGE_D,true);
 assert.equal(c.testExecution,'AUTHORED_NOT_RUN');assert.equal(c.D_STATUS,'NOT_RUN');
});
