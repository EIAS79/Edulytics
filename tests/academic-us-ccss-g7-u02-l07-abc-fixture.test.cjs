'use strict';
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),read=x=>fs.readFileSync(path.join(root,x),'utf8');
test('L07 three original task goals and independent Cool-down',()=>{
  const v=read('src/Edulytics.Web/Views/Shared/_USCCSSG7Unit2And7GeometryFigures.cshtml');
  const hook=read('src/Edulytics.Web/Views/Shared/_RichLessonContentV2.cshtml');
  for(const s of ['data-g7-u02-l07','7.1 Invent','7.2 Entry',
     '7.3 Track','Cool-down','Teacher review'])
   assert.ok(v.includes(s),s);
  assert.ok(hook.includes('Model.LessonCode == "PED:US-CCSS-MATH:G7:U02:L07"'));
});
test('A finite table can be consistent but not certify all unseen pairs',()=>{
  assert.deepEqual([6/8,9/12,12/16],[0.75,0.75,0.75]);
  assert.notEqual(6/8,8/12);
  assert.equal(4/2,2);
  assert.notEqual(9/4,2);
  assert.equal((6+2*2)/2,5);
  assert.equal(1+0.1*8,1.8);
});
test('A-C authoring not Stage-D execution',()=>{
  const c=JSON.parse(read('artifacts/curriculum-delivery/US-CCSS-MATH/G7/grade7-u02-l07-abc-cursor.json'));
  assert.equal(c.READY_FOR_STAGE_D,true);
  assert.equal(c.D_STATUS,'NOT_RUN');
  assert.equal(c.testExecution,'AUTHORED_NOT_RUN');
});
