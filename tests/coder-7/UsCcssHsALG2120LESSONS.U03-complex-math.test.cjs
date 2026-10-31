'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const close=(x,y,tol=1e-9)=>assert.ok(Math.abs(x-y)<=tol,`${x} != ${y}`);
const add=(z,w)=>[z[0]+w[0],z[1]+w[1]];
const mul=(z,w)=>[z[0]*w[0]-z[1]*w[1],z[0]*w[1]+z[1]*w[0]];
const check=(z,w)=>{close(z[0],w[0]);close(z[1],w[1]);};
test('U03 L10 no real square below zero, new unit squared is minus one',()=>{
  for(const x of [-9,-2,-.5,0,.5,2,9])assert.ok(x*x>=0);
  check(mul([0,1],[0,1]),[-1,0]);check(mul([0,3],[0,3]),[-9,0]);
});
test('U03 L11 imaginary square roots and complex-plane coordinates',()=>{
  check(mul([0,3],[0,3]),[-9,0]);check(mul([0,-3],[0,-3]),[-9,0]);
  check(mul([0,Math.sqrt(5)],[0,Math.sqrt(5)]),[-5,0]);
  check(mul([0,6],[0,6]),[-36,0]);
});
test('U03 L12 add/subtract and period-four powers of i',()=>{
  check(add([2,3],[-8,-8]),[-6,-5]);check(add([-3,2],[-4,5]),[-7,7]);
  let z=[1,0];for(let n=0;n<=8;n++){
    check(z,[[1,0],[0,1],[-1,0],[0,-1]][n%4]);z=mul(z,[0,1]);
  }
});
test('U03 L13 complex products preserve real and imaginary coefficients',()=>{
  check(mul([0,4],[0,3]),[-12,0]);check(mul([-3,9],[0,5]),[-45,-15]);
  check(mul([8,1],[-5,3]),[-43,19]);check(mul([3,2],[3,2]),[5,12]);
  check(mul([3,2],[3,-2]),[13,0]);
});
test('U03 L14 powers of i and one plus i',()=>{
  let i=[1,0];for(let n=1;n<=100;n++)i=mul(i,[0,1]);check(i,[1,0]);
  i=[1,0];for(let n=1;n<=38;n++)i=mul(i,[0,1]);check(i,[-1,0]);
  let z=[1,0];for(let n=1;n<=8;n++)z=mul(z,[1,1]);check(z,[16,0]);
});
test('U03 L15 unique complex factor given nonzero first factor',()=>{
  check(mul([2,3],[4,2]),[2,16]);
  const a=(2*2+3*16)/(2*2+3*3);const b=(2*16-3*2)/(2*2+3*3);
  close(a,4);close(b,2);
  check(add([10,4],[-10,-7]),[0,-3]);
});
test('U03 L16 equivalent quadratic radical solutions',()=>{
  for(const sign of [-1,1]){const x=3+sign*Math.sqrt(2);close(x*x-6*x+7,0);}
  close(Math.sqrt(8)/2,Math.sqrt(2));
});
test('U03 L17 completing square complex root cases',()=>{
  for(const sign of [-1,1]){const x=4+sign*Math.sqrt(3);close(x*x-8*x+13,0);}
  for(const sign of [-1,1]){
    const x=[4,sign*Math.sqrt(3)],square=mul(x,x);
    const polynomial=add(add(square,[-8*x[0],-8*x[1]]),[19,0]);check(polynomial,[0,0]);
  }
});
test('U03 L18 quadratic formula complex verification',()=>{
  for(const sign of [-1,1]){
    const x=[6,sign*Math.sqrt(5)];check(add(add(mul(x,x),[-12*x[0],-12*x[1]]),[41,0]),[0,0]);
    const y=[5/3,sign*5*Math.sqrt(5)/3];
    const y2=mul(y,y);check(add(add([3*y2[0],3*y2[1]],[-10*y[0],-10*y[1]]),[50,0]),[0,0]);
  }
});
test('U03 L19 discriminant distinguishes graph intercepts',()=>{
  const delta=(a,b,c)=>b*b-4*a*c;
  assert.ok(delta(2,4,0)>0);assert.equal(delta(2,4,2),0);assert.ok(delta(2,4,4)<0);
  check(mul([-1,1],[-1,1]),[0,-2]);
  for(const sign of [-1,1]){const x=[-1,sign],x2=mul(x,x);check(add(add([2*x2[0],2*x2[1]],[4*x[0],4*x[1]]),[4,0]),[0,0]);}
});
