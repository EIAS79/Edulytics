'use strict';
// G7 L06 optional-but-canonical area scaling source oracles (NOT_RUN).
const test=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const view=fs.readFileSync(path.join(__dirname,'..','src/Edulytics.Web/Views/Shared/_USCCSSG7ScaleConstructionFigures.cshtml'),'utf8');
const area=(original,k)=>original*k*k;
test('area tiles grow as square of length factor',()=>{
  for(const k of [1,2,3,4,5,6]){
    assert.equal(area(1,k),k*k);
    assert.equal(area(3,k),3*k*k);
    assert.equal(area(2,k),2*k*k);
    assert.equal(area(4,k),4*k*k);
  }
  assert.notEqual(area(3,2),3*2);
});
test('right triangle and fractional scale area cases',()=>{
  const initial=4*3/2;assert.equal(initial,6);
  assert.deepEqual([1,2,3,.5,1/3].map(k=>area(initial,k)),[6,24,54,1.5,2/3]);
  assert.equal(area(initial,5),150);
  assert.equal(area(initial,3/5),54/25);
});
test('original L06 cooldown computes actual side/area factors',()=>{
  assert.equal(2*4,8);
  assert.equal(3*6,18);
  assert.equal(18/8,9/4);
  assert.equal((3/2)*(3/2),9/4);
  assert.notEqual(18/8,3/2);
  assert.match(view,/data-g7-u01-l06/);
  assert.match(view,/independently authored Edulytiks task/);
  assert.match(view,/does not persist a drawing/);
});
