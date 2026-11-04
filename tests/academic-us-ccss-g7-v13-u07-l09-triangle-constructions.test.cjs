'use strict';
// Run with: node --test tests/academic-us-ccss-g7-v13-u07-l09-triangle-constructions.test.cjs
// Course-only source/coordinate fixture, not an authenticated learner/teacher runtime test.
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),dir='artifacts/curriculum-delivery/US-CCSS-MATH/G7/';
const e=JSON.parse(fs.readFileSync(path.join(root,dir+'grade7-v13-u07-l09-source-construction-rubric.json'),'utf8'));
const svg=fs.readFileSync(path.join(root,e.firstPartySvg),'utf8');
const rich=JSON.parse(fs.readFileSync(path.join(root,e.richPack),'utf8'));
const ledger=JSON.parse(fs.readFileSync(path.join(root,dir+'grade7-authoring-ledger.json'),'utf8'));
const near=(x,y,label,tol=.0001)=>assert.ok(Math.abs(x-y)<=tol,label+': got '+x+', expected '+y);
const length=(x,y)=>Math.hypot(x[0]-y[0],x[1]-y[1]);
function polygon(id){
 const found=svg.match(new RegExp('<polygon id="'+id+'"[^>]*points="([^"]+)"[^>]*data-scale="([\\d.]+)"'));
 assert.ok(found,'Missing construction '+id);
 return {points:found[1].trim().split(/\s+/).map(p=>p.split(',').map(Number)),scale:Number(found[2])};
}
function angle(A,B,C){
 const v=[A[0]-B[0],A[1]-B[1]],w=[C[0]-B[0],C[1]-B[1]];
 const dot=v[0]*w[0]+v[1]*w[1],norm=Math.hypot(...v)*Math.hypot(...w);
 return Math.acos(Math.max(-1,Math.min(1,dot/norm)))*180/Math.PI;
}
test('Source reference SVG is passive, accessible and lesson-scoped',()=>{
 assert.equal(e.lessonCode,'PED:US-CCSS-MATH:G7:U07:L09');
 assert.match(svg,/<svg[^>]*role="img"[^>]*aria-labelledby=/);
 assert.match(svg,/<title id="g7l09title">/);
 assert.match(svg,/<desc id="g7l09desc">/);
 assert.doesNotMatch(svg,/<script|<foreignObject|\bonload\s*=|\bonclick\s*=/i);
 assert.equal(e.publicAsset,'/curriculum/us-ccss-g7/geometry/'+path.basename(e.firstPartySvg));
});
test('60/60 and 4cm construct a single congruent equilateral triangle shape',()=>{
 const t=polygon('g7-l09-equilateral');assert.equal(t.points.length,3);
 for(let i=0;i<3;i++)near(length(t.points[i],t.points[(i+1)%3])/t.scale,4,'equilateral side '+i);
 near(angle(t.points[1],t.points[0],t.points[2]),60,'equilateral angle A',.0003);
 assert.equal(e.mathModel.one.countNoncongruent,1);
});
test('Two right angles prevent a nondegenerate triangle',()=>{
 const impossible=e.mathModel.none;
 assert.deepEqual(impossible.givenAnglesDeg,[90,90]);
 assert.equal(impossible.countNoncongruent,0);
 assert.equal(impossible.verticalRayOrigins[0][0],impossible.base[0][0]);
 assert.equal(impossible.verticalRayOrigins[1][0],impossible.base[1][0]);
 assert.notEqual(impossible.base[0][0],impossible.base[1][0]);
 assert.equal(impossible.base[0][1],impossible.base[1][1]);
});
test('Both right triangles have 60/90/30 degrees and 4cm on different sides',()=>{
 const first=polygon('g7-l09-triangle-ab-four'),second=polygon('g7-l09-triangle-bc-four');
 const a=first.points,b=second.points;
 for(const t of [first,second]){
  const [A,B,C]=t.points;
  near(angle(B,A,C),60,'angle A',.0005);
  near(angle(A,B,C),90,'angle B',.0005);
  near(angle(A,C,B),30,'angle C',.0005);
 }
 near(length(a[0],a[1])/first.scale,4,'AB length first');
 near(length(b[1],b[2])/second.scale,4,'BC length second');
 const sideAB2=length(b[0],b[1])/second.scale;
 near(sideAB2,4/Math.sqrt(3),'AB length second',.00001);
 assert.ok(Math.abs(sideAB2-4)>1,'must be noncongruent');
 assert.equal(e.mathModel.multiple.countNoncongruent,'MORE_THAN_ONE');
});
test('Rich V2 target was repaired for real source 9.1-9.3 conditions, other lessons intact',()=>{
 assert.equal(rich.Lessons.length,16);
 const lesson=rich.Lessons.find(x=>x.LessonCode===e.lessonCode);
 assert.ok(lesson);
 assert.equal(lesson.WorkedExamples.length,4);
 assert.match(lesson.ExplanationParagraphs.join(' '),/V13 source-specific construction repair/);
 assert.match(lesson.WorkedExamples[3].Question,/two 90° angles/);
 assert.match(lesson.SummaryPoints.join(' '),/unspecified/);
 assert.ok(lesson.Visuals[0].Rows.some(x=>x.Cells.includes('Zero')));
});
test('Teacher-only rubric has correct wrong impossible degenerate ambiguous malformed cases',()=>{
 assert.equal(e.sourceActivityComparison.length,4);
 assert.equal(e.teacherOnlyRubric.criteria.length,4);
 assert.equal(e.teacherOnlyRubric.maxScore,12);
 for(const row of e.teacherOnlyRubric.criteria)assert.deepEqual(row.points,[0,1,2,3]);
 for(const kind of ['correct','wrong','ambiguous','degenerate','incomplete','malformed'])
  assert.ok(e.validationCases.some(x=>x.kind===kind),'Missing '+kind);
 assert.equal(e.teacherOnlyRubric.submittedStudentArtifacts,false);
 assert.equal(e.teacherOnlyRubric.scoredByTeacher,false);
});
test('Per-lesson evidence and academic release gates remain fail closed',()=>{
 const target=ledger.lessons.find(x=>x.lessonCode===e.lessonCode);
 assert.ok(target);
 assert.equal(target.v13GradeOwnedSvg,e.firstPartySvg);
 assert.equal(target.v13RubricEvidence,dir+'grade7-v13-u07-l09-source-construction-rubric.json');
 assert.equal(target.v13NodeTest,'tests/academic-us-ccss-g7-v13-u07-l09-triangle-constructions.test.cjs');
 assert.equal(ledger.counts.catalogue,145);
 assert.equal(ledger.counts.uniqueRich,145);
 assert.equal(ledger.counts.staticRichWorkedExamples,584);
 assert.equal(ledger.counts.richVisualModels,151);
 assert.ok(ledger.counts.gradeOwnedSvgFigureCandidates > ledger.v12.originalSvgCandidates);
 for(const k of ['stageAVerified','stageBEffectiveRuntimeVerified','stageCVerified','stageDVerified'])
  assert.equal(ledger.counts[k],0);
 for(const k of ['A','B','C','D'])assert.equal(e.academic[k],'NOT_VERIFIED');
 assert.equal(e.integration.exactSelectedPractice,'NOT_EXECUTED');
 assert.equal(e.release,'RELEASE_BLOCKED');
});
