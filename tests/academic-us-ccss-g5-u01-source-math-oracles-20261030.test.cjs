'use strict';
// Grade 5 Unit 1 source-specific math oracles. WRITTEN_NOT_RUN (A–B–C phase).
const test = require('node:test');
const assert = require('node:assert/strict');
test('U01 construction and packed prisms respect unit-cube counts',()=>{
 assert.deepEqual([2*2*2,5+3,2*1*4,5*1*2],[8,8,8,10]);
 assert.deepEqual([4*2*5,4*3*5,4*4*5],[40,60,80]);
 assert.equal(5*24,120);assert.equal(6*20,120);assert.equal(4*30,120);
 assert.deepEqual([40*1,40*2,40*3,40*10,40*25],[40,80,120,400,1000]);
});
test('U01 composite volumes and units use independent arithmetic',()=>{
 assert.equal(3*5*3+1,46);assert.equal(3*5*3+5,50);assert.equal(3*6*3+6,60);assert.equal(3*5*3+2*2*3,57);
 assert.equal(2*7*2+5*5*2,78);assert.equal(9*3*7+5*2*7,259);
 assert.equal(5*8*6+5*4*9,420);assert.equal(5*4*3+5*12*6,420);
 assert.equal(5*5*5-2*3*5,95);assert.equal(3*5*5+2*2*5,95);
 assert.equal(4*8*3+6*4*3,168);assert.equal(10*4*3+4*4*3,168);assert.equal(10*8*3-6*4*3,168);
 assert.equal(10*6*5,300);assert.equal(4*9*5,180);assert.equal(30*9,270);
 assert.equal(10*3*4+8*3*4,216);
});
test('U01 2021 optional modeling preserves 60 containers not fabricated ship estimate',()=>{
 for(const a of [[1,6,10],[2,5,6],[3,4,5],[2,3,10],[1,3,20]])assert.equal(a.reduce((x,y)=>x*y,1),60);
});
