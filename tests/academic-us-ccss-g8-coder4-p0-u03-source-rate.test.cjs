
'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('fs'),path=require('path'),crypto=require('crypto');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const sha=p=>crypto.createHash('sha256').update(fs.readFileSync(path.join(root,p))).digest('hex');
const e=read('docs/curriculum/us-ccss-g8-coder4-p0-u03-remaining-source-fixes-20261030.json');
const curr=read('docs/curriculum/us-ccss-g8-coder4-p0-current-execution-ledger-20261030.json');
const audit=read('docs/curriculum/us-ccss-g8-coder4-p0-current-source-practice-teacher-audit-20261030.json');
const roles=read('docs/curriculum/us-ccss-g8-coder4-p0-standards-role-resolution-queue-20261030.json');
const prev=read('docs/curriculum/us-ccss-g8-coder4-p0-cumulative-execution-ledger-20261030.json');
const pack=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u03-l01-l14-2017.rich-lesson-v2.json');
const src=read('docs/curriculum/us-ccss-g8-coder4-v9-activity-figure-inventory-20261029.json');
const near=(a,b,epsilon=1e-5)=>assert.ok(Math.abs(a-b)<epsilon,`${a} != ${b}`);
const getFig=(id)=>{const f=e.figures.find(x=>x.lessonCode.endsWith(':'+id));assert.ok(f);const file='src/Edulytics.Web/wwwroot'+f.assetPath;const svg=fs.readFileSync(path.join(root,file),'utf8');assert.equal(sha(file),f.sha256);return {f,svg};};
const attr=(s)=>Object.fromEntries([...s.matchAll(/([a-z0-9-]+)="([^"]+)"/g)].map(m=>[m[1],m[2]]));
const shape=(svg,kind,id)=>{const m=svg.match(new RegExp('<'+kind+' id="'+id+'" ([^>]+)>'));assert.ok(m,id);return attr(m[1]);};
test('U03 13 remaining public student-source activity spot checks add content-specific examples, preserve worked content and avoid false teacher acceptance',()=>{
 assert.equal(e.reviewedLessons.length,13);
 assert.equal(pack.Lessons.length,14);
 assert.equal(sha('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u03-l01-l14-2017.rich-lesson-v2.json'),e.richFileSha256);
 for(const rec of e.reviewedLessons){
  const l=pack.Lessons.find(x=>x.LessonCode===rec.lessonCode),s=src.rows.find(x=>x.lessonCode===rec.lessonCode);
  assert.ok(l&&s);
  assert.deepEqual(rec.sourceStudentActivities,s.activities.map(x=>x.title));
  for(const p of rec.newOriginalTeachingParagraphs)assert.ok(l.ExplanationParagraphs.includes(p),rec.lessonCode);
  assert.ok(l.WorkedExamples.length>=3);
  assert.equal(rec.sourceTeacherDetailedReview,false);
  assert.equal(rec.fullAcademicSourceParity,false);
  assert.equal(rec.teacherGradeSaved,false);
 }
});
test('U03 source-specific rates: correct comparative wages and affine cup stacks, negative math tests',()=>{
 near(12*8.4,100.8);near(12*7,84);near(12*(8.4-7),16.8);
 near(150/8.4,17.85714285714,1e-8);near(150/7,21.4285714286,1e-8);
 near((23-15)/(12-6),4/3);
 const height=n=>7+(4/3)*n;
 near(height(6),15);near(height(12),23);
 near((50-7)/(4/3),32.25);
 assert.ok(height(32)<50);
 assert.ok(height(33)>=50);
 near(height(33),51);
 assert.notEqual(height(0),0,'stack model is not proportional through origin');
});
test('U03 L04 wages original SVG matches both line slopes, plotted 12-hour totals, safe static-only content',()=>{
 const {f,svg}=getFig('L04'),o=f.model.origin;
 assert.match(svg,/<desc id="desc">/);assert.doesNotMatch(svg,/<script|<image|<foreignObject|onerror=/i);
 for(const [name,rate] of [['wage-840',8.4],['wage-700',7]]){
  const l=shape(svg,'line',name);
  near((+l.x2-(+l.x1))/f.model.xscale,20);
  const dy=(+l.y1-(+l.y2))/f.model.yscale;
  near(dy/20,rate);
 }
 for(const [name,rate] of [['wage-at-12-high',8.4],['wage-at-12-low',7]]){
  const c=shape(svg,'circle',name);
  near((+c.cx-o[0])/f.model.xscale,12);
  near((o[1]-(+c.cy))/f.model.yscale,12*rate);
 }
 assert.equal(f.authenticatedStudentRendered,false);
});
test('U03 L05 cup-height original SVG matches affine interpolation, rejects 32 cups as reaching 50',()=>{
 const {f,svg}=getFig('L05'),o=f.model.origin;
 assert.match(svg,/<desc id="desc">/);assert.doesNotMatch(svg,/<script|<foreignObject|<image/i);
 const line=shape(svg,'line','cup-height-line');
 near((+line.x1-o[0])/f.model.xscale,0);
 near((o[1]-(+line.y1))/f.model.yscale,7);
 for(const [name,n,h] of [['six',6,15],['twelve',12,23],['thirty-two',32,7+128/3],['thirty-three',33,51]]){
  const c=shape(svg,'circle','cup-'+name);
  near((+c.cx-o[0])/f.model.xscale,n);
  near((o[1]-(+c.cy))/f.model.yscale,h,.0001);
 }
 assert.equal(f.model.firstWholeCupsAtOrAboveTarget,33);
});
test('U03 independent source student numeric examples: steps, signs, fares, coordinate slopes and line domains',()=>{
 near(10/8,5/4);near(15/12,5/4);
 for(const [n,colors] of [[4,[16,12,4]],[10,[100,30,10]]]){
  assert.deepEqual([n*n,3*n,n],colors);
 }
 for(const [rides,balance] of [[0,40],[1,37.5],[2,35],[16,0]])near(40-2.5*rides,balance);
 assert.ok(40-2.5*17<0);
 near((2-11)/(8-1),-9/7);
 const cases=[
  {p:[[5,2],[11,14]],s:2},
  {p:[[1,7],[4,1]],s:-2},
  {p:[[3,5],[11,9]],s:.5},
  {p:[[-1,4],[-3,5]],s:-.5}
 ];
 for(const a of cases)near((a.p[1][1]-a.p[0][1])/(a.p[1][0]-a.p[0][0]),a.s);
 assert.ok(!Number.isFinite((5-3)/(3-3)));
 assert.equal(2.39*0+0.59*0,0);
 near(50+20*12,290);
 near(15*2,30);near(15*.5,7.5);
});
test('Current 131-row audit 44 source student page spot checks; roles pending 22; A-D unchanged',()=>{
 assert.equal(curr.rows.length,131);assert.equal(audit.rows.length,131);
 assert.deepEqual(curr.rows.map(x=>x.lessonCode),prev.rows.map(x=>x.lessonCode));
 assert.equal(curr.counts.assets,43);
 assert.equal(curr.counts.sourceFigurePriorityWithOriginalAsset,38);
 assert.equal(curr.counts.sourceFigurePriorityWithoutOriginalAsset,75);
 assert.equal(curr.counts.sourceActivitySpotReviewedLessons,44);
 assert.equal(roles.totalFlagged,22);
 assert.equal(roles.pendingResolutions,22);
 assert.equal(audit.unitSummary.U03.sourceStudentLessonPagesCheckedInP0,14);
 const modified=curr.rows.filter((x,i)=>x.verifiedCourseOwnedSvgFile!==prev.rows[i].verifiedCourseOwnedSvgFile);
 assert.deepEqual(modified.map(x=>x.lessonCode.split(':').at(-1)),['L04','L05']);
 for(const r of curr.rows){
  assert.equal(r.A,'NOT_ACCEPTED');assert.equal(r.B,'NOT_ACCEPTED');
  assert.equal(r.C,'NOT_VERIFIED');assert.equal(r.D,'NOT_VERIFIED');
  assert.equal(r.svgInAuthenticatedStudentRenderer,false);
  assert.equal(r.actualPostgresqlAttemptPersistedAndReadBack,false);
 }
});
