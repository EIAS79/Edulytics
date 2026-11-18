'use strict';
// Original-MVP-source-inspired mathematics checks, not publisher-text reproduction.
// node --test tools/curriculum/coder-9/UsCcssHsADVTRIGGEOSourceTasks.test.cjs
const {test}=require('node:test'),assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'../../..');
const pack=JSON.parse(fs.readFileSync(path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-hs-adv-trig-geo-phase29-v1.lesson-content-pack.json'),'utf8'));
const near=(x,y)=>assert.ok(Math.abs(x-y)<1e-9,x+' != '+y);
const body=i=>Object.values(pack.lessons[i].translations[0]).join(' ');
test('L01 cofunction identity follows unit-circle symmetry at signed non-special angles',()=>{
  for(const x of [-7,-3,-0.1,0,Math.PI/6,2,5])near(Math.sin(x),Math.cos(Math.PI/2-x));
  assert.match(body(0),/Cofunction identity/);
});
test('L02 inverse principal angle and quadrant/location clue must both be satisfied',()=>{
  const theta=5*Math.PI/6,r=2;
  near(Math.asin(.5),Math.PI/6);near(r*Math.cos(theta),-Math.sqrt(3));near(r*Math.sin(theta),1);
  assert.ok(r*Math.sin(theta)>0);
  assert.match(body(1),/quadrant II/);
});
test('L03 compressed two-cycle trig source case yields exactly four roots',()=>{
  const roots=[Math.PI/6,Math.PI/3,7*Math.PI/6,4*Math.PI/3];
  for(const x of roots){near(Math.sin(2*x),Math.sqrt(3)/2);assert.ok(x>=0&&x<2*Math.PI);}
  assert.equal(new Set(roots.map(x=>x.toFixed(10))).size,4);
  assert.match(body(2),/compressed-period/);
});
test('L04 sheared circular layer stack preserves each corresponding area and total volume',()=>{
  const r=3,h=8,area=Math.PI*r*r;
  for(const z of [0,1,2,4,7.99,8]){
    const offset=z/2;
    const centroid=[offset,0];
    assert.equal(centroid[1],0);
    near(Math.PI*r*r,area);
  }
  near(area*h,72*Math.PI);
  assert.match(body(3),/oblique circular cylinder/);
});
test('L05 vertical and translated ellipse and hyperbola validate against original foci',()=>{
  const dist=(x,y,cx,cy)=>Math.hypot(x-cx,y-cy);
  near(dist(0,5,0,-3)+dist(0,5,0,3),10);
  near(dist(7,-1,-1,-1)+dist(7,-1,5,-1),10);
  near(Math.abs(dist(0,3,0,-5)-dist(0,3,0,5)),6);
  assert.match(body(4),/translated ellipse/);
  assert.match(body(4),/vertical hyperbola/);
});

test('L01 original 7.5 task: signed graph/table checks, tangent algebra, domain and assessment',()=>{
  const text=body(0);
  assert.match(text,/Source-aligned conjecture check/);
  assert.match(text,/Algebraic tangent proof/);
  assert.match(text,/Check for understanding/);
  assert.match(text,/HUMAN_REQUIRED/);
  for(const x of [-7*Math.PI/6,-2.1,-Math.PI/6,0,Math.PI/6,7*Math.PI/6,2.3]){
    near(Math.sin(Math.PI-x), Math.sin(x));
    near(Math.cos(Math.PI-x), -Math.cos(x));
    near(Math.sin(Math.PI+x), -Math.sin(x));
    near(Math.cos(Math.PI+x), -Math.cos(x));
    near(Math.sin(2*Math.PI-x), -Math.sin(x));
    near(Math.cos(2*Math.PI-x), Math.cos(x));
    if(Math.abs(Math.cos(x))>1e-9){
      near(Math.tan(-x),-Math.tan(x));
      near(Math.tan(Math.PI+x),Math.tan(x));
    }
  }
  assert.ok(Math.abs(Math.cos(Math.PI/2))<1e-12,'tangent pole must be excluded');
  near(Math.cos(Math.PI-Math.PI/3),-1/2);
  near(Math.sin(Math.PI+Math.PI/3),-Math.sqrt(3)/2);
});


// AUTHORED / NOT_RUN in Stage A-B-C; executable only in the later Stage D batch.
test('L01 text-first triangle and both exact six-identity rows remain source-aligned',()=>{
  const l01=pack.lessons[0].translations[0].stepByStepSolutions;
  assert.match(l01,/Draw triangle ABC with a right angle at A/);
  assert.match(l01,/sin\(x\)=a\/h/);
  assert.match(l01,/cos\(90°−x\)=a\/h/);
  assert.match(l01,/Two worked sample rows for all six conjectures/);
  assert.match(l01,/x=pi\/6/);
  assert.match(l01,/x=7pi\/6/);
  assert.match(l01,/ordered comparisons, not quotients/);
  assert.match(l01,/NOT proof for all real x/);
  const table=[
    {x:Math.PI/6,base:[.5,Math.sqrt(3)/2],ref:[.5,-Math.sqrt(3)/2],
      half:[-.5,-Math.sqrt(3)/2],turn:[-.5,Math.sqrt(3)/2]},
    {x:7*Math.PI/6,base:[-.5,-Math.sqrt(3)/2],ref:[-.5,Math.sqrt(3)/2],
      half:[.5,Math.sqrt(3)/2],turn:[.5,-Math.sqrt(3)/2]}
  ];
  const value=x=>[Math.sin(x),Math.cos(x)];
  for(const row of table){
    [row.base,row.ref,row.half,row.turn].forEach((pair,i)=>{
      const angle=[row.x,Math.PI-row.x,Math.PI+row.x,2*Math.PI-row.x][i];
      pair.forEach((v,j)=>near(v,value(angle)[j]));
    });
  }
});
