'use strict';
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=x=>fs.readFileSync(path.join(root,x),'utf8');
test('L09 info gap, rankings and first-party cooldown present',()=>{
 const v=read('src/Edulytics.Web/Views/Shared/_USCCSSG7Unit2And7GeometryFigures.cshtml');
 const h=read('src/Edulytics.Web/Views/Shared/_RichLessonContentV2.cshtml');
 for(const s of ['data-g7-u02-l09','9.1 Ask','9.2 Two-card',
 '9.3 Compare','Cool-down','Teacher review'])
  assert.ok(v.includes(s),s);
 assert.ok(h.includes('Model.LessonCode == "PED:US-CCSS-MATH:G7:U02:L09"'));
});
test('Independent rates, totals and clock arithmetic',()=>{
 assert.equal(10000/156.25,64);
 assert.equal(1325/5*200,53000);
 assert.equal(331*120,39720);
 assert.equal(800/3*150,40000);
 assert.ok(331>800/3&&800/3>265&&265>50000/210);
});
test('Source checkpoint is authoring-only',()=>{
 const c=JSON.parse(read('artifacts/curriculum-delivery/US-CCSS-MATH/G7/grade7-u02-l09-abc-cursor.json'));
 assert.equal(c.READY_FOR_STAGE_D,true);
 assert.equal(c.D_STATUS,'NOT_RUN');
 assert.equal(c.testExecution,'AUTHORED_NOT_RUN');
});
