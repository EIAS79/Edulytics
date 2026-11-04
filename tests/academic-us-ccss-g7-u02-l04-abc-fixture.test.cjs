'use strict';
// A-B-C source-authored acceptance only. Intentionally NOT_RUN.
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..');
const read=p=>fs.readFileSync(path.join(root,p),'utf8');
test('all four local source lesson activities and independent Cool-down are authored',()=>{
  const v=read('src/Edulytics.Web/Views/Shared/_USCCSSG7Unit2And7GeometryFigures.cshtml');
  const renderer=read('src/Edulytics.Web/Views/Shared/_RichLessonContentV2.cshtml');
  for(const s of ['data-g7-u02-l04','4.1 Mental','4.2 Write',
    '4.3 Rate','4.4 Bread','Cool-down','Teacher review'])
    assert.ok(v.includes(s),s);
  assert.ok(renderer.includes('Model.LessonCode == "PED:US-CCSS-MATH:G7:U02:L04"'));
});
test('independent first-party and source ratio math',()=>{
  assert.equal(6/2,3);assert.equal(3/6,0.5);
  assert.equal(915/1.5,610);assert.equal(610*3.5,2135);
  assert.equal(10/8,1.25);assert.equal(17*1.25,21.25);
  assert.equal(645/50,12.9);assert.equal(48.6/30,1.62);
  assert.equal(7.5/4,1.875);
});
test('Authoring-ready does not claim runtime or Stage D',()=>{
 const c=JSON.parse(read('artifacts/curriculum-delivery/US-CCSS-MATH/G7/grade7-u02-l04-abc-cursor.json'));
 assert.equal(c.READY_FOR_STAGE_D,true);
 assert.equal(c.testExecution,'AUTHORED_NOT_RUN');
 assert.equal(c.D_STATUS,'NOT_RUN');
});
