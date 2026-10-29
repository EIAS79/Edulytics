
'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const root=path.resolve(__dirname,'..'),read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const evidence=read('docs/curriculum/us-ccss-g8-coder4-v13-u01-source-geometry-evidence-20261029.json');
const ledger=read('docs/curriculum/us-ccss-g8-coder4-v13-implementation-ledger-20261029.json');
const audit=read('docs/curriculum/us-ccss-g8-coder4-v13-course-source-practice-teacher-audit-20261029.json');
const prior=read('docs/curriculum/us-ccss-g8-coder4-v12-implementation-ledger-20261029.json');
const source=read('docs/curriculum/us-ccss-g8-coder4-v9-activity-figure-inventory-20261029.json');
const sha=p=>crypto.createHash('sha256').update(fs.readFileSync(path.join(root,p))).digest('hex');
const near=(a,b,t=.0002)=>assert.ok(Math.abs(a-b)<t,`${a} not approximately ${b}`);
const length2=(a,b)=>(a.x-b.x)**2+(a.y-b.y)**2;
const half=(p,c)=>({x:2*c.x-p.x,y:2*c.y-p.y});
const rotate=(p,d)=>{const t=d*Math.PI/180;return{x:p.x*Math.cos(t)-p.y*Math.sin(t),y:p.x*Math.sin(t)+p.y*Math.cos(t)};};
const orient=(a,b,c)=>(b.x-a.x)*(c.y-a.y)-(b.y-a.y)*(c.x-a.x);
const origin={L08:[{x:223,y:323,scale:55},{x:600,y:395,scale:35}],L09:[{x:142,y:365,scale:28},{x:439,y:365,scale:28},{x:736,y:365,scale:28}],L10:[{x:115,y:390,scale:65},{x:690,y:395,scale:75}]};
const geom=(id)=>{
 const item=evidence.figures.find(x=>x.lessonCode.endsWith(':'+id));
 assert.ok(item,'figure '+id+' missing');
 const file='src/Edulytics.Web/wwwroot'+item.assetPath;
 const svg=fs.readFileSync(path.join(root,file),'utf8');
 return {item,svg,file};
};
function coord(line){return line.split(',').map(Number);}
function group(svg,id,type){
 const name=type||'polygon';
 const m=svg.match(new RegExp('<'+name+' id="'+id+'" ([^>]+)>'));
 assert.ok(m,'missing geometry '+id);
 const attrs=Object.fromEntries([...m[1].matchAll(/([\w-]+)="([^"]+)"/g)].map(x=>[x[1],x[2]]));
 if(name==='polygon')return attrs.points.split(/\s+/).map(x=>{const [a,b]=coord(x);return{x:a,y:b};});
 return [{x:Number(attrs.x1),y:Number(attrs.y1)},{x:Number(attrs.x2),y:Number(attrs.y2)}];
}
function unproject(q,frame){return {x:(q.x-frame.x)/frame.scale,y:(frame.y-q.y)/frame.scale};}
function match(actual,expected,frame){assert.equal(actual.length,expected.length);for(let i=0;i<expected.length;i++){const p=unproject(actual[i],frame);near(p.x,expected[i].x);near(p.y,expected[i].y);}}
test('U01 V13 documents original source scope without misrepresenting full parity',()=>{
 assert.equal(evidence.lessons.length,3);
 const list=['L08','L09','L10'];
 for(const x of evidence.lessons){
  assert.ok(list.some(id=>x.lessonCode.endsWith(':'+id)));
  assert.ok(x.addedTeacherStudentActivityPrompts.length>=3);
  assert.equal(x.fullSourceParityAccepted,false);
  assert.equal(x.teacherRubricPersisted,false);
  assert.equal(x.interactiveConstructionImplemented,false);
 }
 for(const id of list){
  const pack=id==='L08'?'us-ccss-g8-u01-l01-l08-2017.rich-lesson-v2.json':'us-ccss-g8-u01-l09-l17-2017.rich-lesson-v2.json';
  const file='src/Edulytics.Core/Curriculum/LessonContent/RichV2/'+pack;
  const extended=read('docs/curriculum/us-ccss-g8-coder4-v13-u01-l11-l17-source-evidence-20261029.json');
  // L09-L17 pack received a later source-specific batch. Its older V13 hash remains
  // an immutable historical checkpoint, not a checksum of the newest pack.
  const expected=pack.includes('l09-l17')?extended.updatedRichCandidateHash:evidence.updatedRichCandidateHashes[pack];
  assert.equal(sha(file),expected);
  const rich=read(file).Lessons.find(x=>x.LessonCode.endsWith(':'+id));
  const src=source.rows.find(x=>x.lessonCode===rich.LessonCode);
  assert.equal(rich.WorkedExamples.length,3,'existing worked examples must remain intact');
  assert.equal(rich.ExplanationParagraphs.filter(p=>p.startsWith('Activity ')).length,id==='L10'?4:3);
  assert.equal(src.activities.length,id==='L10'?4:3);
  assert.equal(rich.Visuals.length,id==='L09'?1:0);
 }
});
test('Three original accessible SVGs have correct SHA, safe markup and no publisher image use',()=>{
 for(const id of ['L08','L09','L10']){
  const {item,svg,file}=geom(id);
  assert.equal(sha(file),item.sha256);
  assert.match(svg,/^<svg xmlns="http:\/\/www.w3.org\/2000\/svg"/);
  assert.match(svg,/role="img" aria-labelledby="title desc"/);
  assert.match(svg,/<title id="title">/);
  assert.match(svg,/<desc id="desc">/);
  assert.match(svg,/<\/svg>\s*$/);
  assert.doesNotMatch(svg,/<script|<foreignObject|onload=|onerror=|<iframe|<image|<a\s/i);
  assert.doesNotMatch(svg,/cms-assets\.illustrativemathematics\.org/i);
  assert.equal(item.authenticatedStudentRendered,false);
  assert.equal(item.sourceParityAccepted,false);
 }
});
test('L08 four quarter turns are exact rigid rotations, forming a square',()=>{
 const {item,svg}=geom('L08'),a=item.data.triangle,frame=origin.L08[0];
 assert.equal(item.data.turns.length,4);
 for(let i=0;i<4;i++){
  const expected=a.map(p=>rotate(p,item.data.turns[i]));
  const drawn=group(svg,'quarter-'+i);
  match(drawn,expected,frame);
  for(let m=0;m<3;m++)for(let n=m+1;n<3;n++)
   near(length2(unproject(drawn[m],frame),unproject(drawn[n],frame)),length2(a[m],a[n]));
  near(orient(expected[0],expected[1],expected[2]),orient(...a));
 }
 let tips=item.data.turns.flatMap(d=>a.slice(1).map(p=>rotate(p,d)));
 const uniq=new Set(tips.map(p=>Math.round(p.x)+','+Math.round(p.y)));
 assert.equal(uniq.size,4,'right isosceles copies share four outer square vertices');
 near(Math.max(...tips.map(p=>p.x)),3);
 near(Math.min(...tips.map(p=>p.x)),-3);
});
test('L08 180-degree rotations distinguish endpoint, midpoint and off-line centers',()=>{
 const {item,svg}=geom('L08'),a=item.data.segment;
 const frame=origin.L08[1];
 assert.equal(item.data.centers.length,3);
 for(const c of item.data.centers){
  const out=a.map(p=>half(p,c)),got=group(svg,'half-'+c.name,'line');
  match(got,out,frame);
  near(length2(out[0],out[1]),length2(...a));
  const twice=out.map(p=>half(p,c));
  for(let i=0;i<2;i++){near(twice[i].x,a[i].x);near(twice[i].y,a[i].y);}
  if(c.name==='midpoint'){near(out[0].x,a[1].x);near(out[1].x,a[0].x);}
  if(c.name==='endpoint'){assert.ok(out[0].x>a[1].x);}
  if(c.name==='off-line'){assert.ok(out.every(p=>p.y===4));}
 }
});
test('L09 coordinate images preserve parallelism and exact perpendicular separation',()=>{
 const {item,svg}=geom('L09');
 assert.deepEqual(item.data.lines,[1,4]);
 const maps=[p=>({x:p.x+2,y:p.y+3}),p=>({x:-p.x,y:-p.y}),p=>({x:p.x,y:-p.y})];
 for(let j=0;j<3;j++){
  for(let k=0;k<2;k++){
   const y=item.data.lines[k],a={x:-3,y},b={x:4,y},fr=origin.L09[j];
   match(group(svg,'source-'+j+'-'+k,'line'),[a,b],fr);
   match(group(svg,'image-'+j+'-'+k,'line'),[maps[j](a),maps[j](b)],fr);
  }
  const y0=maps[j]({x:0,y:1}).y,y1=maps[j]({x:0,y:4}).y;
  near(Math.abs(y1-y0),3);
 }
 assert.notDeepEqual(maps[1]({x:3,y:4}),maps[2]({x:3,y:4}),'half-turn and mirror differ on labeled points');
});
test('L10 half-turn creates a true parallelogram with half-area triangle; equal-leg triangle is exact',()=>{
 const {item,svg}=geom('L10'),[A,B,C]=item.data.triangle,M=item.data.midpoint;
 const frame=origin.L10[0];
 const images=[A,B,C].map(p=>half(p,M)),D=images[1];
 match(group(svg,'triangle-original'),[A,B,C],frame);
 match(group(svg,'triangle-halfturn'),images,frame);
 near(images[0].x,C.x);near(images[0].y,C.y);
 near(images[2].x,A.x);near(images[2].y,A.y);
 near(D.x,4);near(D.y,0);
 const triArea=Math.abs(orient(A,B,C))/2;
 const paraArea=Math.abs(orient(A,B,D));
 near(triArea,4);near(paraArea,8);near(paraArea,triArea*2);
 near(B.x-A.x,C.x-D.x);near(B.y-A.y,C.y-D.y);
 near(C.x-B.x,D.x-A.x);near(C.y-B.y,D.y-A.y);
 const iso=item.data.isosceles;
 match(group(svg,'isosceles-original'),[...iso.base,iso.apex],origin.L10[1]);
 near(length2(iso.base[0],iso.apex),9);
 near(length2(iso.base[1],iso.apex),9);
 near(length2(...iso.base),4);
});
test('V13 full nine-unit academic inventory is evidence-honest and no old lessons were removed',()=>{
 assert.equal(ledger.rows.length,131);
 assert.equal(audit.rows.length,131);
 assert.equal(new Set(ledger.rows.map(x=>x.lessonCode)).size,131);
 assert.deepEqual(ledger.rows.map(x=>x.lessonCode),prior.rows.map(x=>x.lessonCode));
 assert.equal(ledger.counts.assets,34);
 assert.equal(ledger.counts.sourceFigurePriorityWithOriginalAsset,32);
 assert.equal(ledger.counts.sourceFigurePriorityWithoutOriginalAsset,81);
 assert.equal(ledger.counts.AAccepted,0);assert.equal(ledger.counts.BAccepted,0);
 assert.equal(ledger.counts.CVerified,0);assert.equal(ledger.counts.DVerified,0);
 const delta=ledger.rows.filter((x,i)=>x.verifiedCourseOwnedSvgFile!==prior.rows[i].verifiedCourseOwnedSvgFile);
 assert.deepEqual(delta.map(x=>x.lessonCode.split(':').at(-1)),['L08','L09','L10']);
 assert.equal(Object.keys(audit.unitSummary).length,9);
 const units=Object.values(audit.unitSummary);
 assert.equal(units.reduce((sum,u)=>sum+u.lessons,0),131);
 assert.equal(units.reduce((sum,u)=>sum+u.studentActivities,0),411);
 assert.equal(units.reduce((sum,u)=>sum+u.studentTables,0),79);
 assert.equal(units.reduce((sum,u)=>sum+u.sourceRoleDiscrepancies,0),22);
 assert.equal(units.reduce((sum,u)=>sum+u.staticUnmappedLessons,0),12);
 assert.equal(units.reduce((sum,u)=>sum+u.multipleFamilyLessons,0),18);
 for(const r of audit.rows){
  assert.equal(r.perLessonAcademicAcceptance.A,'NOT_ACCEPTED');
  assert.equal(r.perLessonAcademicAcceptance.B,'NOT_ACCEPTED');
  assert.equal(r.perLessonAcademicAcceptance.C,'NOT_VERIFIED');
  assert.equal(r.perLessonAcademicAcceptance.D,'NOT_VERIFIED');
  assert.equal(r.practiceContractAudit.actualSelectedPracticeFamilyTested,false);
  assert.equal(r.teacherAssessmentAudit.teacherScoringRuntimeVerified,false);
  assert.equal(r.visuals.authenticatedStudentRenderVerified,false);
 }
});
