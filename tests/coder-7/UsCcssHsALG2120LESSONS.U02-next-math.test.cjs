'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const near=(x,y,tol=1e-8)=>assert.ok(Math.abs(x-y)<tol, String(x)+' !== '+String(y));
test('U02 L10 multiplicity preserves root but changes crossing behavior',()=>{
 const f=x=>(x+1)*(x-3)**2;
 near(f(-1),0);near(f(3),0);near(f(0),9);
 assert.ok(f(-1.1)<0 && f(-.9)>0,'simple root crosses');
 assert.ok(f(2.9)>0 && f(3.1)>0,'double root touches');
 const g=x=>(x-3)**3;
 assert.ok(g(2.9)<0 && g(3.1)>0,'triple root crosses');
});
test('U02 L11 quadratic intersections and no canceled solution',()=>{
 const f=x=>(x+7)*(x-4),g=x=>x-4;
 [-6,4].forEach(x=>near(f(x),g(x)));
 near(f(-6),-10);near(f(4),0);
 const a=x=>x*x-7;
 [-3,3].forEach(x=>near(a(x),2));
});
test('U02 L12–L13 division gives quotient and precise remainder',()=>{
 const p=x=>x**3+5*x*x+2*x-8,q=x=>x*x+6*x+8;
 for(const x of [-4,-2,0,1,5])near(p(x),(x-1)*q(x));
 const c=x=>2*x**3+7*x*x+7*x+2,d=x=>2*x*x+5*x+2;
 for(const x of [-5,-1,0,1,3])near(c(x),(x+1)*d(x));
 for(const x of [-5,-1,0,1,3])near(x*x+1,(x-1)*(x+1)+2);
});
test('U02 L14 information-gap examples are underdetermined',()=>{
 const a=x=>-x*x*(x+4),b=x=>-x*(x+4)**2;
 for(const x of [-4,0]){near(a(x),0);near(b(x),0);}
 assert.notEqual(a(1),b(1));
 const p=x=>x**3-7*x+6;
 for(const x of [-3,1,2])near(p(x),(x-2)*(x+3)*(x-1));
});
test('U02 L15 remainder theorem and coefficient constraint',()=>{
 const f=(x,u)=>x**4-u*x**3+24*x*x-32*x+16;
 near(f(2,8),0);near(f(2,7),8);
 near(6*2**2-7*2-5,5);
});
test('U02 L16 452-cubic-centimeter cylinder surface area optimum',()=>{
 const V=452;
 const h=r=>V/(Math.PI*r*r);
 const S=r=>2*Math.PI*r*r+2*V/r;
 const r=(226/Math.PI)**(1/3);
 near(Math.PI*r*r*h(r),V);
 near(h(r),2*r);
 near(S(r),326.0419641142792,1e-6);
 assert.ok(S(r)<S(2)&&S(r)<S(4)&&S(r)<S(6));
});
test('U02 L17 preserved rational speed and vertical pole',()=>{
 const t=r=>10/r;
 near(t(8),1.25);near(t(40/3),.75);
 assert.ok(t(.001)>t(1));
 assert.ok(!Number.isFinite(t(0)),'speed zero undefined');
});
test('U02 L18 source paperback average and profit threshold',()=>{
 const c=x=>4+120/x;
 near(c(50),6.4);near(c(100),5.2);
 near(8*30-(120+4*30),0);
 near(8*31-(120+4*31),4);
 near((7+7)/7,1+7/7);
 assert.notEqual(7/(7+7),1+7/7);
});
