'use strict';
const fs=require('node:fs'),path=require('node:path'),assert=require('node:assert/strict');
const root=path.resolve(__dirname,'../..');
const base=path.join(root,'artifacts/curriculum-delivery/US-CCSS-MATH/G7');
const counts=[13,15,11,16,17,23,17,20,13];
const code=(u,l)=>'PED:US-CCSS-MATH:G7:U'+String(u).padStart(2,'0')+':L'+String(l).padStart(2,'0');
const read=p=>JSON.parse(fs.readFileSync(p,'utf8'));
const entry=(u,l)=>path.join(base,'grade7-u'+String(u).padStart(2,'0')+'-l'+String(l).padStart(2,'0')+'-abc-cursor.json');
const blue=read(path.join(root,'src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/us-ccss-math-grade7-our-2017.lesson-blueprint.json'));
const closure=read(path.join(base,'grade7-abc-full-grade-closure-145-of-145-20261104.json'));
assert.equal(blue.Lessons.length,145,'blueprint must contain exactly 145 lessons');
assert.equal(new Set(blue.Lessons.map(x=>x.LessonCode)).size,145,'duplicate lesson codes');
assert.equal(closure.lessonsClosed,145,'A-C handoff not 145/145');
assert.equal(closure.status,'READY_FOR_STAGE_D','handoff not ready');
let ordinal=0,missing=[];
for(let u=1;u<=9;u++) for(let l=1;l<=counts[u-1];l++){
 ordinal++;
 const p=entry(u,l);
 if(!fs.existsSync(p)){missing.push(code(u,l));continue;}
 const c=read(p);
 assert.equal(c.READY_FOR_STAGE_D,true,'not ready: '+code(u,l));
 assert.ok(c.D_STATUS===undefined||c.D_STATUS==='NOT_RUN','stale D status: '+code(u,l));
 assert.ok(['AUTHORED_NOT_RUN','TEST_AUTHORED_NOT_RUN',undefined].includes(c.testExecution),'test status: '+code(u,l));
 assert.equal(c.ordinal??ordinal,ordinal,'ordinal: '+code(u,l));
 assert.equal(c.LessonCode??c.lessonCode,code(u,l),'code mismatch: '+code(u,l));
}
assert.deepEqual(missing,[],'missing A-C cursor(s)');
assert.equal(ordinal,145);
if(process.argv[2]==='shard'){
 const batch=process.argv[3],bounds={D1:[1,50],D2:[51,100],D3:[101,145]};
 assert.ok(Object.hasOwn(bounds,batch),'unknown shard');
 let files=[],idx=0;
 for(let u=1;u<=9;u++)for(let l=1;l<=counts[u-1];l++){
  idx++;
  if(idx<bounds[batch][0]||idx>bounds[batch][1])continue;
  const f=path.join(root,'tests/academic-us-ccss-g7-u'+String(u).padStart(2,'0')+'-l'+String(l).padStart(2,'0')+'-abc-fixture.test.cjs');
  assert.ok(fs.existsSync(f),'missing lesson test: '+code(u,l));
  files.push(path.relative(root,f).replaceAll('\\','/'));
 }
 assert.equal(files.length,bounds[batch][1]-bounds[batch][0]+1);
 process.stdout.write(files.join('\n')+'\n');
}else {
 console.log('G7 candidate inventory: 145/145 A-C handoffs checked; D tests NOT yet verified');
 console.log('Source-element parity / human approval / runtime tests are independent, not granted by this inventory');
}