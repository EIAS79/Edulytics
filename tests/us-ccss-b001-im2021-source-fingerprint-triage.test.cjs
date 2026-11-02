'use strict';
const test=require('node:test');
const assert=require('node:assert/strict');
const fs=require('node:fs');
const path=require('node:path');
const root=path.resolve(__dirname,'..');
const catalogue=JSON.parse(fs.readFileSync(path.join(root,'docs/curriculum/batches/US-CCSS-B001-CATALOGUE.json'),'utf8'));
const report=JSON.parse(fs.readFileSync(path.join(root,'docs/curriculum/batches/US-CCSS-B001-IM2021-500-SOURCE-FINGERPRINTS-TRIAGE.json'),'utf8'));
const hash=/^[a-f0-9]{64}$/;
test('Original 2021 teacher source hashes exist for exactly the 500 B001 lesson identities',()=>{
  assert.equal(report.schemaVersion,1);
  assert.equal(report.batchId,'US-CCSS-B001');
  assert.equal(report.lessons.length,500);
  assert.equal(new Set(report.lessons.map(l=>l.lessonCode)).size,500);
  assert.equal(catalogue.lessons.length,500);
  report.lessons.forEach((l,i)=>{
    assert.equal(l.ordinal,i+1);
    assert.equal(l.lessonCode,catalogue.lessons[i].lessonCode);
    assert.match(l.sourcePreparationSha256,hash);
    assert.match(l.sourceFullLessonSha256,hash);
    assert.equal(l.lessonPurposeExtracted,true);
    assert.equal(l.fullLessonActivitiesPresent,true);
    assert.ok(l.purposeGoalTokenRecallHeuristic>=0 && l.purposeGoalTokenRecallHeuristic<=1);
  });
});
test('Lexical-review diagnostic counts reconcile but never imply academic approvals',()=>{
  const cases={
    purposePresent:'lessonPurposeExtracted',
    goalsPresent:'teacherLearningGoalsExtracted',
    fullLessonActivitiesPresent:'fullLessonActivitiesPresent',
    genericExamplePhrase:'rawPackSourceTaskGeneric',
    genericExplanationPhrase:'rawPackTeacherFacingPhrase'
  };
  for(const [name,field] of Object.entries(cases))
    assert.equal(report.counts[name],report.lessons.filter(l=>l[field]).length,name);
  assert.equal(report.counts.lowTokenRecallBelowPointSix,report.lessons.filter(l=>l.purposeGoalTokenRecallHeuristic<0.6).length);
  assert.equal(report.counts.purposePresent,500);
  assert.equal(report.counts.fullLessonActivitiesPresent,500);
  assert.equal(report.counts.goalsPresent,499);
  assert.equal(report.counts.genericExamplePhrase,97);
  assert.equal(report.counts.genericExplanationPhrase,146);
  assert.equal(report.counts.lowTokenRecallBelowPointSix,7);
  assert.match(report.method,/weak lexical heuristic/i);
  assert.ok(report.caveats.some(x=>/not a human-style goal and activity interpretation/.test(x)));
  assert.equal('academicallyApproved' in report,false);
});
