'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const crypto=require('node:crypto');
const root=path.resolve(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const manifest=read('docs/curriculum/us-ccss-g8-coder4-v12-u01-l02-source-geometry-evidence-20261029.json');
const ledger=read('docs/curriculum/us-ccss-g8-coder4-v12-implementation-ledger-20261029.json');
const oldLedger=read('docs/curriculum/us-ccss-g8-coder4-v11-implementation-ledger-20261029.json');
const source=read('docs/curriculum/us-ccss-g8-coder4-v9-activity-figure-inventory-20261029.json');
const pack=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u01-l01-l08-2017.rich-lesson-v2.json');
const svg=fs.readFileSync(path.join(root,'src/Edulytics.Web/wwwroot',manifest.assetPath.slice(1)),'utf8');
const near=(x,y,e=.0002)=>assert.ok(Math.abs(x-y)<e,`${x} must equal ${y} within ${e}`);
const area=pts=>pts.reduce((sum,p,i)=>sum+p.x*pts[(i+1)%pts.length].y-pts[(i+1)%pts.length].x*p.y,0)/2;
const d2=(a,b)=>(a.x-b.x)**2+(a.y-b.y)**2;
const polygon=id=>{
 const m=svg.match(new RegExp('<polygon id="'+id+'" points="([^"]+)"'));
 assert.ok(m,'missing SVG polygon '+id);
 return m[1].trim().split(/\s+/).map(pair=>{const [x,y]=pair.split(',').map(Number);assert.ok(Number.isFinite(x)&&Number.isFinite(y));return {x,y};});
};
const world=(pts,projection)=>pts.map(p=>({x:(p.x-projection.originPx.x)/projection.pixelsPerUnit,y:(projection.originPx.y-p.y)/projection.pixelsPerUnit}));
test('V12 source activity gap is lesson-specific and does not counterfeit visual or source acceptance',()=>{
 assert.equal(manifest.lessonCode,'PED:US-CCSS-MATH:G8:U01:L02');
 const src=source.rows.find(r=>r.lessonCode===manifest.lessonCode);
 assert.ok(src);
 assert.equal(src.studentActivityCount,3);
 assert.deepEqual(src.activities.map(x=>x.title),manifest.sourceActivityReview.map(x=>x.sourceActivity));
 assert.equal(src.sourceStudentUrl,manifest.sourceStudentUrl);
 assert.equal(src.sourceTeacherUrl,manifest.sourceTeacherUrl);
 assert.equal(manifest.sourceActivityReview[2].interactiveCardSortImplemented,false);
 assert.equal(manifest.acceptance.sourceParityComplete,false);
 for(const x of ['svgAttachedToRichV2','authenticatedStudentHtmlVerified','practiceGeneratedAndPersisted','teacherScoringPersisted','realTwoSchoolPostgresFixturePassed','academicReviewerSignoff']){
  assert.equal(manifest.acceptance[x],false,x);
 }
 const lesson=pack.Lessons.find(l=>l.LessonCode===manifest.lessonCode);
 assert.ok(lesson);
 assert.ok(lesson.ExplanationParagraphs.some(s=>s.includes('120° counterclockwise turn')));
 assert.ok(lesson.ExplanationParagraphs.some(s=>s.includes('source activities 2.2 and 2.3')));
 assert.ok(lesson.SummaryPoints.some(s=>s.includes('original figure does not have to be symmetric')));
 assert.ok(!lesson.SummaryPoints.includes('Reflections require a line of symmetry.'));
 assert.equal(lesson.WorkedExamples.length,3);
});
test('Original SVG has safe accessible content and matches the recorded asset hash',()=>{
 assert.equal(crypto.createHash('sha256').update(svg).digest('hex'),manifest.sha256);
 assert.match(svg,/^<svg xmlns="http:\/\/www.w3.org\/2000\/svg"/);
 assert.match(svg,/role="img" aria-labelledby="title desc"/);
 assert.match(svg,/<title id="title">/);
 assert.match(svg,/<desc id="desc">/);
 assert.match(svg,/<\/svg>\s*$/);
 assert.match(svg,/120° counterclockwise/);
 assert.match(svg,/Reflection across x = 0/);
 assert.doesNotMatch(svg,/<script|<foreignObject|onload=|onerror=|<iframe|<image|<a\s/i);
 assert.doesNotMatch(svg,/illustrativemathematics\.org|kendallhunt\.com/i);
});
test('SVG 120-degree CCW quadrilateral is mathematically exact and not a clockwise or quarter-turn',()=>{
 const spec=manifest.projection.rotation;
 const a=spec.original;
 const drawnOriginal=world(polygon('rotation-original'),spec);
 const drawnImage=world(polygon('rotation-image'),spec);
 assert.equal(a.length,4);
 assert.equal(drawnOriginal.length,a.length);
 assert.equal(drawnImage.length,a.length);
 const rad=2*Math.PI/3;
 for(let i=0;i<a.length;i++){
  near(drawnOriginal[i].x,a[i].x);
  near(drawnOriginal[i].y,a[i].y);
  const expected={x:-.5*a[i].x-Math.sqrt(3)/2*a[i].y,y:Math.sqrt(3)/2*a[i].x-.5*a[i].y};
  near(drawnImage[i].x,expected.x);
  near(drawnImage[i].y,expected.y);
  for(let j=i+1;j<a.length;j++)near(d2(drawnImage[i],drawnImage[j]),d2(a[i],a[j]),.0005);
 }
 assert.ok(area(a)*area(drawnImage)>0,'rotation must preserve signed orientation');
 near(drawnImage[0].x,a[0].x);near(drawnImage[0].y,a[0].y);
 assert.ok(drawnImage[1].y>1.7,'not a clockwise or 90-degree rotation');
 near(Math.atan2(drawnImage[1].y,drawnImage[1].x),rad,.0001);
});
test('SVG vertical reflection keeps distances, flips signed orientation and really uses x = 0',()=>{
 const spec=manifest.projection.reflection;
 const a=spec.original,b=world(polygon('reflection-image'),spec),original=world(polygon('reflection-original'),spec);
 assert.equal(a.length,4);
 for(let i=0;i<a.length;i++){
  near(original[i].x,a[i].x);near(original[i].y,a[i].y);
  near(b[i].x,-a[i].x);near(b[i].y,a[i].y);
  near((a[i].x+b[i].x)/2,0);
  near(Math.abs(a[i].x),Math.abs(b[i].x));
  for(let j=i+1;j<a.length;j++)near(d2(a[i],a[j]),d2(b[i],b[j]),.0005);
 }
 assert.ok(area(a)*area(b)<0,'reflection must reverse signed orientation, unlike rotation');
});
test('V12 ledger keeps 131 lessons, all per-lesson A-D blocked and immutable V11 evidence',()=>{
 assert.equal(ledger.rows.length,131);
 assert.equal(new Set(ledger.rows.map(r=>r.lessonCode)).size,131);
 assert.equal(ledger.counts.assets,31);
 assert.equal(ledger.counts.sourceFigurePriorityWithOriginalAsset,29);
 assert.equal(ledger.counts.sourceFigurePriorityWithoutOriginalAsset,84);
 assert.equal(ledger.counts.AAccepted,0);assert.equal(ledger.counts.BAccepted,0);
 assert.equal(ledger.counts.CVerified,0);assert.equal(ledger.counts.DVerified,0);
 const changed=ledger.rows.filter((r,i)=>JSON.stringify(r)!==JSON.stringify(oldLedger.rows[i]));
 assert.equal(changed.length,1);
 assert.equal(changed[0].lessonCode,manifest.lessonCode);
 assert.equal(changed[0].verifiedCourseOwnedSvgSha256,manifest.sha256);
 assert.equal(changed[0].svgInAuthenticatedStudentRenderer,false);
 assert.equal(ledger.counts.authenticatedSvgRenderedLessons,0);
 assert.equal(ledger.counts.fullAcademicSourceParityAcceptedLessons,0);
 for(const row of ledger.rows){
  assert.equal(row.A,'NOT_ACCEPTED');assert.equal(row.B,'NOT_ACCEPTED');
  assert.equal(row.C,'NOT_VERIFIED');assert.equal(row.D,'NOT_VERIFIED');
  assert.equal(row.actualPostgresqlAttemptPersistedAndReadBack,false);
  assert.equal(row.teacherScoringPersistenceVerified,false);
 }
});
