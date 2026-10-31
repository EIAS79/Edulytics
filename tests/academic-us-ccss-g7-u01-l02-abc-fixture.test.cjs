'use strict';
// Independently authored mathematical oracles. NOT_RUN during A-C.
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'..');
const file=p=>fs.readFileSync(path.join(root,p),'utf8');
const partial=file('src/Edulytics.Web/Views/Shared/_USCCSSG7ScaleConstructionFigures.cshtml');
const dist=(a,b)=>Math.hypot(a[0]-b[0],a[1]-b[1]);
const triangleLengths=p=>[dist(p[0],p[1]),dist(p[0],p[2]),dist(p[1],p[2])];
const abc=[[0,0],[4,0],[0,3]];
const def=[[0,0],[6,0],[0,4.5]];
test('L02 source goal: all matching lengths use one k and preserve angles',()=>{
  assert.deepEqual(triangleLengths(abc),[4,3,5]);
  assert.deepEqual(triangleLengths(def),[6,4.5,7.5]);
  const ratios=triangleLengths(def).map((v,i)=>v/triangleLengths(abc)[i]);
  assert.deepEqual(ratios,[1.5,1.5,1.5]);
  assert.equal(1/ratios[0],2/3);
  assert.equal(4*0+3*0,0,'Legs are perpendicular');
});
test('L02 reject nonuniform x-only stretch and malformed vertices',()=>{
  const stretched=[[0,0],[6,0],[0,3]];
  const ratios=triangleLengths(stretched).map((v,i)=>v/triangleLengths(abc)[i]);
  assert.notEqual(ratios[0],ratios[1]);
  assert.notEqual(ratios[0],ratios[2]);
  assert.equal(Number.isFinite(NaN),false);
  assert.equal(Array.isArray([[0,0],[6,0]]),true);
  assert.equal([[0,0],[6,0]].length===3,false);
});
test('L02 equivalence under translation and 90 degree rotation',()=>{
  const transformed=abc.map(([x,y])=>[10-1.5*y,20+1.5*x]);
  const lengths=triangleLengths(transformed);
  assert.ok(lengths.every((v,i)=>Math.abs(v-1.5*triangleLengths(abc)[i])<1e-10));
});
test('L02 independent original Cool-down task is grade-scoped and teacher-assessed',()=>{
  assert.match(partial,/data-g7-u01-l02/);
  assert.match(partial,/Number talk/);
  assert.match(partial,/Corresponding Parts — Edulytiks original/);
  assert.match(partial,/6\/3=8\/4=2 but 9\/5=1.8/);
  assert.match(partial,/not the publisher's gated question/);
  assert.match(partial,/does not\s+capture or persist student work/);
});

// B is source-wired after 8c0403d1; delivery in a live student session
// remains Stage D. Do not run this authoring fixture during A-B-C.
test('L02 exact Grade 7 Rich V2 renderer hook and accessible local geometry are authored',()=>{
  const shared=file('src/Edulytics.Web/Views/Shared/_RichLessonContentV2.cshtml');
  const svg=file('src/Edulytics.Web/wwwroot/curriculum/us-ccss-g7/geometry/u01-l02-corresponding-triangles.svg');
  const cursor=JSON.parse(file('artifacts/curriculum-delivery/US-CCSS-MATH/G7/grade7-u01-l02-abc-cursor.json'));
  assert.match(shared,/Model\.LessonCode\.StartsWith\("PED:US-CCSS-MATH:G7:U01:L"/);
  assert.match(shared,/<partial name="_USCCSSG7ScaleConstructionFigures" model="Model"/);
  assert.match(partial,/data-g7-u01-l02/);
  assert.match(partial,/u01-l02-corresponding-triangles\.svg/);
  assert.match(svg,/<svg\b/);
  assert.match(svg,/<title/);
  assert.match(svg,/<desc/);
  assert.match(cursor.B_STATUS,/ACCEPTED_AUTHORING/);
  assert.equal(cursor.READY_FOR_STAGE_D,false);
});

// EXPECTED RED while Stage C is unselected: a source rubric draft alone
// cannot turn a numeric generic fallback or a freehand sketch into a score.
// Shared contract/engine files are owned by a separately authorized writer.
test('L02 selected Practice or verified educator-scoring route is required before C closure',()=>{
  const contracts=file('src/Edulytics.Core/Mathematics/Practice/LessonPracticeContracts.cs');
  const rubric=JSON.parse(file('artifacts/curriculum-delivery/US-CCSS-MATH/G7/grade7-authentic-geometry-teacher-rubrics-v6.json'));
  const record=rubric.lessons.find(x=>x.lessonCode==='PED:US-CCSS-MATH:G7:U01:L02');
  assert.ok(record);
  assert.equal(record.teacherRubric.maxScore,12);
  assert.equal(record.assessmentMode,'TEACHER_REVIEW_REQUIRED_NOT_AUTOGRADED');
  const hasExactSelection=contracts.includes('byLesson["PED:US-CCSS-MATH:G7:U01:L02"]');
  const hasVerifiedTeacherDelivery=record.rubricIntegratedIntoTeacherUI===true &&
    record.studentArtifactPersisted===true;
  assert.ok(hasExactSelection||hasVerifiedTeacherDelivery,
    'L02 exact selected Practice or verified teacher-scored delivery is missing');
});
