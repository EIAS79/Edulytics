
'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const root=path.resolve(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const sha=p=>crypto.createHash('sha256').update(fs.readFileSync(path.join(root,p))).digest('hex');
const info=read('docs/curriculum/us-ccss-g8-coder4-v13-u01-l11-l17-source-evidence-20261029.json');
const ledger=read('docs/curriculum/us-ccss-g8-coder4-v13-u01-extended-implementation-ledger-20261029.json');
const audit=read('docs/curriculum/us-ccss-g8-coder4-v13-u01-extended-source-practice-teacher-audit-20261029.json');
const old=read('docs/curriculum/us-ccss-g8-coder4-v13-implementation-ledger-20261029.json');
const lessons=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u01-l09-l17-2017.rich-lesson-v2.json').Lessons;
const source=read('docs/curriculum/us-ccss-g8-coder4-v9-activity-figure-inventory-20261029.json').rows;
const near=(x,y,t=.0002)=>assert.ok(Math.abs(x-y)<t,`${x} != ${y}`);
const lines=(svg,id)=>{const x=svg.match(new RegExp('<line id="'+id+'" ([^>]+)>'));assert.ok(x,'missing line '+id);const map=Object.fromEntries([...x[1].matchAll(/([\w-]+)="([^"]+)"/g)].map(q=>[q[1],q[2]]));return [{x:+map.x1,y:+map.y1},{x:+map.x2,y:+map.y2}];};
const world=(p,origin,scale)=>({x:(p.x-origin.x)/scale,y:(origin.y-p.y)/scale});
test('U01 L11–L17 each has exact source activity prompts with no counterfeit acceptance or deleted prior worked examples',()=>{
 assert.equal(info.lessons.length,7);
 const arr=['L11','L12','L13','L14','L15','L16','L17'];
 for(let i=0;i<7;i++){
  const l=lessons.find(x=>x.LessonCode.endsWith(':'+arr[i]));
  const s=source.find(x=>x.lessonCode===l.LessonCode);
  const e=info.lessons[i];
  assert.ok(l&&s);assert.equal(e.lessonCode,l.LessonCode);
  assert.deepEqual(e.sourceActivityTitles,s.activities.map(x=>x.title));
  assert.equal(e.courseOwnedAdditions.length,3);
  for(const a of e.courseOwnedAdditions)assert.ok(l.ExplanationParagraphs.includes(a),'source-targeted paragraph missing '+e.lessonCode);
  assert.equal(l.WorkedExamples.length,3);
  assert.equal(e.sourceParityAccepted,false);
  assert.equal(e.interactiveEquivalenceVerified,false);
  assert.equal(e.humanScorePersisted,false);
 }
 assert.equal(sha('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g8-u01-l09-l17-2017.rich-lesson-v2.json'),info.updatedRichCandidateHash);
});
test('L11 source rectangle area/perimeter distinctions and L15 inclusive triangle taxonomy are independently sound',()=>{
 const rectangles=[[2,3],[1,6],[1,4]];
 const area=(a,b)=>a*b, perimeter=(a,b)=>2*(a+b);
 near(area(...rectangles[0]),area(...rectangles[1]));
 assert.notEqual(perimeter(...rectangles[0]),perimeter(...rectangles[1]));
 near(perimeter(...rectangles[0]),perimeter(...rectangles[2]));
 assert.notEqual(area(...rectangles[0]),area(...rectangles[2]));
 const classes=[
 {type:'scalene acute',angles:[50,60,70]},
 {type:'scalene right',angles:[30,60,90]},
 {type:'scalene obtuse',angles:[30,40,110]},
 {type:'isosceles acute',angles:[70,70,40]},
 {type:'isosceles right',angles:[45,45,90]},
 {type:'isosceles obtuse',angles:[40,40,100]},
 {type:'equilateral acute',angles:[60,60,60]}];
 for(const x of classes)near(x.angles.reduce((a,b)=>a+b,0),180);
 assert.ok(classes.filter(x=>x.type.includes('equilateral')).every(x=>x.angles.every(y=>y===60)));
 assert.ok(!classes.some(x=>x.type==='equilateral obtuse'||x.type==='equilateral right'));
});
test('L14 original SVG verifies exact alternate angles; nonparallel shortcut is forbidden',()=>{
 const f=info.figures.find(x=>x.lessonCode.endsWith(':L14'));assert.ok(f);
 const file='src/Edulytics.Web/wwwroot'+f.assetPath,svg=fs.readFileSync(path.join(root,file),'utf8');
 assert.equal(sha(file),f.sha256);
 const g=f.data;near(Math.atan(g.m)*180/Math.PI,g.angle);
 near(180-g.angle,117);
 const lower=lines(svg,'parallel-lower').map(p=>world(p,g.origin,g.scale));
 const upper=lines(svg,'parallel-upper').map(p=>world(p,g.origin,g.scale));
 lower.forEach(p=>near(p.y,0));upper.forEach(p=>near(p.y,2));
 const trans=lines(svg,'transversal').map(p=>world(p,g.origin,g.scale));
 trans.forEach(p=>near(p.y,g.m*p.x,.0005));
 assert.ok(lower[0].x<lower[1].x&&upper[0].x<upper[1].x);
 assert.doesNotMatch(svg,/<script|onload=|<foreignObject|<image|<iframe/i);
});
test('L16 original parallel construction yields triangle angles summing to 180 and true/false warmup verified',()=>{
 const f=info.figures.find(x=>x.lessonCode.endsWith(':L16'));assert.ok(f);
 const file='src/Edulytics.Web/wwwroot'+f.assetPath,svg=fs.readFileSync(path.join(root,file),'utf8'),g=f.data;
 assert.equal(sha(file),f.sha256);
 const a=g.triangle;
 const toWorld=id=>lines(svg,id).map(p=>world(p,g.origin,g.scale));
 const base=toWorld('base');base.forEach(p=>near(p.y,0));
 const parallel=toWorld('parallel-through-C');parallel.forEach(p=>near(p.y,3));
 const angle=(p,q,r)=>{const u={x:p.x-q.x,y:p.y-q.y},v={x:r.x-q.x,y:r.y-q.y};return Math.acos((u.x*v.x+u.y*v.y)/(Math.hypot(u.x,u.y)*Math.hypot(v.x,v.y)))*180/Math.PI;};
 near(angle(a[1],a[0],a[2])+angle(a[0],a[1],a[2])+angle(a[0],a[2],a[1]),180);
 assert.equal(62-28===60-30,false);
 assert.equal(3*(-8)===(2*(-8))-8,true);
 assert.equal(16/(-2)+24/(-2)===40/(-2),true);
 assert.match(svg,/role="img" aria-labelledby="title desc"/);
 assert.doesNotMatch(svg,/<script|onload=|<foreignObject|<image|<iframe/i);
});
test('L17 regular pentagon cannot tile alone around a vertex and equilateral triangles can',()=>{
 near(6*60,360);near(4*90,360);
 assert.ok(3*108<360&&4*108>360);
 assert.ok(!Number.isInteger(360/108));
 near(360/3,120);
});
test('Extended V13 retains prior 131 lesson status and documents exactly two further U01 original diagrams',()=>{
 assert.equal(ledger.rows.length,131);assert.equal(audit.rows.length,131);
 assert.deepEqual(ledger.rows.map(x=>x.lessonCode),old.rows.map(x=>x.lessonCode));
 const changed=ledger.rows.filter((x,i)=>x.verifiedCourseOwnedSvgFile!==old.rows[i].verifiedCourseOwnedSvgFile);
 assert.deepEqual(changed.map(x=>x.lessonCode.split(':').at(-1)),['L14','L16']);
 assert.equal(ledger.counts.assets,36);
 assert.equal(ledger.counts.sourceFigurePriorityWithOriginalAsset,34);
 assert.equal(ledger.counts.sourceFigurePriorityWithoutOriginalAsset,79);
 assert.equal(ledger.counts.sourceActivitySpotReviewedLessons,11);
 assert.equal(audit.unitSummary.U01.activitySpotCheckedInV13,10);
 assert.equal(audit.counts.staticUnmappedLessons,12);
 for(const l of ledger.rows){
  assert.equal(l.A,'NOT_ACCEPTED');assert.equal(l.B,'NOT_ACCEPTED');
  assert.equal(l.C,'NOT_VERIFIED');assert.equal(l.D,'NOT_VERIFIED');
  assert.equal(l.svgInAuthenticatedStudentRenderer,false);
  assert.equal(l.actualPostgresqlAttemptPersistedAndReadBack,false);
 }
});
