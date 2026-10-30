'use strict';
// Execute with: node --test tests/academic-us-ccss-g7-p0-u07-l06-geometry.test.cjs
// Verifies grade-only source geometry and candidate safety; NOT authenticated student UI.
const t=require('node:test'),a=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),base='artifacts/curriculum-delivery/US-CCSS-MATH/G7/';
const e=JSON.parse(fs.readFileSync(path.join(root,base+'grade7-p0-u07-l06-source-geometry-rubric.json'),'utf8'));
const svg=fs.readFileSync(path.join(root,e.asset),'utf8');
const rich=JSON.parse(fs.readFileSync(path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g7-u07-l02-l17-our-2017.rich-lesson-v2.json'),'utf8'));
const ledger=JSON.parse(fs.readFileSync(path.join(root,base+'grade7-authoring-ledger.json'),'utf8'));
const near=(v,w,label,tol=.0006)=>a.ok(Math.abs(v-w)<tol,label+' '+v+' vs '+w);
const dist=(p,q)=>Math.hypot(p[0]-q[0],p[1]-q[1]);
function polygon(id) {
  const found=svg.match(new RegExp('<polygon id="'+id+'"[^>]*data-scale="([\\d.]+)"[^>]*points="([^"]+)"'));
  a.ok(found,'Missing source polygon '+id);
  const points=found[2].trim().split(/\s+/).map(pair=>pair.split(',').map(Number));
  a.ok(points.every(pair=>pair.length===2&&pair.every(Number.isFinite)));
  return {scale:Number(found[1]),points};
}
test('SVG is original accessible passive teaching image, not an approved student response',()=>{
 a.match(svg,/<svg[^>]*role="img"[^>]*aria-labelledby=/);
 a.match(svg,/<title id="g7-l06-title">/);a.match(svg,/<desc id="g7-l06-desc">/);
 a.doesNotMatch(svg,/<script|<foreignObject|\bonload\s*=|\bonclick\s*=/i);
 a.equal(e.lessonCode,'PED:US-CCSS-MATH:G7:U07:L06');
});
test('Both quadrilaterals have exactly 9,4,5,6 inches in order but nonmatching AC',()=>{
 const q1=polygon('g7-l06-quad-seven'),q2=polygon('g7-l06-quad-nine');
 for(const q of [q1,q2]){
  a.equal(q.points.length,4);
  const sides=q.points.map((point,i)=>dist(point,q.points[(i+1)%4])/q.scale);
  sides.forEach((v,i)=>near(v,[9,4,5,6][i],'quadrilateral side '+i));
 }
 near(dist(q1.points[0],q1.points[2])/q1.scale,7,'first diagonal');
 near(dist(q2.points[0],q2.points[2])/q2.scale,9,'second diagonal');
 a.notEqual(e.mathModel.quad1.diagonalAC,e.mathModel.quad2.diagonalAC);
});
test('Original 4-5-8 inch triangle actually closes; 3-4-9 cannot',()=>{
 const p=polygon('g7-l06-triangle');
 a.equal(p.points.length,3);
 const lengths=p.points.map((point,i)=>dist(point,p.points[(i+1)%3])/p.scale);
 lengths.forEach((v,i)=>near(v,[8,5,4][i],'triangle side '+i));
 a.equal(3+4<9,true);
 a.equal(e.mathModel.failedClosure.longest-e.mathModel.failedClosure.shorterTotal,2);
});
test('Source pedagogy repair preserves all other Grade 7 Rich candidates',()=>{
 const l=rich.Lessons.find(row=>row.LessonCode===e.lessonCode);
 a.ok(l);a.equal(l.WorkedExamples.length,4);
 a.match(l.ExplanationParagraphs.join(' '),/Defer the general triangle-inequality rule/);
 a.match(l.WorkedExamples[0].Method,/Physically join/);
 a.match(l.WorkedExamples[3].Check,/Teacher reviews actual hinged models/);
 a.ok(rich.Lessons.some(row=>row.LessonCode.endsWith('G7:U07:L07')));
});
test('Source tasks and teacher-only rubric cover incomplete, wrong, degenerate and malformed',()=>{
 a.equal(e.sourceComparison.length,4);
 a.ok(e.sourceComparison.some(row=>row.activity.startsWith('6.2')));
 a.ok(e.sources.standardsAddressing.includes('7.G.A.2'));
 a.equal(e.teacherOnlyRubric.criteria.length,4);a.equal(e.teacherOnlyRubric.maxScore,12);
 for(const row of e.teacherOnlyRubric.criteria)a.deepEqual(row.points,[0,1,2,3]);
 for(const kind of ['correct','wrong','degenerate','incomplete','malformed'])a.ok(e.cases.some(row=>row.kind===kind),kind);
 a.equal(e.teacherOnlyRubric.studentWorkPersisted,false);
});
test('Full-grade acceptance remains fail-closed with only candidate increments',()=>{
 a.equal(ledger.counts.catalogue,145);a.equal(ledger.counts.uniqueRich,145);
 a.equal(ledger.counts.gradeOwnedSvgFigureCandidates,20);
 a.equal(ledger.counts.gradeOwnedTeacherRubricDrafts,21);
 for(const k of ['stageAVerified','stageBEffectiveRuntimeVerified','stageCVerified','stageDVerified'])a.equal(ledger.counts[k],0);
 for(const k of ['A','B','C','D'])a.equal(e.academic[k],'NOT_VERIFIED');
 const lesson=ledger.lessons.find(row=>row.lessonCode===e.lessonCode);
 a.equal(lesson.p0OriginalSvg,e.asset);
 a.equal(e.runtime.selectedPracticeFamily,'NOT_EXECUTED');
 a.equal(e.runtime.teacherScoringPersistence,'NOT_RUN');
 a.equal(e.release,'RELEASE_BLOCKED');
});
