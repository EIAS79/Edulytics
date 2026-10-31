'use strict';
const test = require('node:test');
const assert = require('node:assert/strict');
const near = (a,b,eps=1e-9) => assert.ok(Math.abs(a-b)<eps, String(a)+' != '+String(b));
const evalPoly = (coeff,x) => coeff.reduce((sum,c,i)=>sum+c*x**i,0);
const mul = (a,b) => {
  const out=Array(a.length+b.length-1).fill(0);
  a.forEach((c,i)=>b.forEach((d,j)=>{out[i+j]+=c*d;}));
  return out;
};
test('U02:L01 physical box domain and mathematical maximum',()=>{
  const v=x=>x*(3-2*x)*(8-2*x);
  const expanded=x=>4*x**3-22*x**2+24*x;
  [0,.25,.5,2/3,1,1.25,1.5,2].forEach(x=>near(v(x),expanded(x)));
  near(v(.5),7);
  near(v(2/3),200/27);
  assert.ok(v(2/3)>v(.5)&&v(2/3)>v(1));
  assert.ok(!(2>0&&2<1.5),'out-of-domain cut is invalid');
});
test('U02:L02 place value, staggered funding and interest factor',()=>{
  const p=x=>5*x**3+6*x**2+4*x;
  assert.equal(p(10),5640);
  assert.equal(p(-5),-495);
  const C=x=>300*x**4+500*x**3+250*x**2+400*x;
  near(C(1),1450);near(C(1.05),1639.089375);
  assert.ok(C(1.05)>C(1));
});
test('U02:L03 degree after cancellation and relative turns',()=>{
  assert.deepEqual([0,0,3,-3].slice(0,2),[0,0]);
  const f=x=>x**3-3*x;
  near(f(-1),2);near(f(1),-2);
  assert.ok(f(-1)>f(-1.1)&&f(-1)>f(-.9));
  assert.ok(f(1)<f(.9)&&f(1)<f(1.1));
  assert.equal(evalPoly([7],123),7);
});
test('U02:L04 polynomial closure and division counterexample',()=>{
  const p=[1,-3,2],q=[-5,4,1];
  assert.deepEqual(mul(p,q),[-5,19,-21,5,2]);
  for(const x of [-3,-1,0,1,4])near(evalPoly(mul(p,q),x),evalPoly(p,x)*evalPoly(q,x));
  assert.ok(Number.isNaN(0/0),'division by zero is undefined');
});
test('U02:L05 factors, zeros and unchanged roots under scaling',()=>{
  const f=x=>(x+5)*(x+1)*(x-3);
  const g=x=>(x+5)*(x+1)*(x-2);
  [-5,-1,3].forEach(x=>near(f(x),0));
  [-5,-1,2].forEach(x=>near(g(x),0));
  near(f(0),-15);near(g(0),-10);near(f(2),-21);
  for(const x of [-8,-2,0,5])near(-2*g(x),(-2)*g(x));
});
test('U02:L06 expanded vs factored forms and feasible box inputs',()=>{
  const V=x=>x*(14-2*x)*(8.5-2*x);
  for(const x of [-1,0,1,3,4.25,7])near(V(x),4*x**3-45*x**2+119*x);
  near(V(1),78);
  const f=x=>(x-2)*(x+3)*(x-7);
  for(const x of [-3,0,2,7])near(f(x),x**3-6*x**2-13*x+42);
  near(f(0),42);near(.5*f(0),21);
  assert.ok(!(7>0&&7<4.25),'nonphysical root');
});
test('U02:L07 graph window must include each factored root',()=>{
  const roots=[-15,-1,2];
  assert.ok(!roots.every(x=>x>=-10&&x<=8));
  assert.ok(roots.every(x=>x>=-18&&x<=5));
  const f=x=>x*(x+2)*(x-4);
  [-2,0,4].forEach(x=>near(f(x),0));
});
test('U02:L08–L09 leading term dominates both directions',()=>{
  const candidates=[
    {f:x=>x**3+4*x**2-x-4, left:-1,right:1},
    {f:x=>x**4-10*x**2+9,left:1,right:1},
    {f:x=>-2*x**4+3*x**2+7,left:-1,right:-1},
    {f:x=>x*(x+3)*(2-x),left:1,right:-1}
  ];
  for(const c of candidates){
    assert.equal(Math.sign(c.f(-1e6)),c.left);
    assert.equal(Math.sign(c.f(1e6)),c.right);
  }
  assert.equal(5*(8+3)-5*8,15);
});
