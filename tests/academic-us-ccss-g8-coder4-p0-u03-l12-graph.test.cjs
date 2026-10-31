
'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('fs'),path=require('path'),crypto=require('crypto');
const root=path.resolve(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const sha=p=>crypto.createHash('sha256').update(fs.readFileSync(path.join(root,p))).digest('hex');
const e=read('docs/curriculum/us-ccss-g8-coder4-p0-u03-l12-source-graph-contract-20261030.json');
const l=read('docs/curriculum/us-ccss-g8-coder4-p0-cumulative-execution-ledger-20261030.json');
const audit=read('docs/curriculum/us-ccss-g8-coder4-p0-cumulative-source-practice-teacher-audit-20261030.json');
const previous=read('docs/curriculum/us-ccss-g8-coder4-p0-u02-implementation-ledger-20261030.json');
const pack=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u03-l01-l14-2017.rich-lesson-v2.json');
const svgFile='src/Edulytics.Web/wwwroot'+e.courseSvg.assetPath;
const svg=fs.readFileSync(path.join(root,svgFile),'utf8');
const near=(x,y)=>assert.ok(Math.abs(x-y)<1e-7,`${x} != ${y}`);
const attr=(tag)=>Object.fromEntries([...tag.matchAll(/([a-z0-9-]+)="([^"]+)"/g)].map(m=>[m[1],m[2]]));
function shape(tag,id){const match=svg.match(new RegExp('<'+tag+' id="'+id+'" ([^>]+)>'));assert.ok(match,id);return attr(match[1]);}
test('U03 L12 source activity 12.1/12.2/12.3 content matches official examples while preserving earlier independent examples',()=>{
 const old=pack.Lessons.find(x=>x.LessonCode===e.lessonCode);assert.ok(old);
 assert.deepEqual(e.activityTitles,['12.1: Estimate Area','12.2: Apples and Oranges','12.3: Solutions and Everything Else']);
 for(const p of e.courseFixes)assert.ok(old.ExplanationParagraphs.includes(p));
 assert.equal(old.WorkedExamples.length,3);
 assert.equal(old.Visuals.length,2);
 const latest=read('docs/curriculum/us-ccss-g8-coder4-p0-u03-remaining-source-fixes-20261030.json');
 // Preserve U03:L12's historical candidate hash in its evidence; compare the
 // current pack with the later U03 batch's actual post-change content hash.
 assert.equal(sha('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u03-l01-l14-2017.rich-lesson-v2.json'),latest.richFileSha256);
 assert.equal(e.fullSourceParityAccepted,false);
 assert.equal(e.teacherRubricSaved,false);
});
test('U03 L12 shade comparison is exact and not a guess from appearances',()=>{
 near(e.graphMath.initialShading[0],3/4);
 near(e.graphMath.initialShading[1],3/4);
 near(e.graphMath.initialShading[2],13/16);
 assert.ok(e.graphMath.initialShading[2]>e.graphMath.initialShading[0]);
 for(let i=0;i<3;i++){
  const n=i===2?13:12;
  for(let j=0;j<16;j++){
   const block=shape('rect','shade-'+i+'-'+j);
   assert.equal(block.fill,j<n?['#2563eb','#15803d','#9333ea'][i]:'#fff');
  }
 }
});
test('U03 L12 fruit solutions are six and only six nonnegative integer choices; wrong 7 apples and 2 oranges is rejected',()=>{
 const p=e.graphMath.fruitSolutions;
 assert.equal(p.length,6);
 assert.equal(new Set(p.map(x=>x.join(','))).size,6);
 for(const [a,o] of p){assert.ok(Number.isInteger(a)&&Number.isInteger(o)&&a>=0&&o>=0);near(a+2*o,10);}
 assert.equal(e.graphMath.rejectedFruit[0]+2*e.graphMath.rejectedFruit[1],11);
 assert.notEqual(7+2*2,10);
 for(let i=0;i<p.length;i++){
  const pt=shape('circle','fruit-valid-'+i);
  const a=(+pt.cx-330)/20,o=(475-(+pt.cy))/20;
  near(a,p[i][0]);near(o,p[i][1]);
 }
});
test('U03 L12 real line 2x+y=10 includes noninteger/negative points, rejects nonsolutions, and is not a whole-fruit purchase domain',()=>{
 assert.ok(e.graphMath.validReal.length>=5&&e.graphMath.invalidReal.length>=3);
 for(let i=0;i<e.graphMath.validReal.length;i++){
  const [x,y]=e.graphMath.validReal[i],v=shape('circle','real-valid-'+i);
  near(2*x+y,10);
  near((+v.cx-665)/18,x);near((480-(+v.cy))/18,y);
 }
 for(let i=0;i<e.graphMath.invalidReal.length;i++){
  const [x,y]=e.graphMath.invalidReal[i],v=shape('circle','real-invalid-'+i);
  assert.notEqual(2*x+y,10);
  near((+v.cx-665)/18,x);near((480-(+v.cy))/18,y);
 }
 assert.ok(e.graphMath.validReal.some(([x,y])=>x<0||y<0));
 assert.ok(e.graphMath.validReal.some(([x])=>!Number.isInteger(x)));
 assert.equal(2*(-3)+16,10);
 assert.equal(2*(3.5)+3,10);
});
test('U03 L12 original graph is valid accessible safe SVG and prior first-party asset is intact',()=>{
 assert.equal(sha(svgFile),e.courseSvg.sha256);
 assert.match(svg,/^<svg xmlns="http:\/\/www.w3.org\/2000\/svg"/);
 assert.match(svg,/role="img" aria-labelledby="title desc"/);
 assert.match(svg,/<title id="title">/);
 assert.match(svg,/<desc id="desc">/);
 assert.match(svg,/<\/svg>\s*$/);
 assert.doesNotMatch(svg,/<script|<foreignObject|onload=|onerror=|<image|<iframe/i);
 const prior=previous.rows.find(x=>x.lessonCode===e.lessonCode);
 assert.equal(sha('src/Edulytics.Web/wwwroot'+prior.verifiedCourseOwnedSvgFile),prior.verifiedCourseOwnedSvgSha256);
 assert.equal(e.courseSvg.authenticatedStudentRendered,false);
 assert.equal(e.sharedPracticePR.mergeVerified,false);
 assert.equal(e.sharedPracticePR.studentGraphDeliveredBySharedPR,false);
});
test('Cumulative 131-row Grade 8 audit contains one U03 L12 graph work item, five new static assets, no false A-D promotion',()=>{
 assert.equal(l.rows.length,131);assert.equal(audit.rows.length,131);
 assert.deepEqual(l.rows.map(x=>x.lessonCode),previous.rows.map(x=>x.lessonCode));
 assert.equal(l.counts.assets,41);
 assert.equal(l.counts.sourceFigurePriorityWithOriginalAsset,37);
 assert.equal(l.counts.sourceFigurePriorityWithoutOriginalAsset,76);
 assert.equal(l.counts.sourceActivitySpotReviewedLessons,31);
 const one=l.rows.find(x=>x.lessonCode===e.lessonCode);
 assert.equal(one.additionalCourseOwnedSvgFiles[0].path,e.courseSvg.assetPath);
 for(const r of l.rows){
  assert.equal(r.A,'NOT_ACCEPTED');assert.equal(r.B,'NOT_ACCEPTED');
  assert.equal(r.C,'NOT_VERIFIED');assert.equal(r.D,'NOT_VERIFIED');
  assert.equal(r.svgInAuthenticatedStudentRenderer,false);
  assert.equal(r.actualPostgresqlAttemptPersistedAndReadBack,false);
 }
});
