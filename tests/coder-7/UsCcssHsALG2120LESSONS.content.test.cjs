'use strict';
const test = require('node:test');
const assert = require('node:assert/strict');
const fs = require('node:fs');
const path = require('node:path');
const root = path.resolve(__dirname, '../..');
const read = p => JSON.parse(fs.readFileSync(path.join(root, p), 'utf8'));
const bp = read('src/Edulytics.Core/Curriculum/LessonBlueprints/Packs/us-ccss-math-hs-traditional-algebra2-im1e.lesson-blueprint.json');
const content = read('src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-hs-alg2-phase29-v1.lesson-content-pack.json');
const ledger = read('artifacts/curriculum-delivery/US-CCSS-MATH/coder-7/algebra2-120-lesson-progress.json');
test('120 lesson identities, provenance and unit counts', () => {
  assert.equal(bp.BlueprintCode, 'US-CCSS-MATH:HS:TRAD:ALG2:IM-1E');
  assert.equal(bp.Pathway, 'Traditional');
  assert.equal(content.pedagogicalSourceEdition, 'First Edition 2019-2021');
  assert.equal(bp.Lessons.length, 120);
  assert.equal(content.lessons.length, 120);
  assert.equal(ledger.lessons.length, 120);
  const counts = [0,0,0,0,0,0,0];
  for(let i=0;i<120;i++){
    const a=bp.Lessons[i], b=content.lessons[i], e=ledger.lessons[i];
    assert.equal(a.LessonCode, b.lessonCode);
    assert.equal(a.LessonCode, e.lessonCode);
    assert.equal(a.SourceUrl, b.sourceUrl);
    assert.equal(b.sourceSha256, e.sourceSha256);
    assert.equal(b.canonicalBodySha256, e.baselineCanonicalBodySha256);
    counts[Number(a.UnitNumber)-1]++;
  }
  assert.deepEqual(counts, [11,26,19,18,11,19,16]);
});
test('all eleven revised Unit 1 lessons have actual teaching and review limitations',()=>{
  const rows = ledger.lessons.filter(x=>x.unitCode==='U01' && x.contentStatus==='CONTENT_EDITED_NEEDS_RUNTIME_VALIDATION');
  assert.equal(rows.length, 11);
  for(const x of rows){
    const body = content.lessons[x.ordinal-1].translations[0];
    for(const k of ['explanation','keyConceptsAndRules','workedExamples','stepByStepSolutions','commonMistakes','quickSummary']){
      assert.ok(body[k].length > 160, x.lessonCode+' '+k);
    }
    assert.ok(!body.explanation.startsWith('Learning goal: You will '));
    assert.ok(!body.stepByStepSolutions.startsWith('Step 1: Identify what the task asks'));
    assert.equal(x.runtimeMaterializationStatus,'NOT_VERIFIED');
    assert.equal(x.sourceToStudentCompared,true);
  }
  const pending = ledger.lessons.find(x=>['CONTENT_DEFECT_TRIAGE_PENDING','EXISTING_AUTHORED_REVIEW_PENDING'].includes(x.contentStatus));
  assert.equal(ledger.nextCurriculumCursor.ordinal, pending ? pending.ordinal : ledger.lessons.length + 1);
  assert.equal(ledger.claims.academicCourseComplete,false, 'content completion cannot imply academic approval');
  assert.equal(ledger.checkedStaticCounts.sourceGroundedContentEdits, ledger.lessons.filter(x=>x.contentStatus==='CONTENT_EDITED_NEEDS_RUNTIME_VALIDATION').length);
  assert.equal(ledger.checkedStaticCounts.remainingDefectTriage, ledger.lessons.filter(x=>x.contentStatus==='CONTENT_DEFECT_TRIAGE_PENDING').length);
  assert.equal(ledger.claims.academicCourseComplete,false);
});
test('independent mathematical oracles for Unit 1 examples',()=>{
  let t=1;
  for(let n=1;n<=8;n++){if(n>1)t=2*t+1;assert.equal(t,2**n-1);}
  for(let n=0;n<=8;n++)assert.equal(2**n*80*(0.5**n),80);
  assert.deepEqual([1,2,3,4,5].map(n=>n*(n+1)/2),[1,3,6,10,15]);
  assert.deepEqual([1,2,3,4].map(n=>n*(n+1)*(2*n+1)/6),[1,5,14,30]);
  assert.equal(Math.round((2/3)**3*1e12),Math.round((8/27)*1e12));
  assert.equal(3*(1/4+1/16+1/64)+1/64,1);
  const a=n=>23000+6000*n,b=n=>3125*1.2**n;
  for(let n=0;n<22;n++)assert.ok(b(n)<=a(n));
  assert.ok(b(22)>a(22));
});
