'use strict';

// Grade 6-owned source/math regression only. These tests do not simulate authenticated
// Practice, tenant isolation, original publisher applets, or human assessment.
const {test} = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const root = path.resolve(__dirname, '..');
const pack = JSON.parse(fs.readFileSync(path.join(root,
  'src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g6-phase29-v1.lesson-content-pack.json'), 'utf8'));
const evidence = JSON.parse(fs.readFileSync(path.join(root,
  'artifacts/curriculum-delivery/US-CCSS-MATH/G6/g6-u05-l06-l09-public-source-task-evidence-20261030.json'), 'utf8'));
const lesson = n => pack.lessons.find(l=>l.lessonCode ===
  'PED:US-CCSS-MATH:G6:U05:L'+String(n).padStart(2,'0'))?.translations?.find(t=>t.cultureCode === 'en');
const near=(x,y)=>Math.abs(x-y)<1e-10;

test('U05 L06-L09 Grade 6-only source body evidence retains honest A-D gates', () => {
  assert.equal(evidence.summary.priorDirectStudentTaskBodyComparisons,137);
  assert.equal(evidence.summary.totalDirectStudentTaskBodyComparisons,141);
  assert.equal(evidence.summary.remaining,6);
  assert.equal(evidence.summary.fullStageAAccepted,0);
  assert.deepEqual(evidence.lessonEvidence.map(x=>x.lessonCode),
    [6,7,8,9].map(n=>'PED:US-CCSS-MATH:G6:U05:L'+String(n).padStart(2,'0')));
  for(const row of evidence.lessonEvidence){
    assert.ok(row.sourceTaskPurposes.length>=4,row.lessonCode);
    assert.equal(row.publicStudentTaskTextCompared,true);
    assert.equal(row.publicTeacherActivityTextCompared,true);
    assert.equal(row.publisherProtectedTeacherStudentResponseAccess,false);
    assert.equal(row.humanRubricDraft.implemented,false);
    assert.equal(row.humanRubricDraft.teacherScorePersisted,false);
    assert.equal(row.exactSelectedPracticeContract,'NOT_RUN');
    assert.equal(row.answerSubmissionCases,'NOT_RUN_IN_APPLICATION');
    assert.equal(row.stageAAccepted,false);
    assert.equal(row.academicAcceptance,'NOT_ACCEPTED');
  }
  assert.match(lesson(6).workedExamples,/Source-review V11 specific task/);
  assert.match(lesson(7).workedExamples,/2\.4-by-1\.3 rectangle/);
  assert.match(lesson(8).workedExamples,/20×0\.04=0\.8/);
  assert.match(lesson(9).workedExamples,/372÷3=124/);
});

test('U05 L06 decimal factor and square-area oracles reject scale mistakes', () => {
  assert.ok(near(0.23*1.5,0.345));
  assert.ok(near(0.16*0.03,0.0048));
  assert.ok(near(0.04*0.02,0.0008));
  assert.ok(near(0.04*0.07,0.0028));
  assert.equal(21*47,987);
  assert.ok(near(2.1*4.7,9.87));
  assert.ok(near(21*0.047,0.987));
  assert.ok(near(0.021*4.7,0.0987));
  assert.equal(4*2*100,800);
  assert.ok(near(8*(0.01*0.01),0.0008));
  assert.ok(!near(0.16*0.03,0.48)); // incorrect
  assert.ok(!near(0.04*0.02,0.08)); // incorrect
});

test('U05 L07 independently verifies each source rectangle partition', () => {
  assert.equal(24*13,312);
  assert.equal(200+60+40+12,312);
  assert.equal(240+72,312);
  assert.equal(260+52,312);
  assert.equal(18*14,252);
  assert.equal(100+40+80+32,252);
  assert.ok(near(2.4*1.3,3.12));
  assert.ok(near(2+0.6+0.4+0.12,3.12));
  assert.ok(near(2.4+0.72,3.12));
  assert.ok(!near(2+0.6+0.4,3.12)); // missing corner
});

test('U05 L08 models exact numeric factors and retains approximate conversion label', () => {
  assert.deepEqual([5,0.8,0.04,5.84].map(x=>20*x),[100,16,0.8,116.8]);
  assert.ok(near(25*12/100,3));
  assert.ok(near(4.6*0.9,4.14));
  assert.ok(near(16.5*0.7,11.55));
  assert.ok(near(18.2*12.75,232.05));
  assert.ok(near(6.25*2.5,15.625));
  assert.ok(near(6.25*2.54,15.875));
  assert.ok(!near(6.25*2.5,6.25*2.54));
});

test('U05 L09 partial quotient operations preserve divisor/remainder invariants', () => {
  assert.deepEqual([372/3,657/3,896/4,1332/9,1115/5,665/7,432/16],[124,219,224,148,223,95,27]);
  assert.equal(657-600-30-27,0);
  assert.equal(600/3+30/3+27/3,219);
  assert.equal(372,3*(100+20+4));
  assert.equal(345,3*(100+10+5));
  assert.ok(!near((600+30+27),219));
});

// The strict fixture checks are NOT the application grader. They demonstrate
// independent negative/malformed cases without claiming Stage C acceptance.
test('Independent numeric-oracle boundary classifies correct, incorrect, malformed inputs', () => {
  const verify=(response,expected)=>{
    if(typeof response!=='string'||!/^[+-]?(?:\d+(?:\.\d+)?|\.\d+)$/.test(response.trim()))return 'MALFORMED';
    return near(Number(response),expected)?'CORRECT':'INCORRECT';
  };
  assert.equal(verify('0.0048',0.0048),'CORRECT');
  assert.equal(verify('0.48',0.0048),'INCORRECT');
  assert.equal(verify('0.004800',0.0048),'CORRECT');
  assert.equal(verify('0.0048 meters',0.0048),'MALFORMED');
  assert.equal(verify('NaN',0.0048),'MALFORMED');
  assert.equal(verify('3.12',3.12),'CORRECT');
  assert.equal(verify('',3.12),'MALFORMED');
});
