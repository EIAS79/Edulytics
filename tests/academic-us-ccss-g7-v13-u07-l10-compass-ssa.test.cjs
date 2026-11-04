'use strict';
// To execute in full authorized checkout:
// node --test tests/academic-us-ccss-g7-v13-u07-l10-compass-ssa.test.cjs
// Static fixture cannot certify student UI, exact Practice or stored teacher rubrics.
const t=require('node:test'),a=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..'),dir='artifacts/curriculum-delivery/US-CCSS-MATH/G7/';
const e=JSON.parse(fs.readFileSync(path.join(root,dir+'grade7-v13-u07-l10-source-compass-rubric.json'),'utf8'));
const svg=fs.readFileSync(path.join(root,e.asset),'utf8');
const rich=JSON.parse(fs.readFileSync(path.join(root,e.richFile),'utf8'));
const ledger=JSON.parse(fs.readFileSync(path.join(root,dir+'grade7-authoring-ledger.json'),'utf8'));
const distance=(p,q)=>Math.hypot(p[0]-q[0],p[1]-q[1]);
const near=(actual,want,label,tol=.0002)=>a.ok(Math.abs(actual-want)<tol,label+': '+actual+' differs from '+want);
function polygon(id) {
 const match=svg.match(new RegExp('<polygon id="'+id+'"[^>]*points="([^"]+)"[^>]*data-scale="([\\d.]+)"'));
 a.ok(match,'missing SVG polygon '+id);
 return {p:match[1].trim().split(/\s+/).map(pair=>pair.split(',').map(Number)),s:+match[2]};
}
function deg(A,B,C) {
 const u=[A[0]-B[0],A[1]-B[1]],v=[C[0]-B[0],C[1]-B[1]];
 return Math.acos(Math.max(-1,Math.min(1,(u[0]*v[0]+u[1]*v[1])/(Math.hypot(...u)*Math.hypot(...v)))))*180/Math.PI;
}
t('Original accessible Grade 7 L10 svg has no active handlers',()=>{
 a.equal(e.lessonCode,'PED:US-CCSS-MATH:G7:U07:L10');
 a.match(svg,/<svg[^>]*role="img"[^>]*aria-labelledby=/);
 a.match(svg,/<title id="g7-l10-title">/);
 a.match(svg,/<desc id="g7-l10-desc">/);
 a.doesNotMatch(svg,/<script|<foreignObject|\bonload\s*=|\bonclick\s*=/i);
});
t('40 degrees between two five-centimeter sides gives a shorter third side',()=>{
 const {p,s}=polygon('g7-l10-included-five');const [A,B,C]=p;
 near(distance(A,B)/s,5,'AB 5 cm');
 near(distance(A,C)/s,5,'AC 5 cm');
 near(deg(B,A,C),40,'included angle',.0003);
 near(distance(B,C)/s,2*5*Math.sin(20*Math.PI/180),'third side',.00002);
 a.ok(distance(B,C)/s<5);
});
t('Exactly two positive ray-circle intersections for nonincluded 45/8/6 geometry',()=>{
 const one=polygon('g7-l10-ssa-near'),two=polygon('g7-l10-ssa-far');
 for(const {p,s} of [one,two]){
  const [A,B,C]=p;
  near(distance(A,B)/s,8,'AB 8');
  near(distance(B,C)/s,6,'BC 6',.000005);
  near(deg(B,A,C),45,'angle A',.00001);
 }
 const u=distance(one.p[0],one.p[2])/one.s,v=distance(two.p[0],two.p[2])/two.s;
 near(u,4*Math.SQRT2-2,'near AC',.000005);
 near(v,4*Math.SQRT2+2,'far AC',.000005);
 a.ok(v-u>3.9);
});
t('Angle-only triangle constructions have measured 50 60 70 and size factor 2',()=>{
 const small=polygon('g7-l10-aaa-small'),large=polygon('g7-l10-aaa-large');
 for(const {p} of [small,large]){
  const [A,B,C]=p;
  near(deg(B,A,C),50,'angle A',.0002);
  near(deg(A,B,C),60,'angle B',.0002);
  near(deg(A,C,B),70,'angle C',.0002);
 }
 near(distance(large.p[0],large.p[1])/distance(small.p[0],small.p[1]),2,'scale');
 a.equal([50,60,100].reduce((a,b)=>a+b,0),210);
});
t('Rich lesson 10 is task-aligned and leaves all other unit candidate lessons',()=>{
 const target=rich.Lessons.find(x=>x.LessonCode===e.lessonCode);
 a.ok(target);a.equal(rich.Lessons.length,16);a.equal(target.WorkedExamples.length,4);
 a.match(target.ExplanationParagraphs.join(' '),/V13 L10 source-specific repair/);
 a.match(target.WorkedExamples[3].Question,/45° angle at A/);
 a.match(target.SummaryPoints.join(' '),/compass/);
});
t('Teacher rubric protects correct wrong malformed ambiguous degenerate and incomplete cases',()=>{
 a.equal(e.activityComparison.length,4);
 a.equal(e.teacherRubric.criteria.length,4);
 a.equal(e.teacherRubric.maxScore,12);
 for(const c of e.teacherRubric.criteria)a.deepEqual(c.points,[0,1,2,3]);
 for(const kind of ['correct','wrong','malformed','ambiguous','degenerate','incomplete'])
  a.ok(e.cases.some(x=>x.type===kind),'missing '+kind);
 a.equal(e.teacherRubric.studentSubmissionPersisted,false);
 a.equal(e.teacherRubric.teacherScoringPersisted,false);
});
t('Cumulative assets may grow while course Stage A-D stay blocked',()=>{
 const lesson=ledger.lessons.find(x=>x.lessonCode===e.lessonCode);
 a.ok(lesson);
 a.equal(lesson.v13GradeOwnedSvg,e.asset);
 a.equal(lesson.v13RubricEvidence,dir+'grade7-v13-u07-l10-source-compass-rubric.json');
 a.equal(lesson.v13NodeTest,'tests/academic-us-ccss-g7-v13-u07-l10-compass-ssa.test.cjs');
 a.equal(ledger.counts.catalogue,145);
 a.equal(ledger.counts.uniqueRich,145);
 a.equal(ledger.counts.staticRichWorkedExamples,584);
 a.ok(ledger.counts.gradeOwnedSvgFigureCandidates > ledger.v12.originalSvgCandidates);
 for(const k of ['stageAVerified','stageBEffectiveRuntimeVerified','stageCVerified','stageDVerified'])a.equal(ledger.counts[k],0);
 for(const k of ['A','B','C','D'])a.equal(e.academic[k],'NOT_VERIFIED');
 a.equal(e.release,'RELEASE_BLOCKED');a.equal(e.integration.exactPractice,'NOT_EXECUTED');
});
