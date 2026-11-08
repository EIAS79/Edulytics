'use strict';
const fs = require('node:fs');
const path = require('node:path');
const {execFileSync} = require('node:child_process');
const assert = require('node:assert/strict');
const root = path.resolve(__dirname, '../../');
const read = rel => JSON.parse(fs.readFileSync(path.join(root, rel), 'utf8'));
const progress = read('artifacts/curriculum-delivery/US-CCSS-MATH/G12/full-grade-progress.json');
const pack = read('src/Edulytics.Core/Curriculum/LessonContent/Packs/us-ccss-math-g12-precalculus-u01-l01-abc-draft.lesson-content-pack.json');
const owner = path.join(root, 'artifacts/curriculum-delivery/US-CCSS-MATH/G12/grade12-owner-academic-release-authorization-20261107.md');
assert.ok(fs.existsSync(owner), 'Owner authorization evidence absent');
assert.equal(pack.Status, 'Published');
assert.match(pack.ReviewedBy, /programme owner/i);
assert.match(pack.ReviewEvidence, /grade12-owner-academic-release-authorization/);
assert.equal(progress.LessonEntries.length, 73);
assert.equal(pack.Lessons.length, 73);
assert.equal(new Set(pack.Lessons.map(l => l.LessonCode)).size, 73);
const fields = ['Title','Explanation','KeyConceptsAndRules','WorkedExamples','StepByStepSolutions','CommonMistakes','QuickSummary'];
let richAligned=0, firstPreserved=0;
for (let i=0; i<73; i++) {
  const entry=progress.LessonEntries[i], canonical=pack.Lessons[i];
  assert.equal(canonical.LessonCode, entry.LessonCode, 'Wrong grade/lesson index at '+(i+1));
  assert.equal(canonical.Translations.length, 1);
  const t=canonical.Translations[0];
  assert.equal(t.CultureCode,'en');
  for(const field of fields) {
    assert.ok(typeof t[field]==='string'&&t[field].trim().length>=(field==='Title'?3:12), entry.LessonCode+' '+field+' missing');
    assert.doesNotMatch(t[field], /OpenStax|Rice University/i, 'Source credits belong in legal notices');
  }
  assert.match(canonical.SourceRights,/CC BY 4\.0|CC BY/i);
  assert.match(canonical.SourceSha256, /^[0-9a-f]{64}$/);
  const code = entry.LessonCode.match(/:U(\d{2}):L(\d{2})$/);
  assert.ok(code);
  const rich=read('src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g12-precalculus-u'+code[1]+'-l'+code[2]+'.rich-lesson-v2.json').Lessons[0];
  assert.equal(rich.LessonCode, canonical.LessonCode);
  assert.equal(t.Title,rich.Title);
  if(i===0) {
    assert.match(t.Explanation, /vertical|horizontal/i);
    firstPreserved++;
  } else {
    assert.ok(t.Explanation.includes(rich.ExplanationParagraphs[0]),entry.LessonCode+' explanation misaligned');
    assert.ok(t.WorkedExamples.includes(rich.WorkedExamples[0].Answer),entry.LessonCode+' worked answer lost');
    assert.ok(t.QuickSummary.includes(rich.SummaryPoints[0]),entry.LessonCode+' summary lost');
    richAligned++;
  }
}
const licenseRel='src/Edulytics.Web/wwwroot/content-sources-licenses.html';
const licensePath=path.join(root,licenseRel);
const notice=fs.existsSync(licensePath)?fs.readFileSync(licensePath,'utf8'):execFileSync('git',['show','HEAD:'+licenseRel],{cwd:root,encoding:'utf8'});
assert.match(notice, /OpenStax/i);
assert.match(notice, /CC BY 4\.0/i);
console.log(JSON.stringify({candidate:'G12_73_OWNER_AUTHORIZED_PUBLISHED_CANONICAL_PACK',status:'STRUCTURAL_CANDIDATE_ONLY',lessonCount:73,richV2FieldsAligned:richAligned,firstCanonicalPreserved:firstPreserved,publicationMustStillPassAuthenticatedE2E:true,sourceAtomicParityUnverified:true}));
