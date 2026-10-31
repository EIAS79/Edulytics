'use strict';
const test = require('node:test');
const assert = require('node:assert/strict');
const close = (a,b,tol=1e-9) => assert.ok(Math.abs(a-b)<=tol, `${a} != ${b}`);

test('U02 L19 long division and fuel-efficiency rational end behavior',()=>{
  const a=x=>(2*x**3+7*x*x+7*x+5)/(x+1);
  const b=x=>2*x*x+5*x+2+3/(x+1);
  for(const x of [-7,-2,0,1,6,100])close(a(x),b(x),1e-7);
  const efficiency=x=>100*x*(x+10)/(100*x+450);
  for(const x of [5,10,25,50,100])close(efficiency(x),x+5.5-2475/(100*x+450));
  assert.ok(!Number.isFinite(a(-1)));
});
test('U02 L20 shirts and batting averages require whole counts',()=>{
  const shirts=x=>4.25+35/x;
  assert.ok(shirts(46)>5 && shirts(47)<=5);
  close(shirts(25),5.65);close(shirts(100),4.60);
  const batting=x=>(24+x)/(110+x);
  assert.ok(batting(12)<.3 && batting(13)>=.3);
  const afterMisses=x=>(24+x)/(113+x);
  assert.ok(afterMisses(14)<.3 && afterMisses(15)>=.3);
});
test('U02 L21 boat-time equality and parallel resistor formula',()=>{
  const r=8/3;
  close(5/(8-r),15/16);close(10/(8+r),15/16);
  assert.ok(r>=0 && r<8);
  close(1/(1/40+1/60),24);
});
test('U02 L22 extraneous roots rejected by original denominators',()=>{
  const f=x=>(5*x+5)/(x+1),g=x=>5/x;
  close(f(1),g(1));
  assert.ok(!Number.isFinite(f(-1)) || Number.isNaN(f(-1)));
  assert.ok(!Number.isFinite(g(0)));
  const h=x=>3/(x*(x-2)),k=x=>(2*x+1)/(x-2);
  for(const x of [-1.5,1])close(h(x),k(x));
  for(const x of [0,2])assert.ok(!Number.isFinite(h(x)));
});
test('U02 L23 consecutive squares and telescoping identities',()=>{
  for(const n of [1,2,5,30,41])close(n*n-(n-1)**2,2*n-1);
  for(const x of [-3,-1,0,1,1.5,2])for(const n of [1,2,3,4,8]){
    const s=Array.from({length:n},(_,k)=>x**k).reduce((a,b)=>a+b,0);
    close((x-1)*s,x**n-1,1e-8);
  }
});
test('U02 L24 Pythagorean triples and fraction identity',()=>{
  for(const [a,b] of [[2,1],[3,2],[5,1],[10,7]]){
    close((a*a-b*b)**2+(2*a*b)**2,(a*a+b*b)**2);
    assert.ok(a*a-b*b>0);
  }
  for(const d of [1,3,15,60])close(2/d,1/d+1/(2*d)+1/(3*d)+1/(6*d));
});
test('U02 L25 finite geometric sums including r=1',()=>{
  const sum=(a,r,n)=>Array.from({length:n},(_,k)=>a*r**k).reduce((x,y)=>x+y,0);
  for(const [a,r,n] of [[5,2,5],[3,4,3],[2,.5,10],[7,1,6]]){
    const formula=r===1?a*n:a*(1-r**n)/(1-r);
    close(sum(a,r,n),formula,1e-8);
  }
  assert.equal(4**5-1,1023);
});
test('U02 L26 house projection and savings contribution threshold',()=>{
  const houses=Array.from({length:6},(_,k)=>71*1.18**k).reduce((a,b)=>a+b,0);
  close(houses,71*(1.18**6-1)/.18,1e-9);
  assert.ok(houses>670 && houses<671);
  close(1-2**(-50),Array.from({length:50},(_,k)=>2**(-(k+1))).reduce((a,b)=>a+b,0));
  const factor=(1.05**50-1)/.05;
  assert.ok(477*factor<100000 && 478*factor>100000);
});
