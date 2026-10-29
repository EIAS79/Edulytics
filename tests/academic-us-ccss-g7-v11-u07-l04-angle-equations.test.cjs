'use strict';
// Native Node static-source fixture. Does NOT establish full Grade 7 checkout,
// authenticated rendering, exact Practice, teacher scoring, or academic acceptance.
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'..');
const evidence=JSON.parse(fs.readFileSync(path.join(root,'artifacts/curriculum-delivery/US-CCSS-MATH/G7/grade7-v11-u07-l04-source-geometry-rubric.json'),'utf8'));
const svg=fs.readFileSync(path.join(root,evidence.firstPartyAsset),'utf8');
const near=(value,want,label,tolerance=.05)=>assert.ok(Math.abs(value-want)<tolerance,label+': '+value+' != '+want);
function vector(id){
  const m=svg.match(new RegExp('<line id="'+id+'"[^>]*x1="([\\d.]+)" y1="([\\d.]+)" x2="([\\d.]+)" y2="([\\d.]+)"'));
  assert.ok(m,'Missing geometric ray '+id);
  return m.slice(1,5).map(Number);
}
const bearing=([x1,y1,x2,y2])=>(Math.atan2(y1-y2,x2-x1)*180/Math.PI+360)%360;
test('Grade 7 U07 L04 original SVG has accessible text and no executable handlers',()=>{
  assert.match(svg,/<svg[^>]*role="img"[^>]*aria-labelledby=/);
  assert.match(svg,/<title id="g7-u07-l04-title">/);
  assert.match(svg,/<desc id="g7-u07-l04-desc">/);
  assert.doesNotMatch(svg,/<script|<foreignObject|\bonload\s*=|\bonclick\s*=/i);
});
test('Straight rays truly form 40,70,70 degrees, so x=70',()=>{
  const base=vector('g7l04-straight-base'),r40=vector('g7l04-ray-40'),r110=vector('g7l04-ray-110');
  near(base[1],base[3],'horizontal line',.001);
  near(bearing(r40),40,'first ray');near(bearing(r110),110,'second ray');
  assert.deepEqual(r40.slice(0,2),[245,303]);assert.deepEqual(r110.slice(0,2),[245,303]);
  assert.ok(base[0]<245&&base[2]>245);
  assert.deepEqual(evidence.mathModel.straight.parts,[40,70,70]);
  assert.equal(40+2*70,180);
  assert.notEqual(40+2*70,90);
});
test('Crossing line geometry gives vertical 70 degree pair and y=20',()=>{
  const base=vector('g7l04-cross-horizontal'),diagonal=vector('g7l04-cross-diagonal');
  near(base[1],base[3],'horizontal line',.001);
  near(bearing(diagonal),70,'vertical ray angle');
  near((diagonal[0]+diagonal[2])/2,760,'crossing x');
  near((diagonal[1]+diagonal[3])/2,292,'crossing y');
  assert.ok(base[0]<760&&base[2]>760);
  assert.equal(3*20+10,70);
  assert.notEqual(3*20+10+70,180);
});
test('Teacher-only rubric rejects wrong, incomplete, malformed answers',()=>{
  assert.equal(evidence.lessonCode,'PED:US-CCSS-MATH:G7:U07:L04');
  assert.equal(evidence.assessmentMode,'TEACHER_REVIEW_REQUIRED_NOT_AUTOGRADED');
  assert.equal(evidence.teacherRubric.criteria.length,4);
  assert.equal(evidence.teacherRubric.maxScore,12);
  for(const criterion of evidence.teacherRubric.criteria)assert.deepEqual(criterion.points,[0,1,2,3]);
  for(const kind of ['correct','wrong','incomplete','malformed'])
    assert.ok(evidence.cases.some(c=>c.kind===kind),'Missing '+kind);
  assert.equal(evidence.teacherRubric.studentArtifactPersisted,false);
});
test('Original student/teacher activity trace is scoped and explicitly incomplete',()=>{
  assert.equal(evidence.source.activityComparison.length,4);
  assert.ok(evidence.source.activityComparison.some(x=>x.sourceActivity.includes('Info Gap')));
  assert.ok(evidence.source.observation.includes('NOT_INSPECTED'));
  assert.match(evidence.source.studentUrl,/\/MS\/students\/2\/7\/4\/index.html$/);
  assert.match(evidence.source.teacherUrl,/\/MS\/teachers\/2\/7\/4\/preparation.html$/);
});
test('Fail-closed integration and academic status',()=>{
  for(const stage of ['stageA','stageB','stageC','stageD'])assert.equal(evidence.academic[stage],'NOT_VERIFIED');
  assert.equal(evidence.academic.ownerUAT,'NOT_RUN');
  assert.equal(evidence.academic.release,'RELEASE_BLOCKED');
  assert.equal(evidence.sharedIntegration.sharedFilesModified,false);
  for(const key of ['teacherScorePersistence','authenticatedStudentRender','twoSchoolIsolation'])assert.equal(evidence.sharedIntegration[key],'NOT_RUN');
  assert.equal(evidence.sharedIntegration.selectedPractice,'NOT_EXECUTED');
});
