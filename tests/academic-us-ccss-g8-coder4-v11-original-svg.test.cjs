'use strict';
const test=require('node:test'),assert=require('node:assert/strict'),fs=require('node:fs'),path=require('node:path'),crypto=require('node:crypto');
const root=path.resolve(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const scenes=read('docs/curriculum/us-ccss-g8-coder4-v11-original-geometry-figures-20261029.json');
const source=read('docs/curriculum/us-ccss-g8-coder4-v9-activity-figure-inventory-20261029.json');
const src=new Map(source.rows.map(x=>[x.lessonCode,x]));
const richDir='src/Edulytics.Core/Curriculum/LessonContent/RichV2';
const lessons=new Map(fs.readdirSync(path.join(root,richDir)).filter(f=>/^us-ccss-g8-.*\.rich-lesson-v2\.json$/.test(f))
 .flatMap(f=>read(richDir+'/'+f).Lessons).map(x=>[x.LessonCode,x]));
const abs=p=>path.join(root,p);
const sq=(a,b)=>(a.x-b.x)**2+(a.y-b.y)**2;
const hypot=(a,b)=>Math.hypot(a.x-b.x,a.y-b.y);
const dot=(v,p,q)=>(p.x-v.x)*(q.x-v.x)+(p.y-v.y)*(q.y-v.y);
const assertNear=(a,b,k)=>assert.ok(Math.abs(a-b)<1e-8,k+': '+a+' != '+b);
test('30 original grade 8 figures are safe, validly attributed, source anchored and carry accessible text alternatives',()=>{
 assert.equal(scenes.count,30);
 assert.equal(scenes.scenes.length,30);
 assert.equal(new Set(scenes.scenes.map(s=>s.lessonCode)).size,30);
 assert.equal(lessons.size,131);
 const types=scenes.scenes.reduce((m,s)=>(m[s.type]=(m[s.type]||0)+1,m),{});
 assert.deepEqual(types,{'transformation':4,'coordinate-graph':21,'right-triangle':4,'area-proof':1});
 assert.deepEqual([...new Set(scenes.scenes.map(s=>s.lessonCode.match(/:U(\d{2}):/)[1]))].sort(),['01','02','03','04','05','06','08','09']);
 for(const scene of scenes.scenes){
  const lesson=lessons.get(scene.lessonCode),sourceRow=src.get(scene.lessonCode);
  assert.ok(lesson&&sourceRow,scene.lessonCode);
  assert.ok(lesson.WorkedExamples.some(x=>x.Question.includes(scene.authoredWorkedExampleAnchor)),scene.lessonCode);
  assert.equal(scene.sourceStudentUrl,sourceRow.sourceStudentUrl);
  assert.equal(scene.sourceTeacherUrl,sourceRow.sourceTeacherUrl);
  assert.equal(scene.attachedToStudentRenderer,false);
  assert.equal(scene.verifiedInAuthenticatedBrowser,false);
  assert.match(scene.assetPath,/^\/curriculum\/us-ccss-g8\/figures\/u\d{2}-l\d{2}\.svg$/);
  const file=abs('src/Edulytics.Web/wwwroot'+scene.assetPath);
  const svg=fs.readFileSync(file,'utf8');
  assert.equal(crypto.createHash('sha256').update(svg).digest('hex'),scene.sha256);
  assert.match(svg,/^<svg xmlns="http:\/\/www\.w3\.org\/2000\/svg"/);
  assert.match(svg,/role="img" aria-labelledby="title desc"/);
  assert.match(svg,/<title id="title">/);
  assert.match(svg,/<desc id="desc">/);
  assert.match(svg,/<\/svg>\s*$/);
  assert.ok(scene.description.length>=65,scene.lessonCode);
  assert.ok(svg.includes(scene.title));
  assert.doesNotMatch(svg,/<script|<foreignObject|onload=|onerror=|<iframe|<image|<a /i);
  assert.ok(!svg.includes('cms-assets.illustrativemathematics.org'));
  assert.ok((svg.match(/<text\b/g)||[]).length>=4,scene.lessonCode);
 }
});
test('all four original transformation diagrams preserve distances and common translations exactly',()=>{
 for(const scene of scenes.scenes.filter(x=>x.type==='transformation')){
  const {original:a,image:b}=scene.data;
  assert.equal(a.length,b.length);
  const dx=b[0].x-a[0].x,dy=b[0].y-a[0].y;
  for(let i=0;i<a.length;i++){
   assertNear(b[i].x-a[i].x,dx,scene.lessonCode);
   assertNear(b[i].y-a[i].y,dy,scene.lessonCode);
   for(let j=i+1;j<a.length;j++)assertNear(sq(a[i],a[j]),sq(b[i],b[j]),scene.lessonCode);
  }
 }
});
test('all 21 coordinate diagrams plot algebraically correct lines, with exact labeled off-line counterexamples',()=>{
 let counterexamples=0;
 for(const scene of scenes.scenes.filter(x=>x.type==='coordinate-graph')){
  const {points,lines,xmin,xmax,ymin,ymax}=scene.data;
  assert.ok(xmin<xmax&&ymin<ymax,scene.lessonCode);
  assert.ok(points.every(p=>Number.isFinite(p.x)&&Number.isFinite(p.y)));
  assert.ok(new Set(points.map(p=>p.label)).size===points.length);
  for(const line of lines){
   if(line.quadratic)continue;
   assert.ok(Number.isFinite(line.m)&&Number.isFinite(line.b));
  }
  for(const point of points){
   const on=lines.length===0||lines.some(line=>Math.abs(point.y-(line.quadratic?point.x**2:line.m*point.x+line.b))<1e-8);
   if(on)continue;
   const deliberate=scene.lessonCode.endsWith('U03:L12')&&point.label==='D' ||
    scene.lessonCode.endsWith('U04:L10')&&point.label==='no';
   assert.equal(deliberate,true,scene.lessonCode+' unexpected point '+point.label);
   counterexamples++;
  }
 }
 assert.equal(counterexamples,2);
});
test('right triangles and first-party dissection diagrams are independently mathematically correct',()=>{
 for(const scene of scenes.scenes.filter(s=>s.type==='right-triangle')){
  const {a,b,c}=scene.data;
  assert.ok([dot(a,b,c),dot(b,a,c),dot(c,a,b)].some(x=>Math.abs(x)<1e-8),scene.lessonCode);
  const dists=[sq(a,b),sq(b,c),sq(c,a)].sort((x,y)=>x-y);
  assertNear(dists[0]+dists[1],dists[2],scene.lessonCode);
  assert.ok(Math.sqrt(dists[2])>0);
 }
 const area=scenes.scenes.find(s=>s.type==='area-proof').data;
 assert.equal(area.a**2+area.b**2,area.c**2);
 assert.equal((area.a+area.b)**2-2*area.a*area.b,area.c**2);
});

test('V11 exact per-lesson ledger keeps 131 distinct assets and synthetic service checks separate from A-D acceptance',()=>{
 const ledger=read('docs/curriculum/us-ccss-g8-coder4-v11-implementation-ledger-20261029.json');
 assert.equal(ledger.rows.length,131);
 assert.equal(ledger.counts.sourceFigurePriority,113);
 assert.equal(ledger.counts.sourceFigurePriorityWithOriginalAsset,28);
 assert.equal(ledger.counts.sourceFigurePriorityWithoutOriginalAsset,85);
 assert.equal(ledger.counts.grade8LessonsWithServiceOnlySyntheticPracticeHarness,1);
 assert.equal(ledger.counts.teacherOnlyCooldowns,117);
 const indexed=new Map(ledger.rows.map(x=>[x.lessonCode,x]));
 assert.equal(indexed.size,131);
 assert.equal(indexed.get('PED:US-CCSS-MATH:G8:U04:L05').localSyntheticServiceGenerationAndFakeAttemptVerified,true);
 assert.equal(indexed.get('PED:US-CCSS-MATH:G8:U03:L12').localSyntheticServiceGenerationAndFakeAttemptVerified,false);
 for(const r of ledger.rows){
  assert.equal(r.A,'NOT_ACCEPTED');assert.equal(r.B,'NOT_ACCEPTED');
  assert.equal(r.C,'NOT_VERIFIED');assert.equal(r.D,'NOT_VERIFIED');
  assert.equal(r.svgInAuthenticatedStudentRenderer,false);
  assert.equal(r.actualPostgresqlAttemptPersistedAndReadBack,false);
  assert.equal(r.teacherScoringPersistenceVerified,false);
  assert.equal(r.studentTeacherAuthenticatedSessionVerified,false);
  if(r.verifiedCourseOwnedSvgFile){
    assert.equal(r.svgHasIndependentMathUnitChecks,true);
    assert.equal(r.verifiedCourseOwnedSvgSha256,scenes.scenes.find(s=>s.lessonCode===r.lessonCode).sha256);
  }
 }
});
