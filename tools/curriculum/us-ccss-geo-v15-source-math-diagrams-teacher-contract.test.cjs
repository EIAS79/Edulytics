'use strict';
const {test}=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs'),path=require('node:path');
const root=path.resolve(__dirname,'..','..');
const prefix='PED:US-CCSS-MATH:HS:TRAD:GEO:IM-1E:';
const load=rel=>JSON.parse(fs.readFileSync(path.join(root,rel),'utf8'));
const files=fs.readdirSync(path.join(root,'src/Edulytics.Core/Curriculum/LessonContent/RichV2'))
 .filter(n=>/^us-ccss-hs-geo-u0[34]-.*\.rich-lesson-v2\.json$/.test(n));
const lessons=files.flatMap(n=>load('src/Edulytics.Core/Curriculum/LessonContent/RichV2/'+n).Lessons);
const byCode=new Map(lessons.map(l=>[l.LessonCode,l]));
const get=(u,l)=>byCode.get(prefix+'U'+String(u).padStart(2,'0')+':L'+String(l).padStart(2,'0'));
const approx=(a,b,tol=0.02)=>assert.ok(Number.isFinite(a)&&Math.abs(a-b)<=tol,'independent calculation '+a+' vs '+b);
const ex=(u,l,i)=>{const e=get(u,l)?.WorkedExamples?.[i];assert.ok(e,'U'+u+':L'+l+'#'+i);return e};

test('V15 U03: 28 additional examples checked with independent calculations and stated numeric answer',()=>{
 const checked=new Set();
 function verify(l,i,actual,expected,answer,epsilon){
  approx(actual,expected,epsilon); const e=ex(3,l,i);
  assert.match(e.Answer,answer,'U03:L'+l+'#'+i);
  assert.ok(e.Question && e.Method && e.Check);
  const id='3:'+l+':'+i;assert.ok(!checked.has(id));checked.add(id);
 }
 verify(1,0,1+2*(3-1),5,/5,1/);
 verify(1,2,12*3/4,9,/9 mm/);
 verify(2,0,150000000/12700*0.02,236.22047244,/236 m/);
 verify(2,2,50800/12700*2,8,/8 cm/);
 verify(3,0,7*3/2,10.5,/10\.5/);
 verify(3,1,6*2/3,4,/4 cm/);
 verify(3,2,Math.hypot(2,4)/Math.hypot(1,2),2,/2/);
 verify(4,0,2*3,6,/y=6/);
 verify(5,1,14/2,7,/7 cm/);
 verify(5,2,15*(2/3),10,/10/);
 const mapped=[[0,0],[3,0],[0,4]].map(([x,y])=>[2*x+1,2*y+1]);
 assert.deepEqual(mapped,[[1,1],[7,1],[1,9]]);
 verify(6,0,Math.hypot(mapped[1][0]-mapped[0][0],mapped[1][1]-mapped[0][1])/3,2,/D=\(1,1\), E=\(7,1\), F=\(1,9\)/);
 verify(6,1,(10/4)*(4/10),1,/5\/2.*2\/5/);
 verify(7,1,7*15/5,21,/21/);
 verify(8,0,5/2,2.5,/2\.5/);
 verify(8,1,12/3,4,/factor 4/);
 verify(9,1,180-45-65,70,/70/);
 verify(10,0,6/4,1.5,/3\/2/);
 verify(10,1,8/4,2,/scale 2/);
 verify(11,1,20*6/15,8,/8/);
 verify(12,0,3*2.5,7.5,/7\.5 ft/);
 verify(12,1,9*3**2,81,/81/);
 verify(12,2,8*(15/6),20,/20/);
 verify(13,0,6*8/10,4.8,/4\.8/);
 verify(13,1,Math.sqrt(4*9),6,/h=6/);
 verify(15,0,Math.hypot(5,12),13,/13 cm/);
 verify(15,1,12*(15/5),36,/36/);
 verify(16,0,2+(8-2)*(3/(3+5)),4.25,/4\.25/);
 verify(16,1,1.6*(5/1),8,/8 m/);
 assert.equal(checked.size,28);
 assert.match(ex(3,6,2).Answer,/No/);
 assert.match(ex(3,16,2).Answer,/infeasible/);
 assert.ok(4.25>4,'negative wall segment boundary: reflected bounce is outside');
});

test('V15 U04 domain: zero and negative side rejected individually; model arithmetic independently checked',()=>{
 const a=ex(4,3,0);assert.ok(a.Answer.includes('The altitude is s√3 for s>0'));
 const solve=s=>{if(!(Number.isFinite(s)&&s>0))throw new RangeError('side');return Math.sqrt((2*s)**2-s*s)};
 for(const invalid of [0,-1,-7,NaN,Infinity])assert.throws(()=>solve(invalid),RangeError);
 for(const positive of [0.5,2,5,8]){
  approx(solve(positive)/positive,Math.sqrt(3),1e-12);
  approx((solve(positive)**2+positive**2)/(2*positive)**2,1,1e-12);
 }
 const run=28*12;approx(run/12,28,1e-12);approx(Math.hypot(28,run),337.1646488,0.01);
 const text=get(4,1).ExplanationParagraphs.join(' ');
 assert.match(text,/336 inches \(28 feet\)/);assert.match(text,/1:10/);
 assert.match(text,/cannot certify accessibility/);
 const unknownVisual=get(4,3).Visuals.find(x=>x.Kind==='ShapeDecomposition');
 assert.ok(unknownVisual);
 assert.equal(unknownVisual.Rows.flatMap(r=>r.Cells).filter(c=>c.includes('= ?')).length,5,
  'all five unknowns remain questions in the learner-native visual');
 assert.doesNotMatch(unknownVisual.Rows.flatMap(r=>r.Cells).join(' '),/4√3|6√3|hypotenuse 12|hypotenuse 10/i,
  'do not expose assessment answers in learner-native content');
});

test('Three V15 SVG assets have original mathematically consistent figures, accessibility, and safe locality',()=>{
 const dir='src/Edulytics.Web/wwwroot/geometry/geo/v15/';
 const names=['u04-l01-ramp-similarity-models.svg','u04-l03-equilateral-altitude-30-60-90.svg','u04-l03-five-unknown-triangle-sides.svg'];
 for(const name of names){
  const s=fs.readFileSync(path.join(root,dir+name),'utf8');
  assert.match(s,/^<svg xmlns="http:\/\/www\.w3\.org\/2000\/svg"/);
  assert.match(s,/<title id="[^"]+">.+<\/title>/);
  assert.match(s,/<desc id="[^"]+">[\s\S]+<\/desc>/);
  assert.match(s,/role="img"/);assert.match(s,/aria-labelledby="[^"]+ [^"]+"/);
  assert.ok(!/<(?:script|iframe|foreignObject|image)\b/i.test(s));
  assert.ok(!/\bon[a-z]+\s*=|\b(?:href|xlink:href)\s*=|@import|url\(/i.test(s));
  assert.ok(s.endsWith('</svg>\n'));
 }
 const alt=fs.readFileSync(path.join(root,dir+names[1]),'utf8');
 const dy=350-72.872;approx(Math.hypot(160,dy),320,0.001);
 approx(dy,160*Math.sqrt(3),0.001);assert.match(alt,/full equilateral side is 2s/i);
 const five=fs.readFileSync(path.join(root,dir+names[2]),'utf8');
 assert.ok(five.includes('5√3'), 'the third given long leg is exposed');
 assert.equal((five.match(/>\?<\/text>/g)||[]).length,5, 'all five unknown sides remain concealed in the learner figure');
 assert.doesNotMatch(five,/4√3|6√3|Five missing values/, 'figure or alternative text must not leak worked answers');
 approx(Math.hypot(100,173.205),200,0.0005);
 const ramps=fs.readFileSync(path.join(root,dir+names[0]),'utf8');
 assert.ok(ramps.includes('rise 0.4 metre and run 4.8 metres'));
 assert.ok(ramps.includes('runs 4 metres and 10 metres'));
});

test('V15 human-scored source tasks have eight-point analytic rubrics, edge cases and explicit NOT_RUN',()=>{
 const d=load('src/Edulytics.Core/Curriculum/LessonContent/Assessment/us-ccss-geo-v15-u04-human-scored-source-activity-candidates.json');
 assert.equal(d.Contracts.length,2);assert.equal(d.NotReleaseApproval,true);
 assert.equal(d.TeacherSubmissionPersistence,'NOT_IMPLEMENTED');
 for(const t of d.Contracts){
  assert.ok(byCode.has(t.LessonCode));assert.equal(t.MachineScoringAllowed,false);
  assert.equal(t.TeacherAssessmentRequired,true);assert.equal(t.Rubric.TotalMax,8);
  assert.equal(t.Rubric.Criteria.reduce((a,b)=>a+b.MaxPoints,0),8);
  assert.equal(t.Rubric.AutoGradeFromScalarProhibited,true);
  assert.equal(t.AuthenticatedRendering,false);
  assert.equal(t.AccessAndPersistence.Status,'NOT_TESTED');
  assert.ok(t.AccessAndPersistence.SchoolTenantIsolationRequired && t.AccessAndPersistence.TeacherRoleRequired);
  assert.ok(Object.values(t.Acceptance).every(v=>v===false));
  for(const kind of ['WRONG','MALFORMED','EQUIVALENT'])assert.ok(t.NegativeAndEdgeReviewCases.some(x=>x.Type===kind));
  assert.ok(t.LinkedVisualAssets.every(s=>s.startsWith('/geometry/geo/v15/')&&!s.includes('..')));
 }
 assert.equal(d.Contracts.find(c=>c.LessonCode.endsWith('U04:L03')).SourceOptional,true);
});

test('V15 manifest and source evidence preserve 124 authored and A-D NO_GO',()=>{
 const d=load('artifacts/curriculum-delivery/US-CCSS-MATH/GEO/geo-v15-source-batches-and-shared-handoff-20261030.json');
 const m=load('artifacts/curriculum-delivery/US-CCSS-MATH/GEO/geo-124-source-manifest-20261028.json');
 assert.equal(m.lessons.length,124);
 assert.equal(d.newIndependentNumericSlots,28);assert.equal(d.priorIndependentNumericSlots,55);
 assert.equal(d.totalIndependentNumericSlots,83);assert.equal(d.remainingNotIndependentlyNumericChecked,289);
 assert.equal(d.sourceTaskRowsReviewed.length,3);assert.equal(d.originalSvgAssetsAdded.length,3);
 const u03=m.lessons.filter(x=>x.lessonCode.includes(':U03:'));
 assert.equal(u03.length,16);
 assert.equal(u03.reduce((sum,x)=>sum+x.v15NumericOracleEvidence.count,0),28);
 assert.ok(u03.every(x=>x.v15NumericOracleEvidence.academicAcceptance===false));
 assert.equal(u03.find(x=>x.lessonCode.endsWith('U03:L14')).v15NumericOracleEvidence.scope,'QUALITATIVE_PROOF_REQUIRES_NONNUMERIC_REVIEW');
 for(const row of d.sourceTaskRowsReviewed){
  const l=m.lessons.find(x=>x.lessonCode===row.lessonCode);assert.ok(l&&l.v15IndependentReaudit);
  for(const stage of ['A_ACCEPTED','B_VERIFIED','C_VERIFIED','D_VERIFIED'])assert.equal(l.v15IndependentReaudit[stage],false);
  assert.equal(row.authenticatedStudentRender,'NOT_RUN');
  assert.equal(row.persistedTeacherGrade,'NOT_RUN');
 }
 assert.equal(d.releaseDecision,'DRAFT_NO_GO');
});
