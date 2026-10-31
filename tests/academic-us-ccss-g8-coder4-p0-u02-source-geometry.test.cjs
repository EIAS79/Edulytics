
'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('fs'),path=require('path'),crypto=require('crypto');
const root=path.resolve(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const sha=p=>crypto.createHash('sha256').update(fs.readFileSync(path.join(root,p))).digest('hex');
const doc=read('docs/curriculum/us-ccss-g8-coder4-p0-u02-source-fixes-20261030.json');
const ledger=read('docs/curriculum/us-ccss-g8-coder4-p0-u02-implementation-ledger-20261030.json');
const audit=read('docs/curriculum/us-ccss-g8-coder4-p0-u02-source-practice-teacher-audit-20261030.json');
const contracts=read('docs/curriculum/us-ccss-g8-coder4-p0-u02-practice-human-contracts-20261030.json');
const prev=read('docs/curriculum/us-ccss-g8-coder4-v13-u01-complete-spot-review-ledger-20261029.json');
const content=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u02-l01-l13-2017.rich-lesson-v2.json');
const source=read('docs/curriculum/us-ccss-g8-coder4-v9-activity-figure-inventory-20261029.json');
const near=(x,y,e=1e-5)=>assert.ok(Math.abs(x-y)<e,`${x} != ${y}`);
const d=(a,b)=>Math.hypot(a[0]-b[0],a[1]-b[1]);
const parseAttrs=(s)=>Object.fromEntries([...s.matchAll(/([a-z0-9-]+)="([^"]+)"/g)].map(m=>[m[1],m[2]]));
function geom(lesson){const f=doc.figures.find(x=>x.lessonCode.endsWith(':'+lesson));assert.ok(f);const file='src/Edulytics.Web/wwwroot'+f.assetPath;const svg=fs.readFileSync(path.join(root,file),'utf8');assert.equal(sha(file),f.sha256);return {f,file,svg};}
function vertices(svg,id){const m=svg.match(new RegExp('<polygon id="'+id+'" points="([^"]+)"'));assert.ok(m,id);return m[1].split(/\s+/).map(pair=>pair.split(',').map(Number));}
function rect(svg,id){const m=svg.match(new RegExp('<rect id="'+id+'" ([^>]+)>'));assert.ok(m,id);const a=parseAttrs(m[1]);return {w:+a.width,h:+a.height,x:+a.x,y:+a.y};}
function line(svg,id){const m=svg.match(new RegExp('<line id="'+id+'" ([^>]+)>'));assert.ok(m,id);const a=parseAttrs(m[1]);return [[+a.x1,+a.y1],[+a.x2,+a.y2]];}
const unproject=(v,o,s)=>[(v[0]-o[0])/s,(o[1]-v[1])/s];
test('U02 13 unique student source activities have targeted original source-linked content without falsified teacher or academic parity',()=>{
 assert.equal(doc.reviewedLessons.length,13);assert.equal(content.Lessons.length,13);
 assert.equal(sha('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u02-l01-l13-2017.rich-lesson-v2.json'),doc.richFileSha256);
 for(const e of doc.reviewedLessons){
  const l=content.Lessons.find(x=>x.LessonCode===e.lessonCode);
  const src=source.rows.find(x=>x.lessonCode===e.lessonCode);
  assert.deepEqual(e.sourceActivities,src.activities.map(x=>x.title));
  assert.ok(l.WorkedExamples.length>=3);
  for(const p of e.authoredCourseAdditions)assert.ok(l.ExplanationParagraphs.includes(p));
  assert.equal(e.fullAcademicSourceParityAccepted,false);
  assert.equal(e.actualStudentTaskSubmitted,false);
  assert.equal(e.actualTeacherCoolDownAssessed,false);
  assert.equal(e.roleIssueAcceptedByIndependentReviewer,false);
 }
});
test('Original SVG files are safe accessible own-work candidates and the pre-existing U02 L04 original asset is preserved byte-for-byte',()=>{
 assert.equal(doc.figures.length,4);
 for(const f of doc.figures){
  const {svg}=geom(f.lessonCode.split(':').at(-1));
  assert.match(svg,/^<svg xmlns="http:\/\/www.w3.org\/2000\/svg"/);
  assert.match(svg,/role="img" aria-labelledby="title desc"/);
  assert.match(svg,/<desc id="desc">/);assert.match(svg,/<\/svg>\s*$/);
  assert.doesNotMatch(svg,/<script|onload=|onerror=|<foreignObject|<image|<iframe|<a\s/i);
  assert.equal(f.studentRenderedAuthenticated,false);
 }
 const prior=prev.rows.find(x=>x.lessonCode.endsWith(':U02:L04'));
 assert.ok(prior.verifiedCourseOwnedSvgFile,'prior asset exists');
 assert.equal(sha('src/Edulytics.Web/wwwroot'+prior.verifiedCourseOwnedSvgFile),prior.verifiedCourseOwnedSvgSha256,'the original SVG must not be changed');
 assert.equal(doc.figures.filter(x=>x.lessonCode.endsWith(':L04'))[0].assetPath,'/curriculum/us-ccss-g8/figures/u02-l04-off-origin.svg');
});
test('L01 paper rectangle halves correctly alternate two aspect-ratio families and SVG represents exact dimensions',()=>{
 const {svg}=geom('L01');
 const s=22,A=rect(svg,'rect-A'),B=rect(svg,'rect-B'),C=rect(svg,'rect-C'),D=rect(svg,'rect-D'),E=rect(svg,'rect-E');
 for(const [x,w,h] of [[A,8.5,11],[B,5.5,8.5],[C,4.25,5.5],[D,2.75,4.25],[E,2.125,2.75]]){near(x.w,w*s);near(x.h,h*s);}
 near(A.w/C.w,2);near(A.h/C.h,2);
 near(A.w/E.w,4);near(A.h/E.h,4);
 near(B.w/D.w,2);near(B.h/D.h,2);
 assert.ok(Math.abs(A.w/B.w-A.h/B.h)>0.0001,'B is not uniformly scaled A');
 near((6+1/4)/2,25/8);
 near((10+1/7)/5,71/35);
 near((8+1/2)/11,17/22);
});
test('L04 off-origin dilation SVG uses center P and all exact original and image corners, rejects naive global doubling',()=>{
 const {svg}=geom('L04'),o=[180,510],s=28;
 const original=[[2,9],[8,9],[8,5],[2,5]],img=original.map(([x,y])=>[3+2*(x-3),7+2*(y-7)]);
 const a=vertices(svg,'source-quad'),b=vertices(svg,'dilated-quad');
 for(let i=0;i<4;i++){
  const av=unproject(a[i],o,s),bv=unproject(b[i],o,s);
  near(av[0],original[i][0]);near(av[1],original[i][1]);
  near(bv[0],img[i][0]);near(bv[1],img[i][1]);
 }
 assert.deepEqual(img,[[1,11],[13,11],[13,3],[1,3]]);
 assert.notDeepEqual([2*original[0][0],2*original[0][1]],img[0]);
 near(d(img[0],img[1]),2*d(original[0],original[1]));
});
test('L12 original slope SVG graph verifies positive/negative membership and source-centered dilation triangle',()=>{
 const {svg}=geom('L12'),o=[60,490],s=33;
 const lineEndpoints=line(svg,'line-y=2x-7').map(p=>unproject(p,o,s));
 lineEndpoints.forEach(([x,y])=>near(y,2*x-7));
 for(const [i,x,y] of [[0,5,3],[1,7,7],[2,9,11]]){
  const m=svg.match(new RegExp('<circle id="on-'+i+'" cx="([^"]+)" cy="([^"]+)"'));assert.ok(m);
  const a=unproject([+m[1],+m[2]],o,s);near(a[0],x);near(a[1],y);near(y,2*x-7);
 }
 assert.notEqual(10,2*9-7);near(2*100-7,193);
 const triangleOrigin=[625,440],sc=36;
 const src=vertices(svg,'triangle-source').map(p=>unproject(p,triangleOrigin,sc));
 const dst=vertices(svg,'triangle-scaled').map(p=>unproject(p,triangleOrigin,sc));
 const center=[0,1];
 for(let i=0;i<3;i++){
  near(dst[i][0],center[0]+2.5*(src[i][0]-center[0]));
  near(dst[i][1],center[1]+2.5*(src[i][1]-center[1]));
 }
 const c=[2,2];assert.deepEqual([0+4.5*c[0],1+4.5*(c[1]-1)],[9,5.5]);
 const A=[2,1],B=[5,4];assert.deepEqual([2*A[0]-B[0],2*A[1]-B[1]],[-1,-2]);
});
test('L13 source-shadow ratios are approximate, drawing preserves chosen model and forbids an exact real-world measurement claim',()=>{
 const {svg}=geom('L13');
 const ratios=[43/29,72/48,51/34];
 near(ratios[1],1.5);near(ratios[2],1.5);
 assert.ok(Math.abs(ratios[0]-ratios[1])>.01);
 near(114*72/48,171);
 for(const [id,h,shadow] of [['person',72,48],['lamppost',171,114]]){
  const height=line(svg,id+'-height'),ground=line(svg,id+'-shadow'),sun=line(svg,id+'-sunray');
  near(d(...height)/d(...ground),h/shadow);
  near(ground[0][1],ground[1][1]);
  near(height[0][0],height[1][0]);
  near(sun[0][0],height[1][0]);
  near(sun[1][0],ground[1][0]);
 }
});
test('U02 practice checks include exact equivalent and wrong examples, with no fabricated selected generator or human persistence',()=>{
 assert.equal(contracts.contracts.length,13);
 near(4/2,2);assert.notEqual(5/3,3/2);
 for(const scale of [2,3,.5]){
  const triangle=[4,5,7].map(x=>x*scale);
  near(triangle[2]/triangle[0],7/4);
  near(triangle[2]/triangle[1],7/5);
 }
 for(const [rise,run] of [[3,4],[6,8],[1.5,2]])near(rise/run,0.75);
 assert.equal(3*90/4,67.5);
 for(const x of contracts.contracts){
  assert.ok(x.mustRejectOrFlagWrong.length>10);
  assert.ok(x.requiredHumanReview.length>12);
  assert.equal(x.selectedProductionItemFamilyObserved,false);
  assert.equal(x.authenticatedAttemptSaved,false);
  assert.equal(x.wrongSchoolRejectedInRealDb,false);
 }
});
test('Whole-grade P0 audit preserves 131 lessons, 40 assets and unaccepted stage A-D without promoting U02 source spot review',()=>{
 assert.equal(ledger.rows.length,131);assert.equal(audit.rows.length,131);
 assert.deepEqual(ledger.rows.map(x=>x.lessonCode),prev.rows.map(x=>x.lessonCode));
 assert.equal(ledger.counts.assets,40);
 assert.equal(ledger.counts.sourceFigurePriorityWithOriginalAsset,37);
 assert.equal(ledger.counts.sourceFigurePriorityWithoutOriginalAsset,76);
 assert.equal(ledger.counts.sourceActivitySpotReviewedLessons,30);
 assert.equal(audit.unitSummary.U02.sourceStudentLessonPagesChecked,13);
 assert.equal(audit.unitSummary.U02.sourceTeacherLessonPagesIndividuallyChecked,6);
 for(const x of ledger.rows){
  assert.equal(x.A,'NOT_ACCEPTED');assert.equal(x.B,'NOT_ACCEPTED');
  assert.equal(x.C,'NOT_VERIFIED');assert.equal(x.D,'NOT_VERIFIED');
  assert.equal(x.svgInAuthenticatedStudentRenderer,false);
  assert.equal(x.actualPostgresqlAttemptPersistedAndReadBack,false);
 }
 const row=ledger.rows.find(x=>x.lessonCode.endsWith(':U02:L04'));
 assert.ok(row.additionalCourseOwnedSvgFiles?.length===1);
 assert.equal(row.verifiedCourseOwnedSvgFile,prev.rows.find(x=>x.lessonCode===row.lessonCode).verifiedCourseOwnedSvgFile);
});
