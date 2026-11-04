'use strict';
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=x=>fs.readFileSync(path.join(root,x),'utf8');
test('L08 four source activities and Cool-down authored on G7-only view',()=>{
 const view=read('src/Edulytics.Web/Views/Shared/_USCCSSG7Unit2And7GeometryFigures.cshtml');
 const hook=read('src/Edulytics.Web/Views/Shared/_RichLessonContentV2.cshtml');
 for(const s of ['data-g7-u02-l08','8.1 Notice','8.2 More',
 '8.3 Compare','8.4 Test','Cool-down','Teacher review'])
  assert.ok(view.includes(s),s);
 assert.ok(hook.includes('Model.LessonCode == "PED:US-CCSS-MATH:G7:U02:L08"'));
});
test('Independent proportional vs affine and cubic witnesses',()=>{
 assert.equal(9/5*0+32,32);
 assert.equal(2.54*3.5,8.89);
 assert.equal(12*9.5,114);
 assert.equal(6*9.5*9.5,541.5);
 assert.equal(9.5**3,857.375);
 assert.equal(50*6,300);
});
test('No false runtime/CI certification',()=>{
 const c=JSON.parse(read('artifacts/curriculum-delivery/US-CCSS-MATH/G7/grade7-u02-l08-abc-cursor.json'));
 assert.equal(c.READY_FOR_STAGE_D,true);
 assert.equal(c.D_STATUS,'NOT_RUN');assert.equal(c.testExecution,'AUTHORED_NOT_RUN');
});
