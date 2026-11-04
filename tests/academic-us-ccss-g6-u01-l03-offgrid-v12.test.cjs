'use strict';

// Grade 6-only source/geometry regressions; NOT application Practice scoring.
const {test}=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'..');
const read=p=>JSON.parse(fs.readFileSync(path.join(root,p),'utf8'));
const record=read('artifacts/curriculum-delivery/US-CCSS-MATH/G6/g6-u01-l03-offgrid-source-v12-evidence-20261030.json');
const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g6-u01-l01-l03-area-20261028.rich-lesson-v2.json');
const lesson=rich.Lessons.find(x=>x.LessonCode==='PED:US-CCSS-MATH:G6:U01:L03');

test('Grade 6 L03 original source task categories and limitations remain explicit',()=>{
  assert.equal(record.sourceTaskBreakdown.length,3);
  assert.equal(record.publicSourceTeacherGoalTaskTextCompared,true);
  assert.equal(record.publicSourceFiguresAltRead,true);
  assert.equal(record.publisherFigurePixelInspection,false);
  assert.equal(record.originalFigureAppletRecreationVerified,false);
  assert.equal(record.protectedTeacherResponsesRead,false);
  assert.equal(record.accepted,false);
  assert.equal(record.released,false);
  assert.match(record.stageA,/^OPEN__/);
  assert.match(record.stageC,/^NOT_RUN__/);
  assert.match(record.stageD,/^NOT_RUN__/);
});

test('Grade 6 L03 off-grid figures independently compute 15, 16, 21 square centimeters',()=>{
  const a=2*(5*3/2),b=4*(2*4/2),c=5*5-2*2;
  assert.deepEqual([a,b,c],[15,16,21]);
  assert.deepEqual(record.independentNumericalChecks.map(x=>x.area),[15,16,21]);
  assert.deepEqual(record.independentNumericalChecks.map(x=>x.units),['cm²','cm²','cm²']);
  assert.notEqual(c,5*5-2); // linear length is not inner square area
  assert.notEqual(b,5*5-2*2); // cannot invent the unknown B hole length
  assert.equal((5*3)/2,7.5);
  assert.equal((2*4)/2,4);
});

test('Grade 6 L03 Rich V2 selects measurable triangles for unknown tilted hole',()=>{
  assert.ok(lesson);
  const ex=lesson.WorkedExamples.find(x=>/Off-grid comparison/.test(x.Title));
  assert.ok(ex);
  assert.match(ex.Answer,/A = 15 cm²; B = 16 cm²; C = 21 cm²/);
  assert.match(ex.Question,/unknown side length/);
  assert.match(ex.Method,/non-overlapping triangle decomposition/);
  assert.match(ex.Steps.join(' '),/subtracting its imagined area.*not justified/);
  assert.match(ex.Check,/teacher must assess/);
  assert.ok(lesson.Visuals.some(x=>/Off-grid shaded regions/.test(x.Title)&&/unshaded tilted square whose side length is not specified/.test(x.Description)));
});
