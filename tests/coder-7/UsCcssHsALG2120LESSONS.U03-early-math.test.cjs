'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const close=(a,b,tol=1e-9)=>assert.ok(Math.abs(a-b)<=tol,`${a} != ${b}`);
test('U03 L01 integer powers, reciprocals and parenthesis',()=>{
  assert.equal(2**3*2**5,2**8);assert.equal(7**5,7**5);
  close(2**5/2**7,1/4);close(7**(-3),1/343);
  assert.equal(-(2**4),-16);assert.equal((-2)**4,16);
  assert.ok(!Number.isFinite(0**(-1)));
});
test('U03 L02 square area and cube volume root bounds',()=>{
  assert.equal(25-4*3,13);close(Math.sqrt(13)**2,13);
  assert.equal(3**3,27);assert.equal(4**3,64);
  assert.ok(Math.cbrt(50)>3&&Math.cbrt(50)<4);
});
test('U03 L03 unit fraction powers and principal roots',()=>{
  close(9**.5,3);close(3**.5,Math.sqrt(3));close(8**(1/3),2);
  close(7**(-1/3),1/Math.cbrt(7));
  assert.equal((-5)**2,25);assert.equal(5**2,25);
});
test('U03 L04 positive rational exponents and estimates',()=>{
  close(5**(2/3),Math.cbrt(25));close(5**(4/3),5*Math.cbrt(5));
  close((2**(1/3))**3,2);close((2**(2/3))**3,4);
  assert.ok(1.26**3>2&&1.26**3<2.001);
});
test('U03 L05 negative rational powers and graph values',()=>{
  close(9**(-.5),1/3);close(9**(-2),1/81);
  close(2**(-1/3),1/Math.cbrt(2));close((2**(-2/3))**3,1/4);
  assert.ok(2**(-2)<2**(-1)&&2**(-1)<2**0);
});
test('U03 L06 squared equations versus principal root',()=>{
  assert.equal((-2)**2,4);assert.equal(2**2,4);
  assert.equal(Math.sqrt(4)+Math.sqrt(9),5);assert.equal(Math.sqrt(25),5);
  assert.ok(Math.sqrt(25)!==-5);assert.ok(Number.isNaN(Math.sqrt(-1)));
});
test('U03 L07 squaring creates extraneous candidates',()=>{
  assert.equal((-2)**2-1,3);assert.equal(2**2-1,3);
  assert.equal((-5+3)**2,4);assert.equal((-1+3)**2,4);
  assert.equal(Math.sqrt(10-1),3);assert.notEqual(Math.sqrt(10-1),-3);
  assert.equal(Math.sqrt((-2)**2),2);
});
test('U03 L08 real cube roots have unique values',()=>{
  close(Math.cbrt(8),2);close(Math.cbrt(-8),-2);
  close(Math.cbrt(-64),-4);close(3.5**3,42.875);
  close(Math.cbrt(23+4),3);close(Math.cbrt(3-(-61))-4,0);
});
test('U03 L09 solve radicals with original-domain checks',()=>{
  close(Math.sqrt(30-5),5);close(Math.cbrt(130-5),5);
  close(Math.cbrt(-8),-2);close(Math.sqrt(8-1),Math.sqrt(7));
  assert.ok(Math.sqrt(4)+2!==0);
  for(const x of [0,1])close(Math.sqrt(x),Math.cbrt(x));
  for(const x of [.25,4,16])assert.ok(Math.abs(Math.sqrt(x)-Math.cbrt(x))>.01);
});
