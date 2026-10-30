'use strict';
// Execute: node --test tests/academic-us-ccss-g7-p0-u07-l08-side-placement.test.cjs
// Static geometry only, not authenticated UI or academic acceptance.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),directory='artifacts/curriculum-delivery/US-CCSS-MATH/G7/';
const e=JSON.parse(fs.readFileSync(path.join(root,directory+'grade7-p0-u07-l08-side-placement-rubric.json'),'utf8'));
const svg=fs.readFileSync(path.join(root,e.asset),'utf8');
const rich=JSON.parse(fs.readFileSync(path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g7-u07-l02-l17-our-2017.rich-lesson-v2.json'),'utf8'));
const ledger=JSON.parse(fs.readFileSync(path.join(root,directory+'grade7-authoring-ledger.json'),'utf8'));
const close=(x,y,name,tol=.001)=>assert.ok(Math.abs(x-y)<tol,name+': '+x+' != '+y);
const len=(a,b)=>Math.hypot(a[0]-b[0],a[1]-b[1]);
function triangle(id) {
 const m=svg.match(new RegExp('<polygon id="'+id+'"[^>]*data-px-per-unit="([\\d.]+)" points="([^"]+)"'));
 assert.ok(m,'missing triangle '+id);
 return {scale:+m[1],v:m[2].trim().split(/\s+/).map(s=>s.split(',').map(Number))};
}
function deg(a,b,c) {
 const u=[a[0]-b[0],a[1]-b[1]],v=[c[0]-b[0],c[1]-b[1]];
 return Math.acos(Math.max(-1,Math.min(1,(u[0]*v[0]+u[1]*v[1])/(Math.hypot(...u)*Math.hypot(...v)))))*180/Math.PI;
}
test('Accessible independent figure contains no active embedded content',()=>{
 assert.equal(e.lessonCode,'PED:US-CCSS-MATH:G7:U07:L08');
 assert.match(svg,/<svg[^>]*role="img"[^>]*aria-labelledby=/);
 assert.match(svg,/<title id="g7-l08-title">/);
 assert.match(svg,/<desc id="g7-l08-desc">/);
 assert.doesNotMatch(svg,/<script|<foreignObject|\bonload\s*=|\bonclick\s*=/i);
});
test('Both triangle constructions really have angles 40, 60 and 80',()=>{
 for(const id of ['g7-l08-triangle-ab-five','g7-l08-triangle-ac-five']){
  const t=triangle(id);assert.equal(t.v.length,3);
  const [A,B,C]=t.v;
  close(deg(B,A,C),40,'angle A',.002);
  close(deg(A,B,C),60,'angle B',.002);
  close(deg(A,C,B),80,'angle C',.002);
 }
});
test('First triangle uses AB=5, second uses AC=5, with unequal corresponding AB',()=>{
 const a=triangle('g7-l08-triangle-ab-five'),b=triangle('g7-l08-triangle-ac-five');
 close(len(a.v[0],a.v[1])/a.scale,5,'first AB');
 close(len(b.v[0],b.v[2])/b.scale,5,'second AC');
 const secondAB=len(b.v[0],b.v[1])/b.scale;
 close(secondAB,5.6858,'second AB approximate',.0002);
 assert.ok(Math.abs(secondAB-5)>.6,'must be noncongruent');
 assert.equal(e.model.congruent,false);
});
test('Grade lesson does not teach formal congruence acronyms as memorization objective',()=>{
 const l=rich.Lessons.find(row=>row.LessonCode===e.lessonCode);
 assert.ok(l);assert.equal(l.WorkedExamples.length,4);
 assert.match(l.ExplanationParagraphs.join(' '),/memorizing SSS, SAS, ASA or AAA names is not a required/);
 assert.match(l.SummaryPoints.join(' '),/corresponding/);
 assert.doesNotMatch(l.KeyConcepts.map(x=>x.Rule).join(' '),/\bUse SSS congruence\b/);
});
test('Source comparison, teacher rubric and wrong/incomplete/malformed cases',()=>{
 assert.equal(e.sourceTasks.length,4);
 assert.equal(e.teacherRubric.criteria.length,4);
 assert.equal(e.teacherRubric.maxScore,12);
 for(const row of e.teacherRubric.criteria)assert.deepEqual(row.points,[0,1,2,3]);
 for(const kind of ['correct','wrong','degenerate','incomplete','malformed'])
  assert.ok(e.cases.some(row=>row.kind===kind),kind);
 assert.equal(e.teacherRubric.submittedStudentArtifact,false);
});
test('All Stage A-D grades stay 0 without student UI, Practice or human assessment evidence',()=>{
 assert.equal(ledger.counts.catalogue,145);assert.equal(ledger.counts.uniqueRich,145);
 assert.equal(ledger.counts.gradeOwnedSvgFigureCandidates,22);
 assert.equal(ledger.counts.gradeOwnedTeacherRubricDrafts,23);
 for(const k of ['stageAVerified','stageBEffectiveRuntimeVerified','stageCVerified','stageDVerified'])assert.equal(ledger.counts[k],0);
 for(const stage of ['A','B','C','D'])assert.equal(e.academic[stage],'NOT_VERIFIED');
 const l=ledger.lessons.find(x=>x.lessonCode===e.lessonCode);
 assert.equal(l.p0OriginalSvg,e.asset);
 assert.equal(e.integration.selectedPractice,'NOT_EXECUTED');
 assert.equal(e.release,'RELEASE_BLOCKED');
});
