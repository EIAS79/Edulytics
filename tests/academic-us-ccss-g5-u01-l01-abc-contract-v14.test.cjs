'use strict';
// Grade 5 U01:L01: independent grade-owned contract fixtures, NOT deployed Practice engine.
// Expected cases model 2021 math concepts; they do NOT prove actual runtime selection or grading.
const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const richPath = path.resolve(__dirname, '../src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g5-u01-l01-l03-2021.rich-lesson-v2.json');
const specPath = path.resolve(__dirname, '../artifacts/curriculum-delivery/US-CCSS-MATH/G5/u01-l01-practice-human-rubric-v14.json');

function inspectSolid(points) {
  if (!Array.isArray(points) || points.length === 0) return {valid: false, reason: 'INVALID_COLLECTION'};
  if (points.some(p => !Array.isArray(p) || p.length !== 3 || p.some(v => !Number.isSafeInteger(v))))
    return {valid: false, reason: 'MALFORMED_COORDINATES'};
  const keys = new Set(points.map(p => p.join(',')));
  if (keys.size !== points.length) return {valid: false, reason: 'DUPLICATE_OVERLAP'};
  const visited = new Set([points[0].join(',')]);
  const pending = [points[0]];
  while (pending.length) {
    const point = pending.pop();
    for (let axis = 0; axis < 3; axis++) for (const delta of [-1, 1]) {
      const next = [...point]; next[axis] += delta;
      const key = next.join(',');
      if (keys.has(key) && !visited.has(key)) {visited.add(key);pending.push(next);}
    }
  }
  if (visited.size !== points.length) return {valid: false, reason: 'DISCONNECTED'};
  return {valid: true, count: points.length};
}
const countSort = values => [...values].sort((a,b) => a - b);
const slab8 = Array.from({length: 2}, (_,y) => Array.from({length: 4},(_,x) => [x,y,0])).flat();
const tower8 = Array.from({length: 8},(_,z) => [0,0,z]);
const slab9 = Array.from({length: 3},(_,y) => Array.from({length: 3},(_,x) => [x,y,0])).flat();
const stair9 = [4,3,2].flatMap((width,y) => Array.from({length:width},(_,x) => [x,y,0]));

function normalize(points) {
  const mins = [0,1,2].map(axis => Math.min(...points.map(p => p[axis])));
  return points.map(p => p.map((v,axis) => v-mins[axis]).join(',')).sort().join(';');
}
function properRotations(points) {
  const perms = [[0,1,2],[0,2,1],[1,0,2],[1,2,0],[2,0,1],[2,1,0]];
  const images=[];
  for (const perm of perms) {
    let swaps=0;
    for (let a=0;a<3;a++) for(let b=a+1;b<3;b++) if (perm[a]>perm[b]) swaps++;
    const parity=swaps%2===0?1:-1;
    for(const sx of [-1,1]) for(const sy of [-1,1]) for(const sz of [-1,1]) {
      if(parity*sx*sy*sz!==1)continue;
      images.push(points.map(p=>[sx*p[perm[0]],sy*p[perm[1]],sz*p[perm[2]]]));
    }
  }
  return images;
}
function sameShapeUpToRotationAndTranslation(a,b) {
  if (!inspectSolid(a).valid || !inspectSolid(b).valid || a.length!==b.length)return false;
  const key=normalize(a);
  return properRotations(b).some(rotated => normalize(rotated)===key);
}

test('eight-cube connected low and tall solids have equal volume but different shape',()=>{
  assert.deepEqual(inspectSolid(slab8),{valid:true,count:8});
  assert.deepEqual(inspectSolid(tower8),{valid:true,count:8});
  assert.equal(sameShapeUpToRotationAndTranslation(slab8,tower8),false);
});
test('nine-cube connected slab and stair have equal volume but distinct non-prism configuration',()=>{
  assert.deepEqual(inspectSolid(slab9),{valid:true,count:9});
  assert.deepEqual(inspectSolid(stair9),{valid:true,count:9});
  assert.equal(sameShapeUpToRotationAndTranslation(slab9,stair9),false);
});
test('proper rotations and translations preserve shape and cube count',()=>{
  const rotated = slab9.map(([x,y,z]) => [z+7,x-11,y+3]);
  assert.equal(sameShapeUpToRotationAndTranslation(slab9,rotated),true);
  assert.equal(properRotations(slab9).length,24);
});
test('count ordering, ties, and height-based misconceptions have independent mathematical oracle',()=>{
  assert.deepEqual(countSort([11,5,9,8]),[5,8,9,11]);
  assert.deepEqual(countSort([8,9,8,5]),[5,8,8,9]);
  assert.equal(tower8.length,slab8.length);
  assert.equal(slab9.length>tower8.length,true); // low model can occupy more space than tall tower
});
test('reject disconnected, overlapping, wrong-count, malformed and scalar-only construction answers',()=>{
  const disconnected=[...tower8.slice(0,8),[99,0,0]];
  assert.deepEqual(inspectSolid(disconnected),{valid:false,reason:'DISCONNECTED'});
  assert.deepEqual(inspectSolid([[0,0,0],[1,1,0]]),{valid:false,reason:'DISCONNECTED'},
    'Cubes touching only along an edge do not form one face-connected solid');
  assert.deepEqual(inspectSolid([[0,0,0],[1,1,1]]),{valid:false,reason:'DISCONNECTED'},
    'Cubes touching only at a vertex do not form one face-connected solid');
  assert.deepEqual(inspectSolid([[0,0,0],[1,0,0]]),{valid:true,count:2},
    'A shared face is valid connectivity');
  assert.deepEqual(inspectSolid([...slab8,slab8[0]]),{valid:false,reason:'DUPLICATE_OVERLAP'});
  assert.equal(inspectSolid(tower8.slice(0,7)).count,7); // does not satisfy target-eight task
  assert.equal(inspectSolid(slab9.slice(0,8)).count,8); // does not satisfy target-nine task
  for(const item of [[],null,9,[[0,0]],[[0,1,2.5]],[['0',0,0]]])
    assert.equal(inspectSolid(item).valid,false);
});
test('source-functional first-party visuals and teacher-only assessment spec exist', {
  skip: (!fs.existsSync(richPath) || !fs.existsSync(specPath)) && 'Requires full original GitHub branch checkout'
},()=>{
  const rich=JSON.parse(fs.readFileSync(richPath,'utf8'));
  const item=rich.Lessons.find(x=>x.LessonCode==='PED:US-CCSS-MATH:G5:U01:L01');
  const spec=JSON.parse(fs.readFileSync(specPath,'utf8'));
  assert.ok(item); assert.equal(spec.stageC,'BLOCKED_SHARED_INTEGRATION');
  assert.equal(spec.lessonCode,item.LessonCode);
  assert.ok(item.Visuals.some(v=>v.Title.includes('Source-described 2021 warm-up A–D')),
    'Verify the source-described models rather than obsolete v14 visual names');
  const lessonText = item.ExplanationParagraphs.join(' ');
  assert.match(lessonText,/original local HTML image alt descriptions have been checked/i);
  assert.match(lessonText,/underlying image binaries and figure reuse rights remain unverified/i);
  assert.match(lessonText,/teacher review rubric \(8 points/i);
  assert.ok(spec.taskContracts.every(t=>t.evaluation.includes('TEACHER') || t.evaluation.includes('HUMAN')));
  assert.ok(spec.independentFixtureExpectations.some(f=>f.name==='numeric_answer_only'));
});
