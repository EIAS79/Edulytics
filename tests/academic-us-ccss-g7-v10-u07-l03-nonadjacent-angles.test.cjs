'use strict';
// Grade 7-only static source/geometry fixture; passing is NOT student/teacher runtime acceptance.
// Run: node --test tests/academic-us-ccss-g7-v10-u07-l03-nonadjacent-angles.test.cjs
const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const root = path.resolve(__dirname, '..');
const evidence = JSON.parse(fs.readFileSync(path.join(root,
  'artifacts/curriculum-delivery/US-CCSS-MATH/G7/grade7-v10-u07-l03-source-geometry-rubric.json'),'utf8'));
const svg = fs.readFileSync(path.join(root,evidence.staticAsset),'utf8');
const ledger = JSON.parse(fs.readFileSync(path.join(root,
  'artifacts/curriculum-delivery/US-CCSS-MATH/G7/grade7-authoring-ledger.json'),'utf8'));

function line(id) {
  const re = new RegExp('<line id="'+id+'"[^>]*x1="([\\d.]+)" y1="([\\d.]+)" x2="([\\d.]+)" y2="([\\d.]+)"');
  const match = svg.match(re);
  assert.ok(match,'Missing measurable vector: '+id);
  return match.slice(1,5).map(Number);
}
const bearing=([x1,y1,x2,y2])=>(Math.atan2(y1-y2,x2-x1)*180/Math.PI+360)%360;
const near=(actual,expected,label,tolerance=.04)=>
  assert.ok(Math.abs(actual-expected)<tolerance,label+': '+actual+' vs '+expected);

test('Original U07 L03 reference is accessible and excludes executable content',()=>{
  assert.match(svg,/<svg[^>]*role="img"[^>]*aria-labelledby=/);
  assert.match(svg,/<title id="g7-u07-l03-title">/);
  assert.match(svg,/<desc id="g7-u07-l03-desc">/);
  assert.doesNotMatch(svg, /<script|<foreignObject|\bonload\s*=|\bonclick\s*=/i);
  assert.equal(evidence.lessonCode,'PED:US-CCSS-MATH:G7:U07:L03');
  assert.match(evidence.sourceStudentUrl,/\/MS\/students\/2\/7\/3\/index.html$/);
});

test('Two drawn straight lines meet and opposite angles are mathematically equal',()=>{
  const h=line('intersecting-horizontal'),d=line('intersecting-diagonal');
  near(h[1],h[3],'horizontal line');
  near(bearing(d),55,'diagonal bearing');
  near((d[0]+d[2])/2,255,'crossing x');
  near((d[1]+d[3])/2,235,'crossing y');
  assert.ok(h[0]<255&&h[2]>255);
  near(180-bearing(d),125,'supplementary obtuse angle');
  assert.deepEqual(evidence.model.crossing.oppositePairs,[[55,55],[125,125]]);
  assert.equal(125+55,180);
  assert.notEqual(125,55);
});

test('Distinct 28 and 62 degree angles are genuinely constructed and complementary',()=>{
  const a=line('separate-angle-28'),b=line('separate-angle-62');
  near(bearing(a),28,'angle A');
  near(bearing(b),62,'angle B');
  assert.notDeepEqual(a.slice(0,2),b.slice(0,2));
  assert.equal(28+62,90);
  assert.equal(evidence.model.nonadjacent.complementarySum,90);
});

test('Rubric covers correct, wrong, incomplete and malformed submissions without auto-approval',()=>{
  assert.equal(evidence.teacherRubric.criteria.length,4);
  assert.equal(evidence.teacherRubric.maxScore,12);
  for(const c of evidence.teacherRubric.criteria) assert.deepEqual(c.points,[0,1,2,3]);
  for(const kind of ['correct','incorrect','incomplete','malformed'])
    assert.ok(evidence.cases.some(c=>c.kind===kind),'Missing '+kind);
  assert.equal(evidence.assessmentMode,'TEACHER_REVIEW_REQUIRED_NOT_AUTOGRADED');
  assert.equal(evidence.runtime.authenticatedStudentAsset,'NOT_RUN');
  assert.equal(evidence.runtime.selectedPractice,'NOT_EXECUTED');
  assert.equal(evidence.runtime.teacherScorePersistence,'NOT_RUN');
  assert.equal(evidence.sharedIntegration.sharedFilesModifiedByCoder3,false);
  for(const gate of ['A','B','C','D']) assert.equal(evidence.stageEvidence[gate],'NOT_VERIFIED');
  assert.equal(evidence.release,'RELEASE_BLOCKED');
});

test('Full-grade ledger preserves all 145 Rich candidates and fail-closed academic gates',()=>{
  assert.equal(ledger.counts.catalogue,145);
  assert.equal(ledger.counts.uniqueRich,145);
  assert.equal(ledger.counts.gradeOwnedSvgFigureCandidates,17);
  assert.equal(ledger.counts.gradeOwnedTeacherRubricDrafts,18);
  for(const key of ['stageAVerified','stageBEffectiveRuntimeVerified','stageCVerified','stageDVerified'])
    assert.equal(ledger.counts[key],0);
  const lesson=ledger.lessons.find(x=>x.lessonCode===evidence.lessonCode);
  assert.ok(lesson);
  assert.equal(lesson.v10GradeOwnedOriginalSvg,evidence.staticAsset);
  assert.equal(lesson.humanAcademicAcceptance,'NOT_EVIDENCED');
  assert.equal(lesson.stages.C,'NOT_VERIFIED');
  assert.equal(lesson.stages.D,'NOT_RUN');
  assert.equal(ledger.v10.release,'RELEASE_BLOCKED');
});
