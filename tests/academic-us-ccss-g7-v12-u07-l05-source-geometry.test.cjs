'use strict';
// Course-owned, static source geometry checks only. Run with:
// node --test tests/academic-us-ccss-g7-v12-u07-l05-source-geometry.test.cjs
// A pass cannot certify authenticated Rich V2 display, Practice, teacher scoring or Stage A-D.
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'..');
const evid=JSON.parse(fs.readFileSync(path.join(root,
  'artifacts/curriculum-delivery/US-CCSS-MATH/G7/grade7-v12-u07-l05-source-geometry-rubric.json'),'utf8'));
const svg=fs.readFileSync(path.join(root,evid.asset),'utf8');
const ledger=JSON.parse(fs.readFileSync(path.join(root,
  'artifacts/curriculum-delivery/US-CCSS-MATH/G7/grade7-authoring-ledger.json'),'utf8'));
function endpoints(id){
  const match=svg.match(new RegExp('<line id="'+id+'"[^>]*x1="([\\d.]+)" y1="([\\d.]+)" x2="([\\d.]+)" y2="([\\d.]+)"'));
  assert.ok(match,'Missing ray '+id);
  const values=match.slice(1,5).map(Number);
  assert.ok(values.every(Number.isFinite));
  return values;
}
const bearing=([x1,y1,x2,y2])=>(Math.atan2(y1-y2,x2-x1)*180/Math.PI+360)%360;
function near(actual,want,label,tol=.08){
  assert.ok(Math.abs(actual-want)<tol,label+': '+actual+' should be '+want);
}
test('Grade 7 source artifact has accessible original metadata, no active SVG content',()=>{
  assert.equal(evid.lessonCode,'PED:US-CCSS-MATH:G7:U07:L05');
  assert.match(svg,/<svg[^>]+role="img"[^>]+aria-labelledby=/);
  assert.match(svg,/<title id="g7u07l05title">/);
  assert.match(svg,/<desc id="g7u07l05desc">/);
  assert.doesNotMatch(svg,/<script|<foreignObject|\bonload\s*=|\bonclick\s*=/i);
  assert.equal(evid.publicAsset,'/curriculum/us-ccss-g7/geometry/'+path.basename(evid.asset));
});
test('Underdetermined Panel A geometrically has a 90° center yet no unique pair',()=>{
  const straight=endpoints('g7l05-a-straight');
  const r40=endpoints('g7l05-a-ray-40'),r130=endpoints('g7l05-a-ray-130');
  near(straight[1],straight[3],'horizontal baseline');
  assert.ok(straight[0]<275&&straight[2]>275);
  assert.deepEqual(r40.slice(0,2),[275,290]);
  assert.deepEqual(r130.slice(0,2),[275,290]);
  near(bearing(r40),40,'first ray');
  near(bearing(r130),130,'second ray');
  near(bearing(r130)-bearing(r40),90,'middle right angle');
  near(180-bearing(r130),50,'third sector of example');
  assert.equal(evid.math.panelA.uniqueSolution,false);
  assert.equal(evid.math.panelA.diagramNotToScale,true);
});
test('Distinct angle solutions satisfy a+b=90; uniqueness claims must fail',()=>{
  const pairs=evid.math.panelA.alternativePairs;
  assert.equal(pairs.length,2);
  assert.notDeepEqual(pairs[0],pairs[1]);
  for(const pair of pairs){
    assert.ok(pair.every(v=>v>0&&v<90));
    assert.equal(pair[0]+90+pair[1],180);
  }
});
test('Panel B rays form three equal 50° sectors and final 30° supplement',()=>{
  const straight=endpoints('g7l05-b-straight');
  near(straight[1],straight[3],'horizontal baseline');
  assert.ok(straight[0]<795&&straight[2]>795);
  const rays=['g7l05-b-ray-50','g7l05-b-ray-100','g7l05-b-ray-150'].map(endpoints);
  assert.ok(rays.every(r=>r[0]===795&&r[1]===290));
  const angles=rays.map(bearing);
  angles.forEach((v,i)=>near(v,[50,100,150][i],'sector boundary '+i));
  const sectors=[angles[0],angles[1]-angles[0],angles[2]-angles[1],180-angles[2]];
  sectors.forEach((v,i)=>near(v,[50,50,50,30][i],'sector '+i));
  assert.deepEqual(evid.math.panelB.actualSectorMeasuresDeg,[50,50,50,30]);
});
test('Grouped angle equation gives x=40; ungrouped distractor is wrong',()=>{
  const x=evid.math.panelB.x;
  assert.equal(3*(x+10)+30,180);
  assert.notEqual(3*x+10+30,180);
  assert.equal(evid.math.panelB.correctEquation,'3(x+10)+30=180');
  assert.equal(evid.math.panelB.incorrectEquation,'3x+10+30=180');
});
test('Source tasks distinguish original figures from source-aligned analogues',()=>{
  const rows=evid.source.studentTaskComparisons;
  assert.equal(rows.length,3);
  assert.ok(rows.some(x=>x.activity.startsWith('5.1')));
  assert.ok(rows.some(x=>x.activity.startsWith('5.2')));
  assert.ok(rows.some(x=>x.activity.startsWith('5.3')));
  assert.ok(rows.every(x=>/NOT_VERIFIED|PARTIAL/.test(x.coverage)));
  assert.match(evid.source.student,/\/MS\/students\/2\/7\/5\/index.html$/);
  assert.match(evid.source.teacher,/\/MS\/teachers\/2\/7\/5\/preparation.html$/);
});
test('Rubric exercises correct/wrong/incomplete/malformed without auto acceptance',()=>{
  assert.equal(evid.teacherAssessment.mode,'TEACHER_REVIEW_REQUIRED_NOT_AUTOGRADED');
  assert.equal(evid.teacherAssessment.maxScore,12);
  assert.equal(evid.teacherAssessment.rubricDraft.length,4);
  for(const row of evid.teacherAssessment.rubricDraft)assert.deepEqual(row.points,[0,1,2,3]);
  for(const kind of ['correct','incorrect','incomplete','malformed'])
    assert.ok(evid.responseCases.some(row=>row.kind===kind),'Missing '+kind);
  assert.equal(evid.teacherAssessment.recordedStudentArtifact,false);
  assert.equal(evid.integration.sharedFilesModified,false);
});
test('145 Rich candidates retained; all A-D grades and runtime acceptance fail closed',()=>{
  assert.equal(ledger.counts.catalogue,145);
  assert.equal(ledger.counts.uniqueRich,145);
  assert.equal(ledger.counts.gradeOwnedSvgFigureCandidates,19);
  assert.equal(ledger.counts.gradeOwnedTeacherRubricDrafts,20);
  for(const k of ['stageAVerified','stageBEffectiveRuntimeVerified','stageCVerified','stageDVerified']) assert.equal(ledger.counts[k],0);
  assert.equal(evid.release,'RELEASE_BLOCKED');
  for(const k of ['A','B','C','D'])assert.equal(evid.academic[k],'NOT_VERIFIED');
  const lesson=ledger.lessons.find(x=>x.lessonCode===evid.lessonCode);
  assert.ok(lesson);
  assert.equal(lesson.v12GradeOwnedOriginalSvg,evid.asset);
  assert.equal(lesson.humanAcademicAcceptance,'NOT_EVIDENCED');
  assert.equal(evid.integration.selectedPractice,'NOT_EXECUTED');
  assert.equal(evid.integration.teacherSubmissionPersistence,'NOT_RUN');
  assert.equal(ledger.v12.release,'RELEASE_BLOCKED');
});
