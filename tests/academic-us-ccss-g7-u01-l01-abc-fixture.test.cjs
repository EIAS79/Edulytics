'use strict';
// Grade-owned, source-informed independent fixture suite. AUTHOR ONLY in A-C.
// NOT a live Practice resolver, CI pass, authentication or Stage D claim.
const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const root = path.resolve(__dirname, '..');
const file = p => fs.readFileSync(path.join(root, p), 'utf8');
const base='artifacts/curriculum-delivery/US-CCSS-MATH/G7/';
const cd=JSON.parse(file(base+'grade7-u01-l01-edulytiks-cooldown.json'));
const cursor=JSON.parse(file(base+'grade7-u01-l01-abc-cursor.json'));
const partial=file('src/Edulytics.Web/Views/Shared/_USCCSSG7ScaleConstructionFigures.cshtml');
const client=file('src/Edulytics.Web/wwwroot/curriculum/us-ccss-g7/geometry/u01-l01-activities.js');
const dist=(p,q)=>Math.hypot(p[0]-q[0],p[1]-q[1]);
const isFinitePoint=p=>Array.isArray(p)&&p.length===2&&p.every(Number.isFinite);
function sameScaledPolygon(a,b) {
  if(!Array.isArray(a)||!Array.isArray(b)||a.length!==b.length||a.length<3||
     !a.every(isFinitePoint)||!b.every(isFinitePoint))return false;
  const baseline=dist(a[0],a[1]),candidate=dist(b[0],b[1]);
  if(baseline<=0||candidate<=0)return false;
  const k=candidate/baseline;
  for(let i=0;i<a.length;i++)for(let j=i+1;j<a.length;j++){
    if(Math.abs(dist(b[i],b[j])-k*dist(a[i],a[j]))>1e-9)return false;
  }
  return true;
}
test('L01 3-4-5 triangle is geometrically defined; A scales, B does not',()=>{
  assert.equal(cd.lessonCode,'PED:US-CCSS-MATH:G7:U01:L01');
  assert.deepEqual(cd.sourceTriangle.legs,[3,4]);
  assert.equal(cd.sourceTriangle.hypotenuse,5);
  assert.equal(Math.hypot(3,4),5);
  assert.deepEqual([6/3,8/4,10/5],[2,2,2]);
  assert.deepEqual([6/3,8/4,9/5],[2,2,1.8]);
  assert.notEqual(9/5,2);
  assert.deepEqual(cd.sourceTriangle.halfScaleVertices,[[0,0],[1.5,0],[0,2]]);
  assert.equal(Math.hypot(1.5,2),2.5);
});
test('Independent coordinate scaling, rotation, translation and malformed oracle',()=>{
  const p=[[0,0],[3,0],[0,4]];
  assert.ok(sameScaledPolygon(p,[[0,0],[6,0],[0,8]]));
  assert.ok(sameScaledPolygon(p,[[0,0],[1.5,0],[0,2]]));
  assert.ok(sameScaledPolygon(p,[[10,20],[10,26],[2,20]]));
  assert.equal(sameScaledPolygon(p,[[0,0],[6,0],[0,4]]),false);
  assert.equal(sameScaledPolygon(p,[[0,0],[6,0],[0,8],[3,4]]),false);
  assert.equal(sameScaledPolygon(p,[[0,0],[6,0],['bad',8]]),false);
  assert.equal(sameScaledPolygon(p,[[0,0],[6,0],[NaN,8]]),false);
});
test('Ten original polygon cards are five real scaled-copy pairs',()=>{
  const match=client.match(/const cards = (\[[\s\S]*?\]);\s*const byId/);
  assert.ok(match,'Could not locate bounded card data');
  const cards=JSON.parse(match[1].replace(/'/g,'"'));
  assert.equal(cards.length,10);
  assert.equal(new Set(cards.map(x=>x[0])).size,10);
  const lookup=new Map(cards.map(x=>[x[0],x]));
  for(const [name,points,mate] of cards){
    const opposite=lookup.get(mate);
    assert.ok(opposite,'Missing card '+mate);
    assert.equal(opposite[2],name,'Pair must be reciprocal');
    assert.ok(sameScaledPolygon(points,opposite[1]),name+' not a genuine scaled copy');
  }
});
test('Student-facing original activity and distinct cooldown authored in G7 view only',()=>{
  assert.match(partial,/data-g7-u01-l01/);
  assert.match(partial,/data-g7-cards/);
  assert.match(partial,/data-g7-portrait-scale/);
  assert.match(partial,/data-g7-cooldown-check/);
  assert.match(partial,/original Edulytiks assessment, not publisher Cool-down/i);
  assert.match(partial,/teacher must assess/i);
  assert.match(partial,/u01-l01-activities\.js/);
  assert.equal(cd.publisherCooldown,'NOT_VERIFIED_GATED');
  assert.match(cd.deliveryStatus,/INVOCATION_UNVERIFIED/);
  assert.equal(cd.maxScore,12);
  assert.equal(cd.scoringRubric.reduce((s,c)=>s+c.points,0),12);
});
test('G7 original visual and construction prompts retain unique switch cases',()=>{
  for(const section of ['var altText =','var constructionPrompt =']){
    const start=partial.indexOf(section),end=partial.indexOf('};',start);
    assert.ok(start>=0&&end>start);
    const arms=[...partial.slice(start,end).matchAll(/"PED:US-CCSS-MATH:G7:U01:L\d\d"\s*=>/g)].map(m=>m[0]);
    assert.equal(arms.length,new Set(arms).size);
  }
});
test('L01 polygon controls provide coordinates to screen readers without duplicate SVG narration',()=>{
  assert.match(client,/const vertices = points\.map/);
  assert.match(client,/vertex ['"]?\+\(index\+1\)/);
  assert.match(client,/vertices on a unit grid/);
  assert.match(client,/button\.setAttribute\('aria-label'/);
  assert.match(client,/'aria-hidden':'true'/);
  assert.match(client,/button\.setAttribute\('aria-pressed', 'false'\)/);
  // Source-level fixture only; actual screen-reader and rendered UI checks belong to D.
});
test('L01 seven F self-check rows agree with the Rich V2 measurements and distinguish shear from rigid rotation',()=>{
  const rich=JSON.parse(file('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g7-u01-our-2017.rich-lesson-v2.json'));
  const lesson=rich.Lessons.find(x=>x.LessonCode==='PED:US-CCSS-MATH:G7:U01:L01');
  const table=lesson.Visuals.find(x=>x.Title==='Original Edulytiks F-shape investigation — seven candidates');
  assert.ok(table);
  const match=client.match(/const fCandidateRows = (\[[\s\S]*?\]);\s*const fCandidates/);
  assert.ok(match,'F source-data rows not found in grade-only client');
  const candidates=JSON.parse(match[1]);
  assert.equal(candidates.length,7);
  assert.deepEqual(candidates.map(x=>x[0]),['A','B','C','D','E','F','G']);
  const accepted=[];
  for(const [id,lengths,condition] of candidates){
    const row=table.Rows.find(x=>x.Label==='Candidate '+id);
    assert.ok(row,'No lesson F row '+id);
    assert.deepEqual(lengths,row.Cells.slice(0,3).map(Number));
    assert.equal(condition,row.Cells[3]);
    const ratios=lengths.map((v,i)=>v/[4,2,1][i]);
    const consistent=ratios.every(v=>v>0&&v===ratios[0]);
    if(consistent&&condition==='upright')accepted.push(id);
    if(id==='D'){
      assert.deepEqual(ratios,[1,1,1]);
      assert.match(condition,/sheared \(angles changed\)/);
    }
  }
  assert.deepEqual(accepted,['A','B','G']);
  assert.match(table.Description,/rigidly rotated or translated F/);
  assert.match(partial,/data-g7-f-choice/);
  assert.match(partial,/data-g7-f-feedback/);
  assert.match(partial,/changed corresponding angles cannot/);
  assert.match(client,/No grade is recorded/);
});
test('L01 reference card ratios are reciprocal in either selection order, while narrative and sketch remain human-reviewed',()=>{
  const match=client.match(/const cards = (\[[\s\S]*?\]);\s*\/\/ These are the lesson/);
  assert.ok(match);
  const cards=JSON.parse(match[1].replace(/'/g,'"'));
  const index=new Map(cards.map(([id,p,matchId])=>[id,{p,matchId}]));
  const sideLength=(p,q)=>Math.hypot(p[0]-q[0],p[1]-q[1]);
  for(const [id,points,mate] of cards){
    const other=index.get(mate);
    assert.ok(other);
    const ratios=points.map((p,i)=>{
      const next=(i+1)%points.length;
      return sideLength(other.p[i],other.p[next])/sideLength(p,points[next]);
    });
    assert.ok(ratios.every(r=>r>0&&Math.abs(r-ratios[0])<1e-9));
    const reverse=1/ratios[0];
    const reverseSide=sideLength(points[0],points[1])/sideLength(other.p[0],other.p[1]);
    assert.ok(Math.abs(reverse-reverseSide)<1e-9,id+' reciprocal factor');
  }
  assert.match(client,/referenceSideFactors/);
  assert.match(client,/Only the intended card match was checked/);
  assert.match(client,/your written explanation and drawing still require teacher review/);
  assert.match(partial,/What the teacher needs to review for this construction/);
  assert.match(partial,/no submission, teacher score, or saved grade/i);
  assert.doesNotMatch(client,/localStorage|XMLHttpRequest|fetch\s*\(|\.submit\s*\(/);
});
test('L01 slider identity case and draft reset do not certify learner work',()=>{
  assert.match(client,/k===1/);
  assert.match(client,/At factor 1 both figures match the original/);
  assert.match(client,/explanation\.value=''/);
  assert.match(partial,/data-g7-cooldown-check/);
  assert.match(partial,/the teacher must assess the drawing and reasoning/i);
});
test('A-C closure remains truthful: shared Practice still blocked',()=>{
  assert.equal(cursor.READY_FOR_STAGE_D,false);
  assert.match(cursor.B_STATUS,/BLOCKED/);
  assert.match(cursor.C_STATUS,/BLOCKED/);
  assert.equal(cursor.testExecutionStatus,'NOT_RUN');
  assert.equal(cd.testExecutionStatus,'TEST_AUTHORED_NOT_RUN');
  const local=cursor['2026_10_31LessonLocalExecution'];
  assert.equal(local.sharedFilesModified,false);
  assert.equal(local.closureSHA,null);
  assert.equal(local.externalMath.pullRequest,608);
  assert.equal(local.externalMath.status,'ISOLATED_HELPER_NOT_COPIED_OR_LESSON_SELECTED');
  assert.equal(local.externalRenderer.pullRequest,599);
  assert.equal(cd.assessmentBoundaries.studentSubmissionAndPersistence,'NOT_IMPLEMENTED');
  assert.equal(cd.assessmentBoundaries.teacherRubricScorePersistence,'NOT_IMPLEMENTED');
  assert.equal(cd.assessmentBoundaries.practiceRouting,'PENDING_SUPERVISOR_SINGLE_WRITER_ISSUE_605');
  assert.equal(cd.assessmentBoundaries.fixtureStatus,'AUTHORED_NOT_RUN');
});

test('L01 source-aligned seven-candidate F table is in actually referenced Rich V2 pack',()=>{
  const rich=JSON.parse(file('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g7-u01-our-2017.rich-lesson-v2.json'));
  const lesson=rich.Lessons.find(x=>x.LessonCode==='PED:US-CCSS-MATH:G7:U01:L01');
  assert.ok(lesson);
  const table=lesson.Visuals.find(x=>x.Title==='Original Edulytiks F-shape investigation — seven candidates');
  assert.ok(table,'Missing independent learner-facing seven-choice geometry table');
  assert.equal(table.Kind,'RatioTable');
  assert.equal(table.Rows.length,9); // heading, original and seven authored alternatives
  const original=table.Rows.find(x=>x.Label==='Original F');
  assert.deepEqual(original.Cells,['4','2','1','upright']);
  const targets=[['A',true],['B',true],['C',false],['D',false],
    ['E',false],['F',false],['G',true]];
  const scaled=(cells)=>{
    const values=cells.slice(0,3).map(Number);
    if(!values.every(x=>Number.isFinite(x)&&x>0)||cells[3]!=='upright')return false;
    const k=values[0]/4;
    return Math.abs(values[1]/2-k)<1e-10 &&
      Math.abs(values[2]/1-k)<1e-10;
  };
  for(const [label,valid] of targets){
    const row=table.Rows.find(r=>r.Label==='Candidate '+label);
    assert.ok(row,'Missing candidate '+label);
    assert.equal(scaled(row.Cells),valid,'Wrong independent classification for '+label);
  }
  assert.equal(scaled(['6','3','1.5','slanted']),false);
  assert.equal(scaled(['6','oops','1.5','upright']),false);
  assert.match(lesson.ExplanationParagraphs.join(' '),/teacher must inspect the finished outline/);
  assert.match(table.Description,/no original publisher figures/);
});
