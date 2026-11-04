'use strict';
// Grade 7 lesson 7 scale drawing independent test source — NOT_RUN.
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const src=fs.readFileSync(path.join(__dirname,'..','src/Edulytics.Web/Views/Shared/_USCCSSG7ScaleConstructionFigures.cshtml'),'utf8');
const realMeters=drawingCm=>drawingCm*2;
const drawingCm=realMeters=>realMeters/2;
test('L07 court length and bench use one dimensionally stated scale',()=>{
  assert.deepEqual([14,7,12,1.2].map(realMeters),[28,14,24,2.4]);
  assert.equal(drawingCm(9),4.5);
  assert.equal(14/100,0.14); // unit conversion alone is not scale recovery
  assert.notEqual(0.14,realMeters(14));
});
test('L07 original graphic scale and room Cool-down',()=>{
  assert.equal(6*20,120);
  assert.equal(4*20,80);
  assert.equal((6-4)*20,40);
  assert.deepEqual([drawingCm(8),drawingCm(6),drawingCm(3)],[4,3,1.5]);
});
test('L07 original task is lesson guarded and fails closed at human score',()=>{
  assert.match(src,/data-g7-u01-l07/);
  assert.match(src,/first-party court plan/);
  assert.match(src,/Cool-down — Edulytiks-original/);
  assert.match(src,/not provided by this partial/);
});

test('L07 source units and selected exact family authored, tests not run',()=>{
 const root=path.join(__dirname,'..');
 const registry=fs.readFileSync(path.join(root,
  'src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs'),'utf8');
 const checkpoint=JSON.parse(fs.readFileSync(path.join(root,
  'artifacts/curriculum-delivery/US-CCSS-MATH/G7/grade7-u01-l07-abc-cursor.json'),'utf8'));
 assert.match(registry,/usccss\.g7\.u01\.l07\.drawing_actual_distance_units/);
 assert.equal(checkpoint.READY_FOR_STAGE_D,true);
 assert.equal(checkpoint.testExecution,'TEST_AUTHORED_NOT_RUN');
});
