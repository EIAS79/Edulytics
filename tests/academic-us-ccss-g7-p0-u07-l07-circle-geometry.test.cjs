'use strict';
// Run: node --test tests/academic-us-ccss-g7-p0-u07-l07-circle-geometry.test.cjs
// Does not prove authenticated rich-view hook, exact Practice, persisted scoring, or A-D approval.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),dir='artifacts/curriculum-delivery/US-CCSS-MATH/G7/';
const e=JSON.parse(fs.readFileSync(path.join(root,dir+'grade7-p0-u07-l07-source-circles-rubric.json'),'utf8'));
const svg=fs.readFileSync(path.join(root,e.gradeOwnedAsset),'utf8');
const ledger=JSON.parse(fs.readFileSync(path.join(root,dir+'grade7-authoring-ledger.json'),'utf8'));
const near=(v,w,label,tol=.0005)=>assert.ok(Math.abs(v-w)<tol,label+': '+v+' not '+w);
const distance=(a,b)=>Math.hypot(a[0]-b[0],a[1]-b[1]);
function circle(id) {
 const m=svg.match(new RegExp('<circle id="'+id+'"[^>]*cx="([\\d.]+)" cy="([\\d.]+)" r="([\\d.]+)"[^>]*data-radius-inch="([\\d.]+)" data-px-per-inch="([\\d.]+)"'));
 assert.ok(m,'Missing geometric circle '+id);
 const nums=m.slice(1,6).map(Number);
 assert.ok(nums.every(Number.isFinite));
 return {center:nums.slice(0,2),r:nums[2],inch:nums[3],scale:nums[4]};
}
function poly(id) {
 const m=svg.match(new RegExp('<polygon id="'+id+'"[^>]*points="([^"]+)"[^>]*data-px-per-inch="([\\d.]+)"'));
 assert.ok(m,'Missing measurable triangle '+id);
 return {points:m[1].trim().split(/\s+/).map(s=>s.split(',').map(Number)),scale:Number(m[2])};
}
test('Accessible independent Grade 7 circle construction asset',()=>{
 assert.equal(e.lessonCode,'PED:US-CCSS-MATH:G7:U07:L07');
 assert.match(svg,/<svg[^>]*role="img"[^>]*aria-labelledby=/);
 assert.match(svg,/<title id="g7-l07-title">/);
 assert.match(svg,/<desc id="g7-l07-desc">/);
 assert.doesNotMatch(svg,/<script|<foreignObject|\bonload\s*=|\bonclick\s*=/i);
});
test('Two radius 3 circles on base 4 meet at upper and lower exact intersections',()=>{
 const a=circle('g7-l07-circle-a'),b=circle('g7-l07-circle-b');
 near(a.r/a.scale,3,'radius A');near(b.r/b.scale,3,'radius B');
 near(distance(a.center,b.center)/a.scale,4,'base AB');
 const top=poly('g7-l07-triangle-upper'),bottom=poly('g7-l07-triangle-lower');
 for(const p of [top,bottom]) {
  assert.equal(p.points.length,3);
  near(distance(p.points[0],p.points[1])/p.scale,4,'base length');
  near(distance(p.points[0],p.points[2])/p.scale,3,'AC');
  near(distance(p.points[1],p.points[2])/p.scale,3,'BC');
 }
 near(top.points[2][0],bottom.points[2][0],'mirror x');
 near((top.points[2][1]+bottom.points[2][1])/2,250,'mirror y');
});
test('Separate 21/4/2 circles are disjoint with 15 inch boundary gap',()=>{
 const a=circle('g7-l07-invalid-circle-left'),b=circle('g7-l07-invalid-circle-right');
 near(distance(a.center,b.center)/a.scale,21,'base');
 near(a.r/a.scale,4,'left radius');near(b.r/b.scale,2,'right radius');
 near((distance(a.center,b.center)-a.r-b.r)/a.scale,15,'strict gap');
 assert.ok(distance(a.center,b.center)>a.r+b.r);
});
test('Third-side boundaries are strict, not zero-area triangles',()=>{
 const x=e.mathModel.thirdSide;
 assert.deepEqual(x.fixedLengthsIn,[5,4]);
 assert.equal(x.strictMinIn,1);assert.equal(x.strictMaxIn,9);
 assert.equal(Math.abs(5-4),1);assert.equal(5+4,9);
 assert.ok([2,3,4,5,6,7,8].every(v=>v>1&&v<9));
 assert.ok([1,9].every(v=>!(v>1&&v<9)));
});
test('Teacher rubric and multiple error types do not auto-approve learners',()=>{
 assert.equal(e.studentActivityComparison.length,4);
 assert.equal(e.teacherRubricDraft.criteria.length,4);
 assert.equal(e.teacherRubricDraft.maxScore,12);
 for(const criterion of e.teacherRubricDraft.criteria)assert.deepEqual(criterion.points,[0,1,2,3]);
 for(const kind of ['correct','wrong','degenerate','nonunique','incomplete','malformed'])
  assert.ok(e.cases.some(row=>row.kind===kind),kind);
 assert.equal(e.teacherRubricDraft.studentArtifactPersisted,false);
 assert.equal(e.integration.selectedPractice,'NOT_EXECUTED');
 assert.equal(e.runtime?.studentSubmission??'NOT_IMPLEMENTED','NOT_IMPLEMENTED');
});
test('Grade 7 stage A-D remain unverified after two candidate geometry batches',()=>{
 assert.equal(ledger.counts.catalogue,145);assert.equal(ledger.counts.uniqueRich,145);
 assert.equal(ledger.counts.gradeOwnedSvgFigureCandidates,21);
 assert.equal(ledger.counts.gradeOwnedTeacherRubricDrafts,22);
 for(const k of ['stageAVerified','stageBEffectiveRuntimeVerified','stageCVerified','stageDVerified'])assert.equal(ledger.counts[k],0);
 const lesson=ledger.lessons.find(x=>x.lessonCode===e.lessonCode);
 assert.equal(lesson.p0OriginalSvg,e.gradeOwnedAsset);
 for(const stage of ['A','B','C','D'])assert.equal(e.academic[stage],'NOT_VERIFIED');
 assert.equal(e.release,'RELEASE_BLOCKED');
});
